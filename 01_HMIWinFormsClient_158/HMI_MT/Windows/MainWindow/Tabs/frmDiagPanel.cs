using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using HMI_MT_Settings;
using HelperLibrary;
using LibraryElements;
using LibraryElements.CalculationBlocks;
using WindowsForms;
using InterfaceLibrary;

namespace HMI_MT
{
    public partial class FrmDiagPanel : Form, IResetStateProtocol
    {
        private readonly MainForm parent;
        private List<ITag> tags;
        private IBasePanelCollection iBPanel;
        private string _pathToDiagnosticPanelfile;

        /// <summary>
        /// Конструктор для запуска панелид диагностики по-умолчанию
        /// </summary>
        public FrmDiagPanel( MainForm linkMainForm )
        {
            InitializeComponent( );
            this.parent = linkMainForm;

            _pathToDiagnosticPanelfile = HMI_Settings.DiagnosticSchema;
        }

        public FrmDiagPanel(MainForm linkMainForm, string pathToDiagnosticPanelFile)
        {
            InitializeComponent();
            this.parent = linkMainForm;

            _pathToDiagnosticPanelfile = pathToDiagnosticPanelFile;
        }

        public void ResetProtocol( )
        {
            if ( !iBPanel.ErrorLoading )
                CalculationRegion.ResetStatusProtocol( iBPanel.CalculationElements );
        }
        private void FrmDiagPanelLoad( object sender, EventArgs e )
        {
            iBPanel = new BlockPanel(_pathToDiagnosticPanelfile, HMI_Settings.SchemaSize);

            if ( !iBPanel.ErrorLoading )
            {
                Text = iBPanel.CaptionOfSchema;
                Size = iBPanel.ClientSize;
                iBPanel.Parent = this;
                iBPanel.PanelClick += PanelClick;
                Controls.Add( (Control)iBPanel );

                foreach (var baseRegion in iBPanel.CalculationElements)
                {
                    var calculationRegion = baseRegion as CalculationRegion;
                    if (calculationRegion != null && calculationRegion.CalculationContext != null)
                    {
                        var blockSignalCalculation = calculationRegion.CalculationContext.Context as BlockSignalCalculation;
                        if (blockSignalCalculation != null)
                        {
                            if (blockSignalCalculation.Text != "Unknown")
                                continue;

                            uint dsGuid = (baseRegion as IDynamicParameters).Parameters.DsGuid;
                            uint devGuid = (baseRegion as IDynamicParameters).Parameters.DeviceGuid;

                            var device = HMI_Settings.CONFIGURATION.GetLink2Device(dsGuid, devGuid);
                            if (device != null)
                            {
                                blockSignalCalculation.Text = device.Description;
                            }
                        }
                    }
                }

                tags = NewMainMnemo.BindingLincks( iBPanel.CalculationElements );
                NewMainMnemo.BindingContextMenu( iBPanel.CalculationElements, this );
                // подписываемся на обновление тегов
                HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags( tags );
            }
        }
        private void PanelClick( object sender, EventArgs e )
        {
            try
            {
                var idp = sender as LibraryElements.IDynamicParameters;
                if ( idp != null && idp.Parameters != null )
                    DevicesLibrary.DeviceFormFactory.CreateForm( this, idp.Parameters.DsGuid, idp.Parameters.DeviceGuid, parent.arrFrm );

                var buttonRegion = sender as ButtonRegion;
                if (buttonRegion != null)
                {
                    var file = AppDomain.CurrentDomain.BaseDirectory + @"Project\MnemoSchemas\" + buttonRegion.Group;

                    if (File.Exists(file))
                    {
                        var formEz = new FrmDiagPanel(parent, file) { MdiParent = parent };
                        formEz.Show();
                    }
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        private void FrmDiagPanelFormClosing(object sender, FormClosingEventArgs e) { HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags( tags ); }
    }
}
