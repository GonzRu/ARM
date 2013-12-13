#define TRACE

/*#############################################################################
 *    Copyright (C) 2006 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	��������: MDI - ���������                                                   
 *           ������� �����                                                     
 *	����                     : MainForm.cs                                      
 *	��� ��������� �����      : Hmi_MT.exe                                       
 *	������ �� ��� ���������� : �#, Framework 3.5                                
 *	�����������              : ���� �.�.                                        
 *	���� ������ ����������   : 25.03.2007                                       
 *	���� ����. ����-�����    : 08.03.2009                                     
 *	���� (v1.0)              :                                                  
 ******************************************************************************
 * ���������:
 * 1. ����(�����): ...c���������...
 *#############################################################################*/

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Printing;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Media;
using Calculator;
using System.Security.Principal;
using System.Xml;
using System.Management;
using System.Diagnostics;
using System.Reflection;
using System.Xml.XPath;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using System.ServiceModel;
using System.Text.RegularExpressions;

using Egida;
using FileManager;
using LibraryElements; 
using WindowsForms;
using Structure;
using LabelTextbox;
using System.IO.Pipes;
using System.Net.NetworkInformation;
using TraceSourceLib;
using fConnectionString;
using InterfaceLibrary;
using SourceMOA;
using BlockDataComposer;
using ProviderCustomerExchangeLib;
using Configuration;
using HMI_MT_Settings;
using DataBaseLib;
using PTKStateLib;

namespace HMI_MT
{
	/// <summary>
	/// ����� ������� ����� ���������� HMI_MT
	/// </summary>
	public partial class MainForm : Form
   {
        [DllImport( "kernel32.dll" )]
		private extern static uint SetSystemTime( ref SYSTEMTIME lpSystemTime );
        [DllImport( "user32.dll" )]
        public static extern void LockWorkStation();
        [DllImport( "iphlpapi.dll", ExactSpelling = true )]
        public static extern int SendARP( int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen );
		[System.Runtime.InteropServices.DllImport( "kernel32.dll" )]
		extern static short QueryPerformanceCounter( ref long x);
		[System.Runtime.InteropServices.DllImport( "kernel32.dll" )]
		extern static short QueryPerformanceFrequency( ref long x );

		#region public

		#region ��� ������ � ����� DataServer
        ///// <summary>
        ///// ��������� �������� � DS
        ///// </summary>
        //public IRequestEntry reqentry;
        #endregion

		public /*MainMnemo*/Form Form_ez;
		/// <summary>
		/// ������� ������� ����� � ��
		/// </summary>
		public bool isBDConnection = true;
		/// <summary>
		/// ������� ������� ����� � ��
		/// </summary>
		public bool isFCConnection = true;
		/// <summary>
		/// ������� ����� � ������� ��� ��
		/// </summary>
		public bool loginToArmWOBD = false;
		/// <summary>
		/// ���� � ����� ������. ������� Project.cfg
		/// </summary>
		public string PathToPrjFile = String.Empty;
		/// <summary>
		/// ������� ������ � �� ��������� ��������� ������ Logger'�
		/// 0 - ������ �������� � ������� ������;
		/// 1 - ������ ��������� ������ - ���������� ������ � �������
		/// 2 - ������ ���������� ������ -  ���������� ������ � �������
		/// </summary>
		public int isWriteMesLoggerAliveToBD = 0;	
		/// <summary>
		/// ������ ���� ��� �������������� ���������� ��������
		/// </summary>
		public ArrayList arrFrm = new ArrayList();
		// ������������
		//public ArrayList KB;
		//public Configurator newKB;
		/// <summary>
		/// ������� � ���� � ������ ��������
		/// </summary>
		public string[] tsdt;
		/// <summary>
		/// ���� �������
		/// </summary>
		public string strLogFilename;
		/// <summary>
		/// ����� ��� ��������� �������� � ��������� ������    
		/// </summary>
		public StatusBarLabel StatusBLabel;
    	public SYSTEMTIME systemTime;
		/// <summary>
		/// DataSet ��� ������� ������� ������������
		/// </summary>
        public DataSet aDS;
		#endregion

		#region ��������� ��������������
		public bool isTCPServer = false;
		public bool isTCPClient = false;
		public static byte[] servbuffer = new byte[4];
		public static byte[] buffer = new byte[2000];
		#endregion

		#region private
	    BackgroundWorker bct;
		string panelInfo = "";
        // ������� � ���������� �������� ������, ���������� � ������ �������
        long currentWorkingSet64 = 0;
        long currentVirtualMemorySize64 = 0;
        long prevWorkingSet64 = 0;
        long prevVirtualMemorySize64 = 0;
		// ������������� �������
		int countSynhr = 5; // ��� � ������ ������
		int currCountSynhr = 0;
        private frmLogs Form_ev;
		//private frmCustomMesPTK Form_em;
		 private frmAutorization Form_ea;
		/// <summary>
		/// ����� (Sinleton) - ������ ����������� � ��
		/// </summary>
		SQLConnectionString scs;

        bool bEnter;    // ������� ��������������� ����� �� ������� �����
	  
		// ����� ���������� ����/ �������
		DataTimeFormat dtFormat = DataTimeFormat.ShowStorageStat;
		// ��� ��������� ���������� �������
		double StartLong = DateTime.MinValue.Ticks;	//long
		double EndLong = DateTime.MinValue.Ticks;	//long
		/// <summary>
		/// ������ - ���� ������
		/// </summary>
		StringBuilder dayofw = new StringBuilder();
		/// <summary>
		/// ������ �� ������ ���������� ������������ ���������
		/// </summary>
        TreeViewLogicalConfig tvlc;
        
		StringDictionary strDictionary = new StringDictionary(); // �������� ������� ��� �������� ������� �������� ������������ �� ��;

		Color oldColor;     // ������ ���� ��� ��������� ������ ������� ��������� ����� � ��
		Color oldColorFC;   // ������ ���� ��� ��������� ������ ������� ��������� ����� � ��

	    private Form formClock;
		#endregion

        #region � ������
        //���������� ��� �������� ������ ��� ������.
        public PrintHMI prt = new PrintHMI(); // ���� � RichTextBox ��� ������, � ���� ��������� ����� ��� ������ � �� ���� �� ����������
        //� ��� �� ����� �������� ����� �� RichTextBox
        string stringPrintText;
        //����������, ������������ ����� ��������, � ������� ����� ������ ������
        int StartPage;
        //����������, ������������ ���������� ������� ��� ������:
        int NumPages;
        //����������, ������������ ����� ������� ��������:
        int PageNumber;
        // ����� ��� ������
        Font font = new Font("Arial Narrow", 8);
        #endregion

        #region �����������
        public MainForm()
        {
            InitializeComponent();
            try
			{
                this.DoubleBuffered = true;
                CenterToScreen();       // ������������� �����

                // ������� ���� � ��� �������
                BackColor = Color.LightCyan;
                Cursor = Cursors.Default;//.Hand;
                CheckForIllegalCrossThreadCalls = false;
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }
        #endregion

        #region �������� � ��������� �����
        private void MainForm_Load(object sender, EventArgs e)
        {		 
			try
			{
                //frmSplashScreen frmSpScr = new frmSplashScreen();
                //Cursor.Hide();
                //AddOwnedForm(frmSpScr);                
                //frmSpScr.Show();
                
                Application.DoEvents();
                //string dat = CRZADevices.CommonCRZADeviceFunction.GetTimeInMTRACustomFormat(DateTime.Now);

                // ������������ ����� ���������� ������ �������
                SetNamesCfgPrgFiles();

                GetAppSettings();

                // ��������� ��������� ��������� ��� ������� ����� �������� �������
                ApplySomeAppSettings();

                // ������� ������ ������ ��� ��������� ������ ����� �� ������ � ��������� ������
                //StatusBLabel = new StatusBarLabel(this, statusStrip1, toolStripStatusLabelClock);

                // ��������������� ������������
                InitConfiguration();

                // ���������������� ��������� �� ���� ������ (�� ����� Project.cfg)
                InitSettingsToBD();

                //  ���������� �������� ������� ���������� ���������� � ��������� ����� � �� � ��������� ���
                //SetAndStartTestFCConnect();

                //  ���������� �������� ������� ���������� ���������� � ����� � ��������� ���
                SetAndStartDataReNew();

                //timerDataTimeUpdate.Start();

                #region � ������
                //���������� ����� ��������, � ������� ������� ������ ������
                printDialog1.PrinterSettings.FromPage = 1;
                //���������� ������������ ����� ���������� ��������.
                printDialog1.PrinterSettings.ToPage = printDialog1.PrinterSettings.MaximumPage;
                #endregion

                // ������� ����� ��� ������������ �������
                bct = new BackgroundWorker();
                bct.DoWork += new DoWorkEventHandler(timerDataTimeUpdateInThread);

                SetTitleMainWindowByPrgName();

                sbMesIE.Text = SetAssemblyVertion();

                CollectInfoAboutMenuToArray();

                SetPositionAndSizeForMainForm();

                bEnter = false;

                oldColor = sbConnectBD.BackColor;
                oldColorFC = sbConnectFC.BackColor;

                //frmSpScr.Close();
                Cursor.Show();

                this.RibbonButtonClockClick( sender, e );

                // ������� ����� ����� ��������
                GC.Collect();
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }

        /// <summary>
        /// ������������ ����� ���������� ������ �������
        /// </summary>
        private void SetNamesCfgPrgFiles()
        {
        	try
			{
                // ��������� ������������� ����� ������������  ������� Project.cfg � ����� ������������ ��������� �������
                PathToPrjFile = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "Project.cfg";

                if (!File.Exists(PathToPrjFile))
                    throw new Exception("���� ������� �����������: " + PathToPrjFile);

                try
                {
                      PathToPrjFile = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "Project.cfg";
                      HMI_Settings.PathToPrjFile = PathToPrjFile;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("(423) : MainForm.cs : SetNamesCfgPrgFiles() : �������������� ���� = {0}", PathToPrjFile));
                }

                // ��������� ������������� ����� ������������ ��������� ������� Configuration.cfg 
                string PathToConfigurationFile = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "Configuration.cfg";

                if (!File.Exists(PathToConfigurationFile))
                    throw new Exception("���� ������������ ��������� ������� Configuration.cfg �����������: " + PathToConfigurationFile);

                try
                {
                    HMI_Settings.PathToConfigurationFile = PathToConfigurationFile;
                    HMI_Settings.XDoc4PathToConfigurationFile = XDocument.Load(HMI_Settings.PathToConfigurationFile);
                }
                catch
                {
                    throw new Exception(string.Format("(391) : MainForm.cs : SetNamesCfgPrgFiles() : �������������� ���� = {0}", PathToConfigurationFile));
                }

                // ������� ������������� ����� ��� ������������ � ��������
                CreateFolderForOscAndDiagram();

                // ��������� ������������� ����� � ���������� ��� �������� ��������� ���
                string PathToPanelStateFile = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "PanelState.xml";

                if ( File.Exists( PathToPanelStateFile ) )
                {
                    HMI_Settings.PathPanelState_xml = PathToPanelStateFile;
                    HMI_Settings.XDoc4PathPanelState_xml = XDocument.Load(HMI_Settings.PathPanelState_xml);
                }
			    else
                {
                    //tsmiPanelState.Visible = false;
                    TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 546, DateTime.Now.ToString() +
                        " : (546)MainForm.cs : SetNamesCfgPrgFiles() : ���� �������� ������ ��������� ��������� ������� �� ������" + PathToPanelStateFile);
                }
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }

        /// <summary>
        /// ������� ��������� ����� ��� �������� ������������ � ��������
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
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        #region ������������� �������� ���������
        /// <summary>
        /// ���������������� ����������� ����� � ����������� ��� ����������
        /// </summary>
        private void GetAppSettings()
        {
            try
			{
                // ip-����� ������� ��� �����������
                var xe_tcp = ( from t in HMI_MT_Settings.HMI_Settings.XDoc4PathToConfigurationFile.Element( "Project" ).Element( "Configuration" ).Element( "Object" ).Elements( "DSAccessInfo" )
                               where t.Attribute( "enable" ).Value.ToLower() == "true"
                               select t ).Single();

                HMI_Settings.IPADDRES_SERVER = xe_tcp.Element( "CustomiseDriverInfo").Element( "IPAddress" ).Attribute( "value" ).Value;

                HMI_Settings.isRegPass = Convert.ToBoolean(HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IsReqPassword").Value);
                HMI_Settings.isNeedLoginAndPassword = bool.Parse(HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IsNeedLoginAndPassword").Attribute("value").Value);
                HMI_Settings.pathLogEvent_pnl4 = HMI_MT.Properties.Settings.Default.PathToLogFile;
                HMI_Settings.sizeLog_pnl4 = HMI_MT.Properties.Settings.Default.LogFileMaxSize;
                HMI_Settings.whatToDoLog_pnl4 = HMI_MT.Properties.Settings.Default.WhatToDoLogFileMaxSize;
                HMI_Settings.IsShowToolTip = HMI_MT.Properties.Settings.Default.IsToolTipShow;
                HMI_Settings.IsToolTipRefDesign = HMI_MT.Properties.Settings.Default.IsToolTipRefDesign;
                HMI_Settings.IsShowTabForms = HMI_MT.Properties.Settings.Default.IsShowTabForms;
                HMI_Settings.Precision = HMI_MT.Properties.Settings.Default.Precision;
                HMI_Settings.LogOnlyDisk = HMI_MT.Properties.Settings.Default.LogOnlyDisk;
                HMI_Settings.IPPointForSerializeMesPan = HMI_MT.Properties.Settings.Default.IPPointForSerializeMesPan;
                HMI_Settings.PortPointForSerializeMesPan = HMI_MT.Properties.Settings.Default.PortPointForSerializeMesPan;
                HMI_Settings.ViewBtn4MainWindow = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("ViewBtn4MainWindow").Value;
                HMI_Settings.HideWindowLineStatus = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("HideWindowLineStatus").Value;
                HMI_Settings.Link2MainForm = this;
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }
        #endregion

        private void ApplySomeAppSettings()
        {
            this.ControlBox = this.MaximizeBox = this.MinimizeBox = Convert.ToBoolean(HMI_Settings.ViewBtn4MainWindow);
        }

        /// <summary>
      /// ���������������� ������������
      /// </summary>
	  private void InitConfiguration()
	  {
		  try
		  {
              // �������������� ��������� ������������
              ConfigurationFactory cf = new ConfigurationFactory();
              HMI_Settings.CONFIGURATION = cf.CreateConfiguration("OnlyMOACfg", HMI_Settings.PathToConfigurationFile);
              // �������� �� ������ ����� � DS
              HMI_Settings.CONFIGURATION.OnConfigDSCommunicationLoss4Client += new ConfigDSCommunicationLoss4Client(CONFIGURATION_OnConfigDSCommunicationLoss4Client);
              if (HMI_Settings.CONFIGURATION == null)
                throw new Exception(@"(502) : X:\Projects\40_Tumen_GPP09\Client\HMI_MT\MainForm.cs : InitConfiguration() : ������������ ������������");
              /*
               * ��� ������ �������� ��� NewDS<->OldHMIClient
               */
              // ��������� ������ ��������� ����� ������ �������� ����������                
              IEnumerable<XElement> xetypebloks = HMI_Settings.XDoc4PathToConfigurationFile.Element("Project").Element("TypeBlockData").Elements("Type");

              foreach (XElement xetypeblok in xetypebloks)
                  HMI_Settings.CONFIGURATION.SetTypeBlockArchivData(xetypeblok.Attribute("name").Value, xetypeblok.Attribute("value").Value);

			  #region ������������� �������� ������ � ���� ������
              //tvsd = new TreeViewSourceData();
              //string path2Cfg = AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar + "Project" + System.IO.Path.DirectorySeparatorChar;
              //tvsd.FillTreeView(path2Cfg, Configuration);
              //frame1.Content = tvsd;
              //tvsd.MouseDoubleClick += new MouseButtonEventHandler(tvsd_MouseDoubleClick);
			  #endregion

			  #region ������������ ������� ����������
			  //tmrRenewInfo = new System.Timers.Timer();

			  //tmrRenewInfo.Interval = 100;

			  //tmrRenewInfo.Elapsed += new System.Timers.ElapsedEventHandler(tmrRenewInfo_Elapsed);
			  //tmrRenewInfo.Stop();
			  #endregion
		  }
		  catch (Exception ex)
		  {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
              throw ex;
		  }
	  }
        
        /// <summary>
        /// ������� �� ������ ����� � DS
        /// </summary>
        /// <param name="state"></param>
      void CONFIGURATION_OnConfigDSCommunicationLoss4Client(bool state)
      {
          ///* 
          // * ���������� � ����� � �� ��������� �� ������� ��� ����� ���� 8, 
          // * ��� ��� ��������� ��������������� �������� � ������ �� ��� �������� ������
          // * � �� ��� ����� ���������� ��� �����������, ����� ����� ��� �������� ������������ 
          // * � ������� Configurator.ReceivePacketInThread()
          // */

          StringBuilder sbm_noConnection = new StringBuilder();

          // ��������� �������� ��������� � ������ ������� � ��������� ��    
          if (state)
          {
              sbm_noConnection.Append("��� ����� � �������� ������ ");

              LinkSetTextISB(sbConnectFC, sbm_noConnection.ToString(), Color.Yellow);
          }
          else
              LinkSetTextISB(sbConnectFC, "���� ����� � �������� ������", sbMesIE.BackColor); // ��� �������������� ����� ����� ����� ���
      }

      /// <summary>
      /// ��������� ��������� �� ���� ������ (�� ����� Project.cfg)
      /// </summary>
      private void InitSettingsToBD()
      {
         System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

         int connStrCnt = ConfigurationManager.ConnectionStrings.Count;

         scs = SQLConnectionString.Iinstance;

         /* 
          * ��������� ������ ����������� � ���� � 
          * ����������� �� ���� ����������� - Windows-������������� ��� 
          * SQL-�������������
          */
         HMI_Settings.ProviderPtkSql = scs.GetConnectStrFromPrjFile(HMI_Settings.XDoc4PathToPrjFile);

         ConnectionStringsSection csSection = config.ConnectionStrings;

         csSection.ConnectionStrings["SqlProviderPTK"].ConnectionString = HMI_Settings.ProviderPtkSql;

         // ���������� ��������� �������� �� ���� ������
         config.Save(ConfigurationSaveMode.Modified);

         // ���������� �� ��������� ��������� ����� � ��
         DBConnectionControl dbcc = new DBConnectionControl(HMI_Settings.ProviderPtkSql);
         dbcc.OnBDConnection += new BDConnection(dbcc_OnBDConnection);
         dbcc.SetInterval(30000);
         dbcc.StartControlConnection2BD();
      }

      void dbcc_OnBDConnection(bool state)
      {
          StringBuilder sbm_noConnection = new StringBuilder();

          // ��������� �������� ��������� � ������ ������� � ��������� ��    
          if (!state)
          {
              sbm_noConnection.Append("��� ����� � ����� ������ ");

              LinkSetTextISB(sbConnectBD, sbm_noConnection.ToString(), Color.Yellow);
          }
          else
              LinkSetTextISB(sbConnectBD, "���� ����� � ����� ������", sbMesIE.BackColor); // ��� �������������� ����� ����� ����� ���

      }

      /// <summary>
      /// ���������� �������� ������� ���������� ���������� � ��������� ����� � ��
      /// � �������� ���
      /// </summary>
      private void SetAndStartTestFCConnect()
      {
         //string strIntervalMesFCConnect = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IntervalMesFCConnect").Value;
         //int iIntervalMesFCConnect;

         //if (!Int32.TryParse(strIntervalMesFCConnect, out iIntervalMesFCConnect))
         //   iIntervalMesFCConnect = 10000;

         //timerTestFCConnect.Stop();
         //timerTestFCConnect.Interval = iIntervalMesFCConnect;
         //timerTestFCConnect.Start();
      }

      /// <summary>
      /// ���������� � ��������� �������� ������� ���������� �����
      /// </summary>
      private void SetAndStartDataReNew()
      {
         //string strIntervalDataReNew = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IntervalDataReNew").Value;
         //int iIntervalDataReNew;

         //if (!Int32.TryParse(strIntervalDataReNew, out iIntervalDataReNew))
         //    iIntervalDataReNew = 2000;

        // timer1 ��� �� ������� ����� - � ��� ������
        //timer1.Interval = iIntervalDataReNew;
        //timer1.Start();
   }

	  ///// <summary>
	  /////����� �������� ����������� ����������
	  ///// </summary>
	  //private void StartTrace()
	  //{
	  //   TraceSourceLib.TraceSourceDiagMes.CreateLog("HMI_MT_Client_TraceSource");
	  //   TraceSourceLib.TraceSourceDiagMes.tracesource.TraceData(TraceEventType.Verbose, 1, "StartTest");
	  //   TraceSourceLib.TraceSourceDiagMes.tracesource.Flush();
	  //}

      /// <summary>
      /// ���������������� �������� �������� ���� ������ �������
      /// </summary>
      private void SetTitleMainWindowByPrgName(){
         // ��������� ���� �������
         string FILE_NAME = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "Project.cfg";

         CommonUtils.CommonUtils.LoadXml(FILE_NAME);

         XmlTextReader reader = new XmlTextReader(FILE_NAME);
         XmlDocument doc = new XmlDocument();
         doc.Load(reader);
         reader.Close();

         // ������� ���� �� �������
         XmlNode oldCd;
         XmlElement root = doc.DocumentElement;

         oldCd = root.SelectSingleNode("/Project/NamePTK");
         this.Text = oldCd.InnerText.Trim();

         XDocument xdsb = XDocument.Load(FILE_NAME);

         sbMesIE.Text = xdsb.Element("Project").Element("NamePTKStatusBar").Value;
      }

      /// <summary>
      /// ���������������� ������ ������� � ����� ������
      /// </summary>
      /// <param Name="strprop"></param>
      private string SetAssemblyVertion()//ref string strprop
      {
         FileInfo fvi = new FileInfo(Application.ExecutablePath);

         return sbMesIE.Text + " (������: " + Assembly.GetExecutingAssembly().GetName().Version + " �� " + fvi.LastWriteTime.ToShortDateString() + ")";
      }

      /// <summary>
      /// ������� ���������� � ���� � ������
      /// </summary>
      private void CollectInfoAboutMenuToArray(){
          HMI_MT_Settings.HMI_Settings.alMenu = new ArrayList();
          //HMI_MT_Settings.HMI_Settings.alMenu.Add(menuStrip1);
          HMI_MT_Settings.HMI_Settings.alMenu.Add(contextMenuStrip1);
      }

      /// <summary>
      /// ���������� ������ � ������� ������� �����
      /// </summary>
      private void SetPositionAndSizeForMainForm()
      {
		  this.Width = Screen.PrimaryScreen.WorkingArea.Width;//.Bounds.
		  this.Height = Screen.PrimaryScreen.WorkingArea.Height;//.Bounds

         this.Left = 0;
         this.Top = 0;
      }

      /// <summary>
      /// � ���� ������ ���������, ������� ��� ����������� ����� ���������� �� �������, �������� ������ �������
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      void DoServerRestart_DoWork(object sender, DoWorkEventArgs e)
      {
         TcpListener listener = new TcpListener(IPAddress.Any, 9871);
         try
         {
            listener.Start();
            using (TcpClient tcpc = listener.AcceptTcpClient())
            using (NetworkStream ns = tcpc.GetStream())
            {
               string br = new BinaryReader(ns).ReadString();
               BinaryWriter bw = new BinaryWriter(ns);
               bw.Write(br);
            }
         }
         catch (Exception ex)
         {
            System.Diagnostics.Trace.TraceInformation(DateTime.Now.ToString() + " :(715) MainForm.cs: DoServerRestart_DoWork : " + ex.Message);
         }

         // ��������� ������

         #region ������� MTRANetGate
           // ���� ���� �������� ������ MTRANetGate.exe, ��������� ��
           Process[] prmtras = Process.GetProcessesByName("MTRANetGate");//.vshost

           foreach (Process pr in prmtras)
              pr.Kill();

           Thread.Sleep(2000);
           #endregion

         System.Diagnostics.Trace.TraceInformation(DateTime.Now.ToString() + " :(718) MainForm.cs: DoServerRestart_DoWork : ���������� ������ �� ���������� �������");
         Process.GetCurrentProcess().Kill();
      }

      private void MainForm_Shown(object sender, EventArgs e)
      {
         MainFormActivate();
      }

      /// <summary>
      /// �������� ��� ����������� �����
      /// </summary>
      /// <param Name="sender"></param>
      ///
      /// <param Name="e"></param>
      //private void MainForm_Activated( object sender, EventArgs e )
      private void MainFormActivate()
      {
         if( bEnter )
            return;

         bEnter = true;  // �������� ��� �������

         HMI_MT_Settings.HMI_Settings.CurrentDateTime = DateTime.Now;

         tsdt = HMI_MT_Settings.HMI_Settings.CurrentDateTime.GetDateTimeFormats();

         aDS = new DataSet("ptk"); // ������������� DataSet

         // �������� ���������� � �� ��� ����� � �������
            TestAndTryConnectionBD();

		  // ������������� ������ GPS ��� ��������� ��� � ��������� ������� GPS
            //GPS gpsInfo = GPS.Iinstance;
            //gpsInfo.InitGPSInfo( HMI_Settings.PathToPrgDevCFG_cdp,HMI_Settings.PathPanelState_xml, Configurator.KB);
            //gpsInfo.OnChangeGPSActive+=new ChangeGPSActive(gpsInfo_OnChangeGPSActive);

		  // ������������� ������ PTKState, ����������� ��������� ��������� ���
            PTKState PtkState = PTKState.Iinstance;
            PtkState.InitPTKStateInfo();
      }

      #region �������� ����������� ���������� � �� � ����� ��� � ���������� ��� ������������ ������
      /// <summary>
      /// ��������� ����������� ���������� � ��
      /// � ������������ ������������ � ����������� 
      /// ����� ������ ����������
      /// </summary>
      private void TestAndTryConnectionBD()
      {
          if (IsConnectionWithBD())
          {
              StartInNormalMode();
              return;
          }

          switch (MessageBox.Show("���������� � �� ����������. \n������� ����� �������� � ������ ���������. \n���������� (Ok), ��������� ������ (������).", "��� ���������� � ��", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1))
          {
              case DialogResult.OK:
                  StartWithoutBD();
                  break;
              case DialogResult.Cancel:
                  isFirsQuestionAboutExit = true;
                  PrepareFormClosing();
                  break;
              default:
                  break;
          }
      }

      /// <summary>
      /// �������� ���������� � �� ��� ����� � �������
      /// </summary>
      /// <returns></returns>
      private bool IsConnectionWithBD()
      {
          // ��������� ����� ���������� � ���������� ������ �� ����� *.config
          SqlConnection asqlconnect = new SqlConnection(HMI_Settings.ProviderPtkSql);
          try
          {
              asqlconnect.Open();
          }
          catch (SqlException ex)
          {
              string errorMes = "";
              // ���������� ���� ������������ ������
              foreach (SqlError connectError in ex.Errors)
                  errorMes += connectError.Message + " (������: " + connectError.Number.ToString() + ") ";
              MessageBox.Show("��� ����� � �� (��� ������� ���)" + errorMes, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
             CommonUtils.CommonUtils.WriteEventToLog(21, "��� ����� � �� (��� ������� ���)" + errorMes, false);//, true, false ); // ������� ��� ����� � ��

              asqlconnect.Close();

              return false;
          }
          catch (Exception ex)
          {
              asqlconnect.Close();

              MessageBox.Show("��� ����� � ��������" + Environment.NewLine + ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Information);
              CommonUtils.CommonUtils.WriteEventToLog(21, "��� ����� � �� (��� ������� ���): " + ex.Message, false);

              return false;
          }
          return true;
      }

      /// <summary>
      /// ������ �������� ������� ������������
      /// </summary>
      private void GetUserAction()
      {
          DataBaseReq dbs = new DataBaseReq(HMI_Settings.ProviderPtkSql, "UserAction~Show");

          // ���������� � StringDictionary
          DataTable dt = new DataTable();
          try
          {
              dt = dbs.GetTableAsResultCMD();
          }
          catch (Exception ex)
          {
              string errMes = ex.Message;
              errMes += "\n��� ���������� �������� � ������ ����� ������������ ���� ����, �� ����� ������/������ � owner ��� �� ���������.)\n ���������� ����� ���������.";
              MessageBox.Show(errMes, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

              Process.GetCurrentProcess().Kill();
          }

          dbs.CloseConnection();

          for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
              strDictionary.Add((dt.Rows[curRow]["Id"]).ToString(), dt.Rows[curRow]["ActionName"].ToString());
      }

      /// <summary>
      /// ������ ������ ��� ����� � �������
      /// </summary>
      private void GetLogin()
      {
          Form_ea = new frmAutorization(this, Target.EnterToSystem);

          if (HMI_Settings.isNeedLoginAndPassword)
          {
              if ((DialogResult = Form_ea.ShowDialog()) != DialogResult.OK)
              {
                  this.Close();   // ������� �� ����������
                  return;
              }
          }
          else
          {
              HMI_MT_Settings.HMI_Settings.UserName = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IsNeedLoginAndPassword").Attribute("nameDefault").Value;
              HMI_MT_Settings.HMI_Settings.UserPassword = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IsNeedLoginAndPassword").Attribute("passDefault").Value;
              Form_ea.DoEnterWithoutPassword(HMI_MT_Settings.HMI_Settings.UserName, HMI_MT_Settings.HMI_Settings.UserPassword);
          }
      }

      /// <summary>
      /// �������� �������� ������������
      /// </summary>
      private void LoadUserProfile()
      {
          //��������� ������� ������������ - ������� ���� �� �������������� ���� UserProfile_���_������������.upf
          if (File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "UserProfile_" + HMI_MT_Settings.HMI_Settings.UserName + ".upf"))
          {
              // ��������� ������� ������������
              //DSProfile.ReadXml(Application.StartupPath + Path.DirectorySeparatorChar + "UserProfile_" + UserName + ".upf");
              // ������ ����� ��������� ������ ������� �� DataSet, ��������
              //DTPribors = DSProfile.Tables[ "Pribors" ];
          }
      }

      /// <summary>
      /// ����� ������� � ���������� ������
      /// </summary>
      private void StartInNormalMode()
      {
          // ���������� ���� ����� ������� ��������
          //SetGoodQuality4AllTags();

          // ������ �������� ������� ������������
          GetUserAction();

          // ����������� �����
          GetLogin();

          // ����������� ���� ������� �����
          //CommonUtils.CommonUtils.TestUserMenuRights(menuStrip1, HMI_MT_Settings.HMI_Settings.arrlUserMenu);

          //��������� ������� ������������
          LoadUserProfile();

          // ������� ������� ����������
          //AddOwnedForm(frmSpScr);
          //frmSpScr.Show();
          Application.DoEvents();

          // ��������� ����� ������ ���������� � ���������� ������������ ��������� � �������
          StartForm();

          // ������� ����� � ��������� ��� ������ �������� �� ���������� ���������
          //HMI_Settings.ClientDFE = ClientDataForExchange.Iinstance;
          //HMI_Settings.ClientDFE.Iniit(this, HMI_Settings.PipeName);

          //!!!!!!//CreateMainMnemo();


          //SetGoodQuality4AllTags();


          // ������� ������ ���������� ������
          CommonUtils.CommonUtils.WriteEventToLog(1, "", true);//, true, false );

          //// ���� ���������� ����� ��������� ��������
          //Thread.Sleep(7000);
          //frmSpScr.Close();
      }

      /// <summary>
      /// ����� ������� � ������������ ������
      /// </summary>
      private void StartWithoutBD()
      {
          isBDConnection = false;
          oldColor = sbConnectBD.BackColor;

          LinkSetTextISB(sbConnectBD, "��� ����� � ��", Color.Yellow);
          //LinkSetLV( null, true );    // ������� ListView ��� ����������  

          // ������� ������ ������ ��� ��
          CommonUtils.CommonUtils.WriteEventToLog(1, "���� � ������� ��� ��",  false);//, true, false );

          loginToArmWOBD = true;	// ������������� ������� ����� � ������� ��� ��

          // �����
          HMI_MT_Settings.HMI_Settings.UserRight = "11111111111111111111111111111111";

          // ������� ������� ����������
          CreateMainMnemo();
      }
      #endregion
	  /// <summary>
	  /// ��������� ����������� �������� ��������� GPS
	  /// </summary>
	  /// <param name="isgpsactive"></param>
	  void gpsInfo_OnChangeGPSActive(bool isgpsactive, byte codeActive, string strDescribeActiveCode)
	  {
		  HMI_Settings.IsGPSActive = isgpsactive;
		  HMI_Settings.GPSActiveCode = codeActive;
		  HMI_Settings.GPSActiveCodeMessage = strDescribeActiveCode;
	  }

      /// <summary>
      /// ��������� ���� ����� �������� �������� (� ��������� ������)
      /// </summary>
       private void SetGoodQuality4AllTags()
       {
          //BackgroundWorker bgwtags = new BackgroundWorker();
          //bgwtags.DoWork += new DoWorkEventHandler(bgwtags_DoWork);
          //bgwtags.RunWorkerAsync();
       }

      /// <summary>
      /// ���������� ������ ����� - ������� ������������ ���������
      /// </summary>
      void StartForm()
      {
          // �������� ������ ������������ ���������
          TreeViewConfig tvc = new TreeViewConfig(tvDevConfig, null, this);//KB

          // �������� ������ ���������� ������������ ���������
          tvlc = new TreeViewLogicalConfig(tvLogicalObjectsConfig, this);
      }

      void bgwtags_DoWork(object sender, DoWorkEventArgs e)
      {
         //foreach (DataSource aFc in KB)
         //   foreach (TCRZADirectDevice tdd in aFc)
         //      foreach (TCRZAGroup tdg in tdd)
         //         foreach (TCRZAVariable tgv in tdg)                     
         //            tgv.Quality = CRZADevices.VarQuality.vqGood;
      }
      #endregion

      #region ������ � ������
      /// <summary>
      /// ������� �� ��������� �������� ����
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

         // �������/������� ����������� ����
         for( int i = 0 ; i < tabForms.TabPages.Count ; i++ )
         {
            TabPage tp = tabForms.TabPages [ i ];
            foreach( Form frowned in ( tp.Tag as Form as Form ).OwnedForms )
               frowned.Hide();
         }

         if( ( tabForms.SelectedTab != null ) && ( tabForms.SelectedTab.Tag != null ) )
         {
            ( tabForms.SelectedTab.Tag as Form ).Select();
            foreach( Form frowned in ( tabForms.SelectedTab.Tag as Form ).OwnedForms )
               frowned.Show();
         }
      }
      #endregion

      #region ������ �������� ����


      private void �����������������ToolStripMenuItem_Click( object sender, EventArgs e )
      {
         ToolStripMenuItem ti = ( ToolStripMenuItem )sender;

         if (!CommonUtils.CommonUtils.IsUserActionBan(CommonUtils.CommonUtils.UserActionType.b03_Administrate_Users, HMI_MT_Settings.HMI_Settings.UserRight))
         {
            foreach (ToolStripDropDownItem tsddi in ti.DropDownItems)
            {
               if (tsddi.Tag != null)
                  if (( bool )tsddi.Tag == true)
                  {
                     tsddi.Available = true;
                     tsddi.Visible = true;
                  }
            }
         }
         else
         {
            foreach (ToolStripDropDownItem tsddi in ti.DropDownItems)
            {

               if (tsddi.Available && tsddi.Visible)
               {
                  tsddi.Tag = true;
                  tsddi.Available = false;
               }
               else
                  tsddi.Available = false;
            }
         }
      }
      private void ������������ToolStripMenuItem_Click( object sender, EventArgs e )
      {
         GC.Collect( );
      }
      private void ������ToolStripMenuItem_Click( object sender, EventArgs e )
      {
         //helpProvider1.HelpNamespace = Application.StartupPath + "\\" + assna
         helpProvider1.HelpNamespace = Application.StartupPath + "\\spravka.chm";
         Help.ShowHelp( this, helpProvider1.HelpNamespace );

      }

        // ����� ����� �������� �������
      private void ShowSpeedAccess() 
      {
          scDeviceObjectConfig.Visible = true;

          Form[] arrF = this.MdiChildren;
          for (int i = 0; i < arrF.Length; i++)
              if (arrF[i].Name == "SpeedAccess")
              {
                  arrF[i].Focus();
                  return;
              }
          // ������� ����� �������� �������
          SpeedAccess sa = new SpeedAccess(this);
          sa.MdiParent = this;

          if (tvlc == null)
              tvlc = new TreeViewLogicalConfig(tvLogicalObjectsConfig, this);

          tvlc.OnChangeTabpage += new ChangeTabpage(sa.tvlc_OnChangeTabpage);
          sa.Show();
      }

      private void tsmiTagBrowser_Click( object sender, EventArgs e )
      {
         //Cursor crs = this.Cursor;

         //this.Cursor = Cursors.WaitCursor;

         //dlgTagBrowser dtb = new dlgTagBrowser( this, KB );
         //dtb.Show( );

         //this.Cursor = crs;
      }

      #region ������
      private void printDocument1_PrintPage( object sender, System.Drawing.Printing.PrintPageEventArgs e )
      {
         //������� ��������� graph ������ Graphics
         Graphics graph = e.Graphics;
         //������� ������ font, �������� ������������� 
         // ����� �������� rtbText
         //Font font = rtbText.Font;
         //�������� �������� ������������ ��������� - ������ ������ �1, 134
         float HeightFont = font.GetHeight( graph );
         //������� ��������� stringformat ������ StringFormat ��� ����������� 
         //�������������� ���������� �������������� ������.
         StringFormat stringformat = new StringFormat( );
         //������� ���������  rectanglefFull � rectanglefText ������ RectangleF ��� 
         //���������� �������� ������ � ������. �1, 104
         RectangleF rectanglefFull, rectanglefText;
         //������� ���������� ��� �������� ����� �������� � �����.
         int NumberSymbols, NumberLines;
         //� �������� ������� ������ ������������� ������ rectanglefFull
         if (graph.VisibleClipBounds.X < 0) rectanglefFull = e.MarginBounds;
         else
            //����������   ������  rectanglefFull
            rectanglefFull = new RectangleF(
               //������������� ����������  X  
                e.MarginBounds.Left - ( e.PageBounds.Width - graph.VisibleClipBounds.Width ) / 2,
               //������������� ����������  Y
                e.MarginBounds.Top - ( e.PageBounds.Height - graph.VisibleClipBounds.Height ) / 2,
               //������������� ������ �������
                e.MarginBounds.Width,
               //������������� ������ �������
                e.MarginBounds.Height );
         rectanglefText = RectangleF.Inflate( rectanglefFull, 0, -2 * HeightFont );
         //���������� ����� �����
         int NumDisplayLines = ( int )Math.Floor( rectanglefText.Height / HeightFont );
         //������������� ������ �������
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
         //��� ������ ��������� ������� ��������� � ������ ��������� ��������
         while (( PageNumber < StartPage ) && ( stringPrintText.Length > 0 ))
         {
            if (prt.rtbText.WordWrap)
               //�������� ��������� ����������, 
               //����������� ������,  � ���������� ����� �������� NumberSymbols
               //� ����� ����� NumberLines
               graph.MeasureString( stringPrintText, font, rectanglefText.Size, stringformat, out NumberSymbols, out NumberLines );
            else
               NumberSymbols = SymbolsInLines( stringPrintText, NumDisplayLines );
            stringPrintText = stringPrintText.Substring( NumberSymbols );
            //����������� ����� ������� 
            PageNumber++;
         }
         //���� ����� ������ stringPrintText ��������� ���� (��� ������ ��� ������),
         // ������������� ������
         if (stringPrintText.Length == 0)
         {
            e.Cancel = true;
            return;
         }
         //������� (������) ����� ��� ������ - stringPrintText, ���������� ��� ����� ����� font,
         //����� ������� �����  - Brushes.Black, ������� ������ - rectanglefText,
         //�������� ������  ��������������� �������������� stringformat
         graph.DrawString( stringPrintText, font, Brushes.Black, rectanglefText, stringformat );
         //�������� ����� ��� ��������� ��������
         if (prt.rtbText.WordWrap)
            graph.MeasureString( stringPrintText, font, rectanglefText.Size, stringformat, out NumberSymbols, out NumberLines );
         else
            NumberSymbols = SymbolsInLines( stringPrintText, NumDisplayLines );
         stringPrintText = stringPrintText.Substring( NumberSymbols );
         //������� ������ stringformat, �������������� ��� ������������ �����.
         stringformat = new StringFormat( );
         //���������  ����� �� ������ �������� �� �����
         stringformat.Alignment = StringAlignment.Far;
         graph.DrawString( "�������� " + PageNumber, font, Brushes.Black, rectanglefFull, stringformat );
         PageNumber++;
         //C���� ���������, ������� �� ����� ��� ������ � ����� ��������, �������� ��� ������
         e.HasMorePages = ( stringPrintText.Length > 0 ) && ( PageNumber < StartPage + NumPages );
         //��� ������ �� ���� ���������������� ���������  ����� �������������� ����������
         if (!e.HasMorePages)
         {
            stringPrintText = prt.rtbText.Text;
            StartPage = 1;
            NumPages = printDialog1.PrinterSettings.MaximumPage;
            PageNumber = 1;
         }
      }

        #region PrintDataSet() - ����������� ������ DataSet
        /*=======================================================================*
        *   static void PrintDataSet( DataSet ds )
        *       ����������� ������ DataSet
        *=======================================================================*/
        static void PrintDataSet( DataSet ds )
        {
           // ����� ��������� ���� �� ���� DataTable ������� DataSet
           Console.WriteLine( "������� � DataSet '{0}'. \n ", ds.DataSetName );
           foreach (DataTable dt in ds.Tables)
           {
              Console.WriteLine( "������� '{0}'. \n ", dt.TableName );
              // ����� ���� ��������
              for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                 Console.Write( dt.Columns[curCol].ColumnName.Trim( ) + "\t" );
              Console.WriteLine( "\n-----------------------------------------------" );

              // ����� DataTable
              for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
              {
                 for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                    Console.Write( dt.Rows[curRow][curCol].ToString( ) + "\t" );
                 Console.WriteLine( );
              }
           }
        }
        #endregion
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

      #region ����� �� ����������
        /// <summary>
        /// ���� ��� �������������� ��������� ��������� ������� � ������������� �������� ����������
        /// </summary>
      bool isFirsQuestionAboutExit = false;
        private bool ExitProgram( )
        {
            if (DialogResult.No == MessageBox.Show("��������� ������?", "�������������", MessageBoxButtons.YesNo))
                return false;

            return true;
        }

        /// <summary>
        /// ���������� ������ �� ����������
        /// </summary>
        private void PrepareFormClosing()
        {
            FormClosingEventArgs ee = new FormClosingEventArgs(CloseReason.UserClosing, true);
            MainForm_FrmClosing(this, ee);
        }

        /// <summary>
        /// ����� ���� �����
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
		private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            PrepareFormClosing();
        }

        /// <summary>
        /// �������� �� �����-���� �������� ���� Alt-F4
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (ExitProgram())
			{
				isFirsQuestionAboutExit = true;
				PrepareFormClosing();
			}

			//if (!isFirsQuestionAboutExit)
			//{
			//    if (DialogResult.No == MessageBox.Show("��������� ������?", "�������������", MessageBoxButtons.YesNo))
			//    {
			//        if (e.CloseReason == CloseReason.UserClosing)
			//        {
			//            e.Cancel = true;

			//            return;
			//        }
			//    }
			//}

			//if (e.CloseReason == CloseReason.UserClosing)
			//    e.Cancel = true;

			//DoExit();
        }
		/// <summary>
		/// ����� ������� �������� (�� ���������)
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void MainForm_FrmClosing(object sender, FormClosingEventArgs e)
		{
			if (!isFirsQuestionAboutExit)
			{
				if (DialogResult.No == MessageBox.Show("��������� ������?", "�������������", MessageBoxButtons.YesNo))
				{
					if (e.CloseReason == CloseReason.UserClosing)
					{
						e.Cancel = true;

						return;
					}
				}
			}

			if (e.CloseReason == CloseReason.UserClosing)
				e.Cancel = true;

			DoExit();
		}

      private void CloseNetManager()
      {
         #region ������� ������� ��������
         // ������� ��������� ��� � ������ ������ �������� ����������� ������ - ��������� DataServer ���
         // ������� ������
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
         // ��������� ������� ������������
         // ������� ������� ��� ����������� �������� ������������
         // ...

		  try
		  {
			  // ��������� ��� �������� ����
			  Form[] arrF = this.MdiChildren;
			  for (int i = 0; i < arrF.Length; i++)
				  arrF[i].Close();

			  // ����������� � ��������� DataSet � ����� - �������� ������������
			  //DSProfile.WriteXml(Application.StartupPath + Path.DirectorySeparatorChar + "UserProfile_" + UserName + ".upf", XmlWriteMode.IgnoreSchema);

			  #region ��������� ���� �����������
			  TraceSourceLib.TraceSourceDiagMes.CloseLog();
			  #endregion

			  CloseNetManager();

			  // ��������� ��������� pipe-�������
              //HMI_Settings.ClientDFE.Close();
		  }
		  catch (Exception ex)
		  {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
		  }
		  finally 
		  {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 1657, DateTime.Now.ToString() + " : (1657)MainForm.cs : DoExit() : ����� �� ����������");
			  TraceSourceLib.TraceSourceDiagMes.FlushLog();

			  // ��������� �� �����
			  Process.GetCurrentProcess().Kill();
		  }
      }
      #endregion

      #region �������
      //private void AliveTimer_Tick( object sender, EventArgs e )
      //{
      //   CommonUtils.CommonUtils.WriteEventToLog( 31, Process.GetCurrentProcess( ).WorkingSet64.ToString( ) + " (��); " + Process.GetCurrentProcess( ).VirtualMemorySize64.ToString( ) + " (��); "
      //      + Process.GetCurrentProcess( ).Threads.Count.ToString( ) + " - �������; " + Process.GetCurrentProcess( ).HandleCount.ToString( ) + " - �����.; "
      //          + Process.GetCurrentProcess().UserProcessorTime.ToString() + " - ����. �����; ", false);//, true, false );
      //}
      #endregion

      #region ������ �������
      #region ��� ����������������� ������ ��������� (��������� ������)
      /*==========================================================================*
			*   private void void LinkSetText(object Value)
			*      ��� ����������������� ������ ���������
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
        * //��� ����������������� ������ ���������
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

      #region �������
		/// <summary>
		/// ��������� MAC-�����
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

      private void �������������������������ToolStripMenuItem_Click( object sender, EventArgs e )
        {
			  frmLogLocal fll = new frmLogLocal();
			  fll.Show();
        }
      #endregion

      #region ������������ � ��
      #region �������� ����� � ��
      private void timerTestFCConnect_Tick( object sender, EventArgs e )
      {
         ///* 
         // * ���������� � ����� � �� ��������� �� ������� ��� ����� ���� 8, 
         // * ��� ��� ��������� ��������������� �������� � ������ �� ��� �������� ������
         // * � �� ��� ����� ���������� ��� �����������, ����� ����� ��� �������� ������������ 
         // * � ������� Configurator.ReceivePacketInThread()
         // */
         //if ( KB == null )
         //   return;

         //bool noConnect = false;  // ����� ������� ��������� ����� � �����-�� ��

         //// ������������ ����� � ��, ������ ������������� ����� � ��
         //foreach( DataSource aFC in KB )
         //{
         //   if( aFC.isLostConnection )
         //         noConnect = true;
         //}

         //StringBuilder sbm_noConnection = new StringBuilder( );

         //// ��������� �������� ��������� � ������ ������� � ��������� ��    
         //if( noConnect )
         //{
         //   sbm_noConnection.Append( "��� ����� � �� � " );

         //   foreach( DataSource aFC in KB )
         //      if( aFC.isLostConnection )
         //         sbm_noConnection.Append( aFC.NumFC.ToString() + ';' );

         //   LinkSetTextISB( sbConnectFC, sbm_noConnection.ToString( ), Color.Yellow );
         //}
         //else
         //{
         //   LinkSetTextISB( sbConnectFC, "���� ����� � ��", toolStripStatusLabelClock.BackColor ); // ��� �������������� ����� ����� ����� ���
         //}
      }
      #endregion

      #region ������������� � ���������� �������
      /// <summary>
      /// private void timerDataTimeUpdate_Tick()
      ///     ��� ������� - ���������� ������� � ��������� ������
      /// </summary>
      private void timerDataTimeUpdate_Tick( object sender, EventArgs e )
      {
         if (!bct.IsBusy)
            bct.RunWorkerAsync( );
      }

      private void timerDataTimeUpdateInThread( object sender, DoWorkEventArgs e )
      {
      //    // ���� ��� ������� ���������������
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

      //   // ���� ����� �������������� ����� � ��
      //   if (currCountSynhr >= countSynhr) // ���������� ������� ������ ������� 
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

      //   // �������� �������� �������

      //   if (dtFormat == DataTimeFormat.ShowClock)
      //      panelInfo = DateTime.Now.ToLongTimeString( );
      //   else if (dtFormat == DataTimeFormat.ShowDay)
      //      panelInfo = DateTime.Now.ToShortDateString( );
      //   else if (dtFormat == DataTimeFormat.ShowStorageStat)
      //      panelInfo = newKB.netman.NumPacketsInNetPackQ.ToString( );

      //   // ������� � ���� � ������ ��������
      //   CurrentDateTime = DateTime.Now;	// ������������� ������� ����� ����������
      //   tsdt = CurrentDateTime.GetDateTimeFormats( );

      //   #region ���� ������
      //   dayofw.Length = 0;
      //   switch (CurrentDateTime.DayOfWeek.ToString())
      //   {
      //      case "Monday":
      //         dayofw.Append("=�����������=");
      //         break;
      //      case "Tuesday":
      //         dayofw.Append("=�������=");
      //         break;
      //      case "Wednesday":
      //         dayofw.Append("=�����=");
      //         break;
      //      case "Thursday":
      //         dayofw.Append("=�������=");
      //         break;
      //      case "Friday":
      //         dayofw.Append("=�������=");
      //         break;
      //      case "Saturday":
      //         dayofw.Append("=�������=");
      //         break;
      //      case "Sunday":
      //         dayofw.Append("=�����������=");
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

      #region ������������� ����� ������� ������ ��� ���������� ���������� �������� ������
      private void timer1_Tick( object sender, EventArgs e )
      {
         //newKB.ReceivePacket( );
      }
      #endregion
      #endregion

      #region ��������� ��������������
      /// <summary>
      /// ��������� ����������� �������
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      private void tsmiIsClientExist_Click(object sender, EventArgs e)
      {
         Ping ping2Client = new Ping();
         PingOptions pingoptions = new PingOptions();

         pingoptions.DontFragment = true;

         // ����� 32 �����
         string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
         byte[] buffer = Encoding.ASCII.GetBytes(data);
         int timeout = 3000;

         PingReply pr = ping2Client.Send(HMI_Settings.IPADDRES_CLIENT, timeout, buffer, pingoptions);

         if (pr.Status == IPStatus.Success)
            MessageBox.Show("Ping �� ����� " + HMI_Settings.IPADDRES_CLIENT + " �������.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
         else
            MessageBox.Show("��������� � ������� " + HMI_Settings.IPADDRES_CLIENT + " ����������.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }      
      #endregion

      #region ������� ����������
		/// <summary>
      /// �������� ������� ���������� ���'� ��� ����� �������� �������
      /// </summary>
		private void CreateMainMnemo( )
      {
          // ��������� �������, ����� ������ ��� ����� ��������� - ���������� ��� ������� ������
          if (!String.IsNullOrEmpty(HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("Mnemoshems").Attribute("showoption").Value))
              if (HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("Mnemoshems").Attribute("showoption").Value == "yes")
                  ShowMnemo();
              else
                  ShowSpeedAccess();
      }

		private void ShowMnemo() 
      {
         #region MainMnemo ��� ����� ����������
         //Form_ez = new MainMnemo( this, "Panel1" );
         //Form_ez.MdiParent = this;
         //Form_ez.WindowState = FormWindowState.Maximized;
         //Form_ez.MaximumSize = this.Size;
         //Form_ez.Width = this.ClientSize.Width;
         //Form_ez.Height =  this.ClientSize.Height;

         //Form_ez.Show( ); 
		  XDocument xdoc = XDocument.Load(PathToPrjFile);
		  IEnumerable<XElement> mnemoshems = xdoc.Element("Project").Element("Mnemoshems").Elements("Mnemo");
		  string file = string.Empty; //AppDomain.CurrentDomain.BaseDirectory + "\\Project\\MnemoSchemas\\Center06 (1600).mnm";

		  foreach (XElement xe in mnemoshems)
			  if (xe.Attribute("panel").Value == "Panel1")
			  {
				  XElement node = xe.Element("Mnemolevel2").Element("FileName");
				  file = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar
					 + "Project" + Path.DirectorySeparatorChar + node.Value;
				  break;
			  }//if

		  Form_ez = new NewMainMnemo(file, this, false);
		  Form_ez.MdiParent = this;
		  Form_ez.Show();
         #endregion

         #region MainMnemo ��� ���� ���������
		   //Form_ez = new frm2Panels( this );
         //Form_ez.MdiParent = this;
         //Form_ez.MaximumSize = this.Size;
         //Form_ez.Width = this.ClientSize.Width;
         //Form_ez.Height = this.ClientSize.Height;
         //Form_ez.WindowState = FormWindowState.Maximized;

         //Form_ez.Show( );
	      #endregion
      }
		#endregion   

		#region ��������������������
		private void ��������������������ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < tabForms.TabPages.Count; i++)
			{
				TabPage tp = tabForms.TabPages[i];

				if (!tp.Visible)
					continue;
				Form fr = (Form)tp.Tag;
				//foreach ( Control c in tp.Controls)
				//{
				//   Control cc = c;
				//}
				////frmBMRZbase fb = tp.
				GetCCforFLP((ControlCollection)fr.Controls);//

			}
		}

		public void GetCCforFLP(ControlCollection cc)
		{
			for (int i = 0; i < cc.Count; i++)
			{
				if (cc[i] is FlowLayoutPanel)
				{
					FlowLayoutPanel flp = (FlowLayoutPanel)cc[i];
					DrawNameForFLP(flp);
				}
				else if (cc[i] is MTRANamedFLPanel)
				{
					MTRANamedFLPanel flp = (MTRANamedFLPanel)cc[i];
					DrawNameForFLP(flp);
				}
				else
					TestCCforFLP(cc[i]);
			}
		}

		private void TestCCforFLP(Control cc)
		{
			for (int i = 0; i < cc.Controls.Count; i++)
			{
				if (cc.Controls[i] is MTRANamedFLPanel)
				{
					MTRANamedFLPanel flp = (MTRANamedFLPanel)cc.Controls[i];
					DrawNameForFLP(flp);
				}

				else if (cc.Controls[i] is FlowLayoutPanel)
				{
					FlowLayoutPanel flp = (FlowLayoutPanel)cc.Controls[i];
					DrawNameForFLP(flp);
				}
				else
				{
					TestCCforFLP(cc.Controls[i]);
				}
			}
		}

		private void DrawNameForFLP(FlowLayoutPanel flp)
		{
			Bitmap t	= new	Bitmap(flp.ClientSize.Width, flp.ClientSize.Height);
			//	�����������	��� ��������� �� ������� �����!
			Graphics	ee	= Graphics.FromImage(t);
			Font drawFont = new Font("Arial", 16);
			SolidBrush drawBrush	= new	SolidBrush(Color.Green);

			//g.Clear(Color.White);
			ee.DrawString(flp.Name,	drawFont, drawBrush,	550.0f, 550.0f);

			//ee.DrawString(
			flp.BackgroundImage = t;
		}	 
		#endregion

      /// <summary>
      /// ������� �� ������� �������
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      private void ReconnectServer()
      {
         System.Diagnostics.Trace.TraceInformation(DateTime.Now.ToString() + " :(3791) MainForm.cs : ������� �� ������� ������� : ");
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
				  string str = br.ReadString();
                  System.Diagnostics.Trace.TraceInformation(DateTime.Now.ToString() + " :(3802) MainForm.cs : ������� �� ������� ������� : ���������");
                  //MessageBox.Show("���������.", this.Text, MessageBonnssswwwxButtons.OK, MessageBoxIcon.Information);
               }
            }
            catch (Exception ex)
            {
               System.Diagnostics.Trace.TraceInformation(DateTime.Now.ToString() + " :(3861) MainForm.cs : ��������������������������ToolStripMenuItem_Click : " + ex.Message);
               TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 3854, "�� ������� ���������� ����� � ��������.\n��������� �������:\n1. ���������� ����� �����.\n2. �� ������� �� ��������.");               
            }
         }
      }

      private void �������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
      {
         CreateFormFEForActPgs();
      }

      private void CreateFormFEForActPgs()
      {
         // frmDiagnostic_FormulaEvalsActivePages frmd_feap = new frmDiagnostic_FormulaEvalsActivePages(HMI_Settings.ClientDFE.slFormulaEvalsForActivePages, HMI_Settings.ClientDFE.DsFEForFEvals);
         //frmd_feap.Show();
      }

      private void tsmiCreateNormalModePanel_Click(object sender, EventArgs e)
      {
         //dlgOptionsFormEditor fnm = new dlgOptionsFormEditor(this,null,KB);
         //fnm.ShowDialog();
      }

      private void �������������������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
      {
         MessageBox.Show("��� ��������� ������ ������ ���������� �������� �������� value (������ <system.diagnostics><switches>) � ���������������� ����� ����������");
         System.Diagnostics.Trace.Refresh();
      }

      private void �����������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
      {
         TraceSourceLib.TraceSourceDiagMes.FlushLog();
      }

	  protected override void OnClosing(CancelEventArgs e)
	  {
		  if (e.Cancel )
			   base.OnClosing(e);
	  }


        /// <summary>
        /// Schema
        /// </summary>
        private void �ibbonButtonSchemaClick( object sender, EventArgs e )
        {
            Form[] arrF = this.MdiChildren;
            for ( int i = 0; i < arrF.Length; i++ )
            {
                var main = arrF[i] as NewMainMnemo;
                if ( main != null && !main.OwnerSchema )
                {
                    arrF[i].Focus();
                    return;
                }
            }

            // ������� ������� ����������
            CreateMainMnemo();
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
                MessageBox.Show( "���� � ��������� ������ ��������� ��������� PanelState.xml �� ����������.", "MainForm.cs(667)",
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
                return;
            }

            //// ����� ��������� ���
            //frmTestPTKState frmtptkstate = new frmTestPTKState();
            ////frmtptkstate.TopLevel = false;
            //frmtptkstate.Show();

            frmDiagPanel diagPanel = new frmDiagPanel();
            diagPanel.MdiParent = this;
            diagPanel.Show();
        }
        /// <summary>
        /// Current mode
        /// </summary>
        private void RibbonButtonCurrentModeClick( object sender, EventArgs e )
        {
            NormalModeLibrary.ComponentFactory.EditUserWindows();
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
            // ������� ������ ������� ���������
            Form_ev = new frmLogs( this );
            Form_ev.MdiParent = this;
            Form_ev.Show();
            if ( !isBDConnection )
                Form_ev.Close();
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

            // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            try
            {
                asqlconnect.Open();
            }
            catch ( Exception ex )
            {
                asqlconnect.Close();
                MessageBox.Show( "��� ����� � ��������" + Environment.NewLine + ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Information );

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
                MessageBox.Show( "������� GPS �������. /n������ ��������� ������� ����������.", "��������� ������� ���", MessageBoxButtons.OK, MessageBoxIcon.Information );
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
            ////��������� ��������� ������
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
            // ��������� ��� �������� ����
            Form[] arrF = this.MdiChildren;
            for ( int i = 0; i < arrF.Length; i++ )
                arrF[i].Close();

            // ������� ����� �����������
            Form_ea = new frmAutorization( this, Target.ChangeUser );

            while ( ( DialogResult = Form_ea.ShowDialog() ) != DialogResult.OK )
            {
                if ( ExitProgram() )
                {
                    isFirsQuestionAboutExit = true;
                    PrepareFormClosing();
                    return;
                }
            }
            //CommonUtils.CommonUtils.TestUserMenuRights( menuStrip1, HMI_MT_Settings.HMI_Settings.arrlUserMenu );
            // ������� ������� ����������
            CreateMainMnemo();
        }
        /// <summary>
        /// Block system
        /// </summary>
        private void RibbonMenuButtonBlockSystemClick( object sender, EventArgs e )
        {
            // ����������� ��� �������� ����
            Form[] arrF = this.MdiChildren;
            for ( int i = 0; i < arrF.Length; i++ )
                arrF[i].WindowState = FormWindowState.Minimized;
            // ���������������� �������� ������������
            CommonUtils.CommonUtils.WriteEventToLog( 15, "���������� �������", true );//, true, false );

            // ����������� �����
            Form_ea = new frmAutorization( this, Target.BlockSystem );

            while ( ( DialogResult = Form_ea.ShowDialog() ) != DialogResult.OK )
            {
                if ( ExitProgram() )
                {
                    isFirsQuestionAboutExit = true;
                    PrepareFormClosing();
                    return;
                }
            }
            // ���������������� �������� ������������
            CommonUtils.CommonUtils.WriteEventToLog( 16, "������������� �������", true );//, true, false );

            // ������������� ��� �������� ����
            arrF = this.MdiChildren;
            for ( int i = 0; i < arrF.Length; i++ )
                arrF[i].WindowState = FormWindowState.Maximized;
        }
        /// <summary>
        /// User information
        /// </summary>
        private void RibbonMenuButtonUserInformationClick( object sender, EventArgs e )
        {
            MessageBox.Show( "��� ������������: \t" + HMI_Settings.UserName + "\n\n������ ����: \t\t" +
                HMI_Settings.GroupName, "���������� � ������� ������������", MessageBoxButtons.OK, MessageBoxIcon.Information );
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
            PrepareFormClosing();
        }
        /// <summary>
        /// Print
        /// </summary>
        internal void RibbonMenuButtonPrintClick( object sender, EventArgs e )
        {
            printDialog1.AllowSelection = prt.rtbText.SelectionLength > 0;

            if ( printDialog1.ShowDialog() == DialogResult.OK )
            {
                printDocument1.DocumentName = Text;
                //���������� �������� ������� ��� ������
                switch ( printDialog1.PrinterSettings.PrintRange )
                {
                    //������� ��� ��������
                    case PrintRange.AllPages:
                        stringPrintText = prt.rtbText.Text;
                        StartPage = 1;
                        NumPages = printDialog1.PrinterSettings.MaximumPage;
                        break;
                    //������� ���������� �������
                    case PrintRange.Selection:
                        stringPrintText = prt.rtbText.SelectedText;
                        StartPage = 1;
                        NumPages = printDialog1.PrinterSettings.MaximumPage;
                        break;
                    //������ ��� �������
                    case PrintRange.SomePages:
                        stringPrintText = prt.rtbText.Text;
                        StartPage = printDialog1.PrinterSettings.FromPage;
                        NumPages = printDialog1.PrinterSettings.ToPage - StartPage + 1;
                        break;
                }
                PageNumber = 1;
                //�������� ���������� ����� ��� ������ ������
                try
                {
                    printDocument1.Print();
                }
                catch
                {
                    System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString() + " : mnuPrint_Click()" );
                    prt.rtbText.Clear();
                    return;
                }

                // ������� rtbText
                prt.rtbText.Clear();
            }
        }
        /// <summary>
        /// Page setup
        /// </summary>
        internal void RibbonMenuButtonPageSetupClick( object sender, EventArgs e )
        {
            //���������� ������
            pageSetupDialog1.ShowDialog();
        }
        /// <summary>
        /// Preview page
        /// </summary>
        internal void RibbonMenuButtonPreviewPageClick( object sender, EventArgs e )
        {
            //�������������� ����������
            printDocument1.DocumentName = Text;
            stringPrintText = prt.rtbText.Text;
            StartPage = 1;
            NumPages = printDialog1.PrinterSettings.MaximumPage;
            PageNumber = 1;
            //���������� ������
            printPreviewDialog1.ShowDialog();
            prt.rtbText.Clear();
        }
        /// <summary>
        /// Page font
        /// </summary>
        private void RibbonMenuButtonPageFontClick( object sender, EventArgs e )
        {
            fontDialog1.Font = font;
            if ( fontDialog1.ShowDialog() == DialogResult.OK )
                font = fontDialog1.Font;
        }

	    #region ������ ������ �� ���������� ������ ������� ��������
      /// <summary>
      /// public bool CanAction()
      /// ��� ��������� ���������� ������ ������������ - ����� ����������� ������� ������������ �������� 
      /// �������� ������������� ������ �������� ������������. ������ ������� ������� ������� � ��������� ������ 
      ///  � ��������� ���������� ��������� �� ��������� �������� ����������� ���������� �� ���������� ������� ��������
      /// </summary>
      /// <param Name="UserName">��� ������������</param>
      /// <param Name="UserID">������������� ������������</param>
      /// <returns></returns>
      //public  bool CanAction()
      //{
      //    // ��������� ������
      //    dlgCanPassword dcp = new dlgCanPassword(HMI_MT_Settings.HMI_Settings.UserName, HMI_MT_Settings.HMI_Settings.UserID);
      //    DialogResult dr = dcp.ShowDialog();
      //    if (dr == DialogResult.Abort)
      //        return false;
      //    else
      //        return true;
      //}
      #endregion
	}
}
