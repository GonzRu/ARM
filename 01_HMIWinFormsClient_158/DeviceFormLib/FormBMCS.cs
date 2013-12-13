using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DebugStatisticLibrary;

using InterfaceLibrary;
using System.Collections;
using HMI_MT_Settings;
using System.Globalization;

using TypeBlockData4Req = InterfaceLibrary.TypeBlockData4Req;
using DeviceFormLib.BlockTabs;

namespace DeviceFormLib
{
    public partial class FormBmcs : Form, IDeviceForm
    {
        TabPage tpCurrent;
        //readonly frmEngine frmengine;
        readonly FrmEngineNew engineNew;
        readonly UInt32 uniDs, uniDev = 0xffffffff;
        /// <summary>
        /// признак того что вкладка уставок уже была сформирована
        /// </summary>
        Boolean isTpConfigEntered;
        /// <summary>
        /// признак того что вкладка срабатывания уже была сформирована
        /// </summary>
        Boolean isTpRunningEntered;
        /// <summary>
        /// признак того что вкладка индикаторы уже была сформирована
        /// </summary>
        Boolean isTpIndicatorsEntered;
        /// <summary>
        /// флаг - покаывать ли сообщение об отсутсвии архивных записей 
        /// или нет
        /// </summary>
        Boolean isMesView;
        Boolean isCurrentInfoEnter;

        public FormBmcs( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent();

            try
            {
                uniDs = unids;
                uniDev = unidev;
                //frmengine = new frmEngine( unids, unidev, this );
                engineNew = new FrmEngineNew( unids, unidev, this );

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
        private void InitIndicators()
        {
            var device = HMI_Settings.CONFIGURATION.GetLink2Device( uniDs, uniDev );
            if ( device == null )
            {
                DebugStatistics.WindowStatistics.AddStatistic( string.Format( "Ошибка получения устройства для инициализации индикаторов (DsGuid:{0} DevGuid:{1})", uniDs, uniDs ) );
                return;
            }

            var group = FrmEngineNew.GetGroupByTypeOfPanel( device.GetGroupHierarchy( ), "Индикация" );
            if ( group == null )
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Не найдена группа индикаторов" );
                return;
            }

            var tags = FrmEngineNew.CollectTagDescriptions( group, uniDs, uniDev );
            if ( tags == null )
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Не найдено описание тэгов в группе индикаторов" );
                return;
            }

            foreach ( var tag in tags )
            {
                var indicator = new HelperControlsLibrary.IndicatorControl();
                indicator.BindingSignal(tag.Title, uniDs, uniDev, tag.Source.TagGUID);
                flowLayoutPanel2.Controls.Add(indicator);
            }
        }
        private void FormSiriusDDeviceFormClosing( object sender, FormClosingEventArgs e )
        {
            // отписываемся от тегов
            HMI_Settings.HMIControlsTagReNew( tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe );
        }

        public void CreateDeviceForm() { }
        public void InitInterfaceElementsClick()
        {
            tabControl.Selected += this.TabControlSelected;
            tpCurrentInfo.Enter += this.TpCurrentInfoEnter;
            tpIndicators.Enter += this.TpIndicatorsEnter;
            tpRunning.Enter += this.TpRunningEnter;
            tpConfig.Enter += this.TpConfigEnter;

            this.selectControl2.btnUpdate.Click += this.SelectControl2BtnUpdateClick;
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
        private void TpIndicatorsEnter( object sender, EventArgs e )
        {
            if ( !isTpIndicatorsEntered )
            {
                this.InitIndicators( );
                isTpIndicatorsEntered = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpIndicators );
        }
        private void TpRunningEnter( object sender, EventArgs e )
        {
            if ( !isTpRunningEntered )
            {
                //frmengine.PlaceTagsOnTPFlps( "Накопительная информация" );
                engineNew.PlaceTagsOnPanels( "Накопительная информация" );
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
                engineNew.PlaceTagsOnPanels( "Уставки", true );
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

                /*IRequestData req = */HMI_Settings.CONFIGURATION.GetData( 0, uniDev, "ArhivUstavkiBlockData", arparam, idBlock );
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
                                                                            selectControl2.StartDateCollapsed,
                                                                            selectControl2.EndTimeCollapsed,
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
                var lstModifiedTags = engineNew.GetChangedTags( tableLayoutPanel5 );//frmengine.GetList4WriteUst();

                if ( !lstModifiedTags.Any() )
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
                                                                   (List<ITag>)lstModifiedTags ), this );

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
                HMI_Settings.HMIControlsTagReNew( this.tpConfig, HMI_Settings.SubscribeAction.UnSubscribe );
                this.tpConfig.Tag = false;
            }
            else
            {
                var lstModifiedTags = engineNew.GetChangedTags( tableLayoutPanel5 );//this.frmengine.GetList4WriteUst();
                if ( !lstModifiedTags.Any() )
                    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
            }
        }
        /// <summary>
        /// Чтение накопительной информации
        /// </summary>
        private void ButtonReadClick( object sender, EventArgs e )
        {
            // правильная запись в журнал действий пользователя номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 35, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "RCD", new byte[] { }, this );
        }
        /// <summary>
        /// Сброс накопительной информации
        /// </summary>
        private void ButtonResetClick( object sender, EventArgs e )
        {
            if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b06_Reset_info, HMI_Settings.UserRight ) )
                return;

            if ( MessageBox.Show( "Выполнить сброс?", "Попытка сброса информации", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.No )
                return;

            // правильная запись в журнал действий пользователя номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 35, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "CCD", new byte[] { }, this );
        }
        /// <summary>
        /// Идентификатор блока
        /// </summary>
        public uint Guid { get { return uniDev; } }
    }
}
