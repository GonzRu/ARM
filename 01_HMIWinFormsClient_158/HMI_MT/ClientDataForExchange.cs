using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetCrzaDevices;
using CRZADevices;
using Calculator;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.IO.Pipes;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Data;
using TraceSourceLib;
using System.IO.Compression;

namespace HMI_MT
{
   public delegate void OSCReadyHandler();//MemoryStream ms
   public delegate void CMDExecuted(byte[] cmdrez, CRZADevices.GetReceivedPacket recpack);
   public delegate bool CMDSended(byte[] cmdrez);

   public sealed class ClientDataForExchange
   {
      #region Данные для организации удаленного доступа
      /// <summary>
      /// Класс двунаправленного канала для обмена данными
      /// </summary>
      PipeClientInOut pcIO;

      /// <summary>
      /// массив для входных пакетов - в нее кладет пакеты объект класса канала PipeServerInOut
      /// а читает объект класса ServerDataForExchange
      /// </summary>
      ArrayForExchange arrForReceiveData;

      /// <summary>
      /// массив для выходных пакетов - в нее кладет пакеты объект класса ServerDataForExchange
      /// а читает объект класса канала PipeServerInOut 
      /// </summary>
      ArrayForExchange arrForSendData = new ArrayForExchange();

      /// <summary>
      /// событие поступления пакета с результатами выполнения команды
      /// </summary>
      public event CMDExecuted OnCMDExecuted;

      /// <summary>
      /// событие отправки команды на сервер
      /// </summary>
      public event CMDSended OnCMDSended;

      /// <summary>
      /// длина заголовка пакета
      /// </summary>
      int lenTitle = 10;

      int intervalForConnect;
      byte[] lenInDataAsArrByte; // для приема ключа, длины и типа пакета
      StringBuilder arrreg = new StringBuilder();
      /// <summary>
      /// таймер на сброс источников при отсутсвии связи с DataServer
      /// </summary>
      System.Timers.Timer timerResetFC;
      #endregion

       #region Осциллограммы
       /* 
        * список осциллограмм для чтения (требуется для случая объединения осциллограмм, когда их читается несколько)
        */ 
       public SortedList<int, string> slOsc4Read = new SortedList<int, string>();

       public bool CancelOSCRead
       {
           get { return cancelOSCRead; }
           set
           {
               cancelOSCRead = value;
               if (value)
                   stopwatch.Stop();
           }
       }
       bool cancelOSCRead;

       /// <summary>
       /// флаг показывающий состояние получения осциллограммы
       /// </summary>
       public bool IsOSCinProcessing;
       // массив содержимого осциллограммы
        public byte[] arrOsc;    
      // событие готовности осциллограммы
      public event OSCReadyHandler OSCReadyEvent;
      /// <summary>
      /// класс для замера времени
      /// </summary>
      Stopwatch stopwatch;
       #endregion
       
      [DllImport( "kernel32.dll" )]
		private extern static uint SetSystemTime( ref SYSTEMTIME lpSystemTime );

       /// <summary>
      /// slTagsForActivePages - список активных страниц и соответсвующих им массивов зарегестрированных 
      /// для постоянного объявления переменных. Этот список нужен для того чтобы 
      /// при закрытии страницы уменьшить счетчик для соответсвующей переменной в таблице slTags_usage_count
      /// </summary>
      private SortedList<string, ArrayList> slTagsForActivePages;

      #region списки slID_response и slTags_response - связаны через строку string кот. иденифиц тег подлежащий обновлению
      /// <summary>
      /// slTags_response - список ссылок на регистры сетевого уровня АСУ
      /// </summary>
      private SortedList<string, varMT> slTags_response;
      /// <summary>
      /// slID_response - список устанавливающий соответсвтвие между числом и ссылкой на регистры сетевого уровня АСУ в slTags_response
      /// сервер присылает значение переменной идентифицируя его числом, далее клиент смотрит актуальность ссылки на переменную
      /// в slTags_response, и если да, то передает, пришедший с ним байтовый массив на обработчик соответствующей переменной
      /// </summary>
      private SortedList<ushort, string> slID_response;

      /// <summary>
      /// список соответсвия строкового представления формулы и класса FormulaEval
      /// </summary>
      private SortedList<string, FormulaEval> slFormulaEval;

      /// <summary>
      /// список активных страниц и экземпляров класса FormulaEval для них
      /// Это нужно для работы формы показывающей значения активных формул для текущих страниц
      /// </summary>
	  public SortedList<string, SortedList<string, FormulaEval>> slFormulaEvalsForActivePages;
	   /// <summary>
	  /// список активных страниц и экземпляров класса AdapterBase для них
	  /// Это нужно для работы формы показывающей значения активных формул для текущих страниц
	   /// </summary> 
	  public SortedList<string, SortedList<string, AdapterBase>> slAdapterBaseForActivePages;

      public DataSet DsFEForFEvals;
      #endregion

      /* slTags_usage_count - число активных использований тега на различных страницах
      * используется при формировании запроса к серверу, 
      * сервер возвращает индекс в таблице и значение в виде массива байт, 
      * который должен быть передан обработчику соответствующего типа переменной, 
      * при условии, что ссылка на нее еще существует в slTags_response.
      * значение -1 соответствует случаю, когда тег нигогда не убирается из 
      * этой таблицы
      */
      private SortedList<string, int> slTags_usage_count;
      MainForm parent;
      XDocument xdoc;
	  #region Инициализациия (паттерн Одиночка)
	   /// <summary>
      /// закрытый конструктор
      /// </summary>
	  private ClientDataForExchange()
      {
      }

	  protected class InstanceHolder
	  {
		  static InstanceHolder() { }
		  internal static readonly ClientDataForExchange _instance = new ClientDataForExchange();
	  }

	  public static ClientDataForExchange Iinstance { get { return InstanceHolder._instance; } }


	  public void Iniit(Form prnt, string pipeName)
	  {
		  parent = prnt as MainForm;

		  slTagsForActivePages = new SortedList<string, ArrayList>();
		  slTags_usage_count = new SortedList<string, int>();
		  slTags_response = new SortedList<string, varMT>();
		  slID_response = new SortedList<ushort, string>();
		  slFormulaEval = new SortedList<string, FormulaEval>();
		  slFormulaEvalsForActivePages = new SortedList<string, SortedList<string, FormulaEval>>();
		  slAdapterBaseForActivePages = new SortedList<string, SortedList<string, AdapterBase>>();
		  DsFEForFEvals = new DataSet();

		  //this.OSCReadyEvent += new OSCReadyHandler(ClientDataForExchange_OSCReadyEvent);
		  stopwatch = new Stopwatch();

		  isReqPacketsRead = true;   // необходимо известить сервер о пакетах устройств, которые постоянно должны поступать с сервера

		  xdoc = XDocument.Load(HMI_Settings.PathToPrjFile);

		  lenInDataAsArrByte = new byte[lenTitle];

		  #region настраиваем таймеры
		  // настраиваем таймер на случай потери связи с фк
		  timerResetFC = new System.Timers.Timer();
		  timerResetFC.Interval = 30000;
		  timerResetFC.Elapsed += new System.Timers.ElapsedEventHandler(timerResetFC_Elapsed);
		  timerResetFC.Start();

		  // настраиваем интервал ожидания соединения с pipe-сервером
		  if (!int.TryParse(xdoc.Element("Project").Element("ARMRole").Element("IsTCPClient").Element("intervalForConnect").Attribute("timer_value").Value, out intervalForConnect))
		  {
			  intervalForConnect = 20000;
			  MessageBox.Show("Не задано значение интервал ожидания соединения с pipe-сервером.\n Секция intervalForConnect файла Project.cfg. "
				 + "\n Значение по умолчанию = " + intervalForConnect.ToString(), "ClientDataForExchange", MessageBoxButtons.OK, MessageBoxIcon.Error);
		  }
		  #endregion

		  StartPipeClient(pipeName);
	  }

	  #endregion
      #region логика формирования данных для обмена
      /// <summary>
      /// добавление массива идентификац строк тегов, извлекаемых из FormulaEval
      /// формат: фк.устр.гр.адр_рег.бит_маска
      /// нас интересует - фк.устр.гр.адр_рег
      /// </summary>
      /// <param Name="arrtags"></param>
      public void AddArrTags(string pageCaption, FormulaEval feval)
      {
         ArrayList arrtags;
         arrtags = feval.arrTagVal;
         StringBuilder newIdentStr = new StringBuilder();

         // проверим есть ли данная страница в slTagsForActivePages
         if (!slTagsForActivePages.ContainsKey(pageCaption))
            slTagsForActivePages.Add(pageCaption, new ArrayList());

         // проверим есть ли данная страница в slFormulaEvalsForActivePages
         if (!slFormulaEvalsForActivePages.ContainsKey(pageCaption))
			 slFormulaEvalsForActivePages.Add(pageCaption, new SortedList<string, FormulaEval>());

         if (!DsFEForFEvals.Tables.Contains(pageCaption))
            CreateTableForPage(pageCaption);

         // добавляем FormulaEval
		 if (!(slFormulaEvalsForActivePages[pageCaption] as SortedList<string, FormulaEval>).ContainsKey(feval.SourceFormula))
         {
			 (slFormulaEvalsForActivePages[pageCaption] as SortedList<string, FormulaEval>).Add(feval.SourceFormula, feval);
            DataRow drds = DsFEForFEvals.Tables[pageCaption].NewRow();
            drds["Formula"] = feval.SourceFormula;
            drds["Caption"] = feval.tRezFormulaEval.CaptionIE;            
            DsFEForFEvals.Tables[pageCaption].Rows.Add(drds);
         }

         foreach (TagVal tvs in arrtags)
         {
            newIdentStr.Length = 0;
            string[] strtgs = tvs.strTagIdent.Split(new char[] { '.' });
            if (strtgs.Length == 3) // тег константа
               continue;
            newIdentStr.Append(strtgs[0] + ".");   // fk
            newIdentStr.Append(strtgs[1] + ".");   // dev
            //newIdentStr.Append(strtgs[2] + ".");   // group
            newIdentStr.Append(strtgs[3]);         // reg

            #region формируем ссылку на переменную нижнего уровня
            /* если на тег уже ссылались, увеличим счетчик использования
             * и добавим в список тегов страницы
             */
            if (slTags_response.ContainsKey(newIdentStr.ToString()))
            {
               (slTagsForActivePages[pageCaption] as ArrayList).Add(newIdentStr.ToString());
               slTags_usage_count[newIdentStr.ToString()]++;
               continue;
            }
            // если записи нет, то формируем ее
            foreach (MTDevice aBMRZ in Configurator.MTD)
               if (aBMRZ.NFC == int.Parse(strtgs[0]))
                  if (aBMRZ.NDev == int.Parse(strtgs[1]))
                     if (aBMRZ.varDev.ContainsKey(int.Parse(strtgs[3])))
                     {
                        if (!slTags_response.ContainsKey(newIdentStr.ToString()))
                           slTags_response.Add(newIdentStr.ToString(), (varMT)aBMRZ.varDev[int.Parse(strtgs[3])]);
                        // запомним переменную в массиве для страницы в slTagsForActivePages
                        (slTagsForActivePages[pageCaption] as ArrayList).Add(newIdentStr.ToString());
                        // установим счетчик использования для данной переменной
                        if (!slTags_usage_count.ContainsKey(newIdentStr.ToString()))
                           slTags_usage_count.Add(newIdentStr.ToString(), 1);
                        else
                           // увеличиваем счетчик использования переменной
                           slTags_usage_count[newIdentStr.ToString()]++;
                        break;
                     }
            #endregion
         }

         #region формируем ссылку на переменные ФК
         int[] arrFC = { 60000, 60001, 60003, 60013, 60050, 60052, 60053, 60056, 60057, 60058 };
         // переменные собственно фк
         int[] arrFCasDev = { 00000, 00002, 00004, 00006, 00007 };

         foreach (MTDevice aBMRZ in Configurator.MTD)
         {
            if (aBMRZ is FC_net)
               for (int i = 0; i < arrFCasDev.Length; i++)
               {
                  if (aBMRZ.varDev.ContainsKey(arrFCasDev[i]))
                  {
                     newIdentStr.Length = 0;
                     newIdentStr.Append(aBMRZ.NFC.ToString() + ".");   // fk
                     newIdentStr.Append("0.");   // dev
                     newIdentStr.Append(arrFCasDev[i].ToString());         // reg

                     if (!slTags_response.ContainsKey(newIdentStr.ToString()))
                        slTags_response.Add(newIdentStr.ToString(), (varMT)aBMRZ.varDev[arrFCasDev[i]]);
                     // установим счетчик использования для данной переменной
                     if (!slTags_usage_count.ContainsKey(newIdentStr.ToString()))
                        slTags_usage_count.Add(newIdentStr.ToString(), 1);
                     else
                        // увеличиваем счетчик использования переменной
                        slTags_usage_count[newIdentStr.ToString()] = 1;
                     //break;
                  }
               }
            else
               for (int i = 0; i < arrFC.Length; i++)
               {
                  if (aBMRZ.varDev.ContainsKey(arrFC[i]))
                  {
                     newIdentStr.Length = 0;
                     newIdentStr.Append(aBMRZ.NFC.ToString() + ".");   // fk
                     newIdentStr.Append(aBMRZ.NDev.ToString() + ".");   // dev
                     newIdentStr.Append(arrFC[i].ToString());         // reg

                     if (!slTags_response.ContainsKey(newIdentStr.ToString()))
                        slTags_response.Add(newIdentStr.ToString(), (varMT)aBMRZ.varDev[arrFC[i]]);
                     // установим счетчик использования для данной переменной
                     if (!slTags_usage_count.ContainsKey(newIdentStr.ToString()))
                        slTags_usage_count.Add(newIdentStr.ToString(), 1);
                     else
                        // увеличиваем счетчик использования переменной
                        slTags_usage_count[newIdentStr.ToString()] = 1;
                     //break;
                  }               
               }
         }
         #endregion
      }

	   /// <summary>
	   /// добавление тегов в виде списка 
		/// идентификац строк тегов
	   /// </summary>
	   /// <param name="pageCaption"></param>
	   /// <param name="lstidtgs"></param>
	  public void AddArrTags(string pageCaption, AdapterBase aBase)
	  {
		  ArrayList arrtags = new ArrayList();
		  try
		  {
			  foreach (XElement xe in aBase.XeRawDescribe.Elements("value"))
				  arrtags.Add(xe);// = feval.arrTagVal;

			  StringBuilder newIdentStr = new StringBuilder();

			  // проверим есть ли данная страница в slTagsForActivePages
			  if (!slTagsForActivePages.ContainsKey(pageCaption))
				  slTagsForActivePages.Add(pageCaption, new ArrayList());

			  // проверим есть ли данная страница в slAdapterBaseForActivePages
			  if (!slAdapterBaseForActivePages.ContainsKey(pageCaption))
				  slAdapterBaseForActivePages.Add(pageCaption, new SortedList<string, AdapterBase>());

			  if (!DsFEForFEvals.Tables.Contains(pageCaption))
				  CreateTableForPage(pageCaption);

			  // добавляем AdapterBase
			  foreach (XElement xe in arrtags)
			  {
				  // константы отбразываем
				  if (xe.Attribute("tag").Value.Contains('='))
					  continue;

				  if (!(slAdapterBaseForActivePages[pageCaption] as SortedList<string, AdapterBase>).ContainsKey(xe.Attribute("tag").Value))
				  {
					  (slAdapterBaseForActivePages[pageCaption] as SortedList<string, AdapterBase>).Add(xe.Attribute("tag").Value, aBase);
					  DataRow drds = DsFEForFEvals.Tables[pageCaption].NewRow();
					  drds["Formula"] = aBase.XeRawDescribe.Attribute("express").Value;
					  drds["Caption"] = aBase.XeRawDescribe.Attribute("Caption").Value;
					  DsFEForFEvals.Tables[pageCaption].Rows.Add(drds);
				  }
				  //}

				  //foreach (TagVal tvs in arrtags)
				  //{
				  newIdentStr.Length = 0;
				  string[] strtgs = xe.Attribute("tag").Value.Split(new char[] { '.' });
				  //if (strtgs.Length == 3) // тег константа
				  //    continue;
				  newIdentStr.Append(strtgs[0] + ".");   // fk
				  newIdentStr.Append(strtgs[1] + ".");   // dev
				  //newIdentStr.Append(strtgs[2] + ".");   // group
				  newIdentStr.Append(strtgs[3]);         // reg

				  #region формируем ссылку на переменную нижнего уровня
				  /* если на тег уже ссылались, увеличим счетчик использования
		     * и добавим в список тегов страницы
		     */
				  if (slTags_response.ContainsKey(newIdentStr.ToString()))
				  {
					  (slTagsForActivePages[pageCaption] as ArrayList).Add(newIdentStr.ToString());
					  slTags_usage_count[newIdentStr.ToString()]++;
					  continue;
				  }
				  // если записи нет, то формируем ее
				  foreach (MTDevice aBMRZ in Configurator.MTD)
					  if (aBMRZ.NFC == int.Parse(strtgs[0]))
						  if (aBMRZ.NDev == int.Parse(strtgs[1]))
							  if (aBMRZ.varDev.ContainsKey(int.Parse(strtgs[3])))
							  {
								  if (!slTags_response.ContainsKey(newIdentStr.ToString()))
									  slTags_response.Add(newIdentStr.ToString(), (varMT)aBMRZ.varDev[int.Parse(strtgs[3])]);
								  // запомним переменную в массиве для страницы в slTagsForActivePages
								  (slTagsForActivePages[pageCaption] as ArrayList).Add(newIdentStr.ToString());
								  // установим счетчик использования для данной переменной
								  if (!slTags_usage_count.ContainsKey(newIdentStr.ToString()))
									  slTags_usage_count.Add(newIdentStr.ToString(), 1);
								  else
									  // увеличиваем счетчик использования переменной
									  slTags_usage_count[newIdentStr.ToString()]++;
								  break;
							  }
				  #endregion
			  }
		  }
		  catch (Exception ex)
		  {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
		  }
	  }

      private void CreateTableForPage(string pageCaption)
      {
         DataTable dt = new DataTable(pageCaption);

         dt.Columns.Add("Formula", typeof(string));
         dt.Columns.Add("Caption", typeof(string));
         dt.Columns.Add("Value", typeof(string));

         DsFEForFEvals.Tables.Add(dt);
      }

      /// <summary>
      /// удаление ссылок на теги данной страницы (формы)
      /// </summary>
      /// <param Name="pageCaption"></param>
      public void RemoveRefToPageTags(string pageCaption)
      {
		  try
		  {
			  if (!slTagsForActivePages.ContainsKey(pageCaption))
				  return;

			  if (!slFormulaEvalsForActivePages.ContainsKey(pageCaption))
				  return;
			  else
				  slFormulaEvalsForActivePages.Remove(pageCaption);

			  if (DsFEForFEvals.Tables.Contains(pageCaption))
				  DsFEForFEvals.Tables.Remove(pageCaption);

			  foreach (string stval in (slTagsForActivePages[pageCaption] as ArrayList))
			  {
				  // уменьшаем счетчик использования переменной
				  if (!slTags_usage_count.ContainsKey(stval))
					  continue;

				  slTags_usage_count[stval]--;
				  // если счетчик == 0, то удаляем соответсвующие элементы из списков:
				  if (slTags_usage_count[stval] == 0)
				  {
					  slTags_usage_count.Remove(stval);
					  slTags_response.Remove(stval);
				  }
			  }
			  // теперь удаляем саму страницу из списка активных страниц
			  slTagsForActivePages.Remove(pageCaption);
		  }
		  catch (Exception ex)
		  {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
		  }
      }

	  /// <summary>
	  /// удаление ссылок на теги данной страницы (формы)
	  /// теги задавались с помощью адаптеров
	  /// </summary>
	  /// <param Name="pageCaption"></param>
	  public void RemoveAdapterBaseRefToPageTags(string pageCaption)
	  {
		  try
		  {
			  if (!slTagsForActivePages.ContainsKey(pageCaption))
				  return;

			  if (!slAdapterBaseForActivePages.ContainsKey(pageCaption))
				  return;
			  else
				  slAdapterBaseForActivePages.Remove(pageCaption);

			  if (DsFEForFEvals.Tables.Contains(pageCaption))
				  DsFEForFEvals.Tables.Remove(pageCaption);

			  foreach (string stval in (slTagsForActivePages[pageCaption] as ArrayList))
			  {
				  // уменьшаем счетчик использования переменной
				  if (!slTags_usage_count.ContainsKey(stval))
					  continue;

				  slTags_usage_count[stval]--;
				  // если счетчик == 0, то удаляем соответсвующие элементы из списков:
				  if (slTags_usage_count[stval] == 0)
				  {
					  slTags_usage_count.Remove(stval);
					  slTags_response.Remove(stval);
				  }
			  }
			  // теперь удаляем саму страницу из списка активных страниц
			  slTagsForActivePages.Remove(pageCaption);
		  }
		  catch (Exception ex)
		  {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
		  }
	  }

      #region формирование строк запроса на чтение отд. регистров и пакетов
      bool statemode = false;
      /// <summary>
      /// сформировать строку запроса содержимого отдельных регистров в формате:
      /// @ид_перем#фк.устр.рег
      /// </summary>
      /// <param Name="stateMode">режим вывода инфо об обмене в консоль приложения - 
      /// пункт меню "Вывод диагностики обмена с сервером"</param>
      /// <returns></returns>
      public string GetStringForServerInfoQuery(bool stateMode)
      {
        string rez = String.Empty;

         try
         {
            statemode = stateMode;

            if( statemode )
            {
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 554, "(554) ClientDataForExchange.cs : GetStringForServerInfoQuery() : =====================================================");
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 555, "(555) ClientDataForExchange.cs : GetStringForServerInfoQuery() : ==== " + DateTime.Now.ToLongDateString() + " === " + DateTime.Now.ToLongTimeString());
            }

            rez += GetStringForRegistersQuery();

            // проверяем необходимость чтения осциллограммы
            if( isReqOSCRead )
            {
               string this_IP = String.Empty;
               IPHostEntry host = System.Net.Dns.GetHostEntry(this_IP);
               ArrayList arrIPs = new ArrayList();

               foreach (IPAddress ip in host.AddressList)
                  arrIPs.Add(ip.ToString());

               rez += "@osc#" + idReqOsc + "#" + arrIPs[0];
               // контрольный вывод
               if( statemode )
               {
				   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 574, "(574) ClientDataForExchange.cs : GetStringForServerInfoQuery() : @osc#" + idReqOsc);
				   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 575, "(575) ClientDataForExchange.cs : GetStringForServerInfoQuery() : -----------------------------------------------------");
               }
               isReqOSCRead = false;
               idReqOsc = 0;
            }

            // проверяем нужно ли сообщать серверу группы постоянного чтения
            if( isReqPacketsRead )
            {
               rez += GetStringForRegularPacketQuery();
               isReqPacketsRead = false;
            }
            // проверяем нужно ли сообщать серверу группы чтения по запросу
            if( isPeriodecPacketsRead )
            {
               rez += GetStringForPeriodicPacketQuery();
               isPeriodecPacketsRead = false;
            }
         }catch(Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 768, DateTime.Now.ToString() + " (768) (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs :   GetStringForServerInfoQuery : ОШИБКА - " + ex.Message);
            throw new Exception(DateTime.Now.ToString() + " (768) (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs :   GetStringForServerInfoQuery : ОШИБКА - " + ex.Message);
         }

         return rez;
     }

      /// <summary>
      /// сформировать строку запроса содержимого отдельных регистров в формате:
      /// @ид_перем#фк.устр.рег
      /// </summary>
      /// <returns></returns>
      private string GetStringForRegistersQuery()
      {
         string rez = String.Empty;

         try
         {
            slID_response.Clear();
            short key = 0;

            try
            {
               foreach( string sttag in slTags_response.Keys )
                  slID_response.Add( (ushort)key++, sttag );
            }
            catch( Exception ex )
            {
               TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 795, DateTime.Now.ToString() + " (795) (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs :   GetStringForRegistersQuery() : ОШИБКА - " + ex.Message);
               throw new Exception(DateTime.Now.ToString() + " (795) (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs :   GetStringForRegistersQuery : ОШИБКА - " + ex.Message);
            }

            // формируем строку запроса
            try
            {
               for( int i = 0 ; i < slID_response.Count ; i++ )
                  rez += "@" + ( slID_response.ElementAt( i ).Key ).ToString() + "#" + slID_response.ElementAt( i ).Value;

            }
            catch( Exception ex )
            {
               TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 807, DateTime.Now.ToString() + " (807) (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs :   GetStringForRegistersQuery : ОШИБКА - " + ex.Message);
               throw new Exception(DateTime.Now.ToString() + " (807) (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs :   GetStringForRegistersQuery : ОШИБКА - " + ex.Message);
            }

            // контрольный вывод
            if( statemode )
            {
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 643, "(643) ClientDataForExchange.cs : GetStringForRegistersQuery() : =====================================================");
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 644, "(644) ClientDataForExchange.cs : GetStringForRegistersQuery() : " + rez);
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 645, "(645) ClientDataForExchange.cs : GetStringForRegistersQuery() : -----------------------------------------------------");
            }
         }
         catch( Exception ex )
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 650, DateTime.Now.ToString() + " (650) (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs :   GetStringForRegistersQuery : ОШИБКА - " + ex.Message);
			throw new Exception(DateTime.Now.ToString() + " (650)  (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs :   GetStringForRegistersQuery() :  ОШИБКА - " + ex.Message);
         }
         return rez;
      }

      public bool isReqPacketsRead = false; // флаг необходимости формирования запроса на чтение постоянных групп
      /// <summary>
      /// сформировать строку запроса содержимого отдельных пакетов в формате:
      /// @regpackets#фк.устр.список_адресов_групп_постоянного_чтения_через_запятую
      /// </summary>
      /// <returns></returns>
      private string GetStringForRegularPacketQuery()
      {
          string rez = String.Empty;
         StringBuilder strtmp = new StringBuilder();
         try
         {

            rez += "@regpackets";
            foreach( DataSource aFc in Configurator.KB /*parent.newKB.KB*/ )
               foreach( TCRZADirectDevice tdd in aFc )
               {
                  strtmp.Length = 0;

                  for( int i = 0 ; i < tdd.SlRegularPacket2Send.Count ; i++ )
                  {
                     strtmp.Append( tdd.SlRegularPacket2Send.ElementAt( i ).Key.ToString() + "," );

                     rez += "#" + tdd.NumFC.ToString() + "." + tdd.NumDev.ToString() + "." + strtmp.ToString();
                  }
               }
            // контрольный вывод
            if( statemode )
            {
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 685, "(685) ClientDataForExchange.cs : GetStringForRegularPacketQuery() : rez :" + rez);
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 686, "(686) ClientDataForExchange.cs : GetStringForRegularPacketQuery() : -----------------------------------------------------");
            }
         }
         catch( Exception ex )
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 691, DateTime.Now.ToString() + " (691) (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs :   GetStringForRegularPacketQuery : ОШИБКА - " + ex.Message);
            throw new Exception(DateTime.Now.ToString() + " (691) (== ОШИБКА ===)  : HMI_MT.ClientDataForExchange.cs :   GetStringForRegularPacketQuery : ОШИБКА - " + ex.Message);
         }

         return rez;
      }

      public bool isPeriodecPacketsRead = false; // флаг необходимости формирования запроса на чтение периодических групп
      /// <summary>
      /// сформировать строку запроса содержимого отдельных пакетов в формате:
      /// @periodicpackets#фк.устр.список_адресов_групп_периодического_чтения_через_запятую
      /// </summary>
      /// <returns></returns>
      private string GetStringForPeriodicPacketQuery()
      {
         /* 
          * для формирован ия строки с требованием периодически читать 
          * группы необходимо вызывать ф-цию SetReq4PeriodicPacketQuery()
          * из кода формы где требуется результат обработки пакета (см. frmBMRZ или frmEkra)
          */

         string rez = String.Empty;
         StringBuilder strtmp = new StringBuilder();

         rez += "@periodicpackets";
         foreach (string stpp in arr_periodic_packets)
            rez += "#" + stpp;

         // контрольный вывод
         if (statemode)
         {
			 TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 722, "(722) ClientDataForExchange.cs : GetStringForPeriodicPacketQuery() : rez :" + rez);
			 TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 723, "(723) ClientDataForExchange.cs : GetStringForPeriodicPacketQuery() : -----------------------------------------------------");
         }
         arr_periodic_packets.Clear();

         return rez;
      }

      ArrayList arr_periodic_packets = new ArrayList();
      /// <summary>
      /// сформировать запрос на пакеты с данными группы numgrindev
      /// </summary>
      /// <param Name="numgrindev">номер группы в устройстве</param>
      public void SetReq4PeriodicPacketQuery( int numfc,int numdev, int numgrindev)
      {
         StringBuilder strtmp = new StringBuilder();
         bool isdevfnd = false;

         isPeriodecPacketsRead = true;

         foreach (DataSource aFc in Configurator.KB)
         {
            foreach (TCRZADirectDevice tdd in aFc)
               if (tdd.NumDev == numdev && tdd.NumFC == numfc )
               {
                  strtmp.Length = 0;

                  for (int i = 0; i < tdd.SlPeriodicPacket2Send.Count; i++)
                  {
                     if (tdd.SlPeriodicPacket2Send.ElementAt(i).Value == numgrindev)
                        strtmp.Append(tdd.SlPeriodicPacket2Send.ElementAt(i).Key.ToString() + ",");
                  }

                  arr_periodic_packets.Add(tdd.NumFC.ToString() + "." + tdd.NumDev.ToString() + "." + strtmp.ToString());
                  isdevfnd = true;
                  break;
               }
            if (isdevfnd)
               break;
         }
      }
      #endregion

      #region обеспечение чтения осциллограмм с сервера
      bool isReqOSCRead;    // флаг необходимости чтения осциллограммы

      int idReqOsc = 0;        // идентификатор записи с осциллограммой в БД
      /// <summary>
      /// установить требование для чтения осциллограммы с сервера
      /// </summary>
      /// <param Name="idrec"></param>
      void SetReqForOSCRead(int idrec)
      {
         stopwatch.Reset();
         stopwatch.Start();
         isReqOSCRead = true;
         idReqOsc = idrec;
         cancelOSCRead = false;

         using (dlgOscReceiveProcess dlgORP = new dlgOscReceiveProcess(this)) 
         {
            dlgORP.ShowDialog();
         }
      }
       MemoryStream ms_arrosc4copy;
      public void SetArrOSC(byte[] arrosc)
      {
         arrOsc = new byte[arrosc.Length];
         Buffer.BlockCopy(arrosc, 0, arrOsc, 0, arrosc.Length);

         // чтобы развязать связку диалогового окна и этого класса создадим таймер, 
         // по срабатыванию которого вызовется событие окончания чтения осциллограммы
         tmrOscevent = new System.Timers.Timer();
         tmrOscevent.Elapsed += new System.Timers.ElapsedEventHandler(tmrOscevent_Elapsed);
         tmrOscevent.Interval = 500;
         ms_arrosc4copy = new MemoryStream(arrosc);
         tmrOscevent.Start();
         //ClientDataForExchange_OSCReadyEvent(arrosc);
      }

      System.Timers.Timer tmrOscevent;

      void tmrOscevent_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {
         tmrOscevent.Stop();

         OnOSCReadyEvent();
         ClientDataForExchange_OSCReadyEvent(ms_arrosc4copy.ToArray());
      }

      public void OnOSCReadyEvent()
      {
         if (OSCReadyEvent != null)
            OSCReadyEvent();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param Name="ide">идентификатор записи с блоком данных в БД</param>
      /// <param Name="ifa">имя файла для записи осц.</param>
      public void AddOSC2List(int ide, string ifa)
      {
         if (!slOsc4Read.ContainsKey(ide))
            slOsc4Read.Add(ide, ifa);
      }

      /// <summary>
      /// старт процесса чтения осциллограммы
      /// </summary>
      public void StartProceccReadOSC(short cntselectOSC)
      {
         /*   работаем только с первой записью, 
              если список пуст, то это говорит о том, что все осциллограммы считаны
          */
         cntSelectOSC = cntselectOSC;
         SetReqForOSCRead(Convert.ToInt32(slOsc4Read.ElementAt(0).Key));
      }

      short cntSelectOSC;
      ArrayList asb = new ArrayList();

      void ClientDataForExchange_OSCReadyEvent(byte[] arrosc)
      {
         StringBuilder sb = new StringBuilder();

		/*
		* осциллограмма может состоять из нескольких кусков,
		* поэтому полученный пакет имеет формат исходя 
		* из которого можно эти куски вычленить:
		* 1 байт - число кусков;
		* 2 байта - длина очередного куска
		* n байт - содержимое очередного куска
		*/
		 BinaryReader br = new BinaryReader(new MemoryStream(arrosc));

		  // выясним сколько фрагментов в осциллограмме
		 byte cntfragments = br.ReadByte();

		 asb.Add(slOsc4Read.ElementAt(0).Value);  // добавили название файла с осциллограммой

		  // выяснили раздельно имя файла и его расширение
		 string filename = AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + Path.GetFileNameWithoutExtension(slOsc4Read.ElementAt(0).Value);
		 string fileextens = Path.GetExtension(slOsc4Read.ElementAt(0).Value);

		 CreateOscFile(filename, fileextens, br);

		 // удаляем верхнюю запись в списке считываемых осциллограмм
		 slOsc4Read.RemoveAt(0);

		  // если есть другие фрагменты осциллограммы то формируем файлы из них
		 for (int i = 1; i < cntfragments; i++)
			 CreateOscFile(filename + "_" + i.ToString() /*номер фрагмента*/, fileextens, br);

         if (slOsc4Read.Count != 0)
         {
            StartProceccReadOSC(cntSelectOSC); // читаем дальше осциллограммы
            return;
         }

         // все осциллограммы прочитаны - запускаем fastview OscView.exe
         stopwatch.Stop();    // остановили подсчет времени

		 // запускаем OscView.exe
		 string oscviewexePath = AppDomain.CurrentDomain.BaseDirectory + "\\OscView\\OscView.exe";
		 if (!File.Exists(oscviewexePath))
		 {
			 MessageBox.Show("Отсутствует файл запуска OscView. Путь : " + oscviewexePath, "(725) ClientDataForExchange.cs : ClientDataForExchange_OSCReadyEvent",MessageBoxButtons.OK, MessageBoxIcon.Error);
			 return;
		 }

         Process prc = new Process();

         switch (cntSelectOSC)
         {
            case 0:
               return;
            case 1:
               prc.StartInfo.FileName =   oscviewexePath;
               prc.StartInfo.Arguments = "\"" + AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + asb[0].ToString() + "\"";
               break;
            default: // 2 и более осциллограммы
               sb = new StringBuilder();
               foreach (string s in asb)
               {
                  sb.Append("\"" +  s.ToString() + "\"");
                  sb.Append(" ");
               }
			   prc.StartInfo.FileName = oscviewexePath;
               prc.StartInfo.Arguments = "-o " + sb.ToString();
               break;
         }
         prc.Start();
         asb.Clear();
         slOsc4Read.Clear();
      }

	  private void CreateOscFile(string p,string e, BinaryReader br)
	  {
		  FileStream f;
		  int lenfragment = 0;

			  try
			  {
				  lenfragment = br.ReadInt32();
				  f = File.Create(p + e);
			  }
			  catch (Exception ex)
			  {
				  MessageBox.Show(ex.Message);
				  f = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + "последняя_осциллограмма.osc");//ifa
			  }

			  f.Write(br.ReadBytes((int)lenfragment), 0, (int)lenfragment);
			  f.Close();
	  }

      /// <summary>
      /// возвращает строку с временем в сек
      /// прошедшим с начала формирования осциллограмм(ы)
      /// </summary>
      /// <returns></returns>
      public string GetStrTimeReadOSC()
      {
         return Convert.ToString(stopwatch.ElapsedTicks / Stopwatch.Frequency);
      }
      #endregion

      public void SetRegsFromServerTags(byte[] buffer)
      {
         MemoryStream ms = new MemoryStream(buffer);
         BinaryReader br = new BinaryReader(ms);
         ushort idreg;
         StringBuilder strForIdReg = new StringBuilder();
         string[] arrforreg;
         int key;
         //string fcc;
         List<string> lstfc = new List<string>();

         while (br.BaseStream.Position < br.BaseStream.Length)
            try
            {
               strForIdReg.Length = 0;
               idreg = br.ReadUInt16();
               if (slID_response.ContainsKey(idreg))
                  strForIdReg.Append(slID_response[idreg]);
               else
               {
                  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 763, DateTime.Now.ToString() + "(763)  (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs : SetRegsFromServerTags :  ОШИБКА : в списке отсутсвует ключ = " + idreg.ToString());
                  return;
               }
               if (strForIdReg.ToString() == "0.32.38")
               {
                  strForIdReg.Length = 0;
                  strForIdReg.Append("0.32.38");
               }
               // извлекаем номер регистра - будет key
               arrforreg = (strForIdReg.ToString()).Split(new char[] { '.' });
               key = int.Parse(arrforreg[2]);
               //fcc = int.Parse(arrforreg[0]);

               if (!lstfc.Contains(arrforreg[0]))
                  lstfc.Add(arrforreg[0]);

					if (slTags_response.ContainsKey(strForIdReg.ToString()))
						(slTags_response[strForIdReg.ToString()] as varMT).ExtractValue(ref br, ref key, 0 , 0 );//Convert.ToUInt32(DateTime.Now) Convert.ToByte(DateTime.Now.Millisecond)
               else
                  /* страница(форма) уже неактивна то мы по идентификационной строке должны найти переменную нижнего уровня
                   * и увеличить позицию в потоке на число байт этой переменной нижнего уровня - типа фиктивное чтение
                   */
                  foreach (MTDevice aBMRZ in Configurator.MTD)
                     if (aBMRZ.NFC == int.Parse(arrforreg[0]))
                        if (aBMRZ.NDev == int.Parse(arrforreg[1]))
                           if (aBMRZ.varDev.ContainsKey(int.Parse(arrforreg[2])))
                              br.BaseStream.Position += (aBMRZ.varDev[int.Parse(arrforreg[2])] as varMT).varMT_Len;
            }
            catch (Exception ex)
            {
               TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 704, DateTime.Now.ToString() + "(704)  (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs : SetRegsFromServerTags :  ОШИБКА : " + ex.Message);
//return;
               continue;
			   }

         // подтверждаем качество - при новой технологии получения инфо без пакетов CountLastPacket
         // поэтому действия ниже не имеют смысла
         foreach (string stf in lstfc)
            foreach (MTDevice aBMRZ in Configurator.MTD)
               if ((aBMRZ as FC_net) != null)
                  if ((aBMRZ as FC_net).numFC.ToString() == stf)
                     (aBMRZ as FC_net).CountLastPacket++;// = 1;
                     
      }
      #endregion

      #region обмен посредством каналов
	        /// старт pipe-клиента (pipe-сервер - главная форма приложения -шлюза)
      /// </summary>
      private void StartPipeClient(string pipeName)
      {
        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Information, 826, DateTime.Now.ToString() + " : HMI_MT.ClientDataForExchange.cs : StartPipeClient :" );

         // СБРАСЫВАЕМ ФК
         foreach( DataSource aFc in Configurator.KB /*newKB.KB*/ )
            aFc.isFCConnection = false;

         #region стартуем сетевой манеджер, если есть активные сеансы, закрываем их
         //StartGateForLocalSystem();
         #endregion

         // создаем канал
         pcIO = new PipeClientInOut( pipeName );
         pcIO.Start( intervalForConnect );

         /* 
          * создаем объекты классов вх. и вых массивов для того, чтобы класс ServerDataForExchange 
          * обменивался данными через них
          * 
          * arrForReceiveData - массив входных пакетов - в него кладет пакеты объект класса канала PipeServerInOut
          * а читает объект класса ServerDataForExchange
          * 
          * arrForSendData - массив выходных пакетов - в него кладет пакеты объект класса канала ClientDataForExchange
          * а читает объект класса PipeClientInOut и отправлет pipe-серверу
          */
         arrForReceiveData = new ArrayForExchange();
         // привязка к событию появления данных в массиве вх пакетов
         arrForReceiveData.packetAppearance += new ByteArrayPacketAppearance( this.byteQueque_packetAppearance );
         pcIO.arrForReseivePackets = arrForReceiveData;

         arrForSendData = new ArrayForExchange();
         // привязка к событию появления данных в массиве вых пакетов
         arrForSendData.packetAppearance += new ByteArrayPacketAppearance( pcIO.byteQueque_packetAppearance );
      }

      // закрытие данного экземпляра ClientDataForExchange
      public void Close()
      {
         if( pcIO != null )
            pcIO.CloseAndDisconnectPipeClient();
      }

      /// <summary>
      /// старт локального шлюза - PipeLocalProxy.exe
      /// </summary>
      private void StartGateForLocalSystem() 
      {
         Process[] prmans;
         Process prc = new Process();

         #region если сетевой клиент запущен - завершаем его
         // вначале определим что в данном сеансе является поставщиком данных - локальный DataServer или
         // сетевой клиент
         string nameprc = xdoc.Element("Project").Element("SystemDescribe").Attribute("pipeEXEname").Value;
         if (String.IsNullOrEmpty(nameprc))
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 758, DateTime.Now.ToString() + " : HMI.ClientDataForExchange.cs : StartGateForLocalSystem : источник данных не задан, запустите его самостоятельно в течении 10 сек.");
            Thread.Sleep(10000);
            return;  // самостоятельный запуск источника данных
         }


         prmans = Process.GetProcessesByName(nameprc);

         foreach (Process pr in prmans)
            if (pr.Id != Process.GetCurrentProcess().Id)
               try 
               {
                  pr.Kill();
                  Thread.Sleep(2000);
               }
               catch(Exception ex)
               {
                  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 749, DateTime.Now.ToString() + "(749)  (== ОШИБКА ===) : HMI.ClientDataForExchange.cs : ОШИБКА - StartGateForLocalSystem : " + ex.Message);
               }

         #endregion  
       
         // стартуем шлюз
         // Изменим текущую папку
         Thread.Sleep(6000);
         Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory + nameprc;
         prc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + nameprc + Path.DirectorySeparatorChar + nameprc + ".exe";
         prc.Start();
         Thread.Sleep(2000);

         // восстановим текущую папку
         Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

         // подпишем устройства на события отправки и выполнения команды
         foreach (DataSource aFc in Configurator.KB)
            foreach (TCRZADirectDevice tcdd in aFc.Devices)
            { 
               this.OnCMDExecuted += tcdd.CmdExecuted;
               this.OnCMDSended += tcdd.CmdSended;
            }
      }

      byte[] arrClientKey = new byte[] { 0x3f, 0xf3, 0x3f, 0xf3 };
      bool IamBusy = false;
      //int ClientKey = BitConverter.ToInt32(arrClientKey, 0);

      /// <summary>
      /// <summary>
      /// вызывается при появлении данных в массиве для приема пакетов по каналу от pipe-сервера
      /// </summary>
      /// <param Name="pq"></param>
      public void byteQueque_packetAppearance(byte[] pq)
      {
         int ij = 0;
         if (pq.Length == 222 || pq.Length == 212)
            ij = 1;
         //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, DateTime.Now.ToString() + " (0) : Точка = 8 = (ClientDataForExchange.cs::byteQueque_packetAppearance()) : длина пакета = " + pq.Length.ToString());

         ushort typeData = BitConverter.ToUInt16(pq,8);
         int len = BitConverter.ToInt32(pq,4);

         MemoryStream msread = new MemoryStream(pq);

         // действия в соответствии с типом запроса
         arrreg.Length = 0;
         short lenCurPacket;
         StringBuilder receivestr = new StringBuilder(); // строка для приема времени

         SYSTEMTIME systemTime;
         #region systemTime
		   systemTime.wYear = (ushort)DateTime.Now.Year;
         systemTime.wMonth = (ushort)DateTime.Now.Month;
         systemTime.wDayOfWeek = (ushort)DateTime.Now.DayOfWeek;
         systemTime.wDay = (ushort)DateTime.Now.Day;
         systemTime.wHour = (ushort)DateTime.Now.Hour;
         systemTime.wMinute = (ushort)DateTime.Now.Minute;
         systemTime.wSecond = (ushort)DateTime.Now.Second;
         systemTime.wMilliseconds = (ushort)DateTime.Now.Millisecond;
	      #endregion

         TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 927, DateTime.Now.ToString() + " (927) : HMI_MT.ClientDataForExchange.cs : byteQueque_packetAppearance() : прочитали пакет : тип = " + typeData.ToString() + "; длина = " + len.ToString());
         
         try 
         {
            switch (typeData)
            {
               case 0:
               case 3:
                  //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, DateTime.Now.ToString() + " (0) : Точка = 8.3 = (ClientDataForExchange.cs::byteQueque_packetAppearance()) ");
                  /* 
                   * поддержание связи -
                   * отсылаем обратно
                   */
                  SendTitleWithLen0(-1);
                  break;
               case 1:
                  //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, DateTime.Now.ToString() + " (0) : Точка = 8.1 = (ClientDataForExchange.cs::byteQueque_packetAppearance()) ");
                  byte[] bufferp = new byte[msread.Length - lenTitle];
                  Buffer.BlockCopy(msread.ToArray(), lenTitle, bufferp, 0, bufferp.Length);
                  MemoryStream mstr = new MemoryStream(bufferp);
                  BinaryReader mstbr = new BinaryReader(mstr);

                  byte[] arr = new byte[bufferp.Length];

                  while (mstbr.BaseStream.Position < len)
                  {
                     lenCurPacket = mstbr.ReadInt16();
                     arr = new byte[lenCurPacket];
                     arr = mstbr.ReadBytes(lenCurPacket);
                     parent.newKB.SendPacketsToFC(arr);                     
                  }
                  #region для поддержания связи - отсылаем обратно заголовок с нулевой длиной
                  SendTitleWithLen0(-1);
                  #endregion
                  break;
               case 2:
                  //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, DateTime.Now.ToString() + " (0) : Точка = 8.2 = (ClientDataForExchange.cs::byteQueque_packetAppearance()) ");
                  #region принимаем состояние сервера - панель сообщений и т.п.
                  // ... код удален как устаревший
                  #endregion
                  break;
               case 4:
                  //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, DateTime.Now.ToString() + " (0) : Точка = 8.4 = (ClientDataForExchange.cs::byteQueque_packetAppearance()) ");
                  #region устанавливаем системное время
                  receivestr.Length = 0;
                  byte[] buft = new byte[len];
                  Buffer.BlockCopy(msread.ToArray(), lenTitle, buft, 0, len);

                  receivestr.Append(Encoding.ASCII.GetString(buft));
                  string[] dt = receivestr.ToString().Split(new char[] { '=' });
                  for (int i = 0; i < dt.Count(); i += 2)
                  {
                     switch (dt[i])
                     {
                        case "wYear":
                           systemTime.wYear = Convert.ToUInt16(dt[i + 1]);
                           break;
                        case "wMonth":
                           systemTime.wMonth = Convert.ToUInt16(dt[i + 1]);
                           break;
                        case "wDay":
                           systemTime.wDay = Convert.ToUInt16(dt[i + 1]);
                           break;
                        case "wDayOfWeek":
                           systemTime.wDayOfWeek = Convert.ToUInt16(dt[i + 1]);
                           break;
                        case "wHour":
                           systemTime.wHour = Convert.ToUInt16(dt[i + 1]);
                           break;
                        case "wMinute":
                           systemTime.wMinute = Convert.ToUInt16(dt[i + 1]);
                           break;
                        case "wSecond":
                           systemTime.wSecond = Convert.ToUInt16(dt[i + 1]);
                           break;
                        case "wMilliseconds":
                           systemTime.wMilliseconds = Convert.ToUInt16(dt[i + 1]);
                           break;
                        default:
                           Trace.TraceInformation(" (== ОШИБКА ===) Ошибка в MainForm.RunServer() : неправильный формат времени = " + receivestr.ToString());
                           break;
                     }
                  }
                  SetSystemTime(ref systemTime);  // установка системного времени
                  #endregion
                  #region для поддержания связи - отсылаем обратно заголовок с нулевой длиной
                  SendTitleWithLen0(-1);
                  #endregion
                  break;
               case 5:
                  //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, DateTime.Now.ToString() + " (0) : Точка = 8.5 = (ClientDataForExchange.cs::byteQueque_packetAppearance()) ");
                  /* 
                   *  запрос на список тегов (чтение конкретных регистров)
                   *  требуемых к обновлению на клиенте
                   */
                  arrreg.Append(GetStringForServerInfoQuery(parent.tsmiServerDiagOut2Consol.Checked));
                  // преобразуем в массив байт
                  byte[] arrstr = Encoding.ASCII.GetBytes(arrreg.ToString());
                  byte[] arr4send = new byte[arrstr.Length + lenTitle];

                  // формируем заголовок
                  Buffer.BlockCopy(arrClientKey, 0, arr4send, 0, arrClientKey.Length);
                  Buffer.BlockCopy(BitConverter.GetBytes(arr4send.Length - lenTitle), 0, arr4send, 4, 4);   // длина данных в пакете не должна учитываать заголовок
                  Buffer.BlockCopy(BitConverter.GetBytes(typeData), 0, arr4send, 8, 2);
                  // заголовок + данные
                  Buffer.BlockCopy(arrstr, 0, arr4send, lenTitle, arrstr.Length);

                  //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 1, DateTime.Now.ToString() + " (1) : Точка = 8.5 -> 7 = (ClientDataForExchange.cs::byteQueque_packetAppearance()) ");
                  arrForSendData.Add(arr4send);                  
                  break;
               case 6:
                  //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, DateTime.Now.ToString() + " (0) : Точка = 8.6 = (ClientDataForExchange.cs::byteQueque_packetAppearance()) ");
                  // принимаем результат запроса на чтение конкретных регистров
                  // сначала длину, а затем сами данные
                  byte[] buffer = new byte[msread.Length - lenTitle];
                  if (buffer.Length != 0)
                  {
                     // передаем на разбор
                     Buffer.BlockCopy(msread.ToArray(), lenTitle, buffer, 0, buffer.Length);
                     SetRegsFromServerTags(buffer);
                  }
                  #region для поддержания связи - отсылаем обратно заголовок с нулевой длиной
                  SendTitleWithLen0(-1);
                  #endregion
                  break;
               case 7:
                  //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, DateTime.Now.ToString() + " (0) : Точка = 8.7 = (ClientDataForExchange.cs::byteQueque_packetAppearance()) ");
                  // принимаем длину осциллограммы  
                  byte[] bufferosc = new byte[msread.Length - lenTitle];
                  Buffer.BlockCopy(msread.ToArray(), lenTitle, bufferosc, 0, bufferosc.Length);

                  MemoryStream msosc = new MemoryStream(bufferosc);
                  BinaryReader br = new BinaryReader(msosc);
                  int lenreceive_orig = br.ReadInt32();
                  int lenreceive = br.ReadInt32();
                  byte[] bufsum = new byte[lenreceive];
                  // приняли сжатый поток - разжимаем
                  br.Read(bufsum,0,lenreceive);

                  // 

                  #region код с декомпрессией
                  //      if (lenreceive_orig < 10000) // 30000 ограничение сделано для диаграмм, чтобы их не сжимать
                  //   SetArrOSC(bufsum);
                  //else
                  //{ 
                  //   using (MemoryStream ms = new MemoryStream(bufsum))
                  //   {
                  //      byte[] bufsum_orig = new byte[lenreceive_orig];
                  //      using (Stream ds = new DeflateStream(ms, CompressionMode.Decompress, true))
                  //      {
                  //         ds.Read(bufsum_orig, 0, lenreceive_orig);
                  //      }
                  //      SetArrOSC(bufsum_orig);
                  //   }
                  //} 
	               #endregion

                        #region код без декомпрессии
                        SetArrOSC(bufsum);
                        #endregion

                  #region для поддержания связи - отсылаем обратно заголовок с нулевой длиной
                  SendTitleWithLen0(-1);
                  #endregion
                  break;
               case 8:
                  //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, DateTime.Now.ToString() + " (0) : Точка = 8.8 = (ClientDataForExchange.cs::byteQueque_packetAppearance()) ");
                  timerResetFC.Stop();
                  // состояние связи с источниками
                  byte[] bbuffer = new byte[msread.Length - lenTitle];
                  int ii = 0;

                  Buffer.BlockCopy(msread.ToArray(), lenTitle, bbuffer, 0, bbuffer.Length);
                  if (bbuffer.Length != 0)
                     foreach (DataSource aFC in Configurator.KB)
                     {
                        aFC.isFCConnection = Convert.ToBoolean(bbuffer[ii]);
						//if (!aFC.isFCConnection)
						//   aFC.isFCConnection = aFC.isFCConnection;
                        //aFC.isLostConnection = !Convert.ToBoolean(bbuffer[ii]);   // это значение уст при регулярном чтении в файле Configurator.cs
                        ii++;
                     }

                  #region для поддержания связи - отсылаем обратно заголовок с нулевой длиной
                  SendTitleWithLen0(-1);
                  timerResetFC.Start();
                  #endregion
                  break;
               case 10:
                  /* 
                  *  запрос на отправку серверу пакетов команд,
                  *  запрошенных для выполнения клиентом
                  */
                  byte[] cmdPack = new byte[0];
                  byte[] cmdPack_copy = new byte[0];
                  foreach (DataSource aFc in Configurator.KB)
                     foreach (TCRZADirectDevice tdd in aFc)
                        if (tdd.StatusReadyForSendCMD)
                        {
                           cmdPack = new byte[tdd.arrCommandPacks.Length];
                           cmdPack_copy = new byte[tdd.arrCommandPacks.Length];
                           Buffer.BlockCopy(tdd.arrCommandPacks, 0, cmdPack, 0, cmdPack.Length);
                           Buffer.BlockCopy(tdd.arrCommandPacks, 0, cmdPack_copy, 0, cmdPack_copy.Length);
                           if (OnCMDSended != null)
                              OnCMDSended(cmdPack);
                        }
                  if (cmdPack_copy.Length != 0)
                     Buffer.BlockCopy(cmdPack_copy, 0, cmdPack, 0, cmdPack_copy.Length);

                  arr4send = new byte[cmdPack.Length + lenTitle];

                  //// формируем заголовок
                  Buffer.BlockCopy(arrClientKey, 0, arr4send, 0, arrClientKey.Length);
                  Buffer.BlockCopy(BitConverter.GetBytes(arr4send.Length - lenTitle), 0, arr4send, 4, 4);   // длина данных в пакете не должна учитываать заголовок
                  Buffer.BlockCopy(BitConverter.GetBytes(typeData), 0, arr4send, 8, 2);
                  //// заголовок + данные
                  Buffer.BlockCopy(cmdPack, 0, arr4send, lenTitle, cmdPack.Length);

                  arrForSendData.Add(arr4send);
                  break;
               case 11:
                  /*
                   * принимаем результат выполнения команды
                   * рассылаем его всем устройствам, кот. подписались 
                   * на это
                   */
                  byte[] bufferpc = new byte[msread.Length - lenTitle];
                  Buffer.BlockCopy(msread.ToArray(), lenTitle, bufferpc, 0, bufferpc.Length);

                  if (OnCMDExecuted != null)
                     OnCMDExecuted(bufferpc, GetRecPack);

                  #region для поддержания связи - отсылаем обратно заголовок с нулевой длиной
                  SendTitleWithLen0(-1);
                  timerResetFC.Start();
                  #endregion
                  break;
               default:
                  break;
            }
         }catch(Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 1185, DateTime.Now.ToString() + " : HMI_MT.ClientDataForExchange.cs : AsyncPipeRead :  " + ex.Message);
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, DateTime.Now.ToString() + " (0) : Точка = 8.??? = (ClientDataForExchange.cs::byteQueque_packetAppearance()) ");
            #region для поддержания связи - отсылаем обратно заголовок с нулевой длиной
            SendTitleWithLen0( -1 );
            #endregion
            // инициируем новый сеанс ожидания
            // tmrReRunClient.Start();
            return;
         }
         TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 1190, DateTime.Now.ToString() + " : HMI_MT.ClientDataForExchange.cs : AsyncPipeRead :  начало ожидания следующей порции данных : ");
      }
      /// <summary>
      /// посылка пакета с рез работы команды источнику для разбора
      /// </summary>
      /// <param Name="pack"></param>
      public void GetRecPack(byte[] pack)
      {
         parent.newKB.SendPacketsToFC(pack); 
      }
      /// <summary>
      /// для поддержания связи - отсылаем обратно заголовок с нулевой длиной
      /// </summary>
      private void SendTitleWithLen0(short typep)
      {
         int lenp = 0;
         Buffer.BlockCopy(BitConverter.GetBytes(lenp), 0, lenInDataAsArrByte, 4, 4);
         if (typep != -1)
            Buffer.BlockCopy(BitConverter.GetBytes(typep), 0, lenInDataAsArrByte, 8, 2);

         // формируем заголовок
         Buffer.BlockCopy(arrClientKey, 0, lenInDataAsArrByte, 0, arrClientKey.Length);

         try 
         {
            //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 1, DateTime.Now.ToString() + " (1) : Точка = 8.0 -> 7 = (ClientDataForExchange.cs::SendTitleWithLen0()) ");
            arrForSendData.Add(lenInDataAsArrByte);//arrSend

            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 947, DateTime.Now.ToString() + " : HMI_MT.ClientDataForExchange.cs : SendTitleWithLen0 : отправка поддерживающего пакет типа - " + typep.ToString());
         }catch(Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 950, DateTime.Now.ToString() + " : HMI_MT.ClientDataForExchange.cs : SendTitleWithLen0 :  " + ex.Message);
            return;
         }
      }

      /// <summary>
      /// обработка срабатывания таймера на сброс ФК при отсутсвии связи с DataServer
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      void timerResetFC_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {
         foreach (DataSource aFC in Configurator.KB)
            aFC.isFCConnection = false;
      }
      #endregion
   }
}
