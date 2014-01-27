﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InterfaceLibrary;
using HMI_MT_Settings;

namespace HelperControlsLibrary.TeleMechanica
{
    public class TeleMechanicaCommandTabPage : TabPage
    {
        #region Private Fields
        private DataGridView _commandsDataGridView;

        private uint _dsGuid;
        private uint _devGuid;
        #endregion

        public TeleMechanicaCommandTabPage(uint dsGuid, uint devGuid) : base ("Комманды")
        {
            _dsGuid = dsGuid;
            _devGuid = devGuid;
            this.Width = 500;

            TableLayoutPanel commonTableLayoutPanel = new TableLayoutPanel();
            commonTableLayoutPanel.RowCount = 2;
            commonTableLayoutPanel.ColumnCount = 1;
            commonTableLayoutPanel.AutoSize = true;

            #region commandsDataGridView
            var dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));

            _commandsDataGridView = new DataGridView()
                {
                    Dock = DockStyle.Fill,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToResizeRows = false,
                    AutoSize = true,
                    Width = 1000,
                    Height = 1000
                };
            _commandsDataGridView.MultiSelect = false;
            _commandsDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            _commandsDataGridView.CellClick += DataGridViewOnCellClickHandler;
            _commandsDataGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "Название комманды", Width = 500 });
            _commandsDataGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "Параметр комманды", Width = 200 });
            _commandsDataGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "Выполнить", Width = 150 });
            _commandsDataGridView.Columns[0].ReadOnly = true;
            InitCommandsDataGridView(_dsGuid, _devGuid);
            #endregion

            commonTableLayoutPanel.Controls.Add(_commandsDataGridView, 0, 1);

            Controls.Add(commonTableLayoutPanel);
        }

        #region Public Metods
        #endregion

        #region Private Metods
        #region DataGrid's Metods
        /// <summary>
        /// Insert comands and parameters in DataGrid
        /// </summary>
        /// <param name="dsGuid"></param>
        /// <param name="devGuid"></param>
        private void InitCommandsDataGridView(uint dsGuid, uint devGuid)
        {
            IDevice device = HMI_Settings.CONFIGURATION.GetLink2Device(dsGuid, devGuid);
            if (device == null)
                throw new Exception(
                    string.Format("HelperControlsLibrary.TeleMechanica.TeleMechanicaCommandTabPage::InitCommandsDataGridView: Нет связанного устройства с данной формой unids = {0}; unidev = {1}",
                                   dsGuid.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                   devGuid.ToString(System.Globalization.CultureInfo.InvariantCulture)));

            var commandsList = device.GetListDeviceCommands();
            foreach (var command in commandsList)
            {
                _commandsDataGridView.Rows.Add(CreateCommandDataGridRow(command));
            }
        }

        /// <summary>
        /// Create DataGridRow from IDeviceCommand
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private DataGridViewRow CreateCommandDataGridRow(IDeviceCommand command)
        {
            var commandNameDataGridViewCell = new DataGridViewTextBoxCell() { Value = command.CmdDispatcherName };

            var commandParametersDataGridViewCell = new DataGridViewComboBoxCell();
            if (command.Parameters != null)
                foreach (var parameter in command.Parameters)
                    commandParametersDataGridViewCell.Items.Add(parameter.Name);
            try
            {
                commandParametersDataGridViewCell.Value = commandParametersDataGridViewCell.Items[0].ToString();
            }
            catch
            {
            }

            var executeCommandButtonDataGridViewCell = new DataGridViewButtonCell() { Value = "Выполнить" };

            var commandDataGridViewRow = new DataGridViewRow();
            commandDataGridViewRow.Tag = command;
            commandDataGridViewRow.Cells.Add(commandNameDataGridViewCell);
            commandDataGridViewRow.Cells.Add(commandParametersDataGridViewCell);
            commandDataGridViewRow.Cells.Add(executeCommandButtonDataGridViewCell);

            return commandDataGridViewRow;
        }

        /// <summary>
        /// DataGridCell Mouse Click Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void DataGridViewOnCellClickHandler(object sender, DataGridViewCellEventArgs eventArgs)
        {
            if (eventArgs.ColumnIndex != 2 || eventArgs.RowIndex == -1)
                return;

            IDeviceCommand command = _commandsDataGridView.Rows[eventArgs.RowIndex].Tag as IDeviceCommand;
            DataGridViewComboBoxCell comboBoxDataGridView = _commandsDataGridView[1, eventArgs.RowIndex] as DataGridViewComboBoxCell;

            byte value = (byte)comboBoxDataGridView.Items.IndexOf(comboBoxDataGridView.Value);

            string cmd = command.IECAddress;
            byte[] param = new byte[1] { value };

            HMI_Settings.CONFIGURATION.ExecuteCommand(_dsGuid, _devGuid, cmd, param, null);
        }
        #endregion
        #endregion
    }
}