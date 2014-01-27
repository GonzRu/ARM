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

            TableLayoutPanel commonTableLayoutPanel = new TableLayoutPanel();
            commonTableLayoutPanel.RowCount = 2;
            commonTableLayoutPanel.ColumnCount = 1;
            commonTableLayoutPanel.AutoSize = true;

            #region commandsDataGridView
            _commandsDataGridView = new DataGridView()
                {
                    Dock = DockStyle.Fill,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToResizeRows = false,
                    ReadOnly = true
                };
            _commandsDataGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "Название комманды" });
            _commandsDataGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "Параметр комманды" });
            _commandsDataGridView.Columns.Add(new DataGridViewColumn() { HeaderText = "Выполнить" });
            _commandsDataGridView.AutoSize = true;
            _commandsDataGridView.MultiSelect = false;
            _commandsDataGridView.ReadOnly = false;
            InitCommandsDataGridView(_dsGuid, _devGuid);            
            #endregion

            commonTableLayoutPanel.Controls.Add(_commandsDataGridView, 0, 1);

            Controls.Add(commonTableLayoutPanel);
        }

        #region Public Metods
        #endregion

        #region Private Metods
        #region DataGrid's Metods
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
                _commandsDataGridView.Rows.Add(CreateCommandRDataGridow(command));
            }
        }

        private DataGridViewRow CreateCommandRDataGridow(IDeviceCommand command)
        {
            var commandNameDataGridViewCell = new DataGridViewTextBoxCell() { Value = command.CmdDispatcherName };

            var commandParametersDataGridViewCell = new DataGridViewComboBoxCell();
            if (command.Parameters != null)
                foreach (var parameter in command.Parameters)
                    commandParametersDataGridViewCell.Items.Add(parameter.Value.ToString() + parameter.Name);
                    //commandParametersDataGridViewCell.Items.Add(parameter);
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
        #endregion
        #endregion
    }
}
