using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using InterfaceLibrary;
using HMI_MT_Settings;
using System.IO;
using DeviceFormLib.BlockTabs;

namespace DeviceFormLib
{
    public partial class FormOpcDevice : Form, IDeviceForm
    {
        private TabPage tpCurrent;
        //private readonly frmEngine frmengine;
        private readonly FrmEngineNew engineNew;
        private readonly UInt32 uniDev = 0xffffffff;
        private bool isDiscretButtons;

        public FormOpcDevice( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent();
            try
            {
                uniDev = unidev;
                //frmengine = new frmEngine( unids, unidev, this );
                engineNew = new FrmEngineNew( unids, unidev, this );
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
                //frmengine.InitFrm( this, tabControl, null );
                engineNew.InitFrm( this, tabControl );
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
            tpCurrentInfo.Enter += TpCurrentInfoEnter;
        }
        public void ActivateTabPage( string typetabpage ) { }
        public void reqAvar_OnReqExecuted( IRequestData req ) { }

        private void TpCurrentInfoEnter( object sender, EventArgs e )
        {
            if ( !isDiscretButtons )
            {
                isDiscretButtons = true;
                foreach ( var iHmiTagAccess in mtraNamedFLPanel2.Controls.Cast<IHMITagAccess>() )
                    if ( iHmiTagAccess.LinkedTag.AccessToValue == "ReadWrite" &&
                         iHmiTagAccess.LinkedTag.Type == "Discret" )
                    {
                        var button1 = new Button
                            {
                                Text = "Установить",
                                Width = 75,
                                Height = 23,
                                Left = 20,
                                Top = 20,
                                Tag = iHmiTagAccess
                            };
                        var button2 = new Button
                            {
                                Text = "Сбросить",
                                Width = 75,
                                Height = 23,
                                Left = 20 + button1.Width + 5,
                                Top = 20,
                                Tag = iHmiTagAccess
                            };
                        button1.Click += Button1OnClick;
                        button2.Click += Button2OnClick;

                        var gBox = new GroupBox
                            {
                                Text = string.Format( "Управление: {0}", iHmiTagAccess.VisibleText ),
                                Height = button1.Top + button1.Height + 5
                            };
                        gBox.Controls.Add( button1 );
                        gBox.Controls.Add( button2 );
                        mtraNamedFLPanel3.Controls.Add( gBox );
                    }
            }

            tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpCurrentInfo );
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
        /// Событие для выставления значения
        /// </summary>
        private void Button1OnClick( object sender, EventArgs eventArgs )
        {
            try
            {
                var dr = MessageBox.Show( "Установить?", "Предупреждение", MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question );
                if ( dr == DialogResult.No )
                    return;

                // правильная запись в журнал действий пользователя номер устройства с учетом фк
                //CommonUtils.CommonUtils.WriteEventToLog( ???, uniDev.ToString( CultureInfo.InvariantCulture ), true );//"выдана команда WCP - запись уставок."

                var iHmiTagAccess = (IHMITagAccess)( (Button)sender ).Tag;
                HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "WValue",
                                                           CreatePackage( iHmiTagAccess.LinkedTag, 1 ), this );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Событие для сброса значения
        /// </summary>
        private void Button2OnClick( object sender, EventArgs eventArgs )
        {
            try
            {
                var dr = MessageBox.Show( "Сбросить?", "Предупреждение", MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question );
                if ( dr == DialogResult.No )
                    return;

                // правильная запись в журнал действий пользователя номер устройства с учетом фк
                //CommonUtils.CommonUtils.WriteEventToLog( ???, uniDev.ToString( CultureInfo.InvariantCulture ), true );//"выдана команда WCP - запись уставок."

                var iHmiTagAccess = (IHMITagAccess)( (Button)sender ).Tag;
                HMI_Settings.CONFIGURATION.ExecuteCommand( 0, uniDev, "WValue",
                                                           CreatePackage( iHmiTagAccess.LinkedTag, 0 ), this );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Формирование пакета для отправки
        /// </summary>
        /// <param name="tag">Тэг</param>
        /// <param name="value">значение для тэга</param>
        /// <returns>Байтовый массив для отправки</returns>
        private static byte[] CreatePackage( ITag tag, uint value )
        {
            try
            {
                byte[] body;
                using ( var stream = new MemoryStream() )
                    using ( var binary = new BinaryWriter( stream ) )
                    {
                        binary.Write( (int)tag.TagGUID ); // 4 байта - TagGUID
                        binary.Write( (byte)0 ); // 1 байт - 0(raw), 1(primary), 2(secondary)
                        binary.Write( (UInt16)1 ); // 2 байта - длинна значения тэга
                        binary.Write( (byte)value ); // значение для тэга
                        body = stream.ToArray();
                    }

                using ( var stream = new MemoryStream() )
                    using ( var binary = new BinaryWriter( stream ) )
                    {
                        binary.Write( (UInt16)tag.Device.UniDS_GUID ); // 2 байта уник номер DS
                        binary.Write( (int)tag.Device.UniObjectGUID ); // 4 байта уник номер устройства

                        var bt = Encoding.UTF8.GetBytes( "WValue" );
                        binary.Write( (ushort)bt.Length );
                        binary.Write( bt );
                        binary.Write( (ushort)body.Length );
                        binary.Write( body );
                        body = stream.ToArray();
                    }

                using ( var stream = new MemoryStream() )
                    using ( var binary = new BinaryWriter( stream ) )
                    {
                        // тип пакета 4 - команда записи значений в OPC
                        binary.Write( new byte[] { 0x04 }, 0, 1 );
                        // идентификатор корреляции
                        binary.Write( new byte[] { 0x00, 0x00 }, 0, 2 );

                        binary.Write( Convert.ToUInt16( body.Length ) );
                        binary.Write( body, 0, body.Length );
                        body = stream.ToArray();
                    }

                using ( var stream = new MemoryStream() )
                    using ( var binary = new BinaryWriter( stream ) )
                    {
                        binary.Write( new byte[] { 0xfd, 0x7f }, 0, 2 );
                        // запишем 4 байта резерва для общей длины доп параметров команды
                        binary.Write( Convert.ToUInt32( body.Length ) );
                        binary.Write( body, 0, body.Length );
                        return stream.ToArray();
                    }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                throw;
            }
        }
        /// <summary>
        /// Идентификатор блока
        /// </summary>
        public uint Guid { get { return uniDev; } }
    }
}
