using System;
using System.Collections;
using System.Globalization;

using DebugStatisticLibrary;
using InterfaceLibrary;
using System.Windows.Forms;
using System.IO;
using HMI_MT_Settings;
using PTKStateLib;

namespace DevicesLibrary
{
    public static class DeviceFormFactory
    {
        public static IDeviceForm CreateForm(Form parent, uint dsGuid, uint devGuid, ArrayList arrFrm)
        {
            // извлекаем описание из PrgDevCFG.cdp
            try
            {
                var xeDescDev = HMI_Settings.CONFIGURATION.GetDeviceXMLDescription((int)dsGuid, "MOA_ECU", (int)devGuid);

                if (xeDescDev == null)
                {
                    MessageBox.Show(string.Format("Элемент не привязан к устройству (GUID = {0}) в текущей конфигурации.", devGuid), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    throw new Exception( string.Format( "{0} : DeviceFormFactory : {1}", DateTime.Now, "Элемент не привязан к устройству в текущей конфигурации" ) );
                }

                var objGuid = xeDescDev.Attribute("objectGUID").Value;

                xeDescDev = xeDescDev.Element( "DescDev" );

                var strTypeBlock = xeDescDev.Element( "nameELowLevel" ).Value;

                string FrmTagsDescript = Path.GetDirectoryName(HMI_Settings.PathToConfigurationFile) +
                    "\\Configuration\\0#DataServer\\Devices\\" + objGuid + '@' + strTypeBlock + ".cfg";

                if (!File.Exists(FrmTagsDescript))
                {
                    MessageBox.Show(string.Format("Файл описания формы не существует ({0})", FrmTagsDescript), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new Exception(
                        string.Format( "{0} : DeviceFormFactory : {1}", DateTime.Now, "Элемент не привязан к устройству в текущей конфигурации" ) );
                }

                //var folderDevDescript = string.Format("{0}\\Project\\{1}\\", AppDomain.CurrentDomain.BaseDirectory, xeDescDev.Element( "nameR" ).Value);

                DebugStatistics.WindowStatistics.AddStatistic( "Создание и инициализация формы." );
                
                IDeviceForm deviceform = new FormBlock( strTypeBlock, 0, uint.Parse( objGuid ) );
                
                DebugStatistics.WindowStatistics.AddStatistic( "Создание и инициализация формы завершено." );

                ( deviceform as Form ).Text = CommonUtils.CommonUtils.GetDispCaptionForDevice( (int)devGuid );

                var isconnectState = false;
                var connectState = PTKState.Iinstance.GetValueAsString(dsGuid, devGuid, "Связь");

                if (devGuid == 25855) connectState = "true";

                if ( connectState == string.Empty || bool.TryParse( connectState, out isconnectState ) )
                {
                    if ( !isconnectState )
                        MessageBox.Show( "Терминал недоступен или с ним нет связи", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                }

                foreach ( IDeviceForm f in arrFrm )
                {
                    if ( f.Guid == deviceform.Guid )
                    {
                        ( (Form)f ).Focus( );
                        ( (Form)deviceform ).Dispose( );
                        return f;
                    }
                }

                DebugStatistics.WindowStatistics.AddStatistic( "Запуск формы." );

                (deviceform as Form).MdiParent = HMI_Settings.Link2MainForm;
                (deviceform as Form).MaximumSize = parent.Size;
                (deviceform as Form).Dock = DockStyle.Fill;
                (deviceform as Form).WindowState = FormWindowState.Maximized;
                (deviceform as Form).Show();
                (deviceform as Form).Closing += ( sender, args ) => arrFrm.Remove( sender );
                arrFrm.Add((Form)deviceform);

                return deviceform;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(@"(45) : {0} : X:\Projects\01_HMIWinFormsClient\DeviceFormLib\DeviceFormFactory.cs : CreategForm() : ОШИБКА : {1}", DateTime.Now, ex.Message));
            }
        }
    }
}
