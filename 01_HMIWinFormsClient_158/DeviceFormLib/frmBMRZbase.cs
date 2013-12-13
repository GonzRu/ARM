/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	��������: ����� ��� ������ � ������ (�������).                                                           
 *                                                                             
 *	����                     : frmBMRZbase.cs                                         
 *	��� ��������� �����      : 
 *	������ �� ��� ���������� : �#, Framework 2.0                                
 *	�����������              : ���� �.�.                                        
 *	���� ������ ����������   : xx.04.2007 - xx.06.2009
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
//using NetCrzaDevices;
using System.IO;
using System.Globalization;
//using NSNetNetManager;
using LabelTextbox;
//using CRZADevices;
using CommonUtils;
using System.Linq;
using System.Xml.Linq;
using HMI_MT_Settings;
using InterfaceLibrary;

namespace DeviceFormLib
{
   public partial class frmBMRZbase : Form
   {
      #region ��������
      public ErrorProvider errorProvider
      {
         get
         {
            return errorProvider1;
         }
      }
      public SplitContainer SplitContMain
      {
         get
         {
            return splitContainer1;
         }
      }

      public TabControl tc_Main_frmBMRZbase
      {
         get
         {
            return tc_Main;
         }
      }

      int iFC;            // ����� �� �������������
      public int IFC
      {
         get
         {
            return iFC;
         }
         set
         {
            iFC = value;
         }
      }

      string strFC;       // ����� �� ������
      public string StrFC
      {
         get
         {
            return strFC;
         }
         set
         {
            strFC = value;
         }
      }

      int iIDDev;         // ����� ���������� �������������
      public int IIDDev
      {
         get
         {
            return iIDDev;
         }
         set
         {
            iIDDev = value;
         }
      }

      string strIDDev;    // ����� ���������� ������
      public string StrIDDev
      {
         get
         {
            return strIDDev;
         }
         set
         {
            strIDDev = value;
         }
      }

      int inumLoc;         // ����� ������ �������������
      public int InumLoc
      {
         get
         {
            return inumLoc;
         }
         set
         {
            inumLoc = value;
         }
      }

      string strnumLoc;    // ����� ������ ������
      public string StrnumLoc
      {
         get
         {
            return strnumLoc;
         }
         set
         {
            strnumLoc = value;
         }
      }
      #endregion

      #region public
      //public MainForm parent;
      public XDocument xdoc;
      public SortedList DevPanelTypes;
      public Hashtable htFormulaEvals = new Hashtable();   // ���-������� ��� �������� FormulaEval ��� ������ ����� ���������� - ���� (������ �������, ������ �� FormulaEval)
      public SortedList slTPtoArrVars = new SortedList( );  // ��� �������� ����������� tabpage � ������� ���������� ��� ���� �������
      public SortedList slnflps;                            // ������������� ������ flp � ������� � ��������� �������������, �.�. ���� (Name, Caption)
      public SortedList slFLP = new SortedList( );	         // ��� �������� ��� � FlowLayoutPanel, �.�. ���� (flp.Name, ������ �� FLP)
      public ArrayList arDopPanel;                          // ������ �������������� (������) �������
      public string path2PrgDevCFG = string.Empty;          // PrgDevCFG.cdp
      public string path2FrmDev = string.Empty;             // ���� � ����� �������� ����� �������
      public string path2DeviceCFG = string.Empty;          // device.cfg
      public ErrorProvider erp = new ErrorProvider( );
      /// <summary>
      /// ����� ��� ����� ������ � ���������
      /// </summary>
      public int adrForUstavBlock;
      /// <summary>
      /// ����� ��� ����� ������ � ��������
      /// </summary>
      public int adrForAvarBlock;
      /// <summary>
      /// ������ ��������� �� ������� ������� -  
      /// ��� ����������� ����� �� ���������
      /// </summary>
      public ArrayList UstavkiControls;
      /// <summary>
      /// ���� � ����� � ������� ����������
      /// � ����� Project
      /// </summary>
      public string nfXMLConfig;
      /// <summary>
      /// ��� ����� � �������� �����
      /// </summary>
      public string fileFrmTagsDescript;
      #endregion

      #region private
      string nfXMLConfigFC;                                 // ��� ����� � ��������� ����
      ushort iclm = 16;                                     // ����� ������� � �����
      SortedList slLocal;
      EncodingInfo eii;
      SortedList slEncoding;
      SortedList se = new SortedList( );
      SortedList sl_tpnameUst = new SortedList( );
      StringBuilder sbse = new StringBuilder( );
      DataSet dsTagTables;
      //protected TCRZADirectDevice tcdirdev;                           // ������ �� ����������
      /// <summary>
      /// ������ ���� ������. �� ����� PrgDevCFG.cdp 
      /// ��� ������� ����������
      /// </summary>
      private SortedList<string,string> slKoefRatioValue = new SortedList<string,string>();
      /// <summary>
      /// ������ ����� ��� ��������/�������
      /// </summary>
      //public List<ITag> taglist;
      #endregion

      #region �����������
      public frmBMRZbase( )
      {
         InitializeComponent( );
      }

      public frmBMRZbase(int iFC, int iIDDev, string fXML, string ftagsxmldescript)
      {
         InitializeComponent( );
         //parent = linkMainForm;
         this.iFC = iFC;                 // ����� �� �������������
         strFC = iFC.ToString( );         // ����� �� ������
         this.iIDDev = iIDDev;           // ����� ���������� �������������
         strIDDev = iIDDev.ToString( );   // ����� ���������� ������
         string TypeName = String.Empty;
         string nameR = String.Empty;
         string nameELowLevel = String.Empty;
         string nameEHighLevel = String.Empty;

         fileFrmTagsDescript = ftagsxmldescript;
         nfXMLConfig = fXML;
			try
			{
             /*�������� �������� ����� � ��������� ����������
              ��� ����� ���������� ���� PrgDevCFG.cdp
              */
             //XDocument xdoc_PrgDevCFG = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project\\Configuration\\0#DataServer\\Sources\\MOA_ECU" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp");
             //// ������ ����������
             //IEnumerable<XElement> xefcs = xdoc_PrgDevCFG.Descendants("SourceECU");
             //foreach ( XElement xefc in xefcs )
             //{
             //   if ( xefc.Attribute( "numFC" ).Value == iFC.ToString( ) )
             //   {
             //      IEnumerable<XElement> xedevs = xefc.Descendants( "Device" );
             //      foreach ( XElement xedev in xedevs )
             //      {
             //          XElement xedevv = xedev.Element("DescDev");
             //         if ( xedev.Attribute( "DevGUID" ).Value == ( iIDDev + iFC * 256 ).ToString( ) )
             //         {
             //            TypeName = xedevv.Element( "TypeName" ).Value;
             //            nameR = xedevv.Element( "nameR" ).Value;
             //            nameELowLevel = xedevv.Element( "nameELowLevel" ).Value;
             //            nameEHighLevel = xedevv.Element( "nameEHighLevel" ).Value;
             //            //this.Text = nameR + " ( ��.� " + iIDDev.ToString( ) + " )" + " - ��. �" + xedev.Element( "NumLock" ).Value;
             //            this.Text = nameR + " ( ��.� " + iIDDev.ToString() + " )" + xedevv.Element("DescDev").Value;
             //            // ��������� ������ � ���� ������ ��� ������� ����������
             //            string tr = (string)xedevv.Element("TransformationRatio");
             //            if (tr != null)
             //            //if (!String.IsNullOrEmpty((string)(xedev.Element("TransformationRatio"))))
             //            {
             //               IEnumerable<XElement> xetrs = xedevv.Element("TransformationRatio").Elements();
             //               foreach (XElement xetr in xetrs)
             //                  slKoefRatioValue.Add(xetr.Attribute("id").Value, xetr.Attribute("value").Value);
             //            }
             //         }
             //      }
             //   }
             //}

                /*
                * �������� �������� ����� � ��������� ����������
                * ��� ����� ���������� ���� PrgDevCFG.cdp ���������
                */

                XElement xedev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription(0, "MOA_ECU", iFC * 256 + iIDDev);
                xedev = xedev.Element("DescDev");   // ����������

                TypeName = xedev.Element("TypeName").Value;
                nameR = xedev.Element("nameR").Value;
                nameELowLevel = xedev.Element("nameELowLevel").Value;
                nameEHighLevel = xedev.Element("nameEHighLevel").Value;
                this.Text = nameR + " ( ��.� " + iIDDev.ToString() + " )" + xedev.Element("DescDev").Value;

             //foreach ( DataSource fc in linkMainForm.KB )
             //   if ( fc.NumFC == iFC )
             //      foreach ( TCRZADirectDevice tcdd in fc.Devices )
             //         if ( tcdd.NumDev == iIDDev )
             //            tcdirdev = tcdd;

             //// �� ������ ����������� ����� �������� ���������� ��������� ��������� ������� �� tabcontrole - tcDevTags
             //// ��������� ����������
             ////dsTagTables = new DataSet( );
             ////XDocument xdoc_txt = XDocument.Load( AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + TypeName + Path.DirectorySeparatorChar + "device.cfg" );
             ////IEnumerable<XElement> xegftts = xdoc_txt.Element( "Device" ).Descendants( "GroupInDev" );
             ////foreach ( XElement xegftt in xegftts )
             ////   CreateTPforGroup( xegftt, tcDevTags );			
             }
			    catch(Exception ex)
			    {
				    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			    }
      }
      #endregion

      #region �������� �����
      private void frmBMRZbase_Load( object sender, EventArgs e )
      {
         if ( !DesignMode )
            timer1.Enabled = true;

         GetCCforFLP( ( ControlCollection ) this.Controls );
      }
      #endregion

      #region ���� �� ������� � ����������� � ��������� ���������� � ������ � �� ������ ������� ��� �� ��������� ��� ���������� ������ � �������� ���� �������
      public void tabStatusDev_Command_Enter( object sender, EventArgs e )
      {
         /*
         * �������� ������
         */
          if (arDopPanel != null)
            foreach ( UserControl p in arDopPanel )
                p.Visible = false;

         TabPage tp_this = ( TabPage ) sender;
         ArrayList arrVars = ( ArrayList ) slTPtoArrVars [ tp_this.Text ];
         if ( arrVars.Count != 0 )
            return;

         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, null, false );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref �� ������������ (?)
      }
      #endregion

      #region ���� �� ������� � ����������� � ��������� �� � ��� ������
      public void tabStatusFC_Enter( object sender, EventArgs e )
      {
         TabPage tp_this = ( TabPage ) sender;
         ArrayList arrVars = ( ArrayList ) slTPtoArrVars [ tp_this.Text ];
         if ( arrVars.Count != 0 )
            return;

         nfXMLConfigFC = "frm_tbpFC.xml"; // ���� � ����� � ����������� ������ ����������

         // ��� ����������� ������ �� �� �� ����� ��������� ����� ����������

         string old_strIDDev = strIDDev;
         int old_iIDDev = iIDDev;
         string sfc = StrFC;

         iIDDev = iFC * 256;
         strIDDev = iIDDev.ToString( );

         arrVars.Add( "arrStatusFCCommand" ); // ����� ���-�� ���������������� ������������� ������������ ��� �� �� ��� ���������� ����������
         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, null, false );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref �� ������������ (?)

         strIDDev = old_strIDDev;
         iIDDev = old_iIDDev;
      }
      #endregion

      #region ���� �� ������� ��� ��������� ��������� �������
      public void tbpPacketViewer_Enter( object sender, EventArgs e )
      {
         //// ����� ���������
         //slEncoding = new SortedList( );
         //int ii = 0;
         //foreach ( EncodingInfo ei in Encoding.GetEncodings( ) )
         //{
         //   slEncoding [ ii ] = ei;
         //   cbEncode.Items.Add( "[" + ei.CodePage.ToString( ) + "]" + " : " + ei.DisplayName );
         //   if ( ei.CodePage == 866 )
         //      cbEncode.SelectedIndex = ii;    // ��������� �� ���������
         //   ii++;
         //}
         //eii = ( EncodingInfo ) slEncoding [ cbEncode.SelectedIndex ];  //EncodingInfo

         //slLocal = new SortedList( );
         //// ������ SortedList ��� ������� ����������
         //foreach ( FC aFC in parent.KB )
         //   if ( aFC.NumFC == iFC )
         //   {
         //      foreach ( TCRZADirectDevice aDev in aFC )
         //         if ( aDev.NumDev == iIDDev )
         //         {
         //            slLocal = aDev.CRZAMemDev;
         //            break;
         //         }
         //      break;
         //   }
         //// ��������� ComboBox
         //cbAvailablePackets.Items.Clear( );
         //for ( int i = 0 ;i < slLocal.Count ;i++ )
         //   cbAvailablePackets.Items.Add( slLocal.GetKey( i ) );
         //try
         //{
         //   cbAvailablePackets.SelectedIndex = 0;
         //}
         //catch ( Exception eee )
         //{
         //   MessageBox.Show( "��� ������ ��� �����������. " + eee.Message );
         //}
      }

      private void cbAvailablePackets_SelectedIndexChanged( object sender, EventArgs e )
      {
         //ReNew( );
      }

      private void PacketViewer_Output( byte [ ] brP, ushort numColumn )
      {
         //int lenpack = BitConverter.ToInt16( brP, 0 );

         //short numdev = BitConverter.ToInt16( brP, 2 );

         //ushort add10 = BitConverter.ToUInt16( brP, 4 );	//������ ����� ����� ������

         //// ������ ������ � ������ 
         //byte[] memX = new byte [ brP.Length - 6 ];
         //System.Buffer.BlockCopy( brP, 6, memX, 0, brP.Length - 6 );

         //Encoding e = Encoding.ASCII;
         //try
         //{
         //   e = eii.GetEncoding( );
         //}
         //catch
         //{
         //   MessageBox.Show( "������ ��� ������ ���������" );
         //}

         //char[] arrCh = new char [ e.GetCharCount( memX, 0, memX.Length ) ];
         //e.GetChars( memX, 0, memX.Length, arrCh, 0 );

         //// ��������� ListView

         //ColumnHeader ch = new ColumnHeader( );
         //ch.DisplayIndex = 0;
         //ch.Name = "clm_" + ch.DisplayIndex.ToString( "X2" );
         //ch.Text = "";
         //ch.TextAlign = HorizontalAlignment.Center;
         //ch.Width = 1;       // ������ �������
         //lstvDump.Columns.Add( ch );

         //ch = new ColumnHeader( );
         //ch.DisplayIndex = 1;
         //ch.Name = "clmOffset_10";
         //ch.Text = "���� 10";
         //ch.TextAlign = HorizontalAlignment.Right;
         //ch.Width = 70;
         //lstvDump.Columns.Add( ch );

         //ch = new ColumnHeader( );
         //ch.DisplayIndex = 2;
         //ch.Name = "clmOffset_16";
         //ch.Text = "���� 16";
         //ch.TextAlign = HorizontalAlignment.Right;
         //ch.Width = 70;
         //lstvDump.Columns.Add( ch );

         //int ii;
         //for ( ii = 0 ;ii < numColumn ;ii++ )
         //{
         //   ch = new ColumnHeader( );
         //   ch.DisplayIndex = ii + 3;
         //   ch.Name = "clm_" + ch.DisplayIndex.ToString( "X2" );
         //   ch.Text = ii.ToString( "X2" );
         //   ch.TextAlign = HorizontalAlignment.Center;
         //   ch.Width = 30;
         //   lstvDump.Columns.Add( ch );
         //}

         //ch = new ColumnHeader( );
         //ch.DisplayIndex = ii + 1;
         //ch.Name = "clm_symbols";
         //ch.Text = "����. ������";
         //ch.TextAlign = HorizontalAlignment.Left;
         //ch.Width = 150;
         //lstvDump.Columns.Add( ch );

         //// ��������� ����� � ������ ������� - ��������� � ����������������� �������������� ���������� ��������
         //// ??

         //char chS;
         //StringBuilder strB = new StringBuilder( );

         //for ( int i = 0 ;i < lenpack - 6 ; )
         //{
         //   ListViewItem li = new ListViewItem( );
         //   //li.SubItems.Clear();
         //   li.SubItems.Add( add10.ToString( ) );
         //   li.SubItems.Add( add10.ToString( "X4" ) );
         //   strB.Length = 0;
         //   int j;
         //   for ( j = 0 ;j < iclm ;j++ )
         //   {
         //      li.SubItems.Add( memX [ i ].ToString( "X2" ) );

         //      // ���������� ��������
         //      try
         //      {
         //         chS = Convert.ToChar( arrCh [ i ] );
         //      }
         //      catch
         //      {
         //         MessageBox.Show( "�������� �� ��������������" );
         //         return;
         //      }

         //      if ( Char.IsLetterOrDigit( chS ) )
         //         strB.Append( arrCh [ i ] );
         //      else
         //         strB.Append( "." );
         //      i++;
         //      if ( i >= lenpack - 6 )
         //         break;
         //   }

         //   li.SubItems.Add( strB.ToString( ) );

         //   LinkSetLV( li, false );


         //   add10 += iclm;
         //}

         //// ������ listview
         //lstvDump.Width = 0;
         //for ( int i = 0 ;i < lstvDump.Columns.Count ;i++ )
         //{
         //   ch = lstvDump.Columns [ i ];
         //   lstvDump.Width += ch.Width;
         //}
      }

      private void button1_Click( object sender, EventArgs e )
      {
         ReNew( );
      }

      private void ReNew( )
      {
         //// ��������
         //lstvDump.Clear( );
         //// ����� � ListView ������ ������
         //int kl = Convert.ToInt32( cbAvailablePackets.Text );
         //object kt = slLocal [ kl ];
         ////PacketViewer_Output( ( BinaryReader ) kt, iclm );
         //PacketViewer_Output( ( byte [ ] ) kt, iclm );
      }

      private void rbClm16_CheckedChanged( object sender, EventArgs e )
      {
         //RadioButton rb = ( RadioButton ) sender;
         //if ( rb.Checked )
         //   iclm = Convert.ToUInt16( rb.Tag );  // ����� �������

         //ReNew( );
      }

      private void cbEncode_SelectedIndexChanged( object sender, EventArgs e )
      {
         //eii = ( EncodingInfo ) slEncoding [ cbEncode.SelectedIndex ];  //EncodingInfo
      }
      #endregion

      #region ��������� ������� �������� ��������
      /// <summary>
      /// ������������ �������� �������� ��������
      /// </summary>
      /// <param Name="groupname">��� ������</param>
      /// <param Name="tabpage">tabpage �� ��� ����� ������������� ���� ������</param>
      /// <param Name="arlist"></param>
      /// <param Name="pnlTP"></param>
      public void PrepareTabPagesForGroup( string groupname, TabPage tabpage, ref ArrayList arlist, Panel pnlTP, bool isClickable )
      {
         if ( !arlist.Contains( "arrStatusFCCommand" ) )
         {
            #region ���� ������ �� �������� ����������, �� ��
            if ( !File.Exists( path2PrgDevCFG ) )
               throw new Exception( "���� �� ������ : " + path2DeviceCFG );

            // ������ �������� ������ � ����� �������� ����������
            XDocument xdoc_txt = XDocument.Load( path2DeviceCFG );
            //IEnumerable<XElement> xegftts = xdoc_txt.Descendants( "GroupForTheTag" );
            var xe_config = ( from x in xdoc_txt.Descendants( "GroupInDev" )
                              where x.Element( "Name" ).Value == groupname
                              select x ).DefaultIfEmpty( ).Single( );
            if ( xe_config == null )
            {
               MessageBox.Show( "��� ������ ��� ������������� ������� ��� ������ " + groupname, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
               return;
            }

            // ���� ���� ���������, ������� tabcontrol
            if ( xe_config.Descendants( "SubGroup" ).Count( ) == 0 && ( ( string ) xe_config.Element( "Tags" ) == null ) )
            {
               MessageBox.Show( "��� ������ ��� �����������", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information );
               return;
            }

            // slnflps - ������������� ������ flp � ������� � ��������� �������������
            /*SortedList*/ slnflps = CreateTPforGroup( xe_config, tabpage, tabpage.Text, pnlTP );

            // ������ ����� flp ����� �������� � ������
            GetCCforFLP( ( ControlCollection ) this.Controls );
            #endregion
         }

         arlist = CreateArrayList( tabpage.Text );    //grname

         PlaceVisElemOnForm( tabpage.Text, String.Empty , arlist, isClickable );
      }

      public void PlaceVisElemOnForm( string tabpageName, string nameTagetFLP, ArrayList arlist, bool isClickable )
      {
          //FormulaEval ev;
          FormulaEvalNDS ev;

          if (tabpageName == "�������")
              UstavkiControls = new ArrayList();

          errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

          // ��������� ����������� �� �����
          for (int i = 0; i < arlist.Count; i++)
          {
              if (htFormulaEvals.ContainsKey(arlist[i]))
                  ev = (FormulaEvalNDS)htFormulaEvals[arlist[i]];
              else
              {
                  MessageBox.Show("��� " + arlist[i] + " ����������� � ������� ����� ������ �����.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                  continue;
              }

              // ������� ��������� ������� ��� ���������� ���� � ��� ���
              CheckBoxVar chBV;
              ctlLabelTextbox usTB;
              ComboBoxVar cBV;
              string key = String.Empty;

              // ���������� flp
              if (String.IsNullOrEmpty(tabpageName) && !String.IsNullOrEmpty(nameTagetFLP))
              {
                  key = (string)slnflps.GetKey(slnflps.IndexOfValue(nameTagetFLP));
              }
              else
                  key = slnflps.ContainsValue(ev.ToP) ? (string)slnflps.GetKey(slnflps.IndexOfValue(ev.ToP)) : ev.ToP;

              switch (ev.ToT)
              {
                  case TypeOfTag.Combo:
                      //cBV = new ComboBoxVar((string[])((TagEval)((TagVal)ev.arrTagVal[0]).linkTagEval).arrStrCB.ToArray(typeof(string)), 0);
                      //if (!slFLP.ContainsKey(key))
                      //    // �������� �������� ���� ������ ������ ��������
                      //    key = ev.ToP;
                      //cBV.Parent = (FlowLayoutPanel)slFLP[key];
                      //cBV.AutoSize = true;
                      //cBV.addrLinkVar = ev.addrVar;
                      //cBV.typetag = ev.tRezFormulaEval.TypeTag;
                      //cBV.addrLinkVarBitMask = ev.addrVarBitMask;
                      //#region ���� ������� - ��������� � ������
                      //if (tabpageName == "�������")
                      //{
                      //    UstavkiControls.Add(cBV);
                      //    cBV.TypeView = TypeViewValue.Combobox;
                      //    if (ev.ReadWrite == "r")
                      //        cBV.TypeView = TypeViewValue.Textbox;
                      //    else
                      //        cBV.TypeView = TypeViewValue.Combobox;
                      //}
                      //else
                      //    cBV.TypeView = TypeViewValue.Textbox;
                      //#endregion

                      //ev.OnChangeValForm += cBV.LinkSetText;
                      //ev.FirstValue();
                            cBV = new ComboBoxVar(ev.LinkVariableNewDS.SlEnumsParty , 0, ev);
                             cBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
                             cBV.AutoSize = true;
                             cBV.TypeView = TypeViewValue.Textbox;//.Combobox;

                              Binding bndcb = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                             bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                             cBV.tbText.DataBindings.Add(bndcb);
                             ev.LinkVariableNewDS.BindindTag = bndcb;
                             cBV.lblCaption.Text = ev.CaptionFE;
 
                      break;
                  case TypeOfTag.String:
                  case TypeOfTag.Analog:
                      //usTB = new ctlLabelTextbox();
                      //usTB.lblCaption.Text = "";
                      ////key = slnflps.ContainsValue( ev.ToP ) ? ( string ) slnflps.GetKey( slnflps.IndexOfValue( ev.ToP ) ) : ev.ToP;
                      //if (!slFLP.ContainsKey(key))
                      //    // �������� �������� ���� ������ ������ ��������
                      //    key = ev.ToP;
                      //usTB.Parent = (FlowLayoutPanel)slFLP[key];
                      //usTB.AutoSize = true;
                      //usTB.addrLinkVar = ev.addrVar;
                      //usTB.typetag = ev.tRezFormulaEval.TypeTag;
                      //usTB.mask = ev.bitmask;
                      //usTB.txtLabelText.ReadOnly = true;
                      //usTB.SetErrorProvider(ref errorProvider1);

                      ////// ��������� ��������� ����� -  ���� ������ � �������� - ���������������
                      ////if (String.IsNullOrEmpty(ev.StrFormat) || ev.StrFormat == "0")
                      ////   ev.StrFormat = HMI_Settings.Precision;

                      //ev.OnChangeValForm += usTB.LinkSetText;
                      //ev.FirstValue();
                      //if (tabpageName == "�������")
                      //{
                      //    UstavkiControls.Add(usTB);
                      //    if (ev.ReadWrite == "r")
                      //        usTB.txtLabelText.ReadOnly = true;
                      //    else
                      //        usTB.txtLabelText.ReadOnly = false;
                      //}
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
                      ////key = slnflps.ContainsValue( ev.ToP ) ? ( string ) slnflps.GetKey( slnflps.IndexOfValue( ev.ToP ) ) : ev.ToP;
                      //if (!slFLP.ContainsKey(key))
                      //    // �������� �������� ���� ������ ������ ��������
                      //    key = ev.ToP;
                      //chBV.Parent = (FlowLayoutPanel)slFLP[key];
                      //chBV.AutoSize = true;
                      //chBV.addrLinkVar = ev.addrVar;
                      //chBV.addrLinkVarBitMask = ev.addrVarBitMask;

                      //// ��� ������� �.�. checkbox, � �� ������
                      //if (tabpageName == "�������")
                      //    chBV.btnCheck.Visible = false;

                      //ev.OnChangeValForm += chBV.LinkSetText;
                      //ev.FirstValue();
                      //if (tabpageName == "�������")
                      //    UstavkiControls.Add(chBV);
							 chBV = new CheckBoxVar(ev);
                             chBV.IsClickable = isClickable;
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
      }
      #endregion

      #region �������� tabpage ��� ������ �����
      private void CreateTPforGroup( XElement xegftt, TabControl tc )
      {
         timer1.Enabled = false;

         //return;  // ���� �� ����������

         TabPage tpg = new TabPage( );
         string strn = String.Empty;
         if ( String.IsNullOrEmpty( ( string ) xegftt.Element( "Name" ) ) )
            if ( !String.IsNullOrEmpty( ( string ) xegftt.Attribute( "Name" ) ) )
               strn = xegftt.Attribute( "Name" ).Value;
            else
               throw new Exception( "��� �������� ���������" );
         else
            strn = xegftt.Element( "Name" ).Value;

         tpg.Text = strn;
         tc.TabPages.Add( tpg );

         // ���� �� ���� ��� �����
         if ( !String.IsNullOrEmpty( ( string ) xegftt.Element( "Tags" ) ) )
         {
            IEnumerable <XElement> xetwog = xegftt.Element( "Tags" ).Elements( "Tag" );
            if ( xetwog.Count( ) != 0 )
            {
               // �������� ���� �� ���������, ���� ��� �� tabpage ��� ����� ��� ����� �� �������
               IEnumerable <XElement> xetg = xegftt.Elements( "SubGroup" );
               if ( xetg.Count( ) == 0 )
                  CreateDataGridView( xegftt, tpg );// ���� �������� ���, �� ��������� DataGridView ��� ����� ���������
               else
               {
                  TabControl tctp = new TabControl( );
                  tctp.Dock = DockStyle.Fill;
                  tpg.Controls.Add( tctp );
                  TabPage ntp = new TabPage( );
                  ntp.Text = "����� ������ �� ������";   // ����� ���� ���� ��� ��������
                  tctp.Controls.Add( ntp );
                  CreateDataGridView( xegftt.Element( "Tags" ), ntp );// ���� ��������� ����, �� ��������� DataGridView ��� ����� ���������
                  foreach ( XElement xelem in xetg )
                     CreateTPforGroup( xelem, tctp );
               }
            }
         }
         else
         {
            IEnumerable <XElement> xesgs = xegftt.Elements( "SubGroup" );
            if ( xesgs.Count( ) > 0 )
            {
               TabControl tctp = new TabControl( );
               tctp = new TabControl( );
               tctp.Dock = DockStyle.Fill;
               tpg.Controls.Add( tctp );

               foreach ( XElement xetpsg in xesgs )
               {
                  //ASU_level_Describe
                  CreateTPforGroup( xetpsg, tctp );
               }
            }
            //else                     }

            //   CreateDataGridView( xegftt, tpg );// ���� �������� ���, �� ��������� DataGridView ��� ����� ���������
         }
         timer1.Enabled = true;
      }

      /// <summary>
      /// �������� ������� ��� �������� �� tabpage ��� ������
      /// </summary>
      /// <param Name="xe"></param>
      /// <param Name="tabpage">tabpage ��� ���������� �������, ���� �� ������ panel</param>
      /// <param Name="pnlParent">������ ��� ���������� �������</param>
      /// <returns></returns>
      public SortedList CreateTPforGroup( XElement xe, TabPage tabpage, string prefixForFLP, Panel pnlParent )
      {
         // �������� �������� ������� �� Device.cfg
         if ( slnflps == null )
            try
            {
               slnflps = new SortedList( );

               XDocument xdoc_dcfg = XDocument.Load( path2DeviceCFG );
               var tops = from tt in xdoc_dcfg.Element( "Device" ).Element( "TypeOfPanelSections" ).Elements( "TypeOfPanel" )
                          select tt;

               foreach ( XElement top in tops )
                  slnflps [ top.Element( "Name" ).Value ] = top.Element( "Caption" ).Value;
            }
            catch ( Exception e )
            {
               throw new Exception( e.Message );
            }

         /* ���������� TabPage � ������
          * ��������� �����\�������� 
          */
         if ( xe.Elements( "SubGroup" ).Count( ) != 0 )
         {
            TabControl tc = new TabControl( );
            tc.Dock = DockStyle.Fill;

            if ( pnlParent == null )
               tabpage.Controls.Add( tc );
            else
               pnlParent.Controls.Add( tc );

            string sss = ( string ) xe.Element( "Tags" );
            //if ( !String.IsNullOrEmpty( ( string ) xe.Element( "Tags" ) ) )
            if ( !String.IsNullOrEmpty( sss ) )
            {
               MTRANamedFLPanel flp = new MTRANamedFLPanel( );
               flp.Caption = prefixForFLP; //tptwog.Text;xe.Element( "Name" ).Value // + "#" + "TagsWOGroups"
               if ( slnflps.ContainsValue( flp.Caption ) )
               {
                  TabPage tptwog = new TabPage("����� ������ �� ������");   // ����� ���� ���� ��� ��������
                  tptwog.Controls.Add( flp );
                  flp.Parent = tptwog;
                  flp.Dock = DockStyle.Fill;
                  flp.Name = ( string ) slnflps.GetKey( slnflps.IndexOfValue( flp.Caption ) );  //               tptwog.Name + "_" + "TagsWOGroups";
                  tc.TabPages.Add( tptwog );
               }
               else
                  //MessageBox.Show( "FLP � ������ = " + flp.Caption + " = ���������� � device.cfg.\n �������� �� �������.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
                  Console.WriteLine("(758) FLP � ������ = " + flp.Caption + " = ���������� � device.cfg.\n �������� �� �������.");
            }

            foreach ( XElement xee in xe.Elements( "SubGroup" ) )
            {

               TabPage tptwog = new TabPage( xee.Element( "Name" ).Value );
               if ( xee.Elements( "SubGroup" ).Count( ) != 0 )
               {
                  SortedList sltmp = CreateTPforGroup( xee, tptwog, prefixForFLP + "#" + tptwog.Text, null );
               }
               else
               {
                  MTRANamedFLPanel flp = new MTRANamedFLPanel( );
                  // � �������� ����������� flp �������� �������� ������ � ���������
                  flp.Caption = prefixForFLP + "#" + xee.Element( "Name" ).Value;//xe.Element("Name").Value
                  if ( slnflps.ContainsValue( flp.Caption ) )
                  {
                     tptwog.Controls.Add( flp );
                     flp.Parent = tptwog;
                     flp.Dock = DockStyle.Fill;
                     flp.Name = ( string ) slnflps.GetKey( slnflps.IndexOfValue( flp.Caption ) );
                  }
                  else
                  {
                     //MessageBox.Show( "FLP � ������ = " + flp.Caption + " = ���������� � device.cfg.\n �������� �� �������.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
                     Console.WriteLine("(784) FLP � ������ = " + flp.Caption + " = ���������� � device.cfg.\n �������� �� �������.");
                     continue;
                  }
               }
               tc.TabPages.Add( tptwog );
            }
         }
         else if ( !String.IsNullOrEmpty( ( string ) xe.Element( "Tags" ) ) )
         {
            // ���� �������� ���, � ���� ������ ���� ��� �����, �� ��������� flp ����� �� tabpage
            MTRANamedFLPanel flp = new MTRANamedFLPanel( );
            if ( pnlParent == null )
               tabpage.Controls.Add( flp );
            else
               pnlParent.Controls.Add( flp );

            flp.Parent = tabpage;
            flp.Dock = DockStyle.Fill;
            flp.Caption = prefixForFLP;   // + "#" + "TagsWOGroups"
            // �������� ������������� �������
            if ( slnflps.ContainsValue( flp.Caption ) )
               flp.Name = ( string ) slnflps.GetKey( slnflps.IndexOfValue( flp.Caption ) );
            else
            {
               //MessageBox.Show( "FLP � ������ = " + flp.Caption + " = ���������� � device.cfg.\n �������� �� �������.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
               Console.WriteLine("(809) FLP � ������ = " + flp.Caption + " = ���������� � device.cfg.\n �������� �� �������.");

            }

            //flp.Name = prefixForFLP + "_" + "TagsWOGroups"; // xe.Element( "Name" ).Value           "TagsWOGroups";//tabpage.Name + "_" + 
            //flp.Caption = tabpage.Text;
         }
         return slnflps;
      }

      private void CreateDataGridView( XElement xegftt, TabPage tpg )
      {
         DataTable dt = new DataTable( );
         DataGridView dgv = new DataGridView( );
         dgv.Dock = DockStyle.Fill;
         dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

         dt.Columns.Add( "AddessModbus", typeof( System.String ) );
         dt.Columns.Add( "TypeField", typeof( System.String ) );
         dt.Columns.Add( "TagGUID", typeof( System.String ) );
         dt.Columns.Add( "BitMask", typeof( System.String ) );
         dt.Columns.Add( "Description", typeof( System.String ) );
         dt.Columns.Add( "Dimention", typeof( System.String ) );
         dt.Columns.Add( "Value", typeof( System.String ) );

         IEnumerable<XElement> xesgtgs = xegftt.Descendants( "Tag" );
         foreach ( XElement xesgtg in xesgtgs )
         {
            if ( String.IsNullOrEmpty( ( string ) xesgtg.Element( "ASU_level_Describe" ) ) )
               continue;

            DataRow dr = dt.NewRow( );

            foreach ( XElement xec in xesgtg.Element( "ASU_level_Describe" ).Elements( ) )
               if ( dt.Columns.Contains( xec.Name.ToString( ) ) )
                  dr [ xec.Name.ToString( ) ] = xec.Value;

            dt.Rows.Add( dr );
         }
         dgv.DataSource = dt;
         dsTagTables.Tables.Add( dt );
         tpg.Controls.Add( dgv );
      }

      /// <summary>
      /// ���������� ����� FLP � ������ (��� ����������� ������������ ��������� �� ������)
      /// </summary>
      /// <param Name="cc"></param>
      public void GetCCforFLP(Control.ControlCollection cc )
      {
         for ( int i = 0 ;i < cc.Count ;i++ )
         {
            if ( cc [ i ] is FlowLayoutPanel )
            {
               FlowLayoutPanel flp = ( FlowLayoutPanel ) cc [ i ];
               if ( slFLP.ContainsKey( flp.Name ) )
                  continue;

               slFLP [ flp.Name ] = flp;
            }
            else if ( cc [ i ] is MTRANamedFLPanel )
            {
               MTRANamedFLPanel flp = ( MTRANamedFLPanel ) cc [ i ];
               if ( slFLP.ContainsKey( flp.Name ) )
                  continue;

               slFLP [ flp.Name ] = flp;

               if ( slnflps.ContainsKey( flp.Name ) )
                  continue;
               slnflps.Add( flp.Name, flp.Caption );
            }
            else
               TestCCforFLP( cc [ i ] );
         }
      }

      private void TestCCforFLP( Control cc )
      {
         for ( int i = 0 ;i < cc.Controls.Count ;i++ )
         {
            if ( cc.Controls [ i ] is MTRANamedFLPanel )
            {
               MTRANamedFLPanel flp = ( MTRANamedFLPanel ) cc.Controls [ i ];
               if ( slFLP.ContainsKey( flp.Name ) )
                  continue;

               slFLP.Add( flp.Name, flp );
               
               if (slnflps == null)
                  continue;

               if ( slnflps.ContainsKey( flp.Name ) )
                  continue;
               slnflps.Add( flp.Name, flp.Caption );
            }

            else if ( cc.Controls [ i ] is FlowLayoutPanel )
            {
               FlowLayoutPanel flp = ( FlowLayoutPanel ) cc.Controls [ i ];
               if ( slFLP.ContainsKey( flp.Name ) )
                  continue;

               slFLP [ flp.Name ] = flp;
            }
            else
            {
               TestCCforFLP( cc.Controls [ i ] );
            }
         }
      }

      /// <summary>
      /// �������� ������� ArrayList � ��������� ���������� �� ����������� ����� XML
      /// </summary>
      /// <param Name="arrVar"> ������  ArrayList
      ///������</param>
      /// <param Name="nameFile">��� ����� XML
      ///������</param>
      public ArrayList CreateArrayList( /*ref ArrayList arrVar,*/ string name_arrVar )
      {
         // ������ XML
         if ( nfXMLConfig == null )
            return ( ArrayList ) slTPtoArrVars [ name_arrVar ];

         XDocument xd;

         if ( ( ( ArrayList ) slTPtoArrVars [ name_arrVar ] ).Contains( "arrStatusFCCommand" ) )         
         {
            ( ( ArrayList ) slTPtoArrVars [ name_arrVar ] ).Clear();
            xd = XDocument.Load( nfXMLConfigFC );
         }
         else
            xd = XDocument.Load( fileFrmTagsDescript );

         XElement xegr = null;
         try
         {
            xegr = ( from p in xd.Element( "MT" ).Element( "BMRZ" ).Element( "frame" ).Elements( )
                              where p.Name == name_arrVar
                              select p ).Single( );
         }
         catch ( XmlException ee )
         {
            Console.WriteLine( ee.Message );
         }
         return GetArrFrmls(xegr, name_arrVar);
      }

      /// <summary>
      /// �������� ������� ArrayList � ��������� ���������� �� ����������� ����� XML
      /// </summary>
      /// <param Name="xegr"></param>
      /// <param Name="name_arrVar">�������� ������ � ������ � ����� ����� frmXXX.xml</param>
      /// <returns></returns>
      public ArrayList GetArrFrmls(XElement xegr, string name_arrVar)
      {
         ArrayList arrVars = new ArrayList();
         SortedList<string, string> sl = new SortedList<string, string>();
         ArrayList alVal = new ArrayList();
         StringBuilder strformula = new StringBuilder();
         //FormulaEval fe;
         FormulaEvalNDS fe;

         IEnumerable<XElement> xefs = xegr.Elements("formula");
         foreach (XElement xef in xefs)
         {
             // ��������� �������� �������
             sl["formula"] = xef.Attribute("express").Value;
             sl["caption"] = xef.Attribute("Caption").Value;
             sl["dim"] = xef.Attribute("Dim").Value;
             sl["TypeOfTag"] = xef.Attribute("TypeOfTag").Value;
             sl["TypeOfPanel"] = xef.Attribute("TypeOfPanel").Value;
             if (String.IsNullOrEmpty((string)xef.Attribute("ReadWrite")))
                 sl["ReadWrite"] = "wr"; // �� ��������� ��� �������� �� ��/���
             else
                 sl["ReadWrite"] = xef.Attribute("ReadWrite").Value;

             // ��������� ����������� �� �������� ����� - ��������� �����
             if (String.IsNullOrEmpty((string)xef.Attribute("PosComma")))
                 sl["Precision"] = "-1"; // ��������� ����� - �� �����
             else
                 sl["Precision"] = xef.Attribute("PosComma").Value;

             TypeOfTag ToT = TypeOfTag.NaN;
             string ToP = "";

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

             strformula.Length = 0;

             if (ToP == "no")
                 continue;

             if (alVal.Count == 0)
                 break;

             for (int i = 0; i < alVal.Count; i++)
             {
                 strformula.Append(i.ToString() + "(" + strFC + "." + strIDDev + (string)alVal[i] + ")");
             }

             if (!htFormulaEvals.ContainsKey(strformula.ToString() + "#" + ToP))
             {
                 //fe = new FormulaEval(parent.KB, strformula.ToString(), sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP);
                 fe = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, strformula.ToString(), sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP);
                 //fe.StrFormat = sl["Precision"];
                 //fe.ReadWrite = sl["ReadWrite"];
                 htFormulaEvals.Add(strformula.ToString() + "#" + ToP, fe);
             }
             arrVars.Add(strformula.ToString() + "#" + ToP);
         }

         //if (HMI_Settings.ClientDFE != null)
         //{
         //    switch (name_arrVar)
         //    {
         //        case "arrAvarSign":
         //            break;
         //        case "arrConfigSign":
         //            break;
         //        case "�������":
         //            break;
         //        case "arrStatusDevCommand":
         //            break;
         //        case "arrStatusFCCommand":
         //            break;
         //        case "������������":
         //            break;
         //        case "arrStoreSign":
         //            //foreach (FormulaEval fe in arrVar)
         //            //   if (fe.addrVar < 60000)
         //            //      parent.ClientDFE.AddArrTags(this.Text, fe);
         //            break;
         //        default:
         //            //foreach (FormulaEval efe in htFormulaEvals.Values)
         //            //    HMI_Settings.ClientDFE.AddArrTags(this.Text, efe);
         //            break;
         //    }
         //}

         xefs = xegr.Elements("simple_eval");
         foreach (XElement xef in xefs)
         {
             throw new Exception("������� �� simple_eval �� ����������");
         }

         //// ������������� �� ���������� ����� � DataServer
         //taglist = new List<ITag>();

         //foreach (FormulaEvalNDS fee in htFormulaEvals.Values)
         //{
         //    if (fee.LinkVariableNewDS == null)
         //        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 428, string.Format("() : frmBMRZ.cs : CreateArrayList() : ��� �������� � ���� = {0}", fee.CaptionFE));
         //    else
         //        taglist.Add(fee.LinkVariableNewDS);
         //}
         //HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags(taglist);

         return arrVars;
      }
      #endregion

      #region ����������� ������ DataSet
      static void PrintDataSet( DataSet ds )
      {
         // ����� ��������� ���� �� ���� DataTable ������� DataSet
         Console.WriteLine( "������� � DataSet '{0}'. \n ", ds.DataSetName );
         foreach ( DataTable dt in ds.Tables )
         {
            Console.WriteLine( "������� '{0}'. \n ", dt.TableName );
            // ����� ���� ��������
            for ( int curCol = 0 ;curCol < dt.Columns.Count ;curCol++ )
               Console.Write( dt.Columns [ curCol ].ColumnName.Trim( ) + "\t" );
            Console.WriteLine( "\n-----------------------------------------------" );

            // ����� DataTable
            for ( int curRow = 0 ;curRow < dt.Rows.Count ;curRow++ )
            {
               for ( int curCol = 0 ;curCol < dt.Columns.Count ;curCol++ )
                  Console.Write( dt.Rows [ curRow ] [ curCol ].ToString( ) + "\t" );
               Console.WriteLine( );
            }
         }
      }
      #endregion

      #region ���������� ���� ���������� ������ �������� �������� ���������� (�� ��)
      /// <summary>
      /// SetArhivGroupInDev(dev, 8)
      /// ���������� ���� ���������� ������ �������� �������� ���������� (�� ��)
      /// </summary>
      /// <param Name="dev">����������</param>
      /// <param Name="idGroup">������</param>
      public void SetArhivGroupInDev(int nfc, int dev, int idGroup )
      {
         //foreach (DataSource aFc in parent.KB)//.newKB.KB
         //   foreach ( TCRZADirectDevice tdd in aFc )
         //      if ( tdd.NumDev == dev && tdd.NumFC == nfc )
         //         foreach ( TCRZAGroup tdg in tdd )
         //            if ( tdg.Id == idGroup )
         //               foreach ( TCRZAVariable tgv in tdg )
         //                  //tgv.SetQuality( VarQuality.vqArhiv );
         //                  tgv.Quality = VarQuality.vqArhiv;
      }
      #endregion

      //#region ����� ������ pack � hex-���� � ���� fn
      //public void PrintHexDump( string fn, byte [ ] pack )
      //{
      //   // ������� � ���� - ������� �������
      //   FileStream fs = new FileStream( fn, FileMode.Append );
      //   StreamWriter sw = new StreamWriter( fs );

      //   sw.Write( Environment.NewLine );
      //   sw.Write( "*****************************" );
      //   sw.Write( Environment.NewLine );
      //   sw.Write( DateTime.Now.ToString( ) );
      //   sw.Write( Environment.NewLine );
      //   sw.Write( "*****************************" );
      //   sw.Write( Environment.NewLine );
      //   int ii = 0;
      //   for ( ushort i = 0 ;i < pack.Length ;i++ )
      //   {
      //      // �������� ������ ��������� ������
      //      sw.Write( ii.ToString( "x" ) );
      //      sw.Write( "    " );
      //      for ( int jj = 0 ;jj < 2 ;jj++ )
      //      {
      //         for ( int j = 0 ;j < 4 ;j++ )
      //         {
      //            try
      //            {
      //               sw.Write( pack [ ii ].ToString( "x" ) + " " );
      //               ii++;
      //            }
      //            catch
      //            {
      //               sw.Close( );
      //               fs.Close( );
      //               return;
      //            }
      //         }
      //         sw.Write( "    " );
      //      }
      //      sw.Write( Environment.NewLine );
      //      sw.Write( Environment.NewLine );
      //   }
      //   sw.Close( );
      //   fs.Close( );
      //}
      //#endregion

      #region actDellstV - �������� � ListView
      delegate void SetLVCallback( ListViewItem li, bool actDellstV );

      // actDellstV - �������� � ListView : false - �� �������; true - ��������;
      public void LinkSetLV( object Value, bool actDellstV )
      {
         //if ( !( Value is ListViewItem ) && !actDellstV )
         //   return;   // ������������� ������ ����� ����������

         //ListViewItem li = null;
         //if ( !actDellstV )
         //   li = ( ListViewItem ) Value;
         //if ( this.lstvDump.InvokeRequired )
         //{
         //   if ( !actDellstV )
         //      SetLV( li, actDellstV );
         //   else
         //      SetLV( null, actDellstV );
         //}
         //else
         //{
         //   if ( !actDellstV )
         //      this.lstvDump.Items.Add( li );
         //   else
         //      this.lstvDump.Items.Clear( );
         //}
      }

      private void SetLV( ListViewItem li, bool actDellstV )
      {
         //if ( this.lstvDump.InvokeRequired )
         //{
         //   SetLVCallback d = new SetLVCallback( SetLV );
         //   this.Invoke( d, new object [ ] { li, actDellstV } );
         //}
         //else
         //{
         //   if ( !actDellstV )
         //      this.lstvDump.Items.Add( li );
         //   else
         //      this.lstvDump.Items.Clear( );
         //}
      }
      #endregion

      #region ������ ������� �����
      private void btnPrint_Click( object sender, EventArgs e )
      {
         //PrintHMI frmPrt = new PrintHMI( );
         //StringBuilder sb = new StringBuilder( );
         //;
         //ListViewItem li;

         //// ���������� ���������� ���� ����� lstvDump
         //for ( int i = 0 ;i < lstvDump.Items.Count ;i++ )
         //{
         //   sb.Length = 0;
         //   li = new ListViewItem( );
         //   li = ( ListViewItem ) lstvDump.Items [ i ];
         //   for ( int j = 0 ;j < li.SubItems.Count ;j++ )
         //   {
         //      sb.Append( li.SubItems [ j ].Text );
         //      sb.Append( "\t" );
         //   }
         //   sb.Append( "\n" );
         //   frmPrt.rtbText.AppendText( sb.ToString( ) );
         //}
         //frmPrt.Show( );
      }

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
         //PrintArr( );
         //parent.mnuPrintPreview_Click( sender, e );
      }

      /// <summary>
      /// mnuPrint_Click( object sender, EventArgs e )
      ///     ���������� ������ ���� ������
      /// </summary>
      private void mnuPrint_Click( object sender, EventArgs e )
      {
         //PrintArr( );
         //parent.mnuPrint_Click( sender, e );
      }

      /// <summary>
      /// PrintArr()
      ///     ������ ������� ����������
      /// </summary>
      private void PrintArr( )
      {
         //StringBuilder sb = new StringBuilder( );
         //float f_val;
         //int i_val;
         //string t_val = "";
         //ArrayList arCurPrt = new ArrayList( );

         //object val;

         //// ���������� �������� �������
         //TabPage tp_sel = tc_Main.SelectedTab;

         //sb.Length = 0;

         //// ��������� ��������� ��������
         //sb.Append( "========================================================================\n" );
         //sb.Append( this.Text + "= " + tp_sel.Text + " =" );
         //sb.Append( "\n========================================================================\n" );
         //sb.Append( " \n \n " );
         //arCurPrt = ( ArrayList ) slTPtoArrVars [ tp_sel.Text ];

         //for ( int i = 0 ;i < arCurPrt.Count ;i++ )
         //{

         //   //FormulaEval ev = ( FormulaEval ) arCurPrt [ i ];
         //   FormulaEval ev = htFormulaEvals[arCurPrt[i]] as Calculator.FormulaEval ;
         //   //int cni = htFormulaEvals.Count;
         //   switch ( ev.ToT )
         //   {
         //      case TypeOfTag.Analog:
         //         val = ev.tRezFormulaEval.Value;

         //         if ( val is float )
         //         {
         //            f_val = ( float ) ev.tRezFormulaEval.Value;
         //            t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
         //         }
         //         else if ( val is short )
         //         {
         //            i_val = ( Int16 ) ev.tRezFormulaEval.Value;
         //            t_val = i_val.ToString( );
         //         }
         //         else if ( val is int )
         //         {
         //            i_val = ( Int32 ) ev.tRezFormulaEval.Value;
         //            t_val = i_val.ToString( );
         //         }
         //         else if ( val is string )
         //         {
         //            t_val = ( string ) ev.tRezFormulaEval.Value;
         //         }

         //         sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
         //         break;
         //      case TypeOfTag.Discret:
         //         sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString( ) + " \t " + ev.tRezFormulaEval.DimIE );
         //         break;
         //      case TypeOfTag.Combo:
         //         sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString( ) + " \t " + ev.tRezFormulaEval.DimIE );
         //         break;
         //      default:
         //         continue;
         //   }
         //   sb.Append( " \n " );
         //}
         //parent.prt.rtbText.AppendText( sb.ToString( ) );
      }

      /// <summary>
      /// sbForSimpleVar(StringBuilder sb)
      ///     ������������ ������ ��� ������ ��� ��������� ����������
      /// </summary>
      //private void sbForSimpleVar( StringBuilder sb, FormulaEval b_xxx )
      //{
      //   //float f_val;
      //   //int i_val;
      //   //string t_val = "";
      //   //FormulaEval ev = ( FormulaEval ) b_xxx;
      //   //object val;

      //   //if ( b_xxx == null )
      //   //   return;

      //   //switch ( ev.ToT )
      //   //{
      //   //   case TypeOfTag.NoN:
      //   //      val = ev.tRezFormulaEval.Value;

      //   //      if ( val is float )
      //   //      {
      //   //         f_val = ( float ) ev.tRezFormulaEval.Value;
      //   //         t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
      //   //      }
      //   //      else if ( val is short )
      //   //      {
      //   //         i_val = ( Int16 ) ev.tRezFormulaEval.Value;
      //   //         t_val = i_val.ToString( );
      //   //      }
      //   //      else if ( val is ushort )
      //   //      {
      //   //         i_val = ( UInt16 ) ev.tRezFormulaEval.Value;
      //   //         t_val = i_val.ToString( );
      //   //      }
      //   //      else if ( val is int )
      //   //      {
      //   //         i_val = ( Int32 ) ev.tRezFormulaEval.Value;
      //   //         t_val = i_val.ToString( );
      //   //      }
      //   //      else if ( val is string )
      //   //      {
      //   //         t_val = ( string ) ev.tRezFormulaEval.Value;
      //   //      }

      //   //      sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
      //   //      break;
      //   //   case TypeOfTag.Analog:
      //   //      val = ev.tRezFormulaEval.Value;

      //   //      if ( val is float )
      //   //      {
      //   //         f_val = ( float ) ev.tRezFormulaEval.Value;
      //   //         t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
      //   //      }
      //   //      else if ( val is short )
      //   //      {
      //   //         i_val = ( Int16 ) ev.tRezFormulaEval.Value;
      //   //         t_val = i_val.ToString( );
      //   //      }
      //   //      else if ( val is int )
      //   //      {
      //   //         i_val = ( Int32 ) ev.tRezFormulaEval.Value;
      //   //         t_val = i_val.ToString( );
      //   //      }
      //   //      else if ( val is string )
      //   //      {
      //   //         t_val = ( string ) ev.tRezFormulaEval.Value;
      //   //      }

      //   //      sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
      //   //      break;
      //   //   case TypeOfTag.Discret:
      //   //      sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString( ) + " \t " + ev.tRezFormulaEval.DimIE );
      //   //      break;
      //   //   case TypeOfTag.Combo:
      //   //      sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString( ) + " \t " + ev.tRezFormulaEval.DimIE );
      //   //      break;
      //   //   default:
      //   //      break;
      //   //}
      //   //sb.Append( " \n " );
      //}

      #endregion

      #region ���������� �������� ������� ����� ����������
      private void timer1_Tick( object sender, EventArgs e )
      {
         return;

         //foreach ( DataTable dt in dsTagTables.Tables )
         //{
         //   for ( int i = 0 ;i < dt.Rows.Count ;i++ )
         //   {
         //      DataRow dr = dt.Rows [ i ];
               
         //      dr [ "Value" ] = ( ( TCRZAVariable ) tcdirdev.DeviceTags [ Convert.ToInt32( dr [ "TagGUID" ] ) ] ).ExtractTagValueAsString( );
         //   }
         //}
      }
      #endregion	}

      private void frmBMRZbase_FormClosing(object sender, FormClosingEventArgs e)
      {
          //if (HMI_Settings.ClientDFE != null)
          //   HMI_Settings.ClientDFE.RemoveRefToPageTags(this.Text);
      }

      #region ������ ������� � ��������� ����������� ��� �������� �� ����
      public void ParseBDPacket( byte [ ] pack, int adr, int numgr )
      {
         //CommonUtils.CommonUtils.PrintHexDump("frmBMRZbase.cs : ParseBDPacket", "LogHexPacket.dat", pack, GetAdrBlockData(path2DeviceCFG, numgr), iFC, iIDDev);  // ������� � ���� ��� ��������

         //if( adr == -1 )
         //{
         //   MessageBox.Show( "����� ��� �������� ������ �� ���������", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
         //   return;
         //}

         //parent.newKB.PacketToQueDev( pack, Convert.ToUInt16(adr), iFC, iIDDev ); // 10280 �����  �� ������  ����������
         //// �������� �������������� ������ ���������� ��������
         //SetArhivGroupInDev(iFC, iIDDev, numgr );
      }
      #endregion

      /// <summary>
      /// �������� ����� ����� � ������� ��� ��������� ��� ������� ����������
      /// </summary>
      /// <param Name="path2device_cfg"></param>
      /// <param Name="numgr"></param>
      /// <returns></returns>
      public int GetAdrBlockData( string path2device_cfg, int numgr )
      {
         XDocument xdocdfgd = XDocument.Load(path2device_cfg);

         IEnumerable<XElement> xels = xdocdfgd.Element( "Device" ).Element( "Groups" ).Elements( "Group" );

         foreach ( XElement xel in xels ) 
         {
            if( xel.Attribute( "number" ).Value == numgr.ToString() )
               return Convert.ToInt32( xel.Attribute( "adress" ).Value );
         }
         return -1;  // ������ �� �������
      }

       /// <summary>
       /// ��������� ������� � ����������� � ��������� ����������
       /// </summary>
       /// <param name="PanelInfoTextBox"></param>
       /// <param name="rtbInfo"></param>
      protected void FillTAbPageInfo( TextBox PanelInfoTextBox, RichTextBox rtbInfo )
      {
          //SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
          //try
          //{
          //    asqlconnect.Open( );
          //} catch( SqlException ex )
          //{
          //    string errorMes = "";

          //    // ���������� ���� ������������ ������
          //    foreach( SqlError connectError in ex.Errors )
          //        errorMes += connectError.Message + " (������: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
          //    parent.WriteEventToLog( 21, "��� ����� � �� (UstavBD): " + errorMes, this.Name, false );
          //    System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMRZ : ��� ����� � �� (UstavBD)" );
          //    asqlconnect.Close( );
          //    return;
          //} catch( Exception ex )
          //{
          //    MessageBox.Show( "��� ����� � ��������" + Environment.NewLine + ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Information );
          //    asqlconnect.Close( );
          //    return;
          //}
          //// ������������ ������ ��� ������ �������� ���������
          //SqlCommand cmd = new SqlCommand( "GetBlockInfo", asqlconnect );
          //cmd.CommandType = CommandType.StoredProcedure;

          //// ������� ���������
          //// id ����������
          //SqlParameter pidBlock = new SqlParameter( );
          //pidBlock.ParameterName = "@BlockId";
          //pidBlock.SqlDbType = SqlDbType.Int;
          //pidBlock.Value = IFC * 256 + IIDDev;//IIDDev;
          //pidBlock.Direction = ParameterDirection.Input;
          //cmd.Parameters.Add( pidBlock );

          //// ���������� DataSet
          //DataSet aDS = new DataSet( "ptk" );
          //SqlDataAdapter aSDA = new SqlDataAdapter( );
          //aSDA.SelectCommand = cmd;

          ////aSDA.sq
          //aSDA.Fill( aDS, "TbInfo" );

          //asqlconnect.Close( );

          ////PrintDataSet( aDS );
          //// ��������� ������
          //DataTable dtI = aDS.Tables [ "TbInfo" ];

          //// ��������� RichTextBox
          //for( int curRow = 0 ; curRow < dtI.Rows.Count ; curRow++ )
          //{
          //    DateTime t = ( DateTime ) dtI.Rows [ curRow ] [ "DateTime_Modify" ];
          //    PanelInfoTextBox.Text = PanelInfoTextBox.Text + CommonCRZADeviceFunction.GetTimeInMTRACustomFormat( t );
          //    byte[] arrInfo = ( byte [ ] ) dtI.Rows [ curRow ] [ "Data" ];

          //    System.Text.UTF8Encoding utf = new UTF8Encoding( );
          //    string str = utf.GetString( arrInfo );

          //    rtbInfo.AppendText( utf.GetString( arrInfo ) );
          //}
          //aSDA.Dispose( );
          //aDS.Dispose( );
      }
      /// <summary>
      /// ������������� �����
      /// </summary>
      public uint Guid { get { return (uint)( iFC * 256 + iIDDev ); } }
   }
}