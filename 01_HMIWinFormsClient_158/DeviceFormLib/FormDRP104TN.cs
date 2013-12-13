using System;
using System.Text;
using System.Windows.Forms;
using DeviceFormLib.BlockTabs;
using InterfaceLibrary;
using HMI_MT_Settings;
using System.Collections;
using System.Globalization;

namespace DeviceFormLib
{
    public partial class FormDRP104TN : Form, IDeviceForm, ReportLibrary.IReport
    {
        private TabPage tpCurrent;
        private readonly frmEngine frmengine;
        private readonly UInt32 uniDev = 0xffffffff;
        /// <summary>
        /// признак того что вкладка уставок уже была сформирована
        /// </summary>
        Boolean isTpConfigEntered;
        /// <summary>
        /// флаг - покаывать ли сообщение об отсутсвии архивных записей 
        /// или нет
        /// </summary>
        Boolean isMesView;

        public FormDRP104TN( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent();
            try
            {
                uniDev = unidev;
                frmengine = new frmEngine( unids, unidev, this );

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
            tabControl.Selected += TabControlSelected;
            tpCurrentInfo.Enter += TpCurrentInfoEnter;
            tpJournal.Enter += TpJournalEnter;
            tpConfig.Enter += TpConfigEnter;

            selectUserControl2.btnUpdate.Click += SelectControl2BtnUpdateClick;
            readWriteUstavky1.btnReadUstFC.Click += BtnReadUstFcClick;
            readWriteUstavky1.btnWriteUst.Click += BtnWriteUstClick;
            readWriteUstavky1.btnResetValues.Click += BtnResetValuesClick;
            readWriteUstavky1.btnFix4Change.CheckedChanged += BtnFix4ChangeOnCheckedChanged;

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
                        tabControl.SelectedTab = tpJournal;
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
        /// Выбор вкладки
        /// </summary>
        private void TabControlSelected( object sender, TabControlEventArgs e )
        {
            HMI_Settings.HMIControlsTagReNew( tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe );
            tpCurrent.Tag = false; // признак отписки от тегов для данной TabPage
            tpCurrent = e.TabPage;  // запомним новую текущую вкладку
        }
        private void TpCurrentInfoEnter( object sender, EventArgs e )
        {
            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpCurrentInfo );
        }
        private void TpJournalEnter( object sender, EventArgs e )
        {
            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpJournal );
        }
        private void TpConfigEnter( object sender, EventArgs e )
        {
            if ( !isTpConfigEntered )
            {
                frmengine.CreateTPData( tpConfig, pnlTPConfig, "Уставки", true );
                isTpConfigEntered = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
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
                    idBlock, Encoding.UTF8.GetBytes( HMI_Settings.ProviderPtkSql )
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
        /// Конфигурация и уставки
        /// </summary>
        private void SelectControl2BtnUpdateClick( object sender, EventArgs e )
        {
            isMesView = true;

            var list = HelperControlsLibrary.FrmEngine.AvarUstavkyDataBase( uniDev, isMesView,
                                                                            selectUserControl2.StartDateCollapsed,
                                                                            selectUserControl2.EndTimeCollapsed,
                                                                            TypeBlockData4Req.TypeBlockData4Req_Ustavki );
            if ( list.Count == 0 )
            {
                isMesView = false;
                MessageBox.Show( "Архивных данных для этого устройства нет.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information );
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
                    MessageBox.Show( "Уставки не изменялись. \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information );
                    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                    return;
                }

                if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, HMI_Settings.UserRight ) )
                {
                    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                    return;
                }

                if ( HMI_Settings.isRegPass && CommonUtils.CommonUtils.CanAction() )
                {
                    if ( MessageBox.Show( "Записать уставки?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.No )
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