using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Xml.Linq;

using InterfaceLibrary;
using HMI_MT_Settings;


namespace DeviceFormLib
{
    public partial class FormUsoMtr : Form, IDeviceForm
    {
        TabPage tpCurrent;
        //readonly frmEngine frmengine;
        readonly FrmEngineNew engineNew;
        readonly UInt32 uniDev = 0xffffffff;
        Boolean isCurrentInfoEnter;

        public FormUsoMtr( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent( );

            try
            {
                uniDev = unidev;
                //frmengine = new frmEngine( unids, unidev, this );
                engineNew = new FrmEngineNew( unids, unidev, this );

                /*tabControl.Controls.Add( new EventBlockTabPage( unidev ) );
                tabControl.Controls.Add( new OscDiagTabPage( unidev, OscDiagTabPage.OscDiagPanelVisible.Oscilograms ) );
                tabControl.Controls.Add( new InformationTabPage( unidev ) );
                tabControl.Controls.Add( new DataBaseFilesLibrary.TabPageDBFile( uniDev ) );*/

                var buttons = CreateCommandButtons( (int)uniDev, this );
                if ( buttons != null ) flowLayoutPanel1.Controls.AddRange( buttons );
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
        }

        public void CreateDeviceForm() { }
        public void InitInterfaceElementsClick()
        {
            tabControl.Selected += TabControlSelected;
            tpCurrentInfo.Enter += TpCurrentInfoEnter;
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
                //frmengine.PlaceTagsOnTPFlps( "Контроль" );
                engineNew.PlaceTagsOnPanels( "Контроль" );
                isCurrentInfoEnter = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpCurrentInfo );
        }
        /// <summary>
        /// Идентификатор блока
        /// </summary>
        public uint Guid { get { return uniDev; } }

        /// <summary>
        /// Создание кнопок комманд для элемента отображения
        /// </summary>
        internal static Control[] CreateCommandButtons(int uniDev, Form control)
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
                                        if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, HMI_Settings.UserRight ) ||
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

                                            /*ICommand cmd = */
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
