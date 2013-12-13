/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс конфигурации источника
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\SourceMOA.cs                             
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 07.02.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using InterfaceLibrary;
using System.Diagnostics;

namespace SourceMOA
{
	public class MOAConfiguration : ISourceConfiguration
	{
		#region События
		#endregion

		#region Свойства
        /// <summary>
        /// Уник номер DataServer 
        /// которому принадлежит источник
        /// </summary>
        public uint UniDS_GUID
        {
            get{ return uniDS_GUID; }
        }
        uint uniDS_GUID = 0xffffffff;
		#endregion

		#region public
		#endregion

		#region private
        /// <summary>
        /// список устройств источника
        /// </summary>
        List<IDevice> lstDevices = new List<IDevice>();
		#endregion

        #region конструктор(ы)
		public MOAConfiguration()
		{

		}
		#endregion					

		#region public-методы
		#endregion

        #region public-методы реализации интерфейса ISourceConfiguration
        /// <summary>
        /// имя источника
        /// (атрибут nameSourceDriver = ...)
        /// </summary>
        public string NameSource
        { 
            get{return nameSource;}
        }
        string nameSource = string.Empty;
        /// <summary>
        /// путь к файлу описания конфигурации источника
        /// </summary>
        public string Path2src_prgdevcfg 
        { 
            get{ return path2src_prgdevcfg;}
        }
        string path2src_prgdevcfg = string.Empty;
        /// <summary>
        /// Создать и инициализировать конфигурацию источника
        /// </summary>   
        public void InitSrcConfiguration(string path2prgcfg, XElement xe_Src, string uniDS_GUID)
        {
            try
			{
                nameSource = xe_Src.Element("SourceDriver").Attribute("nameSourceDriver").Value;

                this.uniDS_GUID = uint.Parse(uniDS_GUID);

                path2src_prgdevcfg = Path.GetDirectoryName(path2prgcfg) + Path.DirectorySeparatorChar + "Configuration" + 
                                        Path.DirectorySeparatorChar + uniDS_GUID + "#DataServer\\Sources" +
                                        Path.DirectorySeparatorChar + nameSource + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";

                if (!File.Exists(path2src_prgdevcfg))
                    throw new Exception(string.Format("(103) : SourceMOA.cs : InitSrcConfiguration() : Файл {0} не существует.", path2src_prgdevcfg));

                CustomizeDevicesList(path2src_prgdevcfg);
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
       }

        /// <summary>
        /// получить ссылку на устройство
        /// по строке идентификации, специфичной для устройства
        /// например для источника MOA - fc.dev
        /// </summary>
        /// <param name="uniDevGuid"></param>
        /// <returns></returns>
        public IDevice GetDeviceLinkBySrcSpecificStrDescribe(int uniDevGuid)
        {
            if (uniDevGuid < 256)
                return null;

            IDevice dev = null;

            try
			{
                    //dev = (from d in lstDevices where (d.XESsectionDescribe.Attribute("NumECU").Value == strarr[0] && d.XESsectionDescribe.Attribute("NumDev").Value == strarr[1]) select d).Single<IDevice>();
                    foreach ( var device in lstDevices )
                        if ( device.XESsectionDescribe.Attribute( "objectGUID" ).Value == uniDevGuid.ToString() )
                        {
                            dev = device;
                            break;
                        }                
			}
			catch
			{
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 135, string.Format("{0} : {1} : {2} : Запрос несуществующего устройства : {3}", DateTime.Now.ToString(), "SourceMOA.cs", "GetDeviceLinkBySrcSpecificStrDescribe()", uniDevGuid));
			}

            return dev;
        }
        /// <summary>
        /// Получить список устройств для этого источника
        /// </summary>
        /// <returns></returns>
        public List<IDevice> GetDeviceList4TheSource()
        {
            return lstDevices; 
        }
        /// <summary>
        /// функция получения правильного TagGUID:
        ///		TagGUID = адр. рег сдвинуть влево на 8 бит 
        ///		в младшие 4 бита поместить маску 
        ///		битового поля (если она есть)
        /// </summary>
        /// <param name="strtemp"></param>
        /// <returns></returns>
        public uint GetTagGUID(string srcs_pecific_describe)
        {
            uint rez = 0;

            string bitmask;
            try
            {
                string[] strsplt = srcs_pecific_describe.Split(new char []{'.'});

                if (!uint.TryParse(strsplt[0], out rez))    // reg
                    throw new Exception(string.Format("(171) : SourceMOA.cs : GetTagGUID() : Неправильный адрес тега для расчета TagGUID = {0}", srcs_pecific_describe));

                rez = rez << 16;

                if (!string.IsNullOrWhiteSpace(strsplt[1])) //bitmask
                {
                    bitmask = strsplt[1].Replace("\"", "");
                    UInt32 bb = Convert.ToUInt32(bitmask.Trim(), 16);
                    rez = rez + bb;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("(488) : FormulaEvalNDS.cs : GetTagGUID() : Несуществующий адрес ModBus = {0}", srcs_pecific_describe));
            }

            return rez;
        }
		#endregion			

		#region private-методы
        /// <summary>
        /// сформировать список устройств указанного источника
        /// </summary>
        /// <param name="path2prgdevcfgDirectory"></param>
        /// <param name="xe_src"></param>
        /// <param name="lstDevices"></param>
        private void CustomizeDevicesList(string path2src_prgdevcfg)   //, XElement xe_src, ref List<IDevice> lstDevices, uint UniDS_GUID)
        {
            DeviceFactory devFactory = new DeviceFactory();
            IDevice ecu_current = null, device;

            try
            {
                XDocument xdoc_SrcPrgDevCfg = XDocument.Load(path2src_prgdevcfg);

                #region ecu + его устройства
                var xe_ecus = xdoc_SrcPrgDevCfg.Element("MTRA").Element("Configuration").Elements("SourceECU");

                foreach (var xe_ecu in xe_ecus)
                {
                    if (IsDevMayBeHandling(xe_ecu, lstDevices))
                    {
                        ecu_current = (IDevice)devFactory.CreateDevice(xe_ecu, path2src_prgdevcfg, UniDS_GUID);

                        if (ecu_current == null)
                            throw new Exception(string.Format("(163) : SourceMOA.cs : CustomizeDevicesList() : ECU не создан = {0}", xe_ecu.Attribute("describe").Value));

                        lstDevices.Add(ecu_current);
                    }

                    // если в устройстве ECU есть устройства, то добавим их
                    foreach (var xe_devicesInecu in xe_ecu.Element("ECUDevices").Elements("Device"))
                    {
                        if (IsDevMayBeHandling(xe_devicesInecu, lstDevices))
                        {
                            device = devFactory.CreateDevice(xe_devicesInecu, path2src_prgdevcfg, UniDS_GUID);

                            if (device == null)
                                throw new Exception(string.Format("(176) : SourceMOA.cs : CustomizeDevicesList() : устройство для ECU не создано - objectGUID = {0}", xe_devicesInecu.Attribute("objectGUID").Value));

                            (ecu_current as IECU).AddDevices(device);
                            lstDevices.Add(device);
                        }
                    }
                }
                #endregion

                #region устройства вне каких-либо ECU
                var xe_devicesWOecus = xdoc_SrcPrgDevCfg.Element("MTRA").Element("Configuration").Elements("Device");

                foreach (var xe_devicesWOecu in xe_devicesWOecus)
                {
                    if (IsDevMayBeHandling(xe_devicesWOecu, lstDevices))
                    {
                        device = devFactory.CreateDevice(xe_devicesWOecu, path2src_prgdevcfg, UniDS_GUID);

                        if (device == null)
                            throw new Exception(string.Format("(176) : SourceMOA.cs : CustomizeDevicesList() : устройство не создано - objectGUID = {0}", xe_devicesWOecu.Attribute("objectGUID").Value));

                        lstDevices.Add(device);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// проверка существования устройства
        /// с одинаковым objectGUID в обшем списке устройств
        /// отдельного DS. или проверка его доступности для обработки
        /// </summary>
        /// <returns></returns>
        bool IsDevMayBeHandling(XElement xedev4test, List<IDevice> lstDevices)
        {
            try
            {
                if (xedev4test.Attribute("enable").Value.ToLower() == bool.FalseString.ToLower())
                    return false;
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw ex;
            }

            return true;
        }
		#endregion
	}
}
