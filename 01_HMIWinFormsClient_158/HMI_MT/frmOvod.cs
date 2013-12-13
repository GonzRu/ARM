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

namespace HMI_MT
{
   public partial class frmOvod_SP : frmBMRZbase
   {
      #region Public
      public ArrayList arrDeviceInfo = new ArrayList( );
      #endregion

      #region private
      // hashTable для хранения строк событий и ключей доступа к ним
      // сортированный список с именами панелей и фреймов
      SortedList DevPanelTypes;

      // нижние панели
      CurrentPanelControl  pnlCurrent;

      #endregion

      #region Конструкторы
      public frmOvod_SP( )
      {
         InitializeComponent( );
      }
      public frmOvod_SP( MainForm linkMainForm, int iFC, int iIDDev, string fxml )
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
      private void frmOvod_SP_Load( object sender, EventArgs e )
      {
         tabpageControl.Enter += new EventHandler( tabpageControl_Enter );
         slTPtoArrVars.Add( tabpageControl.Text, new ArrayList( ) );

         //tabStatusDev_Command.Enter += new EventHandler( tabStatusDev_Command_Enter );
         //slTPtoArrVars.Add( tabStatusDev_Command.Text, new ArrayList( ) );

			//tp_statusDev.Enter += new EventHandler( tabStatusDev_Command_Enter );
			//slTPtoArrVars.Add( tp_statusDev.Text, new ArrayList( ) );

			//tp_RezHMICmd.Enter += new EventHandler( tabStatusDev_Command_Enter );
			//slTPtoArrVars.Add( tp_RezHMICmd.Text, new ArrayList() );

			//tabStatusFC.Enter += new EventHandler( tabStatusFC_Enter );
			//slTPtoArrVars.Add( tabStatusFC.Text, new ArrayList( ) );

         //tbpPacketViewer.Enter += new EventHandler( tbpPacketViewer_Enter );

         #region по источнику и номеру устройства опрделяем пути к файлам 
         path2PrgDevCFG = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";
         XDocument xdoc = XDocument.Load( path2PrgDevCFG );
         IEnumerable<XElement> xes = xdoc.Descendants( "FC" );
         var xe = ( from nn in
                       ( from n in xes
                         where n.Attribute( "numFC" ).Value == StrFC
                         select n ).Descendants( "Device" )
                    where nn.Element( "NumDev" ).Value == ( IIDDev - ( 256 * IFC ) ).ToString( )
                    select nn ).First( );

         path2DeviceCFG = AppDomain.CurrentDomain.BaseDirectory + xe.Element( "nameR" ).Value + Path.DirectorySeparatorChar + "Device.cfg";
         path2FrmDev = AppDomain.CurrentDomain.BaseDirectory + xe.Element( "nameR" ).Value + Path.DirectorySeparatorChar + "frm" + xe.Element( "nameELowLevel" ).Value;
         if ( !File.Exists( path2DeviceCFG ) )
         {
            MessageBox.Show( "Файл =" + path2DeviceCFG + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }
         if ( !File.Exists( path2FrmDev  + ".xml") )
         {
            path2FrmDev += "_" + IIDDev.ToString();
            if (!File.Exists(path2FrmDev + ".xml"))
            {
               MessageBox.Show("Файл =" + path2FrmDev + ".xml" + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
               return;
            }
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
      }
      #endregion

      #region Обработчики входов на вкладки
      #region Текущая
      void tabpageControl_Enter( object sender, EventArgs e )
      {
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

         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, null );//pnlTPControl
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)
         //PrepareAdditionalFLP( pnlCurrent.Controls );
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
         if (!File.Exists(path2FrmDev + ".xml"))
            throw new Exception("Файл не найден : " + path2FrmDev + ".xml");

         XDocument xdoc_frm = XDocument.Load(path2FrmDev + ".xml");
         string faddname = String.Empty;

         if ( !String.IsNullOrEmpty( ( string ) xdoc_frm.Element( "MT" ).Element( "FileAdditionalFLP" ) ) )
            faddname = xdoc_frm.Element( "MT" ).Element( "FileAdditionalFLP" ).Element( "Name" ).Value;
         else
         {
            MessageBox.Show( "В файле описания формы нет ссылки на доп панель", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }

         faddname = Path.GetDirectoryName(path2FrmDev + ".xml") + Path.DirectorySeparatorChar + faddname;

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
            ArrayList arrf = GetArrFrmls( flpframe,String.Empty );

            // отображаем
            PlaceVisElemOnForm( "", flpframe.Attribute( "MTFLPNameR" ).Value, arrf );
         }
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
         //pnlCurrent = new CurrentPanelControl( );
         //SplitContMain.Panel2.Controls.Add( pnlCurrent );
         //pnlCurrent.Dock = DockStyle.Fill;
         //arDopPanel.Add( pnlCurrent);

         //DinamicControl rr;
         ///* 
         // * создадим динамический элемент для его размещения на панели pnl
         //*/
         //int xx = pnlCurrent.PnlImgDev.Width;
         //int yy = pnlCurrent.PnlImgDev.Height;
         //CommonUtils.CommonUtils.CreateDevImg4Panel( out rr, parent.KB, IFC, IIDDev, pnlCurrent.PnlImgDev); //, xed, ControlSizeVariant.SizeofControl 
         #endregion

         foreach ( UserControl p in arDopPanel )
            p.Visible = false;
      }
      #endregion

      private void tc_Main_SelectedIndexChanged( object sender, EventArgs e )
      {

      }
      #endregion
   }
}
