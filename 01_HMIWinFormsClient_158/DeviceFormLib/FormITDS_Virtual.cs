using System;
using System.Globalization;
using System.Windows.Forms;
using InterfaceLibrary;
using HMI_MT_Settings;
using System.Diagnostics;

namespace DeviceFormLib
{
    public partial class FormITDS_Virtual : Form, IDeviceForm
    {
        TabPage tpCurrent;
        //readonly frmEngine frmengine;
        readonly UInt32 uniDev = 0xffffffff;
        readonly FrmEngineNew engineNew;
        Boolean isInPut1, isInPut2, isInPut3, isInPut4, isInPut5, isInPut6, isOutPut1;

        public FormITDS_Virtual( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent();

            try
            {
                this.Guid = unidev;
                //frmengine = new frmEngine( unids, unidev, this );
                engineNew = new FrmEngineNew( unids, unidev, this );

                /*tabControl.Controls.Add( new EventBlockTabPage( unidev ) );
                tabControl.Controls.Add( new OscDiagTabPage( unidev, OscDiagTabPage.OscDiagPanelVisible.Oscilograms ) );
                tabControl.Controls.Add( new InformationTabPage( unidev ) );
                tabControl.Controls.Add( new DataBaseFilesLibrary.TabPageDBFile( uniDev ) );*/
                CreateCommandButtons();
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Создание кнопок комманд для элемента отображения
        /// </summary>
        private void CreateCommandButtons()
        {
            // извлекаем описание из PrgDevCFG.cdp
            var xeDescDev = HMI_Settings.CONFIGURATION.GetDeviceXMLDescription( 0, "MOA_ECU", (int)uniDev );
            if ( xeDescDev == null ) return;

            var menu = xeDescDev.Element( "DescDev" );
            if ( menu == null ) return;

            menu = menu.Element( "ContextMenu" );
            if ( menu == null ) return;

            var xItems = menu.Elements( "MenuItem" );
            foreach ( var item in xItems )
            {
                var xCommand = item.Attribute( "command" );
                if ( xCommand == null ) continue;
                var xContext = item.Attribute( "context" );
                if ( xContext == null ) continue;
                var xCommandCode = item.Attribute( "code" );
                if ( xCommandCode == null ) continue;

                var button = new Button
                {
                    Width = 160,
                    Text = xContext.Value,
                    Tag = new object[] { xCommand.Value, xCommandCode.Value }
                };
                button.Click += ( sender, args ) =>
                {
                    if ( CommonUtils.CommonUtils.IsUserActionBan(
                         CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, HMI_Settings.UserRight ) ||
                         ( HMI_Settings.isRegPass && !CommonUtils.CommonUtils.CanAction() ) )
                        return;

                    var dlg = MessageBox.Show( "Выполнить команду?", "Подтверждение",
                                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question );
                    if ( dlg != DialogResult.Yes ) return;

                    var btn = (Button)sender;
                    var obj = btn.Tag as object[];
                    if ( obj == null || obj.Length == 0 )
                    {
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(
                            TraceEventType.Error, (int)uniDev, "Ошибка в группе данных для команды" );
                        MessageBox.Show( "Выполнение команды прервано", "Ошибка описания команды",
                                         MessageBoxButtons.OK, MessageBoxIcon.Error );
                        return;
                    }

                    // выполняем действия по включению выключателя вначале определим устройство
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(
                        TraceEventType.Critical, 618, string.Format(
                            "Поступила команда для устройства: {0}", (int)uniDev ) );


                    try
                    {
                        var cmdStr = obj[0].ToString();
                        var cmdNum = int.Parse( obj[1].ToString() );

                        CommonUtils.CommonUtils.WriteEventToLog( cmdNum, uniDev.ToString( CultureInfo.InvariantCulture ), true );

                        /*ICommand cmd = */
                        HMI_Settings.CONFIGURATION.ExecuteCommand( 0, 25607/*uniDev*/, cmdStr, new byte[] { }, this );
                    }
                    catch
                    {
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(
                            TraceEventType.Error, (int)uniDev,
                            "Ошибка в группе данных для команды контекстного меню" );
                        MessageBox.Show( "Выполнение команды прервано", "Ошибка описания команды",
                                         MessageBoxButtons.OK, MessageBoxIcon.Error );
                    }
                };

                flowLayoutPanel1.Controls.Add( button );
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
            tpInPut1.Enter += TpInPut1Enter;
            tpInPut2.Enter += TpInPut2Enter;
            tpInPut3.Enter += TpInPut3Enter;
            tpInPut4.Enter += TpInPut4Enter;
            tpInPut5.Enter += TpInPut5Enter;
            tpInPut6.Enter += TpInPut6Enter;
            tpOutPut1.Enter += TpOutPut1Enter;
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
        private void TpInPut1Enter( object sender, EventArgs e )
        {
            //CommonUtils.CommonUtils.RemoveHMIUserControls(tpCurrentInfo);
            if ( !isInPut1 )
            {
                //frmengine.PlaceTagsOnTPFlps( "Входы 1" );
                engineNew.PlaceTagsOnPanels( "Входы 1" );
                isInPut1 = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpInPut1 );
        }
        private void TpInPut2Enter( object sender, EventArgs e )
        {
            //CommonUtils.CommonUtils.RemoveHMIUserControls(tpCurrentInfo);
            if ( !isInPut2 )
            {
                //frmengine.PlaceTagsOnTPFlps( "Входы 2" );
                engineNew.PlaceTagsOnPanels( "Входы 2" );
                isInPut2 = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpInPut2 );
        }
        private void TpInPut3Enter( object sender, EventArgs e )
        {
            //CommonUtils.CommonUtils.RemoveHMIUserControls(tpCurrentInfo);
            if ( !isInPut3 )
            {
                //frmengine.PlaceTagsOnTPFlps( "Входы 3" );
                engineNew.PlaceTagsOnPanels( "Входы 3" );
                isInPut3 = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpInPut3 );
        }
        private void TpInPut4Enter( object sender, EventArgs e )
        {
            //CommonUtils.CommonUtils.RemoveHMIUserControls(tpCurrentInfo);
            if ( !isInPut4 )
            {
                //frmengine.PlaceTagsOnTPFlps( "Входы 4" );
                engineNew.PlaceTagsOnPanels( "Входы 4" );
                isInPut4 = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpInPut4 );
        }
        private void TpInPut5Enter( object sender, EventArgs e )
        {
            //CommonUtils.CommonUtils.RemoveHMIUserControls(tpCurrentInfo);
            if ( !isInPut5 )
            {
                //frmengine.PlaceTagsOnTPFlps( "Входы 5" );
                engineNew.PlaceTagsOnPanels( "Входы 5" );
                isInPut5 = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpInPut5 );
        }
        private void TpInPut6Enter( object sender, EventArgs e )
        {
            //CommonUtils.CommonUtils.RemoveHMIUserControls(tpCurrentInfo);
            if ( !isInPut6 )
            {
                //frmengine.PlaceTagsOnTPFlps( "Входы 6" );
                engineNew.PlaceTagsOnPanels( "Входы 6" );
                isInPut6 = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpInPut6 );
        }
        private void TpOutPut1Enter( object sender, EventArgs e )
        {
            //CommonUtils.CommonUtils.RemoveHMIUserControls(tpCurrentInfo);
            if ( !isOutPut1 )
            {
                //frmengine.PlaceTagsOnTPFlps( "Выходы 1" );
                engineNew.PlaceTagsOnPanels( "Выходы 1" );
                isOutPut1 = true;
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpOutPut1 );
        }
        /// <summary>
        /// Идентификатор блока
        /// </summary>
        public uint Guid { get; private set; }
    }
}
