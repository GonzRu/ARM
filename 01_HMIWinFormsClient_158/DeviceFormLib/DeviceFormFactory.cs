using System;
using System.Collections;
using System.Globalization;

using DebugStatisticLibrary;

using InterfaceLibrary;
using System.Windows.Forms;
using System.IO;
using HMI_MT_Settings;
using PTKStateLib;

namespace DeviceFormLib
{
    public static class DeviceFormFactory
    {
        /*
         * Данная сборка не актуальна!!!!!
         * Нужно все построения внешних видов блоков перенаправить на сборку DevicesLibrary
         * Библиотека подготовлена и она использует HelperControlsLibrary, как и данная
         */
        private static IDeviceForm CreateDeviceForm( string typeBlock, uint dsGuid, uint devGuid )
        {
            if ( typeBlock.Contains( "Sirius" ) )
            {
                if ( typeBlock.Contains( "D" ) )
                {
                    if (typeBlock.Contains( "_5A" )) return new FormSiriusD( dsGuid, devGuid );
                }

                if ( typeBlock.Contains( "CS" ) ) return new FormSiriusCS( dsGuid, devGuid );
                if ( typeBlock.Contains( "T3" ) ) return new FormSiriusT3( dsGuid, devGuid );
                if ( typeBlock.Contains( "UV" ) ) return new FormSiriusUV( dsGuid, devGuid );
                if ( typeBlock.Contains( "TN" ) ) return new FormSiriusTN( dsGuid, devGuid );
                if ( typeBlock.Contains( "2_S" ) )
                {
                    if ( typeBlock.Contains( "_202" ) ) return new FormSirius2S202( dsGuid, devGuid );
                    if ( typeBlock.Contains( "_312" ) ) return new FormSirius2S312( dsGuid, devGuid );
                }
                if ( typeBlock.Contains( "2_V" ) ) return new FormSirius2V( dsGuid, devGuid );
                if ( typeBlock.Contains( "2_ML" ) ) return new FormSirius2ML( dsGuid, devGuid );
                if ( typeBlock.Contains( "2_L" ) ) return new FormSirius2L( dsGuid, devGuid );
                if ( typeBlock.Contains( "3_SV" ) ) /*return new FormSiriusBlock( dsGuid, devGuid );//*/return new FormSirius3SV( dsGuid, devGuid );
                if ( typeBlock.Contains( "3_LV" ) )
                {
                    if ( typeBlock.Contains( "_02" ) ) return new FormSirius3LV02( dsGuid, devGuid );
                    if ( typeBlock.Contains( "_03" ) ) return new FormSirius3LV03v2( dsGuid, devGuid );//return new FormSirius3LV03( dsGuid, devGuid );
                }
                if ( typeBlock.Contains( "3_DFZ" ) ) return new FormSirius3DFZ( dsGuid, devGuid );
                if ( typeBlock.Contains( "3_DZSH" ) ) return new FormSirius3DZSH( dsGuid, devGuid );
                if ( typeBlock.Contains( "IMF_1R" ) ) return new FormSiriusIMF3R( dsGuid, devGuid );
                if ( typeBlock.Contains( "IMF_3R" ) ) return new FormSiriusIMF3R( dsGuid, devGuid );
                if ( typeBlock.Contains( "RNM_1" ) ) return new FormSiriusRNM1( dsGuid, devGuid );
                if ( typeBlock.Contains( "2_RN" ) ) return new FormSirius2RN1( dsGuid, devGuid );
                if ( typeBlock.Contains( "OB" ) ) return new FormSirius2OB( dsGuid, devGuid );
            }

            if ( typeBlock.Contains( "BMRZ" ) )
            {
                if ( typeBlock.Contains( "BRCN" ) && typeBlock.Contains( "100" ) )
                    return new FormBmrzDevice( dsGuid, devGuid, typeBlock );
                if ( typeBlock.Contains( "100" ) )
                    return new FormBmrz100SeriesDevice( dsGuid, devGuid, typeBlock );
                return new FormBmrzDevice( dsGuid, devGuid, typeBlock );
            }

            if ( typeBlock.Contains( "BMCS" ) )
                return new FormBmcs( dsGuid, devGuid );

            if ( typeBlock.Contains( "PNZP_M" ) )
                return new FormPNZPM( dsGuid, devGuid );

            if ( typeBlock.Contains( "ITDS" ) )
            {
                if ( typeBlock.Contains( "VirtualDevice" ) ) return new FormITDS_Virtual( dsGuid, devGuid );
                if ( typeBlock.Contains( "DIN32C" ) ) return new FormUnknown( dsGuid, devGuid );
                if ( typeBlock.Contains( "DOUT8" ) ) return new FormUnknown( dsGuid, devGuid );
            }

            if ( typeBlock.Contains( "USO" ))
            {
                if ( typeBlock.Contains( "MTR" ) ) return new FormUsoMtr( dsGuid, devGuid );
                if ( typeBlock.Contains( "MTD" ) ) return new FormUsoMtd( dsGuid, devGuid );
            }

            if ( typeBlock.Contains( "NOVAR" ) )
            {
                if ( typeBlock.Contains( "206" ) ) return new FormUnknown( dsGuid, devGuid );
            }

            if ( typeBlock.Contains( "TM_1" ) )
                return new FormUnknown( dsGuid, devGuid );

            if ( typeBlock.Contains( "Orion" ) )
            {
                if ( typeBlock.Contains( "2_SP" ) ) return new FormOrion2SP( dsGuid, devGuid );
            }

            if ( typeBlock.Contains( "DRP" ) )
            {
                if ( typeBlock.Contains( "104_TN" ) ) return new FormDRP104TN( dsGuid, devGuid );
            }

            if ( typeBlock.Contains( "Ovod" ) )
            {
                if ( typeBlock.Contains( "_MD" ) ) return new FormOvodMd( dsGuid, devGuid );
            }

            if (typeBlock.Contains("Ekra")) return new FormEkra( dsGuid, devGuid );

            if (typeBlock.Contains("OPC")) return new FormOpcDevice( dsGuid, devGuid );

            if (typeBlock.Contains("Adam")) return new FormAdam( dsGuid, devGuid );

            if (typeBlock.Contains("Sank")) return new FormSank( dsGuid, devGuid );

            #region Old
            //if ( strTypeBlock.Contains( "OZZ" ) ) deviceform = new frmSirius_OZZ( unidev / 256, unidev % 256, folderDevDescript, FrmTagsDescript );
            //if ( strTypeBlock.Contains( "BMRZ" ) && strTypeBlock.Contains( "100" ) )
            //{
            //    deviceform = new Form4Device( 0, uint.Parse( objGuid ), strTypeBlock );
            //}
            //if ( strTypeBlock.Contains( "DUGA" ) )
            //{
            //    deviceform = new frmDuga( nFc, idDev, folderDevDescript, FrmTagsDescript );
            //}
            #endregion

            return new FormUnknown( dsGuid, devGuid );
        }
        public static IDeviceForm CreateForm(Form parent,int guid, PTKState ptkstate, ArrayList arrFrm)
        {
            // извлекаем описание из PrgDevCFG.cdp
            try
            {
                var xeDescDev = HMI_Settings.CONFIGURATION.GetDeviceXMLDescription(0, "MOA_ECU", guid);

                if (xeDescDev == null)
                {
                    MessageBox.Show( string.Format( "Элемент не привязан к устройству (GUID = {0}) в текущей конфигурации.", guid ), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                    throw new Exception( string.Format( "{0} : DeviceFormFactory : {1}", DateTime.Now, "Элемент не привязан к устройству в текущей конфигурации" ) );
                }

                var objGuid = xeDescDev.Attribute("objectGUID").Value;

                xeDescDev = xeDescDev.Element( "DescDev" );

                var strTypeBlock = xeDescDev.Element( "nameELowLevel" ).Value;

                string FrmTagsDescript = Path.GetDirectoryName(HMI_Settings.PathToConfigurationFile) +
                    "\\Configuration\\0#DataServer\\Devices\\" + objGuid + '@' + xeDescDev.Element("nameELowLevel").Value + ".cfg";

                if (!File.Exists(FrmTagsDescript))
                {
                    MessageBox.Show(string.Format("Файл описания формы не существует ({0})", FrmTagsDescript), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new Exception(
                        string.Format( "{0} : DeviceFormFactory : {1}", DateTime.Now, "Элемент не привязан к устройству в текущей конфигурации" ) );
                }

                //var folderDevDescript = string.Format("{0}\\Project\\{1}\\", AppDomain.CurrentDomain.BaseDirectory, xeDescDev.Element( "nameR" ).Value);

                DebugStatistics.WindowStatistics.AddStatistic( "Создание и инициализация формы." );

                IDeviceForm deviceform = new FormBlock( 0, uint.Parse( objGuid ) ); // CreateDeviceForm( strTypeBlock, 0, uint.Parse( objGuid ) );
                
                DebugStatistics.WindowStatistics.AddStatistic( "Создание и инициализация формы завершено." );

                //if (deviceform == null)
                //    throw new Exception( string.Format( "Форма {0} не поддерживается\n" +
                //                                        "Приведите тип блока к правильному формату описания\n" +
                //                                        "Наименование типа должно быть в латинской транскрипции", strTypeBlock ) );

                ( deviceform as Form ).Text = CommonUtils.CommonUtils.GetDispCaptionForDevice( guid );

                var isconnectState = false;
                var connectState = PTKState.Iinstance.GetValueAsString(guid.ToString( CultureInfo.InvariantCulture ), "Связь");

                if ( guid == 25855 ) connectState = "true";

                if (connectState == string.Empty || bool.TryParse( connectState, out isconnectState ))
                {
                    if ( !isconnectState )
                        MessageBox.Show("Терминал недоступен или с ним нет связи", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                foreach (IDeviceForm f in arrFrm)
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
                arrFrm.Add(deviceform as Form);

                return deviceform;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(@"(45) : {0} : X:\Projects\01_HMIWinFormsClient\DeviceFormLib\DeviceFormFactory.cs : CreategForm() : ОШИБКА : {1}", DateTime.Now, ex.Message));
            }
        }
    }
}
