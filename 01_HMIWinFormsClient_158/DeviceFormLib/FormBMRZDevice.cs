using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using DeviceFormLib.BlockTabs;
using InterfaceLibrary;
using HMI_MT_Settings;
using System.Collections;
using TypeBlockData4Req = InterfaceLibrary.TypeBlockData4Req;

namespace DeviceFormLib
{
    public partial class FormBmrzDevice : Form, IDeviceForm, ReportLibrary.IReport
    {
        private TabPage tpCurrent;
        private readonly frmEngine frmengine;
        private readonly UInt32 uniDev = 0xffffffff;
        private readonly OscDiagTabPage oscDiagTabPage;
        /// <summary>
        /// признак того что вкладка уставок уже была сформирована
        /// </summary>
        private Boolean isTpConfigEntered;
        /// <summary>
        /// флаг - покаывать ли сообщение об отсутсвии архивных записей 
        /// или нет
        /// </summary>
        private Boolean isMesView;

        public FormBmrzDevice( UInt32 unids, UInt32 unidev, string type )
        {
            InitializeComponent();
            try
            {
                uniDev = unidev;
                frmengine = new frmEngine( unids, unidev, this );

                //tabControl.Controls.Add( new EventBlockTabPage( unidev ) );
                oscDiagTabPage = new OscDiagTabPage( unidev );
                tabControl.Controls.Add( oscDiagTabPage );
                //tabControl.Controls.Add( new InformationTabPage( unidev ) );
                tabControl.Controls.Add( new DataBaseFilesLibrary.TabPageDbFile( unidev ) );
                this.InitBlockPanels( type );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Инициализация под конкретный блок
        /// </summary>
        /// <param name="type">Тип блока</param>
        private void InitBlockPanels( string type )
        {
            switch ( type )
            {
                #region Old
                //case "БМРЗ-ТР-06-40-14":
                //    tabControl.Controls.Remove( tabRPN ); // вкладка РПН
                //    oscDiagTabPage.SetPanelVisible( OscDiagTabPage.OscDiagPanelVisible.Oscilograms ); // диаграммы
                //    break;
                //case "БМРЗ-БМПА-04-04":
                //    tabControl.Controls.Remove( tabRPN ); // вкладка РПН
                //    tabControl.Controls.Remove( tpConfig ); // вкладка "уставки"
                //    splitContainer6.Panel1Collapsed = true; // аналоговые сигналы
                //    splitContainer19.Panel1Collapsed = true; // признаки пуска защиты
                //    splitContainer17.Panel2Collapsed = true; // пуск и срабатывание защиты
                //    splitContainer10.Panel2Collapsed = true; // счетчики
                //    splitContainer23.Panel2Collapsed = true; // максметр
                //    splitContainer4.Panel2Collapsed = true; // статус 2
                //    maxMeterControl1.groupBox2.Visible = false; // кнопки максметра
                //    break;
                #endregion
                
                case "BMRZ_CRN_01_01_11":
                    splitContainer22.Panel2Collapsed = true; // выключатель
                    splitContainer4.Panel2Collapsed = true; // статус 2
                    break;
                case "BMRZ_BRCN_100_A_01_111110":
                case "BMRZ_BRCN_100_A_01_250108":
                    tabControl.Controls.Remove( tabRPN ); // вкладка РПН
                    tabControl.Controls.Remove( tabEmergency ); // вкладка Информация об авариях
                    tabControl.Controls.Remove( tpAccumulationInfo ); // вкладка Накопительная информация
                    splitContainer11.Panel2Collapsed = true; // вызов
                    splitContainer3.Panel1Collapsed = true; // неисправность бмрз
                    break;
                case "BMRZ_DZSH_02_11":
                    tabControl.Controls.Remove( tabRPN ); // вкладка РПН
                    splitContainer4.Panel2Collapsed = true; // статус 2
                    splitContainer22.Panel2Collapsed = true; // выключатель
                    break;
                default:
                    tabControl.Controls.Remove( tabRPN ); // вкладка РПН
                    splitContainer4.Panel2Collapsed = true; // статус 2
                    break;

            }
        }
        /// <summary>
        /// Открытие формы
        /// </summary>
        private void FormBmrzDeviceLoad( object sender, EventArgs e )
        {
            try
            {
                frmengine.InitFrm( this, tabControl, null );
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
            foreach ( TabPage control in tabControl.Controls )
                control.Enter += TabEnter;

            selectUserControl1.btnUpdate.Click += SelectUserControl1BtnUpdateClick;
            maxMeterControl1.btnNakopitelRead.Click += BtnNakopitelReadClick;
            maxMeterControl1.btnNakopitelReset.Click += BtnNakopitelResetClick;
            maxMeterControl1.btnMaxmeterRead.Click += BtnMaxmeterReadClick;
            maxMeterControl1.btnMaxmeterReset.Click += BtnMaxmeterResetClick;
            selectUserControl2.btnUpdate.Click += SelectUserControl2BtnUpdateClick;
            readWriteUstavky1.btnReadUstFC.Click += BtnReadUstFcClick;
            readWriteUstavky1.btnWriteUst.Click += BtnWriteUstClick;
            readWriteUstavky1.btnResetValues.Click += BtnResetValuesClick;
            readWriteUstavky1.btnFix4Change.CheckedChanged += BtnFix4ChangeOnCheckedChanged;

            lstvAvar.ItemActivate += LstvAvarItemActivate;
            lstvConfig.ItemActivate += LstvConfigItemActivate;
        }
        /// <summary>
        /// активировать определенную вкладку
        /// </summary>
        /// <param name="typetabpage"></param>
        public void ActivateTabPage( string typetabpage )
        {
            try
            {
                switch ( typetabpage )
                {
                    case "Авария":
                        tabControl.SelectedTab = tabEmergency;
                        break;
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        public void reqAvar_OnReqExecuted( IRequestData req ) { }
        /// <summary>
        /// Вход на вкладку
        /// </summary>
        private void TabEnter( object sender, EventArgs e )
        {
            if ( sender == tpConfig && !isTpConfigEntered )
            {
                frmengine.CreateTPData( (TabPage)sender, pnlTPConfig, "Уставки", true );
                isTpConfigEntered = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( (TabPage)sender );
        }

        /// <summary>
        /// Информация об авариях
        /// </summary>
        private void SelectUserControl1BtnUpdateClick( object sender, EventArgs e )
        {
            isMesView = true;

            var list = HelperControlsLibrary.FrmEngine.AvarUstavkyDataBase( uniDev, isMesView,
                                                                            selectUserControl1.StartDateCollapsed,
                                                                            selectUserControl1.EndTimeCollapsed,
                                                                            TypeBlockData4Req.TypeBlockData4Req_Srabat );
            if ( list.Count == 0 )
            {
                isMesView = false;
                MessageBox.Show( "Архивных данных по авариям для этого устройства нет.", Text, MessageBoxButtons.OK,
                                 MessageBoxIcon.Information );
            }
            else
            {
                lstvAvar.Items.Clear();
                lstvAvar.Items.AddRange( list.ToArray() );
            }
        }
        /// <summary>
        /// вывод информации при выборе конкретной аварии (Информация об авариях)
        /// </summary>
        private void LstvAvarItemActivate( object sender, EventArgs e )
        {
            try
            {
                if ( lstvAvar.SelectedItems.Count == 0 )
                    return;

                var idBlock = (int)lstvAvar.SelectedItems[0].Tag;
                var arparam = new ArrayList
                    {
                        idBlock,
                        Encoding.UTF8.GetBytes( HMI_Settings.ProviderPtkSql )
                    };

                /*var req =*/
                HMI_Settings.CONFIGURATION.GetData( 0, uniDev, "ArhivAvariBlockData", arparam, idBlock );
                //req.OnReqExecuted += reqAvar_OnReqExecuted;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// выбор конкретной записи из списка
        /// </summary>
        private void LstvConfigItemActivate( object sender, EventArgs e )
        {
            try
            {
                if ( lstvConfig.SelectedItems.Count == 0 )
                    return;

                var idBlock = (int)lstvConfig.SelectedItems[0].Tag;
                var arparam = new ArrayList
                    {
                        idBlock,
                        Encoding.UTF8.GetBytes( HMI_Settings.ProviderPtkSql )
                    };

                /*IRequestData req = */
                HMI_Settings.CONFIGURATION.GetData( 0, uniDev, "ArhivUstavkiBlockData", arparam, idBlock );
                HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                //req.OnReqExecuted += new ReqExecuted( reqAvar_OnReqExecuted );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Накопитель (чтение)
        /// </summary>
        private void BtnNakopitelReadClick( object sender, EventArgs e )
        {
            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 8, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "IMC", new byte[] { }, this );
        }
        /// <summary>
        /// Накопитель (сброс)
        /// </summary>
        private void BtnNakopitelResetClick( object sender, EventArgs e )
        {
            if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b06_Reset_info,
                                                          HMI_Settings.UserRight ) )
                return;

            var dr = MessageBox.Show( "Сбросить накопительную информацию блока?", "Предупреждение",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question );
            if ( dr != DialogResult.Yes )
                return;

            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 35, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "CCD", new byte[] { }, this );
        }
        /// <summary>
        /// Максметр (чтение)
        /// </summary>
        private void BtnMaxmeterReadClick( object sender, EventArgs e )
        {
            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк            
            CommonUtils.CommonUtils.WriteEventToLog( 10, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "IMD", new byte[] { }, this );
        }
        /// <summary>
        /// Максметр (сброс)
        /// </summary>
        private void BtnMaxmeterResetClick( object sender, EventArgs e )
        {
            if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b06_Reset_info,
                                                          HMI_Settings.UserRight ) )
                return;

            var dr = MessageBox.Show( "Сбросить максметр?", "Предупреждение", MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question );
            if ( dr != DialogResult.Yes )
                return;

            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 35, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "CMD", new byte[] { }, this );
        }
        /// <summary>
        /// Конфигурация и уставки
        /// </summary>
        private void SelectUserControl2BtnUpdateClick( object sender, EventArgs e )
        {
            isMesView = true;

            var list = HelperControlsLibrary.FrmEngine.AvarUstavkyDataBase( uniDev, isMesView,
                                                                            selectUserControl2.StartDateCollapsed,
                                                                            selectUserControl2.EndTimeCollapsed,
                                                                            TypeBlockData4Req.TypeBlockData4Req_Ustavki );
            if ( list.Count == 0 )
            {
                isMesView = false;
                MessageBox.Show( "Архивных данных для этого устройства нет.", Text, MessageBoxButtons.OK,
                                 MessageBoxIcon.Information );
            }
            else
            {
                lstvConfig.Items.Clear();
                lstvConfig.Items.AddRange( list.ToArray() );
            }
        }
        /// <summary>
        /// Чтение уставок
        /// </summary>
        private void BtnReadUstFcClick( object sender, EventArgs e )
        {
            // правильная запись в журнал действий пользователя номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 7, uniDev.ToString( CultureInfo.InvariantCulture ), true );
            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "IMP", new byte[] { }, this );

            //HMI_Settings.HMIControlsTagReNew( tpConfig, HMI_Settings.SubscribeAction.Subscribe );
            HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
        }
        /// <summary>
        /// Запись уставок
        /// </summary>
        private void BtnWriteUstClick( object sender, EventArgs e )
        {
            try
            {
                // определим теги которые изменились
                var lstModifiedTags = frmengine.GetList4WriteUst();

                if ( lstModifiedTags.Count == 0 )
                {
                    MessageBox.Show( "Уставки не изменялись. \nВыполнение команды отменено.", "Сообщение",
                                     MessageBoxButtons.OK, MessageBoxIcon.Information );
                    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                    return;
                }

                if (
                    CommonUtils.CommonUtils.IsUserActionBan(
                        CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, HMI_Settings.UserRight ) )
                {
                    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                    return;
                }

                if ( HMI_Settings.isRegPass && CommonUtils.CommonUtils.CanAction() )
                {
                    if (
                        MessageBox.Show( "Записать уставки?", "Предупреждение", MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question ) == DialogResult.No )
                    {
                        HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                        return;
                    }
                    // правильная запись в журнал действий пользователя номер устройства с учетом фк
                    CommonUtils.CommonUtils.WriteEventToLog( 6, uniDev.ToString( CultureInfo.InvariantCulture ), true );
                    //"выдана команда WCP - запись уставок."

                    HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "WCP",
                                                               CommonUtils.CommonUtils.GetUstConfMemXPacket(
                                                                   lstModifiedTags ), this );

                    // сбрасываем признаки изменения тега
                    CommonUtils.CommonUtils.ResetIndicationModifiedTag( tpConfig );
                    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Очистить поля формы
        /// </summary>
        private void BtnResetValuesClick( object sender, EventArgs e )
        {
            // отписались и обнулились
            HMI_Settings.HMIControlsTagReNew( tpConfig, HMI_Settings.SubscribeAction.UnSubscribeAndClear );
            tpConfig.Tag = false;
        }
        /// <summary>
        /// Проверка режима задания уставок
        /// </summary>
        private void BtnFix4ChangeOnCheckedChanged( object sender, EventArgs eventArgs )
        {
            var chBox = (CheckBox)sender;
            if ( chBox.Checked )
            {
                HMI_Settings.HMIControlsTagReNew( tpConfig, HMI_Settings.SubscribeAction.UnSubscribe );
                tpConfig.Tag = false;
            }
            else
            {
                var lstModifiedTags = frmengine.GetList4WriteUst();
                if ( lstModifiedTags.Count == 0 )
                    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
            }
        }
        /// <summary>
        /// Выбор вкладки
        /// </summary>
        private void TabControlSelected( object sender, TabControlEventArgs e )
        {
            HMI_Settings.HMIControlsTagReNew( tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe );
            tpCurrent.Tag = false; // признак отписки от тегов для данной TabPage
            tpCurrent = e.TabPage; // запомним новую текущую вкладку
        }
        /// <summary>
        /// РПН (прибавить)
        /// </summary>
        private void BtnRpnReadClick( object sender, EventArgs e )
        {
            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк            
            CommonUtils.CommonUtils.WriteEventToLog( 36, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "INC", new byte[] { }, this );
        }
        /// <summary>
        /// РПН (убавить)
        /// </summary>
        private void BtnRpnWriteClick( object sender, EventArgs e )
        {
            if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b07_RPN,
                                                          HMI_Settings.UserRight ) )
                return;

            DialogResult dr = MessageBox.Show( "Записать РПН?", "Предупреждение", MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question );
            if ( dr != DialogResult.Yes )
                return;

            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 37, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "DEC", new byte[] { }, this );
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