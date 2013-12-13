using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using LabelTextbox;
using CRZADevices;
using Calculator;

namespace HMI_MT
{
	public partial class dlgTPResurs : Form
	{
		private MainForm parent;
		string nfXMLConfig; // ��� ����� � ��������� 
		string nfXMLConfigFC; // ��� ����� � ��������� ����
		int iFC;            // ����� �� �������������
		string strFC;       // ����� �� ������
		int iIDDev;         // ����� ���������� �������������
		string strIDDev;    // ����� ���������� ������
		int inumLoc;         // ����� ������ �������������
		string strnumLoc;    // ����� ������ ������
		ArrayList arrVar =new ArrayList();
		private string name_arrVar = "";

		SortedList se = new SortedList();
		SortedList sl_tpnameUst = new SortedList();
		StringBuilder sbse = new StringBuilder();

		public dlgTPResurs( MainForm parent, string name_arrVar, string strFC, string strIDDev, string strnumLoc, string nfXMLConfig, string nfXMLConfigFC )
		{
			InitializeComponent();
			this.parent = parent;
			this.nfXMLConfig = nfXMLConfig;
			this.nfXMLConfigFC = nfXMLConfigFC;
			this.name_arrVar = name_arrVar;
			this.strFC = strFC;
			this.strIDDev = strIDDev;
			this.strnumLoc = strnumLoc;
			iIDDev = Convert.ToInt32(strIDDev);
		}

		private void btnCloseRes_Click( object sender, EventArgs e )
		{
			this.Close();
		}

		private void dlgTPResurs_Shown( object sender, EventArgs e )
		{
			btnWriteResurs.Enabled = false;

			CreateArrayList( arrVar, name_arrVar );

			// ��������� ����������� �� �����
			for( int i = 0 ; i < arrVar.Count ; i++ )
			{
				FormulaEval ev = ( FormulaEval ) arrVar[i];
				// ������� ��������� ������� ��� ���������� ���� � ��� ���
				CheckBoxVar chBV;
				ctlLabelTextbox usTB;
				switch( ev.ToP )
				{
					case TypeOfPanel.TP_ResursUst:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpResursUst;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfPanel.TP_ResursCount:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpResursCount;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();						
						break;
					case TypeOfPanel.TP_ResursOther:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpResursOther;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();						
						break;
					case TypeOfPanel.TP_ResursUst_Commut:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpResursUst_Commut;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;					
					default:
						break;
				}
			}
		}
		/// <summary>
		/// �������� ������� ArrayList � ��������� ���������� �� ����������� ����� XML
		/// </summary>
		/// <param name="arrVar"> ������  ArrayList
		///������</param>
		/// <param name="nameFile">��� ����� XML
		///������</param>
		private void CreateArrayList( ArrayList arrVar, string name_arrVar )
		{
			SortedList sl = new SortedList();
			ArrayList alVal = new ArrayList();
			System.Xml.XmlTextReader reader;

			// ������ XML
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
									TypeOfPanel ToP = TypeOfPanel.no;

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
									ToP = ( string ) sl["TypeOfPanel"];
									//switch( ( string ) sl["TypeOfPanel"] )
									//{
									//   case "TP_ResursUst":
									//      ToP = TypeOfPanel.TP_ResursUst;
									//      break;
									//   case "TP_ResursCount":
									//      ToP = TypeOfPanel.TP_ResursCount;
									//      break;
									//   case "TP_ResursOther":
									//      ToP = TypeOfPanel.TP_ResursOther;
									//      break;
									//   case "TP_ResursUst_Commut":
									//      ToP = TypeOfPanel.TP_ResursUst_Commut;
									//      break;
											
									//   case "no":
									//      ToP = TypeOfPanel.no;
									//      break;

									//   default:
									//      MessageBox.Show( "��� ������ ������ ���� " );
									//      break;
									//}
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
									//sbse.Length = 0;
									//sbse.Append( reader.GetAttribute( "name" ) );
									//reader.Read();
									//for( int i = 0 ; i < tbcConfig.Controls.Count ; i++ )
									//{
									//   if( tbcConfig.Controls[i] is TabPage && tbcConfig.Controls[i].Name == sbse.ToString() )
									//   {
									//      tbcConfig.Controls[i].Text = reader.GetAttribute( 0 );
									//      sl_tpnameUst[sbse.ToString()] = tbcConfig.Controls[i];
									//   }
									//}
									//reader.Read();
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
		}

		private void btnReadResursFromDev_Click( object sender, EventArgs e )
		{
			btnWriteResurs.Enabled = true;

			parent.newKB.ExecuteCommand( Convert.ToInt32( strFC ), Convert.ToInt32( strIDDev ), "RBH", null, parent.toolStripProgressBar1 );	//iFC iIDDev
			// ���������������� �������� ������������
			parent.WriteEventToLog( 32, strIDDev, this.Name, true, true, false );//"������ ������� RCD - ������ �������������." iIDDev.ToString()
		}

		private void btnWriteResurs_Click( object sender, EventArgs e )
		{
			SortedList slLocal;
			bool isResChange = false;   // ���� ��������� ������� ��� ������������ ������������ �������

			DialogResult dr = MessageBox.Show( "�������� ������ �������?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
			if( dr == DialogResult.No )
				return;

			slLocal = new SortedList();
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

			// ������ ������ � ������ 
			int lenpack = 0;
			try
			{
				lenpack = BitConverter.ToInt16( ( byte[] ) slLocal[62200], 0 );
			} catch( ArgumentNullException ex )
			{
				MessageBox.Show( "��� ������ ��� ������. \n���������� ������� ��������.", "���������", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			short numdev = BitConverter.ToInt16( ( byte[] ) slLocal[62200], 2 );

			ushort add10 = BitConverter.ToUInt16( ( byte[] ) slLocal[62200], 4 );	//������ ����� ����� ������

			byte[] memX = new byte[lenpack - 6];
			System.Buffer.BlockCopy( ( byte[] ) slLocal[62200], 6, memX, 0, ( ( byte[] ) slLocal[62200] ).Length - 6 );

			ChangeResFromFlp( splitContainer4.Panel1.Controls[0], memX, ref isResChange );
			ChangeResFromFlp( splitContainer4.Panel2.Controls[0], memX, ref isResChange );
			ChangeResFromFlp( splitContainer3.Panel2.Controls[0], memX, ref isResChange );
			ChangeResFromFlp( splitContainer2.Panel2.Controls[0], memX, ref isResChange );

			//------------------------------
			if( !isResChange )
			{
				MessageBox.Show( "������� �� ����������. \n���������� ������� ��������.", "���������", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}
			// ��������� ����� � ������� ��� �������� ��������� �������
			byte[] memXOut = new byte[memX.Length];
			Buffer.BlockCopy( memX, 4, memXOut, 4, memX.Length - 4 );  // Handle ���� �������

			parent.newKB.ExecuteCommand( Convert.ToInt32( strFC ), Convert.ToInt32( strIDDev ), "WBH", memXOut, parent.toolStripProgressBar1 );	//iFC iIDDev

			// ���������������� �������� ������������
			parent.WriteEventToLog( 33, strIDDev, this.Name, true, true, false );//"������ ������� WBH - ������ �������." 
			isResChange = false;
		}
		/// <summary>
		/// private void ChangeResFromFlp( object fl)
		/// </summary>
		/// <param name="fl"></param>
		private void ChangeResFromFlp( object fl, byte[] memX, ref bool isResChange )
		{
			if( !( fl is FlowLayoutPanel ) )
				return;

			FlowLayoutPanel flp = ( FlowLayoutPanel ) fl;
			ctlLabelTextbox ultb;
			for( int n = 0 ; n < flp.Controls.Count ; n++ )
			{
				if( flp.Controls[n] is ctlLabelTextbox )
				{
					ultb = ( ctlLabelTextbox ) flp.Controls[n];
					if( ultb.isChange )
					{
						CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62200 );
						//StrToBCD_Field( ultb, memX );
						isResChange = true;
					}
				}
			}
		}
		/// <summary>
		/// ������ ������� � ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnReadResursFromFC_Click( object sender, EventArgs e )
		{
			btnWriteResurs.Enabled = true;
			parent.newKB.ExecuteCommand( Convert.ToInt32( strFC ), Convert.ToInt32( strIDDev ), "IMB", null, parent.toolStripProgressBar1 );	//iFC iIDDev
			// ���������������� �������� ������������
			parent.WriteEventToLog( 32, strIDDev, this.Name, true, true, false );//"������ ������� - ������ �������." iIDDev.ToString()
		}        
	}
}