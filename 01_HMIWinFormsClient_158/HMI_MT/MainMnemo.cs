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
      #region ��������
      #endregion
      #region private-����� ������
		/// <summary>
		/// ����� ��� ������ � �������� �������� ������
		/// </summary>
		private CurrentModePanels cmp;
      private MainForm parent;
      private string panelName = string.Empty;
      string fileName = "";
      Panel pnl = new Panel( );  // ������ ��� ����������� ����������. �� ����� ������� � �������� ���������� ��� ����������� ���������������� ��� ����� ��������
      FormulaEval ev;
      SplitContainer splitContainer1;
      // ������ ����������� ������
      dlgOptionsFormEditor fnm;

      bool isFirstPaint = false;
      Bitmap t;
      Frm2mnemopanel fct2d;      // �����, �� ������� ������������ ����������

		List<ICalculationControl> cntrllist;
      #endregion
      #region public-����� ������
      #endregion
      #region protected-����� ������
      //protected 
      #endregion
      #region ��������� ����� ������
      XDocument xdoc;
      List<Element> list;         //������ �����
      #endregion

      #region ������������
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

         #region ��������� �������� ����������
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

               // ����������� ������
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
            MessageBox.Show("MainMnemo.cs (142) : ������ �������� ���������� : ���� : " + fileName, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
         }

         XDocument xdocMnemo = XDocument.Load(fileName);
         if ( !String.IsNullOrEmpty( ( string ) xdocMnemo.Element( "namespace" ).Element( "MnemoCaption" ) ) )
            this.Text = xdocMnemo.Element("namespace").Element("MnemoCaption").Value; 
         #endregion
      }
      #endregion

      #region ��������, ���������, ������ �����������, �������� �����
      /// <summary>
      /// �������� �����
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param> 
      private void MainMnemo_Load( object sender, EventArgs e )
      {
         // ���������� ������� �������
         Directory.SetCurrentDirectory( AppDomain.CurrentDomain.BaseDirectory );

         // ����������� ����������� ���� �����
         //if (!this.DesignMode)
         CommonUtils.CommonUtils.TestUserMenuRights( contextMenuStrip1, parent.arrlUserMenu );
         this.DoubleBuffered = true;

         #region ������ ���������������� ���� - ��������� ����������
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

         // ������� ������ �� ����
		  if (HMI_Settings.ClientDFE != null)
			  HMI_Settings.ClientDFE.RemoveRefToPageTags(this.Text);

         if (t != null)
            t.Dispose( );

		  pnl.Dispose( );
         
		  if ( splitContainer1 != null )
            splitContainer1.Dispose( );
      }
      #endregion

      #region �������� ����������
		 /// <summary>
		 /// �������� ��������
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

                     ev = new FormulaEval(parent.KB, "0(" + ft.NFC.ToString() + "." + ft.NDev.ToString() + ".0.60013.0)", "0", "��������� ���������", "", TypeOfTag.Analog, "");
                  }
                  catch (Exception ex)
                  {
                     MessageBox.Show(" (253) : HMI_MT.MainMnemo.cs : BindingLincks : ������ : " + ex.Message, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                     MessageBox.Show(" (268) : HMI_MT.MainMnemo.cs : BindingLincks : ������ : " + ex.Message, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                     continue;
                  }

                  ev.OnChangeValFormTI += icc.LinkSetText;// ����������� ������� ��������� �������� ����
                  ev.FirstValue();

                  if (HMI_Settings.ClientDFE != null)
                     HMI_Settings.ClientDFE.AddArrTags(this.Text, ev);
               }

				// �������� z-�������
				if (icc is DinamicControl)
				{
					DinamicControl dc = icc as DinamicControl;
					if (dc.Parameters.Name == "����-��")
						dc.SendToBack();
					else
						dc.BringToFront();
				}
			}
		}

      XDocument xdoc_CfgCdp;
      IEnumerable<XElement> xefcs;
       /// <summary>
       /// ������� ������ ������������ ���������� � ������������ 
       /// (��� ���� ����� �������� ������ ��������� ���� ��� ������� ����������� � ����)
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
      /// �������� ����� � �����������
      /// </summary>
      protected void LoadSchem(string fn, List<Element> list, /*SplitterPanel panel,*/ EventHandler ehls)
      {
         //int _winWidth = 640, _winHeight = 480;
         int _winWidth = 1600, _winHeight = 1200;

         // ������� ����� ��� ���������� �� ������ SplitterPanel panel
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
         file.ReadDatas( ref list, ref _winWidth, ref _winHeight );  // ����� �������� ��� �������
			// ��������� �������� ����������
		   this.Text = file.GetMnenoCaption();
         file = null;

			this.DeleteElements(list, out cntrllist, fct2d, ehls);

         #region ��������: ���������� ������ ���������
         list.Sort( ListCompare );
		   fct2d.Refresh( );
         #endregion

         parent.UseWaitCursor = false;

		  /*
		   *  ���� ��� ������ ����� ���������� ������ ����������� ������,
		   *  �� ������� ��
		   */
		   cmp = new CurrentModePanels( parent.UserName ,this);
	  }

      /// <summary>
      /// �������� ���� ������������ ��������� �� ������ ������ � ����������� �� ���������
      /// </summary>
		private void DeleteElements(List<Element> list, out List<ICalculationControl> _cntrllist, Form panel, EventHandler eh)
      {
         _cntrllist = new List<ICalculationControl>();
         foreach (Element _search in list)
         {
            if (_search.ElementModel == Model.Dinamic)
            {
               BaseDinamicControl bdc = null;

               DinamicControl idc = new DinamicControl(_search, true);//�������� UserControl'a
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

                  DinamicControlTrunck dctrnk = new DinamicControlTrunck(_search);//�������� UserControl'a
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
                  // ��������� �������� �� PrgDevCFG.cdp
                  XElement xeDescDev = CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG(idp.Parameters.FK, idp.Parameters.Device);
                  if (xeDescDev == null)
                     continue;
                  try
                  {
                     string strNameBlock = xeDescDev.Element("nameR").Value;
                     string strRefDesign = xeDescDev.Element("DescDev").Value;

                     // ��������� ��� ������������ ����
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

         //�������� �� ������
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

         // ����������� ��� ��������� �� ������� �����!
         Graphics ee = Graphics.FromImage(t);
         
         //��� ������������ ������ ������ � ����
         ee.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

         foreach (Element search in list)
         {
            search.DrawElement(ee);//e.Graphics
         }

         fct2d.BackgroundImage = t;
      }

      /// <summary>
      /// ���������� ��������� �� ������ �����������
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

      #region ������ ����������� ������
      private void ��������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
      {
		  DinamicControl tt = (DinamicControl)contextMenuStrip1.SourceControl;

		  fnm = new dlgOptionsFormEditor(this, tt, parent.KB);
		  fnm.ShowDialog();
        }
      #endregion

      #region ������� ������������ ����
      private void ��������ToolStripMenuItem_Click( object sender, EventArgs e )
        {
           DinamicControl tt = ( DinamicControl ) contextMenuStrip1.SourceControl;

           //if ( tt.GetRepairStatus( ) )   //if ( tt.isRemont ) - �������� ���������� ���������
           //{
           //   MessageBox.Show( "�������� ���������!\n ��������� ��������� �����������!", "������", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
           //   return;
           //}

           if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, parent.UserRight ) )
              return;

           if ( parent.isReqPassword )
              if ( !parent.CanAction( ) )
              {
                 MessageBox.Show( "���������� �������� ���������" );
                 return;
              }

           ConfirmCommand dlg = new ConfirmCommand( );
           dlg.label1.Text = "��������?";

           if ( !( DialogResult.OK == dlg.ShowDialog( ) ) )
              return;

           // ��������� �������� �� ��������� �����������
           // ������� ��������� ����������

           //ControlSwitch tt = ( ControlSwitch ) contextMenuStrip1.SourceControl;
		   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 618, "(618) MainMnemo.cs : ��������ToolStripMenuItem_Click() : ��������� ������� \"��������\" ��� ����������: " + tt.GetDinamicType() + "; id = " + tt.GetDevice() );
            
            // ���������� ������ � ������ �������� ������������
            // ����� ���������� � ������ ��
           int numdevfc = tt.GetFK() * 256 + tt.GetDevice();
           parent.WriteEventToLog(3, numdevfc.ToString(), this.Name, true);// true, false );

           if ( parent.newKB.ExecuteCommand( tt.GetFK( ), tt.GetDevice( ), "CCB", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
              parent.WriteEventToLog( 35, "������� \"CCB\" ���� � ����. ���������� - "
					  + tt.GetFK().ToString() + "." + tt.GetDevice().ToString(), this.Name, true);// true, false );
        }

      private void ���������ToolStripMenuItem_Click( object sender, EventArgs e )
      {
         DinamicControl tt = ( DinamicControl ) contextMenuStrip1.SourceControl;
         
         //if ( tt.GetRepairStatus() )   //if ( tt.isRemont ) - �������� ���������� ���������
         //{
         //   MessageBox.Show( "�������� ���������!\n ��������� ��������� �����������!", "������", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
         //   return;
         //}

         if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, parent.UserRight ) )
            return;

         if ( parent.isReqPassword )
            if ( !parent.CanAction( ) )
            {
               MessageBox.Show( "���������� �������� ���������" );
               return;
            }

         ConfirmCommand dlg = new ConfirmCommand( );
         dlg.label1.Text = "���������?";

         if ( !( DialogResult.OK == dlg.ShowDialog( ) ) )
            return;

         // ��������� �������� �� ���������� �����������
		 TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 657, "(657) MainMnemo.cs : ���������ToolStripMenuItem_Click() : ��������� ������� \"���������\" ��� ����������: " + tt.GetDinamicType() + "; id = " + tt.GetDevice());

         // ������ � ������
         int numdevfc = tt.GetFK() * 256 + tt.GetDevice();

         parent.WriteEventToLog(4, numdevfc.ToString(), this.Name, true);//, true, false );

           if ( parent.newKB.ExecuteCommand( tt.GetFK(), tt.GetDevice(), "OCB", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
              parent.WriteEventToLog( 35, "������� \"OCB\" ���� � ����. ���������� - "
					  + tt.GetFK().ToString() + "." + tt.GetDevice().ToString(), this.Name, true);//, true, false );
        }

      private void �����������ToolStripMenuItem_Click( object sender, EventArgs e )
        {
           ToolStripDropDownItem tsddi = ( ToolStripDropDownItem ) sender;
           ContextMenuStrip tsi = ( ContextMenuStrip ) tsddi.Owner;

           if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b02_ACK_Signaling, parent.UserRight ) )
              return;

           ConfirmCommand dlg = new ConfirmCommand( );
           dlg.label1.Text = "�������� ������������?";

           if ( !( DialogResult.OK == dlg.ShowDialog( ) ) )
              return;

           // ��������� �������� �� ������������ �����������
           // ������� ��������� ����������
           DinamicControl tt = ( DinamicControl ) tsi.SourceControl;
		   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 686, "(686) MainMnemo.cs : �����������ToolStripMenuItem_Click() : ��������� ������� \"�������� ������������\" ��� ����������: " + tt.GetDinamicType() + "; id = " + tt.GetDevice());
           // ������ � ������
           int numdevfc = tt.GetFK() * 256 + tt.GetDevice();

           parent.WriteEventToLog(20, numdevfc.ToString(), tt.GetDinamicType(), true);//, true, false );

           if ( parent.newKB.ExecuteCommand( tt.GetFK(), tt.GetDevice(), "ECC", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
              parent.WriteEventToLog(35, "������� \"�������� ������������\" ���� � ����. ���������� - "
					  + tt.GetFK().ToString() + "." + tt.GetDevice().ToString(), tt.GetDinamicType(), true);//, true, false );
        }
      #endregion      

      #region ����� �����
      private void ����������ToolStripMenuItem_Click( object sender, EventArgs e )
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

      #region ��������� ���������
      private void tsmiRemont_CheckedChanged( object sender, EventArgs e )
       {
          //DinamicControl tt = ( DinamicControl ) contextMenuStrip1.SourceControl;
          //if ( tsmiRemont.Checked )
          //   tt.SetRepairStatus(true);
          //else
          //   tt.SetRepairStatus( false ); 
         
          //tt.Invalidate( );

          //// ������������� ��������� ����������
          //foreach ( FC aFc in parent.newKB.KB )
          //   foreach ( TCRZADirectDevice tdd in aFc )
          //      if ( tdd.NumDev == tt.GetDevice() )
          //         tdd.isRemont = tt.GetRepairStatus();
       } 
      #endregion

      #region ������, ��������� ����������� ������
      private void ��������������������������ToolStripMenuItem_Click_1( object sender, EventArgs e )
       {
		  //PanelNormalMode pnm = new PanelNormalMode( );
		  //pnm.MdiParent = this;
		  //pnm.Show( );
       }
      #endregion

      #region �������� � ������������� ����������

      protected void dc_MouseClick( object sender, MouseEventArgs e )
      {
         if (e.Button == MouseButtons.Right)
            return;

         IDynamicParameters idp = sender as IDynamicParameters;

         if (idp != null)
         {
            int nFC = idp.Parameters.FK;
            int idDev = idp.Parameters.Device;

            // ��������� �������� �� PrgDevCFG.cdp
            XElement xeDescDev = CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG( nFC, idDev );

            if ( xeDescDev == null )
            {
               MessageBox.Show( "������� �� �������� � ���������� � ������� ������������." , this.Name, MessageBoxButtons.OK,MessageBoxIcon.Warning);
               return;
            }

            string strNameBlock = xeDescDev.Element( "nameR" ).Value;
            string strRefDesign = xeDescDev.Element( "DescDev" ).Value;

            string FileNameDescript = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + strNameBlock + Path.DirectorySeparatorChar
               + "frm" + xeDescDev.Element( "nameELowLevel" ).Value + ".xml";

            if ( !File.Exists( FileNameDescript ) )
            {
               MessageBox.Show( "���� �������� ����� �� ���������� (" + FileNameDescript + ")", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error );
               return;
            }

            #region �� ���� �������� ��������� ������� ���������� � ������������
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
               MessageBox.Show("���������� " + idDev.ToString() + " ����������� � �������� ������������.\n��� ����������� ���������� ��������� ������������.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
               return;
            }
            if (idp.Parameters.Cell == 0)
            {
               MessageBox.Show("������� ����� ������ ��� ���������� " + idDev.ToString() + ".\n ��� ����������� ���������� ��������� ������������.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
               case "����":
                  //frm = new frmEkra(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                  break;
               case "����":
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

            frm.Text = strNameBlock + " ( ��.� " + idp.Parameters.Device + " ) : " + idp.Parameters.Symbol;
            string sf = frm.Name;

            XDocument reader = XDocument.Load( strP );
		      int devguid = idp.Parameters.FK * 256 + idp.Parameters.Device;

            bool isconnectState = false; 
		      string connectState = PTKState.Iinstance.GetValueAsString(devguid.ToString(), "�����");

            if (bool.TryParse(connectState, out isconnectState))
            {
               if (!isconnectState)
                  MessageBox.Show("�������� ���������� ��� � ��� ��� �����", "��������������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
		/// ��������� ��������� ������� ������������ ���� ������
		/// </summary>
		/// <param name="cms">����������� ����</param>
		/// <param name="compressnumdev">DevGUID ����������</param>
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
       /// ������� �� ����� ���� ���������/���������� ���� ���, ����������� �������
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
             * ��� ������������� ���� ��� ����� ������, ��� ���������� � �������� ��������� � ������������ ����� ������ ���������� ������
             * ���� � ��� �������� � ����� �� �������
             */
            switch ((sender as ToolStripMenuItem).Text)
            {
               case "��������":
                  if (parent.newKB.ExecuteCommand(idp.Parameters.FK, idp.Parameters.Device, "OCB", String.Empty, BitConverter.GetBytes(idp.Parameters.Cell), parent.toolStripProgressBar1, parent.statusStrip1, parent))
                     parent.WriteEventToLog(35, "������� \"��������\" ���� � ����. ���������� - "
                     + idp.Parameters.FK.ToString() + "." + idp.Parameters.FK.ToString(), idp.Parameters.Type, true);//, true, false );
                  break;
               case "����������":
                  if (parent.newKB.ExecuteCommand(idp.Parameters.FK, idp.Parameters.Device, "CCB", String.Empty, BitConverter.GetBytes(idp.Parameters.Cell), parent.toolStripProgressBar1, parent.statusStrip1, parent))
                     parent.WriteEventToLog(35, "������� \"����������\" ���� � ����. ���������� - "
                     + idp.Parameters.FK.ToString() + "." + idp.Parameters.FK.ToString(), idp.Parameters.Type, true);//, true, false );
                  break;
               default:
                  break;
            }
         }//if         
      }
	}
}