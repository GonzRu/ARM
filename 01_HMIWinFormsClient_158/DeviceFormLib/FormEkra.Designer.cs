namespace DeviceFormLib
{
    partial class FormEkra
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpCurrentInfo = new System.Windows.Forms.TabPage();
            this.pnlCurValue = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel1 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel2 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel6 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer10 = new System.Windows.Forms.SplitContainer();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel4 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel5 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.mtraNamedFLPanel3 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel8 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.tabPageCurStateInfoDev = new System.Windows.Forms.TabPage();
            this.pnlCurState = new System.Windows.Forms.Panel();
            this.mtraNamedFLPanel7 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.tpConfig = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.lstvConfig = new System.Windows.Forms.ListView();
            this.clmnHDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmUstComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlTPConfig = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.readWriteUstavky1 = new HelperControlsLibrary.ReadWriteUstavkyControl();
            this.selectUserControl2 = new HelperControlsLibrary.SelectControl();
            this.tabControl1.SuspendLayout();
            this.tpCurrentInfo.SuspendLayout();
            this.pnlCurValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).BeginInit();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.Panel2.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer10)).BeginInit();
            this.splitContainer10.Panel1.SuspendLayout();
            this.splitContainer10.Panel2.SuspendLayout();
            this.splitContainer10.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageCurStateInfoDev.SuspendLayout();
            this.pnlCurState.SuspendLayout();
            this.tpConfig.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpCurrentInfo);
            this.tabControl1.Controls.Add(this.tabPageCurStateInfoDev);
            this.tabControl1.Controls.Add(this.tpConfig);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1174, 741);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabControlSelected);
            // 
            // tpCurrentInfo
            // 
            this.tpCurrentInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tpCurrentInfo.Controls.Add(this.pnlCurValue);
            this.tpCurrentInfo.Location = new System.Drawing.Point(4, 22);
            this.tpCurrentInfo.Name = "tpCurrentInfo";
            this.tpCurrentInfo.Size = new System.Drawing.Size(1166, 715);
            this.tpCurrentInfo.TabIndex = 9;
            this.tpCurrentInfo.Text = "Текущая_информация";
            // 
            // pnlCurValue
            // 
            this.pnlCurValue.BackColor = System.Drawing.SystemColors.Control;
            this.pnlCurValue.Controls.Add(this.splitContainer1);
            this.pnlCurValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCurValue.Location = new System.Drawing.Point(0, 0);
            this.pnlCurValue.Name = "pnlCurValue";
            this.pnlCurValue.Size = new System.Drawing.Size(1166, 715);
            this.pnlCurValue.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(1166, 715);
            this.splitContainer1.SplitterDistance = 609;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1166, 715);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.splitContainer6);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1158, 689);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Аналоговые сигналы";
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(3, 3);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer6.Size = new System.Drawing.Size(1152, 683);
            this.splitContainer6.SplitterDistance = 207;
            this.splitContainer6.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.mtraNamedFLPanel1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(207, 683);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Текущие значения аналоговых входов:";
            // 
            // mtraNamedFLPanel1
            // 
            this.mtraNamedFLPanel1.AutoScroll = true;
            this.mtraNamedFLPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel1.Caption = "flpCurAnalInputs_Primary";
            this.mtraNamedFLPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel1.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel1.Name = "mtraNamedFLPanel1";
            this.mtraNamedFLPanel1.Size = new System.Drawing.Size(201, 664);
            this.mtraNamedFLPanel1.TabIndex = 0;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.groupBox4);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.splitContainer9);
            this.splitContainer7.Size = new System.Drawing.Size(941, 683);
            this.splitContainer7.SplitterDistance = 222;
            this.splitContainer7.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.mtraNamedFLPanel2);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(222, 683);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Текущие значения аналоговых величин:";
            // 
            // mtraNamedFLPanel2
            // 
            this.mtraNamedFLPanel2.AutoScroll = true;
            this.mtraNamedFLPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel2.Caption = "flpCurAnalValues_Primary";
            this.mtraNamedFLPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel2.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel2.Name = "mtraNamedFLPanel2";
            this.mtraNamedFLPanel2.Size = new System.Drawing.Size(216, 664);
            this.mtraNamedFLPanel2.TabIndex = 0;
            // 
            // splitContainer9
            // 
            this.splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer9.Location = new System.Drawing.Point(0, 0);
            this.splitContainer9.Name = "splitContainer9";
            // 
            // splitContainer9.Panel1
            // 
            this.splitContainer9.Panel1.Controls.Add(this.groupBox5);
            this.splitContainer9.Panel1Collapsed = true;
            // 
            // splitContainer9.Panel2
            // 
            this.splitContainer9.Panel2.Controls.Add(this.splitContainer10);
            this.splitContainer9.Size = new System.Drawing.Size(715, 683);
            this.splitContainer9.SplitterDistance = 250;
            this.splitContainer9.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.mtraNamedFLPanel6);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(250, 100);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Первичные/вторичные номинал. знач. аналог. датчиков";
            // 
            // mtraNamedFLPanel6
            // 
            this.mtraNamedFLPanel6.AutoScroll = true;
            this.mtraNamedFLPanel6.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel6.Caption = "flpDatchics";
            this.mtraNamedFLPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel6.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel6.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel6.Name = "mtraNamedFLPanel6";
            this.mtraNamedFLPanel6.Size = new System.Drawing.Size(244, 81);
            this.mtraNamedFLPanel6.TabIndex = 0;
            // 
            // splitContainer10
            // 
            this.splitContainer10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer10.Location = new System.Drawing.Point(0, 0);
            this.splitContainer10.Name = "splitContainer10";
            // 
            // splitContainer10.Panel1
            // 
            this.splitContainer10.Panel1.Controls.Add(this.groupBox10);
            // 
            // splitContainer10.Panel2
            // 
            this.splitContainer10.Panel2.Controls.Add(this.groupBox11);
            this.splitContainer10.Size = new System.Drawing.Size(715, 683);
            this.splitContainer10.SplitterDistance = 348;
            this.splitContainer10.TabIndex = 0;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.mtraNamedFLPanel4);
            this.groupBox10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox10.Location = new System.Drawing.Point(0, 0);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(348, 683);
            this.groupBox10.TabIndex = 1;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Дискретные входы:";
            // 
            // mtraNamedFLPanel4
            // 
            this.mtraNamedFLPanel4.AutoScroll = true;
            this.mtraNamedFLPanel4.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel4.Caption = "flpDiscretIn";
            this.mtraNamedFLPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel4.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel4.Name = "mtraNamedFLPanel4";
            this.mtraNamedFLPanel4.Size = new System.Drawing.Size(342, 664);
            this.mtraNamedFLPanel4.TabIndex = 0;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.mtraNamedFLPanel5);
            this.groupBox11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox11.Location = new System.Drawing.Point(0, 0);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(363, 683);
            this.groupBox11.TabIndex = 1;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Дискретные выходы:";
            // 
            // mtraNamedFLPanel5
            // 
            this.mtraNamedFLPanel5.AutoScroll = true;
            this.mtraNamedFLPanel5.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel5.Caption = "flpDiscretOut";
            this.mtraNamedFLPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel5.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel5.Name = "mtraNamedFLPanel5";
            this.mtraNamedFLPanel5.Size = new System.Drawing.Size(357, 664);
            this.mtraNamedFLPanel5.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.mtraNamedFLPanel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1158, 689);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Дискретные сигналы";
            // 
            // mtraNamedFLPanel3
            // 
            this.mtraNamedFLPanel3.AutoScroll = true;
            this.mtraNamedFLPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel3.Caption = "flpDiscret";
            this.mtraNamedFLPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel3.Location = new System.Drawing.Point(3, 3);
            this.mtraNamedFLPanel3.Name = "mtraNamedFLPanel3";
            this.mtraNamedFLPanel3.Size = new System.Drawing.Size(1152, 683);
            this.mtraNamedFLPanel3.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mtraNamedFLPanel8);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 46);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Статус:";
            // 
            // mtraNamedFLPanel8
            // 
            this.mtraNamedFLPanel8.AutoScroll = true;
            this.mtraNamedFLPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel8.Caption = "Статус";
            this.mtraNamedFLPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel8.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel8.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel8.Name = "mtraNamedFLPanel8";
            this.mtraNamedFLPanel8.Size = new System.Drawing.Size(144, 27);
            this.mtraNamedFLPanel8.TabIndex = 0;
            // 
            // tabPageCurStateInfoDev
            // 
            this.tabPageCurStateInfoDev.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageCurStateInfoDev.Controls.Add(this.pnlCurState);
            this.tabPageCurStateInfoDev.Location = new System.Drawing.Point(4, 22);
            this.tabPageCurStateInfoDev.Name = "tabPageCurStateInfoDev";
            this.tabPageCurStateInfoDev.Size = new System.Drawing.Size(1166, 715);
            this.tabPageCurStateInfoDev.TabIndex = 10;
            this.tabPageCurStateInfoDev.Text = "Текущее состояние и сведения о приборе";
            // 
            // pnlCurState
            // 
            this.pnlCurState.BackColor = System.Drawing.SystemColors.Control;
            this.pnlCurState.Controls.Add(this.mtraNamedFLPanel7);
            this.pnlCurState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCurState.Location = new System.Drawing.Point(0, 0);
            this.pnlCurState.Name = "pnlCurState";
            this.pnlCurState.Size = new System.Drawing.Size(1166, 715);
            this.pnlCurState.TabIndex = 0;
            // 
            // mtraNamedFLPanel7
            // 
            this.mtraNamedFLPanel7.AutoScroll = true;
            this.mtraNamedFLPanel7.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel7.Caption = "PriborInfo";
            this.mtraNamedFLPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel7.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel7.Location = new System.Drawing.Point(0, 0);
            this.mtraNamedFLPanel7.Name = "mtraNamedFLPanel7";
            this.mtraNamedFLPanel7.Size = new System.Drawing.Size(1166, 715);
            this.mtraNamedFLPanel7.TabIndex = 0;
            // 
            // tpConfig
            // 
            this.tpConfig.BackColor = System.Drawing.SystemColors.Control;
            this.tpConfig.Controls.Add(this.tableLayoutPanel1);
            this.tpConfig.Location = new System.Drawing.Point(4, 22);
            this.tpConfig.Name = "tpConfig";
            this.tpConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfig.Size = new System.Drawing.Size(1166, 715);
            this.tpConfig.TabIndex = 12;
            this.tpConfig.Text = "Уставки";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer8, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1160, 709);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // splitContainer8
            // 
            this.splitContainer8.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer8.IsSplitterFixed = true;
            this.splitContainer8.Location = new System.Drawing.Point(3, 3);
            this.splitContainer8.Name = "splitContainer8";
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer8.Panel1.Controls.Add(this.lstvConfig);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.pnlTPConfig);
            this.splitContainer8.Size = new System.Drawing.Size(1154, 628);
            this.splitContainer8.SplitterDistance = 200;
            this.splitContainer8.TabIndex = 1;
            // 
            // lstvConfig
            // 
            this.lstvConfig.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstvConfig.BackColor = System.Drawing.Color.LightCyan;
            this.lstvConfig.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmnHDate,
            this.clmUstComment});
            this.lstvConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvConfig.FullRowSelect = true;
            this.lstvConfig.GridLines = true;
            this.lstvConfig.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvConfig.Location = new System.Drawing.Point(0, 0);
            this.lstvConfig.Name = "lstvConfig";
            this.lstvConfig.Size = new System.Drawing.Size(198, 626);
            this.lstvConfig.TabIndex = 0;
            this.lstvConfig.UseCompatibleStateImageBehavior = false;
            this.lstvConfig.View = System.Windows.Forms.View.Details;
            // 
            // clmnHDate
            // 
            this.clmnHDate.Text = "Дата";
            this.clmnHDate.Width = 195;
            // 
            // clmUstComment
            // 
            this.clmUstComment.Text = "Комментарий";
            this.clmUstComment.Width = 200;
            // 
            // pnlTPConfig
            // 
            this.pnlTPConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTPConfig.Location = new System.Drawing.Point(0, 0);
            this.pnlTPConfig.Name = "pnlTPConfig";
            this.pnlTPConfig.Size = new System.Drawing.Size(948, 626);
            this.pnlTPConfig.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.readWriteUstavky1);
            this.panel2.Controls.Add(this.selectUserControl2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 637);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1154, 69);
            this.panel2.TabIndex = 0;
            // 
            // readWriteUstavky1
            // 
            this.readWriteUstavky1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.readWriteUstavky1.Location = new System.Drawing.Point(702, 3);
            this.readWriteUstavky1.Name = "readWriteUstavky1";
            this.readWriteUstavky1.Size = new System.Drawing.Size(307, 64);
            this.readWriteUstavky1.TabIndex = 2;
            // 
            // selectUserControl2
            // 
            this.selectUserControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectUserControl2.Location = new System.Drawing.Point(5, 3);
            this.selectUserControl2.Name = "selectUserControl2";
            this.selectUserControl2.Size = new System.Drawing.Size(691, 64);
            this.selectUserControl2.TabIndex = 1;
            // 
            // Frm4DeviceEkra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 741);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormEkra";
            this.Text = "frm4DeviceEkra";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBmrzDeviceFormClosing);
            this.Load += new System.EventHandler(this.FormBmrzDeviceLoad);
            this.tabControl1.ResumeLayout(false);
            this.tpCurrentInfo.ResumeLayout(false);
            this.pnlCurValue.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).EndInit();
            this.splitContainer9.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.splitContainer10.Panel1.ResumeLayout(false);
            this.splitContainer10.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer10)).EndInit();
            this.splitContainer10.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabPageCurStateInfoDev.ResumeLayout(false);
            this.pnlCurState.ResumeLayout(false);
            this.tpConfig.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpCurrentInfo;
        private System.Windows.Forms.Panel pnlCurValue;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.SplitContainer splitContainer9;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.SplitContainer splitContainer10;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPageCurStateInfoDev;
        private System.Windows.Forms.Panel pnlCurState;
        private System.Windows.Forms.TabPage tpConfig;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.ListView lstvConfig;
        private System.Windows.Forms.ColumnHeader clmnHDate;
        private System.Windows.Forms.Panel pnlTPConfig;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel1;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel2;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel3;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel4;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel5;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel6;
        private System.Windows.Forms.ColumnHeader clmUstComment;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel7;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel8;
        private System.Windows.Forms.Panel panel2;
        private HelperControlsLibrary.SelectControl selectUserControl2;
        private HelperControlsLibrary.ReadWriteUstavkyControl readWriteUstavky1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    }
}