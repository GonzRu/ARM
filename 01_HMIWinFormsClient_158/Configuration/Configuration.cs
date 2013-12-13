/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Интерфейс взаимодействия с конфигурацией устройств проекта 
 *	            из различных источников
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\Configuration\DeviceProjectConfiguration.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 11.11.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/


using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using InterfaceLibrary;
using System.Xml.Linq;
using DataServersLib;
using RequsEtntryLib;
using System.Diagnostics;
using CommandLib;
using System.Windows.Forms;
using OscillogramsLib;
using System.Threading;
using System.ComponentModel;
using CommonUtils;

namespace Configuration
{
    public class Configuration : IConfiguration
    {
        #region События
        /// <summary>
        /// событие от конфигурации
        /// потери связи с DataServer
        /// </summary>
        public event ConfigDSCommunicationLoss4Client OnConfigDSCommunicationLoss4Client;
        #endregion

		#region Свойства
		#endregion

		#region public
		#endregion

		#region private
        /// <summary>
        /// список DataServer'ов проекта
        /// упорядоченный по номерам
        /// </summary>
        SortedList<uint, IDataServer> slDataServers = new SortedList<uint, IDataServer>();

        /// <summary>
        /// для подсчета времени выполнения команды
        /// </summary>
        Stopwatch stopWatch;
        double interval2 = 0;

        /// <summary>
        /// список типов архивных записей и их значений для формир запроса к БД
        /// для чтения блока данных (уставки, аварии, осциллограммы, диаграммы)
        /// </summary>
        Dictionary<string, string> dlTypeBlockArchivData;
        /// <summary>
        /// для запуска процесса чтения осциллограммы в отдельном потоке
        /// </summary>
        BackgroundWorker bgwoscill = new BackgroundWorker();
		#endregion

		#region конструктор(ы)
        public Configuration ()
        {
            //slUniDSObjects = new SortedList<uint, KeyValuePair<XElement, List<IDevice>>>();
        }
		#endregion

        #region public-методы реализации интерфейса IConfiguration
        /// <summary>
        /// ссылка на класс диспетчеризации 
        /// запросов для подписки/отписки на теги 
        /// конкретному  DataServer 
        /// </summary>
        public ICfgReqEntry CfgReqEntry 
        { 
            get{ return cfgReqEntry; }
        }
        ICfgReqEntry cfgReqEntry = null;
        /// <summary>
        /// активные (запущенные команды)
        /// </summary>
        public IActiveCommands ActiveCommands 
        { 
            get{return activeCommands;}
        }
        IActiveCommands activeCommands = null;
        /// <summary>
        /// активные осциллограммы (фрагменты для сборки)
        /// </summary>
        public IActiveOscillograms ActiveOscillograms { get{return activeOscillograms;} }
        IActiveOscillograms activeOscillograms = null;
        /// <summary>
        /// создать конфигурацию на основе 
        /// файлов конфигурации источника
        /// </summary>
        public void CreateConfiguration(string path2prgcfg)
        {
            try
            {
                XDocument xdoc = XDocument.Load(path2prgcfg);

                // создаем компонент диспетчеризации запросов уровня конфигурации
                CfgRequestFactory cfrf = new CfgRequestFactory();
                cfgReqEntry = cfrf.CreateCfgRequestEntry("ordinal");

                // коллекция описаний DataServers
                var xe_DSs = xdoc.Element( "Project").Element( "Configuration" ).Elements( "Object" );

                DataServerFactory dsf = new DataServerFactory(); 
                
                foreach (var xe_DS in xe_DSs)
                {
                    if (xe_DS.Attribute("enable").Value != "true")
                        continue;

                    // создаем DataServer
                    IDataServer ds = null;
			        try
			        {
                        ds = dsf.CreateDataServer(path2prgcfg,"version_1", xe_DS);
                        ds.OnDSCommunicationLoss4Client += new DSCommunicationLoss4Client(ds_OnDSCommunicationLoss4Client);
                        if (ds == null)
                            throw new Exception(string.Format(@"(135) : X:\Projects\40_Tumen_GPP09\Client\Configuration\Configuration.cs : CreateConfiguration() : DataServer не создан name = {0}", xe_DS.Attribute("name").Value));
			        }
			        catch(Exception ex)
			        {
				        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
                        continue;
			        }

                    slDataServers.Add(ds.UniDS_GUID,ds);
                }

                if (slDataServers.Count == 0)
                    throw new Exception(@"(150) : X:\Projects\40_Tumen_GPP09\Client\Configuration\Configuration.cs : CreateConfiguration() : При ининциализации конфигурации не было создано ни одного экземпляра DataServer");

                // создаем очередь активных команд
                ActiveCommandsFactory acf = new ActiveCommandsFactory();
                activeCommands = acf.CreateActiveCommands("moaactivecmd");

                // создаем очередь активных осциллограмм
                ActiveOscillogramsFactory aco = new ActiveOscillogramsFactory();
                activeOscillograms = aco.CreateActiveOscillograms("moasimpleosc");

                // создаем список значений типов архивных записей
                dlTypeBlockArchivData = new Dictionary<string,string>();

                // получение осциллограммы в отдельном потоке
                bgwoscill.DoWork += new DoWorkEventHandler(bgwoscill_DoWork);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw ex;
            }
        }

        /// <summary>
        /// возвратить массив уник номеров DataServer:
        /// </summary>
        /// <returns></returns>
        public ArrayList GetDataServerNumbers()
        {
            ArrayList arrlistServersNumber = new ArrayList();
            try
            {
                foreach (var elem in slDataServers)
                    arrlistServersNumber.Add(elem.Key);

                return arrlistServersNumber;
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw ex;
            }
        }
        /// <summary>
        /// возвратить XElement - секцию с описанием DataServer
        /// </summary>
        /// <param name="dsnumber">уник номер DataServer</param>
        /// <returns></returns>
        public XElement GetDataServerDescription(uint dsnumber)
        {
            try
            {
                return slDataServers[dsnumber].GetDataServerXMLDescribe();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                //throw ex;
                return null;
            }
        }
        /// <summary>
        /// возвратить список устройств указанного DataServer
        /// </summary>
        /// <param name="dsnumber">уник номер DataServer</param>
        /// <returns></returns>
        public List<IDevice> GetDataServerDevices(uint dsnumber)
        {
            List<IDevice> lstDSDevices = null;
            try
            {
                //return slUniDSObjects[dsnumber].Value;
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw ex;
            }

            return lstDSDevices;
        }

        /// <summary>
        /// получить ссылку на устройство
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="uniDevGuid"></param>
        /// <returns></returns>
        public IDevice GetLink2Device(uint ds, uint uniDevGuid)
        {
            IDevice dev = null;

            try
            {
                IDataServer theds = slDataServers[ds];

                List<IDevice> lstdev = theds.GetListDevice();//kvp.Value;

                foreach( IDevice d in lstdev )
                {
                    if (d.UniObjectGUID == uniDevGuid)
                    {
                        dev = d;
                        break;
                    }
                }
                
                //dev = (from d in lstdev where d.UniObjectGUID == uniDevGuid select d).Single<IDevice>();
           }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return dev;
        }
        /// <summary>
        /// ссылка на тег 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="uniDevGuid"></param>
        /// <param name="uniTagGuid"></param>
        public ITag GetLink2Tag(uint ds, uint uniDevGuid, uint uniTagGuid)
        {
            ITag tag = null;

            try
			{
                IDevice dev = GetLink2Device(ds, uniDevGuid);

                if (dev != null)
                    tag = dev.GetTag(uniTagGuid);
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

            return tag;
        }

        /// <summary>
        /// извлечь описание устройства в конфиг файле источника
        /// по правилам нотации источника
        /// </summary>
        /// <param name="unids_guid">уник ном ds</param>
        /// <param name="src_name">имя источника</param>
        /// <param name="uniDevGuid">идентифиц устр в источнике</param>
        /// <returns></returns>
        public XElement GetDeviceXMLDescription( int unids_guid, string src_name, int uniDevGuid )
        {
            XElement xe_deskdev = null;
            try
			{
                ISourceConfiguration srccfg = GetSrcCfgByName(unids_guid, src_name);

                if (srccfg == null)
                    throw new Exception(string.Format("Configuration.cs : GetDeviceXMLDescription() : Ошибка обращения к конфигурации = {0}", src_name));

                IDevice dev = srccfg.GetDeviceLinkBySrcSpecificStrDescribe(uniDevGuid);

                if (dev != null)
                    xe_deskdev = dev.XESsectionDescribe;
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

            return xe_deskdev;
        }

        /// <summary>
        /// функция вычисления TagGUID 
        /// для тегов конкретного источника, если он
        /// формируется по определенному алгоритму
        /// </summary>
        /// <param name="unids_guid"></param>
        /// <param name="src_name"></param>
        /// <param name="idreg"></param>
        /// <param name="bitMsk"></param>
        /// <returns></returns>
        public uint GetTagGUID(int unids_guid, string src_name, string str_dev_ident_in_src_notation)
        {
            uint rez = 0;

            try
            {
                ISourceConfiguration srccfg = GetSrcCfgByName(unids_guid, src_name);

                if (srccfg == null)
                    throw new Exception(string.Format("(275) : Configuration.cs : GetTagGUID() : Ошибка обращения к конфигурации = {0}", src_name));

                rez = srccfg.GetTagGUID(str_dev_ident_in_src_notation);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return rez;
        }

        /// <summary>
        /// получить ссылку на DataServer
        /// по его уникальному номеру
        /// </summary>
        /// <param name="unidsGuid">уник номер DataServer</param>
        /// <returns></returns>
        public IDataServer GetDataServer(uint unidsGuid)
        {
            IDataServer ds = null;

            if (slDataServers.Keys.Contains(unidsGuid))
                ds = slDataServers[unidsGuid];

            return ds;
        }
        /// <summary>
        /// запустить команду верхнего уровня и передает устройству на низний уровень
        /// </summary>
        /// <param name="ds">уник номер DataServer</param>
        /// <param name="objectGuid">уник номер объекта</param>
        /// <param name="idcmd">идентификатор (имя) команды</param>
        /// <param name="parameters">массив параметров</param>
        /// <param name="parentfrm">родительская форма для случая когда нужно показывать окно выполнения команды, может
        /// быть null</param>
        /// <returns></returns>
        public ICommand ExecuteCommand(uint ds, uint objectGuid, string idcmd, byte[] parameters, Form parentfrm)
        {
            ICommand cmd = null;

            CommandFactory cf = new CommandFactory();
			try
			{
                #region подготовка массива параметров arrParams для выполнения команды
                ArrayList arrParams = new ArrayList();
                arrParams.Add(ds);
                arrParams.Add(objectGuid);
                arrParams.Add(idcmd);
                arrParams.Add(parameters);
                arrParams.Add(parentfrm);
                #endregion

                cmd = cf.CreateCommand("moacmd", arrParams);
                cmd.OnCmdExecuted += new CmdExecuted(cmd_OnCmdExecuted);

                // засекаем время
                stopWatch = Stopwatch.StartNew();
                // ставим команду в очередь активных команд
                /* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                 * обратить внимание на то, что запуск команды
                 * происходит синхронно, пока команда не выполнится
                 * все будет висеть - найти возможность вывести в отд. поток
                 * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                 */
                ActiveCommands.AddCmd(cmd);
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

            return cmd;
        }
        /// <summary>
        /// запросить данные (уставки, аварии, осциллограммы из БД)
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="objectGuid"></param>
        /// <param name="comment"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        //public IRequestData GetData(uint ds, uint objectGuid, string comment, ArrayList arparams)
        //{
        //    IRequestData reqdata = null;

        //    RequestDataFactory rdf = new RequestDataFactory();
        //    try
        //    {
        //        #region подготовка массива параметров arrParams для выполнения запроса
        //        ArrayList arrParams = new ArrayList();
        //        arrParams.Add(ds);
        //        arrParams.Add(objectGuid);
        //        arrParams.Add(arparams);
        //        #endregion

        //        reqdata = rdf.CreateRequestData(comment, arrParams);

        //        IDataServer dse = GetDataServer(ds);
        //        dse.ExecuteRequest(reqdata);
        //    }
        //    catch(Exception ex)
        //    {
        //        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
        //    }

        //    return reqdata;
        //}

        /// <summary>
        /// перегруженный вариант запроса данных (уставки, аварии) из БД
        /// клиент ждет специфич ответа по запросу от DS
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="objectGuid"></param>
        /// <param name="comment"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public IRequestData GetData(uint ds, uint objectGuid, string comment, ArrayList arparams, Int32 uniGuidRequest)
        {
            IRequestData reqdata = null;

            RequestDataFactory rdf = new RequestDataFactory();
            try
            {
                #region подготовка массива параметров arrParams для выполнения запроса
                ArrayList arrParams = new ArrayList();
                arrParams.Add(ds);
                arrParams.Add(objectGuid);
                arrParams.Add(arparams);
                arrParams.Add(uniGuidRequest);
                #endregion
                
                IDevice dev = this.GetLink2Device(ds, objectGuid);

                reqdata = rdf.CreateRequestData(comment, arrParams);

                IDataServer dse = GetDataServer(ds);
                dse.ExecuteRequest(reqdata);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return reqdata;
        }

        /// <summary>
        /// запрос на чтение осциллограммы/диаграммы
        /// </summary>
        /// <param name="idrec"></param>
        public IOscillogramma GetOscData(uint ds, UInt32 idrec)
        {
            IOscillogramma osc = null;

            OscFactory of = new OscFactory();
            try
            {
                osc = of.CreateOscillogramm(ds, "simple", idrec);
                //cmd.OnCmdExecuted += new CmdExecuted(cmd_OnCmdExecuted);

                // запуск в отдельном потоке
                bgwoscill.RunWorkerAsync(osc);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return osc;
        }

        void bgwoscill_DoWork(object sender, DoWorkEventArgs e)
        {
            ActiveOscillograms.AddOsc(e.Argument as IOscillogramma);
        }

        /// <summary>
        /// событе завершения команды
        /// </summary>
        /// <param name="cmd"></param>
        void cmd_OnCmdExecuted(ICommand cmd)
        {
        	try
			{
                // время выполнения
                interval2 = Convert.ToDouble(stopWatch.ElapsedMilliseconds) / 1000;
                if (cmd.ResultTriggering == CommandResult._4_SUCCESS_TRIGGERING)
                    //MessageBox.Show("Команда " + cmd.CmdDispatcherName/*.CmdName*/ + " выполнена.\n Время выполнения: (" + interval2.ToString("F3") + " сек)", "Выполнение команды", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmd.Lta.SetHMIElementsText(string.Empty, string.Empty, string.Format("Команда {0} выполнена", cmd.CmdDispatcherName),string.Format("Время выполнения: {0} сек", interval2.ToString("F3")),"Закрыть");
                else
                    //MessageBox.Show("Команда " + cmd.CmdDispatcherName/*.CmdName*/ + " выполнена с ошибкой.\n Код ошибки: " + cmd.ResultTriggering.ToString(), "Выполнение команды", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmd.Lta.SetHMIElementsText(string.Empty, string.Empty, string.Format("Команда {0} выполнена с ошибкой", cmd.CmdDispatcherName), string.Format("Код ошибки: {0}", cmd.ResultTriggering.ToString()), "Закрыть");
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }
        /// <summary>
        /// установить элемент списка значений 
        /// типов архивных блоков
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetTypeBlockArchivData(string name, string value)
        {
            try
			{
                if (!dlTypeBlockArchivData.ContainsKey(name))
                    dlTypeBlockArchivData.Add(name,value);
                else
                    throw new Exception(string.Format("(409) : Configuration.cs : SetTypeBlockArchivData() : Повтор ключа в списке = {0}", name));
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }
        /// <summary>
        /// получить значение элемента списка значений 
        /// типов архивных блоков
        /// </summary>
        /// <param name="name"></param>
        public string GetTypeBlockArchivData(string name)
        {
            try
            {
                if (!dlTypeBlockArchivData.ContainsKey(name))
                    throw new Exception(string.Format("(426) : Configuration.cs : GetTypeBlockArchivData() : Параметр {0} отсутсвует в списке.", name));
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return dlTypeBlockArchivData[name];
        }
        #endregion

        #region public-методы
        #endregion

        #region private-методы
        void ds_OnDSCommunicationLoss4Client(bool state)
        {
            if (OnConfigDSCommunicationLoss4Client != null)
                OnConfigDSCommunicationLoss4Client(state);
        }
        ///// <summary>
        ///// сформировать список устройств указанного DS
        ///// </summary>
        ///// <param name="xAttribute"></param>
        ///// <returns></returns>
        //private List<IDevice> GetDSDevises(string path2prgdevcfg, XElement xe_ds)
        //{
        //    List<IDevice> lstDevices = new List<IDevice>();

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

        //    return lstDevices;
        //}

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
                var xe_devs = from d in lstDevices where d.UniObjectGUID == uint.Parse(xedev4test.Attribute("objectGUID").Value) select d;

                if (xe_devs.Count() != 0)
                    throw new Exception(string.Format("(194) : SourceMOA.cs : IsExistDevInList() : Повтор objectGUID : {0}",
                                            xedev4test.ToString()));

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
        
        /// <summary>
        /// получить ссылку на конфигурацию 
        /// источника по номеру ds и имени источника
        /// </summary>
        /// <param name="unids_guid"></param>
        /// <param name="src_name"></param>
        /// <returns></returns>
        private ISourceConfiguration GetSrcCfgByName(int unids_guid, string src_name)
        {
            ISourceConfiguration srccfg = null;

            try
			{
                IDataServer ds = slDataServers[(uint)unids_guid];

                srccfg = ds.GetSrcCfgByName(src_name);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

            return srccfg;
        }
        #endregion
    }
}
