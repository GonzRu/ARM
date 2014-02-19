/*#############################################################################
 *    Copyright (C) 2008 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Форма быстрого доступа к устройствам
 *                                                                             
 *	Файл                     : SpeedAccess.cs                                          
 *	Тип конечного файла      :                                          
 *	версия ПО для разработки : С#, Framework 3.5                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 24.12.2008                                       
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *############################################################################*/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;

using HelperLibrary;
using LibraryElements;
using WindowsForms;
using System.Diagnostics;
using InterfaceLibrary;

namespace HMI_MT
{
    public partial class SpeedAccess : Form, IResetStateProtocol
    {
        #region private-члены класса
        private MainForm parent;
        ArrayList aCS = new ArrayList();
        ArrayList aBut = new ArrayList();
        ArrayList arrNormalModeForms = new ArrayList();	// массив форм панелей нормального режима
        IList<BaseRegion> panelRegions = new List<BaseRegion>();

        /// <summary>
        /// путь к файлу Configuration.cfg
        /// </summary>
        string PathToConfigurationCfg = String.Empty;
        XDocument xdoc;
        /// <summary>
        /// путь к файлу PrgDevCFG.cdp
        /// </summary>
        string PathToDevCfg = String.Empty;
        XDocument xdocCFG;

        // сортированный список для хранения описания устройств, упорядоченный по значению ячейки
        SortedList<uint, XElement> sldev;
        List<ITag> taglist = new List<ITag>();
        #endregion


        #region конструктор
        public SpeedAccess( MainForm linkMainForm )//, int iFC, int iIDDev, int inumLoc
        {
            InitializeComponent();
            parent = linkMainForm;
        }
        #endregion

        private void SpeedAccess_Layout( object sender, LayoutEventArgs e )
        {
            this.Width = this.Parent.Width;
            this.Height = this.Parent.Width;
        }

        /// <summary>
        /// void SpeedAccess_Load()
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void SpeedAccess_Load( object sender, EventArgs e )
        {
            // установить текущий каталог
            Directory.SetCurrentDirectory( AppDomain.CurrentDomain.BaseDirectory );

            // настраиваем контекстное меню формы
            //CommonUtils.CommonUtils.TestUserMenuRights( contextMenuStrip1, parent.arrlUserMenu );

            #region читаем содержимое файла конфигурации устройств проекта - PrgDevCFG.cdp
            PathToDevCfg = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project\\Configuration\\0#DataServer\\Sources\\MOA_ECU" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";

            if ( !File.Exists( PathToDevCfg ) )
                throw new Exception( "Не задан файл конфигурации устройств проекта PrgDevCFG.cdp" );

            xdocCFG = XDocument.Load( PathToDevCfg );
            #endregion

            #region читаем содержимое файла конфигурации проекта - Configuration.cfg
            PathToConfigurationCfg = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "Configuration.cfg";

            if ( !File.Exists( PathToConfigurationCfg ) )
                throw new Exception( "Не задан файл конфигурации  проекта Project.cfg" );

            xdoc = XDocument.Load( PathToConfigurationCfg );
            #endregion

            #region соберем описания всех устройств в сортированный список sldev
            /* 
          * ключ - уникальный номер устройства DevGUID
          * значение - XElement с описанием устройства          
          */
            StringBuilder sbnl = new StringBuilder();

            IEnumerable<XElement> devlist = xdocCFG.Element( "MTRA" ).Element( "Configuration" ).Descendants( "Device" );

            sldev = new SortedList<uint, XElement>();
            foreach ( XElement xe in devlist )
            {
                sbnl.Length = 0;
                sbnl.Append( xe.Attribute( "objectGUID" ).Value.ToString() );
                sldev.Add( Convert.ToUInt32( sbnl.ToString() ), xe );
            }
            #endregion

            #region читаем конфигурационный файл - выясняем число секций - перечисляем секции - генерим TabPage

            XElement section_configuration = xdoc.Element( "Project" ).Element( "SectionConfiguration" );
            IEnumerable<XElement> sections = section_configuration.Descendants( "Section" );

            foreach ( XElement xt in sections )
            {
                TabPage tp = new TabPage( xt.Attribute( "name" ).Value );

                // создаем иерархию вкладок для подгрупп (если они есть)
                //IEnumerable<XElement> xesgs = xt.Elements("SubSection");
                //if (xesgs.Count() > 0)
                CreateTP4SubGroup( tp, xt );
                //else
                //   CreateFLP4Devices(tp, xt);

                tcSpeedAccess.TabPages.Add( tp );
            }
            #endregion
        }

        private void CreateTP4SubGroup( TabPage tp, XElement xt )
        {
            FlowLayoutPanel flpGB = new FlowLayoutPanel();
            flpGB.Dock = DockStyle.Fill;
            flpGB.AutoScroll = true;
            flpGB.Parent = tp;

            // размещаю подгруппы и устройства вне подгрупп

            IEnumerable<XElement> xesgss = xt.Elements( "SubSection" );

            // если есть подгруппы
            foreach ( XElement xtt in xesgss )
                CreateGBForSubGroup( flpGB, xtt );

            CreateFLP4Devices( null, flpGB, xt );
        }

        void CreateGBForSubGroup( FlowLayoutPanel flpGB, XElement xtt )
        {
            GroupBox gbn = new GroupBox();
            gbn.Text = xtt.Attribute( "name" ).Value;

            FlowLayoutPanel flpGBB = new FlowLayoutPanel();
            flpGBB.Dock = DockStyle.Fill;
            flpGBB.AutoScroll = true;
            gbn.Controls.Add( flpGBB );

            flpGB.Controls.Add( gbn );

            CreateFLP4Devices( gbn, flpGBB, xtt );
            gbn.Height += flpGBB.Height * 1 / 2;
        }

        private void CreateFLP4Devices( GroupBox gbpar, FlowLayoutPanel flpgb, XElement xt )
        {
            IEnumerable<XElement> bays = xt.Elements( "Bay" );

            foreach ( XElement bayinthesec in bays )
            {
                // генерим groupbox
                GroupBox gbs = new GroupBox();
                gbs.Text = CommonUtils.CommonUtils.GetDispCaptionForDevice( int.Parse( bayinthesec.Attribute( "key" ).Value ) );

                FlowLayoutPanel flpgbs = new FlowLayoutPanel();
                //flpgbs.AutoSize = true;
                flpgbs.Dock = DockStyle.Fill;
                flpgbs.FlowDirection = FlowDirection.TopDown;
                flpgbs.BorderStyle = BorderStyle.FixedSingle;

                if ( FindDevForGB( flpgbs, gbs, bayinthesec.Attribute( "key" ).Value ) )
                    gbs.Controls.Add( flpgbs );
                else
                {
                    Trace.TraceInformation( "\n" + DateTime.Now.ToString() + ": Блок в ячейке " + bayinthesec.Attribute( "key" ).Value + " не найден" );
                    continue;
                }
                flpgb.Controls.Add( gbs );
            }
            if ( gbpar != null )
                gbpar.Width = bays.Count() * gbpar.Width + gbpar.Width * 1 / 4;

            #region это временная заплатка для того чтобы сделать кнопку для ауры
            IEnumerable<XElement> Buttons = xt.Elements( "Button" );
            foreach ( XElement buttoninthesec in Buttons )
            {
                // генерим groupbox
                GroupBox gbs = new GroupBox();
                gbs.Text = buttoninthesec.Attribute( "frmSpeedAccess_name" ).Value;
                FlowLayoutPanel flpgbs = new FlowLayoutPanel();
                //flpgbs.AutoSize = true;
                flpgbs.Dock = DockStyle.Fill;
                flpgbs.FlowDirection = FlowDirection.TopDown;
                flpgbs.BorderStyle = BorderStyle.FixedSingle;
                Button btn = new Button();
                btn.Parent = flpgbs;
                btn.Dock = DockStyle.Fill;
                btn.Text = buttoninthesec.Attribute( "text_button" ).Value;
                btn.AutoSize = false;
                btn.Width = 75;
                btn.Height = 50;
                btn.BackColor = Color.LightSalmon;
                gbs.Controls.Add( btn );
                flpgb.Controls.Add( gbs );
                gbpar.Width = btn.Width + gbpar.Width * 2 / 4;

                switch ( gbs.Text )
                {
                    case "КТС АУРА":
                        url = buttoninthesec.Attribute( "url" ).Value;
                        btn.Click += new EventHandler( btn_Click );
                        break;
                    case "ШПТ":
                        btn.Click += new EventHandler( btnSpt_Click );
                        break;
                    default:
                        break;
                }
            }
            #endregion
        }

        string url = string.Empty;

        void btn_Click( object sender, EventArgs e )
        {
            Process.Start( url );
        }
        void btnSpt_Click( object sender, EventArgs e )
        {
            // создаем доп форму для ШПТ
            CreateShPTForm();
        }

        #region Создание формы для панели ШПТ
        Form ShptForm;
        TextBox tbUonBus;
        XDocument xdocaddfrm;
        /// <summary>
        /// Создание формы для панели ШПТ
        /// создается как подчиненная форма для 
        /// формы мнемосхемы
        /// </summary>
        private void CreateShPTForm()
        {
            //ShptForm = new Form();
            //ShptForm.Owner = this;
            //ShptForm.ControlBox = true;
            //ShptForm.ShowIcon = false;
            //ShptForm.StartPosition = FormStartPosition.Manual;
            //ShptForm.AutoSize = false;
            //ShptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            //ShptForm.ShowInTaskbar = false;
            ////ShptForm.MouseDoubleClick += new MouseEventHandler( ShptForm_MouseDoubleClick );
            //ShptForm.FormClosing += new FormClosingEventHandler(ShptForm_FormClosing);

            //// настройки из конфиг файла
            //xdocaddfrm = XDocument.Load(Path.GetDirectoryName(HMI_Settings.PathToPrjFile) + Path.DirectorySeparatorChar + "frmAdditional.xml");
            //IEnumerable<XElement> frms = xdocaddfrm.Element("MT").Elements("AdditionalForm");
            //foreach (XElement frm in frms)
            //{
            //   if (frm.Attribute("name").Value != "Shpt")
            //      continue;

            //   ShptForm.Text = frm.Attribute("caption").Value;
            //   ShptForm.Width = Convert.ToInt32(frm.Attribute("Width").Value);
            //   ShptForm.Height = Convert.ToInt32(frm.Attribute("Height").Value);
            //   ShptForm.Left = Convert.ToInt32(frm.Attribute("Left").Value);
            //   ShptForm.Top = Convert.ToInt32(frm.Attribute("Top").Value);

            //   // создаем теги
            //   CheckBoxVar chBV;
            //   ctlLabelTextbox usTB;
            //   ComboBoxVar cBV;
            //   FormulaEval ev = null;
            //   FlowLayoutPanel flp = new FlowLayoutPanel();
            //   StringBuilder strformula = new StringBuilder();

            //   flp.Dock = DockStyle.Fill;
            //   flp.Parent = ShptForm;
            //   flp.FlowDirection = FlowDirection.TopDown;
            //   flp.AutoScroll = true;

            //   tbUonBus = new TextBox();
            //   Label lbl = new Label();
            //   lbl.AutoSize = true;
            //   lbl.Text = "Напряжение на шине (B)";
            //   lbl.Parent = flp;
            //   tbUonBus.Parent = flp;
            //   lbl.Left = tbUonBus.Left + tbUonBus.Width + 5;
            //   lbl.Top = tbUonBus.Top;

            //   IEnumerable<XElement> xetags = frm.Element("Formframe").Elements("formula");
            //   foreach (XElement xetag in xetags)
            //   {
            //      switch (xetag.Attribute("TypeOfTag").Value)
            //      {
            //         case "Discret":
            //            //string[] strelems = (xetag.Element( "formula" ).Element("value").Value.Split(new char[]{'.'}));

            //            strformula.Length = 0;
            //            strformula.Append("0(" + xetag.Element("value").Attribute("tag").Value + ")");

            //            ev = new FormulaEval(parent.KB, strformula.ToString(), "0",
            //                                 xetag.Attribute("Caption").Value,
            //                                 xetag.Attribute("Dim").Value,
            //                                 TypeOfTag.Discret,
            //                                 xetag.Attribute("TypeOfPanel").Value);

            //            chBV = new CheckBoxVar();
            //            chBV.Parent = flp;
            //            chBV.AutoSize = true;
            //            chBV.addrLinkVar = ev.addrVar;
            //            chBV.addrLinkVarBitMask = ev.addrVarBitMask;
            //            ev.OnChangeValForm += chBV.LinkSetText;
            //            ev.FirstValue();

            //            if (HMI_Settings.ClientDFE != null)
            //                HMI_Settings.ClientDFE.AddArrTags(this.Text, ev);
            //            break;
            //         case "Analog":
            //            strformula.Length = 0;
            //            strformula.Append("0(" + xetag.Element("value").Attribute("tag").Value + ")");

            //            ev = new FormulaEval(parent.KB, strformula.ToString(), "0",
            //                                 xetag.Attribute("Caption").Value,
            //                                 xetag.Attribute("Dim").Value,
            //                                 TypeOfTag.Discret,
            //                                 xetag.Attribute("TypeOfPanel").Value);

            //            ev.StrFormat = HMI_Settings.Precision;
            //            ev.OnChangeValForm += LinkSetText_U_On_Bus;
            //            ev.FirstValue();
            //            break;
            //         default:
            //            MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            break;
            //      }
            //      if (HMI_Settings.ClientDFE != null)
            //          HMI_Settings.ClientDFE.AddArrTags(this.Text, ev);
            //   }
            //   //usTB = new ctlLabelTextbox();
            //   //usTB..SetText_lblDim();
            //   //usTB.lblCaption.Text = "Напряжение на шине";
            //   //usTB.Parent = flp;
            //   //usTB.AutoSize = true;
            //   ////usTB.addrLinkVar = ev.addrVar;
            //   //usTB.typetag = "TUIntVariable";
            //   //usTB.mask = ev.bitmask;
            //   //usTB.txtLabelText.ReadOnly = false;
            //   flp.MouseDoubleClick += new MouseEventHandler(ShptForm_MouseDoubleClick);
            //}

            //ShptForm.Show();
        }

        void ShptForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            SaveShptPosition();
        }

        void ShptForm_MouseDoubleClick( object sender, MouseEventArgs e )
        {
            DoReNew();
        }

        /// <summary>
        /// сохранить позицию окна ШПТ
        /// </summary>
        private void SaveShptPosition()
        {
            //if (xdocaddfrm == null)
            //   return;

            //IEnumerable<XElement> frms = xdocaddfrm.Element("MT").Elements("AdditionalForm");
            //foreach (XElement frm in frms)
            //{
            //   if (frm.Attribute("name").Value != "Shpt")
            //      continue;

            //   frm.Attribute("Left").Value = ShptForm.Left.ToString();
            //   frm.Attribute("Top").Value = ShptForm.Top.ToString();
            //}
            //xdocaddfrm.Save(Path.GetDirectoryName(HMI_Settings.PathToPrjFile) + Path.DirectorySeparatorChar + "frmAdditional.xml");
        }

        private void LinkSetText_U_On_Bus( object Value, string format )
        {
            DoReNew();
        }

        private void DoReNew()
        {
            string tag20 = ExtractVarAsString( 4, 5, 3, 20 );
            string tag31 = ExtractVarAsString( 4, 5, 3, 31 );

            if ( tag20 == String.Empty || tag31 == String.Empty )
                return;

            float itag20 = Convert.ToSingle( tag20 );
            float itag31 = Convert.ToSingle( tag31 );

            float rez = 0;

            if ( itag31 != 0 )
                rez = ( ( 2 * itag20 - itag31 ) / itag31 ) * 400;

            tbUonBus.Text = rez.ToString();
        }

        private string ExtractVarAsString( int src, int dv, int igr, int adrindev )
        {
            //foreach (DataSource ds in parent.KB)
            //   if (ds.NumFC == src)
            //      foreach (TCRZADirectDevice dev in ds)
            //         if (dev.NumDev == dv)
            //            foreach (TCRZAGroup gr in dev.Groups)
            //               if (gr.Id == igr)
            //                  foreach (TCRZAVariable var in gr.Variables)
            //                     if (var.RegInDev == adrindev)
            //                        return var.ExtractTagValueAsString();
            return String.Empty;
        }

        #endregion

        public void ResetProtocol()
        {
            CalculationRegion.ResetStatusProtocol( panelRegions );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param Name="gbs"></param>
        /// <param Name="key">номер ячейки, по нему ищем описание устройства в PrgDevCFG.cdp</param>
        /// <returns></returns>
        private bool FindDevForGB( FlowLayoutPanel flpgbs, GroupBox gbs, string key )
        {
            try
            {
                // извлекаем описание блока
                if ( !sldev.ContainsKey( Convert.ToUInt32( key ) ) )
                    return false;

                IDevice dev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device( 0, uint.Parse( key ) );

                if ( dev == null )
                {
                    //throw new Exception(string.Format("(196) : SpeedAccess.cs : FindDevForGB() : Устройство не найдено.DevGuid = = {0}", key.ToString()));
                }

                XElement xed = sldev[Convert.ToUInt32( key )];
                // определим тип контекстного меню
                //string contextmenutype = xed.Element( "DescDev" ).Element( "TypeContextMenu" ).Attribute( "name" ).Value;

                // генерим панель
                Panel pnl = new Panel();
                pnl.Height = flpgbs.Height / 2;
                pnl.Width = flpgbs.Width;//-5;
                //pnl.Dock = DockStyle.Top;
                //pnl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                pnl.BorderStyle = BorderStyle.Fixed3D;
                pnl.BackColor = Color.Black;
                flpgbs.Controls.Add( pnl );



                // создаем панель для размещения изображения блока
                Panel pnlblock = new Panel();
                pnlblock.Size = new Size( pnl.Height, pnl.Height );
                pnl.Controls.Add( pnlblock );
                Panel pnlblin = new Panel();
                pnlblin.Size = new Size( pnlblock.Height - 10, pnlblock.Height - 10 );
                pnlblin.Location = new Point( 5, 3 );
                pnlblock.Controls.Add( pnlblin );


                #region Генерим TextBox и панель для него
                Panel pnl4TB = new Panel();
                pnl4TB.Location = new Point( pnl.Height, pnlblock.Top );
                pnl4TB.Size = new Size( pnl.Width - pnlblock.Width, pnl.Height );
                pnl4TB.Parent = pnl;

                TextBoxEx newTB = new TextBoxEx();// TextBox();
                newTB.Multiline = true;
                newTB.BackColor = Color.White;
                newTB.TextAlign = HorizontalAlignment.Left;
                newTB.Dock = DockStyle.Fill;
                newTB.ReadOnly = true;
                newTB.Parent = pnl4TB;
                newTB.KeyPress += new KeyPressEventHandler( newTB_KeyPress );

                string[] st = { "" };

                st[0] = CommonUtils.CommonUtils.GetDispNameForDevice( int.Parse( key ) );// aDev.NumFC * 256 + aDev.NumDev
                newTB.Font = new Font( FontFamily.GenericSansSerif, 10.0F, FontStyle.Underline );
                newTB.Lines = st;
                #endregion

                IBasePanel iBPanel;

                if ( !CommonUtils.CommonUtils.CreateDevImg4Panel( out iBPanel, ref taglist, uint.Parse( key ), pnlblin ) )
                    return false;

                panelRegions.Add( iBPanel.Core );
                iBPanel.PanelClick += IbPanelPanelClick;
                CommonUtils.CommonUtils.CreateContextMenu( iBPanel.Core, xed, this );
                aCS.Add( iBPanel );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                return false;
            }
            return true;
        }
        /// <summary>
        /// обработчик клика на обычных блоках БМРЗ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IbPanelPanelClick( object sender, EventArgs e )
        {
            try
            {
                var idp = sender as IDynamicParameters;

                if ( idp != null && idp.Parameters != null )
                    DevicesLibrary.DeviceFormFactory.CreateForm( this, idp.Parameters.DsGuid, idp.Parameters.DeviceGuid, parent.arrFrm );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }

        void newTB_KeyPress( object sender, KeyPressEventArgs e )
        {
            ( sender as TextBox ).SelectionStart = ( sender as TextBox ).TextLength;
        }

        /// <summary>
        /// void ремонтноеПоложениеToolStripMenuItem_CheckedChanged( )
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void ремонтноеПоложениеToolStripMenuItem_CheckedChanged( object sender, EventArgs e )
        {
            //ToolStripDropDownItem tsddi = (ToolStripDropDownItem)sender;
            //ContextMenuStrip tsi = (ContextMenuStrip)tsddi.Owner;

            //DinamicControl tt = (DinamicControl)tsi.SourceControl;

            //tt.Invalidate();
        }

        /// <summary>
        /// void SpeedAccess_Activated()
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void SpeedAccess_Activated( object sender, EventArgs e )
        {
        }

        private void SpeedAccess_FormClosing( object sender, FormClosingEventArgs e )
        {
            SaveShptPosition();

            // удаляем ссылки на теги - отписываемся от тегов
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags( taglist );

            // закрываем боковую панель на parent
            parent.scDeviceObjectConfig.Visible = false;
        }

        /// <summary>
        /// смена активной страницы быстрого доступа по инициативе 
        /// левой панели объектов проекта
        /// </summary>
        /// <param Name="tpname"></param>
        public void tvlc_OnChangeTabpage( string tpname )
        {
            for ( int i = 0; i < tcSpeedAccess.TabPages.Count; i++ )
                if ( ( tcSpeedAccess.TabPages[i] as TabPage ).Text == tpname )
                    tcSpeedAccess.SelectedTab = ( tcSpeedAccess.TabPages[i] as TabPage );
        }
    }
}