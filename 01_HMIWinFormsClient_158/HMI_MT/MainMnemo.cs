using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Calculator;
using LabelTextbox;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using CommonUtils;
using CRZADevices;
using System.Linq;
using System.Xml.Linq;
using FileManager;
using LibraryElements;
using WindowsForms;
using Structure;
using System.Diagnostics;

namespace HMI_MT
{
    public partial class MainMnemo : Form
	{
      #region Свойства
      #endregion
      #region private-члены класса
		/// <summary>
		/// класс для работы с панелями текущего режима
		/// </summary>
		private CurrentModePanels cmp;
      private MainForm parent;
      private string panelName = string.Empty;
      string fileName = "";
      Panel pnl = new Panel( );  // панель для отображения мнемосхемы. ее можно двигать в пределах контейнера для правильного позиционирования при смене монитора
      FormulaEval ev;
      SplitContainer splitContainer1;
      // панели нормального режима
      dlgOptionsFormEditor fnm;

      bool isFirstPaint = false;
      Bitmap t;
      Frm2mnemopanel fct2d;      // форма, на которой отображается мнемосхема

		List<ICalculationControl> cntrllist;
      #endregion
      #region public-члены класса
      #endregion
      #region protected-члены класса
      //protected 
      #endregion
      #region связанные члены класса
      XDocument xdoc;
      List<Element> list;         //список фигур
      #endregion

      #region Конструкторы
      public MainMnemo()
      {
         InitializeComponent();
      }

      public MainMnemo(MainForm linkMainForm, string pn)
      {
         InitializeComponent();

         SetStyle(ControlStyles.UserPaint, true);
         SetStyle(ControlStyles.AllPaintingInWmPaint, true);
         SetStyle(ControlStyles.DoubleBuffer, true);

         parent = linkMainForm;
         panelName = pn;

         #region извлекаем название мнемосхемы
         xdoc = XDocument.Load(parent.PathToPrjFile);
         IEnumerable<XElement> mnemoshems = xdoc.Element("Project").Element("Mnemoshems").Elements("Mnemo");

         //this.Paint +=new PaintEventHandler(MainMnemo_Paint);
         if (panelName == "none")
            return;

         XElement xm;
         foreach (XElement xe in mnemoshems)
            if (xe.Attribute("panel").Value == panelName)
            {
               if ( !String.IsNullOrEmpty( ( string ) xe.Element( "Mnemolevel2" ) ) )
               {
                  fileName = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + xe.Element( "Mnemolevel2" ).Element( "FileName" ).Value;
                  xm = xe.Element( "Mnemolevel2" );
                  //fileName = Application.StartupPath + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + xe.Element("Mnemolevel2").Element("FileName").Value;
               }
               else
               {
                  fileName = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + xe.Element( "FileName" ).Value;
                  xm = xe;
                  //fileName = Application.StartupPath + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + xe.Element( "FileName" ).Value;
               }

               // настраиваем панель
               pnl.Dock = DockStyle.None;
               int x = Int32.Parse( xm.Element( "location" ).Attribute( "x" ).Value );
               int y = Int32.Parse( xm.Element( "location" ).Attribute( "y" ).Value );
               pnl.Location = new Point( x, y );
               int width = Int32.Parse( xm.Element( "size" ).Attribute( "width" ).Value );
               int height = Int32.Parse( xm.Element( "size" ).Attribute( "height" ).Value );
               pnl.Size = new Size( width, height );
               // splitContainer1
               this.splitContainer1 = new System.Windows.Forms.SplitContainer( );
               this.splitContainer1.SuspendLayout( );
               this.SuspendLayout( );
               this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.None;//.FixedSingle;//.None;
               this.splitContainer1.Dock = System.Windows.Forms.DockStyle.None;
               this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
               this.splitContainer1.Name = "splitContainer1";
               this.splitContainer1.Size = new System.Drawing.Size( 1700, 1200 );
               this.splitContainer1.SplitterDistance = 510;
               this.splitContainer1.TabIndex = 0;
               splitContainer1.Panel1Collapsed = true;
               splitContainer1.Parent = pnl;
               pnl.Controls.Add( splitContainer1 );
               pnl.BringToFront( );
               this.Controls.Add( pnl );
               splitContainer1.Width = pnl.ClientSize.Width;
               splitContainer1.Height = pnl.ClientSize.Height;
               splitContainer1.Invalidate( );
               break;
            }
            else
               continue;

         if (String.IsNullOrEmpty(fileName) || !File.Exists(fileName))
         {
            MessageBox.Show("MainMnemo.cs (142) : Ошибка загрузки мнемосхемы : файл : " + fileName, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
         }

         XDocument xdocMnemo = XDocument.Load(fileName);
         if ( !String.IsNullOrEmpty( ( string ) xdocMnemo.Element( "namespace" ).Element( "MnemoCaption" ) ) )
            this.Text = xdocMnemo.Element("namespace").Element("MnemoCaption").Value; 
         #endregion
      }
      #endregion

      #region загрузка, активация, первое отображение, закрытие формы
      /// <summary>
      /// загрузка формы
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param> 
      private void MainMnemo_Load( object sender, EventArgs e )
      {
         // установить текущий каталог
         Directory.SetCurrentDirectory( AppDomain.CurrentDomain.BaseDirectory );

         // настраиваем контекстное меню формы
         //if (!this.DesignMode)
         CommonUtils.CommonUtils.TestUserMenuRights( contextMenuStrip1, parent.arrlUserMenu );
         this.DoubleBuffered = true;

         #region читаем конфигурационный файл - загружаем мнемосхему
         //string fileName = "";

         //xdoc = XDocument.Load(parent.PathToPrjFile);
         //IEnumerable<XElement> mnemoshems = xdoc.Element("Project").Element("Mnemoshems").Elements("Mnemo");

         if (panelName == "none")
            return;

         list = new List<Element>( );
         LoadSchem(fileName, list, /*splitContainer1.Panel2,*/ dctrnk_ChangeValue);
         #endregion
     }

      private void MainMnemo_Shown( object sender, EventArgs e )
      {
         if (panelName == "none")
            return;

			this.BindingLincks();

			if (!isFirstPaint)
				this.MainMnemo_Paint(sender, null);
      }

      private void MainMnemo_Activated( object sender, EventArgs e )
      {
		  DinamicControl tt;

         for ( int i = 0 ; i < this.Controls.Count ; i++ )
            if ( this.Controls[ i ] is DinamicControl )
               tt = ( DinamicControl ) this.Controls[ i ];
      }

      private void MainMnemo_FormClosing( object sender, FormClosingEventArgs e )
      {
		  if (cmp != null)
			  cmp.SavePanels();

		  if (e.CloseReason == CloseReason.MdiFormClosing)
		  {
			  e.Cancel = true;
		  }

		  if (e.Cancel == true)
			  return;

         // удаляем ссылки на теги
		  if (HMI_Settings.ClientDFE != null)
			  HMI_Settings.ClientDFE.RemoveRefToPageTags(this.Text);

         if (t != null)
            t.Dispose( );

		  pnl.Dispose( );
         
		  if ( splitContainer1 != null )
            splitContainer1.Dispose( );
      }
      #endregion

      #region загрузка мнемосхемы
		 /// <summary>
		 /// Создание привязок
		 /// </summary>
		private void BindingLincks()
		{
			if (cntrllist == null) return;
			StringBuilder tagident = new StringBuilder();
			foreach (ICalculationControl icc in cntrllist)
			{
            if (icc.Calculation != null)
               foreach (FormulaTag ft in icc.Calculation.Tags)
               {
                  try
                  {
                     if (!GetEnableStatusDev(ft.NFC, ft.NDev))
                        break;

                     ev = new FormulaEval(parent.KB, "0(" + ft.NFC.ToString() + "." + ft.NDev.ToString() + ".0.60013.0)", "0", "Состояние протокола", "", TypeOfTag.Analog, "");
                  }
                  catch (Exception ex)
                  {
                     MessageBox.Show(" (253) : HMI_MT.MainMnemo.cs : BindingLincks : ошибка : " + ex.Message, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                     continue;
                  }

                  ev.OnChangeValFormTI += icc.LinkSetTextStatusDev;
                  ev.FirstValue();

                  if (HMI_Settings.ClientDFE != null)
                     HMI_Settings.ClientDFE.AddArrTags(this.Text, ev);

                  tagident.Length = 0;
                  tagident.Append(ft.Result);

                  try
                  {
                     ev = new FormulaEval(parent.KB, "0(" + tagident + ")", "0", "", "", TypeOfTag.Discret, "");
                  }
                  catch (Exception ex)
                  {
                     MessageBox.Show(" (268) : HMI_MT.MainMnemo.cs : BindingLincks : ошибка : " + ex.Message, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                     continue;
                  }

                  ev.OnChangeValFormTI += icc.LinkSetText;// привязываем функцию обработки качества тега
                  ev.FirstValue();

                  if (HMI_Settings.ClientDFE != null)
                     HMI_Settings.ClientDFE.AddArrTags(this.Text, ev);
               }

				// выстроим z-порядок
				if (icc is DinamicControl)
				{
					DinamicControl dc = icc as DinamicControl;
					if (dc.Parameters.Name == "ОВОД-МД")
						dc.SendToBack();
					else
						dc.BringToFront();
				}
			}
		}

      XDocument xdoc_CfgCdp;
      IEnumerable<XElement> xefcs;
       /// <summary>
       /// извлечь статус включенности устройства в конфигурацию 
       /// (для того чтобы избежать ошибки отсутсвия тега при попытке подключения к нему)
       /// </summary>
       /// <param Name="nfc"></param>
       /// <param Name="ndev"></param>
       /// <returns></returns>
      private bool GetEnableStatusDev(int nfc, int ndev) 
      {
         if (xdoc_CfgCdp == null)
         {
            xdoc_CfgCdp = XDocument.Load(HMI_Settings.PathToPrgDevCFG_cdp);
            xefcs = xdoc_CfgCdp.Element("MT").Element("Configuration").Elements();//.Descendants("FC");
         }
         
         IEnumerable<XElement> xedevs;

         foreach (XElement xefc in xefcs)
            if (nfc == int.Parse(xefc.Attribute("numFC").Value))
            {
               xedevs = xefc.Descendants("Device");//.Element("FCDevices").Elements

               foreach (XElement xedev in xedevs)
               {
                  if (ndev == int.Parse(xedev.Element("NumDev").Value))
                  {
                     if (xedev.Attribute("enable").Value == "True")
                        return true;
                     else
                        return false;
                  }
               }
            }
         return false;
      }

      /// <summary>
      /// Загрузка файла с мнемосхемой
      /// </summary>
      protected void LoadSchem(string fn, List<Element> list, /*SplitterPanel panel,*/ EventHandler ehls)
      {
         //int _winWidth = 640, _winHeight = 480;
         int _winWidth = 1600, _winHeight = 1200;

         // создаем форму для размещения на панели SplitterPanel panel
         fct2d = new Frm2mnemopanel();
         fct2d.TopLevel = false;
         fct2d.Parent = splitContainer1.Panel2;
         splitContainer1.Panel2.AutoScroll = true;
         fct2d.SendToBack();
         fct2d.Dock = DockStyle.Fill;
         fct2d.AutoScroll = true;
         fct2d.Show();

         this.Paint += new PaintEventHandler(MainMnemo_Paint);

         //t = new Bitmap(fct2d.ClientSize.Width, fct2d.ClientSize.Height);
         //ee = Graphics.FromImage(t);

         parent.UseWaitCursor = true;
         SchemasStream file = new SchemasStream( );
         file.LoadFile( fn );
         file.ReadDatas( ref list, ref _winWidth, ref _winHeight );  // здесь задержка при запуске
			// извлекаем название менмосхемы
		   this.Text = file.GetMnenoCaption();
         file = null;

			this.DeleteElements(list, out cntrllist, fct2d, ehls);

         #region действие: сортировка списка элементов
         list.Sort( ListCompare );
		   fct2d.Refresh( );
         #endregion

         parent.UseWaitCursor = false;

		  /*
		   *  если для данной формы определены панели нормального режима,
		   *  то выведем их
		   */
		   cmp = new CurrentModePanels( parent.UserName ,this);
	  }

      /// <summary>
      /// Удаление всех динамических элементов из общего списка с последующим их созданием
      /// </summary>
		private void DeleteElements(List<Element> list, out List<ICalculationControl> _cntrllist, Form panel, EventHandler eh)
      {
         _cntrllist = new List<ICalculationControl>();
         foreach (Element _search in list)
         {
            if (_search.ElementModel == Model.Dinamic)
            {
               BaseDinamicControl bdc = null;

               DinamicControl idc = new DinamicControl(_search, true);//создание UserControl'a
               if (!idc.InitializeError)
               {
                  idc.AuraOn = false;
                  idc.MouseClick += new MouseEventHandler(dc_MouseClick);
                  _cntrllist.Add((ICalculationControl)idc);
                  bdc = idc;

                  if (panel == null)
                  {
                     idc.Parent = this;
                     this.Controls.Add((DinamicControl)idc);
                  }
                  else
                  {
                     idc.Parent = panel;
                     panel.Controls.Add((DinamicControl)idc);
                  }
               }
               else
               {
                  idc.Dispose();

                  DinamicControlTrunck dctrnk = new DinamicControlTrunck(_search);//создание UserControl'a
                  if (!dctrnk.InitializeError)
                  {
                     dctrnk.AuraOn = false;
                     bdc = dctrnk;

                     if (_search is Key)
                     {
                        dctrnk.MouseClick += new MouseEventHandler(dc_MouseClick);
                        if (panel == null)
                        {
                           dctrnk.Parent = this;
                           this.Controls.Add((DinamicControlTrunck)dctrnk);
                        }
                        else
                        {
                           dctrnk.Parent = panel;
                           panel.Controls.Add((DinamicControlTrunck)dctrnk);
                        }//if_else
                     }//if (_search is Key)
                     else
                     {
                        dctrnk.ChangeValue += new EventHandler(eh);
                        _cntrllist.Add((ICalculationControl)dctrnk);
                     }
                  }
                  else
                     dctrnk.Dispose();
               }//if_else

               if (_search is IDynamicParameters)
               {
                  IDynamicParameters idp = (IDynamicParameters)_search;
                  // извлекаем описание из PrgDevCFG.cdp
                  XElement xeDescDev = CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG(idp.Parameters.FK, idp.Parameters.Device);
                  if (xeDescDev == null)
                     continue;
                  try
                  {
                     string strNameBlock = xeDescDev.Element("nameR").Value;
                     string strRefDesign = xeDescDev.Element("DescDev").Value;

                     // определим тип контекстного меню
                     string contextmenutype = xeDescDev.Element("TypeContextMenu").Value;

                     switch (contextmenutype)
                     {
                        case "Ekra":
                           bdc.ContextMenuStrip = null;
                           break;
                        case "None":
                           bdc.ContextMenuStrip = null;
                           break;
                        case "USO_HANDSET":
                           bdc.ContextMenuStrip = contextMenuStrip_USO_HANDSET;
                           break;
                        case "contextMenuStrip1":
                           bdc.ContextMenuStrip = contextMenuStrip1;
                           break;
                        default:
                           bdc.ContextMenuStrip = contextMenuStrip1;
                           break;
                     }//switch

                     string tfn = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar
                        + strNameBlock + Path.DirectorySeparatorChar
                        + "frm" + xeDescDev.Element("nameELowLevel").Value + ".xml";

                     idp.Parameters.FileNameDescript = tfn;
                     idp.Parameters.Symbol = strRefDesign;
                  }
                  catch (Exception exx) { MessageBox.Show(exx.Message); }
               }//if (_search is IDynamicParameters)
            }//if (_search.ElementModel == Model.Dinamic) 
         }//foreach

         //удаление из списка
         for (int i = 0; i < list.Count; i++)
         {
            if (list[i].ElementModel == Model.Dinamic && (list[i] is DynamicElement || list[i] is Block || list[i] is Key))
            {
               list.RemoveAt(i);
               i = -1;
            }
         }
      }

      private void dctrnk_ChangeValue( object sender, EventArgs e )
      {
			this.MainMnemo_Paint(sender, null);
      }

      private void MainMnemo_Paint(object sender, PaintEventArgs e)
      {
         t = new Bitmap(fct2d.ClientSize.Width, fct2d.ClientSize.Height);

         // Поверхность для рисования на битовой карте!
         Graphics ee = Graphics.FromImage(t);
         
         //Для качественной записи текста в файл
         ee.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

         foreach (Element search in list)
         {
            search.DrawElement(ee);//e.Graphics
         }

         fct2d.BackgroundImage = t;
      }

      /// <summary>
      /// Сортировка элементов по уровню отображения
      /// </summary>
      private static int ListCompare( Element elem1, Element elem2 )
      {
         if ( elem1.Level > elem2.Level )
            return 0;
         else
            if ( elem1.Level < elem2.Level )
               return -1;
            else
               return 1;
      }      
      #endregion

      #region панель нормального режима
      private void параметрыНормальногоРежимаToolStripMenuItem_Click(object sender, EventArgs e)
      {
		  DinamicControl tt = (DinamicControl)contextMenuStrip1.SourceControl;

		  fnm = new dlgOptionsFormEditor(this, tt, parent.KB);
		  fnm.ShowDialog();
        }
      #endregion

      #region команды контекстного меню
      private void включитьToolStripMenuItem_Click( object sender, EventArgs e )
        {
           DinamicControl tt = ( DinamicControl ) contextMenuStrip1.SourceControl;

           //if ( tt.GetRepairStatus( ) )   //if ( tt.isRemont ) - проверка ремонтного положения
           //{
           //   MessageBox.Show( "Действие запрещено!\n Ремонтное положение выключателя!", "Опасно", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
           //   return;
           //}

           if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, parent.UserRight ) )
              return;

           if ( parent.isReqPassword )
              if ( !parent.CanAction( ) )
              {
                 MessageBox.Show( "Выполнение действия запрещено" );
                 return;
              }

           ConfirmCommand dlg = new ConfirmCommand( );
           dlg.label1.Text = "Включить?";

           if ( !( DialogResult.OK == dlg.ShowDialog( ) ) )
              return;

           // выполняем действия по включению выключателя
           // вначале определим устройство

           //ControlSwitch tt = ( ControlSwitch ) contextMenuStrip1.SourceControl;
		   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 618, "(618) MainMnemo.cs : включитьToolStripMenuItem_Click() : Поступила команда \"Включить\" для устройства: " + tt.GetDinamicType() + "; id = " + tt.GetDevice() );
            
            // правильная запись в журнал действий пользователя
            // номер устройства с цчетом фк
           int numdevfc = tt.GetFK() * 256 + tt.GetDevice();
           parent.WriteEventToLog(3, numdevfc.ToString(), this.Name, true);// true, false );

           if ( parent.newKB.ExecuteCommand( tt.GetFK( ), tt.GetDevice( ), "CCB", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
              parent.WriteEventToLog( 35, "Команда \"CCB\" ушла в сеть. Устройство - "
					  + tt.GetFK().ToString() + "." + tt.GetDevice().ToString(), this.Name, true);// true, false );
        }

      private void отключитьToolStripMenuItem_Click( object sender, EventArgs e )
      {
         DinamicControl tt = ( DinamicControl ) contextMenuStrip1.SourceControl;
         
         //if ( tt.GetRepairStatus() )   //if ( tt.isRemont ) - проверка ремонтного положения
         //{
         //   MessageBox.Show( "Действие запрещено!\n Ремонтное положение выключателя!", "Опасно", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
         //   return;
         //}

         if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, parent.UserRight ) )
            return;

         if ( parent.isReqPassword )
            if ( !parent.CanAction( ) )
            {
               MessageBox.Show( "Выполнение действия запрещено" );
               return;
            }

         ConfirmCommand dlg = new ConfirmCommand( );
         dlg.label1.Text = "Отключить?";

         if ( !( DialogResult.OK == dlg.ShowDialog( ) ) )
            return;

         // выполняем действия по отключению выключателя
		 TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 657, "(657) MainMnemo.cs : отключитьToolStripMenuItem_Click() : Поступила команда \"Отключить\" для устройства: " + tt.GetDinamicType() + "; id = " + tt.GetDevice());

         // запись в журнал
         int numdevfc = tt.GetFK() * 256 + tt.GetDevice();

         parent.WriteEventToLog(4, numdevfc.ToString(), this.Name, true);//, true, false );

           if ( parent.newKB.ExecuteCommand( tt.GetFK(), tt.GetDevice(), "OCB", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
              parent.WriteEventToLog( 35, "Команда \"OCB\" ушла в сеть. Устройство - "
					  + tt.GetFK().ToString() + "." + tt.GetDevice().ToString(), this.Name, true);//, true, false );
        }

      private void квитироватьToolStripMenuItem_Click( object sender, EventArgs e )
        {
           ToolStripDropDownItem tsddi = ( ToolStripDropDownItem ) sender;
           ContextMenuStrip tsi = ( ContextMenuStrip ) tsddi.Owner;

           if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b02_ACK_Signaling, parent.UserRight ) )
              return;

           ConfirmCommand dlg = new ConfirmCommand( );
           dlg.label1.Text = "Сбросить сигнализацию?";

           if ( !( DialogResult.OK == dlg.ShowDialog( ) ) )
              return;

           // выполняем действия по квитированию выключателя
           // вначале определим устройство
           DinamicControl tt = ( DinamicControl ) tsi.SourceControl;
		   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 686, "(686) MainMnemo.cs : квитироватьToolStripMenuItem_Click() : Поступила команда \"Сбросить сигнализацию\" для устройства: " + tt.GetDinamicType() + "; id = " + tt.GetDevice());
           // запись в журнал
           int numdevfc = tt.GetFK() * 256 + tt.GetDevice();

           parent.WriteEventToLog(20, numdevfc.ToString(), tt.GetDinamicType(), true);//, true, false );

           if ( parent.newKB.ExecuteCommand( tt.GetFK(), tt.GetDevice(), "ECC", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
              parent.WriteEventToLog(35, "Команда \"Сбросить сигнализацию\" ушла в сеть. Устройство - "
					  + tt.GetFK().ToString() + "." + tt.GetDevice().ToString(), tt.GetDinamicType(), true);//, true, false );
        }
      #endregion      

      #region умная метка
      private void умнаяМеткаToolStripMenuItem_Click( object sender, EventArgs e )
        {
           //ToolStripMenuItem tt = ( ToolStripMenuItem ) sender;
           //SmartLabel sm = new SmartLabel( parent.KB );
           //sm.Left = cmsFrmMnemo.Left;
           //sm.Top = cmsFrmMnemo.Top;
           //sm.AutoSize = true;
           //sm.Show();
           //this.Controls.Add( sm );
           //pb.WireControl( sm );
           //sm.BringToFront(); 
        }
      #endregion

      private void MainMnemo_MouseClick( object sender, MouseEventArgs e )
        {
          //pb.HideHandles();
          //foreach ( Element _searchelem in list )//*/IDrawAllElements
          //{
          //   if (_searchelem.Collision()
          //      _searchelem.DrawElement( e.Graphics );
          //}
        }

      #region ремонтное положение
      private void tsmiRemont_CheckedChanged( object sender, EventArgs e )
       {
          //DinamicControl tt = ( DinamicControl ) contextMenuStrip1.SourceControl;
          //if ( tsmiRemont.Checked )
          //   tt.SetRepairStatus(true);
          //else
          //   tt.SetRepairStatus( false ); 
         
          //tt.Invalidate( );

          //// устанавливаем состояние устройства
          //foreach ( FC aFc in parent.newKB.KB )
          //   foreach ( TCRZADirectDevice tdd in aFc )
          //      if ( tdd.NumDev == tt.GetDevice() )
          //         tdd.isRemont = tt.GetRepairStatus();
       } 
      #endregion

      #region прибор, параметры нормального режима
      private void параметрыНормальногоРежимаToolStripMenuItem_Click_1( object sender, EventArgs e )
       {
		  //PanelNormalMode pnm = new PanelNormalMode( );
		  //pnm.MdiParent = this;
		  //pnm.Show( );
       }
      #endregion

      #region действия с динамическими элементами

      protected void dc_MouseClick( object sender, MouseEventArgs e )
      {
         if (e.Button == MouseButtons.Right)
            return;

         IDynamicParameters idp = sender as IDynamicParameters;

         if (idp != null)
         {
            int nFC = idp.Parameters.FK;
            int idDev = idp.Parameters.Device;

            // извлекаем описание из PrgDevCFG.cdp
            XElement xeDescDev = CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG( nFC, idDev );

            if ( xeDescDev == null )
            {
               MessageBox.Show( "Элемент не привязан к устройству в текущей конфигурации." , this.Name, MessageBoxButtons.OK,MessageBoxIcon.Warning);
               return;
            }

            string strNameBlock = xeDescDev.Element( "nameR" ).Value;
            string strRefDesign = xeDescDev.Element( "DescDev" ).Value;

            string FileNameDescript = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + strNameBlock + Path.DirectorySeparatorChar
               + "frm" + xeDescDev.Element( "nameELowLevel" ).Value + ".xml";

            if ( !File.Exists( FileNameDescript ) )
            {
               MessageBox.Show( "Файл описания формы не существует (" + FileNameDescript + ")", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error );
               return;
            }

            #region по двум условиям проверяем наличие устройства в конфигурации
            bool isFnd = false;
            foreach ( DataSource aFc in parent.KB )
            foreach (TCRZADirectDevice tdd in aFc)
            {
               if (tdd.NumDev == idDev)
               {
                  isFnd = true;
                  break;
               }
            }
            if ( !isFnd )
            {
               MessageBox.Show("Устройство " + idDev.ToString() + " отсутствует в активной конфигурации.\nДля активизации устройства исправьте конфигурацию.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
               return;
            }
            if (idp.Parameters.Cell == 0)
            {
               MessageBox.Show("Нулевой номер ячейки для устройства " + idDev.ToString() + ".\n Для активизации устройства исправьте конфигурацию.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
               return;
            } 
            #endregion

            Form frm = new Form( );

            string strP = FileNameDescript;

            switch (HMI_Settings.slDevClasses[strNameBlock].ToString())
            {
               case "ControlSwitch":
                  frm = new frmBMRZ(parent, idp.Parameters.FK, idp.Parameters.Device, idp.Parameters.Cell, strP);
                  break;
               case "ControlSwitch_Sirius":
                  //frm = new frmSirius_SP(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                  break;
               case "ЭКРА":
                  //frm = new frmEkra(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                  break;
               case "ОВОД":
                  //frm = new frmOvod_SP(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                  break;
               case "Masterpact":
                  //frm = new frmMasterpact(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                  break;
               case "ENIP":
                  //frm = new frmEnip(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                  break;
               default:
                  return;
            }

            frm.Text = strNameBlock + " ( ид.№ " + idp.Parameters.Device + " ) : " + idp.Parameters.Symbol;
            string sf = frm.Name;

            XDocument reader = XDocument.Load( strP );
		      int devguid = idp.Parameters.FK * 256 + idp.Parameters.Device;

            bool isconnectState = false; 
		      string connectState = PTKState.Iinstance.GetValueAsString(devguid.ToString(), "Связь");

            if (bool.TryParse(connectState, out isconnectState))
            {
               if (!isconnectState)
                  MessageBox.Show("Терминал недоступен или с ним нет связи", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            frm.Name = reader.Element( "MT" ).Element( "BMRZ" ).Element( "frame" ).Attribute( "Name" ).Value;

            foreach ( Form f in parent.arrFrm )
            {
               if ( f.Text == frm.Text )
               {
                  f.Focus( );
                  frm.Dispose( );
                  return;
               }
            }

            frm.MdiParent = this.MdiParent;
            frm.MaximumSize = this.Size;
            frm.Dock = DockStyle.Fill;//.Size = frm.MdiParent.ClientSize; //parent.ClientSize;//.Size;.MaximumSize
            frm.WindowState = FormWindowState.Maximized;
            frm.Show( );
            parent.arrFrm.Add( frm );
         }//if (idp != null)
      }

		private void contextMenuStrip1_Opening( object sender, CancelEventArgs e )
		{
         IDynamicParameters idp = sender as IDynamicParameters;
         if (idp != null)
         {
            int compressnumdev = idp.Parameters.FK * 256 + idp.Parameters.Device;
            CustomizeContextMenuItems(contextMenuStrip1, compressnumdev);
         }
		}

		/// <summary>
		/// настройка видимости пунктов контекстного меню блоков
		/// </summary>
		/// <param name="cms">контекстное меню</param>
		/// <param name="compressnumdev">DevGUID устройства</param>
		private void CustomizeContextMenuItems( ContextMenuStrip cms,int compressnumdev)
		{
			try
			{
				IEnumerable<XElement> xecmstrips = (CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG(compressnumdev)).Element("TypeContextMenu").Elements("ContextMenuItem");

				foreach (XElement xecmstrip in xecmstrips)
				{
					if (PTKState.Iinstance.IsAdapterExist(compressnumdev.ToString(), xecmstrip.Attribute("name_adapter4enable").Value))
					{
						bool rpo = bool.Parse(PTKState.Iinstance.GetValueAsString(compressnumdev.ToString(), xecmstrip.Attribute("name_adapter4enable").Value));
						if (!rpo)
							contextMenuStrip1.Items[xecmstrip.Attribute("name").Value].Enabled = false;
						else
							contextMenuStrip1.Items[xecmstrip.Attribute("name").Value].Enabled = true;
					}
					else
						contextMenuStrip1.Items[xecmstrip.Attribute("name").Value].Enabled = true;
				}
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}
      #endregion
       /// <summary>
       /// реакция на выбор меню замыкания/размыкания ноже УСО, управляемых вручную
       /// </summary>
       /// <param Name="sender"></param>
       /// <param Name="e"></param>
      private void toolStripMenuItem_usohandset_On_Click(object sender, EventArgs e)
      {
         ToolStripDropDownItem tsddi = (ToolStripDropDownItem)sender;
         ContextMenuStrip tsi = (ContextMenuStrip)tsddi.Owner;
         IDynamicParameters idp = tsi.SourceControl as IDynamicParameters;

         if (idp != null)
         {
            /*
             * для идентификации ножа исп номер ячейки, кот передается в качестве параметра и относительно этого номера происходит связка
             * ножа и его описания в файле на сервере
             */
            switch ((sender as ToolStripMenuItem).Text)
            {
               case "Замкнуть":
                  if (parent.newKB.ExecuteCommand(idp.Parameters.FK, idp.Parameters.Device, "OCB", String.Empty, BitConverter.GetBytes(idp.Parameters.Cell), parent.toolStripProgressBar1, parent.statusStrip1, parent))
                     parent.WriteEventToLog(35, "Команда \"Замкнуть\" ушла в сеть. Устройство - "
                     + idp.Parameters.FK.ToString() + "." + idp.Parameters.FK.ToString(), idp.Parameters.Type, true);//, true, false );
                  break;
               case "Разомкнуть":
                  if (parent.newKB.ExecuteCommand(idp.Parameters.FK, idp.Parameters.Device, "CCB", String.Empty, BitConverter.GetBytes(idp.Parameters.Cell), parent.toolStripProgressBar1, parent.statusStrip1, parent))
                     parent.WriteEventToLog(35, "Команда \"Разомкнуть\" ушла в сеть. Устройство - "
                     + idp.Parameters.FK.ToString() + "." + idp.Parameters.FK.ToString(), idp.Parameters.Type, true);//, true, false );
                  break;
               default:
                  break;
            }
         }//if         
      }
	}
}