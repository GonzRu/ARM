#define TRACE

/*#############################################################################
 *    Copyright (C) 2006 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: MDI - контейнер                                                   
 *           Главная форма                                                     
 *	Файл                     : MainForm.cs                                      
 *	Тип конечного файла      : Hmi_MT.exe                                       
 *	версия ПО для разработки : С#, Framework 3.5                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 25.03.2007                                       
 *	Дата посл. корр-ровки    : 08.03.2009                                     
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Xml.Linq;

using Egida;

using System.Net.NetworkInformation;
using MessagePanel;
using TraceSourceLib;
using InterfaceLibrary;
using Configuration;
using HMI_MT_Settings;
using DataBaseLib;
using PTKStateLib;
using DebugStatisticLibrary;
using HMI_MT.Properties;
using NormalModeLibrary.Windows;

namespace HMI_MT
{
	/// <summary>
	/// Класс главной формы приложения HMI_MT
	/// </summary>
	public partial class MainForm : Form
   {
        [DllImport( "iphlpapi.dll", ExactSpelling = true )]
        public static extern int SendARP( int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen );

		#region public

		#region Для работы с новым DataServer
        ///// <summary>
        ///// компонент запросов к DS
        ///// </summary>
        //public IRequestEntry reqentry;
        #endregion

		public /*MainMnemo*/Form Form_ez;
		/// <summary>
		/// признак текущей связи с БД
		/// </summary>
		public bool isBDConnection = true;
		/// <summary>
		/// признак текущей связи с БД
		/// </summary>
		public bool isFCConnection = true;
		/// <summary>
		/// признак входа в систему без БД
		/// </summary>
		public bool loginToArmWOBD = false;
		/// <summary>
		/// путь к файлу конфиг. проекта Project.cfg
		/// </summary>
		public string PathToPrjFile = String.Empty;
		/// <summary>
		/// признак записи в БД изменения состояния работы Logger'а
		/// 0 - логгер работает в обычном режиме;
		/// 1 - логгер прекратил работу - производим запись в журналы
		/// 2 - логгер возобновил работу -  производим запись в журналы
		/// </summary>
		public int isWriteMesLoggerAliveToBD = 0;	
		/// <summary>
		/// массив форм для предотвращения повторного открытия
		/// </summary>
		public ArrayList arrFrm = new ArrayList();
		// конфигурация
		//public ArrayList KB;
		//public Configurator newKB;
		/// <summary>
		/// времена и даты в разных форматах
		/// </summary>
		public string[] tsdt;
		/// <summary>
		/// файл журнала
		/// </summary>
		public string strLogFilename;
		/// <summary>
		/// класс для изменения элемента в статусной строке    
		/// </summary>
		public StatusBarLabel StatusBLabel;
    	public SYSTEMTIME systemTime;
		/// <summary>
		/// DataSet для перечня событий пользователя
		/// </summary>
        public DataSet aDS;
		#endregion

		#region Удаленное взаимодействие
		public bool isTCPServer = false;
		public bool isTCPClient = false;
		public static byte[] servbuffer = new byte[4];
		public static byte[] buffer = new byte[2000];
		#endregion

		#region private
	    BackgroundWorker bct;

	    private frmLogs Form_ev;
	    private frmAutorization Form_ea;

		/// <summary>
		/// класс (Sinleton) - строка подключения к БД
		/// </summary>
		SQLConnectionString scs;

        bool bEnter;    // признак первоначального входа на главную форму
	  
		/// <summary>
		/// ссылка на дерево логической конфигурации устройств
		/// </summary>
        TreeViewLogicalConfig tvlc;
        
		StringDictionary strDictionary = new StringDictionary(); // создание словаря для хранения перечня действий пользователя из БД;

		Color oldColor;     // старый цвет для элементов строки статуса состояния связи с БД
		Color oldColorFC;   // старый цвет для элементов строки статуса состояния связи с ФК

	    private Form formClock;
		#endregion

        #region к печати
        //Переменная для хранения текста для печати.
        public PrintHMI prt = new PrintHMI(); // окно с RichTextBox для печати, в него выводится текст для печати и из него он выводиться
        //В нее мы будем помещать текст из RichTextBox
        string stringPrintText;
        //Переменная, определяющая номер страницы, с которой нужно начать печать
        int StartPage;
        //Переменная, определяющая количество страниц для печати:
        int NumPages;
        //Переменная, определяющая номер текущей страницы:
        int PageNumber;
        // шрифт для печати
        Font font = new Font("Arial Narrow", 8);
        #endregion

        #region Конструктор
        public MainForm()
        {
            InitializeComponent();
            try
			{
                this.DoubleBuffered = true;
                CenterToScreen();       // центрирование формы

                // фоновый цвет и вид курсора
                BackColor = Color.LightCyan;
                Cursor = Cursors.Default;//.Hand;
                CheckForIllegalCrossThreadCalls = false;
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
        }
        #endregion

        #region загрузка и активация формы
        private void MainForm_Load(object sender, EventArgs e)
        {
			try
			{
                DebugStatistics.WindowStatistics.AddStatistic( "Запуск главной формы окна." );
               
                Application.DoEvents();

                DebugStatistics.WindowStatistics.AddStatistic( "Сбор путей к файлам конфигурации." );
                // Сформировать имена конфигурац файлов проекта
                SetNamesCfgPrgFiles();
                DebugStatistics.WindowStatistics.AddStatistic( "Сбор путей к файлам конфигурации завершено." );

                DebugStatistics.WindowStatistics.AddStatistic( "Чтение конфигурации." );
                GetAppSettings();
                DebugStatistics.WindowStatistics.AddStatistic( "Чтение конфигурации завершено." );

                DebugStatistics.WindowStatistics.AddStatistic( "Сбор путей к файлам конфигурации." );
                // применить некоторые настройки для данного этапа загрузки системы
                ApplySomeAppSettings();
                DebugStatistics.WindowStatistics.AddStatistic( "Сбор путей к файлам конфигурации завершено." );

                DebugStatistics.WindowStatistics.AddStatistic( "Построение конфигурации." );
                // иницилизировать конфигурацию
                InitConfiguration();
                DebugStatistics.WindowStatistics.AddStatistic( "Построение конфигурации завершено." );

                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация базы данных." );
                // инициализировать настройки на базу данных (из файла Project.cfg)
                InitSettingsToBD();
                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация базы данных завершено." );

                #region к печати
                //Определяем номер страницы, с которой следует начать печать
                printDialog1.PrinterSettings.FromPage = 1;
                //Определяем максимальный номер печатаемой страницы.
                printDialog1.PrinterSettings.ToPage = printDialog1.PrinterSettings.MaximumPage;
                #endregion

                // фоновый поток для отслеживания времени
                bct = new BackgroundWorker();
                bct.DoWork += timerDataTimeUpdateInThread;

                SetTitleMainWindowByPrgName();

                sbMesIE.Text = SetAssemblyVertion();

                CollectInfoAboutMenuToArray();

                SetPositionAndSizeForMainForm();

                bEnter = false;

                oldColor = sbConnectBD.BackColor;
                oldColorFC = sbConnectFC.BackColor;

                //frmSpScr.Close();
                Cursor.Show();

                //DebugStatistics.WindowStatistics.AddStatistic( "Загрузка экранных часов." );
                //// загрузка экранных часов
                //this.RibbonButtonClockClick( sender, e );
                //DebugStatistics.WindowStatistics.AddStatistic( "Загрузка экранных часов завершена." );

                DebugStatistics.WindowStatistics.AddStatistic( "Загрузка параметров нормального режима." );
                // загрузка данных панелей NormalMode
                NormalModeLibrary.ComponentFactory.Factory.LoadXml();
                DebugStatistics.WindowStatistics.AddStatistic( "Загрузка параметров нормального режима завершена." );

                DebugStatistics.WindowStatistics.AddStatistic("Загрузка PanelState.xml");
                PTKState.Iinstance.InitPTKStateInfo();
                DebugStatistics.WindowStatistics.AddStatistic("Загрузка PanelState.xml завершена.");

                // соберем мусор после загрузки
                GC.Collect();

                Application.OpenForms[0].Activate();

                
                scDeviceObjectConfig.Width = Settings.Default.SpeedAccessTreeViewWidth;
                scDeviceObjectConfig.Resize += (s, args) =>
                                                   {
                                                       Settings.Default.SpeedAccessTreeViewWidth = scDeviceObjectConfig.Width;
                                                       Settings.Default.Save();
                                                   };
                 
            }
			catch(Exception ex)
			{
				TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }

        /// <summary>
        /// Сформировать имена конфигурац файлов проекта
        /// </summary>
        private void SetNamesCfgPrgFiles()
        {
        	try
			{
                // проверяем существование файла конфигурации  проекта Project.cfg и файла конфигурации устройств проекта
                PathToPrjFile = AppDomain.CurrentDomain.BaseDirectory + /*Path.DirectorySeparatorChar +*/ "Project" + Path.DirectorySeparatorChar + "Project.cfg";

                if (!File.Exists(PathToPrjFile))
                    throw new Exception("Файл проекта отсутствует: " + PathToPrjFile);

                try
                {
                      PathToPrjFile = AppDomain.CurrentDomain.BaseDirectory + /*Path.DirectorySeparatorChar +*/ "Project" + Path.DirectorySeparatorChar + "Project.cfg";
                      HMI_Settings.PathToPrjFile = PathToPrjFile;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("(423) : MainForm.cs : SetNamesCfgPrgFiles() : Несуществующий файл = {0}", PathToPrjFile));
                }

                // проверяем существование файла конфигурации устройств проекта Configuration.cfg 
                string PathToConfigurationFile = AppDomain.CurrentDomain.BaseDirectory + /*Path.DirectorySeparatorChar +*/ "Project" + Path.DirectorySeparatorChar + "Configuration.cfg";

                if (!File.Exists(PathToConfigurationFile))
                    throw new Exception("Файл конфигурации устройств проекта Configuration.cfg отсутствует: " + PathToConfigurationFile);

                try
                {
                    HMI_Settings.PathToConfigurationFile = PathToConfigurationFile;
                    HMI_Settings.XDoc4PathToConfigurationFile = XDocument.Load(HMI_Settings.PathToConfigurationFile);
                }
                catch
                {
                    throw new Exception(string.Format("(391) : MainForm.cs : SetNamesCfgPrgFiles() : Несуществующий файл = {0}", PathToConfigurationFile));
                }

                // смотрим существование папки для осциллограмм и диаграмм
                CreateFolderForOscAndDiagram();

                // проверяем существование файла с адаптерами для описания состояния ПТК
                string PathToPanelStateFile = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + "PanelState.xml";

                if ( File.Exists( PathToPanelStateFile ) )
                {
                    HMI_Settings.PathPanelState_xml = PathToPanelStateFile;
                    HMI_Settings.XDoc4PathPanelState_xml = XDocument.Load(HMI_Settings.PathPanelState_xml);
                }
			    else
                {
                    //tsmiPanelState.Visible = false;
                    TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 546, DateTime.Now.ToString() +
                        " : (546)MainForm.cs : SetNamesCfgPrgFiles() : Файл описания панели состояния устройств проекта не найден" + PathToPanelStateFile);
                }
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }

        /// <summary>
        /// создать отдельную папку для хранения осциллограмм и диаграмм
        /// </summary>
        private void CreateFolderForOscAndDiagram()
        {
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms");
            }
            catch (Exception ex)
            {
                TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        #region инициализация настроек программы
        /// <summary>
        /// инициализировать статический класс с настройками для приложения
        /// </summary>
        private void GetAppSettings()
        {
            try
			{
                HMI_Settings.Link2MainForm = this;

                // ip-адрес сервера для перезапуска
                var xe_tcp = ( from t in HMI_Settings.XDoc4PathToConfigurationFile.Element( "Project" ).Element( "Configuration" ).Element( "Object" ).Elements( "DSAccessInfo" )
                               where t.Attribute( "enable" ).Value.ToLower() == "true"
                               select t ).Single();

                HMI_Settings.IPADDRES_SERVER = xe_tcp.Element( "CustomiseDriverInfo").Element( "IPAddress" ).Attribute( "value" ).Value;

                HMI_Settings.isRegPass = Convert.ToBoolean(HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IsReqPassword").Value);
                HMI_Settings.isNeedLoginAndPassword = bool.Parse(HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IsNeedLoginAndPassword").Attribute("value").Value);
                HMI_Settings.pathLogEvent_pnl4 = Properties.Settings.Default.PathToLogFile;
                HMI_Settings.sizeLog_pnl4 = Properties.Settings.Default.LogFileMaxSize;
                HMI_Settings.whatToDoLog_pnl4 = Properties.Settings.Default.WhatToDoLogFileMaxSize;
                HMI_Settings.IsShowToolTip = Properties.Settings.Default.IsToolTipShow;
                HMI_Settings.IsToolTipRefDesign = Properties.Settings.Default.IsToolTipRefDesign;
                HMI_Settings.IsShowTabForms = Properties.Settings.Default.IsShowTabForms;
                HMI_Settings.Precision = Properties.Settings.Default.Precision;
                HMI_Settings.LogOnlyDisk = Properties.Settings.Default.LogOnlyDisk;
                HMI_Settings.IPPointForSerializeMesPan = Properties.Settings.Default.IPPointForSerializeMesPan;
                HMI_Settings.PortPointForSerializeMesPan = Properties.Settings.Default.PortPointForSerializeMesPan;
                HMI_Settings.ViewBtn4MainWindow = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("ViewBtn4MainWindow").Value;
                HMI_Settings.HideWindowLineStatus = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("HideWindowLineStatus").Value;
                HMI_Settings.AuraUrl = HMI_Settings.XDoc4PathToPrjFile.Element( "Project" ).Element( "Aura" ).Attribute( "url" ).Value;
                HMI_Settings.DiagnosticSchema = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("DiagnosticSchema").Value;
                HMI_Settings.MainMnenoSchema = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("MainMnenoSchema").Value;

			    var res = HMI_Settings.XDoc4PathToPrjFile.Element( "Project" ).Element( "SchemaTransform" ).Attribute( "x" ).Value;
			    HMI_Settings.SchemaSize.X = float.Parse( res.Replace( '.', ',' ) );
			    res = HMI_Settings.XDoc4PathToPrjFile.Element( "Project" ).Element( "SchemaTransform" ).Attribute( "y" ).Value;
                HMI_Settings.SchemaSize.Y = float.Parse( res.Replace( '.', ',' ) );
			}
			catch(Exception ex)
			{
				TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }
        #endregion

        private void ApplySomeAppSettings() { ControlBox = MaximizeBox = MinimizeBox = Convert.ToBoolean( HMI_Settings.ViewBtn4MainWindow ); }

      /// <summary>
      /// инициализировать конфигурацию
      /// </summary>
	  private void InitConfiguration()
	  {
		  try
		  {
              // инициализируем компонент конфигурации
              ConfigurationFactory cf = new ConfigurationFactory();
              HMI_Settings.CONFIGURATION = cf.CreateConfiguration("OnlyMOACfg", HMI_Settings.PathToConfigurationFile);
              // подписка на потерю связи с DS
              HMI_Settings.CONFIGURATION.OnConfigDSCommunicationLoss4Client += CONFIGURATION_OnConfigDSCommunicationLoss4Client;
              if (HMI_Settings.CONFIGURATION == null)
                throw new Exception(@"(502) : X:\Projects\40_Tumen_GPP09\Client\HMI_MT\MainForm.cs : InitConfiguration() : некорректная конфигурация");
              /*
               * для нового варианта ПТК NewDS<->OldHMIClient
               */
              // формируем список доступных типов блоков архивной информации                
              IEnumerable<XElement> xetypebloks = HMI_Settings.XDoc4PathToConfigurationFile.Element("Project").Element("TypeBlockData").Elements("Type");

              foreach (XElement xetypeblok in xetypebloks)
                  HMI_Settings.CONFIGURATION.SetTypeBlockArchivData(xetypeblok.Attribute("name").Value, xetypeblok.Attribute("value").Value);

			  #region представление иерархии данных в виде дерева
              //tvsd = new TreeViewSourceData();
              //string path2Cfg = AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar + "Project" + System.IO.Path.DirectorySeparatorChar;
              //tvsd.FillTreeView(path2Cfg, Configuration);
              //frame1.Content = tvsd;
              //tvsd.MouseDoubleClick += new MouseButtonEventHandler(tvsd_MouseDoubleClick);
			  #endregion

			  #region иницализация таймера обновления
			  //tmrRenewInfo = new System.Timers.Timer();

			  //tmrRenewInfo.Interval = 100;

			  //tmrRenewInfo.Elapsed += new System.Timers.ElapsedEventHandler(tmrRenewInfo_Elapsed);
			  //tmrRenewInfo.Stop();
			  #endregion
		  }
		  catch (Exception ex)
		  {
			  TraceSourceDiagMes.WriteDiagnosticMSG(ex);
              throw ex;
		  }
	  }
        
        /// <summary>
        /// реакция на потерю связи с DS
        /// </summary>
        /// <param name="state"></param>
      void CONFIGURATION_OnConfigDSCommunicationLoss4Client(bool state)
      {
          ///* 
          // * информация о связи с фк поступает от сервера как пакет типа 8, 
          // * при его обработке устанавливаются признаки в классе фк для верхнего уровня
          // * и мы ими здесь пользуемся для отображения, кроме этого эти признаки используются 
          // * в функции Configurator.ReceivePacketInThread()
          // */

          StringBuilder sbm_noConnection = new StringBuilder();

          // формируем итоговое сообщение в строке статуса о состоянии фк    
          if ( state )
          {
              sbm_noConnection.Append( "Нет связи с Сервером данных " );
              LinkSetTextISB( sbConnectFC, sbm_noConnection.ToString(), Color.Yellow );
              ResetSchemaStateProtocol();
          }
          else
          {
              LinkSetTextISB( sbConnectFC, "Есть связь с Сервером данных", sbMesIE.BackColor ); // для восстановления цвета взяли чужой фон
          }
      }

      /// <summary>
      /// формируем настройки на базу данных (из файла Project.cfg)
      /// </summary>
      private void InitSettingsToBD()
      {
         System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
         scs = SQLConnectionString.Iinstance;

         /* 
          * формируем строку подключения к базе в 
          * зависимости от типа подключения - Windows-идентификация или 
          * SQL-идентификация
          */
         HMI_Settings.ProviderPtkSql = scs.GetConnectStrFromPrjFile(HMI_Settings.XDoc4PathToPrjFile);

         ConnectionStringsSection csSection = config.ConnectionStrings;

         csSection.ConnectionStrings["SqlProviderPTK"].ConnectionString = HMI_Settings.ProviderPtkSql;

         // записываем изменения настроек на базу данных
         config.Save(ConfigurationSaveMode.Modified);

         // подпишемся на изменение состояния связи с БД
         DBConnectionControl dbcc = new DBConnectionControl(HMI_Settings.ProviderPtkSql);
         dbcc.OnBDConnection += new BDConnection(dbcc_OnBDConnection);
         dbcc.SetInterval(5000);
         dbcc.StartControlConnection2BD();
      }

      void dbcc_OnBDConnection(bool state)
      {
          StringBuilder sbm_noConnection = new StringBuilder();

          // формируем итоговое сообщение в строке статуса о состоянии фк    
          if (!state)
          {
              sbm_noConnection.Append("Нет связи с Базой данных ");

              LinkSetTextISB(sbConnectBD, sbm_noConnection.ToString(), Color.Yellow);
          }
          else
              LinkSetTextISB(sbConnectBD, "Есть связь с Базой данных", sbMesIE.BackColor); // для восстановления цвета взяли чужой фон

      }

      /// <summary>
      /// инициализировать название главного окна именем проекта
      /// </summary>
      private void SetTitleMainWindowByPrgName(){
         // открываем файл проекта
         string FILE_NAME = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "Project.cfg";

         CommonUtils.CommonUtils.LoadXml(FILE_NAME);

         XmlTextReader reader = new XmlTextReader(FILE_NAME);
         XmlDocument doc = new XmlDocument();
         doc.Load(reader);
         reader.Close();

         // выделим узел по условию
         XmlNode oldCd;
         XmlElement root = doc.DocumentElement;

         oldCd = root.SelectSingleNode("/Project/NamePTK");
         this.Text = oldCd.InnerText.Trim();

         XDocument xdsb = XDocument.Load(FILE_NAME);

         sbMesIE.Text = xdsb.Element("Project").Element("NamePTKStatusBar").Value;
      }

      /// <summary>
      /// инициализировать строку номером и датой сборки
      /// </summary>
      /// <param Name="strprop"></param>
      private string SetAssemblyVertion()//ref string strprop
      {
         FileInfo fvi = new FileInfo(Application.ExecutablePath);

         return sbMesIE.Text + " (сборка: " + Assembly.GetExecutingAssembly().GetName().Version + " от " + fvi.LastWriteTime.ToShortDateString() + ")";
      }

      /// <summary>
      /// собрать информацию о меню в массив
      /// </summary>
      private void CollectInfoAboutMenuToArray(){
          HMI_MT_Settings.HMI_Settings.alMenu = new ArrayList();
          //HMI_MT_Settings.HMI_Settings.alMenu.Add(menuStrip1);
          //HMI_MT_Settings.HMI_Settings.alMenu.Add(contextMenuStrip1);
      }

      /// <summary>
      /// установить размер и позицию главной формы
      /// </summary>
      private void SetPositionAndSizeForMainForm()
      {
		  this.Width = Screen.PrimaryScreen.WorkingArea.Width;//.Bounds.
		  this.Height = Screen.PrimaryScreen.WorkingArea.Height;//.Bounds

         this.Left = 0;
         this.Top = 0;
      }

      private void MainForm_Shown(object sender, EventArgs e)
      {
         MainFormActivate();
      }

      /// <summary>
      /// Действия при активизации формы
      /// </summary>
      /// <param Name="sender"></param>
      ///
      /// <param Name="e"></param>
      //private void MainForm_Activated( object sender, EventArgs e )
      private void MainFormActivate()
      {
         if( bEnter )
            return;

         bEnter = true;  // значение при запуске

         HMI_MT_Settings.HMI_Settings.CurrentDateTime = DateTime.Now;

         tsdt = HMI_MT_Settings.HMI_Settings.CurrentDateTime.GetDateTimeFormats();

         aDS = new DataSet("ptk"); // инициашизация DataSet

         // проверка соединения с БД при входе в ситсему
            TestAndTryConnectionBD();

		  // инициализация класса GPS для получения инф о состоянии антенны GPS
            //GPS gpsInfo = GPS.Iinstance;
            //gpsInfo.InitGPSInfo( HMI_Settings.PathToPrgDevCFG_cdp,HMI_Settings.PathPanelState_xml, Configurator.KB);
            //gpsInfo.OnChangeGPSActive+=new ChangeGPSActive(gpsInfo_OnChangeGPSActive);
      }

      #region проверка возможности соединения с БД и старт ПТК в нормальном или ограниченном режиме
      /// <summary>
      /// проверить возможность соединения с БД
      /// с возможностью формирования и запоминания 
      /// новой строки соединения
      /// </summary>
      private void TestAndTryConnectionBD()
      {
          if (IsConnectionWithBD())
          {
              StartInNormalMode();
              return;
          }

          switch (MessageBox.Show("Соединение с БД отсутсвует. \nСистема будет запущена в режиме просмотра. \nПродолжить (Ok), Завершить работу (Отмена).", "Нет соединения с БД", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
          {
              case DialogResult.OK:
                  StartWithoutBD();
                  break;
              case DialogResult.Cancel:
                  ExitWithoutAskDialog();
                  break;
              default:
                  break;
          }
      }

      /// <summary>
      /// проверка соединения с БД при входе в систему
      /// </summary>
      /// <returns></returns>
      private bool IsConnectionWithBD()
      {
          DebugStatistics.WindowStatistics.AddStatistic( "Проверка связи с базой данных." );

          // получение строк соединения и поставщика данных из файла *.config
          SqlConnection asqlconnect = new SqlConnection(HMI_Settings.ProviderPtkSql);
          try
          {
              asqlconnect.Open();
          }
          catch (SqlException ex)
          {
              string errorMes = "";
              // интеграция всех возвращаемых ошибок
              foreach (SqlError connectError in ex.Errors)
                  errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ") ";
              MessageBox.Show("Нет связи с БД (при запуске АРМ)" + errorMes, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
             CommonUtils.CommonUtils.WriteEventToLog(21, "Нет связи с БД (при запуске АРМ)" + errorMes, false);//, true, false ); // событие нет связи с БД

              asqlconnect.Close();

              DebugStatistics.WindowStatistics.AddStatistic( "Проверка связи с базой данных завершено." );

              return false;
          }
          catch (Exception ex)
          {
              asqlconnect.Close();

              MessageBox.Show("Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
              CommonUtils.CommonUtils.WriteEventToLog(21, "Нет связи с БД (при запуске АРМ): " + ex.Message, false);

              DebugStatistics.WindowStatistics.AddStatistic( "Проверка связи с базой данных завершено." );

              return false;
          }

          DebugStatistics.WindowStatistics.AddStatistic( "Проверка связи с базой данных завершено." );

          return true;
      }

      /// <summary>
      /// читаем перечень событий пользователя
      /// </summary>
      private void GetUserAction()
      {
          DebugStatistics.WindowStatistics.AddStatistic( "Чтение данных о пользователях." );

          DataBaseReq dbs = new DataBaseReq(HMI_Settings.ProviderPtkSql, "UserAction~Show");

          // запоминаем в StringDictionary
          DataTable dt = new DataTable();
          try
          {
              dt = dbs.GetTableAsResultCMD();
          }
          catch (Exception ex)
          {
              string errMes = ex.Message;
              errMes += "\nЭто исключение возможно в случае когда пользователь базе есть, но права чтения/записи и owner ему не назначены.)\n Приложение будет завершено.";
              MessageBox.Show(errMes, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

              Process.GetCurrentProcess().Kill();
          }

          dbs.CloseConnection();

          for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
              strDictionary.Add((dt.Rows[curRow]["Id"]).ToString(), dt.Rows[curRow]["ActionName"].ToString());

          DebugStatistics.WindowStatistics.AddStatistic( "Чтение данных о пользователях завершено." );
      }

      /// <summary>
      /// Запрос логина для входа в систему
      /// </summary>
      private void GetLogin()
      {
          DebugStatistics.WindowStatistics.AddStatistic( "Подготовка к запросу логин\\пароля." );

          Form_ea = new frmAutorization(this, Target.EnterToSystem);
          Form_ea.TopMost = false;

          if (HMI_Settings.isNeedLoginAndPassword)
          {
              while ((DialogResult = Form_ea.ShowDialog()) != DialogResult.OK)
              {
                  ExitWithAskDialog();
              }
          }
          else
          {
              HMI_MT_Settings.HMI_Settings.UserName = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IsNeedLoginAndPassword").Attribute("nameDefault").Value;
              HMI_MT_Settings.HMI_Settings.UserPassword = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IsNeedLoginAndPassword").Attribute("passDefault").Value;
              Form_ea.DoEnterWithoutPassword(HMI_MT_Settings.HMI_Settings.UserName, HMI_MT_Settings.HMI_Settings.UserPassword);
          }

          NewUserLoged();
      }

      /// <summary>
      /// старт системы в нормальном режиме
      /// </summary>
      private void StartInNormalMode()
      {
          DebugStatistics.WindowStatistics.AddStatistic( "Старт системы." );

          // Установить всем тегам хорошее качество
          //SetGoodQuality4AllTags();

          // читаем перечень событий пользователя
          GetUserAction();

          // запрашиваем логин
          GetLogin();

          // настраиваем меню главной формы
          //CommonUtils.CommonUtils.TestUserMenuRights(menuStrip1, HMI_MT_Settings.HMI_Settings.arrlUserMenu);

          //загружаем профайл пользователя
          //LoadUserProfile();

          // создаем главную мнемосхему
          //AddOwnedForm(frmSpScr);
          //frmSpScr.Show();
          Application.DoEvents();

          DebugStatistics.WindowStatistics.AddStatistic( "Формирование левой панели и физической конфигурации." );
          // формируем левую панель логической и физической конфигурации устройств и проекта
          StartForm();
          DebugStatistics.WindowStatistics.AddStatistic( "Формирование левой панели и физической конфигурации завершено." );

          CreateMainMnemo();

          // событие начала нормальной работы
          CommonUtils.CommonUtils.WriteEventToLog(1, "", true);
      }

      /// <summary>
      /// старт системы в ограниченном режиме
      /// </summary>
      private void StartWithoutBD()
      {
          DebugStatistics.WindowStatistics.AddStatistic( "Старт системы с ограничениями." );

          isBDConnection = false;
          oldColor = sbConnectBD.BackColor;

          LinkSetTextISB(sbConnectBD, "Нет связи с БД", Color.Yellow);
          //LinkSetLV( null, true );    // очищаем ListView для обновления  

          // событие начала работы без БД
          CommonUtils.CommonUtils.WriteEventToLog(1, "Вход в систему без БД",  false);//, true, false );

          loginToArmWOBD = true;	// устанавливаем признак входа в систему без БД

          // права
          HMI_Settings.UserRight = "11111111111111111111111111111111";
          
          //DeviceFormLib.DebugStatistics.WindowStatistics.AddStatistic( "Подготовка к запуску мнемосхемы." );
          // создаем главную мнемосхему
          //CreateMainMnemo(); //Отключено по причине того, что при загрузке без соединения схема мало нужна
      }
      #endregion

      /// <summary>
      /// заполнение панели слева - деревья конфигурации устройств
      /// </summary>
      void StartForm()
      {
          // заполним дерево конфигурации устройств
          TreeViewConfig tvc = new TreeViewConfig(tvDevConfig, null, this);//KB

          // заполним дерево логической конфигурации устройств
          tvlc = new TreeViewLogicalConfig(tvLogicalObjectsConfig, this);
      }
      #endregion

      #region Работа с формой
      /// <summary>
      /// реакция на изменение размеров окна
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
			double dT = Convert.ToDouble(this.Width);
			double scale = dT / 1024;
			dT = scale * 768;
			this.Height = Convert.ToInt32(dT);

			if (this.Width < 1024 | this.Height < 768)
			{
				this.Width = 1024;
				this.Height = 768;
			}
      }

      private void MainForm_MdiChildActivate( object sender, EventArgs e )
      {
         if (!HMI_Settings.IsShowTabForms)
         {
            tabForms.Visible = false;
            return;
         }

         if (this.ActiveMdiChild == null)
            tabForms.Visible = false; // If no any child form, hide tabControl
         else
         {
            this.ActiveMdiChild.WindowState = FormWindowState.Maximized; // Child form always maximized

            // If child form is new and no has tabPage, create new tabPage
            if (this.ActiveMdiChild.Tag == null)
            {
               // Add a tabPage to tabControl with child form caption
               TabPage tp = new TabPage( this.ActiveMdiChild.Text );
               tp.Tag = this.ActiveMdiChild;
               tp.Parent = tabForms;
               tabForms.SelectedTab = tp;

               if ( this.ClientRectangle.Height < ActiveMdiChild.Bounds.Height )
                  ActiveMdiChild.Height = this.ClientRectangle.Height;

               this.ActiveMdiChild.Tag = tp;
               this.ActiveMdiChild.FormClosed += new FormClosedEventHandler( ActiveMdiChild_FormClosed );
            }
            else
            {
               for (int i = 0; i < tabForms.TabPages.Count; i++)
               {
                  TabPage tpp = tabForms.TabPages[i];
                  if (tpp.Tag == this.ActiveMdiChild)
                  {
                     tabForms.SelectedTab = tpp;
                     //tpp.Select();
                     break;
                  }
               }
            }

            if (!tabForms.Visible)
               tabForms.Visible = true;
         }
      }
      // If child form closed, remove tabPage
      private void ActiveMdiChild_FormClosed( object sender, FormClosedEventArgs e )
      {
         ( ( sender as Form ).Tag as TabPage ).Dispose( );
      }

      private void tabForms_SelectedIndexChanged( object sender, EventArgs e )
      {
         //if (( tabForms.SelectedTab != null ) && ( tabForms.SelectedTab.Tag != null ))
         //   ( tabForms.SelectedTab.Tag as Form ).Select( );

         // спрячем/покажем подчиненные окна
         for( int i = 0 ; i < tabForms.TabPages.Count ; i++ )
         {
            TabPage tp = tabForms.TabPages [ i ];
            foreach( Form frowned in ( tp.Tag as Form as Form ).OwnedForms )
               frowned.Hide();
         }

         if( ( tabForms.SelectedTab != null ) && ( tabForms.SelectedTab.Tag != null ) )
         {
            ( tabForms.SelectedTab.Tag as Form ).Select();
            foreach (Form frowned in (tabForms.SelectedTab.Tag as Form).OwnedForms)
            {
                var viewWindow = frowned as ViewWindow;
                if (viewWindow != null)
                    viewWindow.ShowIfNeed();
                else
                    frowned.Show();
            }
         }
      }
      #endregion

      #region пункты главного меню
      // старт формы быстрого доступа
      private void ShowSpeedAccess() 
      {          
          scDeviceObjectConfig.Visible = true;

          Form[] arrF = MdiChildren;
          for (int i = 0; i < arrF.Length; i++)
              if (arrF[i].Name == "SpeedAccess")
              {
                  arrF[i].Focus();
                  return;
              }
          // выводим форму быстрого доступа
          var sa = new SpeedAccess( this ) { MdiParent = this };

          if (tvlc == null)
              tvlc = new TreeViewLogicalConfig(tvLogicalObjectsConfig, this);

          tvlc.OnChangeTabpage += sa.tvlc_OnChangeTabpage;
          sa.Show();
      }

      #region Печать
      private void printDocument1_PrintPage( object sender, System.Drawing.Printing.PrintPageEventArgs e )
      {
         //Создаем экземпляр graph класса Graphics
         Graphics graph = e.Graphics;
         //Создаем объект font, которому устанавливаем 
         // шрифт элемента rtbText
         //Font font = rtbText.Font;
         //Получаем значение межстрочного интервала - высоту шрифта Т1, 134
         float HeightFont = font.GetHeight( graph );
         //Создаем экземпляр stringformat класса StringFormat для определения 
         //дополнительных параметров форматирования текста.
         StringFormat stringformat = new StringFormat( );
         //Создаем экземляры  rectanglefFull и rectanglefText класса RectangleF для 
         //определния областей печати и текста. Т1, 104
         RectangleF rectanglefFull, rectanglefText;
         //Создаем переменные для подсчета числа символов и строк.
         int NumberSymbols, NumberLines;
         //В качестве области печати устанавливаем объект rectanglefFull
         if (graph.VisibleClipBounds.X < 0) rectanglefFull = e.MarginBounds;
         else
            //Определяем   объект  rectanglefFull
            rectanglefFull = new RectangleF(
               //Устанавливаем координату  X  
                e.MarginBounds.Left - ( e.PageBounds.Width - graph.VisibleClipBounds.Width ) / 2,
               //Устанавливаем координату  Y
                e.MarginBounds.Top - ( e.PageBounds.Height - graph.VisibleClipBounds.Height ) / 2,
               //Устанавливаем ширину области
                e.MarginBounds.Width,
               //Устанавливаем высоту области
                e.MarginBounds.Height );
         rectanglefText = RectangleF.Inflate( rectanglefFull, 0, -2 * HeightFont );
         //Определяем число строк
         int NumDisplayLines = ( int )Math.Floor( rectanglefText.Height / HeightFont );
         //Устанавливаем высоту области
         rectanglefText.Height = NumDisplayLines * HeightFont;

         if (prt.rtbText.WordWrap)
         {
            stringformat.Trimming = StringTrimming.Word;
         }
         else
         {
            stringformat.Trimming = StringTrimming.EllipsisCharacter;
            stringformat.FormatFlags |= StringFormatFlags.NoWrap;
         }
         //При печати выбранных страниц переходим к первой стартовой странице
         while (( PageNumber < StartPage ) && ( stringPrintText.Length > 0 ))
         {
            if (prt.rtbText.WordWrap)
               //Измеряем текстовые переменные, 
               //формирующие печать,  и возвращаем число символов NumberSymbols
               //и число строк NumberLines
               graph.MeasureString( stringPrintText, font, rectanglefText.Size, stringformat, out NumberSymbols, out NumberLines );
            else
               NumberSymbols = SymbolsInLines( stringPrintText, NumDisplayLines );
            stringPrintText = stringPrintText.Substring( NumberSymbols );
            //Увеличиваем число страниц 
            PageNumber++;
         }
         //Если длина строки stringPrintText равняется нулю (нет текста для печати),
         // Останавливаем печать
         if (stringPrintText.Length == 0)
         {
            e.Cancel = true;
            return;
         }
         //Выводим (рисуем) текст для печати - stringPrintText, используем для этого шрифт font,
         //кисть черного цвета  - Brushes.Black, область печати - rectanglefText,
         //передаем строку  дополнительного форматирования stringformat
         graph.DrawString( stringPrintText, font, Brushes.Black, rectanglefText, stringformat );
         //Получаем текст для следующей страницы
         if (prt.rtbText.WordWrap)
            graph.MeasureString( stringPrintText, font, rectanglefText.Size, stringformat, out NumberSymbols, out NumberLines );
         else
            NumberSymbols = SymbolsInLines( stringPrintText, NumDisplayLines );
         stringPrintText = stringPrintText.Substring( NumberSymbols );
         //Очищаем объект stringformat, использованный для формирования полей.
         stringformat = new StringFormat( );
         //Добавляем  вывод на каждую страницу ее номер
         stringformat.Alignment = StringAlignment.Far;
         graph.DrawString( "Страница " + PageNumber, font, Brushes.Black, rectanglefFull, stringformat );
         PageNumber++;
         //Cнова проверяем, имеется ли текст для печати и номер страницы, заданной для печати
         e.HasMorePages = ( stringPrintText.Length > 0 ) && ( PageNumber < StartPage + NumPages );
         //Для печати из окна предварительного просмотра  снова инициализируем переменные
         if (!e.HasMorePages)
         {
            stringPrintText = prt.rtbText.Text;
            StartPage = 1;
            NumPages = printDialog1.PrinterSettings.MaximumPage;
            PageNumber = 1;
         }
      }

      int SymbolsInLines( string stringPrintText, int NumLines )
        {
            int index = 0;
            for( int i = 0; i < NumLines; i++ )
            {
                index = 1 + stringPrintText.IndexOf( '\n', index );
                if( index == 0 )
                    return stringPrintText.Length;
            }
            return index;
        }
      #endregion
      #endregion

        #region выход из приложения

        /// <summary>
        /// Show ask dialog for exit
        /// </summary>
        private bool askIsNeedExitProgram()
        {
            if (DialogResult.No == MessageBox.Show("Завершить работу?", "Подтверждение", MessageBoxButtons.YesNo))
                return false;

            return true;
        }

        /// <summary>
        /// Main function for Exit with Ask Dialog
        /// </summary>
        private void ExitWithAskDialog()
        {
            if (askIsNeedExitProgram())
                DoExit();
        }

        /// <summary>
        /// Main function for Exit without Ask Dialog
        /// </summary>
        private void ExitWithoutAskDialog()
        {
            DoExit();
        }

        /// <summary>
        /// пункт меню выход
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
		private void выходToolStripMenuItem_Click(object sender, EventArgs e)
		{
            ExitWithAskDialog();
        }

        /// <summary>
        /// закрытие по каким-либо причинам типа Alt-F4
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitWithAskDialog();
        }

      private void CloseNetManager()
      {
         #region убиваем сетевой манеджер
         // вначале определим что в данном сеансе является поставщиком данных - локальный DataServer или
         // сетевой клиент
         string nameprc = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("SystemDescribe").Attribute("pipeEXEname").Value;

         if (!String.IsNullOrEmpty(nameprc))
         {
            Process[] prmans;

            prmans = Process.GetProcessesByName(nameprc);

            foreach (Process pr in prmans)
               pr.Kill();

            prmans = Process.GetProcessesByName(nameprc + ".vshost");

            foreach (Process pr in prmans)
               pr.Kill();

            Thread.Sleep(2000);
         }
         #endregion
      }

      private void DoExit()
      {
         // сохраняем профайл пользователя
         // готовим таблицы для запоминания профайла пользователя
         // ...

		  try
		  {
			  // закрываем все дочерние окна
			  Form[] arrF = this.MdiChildren;
			  for (int i = 0; i < arrF.Length; i++)
				  arrF[i].Close();

			  // сериализуем и сохраняем DataSet в файле - профайле пользователя
			  //DSProfile.WriteXml(Application.StartupPath + Path.DirectorySeparatorChar + "UserProfile_" + UserName + ".upf", XmlWriteMode.IgnoreSchema);

			  #region закрываем логи трассировки
			  TraceSourceLib.TraceSourceDiagMes.CloseLog();
			  #endregion

			  CloseNetManager();

			  // закрываем экземпляр pipe-клиента
              //HMI_Settings.ClientDFE.Close();
		  }
		  catch (Exception ex)
		  {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
		  }
		  finally 
		  {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 1657, DateTime.Now.ToString() + " : (1657)MainForm.cs : DoExit() : Выход из приложения");
			  TraceSourceLib.TraceSourceDiagMes.FlushLog();

			  // убиваемся об стену
			  Process.GetCurrentProcess().Kill();
		  }
      }
      #endregion

      #region Строка статуса
      #region для потокобезопасного вызова процедуры (статусная строка)
      /*==========================================================================*
			*   private void void LinkSetText(object Value)
			*      для потокобезопасного вызова процедуры
			*==========================================================================*/
        delegate void SetTextCallback( ToolStripStatusLabel sbLabel, string sbLabelText, Color sbItemColor );

        public void LinkSetTextISB( ToolStripStatusLabel sbLabel, string sbLabelText, Color sbItemColor )
        {
            if( statusStrip1.InvokeRequired )
                {
                    SetTextItemSB( sbLabel, sbLabelText, sbItemColor );
                }
                else
                {
                    sbLabel.Text = sbLabelText;
                    sbLabel.BackColor = sbItemColor;
                }
        }

        /*==========================================================================*
        * private void SetText(object Value)
        * //для потокобезопасного вызова процедуры
        *==========================================================================*/
        private void SetTextItemSB( ToolStripStatusLabel sbLabel, string sbLabelText, Color sbItemColor )
        {
           try
           {
              if( statusStrip1.InvokeRequired )
              {
                 SetTextCallback d = new SetTextCallback( SetTextItemSB );
                 this.Invoke( d, new object[] { sbLabel, sbLabelText, sbItemColor } );
              }
              else
              {
                 sbLabel.Text = sbLabelText;
                 sbLabel.BackColor = sbItemColor;
              }
           }
           catch
           {
              return;
           }
        }
        #endregion
      #endregion

      #region Журналы
		/// <summary>
		/// разрешаем MAC-адрес
		/// </summary>
		/// <param Name="ipadr"></param>
		/// <returns></returns>
		  private string GetMACOnIP(IPAddress ipadr)
		  {
			  byte[] ab = new byte[6];
			  int len = ab.Length;
			  int r = SendARP(BitConverter.ToInt32(ipadr.GetAddressBytes(), 0), 0, ab, ref len); //( int ) TempA.Address

			  return BitConverter.ToString(ab, 0, 6);
		  }

      private void просмотрЛокальногоЖурналаToolStripMenuItem_Click( object sender, EventArgs e )
        {
			  frmLogLocal fll = new frmLogLocal();
			  fll.Show();
        }
      #endregion

      #region Конфигурация и ФК
      #region проверка связи с ФК
      private void timerTestFCConnect_Tick( object sender, EventArgs e )
      {
         ///* 
         // * информация о связи с фк поступает от сервера как пакет типа 8, 
         // * при его обработке устанавливаются признаки в классе фк для верхнего уровня
         // * и мы ими здесь пользуемся для отображения, кроме этого эти признаки используются 
         // * в функции Configurator.ReceivePacketInThread()
         // */
         //if ( KB == null )
         //   return;

         //bool noConnect = false;  // общий признак отсутсвия связи с каким-то фк

         //// контролируем связь с ФК, заодно контроллируем связь с БД
         //foreach( DataSource aFC in KB )
         //{
         //   if( aFC.isLostConnection )
         //         noConnect = true;
         //}

         //StringBuilder sbm_noConnection = new StringBuilder( );

         //// формируем итоговое сообщение в строке статуса о состоянии фк    
         //if( noConnect )
         //{
         //   sbm_noConnection.Append( "Нет связи с ФК № " );

         //   foreach( DataSource aFC in KB )
         //      if( aFC.isLostConnection )
         //         sbm_noConnection.Append( aFC.NumFC.ToString() + ';' );

         //   LinkSetTextISB( sbConnectFC, sbm_noConnection.ToString( ), Color.Yellow );
         //}
         //else
         //{
         //   LinkSetTextISB( sbConnectFC, "Есть связь с ФК", toolStripStatusLabelClock.BackColor ); // для восстановления цвета взяли чужой фон
         //}
      }
      #endregion

      #region синхронизация и обновление времени
      /// <summary>
      /// private void timerDataTimeUpdate_Tick()
      ///     тик таймера - обновление времени в статусной строке
      /// </summary>
      private void timerDataTimeUpdate_Tick( object sender, EventArgs e )
      {
         if (!bct.IsBusy)
            bct.RunWorkerAsync( );
      }

      private void timerDataTimeUpdateInThread( object sender, DoWorkEventArgs e )
      {
      //    // пока для Тихвина закомментировал
      //    //if ( HMI_Settings.IsTCPClient )
      //    //{
      //    //    newKB.CRZATimeFC = remoteObj.GetServerTime();


      //    //    try
      //    //    {
      //    //        remoteSecClientObj.SetTime(newKB.CRZATimeFC);

      //    //    }
      //    //    catch (Exception er)

      //    //    {
      //    //        MessageBox.Show(er.Message, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);


      //    //    }
      //    //}

      //   // если нужно синхронизируем время с ФК
      //   if (currCountSynhr >= countSynhr) // количество попыток чтения времени 
      //   {
      //      currCountSynhr = 0;

      //      if (newKB.CRZATimeFC == DateTime.MinValue || newKB.CRZATimeFC.Year == 1970 || !isFCConnection)
      //         return;

      //      systemTime.wYear = ( ushort )newKB.CRZATimeFC.Year;
      //      systemTime.wMonth = ( ushort )newKB.CRZATimeFC.Month;
      //      systemTime.wDayOfWeek = ( ushort )newKB.CRZATimeFC.DayOfWeek;
      //      systemTime.wDay = ( ushort )newKB.CRZATimeFC.Day;
      //      systemTime.wHour = ( ushort )newKB.CRZATimeFC.Hour;
      //      systemTime.wMinute = ( ushort )newKB.CRZATimeFC.Minute;
      //      systemTime.wSecond = ( ushort )newKB.CRZATimeFC.Second;
      //      systemTime.wMilliseconds = ( ushort )newKB.CRZATimeFC.Millisecond;

      //      SetSystemTime( ref systemTime );
      //   }

      //   currCountSynhr++;

      //   // создание текущего формата

      //   if (dtFormat == DataTimeFormat.ShowClock)
      //      panelInfo = DateTime.Now.ToLongTimeString( );
      //   else if (dtFormat == DataTimeFormat.ShowDay)
      //      panelInfo = DateTime.Now.ToShortDateString( );
      //   else if (dtFormat == DataTimeFormat.ShowStorageStat)
      //      panelInfo = newKB.netman.NumPacketsInNetPackQ.ToString( );

      //   // времена и даты в разных форматах
      //   CurrentDateTime = DateTime.Now;	// устанавливаем текущее время компьютера
      //   tsdt = CurrentDateTime.GetDateTimeFormats( );

      //   #region день недели
      //   dayofw.Length = 0;
      //   switch (CurrentDateTime.DayOfWeek.ToString())
      //   {
      //      case "Monday":
      //         dayofw.Append("=понедельник=");
      //         break;
      //      case "Tuesday":
      //         dayofw.Append("=вторник=");
      //         break;
      //      case "Wednesday":
      //         dayofw.Append("=среда=");
      //         break;
      //      case "Thursday":
      //         dayofw.Append("=четверг=");
      //         break;
      //      case "Friday":
      //         dayofw.Append("=пятница=");
      //         break;
      //      case "Saturday":
      //         dayofw.Append("=суббота=");
      //         break;
      //      case "Sunday":
      //         dayofw.Append("=воскресенье=");
      //         break;
      //   }

      //   #endregion

      //   StatusBLabel.ShowStr("1_ShowDay", DateTime.Now.ToShortDateString(), Color.Black);
      //   StatusBLabel.ShowStr("2_ShowDayOfWeek", dayofw.ToString(), Color.Blue);
      //   StatusBLabel.ShowStr("3_ShowClock", DateTime.Now.ToLongTimeString(), Color.Black);
         
      //   panelInfo = (Process.GetCurrentProcess().WorkingSet64 / 1024).ToString() + "/" + (Process.GetCurrentProcess().VirtualMemorySize64 / 1024).ToString();
      //   currentWorkingSet64 = Process.GetCurrentProcess().WorkingSet64;
      //   currentVirtualMemorySize64 = Process.GetCurrentProcess().VirtualMemorySize64;

      //   if (prevWorkingSet64 < currentWorkingSet64 || prevVirtualMemorySize64 < currentVirtualMemorySize64)
      //      StatusBLabel.ShowStr("4_ShowStorageStat", panelInfo, Color.Red);
      //   else
      //      StatusBLabel.ShowStr("4_ShowStorageStat", panelInfo, Color.Black);

      }
      #endregion

      #region периодический опрос нижнего уровня для обновления информации верхнего уровня
      private void timer1_Tick( object sender, EventArgs e )
      {
         //newKB.ReceivePacket( );
      }
      #endregion
      #endregion

      #region удаленное взаимодействие
      /// <summary>
      /// проверить доступность клиента
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      private void tsmiIsClientExist_Click(object sender, EventArgs e)
      {
         Ping ping2Client = new Ping();
         PingOptions pingoptions = new PingOptions();

         pingoptions.DontFragment = true;

         // буфер 32 байта
         string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
         byte[] buffer = Encoding.ASCII.GetBytes(data);
         int timeout = 3000;

         PingReply pr = ping2Client.Send(HMI_Settings.IPADDRES_CLIENT, timeout, buffer, pingoptions);

         if (pr.Status == IPStatus.Success)
            MessageBox.Show("Ping на адрес " + HMI_Settings.IPADDRES_CLIENT + " успешно.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
         else
            MessageBox.Show("Компьютер с адресом " + HMI_Settings.IPADDRES_CLIENT + " недоступен.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      /// <summary>
      /// команда на рестарт сервера
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      private void ReconnectServer()
      {
          Trace.TraceInformation(DateTime.Now.ToString() + " :(3791) MainForm.cs : команда на рестарт сервера : ");
          using (TcpClient client = new TcpClient())
          {
              try
              {
                  client.Connect(IPAddress.Parse(HMI_Settings.IPADDRES_SERVER), 9871);
                  using (NetworkStream ns = client.GetStream())
                  {
                      BinaryWriter bw = new BinaryWriter(ns);
                      BinaryReader br = new BinaryReader(ns);
                      bw.Write("restart");
                      bw.Flush();
                      br.ReadString();
                      Trace.TraceInformation(DateTime.Now.ToString() + " :(3802) MainForm.cs : команда на рестарт сервера : выполнено");
                      //MessageBox.Show("Выполнено.", this.Text, MessageBonnssswwwxButtons.OK, MessageBoxIcon.Information);
                  }
              }
              catch (Exception ex)
              {
                  Trace.TraceInformation(DateTime.Now.ToString() + " :(3861) MainForm.cs : восстановитьСвязьССерверомToolStripMenuItem_Click : " + ex.Message);
                  TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 3854, "Не удалось установить связь с сервером.\nВозможная причины:\n1. Недоступен канал связи.\n2. ПО сервера не работает.");
              }
          }
      }
      #endregion

    #region главная мнемосхема
		/// <summary>
      /// создание главной мнемосхемы арм'а или формы быстрого доступа
      /// </summary>
		private void CreateMainMnemo( )
		{
		    DebugStatistics.WindowStatistics.AddStatistic( "Подготовка и запуск мнемосхемы." );
		    Form_ez = new NewMainMnemo( HMI_Settings.MainMnenoSchema, this, false ) { MdiParent = this };
		    Form_ez.Show( );
		    DebugStatistics.WindowStatistics.AddStatistic( "Подготовка и запуск мнемосхемы выполнен." );
		}
    #endregion   

        #region Private-Metods
        /// <summary>
        /// Сброс протоколов состояния
        /// </summary>
        private void ResetSchemaStateProtocol()
        {
            for ( int i = 0; i < MdiChildren.Length; i++ )
            {
                var win = MdiChildren[i] as IResetStateProtocol;
                if ( win != null )
                    win.ResetProtocol( );
            } 
        }
        #endregion

        #region Ribbon metods
        /// <summary>
        /// Schema
        /// </summary>
        private void RibbonButtonSchemaClick( object sender, EventArgs e )
        {
            Form[] arrF = MdiChildren;
            for ( int i = 0; i < arrF.Length; i++ )
            {
                var main = arrF[i] as NewMainMnemo;
                if ( main != null && !main.OwnerSchema )
                {
                    arrF[i].Focus();
                    return;
                }
            }

            // выводим главную мнемосхему
            CreateMainMnemo( );
        }
        /// <summary>
        /// Fast access panel
        /// </summary>
        private void RibbonButtonFastAccessClick( object sender, EventArgs e )
        {
            ShowSpeedAccess();
        }
        /// <summary>
        /// Open tree
        /// </summary>
        private void RibbonButtonOpenTreeClick( object sender, EventArgs e )
        {
            scDeviceObjectConfig.Visible = true;
        }
        /// <summary>
        /// Close tree
        /// </summary>
        private void RibbonButtonCloseTreeClick( object sender, EventArgs e )
        {
            scDeviceObjectConfig.Visible = false;
        }
        /// <summary>
        /// Diagnostic panel
        /// </summary>
        private void RibbonButtonDiagnosticPanelClick( object sender, EventArgs e )
        {
            if ( !File.Exists( HMI_Settings.PathPanelState_xml ) )
            {
                MessageBox.Show( "Файл с описанием панели состояния устройств PanelState.xml не существует.", "MainForm.cs(667)",
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
                return;
            }

            var diagPanel = new FrmDiagPanel( this ) { MdiParent = this };
            diagPanel.Show( );
        }
        /// <summary>
        /// Current mode
        /// </summary>
        private void RibbonButtonCurrentModeClick( object sender, EventArgs e )
        {
            NormalModeLibrary.ComponentFactory.EditUserWindows(HMI_Settings.UserName);
        }
        /// <summary>
        /// Clock
        /// </summary>
        private void RibbonButtonClockClick( object sender, EventArgs e )
        {
            if ( this.formClock == null || this.formClock.IsDisposed )
            {
                this.formClock = new ClockForm { Owner = this };
                this.formClock.Show();
            }
            else
                this.formClock.Focus();
        }
        /// <summary>
        /// Journals
        /// </summary>
        private void RibbonButtonJournalsClick( object sender, EventArgs e )
        {
            Form[] arrF = this.MdiChildren;
            for ( int i = 0; i < arrF.Length; i++ )
                if ( arrF[i].Name == "frmLogs" )
                {
                    arrF[i].Focus();
                    return;
                }
            // выводим журнал событий устройств
            Form_ev = new frmLogs( this );
            Form_ev.MdiParent = this;
            Form_ev.Show();
            if ( !isBDConnection )
                Form_ev.Close();
        }
        /// <summary>
        /// Administration
        /// </summary>
        private void RibbonButtonAdministrationClick( object sender, EventArgs e )
        {
            ToolStripMenuItem ti = (ToolStripMenuItem)sender;

            if ( !CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b03_Administrate_Users, HMI_MT_Settings.HMI_Settings.UserRight ) )
            {
                foreach ( ToolStripDropDownItem tsddi in ti.DropDownItems )
                {
                    if ( tsddi.Tag != null )
                        if ( (bool)tsddi.Tag )
                        {
                            tsddi.Available = true;
                            tsddi.Visible = true;
                        }
                }
            }
            else
            {
                foreach ( ToolStripDropDownItem tsddi in ti.DropDownItems )
                {

                    if ( tsddi.Available && tsddi.Visible )
                    {
                        tsddi.Tag = true;
                        tsddi.Available = false;
                    }
                    else
                        tsddi.Available = false;
                }
            }
        }
        /// <summary>
        /// Use access
        /// </summary>
        private void RibbonButtonUseAccessClick( object sender, EventArgs e )
        {
            Form[] arrF = this.MdiChildren;
            for ( int i = 0; i < arrF.Length; i++ )
                if ( arrF[i].Name == "frmUserGroupRights" )
                {
                    arrF[i].Focus();
                    return;
                }

            // получение строк соединения и поставщика данных из файла *.config
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            try
            {
                asqlconnect.Open();
            }
            catch ( Exception ex )
            {
                asqlconnect.Close();
                MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );

                return;
            }

            frmUserGroupRights fa = new frmUserGroupRights( this );
            fa.MdiParent = this;
            fa.WindowState = FormWindowState.Maximized;
            fa.Show();
        }
	    /// <summary>
        /// Set PTK clock
        /// </summary>
        private void RibbonButtonSetPTKClockClick( object sender, EventArgs e )
        {
            if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b04_Set_Time, HMI_MT_Settings.HMI_Settings.UserRight ) )
                return;

            if ( HMI_Settings.IsGPSActive )
            {
                MessageBox.Show( "Антенна GPS активна. /nРучная установка времени невозможна.", "Установка времени ПТК", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }

            using ( dlgSetSystemTime dsst = new dlgSetSystemTime( this ) )
            {
                dsst.ShowDialog();
            }
        }
        /// <summary>
        /// Reset all commands
        /// </summary>
        private void RibbonButtonResetAllCommandsClick( object sender, EventArgs e )
        {
            ////проверяем состояние команд
            //foreach (DataSource aFc in KB)
            //   foreach (TCRZADirectDevice tdd in aFc)
            //   {
            //      tdd.StatusReadyForSendCMD = false;
            //      foreach (TCRZACommand tc in tdd.Commands)
            //           tc.StatusActive = false;
            //   }

            //toolStripProgressBar1.Value = 0;
        }

        dlgViewReqestData dvrd;
        /// <summary>
        /// View commands
        /// </summary>
        private void RibbonButtonViewCommandsClick( object sender, EventArgs e )
        {
            switch ( ( sender as ToolStripMenuItem ).Checked.ToString().ToLower() )
            {
                case "true":
                    dvrd = new dlgViewReqestData();
                    dvrd.Show();
                    break;
                case "false":
                    dvrd.Close();
                    break;
            }   
        }
        /// <summary>
        /// Reconnect
        /// </summary>
        private void RibbonButtonReconnectClick( object sender, EventArgs e )
        {
            ReconnectServer();
        }
        /// <summary>
        /// Switch user
        /// </summary>
        private void RibbonMenuButtonSwitchUserClick( object sender, EventArgs e )
        {
            // закрываем все дочерние окна
            Form[] arrF = this.MdiChildren;
            for ( int i = 0; i < arrF.Length; i++ )
                arrF[i].Close();

            // выводим форму авторизации
            Form_ea = new frmAutorization( this, Target.ChangeUser );

            while ( ( DialogResult = Form_ea.ShowDialog() ) != DialogResult.OK )
            {
                ExitWithAskDialog();
            }
            //CommonUtils.CommonUtils.TestUserMenuRights( menuStrip1, HMI_MT_Settings.HMI_Settings.arrlUserMenu );
            // создаем главную мнемосхему
            CreateMainMnemo();

            NewUserLoged();
        }
        /// <summary>
        /// Block system
        /// </summary>
        private void RibbonMenuButtonBlockSystemClick( object sender, EventArgs e )
        {
            // сворачиваем все дочерние окна
            Form[] arrF = this.MdiChildren;
            for ( int i = 0; i < arrF.Length; i++ )
                arrF[i].WindowState = FormWindowState.Minimized;
            // документирование действия пользователя
            CommonUtils.CommonUtils.WriteEventToLog( 15, "Блокировка системы", true );//, true, false );

            // запрашиваем логин
            Form_ea = new frmAutorization( this, Target.BlockSystem );

            while ( ( DialogResult = Form_ea.ShowDialog() ) != DialogResult.OK )
            {
                if ( askIsNeedExitProgram() )
                {
                    ExitWithAskDialog();
                }
            }
            // документирование действия пользователя
            CommonUtils.CommonUtils.WriteEventToLog( 16, "Разблокировка системы", true );//, true, false );

            // разворачиваем все дочерние окна
            arrF = this.MdiChildren;
            for ( int i = 0; i < arrF.Length; i++ )
                arrF[i].WindowState = FormWindowState.Maximized;

            NewUserLoged();
        }
        /// <summary>
        /// User information
        /// </summary>
        private void RibbonMenuButtonUserInformationClick( object sender, EventArgs e )
        {
            MessageBox.Show( "Имя пользователя: \t" + HMI_Settings.UserName + "\n\nГруппа прав: \t\t" +
                HMI_Settings.GroupName, "Информация о текущем пользователе", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }
        /// <summary>
        /// About
        /// </summary>
        private void RibbonMenuButtonAboutClick( object sender, EventArgs e )
        {
            AboutForm af = new AboutForm();
            af.ShowDialog();
        }
        /// <summary>
        /// Exit
        /// </summary>
        private void RibbonMenuButtonExitClick( object sender, EventArgs e )
        {
            ExitWithAskDialog();
        }
        /// <summary>
        /// Print
        /// </summary>
        internal void RibbonMenuButtonPrintClick( object sender, EventArgs e )
        {
            var form = ActiveMdiChild as HelperControlsLibrary.ReportLibrary.IReport;
            if ( form == null )
            {
                MessageBox.Show( this, "Печать данных не поддерживается", "Информация", MessageBoxButtons.OK,
                                 MessageBoxIcon.Information );
                return;
            }

            form.Print( );
        }
        /// <summary>
        /// Page setup
        /// </summary>
        internal void RibbonMenuButtonPageSetupClick( object sender, EventArgs e )
        {
            ////Показываем диалог
            //pageSetupDialog1.ShowDialog();
        }
        /// <summary>
        /// Preview page
        /// </summary>
        internal void RibbonMenuButtonPreviewPageClick( object sender, EventArgs e )
        {
            ////Инициализируем переменные
            //printDocument1.DocumentName = Text;
            //stringPrintText = prt.rtbText.Text;
            //StartPage = 1;
            //NumPages = printDialog1.PrinterSettings.MaximumPage;
            //PageNumber = 1;
            ////Показываем диалог
            //printPreviewDialog1.ShowDialog();
            //prt.rtbText.Clear();
        }
        /// <summary>
        /// Page font
        /// </summary>
        private void RibbonMenuButtonPageFontClick( object sender, EventArgs e )
        {
            //fontDialog1.Font = font;
            //if ( fontDialog1.ShowDialog() == DialogResult.OK )
            //    font = fontDialog1.Font;
        }
        /// <summary>
        /// РАПС Аура
        /// </summary>
        private void RibbonMenuButtonAuraClick( object sender, EventArgs e )
        {
            Process.Start( HMI_Settings.AuraUrl );
        }
        #endregion

        #region NewUserLoged
        private void NewUserLoged()
        {
            InitMessagePanelProvider();

            InitNormalModePanels();
        }
        #endregion

        #region NormalMode
        private void InitNormalModePanels()
        {
            /********************************************************************************************************/
            DebugStatistics.WindowStatistics.AddStatistic("Запуск панелей нормального режима.");
            NormalModeLibrary.ComponentFactory.Factory.ActivatedMainMnemoForms(Form_ez);
            DebugStatistics.WindowStatistics.AddStatistic("Запуск панелей нормального режима завершен.");
            /********************************************************************************************************/
        }
        #endregion

        #region MessagePanelProvider и индикация ведомостей и журналов
        private System.Timers.Timer _journalAlarmTimer = new System.Timers.Timer();
	    private bool _isJournalMenuItemAlarmed = false;

        private void InitMessagePanelProvider()
        {
            HMI_Settings.MessageProvider.StartWork(HMI_Settings.UserID);

            InitJournalMenuItemHandlers();
        }

        private void InitJournalMenuItemHandlers()
        {
            HMI_Settings.MessageProvider.AlarmMessagesAppeared += MessageProviderOnMessagesUpdated;

            _journalAlarmTimer.Interval = 1000;
            _journalAlarmTimer.Elapsed += TimerOnElapsed;

            tabForms.SelectedIndexChanged += TabFormsOnSelectedIndexChanged;
        }

	    private void TabFormsOnSelectedIndexChanged(object sender, EventArgs eventArgs)
	    {
	        TabControl tabControl = sender as TabControl;

            if (tabControl.SelectedTab != null && tabControl.SelectedTab.Text == "Ведомости и журналы")
            {
                StopJournalAlarmTimer();
                _isJournalMenuItemAlarmed = false;
            }
	    }

	    private void MessageProviderOnMessagesUpdated()
        {
            if (!_isJournalMenuItemAlarmed)
            {
                _isJournalMenuItemAlarmed = true;
                StartJournalAlarmTimer();
            }
        }

	    private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (ведомостиИЖурналыToolStripMenuItem.BackColor == Color.Yellow)
                ведомостиИЖурналыToolStripMenuItem.BackColor = SystemColors.Control;
            else
                ведомостиИЖурналыToolStripMenuItem.BackColor = Color.Yellow;
        }

        private void StartJournalAlarmTimer()
        {
           _journalAlarmTimer.Start(); 
        }

        private void StopJournalAlarmTimer()
        {
            _journalAlarmTimer.Stop();
            ведомостиИЖурналыToolStripMenuItem.BackColor = SystemColors.Control;
        }

        
	    #endregion
   }
}
