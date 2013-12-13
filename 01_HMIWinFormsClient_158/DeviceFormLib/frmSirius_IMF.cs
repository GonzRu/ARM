using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Calculator;
using LabelTextbox;
using CRZADevices;
using CommonUtils;
using System.Xml.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using FileManager;
using LibraryElements;
using WindowsForms;
using Structure;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace HMI_MT
{
   public partial class frmSirius_IMF : frmBMRZbase
   {
      #region Public
      public ArrayList arrDeviceInfo = new ArrayList( );
      #endregion

      #region private
      // hashTable для хранения строк событий и ключей доступа к ним
      Hashtable htStringEvent;
      uint lenMaskInModbusWords;
      // сортированный список с именами панелей и фреймов
      SortedList DevPanelTypes;
      SortedList slLocal;

      // нижние панели
      ConfigPanelControl   pnlConfig;
      SrabatPanelControl   pnlSrabat;
      CurrentPanelControl  pnlCurrent;
      OscDiagPanelControl  pnlOscDiag;
      LogDevPanelControl   pnlLogDev;

      DataTable dtO;    // таблица с осциллограммами
      DataTable dtG;    // таблица с диаграммами
      DataTable dtA;    // таблица с авариями
      DataTable dtU;    // таблица с уставками
      DataTable dtLD;    // журнал событий блока
      #endregion

      #region Конструкторы
      public frmSirius_IMF( )
      {
         InitializeComponent( );
      }
      public frmSirius_IMF( MainForm linkMainForm, int iFC, int iIDDev, string fxml )
         : base( linkMainForm, iFC, iIDDev, fxml )
      {
         InitializeComponent( );

         //переупорядочим вкладки, отодвинув базовые назад
         ArrayList artp = new ArrayList( );

         foreach ( TabPage tp in tc_Main_frmBMRZbase.TabPages )
         {
            artp.Add( tp );
         }

         int i = artp.Count - 1;

         tc_Main_frmBMRZbase.Multiline = true;  // отображение корешков в несколько рядов

         foreach ( TabPage tp in artp )
         {
            tc_Main_frmBMRZbase.TabPages [ i ] = tp;
            i--;
         }
      }
      #endregion

      #region Load
      private void frmSirius_IMF_Load( object sender, EventArgs e )
      {
         tabpageControl.Enter += new EventHandler( tabpageControl_Enter );
         slTPtoArrVars.Add( tabpageControl.Text, new ArrayList( ) );

         //tabStatusDev_Command.Enter += new EventHandler(tabStatusDev_Command_Enter);
         //slTPtoArrVars.Add(tabStatusDev_Command.Text, new ArrayList());

         //tabpageDeviceInfo.Enter += new EventHandler( tabStatusDev_Command_Enter );
         //slTPtoArrVars.Add( tabpageDeviceInfo.Text, new ArrayList( ) );

         tabpageConfig.Enter += new EventHandler( tabpageConfig_Enter );
         slTPtoArrVars.Add( tabpageConfig.Text, new ArrayList( ) );

         tabpageSrabat.Enter += new EventHandler( tabpageSrabat_Enter );
         slTPtoArrVars.Add( tabpageSrabat.Text, new ArrayList( ) );

         //tbpPacketViewer.Enter += new EventHandler( tbpPacketViewer_Enter );

         #region по источнику и номеру устройства опрделяем пути к файлам 
         path2PrgDevCFG = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";
         XDocument xdoc = XDocument.Load( path2PrgDevCFG );
         IEnumerable<XElement> xes = xdoc.Descendants( "FC" );
         var xe = ( from nn in
                       ( from n in xes
                         where n.Attribute( "numFC" ).Value == StrFC
                         select n ).Descendants( "Device" )
                    where nn.Element("NumDev").Value == IIDDev.ToString()  //( IIDDev - ( 256 * IFC ) )
                    select nn ).First( );

         path2DeviceCFG = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + xe.Element( "nameR" ).Value + Path.DirectorySeparatorChar + "Device.cfg";
         path2FrmDev = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + xe.Element( "nameR" ).Value + Path.DirectorySeparatorChar + "frm" + xe.Element( "nameELowLevel" ).Value + ".xml";
         if ( !File.Exists( path2DeviceCFG ) )
         {
            MessageBox.Show( "Файл =" + path2DeviceCFG + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }
         if ( !File.Exists( path2FrmDev ) )
         {
            MessageBox.Show( "Файл =" + path2FrmDev + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }
         #endregion

         // формируем сортированный список с панелями
         xdoc = XDocument.Load( path2DeviceCFG );
         DevPanelTypes = new SortedList( );

         if ( !String.IsNullOrEmpty( ( string ) xdoc.Element( "Device" ).Element( "TypeOfPanelSections" ) ) )
         {
            IEnumerable<XElement> etypes = xdoc.Element( "Device" ).Element( "TypeOfPanelSections" ).Elements( "TypeOfPanel" );

            foreach ( XElement xr in etypes )
               // определим вариант формата секции TypeOfPanel
               if ( ( string ) xr.Element( "Name" ) == null )
                  DevPanelTypes.Add( xr.Value, String.Empty );
               else
                  DevPanelTypes.Add( xr.Element( "Name" ).Value, xr.Element( "Caption" ).Value );
         }
         else
            MessageBox.Show( "Типы панелей в файле Device.cfg отсутсвуют", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning );

         GetCCforFLP( ( ControlCollection ) this.Controls );

         // заголовок формы
         //this.Text = xe.Element( "nameR" ).Value + " ( ид.№ " + this.IIDDev.ToString( ) + " )"; // + " " + rr.cwInfo.strRefDesign+ " ( ид.№ " + rr.cwInfo.idDev + " ) - яч. № " + rr.cwInfo.nLoc

         // создаем нижние панели
         CreateTabPanel( );

         #region устанавливаем пикеры для вывода аварийной информации за последние сутки
		 pnlSrabat.dtpEndDateAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		 pnlSrabat.dtpEndTimeAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		 pnlSrabat.dtpStartDateAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
         TimeSpan ts = new TimeSpan( 3, 0, 0, 0 );
         pnlSrabat.dtpStartDateAvar.Value = pnlSrabat.dtpStartDateAvar.Value - ts;
		 pnlSrabat.dtpStartTimeAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
         #endregion

         #region устанавливаем пикеры для вывода осциллограмм и диаграмм за последние сутки
		 pnlOscDiag.dtpEndData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		 pnlOscDiag.dtpEndTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		 pnlOscDiag.dtpStartData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
         ts = new TimeSpan( 1, 0, 0, 0 );
         pnlOscDiag.dtpStartData.Value = pnlOscDiag.dtpStartData.Value - ts;
		 pnlOscDiag.dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
         #endregion

         #region устанавливаем пикеры для вывода последнего журнала устройства из БД
		 pnlLogDev.dtpEndData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		 pnlLogDev.dtpEndTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		 pnlLogDev.dtpStartData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
         ts = new TimeSpan( 360, 0, 0, 0 );
         pnlLogDev.dtpStartData.Value = pnlLogDev.dtpStartData.Value - ts;
		 pnlLogDev.dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
		 #endregion
      }
      #endregion

      #region Обработчики входов на вкладки
      #region Текущая
      void tabpageControl_Enter( object sender, EventArgs e )
      {
          splitContainer1.Panel2Collapsed = true;
         /*
          * скрываем панели
          */
         foreach ( UserControl p in arDopPanel )
            p.Visible = false;

         //pnlCurrent.Visible = true;

         TabPage tp_this = ( TabPage ) sender;
         ArrayList arrVars = ( ArrayList ) slTPtoArrVars [ tp_this.Text ];
         if ( arrVars.Count != 0 )
            return;

         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, null/*pnlTPControl*/ );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)
         PrepareAdditionalFLP( pnlCurrent.Controls );
      }
      
      /// <summary>
      /// сформировать виз элементы на доп панелях MTRANamedFLPanel, кот принадлежат некоторому контролу
      /// </summary>
      private void PrepareAdditionalFLP(Control.ControlCollection cntrlCC )
      {
         // новые flp нужно добавить в список, скорее всего они там уже есть
         GetCCforFLP( cntrlCC );

         /* 
          * читаем файл с доп панелями, имя файла или файлов
          * извлекаем из frmxxx.xml,
          * формируем arrList для каждой из них, 
          * добавляем их в slTPtoArrVars
          * создаем виз элементы для формул
          * и отображаем
          */
         if ( !File.Exists( path2FrmDev ) )
            throw new Exception( "Файл не найден : " + path2FrmDev );

         XDocument xdoc_frm = XDocument.Load( path2FrmDev );
         string faddname = String.Empty;

         if ( !String.IsNullOrEmpty( ( string ) xdoc_frm.Element( "MT" ).Element( "FileAdditionalFLP" ) ) )
            faddname = xdoc_frm.Element( "MT" ).Element( "FileAdditionalFLP" ).Element( "Name" ).Value;
         else
         {
            MessageBox.Show( "В файле описания формы нет ссылки на доп панель", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }

         faddname = Path.GetDirectoryName( path2FrmDev ) + Path.DirectorySeparatorChar + faddname;

         if ( !File.Exists( faddname ) )
         {
            MessageBox.Show( "Файл с описанием доп панели не найден : " + faddname, this.Name,MessageBoxButtons.OK,MessageBoxIcon.Warning);
            return;
         }

         XDocument xdoc_addflp;
         try
         {
            xdoc_addflp = XDocument.Load( faddname );
         }
         catch ( Exception e )
         {
            throw new Exception( "Ошибка в формате xml-документа: " + faddname );
         }         

         IEnumerable<XElement> flpframes = xdoc_addflp.Element( "MT" ).Element( "AdditionalFLP" ).Elements( "FLPframe" );

         foreach ( XElement flpframe in flpframes )
         {
            // формируем массив формул
            ArrayList arrf = GetArrFrmls( flpframe, String.Empty );

            // отображаем
            PlaceVisElemOnForm( "", flpframe.Attribute( "MTFLPNameR" ).Value, arrf );
         }
      }
	   #endregion
      
      #region Срабатывание
      void tabpageSrabat_Enter( object sender, EventArgs e )
      {
          splitContainer1.Panel2Collapsed = false;
         /*
         * скрываем панели
         */
         foreach ( UserControl p in arDopPanel )
            p.Visible = false;

         pnlSrabat.Visible = true;

         lstvAvar.Items.Clear( );

         AvarBD( );

         //-------------------------------------------------------------------
         //готовим инф. для отображения текущих значений аналоговых и дискретных сигналов
         TabPage tp_this = ( TabPage ) sender;
         ArrayList arrVars = ( ArrayList ) slTPtoArrVars [ tp_this.Text ];
         if ( arrVars.Count != 0 )//arrStatusDevCommand
            return;

         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, pnlTPSpab );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)
      }

      void btnResetValues_Click( object sender, EventArgs e )
      {
         parent.newKB.ResetGroup( IFC, IIDDev, 14 );
      }

      void btnReNew_Click( object sender, EventArgs e )
      {
         AvarBD( );
         //pnlSrabat.tcAvarBottomPanel.SelectTab( 0 );
      }
      #endregion

      #region Уставки
      void tabpageConfig_Enter( object sender, EventArgs e )
      {
          splitContainer1.Panel2Collapsed = false;

         #region устанавливаем пикеры для изменения уставок за последние сутки
		  pnlConfig.dtpEndDateConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		  pnlConfig.dtpEndTimeConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		  pnlConfig.dtpStartDateConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

         TimeSpan ts = new TimeSpan( 1, 0, 0, 0 );
         pnlConfig.dtpStartDateConfig.Value = pnlConfig.dtpStartDateConfig.Value - ts;
		 pnlConfig.dtpStartTimeConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
         #endregion

         /*
          * скрываем панели
          */
         foreach ( UserControl p in arDopPanel )
            p.Visible = false;

         pnlConfig.Visible = true;

         TabPage tp_this = ( TabPage ) sender;
         ArrayList arrVars = ( ArrayList ) slTPtoArrVars [ tp_this.Text ];
         if ( arrVars.Count != 0 )
            return;

         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, pnlTPConfig );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)

         UstavBD( );
      }

      void btnReNewUstBD_Click( object sender, EventArgs e )
      {
         UstavBD( );
         //pnlConfig.tcUstConfigBottomPanel.SelectTab( 0 );
      }

      void btnReadConfig_Click( object sender, EventArgs e )
      {
         if ( parent.newKB.ExecuteCommand( IFC, IIDDev, "RCP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, this ) )
				parent.WriteEventToLog(35, "Команда \"RCP\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false );

         // документирование действия пользователя
			parent.WriteEventToLog(7, IIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда RCP - чтения уставок."

			HMI_Settings.ClientDFE.SetReq4PeriodicPacketQuery(IFC, IIDDev, 14);
      }

      void btnReadUstFC_Click( object sender, EventArgs e )
      {
         if ( parent.newKB.ExecuteCommand( IFC, IIDDev, "IMP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, this ) )
				parent.WriteEventToLog(35, "Команда \"IMP\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false );

         // документирование действия пользователя
			parent.WriteEventToLog(7, IIDDev.ToString(), this.Name, true);//, true, false );//"выдана команда IMP - чтения уставок."

			HMI_Settings.ClientDFE.SetReq4PeriodicPacketQuery(IFC, IIDDev, 14);
      }

      void btnWriteConfig_Click( object sender, EventArgs e )
      {
         if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, parent.UserRight ) )
            return;

         if ( parent.isReqPassword )
            if ( !parent.CanAction( ) )
            {
               MessageBox.Show( "Выполнение действия запрещено" );
               return;
            }

         DialogResult dr = MessageBox.Show( "Записать уставки?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
         if ( dr == DialogResult.No )
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
         byte[] aTmp2 = new byte [ 2 ];

         // найдем SortedList для нужного устройства
         slLocal = new SortedList( );
         foreach ( FC aFC in parent.KB )
            if ( aFC.NumFC == IFC ) //iFC
            {
               foreach ( TCRZADirectDevice aDev in aFC )
                  if ( aDev.NumDev == IIDDev ) //iIDDev
                  {
                     slLocal = aDev.CRZAMemDev;
                     break;
                  }
               break;
            }

         int lenpack = 0;
         try
         {
            lenpack = BitConverter.ToInt16( ( byte [ ] ) slLocal [ 60200 ], 0 );
         }
         catch ( ArgumentNullException ex )
         {
            MessageBox.Show( "Нет данных для записи: " + ex.Message + ". \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information );
            return;
         }

         short numdev = BitConverter.ToInt16( ( byte [ ] ) slLocal [ 60200 ], 2 );

         ushort add10 = BitConverter.ToUInt16( ( byte [ ] ) slLocal [ 60200 ], 4 );	//читаем адрес блока данных
         
         byte[] memX = new byte [ lenpack - 6 ];   // 6 байт - номер устройства, длина пакета, адрес блока данных

         System.Buffer.BlockCopy( ( byte [ ] ) slLocal [ 60200 ], 14, memX, 0, lenpack - 14 );//6 <-> 6+8 - 8 байт - два времени (см. протокол)

         TabControl tbkConfig = new TabControl( );

         //foreach ( Control cc in tabpageConfig.Controls )
         //   if ( cc is TabControl )
         //   {
         //      tbkConfig = ( TabControl ) cc;
         //      break;
         //   }

         foreach ( Control cc in tabpageConfig.Controls )
            if ( cc.Name == "splitContainer_tpConfig" )
            {
               SplitContainer sc = (SplitContainer)cc;
               foreach ( Control ccc in sc.Panel2.Controls )
                  if ( ccc is Panel )
                  {
                     Panel pnl = ( Panel ) ccc;
                     foreach ( Control cccc in pnl.Controls )
                        if ( cccc is TabControl )
                        {
                           tbkConfig = ( TabControl ) cccc;
                           break;
                        }
                  }
            }

         // смотрим изменения
         for (int i = 0; i < tbkConfig.Controls.Count; i++)
         {
            if (tbkConfig.Controls[i] is TabPage)
            {
               tp = (TabPage)tbkConfig.Controls[i];
               for (int j = 0; j < tp.Controls.Count; j++)
               {
                  if (tp.Controls[j] is FlowLayoutPanel)
                  {
                     flp = (FlowLayoutPanel)tp.Controls[j];
                     for (int n = 0; n < flp.Controls.Count; n++)
                     {
                        if (flp.Controls[n] is ctlLabelTextbox)
                        {
                           ultb = (ctlLabelTextbox)flp.Controls[n];
                           if (ultb.isChange)
                           {
                              ultb.isChange = false;  // сбросим флаг индикации изменения уставки

                              //if ( !CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 60204 ) )
                              //   return;
                              /*
                               * для сириусов особенность в работе с уставками:
                               * значения уставок хранятся в uint16, если есть коэф трансф, то после умножения 
                               * разряды могут терятся, значение искажается, 
                               * чтобы восстановить его, требуется заново умножить новое значение на на 
                               * коэф трансформации
                               */
                              switch (ultb.typetag)
                              {
                                 case "TUIntVariable":
                                    UInt16 tu16 = 0;
                                    CultureInfo cinfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
                                    RezFormulaEval rfe = (RezFormulaEval)ultb.Tag;

                                    // выясним есть ли у поля коэф. трансф.
                                    if (!String.IsNullOrEmpty(rfe.strKTR))
                                    {
                                       // проверим десятичный разделитель
                                       if (ultb.TextboxText.Contains("."))
                                          cinfo.NumberFormat.NumberDecimalSeparator = ".";
                                       else if (ultb.TextboxText.Contains(","))
                                          cinfo.NumberFormat.NumberDecimalSeparator = ",";

                                       Thread.CurrentThread.CurrentCulture = cinfo;

                                       float fltmp = Convert.ToSingle(ultb.TextboxText);

                                       fltmp = fltmp * Convert.ToUInt16(rfe.strKTR);
                                       tu16 = Convert.ToUInt16(fltmp);
                                    }
                                    else
                                       tu16 = Convert.ToUInt16(ultb.TextboxText);

                                    byte[] tmask = new byte[2];
                                    tmask = BitConverter.GetBytes(tu16);
                                    // запоминаем изменения
                                    ainmemX = (ultb.addrLinkVar - 60204) * 2;//baseAdr

                                    memX[ainmemX] = tmask[1];
                                    memX[ainmemX + 1] = tmask[0];

                                    break;
                              }
                              isUstChange = true;
                           }
                        }
                        else if (flp.Controls[n] is ComboBoxVar)
                        {
                           cbTmp = (ComboBoxVar)flp.Controls[n];
                           if (cbTmp.isChange)
                           {
                              isUstChange = true;
                              cbTmp.isChange = false;  // сбрасываем признак изменения у конкретного ComboBoxVar'а
                              // записываем изменения по ComboBoxVar'ам в исходный пакет (корректируем массив memX)
                              uint a = cbTmp.addrLinkVar; // адрес переменной
                              // получим значение
                              int st = cbTmp.cbVar.SelectedIndex;
                              byte[] bst = new byte[4];
                              bst = BitConverter.GetBytes(st);
                              Buffer.BlockCopy(bst, 0, aTmp2, 0, 2);
                              Array.Reverse(aTmp2);
                              // запоминаем изменения
                              ainmemX = (a - 60204) * 2;//60200 <> 60204
                              Buffer.BlockCopy(aTmp2, 0, memX, (int)ainmemX, 2);
                           }
                        }
                        else if (flp.Controls[n] is CheckBoxVar)
                        {
                           chbTmp = (CheckBoxVar)flp.Controls[n];
                           if (chbTmp.isChange)
                           {
                              isUstChange = true;
                              chbTmp.isChange = false;    // сбрасываем признак изменения у конкретного CheckBoxVar'а
                              // извлечем битовое поле из исходного массива
                              ainmemX = (chbTmp.addrLinkVar - 60200) * 2;   // это адрес
                              //aTmp2 = new byte[2];
                              Buffer.BlockCopy(memX, (int)ainmemX, aTmp2, 0, 2);
                              string bitmask = chbTmp.addrLinkVarBitMask;
                              UInt16 ibitmask = Convert.ToUInt16(chbTmp.addrLinkVarBitMask, 16);
                              Array.Reverse(aTmp2);
                              UInt16 rezbit = BitConverter.ToUInt16(aTmp2, 0);
                              if (chbTmp.checkBox1.Checked == true)
                                 rezbit = Convert.ToUInt16(rezbit | ibitmask);
                              else
                              {
                                 UInt16 ti = (UInt16)~ibitmask; //Convert.ToUInt16()
                                 rezbit = Convert.ToUInt16(rezbit & ~ibitmask);
                              }
                              // записать на место
                              aTmp2 = BitConverter.GetBytes(rezbit);
                              Array.Reverse(aTmp2);
                              Buffer.BlockCopy(aTmp2, 0, memX, (int)ainmemX, 2);
                           }
                        }
                     }
                  }
               }
            }
         }
         //------------------------------
         #region пока для сириуса закомментировали
         //// аналогично для панели уставок
         ////for( int n = 0 ; n < pnlConfig.Controls.Count ; n++ )
         //for ( int n = 0 ;n < Config_BottomPanel.Controls.Count ;n++ )
         //   //if( pnlConfig.Controls[n] is ctlLabelTextbox )
         //   if ( ( Config_BottomPanel.Controls [ n ] as ctlLabelTextbox ) != null )
         //   {
         //      ultb = ( ctlLabelTextbox ) Config_BottomPanel.Controls [ n ];
         //      if ( ultb.Name == "ctlTimeUstavkiSbros" )
         //         continue;

         //      if ( ultb.isChange )
         //      {
         //         if ( !CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 ) )
         //            return;
         //         isUstChange = true;
         //      }
         //   }
         #endregion
         //------------------------------
         if ( !isUstChange )
         {
            MessageBox.Show( "Уставки не изменялись. \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information );
            return;
         }
         // формируем пакет и команду для отправки изменения уставок
         //byte[] memXOut = new byte [ memX.Length ];
         byte[] memXOut = new byte [ memX.Length + 4 ];

         // размер д.б. не менее 196 слов = 196 * 2 = 392 байта
         if ( memXOut.Length < 392 )
            memXOut = new byte [ 392 ];

         StringBuilder cmd2 = new StringBuilder( );
         cmd2.Append( "PN1\0" );
         byte[] scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString( ) );
         Buffer.BlockCopy( scmd2, 0, memXOut, 0, 4 );
         Buffer.BlockCopy( memX, 0, memXOut, 4, memX.Length );
         //Buffer.BlockCopy( memX, 4, memXOut, 4, memX.Length - 4 );  // Handle пока нулевой

         if ( parent.newKB.ExecuteCommand( IFC, IIDDev, "WCP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
				parent.WriteEventToLog(35, "Команда \"WCP\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false );
         // документирование действия пользователя
			parent.WriteEventToLog(6, IIDDev.ToString(), this.Name, true);//, true, false );			//"выдана команда WCP - запись уставок."
         isUstChange = false;
      }
      #endregion

      #region Диаграммы и осщиллограммы
      private void tabPageOscDiag_Enter( object sender, EventArgs e )
      {
          splitContainer1.Panel2Collapsed = false;

         foreach ( UserControl p in arDopPanel )
            p.Visible = false;

         pnlOscDiag.Visible = true;

         DiagBD( );
         OscBD( );                                  
      }

      void btnReNewOscDg_Click( object sender, EventArgs e )
      {
         DiagBD( );
         OscBD( );                                  
      }

      #region Осциллограммы и диаграммы (запросы из БД)
      /// <summary>
      /// получение осциллограмм из базы
      /// </summary>
      public void OscBD( )
      {
         dgvOscill.Rows.Clear( );
         // получение строк соединения и поставщика данных из файла *.config
         //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
         SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
         try
         {
            asqlconnect.Open( );
         }
         catch ( SqlException ex )
         {
            string errorMes = "";
            // интеграция всех возвращаемых ошибок
            foreach ( SqlError connectError in ex.Errors )
               errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
				parent.WriteEventToLog(21, "Нет связи с БД (OscBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
            System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMRZ : Нет связи с БД (OscBD)" );
            asqlconnect.Close( );
            return;
         }
         catch ( Exception ex )
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
         pidBlock.Value = IFC * 256 + IIDDev;//IIDDev;;
         pidBlock.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pidBlock );

         // 3. начальное время
         SqlParameter dtMim = new SqlParameter( );
         dtMim.ParameterName = "@dt_start";
         dtMim.SqlDbType = SqlDbType.DateTime;
         TimeSpan tss = new TimeSpan( 0, pnlOscDiag.dtpStartData.Value.Hour - pnlOscDiag.dtpStartTime.Value.Hour, pnlOscDiag.dtpStartData.Value.Minute - pnlOscDiag.dtpStartTime.Value.Minute, pnlOscDiag.dtpStartData.Value.Second - pnlOscDiag.dtpStartTime.Value.Second );
         DateTime tim = pnlOscDiag.dtpStartData.Value - tss;
         dtMim.Value = tim;
         dtMim.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMim );

         // 2. конечное время
         SqlParameter dtMax = new SqlParameter( );
         dtMax.ParameterName = "@dt_end";
         dtMax.SqlDbType = SqlDbType.DateTime;
         tss = new TimeSpan( 0, pnlOscDiag.dtpEndData.Value.Hour - pnlOscDiag.dtpEndTime.Value.Hour, pnlOscDiag.dtpEndData.Value.Minute - pnlOscDiag.dtpEndTime.Value.Minute, pnlOscDiag.dtpEndData.Value.Second - pnlOscDiag.dtpEndTime.Value.Second );
         tim = pnlOscDiag.dtpEndData.Value - tss;
         dtMax.Value = tim;
         dtMax.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMax );

         // 5. тип записи
         SqlParameter ptypeRec = new SqlParameter( );
         ptypeRec.ParameterName = "@type";
         ptypeRec.SqlDbType = SqlDbType.Int;

         ptypeRec.Value = 8; //4 информация по осциллограммам
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
         dtO = aDS.Tables [ "TbOscill" ];//TbAlarm
         for ( int curRow = 0 ;curRow < dtO.Rows.Count ;curRow++ )
         {
            int i = dgvOscill.Rows.Add( );   // номер строки
            dgvOscill [ "clmChBoxOsc", i ].Value = false;
            dgvOscill [ "clmBlockNameOsc", i ].Value = dtO.Rows [ curRow ] [ "BlockName" ];
            dgvOscill [ "clmBlockIdOsc", i ].Value = dtO.Rows [ curRow ] [ "BlockID" ];
            dgvOscill [ "clmBlockTimeOsc", i ].Value = dtO.Rows [ curRow ] [ "TimeBlock" ];
            dgvOscill [ "clmCommentOsc", i ].Value = dtO.Rows [ curRow ] [ "Comment" ];
            dgvOscill [ "clmID", i ].Value = dtO.Rows [ curRow ] [ "ID" ];
         }
         aSDA.Dispose( );
         aDS.Dispose( );
      }

      /// <summary>
      /// получение диаграмм из базы
      /// </summary>
      public void DiagBD( )
      {
         this.dgvDiag.Rows.Clear( );
         // получение строк соединения и поставщика данных из файла *.config
         //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
         SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
         try
         {
            asqlconnect.Open( );
         }
         catch ( SqlException ex )
         {
            string errorMes = "";
            // интеграция всех возвращаемых ошибок
            foreach ( SqlError connectError in ex.Errors )
               errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
				parent.WriteEventToLog(21, "Нет связи с БД (DiagBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
            System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMRZ : Нет связи с БД (DiagBD)" );
            asqlconnect.Close( );
            return;
         }
         catch ( Exception ex )
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
         pidBlock.Value = IFC * 256 + IIDDev;//IIDDev;;
         pidBlock.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pidBlock );

         // 3. начальное время
         SqlParameter dtMim = new SqlParameter( );
         dtMim.ParameterName = "@dt_start";
         dtMim.SqlDbType = SqlDbType.DateTime;
         TimeSpan tss = new TimeSpan( 0, pnlOscDiag.dtpStartData.Value.Hour - pnlOscDiag.dtpStartTime.Value.Hour, pnlOscDiag.dtpStartData.Value.Minute - pnlOscDiag.dtpStartTime.Value.Minute, pnlOscDiag.dtpStartData.Value.Second - pnlOscDiag.dtpStartTime.Value.Second );
         DateTime tim = pnlOscDiag.dtpStartData.Value - tss;
         dtMim.Value = tim;
         dtMim.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMim );

         // 2. конечное время
         SqlParameter dtMax = new SqlParameter( );
         dtMax.ParameterName = "@dt_end";
         dtMax.SqlDbType = SqlDbType.DateTime;
         tss = new TimeSpan( 0, pnlOscDiag.dtpEndData.Value.Hour - pnlOscDiag.dtpEndTime.Value.Hour, pnlOscDiag.dtpEndData.Value.Minute - pnlOscDiag.dtpEndTime.Value.Minute, pnlOscDiag.dtpEndData.Value.Second - pnlOscDiag.dtpEndTime.Value.Second );
         tim = pnlOscDiag.dtpEndData.Value - tss;
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
         dtG = aDS.Tables [ "TbDiag" ];//TbAlarm
         for ( int curRow = 0 ;curRow < dtG.Rows.Count ;curRow++ )
         {
            int i = dgvDiag.Rows.Add( );   // номер строки
            dgvDiag [ "clmChBoxDiag", i ].Value = false;
            dgvDiag [ "clmBlockNameDiag", i ].Value = dtG.Rows [ curRow ] [ "BlockName" ];
            dgvDiag [ "clmBlockIdDiag", i ].Value = dtG.Rows [ curRow ] [ "BlockID" ];
            dgvDiag [ "clmBlockTimeDiag", i ].Value = dtG.Rows [ curRow ] [ "TimeBlock" ];
            dgvDiag [ "clmCommentDiag", i ].Value = dtG.Rows [ curRow ] [ "Comment" ];
            dgvDiag [ "clmIDDiag", i ].Value = dtG.Rows [ curRow ] [ "ID" ];

            //dgvDiag["clmViewDiag", i].Value = "Диаграмма";
         }
         aSDA.Dispose( );
         aDS.Dispose( );
      }
      private void dgvOscill_CellContentClick(object sender, DataGridViewCellEventArgs e)
      {
         byte[] arrO = null;
         string ifa;         // имя файла
         DataGridViewCell de;
         char[] sep = { ' ' };
         string[] sp;
         StringBuilder sb;
         if (e.ColumnIndex == 0)
         {
            dgvOscill[e.ColumnIndex, e.RowIndex].Value = (bool)dgvOscill[e.ColumnIndex, e.RowIndex].Value ? false : true;
            btnUnionOsc.Enabled = true;
            return;
         }
         else if (e.ColumnIndex != 5)
            return;

         btnUnionOsc.Enabled = false;
         // сбрасываем все флажки
         for (int i = 0; i < dtO.Rows.Count; i++)
            dgvOscill[0, i].Value = false;

         try
         {
            de = dgvOscill["clmID", e.RowIndex];
         }
         catch
         {
            MessageBox.Show("dgvOscill_CellContentClick - исключение");
            return;
         }
         int ide = (int)de.Value;

         // по ide найти запись в dto, извлечь блок с осциллограммой (диаграммой), записать в файл, запустить fastview
         // перечисляем записи в dto
         int curRow;

         for (curRow = 0; curRow < dtO.Rows.Count; curRow++)
         {
            if (ide == ((int)dtO.Rows[curRow]["ID"]))
            {
               arrO = (byte[])dtO.Rows[curRow]["Data"];
               break;
            }
         }
         // записываем массив байт в файл
         // формируем имя файла в зависимости от типа - диаграмма или осциллограмма
         #region старый код
         //ifa = (string)dtO.Rows[curRow]["BlockName"] + ".trd";

         //// удаляем пробелы
         //sp = ifa.Split(sep);
         //sb = new StringBuilder();
         //for (int i = 0; i < sp.Length; i++)
         //{
         //   sb.Append(sp[i]);
         //}         
         #endregion

         #region Осциллограмма Сириус - формирование имени файла
         ifa = (string)dtO.Rows[curRow]["BlockName"] + "_#_" + Convert.ToString(dtO.Rows[curRow]["BlockID"]) + "_#Tblock_" + Convert.ToString(dtO.Rows[curRow]["TimeBlock"]) + "_#TFC_" + Convert.ToString(dtO.Rows[curRow]["TimeFC"]);

         // удаляем пробелы
         sp = ifa.Split(sep);
         sb = new StringBuilder();
         for (int i = 0; i < sp.Length; i++)
         {
            sb.Append(sp[i]);
         }
         string sbb = sb.ToString();
         sb.Length = 0;
         for (int i = 0; i < sbb.Length; i++)
         {
            if (sbb[i] == '.' || sbb[i] == '/' || sbb[i] == '\\' || sbb[i] == '/' || sbb[i] == ':')
               sb.Append('_');
            else
               sb.Append(sbb[i]);
         }
         sb.Append(".trd");
         #endregion

         #region запись в файл, запуск OscView - Старый код
         //FileStream f = File.Create(Environment.CurrentDirectory.ToString() + "\\" + sb.ToString());
         //f.Write(arrO, 0, arrO.Length);
         //f.Close();
         //// запускаем fastview
         //Process prc = new Process();
         //prc.StartInfo.FileName = Environment.CurrentDirectory.ToString() + "\\OscView\\OscView.exe  ";
         //prc.StartInfo.Arguments = sb.ToString();
         //prc.Start(); 
         #endregion

         #region запись в файл, запуск OscView
         FileStream f = null;
         try
         {
            f = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + sb.ToString());
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.ToString() + "\nФайл : " + AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + sb.ToString(), this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
         }

         f.Write(arrO, 0, arrO.Length);
         f.Close();
         // запускаем OscView

         Process prc = new Process();
         prc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\OscView\\OscView.exe  ";
         prc.StartInfo.Arguments = "\"" + AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + sb.ToString() + "\"";
         prc.Start();
         #endregion
      }

      private void dgvDiag_CellContentClick(object sender, DataGridViewCellEventArgs e)
      {
         byte[] arrO = null;
         string ifa;         // имя файла
         DataGridViewCell de;
         char[] sep = { ' ' };
         string[] sp;
         StringBuilder sb;
         if (e.ColumnIndex == 0)
         {
            dgvDiag[e.ColumnIndex, e.RowIndex].Value = (bool)dgvDiag[e.ColumnIndex, e.RowIndex].Value ? false : true;
            btnUnionDiag.Enabled = true;
            return;
         }
         else if (e.ColumnIndex != 5)
            return;

         btnUnionDiag.Enabled = false;
         // сбрасываем все флажки
         for (int i = 0; i < dtG.Rows.Count; i++)
            dgvDiag[0, i].Value = false;
         try
         {
            de = dgvDiag["clmIDDiag", e.RowIndex];
         }
         catch
         {
            MessageBox.Show("dgvDiag_CellContentClick - исключение");
            return;
         }
         int ide = (int)de.Value;

         // по ide найти запись в dto, извлечь блок с осциллограммой (диаграммой), записать в файл, запустить fastview
         // перечисляем записи в dto
         int curRow;

         for (curRow = 0; curRow < dtG.Rows.Count; curRow++)
         {
            if (ide == ((int)dtG.Rows[curRow]["ID"]))
            {
               arrO = (byte[])dtG.Rows[curRow]["Data"];
               break;
            }
         }
         // записываем массив байт в файл
         // формируем имя файла в зависимости от типа - диаграмма или осциллограмма
         ifa = (string)dtG.Rows[curRow]["BlockName"] + ".dgm";

         // удаляем пробелы
         sp = ifa.Split(sep);
         sb = new StringBuilder();
         for (int i = 0; i < sp.Length; i++)
         {
            sb.Append(sp[i]);
         }

         FileStream f = File.Create(Environment.CurrentDirectory.ToString() + "\\" + sb.ToString());
         f.Write(arrO, 0, arrO.Length);
         f.Close();
         // запускаем fastview
         Process prc = new Process();

         prc.StartInfo.FileName = Environment.CurrentDirectory.ToString() + "\\Fastview\\fastview.exe  ";
         prc.StartInfo.Arguments = sb.ToString();
         prc.Start();
      }
     #endregion
      #endregion

      #region Журнал событий блока
      private void tabpageEventLog_Enter( object sender, EventArgs e )
      {
          splitContainer1.Panel2Collapsed = false;
         /*
         * скрываем панели
         */
         foreach ( UserControl p in arDopPanel )
            p.Visible = false;

         pnlLogDev.Visible = true;

         // формируем hashtable
         htStringEvent = new Hashtable( );
         // загружаем список событий
         XDocument xdocevent = XDocument.Load( path2DeviceCFG );

         var vr = xdocevent.Element( "Device" ).Element( "Events" );
         if (vr == null)
         {
            MessageBox.Show("Для данного блока нет данных для работы с журналом событий", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
         }

         var lstevents = from t in xdocevent.Element( "Device" ).Element( "Events" ).Element( "section" ).Descendants( "param" )
                        select t;

         double cntkey = 0;
         double dcntkey = 0;
         foreach( XElement xeevent in lstevents )
         {
            dcntkey = Math.Pow( 2 , cntkey );
            htStringEvent.Add( dcntkey, xeevent.Attribute( "Name" ).Value );
            cntkey++;
         }
         // считаем количество слов, занимаемых в отдельной записи события (используем регулярное выражение)
         //XElement xe1 = xdocevent.Element( "Device" );
         //xe1 = xdocevent.Element( "Device" ).Element( "Events" );
         //xe1 = xdocevent.Element( "Device" ).Element( "Events" ).Element( "set" );
         IEnumerable<XElement> xe = xdocevent.Element( "Device" ).Element( "Events" ).Element( "set" ).Elements( "param" );
         string eventdatalen = ( from t in xdocevent.Element( "Device" ).Element( "Events" ).Element( "set" ).Elements( "param" )
                              where t.Attribute( "path" ).Value == "Event_Data"
                              select t ).First( ).Attribute( "type" ).Value;
         Match cntb = Regex.Match( eventdatalen, @"table:(.*$)" );

         if ( !UInt32.TryParse( cntb.Groups [ 1 ].Value, out lenMaskInModbusWords ) )
         {
            MessageBox.Show( "Не задано длина маски в записи события журнала событий.\n Отчет о содержимом журнала сформирован не будет.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
            return;
         }

         //LogDeviceBD( );
      }

      void btnReNewLogDev_Click( object sender, EventArgs e )
      {
         // чистим Textbox'ы
         foreach ( Control cntrlPnl in pnlLogDev.Controls )
         {
            if ( (cntrlPnl as Panel) != null )
               foreach ( Control cntrl in cntrlPnl.Controls )
                  if ( ( cntrl as TextBox ) != null )
                     cntrl.Text = String.Empty;
         }

         //LogDeviceBD( );
      }
      #endregion

      #region События блока
      PanelBottomEventBlock pnlBottomEV;
      private void tabpageEventBlock_Enter(object sender, EventArgs e)
      {
          splitContainer1.Panel2Collapsed = false;
         //pnlBottomEV.InitPickers();

         /*
          * скрываем панели
          */
         foreach (UserControl p in arDopPanel)
            p.Visible = false;

         pnlBottomEV.Visible = true;

         TabPage tp_this = (TabPage)sender;
      }
      #endregion   

      #endregion

      #region Формирование вкладок
      #region Формирование нижних (доп) панелей
      void CreateTabPanel( )
      {
         if ( arDopPanel == null )
            arDopPanel = new ArrayList( );

         #region Текущая - контроль
         pnlCurrent = new CurrentPanelControl( );
         SplitContMain.Panel2.Controls.Add( pnlCurrent );
         pnlCurrent.Dock = DockStyle.Fill;
         arDopPanel.Add( pnlCurrent);

         DinamicControl rr;
         /* 
          * создадим динамический элемент для его размещения на панели pnl
         */
         int xx = pnlCurrent.PnlImgDev.Width;
         int yy = pnlCurrent.PnlImgDev.Height;

         ArrayList arrFE = new ArrayList();
         CommonUtils.CommonUtils.CreateDevImg4Panel( out rr, parent.KB, IFC, IIDDev, pnlCurrent.PnlImgDev, ref arrFE); //, xed, ControlSizeVariant.SizeofControl 
         #endregion

         #region Уставки
         pnlConfig = new ConfigPanelControl( );
         SplitContMain.Panel2.Controls.Add( pnlConfig );
         //формируем панель для уставок
         pnlConfig.Dock = DockStyle.Fill;
         //pnlConfig.btnReadUstBlock.Click += new EventHandler( btnReadConfig_Click );
         pnlConfig.btnReadUstFC.Click += new EventHandler( btnReadUstFC_Click );
         pnlConfig.btnWriteUst.Click += new EventHandler( btnWriteConfig_Click );
         pnlConfig.btnResetValues.Click += new EventHandler( btnResetValues_Click );
         pnlConfig.btnReNewUstBD.Click += new EventHandler( btnReNewUstBD_Click );
         pnlConfig.Visible = false;
         arDopPanel.Add( pnlConfig );
         #endregion

         #region Аварии-срабатывание
         pnlSrabat = new SrabatPanelControl( );
         SplitContMain.Panel2.Controls.Add( pnlSrabat );
         //формируем панель для уставок
         pnlSrabat.Dock = DockStyle.Fill;
         pnlSrabat.Visible = false;
         pnlSrabat.btnReNew.Click += new EventHandler( btnReNew_Click );
         arDopPanel.Add( pnlSrabat );
         //lstvAvar.ItemActivate += new EventHandler( lstvAvar_ItemActivate );
         #endregion

         #region Осциллограммы и диаграммы
         pnlOscDiag = new OscDiagPanelControl( );
         SplitContMain.Panel2.Controls.Add( pnlOscDiag );
         pnlOscDiag.btnReNew.Click += new EventHandler( btnReNewOscDg_Click );
         pnlOscDiag.Dock = DockStyle.Fill;
         arDopPanel.Add( pnlOscDiag );
         #endregion

         #region Журнал событий блока
         pnlLogDev = new LogDevPanelControl( );
         SplitContMain.Panel2.Controls.Add( pnlLogDev );
         pnlLogDev.btnReNew.Click += new EventHandler( btnReNewLogDev_Click );
         pnlLogDev.Dock = DockStyle.Fill;
         arDopPanel.Add( pnlLogDev );
         #endregion

         #region События блока
         pnlBottomEV = new PanelBottomEventBlock();
         SplitContMain.Panel2.Controls.Add(pnlBottomEV);
         //формируем панель для выборки диапазона событий блока
         pnlBottomEV.Dock = DockStyle.Fill;
         pnlBottomEV.InitPanel(IFC, IIDDev, lstvEventBlock);

         arDopPanel.Add(pnlBottomEV);
         #endregion

         foreach ( UserControl p in arDopPanel )
            p.Visible = false;
      }
      #endregion

      #region вывод информации при выборе конкретной аварии
      private void lstvAvar_ItemActivate( object sender, EventArgs e )
      {
         if ( lstvAvar.SelectedItems.Count > 0 )
         {
            // получение строк соединения и поставщика данных из файла *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
            try
            {
               asqlconnect.Open( );
            }
            catch ( SqlException ex )
            {
               string errorMes = "";
               // интеграция всех возвращаемых ошибок
               foreach ( SqlError connectError in ex.Errors )
                  errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
					parent.WriteEventToLog(21, "Нет связи с БД (lstvAvar_ItemActivate): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
               System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMRZ : lstvAvar_ItemActivate()" );
               asqlconnect.Close( );
               return;
            }
            catch ( Exception ex )
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
            // 5. тип записи
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
            pid.Value = lstvAvar.SelectedItems [ 0 ].Tag;
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
            DataTable dt = aDS.Tables [ 0 ];
            byte[] adata = ( byte [ ] ) dt.Rows [ 0 ] [ "Data" ];

            // вызываем процедуру разбора пакета с аварийной информацией из базы
            // извлекаем шапку
            //const int headingLength = 4;  //6
            //byte[] headingPacket = new byte [ headingLength ];
            //Buffer.BlockCopy( adata, 0, headingPacket, 0, headingLength );
            //byte [] adataToVis = new byte[adata.Length - headingPacket.Length];
            //Buffer.BlockCopy( adata,headingPacket.Length,adataToVis, 0, adataToVis.Length);
//            ParseBDPacket( adataToVis, 10280, IIDDev ); // или 61100 ???

            ParseBDPacket( adata,GetAdrBlockData(path2DeviceCFG,8) , 8 ); // или 10280 61100 ???

            aSDA.Dispose( );
         }
      }
      private void AvarBD( )
      {
         // получение строк соединения и поставщика данных из файла *.config
         //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
         SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
         try
         {
            asqlconnect.Open( );
         }
         catch ( SqlException ex )
         {
            string errorMes = "";
            // интеграция всех возвращаемых ошибок
            foreach ( SqlError connectError in ex.Errors )
               errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString( ) + ") ";

				parent.WriteEventToLog(21, "Нет связи с БД (AvarBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
            System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMRZ : Нет связи с БД (AvarBD)" );
            asqlconnect.Close( );
            return;
         }
         catch ( Exception ex )
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
         pidBlock.Value = IFC * 256 + IIDDev;//IIDDev;;
         pidBlock.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( pidBlock );

         // 3. начальное время
         SqlParameter dtMim = new SqlParameter( );
         dtMim.ParameterName = "@dt_start";
         dtMim.SqlDbType = SqlDbType.DateTime;
         TimeSpan tss = new TimeSpan( 0, pnlSrabat.dtpStartDateAvar.Value.Hour - pnlSrabat.dtpStartTimeAvar.Value.Hour, pnlSrabat.dtpStartDateAvar.Value.Minute - pnlSrabat.dtpStartTimeAvar.Value.Minute, pnlSrabat.dtpStartDateAvar.Value.Second - pnlSrabat.dtpStartTimeAvar.Value.Second );
         DateTime tim = pnlSrabat.dtpStartDateAvar.Value - tss;
         dtMim.Value = tim;
         dtMim.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMim );

         // 2. конечное время
         SqlParameter dtMax = new SqlParameter( );
         dtMax.ParameterName = "@dt_end";
         dtMax.SqlDbType = SqlDbType.DateTime;
         tss = new TimeSpan( 0, pnlSrabat.dtpEndDateAvar.Value.Hour - pnlSrabat.dtpEndTimeAvar.Value.Hour, pnlSrabat.dtpEndDateAvar.Value.Minute - pnlSrabat.dtpEndTimeAvar.Value.Minute, pnlSrabat.dtpEndDateAvar.Value.Second - pnlSrabat.dtpEndTimeAvar.Value.Second );
         tim = pnlSrabat.dtpEndDateAvar.Value - tss;
         dtMax.Value = tim;
         dtMax.Direction = ParameterDirection.Input;
         cmd.Parameters.Add( dtMax );

         // 5. тип записи
         SqlParameter ptypeRec = new SqlParameter( );
         ptypeRec.ParameterName = "@type";
         ptypeRec.SqlDbType = SqlDbType.Int;
         ptypeRec.Value = 2; // информация по авариям
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
         aSDA.Fill( aDS, "TbAlarm" );

         asqlconnect.Close( );

         //PrintDataSet( aDS );
         // извлекаем данные по аварии
         dtA = aDS.Tables [ "TbAlarm" ];

         // заполняем ListView
         lstvAvar.Items.Clear( );
         for ( int curRow = 0 ;curRow < dtA.Rows.Count ;curRow++ )
         {
            DateTime t = ( DateTime ) dtA.Rows [ curRow ] [ "TimeBlock" ];
            ListViewItem li = new ListViewItem( CRZADevices.CommonCRZADeviceFunction.GetTimeInMTRACustomFormat( t ) );
            li.Tag = dtA.Rows [ curRow ] [ "ID" ];
            lstvAvar.Items.Add( li );
         }
         aSDA.Dispose( );
         aDS.Dispose( );
      }
      #endregion

      #region вывод информации при выборе конкретной записи по уставкам
      private void lstvConfig_ItemActivate( object sender, EventArgs e )
      {
         if ( lstvConfig.SelectedItems.Count == 0 )
            return;

         // получение строк соединения и поставщика данных из файла *.config
         //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
         SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
         try
         {
            asqlconnect.Open( );
         }
         catch ( SqlException ex )
         {
            string errorMes = "";
            // интеграция всех возвращаемых ошибок
            foreach ( SqlError connectError in ex.Errors )
               errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
				parent.WriteEventToLog(21, "Нет связи с БД (lstvConfig_ItemActivate): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
            System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMRZ : Нет связи с БД (lstvConfig_ItemActivate)" );
            asqlconnect.Close( );
            return;
         }
         catch ( Exception ex )
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
         pid.Value = lstvConfig.SelectedItems [ 0 ].Tag;
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
         // извлекаем данные по уставкам
         DataTable dt = aDS.Tables [ 0 ];
         byte[] adata = ( byte [ ] ) dt.Rows [ 0 ] [ "Data" ];

         // вызываем процедуру разбора пакета из базы
         ParseBDPacket( adata, GetAdrBlockData(path2DeviceCFG,14), 14 );//60200

         dt.Dispose( );
         aSDA.Dispose( );
         aDS.Dispose( );

         pnlConfig.btnWriteUst.Enabled = true;
      }
      private void UstavBD( )
   {
      //dgvAvar.Rows.Clear();
      // получение строк соединения и поставщика данных из файла *.config
      //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
      SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
      try
      {
         asqlconnect.Open( );
      }
      catch ( SqlException ex )
      {
         string errorMes = "";

         // интеграция всех возвращаемых ошибок
         foreach ( SqlError connectError in ex.Errors )
            errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString( ) + ")" + Environment.NewLine;
			parent.WriteEventToLog(21, "Нет связи с БД (UstavBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
         System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMRZ : Нет связи с БД (UstavBD)" );
         asqlconnect.Close( );
         return;
      }
      catch ( Exception ex )
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
      pidBlock.Value = IFC * 256 + IIDDev;//IIDDev;;
      pidBlock.Direction = ParameterDirection.Input;
      cmd.Parameters.Add( pidBlock );

      // 3. начальное время
      SqlParameter dtMim = new SqlParameter( );
      dtMim.ParameterName = "@dt_start";
      dtMim.SqlDbType = SqlDbType.DateTime;
      TimeSpan tss = new TimeSpan( 0, pnlConfig.dtpStartDateConfig.Value.Hour - pnlConfig.dtpStartTimeConfig.Value.Hour, pnlConfig.dtpStartDateConfig.Value.Minute - pnlConfig.dtpStartTimeConfig.Value.Minute, pnlConfig.dtpStartDateConfig.Value.Second - pnlConfig.dtpStartTimeConfig.Value.Second );
      DateTime tim = pnlConfig.dtpStartDateConfig.Value - tss;
      dtMim.Value = tim;
      dtMim.Direction = ParameterDirection.Input;
      cmd.Parameters.Add( dtMim );

      // 2. конечное время
      SqlParameter dtMax = new SqlParameter( );
      dtMax.ParameterName = "@dt_end";
      dtMax.SqlDbType = SqlDbType.DateTime;
      tss = new TimeSpan( 0, pnlConfig.dtpEndDateConfig.Value.Hour - pnlConfig.dtpEndTimeConfig.Value.Hour, pnlConfig.dtpEndDateConfig.Value.Minute - pnlConfig.dtpEndTimeConfig.Value.Minute, pnlConfig.dtpEndDateConfig.Value.Second - pnlConfig.dtpEndTimeConfig.Value.Second );
      tim = pnlConfig.dtpEndDateConfig.Value - tss;
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
      dtU = aDS.Tables [ "TbUstav" ];

      // заполняем ListView
      lstvConfig.Items.Clear( );
      for ( int curRow = 0 ;curRow < dtU.Rows.Count ;curRow++ )
      {
         DateTime t = ( DateTime ) dtU.Rows [ curRow ] [ "TimeBlock" ];
         ListViewItem li = new ListViewItem( CRZADevices.CommonCRZADeviceFunction.GetTimeInMTRACustomFormat( t ) );
         li.Tag = dtU.Rows [ curRow ] [ "ID" ];
         lstvConfig.Items.Add( li );
      }
      aSDA.Dispose( );
      aDS.Dispose( );
   }
      #endregion

      #region процедура разбора пакета с аварийной информацией из базы
      //private void ParseBDPacket( byte [ ] pack, ushort adr, int dev )
      //{
      //   PrintHexDump( "LogHexPacket.dat", pack );  // выведем в файл для контроля
      //   parent.newKB.PacketToQueDev( pack, adr, IFC,dev ); // 10280 пакет  по адресу  устройства
      //   // объявить соответсвующую группу переменных архивной
      //   SetArhivGroupInDev( dev, 8 );
      //}
      #endregion

      #region вывод информации для чтения журнала событий блока из базы
      //private void LogDeviceBD( )
      //{
      //   lv_clmn_EventDescribe.Width = this.Width - lv_clmn_DateTimeEvent.Width - lv_clmn_MasksEvents.Width;
      //   // получение строк соединения и поставщика данных из файла *.config
      //   SqlConnection asqlconnect = new SqlConnection( HMI_Settings.cstr );
      //   try
      //   {
      //      asqlconnect.Open( );
      //   }
      //   catch ( SqlException ex )
      //   {
      //      string errorMes = "";
      //      // интеграция всех возвращаемых ошибок
      //      foreach ( SqlError connectError in ex.Errors )
      //         errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString( ) + ") ";

      //      parent.WriteEventToLog(21, "Нет связи с БД (AvarBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
      //      System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : frmBMRZ : Нет связи с БД (AvarBD)" );
      //      asqlconnect.Close( );
      //      return;
      //   }
      //   catch ( Exception ex )
      //   {
      //      MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );
      //      asqlconnect.Close( );
      //      return;
      //   }

      //   // формирование данных для вызова хранимой процедуры
      //   SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
      //   cmd.CommandType = CommandType.StoredProcedure;

      //   // входные параметры
      //   // 1. ip FC
      //   SqlParameter pipFC = new SqlParameter( );
      //   pipFC.ParameterName = "@IP";
      //   pipFC.SqlDbType = SqlDbType.BigInt;
      //   pipFC.Value = 0;
      //   pipFC.Direction = ParameterDirection.Input;
      //   cmd.Parameters.Add( pipFC );
      //   // 2. id устройства
      //   SqlParameter pidBlock = new SqlParameter( );
      //   pidBlock.ParameterName = "@id";
      //   pidBlock.SqlDbType = SqlDbType.Int;
      //   pidBlock.Value = IIDDev;
      //   pidBlock.Direction = ParameterDirection.Input;
      //   cmd.Parameters.Add( pidBlock );

      //   // 3. начальное время
      //   SqlParameter dtMim = new SqlParameter( );
      //   dtMim.ParameterName = "@dt_start";
      //   dtMim.SqlDbType = SqlDbType.DateTime;
      //   TimeSpan tss = new TimeSpan( 0, pnlLogDev.dtpStartData.Value.Hour - pnlLogDev.dtpStartTime.Value.Hour, pnlLogDev.dtpStartData.Value.Minute - pnlLogDev.dtpStartTime.Value.Minute, pnlLogDev.dtpStartData.Value.Second - pnlLogDev.dtpStartTime.Value.Second );
      //   DateTime tim = pnlLogDev.dtpStartData.Value - tss;
      //   dtMim.Value = tim;
      //   dtMim.Direction = ParameterDirection.Input;
      //   cmd.Parameters.Add( dtMim );

      //   // 2. конечное время
      //   SqlParameter dtMax = new SqlParameter( );
      //   dtMax.ParameterName = "@dt_end";
      //   dtMax.SqlDbType = SqlDbType.DateTime;
      //   tss = new TimeSpan( 0, pnlLogDev.dtpEndData.Value.Hour - pnlLogDev.dtpEndTime.Value.Hour, pnlLogDev.dtpEndData.Value.Minute - pnlLogDev.dtpEndTime.Value.Minute, pnlLogDev.dtpEndData.Value.Second - pnlLogDev.dtpEndTime.Value.Second );
      //   tim = pnlLogDev.dtpEndData.Value - tss;
      //   dtMax.Value = tim;
      //   dtMax.Direction = ParameterDirection.Input;
      //   cmd.Parameters.Add( dtMax );

      //   // 5. тип записи
      //   SqlParameter ptypeRec = new SqlParameter( );
      //   ptypeRec.ParameterName = "@type";
      //   ptypeRec.SqlDbType = SqlDbType.Int;
      //   ptypeRec.Value = 9; // информация по журналу событий блока
      //   ptypeRec.Direction = ParameterDirection.Input;
      //   cmd.Parameters.Add( ptypeRec );
      //   // 6. ид записи журнала
      //   SqlParameter pid = new SqlParameter( );
      //   pid.ParameterName = "@id_record";
      //   pid.SqlDbType = SqlDbType.Int;
      //   pid.Value = 0;
      //   pid.Direction = ParameterDirection.Input;
      //   cmd.Parameters.Add( pid );

      //   // заполнение DataSet
      //   DataSet aDS = new DataSet( "ptk" );
      //   SqlDataAdapter aSDA = new SqlDataAdapter( );
      //   aSDA.SelectCommand = cmd;

      //   //aSDA.sq
      //   aSDA.Fill( aDS, "TbLogDev" );

      //   asqlconnect.Close( );

      //   //PrintDataSet( aDS );

      //   // извлекаем данные по событиям
      //   dtLD = aDS.Tables [ "TbLogDev" ];

      //   // заполняем ListView
      //   lstvEventLog.Items.Clear( );

      //   for ( int curRow = 0 ;curRow < dtLD.Rows.Count ;curRow++ )
      //   {
      //      DateTime t = ( DateTime ) dtLD.Rows [ curRow ] [ "TimeBlock" ];

      //      // разбираем пакет с событиями
      //      byte[] adata = ( byte [ ] ) dtLD.Rows [ 0 ] [ "Data" ];
      //      byte sec100 = adata[0];
      //      byte sec    = adata [ 1 ];
      //      byte min    = adata [ 2 ];
      //      byte hour   = adata [ 3 ];
      //      byte day    = adata [ 4 ]; 
      //      byte month  = adata [ 5 ];
      //      byte year   = adata [ 6 ];
      //      // формируем строку для вывода на форму
      //      pnlLogDev.TbDateTimeWrFileClockRTU.Text = day.ToString( ) + "." + month.ToString( ) + "." + year + " : " + hour.ToString( ) + "." + min.ToString( ) + "." + sec.ToString( ) + "." + sec100.ToString( );
      //      byte nrtu   = adata [ 7 ];
      //      pnlLogDev.TbNumRTU.Text = nrtu.ToString( );
      //      Encoding ae = Encoding.GetEncoding(866);
      //      string namedev = ae.GetString(adata,8,35);
      //      pnlLogDev.TbNameDev.Text = namedev;
      //      byte nuvs   = adata [ 43 ];
      //      pnlLogDev.TbNumUVS.Text = nuvs.ToString( );
      //      UInt16 chIDF = BitConverter.ToUInt16(adata,44);
      //      pnlLogDev.TbChIdFrmt.Text = chIDF.ToString( );
      //      UInt16 chbin2caption = BitConverter.ToUInt16( adata, 46 );
      //      pnlLogDev.TbNum2Header.Text = chbin2caption.ToString( );

      //      // вторая часть заголовка
      //      UInt16 reccount = BitConverter.ToUInt16( adata, 48 );
      //      pnlLogDev.TbNumEvent.Text = reccount.ToString( );
      //      byte byteCountInEachRec = adata[50];
      //      pnlLogDev.TbNumByteInEachEventRecord.Text = byteCountInEachRec.ToString( );
      //      byte reasonForUnload    = adata [ 51 ];
      //      pnlLogDev.TbReasonUnload.Text = ( ( char ) reasonForUnload ).ToString( );//BitConverter.ToString(new byte[]{reasonForUnload},0 );
            
      //      byte[] arrrec = new byte[byteCountInEachRec];
      //      int posMain = chbin2caption + 48;
      //      uint pos;
      //      // результирующая строка с событиями
      //      StringBuilder strEvents = new StringBuilder( );
      //      lstvEventLog.MouseClick += new MouseEventHandler( lstvEventLog_MouseClick );
      //      while(posMain < adata.Length)
      //      {
      //         pos = 0;
      //         Buffer.BlockCopy( adata, posMain, arrrec, 0, byteCountInEachRec );
      //         // разбираем отдельную запись
      //         byte[] a001 = new byte [ 4 ];
      //         Buffer.BlockCopy( arrrec, (int)pos, a001, 0, a001.Length/*2*/ );
      //         Array.Reverse( a001 );  // время переворачиваем
      //         UInt32 tInMSec10 = BitConverter.ToUInt32( a001, 0);
      //         DateTime dtcuryear = new DateTime( DateTime.Now.Year, 1, 1, 0, 0, 0, 0 );
      //         dtcuryear = dtcuryear.AddMilliseconds( Convert.ToDouble( tInMSec10 * 10) );
      //         pos += 4;
               
      //         // первое слово - маска события
      //         //byte[] a003 = new byte [ 2 ];
      //         //Buffer.BlockCopy( arrrec, pos, a003, 0, a003.Length/*2*/ );
      //         //pos += 2;
      //         //// второе слово - маска события
      //         //byte[] a004 = new byte [ 2 ];
      //         //Buffer.BlockCopy( arrrec, pos, a004, 0, a004.Length/*2*/ );
      //         //pos += 2;
      //         //// третье слово - маска события
      //         //byte[] a005 = new byte [ 2 ];
      //         //Buffer.BlockCopy( arrrec, pos, a005, 0, a005.Length/*2*/ );
      //         //pos += 2;
      //         //// четвертое слово - маска события
      //         //byte[] a006 = new byte [ 2 ];
      //         //Buffer.BlockCopy( arrrec, pos, a006, 0, a006.Length/*2*/ );

      //         byte[] arrb = new byte[lenMaskInModbusWords * 2];

      //         // теперь формируем массив строк событий исходя из значения битовой маски записи событий
      //         Buffer.BlockCopy( arrrec, 4, arrb, 0, (int)lenMaskInModbusWords * 2 );
      //         BitArray bitarray = new BitArray( arrb );

      //         pos += lenMaskInModbusWords * 2;
      //         posMain += byteCountInEachRec;

      //         // формируем массив строк согласно маске
      //         StringCollection scStrEvent = new StringCollection( );
      //         BitArray bitArrayTest = new BitArray( new byte [ arrb.Length ] );
      //         BitArray bitArrayRes = new BitArray( new byte [ arrb.Length ] );
      //         for ( int i = 0 ;i < htStringEvent.Count ;i++ )//bitarray.Length
      //         {
      //            bitArrayTest.Set( i, true );
      //            bitArrayRes = bitArrayTest.And( bitarray );
      //            if ( !bitArrayRes.Get( i ) )
      //            {
      //               bitArrayTest.Set( i, false );
      //               continue;
      //            }
      //            // выясняем что за строка события и добавляем в массив строк
      //            double indexStr = Math.Pow( 2, i );
      //            scStrEvent.Add( htStringEvent[indexStr].ToString());
      //         }

      //         ListViewItem li = new ListViewItem( dtcuryear.ToLongDateString() + " : " + dtcuryear.ToLongTimeString() );
      //         li.SubItems.Add( BitConverter.ToString(arrb) );
      //         strEvents.Length = 0;
               
      //         foreach( string str in scStrEvent ) 
      //            strEvents.Append(str + ";");

      //         li.SubItems.Add( strEvents.ToString() );
      //         li.Tag = scStrEvent;
      //         lstvEventLog.Items.Add( li );
      //      }
      //   }
      //   aSDA.Dispose( );
      //   aDS.Dispose( );
      //}

      //bool svvis = false;

      ///// <summary>
      ///// обработчик клика на ячейке ListView
      ///// </summary>
      ///// <param Name="sender"></param>
      ///// <param Name="e"></param>
      //void lstvEventLog_MouseClick( object sender, MouseEventArgs e )
      //{
      //   if ( svvis )//e.Button != MouseButtons.Right ||
      //   {
      //      svvis = false;
      //      return;
      //   }

      //   svvis = true;
      //   StringCollection sc = ( StringCollection ) lstvEventLog.SelectedItems [ 0 ].Tag;
      //   ListViewItem li = lstvEventLog.SelectedItems [ 0 ];
      //   Form frm = new Form( );
      //   frm.Size = new Size( 300, 200 );
      //   frm.Text = li.SubItems [ 0 ].Text;

      //   TextBox tb = new TextBox( );
      //   tb.Dock = DockStyle.Fill;
      //   tb.Multiline = true;
      //   tb.WordWrap = true;
      //   tb.ScrollBars = ScrollBars.Both;
      //   tb.ReadOnly = true;
      //   tb.TextAlign = HorizontalAlignment.Center;

      //   // первая строка - маска
      //   tb.AppendText( li.SubItems [ 1 ].Text + "\r\n" );
      //   tb.AppendText( "=========================\r\n" );
      //   foreach ( string st in sc )
      //      tb.AppendText( st + "\r\n" );

      //   frm.Controls.Add( tb );
      //   frm.WindowState = FormWindowState.Normal;
      //   frm.StartPosition = FormStartPosition.Manual;
      //   frm.Location = new Point( e.X, e.Y );
      //   frm.FormBorderStyle = FormBorderStyle.FixedSingle;//.FixedDialog;
      //   frm.MaximizeBox = false;
      //   frm.MinimizeBox = false;
      //   frm.Cursor = Cursors.No;
      //   frm.ShowIcon = false;
      //   frm.ShowDialog( );

      //   tb.Dispose();
      //   frm.Close( );
      //}
      #endregion
      #endregion

      private void btnResetSignal_Click( object sender, EventArgs e )
      {
         // выполняем действия по квитированию блока
         Console.WriteLine( "Поступила команда \"Квитировать\" для устройства: {0}; id: {1}", "", ( IFC * 256 + IIDDev ) );//tt.IdDevice
         // запись в журнал
         parent.WriteEventToLog( 20, ( IFC * 256 + IIDDev ).ToString(), "", true );//, true, false );

         if( parent.newKB.ExecuteCommand( IFC, IIDDev, "ECC", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            parent.WriteEventToLog( 35, "Команда \"Квитировать\" ушла в сеть. Устройство - "
               + IFC.ToString() + "." + IIDDev.ToString(), "", true );//, true, false );

      }

      private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
      {

      }

       private void tabPageInfo_Enter( object sender, EventArgs e )
      {
          if( rtbInfo.Lines.Count( ) != 0 )
              return;

          base.FillTAbPageInfo( PanelInfoTextBox, rtbInfo );
      }
   }
}
