/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Форма для работы с блоком (базовая).                                                           
 *                                                                             
 *	Файл                     : frmBMRZbase.cs                                         
 *	Тип конечного файла      : 
 *	версия ПО для разработки : С#, Framework 2.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : xx.04.2007 - xx.06.2009
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
      #region свойства
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

      int iFC;            // номер ФК целочисленный
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

      string strFC;       // номер ФК строка
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

      int iIDDev;         // номер устройства целочисленный
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

      string strIDDev;    // номер устройства строка
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

      int inumLoc;         // номер ячейки целочисленный
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

      string strnumLoc;    // номер ячейки строка
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
      public Hashtable htFormulaEvals = new Hashtable();   // хеш-таблица для хранения FormulaEval для данной формы устройства - пара (строка формулы, ссылка на FormulaEval)
      public SortedList slTPtoArrVars = new SortedList( );  // для хранения соответсвия tabpage и массива переменных для этой вкладки
      public SortedList slnflps;                            // сортированный список flp с именами и условными обозначениями, т.е. пара (Name, Caption)
      public SortedList slFLP = new SortedList( );	         // для хранения инф о FlowLayoutPanel, т.е. пара (flp.Name, ссылка на FLP)
      public ArrayList arDopPanel;                          // массив дополнительных (нижних) панелей
      public string path2PrgDevCFG = string.Empty;          // PrgDevCFG.cdp
      public string path2FrmDev = string.Empty;             // путь к файлу описания формы проекта
      public string path2DeviceCFG = string.Empty;          // device.cfg
      public ErrorProvider erp = new ErrorProvider( );
      /// <summary>
      /// адрес для блока данных с уставками
      /// </summary>
      public int adrForUstavBlock;
      /// <summary>
      /// адрес для блока данных с авариями
      /// </summary>
      public int adrForAvarBlock;
      /// <summary>
      /// массив контролов на вкладке уставки -  
      /// для определения факта их изменения
      /// </summary>
      public ArrayList UstavkiControls;
      /// <summary>
      /// путь к папке с файлами устройства
      /// в папке Project
      /// </summary>
      public string nfXMLConfig;
      /// <summary>
      /// имя файла с описание формы
      /// </summary>
      public string fileFrmTagsDescript;
      #endregion

      #region private
      string nfXMLConfigFC;                                 // имя файла с описанием ЩАСУ
      ushort iclm = 16;                                     // число колонок в дампе
      SortedList slLocal;
      EncodingInfo eii;
      SortedList slEncoding;
      SortedList se = new SortedList( );
      SortedList sl_tpnameUst = new SortedList( );
      StringBuilder sbse = new StringBuilder( );
      DataSet dsTagTables;
      //protected TCRZADirectDevice tcdirdev;                           // ссылка на устройство
      /// <summary>
      /// список коэф трансф. из файла PrgDevCFG.cdp 
      /// для данного устройства
      /// </summary>
      private SortedList<string,string> slKoefRatioValue = new SortedList<string,string>();
      /// <summary>
      /// список тегов для подписки/отписки
      /// </summary>
      //public List<ITag> taglist;
      #endregion

      #region конструктор
      public frmBMRZbase( )
      {
         InitializeComponent( );
      }

      public frmBMRZbase(int iFC, int iIDDev, string fXML, string ftagsxmldescript)
      {
         InitializeComponent( );
         //parent = linkMainForm;
         this.iFC = iFC;                 // номер ФК целочисленный
         strFC = iFC.ToString( );         // номер ФК строка
         this.iIDDev = iIDDev;           // номер устройства целочисленный
         strIDDev = iIDDev.ToString( );   // номер устройства строка
         string TypeName = String.Empty;
         string nameR = String.Empty;
         string nameELowLevel = String.Empty;
         string nameEHighLevel = String.Empty;

         fileFrmTagsDescript = ftagsxmldescript;
         nfXMLConfig = fXML;
			try
			{
             /*вычислим название файла с описанием устройства
              для этого используем файл PrgDevCFG.cdp
              */
             //XDocument xdoc_PrgDevCFG = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project\\Configuration\\0#DataServer\\Sources\\MOA_ECU" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp");
             //// найдем устройство
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
             //            //this.Text = nameR + " ( ид.№ " + iIDDev.ToString( ) + " )" + " - яч. №" + xedev.Element( "NumLock" ).Value;
             //            this.Text = nameR + " ( ид.№ " + iIDDev.ToString() + " )" + xedevv.Element("DescDev").Value;
             //            // заполняем список с коэф трансф для данного устройства
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
                * вычислим название файла с описанием устройства
                * для этого используем файл PrgDevCFG.cdp источника
                */

                XElement xedev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription(0, "MOA_ECU", iFC * 256 + iIDDev);
                xedev = xedev.Element("DescDev");   // подправили

                TypeName = xedev.Element("TypeName").Value;
                nameR = xedev.Element("nameR").Value;
                nameELowLevel = xedev.Element("nameELowLevel").Value;
                nameEHighLevel = xedev.Element("nameEHighLevel").Value;
                this.Text = nameR + " ( ид.№ " + iIDDev.ToString() + " )" + xedev.Element("DescDev").Value;

             //foreach ( DataSource fc in linkMainForm.KB )
             //   if ( fc.NumFC == iFC )
             //      foreach ( TCRZADirectDevice tcdd in fc.Devices )
             //         if ( tcdd.NumDev == iIDDev )
             //            tcdirdev = tcdd;

             //// на основе содержимого файла описания устройства формируем структуру вкладок на tabcontrole - tcDevTags
             //// заполняем рекурсивно
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

      #region загрузка формы
      private void frmBMRZbase_Load( object sender, EventArgs e )
      {
         if ( !DesignMode )
            timer1.Enabled = true;

         GetCCforFLP( ( ControlCollection ) this.Controls );
      }
      #endregion

      #region вход на вкладку с информацией о состоянии устройства и команд и на другие вкладки где не требуется доп подготовка данных и внешнего вида вкладки
      public void tabStatusDev_Command_Enter( object sender, EventArgs e )
      {
         /*
         * скрываем панели
         */
          if (arDopPanel != null)
            foreach ( UserControl p in arDopPanel )
                p.Visible = false;

         TabPage tp_this = ( TabPage ) sender;
         ArrayList arrVars = ( ArrayList ) slTPtoArrVars [ tp_this.Text ];
         if ( arrVars.Count != 0 )
            return;

         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, null, false );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)
      }
      #endregion

      #region вход на вкладку с информацией о состоянии ФК и его команд
      public void tabStatusFC_Enter( object sender, EventArgs e )
      {
         TabPage tp_this = ( TabPage ) sender;
         ArrayList arrVars = ( ArrayList ) slTPtoArrVars [ tp_this.Text ];
         if ( arrVars.Count != 0 )
            return;

         nfXMLConfigFC = "frm_tbpFC.xml"; // файл в папке с запускаемым файлом приложения

         // для отображения данных по ФК на время подменяем номер устройства

         string old_strIDDev = strIDDev;
         int old_iIDDev = iIDDev;
         string sfc = StrFC;

         iIDDev = iFC * 256;
         strIDDev = iIDDev.ToString( );

         arrVars.Add( "arrStatusFCCommand" ); // чтобы как-то идентифицировать необходимость формирования инф по ФК как отдельному устройству
         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, null, false );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)

         strIDDev = old_strIDDev;
         iIDDev = old_iIDDev;
      }
      #endregion

      #region вход на вкладку для просмотра доступных пакетов
      public void tbpPacketViewer_Enter( object sender, EventArgs e )
      {
         //// выбор кодировки
         //slEncoding = new SortedList( );
         //int ii = 0;
         //foreach ( EncodingInfo ei in Encoding.GetEncodings( ) )
         //{
         //   slEncoding [ ii ] = ei;
         //   cbEncode.Items.Add( "[" + ei.CodePage.ToString( ) + "]" + " : " + ei.DisplayName );
         //   if ( ei.CodePage == 866 )
         //      cbEncode.SelectedIndex = ii;    // кодировка по умолчанию
         //   ii++;
         //}
         //eii = ( EncodingInfo ) slEncoding [ cbEncode.SelectedIndex ];  //EncodingInfo

         //slLocal = new SortedList( );
         //// найдем SortedList для нужного устройства
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
         //// заполняем ComboBox
         //cbAvailablePackets.Items.Clear( );
         //for ( int i = 0 ;i < slLocal.Count ;i++ )
         //   cbAvailablePackets.Items.Add( slLocal.GetKey( i ) );
         //try
         //{
         //   cbAvailablePackets.SelectedIndex = 0;
         //}
         //catch ( Exception eee )
         //{
         //   MessageBox.Show( "Нет данных для отображения. " + eee.Message );
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

         //ushort add10 = BitConverter.ToUInt16( brP, 4 );	//читаем адрес блока данных

         //// читаем данные в массив 
         //byte[] memX = new byte [ brP.Length - 6 ];
         //System.Buffer.BlockCopy( brP, 6, memX, 0, brP.Length - 6 );

         //Encoding e = Encoding.ASCII;
         //try
         //{
         //   e = eii.GetEncoding( );
         //}
         //catch
         //{
         //   MessageBox.Show( "Ошибка при выборе кодировки" );
         //}

         //char[] arrCh = new char [ e.GetCharCount( memX, 0, memX.Length ) ];
         //e.GetChars( memX, 0, memX.Length, arrCh, 0 );

         //// формируем ListView

         //ColumnHeader ch = new ColumnHeader( );
         //ch.DisplayIndex = 0;
         //ch.Name = "clm_" + ch.DisplayIndex.ToString( "X2" );
         //ch.Text = "";
         //ch.TextAlign = HorizontalAlignment.Center;
         //ch.Width = 1;       // пустой элемент
         //lstvDump.Columns.Add( ch );

         //ch = new ColumnHeader( );
         //ch.DisplayIndex = 1;
         //ch.Name = "clmOffset_10";
         //ch.Text = "Смещ 10";
         //ch.TextAlign = HorizontalAlignment.Right;
         //ch.Width = 70;
         //lstvDump.Columns.Add( ch );

         //ch = new ColumnHeader( );
         //ch.DisplayIndex = 2;
         //ch.Name = "clmOffset_16";
         //ch.Text = "Смещ 16";
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
         //ch.Text = "Симв. строка";
         //ch.TextAlign = HorizontalAlignment.Left;
         //ch.Width = 150;
         //lstvDump.Columns.Add( ch );

         //// формируем адрес в первой колонке - переводим в шестнадцатеричное четырехзначное символьное значение
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

         //      // символьное значение
         //      try
         //      {
         //         chS = Convert.ToChar( arrCh [ i ] );
         //      }
         //      catch
         //      {
         //         MessageBox.Show( "Действие не поддерживается" );
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

         //// ширина listview
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
         //// обновить
         //lstvDump.Clear( );
         //// вывод в ListView данных пакета
         //int kl = Convert.ToInt32( cbAvailablePackets.Text );
         //object kt = slLocal [ kl ];
         ////PacketViewer_Output( ( BinaryReader ) kt, iclm );
         //PacketViewer_Output( ( byte [ ] ) kt, iclm );
      }

      private void rbClm16_CheckedChanged( object sender, EventArgs e )
      {
         //RadioButton rb = ( RadioButton ) sender;
         //if ( rb.Checked )
         //   iclm = Convert.ToUInt16( rb.Tag );  // число колонок

         //ReNew( );
      }

      private void cbEncode_SelectedIndexChanged( object sender, EventArgs e )
      {
         //eii = ( EncodingInfo ) slEncoding [ cbEncode.SelectedIndex ];  //EncodingInfo
      }
      #endregion

      #region формируем вкладки согласно подгрупп
      /// <summary>
      /// формирование вкладкок согласно подгрупп
      /// </summary>
      /// <param Name="groupname">имя группы</param>
      /// <param Name="tabpage">tabpage на кот будет располагаться теги группы</param>
      /// <param Name="arlist"></param>
      /// <param Name="pnlTP"></param>
      public void PrepareTabPagesForGroup( string groupname, TabPage tabpage, ref ArrayList arlist, Panel pnlTP, bool isClickable )
      {
         if ( !arlist.Contains( "arrStatusFCCommand" ) )
         {
            #region если данные от обычного устройства, не ФК
            if ( !File.Exists( path2PrgDevCFG ) )
               throw new Exception( "Файл не найден : " + path2DeviceCFG );

            // найдем описание группы в файле описания устройства
            XDocument xdoc_txt = XDocument.Load( path2DeviceCFG );
            //IEnumerable<XElement> xegftts = xdoc_txt.Descendants( "GroupForTheTag" );
            var xe_config = ( from x in xdoc_txt.Descendants( "GroupInDev" )
                              where x.Element( "Name" ).Value == groupname
                              select x ).DefaultIfEmpty( ).Single( );
            if ( xe_config == null )
            {
               MessageBox.Show( "Нет данных для форимирования вкладки для группы " + groupname, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
               return;
            }

            // если есть подгруппы, создаем tabcontrol
            if ( xe_config.Descendants( "SubGroup" ).Count( ) == 0 && ( ( string ) xe_config.Element( "Tags" ) == null ) )
            {
               MessageBox.Show( "Нет данных для отображения", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information );
               return;
            }

            // slnflps - сортированный список flp с именами и условными обозначениями
            /*SortedList*/ slnflps = CreateTPforGroup( xe_config, tabpage, tabpage.Text, pnlTP );

            // теперь новые flp нужно добавить в список
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

          if (tabpageName == "Уставки")
              UstavkiControls = new ArrayList();

          errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

          // размещаем динамически на форме
          for (int i = 0; i < arlist.Count; i++)
          {
              if (htFormulaEvals.ContainsKey(arlist[i]))
                  ev = (FormulaEvalNDS)htFormulaEvals[arlist[i]];
              else
              {
                  MessageBox.Show("Тег " + arlist[i] + " отсутствует в массиве тегов данной формы.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                  continue;
              }

              // смотрим категорию вкладки для размещения тега и его тип
              CheckBoxVar chBV;
              ctlLabelTextbox usTB;
              ComboBoxVar cBV;
              string key = String.Empty;

              // определяем flp
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
                      //    // пытаемся получить ключ панели другим способом
                      //    key = ev.ToP;
                      //cBV.Parent = (FlowLayoutPanel)slFLP[key];
                      //cBV.AutoSize = true;
                      //cBV.addrLinkVar = ev.addrVar;
                      //cBV.typetag = ev.tRezFormulaEval.TypeTag;
                      //cBV.addrLinkVarBitMask = ev.addrVarBitMask;
                      //#region если уставки - добавляем в список
                      //if (tabpageName == "Уставки")
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
                      //    // пытаемся получить ключ панели другим способом
                      //    key = ev.ToP;
                      //usTB.Parent = (FlowLayoutPanel)slFLP[key];
                      //usTB.AutoSize = true;
                      //usTB.addrLinkVar = ev.addrVar;
                      //usTB.typetag = ev.tRezFormulaEval.TypeTag;
                      //usTB.mask = ev.bitmask;
                      //usTB.txtLabelText.ReadOnly = true;
                      //usTB.SetErrorProvider(ref errorProvider1);

                      ////// установим положение точки -  пока глючит в уставках - закомментироуем
                      ////if (String.IsNullOrEmpty(ev.StrFormat) || ev.StrFormat == "0")
                      ////   ev.StrFormat = HMI_Settings.Precision;

                      //ev.OnChangeValForm += usTB.LinkSetText;
                      //ev.FirstValue();
                      //if (tabpageName == "Уставки")
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
                      //    // пытаемся получить ключ панели другим способом
                      //    key = ev.ToP;
                      //chBV.Parent = (FlowLayoutPanel)slFLP[key];
                      //chBV.AutoSize = true;
                      //chBV.addrLinkVar = ev.addrVar;
                      //chBV.addrLinkVarBitMask = ev.addrVarBitMask;

                      //// для уставок д.б. checkbox, а не кнопка
                      //if (tabpageName == "Уставки")
                      //    chBV.btnCheck.Visible = false;

                      //ev.OnChangeValForm += chBV.LinkSetText;
                      //ev.FirstValue();
                      //if (tabpageName == "Уставки")
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
                      MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      break;
              }
          }
      }
      #endregion

      #region Создание tabpage для группы тегов
      private void CreateTPforGroup( XElement xegftt, TabControl tc )
      {
         timer1.Enabled = false;

         //return;  // пока не используем

         TabPage tpg = new TabPage( );
         string strn = String.Empty;
         if ( String.IsNullOrEmpty( ( string ) xegftt.Element( "Name" ) ) )
            if ( !String.IsNullOrEmpty( ( string ) xegftt.Attribute( "Name" ) ) )
               strn = xegftt.Attribute( "Name" ).Value;
            else
               throw new Exception( "Нет названия подгруппы" );
         else
            strn = xegftt.Element( "Name" ).Value;

         tpg.Text = strn;
         tc.TabPages.Add( tpg );

         // есть ли теги вне групп
         if ( !String.IsNullOrEmpty( ( string ) xegftt.Element( "Tags" ) ) )
         {
            IEnumerable <XElement> xetwog = xegftt.Element( "Tags" ).Elements( "Tag" );
            if ( xetwog.Count( ) != 0 )
            {
               // проверим есть ли подгруппы, если нет то tabpage для тегов вне групп не создаем
               IEnumerable <XElement> xetg = xegftt.Elements( "SubGroup" );
               if ( xetg.Count( ) == 0 )
                  CreateDataGridView( xegftt, tpg );// если подгрупп нет, то формируем DataGridView для тегов подгруппы
               else
               {
                  TabControl tctp = new TabControl( );
                  tctp.Dock = DockStyle.Fill;
                  tpg.Controls.Add( tctp );
                  TabPage ntp = new TabPage( );
                  ntp.Text = "Общие данные по аварии";   // ранее было Теги вне подгрупп
                  tctp.Controls.Add( ntp );
                  CreateDataGridView( xegftt.Element( "Tags" ), ntp );// если подгруппы есть, то формируем DataGridView для тегов подгруппы
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

            //   CreateDataGridView( xegftt, tpg );// если подгрупп нет, то формируем DataGridView для тегов подгруппы
         }
         timer1.Enabled = true;
      }

      /// <summary>
      /// создание вкладок для подгрупп на tabpage или панели
      /// </summary>
      /// <param Name="xe"></param>
      /// <param Name="tabpage">tabpage для размещения вкладок, если не задана panel</param>
      /// <param Name="pnlParent">панель для размещения вкладок</param>
      /// <returns></returns>
      public SortedList CreateTPforGroup( XElement xe, TabPage tabpage, string prefixForFLP, Panel pnlParent )
      {
         // извлечем названия панелей из Device.cfg
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

         /* сформируем TabPage с учетом
          * структуры групп\подгурпп 
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
                  TabPage tptwog = new TabPage("Общие данные по аварии");   // ранее было Теги вне подгрупп
                  tptwog.Controls.Add( flp );
                  flp.Parent = tptwog;
                  flp.Dock = DockStyle.Fill;
                  flp.Name = ( string ) slnflps.GetKey( slnflps.IndexOfValue( flp.Caption ) );  //               tptwog.Name + "_" + "TagsWOGroups";
                  tc.TabPages.Add( tptwog );
               }
               else
                  //MessageBox.Show( "FLP с именем = " + flp.Caption + " = отсутсвует в device.cfg.\n Добавьте ее вручную.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
                  Console.WriteLine("(758) FLP с именем = " + flp.Caption + " = отсутсвует в device.cfg.\n Добавьте ее вручную.");
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
                  // в понятное обозначение flp забиваем название группы и подгруппы
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
                     //MessageBox.Show( "FLP с именем = " + flp.Caption + " = отсутсвует в device.cfg.\n Добавьте ее вручную.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
                     Console.WriteLine("(784) FLP с именем = " + flp.Caption + " = отсутсвует в device.cfg.\n Добавьте ее вручную.");
                     continue;
                  }
               }
               tc.TabPages.Add( tptwog );
            }
         }
         else if ( !String.IsNullOrEmpty( ( string ) xe.Element( "Tags" ) ) )
         {
            // если подгрупп нет, а есть только теги вне групп, то формируем flp прямо на tabpage
            MTRANamedFLPanel flp = new MTRANamedFLPanel( );
            if ( pnlParent == null )
               tabpage.Controls.Add( flp );
            else
               pnlParent.Controls.Add( flp );

            flp.Parent = tabpage;
            flp.Dock = DockStyle.Fill;
            flp.Caption = prefixForFLP;   // + "#" + "TagsWOGroups"
            // проверим существование вкладки
            if ( slnflps.ContainsValue( flp.Caption ) )
               flp.Name = ( string ) slnflps.GetKey( slnflps.IndexOfValue( flp.Caption ) );
            else
            {
               //MessageBox.Show( "FLP с именем = " + flp.Caption + " = отсутсвует в device.cfg.\n Добавьте ее вручную.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
               Console.WriteLine("(809) FLP с именем = " + flp.Caption + " = отсутсвует в device.cfg.\n Добавьте ее вручную.");

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
      /// добавление новых FLP в список (для возможности последующего обращения по именам)
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
      /// создание массива ArrayList с описанием переменных по содержимому файла XML
      /// </summary>
      /// <param Name="arrVar"> массив  ArrayList
      ///фигуры</param>
      /// <param Name="nameFile">имя файла XML
      ///фигуры</param>
      public ArrayList CreateArrayList( /*ref ArrayList arrVar,*/ string name_arrVar )
      {
         // чтение XML
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
      /// создание массива ArrayList с описанием переменных по содержимому файла XML
      /// </summary>
      /// <param Name="xegr"></param>
      /// <param Name="name_arrVar">название секции с тегами в файле формы frmXXX.xml</param>
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
             // формируем элементы формулы
             sl["formula"] = xef.Attribute("express").Value;
             sl["caption"] = xef.Attribute("Caption").Value;
             sl["dim"] = xef.Attribute("Dim").Value;
             sl["TypeOfTag"] = xef.Attribute("TypeOfTag").Value;
             sl["TypeOfPanel"] = xef.Attribute("TypeOfPanel").Value;
             if (String.IsNullOrEmpty((string)xef.Attribute("ReadWrite")))
                 sl["ReadWrite"] = "wr"; // по умолчанию тег доступен по чт/зап
             else
                 sl["ReadWrite"] = xef.Attribute("ReadWrite").Value;

             // посмотрим опрдеделена ли точность числа - положение точки
             if (String.IsNullOrEmpty((string)xef.Attribute("PosComma")))
                 sl["Precision"] = "-1"; // положение точки - по факту
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
                     MessageBox.Show("Нет такого типа сигнала");
                     break;
             }
             ToP = (string)sl["TypeOfPanel"];

             // читаем теги
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
         //        case "Уставки":
         //            break;
         //        case "arrStatusDevCommand":
         //            break;
         //        case "arrStatusFCCommand":
         //            break;
         //        case "Срабатывание":
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
             throw new Exception("Реакция на simple_eval не определена");
         }

         //// подписываемся на обновление тегов с DataServer
         //taglist = new List<ITag>();

         //foreach (FormulaEvalNDS fee in htFormulaEvals.Values)
         //{
         //    if (fee.LinkVariableNewDS == null)
         //        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 428, string.Format("() : frmBMRZ.cs : CreateArrayList() : Нет привязки к тегу = {0}", fee.CaptionFE));
         //    else
         //        taglist.Add(fee.LinkVariableNewDS);
         //}
         //HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags(taglist);

         return arrVars;
      }
      #endregion

      #region контрольная печать DataSet
      static void PrintDataSet( DataSet ds )
      {
         // метод выполняет цикл по всем DataTable данного DataSet
         Console.WriteLine( "Таблицы в DataSet '{0}'. \n ", ds.DataSetName );
         foreach ( DataTable dt in ds.Tables )
         {
            Console.WriteLine( "Таблица '{0}'. \n ", dt.TableName );
            // вывод имен столбцов
            for ( int curCol = 0 ;curCol < dt.Columns.Count ;curCol++ )
               Console.Write( dt.Columns [ curCol ].ColumnName.Trim( ) + "\t" );
            Console.WriteLine( "\n-----------------------------------------------" );

            // вывод DataTable
            for ( int curRow = 0 ;curRow < dt.Rows.Count ;curRow++ )
            {
               for ( int curCol = 0 ;curCol < dt.Columns.Count ;curCol++ )
                  Console.Write( dt.Rows [ curRow ] [ curCol ].ToString( ) + "\t" );
               Console.WriteLine( );
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

      //#region вывод пакета pack в hex-виде в файд fn
      //public void PrintHexDump( string fn, byte [ ] pack )
      //{
      //   // выведем в файл - текущий каталог
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
      //      // начинаем строку счетчиком адреса
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

      #region actDellstV - действия с ListView
      delegate void SetLVCallback( ListViewItem li, bool actDellstV );

      // actDellstV - действия с ListView : false - не трогать; true - очистить;
      public void LinkSetLV( object Value, bool actDellstV )
      {
         //if ( !( Value is ListViewItem ) && !actDellstV )
         //   return;   // сгенерировать ошибку через исключение

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

      #region Печать массива тегов
      private void btnPrint_Click( object sender, EventArgs e )
      {
         //PrintHMI frmPrt = new PrintHMI( );
         //StringBuilder sb = new StringBuilder( );
         //;
         //ListViewItem li;

         //// перебираем содержимое всех строк lstvDump
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
      ///     Реализация пункта меню Предварительный просмотр
      /// </summary>
      private void mnuPrintPreview_Click( object sender, EventArgs e )
      {
         //PrintArr( );
         //parent.mnuPrintPreview_Click( sender, e );
      }

      /// <summary>
      /// mnuPrint_Click( object sender, EventArgs e )
      ///     Реализация пункта меню Печать
      /// </summary>
      private void mnuPrint_Click( object sender, EventArgs e )
      {
         //PrintArr( );
         //parent.mnuPrint_Click( sender, e );
      }

      /// <summary>
      /// PrintArr()
      ///     Печать массива переменных
      /// </summary>
      private void PrintArr( )
      {
         //StringBuilder sb = new StringBuilder( );
         //float f_val;
         //int i_val;
         //string t_val = "";
         //ArrayList arCurPrt = new ArrayList( );

         //object val;

         //// определяем активную вкладку
         //TabPage tp_sel = tc_Main.SelectedTab;

         //sb.Length = 0;

         //// формируем заголовок листинга
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
         //            t_val = f_val.ToString( "F2" ); // две цифры после запятой
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
      ///     формирование строки для печати для отдельной переменной
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
      //   //         t_val = f_val.ToString( "F2" ); // две цифры после запятой
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
      //   //         t_val = f_val.ToString( "F2" ); // две цифры после запятой
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

      #region обновление сводного перечня тегов устройства
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

      #region разбор пакетов с аварийной информацией или уставкми из базы
      public void ParseBDPacket( byte [ ] pack, int adr, int numgr )
      {
         //CommonUtils.CommonUtils.PrintHexDump("frmBMRZbase.cs : ParseBDPacket", "LogHexPacket.dat", pack, GetAdrBlockData(path2DeviceCFG, numgr), iFC, iIDDev);  // выведем в файл для контроля

         //if( adr == -1 )
         //{
         //   MessageBox.Show( "Адрес для архивной группы не определен", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
         //   return;
         //}

         //parent.newKB.PacketToQueDev( pack, Convert.ToUInt16(adr), iFC, iIDDev ); // 10280 пакет  по адресу  устройства
         //// объявить соответсвующую группу переменных архивной
         //SetArhivGroupInDev(iFC, iIDDev, numgr );
      }
      #endregion

      /// <summary>
      /// получить адрес блока с аварией или уставками для данного устройства
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
         return -1;  // ничего не найдено
      }

       /// <summary>
       /// заполнить вкладку с информацией о состоянии устройства
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

          //    // интеграция всех возвращаемых ошибок
          //    foreach( SqlError connectError in ex.Errors )
          //        errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
          //    parent.WriteEventToLog( 21, "Нет связи с БД (UstavBD): " + errorMes, this.Name, false );
          //    System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMRZ : Нет связи с БД (UstavBD)" );
          //    asqlconnect.Close( );
          //    return;
          //} catch( Exception ex )
          //{
          //    MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );
          //    asqlconnect.Close( );
          //    return;
          //}
          //// формирование данных для вызова хранимой процедуры
          //SqlCommand cmd = new SqlCommand( "GetBlockInfo", asqlconnect );
          //cmd.CommandType = CommandType.StoredProcedure;

          //// входные параметры
          //// id устройства
          //SqlParameter pidBlock = new SqlParameter( );
          //pidBlock.ParameterName = "@BlockId";
          //pidBlock.SqlDbType = SqlDbType.Int;
          //pidBlock.Value = IFC * 256 + IIDDev;//IIDDev;
          //pidBlock.Direction = ParameterDirection.Input;
          //cmd.Parameters.Add( pidBlock );

          //// заполнение DataSet
          //DataSet aDS = new DataSet( "ptk" );
          //SqlDataAdapter aSDA = new SqlDataAdapter( );
          //aSDA.SelectCommand = cmd;

          ////aSDA.sq
          //aSDA.Fill( aDS, "TbInfo" );

          //asqlconnect.Close( );

          ////PrintDataSet( aDS );
          //// извлекаем данные
          //DataTable dtI = aDS.Tables [ "TbInfo" ];

          //// заполняем RichTextBox
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
      /// Идентификатор блока
      /// </summary>
      public uint Guid { get { return (uint)( iFC * 256 + iIDDev ); } }
   }
}