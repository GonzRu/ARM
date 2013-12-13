using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Windows.Forms;
using InterfaceLibrary;
using System.IO;
using HMI_MT_Settings;
using OscillogramsLib;
using System.Globalization;

using TypeBlockData4Req = InterfaceLibrary.TypeBlockData4Req;
using DeviceFormLib.BlockTabs;

namespace DeviceFormLib
{
    public partial class Form4Device : Form, IDeviceForm
    {
        TabPage tpCurrent;
        readonly frmEngine frmengine;
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
        /// <summary>
        /// Номер выбранной позиции комбобокса
        /// </summary>
        int comboChoiseIndex;

        public Form4Device( UInt32 unids, UInt32 unidev, string name )
        {
            InitializeComponent();

            try
            {
                uniDev = unidev;
                frmengine = new frmEngine( unids, unidev, this );
                frmengine.Dev.TypeTagPriorityView = TypeViewTag.PRIMARY; // по умолчанию первичные значения

                tabControl.Controls.Add( new EventBlockTabPage( unidev ) );
                tabControl.Controls.Add( new OscDiagTabPage( unidev, OscDiagTabPage.OscDiagPanelVisible.Oscilograms ) );
                tabControl.Controls.Add( new InformationTabPage( unidev ) );
                tabControl.Controls.Add( new DataBaseFilesLibrary.TabPageDbFile( uniDev ) );

                comboChoiseIndex = this.cbTypeInfo1.SelectedIndex =
                                   this.cbTypeInfo2.SelectedIndex = this.cbTypeInfo3.SelectedIndex = 0;

                switch ( name )
                {
                    case "БМРЗ-104ТН-05-230511":
                        splitContainer10.Panel1Collapsed = true; // выключатель
                        break;
                    case "БРЧН-100-Б-01-191110":
                        splitContainer9.Panel1Collapsed = true; // статус
                        splitContainer10.Panel1Collapsed = true; // выключатель
                        break;
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }

        /// <summary>
        /// загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm4DeviceLoad(object sender, EventArgs e)
        {
            try
            {
                frmengine.InitFrm( this, tabControl, null );
                InitInterfaceElementsClick();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// закрытие формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm4DeviceFormClosing(object sender, FormClosingEventArgs e)
        {
            // отписываемся от тегов
            HMI_Settings.HMIControlsTagReNew(tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe);
        }
        
        public void CreateDeviceForm() { }
        public void InitInterfaceElementsClick()
        {
            foreach ( TabPage control in tabControl.Controls )
                control.Enter += TabEnter;

            this.cbTypeInfo1.SelectedIndexChanged += this.CbTypeInfoSelectedIndexChanged;
            this.cbTypeInfo2.SelectedIndexChanged += this.CbTypeInfoSelectedIndexChanged;
            this.cbTypeInfo3.SelectedIndexChanged += this.CbTypeInfoSelectedIndexChanged;
            this.tabControl.Selected += this.TabControlSelected;
            this.selectControl1.btnUpdate.Click += this.SelectControl1BtnUpdateClick;
            this.selectControl2.btnUpdate.Click += this.SelectControl2BtnUpdateClick;
            this.readWriteUstavky1.btnReadUstFC.Click += this.BtnReadUstFcClick;
            this.readWriteUstavky1.btnWriteUst.Click += this.BtnWriteUstClick;
            this.readWriteUstavky1.btnResetValues.Click += this.BtnResetValuesClick;
            this.readWriteUstavky1.btnFix4Change.CheckedChanged += BtnFix4ChangeOnCheckedChanged;

            this.lstvAvar.ItemActivate += this.LstvAvarItemActivate;
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
                        tabControl.SelectedTab = tabEmergency;
                        break;
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        public void reqAvar_OnReqExecuted( IRequestData req )
        {
            /*
             * в результате запроса возвращаются два 
             * числа, кот интерпретируются как номер 
             * групп срабатывания и пуска => необходимо 
             * на вкладке аварии сгенерировать панели для данных
             * срабатывания и пуска
             */
            using ( var ms = new MemoryStream( req.ReqParamsAsByteAray ) )
                using ( var br = new BinaryReader( ms ) )
                {
                    ms.Position = 0;
                    try
                    {
                        var tsrabat = br.ReadUInt32();
                        var tsrabatms = br.ReadUInt16();
                        /*var typereqsrabat =*/
                        br.ReadUInt16();

                        var idgrsrab = br.ReadInt32();
                        var idgrpusk = br.ReadInt32();

                        // теперь нужно создать теги на вкладке аварий
                        tabControl.SelectedTab = tabEmergency;

                        // отписываемся от прежних тегов
                        HMI_Settings.HMIControlsTagReNew( tabEmergency, HMI_Settings.SubscribeAction.UnSubscribe );
                        string nameSrabat;
                        frmengine.CreateTP4AvarData( idgrsrab, gbsrabat, idgrpusk, gbpusk, out nameSrabat );
                        HMI_Settings.HMIControlsTagReNew( tabEmergency, HMI_Settings.SubscribeAction.Subscribe );
                        var varMtValue = new DateTime( 1970, 1, 1, 0, 0, 0, 0 );
                        varMtValue = varMtValue.AddSeconds( tsrabat );
                        varMtValue = varMtValue.AddMilliseconds( tsrabatms );

                        // название аварии                
                        lblTimeAvar.Text = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat( varMtValue );
                        lblAvarType.Text = nameSrabat;
                    }
                    catch ( Exception ex )
                    {
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                    }
                }
        }
        /// <summary>
        /// изменить тип тега для отображения - первичный вторичный и т.п.
        /// </summary>
        private void CbTypeInfoSelectedIndexChanged( object sender, EventArgs e )
        {
            try
            {
                // запоминание выбранного индекса
                comboChoiseIndex = ( (ComboBox)sender ).SelectedIndex;
                switch ( comboChoiseIndex )
                {
                    case 0:
                        frmengine.Dev.TypeTagPriorityView = TypeViewTag.PRIMARY;
                        break;
                    case 1:
                        frmengine.Dev.TypeTagPriorityView = TypeViewTag.SECONDARY;
                        break;
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
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
        private void TabEnter( object sender, EventArgs e )
        {
            if ( sender == tpConfig && !isTpConfigEntered )
            {
                frmengine.CreateTPData( (TabPage)sender, pnlTPConfig, "Уставки", true );
                isTpConfigEntered = true;
            }

            // установка выбранного индекса
            this.cbTypeInfo1.SelectedIndex = this.cbTypeInfo2.SelectedIndex = this.cbTypeInfo3.SelectedIndex = comboChoiseIndex;

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( (TabPage)sender );
        }

        /// <summary>
        /// Информация об авариях
        /// </summary>
        private void SelectControl1BtnUpdateClick( object sender, EventArgs e )
        {
            isMesView = true;

            var list = HelperControlsLibrary.FrmEngine.AvarUstavkyDataBase( uniDev, isMesView,
                                                                            selectControl1.StartDateCollapsed,
                                                                            selectControl1.EndTimeCollapsed,
                                                                            TypeBlockData4Req.TypeBlockData4Req_Srabat );
            if ( list.Count == 0 )
            {
                isMesView = false;
                MessageBox.Show( "Архивных данных по авариям для этого устройства нет.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information );
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
                    idBlock, Encoding.UTF8.GetBytes( HMI_Settings.ProviderPtkSql )
                };

                var req = HMI_Settings.CONFIGURATION.GetData( 0, uniDev, "ArhivAvariBlockData", arparam, idBlock );
                req.OnReqExecuted += reqAvar_OnReqExecuted;
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
                    idBlock, Encoding.UTF8.GetBytes( HMI_Settings.ProviderPtkSql )
                };

                var req = HMI_Settings.CONFIGURATION.GetData( 0, uniDev, "ArhivUstavkiBlockData", arparam, idBlock );
                req.OnReqExecuted += this.reqAvar_OnReqExecuted;
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
                HMI_Settings.HMIControlsTagReNew( this.tpConfig, HMI_Settings.SubscribeAction.UnSubscribe );
                this.tpConfig.Tag = false;
            }
            else
            {
                var lstModifiedTags = this.frmengine.GetList4WriteUst();
                if ( lstModifiedTags.Count == 0 )
                    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
            }
        }
        /// <summary>
        /// Идентификатор блока
        /// </summary>
        public uint Guid { get { return uniDev; } }
    }
}
