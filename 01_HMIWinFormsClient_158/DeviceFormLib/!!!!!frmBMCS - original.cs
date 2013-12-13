/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	��������: ����� ��� ������ � ������ ����                                                        
 *                                                                             
 *	����                     : frmBMCS.cs                                         
 *	��� ��������� �����      : 
 *	������ �� ��� ���������� : �#, Framework 3.5                                
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
using System.Data;
using System.Text;
using System.Windows.Forms;
using Calculator;
using System.Collections;
//using NetCrzaDevices;
using System.IO;
//using NSNetNetManager;
using LabelTextbox;
using HMI_MT_Settings;
using System.Linq;
using System.Xml.Linq;
using InterfaceLibrary;
using DataBaseLib;

namespace DeviceFormLib
{
    public partial class frmBMCS : Form, IDeviceForm
	{
        /// <summary>
        /// ������� TabPage ��� ��������/������� �����
        /// </summary>
        TabPage tpCurrent;

        XDocument xdoc;
        SortedList slFormElements; // ������ ���������, �������� ������� ����� ����� �������� (��. Device.cfg)
        int iFC;            // ����� �� �������������
        string strFC;       // ����� �� ������
        int iIDDev;         // ����� ���������� �������������
        string strIDDev;    // ����� ���������� ������
        int inumLoc;         // ����� ������ �������������
        string strnumLoc;    // ����� ������ ������
        string nfXMLConfig; // ��� ����� � ��������� 
		string nfXMLConfigFC; // ��� ����� � ��������� ����
        /// <summary>
        /// ��� ����� � �������� �����
        /// </summary>
        string fileFrmTagsDescript;

		  //// ������ �������������� �������
		  //ArrayList arDopPanel;

        ArrayList arrAvarSign = new ArrayList();
        ArrayList arrCurSign = new ArrayList();
        ArrayList arrSystemSign = new ArrayList();
        ArrayList arrStoreSign = new ArrayList();
        ArrayList arrConfigSign = new ArrayList();
        ArrayList arrStatusDevCommand = new ArrayList();
		ArrayList arrStatusFCCommand = new ArrayList();

        ushort iclm = 16;  // ����� ������� � �����
        SortedList slLocal;
        EncodingInfo eii;
        SortedList slEncoding;
        SortedList se = new SortedList();
        SortedList sl_tpnameUst = new SortedList();
        StringBuilder sbse = new StringBuilder();

		  //DataTable dtO;  // ������� � ���������������
		  //DataTable dtG;  // ������� � �����������
		  //DataTable dtA;  // ������� � ��������
		  DataTable dtU;  // ������� � ���������

			SortedList slFLP = new SortedList();	// ��� �������� ��� � FlowLayoutPanel
			SortedList slFLPUst = new SortedList();	// �������� ��� �������
			
        ErrorProvider erp = new ErrorProvider( );
			// ������ �����������
		ArrayList arrIndicators = new ArrayList();
		bool firstShow = false;	// ��� ������� ������ �����, ��������� ������� �������

        /// <summary>
        /// ������ ����� ��� ��������/�������
        /// </summary>
	    //List<ITag> taglist;
        /// <summary>
        /// ������ ����� ��� ��������/�������
        /// �� ����������
        /// </summary>
        //List<ITag> taglistIndicators = new List<ITag>();
        /// <summary>
        /// ���� - ��������� �� ��������� �� ��������� �������� ������� 
        /// ��� ���
        /// </summary>
        bool IsMesView = false;

        #region �����������
		public frmBMCS( )
		{
			InitializeComponent();
		}
        public frmBMCS(/*Form MainForm linkMainForm,*/ int iFC, int iIDDev, /*int inumLoc,*/ string fXML, string ftagsxmldescript)
		{
			InitializeComponent();

            this.iFC = iFC;                 // ����� �� �������������
            strFC = iFC.ToString();         // ����� �� ������
            this.iIDDev = iIDDev;           // ����� ���������� �������������
            strIDDev = iIDDev.ToString();   // ����� ���������� ������
            //this.inumLoc = inumLoc;         // ����� ������ �������������
            strnumLoc = inumLoc.ToString();    // ����� ������ ������
            
            nfXMLConfig = fXML;
            fileFrmTagsDescript = ftagsxmldescript;

		    this.DoubleBuffered = true;
            slFormElements = new SortedList( );

            InitInterfaceElementsClick();

            /*
             * �������� �������� ����� � ��������� ����������
             * ��� ����� ���������� ���� PrgDevCFG.cdp ���������
             */

            string TypeName = String.Empty;
            string nameR = String.Empty;
            string nameELowLevel = String.Empty;
            string nameEHighLevel = String.Empty;

            XElement xedev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription(0, "MOA_ECU", string.Format("{0}.{1}", strFC, strIDDev));
            xedev = xedev.Element("DescDev");   // ����������

            TypeName = xedev.Element("TypeName").Value;
            nameR = xedev.Element("nameR").Value;
            nameELowLevel = xedev.Element("nameELowLevel").Value;
            nameEHighLevel = xedev.Element("nameEHighLevel").Value;
            this.Text = nameR + " ( ��.� " + iIDDev.ToString() + " )" + xedev.Element("DescDev").Value;

            TestCCforFLP( this, slFLP );
		}
        /// <summary>
        /// �������� �����
        /// </summary>
        private void frmBMCS_Load( object sender, EventArgs e )
        {
			    try
			    {
                    //ControlCollection cc = (ControlCollection)this.Controls;
                    //for (int i = 0; i < cc.Count; i++)
                    //{
                    //    if (cc[i] is FlowLayoutPanel)
                    //    {
                    //        FlowLayoutPanel flp = (FlowLayoutPanel)cc[i];
                    //        slFLP[flp.Name] = flp;
                    //    }
                    //    else
                    //    {
                    //        TestCCforFLP(cc[i], slFLP);
                    //    }
                    //}
			        //TestCCforFLP(this, slFLP);

                    TimeSpan ts = new TimeSpan(1, 0, 0, 0);

                    // ������������� ������ ��� ��������� ������� �� ��������� �����
                    dtpEndDateConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                    dtpEndTimeConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                    dtpStartDateConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

                    ts = new TimeSpan(1, 0, 0, 0);
                    dtpStartDateConfig.Value = dtpStartDateConfig.Value - ts;
                    dtpStartTimeConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

                    // ��������� ����������� ������ ��������� ����� �� ��������� Device.cfg (��. ��� � - 00)
                    SetElementsFormFeatures( /*Path.GetDirectoryName()*/ fileFrmTagsDescript);

                    tabControl1.Controls.Add( new DataBaseFilesLibrary.TabPageDBFile( (uint)iIDDev ) );
                }
                catch (Exception ex)
                {
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                }
        }

        private void frmBMCS_Shown(object sender, EventArgs e)
        {
            timerInd.Start();
            firstShow = true;
        }

        /// <summary>
        /// �������� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBMCS_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ������������ �� �����
            HMI_MT_Settings.HMI_Settings.HMIControlsTagReNew(tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe);
        }

        /// <summary>
        /// � ���� ������� ����������� ����� ��
        /// ��������� ���������� � ����� �� ���������
        /// </summary>
        public void InitInterfaceElementsClick()
        {
            tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);

            tbBMCS_Indication.Enter +=new EventHandler(tbBMCS_Indication_Enter);
            tbBMCS_Indication.Leave +=new EventHandler(tbBMCS_Indication_Leave);

            tbBMCS_Store.Enter += new EventHandler(tbBMCS_Store_Enter);
            tbBMCS_Store.Leave += new EventHandler(tbBMCS_Store_Leave);

            tbBMCS_Config.Enter += new EventHandler(tbBMCS_Config_Enter);
            tbBMCS_Config.Leave += new EventHandler(tbBMCS_Config_Leave);

            this.FormClosing +=new FormClosingEventHandler(frmBMCS_FormClosing);

            //splitContainer21.Panel2Collapsed = true;

            //this.FormClosing += new FormClosingEventHandler();  //+=new FormClosingEventHandler(frmBMRZ_FormClosing);
            //tabPage1.Enter +=new EventHandler(tabPage1_Enter);
            //tbpAvar.Enter += new EventHandler(tbpAvar_Enter);
            //lstvAvar.ItemActivate +=new EventHandler(lstvAvar_ItemActivate);
            //btnReNew.Click += new EventHandler(btnReNew_Click);
            //tabStore.Enter += new EventHandler(tabStore_Enter);
            //tbpConfUst.Enter += new EventHandler(tbcConfig_Enter);
            //btnReadUstFC.Click += new EventHandler(btnReadUstFC_Click);
            //lstvConfig.ItemActivate += new EventHandler(lstvConfig_ItemActivate);
            //btnReNewUstBD.Click += new EventHandler(btnReNewUstBD_Click);
            //btnWriteUst.Click += new EventHandler(btnWriteUst_Click);
            //btnResetValues.Click += new EventHandler(btnResetValues_Click);
            //tabPage5.Enter += new EventHandler(tabPage5_Enter);
            //tabSystem.Enter += new EventHandler(tabSystem_Enter);

            //dtpStartData.ValueChanged += new EventHandler(dtpStartData_ValueChanged);
            //dgvOscill.CellContentClick += new DataGridViewCellEventHandler(dgvOscill_CellContentClick);
            //btnReNewOD.Click += new EventHandler(btnReNewOD_Click);

            tbBMCS_Indication_Enter(this, new EventArgs());
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
                        //tabControl1.SelectedTab = tb tbpAvar;
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
        /// ���������� ����������� ��������� ����� �� ��������� Device.cfg (��. ��� � - 00)
        /// </summary>
        /// <param Name="pathtoDevCFG">���� � ����� � ��������� ���������� - ���� Device.cfg</param>
        private void SetElementsFormFeatures(string nfrmXML)
        {

            try
            {
                //xdoc = XDocument.Load( pathtoDevCFG + Path.DirectorySeparatorChar + "Device.cfg" );
                xdoc = XDocument.Load(nfrmXML);

                // ���� �������� ����� �� Name
                if (String.IsNullOrEmpty((string)xdoc.Element("MT").Element("BMRZ").Element("frame").Element("GDIFeatures")))
                    return;

                IEnumerable<XElement> collNameEl = from tp in xdoc.Element("MT").Element("BMRZ").Element("frame").Element("GDIFeatures").Element("ElementsAction").Elements("Element")
                                                   select tp;

                // ���������� �������� � ������
                foreach (XElement xecollNameEl in collNameEl)
                    slFormElements.Add(xecollNameEl.Attribute("name").Value, null);

                // ���� �������� ������ �� �����
                ControlCollection cc;
                cc = (ControlCollection)this.Controls;
                for (int i = 0; i < cc.Count; i++)
                    if (slFormElements.Contains(cc[i].Name))
                        slFormElements[cc[i].Name] = cc[i];
                    else
                        TestCCforElements(cc[i]);

                // �������� �������� ���������
                foreach (XElement xecollNameEl in collNameEl)
                {
                    Control cntrl = (Control)slFormElements[xecollNameEl.Attribute("name").Value];
                    switch (xecollNameEl.Element("Property").Attribute("name").Value)
                    {
                        case "Enabled":
                            ((Control)slFormElements[xecollNameEl.Attribute("name").Value]).Enabled = Convert.ToBoolean(xecollNameEl.Element("Property").Value);
                            break;
                        case "Text":
                            ((Control)slFormElements[xecollNameEl.Attribute("name").Value]).Text = xecollNameEl.Element("Property").Value;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
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
                   IEnumerable<XElement> xefs = xegr.Elements( "formula" );
                   foreach ( XElement xef in xefs )
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

                      sw.WriteLine( sl["caption"] );
                      sw.Flush();

                      switch( ( string ) sl["TypeOfTag"] )
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
                            MessageBox.Show( "��� ������ ���� �������" );
                            break;
                      }
                      ToP = (string) sl["TypeOfPanel"];

                      // ������ ����
                   alVal.Clear();
                   IEnumerable<XElement> xfts = xef.Elements( "value" );
                   StringBuilder sbtag = new StringBuilder();

                   foreach (XElement xft in xfts)
                   {
                      sbtag.Clear();
                      sbtag.Append(xft.Attribute( "tag" ).Value);
                      if (sbtag.ToString() == "external")
                      {
                         sbtag.Clear();
                         //if (slKoefRatioValue.ContainsKey(xft.Attribute("id").Value))
                         //   sbtag.Append(slKoefRatioValue[xft.Attribute("id").Value]);
                         //else
                         //   sbtag.Append(xft.Attribute("DefaultValue").Value);
                      }
                      alVal.Add( sbtag.ToString() );
                   }

                   if( bitmask == "" || bitmask == null )	// bitmask - ��� ������ � BCD-���������� ������� �� �����
                   {
                      frmtgs.Length = 0;
                      for (int i = 0; i < alVal.Count; i++)
                      {
                         frmtgs.Append(i.ToString() + "(" + strFC + "." + strIDDev + ( string ) alVal[i] + ")");
                      }
                      arrVar.Add(new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, frmtgs.ToString(), sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP));

                   }
                   else
                   {
                      if( alVal.Count == 2 )
                          arrVar.Add(new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + strFC + "." + strIDDev + (string)alVal[0] + ")1(" + strFC + "." + strIDDev + (string)alVal[1] + ")", sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP, bitmask));
                      else
                          arrVar.Add(new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + strFC + "." + strIDDev + (string)alVal[0] + ")", sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP, bitmask));
                   }
                }

                   xefs = xegr.Elements( "simple_eval" );
                   foreach ( XElement xef in xefs )
                   {
                      sbse.Clear();
                      sbse.Append(xef.Attribute ( "name" ).Value );
                      se[sbse.ToString()] = xef.Element("value").Attribute("tag").Value;
                   }
               
                  xefs = xegr.Elements( "name_tabpage_ust" );
                  foreach (XElement xef in xefs)
                  {   // ��������� �������� ������� � �������� � ������������
                      sbse.Length = 0;
                      sbse.Append(xef.Attribute("name").Value);
                      for (int i = 0; i < tabCntrl.Controls.Count; i++)// tbkConfig
                      {
                          if (tabCntrl.Controls[i] is TabPage && tabCntrl.Controls[i].Name == sbse.ToString())
                          {
                              tabCntrl.Controls[i].Text = xef.Element("value").Attribute("tag").Value;
                              sl_tpnameUst[sbse.ToString()] = tabCntrl.Controls[i];
                          }
                      }
                  }
 			      } catch( XmlException ee )
			      {
                  System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMRZ : CreateArrayList() : ������ :" + ee.Message);
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

              #region ������������� �� ���������� ����� � DataServer
              //taglist = new List<ITag>();

                //foreach (FormulaEvalNDS fe in arrVar)
                //    taglist.Add(fe.LinkVariableNewDS);

                //HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags(taglist);
                  //      break; 
	            #endregion
          }

		private void TestCCforFLP( Control cc, SortedList sl )
		{
			for( int i = 0 ; i < cc.Controls.Count ; i++ )
			{
				if( cc.Controls[i] is FlowLayoutPanel )
				{
					FlowLayoutPanel flp = ( FlowLayoutPanel ) cc.Controls[i];
					sl[flp.Name] = flp;
				}
				else
					TestCCforFLP( cc.Controls[i], sl );
			}
		}


		#region ���� �� ������� ���������
		private void tbBMCS_Indication_Enter( object sender, EventArgs e )
		{
				timerInd.Start();

                if (arrCurSign.Count != 0)
                {
                    tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbBMCS_Indication);
                    return;
                }

            this.tabControl1.Visible = false;
			CreateArrayList( arrCurSign, "arrCurSign" );

			// ��������� ����������� �� �����
			for( int i = 0 ; i < arrCurSign.Count ; i++ )
			{
                FormulaEvalNDS ev = (FormulaEvalNDS)arrCurSign[i];
				// ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch (ev.ToT)
                {
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox(ev);
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;
                        
                        Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                        bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                        usTB.txtLabelText.DataBindings.Add(bnd);
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.NameFE;
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
                        chBV.checkBox1.Text = ev.NameFE;
                        break;
                    case TypeOfTag.no:
                        break;
                    default:
                        MessageBox.Show("��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

			}
			CreateArrayList( arrSystemSign, "arrSystemSign" );

			// ��������� ����������� �� �����
			for( int i = 0 ; i < arrSystemSign.Count ; i++ )
			{
                FormulaEvalNDS ev = (FormulaEvalNDS)arrSystemSign[i];
				// ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
			    try
			    {
                    switch (ev.ToT)
                    {
                        case TypeOfTag.Analog:
                            usTB = new ctlLabelTextbox(ev);
                            usTB.LabelText = "";
                            usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                            usTB.AutoSize = true;

                            Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                            usTB.txtLabelText.DataBindings.Add(bnd);
                            ev.LinkVariableNewDS.BindindTag = bnd;
                            usTB.Caption_Text = ev.NameFE;
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
                            chBV.checkBox1.Text = ev.NameFE;
                            break;
                        case TypeOfTag.no:
                            break;
                        default:
                            MessageBox.Show("��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
			    catch(Exception ex)
			    {
				    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			    }

            }
			// ����������� ����������
			IndBind();
            this.tabControl1.Visible = true;
            
            // ������������� �� ����
            tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbBMCS_Indication);
		}

        private void tbBMCS_Indication_Leave(object sender, EventArgs e)
        {
            if (firstShow)
            {
                firstShow = false;
                return;
            }
            timerInd.Stop();
        }
        #region ����������
        private void IndBind()
        {
            FormulaEvalNDS efv;
            ArrayList arrVar = new ArrayList();

            // ���� 1
            //efv = new FormulaEval(parent.KB, "0(" + iFC + "." + iIDDev + ".5.112.0100)", "0", "���� 1", "", TypeOfTag.Discret, "");
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0100)", "0", "���� 1", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_1.LinkSetText_Ind;
            ciIN_1.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0100)", "0", "���� 1", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_1.LinkSetText_IndFlash;
            ciIN_1.SetSubscribeLink2Tag(efv.LinkVariableNewDS); 
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0100)", "0", "���� 1", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_1.LinkSetText_IndErr;
            ciIN_1.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            ciIN_1.OnChangeIndode += this.SynhrInd;
            arrVar.Add(efv);

            //// ���� 2
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0200)", "0", "���� 2", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_2.LinkSetText_Ind;
            ciIN_2.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0200)", "0", "���� 2", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_2.LinkSetText_IndFlash;
            ciIN_2.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0200)", "0", "���� 2", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_2.LinkSetText_IndErr;
            ciIN_2.OnChangeIndode += this.SynhrInd;
            ciIN_2.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 3
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0400)", "0", "���� 3", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_3.LinkSetText_Ind;
            ciIN_3.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0400)", "0", "���� 3", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_3.LinkSetText_IndFlash;
            ciIN_3.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0400)", "0", "���� 3", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_3.LinkSetText_IndErr;
            ciIN_3.OnChangeIndode += this.SynhrInd;
            ciIN_3.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 4
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0800)", "0", "���� 4", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_4.LinkSetText_Ind;
            ciIN_4.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0800)", "0", "���� 4", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_4.LinkSetText_IndFlash;
            ciIN_4.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0800)", "0", "���� 4", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_4.LinkSetText_IndErr;
            ciIN_4.OnChangeIndode += this.SynhrInd;
            ciIN_4.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 5
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.1000)", "0", "���� 5", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_5.LinkSetText_Ind;
            ciIN_5.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.1000)", "0", "���� 5", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_5.LinkSetText_IndFlash;
            ciIN_5.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.1000)", "0", "���� 5", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_5.LinkSetText_IndErr;
            ciIN_5.OnChangeIndode += this.SynhrInd;
            ciIN_5.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 6
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.2000)", "0", "���� 6", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_6.LinkSetText_Ind;
            ciIN_6.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.2000)", "0", "���� 6", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_6.LinkSetText_IndFlash;
            ciIN_6.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.2000)", "0", "���� 6", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_6.LinkSetText_IndErr;
            ciIN_6.OnChangeIndode += this.SynhrInd;
            ciIN_6.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 7
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.4000)", "0", "���� 7", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_7.LinkSetText_Ind;
            ciIN_7.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.4000)", "0", "���� 7", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_7.LinkSetText_IndFlash;
            ciIN_7.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.4000)", "0", "���� 7", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_7.LinkSetText_IndErr;
            ciIN_7.OnChangeIndode += this.SynhrInd;
            ciIN_7.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 8
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.8000)", "0", "���� 8", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_8.LinkSetText_Ind;
            ciIN_8.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.8000)", "0", "���� 8", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_8.LinkSetText_IndFlash;
            ciIN_8.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.8000)", "0", "���� 8", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_8.LinkSetText_IndErr;
            ciIN_8.OnChangeIndode += this.SynhrInd;
            ciIN_8.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 9
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0001)", "0", "���� 9", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_9.LinkSetText_Ind;
            ciIN_9.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0001)", "0", "���� 9", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_9.LinkSetText_IndFlash;
            ciIN_9.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0001)", "0", "���� 9", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_9.LinkSetText_IndErr;
            ciIN_9.OnChangeIndode += this.SynhrInd;
            ciIN_9.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 10
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0002)", "0", "���� 10", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_10.LinkSetText_Ind;
            ciIN_10.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0002)", "0", "���� 10", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_10.LinkSetText_IndFlash;
            ciIN_10.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0002)", "0", "���� 10", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_10.LinkSetText_IndErr;
            ciIN_10.OnChangeIndode += this.SynhrInd;
            ciIN_10.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 11
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0004)", "0", "���� 11", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_11.LinkSetText_Ind;
            ciIN_11.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0004)", "0", "���� 11", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_11.LinkSetText_IndFlash;
            ciIN_11.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0004)", "0", "���� 11", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_11.LinkSetText_IndErr;
            ciIN_11.OnChangeIndode += this.SynhrInd;
            ciIN_11.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 12
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0008)", "0", "���� 12", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_12.LinkSetText_Ind;
            ciIN_12.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0008)", "0", "���� 12", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_12.LinkSetText_IndFlash;
            ciIN_12.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0008)", "0", "���� 12", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_12.LinkSetText_IndErr;
            ciIN_12.OnChangeIndode += this.SynhrInd;
            ciIN_12.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 13
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0010)", "0", "���� 13", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_13.LinkSetText_Ind;
            ciIN_13.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0010)", "0", "���� 13", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_13.LinkSetText_IndFlash;
            ciIN_13.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0010)", "0", "���� 13", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_13.LinkSetText_IndErr;
            ciIN_13.OnChangeIndode += this.SynhrInd;
            ciIN_13.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 14
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0020)", "0", "���� 14", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_14.LinkSetText_Ind;
            ciIN_14.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0020)", "0", "���� 14", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_14.LinkSetText_IndFlash;
            ciIN_14.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0020)", "0", "���� 14", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_14.LinkSetText_IndErr;
            ciIN_14.OnChangeIndode += this.SynhrInd;
            ciIN_14.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 15
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0040)", "0", "���� 15", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_15.LinkSetText_Ind;
            ciIN_15.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0040)", "0", "���� 15", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_15.LinkSetText_IndFlash;
            ciIN_15.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0040)", "0", "���� 15", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_15.LinkSetText_IndErr;
            ciIN_15.OnChangeIndode += this.SynhrInd;
            ciIN_15.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 16
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.112.0080)", "0", "���� 16", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_16.LinkSetText_Ind;
            ciIN_16.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.115.0080)", "0", "���� 16", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_16.LinkSetText_IndFlash;
            ciIN_16.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.118.0080)", "0", "���� 16", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_16.LinkSetText_IndErr;
            ciIN_16.OnChangeIndode += this.SynhrInd;
            ciIN_16.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 17
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0100)", "0", "���� 17", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_17.LinkSetText_Ind;
            ciIN_17.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0100)", "0", "���� 17", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_17.LinkSetText_IndFlash;
            ciIN_17.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0100)", "0", "���� 17", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_17.LinkSetText_IndErr;
            ciIN_17.OnChangeIndode += this.SynhrInd;
            ciIN_17.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 18
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0200)", "0", "���� 18", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_18.LinkSetText_Ind;
            ciIN_18.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0200)", "0", "���� 18", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_18.LinkSetText_IndFlash;
            ciIN_18.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0200)", "0", "���� 18", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_18.LinkSetText_IndErr;
            ciIN_18.OnChangeIndode += this.SynhrInd;
            ciIN_18.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 19
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0400)", "0", "���� 19", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_19.LinkSetText_Ind;
            ciIN_19.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0400)", "0", "���� 19", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_19.LinkSetText_IndFlash;
            ciIN_19.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0400)", "0", "���� 19", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_19.LinkSetText_IndErr;
            ciIN_19.OnChangeIndode += this.SynhrInd;
            ciIN_19.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 20
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0800)", "0", "���� 20", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_20.LinkSetText_Ind;
            ciIN_20.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0800)", "0", "���� 20", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_20.LinkSetText_IndFlash;
            ciIN_20.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0800)", "0", "���� 20", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_20.LinkSetText_IndErr;
            ciIN_20.OnChangeIndode += this.SynhrInd;
            ciIN_20.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 21
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.1000)", "0", "���� 21", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_21.LinkSetText_Ind;
            ciIN_21.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.1000)", "0", "���� 21", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_21.LinkSetText_IndFlash;
            ciIN_21.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.1000)", "0", "���� 21", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_21.LinkSetText_IndErr;
            ciIN_21.OnChangeIndode += this.SynhrInd;
            ciIN_21.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 22
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.2000)", "0", "���� 22", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_22.LinkSetText_Ind;
            ciIN_22.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.2000)", "0", "���� 22", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_22.LinkSetText_IndFlash;
            ciIN_22.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.2000)", "0", "���� 22", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_22.LinkSetText_IndErr;
            ciIN_22.OnChangeIndode += this.SynhrInd;
            ciIN_22.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 23
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.4000)", "0", "���� 23", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_23.LinkSetText_Ind;
            ciIN_23.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.4000)", "0", "���� 23", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_23.LinkSetText_IndFlash;
            ciIN_23.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.4000)", "0", "���� 23", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_23.LinkSetText_IndErr;
            ciIN_23.OnChangeIndode += this.SynhrInd;
            ciIN_23.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 24
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.8000)", "0", "���� 24", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_24.LinkSetText_Ind;
            ciIN_24.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.8000)", "0", "���� 24", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_24.LinkSetText_IndFlash;
            ciIN_24.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.8000)", "0", "���� 24", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_24.LinkSetText_IndErr;
            ciIN_24.OnChangeIndode += this.SynhrInd;
            ciIN_24.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 25
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0001)", "0", "���� 25", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_25.LinkSetText_Ind;
            ciIN_25.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0001)", "0", "���� 25", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_25.LinkSetText_IndFlash;
            ciIN_25.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0001)", "0", "���� 25", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_25.LinkSetText_IndErr;
            ciIN_25.OnChangeIndode += this.SynhrInd;
            ciIN_25.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 26
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0002)", "0", "���� 26", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_26.LinkSetText_Ind;
            ciIN_26.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0002)", "0", "���� 26", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_26.LinkSetText_IndFlash;
            ciIN_26.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0002)", "0", "���� 26", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_26.LinkSetText_IndErr;
            ciIN_26.OnChangeIndode += this.SynhrInd;
            ciIN_26.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 27
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0004)", "0", "���� 27", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_27.LinkSetText_Ind;
            ciIN_27.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0004)", "0", "���� 27", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_27.LinkSetText_IndFlash;
            ciIN_27.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0004)", "0", "���� 27", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_27.LinkSetText_IndErr;
            ciIN_27.OnChangeIndode += this.SynhrInd;
            ciIN_27.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 28
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0008)", "0", "���� 28", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_28.LinkSetText_Ind;
            ciIN_28.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0008)", "0", "���� 28", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_28.LinkSetText_IndFlash;
            ciIN_28.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0008)", "0", "���� 28", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_28.LinkSetText_IndErr;
            ciIN_28.OnChangeIndode += this.SynhrInd;
            ciIN_28.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 29
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0010)", "0", "���� 29", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_29.LinkSetText_Ind;
            ciIN_29.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0010)", "0", "���� 29", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_29.LinkSetText_IndFlash;
            ciIN_29.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0010)", "0", "���� 29", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_29.LinkSetText_IndErr;
            ciIN_29.OnChangeIndode += this.SynhrInd;
            ciIN_29.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 30
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0020)", "0", "���� 30", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_30.LinkSetText_Ind;
            ciIN_30.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0020)", "0", "���� 30", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_30.LinkSetText_IndFlash;
            ciIN_30.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0020)", "0", "���� 30", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_30.LinkSetText_IndErr;
            ciIN_30.OnChangeIndode += this.SynhrInd;
            ciIN_30.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 31
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0040)", "0", "���� 31", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_31.LinkSetText_Ind;
            ciIN_31.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0040)", "0", "���� 31", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_31.LinkSetText_IndFlash;
            ciIN_31.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0040)", "0", "���� 31", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_31.LinkSetText_IndErr;
            ciIN_31.OnChangeIndode += this.SynhrInd;
            ciIN_31.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� 32
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.113.0080)", "0", "���� 32", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_32.LinkSetText_Ind;
            ciIN_32.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.116.0080)", "0", "���� 32", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_32.LinkSetText_IndFlash;
            ciIN_32.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.119.0080)", "0", "���� 32", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciIN_32.LinkSetText_IndErr;
            ciIN_32.OnChangeIndode += this.SynhrInd;
            ciIN_32.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� ��� 1
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.114.0100)", "0", "���� ��� 1", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_1.LinkSetText_Ind;
            ciKIS_1.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.117.0100)", "0", "���� ��� 1", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_1.LinkSetText_IndFlash;
            ciKIS_1.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.120.0100)", "0", "���� ��� 1", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_1.LinkSetText_IndErr;
            ciKIS_1.OnChangeIndode += this.SynhrInd;
            ciKIS_1.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� ��� 2
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.114.0200)", "0", "���� ��� 2", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_2.LinkSetText_Ind;
            ciKIS_2.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.117.0200)", "0", "���� ��� 2", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_2.LinkSetText_IndFlash;
            ciKIS_2.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.120.0200)", "0", "���� ��� 2", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_2.LinkSetText_IndErr;
            ciKIS_2.OnChangeIndode += this.SynhrInd;
            ciKIS_2.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� ��� 3
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.114.0400)", "0", "���� ��� 3", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_3.LinkSetText_Ind;
            ciKIS_3.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.117.0400)", "0", "���� ��� 3", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_3.LinkSetText_IndFlash;
            ciKIS_3.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.120.0400)", "0", "���� ��� 3", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_3.LinkSetText_IndErr;
            ciKIS_3.OnChangeIndode += this.SynhrInd;
            ciKIS_3.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// ���� ��� 4
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.114.0800)", "0", "���� ��� 4", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_4.LinkSetText_Ind;
            ciKIS_4.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.117.0800)", "0", "���� ��� 4", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_4.LinkSetText_IndFlash;
            ciKIS_4.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);
            efv = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + iFC + "." + iIDDev + ".5.120.0800)", "0", "���� ��� 4", "", TypeOfTag.Discret, "");
            efv.OnChangeValFormTI += ciKIS_4.LinkSetText_IndErr;
            ciKIS_4.OnChangeIndode += this.SynhrInd;
            ciKIS_4.SetSubscribeLink2Tag(efv.LinkVariableNewDS);
            arrVar.Add(efv);

            //// �������� � ������-��������� �������� ������ ����������
            //if (HMI_Settings.ClientDFE != null)
            //    foreach (FormulaEval fe in arrVar)
            //        HMI_Settings.ClientDFE.AddArrTags(this.Text, fe);


            //taglistIndicators.Clear();

            //foreach (FormulaEvalNDS fe in arrVar)
            //    taglistIndicators.Add(fe.LinkVariableNewDS);

            //HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags(taglistIndicators);

            arrIndicators.Add(ciIN_1);
            arrIndicators.Add(ciIN_2);
            arrIndicators.Add(ciIN_3);
            arrIndicators.Add(ciIN_4);
            arrIndicators.Add(ciIN_5);
            arrIndicators.Add(ciIN_6);
            arrIndicators.Add(ciIN_7);
            arrIndicators.Add(ciIN_8);
            arrIndicators.Add(ciIN_9);
            arrIndicators.Add(ciIN_10);
            arrIndicators.Add(ciIN_11);
            arrIndicators.Add(ciIN_12);
            arrIndicators.Add(ciIN_13);
            arrIndicators.Add(ciIN_14);
            arrIndicators.Add(ciIN_15);
            arrIndicators.Add(ciIN_16);
            arrIndicators.Add(ciIN_17);
            arrIndicators.Add(ciIN_18);
            arrIndicators.Add(ciIN_19);
            arrIndicators.Add(ciIN_20);
            arrIndicators.Add(ciIN_21);
            arrIndicators.Add(ciIN_22);
            arrIndicators.Add(ciIN_23);
            arrIndicators.Add(ciIN_24);
            arrIndicators.Add(ciIN_25);
            arrIndicators.Add(ciIN_26);
            arrIndicators.Add(ciIN_27);
            arrIndicators.Add(ciIN_28);
            arrIndicators.Add(ciIN_29);
            arrIndicators.Add(ciIN_30);
            arrIndicators.Add(ciIN_31);
            arrIndicators.Add(ciIN_32);
            arrIndicators.Add(ciKIS_1);
            arrIndicators.Add(ciKIS_2);
            arrIndicators.Add(ciKIS_3);
            arrIndicators.Add(ciKIS_4);
        }

        public void SynhrInd()
        {
            foreach (CustomIndicator ci in arrIndicators)
            {
                ci.counter = 0;
                ci.V_IndErrPrev = false;
                ci.V_IndFlashPrev = false;
                ci.V_IndPrev = false;
            }
        }

        private void timerInd_Tick(object sender, EventArgs e)
        {
            foreach (CustomIndicator ci in arrIndicators)
            {
                ci.Renew();
            }
        }
        #endregion

        private void btnAck_Click(object sender, EventArgs e)
        {
            if (CommonUtils.CommonUtils.IsUserActionBan(CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, HMI_MT_Settings.HMI_Settings.UserRight))
                return;

            //if (parent.isReqPassword && !parent.CanAction())
            //{
            //    MessageBox.Show("���������� �������� ���������");
            //    return;
            //}

            //ConfirmCommand dlg = new ConfirmCommand();
            //dlg.label1.Text = "�����������?";

            //if (!(DialogResult.OK == dlg.ShowDialog()))
            //    return;
            //   // ��������� �������� �� ������������
            //   Console.WriteLine( "��������� ������� \"�����������\" ��� ����������: {0}; id: {1}", "����", iIDDev );
            //   // ������ � ������
            //   parent.WriteEventToLog(20, strIDDev, "����", true);//, true, false );

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "ECC", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "������� \"�����������\" ���� � ����. ���������� - "
            //           + strFC + "." + strIDDev, "����", true);//, true, false );

            // ���������� ������ � ������ �������� ������������
            // ����� ���������� � ������ ��
            int numdevfc = iFC * 256 + iIDDev;
            CommonUtils.CommonUtils.WriteEventToLog(20, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "ECC", new byte[] { }, this);

        }
		#endregion

        #region ���� �� ������� ����������
        private void tbBMCS_Store_Enter(object sender, EventArgs e)
        {
            if (arrStoreSign.Count != 0)
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbBMCS_Store);
                return;
            }

            CreateArrayList(arrStoreSign, "arrStoreSign");

            // ��������� ����������� �� �����
            for (int i = 0; i < arrStoreSign.Count; i++)
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrStoreSign[i];
                // ������� ��������� ������� ��� ���������� ���� � ��� ���
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch (ev.ToT)
                {
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox(ev);
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                        bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                        usTB.txtLabelText.DataBindings.Add(bnd);
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;   //NameFE?
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
                    case TypeOfTag.no:
                        break;
                    default:
                        MessageBox.Show("��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }

            // ������������� �� ����
            tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbBMCS_Store);
        }

        void tbBMCS_Store_Leave(object sender, EventArgs e)
        {
        }

        private void btnBMCS_Store_Read_Click(object sender, EventArgs e)
        {
            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RCD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog(35, "������� \"RCD\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true);//, true, false );
            //   // ���������������� �������� ������������
            //   parent.WriteEventToLog(8, iIDDev.ToString(), this.Name, true);//, true, false );//"������ ������� RCD - ������ �������������."
            //   HMI_Settings.ClientDFE.SetReq4PeriodicPacketQuery(iFC, iIDDev, 7);

            // ���������� ������ � ������ �������� ������������
            // ����� ���������� � ������ ��
            int numdevfc = iFC * 256 + iIDDev;
            CommonUtils.CommonUtils.WriteEventToLog(35, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "RCD", new byte[] { }, this);
        }

        private void btnBMCS_Store_Reset_Click(object sender, EventArgs e)
        {
            if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b06_Reset_info, HMI_MT_Settings.HMI_Settings.UserRight ) )
                return;

            DialogResult dr = MessageBox.Show( "�������� ������������� ���������� �����?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
            if( dr != DialogResult.Yes )
                return;
            //{
            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "CCD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //        parent.WriteEventToLog(35, "������� \"CCD\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true);//, true, false );

            //    // ���������������� �������� ������������
            //    parent.WriteEventToLog(9, iIDDev.ToString(), this.Name, true);//, true, false );//"������ ������� CCD - ����� �������������."
            //    HMI_Settings.ClientDFE.SetReq4PeriodicPacketQuery(iFC, iIDDev, 7);
            //}

            // ���������� ������ � ������ �������� ������������
            // ����� ���������� � ������ ��
            int numdevfc = iFC * 256 + iIDDev;
            CommonUtils.CommonUtils.WriteEventToLog(35, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "CCD", new byte[] { }, this);
        }        
        #endregion

		#region ���� �� ������� "������������ � �������"
		/// <summary>
		/// private void tbcConfig_Enter( object sender, EventArgs e )
		///  ���� �� ������� "������������ � �������"
		/// </summary>
        private void tbBMCS_Config_Enter(object sender, EventArgs e)
		{
			lstvConfig.Items.Clear();

			UstavBD();
			//-------------------------------------------------------------------
			//������� ���. ��� ����������� ���������� � ���������� ��������
			if( arrConfigSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbBMCS_Config);
                return;
            }

			btnWriteUst.Enabled = false;
			CreateArrayList( arrConfigSign, "arrConfigSign" );

			Control.ControlCollection ccu;
			ccu = ( Control.ControlCollection ) tabCntrl.Controls;	// 
			for( int i = 0 ; i < ccu.Count ; i++ )
			{
				if( ccu[i] is FlowLayoutPanel )
				{
					FlowLayoutPanel flp = ( FlowLayoutPanel ) ccu[i];
					slFLPUst[flp.Name] = flp;
				}
				else
				{
					TestCCforFLP( ccu[i], slFLPUst );
				}
			}

			// ��������� ����������� �� �����
         int icountCB = 0;
         int icountChB = 0;
         int icountCtlTB = 0;
			for( int i = 0 ; i < arrConfigSign.Count ; i++ )
			{
                FormulaEvalNDS ev = (FormulaEvalNDS)arrConfigSign[i];
				// ������� ��������� ������� ��� ���������� ���� � ��� ���
				CheckBoxVar chBV;
				ctlLabelTextbox usTB;
				ComboBoxVar cBV;
            string[ ] str = { "(0) �1", "(1) �2", "(2) �3", "(3) �4", "(4) �5" };
            
            switch( ev.ToT )
				{
                    case TypeOfTag.Combo:
                        //if (ev.addrVar == 62112 && ev.addrVarBitMask == "00ff")
                        //    cBV = new ComboBoxVar(str, 0);
                        //else
                        //    cBV = new ComboBoxVar((string[])((TagEval)((TagVal)ev.arrTagVal[0]).linkTagEval).arrStrCB.ToArray(typeof(string)), 0);
                        //cBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        //cBV.AutoSize = true;
                        //cBV.addrLinkVar = ev.addrVar;
                        //cBV.evt.CBChangeEvent += new CBChangeEventHandler(SetVisTypeD);
                        //// ���� �� �����?
                        //if (ev.addrVarBitMask != null || ev.addrVarBitMask != String.Empty)
                        //    cBV.addrLinkVarBitMask = ev.addrVarBitMask;
                        //ev.OnChangeValForm += cBV.LinkSetText;
                        //ev.FirstValue();

                        if (ev.LinkVariableNewDS != null)
                        {
                            cBV = new ComboBoxVar(ev.LinkVariableNewDS.SlEnumsParty, 0, ev);
                            cBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                            cBV.AutoSize = true;
                            cBV.TypeView = TypeViewValue.Textbox;//.Combobox;

                            Binding bndcb = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                            cBV.tbText.DataBindings.Add(bndcb);
                            ev.LinkVariableNewDS.BindindTag = bndcb;
                            cBV.lblCaption.Text = ev.CaptionFE;
                        }
                        else
                            icountCB = 0;
                        break;
                    case TypeOfTag.Analog:
                        //usTB = new ctlLabelTextbox();
                        //usTB.lblCaption.Text = "";
                        //usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        //usTB.AutoSize = true;
                        //usTB.addrLinkVar = ev.addrVar;
                        //usTB.txtLabelText.ReadOnly = false;
                        //ev.StrFormat = HMI_Settings.Precision;
                        //ev.OnChangeValForm += usTB.LinkSetText;
                        //ev.FirstValue();
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
                        //chBV = new CheckBoxVar();
                        //chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        //chBV.AutoSize = true;
                        //chBV.addrLinkVar = ev.addrVar;
                        //chBV.addrLinkVarBitMask = ev.addrVarBitMask;
                        //chBV.btnCheck.Visible = false;
                        //ev.OnChangeValForm += chBV.LinkSetText;
                        //ev.FirstValue();
							 chBV = new CheckBoxVar(ev);
                             chBV.IsClickable = true;
							 chBV.CheckBox_Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;

							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
							bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.CheckBox_Text = ev.CaptionFE;
                        break;
                    default:
                        MessageBox.Show("��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
				}
			}
         // ������� 1-8
         arrFlpCfg.Add( BMCS_Config_In_TypeD_1_8 );
         SetTagName( BMCS_Config_In_TypeD_1_8 , "in_");
         arrFlpCfg.Add( BMCS_Config_In_Trog_1_8 );
         SetTagName( BMCS_Config_In_Trog_1_8, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_Ret_1_8 );
         SetTagName( BMCS_Config_In_Ret_1_8, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC1_1_8 );
         SetTagName( BMCS_Config_In_OC1_1_8, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC2_1_8 );
         SetTagName( BMCS_Config_In_OC2_1_8, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC3_1_8 );
         SetTagName( BMCS_Config_In_OC3_1_8, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC4_1_8 );
         SetTagName( BMCS_Config_In_OC4_1_8, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC5_1_8 );
         SetTagName( BMCS_Config_In_OC5_1_8, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_ZS_1_8 );
         SetTagName( BMCS_Config_In_ZS_1_8, "in_" );
         // ������� 9-16
         arrFlpCfg.Add( BMCS_Config_In_TypeD_9_16 );
         SetTagName( BMCS_Config_In_TypeD_9_16, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_Trog_9_16 );
         SetTagName( BMCS_Config_In_Trog_9_16, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_Ret_9_16 );
         SetTagName( BMCS_Config_In_Ret_9_16, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC1_9_16 );
         SetTagName( BMCS_Config_In_OC1_9_16, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC2_9_16 );
         SetTagName( BMCS_Config_In_OC2_9_16, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC3_9_16 );
         SetTagName( BMCS_Config_In_OC3_9_16, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC4_9_16 );
         SetTagName( BMCS_Config_In_OC4_9_16, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC5_9_16 );
         SetTagName( BMCS_Config_In_OC5_9_16, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_ZS_9_16 );
         SetTagName( BMCS_Config_In_ZS_9_16, "in_" );
         // ������� 17-24
         arrFlpCfg.Add( BMCS_Config_In_TypeD_17_24 );
         SetTagName( BMCS_Config_In_TypeD_17_24, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_Trog_17_24 );
         SetTagName( BMCS_Config_In_Trog_17_24, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_Ret_17_24 );
         SetTagName( BMCS_Config_In_Ret_17_24, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC1_17_24 );
         SetTagName( BMCS_Config_In_OC1_17_24, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC2_17_24 );
         SetTagName( BMCS_Config_In_OC2_17_24, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC3_17_24 );
         SetTagName( BMCS_Config_In_OC3_17_24, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC4_17_24 );
         SetTagName( BMCS_Config_In_OC4_17_24, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC5_17_24 );
         SetTagName( BMCS_Config_In_OC5_17_24, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_ZS_17_24 );
         SetTagName( BMCS_Config_In_ZS_17_24, "in_" );
         // ������� 25-32
         arrFlpCfg.Add( BMCS_Config_In_TypeD_25_32 );
         SetTagName( BMCS_Config_In_TypeD_25_32, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_Trog_25_32 );
         SetTagName( BMCS_Config_In_Trog_25_32, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_Ret_25_32 );
         SetTagName( BMCS_Config_In_Ret_25_32, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC1_25_32 );
         SetTagName( BMCS_Config_In_OC1_25_32, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC2_25_32 );
         SetTagName( BMCS_Config_In_OC2_25_32, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC3_25_32 );
         SetTagName( BMCS_Config_In_OC3_25_32, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC4_25_32 );
         SetTagName( BMCS_Config_In_OC4_25_32, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_OC5_25_32 );
         SetTagName( BMCS_Config_In_OC5_25_32, "in_" );
         arrFlpCfg.Add( BMCS_Config_In_ZS_25_32 );
         SetTagName( BMCS_Config_In_ZS_25_32, "in_" );
         // ������� ����� ��� 
         arrFlpCfg.Add( BMCS_Config_InKis_TypeD );
         SetTagName( BMCS_Config_InKis_TypeD, "kis_" );
         arrFlpCfg.Add( BMCS_Config_InKis_OC1);
         SetTagName( BMCS_Config_InKis_OC1, "kis_" );
         arrFlpCfg.Add( BMCS_Config_InKis_OC2);
         SetTagName( BMCS_Config_InKis_OC2, "kis_" );
         arrFlpCfg.Add( BMCS_Config_InKis_OC3);
         SetTagName( BMCS_Config_InKis_OC3, "kis_" );
         arrFlpCfg.Add( BMCS_Config_InKis_OC4);
         SetTagName( BMCS_Config_InKis_OC4, "kis_" );
         arrFlpCfg.Add( BMCS_Config_InKis_OC5);
         SetTagName( BMCS_Config_InKis_OC5, "kis_" );
         arrFlpCfg.Add( BMCS_Config_ZsKis );
         SetTagName( BMCS_Config_ZsKis, "kis_" );

         for( int i = 1 ;i <= 32 ; i++ )
            SetCfgStrStatus( "in_" + i.ToString(), false );
         for( int i = 1 ;i <= 4 ;i++ )
            SetCfgStrStatus( "kis_" + i.ToString( ), false );

         tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbBMCS_Config);
		}

        void tbBMCS_Config_Leave(object sender, EventArgs e)
        {
        }

      private void SetTagName( FlowLayoutPanel flp, string nmn)
      {
         for( int i = 0 ;i < flp.Controls.Count ; i++ )
         {
            if( flp.Controls[ i ] is ctlLabelTextbox )
            {
               ctlLabelTextbox cltb = ( ctlLabelTextbox ) flp.Controls[ i ];
               cltb.Tag = nmn + cltb.lblCaption.Text;
            }
            else if( flp.Controls[ i ] is CheckBoxVar )
            {
               CheckBoxVar chbv = ( CheckBoxVar ) flp.Controls[ i ];
               chbv.Tag = nmn + chbv.checkBox1.Text;
            }
         }
         for( int i = 0 ;i < flp.Controls.Count ;i++ )
         {
            if( flp.Controls[ i ] is ComboBoxVar )
            {
               ComboBoxVar cbv = ( ComboBoxVar ) flp.Controls[ i ];
               cbv.Tag = nmn + cbv.lblCaption.Text;
            }
         }
      }

      private void UstavBD()
		{
            DataBaseReq dbs = new DataBaseReq(HMI_Settings.ProviderPTK_SQL, "ShowDataLog2");

            // ������� ���������
            // 1. ip FC
            dbs.AddCMDParams(new DataBaseParameter("@IP", ParameterDirection.Input, SqlDbType.BigInt, 0));
            // 2. id ����������
            dbs.AddCMDParams(new DataBaseParameter("@id", ParameterDirection.Input, SqlDbType.Int, iFC * 256 + iIDDev));
            // 3. ��������� �����
            TimeSpan tss = new TimeSpan(0, dtpStartDateConfig.Value.Hour - dtpStartTimeConfig.Value.Hour, dtpStartDateConfig.Value.Minute - dtpStartTimeConfig.Value.Minute, dtpStartDateConfig.Value.Second - dtpStartTimeConfig.Value.Second);
            DateTime tim = dtpStartDateConfig.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_start", ParameterDirection.Input, SqlDbType.DateTime, tim));
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

            //dbs.DoStoredProcedure();

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

		private void btnReadUstFC_Click( object sender, EventArgs e )
		{
            btnWriteUst.Enabled = true;

            // ���������� ������ � ������ �������� ������������
            // ����� ���������� � ������ ��
            int numdevfc = iFC * 256 + iIDDev;
            CommonUtils.CommonUtils.WriteEventToLog(7, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "IMP", new byte[] { }, this);

            //if (parent.newKB.ExecuteCommand(iFC, iIDDev, "IMP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
            //    parent.WriteEventToLog(35, "������� \"IMP\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true);//, true, false);

            //tcUstConfigBottomPanel.SelectTab(0);
        }

      /// <summary>
      /// ��������� Enable ��� ��������� � �����. �� �������� ���� �������
      /// </summary>
      private void SetVisTypeD(object sender)
      {
         ComboBoxVar cbv = ( ComboBoxVar ) sender;

        if( cbv.Tag == null )
            return;

         if( cbv.cbVar.Text.Trim( ) == "(0)����" )
            SetCfgStrStatus( cbv.Tag.ToString(), false );
         else
            SetCfgStrStatus( cbv.Tag.ToString( ), true );

         if( cbv.cbVar.Text.Trim( ) == "(3)���" || cbv.cbVar.Text.Trim( ) == "(4)���" || cbv.cbVar.Text.Trim( ) == "(5)����" )
            SetCLMVozvratStatus( cbv.Tag.ToString( ), false );
      }

      /// <summary>
      /// Disable ����� �������� ��� ����� ��������: (3)��� (4)��� (5)����
      /// </summary>
      /// <param Name="cpt"></param>
      /// <param Name="status"></param>
      private void SetCLMVozvratStatus( string cpt, bool status )
      {
         foreach( FlowLayoutPanel flp in arrFlpCfg )
            if( flp.Name == "BMCS_Config_In_Ret_1_8" || flp.Name == "BMCS_Config_In_Ret_9_16" || flp.Name == "BMCS_Config_In_Ret_17_24" || flp.Name == "BMCS_Config_In_Ret_25_32" )
               for( int i = 0 ;i < flp.Controls.Count ;i++ )
                  if( flp.Controls[ i ] is ctlLabelTextbox )
                  {
                     ctlLabelTextbox cltb = ( ctlLabelTextbox ) flp.Controls[ i ];
                     if( cltb.Tag.ToString( ) != cpt )
                        continue;
                     cltb.txtLabelText.Enabled = status;
                     break;
                  }
      }

      ArrayList arrFlpCfg = new ArrayList( );

      /// <summary>
      /// ���������� ��������� ������ � �������� � ����������� �� ��������� ComboBoxVar
      /// </summary>
      /// <param Name="cpt"></param>
      /// <param Name="status"></param>
      private void SetCfgStrStatus( string cpt, bool status )
      {
         foreach( FlowLayoutPanel flp in arrFlpCfg )
         {
            for( int i = 0 ;i < flp.Controls.Count ;i++ )
            {
               if( flp.Controls[ i ] is ctlLabelTextbox )
               {
                  ctlLabelTextbox cltb = ( ctlLabelTextbox ) flp.Controls[ i ];
                  if( cltb.Tag.ToString() != cpt )
                     continue;
                  cltb.txtLabelText.Enabled = status;
                  break;
               }
               else if( flp.Controls[ i ] is CheckBoxVar )
               {
                  CheckBoxVar chbv = ( CheckBoxVar ) flp.Controls[ i ];
                  if( chbv.Tag.ToString() != cpt )
                     continue;
                  chbv.checkBox1.Enabled = status;
                  break;
               }
            }
         }
      }


		private void lstvConfig_ItemActivate( object sender, EventArgs e )
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
            byte[] str_cnt_in_bytes = Encoding.UTF8.GetBytes(HMI_Settings.ProviderPTK_SQL);
            arparam.Add(str_cnt_in_bytes);

            IRequestData req = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetData(0, (uint)numdevfc, "ArhivUstavkiBlockData", arparam, numdevfc);

            //byte[] adata = DataBaseReq.GetBlockData(HMI_Settings.cstr, (int)lstvConfig.SelectedItems[0].Tag);

            // �������� ��������� ������� ������ �� ����
            // SetArhivGroupInDev(iIDDev, 14);
            // ParseBDPacket(adata, 62000, iIDDev);

            btnWriteUst.Enabled = true;
        }
		private void btnResetValues_Click( object sender, EventArgs e )
		{
			btnWriteUst.Enabled = false;
            //			parent.newKB.ResetGroup( iFC, iIDDev, 14 );
		}
		/// <summary>
		/// private void btnWriteUst_Click( object sender, EventArgs e )
		/// ������ �������
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>  
		private void btnWriteUst_Click( object sender, EventArgs e )
		{
        //    if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, parent.UserRight ) )
        //        return;

        //    if( parent.isReqPassword )
        //        if( !parent.CanAction() )
        //        {
        //            MessageBox.Show( "���������� �������� ���������" );
        //            return;
        //        }

        //    DialogResult dr = MessageBox.Show( "�������� �������?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
        //    if( dr == DialogResult.No )
        //        return;

        //    string stri;
        //    TabPage tp;
        //    ctlLabelTextbox ultb;
        //    CheckBoxVar chbTmp;
        //    ComboBoxVar cbTmp;

        //    FlowLayoutPanel flp;
        //    bool isUstChange = false;   // ���� ��������� ������� ��� ������������ ������������ �������
        //    StringBuilder sb = new StringBuilder();
        //    uint ainmemX;    // ����� � ������� memX
        //    byte[] aTmp2 = new byte[2];
        // StringBuilder tempALVBM = new StringBuilder( ); // ��� ���������� �������� ������������ ������ ��� ���� ByteFieldToComboBox

        //    // ������ SortedList ��� ������� ����������
        //    slLocal = new SortedList();
        //    foreach( FC aFC in parent.KB )
        //        if( aFC.NumFC == iFC )
        //        {
        //            foreach( TCRZADirectDevice aDev in aFC )
        //                if( aDev.NumDev == iIDDev )
        //                {
        //                    slLocal = aDev.CRZAMemDev;
        //                    break;
        //                }
        //            break;
        //        }
			
        //    int lenpack = 0;
        //    try
        //    {
        //        lenpack = BitConverter.ToInt16( ( byte[] ) slLocal[62000], 0 );
        //    } catch( ArgumentNullException ex )
        //    {
        //        MessageBox.Show( "��� ������ ��� ������. \n���������� ������� ��������.", "���������", MessageBoxButtons.OK, MessageBoxIcon.Information );
        //        return;
        //    }

        //    short numdev = BitConverter.ToInt16( ( byte[] ) slLocal[62000], 2 );

        //    ushort add10 = BitConverter.ToUInt16( ( byte[] ) slLocal[62000], 4 );	//������ ����� ����� ������

        //    //int lenpack = ( short ) memDevBlock.ReadInt16();
        //    //short numdev = ( short ) memDevBlock.ReadUInt16();
        //    //ushort add10 = ( ushort ) memDevBlock.ReadInt16();	//������ ����� ����� ������

        //    byte[] memX = new byte[lenpack - 6];
        //    //System.Buffer.BlockCopy( ( byte[] ) slLocal[62000], 6, memX, 0, ( ( byte[] ) slLocal[62000] ).Length - 6 );
        //    System.Buffer.BlockCopy( ( byte[] ) slLocal[62000], 6, memX, 0, lenpack - 6 );

        //    for( int j = 0 ; j < slFLPUst.Count ; j++ )
        //    {
        //        flp = ( FlowLayoutPanel ) slFLPUst.GetByIndex(j);
        //        for( int n = 0 ; n < flp.Controls.Count ; n++ )
        //        {
        //            if( flp.Controls[n] is ctlLabelTextbox )
        //            {
        //                ultb = ( ctlLabelTextbox ) flp.Controls[n];
        //                if( ultb.isChange )
        //                {
        //                    CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 );
        //                    //StrToBCD_Field( ultb, memX );
        //                    isUstChange = true;
        //                }
        //           }
        //           else if( flp.Controls[n] is ComboBoxVar )
        //           {
        //               cbTmp = ( ComboBoxVar ) flp.Controls[n];
        //               if( cbTmp.isChange )
        //               {
        //                   isUstChange = true;
        //                   cbTmp.isChange = false;  // ���������� ������� ��������� � ����������� ComboBoxVar'�
        //                   // ���������� ��������� �� ComboBoxVar'�� � �������� ����� (������������ ������ memX)
        //                   uint a = cbTmp.addrLinkVar; // ����� ����������
        //                   // ������� ��������
        //                   int st = cbTmp.cbVar.SelectedIndex;
        //                   byte[] bst = new byte[4];
        //                   bst = BitConverter.GetBytes( st );
        //             ainmemX = ( a - 62000 ) * 2;

        //             // ����� �������� � ����������� �� ������� �����
        //             if( cbTmp.addrLinkVarBitMask == null || cbTmp.addrLinkVarBitMask == String.Empty )
        //             {
        //                Buffer.BlockCopy( bst, 0, aTmp2, 0, 2 );
        //                Array.Reverse( aTmp2 );
        //                // ���������� ���������
        //                Buffer.BlockCopy( aTmp2, 0, memX, ( int ) ainmemX, 2 );
        //             }
        //             else
        //             {
        //                tempALVBM.Length = 0;
        //                //int jj = 0;
        //                for( int i = cbTmp.addrLinkVarBitMask.Length ; i > 0 ; i-- )
        //                //{
        //                   tempALVBM.Append(cbTmp.addrLinkVarBitMask[i - 1]);
        //                   //jj++;
        //                //}
        //                // ���� ����� ����, �� ����������� � �������������� ���� ��������
        //                for( int i = 0 ;i <= cbTmp.addrLinkVarBitMask.Length / 2 ;i += 2 )
        //                   if( tempALVBM[ i ] == 'f' && tempALVBM[ i + 1 ] == 'f' )
        //                      // ���������� ���������
        //                      Buffer.BlockCopy( bst, i / 2, memX, ( int ) ainmemX, 1 ); 
        //             }						   
        //               }
        //        }
        //           else if( flp.Controls[n] is CheckBoxVar )
        //           {
        //               chbTmp = ( CheckBoxVar ) flp.Controls[n];
        //               if( chbTmp.isChange )
        //               {
        //                   isUstChange = true;
        //                   chbTmp.isChange = false;    // ���������� ������� ��������� � ����������� CheckBoxVar'�
        //                   // �������� ������� ���� �� ��������� �������
        //                   ainmemX = ( chbTmp.addrLinkVar - 62000 ) * 2;   // ��� �����
        //                   //aTmp2 = new byte[2];
        //                   Buffer.BlockCopy( memX, ( int ) ainmemX, aTmp2, 0, 2 );
        //                   string bitmask = chbTmp.addrLinkVarBitMask;
        //                   UInt16 ibitmask = Convert.ToUInt16( chbTmp.addrLinkVarBitMask, 16 );
        //                   Array.Reverse( aTmp2 );
        //                   UInt16 rezbit = BitConverter.ToUInt16( aTmp2, 0 );
        //                   if( chbTmp.checkBox1.Checked == true )
        //                       rezbit = Convert.ToUInt16( rezbit | ibitmask );
        //                   else
        //                   {
        //                       UInt16 ti = ( UInt16 ) ~ibitmask; //Convert.ToUInt16()
        //                       rezbit = Convert.ToUInt16( rezbit & ~ibitmask );
        //                   }
        //                   // �������� �� �����
        //                   aTmp2 = BitConverter.GetBytes( rezbit );
        //                   Array.Reverse( aTmp2 );
        //                   Buffer.BlockCopy( aTmp2, 0, memX, ( int ) ainmemX, 2 );
        //               }
        //           }
        //    }
        //}
        //    //------------------------------
        //    // ���������� ��� ������ �������
        //    //for( int n = 0 ; n < pnlConfig.Controls.Count ; n++ )
        //    for( int n = 0 ; n < Config_BottomPanel.Controls.Count ; n++ )
        //        //if( pnlConfig.Controls[n] is ctlLabelTextbox )
        //        if( ( Config_BottomPanel.Controls[n] as ctlLabelTextbox ) != null )
        //        {
        //            ultb = ( ctlLabelTextbox ) Config_BottomPanel.Controls[n];
        //            if( ultb.Name == "ctlTimeUstavkiSbros" )
        //                continue;

        //            if( ultb.isChange )
        //            {
        //                CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 );
        //                //StrToBCD_Field( ultb, memX );
        //                isUstChange = true;
        //            }
        //        }
        //    //------------------------------
        //    if( !isUstChange )
        //    {
        //        MessageBox.Show( "������� �� ����������. \n���������� ������� ��������.", "���������", MessageBoxButtons.OK, MessageBoxIcon.Information );
        //        return;
        //    }
        //    // ��������� ����� � ������� ��� �������� ��������� �������
        //    byte[] memXOut = new byte[memX.Length];
        //    Buffer.BlockCopy( memX, 4, memXOut, 4, memX.Length - 4 );  // Handle ���� �������

        // if( parent.newKB.ExecuteCommand( iFC, iIDDev, "WCP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
        //        parent.WriteEventToLog(35, "������� \"WCP\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true);//, true, false );
        //    // ���������������� �������� ������������
        //    parent.WriteEventToLog(6, iIDDev.ToString(), this.Name, true);//, true, false );			//"������ ������� WCP - ������ �������."
        //    isUstChange = false;
		}

		private void dtpStartDateConfig_ValueChanged( object sender, EventArgs e )
		{
			//UstavBD();
		}
        /// <summary>
        /// �������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReNew_Click(object sender, EventArgs e)
        {
            UstavBD();
        }

		#endregion        

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

      #region �����-�� ������ ���
      /*==========================================================================*
          *   private void void LinkSetText(object Value)
          *      ��� ����������������� ������ ���������
          *==========================================================================*/
      delegate void SetLVCallback(ListViewItem li, bool actDellstV);

      // actDellstV - �������� � ListView : false - �� �������; true - ��������;
      public void LinkSetLV(object Value, bool actDellstV)
      {
          //if( !( Value is ListViewItem ) && !actDellstV )
          //    return;   // ������������� ������ ����� ����������

          //ListViewItem li = null;
          //if( !actDellstV )
          //    li = ( ListViewItem ) Value;
          //if( this.lstvDump.InvokeRequired )
          //{
          //    if( !actDellstV )
          //        SetLV( li, actDellstV );
          //    else
          //        SetLV( null, actDellstV );
          //}
          //else
          //{
          //    if( !actDellstV )
          //        this.lstvDump.Items.Add( li );
          //    else
          //        this.lstvDump.Items.Clear();
          //}
      }

      /*==========================================================================*
      * private void SetText(ListViewItem li)
      * //��� ����������������� ������ ���������
      *==========================================================================*/
      private void SetLV(ListViewItem li, bool actDellstV)
      {
          //if( this.lstvDump.InvokeRequired )
          //{
          //    SetLVCallback d = new SetLVCallback( SetLV );
          //    this.Invoke( d, new object[] { li, actDellstV } );
          //}
          //else
          //{
          //    if( !actDellstV )
          //        this.lstvDump.Items.Add( li );
          //    else
          //        this.lstvDump.Items.Clear();
          //}
      }

      private void btnPrint_Click(object sender, EventArgs e)
      {
          //PrintHMI frmPrt = new PrintHMI();
          //StringBuilder sb = new StringBuilder(); ;
          //ListViewItem li;
          //// ���������� ���������� ���� ����� lstvDump
          //for( int i = 0; i < lstvDump.Items.Count; i++ )
          //{
          //    sb.Length = 0;
          //    li = new ListViewItem();
          //    li = (ListViewItem)lstvDump.Items[i];
          //    for( int j = 0; j < li.SubItems.Count; j++ )
          //    {
          //        sb.Append( li.SubItems[j].Text);
          //        sb.Append( "\t");
          //    }
          //    sb.Append( "\n" );
          //    frmPrt.rtbText.AppendText( sb.ToString() );
          //}
          //frmPrt.Show();
      }

      private void mnuPageSetup_Click(object sender, EventArgs e)
      {
          //parent.mnuPageSetup_Click( sender, e );
      }

      /// <summary>
      /// mnuPrint_Click( object sender, EventArgs e )
      ///     ���������� ������ ���� ��������������� ��������
      /// </summary>
      private void mnuPrintPreview_Click(object sender, EventArgs e)
      {
          //PrintArr();
          //parent.mnuPrintPreview_Click( sender, e );
      }

      /// <summary>
      /// mnuPrint_Click( object sender, EventArgs e )
      ///     ���������� ������ ���� ������
      /// </summary>
      private void mnuPrint_Click(object sender, EventArgs e)
      {
          //PrintArr();
          //parent.mnuPrint_Click( sender, e );
      }

      #region ����������� ������ DataSet
      static void PrintDataSet(DataSet ds)
      {
          // ����� ��������� ���� �� ���� DataTable ������� DataSet
          Console.WriteLine("������� � DataSet '{0}'. \n ", ds.DataSetName);
          foreach (DataTable dt in ds.Tables)
          {
              Console.WriteLine("������� '{0}'. \n ", dt.TableName);
              // ����� ���� ��������
              for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                  Console.Write(dt.Columns[curCol].ColumnName.Trim() + "\t");
              Console.WriteLine("\n-----------------------------------------------");

              // ����� DataTable
              for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
              {
                  for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                      Console.Write(dt.Rows[curRow][curCol].ToString() + "\t");
                  Console.WriteLine();
              }
          }
      }
      #endregion
      #endregion
	}
}
        