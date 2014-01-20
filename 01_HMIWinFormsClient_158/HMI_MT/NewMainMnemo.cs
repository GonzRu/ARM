using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

using HelperLibrary;
using WindowsForms;
using LibraryElements;
using Calculator;
using HMI_MT_Settings;
using InterfaceLibrary;
using DebugStatisticLibrary;

namespace HMI_MT
{
    public partial class NewMainMnemo : Form, IResetStateProtocol
    {
        readonly String filePath;
        readonly MainForm parent;
        readonly Simulation simulation = new Simulation( );
        List<ITag> tags;
        IBasePanelCollection iPanel;

        private NewMainMnemo( String path )
        {
            InitializeComponent();

            SetStyle( ControlStyles.UserPaint, true );
            SetStyle( ControlStyles.AllPaintingInWmPaint, true );
            SetStyle( ControlStyles.DoubleBuffer, true );

            filePath = path;
        }
        public NewMainMnemo( String path, MainForm linkMainForm, bool owner )
            : this( path )
        {
            parent = linkMainForm;
            parent.Resize += ParentResize;
            OwnerSchema = owner;
        }
        private void ParentResize( object sender, EventArgs e )
        {
            NormalModeLibrary.ComponentFactory.Factory.SetStates( parent.WindowState );
        }
        private void PanelClick( object sender, EventArgs e )
        {
            try
            {
                var idp = sender as IDynamicParameters;

                if ( idp != null && idp.Parameters != null )
                    DevicesLibrary.DeviceFormFactory.CreateForm( this, (int)idp.Parameters.DeviceGuid, parent.arrFrm );

                var buttonRegion = sender as ButtonRegion;
                if ( buttonRegion != null )
                {
                    var file = AppDomain.CurrentDomain.BaseDirectory + @"Project\MnemoSchemas\" + buttonRegion.Group;
                    var formEz = new NewMainMnemo( file, parent, true ) { MdiParent = parent };
                    formEz.Show();
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        private void NewMainMnemoLoad( object sender, EventArgs e )
        {
            // установить текущий каталог
            Directory.SetCurrentDirectory( AppDomain.CurrentDomain.BaseDirectory ); //??

            DebugStatistics.WindowStatistics.AddStatistic( "Запуск мнемосхемы." );
            DebugStatistics.WindowStatistics.AddStatistic( "Чтение файла и построение схемы." );
            iPanel = new SchemaPanel( filePath, HMI_Settings.SchemaSize );
            if ( iPanel.ErrorLoading )
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Ошибка. Чтение файла и построение схемы не завершено." );
                return;
            }
            DebugStatistics.WindowStatistics.AddStatistic( "Чтение файла и построение схемы завершено." );

            DebugStatistics.WindowStatistics.AddStatistic( "Запуск инициализации привязки мнемосхемы." );
            Text = iPanel.CaptionOfSchema;
            ClientSize = iPanel.ClientSize;
            BackColor = iPanel.BackColor = Element.BackGroundColor;
            iPanel.Parent = this;
            iPanel.PanelClick += PanelClick;
            Controls.Add( (Control)iPanel );

            try
            {
                simulation.Parse( iPanel.CalculationElements );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }

            tags = BindingLincks( iPanel.CalculationElements );
            HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags( tags ); // подписываемся на обновление тегов

            BindingContextMenu( iPanel.CalculationElements, this );

            DebugStatistics.WindowStatistics.AddStatistic( "Инициализация привязки мнемосхемы завершена." );
        }
        private void NewMainMnemoShown( object sender, EventArgs e )
        {
            if ( iPanel != null && !iPanel.ErrorLoading )
            {
                /********************************************************************************************************/
                DebugStatistics.WindowStatistics.AddStatistic( "Запуск панелей нормального режима." );
                NormalModeLibrary.ComponentFactory.Factory.ActivatedMainMnemoForms( this );
                DebugStatistics.WindowStatistics.AddStatistic( "Запуск панелей нормального режима завершен." );
                /********************************************************************************************************/
            }
        }
        private void NewMainMnemoFormClosing( object sender, FormClosingEventArgs e )
        {
            if ( e.CloseReason == CloseReason.MdiFormClosing )
            {
                e.Cancel = true;
                return;
            }

            // удаляем ссылки на теги - отписываемся от тегов
            HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags( tags );
            parent.Resize -= ParentResize;

            /********************************************************************************************************/
            NormalModeLibrary.ComponentFactory.Factory.DeactivatedMainMnemoForms();
            /********************************************************************************************************/
        }
        public void ResetProtocol( )
        {
            if ( !iPanel.ErrorLoading )
                CalculationRegion.ResetStatusProtocol( iPanel.CalculationElements );
        }

        public Boolean OwnerSchema { get; private set; }

        private static ToolStripMenuItem CreateNornalModeItem( )
        {
            var item = new ToolStripMenuItem( "Параметры текущего режима" )
                           { Tag = "NML" /* NormalModeLibrary - для того, чтоб не отключать меню при показе */ };
            item.Click += ( sender, args ) =>
            {
                //if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, HMI_Settings.UserRight ) )
                //    return;

                var tsi = (ToolStripItem)sender;
                var ibpc = ( (ContextMenuStrip)tsi.Owner ).SourceControl as IBasePanel;

                if ( ibpc == null ) return;

                var idp = ibpc.Core as IDynamicParameters;
                if ( idp != null && idp.Parameters != null )
                {
                    var idevice = HMI_Settings.CONFIGURATION.GetLink2Device( 0, idp.Parameters.DeviceGuid );
                    NormalModeLibrary.ComponentFactory.EditSignals( idevice, HMI_Settings.UserName, NormalModeLibrary.Places.MainMnemo );
                }
            };

            return item;
        }
        /// <summary>
        /// Создание привязок
        /// </summary>
        /// <param name="baseRegions">Обертки графических элементов</param>
        internal static List<ITag> BindingLincks( IEnumerable<BaseRegion> baseRegions )
        {
            DebugStatistics.WindowStatistics.AddStatistic( "Запуск привязки графических элементов." );
            // список тегов для привязки
            var taglist = new List<ITag>( );
            try
            {
                foreach ( var region in baseRegions )
                {
                    FormulaEvalNds ev;

                    // сопоставление данных
                    var idp = region as IDynamicParameters;
                    if ( idp != null && idp.Parameters != null )
                        idp.AdjustmentTags( );

                    // привязка всех тегов расчетной конфигурации устройства
                    var cr = region as CalculationRegion;
                    if ( cr == null || cr.CalculationContext == null || cr.IsDemonstration )
                        continue;

                    foreach ( LibraryElements.Sources.SignalMatchRecord link in cr.CalculationContext.Context.GetTags( ) )
                    {
                        ev = new FormulaEvalNds( HMI_Settings.CONFIGURATION,
                                                 string.Format( "0({0})", link.Result ), "", "" );

                        if ( ev.LinkVariableNewDs != null )
                        {
                            ev.OnChangeValFormTI += cr.LinkSetText;
                            taglist.Add( ev.LinkVariableNewDs );
                        }
                    }
                    // привязка протокола состояния устройства
                    if ( idp != null && idp.Parameters != null )
                    {
                        ev = CommonUtils.CommonUtils.GetConnectionEvalNds( idp.Parameters.Type,
                                                                           idp.Parameters.DsGuid,
                                                                           idp.Parameters.DeviceGuid );
                        if ( ev != null && ev.LinkVariableNewDs != null )
                        {
                            ev.OnChangeValFormTI += cr.LinkSetTextStatusDev;
                            taglist.Add( ev.LinkVariableNewDs );
                        }
                    }
                }
                DebugStatistics.WindowStatistics.AddStatistic( "Запуск привязки графических элементов завершен." );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                DebugStatistics.WindowStatistics.AddStatistic( "Запуск привязки графических элементов завершен неудачно." );
            }

            return taglist;
        }
        /// <summary>
        /// Создание контекстного меню
        /// </summary>
        /// <param name="baseRegions">Обертки графических элементов</param>
        /// <param name="parent">Родительское окно</param>
        internal static void BindingContextMenu( IEnumerable<BaseRegion> baseRegions, Form parent )
        {
            DebugStatistics.WindowStatistics.AddStatistic( "Запуск привязки контекстного меню." );
            foreach ( var region in baseRegions )
            {
                var idp = region as IDynamicParameters;
                if ( idp != null && idp.Parameters != null )
                {
                    // извлекаем описание из PrgDevCFG.cdp
                    var xeDescDev = HMI_Settings.CONFIGURATION.GetDeviceXMLDescription( 0, "MOA_ECU", (int)idp.Parameters.DeviceGuid );

                    if ( xeDescDev == null ) continue;

                    CommonUtils.CommonUtils.CreateContextMenu( region, xeDescDev, parent );

                    /* добавление элементам пункта меню панелей нормального режима */
                    if ( region.MenuStrip != null )
                        region.MenuStrip.Items.Add( CreateNornalModeItem( ) );

                    try
                    {
                        // запоминаем подсказку элементу
                        region.ToolTipMessage = string.IsNullOrEmpty( idp.Parameters.ToolTipMessage )
                                                ? xeDescDev.Element( "DescDev" ).Element( "DescDev" ).Value
                                                : idp.Parameters.ToolTipMessage;
                    }
                    catch ( Exception exx )
                    {
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( exx );
                    }
                }
            }
            DebugStatistics.WindowStatistics.AddStatistic( "Запуск привязки контекстного меню завершен." );
        }
    }
}
