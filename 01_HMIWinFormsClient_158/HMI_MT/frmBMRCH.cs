/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Форма для работы с блоком БМРЗ ВВ-14-31-12.                                                           
 *                                                                             
 *	Файл                     : frmBMRCH.cs                                         
 *	Тип конечного файла      : 
 *	версия ПО для разработки : С#, Framework 2.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : xx.04.2007                                       
 *	Дата (v1.0)              :                                                  
 *******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
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

namespace HMI_MT
{
	public partial class frmBMRCH : Form
   {
      #region переменные класса
      private MainForm parent;
      int iFC;            // номер ФК целочисленный
      string strFC;       // номер ФК строка
      int iIDDev;         // номер устройства целочисленный
      string strIDDev;    // номер устройства строка
      int inumLoc;         // номер ячейки целочисленный
      string strnumLoc;    // номер ячейки строка
      string nfXMLConfig; // имя файла с описанием 
      string nfXMLConfigFC; // имя файла с описанием ЩАСУ
      //// массив дополнительных панелей
      ArrayList arDopPanel;

      ArrayList arrAvarSign = new ArrayList();
      ArrayList arrCurSign = new ArrayList();
      ArrayList arrSystemSign = new ArrayList();
      ArrayList arrStoreSign = new ArrayList();
      ArrayList arrConfigSign = new ArrayList();
      ArrayList arrStatusDevCommand = new ArrayList();
		ArrayList arrStatusFCCommand = new ArrayList();

		DataTable dtA;  // таблица с авариями
		DataTable dtU;  // таблица с уставками

      ushort iclm = 16;  // число колонок в дампе
      SortedList slLocal;
      EncodingInfo eii;
      SortedList slEncoding;
      SortedList se = new SortedList();
      SortedList sl_tpnameUst = new SortedList();
      StringBuilder sbse = new StringBuilder();

      DataTable dtO;  // таблица с осциллограммами
      DataTable dtG;  // таблица с диаграммами

      SortedList slFLP = new SortedList();	// для хранения инф о FlowLayoutPanel
		SortedList slFLPUst = new SortedList();	// отдельно для уставок
      FormulaEval b_62002;
      FormulaEval b_62092;
      ErrorProvider erp = new ErrorProvider( );
      // массив индикаторов
      ArrayList arrIndicators = new ArrayList( );
      bool firstShow = false;	// для первого показа формы, исключить гашение таймера
      #endregion

      #region конструктор
      public frmBMRCH( )
		{
			InitializeComponent();
		}
        public frmBMRCH(MainForm linkMainForm, int iFC, int iIDDev,  string fXML)//int inumLoc,
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

            //Text += " ( " + strIDDev + " ) - яч. № " + strnumLoc;

            // формируем массив панелей и скрываем их
            arDopPanel = new ArrayList();
				//arDopPanel.Add(this.pnlCurrent);
				//arDopPanel.Add( this.pnlAvar );
				//arDopPanel.Add( this.pnlSystem );
				//arDopPanel.Add( this.pnlStore );
				//arDopPanel.Add( this.pnlConfig );
				//arDopPanel.Add( this.pnlOscDiag );
				//arDopPanel.Add( this.pnlStatusSHASU );

				foreach( Panel p in arDopPanel )
					 p.Visible = false;
		}
        /// <summary>
        /// загрузка формы
        /// </summary>
        private void frmBMRCH_Load( object sender, EventArgs e )
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
					  TestCCforFLP( slFLP, cc[i] );
				  }
			  }

				//this.Width = parent.Width;
				//this.Height = parent.Height;
				//tabControl1.Width = this.Width - 10;
				//tabControl1.Height = parent.ClientSize.Height - parent.statusStrip1.Height - parent.panelMes.Height - parent.menuStrip1.Height - parent.tabForms.Height - 10;
				//tabControl1.Height = parent.ClientSize.Height - parent.statusStrip1.Height - parent.panelMes.Height - pnlCurrent.Height - parent.menuStrip1.Height - parent.tabForms.Height - 10;
				
				 // устанавливаем пикеры для вывода аварийной информации за последние сутки
				 dtpEndDateAvar.Value = DateTime.Now;
				 dtpEndTimeAvar.Value = DateTime.Now;;
				 dtpStartDateAvar.Value = DateTime.Now;;

				 TimeSpan ts = new TimeSpan( 1, 0, 0, 0 );
				 dtpStartDateAvar.Value = dtpStartDateAvar.Value - ts;
				 dtpStartTimeAvar.Value = DateTime.Now;

				 // устанавливаем пикеры для изменения уставок за последние сутки
				 dtpEndDateConfig.Value = DateTime.Now;
				 dtpEndTimeConfig.Value = DateTime.Now;
				 dtpStartDateConfig.Value = DateTime.Now; 

				 ts = new TimeSpan( 1, 0, 0, 0 );
				 dtpStartDateConfig.Value = dtpStartDateConfig.Value - ts;
				 dtpStartTimeConfig.Value = DateTime.Now;

				 // устанавливаем пикеры для изменения набора осциллограмм и диаграмм за последние сутки
				dtpEndData.Value = DateTime.Now;
				 dtpEndTime.Value = DateTime.Now;
				 dtpStartData.Value = DateTime.Now;

				 ts = new TimeSpan( 1, 0, 0, 0 );
				 dtpStartData.Value = dtpStartData.Value - ts;
				 dtpStartTime.Value = DateTime.Now;		  
        }
        #endregion
      /// <summary>
		  /// создание массива ArrayList с описанием переменных по содержимому файла XML
		  /// </summary>
		  /// <param Name="arrVar"> массив  ArrayList
		  ///фигуры</param>
		  /// <param Name="nameFile">имя файла XML
		  ///фигуры</param>
      private void CreateArrayList( ArrayList arrVar, string name_arrVar )
		  {
			  SortedList sl = new SortedList();
			  ArrayList alVal = new ArrayList();
			  System.Xml.XmlTextReader reader;

			  // чтение XML
			  if( nfXMLConfig == null )
				  return;

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
											  MessageBox.Show( "Нет такого типа сигнала" );
											  break;
									  }
									  ToP = (string) sl["TypeOfPanel"];

									  // читаем теги
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
									  sbse.Append( reader.GetAttribute( "Name" ) );
									  reader.Read();
									  se[sbse.ToString()] = reader.GetAttribute( 0 );
									  reader.Read();
								  }
								  else if( reader.Name.Equals( "name_tabpage_ust" ) )
								  {   // запоминем названия вкладок в уставках и конфигурации
									  sbse.Length = 0;
									  sbse.Append( reader.GetAttribute( "Name" ) );
									  reader.Read();
									  for( int i = 0 ; i < tbcConfig.Controls.Count ; i++ )
									  {
										  if( tbcConfig.Controls[i] is TabPage && tbcConfig.Controls[i].Name == sbse.ToString() )
										  {
											  tbcConfig.Controls[i].Text = reader.GetAttribute( 0 );
											  sl_tpnameUst[sbse.ToString()] = tbcConfig.Controls[i];
										  }
									  }
									  reader.Read();
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
              System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMMRCH : исключение : " + ee.Message );
				  sw.Close();
				  fs.Close();
			  }
			  sw.Close();
			  fs.Close();

			  reader.Close();
			  if (HMI_Settings.ClientDFE != null)
                  foreach (FormulaEval fe in arrVar)
					  HMI_Settings.ClientDFE.AddArrTags(this.Text, fe);
		  }
		private void TestCCforFLP( SortedList slF, Control cc )
		{
			for( int i = 0 ; i < cc.Controls.Count ; i++ )
			{
				if( cc.Controls[i] is FlowLayoutPanel )
				{
					FlowLayoutPanel flp = ( FlowLayoutPanel ) cc.Controls[i];
					slF[flp.Name] = flp;
				}
				else
				{
					TestCCforFLP( slF, cc.Controls[i] );
				}
			}
		}
      #region контрольная печать DataSet
        static void PrintDataSet( DataSet ds )
        {
            // метод выполняет цикл по всем DataTable данного DataSet
            Console.WriteLine( "Таблицы в DataSet '{0}'. \n ", ds.DataSetName );
            foreach( DataTable dt in ds.Tables )
            {
                Console.WriteLine( "Таблица '{0}'. \n ", dt.TableName );
                // вывод имен столбцов
                for( int curCol = 0; curCol < dt.Columns.Count; curCol++ )
                    Console.Write( dt.Columns[curCol].ColumnName.Trim() + "\t" );
                Console.WriteLine( "\n-----------------------------------------------" );

                // вывод DataTable
                for( int curRow = 0; curRow < dt.Rows.Count; curRow++ )
                {
                    for( int curCol = 0; curCol < dt.Columns.Count; curCol++ )
                        Console.Write( dt.Rows[curRow][curCol].ToString() + "\t" );
                    Console.WriteLine();
                }
            }
        }
        #endregion

      #region установить всем переменным группы качество архивных переменных (из БД)
         /// <summary>
         /// SetArhivGroupInDev(dev, 8)
         /// установить всем переменным группы качество архивных переменных (из БД)
         /// </summary>
         /// <param Name="dev">устройство</param>
         /// <param Name="idGroup">группа</param>
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
      
      #region вывод пакета pack в hex-виде в файд fn
      private void PrintHexDump( string fn , byte[] pack)
        {
            // выведем в файл - текущий каталог
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
                // начинаем строку счетчиком адреса
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
                        catch (Exception ee)
                        {
                           System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMMRCH : исключение : " + ee.Message );
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
      #region вход на вкладку с информацией о состоянии устройства и команд
        private void tabStatusDev_Command_Enter( object sender, EventArgs e )
        {
				//// скрываем панели
				//foreach( Panel p in arDopPanel )
				//    p.Visible = false;

            if( arrStatusDevCommand.Count != 0 )
                return;

            CreateArrayList(arrStatusDevCommand, "arrStatusDevCommand");

            // размещаем динамически на форме
            for( int i = 0; i < arrStatusDevCommand.Count; i++ )
            {
                FormulaEval ev = ( FormulaEval )arrStatusDevCommand[ i ];
                // смотрим категорию вкладки для размещения тега и его тип
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
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;
							 ev.OnChangeValForm += chBV.LinkSetText;
							 ev.FirstValue();
							 break;
						 case TypeOfTag.no:
							 break;
						 default:
							 MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
							 break;
					 }					 
            }
        }
        #endregion

      #region вход на вкладку для просмотра доступных пакетов
      //  private void tbpPacketViewer_Enter( object sender, EventArgs e )
      //  {
      //      // выбор кодировки
      //      slEncoding = new SortedList();
      //      int ii = 0;
      //      foreach (EncodingInfo ei in Encoding.GetEncodings())
      //      {
      //          slEncoding[ii] = ei;
      //          cbEncode.Items.Add( "[" + ei.CodePage.ToString() + "]" + " : " + ei.DisplayName   );
      //          if( ei.CodePage == 866)
      //              cbEncode.SelectedIndex = ii;    // кодировка по умолчанию
      //          ii++;
      //      }
      //      eii = ( EncodingInfo ) slEncoding[cbEncode.SelectedIndex];  //EncodingInfo

      //      slLocal = new SortedList();
      //      // найдем SortedList для нужного устройства
      //      foreach( FC aFC in parent.KB )
      //          if( aFC.NumFC == iFC )
      //          {
      //              foreach( TCRZADirectDevice aDev in aFC )
      //                  if( aDev.NumDev == iIDDev )
      //                  {
      //                      slLocal = aDev.CRZAMemDev;
      //                      break;
      //                  }
      //              break;
      //          }
      //      // заполняем ComboBox
      //      cbAvailablePackets.Items.Clear();
      //      for( int i = 0; i < slLocal.Count; i++ )
      //          cbAvailablePackets.Items.Add( slLocal.GetKey( i ) );
      //      try 
      //      {
      //          cbAvailablePackets.SelectedIndex = 0;
      //      }
      //      catch(Exception eee)
      //      {
      //         MessageBox.Show( "Нет данных для отображения. " + eee.Message );
      //      }
      //  }
      //  private void cbAvailablePackets_SelectedIndexChanged( object sender, EventArgs e )
      //  {
      //      ReNew();
      //  }
      //private void PacketViewer_Output(byte[]  brP, ushort numColumn )//BinaryReader
      //  {
      //      int lenpack = BitConverter.ToInt16( brP, 0 );

      //      short numdev = BitConverter.ToInt16( brP, 2 );

      //      ushort add10 = BitConverter.ToUInt16( brP, 4 );	//читаем адрес блока данных
            
      //      // читаем данные в массив 
      //      byte[] memX = new byte[brP.Length - 6];
      //      System.Buffer.BlockCopy( brP, 6, memX, 0, brP.Length - 6 );

      //      Encoding e = Encoding.ASCII;
      //      try
      //      {
      //         e = eii.GetEncoding();
      //      }
      //      catch
      //      {
      //          MessageBox.Show("Ошибка при выборе кодировки");
      //      }
            
      //      char[] arrCh = new char[e.GetCharCount( memX, 0, memX.Length )];
      //      e.GetChars( memX, 0, memX.Length, arrCh, 0 );
            
      //      // формируем ListView

      //      ColumnHeader ch = new ColumnHeader();
      //      ch.DisplayIndex = 0;
      //      ch.Name = "clm_" + ch.DisplayIndex.ToString( "X2" );
      //      ch.Text = "";
      //      ch.TextAlign = HorizontalAlignment.Center;
      //      ch.Width = 1;       // пустой элемент
      //      lstvDump.Columns.Add( ch );

      //      ch = new ColumnHeader();
      //      ch.DisplayIndex = 1;
      //      ch.Name = "clmOffset_10";
      //      ch.Text = "Смещ 10";
      //      ch.TextAlign = HorizontalAlignment.Right;
      //      ch.Width = 70;
      //      lstvDump.Columns.Add( ch );

      //      ch = new ColumnHeader();
      //      ch.DisplayIndex = 2;
      //      ch.Name = "clmOffset_16";
      //      ch.Text = "Смещ 16";
      //      ch.TextAlign = HorizontalAlignment.Right;
      //      ch.Width = 70;
      //      lstvDump.Columns.Add( ch );

      //      int ii;
      //      for(ii = 0; ii < numColumn; ii++ ) 
      //      {
      //          ch = new ColumnHeader();
      //          ch.DisplayIndex = ii + 3;
      //          ch.Name = "clm_" + ch.DisplayIndex.ToString("X2");
      //          ch.Text = ii.ToString( "X2" );
      //          ch.TextAlign = HorizontalAlignment.Center;
      //          ch.Width = 30;
      //          lstvDump.Columns.Add( ch );
      //      }

      //      ch = new ColumnHeader();
      //      ch.DisplayIndex = ii + 1;
      //      ch.Name = "clm_symbols";
      //      ch.Text = "Симв. строка";
      //      ch.TextAlign = HorizontalAlignment.Left;
      //      ch.Width = 150;
      //      lstvDump.Columns.Add( ch );
            
      //      // формируем адрес в первой колонке - переводим в шестнадцатеричное четырехзначное символьное значение
      //      // ??
            
      //      char chS;
      //      StringBuilder strB = new StringBuilder();

      //      for (int i = 0; i < lenpack - 6;)
      //      {
      //          ListViewItem li = new ListViewItem();
      //          //li.SubItems.Clear();
      //          li.SubItems.Add( add10.ToString() );
      //          li.SubItems.Add( add10.ToString( "X4" ) );
      //          strB.Length  = 0;
      //          int j;
      //          for( j = 0; j < iclm; j++ )
      //          {
      //              li.SubItems.Add( memX[i].ToString("X2") );

      //              // символьное значение
      //              try 
      //              {
      //                  chS = Convert.ToChar( arrCh[i] );
      //              }catch
      //              {
      //                  MessageBox.Show("Действие не поддерживается");
      //                  return;
      //              }

      //              if( Char.IsLetterOrDigit( chS ) )
      //                  strB.Append( arrCh[i] );
      //              else
      //                  strB.Append( "." );
      //              i++;
      //              if (i >= lenpack - 6)
      //                  break;                
      //          }

      //          li.SubItems.Add( strB.ToString() );

      //          LinkSetLV( li, false );


      //          add10 += iclm;
      //      }

      //      // ширина listview
      //      lstvDump.Width = 0;
      //      for( int i = 0; i < lstvDump.Columns.Count; i++ )
      //      {
      //          ch = lstvDump.Columns[i];
      //          lstvDump.Width += ch.Width;
      //      }
      //  }
        #endregion
      /*==========================================================================*
          *   private void void LinkSetText(object Value)
          *      для потокобезопасного вызова процедуры
          *==========================================================================*/
      delegate void SetLVCallback( ListViewItem li, bool actDellstV );

      // actDellstV - действия с ListView : false - не трогать; true - очистить;
      public void LinkSetLV( object Value, bool actDellstV )
        {
        //    if( !( Value is ListViewItem ) && !actDellstV )
        //        return;   // сгенерировать ошибку через исключение

        //    ListViewItem li = null;
        //    if( !actDellstV )
        //        li = ( ListViewItem ) Value;
        //    if( this.lstvDump.InvokeRequired )
        //    {
        //        if( !actDellstV )
        //            SetLV( li, actDellstV );
        //        else
        //            SetLV( null, actDellstV );
        //    }
        //    else
        //    {
        //        if( !actDellstV )
        //            this.lstvDump.Items.Add( li );
        //        else
        //            this.lstvDump.Items.Clear();
        //    }
        }

      /*==========================================================================*
        * private void SetText(ListViewItem li)
        * //для потокобезопасного вызова процедуры
        *==========================================================================*/
      private void SetLV( ListViewItem li, bool actDellstV )
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

      private void rbClm16_CheckedChanged( object sender, EventArgs e )
        {
            RadioButton rb = ( RadioButton ) sender;
            if( rb.Checked )
                iclm = Convert.ToUInt16( rb.Tag );  // число колонок

            ReNew();
        }

      private void cbEncode_SelectedIndexChanged( object sender, EventArgs e )
        {
            //eii = ( EncodingInfo ) slEncoding[cbEncode.SelectedIndex];  //EncodingInfo
        }

      private void button1_Click( object sender, EventArgs e )
        {
            ReNew(); 
        }
      private void ReNew( ) 
        {
            //// обновить
            //lstvDump.Clear();
            //// вывод в ListView данных пакета
            //int kl = Convert.ToInt32( cbAvailablePackets.Text );
            //object kt = slLocal[kl];
            ////PacketViewer_Output( ( BinaryReader ) kt, iclm );
            //PacketViewer_Output( ( byte[] ) kt, iclm );
        }

      private void btnPrint_Click( object sender, EventArgs e )
        {
            //PrintHMI frmPrt = new PrintHMI();
            //StringBuilder sb = new StringBuilder(); ;
            //ListViewItem li;
            //// перебираем содержимое всех строк lstvDump
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

      private void mnuPageSetup_Click( object sender, EventArgs e )
        {
            parent.mnuPageSetup_Click( sender, e );
        }
      /// <summary>
        /// mnuPrint_Click( object sender, EventArgs e )
        ///     Реализация пункта меню Предварительный просмотр
        /// </summary>
      private void mnuPrintPreview_Click( object sender, EventArgs e )
        {
            PrintArr();
            parent.mnuPrintPreview_Click( sender, e );
        }
      /// <summary>
        /// mnuPrint_Click( object sender, EventArgs e )
        ///     Реализация пункта меню Печать
        /// </summary>
      private void mnuPrint_Click( object sender, EventArgs e )
        {
            PrintArr();
            parent.mnuPrint_Click( sender, e );
        }
      /// <summary>
        /// PrintArr()
        ///     Печать массива переменных
        /// </summary>
      private void PrintArr()
        {
            StringBuilder sb = new StringBuilder();
            float f_val;
            int i_val;
            string t_val = "";
            ArrayList arCurPrt = new ArrayList();

            object val;

            // определяем активную вкладку
            TabPage tp_sel = tabControl1.SelectedTab;

            sb.Length = 0;
            
            switch(tp_sel.Text)
            {
                case "Текущая информация":
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (Текущая информация)" );
                    sb.Append( "\n========================================================================\n" );
                    sb.Append( " \n \n " );
                    arCurPrt = arrCurSign;
                    break;
                case "Информация об авариях":
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (Информация об авариях)" );
                    sb.Append( "\n========================================================================\n" );
                    
                    sb.Append( " \n \n " );
                    arCurPrt = arrAvarSign;
                    break;
                case "Накопительная информация":
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (Накопительная информация)" );
                    sb.Append( "\n========================================================================\n" );
                    
                    sb.Append( " \n \n " );
                    arCurPrt = arrStoreSign;
                    break;
                case "Конфигурация и уставки":
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (Конфигурация и уставки)" );
                    sb.Append( "\n========================================================================\n" );
                    
                    sb.Append( " \n \n " );
                    arCurPrt = arrConfigSign;
                    break;
                case "Система":
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (Система)" );
                    sb.Append( "\n========================================================================\n" );
                    
                    sb.Append( " \n \n " );
                    arCurPrt = arrSystemSign;
                    break;
                case "Состояние устройства и команд":
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( this.Text + " (Состояние устройства и команд)" );
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
                            t_val = f_val.ToString( "F2" ); // две цифры после запятой
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
        ///     формирование строки для печати для отдельной переменной
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
                        t_val = f_val.ToString( "F2" ); // две цифры после запятой
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
                        t_val = f_val.ToString( "F2" ); // две цифры после запятой
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

		#region вход на вкладку с информацией о состоянии ФК и его команд
		/// <summary>
		/// вход на вкладку с состоянием ФК и состоянием исполняемых им команд
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void tabStatusFC_Enter( object sender, EventArgs e )
		{
			//System.Xml.XmlTextReader reader;

			// скрываем/показываем нужную панель
			//foreach( Panel p in arDopPanel )
			//   p.Visible = false;
			//this.pnlStatusSHASU.Visible = true;
			//pnlStatusSHASU.Left = tabControl1.Left + 5;
			//pnlStatusSHASU.Top = tabControl1.Top + tabControl1.Height + 5;
			//pnlStatusSHASU.Width = tabControl1.Width - 10;

			if( arrStatusFCCommand.Count != 0 )
				return;

			nfXMLConfigFC = "frm_tbpFC.xml";

			// теперь очень безобразный код, но только для этого проекта где ФК единственный - подменяем номер устройства
			// или нужно делать отдельную вкладку для ФК

			string old_strIDDev = strIDDev;
			strIDDev = "0";

			CreateArrayList( arrStatusFCCommand, "arrStatusFCCommand" );

			//this.tbIntervalReadStore.Enabled = false;
			//this.tbIntervalReadMaxMeter.Enabled = false;
			
			// размещаем динамически на форме
			for( int i = 0 ; i < arrStatusFCCommand.Count ; i++ )
			{
				FormulaEval ev = ( FormulaEval ) arrStatusFCCommand[i];
				// смотрим категорию вкладки для размещения тега и его тип
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
						MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
						break;
				}
			}
			strIDDev = old_strIDDev;
		}
		#endregion
		#region вход на вкладку с системной информацией
		/// <summary>
		/// вход на вкладку с системной информацией
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void tabSystem_Enter( object sender, EventArgs e )
		{
			if( arrSystemSign.Count != 0 )
				return;
			//-------------------------------------------------------------------
			CreateArrayList( arrSystemSign, "arrSystemSign" );

			// размещаем динамически на форме
			for( int i = 0 ; i < arrSystemSign.Count ; i++ )
			{
				FormulaEval ev = ( FormulaEval ) arrSystemSign[i];
				// смотрим категорию вкладки для размещения тега и его тип
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
						MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
						break;
				}
			}
		}
		#endregion
		#region вход на вкладку с текущей информацией 
		/// <summary>
		/// вход на вкладку с текущей информацией 
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void tabCurrent_Enter( object sender, EventArgs e )
		{
         timerInd.Start( );

			if( arrCurSign.Count != 0 )
				return;
			CreateArrayList( arrCurSign, "arrCurSign" );

			// размещаем динамически на форме
			for( int i = 0 ; i < arrCurSign.Count ; i++ )
			{
				FormulaEval ev = ( FormulaEval ) arrCurSign[i];
				// смотрим категорию вкладки для размещения тега и его тип
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
						MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
						break;
				}
			}
         // привязываем индикаторы
         IndBind( );
		}
		#endregion
		#region Вход на вкладку с уставками
		private void tabConfig_Enter( object sender, EventArgs e )
		{
			lstvConfig.Items.Clear();
			//btnWriteUst.Enabled = false;
			UstavBD();
			//-------------------------------------------------------------------
			//готовим инф. для отображения аналоговых и дискретных сигналов
			if( arrConfigSign.Count != 0 )
				return;

			CreateArrayList( arrConfigSign, "arrConfigSign" );

			Control.ControlCollection ccu;
			ccu = ( Control.ControlCollection ) tbcConfig.Controls;	// 
			for( int i = 0 ; i < ccu.Count ; i++ )
			{
				if( ccu[i] is FlowLayoutPanel )
				{
					FlowLayoutPanel flp = ( FlowLayoutPanel ) ccu[i];
					slFLPUst[flp.Name] = flp;
				}
				else
				{
					TestCCforFLP( slFLPUst, ccu[i] );
				}
			}

			// размещаем динамически на форме
			for( int i = 0 ; i < arrConfigSign.Count ; i++ )
			{
				FormulaEval ev = ( FormulaEval ) arrConfigSign[i];
				// смотрим категорию вкладки для размещения тега и его тип
				CheckBoxVar chBV;
				ctlLabelTextbox usTB;
				ComboBoxVar cBV;
				switch( ev.ToT )
				{
					case TypeOfTag.Combo:
						cBV = new ComboBoxVar( ( string[] ) ( ( TagEval ) ( ( TagVal ) ev.arrTagVal[0] ).linkTagEval ).arrStrCB.ToArray( typeof( string ) ), 0 );
						cBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
						cBV.AutoSize = true;
						cBV.addrLinkVar = ev.addrVar;
						cBV.isChange = false;
						ev.OnChangeValForm += cBV.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfTag.Analog:
						usTB = new ctlLabelTextbox();
						usTB.lblCaption.Text = "";
						usTB.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
						usTB.AutoSize = true;
						usTB.addrLinkVar = ev.addrVar;
						usTB.isChange = false;
						usTB.txtLabelText.ReadOnly = false;
						ev.StrFormat = HMI_Settings.Precision;
						ev.OnChangeValForm += usTB.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfTag.Discret:
						chBV = new CheckBoxVar();
						chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
						chBV.AutoSize = true;
						chBV.addrLinkVar = ev.addrVar;
						chBV.isChange = false;
						chBV.addrLinkVarBitMask = ev.addrVarBitMask;
						chBV.btnCheck.Visible = false;
						ev.OnChangeValForm += chBV.LinkSetText;
						ev.FirstValue();
						break;
					default:
						MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
						break;
				}
			}
		}
		private void UstavBD( )
		{
			//dgvAvar.Rows.Clear();
			// получение строк соединения и поставщика данных из файла *.config
			//string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
			SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
			try
			{
				asqlconnect.Open();
			} catch( SqlException ex )
			{
				string errorMes = "";
				// интеграция всех возвращаемых ошибок
				foreach( SqlError connectError in ex.Errors )
					errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ")" + Environment.NewLine;
				parent.WriteEventToLog(21, "Нет связи с БД (UstavBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
            System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMMRCH : Нет связи с БД (UstavBD)"  );
				asqlconnect.Close();
				return;
			} catch( Exception ex )
			{
				MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );
				asqlconnect.Close();
				return;
			}
			// формирование данных для вызова хранимой процедуры
			SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
			cmd.CommandType = CommandType.StoredProcedure;

			// входные параметры
			// 1. ip FC
			SqlParameter pipFC = new SqlParameter();
			pipFC.ParameterName = "@IP";
			pipFC.SqlDbType = SqlDbType.BigInt;
			pipFC.Value = 0;
			pipFC.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( pipFC );
			// 2. id устройства
			SqlParameter pidBlock = new SqlParameter();
			pidBlock.ParameterName = "@id";
			pidBlock.SqlDbType = SqlDbType.Int;
			pidBlock.Value = iIDDev;
			pidBlock.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( pidBlock );

			// 3. начальное время
			SqlParameter dtMim = new SqlParameter();
			dtMim.ParameterName = "@dt_start";
			dtMim.SqlDbType = SqlDbType.DateTime;
			TimeSpan tss = new TimeSpan( 0, dtpStartDateConfig.Value.Hour - dtpStartTimeConfig.Value.Hour, dtpStartDateConfig.Value.Minute - dtpStartTimeConfig.Value.Minute, dtpStartDateConfig.Value.Second - dtpStartTimeConfig.Value.Second );
			DateTime tim = dtpStartDateConfig.Value - tss;
			dtMim.Value = tim;
			dtMim.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( dtMim );

			// 2. конечное время
			SqlParameter dtMax = new SqlParameter();
			dtMax.ParameterName = "@dt_end";
			dtMax.SqlDbType = SqlDbType.DateTime;
			tss = new TimeSpan( 0, dtpEndDateConfig.Value.Hour - dtpEndTimeConfig.Value.Hour, dtpEndDateConfig.Value.Minute - dtpEndTimeConfig.Value.Minute, dtpEndDateConfig.Value.Second - dtpEndTimeConfig.Value.Second );
			tim = dtpEndDateConfig.Value - tss;
			dtMax.Value = tim;
			dtMax.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( dtMax );

			// 5. тип записи
			SqlParameter ptypeRec = new SqlParameter();
			ptypeRec.ParameterName = "@type";
			ptypeRec.SqlDbType = SqlDbType.Int;
			ptypeRec.Value = 1; // информация по уставкам
			ptypeRec.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( ptypeRec );
			// 6. ид записи журнала
			SqlParameter pid = new SqlParameter();
			pid.ParameterName = "@id_record";
			pid.SqlDbType = SqlDbType.Int;
			pid.Value = 0;
			pid.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( pid );

			// заполнение DataSet
			DataSet aDS = new DataSet( "ptk" );
			SqlDataAdapter aSDA = new SqlDataAdapter();
			aSDA.SelectCommand = cmd;

			//aSDA.sq
			aSDA.Fill( aDS, "TbUstav" );

			asqlconnect.Close();

			//PrintDataSet( aDS );
			// извлекаем данные по уставкам
			dtU = aDS.Tables["TbUstav"];

			// заполняем ListView
			lstvConfig.Items.Clear();
			for( int curRow = 0 ; curRow < dtU.Rows.Count ; curRow++ )
			{
				DateTime t = ( DateTime ) dtU.Rows[curRow]["TimeBlock"];
				ListViewItem li = new ListViewItem( t.ToShortDateString() );
				li.SubItems.Add( t.ToLongTimeString() + ":" + t.Millisecond );
				li.Tag = dtU.Rows[curRow]["ID"];
				lstvConfig.Items.Add( li );
			}
			aSDA.Dispose();
			aDS.Dispose();
		}
		#endregion

		private void btnResetValues_Click( object sender, EventArgs e )
		{
			btnWriteUst.Enabled = false;
			parent.newKB.ResetGroup( iFC, iIDDev, 14 );
		}

		private void btnReadUstBlock_Click( object sender, EventArgs e )
		{
			btnWriteUst.Enabled = true;

         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RCP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "Команда \"RCP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );

			// документирование действия пользователя
			parent.WriteEventToLog(7, iIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда RCP - чтения уставок."
			if( b_62002 != null )
				b_62002.FirstValue();
			if( b_62092 != null )
				b_62092.FirstValue();
		}

		private void btnReadUstFC_Click( object sender, EventArgs e )
		{
			btnWriteUst.Enabled = true;

         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "IMP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "Команда \"IMP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );

			// документирование действия пользователя
			parent.WriteEventToLog(7, iIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда IMP - чтения уставок."
			if( b_62002 != null )
				b_62002.FirstValue();
			if( b_62092 != null )
				b_62092.FirstValue();
		}

		private void btnWriteUst_Click( object sender, EventArgs e )
		{
			if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, parent.UserRight ) )
				return;

			if( parent.isReqPassword )
				if( !parent.CanAction() )
				{
					MessageBox.Show( "Выполнение действия запрещено" );
					return;
				}

			DialogResult dr = MessageBox.Show( "Записать уставки?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
			if( dr == DialogResult.No )
				return;

			string stri;
			TabPage tp;
			ctlLabelTextbox ultb;
			CheckBoxVar chbTmp;
			ComboBoxVar cbTmp;

			FlowLayoutPanel flp;
			bool isUstChange = false;   // факт изменения уставок для последующего формирования команды
			StringBuilder sb = new StringBuilder();
			uint ainmemX;    // адрес в массиве memX
			byte[] aTmp2 = new byte[2];

			//формируем sortedlist для фиксации изменений в группах АПМ
			SortedList slAPMCh = new SortedList();
			int baseUst = 62000;
			int baseUsti = baseUst;
			for( int i = 0 ; i < 8 ; i++ )
			{
				slAPMCh[baseUsti] = false;
				baseUsti = baseUsti + 47;	// 47 регистров = 90 байт - длина пакета уставок + 4 байта - время или пусто
			}

			// найдем SortedList для нужного устройства
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
			// извлекаем пакет с уставками для корректировки
			//BinaryReader memDevBlock = ( BinaryReader ) slLocal[62000];

			// читаем данные в массив 
			//byte[] memXt = new byte[( ( byte[] ) slLocal[62000] ).Length];
			//System.Buffer.BlockCopy( brP, 6, memX, 0, brP.Length - 6 );

			// читаем данные в массив 
			//memDevBlock.BaseStream.Position = 0;
			int lenpack = 0;
			try
			{
				lenpack = BitConverter.ToInt16( ( byte[] ) slLocal[62000], 0 );
			} catch( ArgumentNullException ex )
			{
				MessageBox.Show( "Нет данных для записи. \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			short numdev = BitConverter.ToInt16( ( byte[] ) slLocal[62000], 2 );

			ushort add10 = BitConverter.ToUInt16( ( byte[] ) slLocal[62000], 4 );	//читаем адрес блока данных

			//int lenpack = ( short ) memDevBlock.ReadInt16();
			//short numdev = ( short ) memDevBlock.ReadUInt16();
			//ushort add10 = ( ushort ) memDevBlock.ReadInt16();	//читаем адрес блока данных

			byte[] memX = new byte[lenpack - 6];
			System.Buffer.BlockCopy( ( byte[] ) slLocal[62000], 6, memX, 0, ( ( byte[] ) slLocal[62000] ).Length - 6 );

				for( int j = 0 ; j < slFLPUst.Count ; j++ )
				{
					flp = ( FlowLayoutPanel ) slFLPUst.GetByIndex(j);
					for( int n = 0 ; n < flp.Controls.Count ; n++ )
					{
						if( flp.Controls[n] is ctlLabelTextbox )
						{
							ultb = ( ctlLabelTextbox ) flp.Controls[n];
							if( ultb.isChange )
							{
								CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 );
								//StrToBCD_Field( ultb, memX );
								isUstChange = true;
								// для какого АПМ посылать
								TestAPMUst( slAPMCh, ultb.addrLinkVar );
							}
						}
						else if( flp.Controls[n] is ComboBoxVar )
						{
							cbTmp = ( ComboBoxVar ) flp.Controls[n];
							if( cbTmp.isChange )
							{
								isUstChange = true;
								cbTmp.isChange = false;  // сбрасываем признак изменения у конкретного ComboBoxVar'а
								// записываем изменения по ComboBoxVar'ам в исходный пакет (корректируем массив memX)
								uint a = cbTmp.addrLinkVar; // адрес переменной
								// получим значение
								int st = cbTmp.cbVar.SelectedIndex;
								byte[] bst = new byte[4];
								bst = BitConverter.GetBytes( st );
								Buffer.BlockCopy( bst, 0, aTmp2, 0, 2 );
								Array.Reverse( aTmp2 );
								// запоминаем изменения
								ainmemX = ( a - 62000 ) * 2;
								Buffer.BlockCopy( aTmp2, 0, memX, ( int ) ainmemX, 2 );
								// для какого АПМ посылать
								TestAPMUst( slAPMCh, cbTmp.addrLinkVar );
							}
						}
						else if( flp.Controls[n] is CheckBoxVar )
						{
							chbTmp = ( CheckBoxVar ) flp.Controls[n];
							if( chbTmp.isChange )
							{
								isUstChange = true;
								chbTmp.isChange = false;    // сбрасываем признак изменения у конкретного CheckBoxVar'а
								// извлечем битовое поле из исходного массива
								ainmemX = ( chbTmp.addrLinkVar - 62000 ) * 2;   // это адрес
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
								// записать на место
								aTmp2 = BitConverter.GetBytes( rezbit );
								Array.Reverse( aTmp2 );
								Buffer.BlockCopy( aTmp2, 0, memX, ( int ) ainmemX, 2 );
								// для какого АПМ посылать
								TestAPMUst( slAPMCh, chbTmp.addrLinkVar );
							}
						}
					}
				}
			//      }
			//   }
			//}
			//------------------------------
			// аналогично для панели уставок
			//for( int n = 0 ; n < pnlConfig.Controls.Count ; n++ )
			for( int n = 0 ; n < Config_BottomPanel.Controls.Count ; n++ )
				//if( pnlConfig.Controls[n] is ctlLabelTextbox )
				if( ( Config_BottomPanel.Controls[n] as ctlLabelTextbox ) != null )
				{
					ultb = ( ctlLabelTextbox ) Config_BottomPanel.Controls[n];
					if( ultb.Name == "ctlTimeUstavkiSbros" )
						continue;

					if( ultb.isChange )
					{
						CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 );
						//StrToBCD_Field( ultb, memX );
						isUstChange = true;
						// для какого АПМ посылать
						TestAPMUst( slAPMCh, ultb.addrLinkVar );
						//long cnt = ultb.addrLinkVar - ( ultb.addrLinkVar - baseUst ) / 47;	// для отладки
						//slAPMCh[ultb.addrLinkVar - ( ultb.addrLinkVar - baseUst ) / 47] = true;
					}
				}
			//------------------------------
			if( !isUstChange )
			{
				MessageBox.Show( "Уставки не изменялись. \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}
			// формируем пакет и команду для отправки изменения уставок
			for( int i = 0 ; i < slAPMCh.Count ; i++ )
			{
				if( ( bool ) slAPMCh.GetByIndex( i ))
				{
					byte[] memXOut = new byte[94];
					memXOut[0] = BitConverter.GetBytes( 'R' ) [0];
					memXOut[1] = BitConverter.GetBytes( 'S' ) [0];
					int ng = i + 1;
					char chh = Convert.ToChar( ng.ToString() );
					memXOut[2] = Convert.ToByte( chh );	//BitConverter.GetBytes( Convert.ToChar(i+1) )[0];
					memXOut[3] = 0;
					int offf = ( ( int ) slAPMCh.GetKey( i ) - baseUst ) * 2 + 4;
					Buffer.BlockCopy( memX, offf, memXOut, 4, 90 );  // Handle пока нулевой	
               if( parent.newKB.ExecuteCommand( iFC, iIDDev, "WCP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
						parent.WriteEventToLog(35, "Команда \"WCP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );
					// документирование действия пользователя
					parent.WriteEventToLog(6, iIDDev.ToString(), this.Name, true);//, true, false );			//"выдана команда WCP - запись уставок."
				}
			}
			isUstChange = false;
		}
		private void TestAPMUst( SortedList sl, uint adr )
		{
			int[] arr = new int[sl.Count];
			for( int i = 0 ; i < sl.Count ; i++ )
				arr[i] = (int) sl.GetKey( i );

			for ( int i = 0; i < arr.Length; i++)
			{
            if( ( adr > arr[ i ] ) && i == arr.Length - 1 )
            {
               sl[ arr[ i ] ] = true;
               break;
            }
				else if( ( adr > arr[i] ) && ( adr < arr[i + 1] ) )
				{
					sl[arr[i]] = true;
					break;
				}
			}
		}

		private void dtpStartDateConfig_ValueChanged( object sender, EventArgs e )
		{
			//UstavBD();
		}

		private void lstvConfig_ItemActivate( object sender, EventArgs e )
		{
			if( lstvConfig.SelectedItems.Count == 0 )
				return;

			// получение строк соединения и поставщика данных из файла *.config
			//string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
			SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
			try
			{
				asqlconnect.Open();
			} catch( SqlException ex )
			{
				string errorMes = "";
				// интеграция всех возвращаемых ошибок
				foreach( SqlError connectError in ex.Errors )
					errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ")" + Environment.NewLine;
				parent.WriteEventToLog(21, "Нет связи с БД (lstvConfig_ItemActivate): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
            System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMMRCH : Нет связи с БД (lstvConfig_ItemActivate)" );
				asqlconnect.Close();
				return;
			} catch( Exception ex )
			{
				MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );
				asqlconnect.Close();
				return;
			}
			// формирование данных для вызова хранимой процедуры
			SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
			cmd.CommandType = CommandType.StoredProcedure;

			// входные параметры
			// 1. ip FC
			SqlParameter pipFC = new SqlParameter();
			pipFC.ParameterName = "@IP";
			pipFC.SqlDbType = SqlDbType.BigInt;
			pipFC.Value = 0;
			pipFC.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( pipFC );
			// 2. id устройства
			SqlParameter pidBlock = new SqlParameter();
			pidBlock.ParameterName = "@id";
			pidBlock.SqlDbType = SqlDbType.Int;
			pidBlock.Value = 0;
			pidBlock.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( pidBlock );
			// 3. время старт
			SqlParameter ptimeStart = new SqlParameter();
			ptimeStart.ParameterName = "@dt_start";
			ptimeStart.SqlDbType = SqlDbType.DateTime;
			ptimeStart.Value = DateTime.Now;
			ptimeStart.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( ptimeStart );
			// 4. время конец
			SqlParameter ptimeFin = new SqlParameter();
			ptimeFin.ParameterName = "@dt_end";
			ptimeFin.SqlDbType = SqlDbType.DateTime;
			ptimeFin.Value = DateTime.Now;
			ptimeFin.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( ptimeFin );
			// 5. тип записи - не нужен - все по Tag
			SqlParameter ptypeRec = new SqlParameter();
			ptypeRec.ParameterName = "@type";
			ptypeRec.SqlDbType = SqlDbType.Int;
			ptypeRec.Value = 0;
			ptypeRec.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( ptypeRec );
			// 6. ид записи журнала
			SqlParameter pid = new SqlParameter();
			pid.ParameterName = "@id_record";
			pid.SqlDbType = SqlDbType.Int;
			pid.Value = lstvConfig.SelectedItems[0].Tag;
			pid.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( pid );

			// заполнение DataSet
			DataSet aDS = new DataSet( "ptk" );
			SqlDataAdapter aSDA = new SqlDataAdapter();
			aSDA.SelectCommand = cmd;

			//aSDA.sq
			aSDA.Fill( aDS );//, "DataLog" 

			asqlconnect.Close();

			//PrintDataSet( aDS );
			// извлекаем данные по аварии
			DataTable dt = aDS.Tables[0];
			byte[] adata = ( byte[] ) dt.Rows[0]["Data"];

			// вызываем процедуру разбора пакета из базы
         ParseBDPacket( adata, 62000, iIDDev );
			SetArhivGroupInDev( iIDDev, 14 );

         // это чтобы при отколюченном устройстве была доступна истор. инф.
         parent.newKB.ReceivePacketForce( iFC, iIDDev );

			dt.Dispose();
			aSDA.Dispose();
			aDS.Dispose();

			btnWriteUst.Enabled = true;
		}
		#region процедура разбора пакета с информацией из базы
		private void ParseBDPacket( byte[] pack, ushort adr, int dev )
		{
			PrintHexDump( "LogHexPacket.dat", pack );  // выведем в файл для контроля
			parent.newKB.PacketToQueDev( pack, adr, iFC,dev );
		}
		#endregion

		private void btnReNew_Click( object sender, EventArgs e )
		{
			AvarBD();
         tcAvarBottomPanel.SelectTab( 0 );
		}
		private void AvarBD( )
		{
			// получение строк соединения и поставщика данных из файла *.config
			//string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
			SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
			try
			{
				asqlconnect.Open();
			} catch( SqlException ex )
			{
				string errorMes = "";
				// интеграция всех возвращаемых ошибок
				foreach( SqlError connectError in ex.Errors )
					errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ") ";

				parent.WriteEventToLog(21, "Нет связи с БД (AvarBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
            System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMMRCH : Нет связи с БД (AvarBD)" );
				asqlconnect.Close();
				return;
			} catch( Exception ex )
			{
				MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );
				asqlconnect.Close();
				return;
			}


			// формирование данных для вызова хранимой процедуры
			SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
			cmd.CommandType = CommandType.StoredProcedure;

			// входные параметры
			// 1. ip FC
			SqlParameter pipFC = new SqlParameter();
			pipFC.ParameterName = "@IP";
			pipFC.SqlDbType = SqlDbType.BigInt;
			pipFC.Value = 0;
			pipFC.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( pipFC );
			// 2. id устройства
			SqlParameter pidBlock = new SqlParameter();
			pidBlock.ParameterName = "@id";
			pidBlock.SqlDbType = SqlDbType.Int;
			pidBlock.Value = iIDDev;
			pidBlock.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( pidBlock );

			// 3. начальное время
			SqlParameter dtMim = new SqlParameter();
			dtMim.ParameterName = "@dt_start";
			dtMim.SqlDbType = SqlDbType.DateTime;
			TimeSpan tss = new TimeSpan( 0, dtpStartDateAvar.Value.Hour - dtpStartTimeAvar.Value.Hour, dtpStartDateAvar.Value.Minute - dtpStartTimeAvar.Value.Minute, dtpStartDateAvar.Value.Second - dtpStartTimeAvar.Value.Second );
			DateTime tim = dtpStartDateAvar.Value - tss;
			dtMim.Value = tim;
			dtMim.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( dtMim );

			// 2. конечное время
			SqlParameter dtMax = new SqlParameter();
			dtMax.ParameterName = "@dt_end";
			dtMax.SqlDbType = SqlDbType.DateTime;
			tss = new TimeSpan( 0, dtpEndDateAvar.Value.Hour - dtpEndTimeAvar.Value.Hour, dtpEndDateAvar.Value.Minute - dtpEndTimeAvar.Value.Minute, dtpEndDateAvar.Value.Second - dtpEndTimeAvar.Value.Second );
			tim = dtpEndDateAvar.Value - tss;
			dtMax.Value = tim;
			dtMax.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( dtMax );

			// 5. тип записи
			SqlParameter ptypeRec = new SqlParameter();
			ptypeRec.ParameterName = "@type";
			ptypeRec.SqlDbType = SqlDbType.Int;
			ptypeRec.Value = 2; // информация по авариям
			ptypeRec.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( ptypeRec );
			// 6. ид записи журнала
			SqlParameter pid = new SqlParameter();
			pid.ParameterName = "@id_record";
			pid.SqlDbType = SqlDbType.Int;
			pid.Value = 0;
			pid.Direction = ParameterDirection.Input;
			cmd.Parameters.Add( pid );

			// заполнение DataSet
			DataSet aDS = new DataSet( "ptk" );
			SqlDataAdapter aSDA = new SqlDataAdapter();
			aSDA.SelectCommand = cmd;

			//aSDA.sq
			aSDA.Fill( aDS, "TbAlarm" );

			asqlconnect.Close();

			//PrintDataSet( aDS );
			// извлекаем данные по аварии
			dtA = aDS.Tables["TbAlarm"];

			// заполняем ListView
			lstvAvar.Items.Clear();
			for( int curRow = 0 ; curRow < dtA.Rows.Count ; curRow++ )
			{
				DateTime t = ( DateTime ) dtA.Rows[curRow]["TimeBlock"];
				ListViewItem li = new ListViewItem( t.ToShortDateString() );
				li.SubItems.Add( t.ToLongTimeString() + ":" + t.Millisecond );
				li.Tag = dtA.Rows[curRow]["ID"];
				lstvAvar.Items.Add( li );
			}
			aSDA.Dispose();
			aDS.Dispose();
		}

		private void tabAvar_Enter( object sender, EventArgs e )
		{
			lstvAvar.Items.Clear();

			AvarBD();
			//-------------------------------------------------------------------
			//готовим инф. для отображения аналоговых и дискретных сигналов

			if( arrAvarSign.Count != 0 )
				return;

			CreateArrayList( arrAvarSign, "arrAvarSign" );

			// размещаем динамически на форме
			for( int i = 0 ; i < arrAvarSign.Count ; i++ )
			{
				FormulaEval ev = ( FormulaEval ) arrAvarSign[i];
				// смотрим категорию вкладки для размещения тега и его тип
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
						MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
						break;
				}
			}            
		}
		/*=======================================================================*
      *   private void lstvAvar_ItemActivate( object sender, EventArgs e )
      *       вывод информации при выборе конкретной аварии
      *=======================================================================*/
		private void lstvAvar_ItemActivate( object sender, EventArgs e )
		{
			if( lstvAvar.SelectedItems.Count > 0 )
			{
				// получение строк соединения и поставщика данных из файла *.config
				//string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
				SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
				try
				{
					asqlconnect.Open();
				} catch( SqlException ex )
				{
					string errorMes = "";
					// интеграция всех возвращаемых ошибок
					foreach( SqlError connectError in ex.Errors )
						errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ")" + Environment.NewLine;
					parent.WriteEventToLog(21, "Нет связи с БД (lstvAvar_ItemActivate): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
               System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMMRCH : Нет связи с БД (lstvAvar_ItemActivate)" );
					asqlconnect.Close();
					return;
				} catch( Exception ex )
				{
					MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );
					asqlconnect.Close();
					return;
				}
				// формирование данных для вызова хранимой процедуры
				SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
				cmd.CommandType = CommandType.StoredProcedure;

				// входные параметры
				// 1. ip FC
				SqlParameter pipFC = new SqlParameter();
				pipFC.ParameterName = "@IP";
				pipFC.SqlDbType = SqlDbType.BigInt;
				pipFC.Value = 0;
				pipFC.Direction = ParameterDirection.Input;
				cmd.Parameters.Add( pipFC );
				// 2. id устройства
				SqlParameter pidBlock = new SqlParameter();
				pidBlock.ParameterName = "@id";
				pidBlock.SqlDbType = SqlDbType.Int;
				pidBlock.Value = 0;
				pidBlock.Direction = ParameterDirection.Input;
				cmd.Parameters.Add( pidBlock );
				// 3. время старт
				SqlParameter ptimeStart = new SqlParameter();
				ptimeStart.ParameterName = "@dt_start";
				ptimeStart.SqlDbType = SqlDbType.DateTime;
				ptimeStart.Value = DateTime.Now;
				ptimeStart.Direction = ParameterDirection.Input;
				cmd.Parameters.Add( ptimeStart );
				// 4. время конец
				SqlParameter ptimeFin = new SqlParameter();
				ptimeFin.ParameterName = "@dt_end";
				ptimeFin.SqlDbType = SqlDbType.DateTime;
				ptimeFin.Value = DateTime.Now;
				ptimeFin.Direction = ParameterDirection.Input;
				cmd.Parameters.Add( ptimeFin );
				// 5. тип записи
				SqlParameter ptypeRec = new SqlParameter();
				ptypeRec.ParameterName = "@type";
				ptypeRec.SqlDbType = SqlDbType.Int;
				ptypeRec.Value = 0;
				ptypeRec.Direction = ParameterDirection.Input;
				cmd.Parameters.Add( ptypeRec );
				// 6. ид записи журнала
				SqlParameter pid = new SqlParameter();
				pid.ParameterName = "@id_record";
				pid.SqlDbType = SqlDbType.Int;
				pid.Value = lstvAvar.SelectedItems[0].Tag;
				pid.Direction = ParameterDirection.Input;
				cmd.Parameters.Add( pid );

				// заполнение DataSet
				DataSet aDS = new DataSet( "ptk" );
				SqlDataAdapter aSDA = new SqlDataAdapter();
				aSDA.SelectCommand = cmd;

				//aSDA.sq
				aSDA.Fill( aDS );//, "DataLog" 

				asqlconnect.Close();

				//PrintDataSet( aDS );
				// извлекаем данные по аварии
				DataTable dt = aDS.Tables[0];
				byte[] adata = ( byte[] ) dt.Rows[0]["Data"];
				UInt16 adrAPM = BitConverter.ToUInt16( adata, 4 );

				// вызываем процедуру разбора пакета с аварийной информацией из базы
            parent.newKB.ResetGroup( iFC, iIDDev, 8 );   // чтобы старая информация для других АПМ не мешалась
				ParseBDPacket( adata, adrAPM, iIDDev );//10280

            // это чтобы при отключенном устройстве была доступна истор. инф.

            parent.newKB.ReceivePacketForce( iFC, iIDDev );
				aSDA.Dispose();
            int nt = 0;
            for( int i = 0 ;i < tc.TabPages.Count ;i++ )
            {
               nt = Convert.ToInt32( tc.TabPages[ i ].Tag );
               if( nt == adrAPM )
               {
                  tc.SelectTab( i );
                  break;
               }
            }
			}
		}

		private void dtpStartDateAvar_ValueChanged( object sender, EventArgs e )
		{
			//AvarBD();
		}

		private void tabStore_Enter( object sender, EventArgs e )
		{
			if( arrStoreSign.Count != 0 )
				return;

			CreateArrayList( arrStoreSign, "arrStoreSign" );
			//--------------------

			// размещаем динамически на форме
			for( int i = 0 ; i < arrStoreSign.Count ; i++ )
			{
				FormulaEval ev = ( FormulaEval ) arrStoreSign[i];
				// смотрим категорию вкладки для размещения тега и его тип
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
						chBV.checkBox1.Text = "";
						chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
						chBV.AutoSize = true;
						ev.OnChangeValForm += chBV.LinkSetText;
						ev.FirstValue();
						break;
					case TypeOfTag.no:
						break;
					default:
						MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
						break;
				}
			}
		}

		private void btnReadStoreFC_Click( object sender, EventArgs e )
		{
         if ( parent.newKB.ExecuteCommand( iFC, iIDDev, "IMC", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "Команда \"IMC\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );
			// документирование действия пользователя
			parent.WriteEventToLog(8, iIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда IMC - чтение накопительной."
			//b_62132.FirstValue();
		}

		private void btnReadStoreBlock_Click( object sender, EventArgs e )
		{
         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RCD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "Команда \"RCD\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );
			// документирование действия пользователя
			parent.WriteEventToLog(8, iIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда RCD - чтение накопительной."
			//b_62132.FirstValue();
		}

		private void btnResetStore_Click( object sender, EventArgs e )
		{
			if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b06_Reset_info, parent.UserRight ) )
				return;

			DialogResult dr = MessageBox.Show( "Сбросить накопительную информацию блока?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
			if( dr == DialogResult.Yes )
			{
            if( parent.newKB.ExecuteCommand( iFC, iIDDev, "CCD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
					parent.WriteEventToLog(35, "Команда \"CCD\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );

				// документирование действия пользователя
				parent.WriteEventToLog(9, iIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда CCD - сброс накопительной."
			}
		}

		private void btnReadMaxMeterFC_Click( object sender, EventArgs e )
		{
         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "IMD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "Команда \"IMD\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );
			// документирование действия пользователя
			parent.WriteEventToLog(10, iIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда IMD - чтение максметра."
		}

		private void btnReadMaxMeterBlock_Click( object sender, EventArgs e )
		{
         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RMD",String.Empty, null,  parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "Команда \"RMD\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );
		}

		private void btnResetMaxMeter_Click( object sender, EventArgs e )
		{
			if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b06_Reset_info, parent.UserRight ) )
				return;

			DialogResult dr = MessageBox.Show( "Сбросить максметр?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
			if( dr == DialogResult.Yes )
			{
            if( parent.newKB.ExecuteCommand( iFC, iIDDev, "CMD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
					parent.WriteEventToLog(35, "Команда \"CMD\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );
				// документирование действия пользователя
				parent.WriteEventToLog(11, iIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда CMD - сброс максметра."
			}
		}

		private void BMMRCH_Current_Paint( object sender, PaintEventArgs e )
		{

		}

		private void button2_Click( object sender, EventArgs e )
		{
			Button btn = (Button) sender;
			byte[] memXOut = new byte[4];
			memXOut[0] = BitConverter.GetBytes( 'R' )[0];
			memXOut[1] = BitConverter.GetBytes( 'S' )[0];
			int ng = Convert.ToInt32( btn.Tag );
			char chh = Convert.ToChar( ng.ToString() );
			memXOut[2] = Convert.ToByte( chh );	//BitConverter.GetBytes( Convert.ToChar(i+1) )[0];
			memXOut[3] = 0;

         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "API", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(38, "Команда \"Возврат АПМ\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );
			// документирование действия пользователя
			parent.WriteEventToLog(38, iIDDev.ToString(), this.Name, true);//, true, false );
      }
      #region вкладка Осциллограммы
      private void tabPage40_Enter( object sender, EventArgs e )
      {
         // заполняем datagrid'ы
         DiagBD( );
         OscBD( );
      }
      /// <summary>
      /// получение осциллограмм из базы
      /// </summary>
      private void OscBD()
      {
         dgvOscill.Rows.Clear( );
         // получение строк соединения и поставщика данных из файла *.config
         //string cnStr = ConfigurationManager.ConnectionStrings[ "SqlProviderPTK" ].ConnectionString;
         SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
         try
         {
            asqlconnect.Open( );
         }
         catch( SqlException ex )
         {
            string errorMes = "";
            // интеграция всех возвращаемых ошибок
            foreach( SqlError connectError in ex.Errors )
               errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
				parent.WriteEventToLog(21, "Нет связи с БД (OscBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
            System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMMRCH : Нет связи с БД (OscBD)" );
            asqlconnect.Close( );
            return;
         }
         catch( Exception ex )
         {
            MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );
            asqlconnect.Close( );
            return;
         }
         // формирование данных для вызова хранимой процедуры
         SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
         cmd.CommandType = CommandType.StoredProcedure;

         // входные параметры
         // 1. ip FC
         SqlParameter pipFC = new SqlParameter( );
         pipFC.ParameterName = "@IP";
         pipFC.SqlDbType = SqlDbType.BigInt;
         pipFC.Value = 0;
         pipFC.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pipFC );
         // 2. id устройства
         SqlParameter pidBlock = new SqlParameter( );
         pidBlock.ParameterName = "@id";
         pidBlock.SqlDbType = SqlDbType.Int;
         pidBlock.Value = iIDDev;
         pidBlock.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pidBlock );

         // 3. начальное время
         SqlParameter dtMim = new SqlParameter( );
         dtMim.ParameterName = "@dt_start";
         dtMim.SqlDbType = SqlDbType.DateTime;
         TimeSpan tss = new TimeSpan( 0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second );
         DateTime tim = dtpStartData.Value - tss;
         dtMim.Value = tim;
         dtMim.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMim );

         // 2. конечное время
         SqlParameter dtMax = new SqlParameter( );
         dtMax.ParameterName = "@dt_end";
         dtMax.SqlDbType = SqlDbType.DateTime;
         tss = new TimeSpan( 0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second );
         tim = dtpEndData.Value - tss;
         dtMax.Value = tim;
         dtMax.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMax );

         // 5. тип записи
         SqlParameter ptypeRec = new SqlParameter( );
         ptypeRec.ParameterName = "@type";
         ptypeRec.SqlDbType = SqlDbType.Int;

         ptypeRec.Value = 4; // информация по осциллограммам
         ptypeRec.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( ptypeRec );
         // 6. ид записи журнала
         SqlParameter pid = new SqlParameter( );
         pid.ParameterName = "@id_record";
         pid.SqlDbType = SqlDbType.Int;
         pid.Value = 0;
         pid.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pid );

         // заполнение DataSet
         DataSet aDS = new DataSet( "ptk" );
         SqlDataAdapter aSDA = new SqlDataAdapter( );
         aSDA.SelectCommand = cmd;

         //aSDA.sq
         aSDA.Fill( aDS, "TbOscill" );//TbAlarm

         asqlconnect.Close( );

         //PrintDataSet( aDS );
         // извлекаем данные по осциллограммам
         dtO = aDS.Tables[ "TbOscill" ];//TbAlarm
         for( int curRow = 0 ;curRow < dtO.Rows.Count ;curRow++ )
         {
            int i = dgvOscill.Rows.Add( );   // номер строки
            dgvOscill[ "clmChBoxOsc", i ].Value = false;
            dgvOscill[ "clmBlockNameOsc", i ].Value = dtO.Rows[ curRow ][ "BlockName" ];
            dgvOscill[ "clmBlockIdOsc", i ].Value = dtO.Rows[ curRow ][ "BlockID" ];
            dgvOscill[ "clmBlockTimeOsc", i ].Value = dtO.Rows[ curRow ][ "TimeBlock" ];
            dgvOscill[ "clmCommentOsc", i ].Value = dtO.Rows[ curRow ][ "Comment" ];
            dgvOscill[ "clmID", i ].Value = dtO.Rows[ curRow ][ "ID" ];
         }
         aSDA.Dispose( );
         aDS.Dispose( );
      }
      /// <summary>
      /// получение диаграмм из базы
      /// </summary>
      private void DiagBD()
      {
         this.dgvDiag.Rows.Clear( );
         // получение строк соединения и поставщика данных из файла *.config
         //string cnStr = ConfigurationManager.ConnectionStrings[ "SqlProviderPTK" ].ConnectionString;
         SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
         try
         {
            asqlconnect.Open( );
         }
         catch( SqlException ex )
         {
            string errorMes = "";
            // интеграция всех возвращаемых ошибок
            foreach( SqlError connectError in ex.Errors )
               errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
				parent.WriteEventToLog(21, "Нет связи с БД (DiagBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД

            asqlconnect.Close( );
            return;
         }
         catch( Exception ex )
         {
            MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );
            asqlconnect.Close( );
            return;
         }
         // формирование данных для вызова хранимой процедуры
         SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
         cmd.CommandType = CommandType.StoredProcedure;

         // входные параметры
         // 1. ip FC
         SqlParameter pipFC = new SqlParameter( );
         pipFC.ParameterName = "@IP";
         pipFC.SqlDbType = SqlDbType.BigInt;
         pipFC.Value = 0;
         pipFC.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pipFC );
         // 2. id устройства
         SqlParameter pidBlock = new SqlParameter( );
         pidBlock.ParameterName = "@id";
         pidBlock.SqlDbType = SqlDbType.Int;
         pidBlock.Value = iIDDev;
         pidBlock.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pidBlock );

         // 3. начальное время
         SqlParameter dtMim = new SqlParameter( );
         dtMim.ParameterName = "@dt_start";
         dtMim.SqlDbType = SqlDbType.DateTime;
         TimeSpan tss = new TimeSpan( 0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second );
         DateTime tim = dtpStartData.Value - tss;
         dtMim.Value = tim;
         dtMim.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMim );

         // 2. конечное время
         SqlParameter dtMax = new SqlParameter( );
         dtMax.ParameterName = "@dt_end";
         dtMax.SqlDbType = SqlDbType.DateTime;
         tss = new TimeSpan( 0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second );
         tim = dtpEndData.Value - tss;
         dtMax.Value = tim;
         dtMax.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMax );

         // 5. тип записи
         SqlParameter ptypeRec = new SqlParameter( );
         ptypeRec.ParameterName = "@type";
         ptypeRec.SqlDbType = SqlDbType.Int;

         ptypeRec.Value = 5; // информация по диаграммам
         ptypeRec.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( ptypeRec );
         // 6. ид записи журнала
         SqlParameter pid = new SqlParameter( );
         pid.ParameterName = "@id_record";
         pid.SqlDbType = SqlDbType.Int;
         pid.Value = 0;
         pid.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pid );

         // заполнение DataSet
         DataSet aDS = new DataSet( "ptk" );
         SqlDataAdapter aSDA = new SqlDataAdapter( );
         aSDA.SelectCommand = cmd;

         //aSDA.sq
         aSDA.Fill( aDS, "TbDiag" );//TbAlarm

         asqlconnect.Close( );

         //PrintDataSet( aDS );
         // извлекаем данные по диаграммам
         dtG = aDS.Tables[ "TbDiag" ];//TbAlarm
         for( int curRow = 0 ;curRow < dtG.Rows.Count ;curRow++ )
         {
            int i = dgvDiag.Rows.Add( );   // номер строки
            dgvDiag[ "clmChBoxDiag", i ].Value = false;
            dgvDiag[ "clmBlockNameDiag", i ].Value = dtG.Rows[ curRow ][ "BlockName" ];
            dgvDiag[ "clmBlockIdDiag", i ].Value = dtG.Rows[ curRow ][ "BlockID" ];
            dgvDiag[ "clmBlockTimeDiag", i ].Value = dtG.Rows[ curRow ][ "TimeBlock" ];
            dgvDiag[ "clmCommentDiag", i ].Value = dtG.Rows[ curRow ][ "Comment" ];
            dgvDiag[ "clmIDDiag", i ].Value = dtG.Rows[ curRow ][ "ID" ];

            //dgvDiag["clmViewDiag", i].Value = "Диаграмма";
         }
         aSDA.Dispose( );
         aDS.Dispose( );
      }
      /// <summary>
      /// объединяем осциллограммы
      /// </summary>
      private void btnUnionOsc_Click( object sender, EventArgs e )
      {
         ArrayList asb = new ArrayList( );    // для хранения имен файлов
         string ifa;
         ;
         StringBuilder sb;
         byte[] arrO = null;
         char[] sep = { ' ', '-' };
         string[] sp;

         // перечисляем записи в таблице dbO, смотрим отмеченные, формируем файлы, вызываем fastview
         for( int curRow = 0 ;curRow < dtO.Rows.Count ;curRow++ )
         {
            if( ( bool ) dgvOscill[ 0, curRow ].Value == true )
            {
               // формируем файл, запоминаем имя в массиве
               arrO = ( byte[] ) dtO.Rows[ curRow ][ "Data" ];

               // формируем имя файла
               ifa = ( string ) dtO.Rows[ curRow ][ "BlockName" ] + "_" + curRow.ToString( ) + ".osc";

               // удаляем пробелы
               sp = ifa.Split( sep );
               sb = new StringBuilder( );
               for( int i = 0 ;i < sp.Length ;i++ )
               {
                  sb.Append( sp[ i ] );
               }
               // сохраняем имя в массиве
               asb.Add( sb );
               // создаем файл
               FileStream f = File.Create( Environment.CurrentDirectory.ToString( ) + "\\" + sb.ToString( ) );
               f.Write( arrO, 0, arrO.Length );
               f.Close( );
            }
         }
         // запускаем fastview
         Process prc = new Process( );
         sb = new StringBuilder( );
         foreach( StringBuilder s in asb )
         {
            sb.Append( s.ToString( ) );
            sb.Append( " " );
         }
         prc.StartInfo.FileName = Environment.CurrentDirectory.ToString( ) + "\\Fastview\\fastview.exe";
         prc.StartInfo.Arguments = "-o " + sb.ToString( );//+ "/"
         prc.Start( );
      }
      /// <summary>
      /// объединяем диаграммы
      /// </summary>
      private void btnUnionDiag_Click( object sender, EventArgs e )
      {
         ArrayList asb = new ArrayList( );    // для хранения имен файлов
         string ifa;
         ;
         StringBuilder sb;
         byte[] arrO = null;
         char[] sep = { ' ', '-' };
         string[] sp;

         // перечисляем записи в таблице dbO, смотрим отмеченные, формируем файлы, вызываем fastview
         for( int curRow = 0 ;curRow < dtG.Rows.Count ;curRow++ )
         {
            if( ( bool ) dgvDiag[ 0, curRow ].Value == true )
            {
               // формируем файл, запоминаем имя в массиве
               arrO = ( byte[] ) dtG.Rows[ curRow ][ "Data" ];

               // формируем имя файла
               ifa = ( string ) dtG.Rows[ curRow ][ "BlockName" ] + "_" + curRow.ToString( ) + ".dgm";

               // удаляем пробелы
               sp = ifa.Split( sep );
               sb = new StringBuilder( );
               for( int i = 0 ;i < sp.Length ;i++ )
               {
                  sb.Append( sp[ i ] );
               }
               // сохраняем имя в массиве
               asb.Add( sb );
               // создаем файл
               FileStream f = File.Create( Environment.CurrentDirectory.ToString( ) + "\\" + sb.ToString( ) );
               f.Write( arrO, 0, arrO.Length );
               f.Close( );
            }
         }
         // запускаем fastview
         Process prc = new Process( );
         sb = new StringBuilder( );
         foreach( StringBuilder s in asb )
         {
            sb.Append( s.ToString( ) );
            sb.Append( " " );
         }
         prc.StartInfo.FileName = Environment.CurrentDirectory.ToString( ) + "\\Fastview\\fastview.exe";
         prc.StartInfo.Arguments = "-m " + sb.ToString( );//+ "/"
         prc.Start( );
      }
      private void dtpStartData_ValueChanged( object sender, EventArgs e )
      {
         // выводим результаты запроса
         dgvOscill.Rows.Clear( );
         OscBD( );
         DiagBD( );
      }
      private void btnReNewOD_Click( object sender, EventArgs e )
      {
         DiagBD( );
         OscBD( );
      }
      private void dgvOscill_CellContentClick( object sender, DataGridViewCellEventArgs e )
        {
            byte[] arrO = null;
            string ifa;         // имя файла
            DataGridViewCell    de;
            char[] sep = { ' ' };
            string[] sp;
            StringBuilder sb;
            if( e.ColumnIndex == 0 )
            {
                dgvOscill[e.ColumnIndex, e.RowIndex].Value = ( bool ) dgvOscill[e.ColumnIndex, e.RowIndex].Value ? false : true;
                btnUnionOsc.Enabled = true;
                return;
            }
            else if( e.ColumnIndex != 5 )
                return;

            btnUnionOsc.Enabled = false;
            // сбрасываем все флажки
            for( int i = 0; i < dtO.Rows.Count; i++ )
                dgvOscill[0, i].Value = false;

            try
            {
                de = dgvOscill["clmID", e.RowIndex];
            }
            catch
            {
                MessageBox.Show( "dgvOscill_CellContentClick - исключение" );
                return;
            }
            int ide = (int) de.Value;

            // по ide найти запись в dto, извлечь блок с осциллограммой (диаграммой), записать в файл, запустить fastview
            // перечисляем записи в dto
            int curRow;

            for( curRow = 0; curRow < dtO.Rows.Count; curRow++ )
            {
                if( ide == ( ( int ) dtO.Rows[curRow]["ID"] ) )
                {
                    arrO = ( byte[] ) dtO.Rows[curRow]["Data"];
                    break;
                }
            }
            // записываем массив байт в файл
            // формируем имя файла в зависимости от типа - диаграмма или осциллограмма
                ifa = ( string ) dtO.Rows[curRow]["BlockName"] + ".osc";

                // удаляем пробелы
                sp = ifa.Split( sep );
                sb = new StringBuilder();
                for( int i = 0; i < sp.Length; i++ )
                {
                    sb.Append( sp[i] );
                }

            FileStream f = File.Create( Environment.CurrentDirectory.ToString() + "\\" + sb.ToString());
            f.Write( arrO, 0, arrO.Length );
            f.Close();
            // запускаем fastview
            Process prc = new Process();
            prc.StartInfo.FileName = Environment.CurrentDirectory.ToString() + "\\Fastview\\fastview.exe  ";
            prc.StartInfo.Arguments = sb.ToString();
            prc.Start();
        }

      private void dgvDiag_CellContentClick( object sender, DataGridViewCellEventArgs e )
        {
            byte[] arrO = null;
            string ifa;         // имя файла
            DataGridViewCell de;
            char[] sep = { ' ' };
            string[] sp;
            StringBuilder sb;
            if( e.ColumnIndex == 0 )
            {
                dgvDiag[e.ColumnIndex, e.RowIndex].Value = ( bool ) dgvDiag[e.ColumnIndex, e.RowIndex].Value ? false : true;
                btnUnionDiag.Enabled = true;
                return;
            }
            else if( e.ColumnIndex != 5 )
                return;

            btnUnionDiag.Enabled = false;
            // сбрасываем все флажки
            for( int i = 0; i < dtG.Rows.Count; i++ )
                dgvDiag[0, i].Value = false;
            try
            {
                de = dgvDiag["clmIDDiag", e.RowIndex];
            }
            catch
            {
                MessageBox.Show( "dgvDiag_CellContentClick - исключение" );
                return;
            }
            int ide = ( int ) de.Value;

            // по ide найти запись в dto, извлечь блок с осциллограммой (диаграммой), записать в файл, запустить fastview
            // перечисляем записи в dto
            int curRow;

            for( curRow = 0; curRow < dtG.Rows.Count; curRow++ )
            {
                if( ide == ( ( int ) dtG.Rows[curRow]["ID"] ) )
                {
                    arrO = ( byte[] ) dtG.Rows[curRow]["Data"];
                    break;
                }
            }
            // записываем массив байт в файл
            // формируем имя файла в зависимости от типа - диаграмма или осциллограмма
            ifa = ( string ) dtG.Rows[curRow]["BlockName"] + ".dgm";

            // удаляем пробелы
            sp = ifa.Split( sep );
            sb = new StringBuilder();
            for( int i = 0; i < sp.Length; i++ )
            {
                sb.Append( sp[i] );
            }

            FileStream f = File.Create( Environment.CurrentDirectory.ToString() + "\\" + sb.ToString() );
            f.Write( arrO, 0, arrO.Length );
            f.Close();
            // запускаем fastview
            Process prc = new Process();

            prc.StartInfo.FileName = Environment.CurrentDirectory.ToString() + "\\Fastview\\fastview.exe  ";
            prc.StartInfo.Arguments = sb.ToString();
            prc.Start();
        }

#endregion

      private void timerInd_Tick( object sender, EventArgs e )
      {
         foreach( CustomIndicator ci in arrIndicators )
         {
            ci.Renew( );
         }
      }

      private void frmBMRCH_Shown( object sender, EventArgs e )
      {
         timerInd.Start( );
         firstShow = true;
      }

      private void tabCurrent_Leave( object sender, EventArgs e )
      {
         if( firstShow )
         {
            firstShow = false;
            return;
         }
         timerInd.Stop( );
      }
      private void IndBind()
     {
         FormulaEval efv;
         ArrayList arrVar = new ArrayList();
         // реле 1
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.72.0001)", "0", "Реле 1", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_1.LinkSetText_Ind;
         arrVar.Add(efv);
         efv.FirstValue( );
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0001)", "0", "Реле 1", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_1.LinkSetText_IndFlash;
         arrVar.Add(efv);
         efv.FirstValue( );
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0001)", "0", "Реле 1", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_1.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0100)", "0", "Реле 1", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_1.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         ciRe_1.OnChangeIndode += this.SynhrInd;
         // реле 2
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.72.0002)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_2.LinkSetText_Ind;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0002)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_2.LinkSetText_IndFlash;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0002)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_2.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0200)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_2.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         ciRe_2.OnChangeIndode += this.SynhrInd;
         // реле 3
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.72.0004)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_3.LinkSetText_Ind;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0004)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_3.LinkSetText_IndFlash;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0004)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_3.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0400)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_3.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         ciRe_3.OnChangeIndode += this.SynhrInd;
         // реле 4
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.72.0008)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_4.LinkSetText_Ind;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0008)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_4.LinkSetText_IndFlash;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0008)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_4.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0800)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_4.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         ciRe_4.OnChangeIndode += this.SynhrInd;
         // реле 5
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.72.0010)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_5.LinkSetText_Ind;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0010)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_5.LinkSetText_IndFlash;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0010)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_5.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.1000)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_5.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         ciRe_5.OnChangeIndode += this.SynhrInd;
         // реле 6
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.72.0020)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_6.LinkSetText_Ind;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0020)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_6.LinkSetText_IndFlash;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0020)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_6.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.2000)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_6.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         ciRe_6.OnChangeIndode += this.SynhrInd;
         // реле 7
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.72.0040)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_7.LinkSetText_Ind;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0040)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_7.LinkSetText_IndFlash;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0040)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_7.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.4000)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_7.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         ciRe_7.OnChangeIndode += this.SynhrInd;
         // реле 8
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0080)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_8.LinkSetText_Ind;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0080)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_8.LinkSetText_IndFlash;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.0080)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_8.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.74.8000)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciRe_8.LinkSetText_IndErr;
         arrVar.Add(efv);
         efv.FirstValue();
         ciRe_8.OnChangeIndode += this.SynhrInd;
         // пуск
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0100)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciPusk.LinkSetText_Ind;
         arrVar.Add(efv);
         efv.FirstValue();
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0200)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciPusk.LinkSetText_IndFlash;
         arrVar.Add(efv);
         efv = new FormulaEval(parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0400)", "0", "", "", TypeOfTag.Discret, "");
         efv.OnChangeValForm += ciPusk.LinkSetText_IndFlash;
         arrVar.Add(efv);
         efv = new FormulaEval(parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.0800)", "0", "", "", TypeOfTag.Discret, "");
         arrVar.Add(efv);
         efv.OnChangeValForm += ciPusk.LinkSetText_IndFlash;
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.1000)", "0", "", "", TypeOfTag.Discret, "" );
         arrVar.Add(efv);
         efv.OnChangeValForm += ciPusk.LinkSetText_IndFlash;
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.2000)", "0", "", "", TypeOfTag.Discret, "" );
         arrVar.Add(efv);
         efv.OnChangeValForm += ciPusk.LinkSetText_IndFlash;
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.4000)", "0", "", "", TypeOfTag.Discret, "" );
         arrVar.Add(efv);
         efv.OnChangeValForm += ciPusk.LinkSetText_IndFlash;
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.73.8000)", "0", "", "", TypeOfTag.Discret, "" );
         arrVar.Add(efv);
         efv.OnChangeValForm += ciPusk.LinkSetText_IndFlash;
         efv.FirstValue( );
         ciPusk.OnChangeIndode += this.SynhrInd;
         // работа
         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.29.ff00)", "0", "", "", TypeOfTag.Analog, "" );
         efv.OnChangeValForm += ciWork.LinkSetText_IndFlashA;
         arrVar.Add(efv);
         efv.FirstValue();

         efv = new FormulaEval( parent.KB, "0(" + iFC + "." + iIDDev + ".3.29.ff00)", "0", "", "", TypeOfTag.Discret, "" );
         efv.OnChangeValForm += ciWork.LinkSetText_IndA;
         arrVar.Add(efv);
         efv.FirstValue();

         ciWork.OnChangeIndode += this.SynhrInd;

          // включаем в клиент-серверный механизм обмена регистрами
		 if (HMI_Settings.ClientDFE != null)
             foreach (FormulaEval fe in arrVar)
				 HMI_Settings.ClientDFE.AddArrTags(this.Text, fe);

        ciRe_1.ColorInd = Brushes.Red;
         arrIndicators.Add( ciRe_1 );
         ciRe_2.ColorInd = Brushes.Red;
         arrIndicators.Add( ciRe_2 );
         ciRe_3.ColorInd = Brushes.Red;
         arrIndicators.Add( ciRe_3 );
         ciRe_4.ColorInd = Brushes.Red;
         arrIndicators.Add( ciRe_4 );
         ciRe_5.ColorInd = Brushes.Red;
         arrIndicators.Add( ciRe_5 );
         ciRe_6.ColorInd = Brushes.Red;
         arrIndicators.Add( ciRe_6 );
         ciRe_7.ColorInd = Brushes.Red;
         arrIndicators.Add( ciRe_7 );
         ciRe_8.ColorInd = Brushes.Red;
         arrIndicators.Add( ciRe_8 );
         ciPusk.ColorInd = Brushes.Yellow;
         arrIndicators.Add( ciPusk );
         ciWork.ColorInd = Brushes.Green;
         arrIndicators.Add( ciWork );
      }
       public void SynhrInd()
      {
         return;  //!!! иначе не мигает если есть аналоговые и дискретные сигналы для мигания

         foreach( CustomIndicator ci in arrIndicators )
         {
            ci.counter = 0;
            ci.V_IndErrPrev = false;
            ci.V_IndFlashPrev = false;
            ci.V_IndPrev = false;
         }
      }

      private void btnAck_Click( object sender, EventArgs e )
      {
         if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b02_ACK_Signaling, parent.UserRight ) )
            return;

         ConfirmCommand dlg = new ConfirmCommand( );
         dlg.label1.Text = "Квитировать?";

         if( !( DialogResult.OK == dlg.ShowDialog( ) ) )
            return;
         // выполняем действия по квитированию
         Console.WriteLine( "Поступила команда \"Квитировать\" для устройства: {0}; id: {1}", "БММРЧ", iIDDev );
         // запись в журнал
			parent.WriteEventToLog(20, strIDDev, "БММРЧ", true);//, true, false );

         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "ECC", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            parent.WriteEventToLog( 35, "Команда \"Квитировать\" ушла в сеть. Устройство - "
					+ strFC + "." + strIDDev, "БММРЧ", true);//, true, false );
      }

      private void btnReNewUstBD_Click( object sender, EventArgs e )
      {
         UstavBD( );
         tcUstConfigBottomPanel.SelectTab( 0 );
      }

      private void frmBMRCH_FormClosing(object sender, FormClosingEventArgs e)
      {
		  if (HMI_Settings.ClientDFE != null)
			  HMI_Settings.ClientDFE.RemoveRefToPageTags(this.Text);
      }
   }
}
        