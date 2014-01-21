using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

using HMI_MT_Settings;
using InterfaceLibrary;

namespace HelperControlsLibrary
{
    /// <summary>
    /// Вкладка отображения всех данных блока
    /// </summary>
    public class BlockViewTagPage : TabPage, ReportLibrary.IReport
    {
        private readonly Form parent;
        private readonly UInt32 uniDs, uniDev;
        private DataGridView dBGridView;
        private BlockViewControl blockViewControl;
        private SelectControl selectControl;
        private ReadWriteUstavkyControl readWriteUstavkyControl;
        private StorageDeviceControl storageDeviceControl;
        private MaxMeterControl maxMeterControl;
        private FlowLayoutPanel buttonLayoutPanel;
        private FlowLayoutPanel botomFlowLayoutPanel;
        private TableLayoutPanel tableLayoutPanel;
        private Category currentCategory = Category.NaN;

        /// <summary>
        /// Конструктор вкладки
        /// </summary>
        /// <param name="parent">Окно родитель</param>
        /// <param name="unids">Идентификатор дата сервера</param>
        /// <param name="unidev">Guid устройства</param>
        /// <param name="text">Имя вкладки</param>
        public BlockViewTagPage( Form parent, UInt32 unids, UInt32 unidev, string text = "Данные блока" )
            : base( text )
        {
            this.parent = parent;
            this.uniDs = unids;
            this.uniDev = unidev;

            this.InitializeComponent( );
        }

        /// <summary>
        /// Раскрывает вкладку и активирует вывод данных из нее
        /// </summary>
       public void ActiveAndShowTreeGroupWithCategory(Category groupCategory)
       {
          blockViewControl.ActiveAndShowTreeGroupWithCategory( groupCategory );
       }

        private void InitializeComponent( )
        {
            this.dBGridView = new DataGridView
                                  {
                                      Dock = DockStyle.Fill,
                                      SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                                      RowHeadersVisible = false,
                                      AllowUserToAddRows = false,
                                      AllowUserToDeleteRows = false,
                                      AllowUserToResizeRows = false,
                                      ReadOnly = true
                                  };
            this.dBGridView.Columns.Add( new DataGridViewTextBoxColumn { HeaderText = "Дата", Width = 190 } );
            this.dBGridView.Columns.Add( new DataGridViewTextBoxColumn { HeaderText = "Комментарий", Width = 100 } );
            this.dBGridView.DoubleClick += this.DbGridViewOnDoubleClick;

            this.selectControl = new SelectControl( );
            this.selectControl.btnUpdate.Click += BtnUpdateOnClick;

            this.readWriteUstavkyControl = new ReadWriteUstavkyControl( );
            this.readWriteUstavkyControl.btnReadUstFC.Click += BtnReadUstFcOnClick;
            this.readWriteUstavkyControl.btnWriteUst.Click += BtnWriteUstOnClick;
            this.readWriteUstavkyControl.btnResetValues.Click += BtnResetValuesOnClick;
            this.readWriteUstavkyControl.btnFix4Change.CheckedChanged += BtnFix4ChangeOnCheckedChanged;

            this.buttonLayoutPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, Width = 800, Height = 150 };
            var buttons = CreateCommandButtons( (int)uniDev, this.parent );
            if ( buttons != null ) this.buttonLayoutPanel.Controls.AddRange( buttons );

            this.storageDeviceControl = new StorageDeviceControl( );
            this.storageDeviceControl.btnStorageRead.Click += BtnStorageReadClick;
            this.storageDeviceControl.btnStorageReset.Click += BtnStorageResetClick;

            this.maxMeterControl = new MaxMeterControl( );
            this.maxMeterControl.btnMaxmeterRead.Click += BtnMaxmeterReadClick;
            this.maxMeterControl.btnMaxmeterReset.Click += BtnMaxmeterResetClick;

            this.blockViewControl = new BlockViewControl( this.uniDs, this.uniDev ) { Dock = DockStyle.Fill };
            this.blockViewControl.CategoryEvent += ActivateSelectComponents;

            this.tableLayoutPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 1 };
            this.tableLayoutPanel.ColumnStyles.Add( new ColumnStyle( SizeType.Percent, 100f ) );
            this.tableLayoutPanel.RowStyles.Add( new RowStyle( SizeType.Percent, 100f ) );
            this.tableLayoutPanel.Controls.Add( this.blockViewControl );
            Controls.Add( this.tableLayoutPanel );

            this.botomFlowLayoutPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true };
            this.botomFlowLayoutPanel.Controls.Add( this.selectControl );
            this.botomFlowLayoutPanel.Controls.Add( this.readWriteUstavkyControl );
            this.botomFlowLayoutPanel.Controls.Add( this.buttonLayoutPanel );
            this.botomFlowLayoutPanel.Controls.Add( this.storageDeviceControl );
            this.botomFlowLayoutPanel.Controls.Add( this.maxMeterControl );

            this.parent.FormClosing += ( sender, args ) => blockViewControl.UnSubscribe( );
        }
        /// <summary>
        /// Выбор архивной записи
        /// </summary>
        private void DbGridViewOnDoubleClick( object sender, EventArgs eventArgs )
        {
            if ( this.dBGridView.SelectedRows.Count == 0 ) return;

            try
            {
                var idBlock = (int)this.dBGridView.SelectedRows[0].Tag;
                var arparam = new System.Collections.ArrayList
                                  {
                                      idBlock,
                                      System.Text.Encoding.UTF8.GetBytes( HMI_Settings.ProviderPtkSql )
                                  };

                switch ( this.currentCategory )
                {
                    case Category.Crush:
                        HMI_Settings.CONFIGURATION.GetData( 0, this.uniDev, "ArhivAvariBlockData", arparam, idBlock );
                        ActiveAndShowTreeGroupWithCategory(Category.Crush);
                        break;
                    case Category.Ustavki:
                        HMI_Settings.CONFIGURATION.GetData( 0, this.uniDev, "ArhivUstavkiBlockData", arparam, idBlock );
                        ActiveAndShowTreeGroupWithCategory(Category.Ustavki);
                        break;
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Активация компонентов
        /// </summary>
        /// <param name="category">Категория группы</param>
        private void ActivateSelectComponents( Category category )
        {
            this.currentCategory = category;

            switch ( category )
            {
                case Category.Crush:
                    {
                        if ( this.tableLayoutPanel.ColumnCount == 1 )
                        {
                            this.tableLayoutPanel.ColumnCount = 2;
                            this.tableLayoutPanel.ColumnStyles.Add( new ColumnStyle( SizeType.Absolute, 200 ) );
                            this.tableLayoutPanel.Controls.Add( this.dBGridView, 2, 0 );
                        }
                        if ( this.tableLayoutPanel.RowCount == 1 )
                        {
                            this.tableLayoutPanel.RowCount = 2;
                            this.tableLayoutPanel.RowStyles.Add( new RowStyle( SizeType.Absolute, 70 ) );
                            this.tableLayoutPanel.Controls.Add( this.botomFlowLayoutPanel, 0, 1 );
                        }

                        //this.dBGridViewRows.Clear( );
                        this.selectControl.Visible = true;
                        this.readWriteUstavkyControl.Visible = false;
                        this.buttonLayoutPanel.Visible = false;
                        this.maxMeterControl.Visible = false;
                        this.storageDeviceControl.Visible = false;
                    }
                    break;
                case Category.Ustavki:
                    {
                        if ( this.tableLayoutPanel.ColumnCount == 1 )
                        {
                            this.tableLayoutPanel.ColumnCount = 2;
                            this.tableLayoutPanel.ColumnStyles.Add( new ColumnStyle( SizeType.Absolute, 200 ) );
                            this.tableLayoutPanel.Controls.Add( this.dBGridView, 2, 0 );
                        }
                        if ( this.tableLayoutPanel.RowCount == 1 )
                        {
                            this.tableLayoutPanel.RowCount = 2;
                            this.tableLayoutPanel.RowStyles.Add( new RowStyle( SizeType.Absolute, 70 ) );
                            this.tableLayoutPanel.Controls.Add( this.botomFlowLayoutPanel, 0, 1 );
                        }

                        //this.dBGridView.Rows.Clear( );
                        this.selectControl.Visible = true;
                        this.readWriteUstavkyControl.Visible = true;
                        this.buttonLayoutPanel.Visible = false;
                        this.maxMeterControl.Visible = false;
                        this.storageDeviceControl.Visible = false;
                    }
                    break;
                case Category.Buttons:
                    {
                        if ( this.tableLayoutPanel.ColumnCount == 2 )
                        {
                            this.tableLayoutPanel.ColumnCount = 1;
                            this.tableLayoutPanel.ColumnStyles.RemoveAt( 1 );
                            this.tableLayoutPanel.Controls.Remove( this.dBGridView );
                        }
                        if ( this.tableLayoutPanel.RowCount == 1 )
                        {
                            this.tableLayoutPanel.RowCount = 2;
                            this.tableLayoutPanel.RowStyles.Add( new RowStyle( SizeType.Absolute, 70 ) );
                            this.tableLayoutPanel.Controls.Add( this.botomFlowLayoutPanel, 0, 1 );
                        }

                        this.selectControl.Visible = false;
                        this.readWriteUstavkyControl.Visible = false;
                        this.buttonLayoutPanel.Visible = true;
                        this.maxMeterControl.Visible = false;
                        this.storageDeviceControl.Visible = false;
                    }
                    break;
                case Category.StorageDevice:
                    {
                        if ( this.tableLayoutPanel.ColumnCount == 2 )
                        {
                            this.tableLayoutPanel.ColumnCount = 1;
                            this.tableLayoutPanel.ColumnStyles.RemoveAt( 1 );
                            this.tableLayoutPanel.Controls.Remove( this.dBGridView );
                        }
                        if ( this.tableLayoutPanel.RowCount == 1 )
                        {
                            this.tableLayoutPanel.RowCount = 2;
                            this.tableLayoutPanel.RowStyles.Add( new RowStyle( SizeType.Absolute, 70 ) );
                            this.tableLayoutPanel.Controls.Add( this.botomFlowLayoutPanel, 0, 1 );
                        }

                        this.selectControl.Visible = false;
                        this.readWriteUstavkyControl.Visible = false;
                        this.buttonLayoutPanel.Visible = false;
                        this.maxMeterControl.Visible = false;
                        this.storageDeviceControl.Visible = true;
                    }
                    break;
                case Category.MaxMeter:
                    {
                        if ( this.tableLayoutPanel.ColumnCount == 2 )
                        {
                            this.tableLayoutPanel.ColumnCount = 1;
                            this.tableLayoutPanel.ColumnStyles.RemoveAt( 1 );
                            this.tableLayoutPanel.Controls.Remove( this.dBGridView );
                        }
                        if ( this.tableLayoutPanel.RowCount == 1 )
                        {
                            this.tableLayoutPanel.RowCount = 2;
                            this.tableLayoutPanel.RowStyles.Add( new RowStyle( SizeType.Absolute, 70 ) );
                            this.tableLayoutPanel.Controls.Add( this.botomFlowLayoutPanel, 0, 1 );
                        }

                        this.selectControl.Visible = false;
                        this.readWriteUstavkyControl.Visible = false;
                        this.buttonLayoutPanel.Visible = false;
                        this.maxMeterControl.Visible = true;
                        this.storageDeviceControl.Visible = false;
                    }
                    break;
                default:
                    {
                        if ( this.tableLayoutPanel.ColumnCount == 2 )
                        {
                            this.tableLayoutPanel.ColumnCount = 1;
                            this.tableLayoutPanel.ColumnStyles.RemoveAt( 1 );

                            this.tableLayoutPanel.Controls.Remove( this.dBGridView );
                        }
                        if ( this.tableLayoutPanel.RowCount == 2 )
                        {
                            this.tableLayoutPanel.RowCount = 1;
                            this.tableLayoutPanel.RowStyles.RemoveAt( 1 );

                            this.tableLayoutPanel.Controls.Remove( this.botomFlowLayoutPanel );
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Обновить таблицу из БД
        /// </summary>
        private void BtnUpdateOnClick( object sender, EventArgs eventArgs )
        {
            var typeBlock = TypeBlockData4Req.TypeBlockData4Req_Unknown;
            switch ( this.currentCategory )
            {
                case Category.Crush: typeBlock = TypeBlockData4Req.TypeBlockData4Req_Srabat; break;
                case Category.Ustavki: typeBlock = TypeBlockData4Req.TypeBlockData4Req_Ustavki; break;
            }

            var list = BlockViewCollector.AvarUstavkyDataBase( this.uniDev, this.selectControl.StartDateCollapsed,
                                                               this.selectControl.EndTimeCollapsed, typeBlock );
            if ( list == null )
            {
                MessageBox.Show( "Ошибка работы с базой данных.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if ( list.Count == 0 )
                MessageBox.Show( "Архивных данных для этого устройства нет.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information );
            else
            {
                this.dBGridView.Rows.Clear( );
                foreach ( var row in list )
                    this.dBGridView.Rows.Add( row );
            }
        }
        /// <summary>
        /// Чтение уставок
        /// </summary>
        private void BtnReadUstFcOnClick( object sender, EventArgs eventArgs )
        {
            // правильная запись в журнал действий пользователя номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 7, this.uniDev.ToString( CultureInfo.InvariantCulture ), true );
            // выдача команда IMP
            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, this.uniDev, "IMP", new byte[] { }, parent );

            this.BtnResetValuesOnClick( sender, eventArgs );
            this.blockViewControl.Subscribes( );
        }
        /// <summary>
        /// Запись уставок
        /// </summary>
        private void BtnWriteUstOnClick( object sender, EventArgs eventArgs )
        {
            try
            {
                if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, HMI_Settings.UserRight ) )
                    return;

                if ( !HMI_Settings.isRegPass || !CommonUtils.CommonUtils.CanAction( ) ) return;

                if ( MessageBox.Show( "Записать уставки?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.No )
                    return;

                var list = this.blockViewControl.GetChangedTags( );

                if ( list.Count == 0 )
                {
                    MessageBox.Show( "Уставки не изменялись. \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information );
                    return;
                }

                // правильная запись в журнал действий пользователя номер устройства с учетом фк
                CommonUtils.CommonUtils.WriteEventToLog( 6, uniDev.ToString( CultureInfo.InvariantCulture ), true );
                // выдача команда WCP
                HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "WCP",
                                                           CommonUtils.CommonUtils.GetUstConfMemXPacket( list ), parent );
                this.blockViewControl.ResetStatusModify( );
                this.readWriteUstavkyControl.ResetButtons( );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Очистить поля формы
        /// </summary>
        private void BtnResetValuesOnClick( object sender, EventArgs eventArgs ) { this.blockViewControl.UnSubscribeAndClear( ); }
        /// <summary>
        /// Проверка режима задания уставок
        /// </summary>
        private void BtnFix4ChangeOnCheckedChanged( object sender, EventArgs eventArgs )
        {
            var chBox = (CheckBox)sender;
            this.blockViewControl.ReadWriteChecked( chBox.Checked );
        }
        /// <summary>
        /// Накопитель (чтение)
        /// </summary>
        private void BtnStorageReadClick(object sender, EventArgs e)
        {
            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 8, uniDev.ToString( CultureInfo.InvariantCulture ), true );
            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "IMC", new byte[] { }, parent );
        }
        /// <summary>
        /// Накопитель (сброс)
        /// </summary>
        private void BtnStorageResetClick(object sender, EventArgs e)
        {
            if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b06_Reset_info, HMI_Settings.UserRight ) ) return;

            var dr = MessageBox.Show( "Сбросить накопительную информацию блока?",
                                      "Предупреждение",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question );
            if ( dr != DialogResult.Yes ) return;

            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 35, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "CCD", new byte[] { }, parent );
        }
        /// <summary>
        /// Максметр (чтение)
        /// </summary>
        private void BtnMaxmeterReadClick(object sender, EventArgs e)
        {
            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк            
            CommonUtils.CommonUtils.WriteEventToLog( 10, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "IMD", new byte[] { }, parent );
        }
        /// <summary>
        /// Максметр (сброс)
        /// </summary>
        private void BtnMaxmeterResetClick(object sender, EventArgs e)
        {
            if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b06_Reset_info,
                                                          HMI_Settings.UserRight ) ) return;

            var dr = MessageBox.Show( "Сбросить максметр?",
                                      "Предупреждение",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question );
            if ( dr != DialogResult.Yes ) return;

            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 35, uniDev.ToString( CultureInfo.InvariantCulture ), true );

            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "CMD", new byte[] { }, parent );
        }
        /// <summary>
        /// Печать
        /// </summary>
        public void Print( ) { blockViewControl.Print( ); }

        /// <summary>
        /// Создание кнопок комманд для элемента отображения
        /// </summary>
        private static Control[] CreateCommandButtons( int uniDev, Form control )
        {
            var xeDescDev = HMI_Settings.CONFIGURATION.GetDeviceXMLDescription( 0, "MOA_ECU", uniDev );
            if ( xeDescDev == null ) return null;

            var menu = xeDescDev.Element( "DescDev" );
            if ( menu == null ) return null;

            menu = menu.Element( "CommandMenu" );
            if ( menu == null ) return null;

            var xItems = menu.Elements( "MenuItem" );

            var list = new List<Control>( );
            foreach ( var item in xItems )
            {
                var content = HelperLibrary.ContentHelper.CreateCommandMenuContent( item );
                if ( content == null ) continue;

                var button = new Button { Width = 160, Text = content.Context, Tag = content };
                button.Click += delegate( object sender, EventArgs args )
                {
                    if ( CommonUtils.CommonUtils.IsUserActionBan(
                         CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, HMI_Settings.UserRight ) ||
                         ( HMI_Settings.isRegPass && !CommonUtils.CommonUtils.CanAction( ) ) ) return;

                    var dlg = MessageBox.Show( "Выполнить команду?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
                    if ( dlg != DialogResult.Yes ) return;

                    var btn = (Button)sender;
                    var obj = (HelperLibrary.CommandContent<byte[]>)btn.Tag;

                    // выполняем действия по включению выключателя вначале определим устройство
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Critical, 618, string.Format( "Поступила команда для устройства: {0}", uniDev ) );

                    try
                    {
                        CommonUtils.CommonUtils.WriteEventToLog( (int)obj.Code, uniDev.ToString( CultureInfo.InvariantCulture ), true );
                        HMI_Settings.CONFIGURATION.ExecuteCommand( 0, (uint)uniDev, obj.Command, obj.Parameter, control );
                    }
                    catch
                    {
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Error, uniDev, "Ошибка в группе данных для команды контекстного меню" );
                        MessageBox.Show( "Выполнение команды прервано", "Ошибка описания команды", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    }
                };

                list.Add( button );
            }

            return list.ToArray( );
        }
    }
}
