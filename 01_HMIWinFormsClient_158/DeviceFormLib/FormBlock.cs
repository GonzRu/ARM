using System;
using System.Windows.Forms;

using HelperControlsLibrary;
using InterfaceLibrary;

namespace DeviceFormLib
{
    public partial class FormBlock : Form, IDeviceForm, ReportLibrary.IReport
    {
        public FormBlock( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent( );
            
            this.Guid = unidev;
            tabControl.Controls.Add( new BlockViewTagPage( this, unids, unidev ) );
            tabControl.Controls.Add( new EventBlockTabPage( unidev ) );
            tabControl.Controls.Add( new OscDiagTabPage( unidev, OscDiagTabPage.OscDiagPanelVisible.Oscilograms ) );
            tabControl.Controls.Add( new InformationTabPage( unidev ) );
            tabControl.Controls.Add( new DataBaseFilesLibrary.TabPageDbFile( unidev ) );
        }

        public void CreateDeviceForm( ) { /*throw new NotImplementedException();*/ }
        public void InitInterfaceElementsClick( ) { /*throw new NotImplementedException();*/ }
        public void ActivateTabPage( string typetabpage ) { /*throw new NotImplementedException();*/ }
        public void reqAvar_OnReqExecuted( IRequestData req ) { /*throw new NotImplementedException();*/ }
        public UInt32 Guid { get; private set; }
        public void Print( ) { ( (BlockViewTagPage)tabControl.Controls[0] ).Print( ); }
    }
}
