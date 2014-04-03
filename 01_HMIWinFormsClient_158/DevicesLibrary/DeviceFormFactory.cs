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
            try
            {
                foreach (IDeviceForm f in arrFrm)
                {
                    if (f.Guid == devGuid)
                    {
                        ((Form) f).Focus();
                        return f;
                    }
                }

                var device = HMI_Settings.CONFIGURATION.GetLink2Device(dsGuid, devGuid);

                if (device == null)
                {
                    //MessageBox.Show(string.Format("Элемент не привязан к устройству (GUID = {0}) в текущей конфигурации.", devGuid), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    throw new Exception( string.Format( "{0} : DeviceFormFactory : {1}", DateTime.Now, "Элемент не привязан к устройству в текущей конфигурации" ) );
                }

                string pathToDeviceFile = Path.GetDirectoryName(HMI_Settings.PathToConfigurationFile) + "\\Configuration\\0#DataServer\\Devices\\" + device.UniObjectGUID + '@' + device.TypeName + ".cfg";
                if (!File.Exists(pathToDeviceFile))
                {
                    //MessageBox.Show(string.Format("Файл описания формы не существует ({0})", FrmTagsDescript), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new Exception(
                        string.Format( "{0} : DeviceFormFactory : {1}", DateTime.Now, "Элемент не привязан к устройству в текущей конфигурации" ) );
                }

                DebugStatistics.WindowStatistics.AddStatistic( "Создание и инициализация формы." );

                IDeviceForm deviceform = new FormBlock(device.TypeName, dsGuid, devGuid);
                
                DebugStatistics.WindowStatistics.AddStatistic( "Создание и инициализация формы завершено." );

                if (HMI_Settings.IsDebugMode)
                    (deviceform as Form).Text = String.Format("({0}) {1} ({2})", devGuid, device.Description, device.TypeName);
                else
                    (deviceform as Form).Text = CommonUtils.CommonUtils.GetDispCaptionForDevice((int)devGuid);

                var isconnectState = false;
                var connectState = PTKState.Iinstance.GetValueAsString(dsGuid, devGuid, "Связь");

                if (devGuid == 25855) connectState = "true";

                if ( connectState == string.Empty || bool.TryParse( connectState, out isconnectState ) )
                {
                    if ( !isconnectState )
                        MessageBox.Show( "Терминал недоступен или с ним нет связи", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning );
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
