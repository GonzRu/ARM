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
using System.Xml.Linq;

using Egida;

using TraceSourceLib;
using InterfaceLibrary;
using HMI_MT_Settings;
using DataBaseLib;
using DebugStatisticLibrary;
using HMI_MT.Properties;
using NormalModeLibrary.Windows;

namespace HMI_MT
{
	/// <summary>
	/// ����� ������� ����� ���������� HMI_MT
	/// </summary>
	public partial class MainForm : Form
   {
		#region public

		public Form Form_ez;

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

		#region private

	    private frmLogs Form_ev;
	    private frmAutorization Form_ea;

		/// <summary>
		/// ����� (Sinleton) - ������ ����������� � ��
		/// </summary>
		SQLConnectionString scs;

        bool bEnter;    // ������� ��������������� ����� �� ������� �����
	  
		/// <summary>
		/// ������ �� ������ ���������� ������������ ���������
		/// </summary>
        TreeViewLogicalConfig tvlc;
        
		StringDictionary strDictionary = new StringDictionary(); // �������� ������� ��� �������� ������� �������� ������������ �� ��;

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

			    if (String.IsNullOrEmpty(HMI_Settings.AuraUrl))
			        ��������ToolStripMenuItem.Visible = false;
			}
			catch(Exception ex)
			{
				TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
        }
        #endregion

        #region �������� � ��������� �����
        private void MainForm_Load(object sender, EventArgs e)
        {
			try
            {
                HMI_Settings.Link2MainForm = this;

                // �������� �� ������ ����� � DS
                HMI_Settings.CONFIGURATION.OnConfigDSCommunicationLoss4Client += CONFIGURATION_OnConfigDSCommunicationLoss4Client;

                ControlBox = MaximizeBox = MinimizeBox = Convert.ToBoolean(HMI_Settings.ViewBtn4MainWindow);

                // ���������������� ��������� �� ���� ������ (�� ����� Project.cfg)
                InitSettingsToBD();

                #region � ������
                //���������� ����� ��������, � ������� ������� ������ ������
                printDialog1.PrinterSettings.FromPage = 1;
                //���������� ������������ ����� ���������� ��������.
                printDialog1.PrinterSettings.ToPage = printDialog1.PrinterSettings.MaximumPage;
                #endregion

                SetTitleMainWindowByPrgName();

                sbMesIE.Text = SetAssemblyVertion();

                SetPositionAndSizeForMainForm();

                bEnter = false;

                Application.OpenForms[0].Activate();

                // �������� � ���������� ������ ������ �������� �������
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
        /// ������� �� ������ ����� � DS
        /// </summary>
      void CONFIGURATION_OnConfigDSCommunicationLoss4Client(bool state)
      {
          // * ���������� � ����� � �� ��������� �� ������� ��� ����� ���� 8, 
          // * ��� ��� ��������� ��������������� �������� � ������ �� ��� �������� ������
          // * � �� ��� ����� ���������� ��� �����������, ����� ����� ��� �������� ������������ 
          // * � ������� Configurator.ReceivePacketInThread()
          // */

          StringBuilder sbm_noConnection = new StringBuilder();

          // ��������� �������� ��������� � ������ ������� � ��������� ��    
          if ( state )
          {
              sbm_noConnection.Append( "��� ����� � �������� ������ " );
              LinkSetTextISB( sbConnectFC, sbm_noConnection.ToString(), Color.Yellow );
              ResetSchemaStateProtocol();
          }
          else
          {
              LinkSetTextISB( sbConnectFC, "���� ����� � �������� ������", sbMesIE.BackColor ); // ��� �������������� ����� ����� ����� ���
          }
      }

      #region ��������� ��
      /// <summary>
      /// ��������� ��������� �� ���� ������ (�� ����� Project.cfg)
      /// </summary>
      private void InitSettingsToBD()
      {
         System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
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
         dbcc.SetInterval(5000);
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
      #endregion ��������� ��


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
      /// ���������� ������ � ������� ������� �����
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
      /// �������� ��� ����������� �����
      /// </summary>
      private void MainFormActivate()
      {
         if( bEnter )
            return;

         bEnter = true;  // �������� ��� �������

         HMI_Settings.CurrentDateTime = DateTime.Now;

         tsdt = HMI_Settings.CurrentDateTime.GetDateTimeFormats();

         aDS = new DataSet("ptk"); // ������������� DataSet

         // �������� ���������� � �� ��� ����� � �������
            TestAndTryConnectionBD();

		  // ������������� ������ GPS ��� ��������� ��� � ��������� ������� GPS
            //GPS gpsInfo = GPS.Iinstance;
            //gpsInfo.InitGPSInfo( HMI_Settings.PathToPrgDevCFG_cdp,HMI_Settings.PathPanelState_xml, Configurator.KB);
            //gpsInfo.OnChangeGPSActive+=new ChangeGPSActive(gpsInfo_OnChangeGPSActive);
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
                  ExitWithoutAskDialog();
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
          DebugStatistics.WindowStatistics.AddStatistic( "�������� ����� � ����� ������." );

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

              DebugStatistics.WindowStatistics.AddStatistic( "�������� ����� � ����� ������ ���������." );

              return false;
          }
          catch (Exception ex)
          {
              asqlconnect.Close();

              MessageBox.Show("��� ����� � ��������" + Environment.NewLine + ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Information);
              CommonUtils.CommonUtils.WriteEventToLog(21, "��� ����� � �� (��� ������� ���): " + ex.Message, false);

              DebugStatistics.WindowStatistics.AddStatistic( "�������� ����� � ����� ������ ���������." );

              return false;
          }

          DebugStatistics.WindowStatistics.AddStatistic( "�������� ����� � ����� ������ ���������." );

          return true;
      }

      /// <summary>
      /// ������ �������� ������� ������������
      /// </summary>
      private void GetUserAction()
      {
          DebugStatistics.WindowStatistics.AddStatistic( "������ ������ � �������������." );

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

          DebugStatistics.WindowStatistics.AddStatistic( "������ ������ � ������������� ���������." );
      }

      /// <summary>
      /// ������ ������ ��� ����� � �������
      /// </summary>
      private void GetLogin()
      {
          DebugStatistics.WindowStatistics.AddStatistic( "���������� � ������� �����\\������." );

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
      /// ����� ������� � ���������� ������
      /// </summary>
      private void StartInNormalMode()
      {
          DebugStatistics.WindowStatistics.AddStatistic( "����� �������." );

          // ������ �������� ������� ������������
          GetUserAction();

          // ����������� �����
          GetLogin();

          // ������� ������� ����������
          //AddOwnedForm(frmSpScr);
          //frmSpScr.Show();
          Application.DoEvents();

          DebugStatistics.WindowStatistics.AddStatistic( "������������ ����� ������ � ���������� ������������." );
          // ��������� ����� ������ ���������� � ���������� ������������ ��������� � �������
          StartForm();
          DebugStatistics.WindowStatistics.AddStatistic( "������������ ����� ������ � ���������� ������������ ���������." );

          CreateMainMnemo();

          // ������� ������ ���������� ������
          CommonUtils.CommonUtils.WriteEventToLog(1, "", true);
      }

      /// <summary>
      /// ����� ������� � ������������ ������
      /// </summary>
      private void StartWithoutBD()
      {
          DebugStatistics.WindowStatistics.AddStatistic( "����� ������� � �������������." );

          isBDConnection = false;

          LinkSetTextISB(sbConnectBD, "��� ����� � ��", Color.Yellow);

          // ������� ������ ������ ��� ��
          CommonUtils.CommonUtils.WriteEventToLog(1, "���� � ������� ��� ��",  false);

          loginToArmWOBD = true;	// ������������� ������� ����� � ������� ��� ��

          // �����
          HMI_Settings.UserRight = "11111111111111111111111111111111";
      }
      #endregion

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

      #region ������ �������� ����
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
        /// Show ask dialog for exit
        /// </summary>
        private bool askIsNeedExitProgram()
        {
            if (DialogResult.No == MessageBox.Show("��������� ������?", "�������������", MessageBoxButtons.YesNo))
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
        /// ����� ���� �����
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
		private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
		{
            ExitWithAskDialog();
        }

        /// <summary>
        /// �������� �� �����-���� �������� ���� Alt-F4
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitWithAskDialog();
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

      #region ��������� ��������������
      /// <summary>
      /// ������� �� ������� �������
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      private void ReconnectServer()
      {
          Trace.TraceInformation(DateTime.Now.ToString() + " :(3791) MainForm.cs : ������� �� ������� ������� : ");
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
                      Trace.TraceInformation(DateTime.Now.ToString() + " :(3802) MainForm.cs : ������� �� ������� ������� : ���������");
                      //MessageBox.Show("���������.", this.Text, MessageBonnssswwwxButtons.OK, MessageBoxIcon.Information);
                  }
              }
              catch (Exception ex)
              {
                  Trace.TraceInformation(DateTime.Now.ToString() + " :(3861) MainForm.cs : ��������������������������ToolStripMenuItem_Click : " + ex.Message);
                  TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 3854, "�� ������� ���������� ����� � ��������.\n��������� �������:\n1. ���������� ����� �����.\n2. �� ������� �� ��������.");
              }
          }
      }
      #endregion

    #region ������� ����������
		/// <summary>
      /// �������� ������� ���������� ���'� ��� ����� �������� �������
      /// </summary>
		private void CreateMainMnemo( )
		{
		    DebugStatistics.WindowStatistics.AddStatistic( "���������� � ������ ����������." );
		    Form_ez = new NewMainMnemo( HMI_Settings.MainMnenoSchema, this, false ) { MdiParent = this };
		    Form_ez.Show( );
		    DebugStatistics.WindowStatistics.AddStatistic( "���������� � ������ ���������� ��������." );
		}
    #endregion   

        #region Private-Metods
        /// <summary>
        /// ����� ���������� ���������
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

            // ������� ������� ����������
            CreateMainMnemo( );
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

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (scDeviceObjectConfig.Visible)
                scDeviceObjectConfig.Visible = false;
            else
                scDeviceObjectConfig.Visible = true;
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

            Form[] arrF = this.MdiChildren;
            for (int i = 0; i < arrF.Length; i++)
                if (arrF[i].Name == "FrmDiagPanel")
                {
                    arrF[i].Focus();
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
            // ������� ������ ������� ���������
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
                ExitWithAskDialog();
            }
            //CommonUtils.CommonUtils.TestUserMenuRights( menuStrip1, HMI_MT_Settings.HMI_Settings.arrlUserMenu );
            // ������� ������� ����������
            CreateMainMnemo();

            NewUserLoged();
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
                if ( askIsNeedExitProgram() )
                {
                    ExitWithAskDialog();
                }
            }
            // ���������������� �������� ������������
            CommonUtils.CommonUtils.WriteEventToLog( 16, "������������� �������", true );//, true, false );

            // ������������� ��� �������� ����
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
                MessageBox.Show( this, "������ ������ �� ��������������", "����������", MessageBoxButtons.OK,
                                 MessageBoxIcon.Information );
                return;
            }

            form.Print( );
        }

        /// <summary>
        /// ���� ����
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
            DebugStatistics.WindowStatistics.AddStatistic("������ ������� ����������� ������.");
            NormalModeLibrary.ComponentFactory.Factory.ActivatedMainMnemoForms(Form_ez);
            DebugStatistics.WindowStatistics.AddStatistic("������ ������� ����������� ������ ��������.");
            /********************************************************************************************************/
        }
        #endregion

        #region MessagePanelProvider � ��������� ���������� � ��������
        private System.Timers.Timer _journalAlarmTimer = new System.Timers.Timer();

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

            if (tabControl.SelectedTab != null && tabControl.SelectedTab.Text == "��������� � �������")
            {
                StopJournalAlarmTimer();
            }
	    }

	    private void MessageProviderOnMessagesUpdated()
        {
                if (tabForms.SelectedTab != null && tabForms.SelectedTab.Text != "��������� � �������")
                {
                    StartJournalAlarmTimer();
                }
        }

	    private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (�����������������ToolStripMenuItem.BackColor == Color.Yellow)
                �����������������ToolStripMenuItem.BackColor = SystemColors.Control;
            else
                �����������������ToolStripMenuItem.BackColor = Color.Yellow;
        }

        private void StartJournalAlarmTimer()
        {
           _journalAlarmTimer.Start(); 
        }

        private void StopJournalAlarmTimer()
        {
            _journalAlarmTimer.Stop();
            �����������������ToolStripMenuItem.BackColor = SystemColors.Control;
        }

        
	    #endregion
   }
}
