/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	��������: ����� ��� ������ � ������ ���� ��-14-31-12.                                                           
 *                                                                             
 *	����                     : frmBMRZVV14_31_12.cs                                         
 *	��� ��������� �����      : 
 *	������ �� ��� ���������� : �#, Framework 2.0                                
 *	�����������              : ���� �.�.                                        
 *	���� ������ ����������   : xx.04.2007                                       
 *	���� (v1.0)              :                                                  
 *******************************************************************************
 * ���������:
 * 1. ����(�����): ...c���������...
 *#############################################################################*/

using System;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Calculator;
using System.Collections;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Globalization;
using LabelTextbox;
using CommonUtils;
using System.Linq;
using System.Xml.Linq;
using InterfaceLibrary;
using HMI_MT_Settings;
using DataBaseLib;
using OscillogramsLib;

namespace DeviceFormLib
{
	public partial class frmBMRZ : Form, IDeviceForm
	{
        #region ���� � ����������
        //private Form parent;//MainForm
        XDocument xdoc;
        SortedList slFormElements; // ������ ���������, �������� ������� ����� ����� �������� (��. Device.cfg)
        int iFC;            // ����� �� �������������
        string strFC;       // ����� �� ������
        int iIDDev;         // ����� ���������� �������������
        string strIDDev;    // ����� ���������� ������
        //string nfXMLConfig; // ��� ����� � ��������� 
        /// <summary>
        ///  ��� ����� � ������ ������� ���� ����������
        /// </summary>
        string folderDevDescrPattern;
        /// <summary>
        /// ��� ����� � �������� �����
        /// </summary>
        string fileFrmTagsDescript;
        string nfXMLConfigFC; // ��� ����� � ��������� ����
        // ������ �������������� �������
        ArrayList arDopPanel;

        ArrayList arrAvarSign = new ArrayList();
        ArrayList arrCurSign = new ArrayList();
        ArrayList arrSystemSign = new ArrayList();
        ArrayList arrStoreSign = new ArrayList();
        ArrayList arrMaxMeterSign = new ArrayList();
        ArrayList arrConfigSign = new ArrayList();
        ArrayList arrStatusDevCommand = new ArrayList();
        ArrayList arrStatusFCCommand = new ArrayList();

        ushort iclm = 16;  // ����� ������� � �����
        SortedList slLocal;
        SortedList se = new SortedList();
        SortedList sl_tpnameUst = new SortedList();
        StringBuilder sbse = new StringBuilder();

        DataTable dtO;  // ������� � ���������������
        DataTable dtG;  // ������� � �����������
        DataTable dtA;  // ������� � ��������
        DataTable dtU;  // ������� � ���������

        SortedList slFLP = new SortedList();	// ��� �������� ��� � FlowLayoutPanel

        OscDiagViewer oscdg;
        /// <summary>
        /// ������ ����� ��� ��������/�������
        /// </summary>
	    List<ITag> taglist;
        /// <summary>
        /// ������ ������� ����� �� ��������
        /// </summary>
        SortedDictionary<string,List<ITag> > slTagListByTabPages = new SortedDictionary<string,List<ITag>>();

        /// <summary>
        /// ������ ���� ������. �� ����� PrgDevCFG.cdp 
        /// ��� ������� ����������
        /// </summary>
        private SortedList<string, string> slKoefRatioValue = new SortedList<string, string>();
        /// <summary>
        /// ���� - ��������� �� ��������� �� ��������� �������� ������� 
        /// ��� ���
        /// </summary>
        bool IsMesView = false;
        #endregion

        #region �����������
        public frmBMRZ(/*Form MainForm linkMainForm,*/ int iFC, int iIDDev, /*int inumLoc,*/ string fXML, string ftagsxmldescript)
		{
			InitializeComponent();
			try
			{
                InitInterfaceElementsClick();

                //parent = linkMainForm;
                this.iFC = iFC;                 // ����� �� �������������
                strFC = iFC.ToString();         // ����� �� ������
                this.iIDDev = iIDDev;           // ����� ���������� �������������
                strIDDev = iIDDev.ToString();   // ����� ���������� ������
                folderDevDescrPattern = fXML;
                fileFrmTagsDescript = ftagsxmldescript;
                string TypeName = String.Empty;
                string nameR = String.Empty;
                string nameELowLevel = String.Empty;
                string nameEHighLevel = String.Empty;

                slFormElements = new SortedList();

                // ��������� ������ ������� � �������� ��
                arDopPanel = new ArrayList();
                arDopPanel.Add(this.pnlCurrent);
                arDopPanel.Add(this.pnlAvar);
                arDopPanel.Add(this.pnlSystem);
                arDopPanel.Add(this.pnlStore);
                arDopPanel.Add(this.pnlConfig);
                arDopPanel.Add(this.pnlOscDiag);
                arDopPanel.Add(this.pnlStatusSHASU);


                foreach (Panel p in arDopPanel)
                    p.Visible = false;

                /*
                 * �������� �������� ����� � ��������� ����������
                 * ��� ����� ���������� ���� PrgDevCFG.cdp ���������
                 */
                XElement xedev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription(0, "MOA_ECU", string.Format("{0}.{1}", strFC, strIDDev));
                xedev = xedev.Element("DescDev");   // ����������

                                TypeName = xedev.Element("TypeName").Value;
                                nameR = xedev.Element("nameR").Value;
                                nameELowLevel = xedev.Element("nameELowLevel").Value;
                                nameEHighLevel = xedev.Element("nameEHighLevel").Value;
                                this.Text = nameR + " ( ��.� " + iIDDev.ToString() + " )" + xedev.Element("DescDev").Value;
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}

        /// <summary>
        /// � ���� ������� ����������� ����� ��
        /// ��������� ���������� � ����� �� ���������
        /// </summary>
        public void InitInterfaceElementsClick()
        {
            this.FormClosing +=new FormClosingEventHandler(frmBMRZ_FormClosing);
            tabPage1.Enter +=new EventHandler(tabPage1_Enter);
            tbpAvar.Enter += new EventHandler(tbpAvar_Enter);
            lstvAvar.ItemActivate +=new EventHandler(lstvAvar_ItemActivate);
            //btnReNew.Click += new EventHandler(btnReNew_Click);
            tabStore.Enter += new EventHandler(tabStore_Enter);
            tbpConfUst.Enter += new EventHandler(tbcConfig_Enter);
            //btnReadUstFC.Click += new EventHandler(btnReadUstFC_Click);
            lstvConfig.ItemActivate += new EventHandler(lstvConfig_ItemActivate);
            //btnReNewUstBD.Click += new EventHandler(btnReNewUstBD_Click);
            
            //btnWriteUst.Click += new EventHandler(btnWriteUst_Click);
            //btnResetValues.Click += new EventHandler(btnResetValues_Click);
            tabPage5.Enter += new EventHandler(tabPage5_Enter);
            tabSystem.Enter += new EventHandler(tabSystem_Enter);

            dtpStartData.ValueChanged += new EventHandler(dtpStartData_ValueChanged);
            dgvOscill.CellContentClick += new DataGridViewCellEventHandler(dgvOscill_CellContentClick);
            dgvDiag.CellContentClick += new DataGridViewCellEventHandler(dgvDiag_CellContentClick);
            //btnReNewOD.Click += new EventHandler(btnReNewOD_Click);
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
                switch(typetabpage)
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

        /// <summary>
        /// �������� �����
        /// </summary>
        private void frmBMRZVV14_31_12_Load( object sender, EventArgs e )
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

            //this.Width = parent.Width;
            //this.Height = parent.Height;

            //tabControl1.Width = this.Width - 10;
            //tabControl1.Height = parent.ClientSize.Height - parent.statusStrip1.Height - pnlCurrent.Height - parent.menuStrip1.Height - parent.tabForms.Height - 10;//parent.panelMes.Height - 

            SetElementsFormFeatures( folderDevDescrPattern );
        }

        /// <summary>
        /// ���������� ����������� ��������� ����� �� ��������� Device.cfg (��. ��� � - 00)
        /// </summary>
        /// <param Name="pathtoDevCFG">���� � ����� � ��������� ���������� - ���� Device.cfg</param>
        private void SetElementsFormFeatures( string pathtoDevCFG )
        {
           xdoc = XDocument.Load( pathtoDevCFG + Path.DirectorySeparatorChar + "Device.cfg" );
           // ���� �������� ����� �� Name
           if ( String.IsNullOrEmpty( ( string ) xdoc.Element( "Device" ).Element( "DeviceFeatures" ).Element( "GDIFeatures" ) ) )
              return;

           IEnumerable<XElement> collNameEl = from tp in xdoc.Element( "Device" ).Element( "DeviceFeatures" ).Element( "GDIFeatures" ).Element( "ElementsAction" ).Elements( "Element" )
                                              select tp;

           // ���������� �������� � ������
           foreach ( XElement xecollNameEl in collNameEl )
              slFormElements.Add( xecollNameEl.Attribute( "Name" ).Value, null );

           // ���� �������� ������ �� �����
           ControlCollection cc;
           cc = ( ControlCollection ) this.Controls;
           for ( int i = 0 ; i < cc.Count ; i++ )
              if ( slFormElements.Contains( cc[ i ].Name ) )
                 slFormElements[ cc[ i ].Name ] = cc[ i ];
              else
                 TestCCforElements( cc[ i ] );

           // �������� �������� ���������
           foreach ( XElement xecollNameEl in collNameEl )
           {
              Control cntrl = ( Control ) slFormElements[ xecollNameEl.Attribute( "Name" ).Value ];
              switch ( xecollNameEl.Element( "Property" ).Attribute( "Name" ).Value )
              {
                 case "Enabled":
                    ( ( Control ) slFormElements[ xecollNameEl.Attribute( "Name" ).Value ] ).Enabled = Convert.ToBoolean( xecollNameEl.Element( "Property" ).Value );
                    break;
                 default:
                    break;
              }
           }
        }

        /// <summary>
        /// ����������� ����� ��������� ��� ��������� � ������ �� ��������� �������
        /// </summary>
        /// <param Name="cc"></param>  
        private void TestCCforElements( Control cc )
        {
           for ( int i = 0 ; i < cc.Controls.Count ; i++ )
              if ( slFormElements.Contains( cc.Controls[ i ].Name ) )
                 slFormElements[ cc.Controls[ i ].Name ] = cc.Controls[ i ];
              else
                 TestCCforElements( cc.Controls[ i ] );
        }

        /// <summary>
        /// �������� �����
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void frmBMRZ_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ������������ �� �����
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(taglist);
        }
        #endregion

        /// <summary>
		  /// �������� ������� ArrayList � ��������� ���������� �� ����������� ����� XML
		  /// </summary>
		  /// <param Name="arrVar"> ������  ArrayList
		  ///������</param>
		  /// <param Name="nameFile">��� ����� XML
		  ///������</param>
        private void CreateArrayList( ArrayList arrVar, string name_arrVar )
		  {
			  SortedList sl = new SortedList();
			  ArrayList alVal = new ArrayList();
                XDocument xd;
			try
			{
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

                        TypeOfTag ToT = TypeOfTag.no;
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
                                ToT = TypeOfTag.no;
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
                        for (int i = 0; i < tbkConfig.Controls.Count; i++)
                        {
                            if (tbkConfig.Controls[i] is TabPage && tbkConfig.Controls[i].Name == sbse.ToString())
                            {
                                tbkConfig.Controls[i].Text = xef.Element("value").Attribute("tag").Value;
                                sl_tpnameUst[sbse.ToString()] = tbkConfig.Controls[i];
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

                // ������������� �� ���������� ����� � DataServer
                taglist = new List<ITag>();

                foreach (FormulaEvalNDS fe in arrVar)
                {
                    if (fe.LinkVariableNewDS == null)
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 428, string.Format("() : frmBMRZ.cs : CreateArrayList() : ��� �������� � ���� = {0}", fe.CaptionFE));               
                    else
                        taglist.Add(fe.LinkVariableNewDS);
                }

                if (!slTagListByTabPages.ContainsKey(name_arrVar))
                    slTagListByTabPages.Add(name_arrVar, taglist);

                HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags(taglist);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
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
      /// <summary>
      /// ���� �� Tab � ������� �����������
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      private void tabPage1_Enter( object sender, EventArgs e )
        {
           // ��������/���������� ������ ������
            foreach( Panel p in arDopPanel )
                p.Visible = false;

            //-------------------------------------//
			pnlCurrent.Visible = true;

            pnlCurrent.Left = tabControl1.Left + 5;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlCurrent.Height;
            pnlCurrent.Parent = splitContainer1.Panel2;
            pnlCurrent.Dock = DockStyle.Fill;

            //pnlCurrent.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlCurrent.Width = tabControl1.Width - 10;
            
            Current_Analog_First.Left = 3;
            Current_Analog_First.Width = tabPage1.Width / 3 - 3 ;
            Current_Analog_First.Height = tabPage1.Height - 10;

            Current_DiscretIn.Left = Current_Analog_First.Left + Current_Analog_First.Width + 3 ;
            Current_DiscretIn.Height = tabPage1.Height - 10;
            Current_DiscretIn.Width = tabPage1.Width / 3 - 3;

            Current_DiscretOut.Left = Current_DiscretIn.Left + Current_DiscretIn.Width + 3;
            Current_DiscretOut.Height = tabPage1.Height - 10;
            Current_DiscretOut.Width = tabPage1.Width / 3 - 3;

            //-------------------------------------------------------------------
            //������� ���. ��� ����������� ������� �������� ���������� � ���������� ��������
            // ������� ���������� ������� (���. 0033 ...) � ������ ������������ �������������
            if( arrCurSign.Count != 0 )
                return;
				 CreateArrayList( arrCurSign, "arrCurSign" );

           // ��������� ����������� �� �����
            for( int i = 0; i < arrCurSign.Count; i++ )
            {
                FormulaEvalNDS ev = ( FormulaEvalNDS ) arrCurSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Analog:
							 usTB = new ctlLabelTextbox(ev);
							 usTB.LabelText = "";
							 usTB.Parent = (FlowLayoutPanel) slFLP[ev.ToP];
							 usTB.AutoSize = true;

							 Binding bnd = new Binding("Text",ev.LinkVariableNewDS,"ValueAsString", true);
							 bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
							 usTB.txtLabelText.DataBindings.Add(bnd);
                            try
			                {
                                ev.LinkVariableNewDS.BindindTag = bnd;
                            }
			                catch(Exception ex)
			                {
				                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			                }							 
							 usTB.Caption_Text = ev.CaptionFE;
							 usTB.Dim_Text = ev.Dim;
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar(ev);
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;

							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
							bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.checkBox1.Text = ev.CaptionFE;
							 break;
						 case TypeOfTag.no:
							 break;
						 default:
							 MessageBox.Show("��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
							 break;
					 }
            }

			// ��������� ��� ������ �� pnlCurrent
            // === ��������� ������ ������ ========\\
            ArrayList tabforremove = new ArrayList( );
            for ( int i = 0 ; i < tcCurrentBottomPanel.TabPages.Count ; i++ )
            {
               TabPage tpt = tcCurrentBottomPanel.TabPages[ i ];
               GroupBox gbt;
               FlowLayoutPanel flpt;

               for ( int j = 0 ; j < tpt.Controls.Count ; j++ )
               {
                  if ( tpt.Controls[ j ] is GroupBox )
                  {
                     gbt = (GroupBox)tpt.Controls[j];
                     tpt.Text = gbt.Text;
                     for ( int jj = 0 ; jj < gbt.Controls.Count ; jj++ )
                        if ( gbt.Controls[ jj ] is FlowLayoutPanel )
                           if ( ( ( FlowLayoutPanel ) gbt.Controls[ jj ] ).Controls.Count == 0 )
                              tabforremove.Add( tpt );
                  }
               }
            }
			// ������� ������ TabPage
            for ( int i = 0 ; i < tabforremove.Count ; i++ )
               tcCurrentBottomPanel.TabPages.Remove( (TabPage)tabforremove[i] );



            tabSystem_Enter( sender, e );
       }

      private void tabPage1_Leave(object sender, EventArgs e)
      {
          if (slTagListByTabPages.ContainsKey("arrCurSign"))
            // ������������ �� �����
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(slTagListByTabPages["arrCurSign"]);
      }
        #endregion

        #region Tab � ��������� �����������
        /*=======================================================================*
        *   private void tbpAvar_Enter( object sender, EventArgs e )
        *       ���� �� Tab � ��������� �����������
        *=======================================================================*/
        private void tbpAvar_Enter( object sender, EventArgs e )
        {
           // ������������� ������ ��� ������ ��������� ���������� �� ��������� �����
           dtpEndDateAvar.Value = DateTime.Now;
           dtpEndTimeAvar.Value = DateTime.Now;
           ;
           dtpStartDateAvar.Value = DateTime.Now;
           ;

           TimeSpan ts = new TimeSpan( 1, 0, 0, 0 );
           dtpStartDateAvar.Value = dtpStartDateAvar.Value - ts;
           dtpStartTimeAvar.Value = DateTime.Now;

            //-------------------------------------------------------------------
            // ��������/���������� ������ ������
            foreach( Panel p in arDopPanel )
                p.Visible = false;
            pnlAvar.Visible = true;

            pnlAvar.Left = tabControl1.Left + 5;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlAvar.Height;
            pnlAvar.Parent = splitContainer1.Panel2;
            pnlAvar.Dock = DockStyle.Fill;

            //pnlAvar.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlAvar.Width = tabControl1.Width - 10;

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

            //-------------------------------------------------------------------
            //������� ���. ��� ����������� ���������� � ���������� ��������
            
            if( arrAvarSign.Count != 0 )
                return;

            lstvAvar.Items.Clear();
            
            AvarBD();

            CreateArrayList( arrAvarSign, "arrAvarSign" );
            
            // ��������� ����������� �� �����
            for( int i = 0; i < arrAvarSign.Count; i++ ) 
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrAvarSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Analog:
                             usTB = new ctlLabelTextbox(ev);
							 usTB.LabelText = "";
							 usTB.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 usTB.AutoSize = true;

							 Binding bnd = new Binding("Text",ev.LinkVariableNewDS,"ValueAsString", true);
							 bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
							 usTB.txtLabelText.DataBindings.Add(bnd);
							 ev.LinkVariableNewDS.BindindTag = bnd;
							 usTB.Caption_Text = ev.CaptionFE;
							 usTB.Dim_Text = ev.Dim;
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar(ev);
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;

							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
            				bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.checkBox1.Text = ev.CaptionFE;
							 break;
						 case TypeOfTag.no:
							 break;
						 default:
							 MessageBox.Show( "��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error );
							 break;
					 }
            }            
        }
        /*=======================================================================*
        *   private void lstvAvar_ItemActivate( object sender, EventArgs e )
        *       ����� ���������� ��� ������ ���������� ������
        *=======================================================================*/
        private void lstvAvar_ItemActivate( object sender, EventArgs e )
        {
            if (lstvAvar.SelectedItems.Count == 0)
                return;
            
            #region ������ ���
		    //��������� ������ �� ������

            //byte[] adata = CommonUtils.CommonUtils.GetBlockData(HMI_Settings.cstr, Convert.ToInt32(lstvAvar.SelectedItems[0].Tag));

            // �������� ��������� ������� ������ � ��������� ����������� �� ����
            //ParseBDPacket( adata, 10280, iIDDev ); 
	        #endregion

            int id_block = (int)lstvAvar.SelectedItems[0].Tag;

            // ����� ���������� � ������ ��
            int numdevfc = iFC * 256 + iIDDev;
            ArrayList arparam = new ArrayList();
            // ����� ��� ������ � ��
            arparam.Add(id_block);
            // ������ �����������
            byte[] str_cnt_in_bytes = Encoding.UTF8.GetBytes(HMI_Settings.cstr);
            arparam.Add(str_cnt_in_bytes);

            IRequestData req = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetData(0, (uint)numdevfc, "ArhivAvariBlockData", arparam, numdevfc);

            //byte[] adata = DataBaseReq.GetBlockData(HMI_Settings.cstr, (int)lstvConfig.SelectedItems[0].Tag);

            // �������� ��������� ������� ������ �� ����
            // SetArhivGroupInDev(iIDDev, 14);
            // ParseBDPacket(adata, 62000, iIDDev);
        }

        private void AvarBD( )
        {
            DataBaseReq dbs = new DataBaseReq(HMI_Settings.cstr, "ShowDataLog2");

            // ������� ���������
            // 1. ip FC
            dbs.AddCMDParams(new DataBaseParameter("@IP", ParameterDirection.Input, SqlDbType.BigInt, 0));
            // 2. id ����������
            dbs.AddCMDParams(new DataBaseParameter("@id", ParameterDirection.Input, SqlDbType.Int, iFC * 256 + iIDDev));
            // 3. ��������� �����
            TimeSpan tss = new TimeSpan(0, dtpStartDateAvar.Value.Hour - dtpStartTimeAvar.Value.Hour, dtpStartDateAvar.Value.Minute - dtpStartTimeAvar.Value.Minute, dtpStartDateAvar.Value.Second - dtpStartTimeAvar.Value.Second);
            DateTime tim = dtpStartDateAvar.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_start", ParameterDirection.Input, SqlDbType.DateTime, tim));
            // 2. �������� �����
            tss = new TimeSpan(0, dtpEndDateAvar.Value.Hour - dtpEndTimeAvar.Value.Hour, dtpEndDateAvar.Value.Minute - dtpEndTimeAvar.Value.Minute, dtpEndDateAvar.Value.Second - dtpEndTimeAvar.Value.Second);
            tim = dtpEndDateAvar.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_end", ParameterDirection.Input, SqlDbType.DateTime, tim));
            // 5. ��� ������
            // ���������� �� �������
            int tbd = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Srabat, iFC, iIDDev);
            dbs.AddCMDParams(new DataBaseParameter("@type", ParameterDirection.Input, SqlDbType.Int, tbd));
            // 6. �� ������ �������
            dbs.AddCMDParams(new DataBaseParameter("@id_record", ParameterDirection.Input, SqlDbType.Int, 0));

            dbs.DoStoredProcedure();

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
                //DateTime t = (DateTime)dtA.Rows[curRow]["TimeBlock"];
                //ListViewItem li = new ListViewItem(t.ToShortDateString());
                //li.SubItems.Add(t.ToLongTimeString() + ":" + t.Millisecond);
                //li.Tag = dtA.Rows[curRow]["ID"];
                //lstvAvar.Items.Add(li);

                DateTime t = (DateTime)dtA.Rows[curRow]["TimeBlock"];
                ListViewItem li = new ListViewItem(CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t));
                //li.SubItems.Add(t.ToLongTimeString() + ":" + t.Millisecond);
                li.Tag = dtA.Rows[curRow]["ID"];
                lstvAvar.Items.Add(li);
            }
           }
        #region ��������� ������� ������ � ��������� ����������� �� ����
        private void ParseBDPacket(byte[] pack, ushort adr , int dev)
        {
            //PrintHexDump( "LogHexPacket.dat" , pack);  // ������� � ���� ��� ��������
            //parent.newKB.PacketToQueDev( pack, adr , iFC,dev); // 10280 �����  �� ������  ����������
            //// �������� �������������� ������ ���������� ��������
            //SetArhivGroupInDev(dev, 8);
        }
        #endregion

        private void btnReNew_Click( object sender, EventArgs e )
        {
            IsMesView = true;
            AvarBD();
            tcAvarBottomPanel.SelectTab(0);//.TabPages[ 0 ].Select( );
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
        private void tbpAvar_Leave(object sender, EventArgs e)
        {
            if (slTagListByTabPages.ContainsKey("arrAvarSign"))
                // ������������ �� �����
                HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(slTagListByTabPages["arrAvarSign"]);
        }
        #endregion

        #region ���� �� ������� ������������� ����������
        private void tabStore_Enter( object sender, EventArgs e )
        {
            // �������� �������, ���������� ������������� ������ ������������� ���������� � ���������
            /*byte[] paramss = new byte[2];
			paramss[0] =  paramss[1] = 0;

			parent.newKB.ExecuteCommand(iFC, iIDDev, "SPC", paramss, parent.toolStripProgressBar1 );
            lblIntervalReadStore1.Enabled = false;
            lblIntervalReadStore2.Enabled = false;
            tbIntervalReadStore.Enabled = false;

            parent.newKB.ExecuteCommand( iFC, iIDDev, "SPM", paramss, parent.toolStripProgressBar1 );
            lblIntervalReadMaxM1.Enabled = false;
            lblIntervalReadMaxM2.Enabled = false;
            tbIntervalReadMaxMeter.Enabled = false;*/

            // ��������/���������� ������ ������
            foreach( Panel p in arDopPanel )
                p.Visible = false;
            pnlStore.Visible = true;

            pnlStore.Left = tabControl1.Left + 5;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlStore.Height;
            pnlStore.Parent = splitContainer1.Panel2;
            pnlStore.Dock = DockStyle.Fill;

            //pnlStore.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlStore.Width = tabControl1.Width;

            if( arrStoreSign.Count != 0 )
                return;

            CreateArrayList(arrStoreSign, "arrStoreSign");
           //--------------------
            
            // ��������� ����������� �� �����
            for( int i = 0; i < arrStoreSign.Count; i++ )
            {
                FormulaEvalNDS ev = ( FormulaEvalNDS ) arrStoreSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
					 CheckBoxVar chBV;
                ctlLabelTextbox usTB;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Analog:
                             usTB = new ctlLabelTextbox(ev);
							 usTB.lblCaption.Text = "";
							 usTB.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 usTB.AutoSize = true;

							 Binding bnd = new Binding("Text",ev.LinkVariableNewDS,"ValueAsString", true);
							 bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
							 usTB.txtLabelText.DataBindings.Add(bnd);
							 ev.LinkVariableNewDS.BindindTag = bnd;
							 usTB.Caption_Text = ev.CaptionFE;
							 usTB.Dim_Text = ev.Dim;
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar(ev);
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;
							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
							bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.checkBox1.Text = ev.CaptionFE;
							 break;
						 case TypeOfTag.no:
							 break;
						 default:
							 MessageBox.Show( "��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error );
							 break;
					 }					 
            }
        }
        private void tabStore_Leave(object sender, EventArgs e)
        {
            if (slTagListByTabPages.ContainsKey("arrStoreSign"))
                // ������������ �� �����
                HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(slTagListByTabPages["arrStoreSign"]);
        }
        #endregion        

        #region ������� "������������ � �������"
        /// <summary>
        /// private void tbcConfig_Enter( object sender, EventArgs e )
        ///  ���� �� ������� "������������ � �������"
        /// </summary>
        private void tbcConfig_Enter( object sender, EventArgs e )
        {
           // ������������� ������ ��� ��������� ������� �� ��������� �����
           dtpEndDateConfig.Value = DateTime.Now;
           dtpEndTimeConfig.Value = DateTime.Now;
           dtpStartDateConfig.Value = DateTime.Now;
           
           TimeSpan ts = new TimeSpan( 1, 0, 0, 0 );
           dtpStartDateConfig.Value = dtpStartDateConfig.Value - ts;
           dtpStartTimeConfig.Value = DateTime.Now;

            // ��������/���������� ������ ������
            foreach( Panel p in arDopPanel )
                p.Visible = false;
            pnlConfig.Visible = true;

            pnlConfig.Left = tabControl1.Left + 5;

            pnlConfig.Left = tabControl1.Left + 5;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlConfig.Height;
            pnlConfig.Parent = splitContainer1.Panel2;
            pnlConfig.Dock = DockStyle.Fill;

            //pnlConfig.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlConfig.Width = tabControl1.Width;

            //-------------------------------------------------------------------
            //������� ���. ��� ����������� ���������� � ���������� ��������
            if( arrConfigSign.Count != 0 )
                return;

           lstvConfig.Items.Clear();
				
            UstavBD();

				 btnWriteUst.Enabled = false;
            CreateArrayList(arrConfigSign, "arrConfigSign");

            // ��� ������ �������� ��� tabpage
            for( int i = 0; i < tbkConfig.Controls.Count; i++ )
            {
                if( tbkConfig.Controls[i] is TabPage )
                {
                    tbkConfig.Controls.RemoveAt( i );
                    i--;
                }
            }

            // ������������ �������� �������
            for( int i = 0; i < sl_tpnameUst.Count; i++ )
                tbkConfig.Controls.Add((Control) sl_tpnameUst.GetByIndex( i ) );

            // ��������� ����������� �� �����
            for( int i = 0; i < arrConfigSign.Count; i++ )
            {
                //FormulaEval ev = ( FormulaEval ) arrConfigSign[i];
                FormulaEvalNDS ev = (FormulaEvalNDS)arrConfigSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                ComboBoxVar cBV;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Combo:
                             //       cBV = new ComboBoxVar((string[])( ( TagEval ) ( ( TagVal ) ev.arrTagVal[0] ).linkTagEval ).arrStrCB.ToArray(typeof(string)), 0);
                             //       cBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
                             //cBV.AutoSize = true;
                             //cBV.addrLinkVar = ev.addrVar;
                             //cBV.typetag = ev.tRezFormulaEval.TypeTag;
                             //ev.OnChangeValForm += cBV.LinkSetText;
                             //ev.FirstValue();

                             //cBV = new ComboBoxVar((string[])( ( TagEval ) ( ( TagVal ) ev.arrTagVal[0] ).linkTagEval ).arrStrCB.ToArray(typeof(string)), 0);
                             cBV = new ComboBoxVar(ev.LinkVariableNewDS.SlEnumsParty , 0);
                             cBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
                             cBV.AutoSize = true;
                             cBV.TypeView = TypeViewValue.Textbox;//.Combobox;

                             //Binding bndcb = new Binding("SelectedText", ev.LinkVariableNewDS, "ValueAsString", true);
                             ////Binding bndcb = new Binding("SelectedIndex", ev.LinkVariableNewDS., "ValueAsMemX", true);
                             //bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                             //cBV.cbVar.DataBindings.Add(bndcb);
                             //ev.LinkVariableNewDS.BindindTag = bndcb;
                             //cBV.lblCaption.Text = ev.CaptionFE;

                             Binding bndcb = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                             bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                             cBV.tbText.DataBindings.Add(bndcb);
                             ev.LinkVariableNewDS.BindindTag = bndcb;
                             cBV.lblCaption.Text = ev.CaptionFE;
                             break;

						 case TypeOfTag.Analog:
							 usTB = new ctlLabelTextbox(ev);
							 usTB.LabelText = "";
							 usTB.Parent = (FlowLayoutPanel) slFLP[ev.ToP];
							 usTB.AutoSize = true;

							 Binding bnd = new Binding("Text",ev.LinkVariableNewDS,"ValueAsString", true);
							 bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
							 usTB.txtLabelText.DataBindings.Add(bnd);
							 ev.LinkVariableNewDS.BindindTag = bnd;
							 usTB.Caption_Text = ev.CaptionFE;
							 usTB.Dim_Text = ev.Dim;
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar(ev);
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;

							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
							bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.checkBox1.Text = ev.CaptionFE;
							 break;
						 default:
							 MessageBox.Show( "��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error );
							 break;
					 }
            }
        }
        private void UstavBD( )
        {
            DataBaseReq dbs = new DataBaseReq(HMI_Settings.cstr, "ShowDataLog2");

            // ������� ���������
            // 1. ip FC
            dbs.AddCMDParams(new DataBaseParameter("@IP", ParameterDirection.Input, SqlDbType.BigInt,0));
            // 2. id ����������
            dbs.AddCMDParams(new DataBaseParameter("@id",ParameterDirection.Input,SqlDbType.Int,iFC * 256 + iIDDev));
            // 3. ��������� �����
            TimeSpan tss = new TimeSpan(0, dtpStartDateConfig.Value.Hour - dtpStartTimeConfig.Value.Hour, dtpStartDateConfig.Value.Minute - dtpStartTimeConfig.Value.Minute, dtpStartDateConfig.Value.Second - dtpStartTimeConfig.Value.Second);
            DateTime tim = dtpStartDateConfig.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_start",ParameterDirection.Input,SqlDbType.DateTime,tim));
            // 2. �������� �����
            tss = new TimeSpan(0, dtpEndDateConfig.Value.Hour - dtpEndTimeConfig.Value.Hour, dtpEndDateConfig.Value.Minute - dtpEndTimeConfig.Value.Minute, dtpEndDateConfig.Value.Second - dtpEndTimeConfig.Value.Second);
            tim = dtpEndDateConfig.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_end", ParameterDirection.Input, SqlDbType.DateTime, tim));
            // 5. ��� ������
            // ���������� �� ��������
            int tbd = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Ustavki, iFC, iIDDev);
            dbs.AddCMDParams(new DataBaseParameter("@type", ParameterDirection.Input, SqlDbType.Int, tbd));
            // 6. �� ������ �������
            dbs.AddCMDParams(new DataBaseParameter("@id_record", ParameterDirection.Input, SqlDbType.Int, 0));

            dbs.DoStoredProcedure();

            // ��������� ������ �� ��������
            dtU = dbs.GetTableAsResultCMD();

            if (dtU.Rows.Count == 0 && IsMesView)
            {
                MessageBox.Show("�������� ������ �� �������� ��� ����� ���������� ���.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsMesView = false;
            }

            // ��������� ListView
            lstvConfig.Items.Clear();
            for (int curRow = 0; curRow < dtU.Rows.Count; curRow++)
            {
                //DateTime t = (DateTime)dtU.Rows[curRow]["TimeBlock"];
                //ListViewItem li = new ListViewItem(t.ToShortDateString());
                //li.SubItems.Add(t.ToLongTimeString() + ":" + t.Millisecond);
                //li.Tag = dtU.Rows[curRow]["ID"];
                //lstvConfig.Items.Add(li);

                DateTime t = (DateTime)dtU.Rows[curRow]["TimeBlock"];
                ListViewItem li = new ListViewItem(CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t));
                //li.SubItems.Add(t.ToLongTimeString() + ":" + t.Millisecond);
                li.Tag = dtU.Rows[curRow]["ID"];
                lstvConfig.Items.Add(li);
            }
        }

        private void btnReadUstFC_Click(object sender, EventArgs e)
        {
            btnWriteUst.Enabled = true;

            // ���������� ������ � ������ �������� ������������
            // ����� ���������� � ������ ��
            int numdevfc = iFC * 256 + iIDDev;
            CommonUtils.CommonUtils.WriteEventToLog(7, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "IMP", new byte[] { });

            //if (parent.newKB.ExecuteCommand(iFC, iIDDev, "IMP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
            //    parent.WriteEventToLog(35, "������� \"IMP\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true);//, true, false);

            tcUstConfigBottomPanel.SelectTab(0);
        }

        #region ����� ���������� ��� ������ ���������� ������ �� ��������
        private void lstvConfig_ItemActivate(object sender, EventArgs e)
        {
            if (lstvConfig.SelectedItems.Count == 0)
                return;

            int id_block = (int)lstvConfig.SelectedItems[0].Tag;

            // ����� ���������� � ������ ��
            int numdevfc = iFC * 256 + iIDDev;
            ArrayList arparam = new ArrayList();
            // ����� ��� ������ � ��
            arparam.Add(id_block);
            // ������ �����������
            byte[] str_cnt_in_bytes = Encoding.UTF8.GetBytes(HMI_Settings.cstr);
            arparam.Add(str_cnt_in_bytes);

            IRequestData req = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetData(0, (uint)numdevfc, "ArhivUstavkiBlockData", arparam, numdevfc);

            //byte[] adata = DataBaseReq.GetBlockData(HMI_Settings.cstr, (int)lstvConfig.SelectedItems[0].Tag);

           // �������� ��������� ������� ������ �� ����
           // SetArhivGroupInDev(iIDDev, 14);
           // ParseBDPacket(adata, 62000, iIDDev);

           btnWriteUst.Enabled = true;
        }
        #endregion

        private void btnReNewUstBD_Click( object sender, EventArgs e )
      {
        IsMesView = true;
         UstavBD( );
         tcUstConfigBottomPanel.SelectTab( 0 );
      }
      /// <summary>
		/// private void btnWriteUst_Click( object sender, EventArgs e )
		/// ������ �������
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>  
		private void btnWriteUst_Click( object sender, EventArgs e )
        {
            MessageBox.Show("���������� �������� ���������");

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
            //foreach (DataSource aFC in parent.KB)
            ////foreach( FC aFC in parent.KB )
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
            //    //System.Buffer.BlockCopy( ( byte[] ) slLocal[62000] , 6, memX, 0, (( byte[] ) slLocal[62000] ).Length - 6 );
            //    System.Buffer.BlockCopy( ( byte[] ) slLocal[62000], 6, memX, 0, lenpack - 6 );

            ////memDevBlock.Read( memX, 0, lenpack - 6 );

            //for( int i = 0; i < tbkConfig.Controls.Count; i++ )
            //{
            //    if( tbkConfig.Controls[i] is TabPage )
            //    {
            //        tp = ( TabPage ) tbkConfig.Controls[i];
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
            //                           if ( !CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 ) )
            //                              return;
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
            //            if ( !CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 ) )
            //               return;
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

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "WCP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //   parent.WriteEventToLog(35, "������� \"WCP\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true);//, true, false );
            //    // ���������������� �������� ������������
            //parent.WriteEventToLog(6, iIDDev.ToString(), this.Name, true);//, true, false );			//"������ ������� WCP - ������ �������."
            //isUstChange = false;
        }		

		private void btnResetValues_Click( object sender, EventArgs e )
		{
            //MessageBox.Show("���������� �������� ���������");
            btnWriteUst.Enabled = false;
            IDevice thisDev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device(0, (uint)(iFC * 256 + iIDDev));
            foreach ( ITag tag in thisDev.GetRtuTags() )
                tag.SetDefaultValue();
            //parent.newKB.ResetGroup(iFC, iIDDev, 14);
		}
        private void tbpConfUst_Leave(object sender, EventArgs e)
        {
            if (slTagListByTabPages.ContainsKey("arrConfigSign"))
                // ������������ �� �����
                HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(slTagListByTabPages["arrConfigSign"]);
        }

        #endregion

        #region ������� ������������� � ���������
        private void tabPage5_Enter( object sender, EventArgs e )
        {
           if (dgvOscill.RowCount != 0 || dgvDiag.RowCount != 0)
              return;

           // ������������� ������ ��� ��������� ������ ������������ � �������� �� ��������� �����
           dtpEndData.Value = DateTime.Now;
           dtpEndTime.Value = DateTime.Now;
           dtpStartData.Value = DateTime.Now;

           TimeSpan ts = new TimeSpan( 1, 0, 0, 0 );
           dtpStartData.Value = dtpStartData.Value - ts;
           dtpStartTime.Value = DateTime.Now;

            // ��������/���������� ������ ������
            foreach( Panel p in arDopPanel )
                p.Visible = false;
            pnlOscDiag.Visible = true;

            pnlOscDiag.Left = tabControl1.Left + 5;

            pnlOscDiag.Left = tabControl1.Left + 5;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlOscDiag.Height;
            pnlOscDiag.Parent = splitContainer1.Panel2;
            pnlOscDiag.Dock = DockStyle.Fill;

            //pnlOscDiag.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlOscDiag.Width = tabControl1.Width - 10;
				
			  //��������� �������
            tabPage5.Height = tabControl1.Height;
            splitContainer_OscDiag.Height = tabPage5.Height;//tabControl1            
            dgvOscill.Height = splitContainer_OscDiag.Panel1.Height - 2 * btnUnionOsc.Height;
            dgvDiag.Height = splitContainer_OscDiag.Panel1.Height - 2 * btnUnionOsc.Height;
        }

        private void OscBD( )
        {
            if (oscdg == null)
                oscdg = new OscDiagViewer();

            oscdg.IdFC = this.iFC;
            oscdg.IdDev = this.iIDDev;
            oscdg.DTStartData = dtpStartData.Value;
            oscdg.DTStartTime = dtpStartTime.Value;
            oscdg.DTEndData = dtpEndData.Value;
            oscdg.DTEndTime = dtpEndTime.Value;
            oscdg.TypeRec = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Osc, iFC, iIDDev);            

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

        // ��������� �������� �� ����
        private void DiagBD( )
        {
            if (oscdg == null)
                oscdg = new OscDiagViewer();

            oscdg.IdFC = this.iFC;
            oscdg.IdDev = this.iIDDev;
            oscdg.DTStartData = dtpStartData.Value;
            oscdg.DTStartTime = dtpStartTime.Value;
            oscdg.DTEndData = dtpEndData.Value;
            oscdg.DTEndTime = dtpEndTime.Value;
            oscdg.TypeRec = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Diagramm, iFC, iIDDev);

            // ��������� ������ �� ��������������
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
            }
        }

        void dgvDiag_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        // ��������� ������� - � ��������� � ������� ��������
        private void dtpStartData_ValueChanged(object sender, EventArgs e)
        {
            // ������� ���������� �������
            dgvOscill.Rows.Clear();
            //OscBD();
            //DiagBD();
        }

        ArrayList asb = new ArrayList();    // ��� �������� ���� ������ � ������ ��� ����������� ������������

        // ������ ������ ����� ������������� 
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

            /*
             * ������ �������� ����� DS,
             * ������� ��� ��������� ��������� ������ ��������� (0)
             * � ���������� ����� ��������� �������� ����� �� ������ �����
             * ����� �������� �������� ����� DS
             */
            oscdg.ShowOSCDg( 0 ,dtO, ide);
        }

        // ���������� �������������
        private void button4_Click(object sender, EventArgs e)
        {
        //    if (oscdg == null)
        //       oscdg = new OscDiagViewer(parent);

        //    oscdg.ClearArrayIDE();

        //    // ����������� ������ � ������� dbO, ������� ����������
        //    for (int curRow = 0; curRow < dtO.Rows.Count; curRow++)
        //       if ((bool)dgvOscill[0, curRow].Value == true)
        //          oscdg.AddIde2ArrayIde(((int)dtO.Rows[curRow]["ID"]));
            
        //   oscdg.ShowUnionOSCDg(dtO);
        //}

        //// ������ ������ ����� ���������
        //private void dgvDiag_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridViewCell de;
        //    char[] sep = { ' ' };
        //    string[] sp;
        //    StringBuilder sb;
        //    if (e.ColumnIndex == 0)
        //    {
        //        dgvDiag[e.ColumnIndex, e.RowIndex].Value = (bool)dgvDiag[e.ColumnIndex, e.RowIndex].Value ? false : true;
        //        btnUnionDiag.Enabled = true;
        //        return;
        //    }
        //    else if (e.ColumnIndex != 5)
        //        return;

        //    btnUnionDiag.Enabled = false;
        //    // ���������� ��� ������
        //    for (int i = 0; i < dtG.Rows.Count; i++)
        //        dgvDiag[0, i].Value = false;
        //    try
        //    {
        //        de = dgvDiag["clmIDDiag", e.RowIndex];
        //    }
        //    catch
        //    {
        //        MessageBox.Show("dgvDiag_CellContentClick - ����������");
        //        return;
        //    }
        //    int ide = (int)de.Value;

        //    oscdg.ShowOSCDg(dtG, ide);
        }

        // ���������� ���������
        private void btnUnionDiag_Click(object sender, EventArgs e)
        {
            //if (oscdg == null)
            //   oscdg = new OscDiagViewer(parent);

            //oscdg.ClearArrayIDE();

            //// ����������� ������ � ������� dbO, ������� ����������
            //for (int curRow = 0; curRow < dtG.Rows.Count; curRow++)
            //   if ((bool)dgvDiag[0, curRow].Value == true)
            //      oscdg.AddIde2ArrayIde(((int)dtG.Rows[curRow]["ID"]));

            //oscdg.ShowUnionOSCDg(dtG);
        }

        // ������ ��������
        private void btnReNewOD_Click( object sender, EventArgs e )
        {
            DiagBD();
            OscBD();                                  
        }
        #endregion

        #region ���� �� ������� � ��������� �����������
        private void tabSystem_Enter( object sender, EventArgs e )
        {
            // ��������/���������� ������ ������
            foreach( Panel p in arDopPanel )
                p.Visible = false;
            pnlSystem.Visible = true;

            pnlSystem.Left = tabControl1.Left + 5;

            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlSystem.Height;
            pnlSystem.Parent = splitContainer1.Panel2;
            pnlSystem.Dock = DockStyle.Fill;

            //pnlSystem.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlSystem.Width = tabControl1.Width;

            gbVizov.Width = tabSystem.Width / 2 - 8;
            gbTest.Width = tabSystem.Width / 2 - 8;

            if( arrSystemSign.Count != 0 )
                return;
            //-------------------------------------------------------------------
             CreateArrayList(arrSystemSign, "arrSystemSign");

            // ��������� ����������� �� �����
            for( int i = 0; i < arrSystemSign.Count; i++ )
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrSystemSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Analog:
                             usTB = new ctlLabelTextbox(ev);
							 usTB.LabelText = "";
							 usTB.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 usTB.AutoSize = true;
							 Binding bnd = new Binding("Text",ev.LinkVariableNewDS,"ValueAsString", true);
							 bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
							 usTB.txtLabelText.DataBindings.Add(bnd);
							 ev.LinkVariableNewDS.BindindTag = bnd;
							 usTB.Caption_Text = ev.CaptionFE;
							 usTB.Dim_Text = ev.Dim;
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar(ev);
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;
							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
							bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.checkBox1.Text = ev.CaptionFE;
							 break;
						 case TypeOfTag.no:
							 break;
						 default:
							 MessageBox.Show( "��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error );
							 break;
					 }					 
            }
        }
        private void tabSystem_Leave(object sender, EventArgs e)
        {
            if (slTagListByTabPages.ContainsKey("arrSystemSign"))
                // ������������ �� �����
                HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(slTagListByTabPages["arrSystemSign"]);
        }
        #endregion

        //��� ����������������� ������ ���������
        delegate void SetLVCallback( ListViewItem li, bool actDellstV );

        // actDellstV - �������� � ListView : false - �� �������; true - ��������;
        public void LinkSetLV( object Value, bool actDellstV )
        {
        }

        //��� ����������������� ������ ���������
        private void SetLV( ListViewItem li, bool actDellstV )
        {
        }

        private void button1_Click( object sender, EventArgs e )
        {
            ReNew(); 
        }

        private void ReNew( ) 
        {
        }

        private void btnPrint_Click( object sender, EventArgs e )
        {
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
            //parent.mnuPrint_Click( sender, e );
        }

        /// <summary>
        /// PrintArr()
        ///     ������ ������� ����������
        /// </summary>
        private void PrintArr()
        {
            //   StringBuilder sb = new StringBuilder();
            //   float f_val;
            //   int i_val;
            //   string t_val = "";
            //   ArrayList arCurPrt = new ArrayList();

            //   object val;

            //   // ���������� �������� �������
            //   TabPage tp_sel = tabControl1.SelectedTab;

            //   sb.Length = 0;

            //   switch (tp_sel.Text)
            //   {
            //      case "������� ����������":
            //         // ��������� ��������� ��������
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (������� ����������)");
            //         sb.Append("\n========================================================================\n");
            //         sb.Append(" \n \n ");
            //         arCurPrt = arrCurSign;
            //         break;
            //      case "���������� �� �������":
            //         // ��������� ��������� ��������
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (���������� �� �������)");
            //         sb.Append("\n========================================================================\n");

            //         sb.Append(" \n \n ");
            //         arCurPrt = arrAvarSign;
            //         break;
            //      case "������������� ����������":
            //         // ��������� ��������� ��������
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (������������� ����������)");
            //         sb.Append("\n========================================================================\n");

            //         sb.Append(" \n \n ");
            //         arCurPrt = arrStoreSign;
            //         break;
            //      case "������������ � �������":
            //         // ��������� ��������� ��������
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (������������ � �������)");
            //         sb.Append("\n========================================================================\n");

            //         sb.Append(" \n \n ");
            //         arCurPrt = arrConfigSign;
            //         break;
            //      case "�������":
            //         // ��������� ��������� ��������
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (�������)");
            //         sb.Append("\n========================================================================\n");

            //         sb.Append(" \n \n ");
            //         arCurPrt = arrSystemSign;
            //         break;
            //      case "��������� ���������� � ������":
            //         // ��������� ��������� ��������
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (��������� ���������� � ������)");
            //         sb.Append("\n========================================================================\n");
            //         sb.Append(" \n \n ");
            //         arCurPrt = arrStatusDevCommand;
            //         break;
            //      default:
            //         break;
            //   }

            //   //MessageBox.Show(arCurPrt.Count.ToString());

            //   string strm = String.Empty;

            //   for (int i = 0; i < arCurPrt.Count; i++)
            //      try
            //      {

            //          FormulaEvalNDS ev = (FormulaEvalNDS)arCurPrt[i];

            //         switch (ev.ToT)
            //         {
            //            case TypeOfTag.Analog:
            //               val = ev.tRezFormulaEval.Value;

            //               if (val is float)
            //               {
            //                  f_val = (float)ev.tRezFormulaEval.Value;
            //                  t_val = f_val.ToString("F2"); // ��� ����� ����� �������
            //               }
            //               else if (val is short)
            //               {
            //                  i_val = (Int16)ev.tRezFormulaEval.Value;
            //                  t_val = i_val.ToString();
            //               }
            //               else if (val is int)
            //               {
            //                  i_val = (Int32)ev.tRezFormulaEval.Value;
            //                  t_val = i_val.ToString();
            //               }
            //               else if (val is string)
            //               {
            //                  t_val = (string)ev.tRezFormulaEval.Value;
            //               }

            //               //sb.Append(ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE);
            //               strm = ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE;
            //               break;
            //            case TypeOfTag.Discret:
            //               //sb.Append(ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE);
            //               strm = ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE;
            //               break;
            //            case TypeOfTag.Combo:
            //               //sb.Append(ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE);
            //               strm = ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE;
            //               break;
            //            default:
            //               continue;
            //         }

            //         string news = String.Empty;
            //         for (int ii = 0; ii < strm.Length; ii++)
            //         {
            //            if (strm[ii] != '\0')
            //               news += strm[ii];
            //         }
            //         sb.Append(news);
            //         sb.Append(" \n ");
            //      }
            //      catch (Exception ex)
            //      {
            //         MessageBox.Show(ex.Message);
            //      }
            //   parent.prt.rtbText.AppendText(sb.ToString());
            //}

            //#region ������������ ������ ��� ������ ��� ��������� ���������� - ������ ��� - ����������������
            ///// <summary>
            ///// sbForSimpleVar(StringBuilder sb)
            /////     ������������ ������ ��� ������ ��� ��������� ����������
            ///// </summary>
            ////private void sbForSimpleVar(StringBuilder sb, FormulaEval b_xxx)
            ////{
            ////    float f_val;
            ////    int i_val;
            ////    string t_val = "";
            ////    FormulaEval ev = ( FormulaEval ) b_xxx;
            ////    object val;

            ////    if( b_xxx == null )
            ////        return;

            ////    switch( ev.ToT )
            ////    {
            ////        case TypeOfTag.no:
            ////            val = ev.tRezFormulaEval.Value;

            ////            if( val is float )
            ////            {
            ////                f_val = ( float ) ev.tRezFormulaEval.Value;
            ////                t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
            ////            }
            ////            else if( val is short )
            ////            {
            ////                i_val = ( Int16 ) ev.tRezFormulaEval.Value;
            ////                t_val = i_val.ToString();
            ////            }
            ////            else if( val is ushort )
            ////            {
            ////                i_val = ( UInt16 ) ev.tRezFormulaEval.Value;
            ////                t_val = i_val.ToString();
            ////            }
            ////            else if( val is int )
            ////            {
            ////                i_val = ( Int32 ) ev.tRezFormulaEval.Value;
            ////                t_val = i_val.ToString();
            ////            }
            ////            else if( val is string )
            ////            {
            ////                t_val = ( string ) ev.tRezFormulaEval.Value;
            ////            }

            ////            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
            ////            break;
            ////        case TypeOfTag.Analog:
            ////            val = ev.tRezFormulaEval.Value;

            ////            if( val is float )
            ////            {
            ////                f_val = ( float ) ev.tRezFormulaEval.Value;
            ////                t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
            ////            }
            ////            else if( val is short )
            ////            {
            ////                i_val = ( Int16 ) ev.tRezFormulaEval.Value;
            ////                t_val = i_val.ToString();
            ////            }
            ////            else if( val is int )
            ////            {
            ////                i_val = ( Int32 ) ev.tRezFormulaEval.Value;
            ////                t_val = i_val.ToString();
            ////            }
            ////            else if( val is string )
            ////            {
            ////                t_val = ( string ) ev.tRezFormulaEval.Value;
            ////            }

            ////            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
            ////            break;
            ////        case TypeOfTag.Discret:
            ////            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
            ////            break;
            ////        case TypeOfTag.Combo:
            ////            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
            ////            break;
            ////        default:
            ////            break;
            ////    }
            ////    sb.Append( " \n " );
            ////}		 
            //#endregion
        }
    }
}
        