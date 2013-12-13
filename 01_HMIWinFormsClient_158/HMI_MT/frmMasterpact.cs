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
   public partial class frmMasterpact : frmBMRZbase
   {
      #region Public
      public ArrayList arrDeviceInfo = new ArrayList();
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

      #endregion


      #region Конструкторы
      public frmMasterpact( )
      {
         InitializeComponent( );
      }
      public frmMasterpact( MainForm linkMainForm, int iFC, int iIDDev, string fxml )
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
      private void frmMasterpact_Load( object sender, EventArgs e )
      {
         tabpageControl.Enter += new EventHandler( tabpageControl_Enter );
         slTPtoArrVars.Add( tabpageControl.Text, new ArrayList() );

         //tabStatusDev_Command.Enter += new EventHandler(tabStatusDev_Command_Enter);
         //slTPtoArrVars.Add(tabStatusDev_Command.Text, new ArrayList());

         //tabpageDeviceInfo.Enter += new EventHandler( tabStatusDev_Command_Enter );
         //slTPtoArrVars.Add( tabpageDeviceInfo.Text, new ArrayList( ) );

         //tabpageConfig.Enter += new EventHandler( tabpageConfig_Enter );
         //slTPtoArrVars.Add( tabpageConfig.Text, new ArrayList() );

         //tabpageSrabat.Enter += new EventHandler( tabpageSrabat_Enter );
         //slTPtoArrVars.Add( tabpageSrabat.Text, new ArrayList() );

         //tbpPacketViewer.Enter += new EventHandler( tbpPacketViewer_Enter );

         #region по источнику и номеру устройства опрделяем пути к файлам
         path2PrgDevCFG = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";
         XDocument xdoc = XDocument.Load( path2PrgDevCFG );
         IEnumerable<XElement> xes = xdoc.Descendants( "FC" );
         var xe = ( from nn in
                       ( from n in xes
                         where n.Attribute( "numFC" ).Value == StrFC
                         select n ).Descendants( "Device" )
                    where nn.Element( "NumDev" ).Value == IIDDev.ToString()  //( IIDDev - ( 256 * IFC ) )
                    select nn ).First();

         path2DeviceCFG = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + xe.Element( "nameR" ).Value + Path.DirectorySeparatorChar + "Device.cfg";
         path2FrmDev = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + xe.Element( "nameR" ).Value + Path.DirectorySeparatorChar + "frm" + xe.Element( "nameELowLevel" ).Value + ".xml";
         if( !File.Exists( path2DeviceCFG ) )
         {
            MessageBox.Show( "Файл =" + path2DeviceCFG + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }
         if( !File.Exists( path2FrmDev ) )
         {
            MessageBox.Show( "Файл =" + path2FrmDev + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }
         #endregion

         // формируем сортированный список с панелями
         xdoc = XDocument.Load( path2DeviceCFG );
         DevPanelTypes = new SortedList();

         if( !String.IsNullOrEmpty( (string) xdoc.Element( "Device" ).Element( "TypeOfPanelSections" ) ) )
         {
            IEnumerable<XElement> etypes = xdoc.Element( "Device" ).Element( "TypeOfPanelSections" ).Elements( "TypeOfPanel" );

            foreach( XElement xr in etypes )
               // определим вариант формата секции TypeOfPanel
               if( (string) xr.Element( "Name" ) == null )
                  DevPanelTypes.Add( xr.Value, String.Empty );
               else
                  DevPanelTypes.Add( xr.Element( "Name" ).Value, xr.Element( "Caption" ).Value );
         }
         else
            MessageBox.Show( "Типы панелей в файле Device.cfg отсутсвуют", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning );

         GetCCforFLP( (ControlCollection) this.Controls );

         // заголовок формы
         //this.Text = xe.Element( "nameR" ).Value + " ( ид.№ " + this.IIDDev.ToString( ) + " )"; // + " " + rr.cwInfo.strRefDesign+ " ( ид.№ " + rr.cwInfo.idDev + " ) - яч. № " + rr.cwInfo.nLoc

         // создаем нижние панели
         CreateTabPanel();

      }
      #endregion
      #region Обработчики входов на вкладки
      #region Текущая
      void tabpageControl_Enter( object sender, EventArgs e )
      {
         /*
          * скрываем панели
          */
         //foreach( UserControl p in arDopPanel )
         //   p.Visible = false;

         //pnlCurrent.Visible = true;

         TabPage tp_this = (TabPage) sender;
         ArrayList arrVars = (ArrayList) slTPtoArrVars [ tp_this.Text ];
         if( arrVars.Count != 0 )
            return;

         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, null/*pnlTPControl*/ );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)
         //PrepareAdditionalFLP( pnlCurrent.Controls );
      }

      /// <summary>
      /// сформировать виз элементы на доп панелях MTRANamedFLPanel, кот принадлежат некоторому контролу
      /// </summary>
      private void PrepareAdditionalFLP( Control.ControlCollection cntrlCC )
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
         if( !File.Exists( path2FrmDev ) )
            throw new Exception( "Файл не найден : " + path2FrmDev );

         XDocument xdoc_frm = XDocument.Load( path2FrmDev );
         string faddname = String.Empty;

         if( !String.IsNullOrEmpty( (string) xdoc_frm.Element( "MT" ).Element( "FileAdditionalFLP" ) ) )
            faddname = xdoc_frm.Element( "MT" ).Element( "FileAdditionalFLP" ).Element( "Name" ).Value;
         else
         {
            MessageBox.Show( "В файле описания формы нет ссылки на доп панель", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }

         faddname = Path.GetDirectoryName( path2FrmDev ) + Path.DirectorySeparatorChar + faddname;

         if( !File.Exists( faddname ) )
         {
            MessageBox.Show( "Файл с описанием доп панели не найден : " + faddname, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }

         XDocument xdoc_addflp;
         try
         {
            xdoc_addflp = XDocument.Load( faddname );
         }
         catch( Exception e )
         {
            throw new Exception( "Ошибка в формате xml-документа: " + faddname );
         }

         IEnumerable<XElement> flpframes = xdoc_addflp.Element( "MT" ).Element( "AdditionalFLP" ).Elements( "FLPframe" );

         foreach( XElement flpframe in flpframes )
         {
            // формируем массив формул
            ArrayList arrf = GetArrFrmls( flpframe, String.Empty );

            // отображаем
            PlaceVisElemOnForm( "", flpframe.Attribute( "MTFLPNameR" ).Value, arrf );
         }
      }
      #endregion
      #endregion
      void tabpageConfig_Enter( object sender, EventArgs e )
      {
         #region устанавливаем пикеры для изменения уставок за последние сутки
         //pnlConfig.dtpEndDateConfig.Value = DateTime.Now;
         //pnlConfig.dtpEndTimeConfig.Value = DateTime.Now;
         //pnlConfig.dtpStartDateConfig.Value = DateTime.Now;

         //TimeSpan ts = new TimeSpan( 1, 0, 0, 0 );
         //pnlConfig.dtpStartDateConfig.Value = pnlConfig.dtpStartDateConfig.Value - ts;
         //pnlConfig.dtpStartTimeConfig.Value = DateTime.Now;
         #endregion

         /*
          * скрываем панели
          */
         //foreach( UserControl p in arDopPanel )
         //   p.Visible = false;

         //pnlConfig.Visible = true;

         TabPage tp_this = (TabPage) sender;
         ArrayList arrVars = (ArrayList) slTPtoArrVars [ tp_this.Text ];
         if( arrVars.Count != 0 )
            return;

         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, null );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)

         //UstavBD();
      }
      #region Формирование нижних (доп) панелей
      void CreateTabPanel()
      {
         //if( arDopPanel == null )
         //   arDopPanel = new ArrayList();

         //#region Текущая - контроль
         //pnlCurrent = new CurrentPanelControl();
         //SplitContMain.Panel2.Controls.Add( pnlCurrent );
         //pnlCurrent.Dock = DockStyle.Fill;
         //arDopPanel.Add( pnlCurrent );

         //DinamicControl rr;
         ///* 
         // * создадим динамический элемент для его размещения на панели pnl
         //*/
         //int xx = pnlCurrent.PnlImgDev.Width;
         //int yy = pnlCurrent.PnlImgDev.Height;

         //ArrayList arrFE = new ArrayList();
         //CommonUtils.CommonUtils.CreateDevImg4Panel( out rr, parent.KB, IFC, IIDDev, pnlCurrent.PnlImgDev, ref arrFE ); //, xed, ControlSizeVariant.SizeofControl 
         //#endregion

         //#region Уставки
         //pnlConfig = new ConfigPanelControl();
         //SplitContMain.Panel2.Controls.Add( pnlConfig );
         ////формируем панель для уставок
         //pnlConfig.Dock = DockStyle.Fill;
         ////pnlConfig.btnReadUstBlock.Click += new EventHandler( btnReadConfig_Click );
         //pnlConfig.btnReadUstFC.Click += new EventHandler( btnReadUstFC_Click );
         //pnlConfig.btnWriteUst.Click += new EventHandler( btnWriteConfig_Click );
         //pnlConfig.btnResetValues.Click += new EventHandler( btnResetValues_Click );
         //pnlConfig.btnReNewUstBD.Click += new EventHandler( btnReNewUstBD_Click );
         //pnlConfig.Visible = false;
         //arDopPanel.Add( pnlConfig );
         //#endregion

         //#region Аварии-срабатывание
         //pnlSrabat = new SrabatPanelControl();
         //SplitContMain.Panel2.Controls.Add( pnlSrabat );
         ////формируем панель для уставок
         //pnlSrabat.Dock = DockStyle.Fill;
         //pnlSrabat.Visible = false;
         //pnlSrabat.btnReNew.Click += new EventHandler( btnReNew_Click );
         //arDopPanel.Add( pnlSrabat );
         ////lstvAvar.ItemActivate += new EventHandler( lstvAvar_ItemActivate );
         //#endregion

         //#region Осциллограммы и диаграммы
         //pnlOscDiag = new OscDiagPanelControl();
         //SplitContMain.Panel2.Controls.Add( pnlOscDiag );
         //pnlOscDiag.btnReNew.Click += new EventHandler( btnReNewOscDg_Click );
         //pnlOscDiag.Dock = DockStyle.Fill;
         //arDopPanel.Add( pnlOscDiag );
         //#endregion

         //#region Журнал событий блока
         //pnlLogDev = new LogDevPanelControl();
         //SplitContMain.Panel2.Controls.Add( pnlLogDev );
         //pnlLogDev.btnReNew.Click += new EventHandler( btnReNewLogDev_Click );
         //pnlLogDev.Dock = DockStyle.Fill;
         //arDopPanel.Add( pnlLogDev );
         //#endregion

         //foreach( UserControl p in arDopPanel )
         //   p.Visible = false;
      }
      #endregion

      /// <summary>
      /// команда Включить
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      private void btnSwitchOn_Click( object sender, EventArgs e )
      {
         if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, parent.UserRight ) )
            return;

         if( parent.isReqPassword )
            if( !parent.CanAction() )
            {
               MessageBox.Show( "Выполнение действия запрещено" );
               return;
            }

         ConfirmCommand dlg = new ConfirmCommand();
         dlg.label1.Text = "Включить?";

         if( !( DialogResult.OK == dlg.ShowDialog() ) )
            return;

         // выполняем действия по включению выключателя
         // вначале определим устройство

         Console.WriteLine( "Поступила команда \"Включить\" для устройства: {0}; id: {1}", "Masterpact", IIDDev ); //.IdDevice 

         // правильная запись в журнал действий пользователя
         // номер устройства с цчетом фк
         int numdevfc = IFC * 256 + IIDDev;
         parent.WriteEventToLog( 3, numdevfc.ToString(), this.Name, true );// true, false );

         if( parent.newKB.ExecuteCommand( IFC, IIDDev, "CCB", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            parent.WriteEventToLog( 35, "Команда \"CCB\" ушла в сеть. Устройство - "
               + IFC.ToString() + "." + IIDDev.ToString(), this.Name, true );//, true, false );
      }

      private void btnSwitchOff_Click( object sender, EventArgs e )
      {
         if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, parent.UserRight ) )
            return;

         if( parent.isReqPassword )
            if( !parent.CanAction() )
            {
               MessageBox.Show( "Выполнение действия запрещено" );
               return;
            }

         ConfirmCommand dlg = new ConfirmCommand();
         dlg.label1.Text = "Отключить?";

         if( !( DialogResult.OK == dlg.ShowDialog() ) )
            return;

         // выполняем действия по отключению выключателя
         Console.WriteLine( "Поступила команда \"Отключить\" для устройства: {0}; id: {1}", "Masterpact", IIDDev );//tt.IdDevice.GetName()

         // правильная запись в журнал действий пользователя
         // номер устройства с цчетом фк
         int numdevfc = IFC * 256 + IIDDev;
         parent.WriteEventToLog( 4, numdevfc.ToString(), this.Name, true );// true, false );

         if( parent.newKB.ExecuteCommand( IFC, IIDDev, "OCB", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            parent.WriteEventToLog( 35, "Команда \"OCB\" ушла в сеть. Устройство - "
               + IFC.ToString() + "." + IIDDev.ToString(), this.Name, true );//, true, false );

      }
   }
}
