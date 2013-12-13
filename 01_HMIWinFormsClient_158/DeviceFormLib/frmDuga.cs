/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	��������: ����� ��� ������ � ������ ����.                                                           
 *                                                                             
 *	����                     : frmDuga.cs                                         
 *	��� ��������� �����      : 
 *	������ �� ��� ���������� : �#, Framework 2.0                                
 *	�����������              : ���� �.�.                                        
 *	���� ������ ����������   : xx.04.2008                                       
 *	���� (v1.0)              :                                                  
 *******************************************************************************
 * ���������:
 * 1. ����(�����): ...c���������...
 *#############################################################################*/

using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Calculator;
using System.Collections;
using System.IO;
using LabelTextbox;
using System.Linq;
using System.Xml.Linq;
using InterfaceLibrary;
using HMI_MT_Settings;
using DataBaseLib;
using OscillogramsLib;

namespace DeviceFormLib
{
    public partial class frmDuga : Form, IDeviceForm
	{
        /// <summary>
        /// ������� TabPage ��� ��������/������� �����
        /// </summary>
        TabPage tpCurrent;
 
        int iFC;            // ����� �� �������������
        string strFC;       // ����� �� ������
        int iIDDev;         // ����� ���������� �������������
        string strIDDev;    // ����� ���������� ������
        int inumLoc;         // ����� ������ �������������

        /// <summary>
        /// ��� ����� � �������� �����
        /// </summary>
        string fileFrmTagsDescript;
		string nfXMLConfigFC; // ��� ����� � ��������� ����
        // ������ �������������� �������
        ArrayList arDopPanel;

        // ������ ������
        ConfigPanelControl pnlConfig;
        SrabatPanelControl pnlSrabat;
        //CurrentPanelControl pnlCurrent;
        OscDiagPanelControl pnlOscDiag;

        ArrayList arrAvarSign = new ArrayList();
        ArrayList arrCurSign = new ArrayList();
        ArrayList arrSystemSign = new ArrayList();
        ArrayList arrStoreSign = new ArrayList();
        ArrayList arrConfigSign = new ArrayList();

        SortedList se = new SortedList();
        SortedList sl_tpnameUst = new SortedList();
        StringBuilder sbse = new StringBuilder();

        DataTable dtO;  // ������� � ���������������
        DataTable dtG;  // ������� � �����������
        DataTable dtA;  // ������� � ��������
        DataTable dtU;  // ������� � ���������
        OscDiagViewer oscdg;
        /// <summary>
        /// ���� - ��������� �� ��������� �� ��������� �������� ������� 
        /// ��� ���
        /// </summary>
        bool IsMesView = false;

        /// <summary>
        /// ��� �������� ���� ������ � ������ ��� ����������� ������������
        /// </summary>
        ArrayList asb = new ArrayList();

        SortedList slFLP = new SortedList();	// ��� �������� ��� � FlowLayoutPanel
        /// <summary>
        /// ������ ���� ������. �� ����� PrgDevCFG.cdp 
        /// ��� ������� ����������
        /// </summary>
        private SortedList<string, string> slKoefRatioValue = new SortedList<string, string>();
 
        #region �����������
        public frmDuga(int iFC, int iIDDev, /*int inumLoc,*/ string fXML, string ftagsxmldescript)
		{
		    InitializeComponent();
            try
            {
                //InitInterfaceElementsClick();

                //parent = linkMainForm;
                this.iFC = iFC;                 // ����� �� �������������
                strFC = iFC.ToString();         // ����� �� ������
                this.iIDDev = iIDDev;           // ����� ���������� �������������
                strIDDev = iIDDev.ToString();   // ����� ���������� ������
                //this.inumLoc = inumLoc;         // ����� ������ �������������

                fileFrmTagsDescript = ftagsxmldescript;

                //Text += " ( " + strIDDev + " ) - ��. � " + strnumLoc;

                tabControl1.Controls.Add( new DataBaseFilesLibrary.TabPageDbFile( (uint)( 256 * iFC + iIDDev ) ) );
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

		}
        /// <summary>
        /// �������� �����
        /// </summary>
        private void frmDuga_Load( object sender, EventArgs e )
        {
			  ControlCollection cc;
			  cc = ( ControlCollection ) this.Controls;
			  for( int i = 0 ; i < cc.Count ; i++ )
			  {
				  if( cc[i] is FlowLayoutPanel )
				  {
					  FlowLayoutPanel flp = ( FlowLayoutPanel ) cc[i];
					  slFLP[flp.Name] = flp;
				  }
				  else
				  {
					  TestCCforFLP( cc[i] );
				  }
			  }

              // ������� ������ ������
              CreateTabPanel();

              InitInterfaceElementsClick();
        }
        /// <summary>
        /// � ���� ������� ����������� ����� ��
        /// ��������� ���������� � ����� �� ���������
        /// </summary>
        public void InitInterfaceElementsClick()
        {
            try
            {
                tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);
                                          
                tabPage1.Enter += new EventHandler(tabPage1_Enter);
                tbpAvar.Enter += new EventHandler(tbpAvar_Enter);
                tabStore.Enter += new EventHandler(tabStore_Enter);
                tbpConfUst.Enter += new EventHandler(tbcConfig_Enter);

                this.FormClosing += new FormClosingEventHandler(frmDuga_FormClosing);
                
                tabPage5.Enter += new EventHandler(tabPage5_Enter);

                lstvAvar.ItemActivate += new EventHandler(lstvAvar_ItemActivate);
                pnlSrabat.btnReNew.Click += new EventHandler(btnReNew_Click);
                //btnReNew.Click += new EventHandler(btnReNew_Click);
                //btnReadUstFC.Click += new EventHandler(btnReadUstFC_Click);
                lstvConfig.ItemActivate += new EventHandler(lstvConfig_ItemActivate);
                pnlConfig.btnReadUstFC.Click += new EventHandler(btnReadUstFC_Click);
                ////pnlConfig.btnWriteUst.Click += new EventHandler(btnWriteConfig_Click);
                ////pnlConfig.btnResetValues.Click += new EventHandler(btnResetValues_Click);
                pnlConfig.btnReNewUstBD.Click += new EventHandler(btnReNewUstBD_Click);

                //dtpStartData.ValueChanged += new EventHandler(dtpStartData_ValueChanged);

                dgvOscill.CellContentClick += new DataGridViewCellEventHandler(dgvOscill_CellContentClick);
                dgvDiag.CellContentClick += new DataGridViewCellEventHandler(dgvDiag_CellContentClick);
                pnlOscDiag.btnReNew.Click += new EventHandler(btnReNewOscDg_Click);

                btnReadMaxMeterFC.Click += new EventHandler(btnReadMaxMeterFC_Click);
                btnResetMaxMeter.Click +=new EventHandler(btnResetMaxMeter_Click);
                btnReadStoreFC.Click +=new EventHandler(btnReadStoreFC_Click);

            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        void frmDuga_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ������������ �� �����
            HMI_MT_Settings.HMI_Settings.HMIControlsTagReNew(tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe);
        }

        /// <summary>
        /// ���������� ������ ����������
        /// IDeviceForm
        /// </summary>
        public void CreateDeviceForm()
        { }

        /// <summary>
        /// ������������ ������������ �������
        /// ��� �������� �����
        /// </summary>
        /// <param name="typetabpage"></param>
        public void ActivateTabPage(string typetabpage)
        {
            try
            {
                switch (typetabpage)
                {
                    case "������":
                        tabControl1.SelectedTab = tbpAvar;
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        #endregion

        #region ������������ ������ (���) �������
        void CreateTabPanel()
        {
            TimeSpan ts;
            try
            {
                if (arDopPanel == null)
                    arDopPanel = new ArrayList();

                #region �������
                pnlConfig = new ConfigPanelControl();
                splitContainer2.Panel2.Controls.Add(pnlConfig);
                ////��������� ������ ��� �������
                pnlConfig.Dock = DockStyle.Fill;
                pnlConfig.Visible = false;
                ////arDopPanel.Add(pnlConfig);
                #endregion

                #region ������-������������
                pnlSrabat = new SrabatPanelControl();
                splitContainer51.Panel1.Controls.Add(pnlSrabat);
                pnlSrabat.Dock = DockStyle.Fill;
                pnlSrabat.Visible = true;
                //pnlSrabat.btnReNew.Click += new EventHandler(btnReNew_Click);
                //arDopPanel.Add(pnlSrabat);
                //lstvAvar.ItemActivate += new EventHandler( lstvAvar_ItemActivate );
                #endregion

                #region ������������� � ���������
                pnlOscDiag = new OscDiagPanelControl();
                splitContainer24.Panel2.Controls.Add(pnlOscDiag);                
                pnlOscDiag.Dock = DockStyle.Fill;
                ////arDopPanel.Add(pnlOscDiag);
                #endregion

                #region ������ ������� �����
                //pnlLogDev = new LogDevPanelControl();
                //splitContainer1.Panel2.Controls.Add(pnlLogDev);
                //pnlLogDev.btnReNew.Click += new EventHandler(btnReNewLogDev_Click);
                //pnlLogDev.Dock = DockStyle.Fill;
                //arDopPanel.Add(pnlLogDev);
                #endregion

                #region ������� �����
                //pnlBottomEV = new PanelBottomEventBlock();
                //splitContainer1.Panel2.Controls.Add(pnlBottomEV);
                ////��������� ������ ��� ������� ��������� ������� �����
                //pnlBottomEV.Dock = DockStyle.Fill;
                //pnlBottomEV.InitPanel(IFC, IIDDev, lstvEventBlock);

                //arDopPanel.Add(pnlBottomEV);
                #endregion

                foreach (UserControl p in arDopPanel)
                    p.Visible = false;

                #region ������������� ������ ��� ������ ������������ � �������� �� ��������� �����
                //pnlOscDiag.dtpEndData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                //pnlOscDiag.dtpEndTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                //pnlOscDiag.dtpStartData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                //ts = new TimeSpan(1, 0, 0, 0);
                //pnlOscDiag.dtpStartData.Value = pnlOscDiag.dtpStartData.Value - ts;
                //pnlOscDiag.dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                #endregion

                #region ������������� ������ ��� ������ ���������� ������� ���������� �� ��
                //pnlLogDev.dtpEndData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                //pnlLogDev.dtpEndTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                //pnlLogDev.dtpStartData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                //ts = new TimeSpan(360, 0, 0, 0);
                //pnlLogDev.dtpStartData.Value = pnlLogDev.dtpStartData.Value - ts;
                //pnlLogDev.dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                #endregion

                #region ������������� ������ ��� ������ ��������� ���������� �� ��������� �����
                pnlSrabat.dtpEndDateAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                pnlSrabat.dtpEndTimeAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                pnlSrabat.dtpStartDateAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                ts = new TimeSpan(3, 0, 0, 0);
                pnlSrabat.dtpStartDateAvar.Value = pnlSrabat.dtpStartDateAvar.Value - ts;
                pnlSrabat.dtpStartTimeAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                #endregion

                #region ������������� ������ ��� ��������� ������� �� ��������� �����
                pnlConfig.dtpEndDateConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                pnlConfig.dtpEndTimeConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                pnlConfig.dtpStartDateConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

                ts = new TimeSpan(1, 0, 0, 0);
                pnlConfig.dtpStartDateConfig.Value = pnlConfig.dtpStartDateConfig.Value - ts;
                pnlConfig.dtpStartTimeConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                #endregion
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        #endregion


		  /// <summary>
		  /// �������� ������� ArrayList � ��������� ���������� �� ����������� ����� XML
		  /// </summary>
		  /// <param name="arrVar"> ������  ArrayList
		  ///������</param>
		  /// <param name="nameFile">��� ����� XML
		  ///������</param>
        private void CreateArrayList(ArrayList arrVar, string name_arrVar)
        {
            SortedList sl = new SortedList();
            ArrayList alVal = new ArrayList();
            XDocument xd;

            // ������ XML
            xd = name_arrVar == "arrStatusFCCommand" ? XDocument.Load(nfXMLConfigFC) : XDocument.Load(fileFrmTagsDescript);

            //����� ���������� � ����
            FileStream fs = File.Create("bmrz.xio");
            StreamWriter sw = new StreamWriter(fs);

            string bitmask = String.Empty;
            StringBuilder frmtgs = new StringBuilder();

            XElement xegr = (from p in xd.Element("MT").Element("BMRZ").Element("frame").Elements()
                             where p.Name == name_arrVar
                             select p).Single();

            try
            {
                IEnumerable<XElement> xefs = xegr.Elements("formula");
                foreach (XElement xef in xefs)
                {
                    // ��������� �������� �������
                    sl["formula"] = xef.Attribute("express").Value;
                    sl["caption"] = xef.Attribute("Caption").Value;
                    sl["dim"] = xef.Attribute("Dim").Value;
                    sl["TypeOfTag"] = xef.Attribute("TypeOfTag").Value;
                    sl["TypeOfPanel"] = xef.Attribute("TypeOfPanel").Value;

                    if (!String.IsNullOrWhiteSpace((string)xef.Attribute("bitmask")))
                        bitmask = xef.Attribute("bitmask").Value;

                    TypeOfTag ToT = TypeOfTag.NaN;
                    string ToP = "";

                    sw.WriteLine(sl["caption"]);
                    sw.Flush();

                    switch ((string)sl["TypeOfTag"])
                    {
                        case "Analog":
                            ToT = TypeOfTag.Analog;
                            break;
                        case "Discret":
                            ToT = TypeOfTag.Discret;
                            break;
                        case "Combo":
                            ToT = TypeOfTag.Combo;
                            break;
                        case "No":
                            ToT = TypeOfTag.NaN;
                            break;
                        default:
                            MessageBox.Show("��� ������ ���� �������");
                            break;
                    }
                    ToP = (string)sl["TypeOfPanel"];

                    // ������ ����
                    alVal.Clear();
                    IEnumerable<XElement> xfts = xef.Elements("value");
                    StringBuilder sbtag = new StringBuilder();

                    foreach (XElement xft in xfts)
                    {
                        sbtag.Clear();
                        sbtag.Append(xft.Attribute("tag").Value);
                        if (sbtag.ToString() == "external")
                        {
                            sbtag.Clear();
                            if (slKoefRatioValue.ContainsKey(xft.Attribute("id").Value))
                                sbtag.Append(slKoefRatioValue[xft.Attribute("id").Value]);
                            else
                                sbtag.Append(xft.Attribute("DefaultValue").Value);
                        }
                        alVal.Add(sbtag.ToString());
                    }

                    if (bitmask == "" || bitmask == null)	// bitmask - ��� ������ � BCD-���������� ������� �� �����
                    {
                        frmtgs.Length = 0;
                        for (int i = 0; i < alVal.Count; i++)
                        {
                            frmtgs.Append(i.ToString() + "(" + strFC + "." + strIDDev + (string)alVal[i] + ")");
                        }
                        arrVar.Add(new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, frmtgs.ToString(), sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP));

                    }
                    else
                    {
                        if (alVal.Count == 2)
                            arrVar.Add(new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + strFC + "." + strIDDev + (string)alVal[0] + ")1(" + strFC + "." + strIDDev + (string)alVal[1] + ")", sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP, bitmask));
                        else
                            arrVar.Add(new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + strFC + "." + strIDDev + (string)alVal[0] + ")", sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP, bitmask));
                    }
                }

                xefs = xegr.Elements("simple_eval");
                foreach (XElement xef in xefs)
                {
                    sbse.Clear();
                    sbse.Append(xef.Attribute("name").Value);
                    se[sbse.ToString()] = xef.Element("value").Attribute("tag").Value;
                }

                xefs = xegr.Elements("name_tabpage_ust");
                foreach (XElement xef in xefs)
                {   // ��������� �������� ������� � �������� � ������������
                    sbse.Length = 0;
                    sbse.Append(xef.Attribute("name").Value);
                    for (int i = 0; i < tbcConfig.Controls.Count; i++)
                    {
                        if (tbcConfig.Controls[i] is TabPage && tbcConfig.Controls[i].Name == sbse.ToString())
                        {
                            tbcConfig.Controls[i].Text = xef.Element("value").Attribute("tag").Value;
                            sl_tpnameUst[sbse.ToString()] = tbcConfig.Controls[i];
                        }
                    }
                }
            }
            catch (XmlException ee)
            {
                System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " : frmBMRZ : CreateArrayList() : ������ :" + ee.Message);
                sw.Close();
                fs.Close();
            }
            sw.Close();
            fs.Close();

            //if (HMI_Settings.ClientDFE != null)
            //{ 
            //    switch(name_arrVar)
            //    {
            //      case "arrAvarSign":
            //            break;
            //      case "arrConfigSign":
            //            break;
            //              case "arrStatusDevCommand":
            //                  break;
            //              case "arrStatusFCCommand":
            //                  break;															 
            //      //case "arrStoreSign":
            //            //foreach (FormulaEval fe in arrVar)
            //            //   if (fe.addrVar < 60000)
            //            //      parent.ClientDFE.AddArrTags(this.Text, fe);
            //            //break;                        
            //      default:

            // ������������� �� ���������� ����� � DataServer
            //taglist = new List<ITag>();

            //foreach (FormulaEvalNDS fe in arrVar)
            //    taglist.Add(fe.LinkVariableNewDS);

            //HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags(taglist);
            //      break;
            //    }                  
            //}
        }

		private void TestCCforFLP( Control cc )
		{
			for( int i = 0 ; i < cc.Controls.Count ; i++ )
			{
				if( cc.Controls[i] is FlowLayoutPanel )
				{
					FlowLayoutPanel flp = ( FlowLayoutPanel ) cc.Controls[i];
					slFLP[flp.Name] = flp;
				}
				else
				{
					TestCCforFLP( cc.Controls[i] );
				}
			}
		}

        #region ���� �� Tab � ������� �����������
        /*=======================================================================*
        *   private void tabPage1_Enter( object sender, EventArgs e )
        *       ���� �� Tab � ������� �����������
        *=======================================================================*/
        private void tabPage1_Enter( object sender, EventArgs e )
        {
            tabSystem_Enter( sender, e );
            //-------------------------------------------------------------------
            //������� ���. ��� ����������� ������� �������� ���������� � ���������� ��������
            // ������� ���������� ������� (���. 0033 ...) � ������ ������������ �������������
            if( arrCurSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabPage1);
                return;
            }

    		 CreateArrayList( arrCurSign, "arrCurSign" );

           // ��������� ����������� �� �����
            for( int i = 0; i < arrCurSign.Count; i++ )
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrCurSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch (ev.ToT)
                {
                    case TypeOfTag.String:
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox(ev);
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                        bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                        usTB.txtLabelText.DataBindings.Add(bnd);
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;
                        usTB.Dim_Text = ev.Dim;
                        break;
                    case TypeOfTag.Discret:
                        chBV = new CheckBoxVar(ev);
                        chBV.checkBox1.Text = "";
                        chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        chBV.AutoSize = true;

                        Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                        bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                        chBV.checkBox1.DataBindings.Add(bndCB);
                        ev.LinkVariableNewDS.BindindTag = bndCB;
                        chBV.checkBox1.Text = ev.CaptionFE;
                        break;
                    case TypeOfTag.NaN:
                        break;
                    default:
                        MessageBox.Show("��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
			  // ��������� ��� ������ �� pnlCurrent
			  if (CurrentControlProgUst.Controls.Count != 0)
				  gbControlProgUst.Visible = true;
			  if (CurrentDirection_P.Controls.Count != 0)
				  gbDirection_P.Visible = true;

              tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabPage1);
        }
        private void tabSystem_Enter( object sender, EventArgs e )
        {
            if ( arrSystemSign.Count != 0 )
                return;
            //-------------------------------------------------------------------
            CreateArrayList( arrSystemSign, "arrSystemSign" );

            // ��������� ����������� �� �����
            for ( int i = 0; i < arrSystemSign.Count; i++ )
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrSystemSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch ( ev.ToT )
                {
                    case TypeOfTag.String:
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox( ev );
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding( "Text", ev.LinkVariableNewDS, "ValueAsString", true );
                        bnd.Format += new ConvertEventHandler( usTB.bnd_Format );
                        usTB.txtLabelText.DataBindings.Add( bnd );
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;
                        usTB.Dim_Text = ev.Dim;
                        break;
                    case TypeOfTag.Discret:
                        chBV = new CheckBoxVar( ev );
                        chBV.checkBox1.Text = "";
                        chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        chBV.AutoSize = true;

                        Binding bndCB = new Binding( "Checked", ev.LinkVariableNewDS, "ValueAsString", true );
                        bndCB.Format += new ConvertEventHandler( chBV.bnd_Format );
                        chBV.checkBox1.DataBindings.Add( bndCB );
                        ev.LinkVariableNewDS.BindindTag = bndCB;
                        chBV.checkBox1.Text = ev.CaptionFE;
                        break;
                    case TypeOfTag.NaN:
                        break;
                    default:
                        MessageBox.Show( "��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        break;
                }
            }
        }
        #endregion

        #region ���� �� Tab � ��������� �����������
        /*=======================================================================*
        *   private void tbpAvar_Enter( object sender, EventArgs e )
        *       ���� �� Tab � ��������� �����������
        *=======================================================================*/
        private void tbpAvar_Enter( object sender, EventArgs e )
        {
            tbpAvar.Height = tabControl1.Height;
            splitContainer1.Height = tabControl1.Height;
            tabControl2.Height = splitContainer1.Height;
            tabPage2.Height = tabControl2.Height;
            tabPage7.Height = tabControl2.Height;

            tbpAvar.Width = tabControl1.Width;
            splitContainer1.Width = tabControl1.Width;
            tabControl2.Width = splitContainer1.Width;
            tabPage2.Width = tabControl2.Width;
            tabPage7.Width = tabControl2.Width;

            lstvAvar.Items.Clear();
            
            AvarBD();
            //-------------------------------------------------------------------
            //������� ���. ��� ����������� ���������� � ���������� ��������
            
            if( arrAvarSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbpAvar);
                return;
            }


            CreateArrayList( arrAvarSign, "arrAvarSign" );
            
            // ��������� ����������� �� �����
            for( int i = 0; i < arrAvarSign.Count; i++ ) 
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrAvarSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch (ev.ToT)
                {
                    case TypeOfTag.String:
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox(ev);
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                        bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                        usTB.txtLabelText.DataBindings.Add(bnd);
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;
                        usTB.Dim_Text = ev.Dim;
                        break;
                    case TypeOfTag.Discret:
                        chBV = new CheckBoxVar(ev);
                        chBV.checkBox1.Text = "";
                        chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        chBV.AutoSize = true;

                        Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                        bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                        chBV.checkBox1.DataBindings.Add(bndCB);
                        ev.LinkVariableNewDS.BindindTag = bndCB;
                        chBV.checkBox1.Text = ev.CaptionFE;
                        break;
                    case TypeOfTag.NaN:
                        break;
                    default:
                        MessageBox.Show("��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }

            tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbpAvar);                       
        }
        /*=======================================================================*
        *   private void lstvAvar_ItemActivate( object sender, EventArgs e )
        *       ����� ���������� ��� ������ ���������� ������
        *=======================================================================*/
        private void lstvAvar_ItemActivate( object sender, EventArgs e )
        {
            //if( lstvAvar.SelectedItems.Count > 0 )
            //{
            //    // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            //    string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            //    SqlConnection asqlconnect = new SqlConnection(cnStr);
            //         try
            //         {
            //             asqlconnect.Open();
            //         } catch( SqlException ex )
            //         {
            //             string errorMes = "";
            //             // ���������� ���� ������������ ������
            //             foreach( SqlError connectError in ex.Errors )
            //                 errorMes += connectError.Message + " (������: " + connectError.Number.ToString() + ")" + Environment.NewLine;
            //             //parent.WriteEventToLog( 21, "��� ����� � �� (lstvAvar_ItemActivate): " + errorMes, this.Name, false, true, false ); // ������� ��� ����� � ��

            //             asqlconnect.Close();
            //             return;
            //         } catch( Exception ex )
            //         {
            //             MessageBox.Show( "��� ����� � ��������" + Environment.NewLine + ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Information );
            //             asqlconnect.Close();
            //             return;
            //         }
            //    // ������������ ������ ��� ������ �������� ���������
            //    SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    // ������� ���������
            //    // 1. ip FC
            //    SqlParameter pipFC = new SqlParameter();
            //    pipFC.ParameterName = "@IP";
            //    pipFC.SqlDbType = SqlDbType.BigInt;
            //    pipFC.Value = 0;
            //    pipFC.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( pipFC );
            //    // 2. id ����������
            //    SqlParameter pidBlock = new SqlParameter();
            //    pidBlock.ParameterName = "@id";
            //    pidBlock.SqlDbType = SqlDbType.Int;
            //         pidBlock.Value = 0;
            //    pidBlock.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( pidBlock );
            //    // 3. ����� �����
            //    SqlParameter ptimeStart = new SqlParameter();
            //    ptimeStart.ParameterName = "@dt_start";
            //    ptimeStart.SqlDbType = SqlDbType.DateTime;
            //    ptimeStart.Value = DateTime.Now;
            //    ptimeStart.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( ptimeStart );
            //    // 4. ����� �����
            //    SqlParameter ptimeFin = new SqlParameter();
            //    ptimeFin.ParameterName = "@dt_end";
            //    ptimeFin.SqlDbType = SqlDbType.DateTime;
            //    ptimeFin.Value = DateTime.Now;
            //    ptimeFin.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( ptimeFin );
            //    // 5. ��� ������
            //    SqlParameter ptypeRec = new SqlParameter();
            //    ptypeRec.ParameterName = "@type";
            //    ptypeRec.SqlDbType = SqlDbType.Int;
            //    ptypeRec.Value = 0;
            //    ptypeRec.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( ptypeRec );
            //    // 6. �� ������ �������
            //    SqlParameter pid = new SqlParameter();
            //    pid.ParameterName = "@id_record";
            //    pid.SqlDbType = SqlDbType.Int;
            //    pid.Value = lstvAvar.SelectedItems[0].Tag;
            //    pid.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( pid );

            //    // ���������� DataSet
            //    DataSet aDS = new DataSet( "ptk" );
            //    SqlDataAdapter aSDA = new SqlDataAdapter();
            //    aSDA.SelectCommand = cmd;
                
            //    //aSDA.sq
            //    aSDA.Fill( aDS);//, "DataLog" 

            //    asqlconnect.Close();

            //         PrintDataSet( aDS );
            //    // ��������� ������ �� ������
            //    DataTable dt = aDS.Tables[0];
            //    byte[] adata = ( byte[] ) dt.Rows[0]["Data"];
                
            //    // �������� ��������� ������� ������ � ��������� ����������� �� ����
            //    ParseBDPacket( adata, 60100, iIDDev );//10280
            //         aSDA.Dispose();
            //}
        }
        /// <summary>
        /// ���������� ������� ������ 
        /// �� �������� ������
        /// </summary>
        /// <param name="req"></param>
        public void reqAvar_OnReqExecuted(IRequestData req)
        {
            try
            {
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        #endregion

        #region ����������� ������ DataSet
        static void PrintDataSet( DataSet ds )
        {
            // ����� ��������� ���� �� ���� DataTable ������� DataSet
            Console.WriteLine( "������� � DataSet '{0}'. \n ", ds.DataSetName );
            foreach( DataTable dt in ds.Tables )
            {
                Console.WriteLine( "������� '{0}'. \n ", dt.TableName );
                // ����� ���� ��������
                for( int curCol = 0; curCol < dt.Columns.Count; curCol++ )
                    Console.Write( dt.Columns[curCol].ColumnName.Trim() + "\t" );
                Console.WriteLine( "\n-----------------------------------------------" );

                // ����� DataTable
                for( int curRow = 0; curRow < dt.Rows.Count; curRow++ )
                {
                    for( int curCol = 0; curCol < dt.Columns.Count; curCol++ )
                        Console.Write( dt.Rows[curRow][curCol].ToString() + "\t" );
                    Console.WriteLine();
                }
            }
        }
        #endregion

        #region ������� ��������� ����������
        private void AvarBD()
        {
            try
            {
                DataBaseReq dbs = new DataBaseReq(HMI_Settings.ProviderPtkSql, "ShowDataLog2");

                var uniDev = (uint)(iFC * 256 + iIDDev);

                // ������� ���������
                // 1. ip FC
                dbs.AddCMDParams(new DataBaseParameter("@IP", ParameterDirection.Input, SqlDbType.BigInt, 0));
                // 2. id ����������
                dbs.AddCMDParams(new DataBaseParameter("@id", ParameterDirection.Input, SqlDbType.Int, uniDev));
                // 3. ��������� �����
                TimeSpan tss = new TimeSpan(0, pnlSrabat.dtpStartDateAvar.Value.Hour - pnlSrabat.dtpStartTimeAvar.Value.Hour, pnlSrabat.dtpStartDateAvar.Value.Minute - pnlSrabat.dtpStartTimeAvar.Value.Minute, pnlSrabat.dtpStartDateAvar.Value.Second - pnlSrabat.dtpStartTimeAvar.Value.Second);
                DateTime tim = pnlSrabat.dtpStartDateAvar.Value - tss;
                dbs.AddCMDParams(new DataBaseParameter("@dt_start", ParameterDirection.Input, SqlDbType.DateTime, tim));
                // 2. �������� �����
                tss = new TimeSpan(0, pnlSrabat.dtpEndDateAvar.Value.Hour - pnlSrabat.dtpEndTimeAvar.Value.Hour, pnlSrabat.dtpEndDateAvar.Value.Minute - pnlSrabat.dtpEndTimeAvar.Value.Minute, pnlSrabat.dtpEndDateAvar.Value.Second - pnlSrabat.dtpEndTimeAvar.Value.Second);
                tim = pnlSrabat.dtpEndDateAvar.Value - tss;
                dbs.AddCMDParams(new DataBaseParameter("@dt_end", ParameterDirection.Input, SqlDbType.DateTime, tim));
                // 5. ��� ������
                // ���������� �� �������
                int tbd = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Srabat, uniDev);

                dbs.AddCMDParams(new DataBaseParameter("@type", ParameterDirection.Input, SqlDbType.Int, tbd));
                // 6. �� ������ �������
                dbs.AddCMDParams(new DataBaseParameter("@id_record", ParameterDirection.Input, SqlDbType.Int, 0));

                //dbs.DoStoredProcedure();

                // ��������� ������ �� �������
                dtA = dbs.GetTableAsResultCMD();

                if (dtA.Rows.Count == 0 && IsMesView)
                {
                    MessageBox.Show("�������� ������ �� ������� ��� ����� ���������� ���.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    IsMesView = false;
                }

                // ��������� ListView
                lstvAvar.Items.Clear();
                for (int curRow = 0; curRow < dtA.Rows.Count; curRow++)
                {
                    DateTime t = (DateTime)dtA.Rows[curRow]["TimeBlock"];
                    ListViewItem li = new ListViewItem(CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t));
                    li.SubItems.Add(dtA.Rows[curRow]["Arg1"].ToString());
                    li.Tag = dtA.Rows[curRow]["ID"];
                    lstvAvar.Items.Add(li);
                }

            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        private void btnReNew_Click(object sender, EventArgs e)
        {
            AvarBD();
        }
        #endregion
     
        #region ����� ������ pack � hex-���� � ���� fn
      private void PrintHexDump( string fn , byte[] pack)
        {
            // ������� � ���� - ������� �������
            FileStream fs = new FileStream( fn, FileMode.Append );
            StreamWriter sw = new StreamWriter( fs );

            sw.Write( Environment.NewLine ); 
            sw.Write( "*****************************" ); sw.Write( Environment.NewLine );
            sw.Write( DateTime.Now.ToString() );
            sw.Write( Environment.NewLine);
            sw.Write( "*****************************" ); sw.Write( Environment.NewLine );
            int ii = 0;
            for( ushort i = 0; i < pack.Length; i++ )
            {
                // �������� ������ ��������� ������
                sw.Write( ii.ToString( "x" ) );
                sw.Write( "    " );
                for( int jj = 0; jj < 2; jj++ )
                {
                    for( int j = 0; j < 4; j++ )
                    {
                        try
                        {
                            sw.Write( pack[ii].ToString( "x" ) + " " );
                            ii++;
                        }
                        catch
                        {
									sw.Close();
									fs.Close();
                            return;
                        }
                    }
                    sw.Write( "    " );
                }
                sw.Write( Environment.NewLine ); sw.Write( Environment.NewLine );
            }
				sw.Close();
				fs.Close();
        }
        #endregion

        #region ���� �� ������� ������������� ����������
        private void tabStore_Enter( object sender, EventArgs e )
        {
            if( arrStoreSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabStore);
                return;
            }

            CreateArrayList(arrStoreSign, "arrStoreSign");
           //--------------------
            
            // ��������� ����������� �� �����
            for( int i = 0; i < arrStoreSign.Count; i++ )
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrStoreSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch (ev.ToT)
                {
                    case TypeOfTag.String:
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox(ev);
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                        bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                        usTB.txtLabelText.DataBindings.Add(bnd);
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;
                        usTB.Dim_Text = ev.Dim;
                        break;
                    case TypeOfTag.Discret:
                        chBV = new CheckBoxVar(ev);
                        chBV.checkBox1.Text = "";
                        chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        chBV.AutoSize = true;

                        Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                        bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                        chBV.checkBox1.DataBindings.Add(bndCB);
                        ev.LinkVariableNewDS.BindindTag = bndCB;
                        chBV.checkBox1.Text = ev.CaptionFE;
                        break;
                    case TypeOfTag.NaN:
                        break;
                    default:
                        MessageBox.Show("��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabStore);
        }
        //private void btnReadStore_Click(object sender, EventArgs e)
        //{
        //    //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "IMC", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
        //    //       parent.WriteEventToLog( 35, "������� \"IMC\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );
        //    //   // ���������������� �������� ������������
        //    //   parent.WriteEventToLog( 8, iIDDev.ToString(), this.Name, true, true, false );//"������ ������� IMC - ������ �������������."
        //    //   //b_62132.FirstValue();
        //}

        private void btnReadStoreFC_Click(object sender, EventArgs e)
        {
            // ���������� ������ � ������ �������� ������������ - ����� ���������� � ������ ��
            int uniDev = iFC * 256 + iIDDev;
            int numdevfc = uniDev;

            CommonUtils.CommonUtils.WriteEventToLog(8, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "IMC", new byte[] { }, this);
        }

        private void btnResetStore_Click(object sender, EventArgs e)
        {
            //if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b06_Reset_info, parent.UserRight ) )
            //    return;

            //DialogResult dr = MessageBox.Show( "�������� ������������� ���������� �����?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
            //if( dr == DialogResult.Yes )
            //{
            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "CCD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //        parent.WriteEventToLog( 35, "������� \"CCD\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );

            //      // ���������������� �������� ������������
            //    parent.WriteEventToLog( 9, iIDDev.ToString(), this.Name, true, true, false );//"������ ������� CCD - ����� �������������."
            //}
        }

        private void btnResetStore_Click_1(object sender, EventArgs e)
        {

            if (CommonUtils.CommonUtils.IsUserActionBan(CommonUtils.CommonUtils.UserActionType.b06_Reset_info, HMI_MT_Settings.HMI_Settings.UserRight))
                return;

            DialogResult dr = MessageBox.Show("�������� ������������� ���������� �����?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            // ���������� ������ � ������ �������� ������������ - ����� ���������� � ������ ��
            int uniDev = iFC * 256 + iIDDev;
            int numdevfc = uniDev;

            CommonUtils.CommonUtils.WriteEventToLog(35, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "CCD", new byte[] { }, this);
        }

        private void btnReadMaxMeterFC_Click(object sender, EventArgs e)
        {
            // ���������� ������ � ������ �������� ������������ - ����� ���������� � ������ ��
            int uniDev = iFC * 256 + iIDDev;
            int numdevfc = uniDev;

            CommonUtils.CommonUtils.WriteEventToLog(10, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "IMD", new byte[] { }, this);
        }

        //private void btnReadMaxMeter_Click(object sender, EventArgs e)
        //{
        //    //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "IMD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
        //    //       parent.WriteEventToLog( 35, "������� \"IMD\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );
        //    //     // ���������������� �������� ������������
        //    //     parent.WriteEventToLog( 10, iIDDev.ToString(), this.Name, true, true, false );//"������ ������� IMD - ������ ���������."

        //}

        private void cbPeriodReadStore_CheckedChanged(object sender, EventArgs e)
        {
            //if( cbPeriodReadStore.Checked )
            //{
            //    lblIntervalReadStore1.Enabled = true;
            //    lblIntervalReadStore2.Enabled = true;
            //    tbIntervalReadStore.Enabled = true;
            //    tbIntervalReadStore.Text = "0";
            //}
            //else
            //{
            //    lblIntervalReadStore1.Enabled = false;
            //    lblIntervalReadStore2.Enabled = false;
            //    tbIntervalReadStore.Enabled = false;
            //    tbIntervalReadStore.Text = "0";
            //    // �������� �������, ���������� ������������� ������ ������������� ����������
            //    byte[] paramss = { 0 };

            //    //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "SPC", String.Empty, paramss, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //    //         parent.WriteEventToLog( 35, "������� \"SPC\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );
            //}
        }

        private void btnReadStoreBlock_Click(object sender, EventArgs e)
        {
            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RCD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "������� \"RCD\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );
            //   // ���������������� �������� ������������
            //   parent.WriteEventToLog( 8, iIDDev.ToString(), this.Name, true, true, false );//"������ ������� RCD - ������ �������������."
            //   //b_62132.FirstValue();
        }

        private void tbIntervalReadStore_KeyDown(object sender, KeyEventArgs e)
        {
            //if( e.KeyValue != (int)Keys.Enter )
            //    return;
            //// �������� �������, ��������������� ������������� ������ ������������� ����������
            //byte[] paramss = new byte[2];
            //byte[] temp_paramss = { Byte.Parse( tbIntervalReadStore.Text ) };

            //if( temp_paramss.Length == 1 )
            //{
            //    paramss[0] = temp_paramss[0];
            //    paramss[1] = 0;
            //}
            //else if( temp_paramss.Length == 2 )
            //    paramss = temp_paramss;

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "SPC", String.Empty, paramss, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "������� \"SPC\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );
        }

        private void btnReadMaxMeterBlock_Click(object sender, EventArgs e)
        {
            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RMD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "������� \"RMD\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );
        }

        private void btnResetMaxMeter_Click(object sender, EventArgs e)
        {
            if (CommonUtils.CommonUtils.IsUserActionBan(CommonUtils.CommonUtils.UserActionType.b06_Reset_info, HMI_MT_Settings.HMI_Settings.UserRight))
                return;

            DialogResult dr = MessageBox.Show("�������� ��������?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            // ���������� ������ � ������ �������� ������������ - ����� ���������� � ������ ��
            int uniDev = iFC * 256 + iIDDev;
            int numdevfc = uniDev;

            CommonUtils.CommonUtils.WriteEventToLog(35, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "CMD", new byte[] { }, this);
        }

        private void cbPeriodReadMaxMeter_CheckedChanged(object sender, EventArgs e)
        {
            //if( cbPeriodReadMaxMeter.Checked )
            //{
            //    lblIntervalReadMaxM1.Enabled = true;
            //    lblIntervalReadMaxM2.Enabled = true;
            //    tbIntervalReadMaxMeter.Enabled = true;
            //    tbIntervalReadMaxMeter.Text = "0";
            //}
            //else
            //{
            //    lblIntervalReadMaxM1.Enabled = false;
            //    lblIntervalReadMaxM2.Enabled = false;
            //    tbIntervalReadMaxMeter.Enabled = false;
            //    tbIntervalReadMaxMeter.Text = "0";
            //    // �������� �������, ���������� ������������� ������ ���������
            //    byte[] paramss = { 0 };

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "SPM", String.Empty, paramss, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //         parent.WriteEventToLog( 35, "������� \"SPM\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );
            //}
        }

        private void tbIntervalReadMaxMeter_KeyDown(object sender, KeyEventArgs e)
        {
            //if( e.KeyValue != ( int ) Keys.Enter )
            //    return;
            //// �������� �������, ��������������� ������������� ������ ���������
            //byte[] paramss = new byte[2];
            //byte[] temp_paramss = { Byte.Parse( tbIntervalReadMaxMeter.Text ) };

            //if( temp_paramss.Length == 1 )
            //{
            //    paramss[0] = temp_paramss[0];
            //    paramss[1] = 0;
            //}
            //else if( temp_paramss.Length == 2 )
            //    paramss = temp_paramss;

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "SPM", String.Empty, paramss, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //           parent.WriteEventToLog( 35, "������� \"SPM\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );
        }

        #endregion
        
        #region ���� �� ������� "������������ � �������"
        /// <summary>
        /// private void tbcConfig_Enter( object sender, EventArgs e )
        ///  ���� �� ������� "������������ � �������"
        /// </summary>
        private void tbcConfig_Enter( object sender, EventArgs e )
        {
            lstvConfig.Items.Clear();

            /*
             * �������� ������
             */
            foreach (UserControl p in arDopPanel)
                p.Visible = false;
            pnlConfig.Visible = true;

            pnlConfig.btnWriteUst.Enabled = false;

            //-------------------------------------------------------------------
            //������� ���. ��� ����������� ���������� � ���������� ��������
            if( arrConfigSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbpConfUst);
                return;
            }

    		 //btnWriteUst.Enabled = false;
            CreateArrayList(arrConfigSign, "arrConfigSign");

            // ��� ������ �������� ��� tabpage
            for( int i = 0; i < tbcConfig.Controls.Count; i++ )
            {
                if( tbcConfig.Controls[i] is TabPage )
                {
                    tbcConfig.Controls.RemoveAt( i );
                    i--;
                }
            }

            // ������������ �������� �������
            for( int i = 0; i < sl_tpnameUst.Count; i++ )
                tbcConfig.Controls.Add((Control) sl_tpnameUst.GetByIndex( i ) );

            // ��������� ����������� �� �����
            for( int i = 0; i < arrConfigSign.Count; i++ )
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrConfigSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                ComboBoxVar cBV;
					 switch( ev.ToT )
					 {
                     //    case TypeOfTag.Combo:
                     //       cBV = new ComboBoxVar((string[])( ( TagEval ) ( ( TagVal ) ev.arrTagVal[0] ).linkTagEval ).arrStrCB.ToArray(typeof(string)), 0);
                     //       cBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
                     //cBV.AutoSize = true;
                     //cBV.addrLinkVar = ev.addrVar;

                     //ev.OnChangeValForm += cBV.LinkSetText;
                     //ev.FirstValue();
                     //break;
                         case TypeOfTag.String:
                         case TypeOfTag.Analog:
                             usTB = new ctlLabelTextbox(ev);
                             usTB.LabelText = "";
                             usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                             usTB.AutoSize = true;

                             Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                             bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                             usTB.txtLabelText.DataBindings.Add(bnd);
                             ev.LinkVariableNewDS.BindindTag = bnd;
                             usTB.Caption_Text = ev.CaptionFE;
                             usTB.Dim_Text = ev.Dim;
                             break;
                         case TypeOfTag.Discret:
                             chBV = new CheckBoxVar(ev);
                             chBV.checkBox1.Text = "";
                             chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                             chBV.AutoSize = true;

                             Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                             bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                             chBV.checkBox1.DataBindings.Add(bndCB);
                             ev.LinkVariableNewDS.BindindTag = bndCB;
                             chBV.checkBox1.Text = ev.CaptionFE;
                             break;
                         case TypeOfTag.NaN:
                             break;
                         default:
                             MessageBox.Show("��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                             break;
                     }
            }
            tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbpConfUst);
        }

        private void UstavBD( )
        {
            DataBaseReq dbs = new DataBaseReq(HMI_Settings.ProviderPtkSql, "ShowDataLog2");
            var uniDev = (uint)(iFC * 256 + iIDDev);

            // ������� ���������
            // 1. ip FC
            dbs.AddCMDParams(new DataBaseParameter("@IP", ParameterDirection.Input, SqlDbType.BigInt, 0));
            // 2. id ����������
            dbs.AddCMDParams(new DataBaseParameter("@id", ParameterDirection.Input, SqlDbType.Int, uniDev));
            // 3. ��������� �����
            TimeSpan tss = new TimeSpan(0, pnlConfig.dtpStartDateConfig.Value.Hour - pnlConfig.dtpStartTimeConfig.Value.Hour, pnlConfig.dtpStartDateConfig.Value.Minute - pnlConfig.dtpStartTimeConfig.Value.Minute, pnlConfig.dtpStartDateConfig.Value.Second - pnlConfig.dtpStartTimeConfig.Value.Second);
            DateTime tim = pnlConfig.dtpStartDateConfig.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_start", ParameterDirection.Input, SqlDbType.DateTime, tim));
            // 2. �������� �����
            tss = new TimeSpan(0, pnlConfig.dtpEndDateConfig.Value.Hour - pnlConfig.dtpEndTimeConfig.Value.Hour, pnlConfig.dtpEndDateConfig.Value.Minute - pnlConfig.dtpEndTimeConfig.Value.Minute, pnlConfig.dtpEndDateConfig.Value.Second - pnlConfig.dtpEndTimeConfig.Value.Second);
            tim = pnlConfig.dtpEndDateConfig.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_end", ParameterDirection.Input, SqlDbType.DateTime, tim));
            // 5. ��� ������
            // ���������� �� ��������
            int tbd = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Ustavki, uniDev);
            dbs.AddCMDParams(new DataBaseParameter("@type", ParameterDirection.Input, SqlDbType.Int, tbd));
            // 6. �� ������ �������
            dbs.AddCMDParams(new DataBaseParameter("@id_record", ParameterDirection.Input, SqlDbType.Int, 0));

            //dbs.DoStoredProcedure();

            // ��������� ������ �� ��������
            dtU = dbs.GetTableAsResultCMD();

            if (dtU.Rows.Count == 0 && IsMesView)
            {
                MessageBox.Show("�������� ������ ��� ����� ���������� ���.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsMesView = false;
            }

            // ��������� ListView
            lstvConfig.Items.Clear();
            for (int curRow = 0; curRow < dtU.Rows.Count; curRow++)
            {
                DateTime t = (DateTime)dtU.Rows[curRow]["TimeBlock"];
                ListViewItem li = new ListViewItem(CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t));
                li.SubItems.Add(dtU.Rows[curRow]["Arg1"].ToString());
                li.Tag = dtU.Rows[curRow]["ID"];
                lstvConfig.Items.Add(li);
            }
        }

        private void btnReadUstFC_Click(object sender, EventArgs e)
        {
            //   btnWriteUst.Enabled = true;

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "IMP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "������� \"IMP\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );

            //   // ���������������� �������� ������������
            //   parent.WriteEventToLog( 7, iIDDev.ToString(), this.Name, true, true, false );//"������ ������� IMP - ������ �������."
            //   if( b_62002 != null )
            //       b_62002.FirstValue();
            //   if( b_62092 != null )
            //       b_62092.FirstValue();

            //pnlConfig.btnWriteUst.Enabled = true;

            // ���������� ������ � ������ �������� ������������
            // ����� ���������� � ������ ��
            int numdevfc = iFC * 256 + iIDDev;

            CommonUtils.CommonUtils.WriteEventToLog(7, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "IMP", new byte[] { }, this);
        }

        private void btnReadUstBlock_Click(object sender, EventArgs e)
        {
            //   btnWriteUst.Enabled = true;

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RCP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "������� \"RCP\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );

            //   // ���������������� �������� ������������
            //   parent.WriteEventToLog( 7, iIDDev.ToString(), this.Name, true, true, false );//"������ ������� RCP - ������ �������."
            //   if( b_62002 != null )
            //       b_62002.FirstValue();
            //   if( b_62092 != null )
            //       b_62092.FirstValue();
        }

        void btnReNewUstBD_Click(object sender, EventArgs e)
        {
            IsMesView = true;
            UstavBD();
        }

        private void lstvConfig_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                if (lstvConfig.SelectedItems.Count == 0)
                    return;

                int id_block = (int)lstvConfig.SelectedItems[0].Tag;

                // ����� ���������� � ������ ��
                int numdevfc = (int)(iFC * 256 + iIDDev);
                ArrayList arparam = new ArrayList();
                // ����� ��� ������ � ��
                arparam.Add(id_block);
                // ������ �����������
                byte[] str_cnt_in_bytes = Encoding.UTF8.GetBytes(HMI_Settings.ProviderPtkSql);
                arparam.Add(str_cnt_in_bytes);

                IRequestData req = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetData(0, (uint)numdevfc, "ArhivUstavkiBlockData", arparam, id_block);
                req.OnReqExecuted += new ReqExecuted(reqAvar_OnReqExecuted);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// private void btnWriteUst_Click( object sender, EventArgs e )
        /// ������ �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>  
        private void btnWriteUst_Click(object sender, EventArgs e)
        {
            //  if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, parent.UserRight ) )
            //      return;

            //    if( parent.isReqPassword )
            //      if( !parent.CanAction() )
            //      {
            //          MessageBox.Show( "���������� �������� ���������" );
            //          return;
            //      }

            //DialogResult dr = MessageBox.Show( "�������� �������?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
            //if( dr == DialogResult.No )
            //    return;

            //string stri;
            //TabPage tp;
            //ctlLabelTextbox ultb;
            //CheckBoxVar chbTmp;
            //ComboBoxVar cbTmp;

            //FlowLayoutPanel flp;
            //bool isUstChange = false;   // ���� ��������� ������� ��� ������������ ������������ �������
            //StringBuilder sb = new StringBuilder();
            //uint ainmemX;    // ����� � ������� memX
            //byte[] aTmp2 = new byte[2];

            //// ������ SortedList ��� ������� ����������
            //slLocal = new SortedList();
            //foreach( FC aFC in parent.KB )
            //    if( aFC.NumFC == iFC )
            //    {
            //        foreach( TCRZADirectDevice aDev in aFC )
            //            if( aDev.NumDev == iIDDev )
            //            {
            //                slLocal = aDev.CRZAMemDev;
            //                break;
            //            }
            //        break;
            //    }
            //// ��������� ����� � ��������� ��� �������������
            ////BinaryReader memDevBlock = ( BinaryReader ) slLocal[62000];

            //// ������ ������ � ������ 
            //     //byte[] memXt = new byte[( ( byte[] ) slLocal[62000] ).Length];
            //    //System.Buffer.BlockCopy( brP, 6, memX, 0, brP.Length - 6 );

            //// ������ ������ � ������ 
            ////memDevBlock.BaseStream.Position = 0;
            //     int lenpack = 0;
            //     try
            //     {
            //         lenpack = BitConverter.ToInt16( ( byte[] ) slLocal[62000], 0 );
            //     } catch( ArgumentNullException ex )
            //     {
            //         MessageBox.Show( "��� ������ ��� ������. \n���������� ������� ��������.", "���������", MessageBoxButtons.OK, MessageBoxIcon.Information );
            //         return;
            //     }

            //     short numdev = BitConverter.ToInt16( ( byte[] ) slLocal[62000] , 2 );

            //     ushort add10 = BitConverter.ToUInt16( ( byte[] ) slLocal[62000] , 4 );	//������ ����� ����� ������

            //    //int lenpack = ( short ) memDevBlock.ReadInt16();
            //    //short numdev = ( short ) memDevBlock.ReadUInt16();
            //    //ushort add10 = ( ushort ) memDevBlock.ReadInt16();	//������ ����� ����� ������

            //byte[] memX = new byte[lenpack - 6];
            //    System.Buffer.BlockCopy( ( byte[] ) slLocal[62000] , 6, memX, 0, (( byte[] ) slLocal[62000] ).Length - 6 );

            ////memDevBlock.Read( memX, 0, lenpack - 6 );

            //for( int i = 0; i < tbcConfig.Controls.Count; i++ )
            //{
            //    if( tbcConfig.Controls[i] is TabPage )
            //    {
            //        tp = ( TabPage ) tbcConfig.Controls[i];
            //        for( int j = 0; j < tp.Controls.Count; j++ )
            //        {
            //            if( tp.Controls[j] is FlowLayoutPanel )
            //            {
            //                flp = ( FlowLayoutPanel ) tp.Controls[j];
            //                for( int n = 0; n < flp.Controls.Count; n++ )
            //                {
            //                    if( flp.Controls[n] is ctlLabelTextbox )
            //                    {
            //                        ultb = ( ctlLabelTextbox ) flp.Controls[n];
            //                        if( ultb.isChange )
            //                        {
            //                                        CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 );
            //                                        //StrToBCD_Field( ultb, memX );
            //                                        isUstChange = true;	
            //                        }
            //                    }
            //                    else if( flp.Controls[n] is ComboBoxVar )
            //                    {
            //                        cbTmp = ( ComboBoxVar ) flp.Controls[n];
            //                        if( cbTmp.isChange )
            //                        {
            //                            isUstChange = true;
            //                            cbTmp.isChange = false;  // ���������� ������� ��������� � ����������� ComboBoxVar'�
            //                            // ���������� ��������� �� ComboBoxVar'�� � �������� ����� (������������ ������ memX)
            //                            uint a = cbTmp.addrLinkVar; // ����� ����������
            //                            // ������� ��������
            //                            int st = cbTmp.cbVar.SelectedIndex;
            //                            byte[] bst = new byte[4];
            //                            bst = BitConverter.GetBytes(st);
            //                            Buffer.BlockCopy( bst, 0, aTmp2, 0, 2 );
            //                            Array.Reverse( aTmp2 );
            //                            // ���������� ���������
            //                            ainmemX = ( a - 62000 ) * 2;
            //                            Buffer.BlockCopy( aTmp2, 0, memX, ( int ) ainmemX, 2 );
            //                        }
            //                    }
            //                    else if( flp.Controls[n] is CheckBoxVar )
            //                    {
            //                        chbTmp = ( CheckBoxVar ) flp.Controls[n];
            //                        if( chbTmp.isChange )
            //                        {
            //                            isUstChange = true;
            //                            chbTmp.isChange = false;    // ���������� ������� ��������� � ����������� CheckBoxVar'�
            //                            // �������� ������� ���� �� ��������� �������
            //                            ainmemX = ( chbTmp.addrLinkVar - 62000 ) * 2;   // ��� �����
            //                            //aTmp2 = new byte[2];
            //                            Buffer.BlockCopy( memX, (int)ainmemX, aTmp2, 0, 2 );
            //                            string bitmask = chbTmp.addrLinkVarBitMask;
            //                            UInt16 ibitmask = Convert.ToUInt16( chbTmp.addrLinkVarBitMask, 16 );
            //                            Array.Reverse( aTmp2 );
            //                            UInt16 rezbit = BitConverter.ToUInt16(aTmp2,0);
            //                            if( chbTmp.checkBox1.Checked == true )
            //                                rezbit = Convert.ToUInt16( rezbit | ibitmask );
            //                            else
            //                            {
            //                                UInt16 ti = (UInt16)~ibitmask; //Convert.ToUInt16()
            //                                rezbit = Convert.ToUInt16( rezbit & ~ibitmask );
            //                            }
            //                            // �������� �� �����
            //                            aTmp2 = BitConverter.GetBytes( rezbit );
            //                            Array.Reverse( aTmp2 );
            //                            Buffer.BlockCopy( aTmp2, 0, memX, ( int ) ainmemX, 2 );
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            ////------------------------------
            //// ���������� ��� ������ �������
            //        //for( int n = 0 ; n < pnlConfig.Controls.Count ; n++ )
            //    for( int n = 0 ; n < Config_BottomPanel.Controls.Count ; n++ )
            //            //if( pnlConfig.Controls[n] is ctlLabelTextbox )
            //        if( ( Config_BottomPanel.Controls[n] as ctlLabelTextbox ) != null)
            //        {
            //            ultb = ( ctlLabelTextbox ) Config_BottomPanel.Controls[n];
            //                if( ultb.Name == "ctlTimeUstavkiSbros" )
            //                    continue;

            //                if( ultb.isChange )
            //                {
            //                    CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 );
            //                    //StrToBCD_Field( ultb, memX );
            //                    isUstChange = true;						
            //                }
            //        }
            ////------------------------------
            //if( !isUstChange )
            //{
            //    MessageBox.Show("������� �� ����������. \n���������� ������� ��������.","���������", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //// ��������� ����� � ������� ��� �������� ��������� �������
            //byte[] memXOut = new byte [memX.Length];
            //Buffer.BlockCopy( memX, 4, memXOut, 4, memX.Length - 4);  // Handle ���� �������

            ////if( parent.newKB.ExecuteCommand( iFC, iIDDev, "WCP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            ////        parent.WriteEventToLog( 35, "������� \"WCP\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true, true, false );
            ////    // ���������������� �������� ������������
            ////    parent.WriteEventToLog( 6, iIDDev.ToString(), this.Name, true, true, false );			//"������ ������� WCP - ������ �������."
            //isUstChange = false;
        }
        //private void dtpStartDateConfig_ValueChanged(object sender, EventArgs e)
        //{
        //    UstavBD();
        //}
        #endregion

        #region ������� ������������� � ���������
        /// <summary>
        /// ������� ������������� � ���������
        /// </summary>
        private void tabPage5_Enter( object sender, EventArgs e )
        {
            if (dgvOscill.RowCount != 0 || dgvDiag.RowCount != 0)
                return;

            foreach (UserControl p in arDopPanel)
                p.Visible = false;

            pnlOscDiag.Visible = true;

            //DiagBD();
            OscBD();
        }
        /// <summary>
        /// ��������� ������������ �� ����
        /// </summary>
        private void OscBD( )
        {
            if (oscdg == null)
                oscdg = new OscDiagViewer();

            oscdg.IdFC = iFC;
            oscdg.IdDev = iIDDev;
            oscdg.DTStartData = pnlOscDiag.dtpStartData.Value;
            oscdg.DTStartTime = pnlOscDiag.dtpStartData.Value;
            oscdg.DTEndData = pnlOscDiag.dtpEndData.Value;
            oscdg.DTEndTime = pnlOscDiag.dtpEndData.Value;
            oscdg.TypeRec = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Osc, (uint)(iFC * 256 + iIDDev) );// TypeBlockData.TypeBlockData_OscSirius;GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Osc, iFC, iIDDev);            

            // ��������� ������ �� ��������������
            dtO = oscdg.Do_SQLProc();

            for (int curRow = 0; curRow < dtO.Rows.Count; curRow++)
            {
                int i = dgvOscill.Rows.Add();   // ����� ������
                dgvOscill["clmChBoxOsc", i].Value = false;
                dgvOscill["clmBlockNameOsc", i].Value = dtO.Rows[curRow]["BlockName"];
                dgvOscill["clmBlockIdOsc", i].Value = dtO.Rows[curRow]["BlockID"];
                dgvOscill["clmBlockTimeOsc", i].Value = dtO.Rows[curRow]["TimeBlock"];
                dgvOscill["clmCommentOsc", i].Value = dtO.Rows[curRow]["Comment"];
                dgvOscill["clmID", i].Value = dtO.Rows[curRow]["ID"];
            }
        }
        /// <summary>
        /// ��������� �������� �� ����
        /// </summary>
        private void DiagBD( )
        {
            if (oscdg == null)
                oscdg = new OscDiagViewer();

            oscdg.IdFC = iFC;
            oscdg.IdDev = iIDDev;
            oscdg.DTStartData = pnlOscDiag.dtpStartData.Value;
            oscdg.DTStartTime = pnlOscDiag.dtpStartData.Value;
            oscdg.DTEndData = pnlOscDiag.dtpEndData.Value;
            oscdg.DTEndTime = pnlOscDiag.dtpEndData.Value;
            oscdg.TypeRec = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Diagramm, (uint)(iFC * 256 + iIDDev) );

            // ��������� ������ �� ����������
            dtG = oscdg.Do_SQLProc();
            for (int curRow = 0; curRow < dtG.Rows.Count; curRow++)
            {
                int i = dgvDiag.Rows.Add();   // ����� ������
                dgvDiag["clmChBoxDiag", i].Value = false;
                dgvDiag["clmBlockNameDiag", i].Value = dtG.Rows[curRow]["BlockName"];
                dgvDiag["clmBlockIdDiag", i].Value = dtG.Rows[curRow]["BlockID"];
                dgvDiag["clmBlockTimeDiag", i].Value = dtG.Rows[curRow]["TimeBlock"];
                dgvDiag["clmCommentDiag", i].Value = dtG.Rows[curRow]["Comment"];
                dgvDiag["clmIDDiag", i].Value = dtG.Rows[curRow]["ID"];

                //dgvDiag["clmViewDiag", i].Value = "���������";
            }
        }
        private void btnReNewOscDg_Click(object sender, EventArgs e)
        {
            DiagBD();
            OscBD();
        }
        private void dgvOscill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewCell de;

            if (e.ColumnIndex == 0)
            {
                dgvOscill[e.ColumnIndex, e.RowIndex].Value = (bool)dgvOscill[e.ColumnIndex, e.RowIndex].Value ? false : true;
                btnUnionOsc.Enabled = true;
                return;
            }
            else if (e.ColumnIndex != 5)
                return;

            asb.Clear();
            btnUnionOsc.Enabled = false;

            // ���������� ��� ������
            for (int i = 0; i < dtO.Rows.Count; i++)
                dgvOscill[0, i].Value = false;

            try
            {
                de = dgvOscill["clmID", e.RowIndex];
            }
            catch
            {
                MessageBox.Show("dgvOscill_CellContentClick - ����������");
                return;
            }

            int ide = (int)de.Value;

            oscdg.ShowOSCDg(0, dtO, ide);
        }
        private void dgvDiag_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewCell de;

            if (e.ColumnIndex == 0)
            {
                dgvDiag[e.ColumnIndex, e.RowIndex].Value = (bool)dgvDiag[e.ColumnIndex, e.RowIndex].Value ? false : true;
                btnUnionOsc.Enabled = true;
                return;
            }
            else if (e.ColumnIndex != 5)
                return;

            asb.Clear();
            btnUnionOsc.Enabled = false;

            // ���������� ��� ������
            for (int i = 0; i < dtG.Rows.Count; i++)
                dgvDiag[0, i].Value = false;

            try
            {
                de = dgvDiag["clmIDDiag", e.RowIndex];
            }
            catch
            {
                MessageBox.Show("dgvDiag_CellContentClick - ����������");
                return;
            }

            int ide = (int)de.Value;

            /*
             * ������ �������� ����� DS,
             * ������� ��� ��������� ��������� ������ ��������� (0)
             * � ���������� ����� ��������� �������� ����� �� ������ �����
             * ����� �������� �������� ����� DS
             */
            oscdg.ShowOSCDg(0, dtG, ide);
        }
        /// <summary>
        /// ���������� �������������
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            //ArrayList asb = new ArrayList();    // ��� �������� ���� ������
            //string ifa; ;
            //StringBuilder sb;
            //byte[] arrO = null;
            //char[] sep = { ' ', '-' };
            //string[] sp;

            //// ����������� ������ � ������� dbO, ������� ����������, ��������� �����, �������� fastview
            //for (int curRow = 0; curRow < dtO.Rows.Count; curRow++)
            //{
            //    if ((bool)dgvOscill[0, curRow].Value == true)
            //    {
            //        // ��������� ����, ���������� ��� � �������
            //        arrO = (byte[])dtO.Rows[curRow]["Data"];

            //        // ��������� ��� �����
            //        ifa = (string)dtO.Rows[curRow]["BlockName"] + "_" + curRow.ToString() + ".osc";

            //        // ������� �������
            //        sp = ifa.Split(sep);
            //        sb = new StringBuilder();
            //        for (int i = 0; i < sp.Length; i++)
            //        {
            //            sb.Append(sp[i]);
            //        }
            //        // ��������� ��� � �������
            //        asb.Add(sb);
            //        // ������� ����
            //        FileStream f = File.Create(Environment.CurrentDirectory.ToString() + "\\" + sb.ToString());
            //        f.Write(arrO, 0, arrO.Length);
            //        f.Close();
            //    }
            //}
            //// ��������� fastview
            //Process prc = new Process();
            //sb = new StringBuilder();
            //foreach (StringBuilder s in asb)
            //{
            //    sb.Append(s.ToString());
            //    sb.Append(" ");
            //}
            //prc.StartInfo.FileName = Environment.CurrentDirectory.ToString() + "\\Fastview\\fastview.exe";
            //prc.StartInfo.Arguments = "-o " + sb.ToString();//+ "/"
            //prc.Start();
        }
        /// <summary>
        /// ���������� ���������
        /// </summary>
        private void btnUnionDiag_Click(object sender, EventArgs e)
        {
            //ArrayList asb = new ArrayList();    // ��� �������� ���� ������
            //string ifa; ;
            //StringBuilder sb;
            //byte[] arrO = null;
            //char[] sep = { ' ', '-' };
            //string[] sp;

            //// ����������� ������ � ������� dbO, ������� ����������, ��������� �����, �������� fastview
            //for (int curRow = 0; curRow < dtG.Rows.Count; curRow++)
            //{
            //    if ((bool)dgvDiag[0, curRow].Value == true)
            //    {
            //        // ��������� ����, ���������� ��� � �������
            //        arrO = (byte[])dtG.Rows[curRow]["Data"];

            //        // ��������� ��� �����
            //        ifa = (string)dtG.Rows[curRow]["BlockName"] + "_" + curRow.ToString() + ".dgm";

            //        // ������� �������
            //        sp = ifa.Split(sep);
            //        sb = new StringBuilder();
            //        for (int i = 0; i < sp.Length; i++)
            //        {
            //            sb.Append(sp[i]);
            //        }
            //        // ��������� ��� � �������
            //        asb.Add(sb);
            //        // ������� ����
            //        FileStream f = File.Create(Environment.CurrentDirectory.ToString() + "\\" + sb.ToString());
            //        f.Write(arrO, 0, arrO.Length);
            //        f.Close();
            //    }
            //}
            //// ��������� fastview
            //Process prc = new Process();
            //sb = new StringBuilder();
            //foreach (StringBuilder s in asb)
            //{
            //    sb.Append(s.ToString());
            //    sb.Append(" ");
            //}
            //prc.StartInfo.FileName = Environment.CurrentDirectory.ToString() + "\\Fastview\\fastview.exe";
            //prc.StartInfo.Arguments = "-m " + sb.ToString();//+ "/"
            //prc.Start();
        }
        #endregion

        private void dtpStartData_ValueChanged( object sender, EventArgs e )
        {
            // ������� ���������� �������
            dgvOscill.Rows.Clear();
            OscBD();
            DiagBD();
        }


        /// <summary>
        /// mnuPrint_Click( object sender, EventArgs e )
        ///     ���������� ������ ���� ��������� ��������
        /// </summary>
        private void mnuPageSetup_Click( object sender, EventArgs e )
        {
            //parent.mnuPageSetup_Click( sender, e );
        }
        /// <summary>
        /// mnuPrint_Click( object sender, EventArgs e )
        ///     ���������� ������ ���� ��������������� ��������
        /// </summary>
        private void mnuPrintPreview_Click( object sender, EventArgs e )
        {
            //PrintArr();
            //parent.mnuPrintPreview_Click( sender, e );
        }
        /// <summary>
        /// mnuPrint_Click( object sender, EventArgs e )
        ///     ���������� ������ ���� ������
        /// </summary>
        private void mnuPrint_Click( object sender, EventArgs e )
        {
            //PrintArr();
            //p//arent.mnuPrint_Click( sender, e );
        }
        /// <summary>
        /// PrintArr()
        ///     ������ ������� ����������
        /// </summary>
        //private void PrintArr()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    float f_val;
        //    int i_val;
        //    string t_val = "";
        //    ArrayList arCurPrt = new ArrayList();

        //    object val;

        //    // ���������� �������� �������
        //    TabPage tp_sel = tabControl1.SelectedTab;

        //    sb.Length = 0;
            
        //    switch(tp_sel.Text)
        //    {
        //        case "������� ����������":
        //            // ��������� ��������� ��������
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (������� ����������)" );
        //            sb.Append( "\n========================================================================\n" );
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrCurSign;
        //            break;
        //        case "���������� �� �������":
        //            // ��������� ��������� ��������
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (���������� �� �������)" );
        //            sb.Append( "\n========================================================================\n" );
                    
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrAvarSign;
        //            break;
        //        case "������������� ����������":
        //            // ��������� ��������� ��������
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (������������� ����������)" );
        //            sb.Append( "\n========================================================================\n" );
                    
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrStoreSign;
        //            break;
        //        case "������������ � �������":
        //            // ��������� ��������� ��������
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (������������ � �������)" );
        //            sb.Append( "\n========================================================================\n" );
                    
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrConfigSign;
        //            break;
        //        case "�������":
        //            // ��������� ��������� ��������
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (�������)" );
        //            sb.Append( "\n========================================================================\n" );
                    
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrSystemSign;
        //            break;
        //        case "��������� ���������� � ������":
        //            // ��������� ��������� ��������
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (��������� ���������� � ������)" );
        //            sb.Append( "\n========================================================================\n" );
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrStatusDevCommand;
        //            break;
        //        default:
        //            break;
        //    }                

        //    for( int i = 0; i < arCurPrt.Count; i++ )
        //    {
        //        FormulaEval ev = ( FormulaEval ) arCurPrt[i];

        //        switch(ev.ToT)
        //        {
        //            case TypeOfTag.Analog:
        //                val = ev.tRezFormulaEval.Value;

        //                if( val is float )
        //                {
        //                    f_val = ( float ) ev.tRezFormulaEval.Value;
        //                    t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
        //                }
        //                else if( val is short )
        //                {
        //                    i_val = ( Int16 ) ev.tRezFormulaEval.Value;
        //                    t_val = i_val.ToString();
        //                }
        //                else if( val is int )
        //                {
        //                    i_val = ( Int32 ) ev.tRezFormulaEval.Value;
        //                    t_val = i_val.ToString();
        //                }
        //                else if( val is string )
        //                { 
        //                    t_val = (string) ev.tRezFormulaEval.Value;
        //                }
        
        //                sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
        //                break;
        //            case TypeOfTag.Discret:
        //                sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
        //                break;
        //            case TypeOfTag.Combo:
        //                sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
        //                break;
        //            default:
        //                continue;
        //        }
        //        sb.Append( " \n " );
        //    }
        //    parent.prt.rtbText.AppendText( sb.ToString() );            
        //}
        /// <summary>
        /// sbForSimpleVar(StringBuilder sb)
        ///     ������������ ������ ��� ������ ��� ��������� ����������
        /// </summary>
        //private void sbForSimpleVar(StringBuilder sb, FormulaEval b_xxx)
        //{
        //    float f_val;
        //    int i_val;
        //    string t_val = "";
        //    FormulaEval ev = ( FormulaEval ) b_xxx;
        //    object val;

        //    if( b_xxx == null )
        //        return;

        //    switch( ev.ToT )
        //    {
        //        case TypeOfTag.NoN:
        //            val = ev.tRezFormulaEval.Value;

        //            if( val is float )
        //            {
        //                f_val = ( float ) ev.tRezFormulaEval.Value;
        //                t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
        //            }
        //            else if( val is short )
        //            {
        //                i_val = ( Int16 ) ev.tRezFormulaEval.Value;
        //                t_val = i_val.ToString();
        //            }
        //            else if( val is ushort )
        //            {
        //                i_val = ( UInt16 ) ev.tRezFormulaEval.Value;
        //                t_val = i_val.ToString();
        //            }
        //            else if( val is int )
        //            {
        //                i_val = ( Int32 ) ev.tRezFormulaEval.Value;
        //                t_val = i_val.ToString();
        //            }
        //            else if( val is string )
        //            {
        //                t_val = ( string ) ev.tRezFormulaEval.Value;
        //            }

        //            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
        //            break;
        //        case TypeOfTag.Analog:
        //            val = ev.tRezFormulaEval.Value;

        //            if( val is float )
        //            {
        //                f_val = ( float ) ev.tRezFormulaEval.Value;
        //                t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
        //            }
        //            else if( val is short )
        //            {
        //                i_val = ( Int16 ) ev.tRezFormulaEval.Value;
        //                t_val = i_val.ToString();
        //            }
        //            else if( val is int )
        //            {
        //                i_val = ( Int32 ) ev.tRezFormulaEval.Value;
        //                t_val = i_val.ToString();
        //            }
        //            else if( val is string )
        //            {
        //                t_val = ( string ) ev.tRezFormulaEval.Value;
        //            }

        //            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
        //            break;
        //        case TypeOfTag.Discret:
        //            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
        //            break;
        //        case TypeOfTag.Combo:
        //            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
        //            break;
        //        default:
        //            break;
        //    }
        //    sb.Append( " \n " );
        //}

        //private void btnResetValues_Click( object sender, EventArgs e )
        //{
        //    //btnWriteUst.Enabled = false;
        //    //parent.newKB.ResetGroup(iFC, iIDDev, 14);
        //}
		
		#region ���� �� ������� � ��������
		private void tabLog_Enter( object sender, EventArgs e )
		{
			// ������������� ������ ������ �������
         //parent.newKB.ExecuteCommand( iFC, iIDDev, "IME", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent );
			//-------------------------------------------------------------------
			//������� ���. ��� ����������� ������� �������� ���������� � ���������� ��������
			// ������� ���������� ������� (���. 0033 ...) � ������ ������������ �������������
			if( arrSystemSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabLog);
                return;
            }

			CreateArrayList( arrSystemSign, "arrSystemSign" );

			// ��������� ����������� �� �����
			for( int i = 0 ; i < arrSystemSign.Count ; i++ )
			{
                FormulaEvalNDS ev = (FormulaEvalNDS)arrSystemSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch (ev.ToT)
                {
                    case TypeOfTag.String:
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox(ev);
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                        bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                        usTB.txtLabelText.DataBindings.Add(bnd);
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;
                        usTB.Dim_Text = ev.Dim;
                        break;
                    case TypeOfTag.Discret:
                        chBV = new CheckBoxVar(ev);
                        chBV.checkBox1.Text = "";
                        chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        chBV.AutoSize = true;

                        Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                        bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                        chBV.checkBox1.DataBindings.Add(bndCB);
                        ev.LinkVariableNewDS.BindindTag = bndCB;
                        chBV.checkBox1.Text = ev.CaptionFE;
                        break;
                    case TypeOfTag.NaN:
                        break;
                    default:
                        MessageBox.Show("��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabLog);            		
		}
        private void button4_Click_1(object sender, EventArgs e)
        {
            // ������������� ������ ������ �������
            // parent.newKB.ExecuteCommand( iFC, iIDDev, "IME", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent );
        }
        #endregion

      private void btnAck_Click( object sender, EventArgs e )
      {
         //if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b02_ACK_Signaling, parent.UserRight ) )
         //   return;

         //ConfirmCommand dlg = new ConfirmCommand( );
         //dlg.label1.Text = "�����������?";

         //if( !( DialogResult.OK == dlg.ShowDialog( ) ) )
         //   return;
         //// ��������� �������� �� ������������
         //Console.WriteLine( "��������� ������� \"�����������\" ��� ����������: {0}; id: {1}", "����", iIDDev );
         //// ������ � ������
         ////parent.WriteEventToLog( 20, strIDDev, "����", true, true, false );

         ////if( parent.newKB.ExecuteCommand( iFC, iIDDev, "ECC", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
         ////   parent.WriteEventToLog( 35, "������� \"�����������\" ���� � ����. ���������� - "
         ////      + strFC + "." + strIDDev, "����", true, true, false );
      }
      /// <summary>
      /// ��� ������������ ����� 
      /// �������, ����� �������������� ��������
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tabControl1_Selected(object sender, TabControlEventArgs e)
      {
          HMI_MT_Settings.HMI_Settings.HMIControlsTagReNew(tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe);
          tpCurrent.Tag = false; // ������� ������� �� ����� ��� ������ TabPage
          tpCurrent = e.TabPage;  // �������� ����� ������� �������
      }
      /// <summary>
      /// ������������� �����
      /// </summary>
      public uint Guid { get { return (uint)( iFC * 256 + iIDDev ); } }
    }
}
        