/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	��������: ����� ��� ������ � ������� ���.                                                           
 *                                                                             
 *	����                     : frmUSO.cs                                         
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
using NetCrzaDevices;
using System.IO;
using System.Globalization;
using NSNetNetManager;
using LabelTextbox;
using CRZADevices;
using CommonUtils;
using System.Linq;
using System.Xml.Linq;

namespace HMI_MT
{
	public partial class frmUSO : Form
	{
      private MainForm parent;
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

      DataTable dtU;  // ������� � ���������

      SortedList slFLP = new SortedList();	// ��� �������� ��� � FlowLayoutPanel
			
      FormulaEval b_62002;
      FormulaEval b_62092;
      ErrorProvider erp = new ErrorProvider( );
      #region �����������
		public frmUSO( )
		{
			InitializeComponent();
		}
        public frmUSO(MainForm linkMainForm, int iFC, int iIDDev, int inumLoc, string fXML)
		{
				InitializeComponent();
            parent = linkMainForm;
            this.iFC = iFC;                 // ����� �� �������������
            strFC = iFC.ToString();         // ����� �� ������
            this.iIDDev = iIDDev;           // ����� ���������� �������������
            strIDDev = iIDDev.ToString();   // ����� ���������� ������
            this.inumLoc = inumLoc;         // ����� ������ �������������
            strnumLoc = inumLoc.ToString();    // ����� ������ ������
            nfXMLConfig = fXML;
            slFormElements = new SortedList( );
		}
      /// <summary>
      /// �������� �����
      /// </summary>
      private void frmUSO_Load( object sender, EventArgs e )
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
         // ��������� ����������� ������ ��������� ����� �� ��������� Device.cfg (��. ��� � - 00)
         SetElementsFormFeatures( Path.GetDirectoryName( nfXMLConfig ) );
      }

      /// <summary>
      /// ����������� ����� FlowLayoutPanel
      /// </summary>
      /// <param Name="cc"></param>  
      private void TestCCforFLP( Control cc )
      {
         for ( int i = 0 ; i < cc.Controls.Count ; i++ )
         {
            if ( cc.Controls[ i ] is FlowLayoutPanel )
            {
               FlowLayoutPanel flp = ( FlowLayoutPanel ) cc.Controls[ i ];
               slFLP[ flp.Name ] = flp;
            }
            else
            {
               TestCCforFLP( cc.Controls[ i ] );
            }
         }
      }

      /// <summary>
      /// ���������� ����������� ��������� ����� �� ��������� Device.cfg (��. ��� � - 00)
      /// </summary>
      /// <param Name="pathtoDevCFG">���� � ����� � ��������� ���������� - ���� Device.cfg</param>
      private void SetElementsFormFeatures( string pathtoDevCFG )
      {         
         xdoc = XDocument.Load( pathtoDevCFG + Path.DirectorySeparatorChar + "Device.cfg");
         // ���� �������� ����� �� Name
         if ( String.IsNullOrEmpty( (string)xdoc.Element( "Device" ).Element( "DeviceFeatures" ).Element( "GDIFeatures" )) )
            return;

         IEnumerable<XElement> collNameEl = from tp in xdoc.Element( "Device" ).Element( "DeviceFeatures" ).Element( "GDIFeatures" ).Element( "ElementsAction" ).Elements( "Element" )
                                            select tp;
         
         // ���������� �������� � ������
         foreach ( XElement xecollNameEl in collNameEl )
            slFormElements.Add(xecollNameEl.Attribute("name").Value, null);

         // ���� �������� ������ �� �����
         ControlCollection cc;
         cc = ( ControlCollection ) this.Controls;
         for ( int i = 0 ; i < cc.Count ; i++ )
            if ( slFormElements.Contains(cc[ i ].Name) )
               slFormElements[cc[ i ].Name] = cc[i];
            else
               TestCCforElements( cc[ i ] );

         // �������� �������� ���������
         foreach ( XElement xecollNameEl in collNameEl )
         {
            Control cntrl = ( Control ) slFormElements[ xecollNameEl.Attribute( "name" ).Value ];
            switch(xecollNameEl.Element("Property").Attribute("name").Value)
            {
               case "Enabled":
                  ( ( Control ) slFormElements[ xecollNameEl.Attribute( "name" ).Value ] ).Enabled = Convert.ToBoolean(xecollNameEl.Element( "Property" ).Value) ;
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
			  System.Xml.XmlTextReader reader;

			  // ������ XML
			  if( nfXMLConfig == null )
				  return;

			  if( name_arrVar == "arrStatusFCCommand" )
				  reader = new XmlTextReader( nfXMLConfigFC );
			  else
				  reader = new XmlTextReader( nfXMLConfig );

			  reader.WhitespaceHandling = WhitespaceHandling.Significant; // ��������� ������ �������� ��������

			  //����� ���������� � ����
			  FileStream fs = File.Create( "bmrz.xio" );
			  StreamWriter sw = new StreamWriter( fs );
			  try
			  {
				  while( reader.Read() )
				  {
					  if( reader.NodeType == XmlNodeType.Element )
					  {
						  if( reader.Name.Equals( name_arrVar ) ) //arrVar.ToString())
						  {
							  while( reader.Read() )
								  if( reader.Name.Equals( "formula" ) )
								  {
									  // ��������� �������� �������
									  sl["formula"] = reader.GetAttribute( "express" );
									  sl["caption"] = reader.GetAttribute( "Caption" );
									  sl["dim"] = reader.GetAttribute( "Dim" );
									  sl["TypeOfTag"] = reader.GetAttribute( "TypeOfTag" );
									  sl["TypeOfPanel"] = reader.GetAttribute( "TypeOfPanel" );
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
									  while( reader.Read() )
										  if( reader.Name.Equals( "value" ) )
											  alVal.Add( reader.GetAttribute( 0 ) );
										  else
											  break;
									  if( alVal.Count == 2 )
										  arrVar.Add( new FormulaEval( parent.KB, "0(" + strFC + "." + strIDDev + ( string ) alVal[0] + ")1(" + strFC + "." + strIDDev + ( string ) alVal[1] + ")", sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP ) );
									  else
										  arrVar.Add( new FormulaEval( parent.KB, "0(" + strFC + "." + strIDDev + ( string ) alVal[0] + ")", sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP ) );
								  }
								  else if( reader.Name.Equals( "simple_eval" ) )
								  {
									  sbse.Length = 0;
									  sbse.Append( reader.GetAttribute( "name" ) );
									  reader.Read();
									  se[sbse.ToString()] = reader.GetAttribute( 0 );
									  reader.Read();
								  }
                          else if( reader.Name.Equals( "name_tabpage_ust" ) )
                          {   // ��������� �������� ������� � �������� � ������������
                             sbse.Length = 0;
                             sbse.Append( reader.GetAttribute( "name" ) );
                             reader.Read( );
                             for( int i = 0 ;i < tbcConfig.Controls.Count ;i++ )
                             {
                                if( tbcConfig.Controls[ i ] is TabPage && tbcConfig.Controls[ i ].Name == sbse.ToString( ) )
                                {
                                   tbcConfig.Controls[ i ].Text = reader.GetAttribute( 0 );
                                   sl_tpnameUst[ sbse.ToString( ) ] = tbcConfig.Controls[ i ];
                                }
                             }
                             reader.Read( );
                          }
								  else if( reader.Name.Equals( "" ) )
									  continue;
								  else
									  break;
							  break;
						  }
						  else
							  continue;
					  }
				  }
			  } catch( XmlException ee )
			  {
				  Console.WriteLine( ee.Message );
				  sw.Close();
				  fs.Close();
			  }
			  sw.Close();
			  fs.Close();

			  reader.Close();

			  if (HMI_Settings.ClientDFE != null)
           {
              switch( name_arrVar )
              {
                 case "arrAvarSign":
                    break;
                 case "arrConfigSign":
                    break;
                 case "arrStatusDevCommand":
                    break;
                 case "arrStatusFCCommand":
                    break;
                 case "arrStoreSign":
                    foreach( FormulaEval fe in arrVar )
                       if( fe.addrVar < 60000 )
						   HMI_Settings.ClientDFE.AddArrTags(this.Text, fe);
                    break;
                 default:
                    foreach( FormulaEval fe in arrVar )
						HMI_Settings.ClientDFE.AddArrTags(this.Text, fe);
                    break;
              }
           }
		  }
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

        #region ���������� ���� ���������� ������ �������� �������� ���������� (�� ��)
         /// <summary>
         /// SetArhivGroupInDev(dev, 8)
         /// ���������� ���� ���������� ������ �������� �������� ���������� (�� ��)
         /// </summary>
         /// <param Name="dev">����������</param>
         /// <param Name="idGroup">������</param>
         private void SetArhivGroupInDev(int dev, int idGroup)
         {
            foreach( FC aFc in parent.KB )
               foreach( TCRZADirectDevice tdd in aFc )
                  if( tdd.NumDev == dev )
                     foreach( TCRZAGroup tdg in tdd )
                        if( tdg.Id == idGroup )
                           foreach( TCRZAVariable tgv in tdg )
                              //tgv.SetQuality( VarQuality.vqArhiv );
                              tgv.Quality = VarQuality.vqArhiv;
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

        /*==========================================================================*
          *   private void void LinkSetText(object Value)
          *      ��� ����������������� ������ ���������
          *==========================================================================*/
        delegate void SetLVCallback( ListViewItem li, bool actDellstV );

        private void mnuPageSetup_Click( object sender, EventArgs e )
        {
            parent.mnuPageSetup_Click( sender, e );
        }
        /// <summary>
        /// mnuPrint_Click( object sender, EventArgs e )
        ///     ���������� ������ ���� ��������������� ��������
        /// </summary>
        private void mnuPrintPreview_Click( object sender, EventArgs e )
        {
            PrintArr();
            parent.mnuPrintPreview_Click( sender, e );
        }
        /// <summary>
        /// mnuPrint_Click( object sender, EventArgs e )
        ///     ���������� ������ ���� ������
        /// </summary>
        private void mnuPrint_Click( object sender, EventArgs e )
        {
            PrintArr();
            parent.mnuPrint_Click( sender, e );
        }
        /// <summary>
        /// PrintArr()
        ///     ������ ������� ����������
        /// </summary>
        private void PrintArr()
        {
            StringBuilder sb = new StringBuilder();
            float f_val;
            int i_val;
            string t_val = "";
            ArrayList arCurPrt = new ArrayList();

            object val;

            // ���������� �������� �������
            TabPage tp_sel = tabControl1.SelectedTab;

            sb.Length = 0;
            
            switch(tp_sel.Text)
            {
                case "������� ����������":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (������� ����������)" );
                    sb.Append( "\n========================================================================\n" );
                    sb.Append( " \n \n " );
                    arCurPrt = arrCurSign;
                    break;
                case "���������� �� �������":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (���������� �� �������)" );
                    sb.Append( "\n========================================================================\n" );
                    
                    sb.Append( " \n \n " );
                    arCurPrt = arrAvarSign;
                    break;
                case "������������� ����������":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (������������� ����������)" );
                    sb.Append( "\n========================================================================\n" );
                    
                    sb.Append( " \n \n " );
                    arCurPrt = arrStoreSign;
                    break;
                case "������������ � �������":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (������������ � �������)" );
                    sb.Append( "\n========================================================================\n" );
                    
                    sb.Append( " \n \n " );
                    arCurPrt = arrConfigSign;
                    break;
                case "�������":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (�������)" );
                    sb.Append( "\n========================================================================\n" );
                    
                    sb.Append( " \n \n " );
                    arCurPrt = arrSystemSign;
                    break;
                case "��������� ���������� � ������":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (��������� ���������� � ������)" );
                    sb.Append( "\n========================================================================\n" );
                    sb.Append( " \n \n " );
                    arCurPrt = arrStatusDevCommand;
                    break;
                default:
                    break;
            }                

            for( int i = 0; i < arCurPrt.Count; i++ )
            {
                FormulaEval ev = ( FormulaEval ) arCurPrt[i];

                switch(ev.ToT)
                {
                    case TypeOfTag.Analog:
                        val = ev.tRezFormulaEval.Value;

                        if( val is float )
                        {
                            f_val = ( float ) ev.tRezFormulaEval.Value;
                            t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
                        }
                        else if( val is short )
                        {
                            i_val = ( Int16 ) ev.tRezFormulaEval.Value;
                            t_val = i_val.ToString();
                        }
                        else if( val is int )
                        {
                            i_val = ( Int32 ) ev.tRezFormulaEval.Value;
                            t_val = i_val.ToString();
                        }
                        else if( val is string )
                        { 
                            t_val = (string) ev.tRezFormulaEval.Value;
                        }
        
                        sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
                        break;
                    case TypeOfTag.Discret:
                        sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
                        break;
                    case TypeOfTag.Combo:
                        sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
                        break;
                    default:
                        continue;
                }
                sb.Append( " \n " );
            }
            parent.prt.rtbText.AppendText( sb.ToString() );            
        }
        /// <summary>
        /// sbForSimpleVar(StringBuilder sb)
        ///     ������������ ������ ��� ������ ��� ��������� ����������
        /// </summary>
        private void sbForSimpleVar(StringBuilder sb, FormulaEval b_xxx)
        {
            float f_val;
            int i_val;
            string t_val = "";
            FormulaEval ev = ( FormulaEval ) b_xxx;
            object val;

            if( b_xxx == null )
                return;

            switch( ev.ToT )
            {
                case TypeOfTag.no:
                    val = ev.tRezFormulaEval.Value;

                    if( val is float )
                    {
                        f_val = ( float ) ev.tRezFormulaEval.Value;
                        t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
                    }
                    else if( val is short )
                    {
                        i_val = ( Int16 ) ev.tRezFormulaEval.Value;
                        t_val = i_val.ToString();
                    }
                    else if( val is ushort )
                    {
                        i_val = ( UInt16 ) ev.tRezFormulaEval.Value;
                        t_val = i_val.ToString();
                    }
                    else if( val is int )
                    {
                        i_val = ( Int32 ) ev.tRezFormulaEval.Value;
                        t_val = i_val.ToString();
                    }
                    else if( val is string )
                    {
                        t_val = ( string ) ev.tRezFormulaEval.Value;
                    }

                    sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
                    break;
                case TypeOfTag.Analog:
                    val = ev.tRezFormulaEval.Value;

                    if( val is float )
                    {
                        f_val = ( float ) ev.tRezFormulaEval.Value;
                        t_val = f_val.ToString( "F2" ); // ��� ����� ����� �������
                    }
                    else if( val is short )
                    {
                        i_val = ( Int16 ) ev.tRezFormulaEval.Value;
                        t_val = i_val.ToString();
                    }
                    else if( val is int )
                    {
                        i_val = ( Int32 ) ev.tRezFormulaEval.Value;
                        t_val = i_val.ToString();
                    }
                    else if( val is string )
                    {
                        t_val = ( string ) ev.tRezFormulaEval.Value;
                    }

                    sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
                    break;
                case TypeOfTag.Discret:
                    sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
                    break;
                case TypeOfTag.Combo:
                    sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
                    break;
                default:
                    break;
            }
            sb.Append( " \n " );
        }

		#region ���� �� ������� � ����������� � ��������� �� � ��� ������
		/// <summary>
		/// ���� �� ������� � ���������� �� � ���������� ����������� �� ������
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void tabStatusFC_Enter( object sender, EventArgs e )
		{
			if( arrStatusFCCommand.Count != 0 )
				return;

			nfXMLConfigFC = "frm_tbpFC.xml";

			// ������ ����� ����������� ���, �� ������ ��� ����� ������� ��� �� ������������ - ��������� ����� ����������
			// ��� ����� ������ ��������� ������� ��� ��

			string old_strIDDev = strIDDev;
			strIDDev = "0";

			CreateArrayList( arrStatusFCCommand, "arrStatusFCCommand" );

			//this.tbIntervalReadStore.Enabled = false;
			//this.tbIntervalReadMaxMeter.Enabled = false;
			
			// ��������� ����������� �� �����
			for( int i = 0 ; i < arrStatusFCCommand.Count ; i++ )
			{
				FormulaEval ev = ( FormulaEval ) arrStatusFCCommand[i];
				// ������� ��������� ������� ��� ���������� ���� � ��� ���
				CheckBoxVar chBV;
				ctlLabelTextbox usTB;
				switch( ev.ToT )
				{
					case TypeOfTag.Analog:
						usTB = new ctlLabelTextbox();
						usTB.lblCaption.Text = "";
						usTB.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
						usTB.AutoSize = true;
						ev.StrFormat = HMI_Settings.Precision;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfTag.Discret:
						chBV = new CheckBoxVar();
						//chBV.checkBox1.Text = "";
						chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
						chBV.AutoSize = true;
						chBV.addrLinkVar = ev.addrVar;
						chBV.addrLinkVarBitMask = ev.addrVarBitMask;
						ev.OnChangeValForm += chBV.LinkSetText;
						ev.FirstValue();
						break;
					default:
						MessageBox.Show( "��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error );
						break;
				}
			}
			strIDDev = old_strIDDev;
		}
		#endregion

		#region ���� �� ������� � ������� �����������
		/// <summary>
		/// ���� �� ������� � ������� ����������� 
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void tabCurrent_Enter( object sender, EventArgs e )
		{
			//-------------------------------------------------------------------
			//������� ���. ��� ����������� ������� �������� ���������� � ���������� ��������
			// ������� ���������� ������� (���. 0033 ...) � ������ ������������ �������������
			if( arrCurSign.Count != 0 )
				return;
			CreateArrayList( arrCurSign, "arrCurSign" );

			// ��������� ����������� �� �����
			for( int i = 0 ; i < arrCurSign.Count ; i++ )
			{
				FormulaEval ev = ( FormulaEval ) arrCurSign[i];
				// ������� ��������� ������� ��� ���������� ���� � ��� ���
				CheckBoxVar chBV;
				ctlLabelTextbox usTB;
				switch( ev.ToT )
				{
					case TypeOfTag.Analog:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
						usTB.AutoSize = true;
						ev.StrFormat = HMI_Settings.Precision;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfTag.Discret:
						chBV = new CheckBoxVar();
						chBV.checkBox1.Text = "";
						chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
						chBV.AutoSize = true;
						ev.OnChangeValForm += chBV.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfTag.no:
						break;
					default:
						MessageBox.Show( "��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error );
						break;
				}
			}
         if ( USO_Current_Analog_Signals.Controls.Count == 0 )
            splitContainer7.Panel1Collapsed = true;
		}
		#endregion

		private void button2_Click( object sender, EventArgs e )
		{
				Button btn = ( Button ) sender;
			byte[] memXOut = new byte[2];
			int ng = Convert.ToInt32( btn.Tag );
			memXOut[1] = Convert.ToByte( btn.Tag );
			//------------------------------------------------------
			if( parent.isReqPassword )
				if( !parent.CanAction() )
				{
					MessageBox.Show( "���������� �������� ���������" );
					return;
				}

			ConfirmCommand dlg = new ConfirmCommand();
			dlg.label1.Text = btn.Text + "?";

			if( !( DialogResult.OK == dlg.ShowDialog() ) )
				return;
			//------------------------------------------------------
         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "CLS", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
			{
            //parent.newKB.ExecuteCommand( iFC, iIDDev, "NOP", null, String.Empty,  parent.toolStripProgressBar1, parent.statusStrip1, parent );
				parent.WriteEventToLog(39, "������� \"CLS\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, false);//, true, false );
			}
			// ���������������� �������� ������������
			parent.WriteEventToLog(39, iIDDev.ToString(), this.Name, true);//, true, false );
		}

		private void button31_Click( object sender, EventArgs e )
		{
			Button btn = ( Button ) sender;
			byte[] memXOut = new byte[2];
			int ng = Convert.ToInt32( btn.Tag );
			memXOut[1] = Convert.ToByte( btn.Tag );

			//------------------------------------------------------
			if( parent.isReqPassword )
				if( !parent.CanAction() )
				{
					MessageBox.Show( "���������� �������� ���������" );
					return;
				}

			ConfirmCommand dlg = new ConfirmCommand();
			dlg.label1.Text = btn.Text + "?";

			if( !( DialogResult.OK == dlg.ShowDialog() ) )
				return;
			//------------------------------------------------------

         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "OPN", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
			{
				//parent.newKB.ExecuteCommand( iFC, iIDDev, "NOP", null, parent.toolStripProgressBar1, parent.statusStrip1, parent );
				parent.WriteEventToLog(40, "������� \"OPN\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, false);//, true, false );
			}
			// ���������������� �������� ������������
			parent.WriteEventToLog(40, iIDDev.ToString(), this.Name, true);//, true, false );
		}
      #region ���� �� ������� "������������ � �������"
      /// <summary>
      /// private void tbcConfig_Enter( object sender, EventArgs e )
      ///  ���� �� ������� "������������ � �������"
      /// </summary>
      private void tbcConfig_Enter( object sender, EventArgs e )
      {
         lstvConfig.Items.Clear( );
         UstavBD( );
         //-------------------------------------------------------------------
         //������� ���. ��� ����������� ���������� � ���������� ��������
         if( arrConfigSign.Count != 0 )
            return;
         btnWriteUst.Enabled = false;
         CreateArrayList( arrConfigSign, "arrConfigSign" );

         // ��� ������ �������� ��� tabpage
         for( int i = 0 ;i < tbcConfig.Controls.Count ;i++ )
         {
            if( tbcConfig.Controls[ i ] is TabPage )
            {
               tbcConfig.Controls.RemoveAt( i );
               i--;
            }
         }

         // ������������ �������� �������
         for( int i = 0 ;i < sl_tpnameUst.Count ;i++ )
            tbcConfig.Controls.Add( ( Control ) sl_tpnameUst.GetByIndex( i ) );

         // ��������� ����������� �� �����
         for( int i = 0 ;i < arrConfigSign.Count ;i++ )
         {
            FormulaEval ev = ( FormulaEval ) arrConfigSign[ i ];
            // ������� ��������� ������� ��� ���������� ���� � ��� ���
            CheckBoxVar chBV;
            ctlLabelTextbox usTB;
            ComboBoxVar cBV;
            switch( ev.ToT )
            {
               case TypeOfTag.Combo:
                  cBV = new ComboBoxVar( ( string[] ) ( ( TagEval ) ( ( TagVal ) ev.arrTagVal[ 0 ] ).linkTagEval ).arrStrCB.ToArray( typeof( string ) ), 0 );
                  cBV.Parent = ( FlowLayoutPanel ) slFLP[ ev.ToP ];
                  cBV.AutoSize = true;
                  cBV.addrLinkVar = ev.addrVar;

                  ev.OnChangeValForm += cBV.LinkSetText;
                  ev.FirstValue( );
                  break;
               case TypeOfTag.Analog:
                  usTB = new ctlLabelTextbox( );
                  usTB.lblCaption.Text = "";
                  usTB.Parent = ( FlowLayoutPanel ) slFLP[ ev.ToP ];
                  usTB.AutoSize = true;
                  usTB.addrLinkVar = ev.addrVar;
                  usTB.txtLabelText.ReadOnly = false;
                  ev.StrFormat = HMI_Settings.Precision;
                  ev.OnChangeValForm += usTB.LinkSetText;
                  ev.FirstValue( );
                  break;
               case TypeOfTag.Discret:
                  chBV = new CheckBoxVar( );
                  chBV.Parent = ( FlowLayoutPanel ) slFLP[ ev.ToP ];
                  chBV.AutoSize = true;
                  chBV.addrLinkVar = ev.addrVar;
                  chBV.addrLinkVarBitMask = ev.addrVarBitMask;
                  chBV.btnCheck.Visible = false;
                  ev.OnChangeValForm += chBV.LinkSetText;
                  ev.FirstValue( );
                  break;
               default:
                  MessageBox.Show( "��� ������ ���� �������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error );
                  break;
            }
         }
         //Config_BottomPanel.Width = pnlConfig.Width - btnResetValues.Width - groupBox11.Width - groupBox9.Width + 10;
         //splitter1.Visible = false;
      }
      private void UstavBD()
      {
         //dgvAvar.Rows.Clear();
         // ��������� ����� ���������� � ���������� ������ �� ����� *.config
         //string cnStr = ConfigurationManager.ConnectionStrings[ "SqlProviderPTK" ].ConnectionString;
         SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
         try
         {
            asqlconnect.Open( );
         }
         catch( SqlException ex )
         {
            string errorMes = "";
            // ���������� ���� ������������ ������
            foreach( SqlError connectError in ex.Errors )
               errorMes += connectError.Message + " (������: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
				parent.WriteEventToLog(21, "��� ����� � �� (UstavBD): " + errorMes, this.Name, false);//, true, false ); // ������� ��� ����� � ��

            asqlconnect.Close( );
            return;
         }
         catch( Exception ex )
         {
            MessageBox.Show( "��� ����� � ��������" + Environment.NewLine + ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Information );
            asqlconnect.Close( );
            return;
         }
         // ������������ ������ ��� ������ �������� ���������
         SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
         cmd.CommandType = CommandType.StoredProcedure;

         // ������� ���������
         // 1. ip FC
         SqlParameter pipFC = new SqlParameter( );
         pipFC.ParameterName = "@IP";
         pipFC.SqlDbType = SqlDbType.BigInt;
         pipFC.Value = 0;
         pipFC.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pipFC );
         // 2. id ����������
         SqlParameter pidBlock = new SqlParameter( );
         pidBlock.ParameterName = "@id";
         pidBlock.SqlDbType = SqlDbType.Int;
         pidBlock.Value = iIDDev;
         pidBlock.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pidBlock );

         // 3. ��������� �����
         SqlParameter dtMim = new SqlParameter( );
         dtMim.ParameterName = "@dt_start";
         dtMim.SqlDbType = SqlDbType.DateTime;
         TimeSpan tss = new TimeSpan( 0, dtpStartDateConfig.Value.Hour - dtpStartTimeConfig.Value.Hour, dtpStartDateConfig.Value.Minute - dtpStartTimeConfig.Value.Minute, dtpStartDateConfig.Value.Second - dtpStartTimeConfig.Value.Second );
         DateTime tim = dtpStartDateConfig.Value - tss;
         dtMim.Value = tim;
         dtMim.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMim );

         // 2. �������� �����
         SqlParameter dtMax = new SqlParameter( );
         dtMax.ParameterName = "@dt_end";
         dtMax.SqlDbType = SqlDbType.DateTime;
         tss = new TimeSpan( 0, dtpEndDateConfig.Value.Hour - dtpEndTimeConfig.Value.Hour, dtpEndDateConfig.Value.Minute - dtpEndTimeConfig.Value.Minute, dtpEndDateConfig.Value.Second - dtpEndTimeConfig.Value.Second );
         tim = dtpEndDateConfig.Value - tss;
         dtMax.Value = tim;
         dtMax.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMax );

         // 5. ��� ������
         SqlParameter ptypeRec = new SqlParameter( );
         ptypeRec.ParameterName = "@type";
         ptypeRec.SqlDbType = SqlDbType.Int;
         ptypeRec.Value = 1; // ���������� �� ��������
         ptypeRec.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( ptypeRec );
         // 6. �� ������ �������
         SqlParameter pid = new SqlParameter( );
         pid.ParameterName = "@id_record";
         pid.SqlDbType = SqlDbType.Int;
         pid.Value = 0;
         pid.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pid );

         // ���������� DataSet
         DataSet aDS = new DataSet( "ptk" );
         SqlDataAdapter aSDA = new SqlDataAdapter( );
         aSDA.SelectCommand = cmd;

         //aSDA.sq
         aSDA.Fill( aDS, "TbUstav" );

         asqlconnect.Close( );

         //PrintDataSet( aDS );
         // ��������� ������ �� ��������
         dtU = aDS.Tables[ "TbUstav" ];

         // ��������� ListView
         lstvConfig.Items.Clear( );
         for( int curRow = 0 ;curRow < dtU.Rows.Count ;curRow++ )
         {
            DateTime t = ( DateTime ) dtU.Rows[ curRow ][ "TimeBlock" ];
            ListViewItem li = new ListViewItem( t.ToShortDateString( ) );
            li.SubItems.Add( t.ToLongTimeString( ) + ":" + t.Millisecond );
            li.Tag = dtU.Rows[ curRow ][ "ID" ];
            lstvConfig.Items.Add( li );
         }
         aSDA.Dispose( );
         aDS.Dispose( );
      }
      #endregion
      private void btnReadUstFC_Click( object sender, EventArgs e )
      {
         btnWriteUst.Enabled = true;

         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "IMP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "������� \"IMP\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true);//, true, false );

         // ���������������� �������� ������������
			parent.WriteEventToLog(7, iIDDev.ToString(), this.Name, true);//, true, false );//"������ ������� IMP - ������ �������."
         if( b_62002 != null )
            b_62002.FirstValue( );
         if( b_62092 != null )
            b_62092.FirstValue( );
      }

      private void btnReadUstBlock_Click( object sender, EventArgs e )
      {
         btnWriteUst.Enabled = true;

         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RCP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "������� \"RCP\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true);//, true, false );

         // ���������������� �������� ������������
			parent.WriteEventToLog(7, iIDDev.ToString(), this.Name, true);//, true, false );//"������ ������� RCP - ������ �������."
         if( b_62002 != null )
            b_62002.FirstValue( );
         if( b_62092 != null )
            b_62092.FirstValue( );
      }

      #region ����� ���������� ��� ������ ���������� ������ �� ��������
      private void lstvConfig_ItemActivate( object sender, EventArgs e )
      {
         if( lstvConfig.SelectedItems.Count == 0 )
            return;

         // ��������� ����� ���������� � ���������� ������ �� ����� *.config
         //string cnStr = ConfigurationManager.ConnectionStrings[ "SqlProviderPTK" ].ConnectionString;
         SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
         try
         {
            asqlconnect.Open( );
         }
         catch( SqlException ex )
         {
            string errorMes = "";
            // ���������� ���� ������������ ������
            foreach( SqlError connectError in ex.Errors )
               errorMes += connectError.Message + " (������: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
				parent.WriteEventToLog(21, "��� ����� � �� (lstvConfig_ItemActivate): " + errorMes, this.Name, false);//, true, false ); // ������� ��� ����� � ��

            asqlconnect.Close( );
            return;
         }
         catch( Exception ex )
         {
            MessageBox.Show( "��� ����� � ��������" + Environment.NewLine + ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Information );
            asqlconnect.Close( );
            return;
         }
         // ������������ ������ ��� ������ �������� ���������
         SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
         cmd.CommandType = CommandType.StoredProcedure;

         // ������� ���������
         // 1. ip FC
         SqlParameter pipFC = new SqlParameter( );
         pipFC.ParameterName = "@IP";
         pipFC.SqlDbType = SqlDbType.BigInt;
         pipFC.Value = 0;
         pipFC.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pipFC );
         // 2. id ����������
         SqlParameter pidBlock = new SqlParameter( );
         pidBlock.ParameterName = "@id";
         pidBlock.SqlDbType = SqlDbType.Int;
         pidBlock.Value = 0;
         pidBlock.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pidBlock );
         // 3. ����� �����
         SqlParameter ptimeStart = new SqlParameter( );
         ptimeStart.ParameterName = "@dt_start";
         ptimeStart.SqlDbType = SqlDbType.DateTime;
         ptimeStart.Value = DateTime.Now;
         ptimeStart.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( ptimeStart );
         // 4. ����� �����
         SqlParameter ptimeFin = new SqlParameter( );
         ptimeFin.ParameterName = "@dt_end";
         ptimeFin.SqlDbType = SqlDbType.DateTime;
         ptimeFin.Value = DateTime.Now;
         ptimeFin.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( ptimeFin );
         // 5. ��� ������ - �� ����� - ��� �� Tag
         SqlParameter ptypeRec = new SqlParameter( );
         ptypeRec.ParameterName = "@type";
         ptypeRec.SqlDbType = SqlDbType.Int;
         ptypeRec.Value = 0;
         ptypeRec.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( ptypeRec );
         // 6. �� ������ �������
         SqlParameter pid = new SqlParameter( );
         pid.ParameterName = "@id_record";
         pid.SqlDbType = SqlDbType.Int;
         pid.Value = lstvConfig.SelectedItems[ 0 ].Tag;
         pid.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pid );

         // ���������� DataSet
         DataSet aDS = new DataSet( "ptk" );
         SqlDataAdapter aSDA = new SqlDataAdapter( );
         aSDA.SelectCommand = cmd;

         //aSDA.sq
         aSDA.Fill( aDS );//, "DataLog" 

         asqlconnect.Close( );

         //PrintDataSet( aDS );
         // ��������� ������ �� ������
         DataTable dt = aDS.Tables[ 0 ];
         byte[] adata = ( byte[] ) dt.Rows[ 0 ][ "Data" ];

         // �������� ��������� ������� ������ �� ����
         ParseBDPacket( adata, 62000, iIDDev );
         SetArhivGroupInDev( iIDDev, 14 );

         dt.Dispose( );
         aSDA.Dispose( );
         aDS.Dispose( );

         btnWriteUst.Enabled = true;
      }
      private void btnResetValues_Click( object sender, EventArgs e )
      {
         btnWriteUst.Enabled = false;
         parent.newKB.ResetGroup( iFC, iIDDev, 14 );
      }
      private void dtpStartDateConfig_ValueChanged( object sender, EventArgs e )
      {
         //UstavBD( );
      }
      /// <summary>
      /// private void btnWriteUst_Click( object sender, EventArgs e )
      /// ������ �������
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>  
      private void btnWriteUst_Click( object sender, EventArgs e )
      {
         if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, parent.UserRight ) )
            return;

         if( parent.isReqPassword )
            if( !parent.CanAction( ) )
            {
               MessageBox.Show( "���������� �������� ���������" );
               return;
            }

         DialogResult dr = MessageBox.Show( "�������� �������?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
         if( dr == DialogResult.No )
            return;

         string stri;
         TabPage tp;
         ctlLabelTextbox ultb;
         CheckBoxVar chbTmp;
         ComboBoxVar cbTmp;

         FlowLayoutPanel flp;
         bool isUstChange = false;   // ���� ��������� ������� ��� ������������ ������������ �������
         StringBuilder sb = new StringBuilder( );
         uint ainmemX;    // ����� � ������� memX
         byte[] aTmp2 = new byte[ 2 ];

         // ������ SortedList ��� ������� ����������
         slLocal = new SortedList( );
         foreach( FC aFC in parent.KB )
            if( aFC.NumFC == iFC )
            {
               foreach( TCRZADirectDevice aDev in aFC )
                  if( aDev.NumDev == iIDDev )
                  {
                     slLocal = aDev.CRZAMemDev;
                     break;
                  }
               break;
            }
         // ��������� ����� � ��������� ��� �������������
         //BinaryReader memDevBlock = ( BinaryReader ) slLocal[62000];

         // ������ ������ � ������ 
         //byte[] memXt = new byte[( ( byte[] ) slLocal[62000] ).Length];
         //System.Buffer.BlockCopy( brP, 6, memX, 0, brP.Length - 6 );

         // ������ ������ � ������ 
         //memDevBlock.BaseStream.Position = 0;
         int lenpack = 0;
         try
         {
            lenpack = BitConverter.ToInt16( ( byte[] ) slLocal[ 62000 ], 0 );
         }
         catch( ArgumentNullException ex )
         {
            MessageBox.Show( "��� ������ ��� ������. \n���������� ������� ��������.", "���������", MessageBoxButtons.OK, MessageBoxIcon.Information );
            return;
         }

         short numdev = BitConverter.ToInt16( ( byte[] ) slLocal[ 62000 ], 2 );

         ushort add10 = BitConverter.ToUInt16( ( byte[] ) slLocal[ 62000 ], 4 );	//������ ����� ����� ������

         //int lenpack = ( short ) memDevBlock.ReadInt16();
         //short numdev = ( short ) memDevBlock.ReadUInt16();
         //ushort add10 = ( ushort ) memDevBlock.ReadInt16();	//������ ����� ����� ������

         byte[] memX = new byte[ lenpack - 6 ];
         System.Buffer.BlockCopy( ( byte[] ) slLocal[ 62000 ], 6, memX, 0, ( ( byte[] ) slLocal[ 62000 ] ).Length - 6 );

         //memDevBlock.Read( memX, 0, lenpack - 6 );

         for( int i = 0 ;i < tbcConfig.Controls.Count ;i++ )
         {
            if( tbcConfig.Controls[ i ] is TabPage )
            {
               tp = ( TabPage ) tbcConfig.Controls[ i ];
               for( int j = 0 ;j < tp.Controls.Count ;j++ )
               {
                  if( tp.Controls[ j ] is FlowLayoutPanel )
                  {
                     flp = ( FlowLayoutPanel ) tp.Controls[ j ];
                     for( int n = 0 ;n < flp.Controls.Count ;n++ )
                     {
                        if( flp.Controls[ n ] is ctlLabelTextbox )
                        {
                           ultb = ( ctlLabelTextbox ) flp.Controls[ n ];
                           if( ultb.isChange )
                           {
                              CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 );
                              //StrToBCD_Field( ultb, memX );
                              isUstChange = true;
                           }
                        }
                        else if( flp.Controls[ n ] is ComboBoxVar )
                        {
                           cbTmp = ( ComboBoxVar ) flp.Controls[ n ];
                           if( cbTmp.isChange )
                           {
                              isUstChange = true;
                              cbTmp.isChange = false;  // ���������� ������� ��������� � ����������� ComboBoxVar'�
                              // ���������� ��������� �� ComboBoxVar'�� � �������� ����� (������������ ������ memX)
                              uint a = cbTmp.addrLinkVar; // ����� ����������
                              // ������� ��������
                              int st = cbTmp.cbVar.SelectedIndex;
                              byte[] bst = new byte[ 4 ];
                              bst = BitConverter.GetBytes( st );
                              Buffer.BlockCopy( bst, 0, aTmp2, 0, 2 );
                              Array.Reverse( aTmp2 );
                              // ���������� ���������
                              ainmemX = ( a - 62000 ) * 2;
                              Buffer.BlockCopy( aTmp2, 0, memX, ( int ) ainmemX, 2 );
                           }
                        }
                        else if( flp.Controls[ n ] is CheckBoxVar )
                        {
                           chbTmp = ( CheckBoxVar ) flp.Controls[ n ];
                           if( chbTmp.isChange )
                           {
                              isUstChange = true;
                              chbTmp.isChange = false;    // ���������� ������� ��������� � ����������� CheckBoxVar'�
                              // �������� ������� ���� �� ��������� �������
                              ainmemX = ( chbTmp.addrLinkVar - 62000 ) * 2;   // ��� �����
                              //aTmp2 = new byte[2];
                              Buffer.BlockCopy( memX, ( int ) ainmemX, aTmp2, 0, 2 );
                              string bitmask = chbTmp.addrLinkVarBitMask;
                              UInt16 ibitmask = Convert.ToUInt16( chbTmp.addrLinkVarBitMask, 16 );
                              Array.Reverse( aTmp2 );
                              UInt16 rezbit = BitConverter.ToUInt16( aTmp2, 0 );
                              if( chbTmp.checkBox1.Checked == true )
                                 rezbit = Convert.ToUInt16( rezbit | ibitmask );
                              else
                              {
                                 UInt16 ti = ( UInt16 ) ~ibitmask; //Convert.ToUInt16()
                                 rezbit = Convert.ToUInt16( rezbit & ~ibitmask );
                              }
                              // �������� �� �����
                              aTmp2 = BitConverter.GetBytes( rezbit );
                              Array.Reverse( aTmp2 );
                              Buffer.BlockCopy( aTmp2, 0, memX, ( int ) ainmemX, 2 );
                           }
                        }
                     }
                  }
               }
            }
         }
         //------------------------------
         // ���������� ��� ������ �������
         //for( int n = 0 ; n < pnlConfig.Controls.Count ; n++ )
         for( int n = 0 ;n < Config_BottomPanel.Controls.Count ;n++ )
            //if( pnlConfig.Controls[n] is ctlLabelTextbox )
            if( ( Config_BottomPanel.Controls[ n ] as ctlLabelTextbox ) != null )
            {
               ultb = ( ctlLabelTextbox ) Config_BottomPanel.Controls[ n ];
               if( ultb.Name == "ctlTimeUstavkiSbros" )
                  continue;

               if( ultb.isChange )
               {
                  CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 );
                  //StrToBCD_Field( ultb, memX );
                  isUstChange = true;
               }
            }
         //------------------------------
         if( !isUstChange )
         {
            MessageBox.Show( "������� �� ����������. \n���������� ������� ��������.", "���������", MessageBoxButtons.OK, MessageBoxIcon.Information );
            return;
         }
         // ��������� ����� � ������� ��� �������� ��������� �������
         byte[] memXOut = new byte[ memX.Length ];
         Buffer.BlockCopy( memX, 4, memXOut, 4, memX.Length - 4 );  // Handle ���� �������

         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "WCP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "������� \"WCP\" ���� � ����. ���������� - " + iIDDev.ToString(), this.Name, true);//, true, false );
         // ���������������� �������� ������������
			parent.WriteEventToLog(6, iIDDev.ToString(), this.Name, true);//, true, false );			//"������ ������� WCP - ������ �������."
         isUstChange = false;
      }		

      #endregion

      #region ��������� ������� ������ � ����������� �� ����
      private void ParseBDPacket( byte[] pack, ushort adr, int dev )
      {
         PrintHexDump( "LogHexPacket.dat", pack );  // ������� � ���� ��� ��������
         parent.newKB.PacketToQueDev( pack, adr, iFC,dev ); // 10280 �����  �� ������  ����������
         // �������� �������������� ������ ���������� ��������
         SetArhivGroupInDev( dev, 8 );
      }
      #endregion

      private void btnReNewUstBD_Click( object sender, EventArgs e )
      {
         UstavBD( );
         tcUstConfigBottomPanel.SelectTab( 0 );
      }

      private void frmUSO_FormClosing( object sender, FormClosingEventArgs e )
      {
		  if (HMI_Settings.ClientDFE != null)
			  HMI_Settings.ClientDFE.RemoveRefToPageTags(this.Text);
      }
	}
}
        