using System;
using System.Windows.Forms;
using System.Xml.Linq;

using InterfaceLibrary;
using HMI_MT_Settings;


namespace DeviceFormLib
{
    public partial class FormUsoMtd : Form, IDeviceForm, ReportLibrary.IReport
    {
        TabPage tpCurrent;
        //readonly frmEngine frmengine;
        readonly FrmEngineNew engineNew;
        readonly UInt32 uniDev = 0xffffffff;
        Boolean isCurrentInfoEnter;
        Boolean isSystemEnter;

        public FormUsoMtd( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent();
            try
            {
                uniDev = unidev;
                //frmengine = new frmEngine( unids, unidev, this );
                engineNew = new FrmEngineNew( unids, unidev, this );

                /*tabControl.Controls.Add( new EventBlockTabPage( unidev ) );
                tabControl.Controls.Add( new OscDiagTabPage( unidev, OscDiagTabPage.OscDiagPanelVisible.Oscilograms ) );
                tabControl.Controls.Add( new InformationTabPage( unidev ) );
                tabControl.Controls.Add( new DataBaseFilesLibrary.TabPageDBFile( uniDev ) );*/

                var buttons = FormUsoMtr.CreateCommandButtons((int)uniDev, this);
                if (buttons != null)
                    flowLayoutPanel1.Controls.AddRange(buttons);
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Открытие формы
        /// </summary>
        private void FormBmrzDeviceLoad( object sender, EventArgs e )
        {
            try
            {
                ////frmengine.InitFrm( this, tabControl, null );
                //frmengine.InitFrmLight( this, tabControl, null );
                InitInterfaceElementsClick();
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Закрытие формы
        /// </summary>
        private void FormBmrzDeviceFormClosing( object sender, FormClosingEventArgs e )
        {
            // отписываемся от тегов
            HMI_Settings.HMIControlsTagReNew( tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe );
        }

        public void CreateDeviceForm() { }
        public void InitInterfaceElementsClick()
        {
            tpCurrentInfo.Enter += this.TpCurrentInfoEnter;
            tabSystem.Enter += this.TabSystemEnter;
        }
        /// <summary>
        /// активировать определенную вкладку
        /// </summary>
        /// <param name="typetabpage"></param>
        public void ActivateTabPage( string typetabpage ) { }
        public void reqAvar_OnReqExecuted( IRequestData req ) { }
        /// <summary>
        /// Выбор вкладки
        /// </summary>
        private void TabControlSelected( object sender, TabControlEventArgs e )
        {
            HMI_Settings.HMIControlsTagReNew( tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe );
            tpCurrent.Tag = false; // признак отписки от тегов для данной TabPage
            tpCurrent = e.TabPage;  // запомним новую текущую вкладку
        }
        /// <summary>
        /// Вход на вкладку
        /// </summary>
        private void TpCurrentInfoEnter( object sender, EventArgs e )
        {
            //CommonUtils.CommonUtils.RemoveHMIUserControls(tpCurrentInfo);
            if ( !isCurrentInfoEnter )
            {
                //frmengine.PlaceTagsOnTPFlps( "Контроль" );
                engineNew.PlaceTagsOnPanels( "Контроль" );
                isCurrentInfoEnter = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpCurrentInfo );
        }
        /// <summary>
        /// Вход на вкладку
        /// </summary>
        private void TabSystemEnter( object sender, EventArgs e )
        {
            //CommonUtils.CommonUtils.RemoveHMIUserControls(tpCurrentInfo);
            if ( !isSystemEnter )
            {
                //frmengine.PlaceTagsOnTPFlps( "Система" );
                engineNew.PlaceTagsOnPanels( "Система" );
                isSystemEnter = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpCurrentInfo );
        }        
        /// <summary>
        /// Печать данных в файл
        /// </summary>
        /// <returns>Поток файла</returns>
        public void Print()
        {
            HelperControlsLibrary.FrmEngine.PreparationOfReportData( uniDev, tpCurrent );
        }
        /// <summary>
        /// Идентификатор блока
        /// </summary>
        public uint Guid { get { return uniDev; } }
    }
}