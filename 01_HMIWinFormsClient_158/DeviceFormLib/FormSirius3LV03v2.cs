using System;
using System.Text;
using System.Windows.Forms;

using HelperControlsLibrary;
using InterfaceLibrary;
using System.Collections;
using HMI_MT_Settings;

using TypeBlockData4Req = InterfaceLibrary.TypeBlockData4Req;
using DeviceFormLib.BlockTabs;

using EventBlockTabPage = DeviceFormLib.BlockTabs.EventBlockTabPage;
using InformationTabPage = DeviceFormLib.BlockTabs.InformationTabPage;
using OscDiagTabPage = DeviceFormLib.BlockTabs.OscDiagTabPage;

namespace DeviceFormLib
{
    public partial class FormSirius3LV03v2 : Form, IDeviceForm, ReportLibrary.IReport
    {
        TabPage tpCurrent;
        //readonly frmEngine frmengine;
        readonly FrmEngineNew engineNew;
        readonly UInt32 uniDev = 0xffffffff;
        /// <summary>
        /// признак того что вкладка уставок уже была сформирована
        /// </summary>
        Boolean isTpConfigEntered;
        /// <summary>
        /// признак того что вкладка срабатывания уже была сформирована
        /// </summary>
        Boolean isTpRunningEntered;
        /// <summary>
        /// флаг - покаывать ли сообщение об отсутсвии архивных записей 
        /// или нет
        /// </summary>
        Boolean isMesView;
        Boolean isCurrentInfoEnter;
        readonly UstavkiViewTabPage ustavkiViewTabPage;

        public FormSirius3LV03v2( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent();

            try
            {
                uniDev = unidev;
                engineNew = new FrmEngineNew( unids, unidev, this );

                ustavkiViewTabPage = new UstavkiViewTabPage( this, unids, unidev );
                ustavkiViewTabPage.Enter += TpConfigEnter;
                tabControl.Controls.Add( ustavkiViewTabPage );

                tabControl.Controls.Add( new EventBlockTabPage( unidev ) );
                tabControl.Controls.Add( new OscDiagTabPage( unidev, OscDiagTabPage.OscDiagPanelVisible.Oscilograms ) );
                tabControl.Controls.Add( new InformationTabPage( unidev ) );
                tabControl.Controls.Add( new DataBaseFilesLibrary.TabPageDbFile( uniDev ) );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        private void FormSiriusDDeviceLoad( object sender, EventArgs e )
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
        private void FormSiriusDDeviceFormClosing( object sender, FormClosingEventArgs e )
        {
            // отписываемся от тегов
            HMI_Settings.HMIControlsTagReNew( tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe );

            ustavkiViewTabPage.Control.BlockViewControlClosing( sender, e );
        }

        public void CreateDeviceForm() { }
        public void InitInterfaceElementsClick()
        {
            tabControl.Selected += TabControlSelected;
            tpCurrentInfo.Enter += TpCurrentInfoEnter;
            tpRunning.Enter += TpRunningEnter;

            selectControl1.btnUpdate.Click += SelectControl1BtnUpdateClick;

            lstvRun.ItemActivate += LstvAvarItemActivate;
        }
        /// <summary>
        /// активировать определенную вкладку
        /// </summary>
        /// <param name="typetabpage"></param>
        public void ActivateTabPage(string typetabpage)
        {
            try
            {
                switch (typetabpage)
                {
                    case "Авария":
                        tabControl.SelectedTab = tpRunning;
                        //tabControl1.SelectedTab = tbpAvar;
                        break;
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        public void reqAvar_OnReqExecuted( IRequestData req ) { }
        /// <summary>
        /// Выбор вкладки
        /// </summary>
        private void TabControlSelected( object sender, TabControlEventArgs e )
        {
            var ustp = tpCurrent as UstavkiViewTabPage;
            if ( ustp != null )
                ustp.Control.BlockViewControlClosing( null, null );

            HMI_Settings.HMIControlsTagReNew( tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe );
            tpCurrent.Tag = false; // признак отписки от тегов для данной TabPage
            tpCurrent = e.TabPage;  // запомним новую текущую вкладку
        }
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
        private void TpRunningEnter( object sender, EventArgs e )
        {
            if ( !isTpRunningEntered )
            {
                //frmengine.PlaceTagsOnTPFlps( "Срабатывание" );
                //frmengine.CreateTPData( tpRunning, pnlUstavCopy, "Копия уставок", false );
                engineNew.PlaceTagsOnPanels( "Срабатывание" );
                engineNew.CreateAutoTabPage( tabPage2, "Копия уставок", false );
                isTpRunningEntered = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpRunning );
        }
        private void TpConfigEnter( object sender, EventArgs e )
        {
            if ( !isTpConfigEntered )
            {
                //frmengine.CreateTPData( tpConfig, pnlTPConfig, "Уставки", true );
                //engineNew.CreateAutoTabPage( pnlTPConfig, "Уставки", true );
                isTpConfigEntered = true;
                //HMI_Settings.HMIControlsTagReNew( tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe );
            }

            tpCurrent = ustavkiViewTabPage;
        }
        /// <summary>
        /// вывод информации при выборе конкретной аварии (Информация об авариях)
        /// </summary>
        private void LstvAvarItemActivate( object sender, EventArgs e )
        {
            try
            {
                if ( lstvRun.SelectedItems.Count == 0 )
                    return;

                var idBlock = (int)lstvRun.SelectedItems[0].Tag;
                var arparam = new ArrayList
                {
                    idBlock, Encoding.UTF8.GetBytes( HMI_Settings.ProviderPtkSql )
                };

                /*var req = */HMI_Settings.CONFIGURATION.GetData( 0, uniDev, "ArhivAvariBlockData", arparam, idBlock );
                //req.OnReqExecuted += reqAvar_OnReqExecuted;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Срабатывание
        /// </summary>
        private void SelectControl1BtnUpdateClick( object sender, EventArgs e )
        {
            isMesView = true;

            var list = FrmEngine.AvarUstavkyDataBase( uniDev, isMesView,
                                                      selectControl1.StartDateCollapsed,
                                                      selectControl1.EndTimeCollapsed,
                                                      TypeBlockData4Req.TypeBlockData4Req_Srabat );
            if ( list.Count == 0 )
            {
                isMesView = false;
                MessageBox.Show( "Архивных данных для этого устройства нет.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
            else
            {
                lstvRun.Items.Clear();
                lstvRun.Items.AddRange( list.ToArray() );
            }
        }

        /// <summary>
        /// Печать данных в файл
        /// </summary>
        /// <returns>Поток файла</returns>
        public void Print( )
        {
            FrmEngine.PreparationOfReportData( uniDev, tpCurrent );
        }
        /// <summary>
        /// Идентификатор блока
        /// </summary>
        public uint Guid { get { return uniDev; } }
    }
}
