using System;
using System.Collections.Generic;
using System.Windows.Forms;

using HMI_MT_Settings;
using HelperLibrary;
using WindowsForms;
using InterfaceLibrary;

namespace HMI_MT
{
    public partial class FrmDiagPanel : Form, IResetStateProtocol
    {
        private readonly MainForm parent;
        private List<ITag> tags;
        private IBasePanelCollection iBPanel;

        public FrmDiagPanel( MainForm linkMainForm )
        {
            InitializeComponent( );
            this.parent = linkMainForm;
        }
        public void ResetProtocol( )
        {
            if ( !iBPanel.ErrorLoading )
                CalculationRegion.ResetStatusProtocol( iBPanel.CalculationElements );
        }
        private void FrmDiagPanelLoad( object sender, EventArgs e )
        {
            iBPanel = new BlockPanel( HMI_Settings.DiagnosticSchema, HMI_Settings.SchemaSize );

            if ( !iBPanel.ErrorLoading )
            {
                Text = iBPanel.CaptionOfSchema;
                Size = iBPanel.ClientSize;
                iBPanel.Parent = this;
                iBPanel.PanelClick += PanelClick;
                Controls.Add( (Control)iBPanel );

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
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        private void FrmDiagPanelFormClosing(object sender, FormClosingEventArgs e) { HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags( tags ); }
    }
}
