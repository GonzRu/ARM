/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Форма для работы с блоками УСО.                                                           
 *                                                                             
 *	Файл                     : frmUSO.cs                                         
 *	Тип конечного файла      : 
 *	версия ПО для разработки : С#, Framework 3.5                               
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
using System.Linq;
using System.Xml.Linq;

namespace HMI_MT
{
	public partial class frmUSO : Form
	{
      private MainForm parent;
      XDocument xdoc;
      SortedList slFormElements; // список элементов, свойства которых нужно будет изменить (см. Device.cfg)
      int iFC;            // номер ФК целочисленный
      string strFC;       // номер ФК строка
      int iIDDev;         // номер устройства целочисленный
      string strIDDev;    // номер устройства строка
      int inumLoc;         // номер ячейки целочисленный
      string strnumLoc;    // номер ячейки строка
      string nfXMLConfig; // имя файла с описанием 
      string nfXMLConfigFC; // имя файла с описанием ЩАСУ

      ArrayList arrAvarSign = new ArrayList();
      ArrayList arrCurSign = new ArrayList();
      ArrayList arrSystemSign = new ArrayList();
      ArrayList arrStoreSign = new ArrayList();
      ArrayList arrConfigSign = new ArrayList();
      ArrayList arrStatusDevCommand = new ArrayList();
      ArrayList arrStatusFCCommand = new ArrayList();

      ushort iclm = 16;  // число колонок в дампе
      SortedList slLocal;
      EncodingInfo eii;
      SortedList slEncoding;
      SortedList se = new SortedList();
      SortedList sl_tpnameUst = new SortedList();
      StringBuilder sbse = new StringBuilder();

      DataTable dtU;  // таблица с уставками

      SortedList slFLP = new SortedList();	// для хранения инф о FlowLayoutPanel
			
      FormulaEval b_62002;
      FormulaEval b_62092;
      ErrorProvider erp = new ErrorProvider( );
      #region конструктор
		public frmUSO( )
		{
			InitializeComponent();
		}
        public frmUSO(MainForm linkMainForm, int iFC, int iIDDev, int inumLoc, string fXML)
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
            slFormElements = new SortedList( );
		}
      /// <summary>
      /// загрузка формы
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
         // установим особенности других элементов формы на основании Device.cfg (см. УСО А - 00)
         SetElementsFormFeatures( Path.GetDirectoryName( nfXMLConfig ) );
      }

      /// <summary>
      /// рекурсивный поиск FlowLayoutPanel
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
      /// установить особенности элементов формы на основании Device.cfg (см. УСО А - 00)
      /// </summary>
      /// <param Name="pathtoDevCFG">путь к папке с описанием устройства - файл Device.cfg</param>
      private void SetElementsFormFeatures( string pathtoDevCFG )
      {         
         xdoc = XDocument.Load( pathtoDevCFG + Path.DirectorySeparatorChar + "Device.cfg");
         // ищем элементы формы по Name
         if ( String.IsNullOrEmpty( (string)xdoc.Element( "Device" ).Element( "DeviceFeatures" ).Element( "GDIFeatures" )) )
            return;

         IEnumerable<XElement> collNameEl = from tp in xdoc.Element( "Device" ).Element( "DeviceFeatures" ).Element( "GDIFeatures" ).Element( "ElementsAction" ).Elements( "Element" )
                                            select tp;
         
         // изменяемые элементы в список
         foreach ( XElement xecollNameEl in collNameEl )
            slFormElements.Add(xecollNameEl.Attribute("name").Value, null);

         // ищем эдементы списка на форме
         ControlCollection cc;
         cc = ( ControlCollection ) this.Controls;
         for ( int i = 0 ; i < cc.Count ; i++ )
            if ( slFormElements.Contains(cc[ i ].Name) )
               slFormElements[cc[ i ].Name] = cc[i];
            else
               TestCCforElements( cc[ i ] );

         // изменяем свойства элементов
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
      /// рекурсивный поиск элементов для включения в список на изменение свойств
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
									  sbse.Append( reader.GetAttribute( "name" ) );
									  reader.Read();
									  se[sbse.ToString()] = reader.GetAttribute( 0 );
									  reader.Read();
								  }
                          else if( reader.Name.Equals( "name_tabpage_ust" ) )
                          {   // запоминем названия вкладок в уставках и конфигурации
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
          *      для потокобезопасного вызова процедуры
          *==========================================================================*/
        delegate void SetLVCallback( ListViewItem li, bool actDellstV );

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

		#region вход на вкладку с текущей информацией
		/// <summary>
		/// вход на вкладку с текущей информацией 
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void tabCurrent_Enter( object sender, EventArgs e )
		{
			//-------------------------------------------------------------------
			//готовим инф. для отображения текущих значений аналоговых и дискретных сигналов
			// текущие аналоговые сигналы (рег. 0033 ...) с учетом коэффициента трансформации
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
					MessageBox.Show( "Выполнение действия запрещено" );
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
				parent.WriteEventToLog(39, "Команда \"CLS\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, false);//, true, false );
			}
			// документирование действия пользователя
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
					MessageBox.Show( "Выполнение действия запрещено" );
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
				parent.WriteEventToLog(40, "Команда \"OPN\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, false);//, true, false );
			}
			// документирование действия пользователя
			parent.WriteEventToLog(40, iIDDev.ToString(), this.Name, true);//, true, false );
		}
      #region вход на вкладку "Конфигурации и уставки"
      /// <summary>
      /// private void tbcConfig_Enter( object sender, EventArgs e )
      ///  вход на вкладку "Конфигурации и уставки"
      /// </summary>
      private void tbcConfig_Enter( object sender, EventArgs e )
      {
         lstvConfig.Items.Clear( );
         UstavBD( );
         //-------------------------------------------------------------------
         //готовим инф. для отображения аналоговых и дискретных сигналов
         if( arrConfigSign.Count != 0 )
            return;
         btnWriteUst.Enabled = false;
         CreateArrayList( arrConfigSign, "arrConfigSign" );

         // для начала скрываем все tabpage
         for( int i = 0 ;i < tbcConfig.Controls.Count ;i++ )
         {
            if( tbcConfig.Controls[ i ] is TabPage )
            {
               tbcConfig.Controls.RemoveAt( i );
               i--;
            }
         }

         // корректируем названия вкладок
         for( int i = 0 ;i < sl_tpnameUst.Count ;i++ )
            tbcConfig.Controls.Add( ( Control ) sl_tpnameUst.GetByIndex( i ) );

         // размещаем динамически на форме
         for( int i = 0 ;i < arrConfigSign.Count ;i++ )
         {
            FormulaEval ev = ( FormulaEval ) arrConfigSign[ i ];
            // смотрим категорию вкладки для размещения тега и его тип
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
                  MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                  break;
            }
         }
         //Config_BottomPanel.Width = pnlConfig.Width - btnResetValues.Width - groupBox11.Width - groupBox9.Width + 10;
         //splitter1.Visible = false;
      }
      private void UstavBD()
      {
         //dgvAvar.Rows.Clear();
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
				parent.WriteEventToLog(21, "Нет связи с БД (UstavBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД

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
         TimeSpan tss = new TimeSpan( 0, dtpStartDateConfig.Value.Hour - dtpStartTimeConfig.Value.Hour, dtpStartDateConfig.Value.Minute - dtpStartTimeConfig.Value.Minute, dtpStartDateConfig.Value.Second - dtpStartTimeConfig.Value.Second );
         DateTime tim = dtpStartDateConfig.Value - tss;
         dtMim.Value = tim;
         dtMim.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMim );

         // 2. конечное время
         SqlParameter dtMax = new SqlParameter( );
         dtMax.ParameterName = "@dt_end";
         dtMax.SqlDbType = SqlDbType.DateTime;
         tss = new TimeSpan( 0, dtpEndDateConfig.Value.Hour - dtpEndTimeConfig.Value.Hour, dtpEndDateConfig.Value.Minute - dtpEndTimeConfig.Value.Minute, dtpEndDateConfig.Value.Second - dtpEndTimeConfig.Value.Second );
         tim = dtpEndDateConfig.Value - tss;
         dtMax.Value = tim;
         dtMax.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMax );

         // 5. тип записи
         SqlParameter ptypeRec = new SqlParameter( );
         ptypeRec.ParameterName = "@type";
         ptypeRec.SqlDbType = SqlDbType.Int;
         ptypeRec.Value = 1; // информация по уставкам
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
         aSDA.Fill( aDS, "TbUstav" );

         asqlconnect.Close( );

         //PrintDataSet( aDS );
         // извлекаем данные по уставкам
         dtU = aDS.Tables[ "TbUstav" ];

         // заполняем ListView
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
				parent.WriteEventToLog(35, "Команда \"IMP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );

         // документирование действия пользователя
			parent.WriteEventToLog(7, iIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда IMP - чтения уставок."
         if( b_62002 != null )
            b_62002.FirstValue( );
         if( b_62092 != null )
            b_62092.FirstValue( );
      }

      private void btnReadUstBlock_Click( object sender, EventArgs e )
      {
         btnWriteUst.Enabled = true;

         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RCP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "Команда \"RCP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );

         // документирование действия пользователя
			parent.WriteEventToLog(7, iIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда RCP - чтения уставок."
         if( b_62002 != null )
            b_62002.FirstValue( );
         if( b_62092 != null )
            b_62092.FirstValue( );
      }

      #region вывод информации при выборе конкретной записи по уставкам
      private void lstvConfig_ItemActivate( object sender, EventArgs e )
      {
         if( lstvConfig.SelectedItems.Count == 0 )
            return;

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
				parent.WriteEventToLog(21, "Нет связи с БД (lstvConfig_ItemActivate): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД

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
         pidBlock.Value = 0;
         pidBlock.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pidBlock );
         // 3. время старт
         SqlParameter ptimeStart = new SqlParameter( );
         ptimeStart.ParameterName = "@dt_start";
         ptimeStart.SqlDbType = SqlDbType.DateTime;
         ptimeStart.Value = DateTime.Now;
         ptimeStart.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( ptimeStart );
         // 4. время конец
         SqlParameter ptimeFin = new SqlParameter( );
         ptimeFin.ParameterName = "@dt_end";
         ptimeFin.SqlDbType = SqlDbType.DateTime;
         ptimeFin.Value = DateTime.Now;
         ptimeFin.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( ptimeFin );
         // 5. тип записи - не нужен - все по Tag
         SqlParameter ptypeRec = new SqlParameter( );
         ptypeRec.ParameterName = "@type";
         ptypeRec.SqlDbType = SqlDbType.Int;
         ptypeRec.Value = 0;
         ptypeRec.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( ptypeRec );
         // 6. ид записи журнала
         SqlParameter pid = new SqlParameter( );
         pid.ParameterName = "@id_record";
         pid.SqlDbType = SqlDbType.Int;
         pid.Value = lstvConfig.SelectedItems[ 0 ].Tag;
         pid.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pid );

         // заполнение DataSet
         DataSet aDS = new DataSet( "ptk" );
         SqlDataAdapter aSDA = new SqlDataAdapter( );
         aSDA.SelectCommand = cmd;

         //aSDA.sq
         aSDA.Fill( aDS );//, "DataLog" 

         asqlconnect.Close( );

         //PrintDataSet( aDS );
         // извлекаем данные по аварии
         DataTable dt = aDS.Tables[ 0 ];
         byte[] adata = ( byte[] ) dt.Rows[ 0 ][ "Data" ];

         // вызываем процедуру разбора пакета из базы
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
      /// запись уставок
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
         StringBuilder sb = new StringBuilder( );
         uint ainmemX;    // адрес в массиве memX
         byte[] aTmp2 = new byte[ 2 ];

         // найдем SortedList для нужного устройства
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
            lenpack = BitConverter.ToInt16( ( byte[] ) slLocal[ 62000 ], 0 );
         }
         catch( ArgumentNullException ex )
         {
            MessageBox.Show( "Нет данных для записи. \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information );
            return;
         }

         short numdev = BitConverter.ToInt16( ( byte[] ) slLocal[ 62000 ], 2 );

         ushort add10 = BitConverter.ToUInt16( ( byte[] ) slLocal[ 62000 ], 4 );	//читаем адрес блока данных

         //int lenpack = ( short ) memDevBlock.ReadInt16();
         //short numdev = ( short ) memDevBlock.ReadUInt16();
         //ushort add10 = ( ushort ) memDevBlock.ReadInt16();	//читаем адрес блока данных

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
                              cbTmp.isChange = false;  // сбрасываем признак изменения у конкретного ComboBoxVar'а
                              // записываем изменения по ComboBoxVar'ам в исходный пакет (корректируем массив memX)
                              uint a = cbTmp.addrLinkVar; // адрес переменной
                              // получим значение
                              int st = cbTmp.cbVar.SelectedIndex;
                              byte[] bst = new byte[ 4 ];
                              bst = BitConverter.GetBytes( st );
                              Buffer.BlockCopy( bst, 0, aTmp2, 0, 2 );
                              Array.Reverse( aTmp2 );
                              // запоминаем изменения
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
                           }
                        }
                     }
                  }
               }
            }
         }
         //------------------------------
         // аналогично для панели уставок
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
            MessageBox.Show( "Уставки не изменялись. \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information );
            return;
         }
         // формируем пакет и команду для отправки изменения уставок
         byte[] memXOut = new byte[ memX.Length ];
         Buffer.BlockCopy( memX, 4, memXOut, 4, memX.Length - 4 );  // Handle пока нулевой

         if( parent.newKB.ExecuteCommand( iFC, iIDDev, "WCP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "Команда \"WCP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );
         // документирование действия пользователя
			parent.WriteEventToLog(6, iIDDev.ToString(), this.Name, true);//, true, false );			//"выдана команда WCP - запись уставок."
         isUstChange = false;
      }		

      #endregion

      #region процедура разбора пакета с информацией из базы
      private void ParseBDPacket( byte[] pack, ushort adr, int dev )
      {
         PrintHexDump( "LogHexPacket.dat", pack );  // выведем в файл для контроля
         parent.newKB.PacketToQueDev( pack, adr, iFC,dev ); // 10280 пакет  по адресу  устройства
         // объявить соответсвующую группу переменных архивной
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
        