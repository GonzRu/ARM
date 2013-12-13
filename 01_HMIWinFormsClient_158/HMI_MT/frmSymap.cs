using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LabelTextbox;
using CRZADevices;
using Calculator;
using CommonUtils;
using System.Xml;
using System.IO;

namespace HMI_MT
{
	public partial class frmSymap : Form
	{
		private MainForm parent;
      int iFC;            // номер ФК целочисленный
      string strFC;       // номер ФК строка
      int iIDDev;         // номер устройства целочисленный
      string strIDDev;    // номер устройства строка
      int inumLoc;         // номер ячейки целочисленный
      string strnumLoc;    // номер ячейки строка
		string nfXMLConfigFC; // имя файла с описанием ЩАСУ
		string nfXMLConfig; // имя файла с описанием 

		ArrayList arrVar = new ArrayList();
		private string name_arrVar = "";

		// для вкладки, отображающей тело пакета
		ushort iclm = 16;  // число колонок в дампе
		SortedList slLocal;
		EncodingInfo eii;
		SortedList slEncoding;
		StringBuilder sbse = new StringBuilder();
		SortedList se = new SortedList();

		public frmSymap( )
		{
			InitializeComponent();
		}
		#region конструктор
		public frmSymap( MainForm linkMainForm, string name_arrVar, int iFC, int iIDDev, int inumLoc, string fXML )
		{
			InitializeComponent();
            parent = linkMainForm;
            this.iFC = iFC;                 // номер ФК целочисленный
            strFC = iFC.ToString();         // номер ФК строка
            this.iIDDev = iIDDev;           // номер устройства целочисленный
            strIDDev = iIDDev.ToString();   // номер устройства строка
            this.inumLoc = inumLoc;         // номер ячейки целочисленный
            strnumLoc = inumLoc.ToString();    // номер ячейки строка
            nfXMLConfig = fXML;
				this.nfXMLConfigFC = "";
				this.name_arrVar = name_arrVar;
		}
		#endregion
		#region вход на вкладку для просмотра доступных пакетов
		private void tbpPacketViewer_Enter( object sender, EventArgs e )
		{
			// выбор кодировки
			slEncoding = new SortedList();
			int ii = 0;
			foreach( EncodingInfo ei in Encoding.GetEncodings() )
			{
				slEncoding[ii] = ei;
				cbEncode.Items.Add( "[" + ei.CodePage.ToString() + "]" + " : " + ei.DisplayName );
				if( ei.CodePage == 866 )
					cbEncode.SelectedIndex = ii;    // кодировка по умолчанию
				ii++;
			}
			eii = ( EncodingInfo ) slEncoding[cbEncode.SelectedIndex];  //EncodingInfo

			slLocal = new SortedList();
			// найдем SortedList для нужного устройства
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
			// заполняем ComboBox
			cbAvailablePackets.Items.Clear();
			for( int i = 0 ; i < slLocal.Count ; i++ )
				cbAvailablePackets.Items.Add( slLocal.GetKey( i ) );
			try
			{
				cbAvailablePackets.SelectedIndex = 0;
			} catch( Exception eee )
			{
				MessageBox.Show( "Нет данных для отображения. " + eee.Message );
			}
		}

		private void cbAvailablePackets_SelectedIndexChanged( object sender, EventArgs e )
		{
			ReNew();
		}
		private void PacketViewer_Output( byte[] brP, ushort numColumn )//BinaryReader
		{
			int lenpack = BitConverter.ToInt16( brP, 0 );

			short numdev = BitConverter.ToInt16( brP, 2 );

			ushort add10 = BitConverter.ToUInt16( brP, 4 );	//читаем адрес блока данных

			// читаем данные в массив 
			byte[] memX = new byte[brP.Length - 6];
			System.Buffer.BlockCopy( brP, 6, memX, 0, brP.Length - 6 );

			Encoding e = Encoding.ASCII;
			try
			{
				e = eii.GetEncoding();
			} catch
			{
				MessageBox.Show( "Ошибка при выборе кодировки" );
			}

			char[] arrCh = new char[e.GetCharCount( memX, 0, memX.Length )];
			e.GetChars( memX, 0, memX.Length, arrCh, 0 );

			// формируем ListView

			ColumnHeader ch = new ColumnHeader();
			ch.DisplayIndex = 0;
			ch.Name = "clm_" + ch.DisplayIndex.ToString( "X2" );
			ch.Text = "";
			ch.TextAlign = HorizontalAlignment.Center;
			ch.Width = 1;       // пустой элемент
			lstvDump.Columns.Add( ch );

			ch = new ColumnHeader();
			ch.DisplayIndex = 1;
			ch.Name = "clmOffset_10";
			ch.Text = "Смещ 10";
			ch.TextAlign = HorizontalAlignment.Right;
			ch.Width = 70;
			lstvDump.Columns.Add( ch );

			ch = new ColumnHeader();
			ch.DisplayIndex = 2;
			ch.Name = "clmOffset_16";
			ch.Text = "Смещ 16";
			ch.TextAlign = HorizontalAlignment.Right;
			ch.Width = 70;
			lstvDump.Columns.Add( ch );

			int ii;
			for( ii = 0 ; ii < numColumn ; ii++ )
			{
				ch = new ColumnHeader();
				ch.DisplayIndex = ii + 3;
				ch.Name = "clm_" + ch.DisplayIndex.ToString( "X2" );
				ch.Text = ii.ToString( "X2" );
				ch.TextAlign = HorizontalAlignment.Center;
				ch.Width = 30;
				lstvDump.Columns.Add( ch );
			}

			ch = new ColumnHeader();
			ch.DisplayIndex = ii + 1;
			ch.Name = "clm_symbols";
			ch.Text = "Симв. строка";
			ch.TextAlign = HorizontalAlignment.Left;
			ch.Width = 150;
			lstvDump.Columns.Add( ch );

			// формируем адрес в первой колонке - переводим в шестнадцатеричное четырехзначное символьное значение
			// ??

			char chS;
			StringBuilder strB = new StringBuilder();

			for( int i = 0 ; i < lenpack - 6 ; )
			{
				ListViewItem li = new ListViewItem();
				//li.SubItems.Clear();
				li.SubItems.Add( add10.ToString() );
				li.SubItems.Add( add10.ToString( "X4" ) );
				strB.Length = 0;
				int j;
				for( j = 0 ; j < iclm ; j++ )
				{
					li.SubItems.Add( memX[i].ToString( "X2" ) );

					// символьное значение
					try
					{
						chS = Convert.ToChar( arrCh[i] );
					} catch
					{
						MessageBox.Show( "Действие не поддерживается" );
						return;
					}

					if( Char.IsLetterOrDigit( chS ) )
						strB.Append( arrCh[i] );
					else
						strB.Append( "." );
					i++;
					if( i >= lenpack - 6 )
						break;
				}

				li.SubItems.Add( strB.ToString() );

				LinkSetLV( li, false );


				add10 += iclm;
			}

			// ширина listview
			lstvDump.Width = 0;
			for( int i = 0 ; i < lstvDump.Columns.Count ; i++ )
			{
				ch = lstvDump.Columns[i];
				lstvDump.Width += ch.Width;
			}
		}
		private void ReNew( )
		{
			// обновить
			lstvDump.Clear();
			// вывод в ListView данных пакета
			int kl = Convert.ToInt32( cbAvailablePackets.Text );
			object kt = slLocal[kl];
			PacketViewer_Output( ( byte[] ) kt, iclm );
		}
		/*==========================================================================*
			*   private void void LinkSetText(object Value)
			*      для потокобезопасного вызова процедуры
			*==========================================================================*/
		delegate void SetLVCallback( ListViewItem li, bool actDellstV );

		// actDellstV - действия с ListView : false - не трогать; true - очистить;
		public void LinkSetLV( object Value, bool actDellstV )
		{
			if( !( Value is ListViewItem ) && !actDellstV )
				return;   // сгенерировать ошибку через исключение

			ListViewItem li = null;
			if( !actDellstV )
				li = ( ListViewItem ) Value;
			if( this.lstvDump.InvokeRequired )
			{
				if( !actDellstV )
					SetLV( li, actDellstV );
				else
					SetLV( null, actDellstV );
			}
			else
			{
				if( !actDellstV )
					this.lstvDump.Items.Add( li );
				else
					this.lstvDump.Items.Clear();
			}
		}

		private void SetLV( ListViewItem li, bool actDellstV )
		{
			if( this.lstvDump.InvokeRequired )
			{
				SetLVCallback d = new SetLVCallback( SetLV );
				this.Invoke( d, new object[] { li, actDellstV } );
			}
			else
			{
				if( !actDellstV )
					this.lstvDump.Items.Add( li );
				else
					this.lstvDump.Items.Clear();
			}
		}
		private void rbClm16_CheckedChanged( object sender, EventArgs e )
		{
			RadioButton rb = ( RadioButton ) sender;
			if( rb.Checked )
				iclm = Convert.ToUInt16( rb.Tag );  // число колонок

			ReNew();
		}
		private void cbEncode_SelectedIndexChanged( object sender, EventArgs e )
		{
			eii = ( EncodingInfo ) slEncoding[cbEncode.SelectedIndex];  //EncodingInfo
		}

		private void button1_Click( object sender, EventArgs e )
		{
			ReNew();
		}
		private void btnPrint_Click( object sender, EventArgs e )
		{
			PrintHMI frmPrt = new PrintHMI();
			StringBuilder sb = new StringBuilder();
			;
			ListViewItem li;
			// перебираем содержимое всех строк lstvDump
			for( int i = 0 ; i < lstvDump.Items.Count ; i++ )
			{
				sb.Length = 0;
				li = new ListViewItem();
				li = ( ListViewItem ) lstvDump.Items[i];
				for( int j = 0 ; j < li.SubItems.Count ; j++ )
				{
					sb.Append( li.SubItems[j].Text );
					sb.Append( "\t" );
				}
				sb.Append( "\n" );
				frmPrt.rtbText.AppendText( sb.ToString() );
			}
			frmPrt.Show();
		}
		#endregion

		/// <summary>
		/// private void frmSymap_Shown( object sender, EventArgs e )
		/// возникает при первом отображении формы
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmSymap_Shown( object sender, EventArgs e )
		{
			CreateArrayList( arrVar, name_arrVar );

			// размещаем динамически на форме
			for( int i = 0 ; i < arrVar.Count ; i++ )
			{
				FormulaEval ev = ( FormulaEval ) arrVar[i];
				// смотрим категорию вкладки для размещения тега и его тип
				CheckBoxVar chBV;
				ctlLabelTextbox usTB;
				switch( ev.ToP )
				{
					/*case TypeOfPanel.SYMAP_AbsVal:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpAbs;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfPanel.SYMAP_Ident:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpIdent;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfPanel.SYMAP_P:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpP;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfPanel.SYMAP_Status1:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpStatus1;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfPanel.SYMAP_Status2:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpStatus2;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfPanel.SYMAP_Status2_2:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpStatus2_2;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
                    case TypeOfPanel.SYMAP_Status_Bus2:
                        usTB = new ctlLabelTextbox();
                        usTB.LabelText = "";
                        usTB.Parent = this.flpStatus_Bus2;
                        usTB.AutoSize = true;
                        usTB.addrLinkVar = ev.addrVar;
                        usTB.txtLabelText.ReadOnly = false;
                        ev.OnChangeValForm += usTB.LinkSetText;
                        ev.FirstValue();
                        break;
					case TypeOfPanel.SYMAP_TempVal:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpTempVal;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfPanel.SYMAP_Time:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpTime;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfPanel.SYMAP_Twork:
						usTB = new ctlLabelTextbox();
						usTB.LabelText = "";
						usTB.Parent = this.flpTwork;
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.txtLabelText.ReadOnly = false;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;*/
					default:
						break;
				}
			}
		}
		
		/// <summary>
		/// создание массива ArrayList с описанием переменных по содержимому файла XML
		/// </summary>
		/// <param name="arrVar"> массив  ArrayList
		///фигуры</param>
		/// <param name="nameFile">имя файла XML
		///фигуры</param>
		private void CreateArrayList( ArrayList arrVar, string name_arrVar )
		{
			SortedList sl = new SortedList();
			ArrayList alVal = new ArrayList();
			System.Xml.XmlTextReader reader;

			// чтение XML
			if( name_arrVar == "arrStatusFCCommand" )
				reader = new XmlTextReader( nfXMLConfigFC );
			else
				reader = new XmlTextReader( nfXMLConfig );

			reader.WhitespaceHandling = WhitespaceHandling.Significant; // обработка только значимых пробелов

			//вывод отладочный в файл
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
									// формируем элементы формулы
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
											MessageBox.Show( "Нет такого типа сигнала" );
											break;
									}
									switch( ( string ) sl["TypeOfPanel"] )
									{
										case "SYMAP_Status1":
											ToP = TypeOfPanel.SYMAP_Status1;
											break;
										case "SYMAP_Status2":
											ToP = TypeOfPanel.SYMAP_Status2;
											break;
										case "SYMAP_Status2_2":
											ToP = TypeOfPanel.SYMAP_Status2_2;
											break;
                                        case "SYMAP_Status_Bus2":
                                            ToP = TypeOfPanel.SYMAP_Status_Bus2;
											break;                                            
										case "SYMAP_Ident":
											ToP = TypeOfPanel.SYMAP_Ident;
											break;
										case "SYMAP_Time":
											ToP = TypeOfPanel.SYMAP_Time;
											break;
										case "SYMAP_P":
											ToP = TypeOfPanel.SYMAP_P;
											break;
										case "SYMAP_AbsVal":
											ToP = TypeOfPanel.SYMAP_AbsVal;
											break;
										case "SYMAP_TempVal":
											ToP = TypeOfPanel.SYMAP_TempVal;
											break;
										case "SYMAP_AllSymap":
											ToP = TypeOfPanel.SYMAP_AllSymap;
											break;
										case "SYMAP_Twork":
											ToP = TypeOfPanel.SYMAP_Twork;
											break;

										case "no":
											ToP = TypeOfPanel.no;
											break;

										default:
											MessageBox.Show( "Нет панели такого типа " );
											break;
									}
									// читаем теги
									alVal.Clear();
									while( reader.Read() )
										if( reader.Name.Equals( "value" ) )
											alVal.Add( reader.GetAttribute( 0 ) );
										else
											break;
									//if( alVal.Count == 2 )
									//   arrVar.Add( new FormulaEval( parent.KB, "0(" + strFC + "." + strIDDev + ( string ) alVal[0] + ")1(" + strFC + "." + strIDDev + ( string ) alVal[1] + ")", sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP ) );
									//else
									//   arrVar.Add( new FormulaEval( parent.KB, "0(" + strFC + "." + strIDDev + ( string ) alVal[0] + ")", sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP ) );
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
								{   // запоминем названия вкладок в уставках и конфигурации
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
	}
}