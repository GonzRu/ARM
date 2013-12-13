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

namespace HMI_MT
{
   public partial class frmEkra : frmBMRZbase
   {
      #region Public
      public ArrayList arrDeviceInfo = new ArrayList( );
      #endregion

      #region private
      // hashTable для хранения строк событий и ключей доступа к ним
      // сортированный список с именами панелей и фреймов
      //SortedList DevPanelTypes;
      //XDocument xdoc;
      DataTable dtO;    // таблица с осциллограммами
      DataTable dtU;    // таблица с уставками
      /// <summary>
      /// DataSet - в него будем помещать таблицы 
      /// в соответсвии с именами групп уставок 
      /// постоянных и сменяемых: $01, $02, 101, 201 ...
      /// </summary>
      DataSet DsUst = new DataSet();
      /// <summary>
      /// список сопоставляющий TagGuid с секцией описаия тега в АСУ (ASU_level_Describe)
      /// </summary>
      SortedList<int, XElement> slTagGuidXes;
      /// <summary>
      /// список имен Handle и размеров соотв блоков
      /// </summary>
      SortedList<string, int > slHandleSizeBlock ;
      /// <summary>
      /// сортированный список для установки соответствия
      /// между адресом тега и TCRZAVariable
      /// </summary>
      SortedList<string, TCRZAVariable> slFromAdr2TCRZAVar;
      /// <summary>
      /// сортированный список для установки соответсвия между адресом тега и таблицей где он содержится
      /// </summary>
      SortedList<string, DataTable> slLinkAdr2DataTable;
      // нижние панели
      OscDiagPanelControl pnlOscDiag;
      ConfigPanelControl pnlConfig;
	  OscDiagViewer oscdg;
      #endregion

      #region Конструкторы
      public frmEkra( )
      {
         InitializeComponent( );
      }
      public frmEkra( MainForm linkMainForm, int iFC, int iIDDev, string fxml )
         : base( linkMainForm, iFC, iIDDev, fxml )
      {
         InitializeComponent( );

         //переупорядочим вкладки, отодвинув базовые назад
         ArrayList artp = new ArrayList( );

         foreach ( TabPage tp in tc_Main_frmBMRZbase.TabPages )
         {
            if(tp.Text != "Состояние устройства и команд")
                artp.Add( tp );
         }
         
         int i = artp.Count - 1;

         tc_Main_frmBMRZbase.Multiline = true;  // отображение корешков в несколько рядов

         foreach ( TabPage tp in artp )
         {
            tc_Main_frmBMRZbase.TabPages [ i ] = tp;
            i--;
         }

         #region добавим панель для кнопок команд
         //Panel pnlcmd = new Panel();
         //pnlcmd.BackColor = Color.LightSalmon;
         //pnlcmd.Dock = DockStyle.Fill;
         //FlowLayoutPanel flpForBtn = new FlowLayoutPanel();
         //flpForBtn.Parent = pnlcmd;
         //flpForBtn.Dock = DockStyle.Fill;

         //#region Добавляем кнопки
         ////btnCmdSPP
         //Button btnCmdSPP_ras = new Button();
         //btnCmdSPP_ras.AutoSize = true;
         //btnCmdSPP_ras.BackColor = SystemColors.Control;
         //btnCmdSPP_ras.Name = "btnRas";
         //btnCmdSPP_ras.Text = "Установить ras-приоритет опроса";
         //btnCmdSPP_ras.Font = new Font("Arial", 10, FontStyle.Bold);
         //btnCmdSPP_ras.Parent = flpForBtn;
         //btnCmdSPP_ras.Click += new EventHandler(btnCmdSPP_ras_Click);

         ////btnCmdSPP
         //Button btnCmdSPP_rai = new Button();
         //btnCmdSPP_rai.AutoSize = true;
         //btnCmdSPP_rai.BackColor = SystemColors.Control;
         //btnCmdSPP_rai.Name = "btnRai";
         //btnCmdSPP_rai.Text = "Установить rai-приоритет опроса";
         //btnCmdSPP_rai.Font = new Font("Arial", 10, FontStyle.Bold);
         //btnCmdSPP_rai.Parent = flpForBtn;
         //btnCmdSPP_rai.Click += new EventHandler(btnCmdSPP_ras_Click);

         ////btnCmdIME
         //Button btnCmdIME = new Button();
         //btnCmdIME.AutoSize = true;
         //btnCmdIME.BackColor = SystemColors.Control;
         //btnCmdIME.Name = "btnIME"; 
         //btnCmdIME.Text = "Включить в исх. пакеты сообщения событий";
         //btnCmdIME.Font = new Font("Arial", 10, FontStyle.Bold);
         //btnCmdIME.Parent = flpForBtn;
         //btnCmdIME.Click += new EventHandler(btnCmdSPP_ras_Click);
         //#endregion
         //SplitContMain.Panel2.Controls.Add(pnlcmd);
         #endregion
         
         SplitContMain.Panel2Collapsed = false;
     }

      void btnCmdSPP_ras_Click(object sender, EventArgs e)
      {
         byte[] memXOut = new byte[4];

         switch ((sender as Button).Name) 
         {
            case "btnRas":
               memXOut[0] = BitConverter.GetBytes( 'r' )[0];
               memXOut[1] = BitConverter.GetBytes( 'a' )[0];
               memXOut[2] = BitConverter.GetBytes( 's' )[0];
               memXOut[3] = 0;
               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "SPP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"SPP(ras)\" ушла в сеть. Устройство - "
                     + IFC.ToString() + "." + IIDDev.ToString(), this.Name, true);//, true, false);
               break;
            case "btnRai":
               memXOut[0] = BitConverter.GetBytes( 'r' )[0];
               memXOut[1] = BitConverter.GetBytes( 'a' )[0];
               memXOut[2] = BitConverter.GetBytes( 'i' )[0];
               memXOut[3] = 0;
               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "SPP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"SPP(rai)\" ушла в сеть. Устройство - "
							+ IFC.ToString() + "." + IIDDev.ToString(), this.Name, true);// true, false);
               break;
            case "btnIME":
               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "IME", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"IME\" ушла в сеть. Устройство - "
							+ IFC.ToString() + "." + IIDDev.ToString(), this.Name, true);// true, false);
               break;
            default:
               MessageBox.Show("Неизвестная команда", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
               break;
         }
      }
      #endregion

      #region Load
      private void frmEkra_Load( object sender, EventArgs e )
      {
         tabpageControl.Enter += new EventHandler( tabpageControl_Enter );
         slTPtoArrVars.Add( tabpageControl.Text, new ArrayList( ) );

         tabPageCurStateInfoDev.Enter += new EventHandler(tabPageCurStateInfoDev_Enter);
         slTPtoArrVars.Add(tabPageCurStateInfoDev.Text, new ArrayList());

         tbpConfUst.Enter += new EventHandler(tbpConfUst_Enter);
         slTPtoArrVars.Add(tbpConfUst.Text, new ArrayList());

         #region по источнику и номеру устройства опрделяем пути к файлам 
         path2PrgDevCFG = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";
         
         xdoc = XDocument.Load( path2PrgDevCFG );
         
         IEnumerable<XElement> xes = xdoc.Descendants( "FC" );
         var xe = ( from nn in
                       ( from n in xes
                         where n.Attribute( "numFC" ).Value == StrFC
                         select n ).Descendants( "Device" )
                    where nn.Element( "NumDev" ).Value == ( IIDDev ).ToString( )
                    select nn ).First( );

         path2DeviceCFG = AppDomain.CurrentDomain.BaseDirectory + xe.Element( "nameR" ).Value + Path.DirectorySeparatorChar + "Device.cfg";
         path2FrmDev = AppDomain.CurrentDomain.BaseDirectory + xe.Element( "nameR" ).Value + Path.DirectorySeparatorChar + "frm" + xe.Element( "nameELowLevel" ).Value + ".xml";
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

         #region формируем сортированный список с панелями
         xdoc = XDocument.Load(path2DeviceCFG);
         DevPanelTypes = new SortedList();
            
         if (!String.IsNullOrEmpty((string)xdoc.Element("Device").Element("TypeOfPanelSections")))
         {
            IEnumerable<XElement> etypes = xdoc.Element("Device").Element("TypeOfPanelSections").Elements("TypeOfPanel");

            foreach (XElement xr in etypes)
               // определим вариант формата секции TypeOfPanel
               if ((string)xr.Element("Name") == null)
                  DevPanelTypes.Add(xr.Value, String.Empty);
               else
                  DevPanelTypes.Add(xr.Element("Name").Value, xr.Element("Caption").Value);
         }
         else
            MessageBox.Show("Типы панелей в файле Device.cfg отсутсвуют", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

         GetCCforFLP((ControlCollection)this.Controls);
         #endregion

         // создаем нижние панели
         CreateTabPanel( );
          
         #region устанавливаем пикеры для вывода осциллограмм и диаграмм за последние сутки
		 pnlOscDiag.dtpEndData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		 pnlOscDiag.dtpEndTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		 pnlOscDiag.dtpStartData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
		  TimeSpan ts = new TimeSpan(1, 0, 0, 0);
         pnlOscDiag.dtpStartData.Value = pnlOscDiag.dtpStartData.Value - ts;
		 pnlOscDiag.dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
         #endregion

          // реакция на событие получения осциллограммы
         //FormulaEval b_10330 = new FormulaEval(parent.KB, "0(" + this.IFC.ToString() + "." + this.IIDDev.ToString() + ".0.60000.0001)", "0", "Чтение осциллограммы", "", TypeOfTag.no, "");
         //b_10330.OnChangeValForm += this.LinkSetText;
         //b_10330.FirstValue();

         //if (parent.newKB.ExecuteCommand(IFC, IIDDev, "IME", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
         //    parent.WriteEventToLog(35, "Команда \"IME\" ушла в сеть. Устройство - "
         //              + IFC.ToString() + "." + IIDDev.ToString(), this.Name, true);// true, false);

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
         if (arDopPanel != null)
            foreach ( UserControl p in arDopPanel )
               p.Visible = false;

         TabPage tp_this = ( TabPage ) sender;
         ArrayList arrVars = ( ArrayList ) slTPtoArrVars [ tp_this.Text ];
         if ( arrVars.Count != 0 )
            return;

         PrepareTabPagesForGroup(tp_this.Text, tp_this, ref arrVars, pnlCurValue);
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)
      }      
	   #endregion

      #region Текущее состояние и инф об устр
      void tabPageCurStateInfoDev_Enter(object sender, EventArgs e)
      {
          splitContainer1.Panel2Collapsed = true;
         /*
          * скрываем панели
          */
         if (arDopPanel != null)
            foreach (UserControl p in arDopPanel)
               p.Visible = false;

         TabPage tp_this = (TabPage)sender;
         ArrayList arrVars = (ArrayList)slTPtoArrVars[tp_this.Text];
         if (arrVars.Count != 0)
            return;

         PrepareTabPagesForGroup(tp_this.Text, tp_this, ref arrVars, pnlCurState);
         slTPtoArrVars[tp_this.Text] = arrVars; // ref не отрабатывает (?)
      }
      #endregion

      #region вкладка с уставками
      /// <summary>
      /// вход на вкладку с уставками
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      void tbpConfUst_Enter(object sender, EventArgs e)
      {
          splitContainer1.Panel2Collapsed = false;
         #region устанавливаем пикеры для изменения уставок за последние сутки
		  pnlConfig.dtpEndDateConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		  pnlConfig.dtpEndTimeConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
		 pnlConfig.dtpStartDateConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

         TimeSpan ts = new TimeSpan(1, 0, 0, 0);
         pnlConfig.dtpStartDateConfig.Value = pnlConfig.dtpStartDateConfig.Value - ts;
		 pnlConfig.dtpStartTimeConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
         #endregion

         /*
         * скрываем панели
         */
         if (arDopPanel != null)
            foreach (UserControl p in arDopPanel)
               p.Visible = false;

         pnlConfig.Visible = true;

         TabPage tp_this = (TabPage)sender;
         ArrayList arrVars = (ArrayList)slTPtoArrVars[tp_this.Text];

         if (arrVars.Count != 0)
            return;

         slFromAdr2TCRZAVar = new SortedList<string, TCRZAVariable>();
         slLinkAdr2DataTable = new SortedList<string, DataTable>();

         PrepareTabPagesForGroup(tp_this.Text, tp_this, ref arrVars, pnlTPConfig);

         PrepareDataSetForWriteUstGroup("Уставки");//tp_this.Text, tp_this, ref arrVars, pnlCurState

         slTPtoArrVars[tp_this.Text] = arrVars; // ref не отрабатывает (?)
      }

      public void PrepareDataSetForWriteUstGroup(string groupname)//string groupname, TabPage tabpage, ref ArrayList arlist, Panel pnlTP
      {
         Debug.Assert(xdoc != null, "xdoc == null");

         // найдем описание группы в файле описания устройства
         //XDocument xdoc_txt = XDocument.Load(path2DeviceCFG);
         //IEnumerable<XElement> xegftts = xdoc_txt.Descendants( "GroupForTheTag" );

         var xe_config = (from x in xdoc.Descendants("GroupInDev")
                          where x.Element("Name").Value == groupname
                          select x).DefaultIfEmpty().Single();

         #region соберем TagGuid и соотв. секции в сорт список
         slTagGuidXes = new SortedList<int, XElement>();

         if (xe_config == null)
         {
            MessageBox.Show("Нет данных для форимирования вкладки для группы " + groupname, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
         }

         IEnumerable<XElement> TagGuidXes= xe_config.Descendants( "ASU_level_Describe" );

         foreach( XElement TagGuidXe in TagGuidXes )
         {
            slTagGuidXes.Add( Convert.ToInt32( TagGuidXe.Element( "TagGUID" ).Value ), TagGuidXe );
         }         
         #endregion


         // строим дерево
         //treeViewUstCFG.Nodes.Add(groupname);

         // создадим DataSet - в него будем помещать таблицы в соответсвии с именами групп уставок постоянных и сменяемых: $01, $02, 101, 201 ...
         CreateDataSet();

         // // пустая группа ?
         if (xe_config.Descendants("SubGroup").Count() == 0 && ((string)xe_config.Element("Tags") == null))
         { 
            MessageBox.Show("Нет данных для отображения группы " + groupname, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
         }

         // проверим есть ли у корневого элемента узел "Tags"
         Debug.Assert(String.IsNullOrEmpty((string)xe_config.Element("Tags")), "(370) : frmEkra.cs : у корневого элемента есть узел Tags");
         #region старый код
         if( !String.IsNullOrEmpty( (string) xe_config.Element( "Tags" ) ) )
            CreateRowForTableSubGroupTags( xe_config );//, dsust, xe_config.Element( "NumberGrInASU" ).Value         
         #endregion            

         if (xe_config.Elements("SubGroup").Count() != 0)
         {
            CreateTableForSubGroupNodeTags(xe_config, xe_config.Element( "NumberGrInASU" ).Value );
         }

         #region старый код
         // slnflps - сортированный список flp с именами и условными обозначениями
         /*SortedList*/
         //slnflps = CreateTPforGroup(xe_config, tabpage, tabpage.Text, pnlTP);

         // теперь новые flp нужно добавить в список
         //GetCCforFLP((ControlCollection)this.Controls);

         //arlist = CreateArrayList(tabpage.Text);//grname

         //PlaceVisElemOnForm(tabpage.Text, String.Empty, arlist); 
         #endregion 
      }

      /// <summary>
      /// DataSet - содержит таблицы в соответсвии с именами групп уставок постоянных и сменяемых: $01, $02, 101, 201 ...
      /// </summary>
      private void CreateDataSet()
      {
         DsUst = new DataSet();

         IEnumerable<XElement> xehandles = xdoc.Element("Device").Element("InfoForWriteUst").Elements("Handle");
         slHandleSizeBlock = new SortedList<string,int>();

         // создаем таблицы с тегами подгрупп для записи
         foreach( XElement xehandle in xehandles )
         {
            DataTable dt = new DataTable( xehandle.Attribute( "id" ).Value.ToString() );
            slHandleSizeBlock.Add(xehandle.Attribute( "id" ).Value.ToString(), Convert.ToInt32(xehandle.Attribute( "num_words" ).Value) );

            dt.Columns.Add( "Имя тега", typeof( System.String ) );
            dt.Columns.Add( "Текущее значение", typeof( System.String ) );
            dt.Columns.Add( "Новое значение", typeof( System.String ) );
            dt.Columns.Add( "Адрес тега", typeof( System.String ) );
            dt.Columns.Add( "TagGUID", typeof( System.String ) );

            DsUst.Tables.Add(dt);
         }
      }

      private void CreateTableForSubGroupNodeTags(/*TreeNode tn,*/ XElement xe, string numgr)
      {
         IEnumerable<XElement> xesgs = xe.Elements("SubGroup");

         foreach (XElement xesg in xesgs)
         {
            // создаем новый treenode для подгруппы
            //TreeNode newtreenode = new TreeNode(xesg.Element("Name").Value);
            //tn.Nodes.Add(newtreenode);

            //Debug.Assert(String.IsNullOrEmpty((string)xe_config.Element("Tags")), "(370) : frmEkra.cs : в описании подгруппы есть узел Tags");

            #region старый код
            if( !String.IsNullOrEmpty( (string) xesg.Element( "Tags" ) ) )
               CreateRowForTableSubGroupTags( xesg ); //newtreenode, , dsust, numgr
            #endregion

            CreateTableForSubGroupNodeTags(/*newtreenode,*/ xesg, numgr);
         }
      }

      StringBuilder sb = new StringBuilder();

      private void CreateRowForTableSubGroupTags(XElement xe) // TreeNode tn, , string numgr
      {
         // инициализируем строки таблицы
         IEnumerable<XElement> xetags = xe.Element( "Tags" ).Descendants( "ASU_level_Describe" );

         if( xetags.Count() == 0 )
            return;
         // формируем таблицу и списки (заодно)
         DataTable dt;
         foreach( XElement xetag in xetags )
         {
            if( !DsUst.Tables.Contains( xetag.Element( "AnyInfo" ).Value.ToString() ) )
               continue;

            dt = DsUst.Tables [ xetag.Element( "AnyInfo" ).Value.ToString() ];
            sb.Length = 0;
            DataRow dtr = dt.NewRow();

            sb.Append( xetag.Element( "AddressModbus" ).Value );

            if( !String.IsNullOrEmpty( xetag.Element( "BitMask" ).Value.Trim() ) )
               sb.Append( "_" + xetag.Element( "BitMask" ).Value );

            dtr [ "Адрес тега" ] = sb.ToString();
            dtr [ "Имя тега" ] = xetag.Element( "Description" ).Value;
            dtr [ "TagGUID" ] = xetag.Element( "TagGUID" ).Value;

            dt.Rows.Add( dtr );

            #region также формируем соответсвие между адресом тега и таблицей где он содержиться - чтобы сделать возможным динамическое обновление
            slLinkAdr2DataTable.Add( sb.ToString(), dt );
            #endregion

            #region одновременно формируем сортированный список - ключ - адрес тега
            slFromAdr2TCRZAVar.Add( sb.ToString(), GetLinkToCRZAVar( xetag.Element( "TagGUID" ).Value ) );

            sb.Length = 0;
            // и подписываемся на обновление
            sb.Append( "0(" + IFC.ToString() + "." + IIDDev.ToString() + "." + xetag.Element( "NumGroup" ).Value + "." + xetag.Element( "AddressModbus" ).Value + "." );

            if( !String.IsNullOrEmpty( xetag.Element( "BitMask" ).Value.Trim() ) )
               sb.Append( xetag.Element( "BitMask" ).Value );
            else
               sb.Append( "0000" );

            sb.Append( ")" );
            #endregion


            #region формируем FormulaEval - для обновления
            TypeOfTag tot = new TypeOfTag();

            switch( xetag.Element( "AnalDiscretType" ).Value )
            {
               case "Analog":
                  tot = TypeOfTag.Analog;
                  break;
               case "Discret":
                  tot = TypeOfTag.Discret;
                  break;
               case "Combo":
                  tot = TypeOfTag.Combo;
                  break;
               default:
                  MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                  break;
            }

            FormulaEval eval = new FormulaEval( parent.KB, sb.ToString(), "0", xetag.Element( "Description" ).Value, xetag.Element( "Dimention" ).Value, tot, xetag.Element( "TypeFrm" ).Value );
            eval.OnChangeValForm += new FormulaEval.ChangeValForm( eval_OnChangeValForm );
            #endregion
         }
      }

      void eval_OnChangeValForm(object valForm, string format)
      {
         sb.Length = 0;
         
         RezFormulaEval rfe = (RezFormulaEval) valForm;

         string[] arrtg = rfe.IdTagIE.Split(new char[] { '.' });

         int ibm;
         if (!int.TryParse(arrtg[4], out ibm))
            throw new Exception("Неправильное значение маски");
            
         sb.Append(arrtg[3]);

         if (ibm != 0)
            sb.Append("_" + arrtg[4]);

         DataTable dt;

         if( !slLinkAdr2DataTable.ContainsKey( sb.ToString() ) )
            return;

         dt = slLinkAdr2DataTable[sb.ToString()];
         
         foreach (DataRow dtr in dt.Rows)
         { 
            if (dtr["Адрес тега"].ToString() == sb.ToString())
            {
               dtr["Текущее значение"] = rfe.Value.ToString();
                  break;
            }
         }
      }

      /// <summary>
      /// получение ссылки на TCRZAVariable по ее адресу
      /// </summary>
      /// <param Name="AddressModbus"></param>
      /// <param Name="BitMask"></param>
      private TCRZAVariable GetLinkToCRZAVar(string TagGUID)
      {
         foreach (FC fc in parent.KB)
            if (fc.NumFC == IFC)
               foreach (TCRZADirectDevice tcdd in fc.Devices)
                  if (tcdd.NumDev == IIDDev) 
                     return tcdd.DeviceTags[Convert.ToInt32(TagGUID)];
                  
         return null;
      }

      private void treeViewUstCFG_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
      {
         //if (e.Button != MouseButtons.Left)
         //   return;

         //TreeNode selected_node = treeViewUstCFG.GetNodeAt(e.X, e.Y);
         //dgvUstCFG.DataSource = selected_node.Tag as DataTable;
         //dgvUstCFG.Columns["Адрес тега"].Width = 5;
         //dgvUstCFG.Columns["TagGUID"].Width = 5;

      }

       #region вывод информации при выборе конкретной записи по уставкам
       private void lstvConfig_ItemActivate(object sender, EventArgs e)
       {
          if (lstvConfig.SelectedItems.Count == 0)
             return;
          pnlConfig.btnWriteUst.Enabled = true;

          // получение строк соединения и поставщика данных из файла *.config
          //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
          SqlConnection asqlconnect = new SqlConnection(HMI_Settings.cstr);
          try
          {
             asqlconnect.Open();
          }
          catch (SqlException ex)
          {
             string errorMes = "";
             // интеграция всех возвращаемых ошибок
             foreach (SqlError connectError in ex.Errors)
                errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ")" + Environment.NewLine;
             parent.WriteEventToLog(21, "Нет связи с БД (lstvConfig_ItemActivate): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
             System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " : frmBMRZ : Нет связи с БД (lstvConfig_ItemActivate)");
             asqlconnect.Close();
             return;
          }
          catch (Exception ex)
          {
             MessageBox.Show("Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
             asqlconnect.Close();
             return;
          }
          // формирование данных для вызова хранимой процедуры
          SqlCommand cmd = new SqlCommand("ShowDataLog", asqlconnect);
          cmd.CommandType = CommandType.StoredProcedure;

          // входные параметры
          // 1. ip FC
          SqlParameter pipFC = new SqlParameter();
          pipFC.ParameterName = "@IP";
          pipFC.SqlDbType = SqlDbType.BigInt;
          pipFC.Value = 0;
          pipFC.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(pipFC);
          // 2. id устройства
          SqlParameter pidBlock = new SqlParameter();
          pidBlock.ParameterName = "@id";
          pidBlock.SqlDbType = SqlDbType.Int;
          pidBlock.Value = 0;
          pidBlock.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(pidBlock);
          // 3. время старт
          SqlParameter ptimeStart = new SqlParameter();
          ptimeStart.ParameterName = "@dt_start";
          ptimeStart.SqlDbType = SqlDbType.DateTime;
          ptimeStart.Value = DateTime.Now;
          ptimeStart.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(ptimeStart);
          // 4. время конец
          SqlParameter ptimeFin = new SqlParameter();
          ptimeFin.ParameterName = "@dt_end";
          ptimeFin.SqlDbType = SqlDbType.DateTime;
          ptimeFin.Value = DateTime.Now;
          ptimeFin.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(ptimeFin);
          // 5. тип записи - не нужен - все по Tag
          SqlParameter ptypeRec = new SqlParameter();
          ptypeRec.ParameterName = "@type";
          ptypeRec.SqlDbType = SqlDbType.Int;
          ptypeRec.Value = TypeBlockData.TypeBlockData_Unknown;
          ptypeRec.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(ptypeRec);
          // 6. ид записи журнала
          SqlParameter pid = new SqlParameter();
          pid.ParameterName = "@id_record";
          pid.SqlDbType = SqlDbType.Int;
          pid.Value = lstvConfig.SelectedItems[0].Tag;
          pid.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(pid);

          // заполнение DataSet
          DataSet aDS = new DataSet("ptk");
          SqlDataAdapter aSDA = new SqlDataAdapter();
          aSDA.SelectCommand = cmd;

          //aSDA.sq
          aSDA.Fill(aDS);//, "DataLog" 

          asqlconnect.Close();

          //PrintDataSet( aDS );
          // извлекаем данные по уставкам
          DataTable dt = aDS.Tables[0];
          byte[] adata = (byte[])dt.Rows[0]["Data"];

          // вызываем процедуру разбора пакета из базы
          parent.SetArhivGroupInDev(IFC, IIDDev, 14);

          ParseBDPacket(adata, GetAdrBlockData(path2DeviceCFG, 14), 14);//60200

          dt.Dispose();
          aSDA.Dispose();
          aDS.Dispose();

          pnlConfig.btnWriteUst.Enabled = true;
       }
       private void UstavBD()
       {
          //dgvAvar.Rows.Clear();
          // получение строк соединения и поставщика данных из файла *.config
          //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
          SqlConnection asqlconnect = new SqlConnection(HMI_Settings.cstr);
          try
          {
             asqlconnect.Open();
          }
          catch (SqlException ex)
          {
             string errorMes = "";

             // интеграция всех возвращаемых ошибок
             foreach (SqlError connectError in ex.Errors)
                errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ")" + Environment.NewLine;
             parent.WriteEventToLog(21, "Нет связи с БД (UstavBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
             System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " : frmBMRZ : Нет связи с БД (UstavBD)");
             asqlconnect.Close();
             return;
          }
          catch (Exception ex)
          {
             MessageBox.Show("Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
             asqlconnect.Close();
             return;
          }
          // формирование данных для вызова хранимой процедуры
          SqlCommand cmd = new SqlCommand("ShowDataLog", asqlconnect);
          cmd.CommandType = CommandType.StoredProcedure;

          // входные параметры
          // 1. ip FC
          SqlParameter pipFC = new SqlParameter();
          pipFC.ParameterName = "@IP";
          pipFC.SqlDbType = SqlDbType.BigInt;
          pipFC.Value = 0;
          pipFC.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(pipFC);
          // 2. id устройства
          SqlParameter pidBlock = new SqlParameter();
          pidBlock.ParameterName = "@id";
          pidBlock.SqlDbType = SqlDbType.Int;
          pidBlock.Value = IFC * 256 + IIDDev;//IIDDev;
          pidBlock.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(pidBlock);

          // 3. начальное время
          SqlParameter dtMim = new SqlParameter();
          dtMim.ParameterName = "@dt_start";
          dtMim.SqlDbType = SqlDbType.DateTime;
          TimeSpan tss = new TimeSpan(0, pnlConfig.dtpStartDateConfig.Value.Hour - pnlConfig.dtpStartTimeConfig.Value.Hour, pnlConfig.dtpStartDateConfig.Value.Minute - pnlConfig.dtpStartTimeConfig.Value.Minute, pnlConfig.dtpStartDateConfig.Value.Second - pnlConfig.dtpStartTimeConfig.Value.Second);
          DateTime tim = pnlConfig.dtpStartDateConfig.Value - tss;
          dtMim.Value = tim;
          dtMim.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(dtMim);

          // 2. конечное время
          SqlParameter dtMax = new SqlParameter();
          dtMax.ParameterName = "@dt_end";
          dtMax.SqlDbType = SqlDbType.DateTime;
          tss = new TimeSpan(0, pnlConfig.dtpEndDateConfig.Value.Hour - pnlConfig.dtpEndTimeConfig.Value.Hour, pnlConfig.dtpEndDateConfig.Value.Minute - pnlConfig.dtpEndTimeConfig.Value.Minute, pnlConfig.dtpEndDateConfig.Value.Second - pnlConfig.dtpEndTimeConfig.Value.Second);
          tim = pnlConfig.dtpEndDateConfig.Value - tss;
          dtMax.Value = tim;
          dtMax.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(dtMax);

          // 5. тип записи
          SqlParameter ptypeRec = new SqlParameter();
          ptypeRec.ParameterName = "@type";
          ptypeRec.SqlDbType = SqlDbType.Int;
          ptypeRec.Value = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Ustavki, parent.KB, this.IFC, this.IIDDev);// TypeBlockData.TypeBlockData_Ustavki; // информация по уставкам
          ptypeRec.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(ptypeRec);
          // 6. ид записи журнала
          SqlParameter pid = new SqlParameter();
          pid.ParameterName = "@id_record";
          pid.SqlDbType = SqlDbType.Int;
          pid.Value = 0;
          pid.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(pid);

          // заполнение DataSet
          DataSet aDS = new DataSet("ptk");
          SqlDataAdapter aSDA = new SqlDataAdapter();
          aSDA.SelectCommand = cmd;

          //aSDA.sq
          aSDA.Fill(aDS, "TbUstav");

          asqlconnect.Close();

          //PrintDataSet( aDS );
          // извлекаем данные по уставкам
          dtU = aDS.Tables["TbUstav"];

          // заполняем ListView
          lstvConfig.Items.Clear();
          for (int curRow = 0; curRow < dtU.Rows.Count; curRow++)
          {
             DateTime t = (DateTime)dtU.Rows[curRow]["TimeBlock"];
             ListViewItem li = new ListViewItem( CRZADevices.CommonCRZADeviceFunction.GetTimeInMTRACustomFormat( t ) );
             li.Tag = dtU.Rows[curRow]["ID"];
             lstvConfig.Items.Add(li);
          }
          aSDA.Dispose();
          aDS.Dispose();
       }
       #endregion
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

         DinamicControl rr;
         /* 
          * создадим динамический элемент для его размещения на панели pnl
         */
         //int xx = pnlCurrent.PnlImgDev.Width;
         //int yy = pnlCurrent.PnlImgDev.Height;

         //ArrayList arrFE = new ArrayList();
         //CommonUtils.CommonUtils.CreateDevImg4Panel(out rr, parent.KB, IFC, IIDDev, pnlCurrent.PnlImgDev, ref arrFE); //, xed, ControlSizeVariant.SizeofControl 
         #endregion

         #region Осциллограммы и диаграммы
         pnlOscDiag = new OscDiagPanelControl();
         SplitContMain.Panel2.Controls.Add(pnlOscDiag);
         pnlOscDiag.btnReNew.Click += new EventHandler(btnReNewOscDg_Click);
         pnlOscDiag.Dock = DockStyle.Fill;
         arDopPanel.Add(pnlOscDiag);
         #endregion

         #region Уставки и конфигурация
         pnlConfig = new ConfigPanelControl();
         SplitContMain.Panel2.Controls.Add(pnlConfig);
         //pnlConfig.btnReadUstBlock.Click += new EventHandler(btnReadUstBlock_Click);
         pnlConfig.btnReadUstFC.Click += new EventHandler(btnReadUstFC_Click);
         pnlConfig.btnReNewUstBD.Click += new EventHandler(btnReNewUstBD_Click);
         pnlConfig.btnResetValues.Click += new EventHandler(btnResetValues_Click);
         pnlConfig.btnWriteUst.Click += new EventHandler(btnWriteUst_Click);

         pnlConfig.Dock = DockStyle.Fill;
         arDopPanel.Add(pnlConfig);
         pnlConfig.btnWriteUst.Enabled = false;
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

      void btnReadUstBlock_Click(object sender, EventArgs e)
      {
         /*
          * вместо команды RPC используем IMP (рекомендация О.А.М.)
          */

         if (parent.newKB.ExecuteCommand(IFC, IIDDev, "IMP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
            parent.WriteEventToLog(35, "Команда \"IMP\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false);

         // документирование действия пользователя
         parent.WriteEventToLog(7, IIDDev.ToString(), this.Name, true);//, true, false);//"выдана команда RCP - чтения уставок."

		 HMI_Settings.ClientDFE.SetReq4PeriodicPacketQuery(IFC, IIDDev, 14);
      }

      void btnReadUstFC_Click(object sender, EventArgs e)
      {
         pnlConfig.btnWriteUst.Enabled = true;
         // перенаправляем
         btnReadUstBlock_Click(sender, e);
      }

      /// <summary>
      /// Записать уставки
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      void btnWriteUst_Click(object sender, EventArgs e)
      {
         pnlConfig.btnWriteUst.Enabled = false;

         if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, parent.UserRight ) )
            return;

         if( parent.isReqPassword )
            if( !parent.CanAction() )
            {
               MessageBox.Show( "Выполнение действия запрещено" );
               return;
            }

         DialogResult drez = MessageBox.Show( "Записать уставки?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
         if( drez == DialogResult.No )
            return;

         // нужно определить изменные теги и заполнить их новые значения в соотв. таблицах
         uint addrLinkVar;
         DataTable dt = null;
         int j =0;

         for( int i = 0 ; i < UstavkiControls.Count ; i++ )
         {
            switch( ( ( UstavkiControls [ i ] ) as UserControl ).Name )
            {
               case "ctlLabelTextbox":
                  if( ( UstavkiControls [ i ] as ctlLabelTextbox ).isChange )
                  {
                     addrLinkVar = ( UstavkiControls [ i ] as ctlLabelTextbox ).addrLinkVar;
                     dt = slLinkAdr2DataTable [ addrLinkVar.ToString() ];

                     foreach( DataRow dr in dt.Rows )
                     {
                        if( dr [ "Адрес тега" ].ToString() == addrLinkVar.ToString() )
                           dr [ "Новое значение" ] = ( UstavkiControls [ i ] as ctlLabelTextbox ).TextboxText;
                     }
                     (UstavkiControls [ i ] as ctlLabelTextbox ).isChange = false;
                  }
                  break;
               case "ComboBoxVar":
                  string keycb = string.Empty;
                  if( ( UstavkiControls [ i ] as ComboBoxVar ).isChange )
                  {
                     if( ( UstavkiControls [ i ] as ComboBoxVar ).typetag == "TBitFieldVariable" )
                     {
                        addrLinkVar = ( UstavkiControls [ i ] as ComboBoxVar ).addrLinkVar;
                        keycb = addrLinkVar.ToString() + "_" + ( UstavkiControls [ i ] as ComboBoxVar ).addrLinkVarBitMask;
                        dt = slLinkAdr2DataTable [ keycb ];
                     }
                     else
                     {
                        addrLinkVar = ( UstavkiControls [ i ] as ComboBoxVar ).addrLinkVar;
                        keycb = addrLinkVar.ToString();
                        dt = slLinkAdr2DataTable [ keycb ];
                     }

                     foreach( DataRow dr in dt.Rows )
                     {
                        if( dr [ "Адрес тега" ].ToString() == keycb )
                        {
                           if( ( UstavkiControls [ i ] as ComboBoxVar ).isThisNumEnumCB )
                           {
                              dr [ "Новое значение" ] = ( UstavkiControls [ i ] as ComboBoxVar ).slNumEnumStr.IndexOfValue((string)(UstavkiControls [ i ] as ComboBoxVar ).cbVar.SelectedItem);                             
                           }
                           else
                              dr [ "Новое значение" ] = ( UstavkiControls [ i ] as ComboBoxVar ).cbVar.SelectedIndex;
                        }
                     }

                     (UstavkiControls [ i ] as ComboBoxVar ).isChange = false;
                  }
                  break;
               case "CheckBoxVar":
                  if( ( UstavkiControls [ i ] as CheckBoxVar ).isChange )
                  {
                     addrLinkVar = ( UstavkiControls [ i ] as ctlLabelTextbox ).addrLinkVar;
                     dt = slLinkAdr2DataTable [ addrLinkVar.ToString() ];

                     foreach( DataRow dr in dt.Rows )
                     {
                        if( dr [ "Адрес тега" ].ToString() == addrLinkVar.ToString() )
                           dr [ "Новое значение" ] = ( UstavkiControls [ i ] as ctlLabelTextbox ).TextboxText;
                     }

                     (UstavkiControls [ i ] as CheckBoxVar ).isChange = false;
                  }
                  break;
               default:
                  MessageBox.Show( "Данный тип тега не поддерживается в процедуре записи уставок" );
                  break;
            }
         }

        string id = String.Empty;
        string num_words = String.Empty;
        TCRZAVariable tvar;
        int adrtg;
        StringBuilder sbval = new StringBuilder();
        byte[] memb = new byte [ 2 ];
        byte[] membmask = new byte [ 2 ];

        /*
         * определили таблицу в которой есть измененная уставка.
         * Теперь нам нужно определить:
         * 1. адрес начала блока
         * 2. длину блока => выделить массив для формирования пакета
         * 
         * Процесс формирования блока похож на разбор пакета на нижнем уровне
         * только в обратную сторону
         * Определив первую переменную, узнаем ее длину, записываем байтовое представление
         * в массив, смещаемся, смотрим есть ли среди тегов тег по новому адресу, если нет, то 
         * еще смещаемся, если есть, то определяем длину и т.д.
         * Для битовых тегов исходя из их значения устанавливаем нужные биты в массиве.
         */

         byte[] packet4send = new byte[Convert.ToInt32(slHandleSizeBlock[dt.TableName] ) * 2];

         int adrbase = Convert.ToInt32( dt.Rows [ 0 ] [ "Адрес тега" ] );

         // формируем пакет для отправки
         foreach( DataRow dr in dt.Rows )
         {
            sb.Length = 0;

            // извлекаем адрес
            sb.Append( dr [ "Адрес тега" ] );
            tvar = slFromAdr2TCRZAVar [ sb.ToString() ];

            sbval.Length = 0;
            if( !String.IsNullOrEmpty( dr [ "Новое значение" ].ToString().Trim() ) )
               sbval.Append( dr [ "Новое значение" ].ToString().Trim() );
            else
               sbval.Append( dr [ "Текущее значение" ].ToString().Trim() );

            /* 
             * для первого варианта реализации формируем содержимое пакета для 
             * отправки автономно, не изменяя никаких данных в конфигурации - 
             * при успешной записи они заменятся новыми значениями при чтении
             */
            switch( tvar.ToString() )
            {
               case "TUIntVariable":
                  if( !int.TryParse( sb.ToString(), out adrtg ) )
                  {
                     MessageBox.Show( "(612) frmEkra.cs : btnWriteUst_Click : Неправильный адрес тега", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
                     continue;
                  }

                  // строка пустая ?
                  if( String.IsNullOrEmpty( sbval.ToString().Trim() ) )
                  {
                     ( tvar as TUIntVariable ).Value = 0;
                  }
                  else
                     ( tvar as TUIntVariable ).Value = Convert.ToUInt16( sbval.ToString() );
                  // нужно ли переворачивать - не нужно
                  Buffer.BlockCopy( ( BitConverter.GetBytes( ( tvar as TUIntVariable ).Value ) ), 0, packet4send, ( adrtg - adrbase ) * 2, ( tvar as TUIntVariable ).length_B );
                  break;
               case "TIntVariable":
                  if( !int.TryParse( sb.ToString(), out adrtg ) )
                  {
                     MessageBox.Show( "(612) frmEkra.cs : btnWriteUst_Click : Неправильный адрес тега", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
                     continue;
                  }

                  // строка пустая ?
                  if( String.IsNullOrEmpty( sbval.ToString().Trim() ) )
                  {
                     ( tvar as TIntVariable ).Value = 0;
                  }
                  else
                     ( tvar as TIntVariable ).Value = Convert.ToInt16( sbval.ToString() );
                  // нужно ли переворачивать - не нужно
                  Buffer.BlockCopy( ( BitConverter.GetBytes( ( tvar as TIntVariable ).Value ) ), 0, packet4send, ( adrtg - adrbase ) * 2, ( tvar as TIntVariable ).length_B );
                  break;

               case "TUIntCBVariable":
                  if( !int.TryParse( sb.ToString(), out adrtg ) )
                  {
                     MessageBox.Show( "(612) frmEkra.cs : btnWriteUst_Click : Неправильный адрес тега", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
                     continue;
                  }

                  // строка пустая ?
                  if( String.IsNullOrEmpty( sbval.ToString().Trim() ) )
                  {
                     ( tvar as TUIntCBVariable ).Value = 0;
                  }
                  else
                     ( tvar as TUIntCBVariable ).Value = Convert.ToUInt16( sbval.ToString() );
                  // нужно ли переворачивать - не нужно
                  Buffer.BlockCopy( ( BitConverter.GetBytes( ( tvar as TUIntCBVariable ).Value ) ), 0, packet4send, ( adrtg - adrbase ) * 2, ( tvar as TUIntCBVariable ).length_B );
                  break;

               case "TFloatVariable":
                  if( !int.TryParse( sb.ToString(), out adrtg ) )
                  {
                     MessageBox.Show( "(612) frmEkra.cs : btnWriteUst_Click : Неправильный адрес тега", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
                     continue;
                  }

                  // строка пустая ?
                  if( String.IsNullOrEmpty( sbval.ToString().Trim() ) )
                  {
                     ( tvar as TFloatVariable ).Value = 0;
                  }
                  else
                     ( tvar as TFloatVariable ).Value = Convert.ToSingle( sbval.ToString() );
                  // нужно ли переворачивать - не нужно
                  Buffer.BlockCopy( ( BitConverter.GetBytes( ( tvar as TFloatVariable ).Value ) ), 0, packet4send, ( adrtg - adrbase ) * 2, ( tvar as TFloatVariable ).length_B );
                  break;
               case "TBitFieldVariable":
               case "TBitField2CBVariable":
                  int i = 0;
                  if( tvar.ToString() == "TBitField2CBVariable" )
                     i = 0;
                  // разделяем адрес регистра и битовую маску
                  string[] adrbitmask = sb.ToString().Split( new char [ ] { '_' } );
                  sb.Length = 0;
                  sb.Append( adrbitmask [ 0 ] );

                  if( !int.TryParse( sb.ToString(), out adrtg ) )
                  {
                     MessageBox.Show( "(612) frmEkra.cs : btnWriteUst_Click : Неправильный адрес тега", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
                     continue;
                  }
                  sb.Length = 0;
                  sb.Append( adrbitmask [ 1 ] );

                  // строка пустая ?
                  int newval = 0;
                  if( String.IsNullOrEmpty( sbval.ToString().Trim() ) )
                  {
                     ( tvar as TBitFieldVariable ).Value = false;
                  }
                  else
                  {
                     if( sbval.ToString() == "False" || sbval.ToString() == "false" )
                        ( tvar as TBitFieldVariable ).Value = false;
                     else if( ( sbval.ToString() == "True" || sbval.ToString() == "true" ) )
                        ( tvar as TBitFieldVariable ).Value = true;
                     else if( int.TryParse( sbval.ToString(), out newval ) )
                        ( tvar as TBitFieldVariable ).Value = Convert.ToBoolean( newval );
                     else
                        throw new Exception( "frmEkra.frm : Неизвестный формат логического значения : " + sbval.ToString() );
                  }
                  // строка пустая ?
                  //if( String.IsNullOrEmpty( sbval.ToString().Trim() ) )
                  //{
                  //   ( tvar as TBitFieldVariable ).Value = false;
                  //}
                  //else
                  //   ( tvar as TBitFieldVariable ).Value = Convert.ToBoolean( Convert.ToInt32( sbval.ToString() ) );

                  // извлечем 2 байта из массива для установки бита

                  Buffer.BlockCopy( packet4send, ( adrtg - adrbase ) * 2, memb, 0, 2 );

                  BitArray ba1 = new BitArray( memb );

                  membmask = BitConverter.GetBytes( Convert.ToUInt16( sb.ToString(), 16 ) );
                  Array.Reverse( membmask );

                  BitArray ba2 = new BitArray( membmask );

                  BitArray brez;

                  //if (Convert.ToBoolean(sbval.ToString()))
                  if( ( tvar as TBitFieldVariable ).Value )
                     brez = ba1.Or( ba2 );  // устанавливаем бит
                  else
                     brez = ba1.And( ba2.Not() ); // сбрасываем бит

                  brez.CopyTo( memb, 0 );

                  // записываем обратно - нужно ли переворачивать - не нужно
                  Buffer.BlockCopy( memb, 0, packet4send, ( adrtg - adrbase ) * 2, ( tvar as TBitFieldVariable ).length_B );
                  break;
               default:
                  MessageBox.Show("Для данного типа тега невозможно сформировать значение уставки");
                  break;
            }
         }

         // формируем команду для отправки пакета packet4send
         byte[] memXOut = new byte [ packet4send.Length + 4 ];

              Buffer.BlockCopy(packet4send, 0, memXOut, 4, packet4send.Length);
              // в первые 4 байта помещаем HANDLE - строка с нулем на конце
              sb.Length = 0;
              sb.Append(dt.TableName.ToString() + "\0");
              byte[] scmd2 = new byte[4];
              scmd2 = Encoding.UTF8.GetBytes(sb.ToString());
              Buffer.BlockCopy(scmd2,0,memXOut,0,4);
              
              if (parent.newKB.ExecuteCommand(IFC, IIDDev, "WCP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                 parent.WriteEventToLog(35, "Команда \"WCP\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false );
              // документирование действия пользователя
              int numdevfc = IFC * 256 + IIDDev;
              parent.WriteEventToLog( 6, numdevfc.ToString(), this.Name, true );//, true, false );			//"выдана команда WCP - запись уставок."
      }

      private bool IsUstInTheSubGroupChange(TreeNode tn) 
      {
         if ((tn.Tag as DataTable) != null)
            foreach (DataRow dr in (tn.Tag as DataTable).Rows)
            {
               if (!String.IsNullOrEmpty(dr["Новое значение"].ToString().Trim()))
                  return true;
            }
         else if (tn.Nodes.Count != 0)
            foreach (TreeNode tnn in tn.Nodes)
            {
               if (IsUstInTheSubGroupChange(tnn))
                  return true;
            }

         return false;
      }
       
      /// <summary>
      /// формирование списка таблиц для узла в котором были измененя уставок - проверить логику
      /// </summary>
      /// <param Name="tn"></param>
      /// <returns></returns>
      private List<DataTable> GetListDataTable(TreeNode tn)
      {
         List<DataTable> ldt = new List<DataTable>();

         if ((tn.Tag as DataTable) != null)
            ldt.Add((tn.Tag as DataTable));

         if (tn.Nodes.Count != 0)
            foreach (TreeNode tnn in tn.Nodes)
               GetListRecDataTable(tnn, ldt);

         return ldt;
      }

      private void GetListRecDataTable(TreeNode tn, List<DataTable> ldt)
      {
         if ((tn.Tag as DataTable) != null)
            ldt.Add((tn.Tag as DataTable));

         if (tn.Nodes.Count != 0)
            foreach (TreeNode tnn in tn.Nodes)
               GetListRecDataTable(tnn, ldt);
      }

      void btnReNewUstBD_Click(object sender, EventArgs e)
      {
         UstavBD();
         //pnlConfig.tcUstConfigBottomPanel.SelectTab(0);
      }

      void btnResetValues_Click(object sender, EventArgs e)
      {
         parent.newKB.ResetGroup( IFC, IIDDev, 14 );
      }
      #endregion

      private void frmEkra_FormClosing(object sender, FormClosingEventArgs e)
      {
		  if (HMI_Settings.ClientDFE != null)
			  HMI_Settings.ClientDFE.RemoveRefToPageTags(this.Text);
      }

    #region вкладка Осциллограммы
    private void tabPage5_Enter(object sender, EventArgs e)
    {
        splitContainer1.Panel2Collapsed = false;

		if (dgvOscill.RowCount != 0 || dgvDiag.RowCount != 0)
			return;

        foreach (UserControl p in arDopPanel)
            p.Visible = false;

        pnlOscDiag.Visible = true;

		OscBD(); 
    }

    void btnReNewOscDg_Click(object sender, EventArgs e)
    {
        OscBD();
    }

    // получение осциллограмм из базы
    private void OscBD()
    {
			if (oscdg == null)
				oscdg = new OscDiagViewer(parent);

			oscdg.IdFC = this.IFC;
			oscdg.IdDev = this.IIDDev;
			oscdg.DTStartData = pnlOscDiag.dtpStartData.Value;
			oscdg.DTStartTime = pnlOscDiag.dtpStartData.Value;
			oscdg.DTEndData = pnlOscDiag.dtpEndData.Value;
			oscdg.DTEndTime = pnlOscDiag.dtpEndData.Value;
			oscdg.TypeRec = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Osc, parent.KB, this.IFC, this.IIDDev);// TypeBlockData.TypeBlockData_OscSirius;

			// извлекаем данные по осциллограммам
			dtO = oscdg.Do_SQLProc();

			for (int curRow = 0; curRow < dtO.Rows.Count; curRow++)
			{
				int i = dgvOscill.Rows.Add();   // номер строки
				dgvOscill["clmChBoxOsc", i].Value = false;
				dgvOscill["clmBlockNameOsc", i].Value = dtO.Rows[curRow]["BlockName"];
				dgvOscill["clmBlockIdOsc", i].Value = dtO.Rows[curRow]["BlockID"];
				dgvOscill["clmBlockTimeOsc", i].Value = dtO.Rows[curRow]["TimeBlock"];
				dgvOscill["clmCommentOsc", i].Value = dtO.Rows[curRow]["Comment"];
				dgvOscill["clmID", i].Value = dtO.Rows[curRow]["ID"];
			}
   }
    ArrayList asb = new ArrayList();    // для хранения имен файлов в случае для объединения осциллограмм
    short cntSelectOSC = 0;             // число выделенных осциллограмм

	/// <summary>
	/// кнопка чтение одной осциллограммы
	/// </summary>
	/// <param Name="sender"></param>
	/// <param Name="e"></param>
	private void dgvOscill_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		DataGridViewCell de;

		if (e.ColumnIndex == 0)
		{
			dgvOscill[e.ColumnIndex, e.RowIndex].Value = (bool)dgvOscill[e.ColumnIndex, e.RowIndex].Value ? false : true;
			btnUnionOsc.Enabled = true;
			return;
		}
		else if (e.ColumnIndex != 5)
			return;

		asb.Clear();
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

		oscdg.ShowOSCDg(dtO, ide);
	}

	//// кнопка чтение одной осциллограммы
	//private void dgvOscill_CellContentClick(object sender, DataGridViewCellEventArgs e)
	//{
	//   byte[] arrO = null;
	//   string ifa;         // имя файла
	//   DataGridViewCell de;
	//   char[] sep = { ' ' };
	//   string[] sp;
	//   StringBuilder sb;
	//   if( e.ColumnIndex == 0 )
	//   {
	//      dgvOscill [ e.ColumnIndex, e.RowIndex ].Value = (bool) dgvOscill [ e.ColumnIndex, e.RowIndex ].Value ? false : true;
	//      btnUnionOsc.Enabled = true;
	//      return;
	//   }
	//   else if( e.ColumnIndex != 5 )
	//      return;

	//   btnUnionOsc.Enabled = false;
	//   // сбрасываем все флажки
	//   for( int i = 0 ; i < dtO.Rows.Count ; i++ )
	//      dgvOscill [ 0, i ].Value = false;

	//   try
	//   {
	//      de = dgvOscill [ "clmID", e.RowIndex ];
	//   }
	//   catch
	//   {
	//      MessageBox.Show( "dgvOscill_CellContentClick - исключение" );
	//      return;
	//   }
	//   int ide = (int) de.Value;

	//   // по ide найти запись в dto, извлечь блок с осциллограммой (диаграммой), записать в файл, запустить fastview
	//   // перечисляем записи в dto
	//   int curRow;

	//   for( curRow = 0 ; curRow < dtO.Rows.Count ; curRow++ )
	//   {
	//      if( ide == ( (int) dtO.Rows [ curRow ] [ "ID" ] ) )
	//      {
	//         arrO = (byte [ ]) dtO.Rows [ curRow ] [ "Data" ];
	//         break;
	//      }
	//   }
	//   // записываем массив байт в файл
	//                   ifa = (string)dtO.Rows[curRow]["BlockName"] + "_#_" + Convert.ToString(dtO.Rows[curRow]["BlockID"]) + "_#Tblock_" + Convert.ToString(dtO.Rows[curRow]["TimeBlock"]) + "_#TFC_" + Convert.ToString(dtO.Rows[curRow]["TimeFC"]);

	//                   // удаляем пробелы
	//                   sp = ifa.Split(sep);
	//                   sb = new StringBuilder();
	//                   for (int i = 0; i < sp.Length; i++)
	//                   {
	//                      sb.Append(sp[i]);
	//                   }
	//                   string sbb = sb.ToString();
	//                   sb.Length = 0;
	//                   for (int i = 0; i < sbb.Length; i++)
	//                   {
	//                      if (sbb[i] == '.' || sbb[i] == '/' || sbb[i] == '\\' || sbb[i] == '/' || sbb[i] == ':')
	//                         sb.Append('_');
	//                      else
	//                         sb.Append(sbb[i]);
	//                   }
	//                   sb.Append(".dfr"); 
	//                   #endregion

	//   #region запись в файл, запуск OscView
	//   FileStream f = null;
	//   try
	//   {
	//      f = File.Create( AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + sb.ToString() );
	//   }
	//   catch( Exception ex )
	//   {
	//      MessageBox.Show( ex.ToString() + "\nФайл : " + AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + sb.ToString(), this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
	//      return;
	//   }

	//   f.Write( arrO, 0, arrO.Length );
	//   f.Close();
	//   // запускаем fastview

	//   Process prc = new Process();
	//   prc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\OscView\\OscView.exe  ";
	//   prc.StartInfo.Arguments = "\"" + AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + sb.ToString() + "\"";
	//   prc.Start();
	//   #endregion
	//}
	#endregion
    private void tabPageInfo_Enter( object sender, EventArgs e )
    {
        if( rtbInfo.Lines.Count( ) != 0 )
            return;

        base.FillTAbPageInfo( PanelInfoTextBox, rtbInfo );
    }
   }
}
