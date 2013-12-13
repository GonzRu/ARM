namespace HMI_MT
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sbMesIE = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbConnectBD = new System.Windows.Forms.ToolStripStatusLabel();
            this.sbConnectFC = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerDataTimeUpdate = new System.Windows.Forms.Timer(this.components);
            this.текущееВремяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.timerSynhrTime = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.включитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отключитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.квитироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.параметрыНормальногоРежимаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.timerTestFCConnect = new System.Windows.Forms.Timer(this.components);
            this.scDeviceObjectConfig = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tpObjects = new System.Windows.Forms.TabPage();
            this.tvLogicalObjectsConfig = new System.Windows.Forms.TreeView();
            this.tvObjectsConfig = new System.Windows.Forms.TreeView();
            this.tpDevices = new System.Windows.Forms.TabPage();
            this.tvDevConfig = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabForms = new System.Windows.Forms.TabControl();
            this.ribbon1 = new System.Windows.Forms.Ribbon();
            this.ribbonOrbMenuItem1 = new System.Windows.Forms.RibbonOrbMenuItem();
            this.ribbonOrbMenuItem2 = new System.Windows.Forms.RibbonOrbMenuItem();
            this.ribbonOrbMenuItem3 = new System.Windows.Forms.RibbonOrbMenuItem();
            this.ribbonOrbOptionButton1 = new System.Windows.Forms.RibbonOrbOptionButton();
            this.ribbonOrbOptionButton2 = new System.Windows.Forms.RibbonOrbOptionButton();
            this.ribbonOrbRecentItem1 = new System.Windows.Forms.RibbonOrbRecentItem();
            this.ribbonOrbRecentItem2 = new System.Windows.Forms.RibbonOrbRecentItem();
            this.ribbonOrbRecentItem3 = new System.Windows.Forms.RibbonOrbRecentItem();
            this.ribbonOrbRecentItem4 = new System.Windows.Forms.RibbonOrbRecentItem();
            this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
            this.ribbonButton1 = new System.Windows.Forms.RibbonButton();
            this.ribbonButton2 = new System.Windows.Forms.RibbonButton();
            this.ribbonButton3 = new System.Windows.Forms.RibbonButton();
            this.ribbonButton4 = new System.Windows.Forms.RibbonButton();
            this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
            this.ribbonButton5 = new System.Windows.Forms.RibbonButton();
            this.ribbonButton6 = new System.Windows.Forms.RibbonButton();
            this.ribbonButton7 = new System.Windows.Forms.RibbonButton();
            this.ribbonTab2 = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel3 = new System.Windows.Forms.RibbonPanel();
            this.ribbonButton8 = new System.Windows.Forms.RibbonButton();
            this.ribbonPanel4 = new System.Windows.Forms.RibbonPanel();
            this.ribbonButton9 = new System.Windows.Forms.RibbonButton();
            this.ribbonButton10 = new System.Windows.Forms.RibbonButton();
            this.ribbonPanel5 = new System.Windows.Forms.RibbonPanel();
            this.ribbonButton11 = new System.Windows.Forms.RibbonButton();
            this.ribbonButton12 = new System.Windows.Forms.RibbonButton();
            this.ribbonButton13 = new System.Windows.Forms.RibbonButton();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scDeviceObjectConfig)).BeginInit();
            this.scDeviceObjectConfig.Panel1.SuspendLayout();
            this.scDeviceObjectConfig.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tpObjects.SuspendLayout();
            this.tpDevices.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 138);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1061, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "Главное меню";
            // 
            // statusStrip1
            // 
            this.statusStrip1.AllowMerge = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbMesIE,
            this.sbConnectBD,
            this.sbConnectFC});
            this.statusStrip1.Location = new System.Drawing.Point(0, 728);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1061, 24);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // sbMesIE
            // 
            this.sbMesIE.BackColor = System.Drawing.SystemColors.Info;
            this.sbMesIE.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sbMesIE.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.sbMesIE.Name = "sbMesIE";
            this.sbMesIE.Size = new System.Drawing.Size(348, 19);
            this.sbMesIE.Spring = true;
            this.sbMesIE.Text = "ПТК \"Эгида\"";
            // 
            // sbConnectBD
            // 
            this.sbConnectBD.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sbConnectBD.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.sbConnectBD.Name = "sbConnectBD";
            this.sbConnectBD.Size = new System.Drawing.Size(348, 19);
            this.sbConnectBD.Spring = true;
            this.sbConnectBD.Text = "Связь с БД";
            // 
            // sbConnectFC
            // 
            this.sbConnectFC.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.sbConnectFC.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.sbConnectFC.Name = "sbConnectFC";
            this.sbConnectFC.Size = new System.Drawing.Size(348, 19);
            this.sbConnectFC.Spring = true;
            this.sbConnectFC.Text = "Связь с Сервером данных";
            this.sbConnectFC.ToolTipText = "dfsfsfsd";
            // 
            // timerDataTimeUpdate
            // 
            this.timerDataTimeUpdate.Interval = 1000;
            this.timerDataTimeUpdate.Tick += new System.EventHandler(this.timerDataTimeUpdate_Tick);
            // 
            // текущееВремяToolStripMenuItem
            // 
            this.текущееВремяToolStripMenuItem.Name = "текущееВремяToolStripMenuItem";
            this.текущееВремяToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "DinEn.bmp");
            this.imageList1.Images.SetKeyName(1, "DinDis.bmp");
            this.imageList1.Images.SetKeyName(2, "LIGHTOFF.bmp");
            this.imageList1.Images.SetKeyName(3, "LIGHTON.bmp");
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // pageSetupDialog1
            // 
            this.pageSetupDialog1.Document = this.printDocument1;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // printDialog1
            // 
            this.printDialog1.AllowCurrentPage = true;
            this.printDialog1.AllowSelection = true;
            this.printDialog1.AllowSomePages = true;
            this.printDialog1.Document = this.printDocument1;
            // 
            // timerSynhrTime
            // 
            this.timerSynhrTime.Interval = 5000;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.включитьToolStripMenuItem,
            this.отключитьToolStripMenuItem,
            this.квитироватьToolStripMenuItem,
            this.toolStripSeparator3,
            this.параметрыНормальногоРежимаToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(241, 120);
            this.contextMenuStrip1.Text = "Контекстное меню выключателя";
            // 
            // включитьToolStripMenuItem
            // 
            this.включитьToolStripMenuItem.Name = "включитьToolStripMenuItem";
            this.включитьToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.включитьToolStripMenuItem.Text = "Включить";
            // 
            // отключитьToolStripMenuItem
            // 
            this.отключитьToolStripMenuItem.Name = "отключитьToolStripMenuItem";
            this.отключитьToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.отключитьToolStripMenuItem.Text = "Отключить";
            // 
            // квитироватьToolStripMenuItem
            // 
            this.квитироватьToolStripMenuItem.Name = "квитироватьToolStripMenuItem";
            this.квитироватьToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.квитироватьToolStripMenuItem.Text = "Сбросить сигнализацию";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(237, 6);
            this.toolStripSeparator3.Visible = false;
            // 
            // параметрыНормальногоРежимаToolStripMenuItem
            // 
            this.параметрыНормальногоРежимаToolStripMenuItem.Name = "параметрыНормальногоРежимаToolStripMenuItem";
            this.параметрыНормальногоРежимаToolStripMenuItem.Size = new System.Drawing.Size(240, 22);
            this.параметрыНормальногоРежимаToolStripMenuItem.Text = "Параметры текущего режима";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "X:\\Projects\\gazProm\\HMI_MT\\bin\\Debug\\Help.chm";
            // 
            // scDeviceObjectConfig
            // 
            this.scDeviceObjectConfig.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scDeviceObjectConfig.Dock = System.Windows.Forms.DockStyle.Left;
            this.scDeviceObjectConfig.Location = new System.Drawing.Point(0, 162);
            this.scDeviceObjectConfig.Name = "scDeviceObjectConfig";
            // 
            // scDeviceObjectConfig.Panel1
            // 
            this.scDeviceObjectConfig.Panel1.Controls.Add(this.tabControl2);
            this.scDeviceObjectConfig.Panel2Collapsed = true;
            this.scDeviceObjectConfig.Size = new System.Drawing.Size(150, 566);
            this.scDeviceObjectConfig.TabIndex = 11;
            this.scDeviceObjectConfig.Visible = false;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tpObjects);
            this.tabControl2.Controls.Add(this.tpDevices);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(146, 562);
            this.tabControl2.TabIndex = 1;
            // 
            // tpObjects
            // 
            this.tpObjects.Controls.Add(this.tvLogicalObjectsConfig);
            this.tpObjects.Controls.Add(this.tvObjectsConfig);
            this.tpObjects.Location = new System.Drawing.Point(4, 22);
            this.tpObjects.Name = "tpObjects";
            this.tpObjects.Padding = new System.Windows.Forms.Padding(3);
            this.tpObjects.Size = new System.Drawing.Size(138, 536);
            this.tpObjects.TabIndex = 0;
            this.tpObjects.Text = "Объекты";
            this.tpObjects.UseVisualStyleBackColor = true;
            // 
            // tvLogicalObjectsConfig
            // 
            this.tvLogicalObjectsConfig.BackColor = System.Drawing.SystemColors.Control;
            this.tvLogicalObjectsConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvLogicalObjectsConfig.Location = new System.Drawing.Point(3, 3);
            this.tvLogicalObjectsConfig.Name = "tvLogicalObjectsConfig";
            this.tvLogicalObjectsConfig.Size = new System.Drawing.Size(132, 530);
            this.tvLogicalObjectsConfig.TabIndex = 1;
            // 
            // tvObjectsConfig
            // 
            this.tvObjectsConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvObjectsConfig.Location = new System.Drawing.Point(3, 3);
            this.tvObjectsConfig.Name = "tvObjectsConfig";
            this.tvObjectsConfig.Size = new System.Drawing.Size(132, 530);
            this.tvObjectsConfig.TabIndex = 0;
            // 
            // tpDevices
            // 
            this.tpDevices.Controls.Add(this.tvDevConfig);
            this.tpDevices.Location = new System.Drawing.Point(4, 22);
            this.tpDevices.Name = "tpDevices";
            this.tpDevices.Padding = new System.Windows.Forms.Padding(3);
            this.tpDevices.Size = new System.Drawing.Size(138, 70);
            this.tpDevices.TabIndex = 1;
            this.tpDevices.Text = "Устройства";
            this.tpDevices.UseVisualStyleBackColor = true;
            // 
            // tvDevConfig
            // 
            this.tvDevConfig.BackColor = System.Drawing.SystemColors.Control;
            this.tvDevConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDevConfig.Location = new System.Drawing.Point(3, 3);
            this.tvDevConfig.Name = "tvDevConfig";
            this.tvDevConfig.Size = new System.Drawing.Size(132, 530);
            this.tvDevConfig.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(150, 162);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 566);
            this.splitter1.TabIndex = 12;
            this.splitter1.TabStop = false;
            // 
            // tabForms
            // 
            this.tabForms.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabForms.Location = new System.Drawing.Point(153, 162);
            this.tabForms.Name = "tabForms";
            this.tabForms.SelectedIndex = 0;
            this.tabForms.Size = new System.Drawing.Size(908, 27);
            this.tabForms.TabIndex = 13;
            this.tabForms.SelectedIndexChanged += new System.EventHandler(this.tabForms_SelectedIndexChanged);
            // 
            // ribbon1
            // 
            this.ribbon1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ribbon1.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.Minimized = false;
            this.ribbon1.Name = "ribbon1";
            // 
            // 
            // 
            this.ribbon1.OrbDropDown.BorderRoundness = 8;
            this.ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonOrbMenuItem1);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonOrbMenuItem2);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonOrbMenuItem3);
            this.ribbon1.OrbDropDown.Name = "";
            this.ribbon1.OrbDropDown.OptionItems.Add(this.ribbonOrbOptionButton1);
            this.ribbon1.OrbDropDown.OptionItems.Add(this.ribbonOrbOptionButton2);
            this.ribbon1.OrbDropDown.RecentItems.Add(this.ribbonOrbRecentItem1);
            this.ribbon1.OrbDropDown.RecentItems.Add(this.ribbonOrbRecentItem2);
            this.ribbon1.OrbDropDown.RecentItems.Add(this.ribbonOrbRecentItem3);
            this.ribbon1.OrbDropDown.RecentItems.Add(this.ribbonOrbRecentItem4);
            this.ribbon1.OrbDropDown.Size = new System.Drawing.Size(527, 204);
            this.ribbon1.OrbDropDown.TabIndex = 0;
            this.ribbon1.OrbImage = global::HMI_MT.Properties.Resources.Logo_32p_;
            // 
            // 
            // 
            this.ribbon1.QuickAcessToolbar.AltKey = null;
            this.ribbon1.QuickAcessToolbar.Enabled = false;
            this.ribbon1.QuickAcessToolbar.Image = null;
            this.ribbon1.QuickAcessToolbar.Tag = null;
            this.ribbon1.QuickAcessToolbar.Text = null;
            this.ribbon1.QuickAcessToolbar.ToolTip = null;
            this.ribbon1.QuickAcessToolbar.ToolTipImage = null;
            this.ribbon1.QuickAcessToolbar.ToolTipTitle = null;
            this.ribbon1.Size = new System.Drawing.Size(1061, 138);
            this.ribbon1.TabIndex = 15;
            this.ribbon1.Tabs.Add(this.ribbonTab1);
            this.ribbon1.Tabs.Add(this.ribbonTab2);
            this.ribbon1.TabSpacing = 6;
            this.ribbon1.Text = "ribbon1";
            // 
            // ribbonOrbMenuItem1
            // 
            this.ribbonOrbMenuItem1.AltKey = null;
            this.ribbonOrbMenuItem1.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.ribbonOrbMenuItem1.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonOrbMenuItem1.Image = global::HMI_MT.Properties.Resources.SwitchUser_32p_;
            this.ribbonOrbMenuItem1.SmallImage = global::HMI_MT.Properties.Resources.SwitchUser_32p_;
            this.ribbonOrbMenuItem1.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonOrbMenuItem1.Tag = null;
            this.ribbonOrbMenuItem1.Text = "Смена пользователя";
            this.ribbonOrbMenuItem1.ToolTip = null;
            this.ribbonOrbMenuItem1.ToolTipImage = null;
            this.ribbonOrbMenuItem1.ToolTipTitle = null;
            this.ribbonOrbMenuItem1.Click += new System.EventHandler(this.RibbonMenuButtonSwitchUserClick);
            // 
            // ribbonOrbMenuItem2
            // 
            this.ribbonOrbMenuItem2.AltKey = null;
            this.ribbonOrbMenuItem2.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.ribbonOrbMenuItem2.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonOrbMenuItem2.Image = global::HMI_MT.Properties.Resources.Lock_32p_;
            this.ribbonOrbMenuItem2.SmallImage = global::HMI_MT.Properties.Resources.Lock_32p_;
            this.ribbonOrbMenuItem2.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonOrbMenuItem2.Tag = null;
            this.ribbonOrbMenuItem2.Text = "Блокировка системы";
            this.ribbonOrbMenuItem2.ToolTip = null;
            this.ribbonOrbMenuItem2.ToolTipImage = null;
            this.ribbonOrbMenuItem2.ToolTipTitle = null;
            this.ribbonOrbMenuItem2.Click += new System.EventHandler(this.RibbonMenuButtonBlockSystemClick);
            // 
            // ribbonOrbMenuItem3
            // 
            this.ribbonOrbMenuItem3.AltKey = null;
            this.ribbonOrbMenuItem3.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.ribbonOrbMenuItem3.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonOrbMenuItem3.Image = global::HMI_MT.Properties.Resources.UserInformation_32p_;
            this.ribbonOrbMenuItem3.SmallImage = global::HMI_MT.Properties.Resources.UserInformation_32p_;
            this.ribbonOrbMenuItem3.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonOrbMenuItem3.Tag = null;
            this.ribbonOrbMenuItem3.Text = "Информация о пользователе";
            this.ribbonOrbMenuItem3.ToolTip = null;
            this.ribbonOrbMenuItem3.ToolTipImage = null;
            this.ribbonOrbMenuItem3.ToolTipTitle = null;
            this.ribbonOrbMenuItem3.Click += new System.EventHandler(this.RibbonMenuButtonUserInformationClick);
            // 
            // ribbonOrbOptionButton1
            // 
            this.ribbonOrbOptionButton1.AltKey = null;
            this.ribbonOrbOptionButton1.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonOrbOptionButton1.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonOrbOptionButton1.Image = global::HMI_MT.Properties.Resources.Exit_16p_;
            this.ribbonOrbOptionButton1.SmallImage = global::HMI_MT.Properties.Resources.Exit_16p_;
            this.ribbonOrbOptionButton1.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonOrbOptionButton1.Tag = null;
            this.ribbonOrbOptionButton1.Text = "Выход";
            this.ribbonOrbOptionButton1.ToolTip = null;
            this.ribbonOrbOptionButton1.ToolTipImage = null;
            this.ribbonOrbOptionButton1.ToolTipTitle = null;
            this.ribbonOrbOptionButton1.Click += new System.EventHandler(this.RibbonMenuButtonExitClick);
            // 
            // ribbonOrbOptionButton2
            // 
            this.ribbonOrbOptionButton2.AltKey = null;
            this.ribbonOrbOptionButton2.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonOrbOptionButton2.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonOrbOptionButton2.Image = global::HMI_MT.Properties.Resources.About_16p_;
            this.ribbonOrbOptionButton2.SmallImage = global::HMI_MT.Properties.Resources.About_16p_;
            this.ribbonOrbOptionButton2.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonOrbOptionButton2.Tag = null;
            this.ribbonOrbOptionButton2.Text = "О программе";
            this.ribbonOrbOptionButton2.ToolTip = null;
            this.ribbonOrbOptionButton2.ToolTipImage = null;
            this.ribbonOrbOptionButton2.ToolTipTitle = null;
            this.ribbonOrbOptionButton2.Click += new System.EventHandler(this.RibbonMenuButtonAboutClick);
            // 
            // ribbonOrbRecentItem1
            // 
            this.ribbonOrbRecentItem1.AltKey = null;
            this.ribbonOrbRecentItem1.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonOrbRecentItem1.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonOrbRecentItem1.Image = ((System.Drawing.Image)(resources.GetObject("ribbonOrbRecentItem1.Image")));
            this.ribbonOrbRecentItem1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbRecentItem1.SmallImage")));
            this.ribbonOrbRecentItem1.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonOrbRecentItem1.Tag = null;
            this.ribbonOrbRecentItem1.Text = "Параметры страницы";
            this.ribbonOrbRecentItem1.ToolTip = null;
            this.ribbonOrbRecentItem1.ToolTipImage = null;
            this.ribbonOrbRecentItem1.ToolTipTitle = null;
            this.ribbonOrbRecentItem1.Click += new System.EventHandler(this.RibbonMenuButtonPageSetupClick);
            // 
            // ribbonOrbRecentItem2
            // 
            this.ribbonOrbRecentItem2.AltKey = null;
            this.ribbonOrbRecentItem2.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonOrbRecentItem2.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonOrbRecentItem2.Image = ((System.Drawing.Image)(resources.GetObject("ribbonOrbRecentItem2.Image")));
            this.ribbonOrbRecentItem2.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbRecentItem2.SmallImage")));
            this.ribbonOrbRecentItem2.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonOrbRecentItem2.Tag = null;
            this.ribbonOrbRecentItem2.Text = "Предварительный просмотр";
            this.ribbonOrbRecentItem2.ToolTip = null;
            this.ribbonOrbRecentItem2.ToolTipImage = null;
            this.ribbonOrbRecentItem2.ToolTipTitle = null;
            this.ribbonOrbRecentItem2.Click += new System.EventHandler(this.RibbonMenuButtonPreviewPageClick);
            // 
            // ribbonOrbRecentItem3
            // 
            this.ribbonOrbRecentItem3.AltKey = null;
            this.ribbonOrbRecentItem3.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonOrbRecentItem3.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonOrbRecentItem3.Image = ((System.Drawing.Image)(resources.GetObject("ribbonOrbRecentItem3.Image")));
            this.ribbonOrbRecentItem3.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbRecentItem3.SmallImage")));
            this.ribbonOrbRecentItem3.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonOrbRecentItem3.Tag = null;
            this.ribbonOrbRecentItem3.Text = "Шрифт печати";
            this.ribbonOrbRecentItem3.ToolTip = null;
            this.ribbonOrbRecentItem3.ToolTipImage = null;
            this.ribbonOrbRecentItem3.ToolTipTitle = null;
            this.ribbonOrbRecentItem3.Click += new System.EventHandler(this.RibbonMenuButtonPageFontClick);
            // 
            // ribbonOrbRecentItem4
            // 
            this.ribbonOrbRecentItem4.AltKey = null;
            this.ribbonOrbRecentItem4.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonOrbRecentItem4.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonOrbRecentItem4.Image = ((System.Drawing.Image)(resources.GetObject("ribbonOrbRecentItem4.Image")));
            this.ribbonOrbRecentItem4.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbRecentItem4.SmallImage")));
            this.ribbonOrbRecentItem4.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonOrbRecentItem4.Tag = null;
            this.ribbonOrbRecentItem4.Text = "Печать";
            this.ribbonOrbRecentItem4.ToolTip = null;
            this.ribbonOrbRecentItem4.ToolTipImage = null;
            this.ribbonOrbRecentItem4.ToolTipTitle = null;
            this.ribbonOrbRecentItem4.Click += new System.EventHandler(this.RibbonMenuButtonPrintClick);
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Panels.Add(this.ribbonPanel1);
            this.ribbonTab1.Panels.Add(this.ribbonPanel2);
            this.ribbonTab1.Tag = null;
            this.ribbonTab1.Text = "Навигация";
            // 
            // ribbonPanel1
            // 
            this.ribbonPanel1.Items.Add(this.ribbonButton1);
            this.ribbonPanel1.Items.Add(this.ribbonButton2);
            this.ribbonPanel1.Tag = null;
            this.ribbonPanel1.Text = "Главное";
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.AltKey = null;
            this.ribbonButton1.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton1.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton1.Image = global::HMI_MT.Properties.Resources.Schema_32p_;
            this.ribbonButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.SmallImage")));
            this.ribbonButton1.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton1.Tag = null;
            this.ribbonButton1.Text = "Мнемосхема";
            this.ribbonButton1.ToolTip = null;
            this.ribbonButton1.ToolTipImage = null;
            this.ribbonButton1.ToolTipTitle = null;
            this.ribbonButton1.Click += new System.EventHandler(this.КibbonButtonSchemaClick);
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.AltKey = null;
            this.ribbonButton2.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton2.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton2.DropDownItems.Add(this.ribbonButton3);
            this.ribbonButton2.DropDownItems.Add(this.ribbonButton4);
            this.ribbonButton2.Image = global::HMI_MT.Properties.Resources.FastAccess_32p_;
            this.ribbonButton2.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.SmallImage")));
            this.ribbonButton2.Style = System.Windows.Forms.RibbonButtonStyle.SplitDropDown;
            this.ribbonButton2.Tag = null;
            this.ribbonButton2.Text = "Быстрый доступ";
            this.ribbonButton2.ToolTip = null;
            this.ribbonButton2.ToolTipImage = null;
            this.ribbonButton2.ToolTipTitle = null;
            this.ribbonButton2.Click += new System.EventHandler(this.RibbonButtonFastAccessClick);
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.AltKey = null;
            this.ribbonButton3.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.ribbonButton3.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton3.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.Image")));
            this.ribbonButton3.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.SmallImage")));
            this.ribbonButton3.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton3.Tag = null;
            this.ribbonButton3.Text = "Открыть дерево";
            this.ribbonButton3.ToolTip = null;
            this.ribbonButton3.ToolTipImage = null;
            this.ribbonButton3.ToolTipTitle = null;
            this.ribbonButton3.Click += new System.EventHandler(this.RibbonButtonOpenTreeClick);
            // 
            // ribbonButton4
            // 
            this.ribbonButton4.AltKey = null;
            this.ribbonButton4.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.ribbonButton4.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton4.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton4.Image")));
            this.ribbonButton4.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton4.SmallImage")));
            this.ribbonButton4.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton4.Tag = null;
            this.ribbonButton4.Text = "Закрыть дерево";
            this.ribbonButton4.ToolTip = null;
            this.ribbonButton4.ToolTipImage = null;
            this.ribbonButton4.ToolTipTitle = null;
            this.ribbonButton4.Click += new System.EventHandler(this.RibbonButtonCloseTreeClick);
            // 
            // ribbonPanel2
            // 
            this.ribbonPanel2.Items.Add(this.ribbonButton5);
            this.ribbonPanel2.Items.Add(this.ribbonButton6);
            this.ribbonPanel2.Items.Add(this.ribbonButton7);
            this.ribbonPanel2.Tag = null;
            this.ribbonPanel2.Text = "Дополнительно";
            // 
            // ribbonButton5
            // 
            this.ribbonButton5.AltKey = null;
            this.ribbonButton5.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton5.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton5.Image = global::HMI_MT.Properties.Resources.DiagPanel_32p_;
            this.ribbonButton5.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton5.SmallImage")));
            this.ribbonButton5.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton5.Tag = null;
            this.ribbonButton5.Text = "Панель диагностики";
            this.ribbonButton5.ToolTip = null;
            this.ribbonButton5.ToolTipImage = null;
            this.ribbonButton5.ToolTipTitle = null;
            this.ribbonButton5.Click += new System.EventHandler(this.RibbonButtonDiagnosticPanelClick);
            // 
            // ribbonButton6
            // 
            this.ribbonButton6.AltKey = null;
            this.ribbonButton6.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton6.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton6.Image = global::HMI_MT.Properties.Resources.CurrentMode_32p_;
            this.ribbonButton6.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton6.SmallImage")));
            this.ribbonButton6.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton6.Tag = null;
            this.ribbonButton6.Text = "Текущий режим";
            this.ribbonButton6.ToolTip = null;
            this.ribbonButton6.ToolTipImage = null;
            this.ribbonButton6.ToolTipTitle = null;
            this.ribbonButton6.Click += new System.EventHandler(this.RibbonButtonCurrentModeClick);
            // 
            // ribbonButton7
            // 
            this.ribbonButton7.AltKey = null;
            this.ribbonButton7.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton7.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton7.Image = global::HMI_MT.Properties.Resources.Clock_32p_;
            this.ribbonButton7.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton7.SmallImage")));
            this.ribbonButton7.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton7.Tag = null;
            this.ribbonButton7.Text = "Часы";
            this.ribbonButton7.ToolTip = null;
            this.ribbonButton7.ToolTipImage = null;
            this.ribbonButton7.ToolTipTitle = null;
            this.ribbonButton7.Click += new System.EventHandler(this.RibbonButtonClockClick);
            // 
            // ribbonTab2
            // 
            this.ribbonTab2.Panels.Add(this.ribbonPanel3);
            this.ribbonTab2.Panels.Add(this.ribbonPanel4);
            this.ribbonTab2.Panels.Add(this.ribbonPanel5);
            this.ribbonTab2.Tag = null;
            this.ribbonTab2.Text = "Инструменты";
            // 
            // ribbonPanel3
            // 
            this.ribbonPanel3.Items.Add(this.ribbonButton8);
            this.ribbonPanel3.Tag = null;
            this.ribbonPanel3.Text = "Основные";
            // 
            // ribbonButton8
            // 
            this.ribbonButton8.AltKey = null;
            this.ribbonButton8.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton8.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton8.Image = global::HMI_MT.Properties.Resources.Journals_32p_;
            this.ribbonButton8.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton8.SmallImage")));
            this.ribbonButton8.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton8.Tag = null;
            this.ribbonButton8.Text = "Журнал";
            this.ribbonButton8.ToolTip = null;
            this.ribbonButton8.ToolTipImage = null;
            this.ribbonButton8.ToolTipTitle = null;
            this.ribbonButton8.Click += new System.EventHandler(this.RibbonButtonJournalsClick);
            // 
            // ribbonPanel4
            // 
            this.ribbonPanel4.Items.Add(this.ribbonButton9);
            this.ribbonPanel4.Items.Add(this.ribbonButton10);
            this.ribbonPanel4.Tag = null;
            this.ribbonPanel4.Text = "Администрирование";
            // 
            // ribbonButton9
            // 
            this.ribbonButton9.AltKey = null;
            this.ribbonButton9.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton9.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton9.Image = global::HMI_MT.Properties.Resources.UseAccess_32p_;
            this.ribbonButton9.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton9.SmallImage")));
            this.ribbonButton9.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton9.Tag = null;
            this.ribbonButton9.Text = "Доступ";
            this.ribbonButton9.ToolTip = null;
            this.ribbonButton9.ToolTipImage = null;
            this.ribbonButton9.ToolTipTitle = null;
            this.ribbonButton9.Click += new System.EventHandler(this.RibbonButtonUseAccessClick);
            // 
            // ribbonButton10
            // 
            this.ribbonButton10.AltKey = null;
            this.ribbonButton10.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton10.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton10.Image = global::HMI_MT.Properties.Resources.Reconnect_32p_;
            this.ribbonButton10.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton10.SmallImage")));
            this.ribbonButton10.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton10.Tag = null;
            this.ribbonButton10.Text = "Востан. связи";
            this.ribbonButton10.ToolTip = null;
            this.ribbonButton10.ToolTipImage = null;
            this.ribbonButton10.ToolTipTitle = null;
            this.ribbonButton10.Click += new System.EventHandler(this.RibbonButtonReconnectClick);
            // 
            // ribbonPanel5
            // 
            this.ribbonPanel5.Items.Add(this.ribbonButton11);
            this.ribbonPanel5.Items.Add(this.ribbonButton12);
            this.ribbonPanel5.Items.Add(this.ribbonButton13);
            this.ribbonPanel5.Tag = null;
            this.ribbonPanel5.Text = "Сервис";
            // 
            // ribbonButton11
            // 
            this.ribbonButton11.AltKey = null;
            this.ribbonButton11.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton11.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton11.Image = global::HMI_MT.Properties.Resources.Clock_32p_;
            this.ribbonButton11.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton11.SmallImage")));
            this.ribbonButton11.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton11.Tag = null;
            this.ribbonButton11.Text = "Установка времени ПТК";
            this.ribbonButton11.ToolTip = null;
            this.ribbonButton11.ToolTipImage = null;
            this.ribbonButton11.ToolTipTitle = null;
            this.ribbonButton11.Click += new System.EventHandler(this.RibbonButtonSetPTKClockClick);
            // 
            // ribbonButton12
            // 
            this.ribbonButton12.AltKey = null;
            this.ribbonButton12.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton12.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton12.Image = global::HMI_MT.Properties.Resources.ResetAllCmd_32p_;
            this.ribbonButton12.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton12.SmallImage")));
            this.ribbonButton12.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton12.Tag = null;
            this.ribbonButton12.Text = "Сброс комманд";
            this.ribbonButton12.ToolTip = null;
            this.ribbonButton12.ToolTipImage = null;
            this.ribbonButton12.ToolTipTitle = null;
            this.ribbonButton12.Click += new System.EventHandler(this.RibbonButtonResetAllCommandsClick);
            // 
            // ribbonButton13
            // 
            this.ribbonButton13.AltKey = null;
            this.ribbonButton13.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.ribbonButton13.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.ribbonButton13.Image = global::HMI_MT.Properties.Resources.ViewCmds_32p_;
            this.ribbonButton13.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton13.SmallImage")));
            this.ribbonButton13.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.ribbonButton13.Tag = null;
            this.ribbonButton13.Text = "Запросы к серверу";
            this.ribbonButton13.ToolTip = null;
            this.ribbonButton13.ToolTipImage = null;
            this.ribbonButton13.ToolTipTitle = null;
            this.ribbonButton13.Click += new System.EventHandler(this.RibbonButtonViewCommandsClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 752);
            this.Controls.Add(this.tabForms);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.scDeviceObjectConfig);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.ribbon1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.scDeviceObjectConfig.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scDeviceObjectConfig)).EndInit();
            this.scDeviceObjectConfig.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tpObjects.ResumeLayout(false);
            this.tpDevices.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel sbMesIE;
        private System.Windows.Forms.ToolStripStatusLabel sbConnectBD;
        private System.Windows.Forms.ToolStripStatusLabel sbConnectFC;
        private System.Windows.Forms.ToolStripMenuItem miToolStrip_currentData;
        private System.Windows.Forms.ToolStripMenuItem текущееВремяToolStripMenuItem;
        private System.Windows.Forms.Timer timerDataTimeUpdate;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timerSynhrTime;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem включитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отключитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem квитироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem параметрыНормальногоРежимаToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.Timer timerTestFCConnect;
        public System.Windows.Forms.SplitContainer scDeviceObjectConfig;
        private System.Windows.Forms.Splitter splitter1;
        public System.Windows.Forms.TabControl tabForms;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tpObjects;
        private System.Windows.Forms.TreeView tvLogicalObjectsConfig;
        private System.Windows.Forms.TreeView tvObjectsConfig;
        private System.Windows.Forms.TabPage tpDevices;
        private System.Windows.Forms.TreeView tvDevConfig;
        private System.Windows.Forms.Ribbon ribbon1;
        private System.Windows.Forms.RibbonTab ribbonTab1;
        private System.Windows.Forms.RibbonPanel ribbonPanel1;
        private System.Windows.Forms.RibbonButton ribbonButton1;
        private System.Windows.Forms.RibbonButton ribbonButton2;
        private System.Windows.Forms.RibbonButton ribbonButton3;
        private System.Windows.Forms.RibbonButton ribbonButton4;
        private System.Windows.Forms.RibbonPanel ribbonPanel2;
        private System.Windows.Forms.RibbonButton ribbonButton5;
        private System.Windows.Forms.RibbonButton ribbonButton6;
        private System.Windows.Forms.RibbonButton ribbonButton7;
        private System.Windows.Forms.RibbonTab ribbonTab2;
        private System.Windows.Forms.RibbonPanel ribbonPanel3;
        private System.Windows.Forms.RibbonPanel ribbonPanel4;
        private System.Windows.Forms.RibbonPanel ribbonPanel5;
        private System.Windows.Forms.RibbonButton ribbonButton8;
        private System.Windows.Forms.RibbonButton ribbonButton9;
        private System.Windows.Forms.RibbonButton ribbonButton10;
        private System.Windows.Forms.RibbonButton ribbonButton11;
        private System.Windows.Forms.RibbonButton ribbonButton12;
        private System.Windows.Forms.RibbonButton ribbonButton13;
        private System.Windows.Forms.RibbonOrbMenuItem ribbonOrbMenuItem1;
        private System.Windows.Forms.RibbonOrbMenuItem ribbonOrbMenuItem2;
        private System.Windows.Forms.RibbonOrbMenuItem ribbonOrbMenuItem3;
        private System.Windows.Forms.RibbonOrbOptionButton ribbonOrbOptionButton1;
        private System.Windows.Forms.RibbonOrbOptionButton ribbonOrbOptionButton2;
        private System.Windows.Forms.RibbonOrbRecentItem ribbonOrbRecentItem1;
        private System.Windows.Forms.RibbonOrbRecentItem ribbonOrbRecentItem2;
        private System.Windows.Forms.RibbonOrbRecentItem ribbonOrbRecentItem3;
        private System.Windows.Forms.RibbonOrbRecentItem ribbonOrbRecentItem4;
    }
}