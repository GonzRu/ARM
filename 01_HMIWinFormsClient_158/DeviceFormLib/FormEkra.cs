using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using HelperControlsLibrary;

using InterfaceLibrary;
using HMI_MT_Settings;
using System.Globalization;
using System.Collections.Generic;

namespace DeviceFormLib
{
    public partial class FormEkra : Form, IDeviceForm, ReportLibrary.IReport
    {
        TabPage tpCurrent;
        readonly FrmEngineNew engineNew;
        readonly UInt32 uniDev = 0xffffffff;
        /// <summary>
        /// признак того что вкладка уставок уже была сформирована
        /// </summary>
        Boolean isTpConfigEntered;
        /// <summary>
        /// флаг - покаывать ли сообщение об отсутсвии архивных записей 
        /// или нет
        /// </summary>
        Boolean isMesView;
        Boolean isCurrentInfoEnter;

        public FormEkra( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent();
            try
            {
                uniDev = unidev;
                engineNew = new FrmEngineNew( unids, unidev, this );

                tabControl1.Controls.Add( new EventBlockTabPage( unidev ) );
                tabControl1.Controls.Add( new OscDiagTabPage( unidev, OscDiagTabPage.OscDiagPanelVisible.Oscilograms ) );
                tabControl1.Controls.Add( new InformationTabPage( unidev ) );
                tabControl1.Controls.Add( new DataBaseFilesLibrary.TabPageDbFile( uniDev ) );
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
                // frmengine.InitFrmLight( this, tabControl1, null );
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
            this.tpCurrentInfo.Enter += this.TpCurrentInfoEnter;
            this.tabPageCurStateInfoDev.Enter += this.TpCurStateInfoDevEnter;
            this.tpConfig.Enter += this.TpConfigEnter;

            this.selectUserControl2.btnUpdate.Click += this.SelectUserControl2BtnUpdateClick;
            this.readWriteUstavky1.btnReadUstFC.Click += this.BtnReadUstFcClick;
            this.readWriteUstavky1.btnWriteUst.Click += this.BtnWriteUstClick;
            this.readWriteUstavky1.btnResetValues.Click += this.BtnResetValuesClick;
            this.readWriteUstavky1.btnFix4Change.CheckedChanged += BtnFix4ChangeOnCheckedChanged;

            this.lstvConfig.ItemActivate += this.LstvConfigItemActivate;
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
        private void TpCurrentInfoEnter( object sender, EventArgs e )
        {
            //CommonUtils.CommonUtils.RemoveHMIUserControls(tpCurrentInfo);
            if ( !isCurrentInfoEnter )
            {
                this.engineNew.PlaceTagsOnPanels( "Контроль" );
                isCurrentInfoEnter = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpCurrentInfo );
        }
        private void TpCurStateInfoDevEnter( object sender, EventArgs e )
        {
            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tabPageCurStateInfoDev );
        }
        private void TpConfigEnter( object sender, EventArgs e )
        {
            if ( !isTpConfigEntered )
            {
                //frmengine.CreateTPData( tpConfig, pnlTPConfig, "Уставки", true );
                this.engineNew.CreateAutoTabPage( pnlTPConfig, "Уставки", true );
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

                IRequestData req = HMI_Settings.CONFIGURATION.GetData( 0, uniDev, "ArhivUstavkiBlockData", arparam, idBlock );
                req.OnReqExecuted += reqAvar_OnReqExecuted;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Конфигурация и уставки
        /// </summary>
        private void SelectUserControl2BtnUpdateClick( object sender, EventArgs e )
        {
            isMesView = true;

            var list = FrmEngine.AvarUstavkyDataBase( uniDev,
                                                      isMesView,
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
                var lstModifiedTags = engineNew.GetChangedTags(pnlTPConfig);//frmengine.GetList4WriteUst();

                if (!lstModifiedTags.Any())
                {
                    MessageBox.Show("Уставки не изменялись. \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HMI_Settings.SetTagsSubscribe4TPCurrent(tpConfig);
                    return;
                }

                if (CommonUtils.CommonUtils.IsUserActionBan(CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, HMI_Settings.UserRight))
                {
                    HMI_Settings.SetTagsSubscribe4TPCurrent(tpConfig);
                    return;
                }

                if (HMI_Settings.isRegPass && CommonUtils.CommonUtils.CanAction())
                {
                    if (MessageBox.Show("Записать уставки?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        HMI_Settings.SetTagsSubscribe4TPCurrent(tpConfig);
                        return;
                    }
                    // правильная запись в журнал действий пользователя номер устройства с учетом фк
                    CommonUtils.CommonUtils.WriteEventToLog(6, uniDev.ToString(CultureInfo.InvariantCulture), true);
                    //"выдана команда WCP - запись уставок."

                    HMI_Settings.CONFIGURATION.ExecuteCommand(0, uniDev, "WCP",
                                                               CommonUtils.CommonUtils.GetUstConfMemXPacket(
                                                                   (List<ITag>)lstModifiedTags), this);

                    // сбрасываем признаки изменения тега
                    CommonUtils.CommonUtils.ResetIndicationModifiedTag(tpConfig);
                    HMI_Settings.SetTagsSubscribe4TPCurrent(tpConfig);
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
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
            if (chBox.Checked)
            {
                HMI_Settings.HMIControlsTagReNew(tpConfig, HMI_Settings.SubscribeAction.UnSubscribe);
                tpConfig.Tag = false;
            }
            else
            {
                var lstModifiedTags = engineNew.GetChangedTags(pnlTPConfig);//this.frmengine.GetList4WriteUst();
                if (!lstModifiedTags.Any())
                    HMI_Settings.SetTagsSubscribe4TPCurrent(tpConfig);
            }
        }
        /// <summary>
        /// Печать данных в файл
        /// </summary>
        /// <returns>Поток файла</returns>
        public void Print()
        {
            FrmEngine.PreparationOfReportData(uniDev, tpCurrent);
        }
        /// <summary>
        /// Идентификатор блока
        /// </summary>
        public uint Guid { get { return uniDev; } }
    }
}
