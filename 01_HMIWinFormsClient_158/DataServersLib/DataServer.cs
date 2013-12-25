/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс реализующий функциональность DataServer -
 *	            компонент ПТК представляющий источники и их устройства
 *	            для отдельного объекта(подстанции)
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\DataServersLib\DataServer.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 14.11.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using BlockDataComposer;
using HMI_MT_Settings;
using InterfaceLibrary;
using System.Text;
using ProviderCustomerExchangeLib;
using RequsEtntryLib;
using SourceConfigurationLib;

namespace DataServersLib
{
    public class DataServer : IDataServer
    {
        #region События
        /// <summary>
        /// событие потери связи с DataServer
        /// </summary>
        public event DSCommunicationLoss4Client OnDSCommunicationLoss4Client;
		#endregion

		#region Свойства
        /// <summary>
        /// список источников данного DataServer
        /// упорядоченный по их именам
        /// </summary>
        Dictionary<string, ISourceConfiguration> DListSourceConfigurations
        {
            get
            {
                return dListSourceConfigurations;
            }
        }
        Dictionary<string, ISourceConfiguration> dListSourceConfigurations = new Dictionary<string, ISourceConfiguration>();
		#endregion

		#region public
		#endregion

		#region private
        /// <summary>
        /// xml - описание конфигурации источников 
        /// данного DS
        /// </summary>
        XElement xMLDataServerDescribe;
        /// <summary>
        /// список устройств текущего DS
        /// </summary>
        List<IDevice> lstDev4ThisDS;
        /// <summary>
        /// формирователь пакетов
        /// </summary>
        IBlockDataComposer bdc = null;
        SortedList<int, IRequestData> slReqCallbackEvent = new SortedList<int,IRequestData>();
		#endregion

		#region конструктор(ы)
		#endregion

        #region public-методы реализации интерфейса IDataServer
        /// <summary>
        /// Уникальный id данного DS
        /// </summary>
        public UInt32 UniDS_GUID 
        { 
            get{return uniDS_GUID;}
        }
        UInt32 uniDS_GUID = 0 ;
        /// <summary>
        /// ссылка на класс управлениния 
        /// актуальностью запросов к DataServer
        /// </summary>
        public IRequestEntry ReqEntry 
        { 
            get{ return reqEntry; }
        }
        IRequestEntry reqEntry = null;

        /// <summary>
        /// создать конфигурацию DataServer'а
        /// </summary>
        public void CreateConfigurationDS(string path2prgcfg, XElement dataServerXMLDescribe)
        {
            lstDev4ThisDS = new List<IDevice>();

            try
			{
                xMLDataServerDescribe = dataServerXMLDescribe;

                // инициализация источников
                // коллекция описаний источников в данном DataServer
                var xe_SrcsInTheDS = xMLDataServerDescribe.Element("Sources").Elements("Source");

                // уник номер ds
                uniDS_GUID = UInt32.Parse(xMLDataServerDescribe.Attribute("UniDS_GUID").Value);

                SourceConfigurationFactory scf = new SourceConfigurationFactory();

                foreach (var xe_Src in xe_SrcsInTheDS)
                {
                    //if (xe_Src.Element("SourceDriver").Attribute("enable").Value == "false")
                    //    continue;

                    ISourceConfiguration sc = scf.CreateSourceConfiguration(path2prgcfg, xe_Src, dataServerXMLDescribe.Attribute("UniDS_GUID").Value);

                    if (sc != null)
                    {
                        dListSourceConfigurations.Add(xe_Src.Element("SourceDriver").Attribute("nameSourceDriver").Value, sc);

                        // формирование списка устройств текущего DS от всех его источников
                        lstDev4ThisDS.AddRange(sc.GetDeviceList4TheSource());
                    }
                    //else
                    //    throw new Exception(string.Format("(100) : DataServer.cs : CreateConfigurationDS() : Конфигурация {0} не создана.", xe_Src.Attribute("name").Value));
                }


                //slUniDSObjects.Add(uint.Parse(xe_DS.Attribute("UniDS_GUID").Value), new KeyValuePair<XElement, List<IDevice>>(xe_DS, lstDev4DS));

                // создать классы для поддержки запроса значений тегов
                CreateInterConnectComponentBySpecificProtocol(dataServerXMLDescribe);
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
                throw ex;
			}
        }
        /// <summary>
        /// получить ссылку на конфигурацию источника по его имени
        /// </summary>
        /// <param name="namesrccfg"></param>
        /// <returns></returns>
        public ISourceConfiguration GetSrcCfgByName(string namesrccfg)
        {
            ISourceConfiguration srccfg = null;
			try
			{
                srccfg = DListSourceConfigurations[namesrccfg];
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
            return srccfg;
        }
        /// <summary>
        /// получить xml-описание конфигурации 
        /// источников данного DS
        /// </summary>
        /// <returns></returns>
        public XElement GetDataServerXMLDescribe()
        {
            return xMLDataServerDescribe;
        }
        /// <summary>
        /// список объектов данного DS, поддерживающего интерфейс 
        ///		IDevice, т. е. имеющего состояние в виде набора тегов
        /// </summary>
        /// <returns></returns>
        public List<IDevice> GetListDevice()
        {
            return lstDev4ThisDS;
        }
        /// <summary>
        /// разобрать ответ от DataServer
        /// </summary>
        /// <param name="data"></param>
        public void ParseData(byte[] data)
        {
            Console.WriteLine( "ReseiveCallback() : Inconing {0} bytes to server.", data.Length );

            BinaryReader brrez = new BinaryReader(new MemoryStream(data));
            byte codeError;
            UInt16 id_correlation_out;
            UInt16 lenfullrez;

            UInt16 UniDsGuid = 0xffff;
            UInt32 LocObjectGuid = 0xffffffff;

            try
            {
                /*
                * восстанавливаем результат:
                * результат возвращается в виде массивов байт,
                * которые пригодны для восстановления к нормальному виду
                * функциями c# типа BitConverter...
                * 
                * описание конфигураций из которых можно взять конкретику по типам это файлы 
                * в папке \DataServer\...\Debug\Project
                */
                brrez.BaseStream.Position = 0;

                // тип пакета
                byte reqtype = brrez.ReadByte();
                switch (reqtype)
                {
                    case 8:
                    case 1:

                        // код ошибки
                        codeError = brrez.ReadByte();

                        if (codeError != 0)
                            //throw new Exception(string.Format("Ответ от DataServer c codeError = {0}", codeError));
                            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Error, 135, string.Format( "Пакет с запросом несуществующих тегов " ) );

                        /*
                        * идентификатор корреляции:
                        * 0 - не нужно следить за тем пришел 
                        *		ответ на запрос или нет
                        */
                        id_correlation_out = brrez.ReadUInt16();
                        // длина результирующего массива со значениями тегов
                        lenfullrez = brrez.ReadUInt16();

                        while (lenfullrez > 0)
                        {
                            // идентификатор DataServer
                            UInt16 UniDsGuid_out = brrez.ReadUInt16();

                            // уник номер объекта в пределах конкретного DataServer
                            UInt32 LocObjectGuid_out = brrez.ReadUInt32();                       

                            // уник номер группы-подгруппы
                            UInt16 grGuid_out = brrez.ReadUInt16();

                            UInt32 tagGUID = brrez.ReadUInt32();
                            if (tagGUID == 0xffffffff)
                            {
                                ParseGroupData(UniDsGuid, LocObjectGuid_out, brrez);
                                break;
                            }

                            // длина байтового массива отдельного тега
                            UInt16 lenByteArr4Tag = brrez.ReadUInt16();
                            //throw new Exception(string.Format("(256) : DataServer.cs : ParseData() : Нулевая длина байтового массива тега TagGUID = {0}", tagGUID.ToString()));

                            byte[] arrtag = new byte[lenByteArr4Tag];
                            brrez.Read(arrtag, 0, lenByteArr4Tag);

                            // метка времени
                            long timestamp = brrez.ReadInt64();
                            // качество
                            byte qual = brrez.ReadByte();

                            DateTime dttag = DateTime.FromBinary(timestamp);

                            VarQualityNewDs qualtag = VarQualityNewDs.vqUndefined;

                            switch(qual)
                            {
                                case 0:
                                    qualtag = VarQualityNewDs.vqUndefined;
                                    break;
                                case 1:
                                    qualtag = VarQualityNewDs.vqGood;
                                    break;
                                default:
                                    qualtag = VarQualityNewDs.vqUndefined;
                                    break;
                            }

                            SetValueTag(UniDsGuid_out, LocObjectGuid_out, tagGUID, arrtag, dttag, qualtag);

                            lenfullrez -= lenByteArr4Tag;
                            lenfullrez -= 23;
                        }
                        break;
                    case 2:

                        // код ошибки
                        codeError = brrez.ReadByte();

                        /*
                        * идентификатор корреляции:
                        * 0 - не нужно следить за тем пришел 
                        *		ответ на запрос или нет
                        */
                        id_correlation_out = brrez.ReadUInt16();
                        // длина результирующего массива со значениями тегов
                        lenfullrez = brrez.ReadUInt16();

                        UniDsGuid = brrez.ReadUInt16();
                        LocObjectGuid = brrez.ReadUInt32();
                        UInt16 lencmdname = brrez.ReadUInt16();
                        string cmdname = Encoding.UTF8.GetString(brrez.ReadBytes(lencmdname));
                        UInt16 lenparams = brrez.ReadUInt16();
                        byte[] rezcmdparams = new byte[]{};

                        if(lenparams > 0)
                            rezcmdparams = brrez.ReadBytes(lenparams);

                        string key = string.Format("{0}.{1}.{2}", UniDsGuid, LocObjectGuid, cmdname);

                        HMI_MT_Settings.HMI_Settings.CONFIGURATION.ActiveCommands.RemoveCmd(key, codeError);
                        break;
                    case 7:
                        // пакет с осциллограммой
                        // код ошибки
                        codeError = brrez.ReadByte();

                        /*
                        * идентификатор корреляции:
                        * 0 - не нужно следить за тем пришел 
                        *		ответ на запрос или нет
                        */
                        id_correlation_out = brrez.ReadUInt16();

                        UniDsGuid = brrez.ReadUInt16();
                        UInt32 idbl = brrez.ReadUInt32();
                        UInt32 lenosc = brrez.ReadUInt32();

                        byte[] rezosc = new byte[] { };

                        if (lenosc > 0)
                            rezosc = brrez.ReadBytes((int)lenosc);

                        HMI_MT_Settings.HMI_Settings.CONFIGURATION.ActiveOscillograms.RemoveOsc(idbl, rezosc);
                        break;
                    case 9:
                        /* 
                         * пакет сo специальным содержимым
                         * расшифровку которого осущ
                         * потребитель-подписчик на 
                         * событие получения ответ на запрос.
                         * Идентификация потребителя производиться
                         * по уник номеру в теле пакета
                         */
                        // код ошибки
                        codeError = brrez.ReadByte();

                        /*
                        * идентификатор корреляции:
                        * 0 - не нужно следить за тем пришел 
                        *		ответ на запрос или нет
                        */
                        id_correlation_out = brrez.ReadUInt16();

                        Int32 uniCode = brrez.ReadInt32();
                        Int32 lendata = brrez.ReadInt32();

                        if (lendata != 0)
                            slReqCallbackEvent[uniCode].ReqParamsAsByteAray = brrez.ReadBytes(lendata);

                        if (slReqCallbackEvent.ContainsKey(uniCode))
                        {
                            slReqCallbackEvent[uniCode].REQ_Executed(0);
                            slReqCallbackEvent.Remove(uniCode);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw new Exception(ex.Message,ex);
            }
        }

        private void ParseGroupData(UInt16 dsGuid, UInt32 devGuid, BinaryReader brrez)
        {
            UInt16 lenByteArr4Tag = 0;
            try
			{
                while (brrez.BaseStream.Position < brrez.BaseStream.Length)
                {
                    UInt32 tagGUID = brrez.ReadUInt32();

                    // длина байтового массива отдельного тега
                    lenByteArr4Tag = brrez.ReadUInt16();

                    if (lenByteArr4Tag == 0)
                        throw new Exception(string.Format(@"(394) : X:\Projects\01_HMIWinFormsClient\DataServersLib\DataServer.cs : ParseGroupData() : длина байтового массива отдельного тега lenByteArr4Tag = {0}", lenByteArr4Tag.ToString()));

                    byte[] arrtag = new byte[lenByteArr4Tag];
                    brrez.Read(arrtag, 0, lenByteArr4Tag);

                    SetValueTag(dsGuid, devGuid, tagGUID, arrtag, DateTime.Now, VarQualityNewDs.vqGood);

                    // метка времени
                    long timestamp = brrez.ReadInt64();
                    // качество
                    byte qual = brrez.ReadByte();
                }
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
                throw ex;
			}
        }
        /// <summary>
        /// выполнить команду
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteCMD(ICommand cmd)
        {
            if (bdc != null)
                bdc.FormAndSendCMDPacket(cmd);
        }
        /// <summary>
        /// выполнить запрос
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteRequest(IRequestData req)
        {
            try
            {
                if (bdc == null)
                    return;

                if (req.UniGuidRequest <= 0)
                return;

                if (slReqCallbackEvent.ContainsKey(req.UniGuidRequest))
                    slReqCallbackEvent.Remove(req.UniGuidRequest);
                    
                slReqCallbackEvent.Add(req.UniGuidRequest, req);

               bdc.FormAndSendRequestPacket(req);
			}
			catch(Exception ex)
			{
			   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }
        /// <summary>
        /// инициировать процесс чтения осциллограммы osc с 
        /// текущего DS
        /// </summary>
        /// <param name="osc"></param>
        public void ReadOsc(IOscillogramma osc)
        {
            if (bdc != null)
                bdc.FormAndSendOscRequestPacket(osc);
        }
		#endregion

		#region public-методы
		#endregion

		#region private-методы
        /// <summary>
        /// сформировать список устройств указанного DS
        /// </summary>
        /// <param name="xAttribute"></param>
        /// <returns></returns>
        //private List<IDevice> GetDSDevises(string path2prgdevcfg, XElement xe_ds)
        //{
        //    lstDev4ThisDS = new List<IDevice>();

        //    try
        //    {
        //        var xe_srcs = xe_ds.Element("Sources").Elements("Source");

        //        //foreach (var xe_src in xe_srcs)
        //        //    CustomizeDevicesList(path2prgdevcfg, xe_src, ref lstDevices, uint.Parse(xe_ds.Attribute("UniDS_GUID").Value));
        //    }
        //    catch (Exception ex)
        //    {
        //        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
        //    }

        //    return lstDev4ThisDS;
        //}

        void CreateInterConnectComponentBySpecificProtocol(XElement dataServerXMLDescribe)
        {
              /*
               * через фабрику инициализируем компонент взаимодействия 
               * по конкретному протоколу 
               * и передаем ссылку на него конкретной версии формирователя пакетов
               */
            try
			{
			  var pcf = new ProviderCustomerFactory( );

              var xe_ds_access = (from d in dataServerXMLDescribe.Elements("DSAccessInfo") where d.Attribute("enable").Value.ToLower() == bool.TrueString.ToLower() select d).Single();

			    var provCust = pcf.CreateProviderConsumerChanel( xe_ds_access.Attribute( "nameSourceDriver" ).Value,
			                                                     xe_ds_access.Element( "CustomiseDriverInfo" ),
			                                                     HMI_Settings.CONFIGURATION );
                if (provCust is ClientServerOnWCF)
                    (provCust as ClientServerOnWCF).OnTagValueChanged += SetValueTag;
                else
			        provCust.OnByteArrayPacketAppearance += ParseData;
			    provCust.OnDSCommunicationLoss += provCust_OnDSCommunicationLoss;

              /*
               * создаем формирователь пакетов через фабрику с тем, 
               * чтобы можно было реализовать разные алгоритмы формирования пакетов
               */
              var bdcf = new BlockDataComposerFactory( );
              bdc = bdcf.CreateBlockDataComposer("ordinal", provCust);

              /*
               * инициализируем компонент запросов тегов к DS 
               * и передаем ему ссылку на экземпляр 
               * формирователя пакетов
               */
              var reqfact = new RequestFactory( );

              if (provCust is ClientServerOnWCF)
                  reqEntry = reqfact.CreateRequestEntry("wcf", provCust);
              else
                  reqEntry = reqfact.CreateRequestEntry("ordinal", bdc);
			}
			catch( Exception ex )
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                throw ex;
			}
        }

        /// <summary>
        /// реакция на событие потери связи с DS
        /// </summary>
        /// <param name="state"></param>
        private void provCust_OnDSCommunicationLoss( bool state )
        {
			try
			{
                if ( state )  // связь потеряна
                    foreach ( var dev in this.lstDev4ThisDS )
                        foreach ( var tag in dev.GetRtuTags( ) )
                            try
                            {
                                if ( tag == null ) continue;
                                tag.SetDefaultValue( );
                            }
			                catch(Exception ex)
			                {
			                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
			                }

			    if ( OnDSCommunicationLoss4Client != null )       // оповещаем клиентов о потере связи с DS
                    OnDSCommunicationLoss4Client( state );
            }
			catch( Exception ex )
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
			}
        }

        /// <summary>
        /// Задает новое значение тегу на основании
        /// идентификатора DataServer, устройства и тега
        /// </summary>
        private void SetValueTag(UInt16 dsGuid, UInt32 devGuid, UInt32 tagGuid, byte [] tagValue, DateTime tagDateTime, VarQualityNewDs tagQuality)
        {
            IDevice dev = (from d in lstDev4ThisDS where d.UniObjectGUID == devGuid select d).Single<IDevice>();

            ITag tag = dev.GetTag(tagGuid);

            if (tag != null)
                tag.SetValue(tagValue, tagDateTime, tagQuality);
        }
        #endregion
    }
}
