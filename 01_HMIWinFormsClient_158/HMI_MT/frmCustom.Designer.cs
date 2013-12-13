namespace HMI_MT
{
    partial class frmCustom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose( bool disposing )
        //{
        //    if( disposing && ( components != null ) )
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
           System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode( "Параметры запуска" );
           System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode( "Цвета" );
           System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode( "Шрифт" );
           System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode( "Журнал событий" );
           System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode( "Безопасность" );
           System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode( "Параметры вывода значений" );
           this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog( );
           this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog( );
           this.colorDialog1 = new System.Windows.Forms.ColorDialog( );
           this.splitter1 = new System.Windows.Forms.Splitter( );
           this.splitContainer1 = new System.Windows.Forms.SplitContainer( );
           this.treeView1 = new System.Windows.Forms.TreeView( );
           this.btnCancel = new System.Windows.Forms.Button( );
           this.button1 = new System.Windows.Forms.Button( );
           this.tabControl1 = new System.Windows.Forms.TabControl( );
           this.tabPage1 = new System.Windows.Forms.TabPage( );
           this.pnl1 = new System.Windows.Forms.Panel( );
           this.groupBox4 = new System.Windows.Forms.GroupBox( );
           this.label9 = new System.Windows.Forms.Label( );
           this.tbIPFCForCMD = new System.Windows.Forms.TextBox( );
           this.label27 = new System.Windows.Forms.Label( );
           this.tbPortFCForCMD = new System.Windows.Forms.TextBox( );
           this.label17 = new System.Windows.Forms.Label( );
           this.tbDataReNew = new System.Windows.Forms.TextBox( );
           this.pnlRemExchange = new System.Windows.Forms.Panel( );
           this.chbIsRetransmittingCMD = new System.Windows.Forms.CheckBox( );
           this.groupBox2 = new System.Windows.Forms.GroupBox( );
           this.label1 = new System.Windows.Forms.Label( );
           this.tbPortNumOut = new System.Windows.Forms.TextBox( );
           this.label5 = new System.Windows.Forms.Label( );
           this.tbPortNumIn = new System.Windows.Forms.TextBox( );
           this.gbCMDGate = new System.Windows.Forms.GroupBox( );
           this.tbIPCMDGateIn = new System.Windows.Forms.TextBox( );
           this.label8 = new System.Windows.Forms.Label( );
           this.tbPortCMDGateIn = new System.Windows.Forms.TextBox( );
           this.label7 = new System.Windows.Forms.Label( );
           this.label29 = new System.Windows.Forms.Label( );
           this.label30 = new System.Windows.Forms.Label( );
           this.tbPortCMDGate = new System.Windows.Forms.TextBox( );
           this.tbIPCMDGate = new System.Windows.Forms.TextBox( );
           this.gbRepeateMode = new System.Windows.Forms.GroupBox( );
           this.label28 = new System.Windows.Forms.Label( );
           this.label25 = new System.Windows.Forms.Label( );
           this.tbPortForRepeater = new System.Windows.Forms.TextBox( );
           this.tbIPForRepeater = new System.Windows.Forms.TextBox( );
           this.chbIsRepeater = new System.Windows.Forms.CheckBox( );
           this.label14 = new System.Windows.Forms.Label( );
           this.tbIPServer = new System.Windows.Forms.TextBox( );
           this.cbMemorizeInProfile = new System.Windows.Forms.CheckBox( );
           this.pnlServerSetup = new System.Windows.Forms.Panel( );
           this.label13 = new System.Windows.Forms.Label( );
           this.tbConnectNumber = new System.Windows.Forms.TextBox( );
           this.button2 = new System.Windows.Forms.Button( );
           this.gbSynhro = new System.Windows.Forms.GroupBox( );
           this.label26 = new System.Windows.Forms.Label( );
           this.tbPortPointForSerializeMesPan = new System.Windows.Forms.TextBox( );
           this.label15 = new System.Windows.Forms.Label( );
           this.tbIPPointForSerializeMesPan = new System.Windows.Forms.TextBox( );
           this.gbRoleARM = new System.Windows.Forms.GroupBox( );
           this.rbClientSecond = new System.Windows.Forms.RadioButton( );
           this.rbClient = new System.Windows.Forms.RadioButton( );
           this.rbServer = new System.Windows.Forms.RadioButton( );
           this.cbRemoutOn = new System.Windows.Forms.CheckBox( );
           this.cbIsToolTipRefDesign = new System.Windows.Forms.CheckBox( );
           this.cbIsShowTabForms = new System.Windows.Forms.CheckBox( );
           this.cbIsShowToolTip = new System.Windows.Forms.CheckBox( );
           this.label16 = new System.Windows.Forms.Label( );
           this.nudStringsInPanMes = new System.Windows.Forms.NumericUpDown( );
           this.pnlTop_pnl1 = new System.Windows.Forms.Panel( );
           this.lbl_pnlTop_pnl1 = new System.Windows.Forms.Label( );
           this.tabPage2 = new System.Windows.Forms.TabPage( );
           this.pnl2 = new System.Windows.Forms.Panel( );
           this.pnlColors_pnl2 = new System.Windows.Forms.Panel( );
           this.groupBox1 = new System.Windows.Forms.GroupBox( );
           this.panel3 = new System.Windows.Forms.Panel( );
           this.btnChInfoCC_pnlColors_pnl2 = new System.Windows.Forms.Button( );
           this.panel2 = new System.Windows.Forms.Panel( );
           this.btnChWarnCC_pnlColors_pnl2 = new System.Windows.Forms.Button( );
           this.panel1 = new System.Windows.Forms.Panel( );
           this.btnChAvarCC_pnlColors_pnl2 = new System.Windows.Forms.Button( );
           this.btnChInfo_pnlColors_pnl2 = new System.Windows.Forms.Button( );
           this.btnChWarn_pnlColors_pnl2 = new System.Windows.Forms.Button( );
           this.btnChAvar_pnlColors_pnl2 = new System.Windows.Forms.Button( );
           this.label4 = new System.Windows.Forms.Label( );
           this.label3 = new System.Windows.Forms.Label( );
           this.label2 = new System.Windows.Forms.Label( );
           this.pnlTop_pnl2 = new System.Windows.Forms.Panel( );
           this.lbl_pnlTop_pnl2 = new System.Windows.Forms.Label( );
           this.tabPage3 = new System.Windows.Forms.TabPage( );
           this.pnl3 = new System.Windows.Forms.Panel( );
           this.pnlTop_pnl3 = new System.Windows.Forms.Panel( );
           this.lbl_pnlTop_pnl3 = new System.Windows.Forms.Label( );
           this.tabPage4 = new System.Windows.Forms.TabPage( );
           this.pnl4 = new System.Windows.Forms.Panel( );
           this.cbLogOnlyDisk = new System.Windows.Forms.CheckBox( );
           this.label10 = new System.Windows.Forms.Label( );
           this.tbAliveInterval = new System.Windows.Forms.TextBox( );
           this.lblLogPlace_pnl4 = new System.Windows.Forms.Label( );
           this.groupBox3 = new System.Windows.Forms.GroupBox( );
           this.rbSaveAs_pnl4 = new System.Windows.Forms.RadioButton( );
           this.rbClear_pnl4 = new System.Windows.Forms.RadioButton( );
           this.linklblLogPlace_pnl4 = new System.Windows.Forms.LinkLabel( );
           this.lblLogMaxSize_pnl4 = new System.Windows.Forms.Label( );
           this.linklblLogSize_pnl4 = new System.Windows.Forms.LinkLabel( );
           this.pnlTop_pnl4 = new System.Windows.Forms.Panel( );
           this.lbl_pnlTop_pnl4 = new System.Windows.Forms.Label( );
           this.tabPage5 = new System.Windows.Forms.TabPage( );
           this.pnl5 = new System.Windows.Forms.Panel( );
           this.cbReqPass = new System.Windows.Forms.CheckBox( );
           this.pnlTop_pnl5 = new System.Windows.Forms.Panel( );
           this.lbl_pnlTop_pnl5 = new System.Windows.Forms.Label( );
           this.tabPage6 = new System.Windows.Forms.TabPage( );
           this.pnl6 = new System.Windows.Forms.Panel( );
           this.label6 = new System.Windows.Forms.Label( );
           this.nudPrecesion = new System.Windows.Forms.NumericUpDown( );
           this.pnlTop_pnl6 = new System.Windows.Forms.Panel( );
           this.lbl_pnlTop_pnl6 = new System.Windows.Forms.Label( );
           this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog( );
           this.splitContainer1.Panel1.SuspendLayout( );
           this.splitContainer1.Panel2.SuspendLayout( );
           this.splitContainer1.SuspendLayout( );
           this.tabControl1.SuspendLayout( );
           this.tabPage1.SuspendLayout( );
           this.pnl1.SuspendLayout( );
           this.groupBox4.SuspendLayout( );
           this.pnlRemExchange.SuspendLayout( );
           this.groupBox2.SuspendLayout( );
           this.gbCMDGate.SuspendLayout( );
           this.gbRepeateMode.SuspendLayout( );
           this.pnlServerSetup.SuspendLayout( );
           this.gbSynhro.SuspendLayout( );
           this.gbRoleARM.SuspendLayout( );
           ( ( System.ComponentModel.ISupportInitialize ) ( this.nudStringsInPanMes ) ).BeginInit( );
           this.pnlTop_pnl1.SuspendLayout( );
           this.tabPage2.SuspendLayout( );
           this.pnl2.SuspendLayout( );
           this.pnlColors_pnl2.SuspendLayout( );
           this.groupBox1.SuspendLayout( );
           this.panel3.SuspendLayout( );
           this.panel2.SuspendLayout( );
           this.panel1.SuspendLayout( );
           this.pnlTop_pnl2.SuspendLayout( );
           this.tabPage3.SuspendLayout( );
           this.pnl3.SuspendLayout( );
           this.pnlTop_pnl3.SuspendLayout( );
           this.tabPage4.SuspendLayout( );
           this.pnl4.SuspendLayout( );
           this.groupBox3.SuspendLayout( );
           this.pnlTop_pnl4.SuspendLayout( );
           this.tabPage5.SuspendLayout( );
           this.pnl5.SuspendLayout( );
           this.pnlTop_pnl5.SuspendLayout( );
           this.tabPage6.SuspendLayout( );
           this.pnl6.SuspendLayout( );
           ( ( System.ComponentModel.ISupportInitialize ) ( this.nudPrecesion ) ).BeginInit( );
           this.pnlTop_pnl6.SuspendLayout( );
           this.SuspendLayout( );
           // 
           // openFileDialog1
           // 
           this.openFileDialog1.FileName = "openFileDialog1";
           // 
           // splitter1
           // 
           this.splitter1.Location = new System.Drawing.Point( 0, 0 );
           this.splitter1.Name = "splitter1";
           this.splitter1.Size = new System.Drawing.Size( 3, 609 );
           this.splitter1.TabIndex = 3;
           this.splitter1.TabStop = false;
           // 
           // splitContainer1
           // 
           this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
           this.splitContainer1.Location = new System.Drawing.Point( 3, 0 );
           this.splitContainer1.Name = "splitContainer1";
           // 
           // splitContainer1.Panel1
           // 
           this.splitContainer1.Panel1.Controls.Add( this.treeView1 );
           this.splitContainer1.Panel1.Controls.Add( this.btnCancel );
           this.splitContainer1.Panel1.Controls.Add( this.button1 );
           // 
           // splitContainer1.Panel2
           // 
           this.splitContainer1.Panel2.Controls.Add( this.tabControl1 );
           this.splitContainer1.Size = new System.Drawing.Size( 948, 609 );
           this.splitContainer1.SplitterDistance = 241;
           this.splitContainer1.TabIndex = 4;
           // 
           // treeView1
           // 
           this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
           this.treeView1.Location = new System.Drawing.Point( 0, 0 );
           this.treeView1.Name = "treeView1";
           treeNode1.Name = "tvnodeStartParam";
           treeNode1.Text = "Параметры запуска";
           treeNode2.Name = "tvnodeColors";
           treeNode2.Text = "Цвета";
           treeNode3.Name = "tvnodeFonts";
           treeNode3.Text = "Шрифт";
           treeNode4.Name = "tvodeLogEvents";
           treeNode4.Text = "Журнал событий";
           treeNode5.Name = "tvnodeSecurity";
           treeNode5.Text = "Безопасность";
           treeNode6.Name = "tvnodeSound";
           treeNode6.Text = "Параметры вывода значений";
           this.treeView1.Nodes.AddRange( new System.Windows.Forms.TreeNode [ ] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6} );
           this.treeView1.Size = new System.Drawing.Size( 241, 563 );
           this.treeView1.TabIndex = 5;
           this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler( this.treeView1_NodeMouseClick );
           // 
           // btnCancel
           // 
           this.btnCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
           this.btnCancel.Location = new System.Drawing.Point( 0, 563 );
           this.btnCancel.Name = "btnCancel";
           this.btnCancel.Size = new System.Drawing.Size( 241, 23 );
           this.btnCancel.TabIndex = 3;
           this.btnCancel.Text = "Отменить";
           this.btnCancel.UseVisualStyleBackColor = true;
           this.btnCancel.Click += new System.EventHandler( this.btnCancel_Click );
           // 
           // button1
           // 
           this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
           this.button1.Location = new System.Drawing.Point( 0, 586 );
           this.button1.Name = "button1";
           this.button1.Size = new System.Drawing.Size( 241, 23 );
           this.button1.TabIndex = 2;
           this.button1.Text = "Принять";
           this.button1.UseVisualStyleBackColor = true;
           this.button1.Click += new System.EventHandler( this.button1_Click );
           // 
           // tabControl1
           // 
           this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
           this.tabControl1.Controls.Add( this.tabPage1 );
           this.tabControl1.Controls.Add( this.tabPage2 );
           this.tabControl1.Controls.Add( this.tabPage3 );
           this.tabControl1.Controls.Add( this.tabPage4 );
           this.tabControl1.Controls.Add( this.tabPage5 );
           this.tabControl1.Controls.Add( this.tabPage6 );
           this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
           this.tabControl1.Location = new System.Drawing.Point( 0, 0 );
           this.tabControl1.Multiline = true;
           this.tabControl1.Name = "tabControl1";
           this.tabControl1.SelectedIndex = 0;
           this.tabControl1.Size = new System.Drawing.Size( 703, 609 );
           this.tabControl1.TabIndex = 12;
           // 
           // tabPage1
           // 
           this.tabPage1.BackColor = System.Drawing.Color.Transparent;
           this.tabPage1.Controls.Add( this.pnl1 );
           this.tabPage1.Location = new System.Drawing.Point( 4, 4 );
           this.tabPage1.Name = "tabPage1";
           this.tabPage1.Padding = new System.Windows.Forms.Padding( 3 );
           this.tabPage1.Size = new System.Drawing.Size( 695, 583 );
           this.tabPage1.TabIndex = 0;
           this.tabPage1.Text = "Параметры запуска";
           this.tabPage1.UseVisualStyleBackColor = true;
           // 
           // pnl1
           // 
           this.pnl1.Controls.Add( this.groupBox4 );
           this.pnl1.Controls.Add( this.label17 );
           this.pnl1.Controls.Add( this.tbDataReNew );
           this.pnl1.Controls.Add( this.pnlRemExchange );
           this.pnl1.Controls.Add( this.cbRemoutOn );
           this.pnl1.Controls.Add( this.cbIsToolTipRefDesign );
           this.pnl1.Controls.Add( this.cbIsShowTabForms );
           this.pnl1.Controls.Add( this.cbIsShowToolTip );
           this.pnl1.Controls.Add( this.label16 );
           this.pnl1.Controls.Add( this.nudStringsInPanMes );
           this.pnl1.Controls.Add( this.pnlTop_pnl1 );
           this.pnl1.Dock = System.Windows.Forms.DockStyle.Fill;
           this.pnl1.Location = new System.Drawing.Point( 3, 3 );
           this.pnl1.Name = "pnl1";
           this.pnl1.Size = new System.Drawing.Size( 689, 577 );
           this.pnl1.TabIndex = 12;
           // 
           // groupBox4
           // 
           this.groupBox4.Controls.Add( this.label9 );
           this.groupBox4.Controls.Add( this.tbIPFCForCMD );
           this.groupBox4.Controls.Add( this.label27 );
           this.groupBox4.Controls.Add( this.tbPortFCForCMD );
           this.groupBox4.Location = new System.Drawing.Point( 11, 183 );
           this.groupBox4.Name = "groupBox4";
           this.groupBox4.Size = new System.Drawing.Size( 173, 83 );
           this.groupBox4.TabIndex = 24;
           this.groupBox4.TabStop = false;
           this.groupBox4.Text = "Вход ФК для приема команд";
           // 
           // label9
           // 
           this.label9.AutoSize = true;
           this.label9.Location = new System.Drawing.Point( 114, 34 );
           this.label9.Name = "label9";
           this.label9.Size = new System.Drawing.Size( 53, 13 );
           this.label9.TabIndex = 27;
           this.label9.Text = "IP-адрес ";
           // 
           // tbIPFCForCMD
           // 
           this.tbIPFCForCMD.Location = new System.Drawing.Point( 7, 31 );
           this.tbIPFCForCMD.Name = "tbIPFCForCMD";
           this.tbIPFCForCMD.Size = new System.Drawing.Size( 101, 20 );
           this.tbIPFCForCMD.TabIndex = 26;
           // 
           // label27
           // 
           this.label27.AutoSize = true;
           this.label27.Location = new System.Drawing.Point( 114, 60 );
           this.label27.Name = "label27";
           this.label27.Size = new System.Drawing.Size( 32, 13 );
           this.label27.TabIndex = 25;
           this.label27.Text = "Порт";
           // 
           // tbPortFCForCMD
           // 
           this.tbPortFCForCMD.Location = new System.Drawing.Point( 41, 57 );
           this.tbPortFCForCMD.Name = "tbPortFCForCMD";
           this.tbPortFCForCMD.Size = new System.Drawing.Size( 67, 20 );
           this.tbPortFCForCMD.TabIndex = 24;
           // 
           // label17
           // 
           this.label17.AutoSize = true;
           this.label17.Location = new System.Drawing.Point( 63, 149 );
           this.label17.Name = "label17";
           this.label17.Size = new System.Drawing.Size( 182, 13 );
           this.label17.TabIndex = 17;
           this.label17.Text = "Интервал обновления данных (мс)";
           // 
           // tbDataReNew
           // 
           this.tbDataReNew.Location = new System.Drawing.Point( 11, 146 );
           this.tbDataReNew.Name = "tbDataReNew";
           this.tbDataReNew.Size = new System.Drawing.Size( 46, 20 );
           this.tbDataReNew.TabIndex = 16;
           // 
           // pnlRemExchange
           // 
           this.pnlRemExchange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           this.pnlRemExchange.Controls.Add( this.chbIsRetransmittingCMD );
           this.pnlRemExchange.Controls.Add( this.groupBox2 );
           this.pnlRemExchange.Controls.Add( this.gbCMDGate );
           this.pnlRemExchange.Controls.Add( this.gbRepeateMode );
           this.pnlRemExchange.Controls.Add( this.chbIsRepeater );
           this.pnlRemExchange.Controls.Add( this.label14 );
           this.pnlRemExchange.Controls.Add( this.tbIPServer );
           this.pnlRemExchange.Controls.Add( this.cbMemorizeInProfile );
           this.pnlRemExchange.Controls.Add( this.pnlServerSetup );
           this.pnlRemExchange.Controls.Add( this.gbRoleARM );
           this.pnlRemExchange.Location = new System.Drawing.Point( 402, 76 );
           this.pnlRemExchange.Name = "pnlRemExchange";
           this.pnlRemExchange.Size = new System.Drawing.Size( 261, 480 );
           this.pnlRemExchange.TabIndex = 15;
           this.pnlRemExchange.Visible = false;
           // 
           // chbIsRetransmittingCMD
           // 
           this.chbIsRetransmittingCMD.AutoSize = true;
           this.chbIsRetransmittingCMD.Location = new System.Drawing.Point( 4, 134 );
           this.chbIsRetransmittingCMD.Name = "chbIsRetransmittingCMD";
           this.chbIsRetransmittingCMD.Size = new System.Drawing.Size( 186, 17 );
           this.chbIsRetransmittingCMD.TabIndex = 29;
           this.chbIsRetransmittingCMD.Text = "Включить ретранляцию команд";
           this.chbIsRetransmittingCMD.UseVisualStyleBackColor = true;
           this.chbIsRetransmittingCMD.CheckedChanged += new System.EventHandler( this.chbIsRetransmittingCMD_CheckedChanged );
           // 
           // groupBox2
           // 
           this.groupBox2.Controls.Add( this.label1 );
           this.groupBox2.Controls.Add( this.tbPortNumOut );
           this.groupBox2.Controls.Add( this.label5 );
           this.groupBox2.Controls.Add( this.tbPortNumIn );
           this.groupBox2.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.groupBox2.Location = new System.Drawing.Point( 3, 415 );
           this.groupBox2.Name = "groupBox2";
           this.groupBox2.Size = new System.Drawing.Size( 215, 37 );
           this.groupBox2.TabIndex = 28;
           this.groupBox2.TabStop = false;
           this.groupBox2.Text = "№ портов для удал. взаимод.";
           // 
           // label1
           // 
           this.label1.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
           this.label1.AutoSize = true;
           this.label1.Location = new System.Drawing.Point( 178, 16 );
           this.label1.Name = "label1";
           this.label1.Size = new System.Drawing.Size( 33, 13 );
           this.label1.TabIndex = 24;
           this.label1.Text = "Исх.";
           // 
           // tbPortNumOut
           // 
           this.tbPortNumOut.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
           this.tbPortNumOut.Location = new System.Drawing.Point( 128, 13 );
           this.tbPortNumOut.Name = "tbPortNumOut";
           this.tbPortNumOut.Size = new System.Drawing.Size( 44, 20 );
           this.tbPortNumOut.TabIndex = 23;
           this.tbPortNumOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
           // 
           // label5
           // 
           this.label5.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
           this.label5.AutoSize = true;
           this.label5.Location = new System.Drawing.Point( 85, 16 );
           this.label5.Name = "label5";
           this.label5.Size = new System.Drawing.Size( 39, 13 );
           this.label5.TabIndex = 22;
           this.label5.Text = "Вход.";
           // 
           // tbPortNumIn
           // 
           this.tbPortNumIn.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
           this.tbPortNumIn.Location = new System.Drawing.Point( 34, 13 );
           this.tbPortNumIn.Name = "tbPortNumIn";
           this.tbPortNumIn.Size = new System.Drawing.Size( 44, 20 );
           this.tbPortNumIn.TabIndex = 21;
           this.tbPortNumIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
           // 
           // gbCMDGate
           // 
           this.gbCMDGate.Controls.Add( this.tbIPCMDGateIn );
           this.gbCMDGate.Controls.Add( this.label8 );
           this.gbCMDGate.Controls.Add( this.tbPortCMDGateIn );
           this.gbCMDGate.Controls.Add( this.label7 );
           this.gbCMDGate.Controls.Add( this.label29 );
           this.gbCMDGate.Controls.Add( this.label30 );
           this.gbCMDGate.Controls.Add( this.tbPortCMDGate );
           this.gbCMDGate.Controls.Add( this.tbIPCMDGate );
           this.gbCMDGate.Enabled = false;
           this.gbCMDGate.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.gbCMDGate.Location = new System.Drawing.Point( 6, 150 );
           this.gbCMDGate.Name = "gbCMDGate";
           this.gbCMDGate.Size = new System.Drawing.Size( 247, 76 );
           this.gbCMDGate.TabIndex = 27;
           this.gbCMDGate.TabStop = false;
           this.gbCMDGate.Text = "Ретрансляция команд";
           // 
           // tbIPCMDGateIn
           // 
           this.tbIPCMDGateIn.Location = new System.Drawing.Point( 89, 19 );
           this.tbIPCMDGateIn.Name = "tbIPCMDGateIn";
           this.tbIPCMDGateIn.Size = new System.Drawing.Size( 100, 20 );
           this.tbIPCMDGateIn.TabIndex = 13;
           // 
           // label8
           // 
           this.label8.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.label8.Location = new System.Drawing.Point( 10, 37 );
           this.label8.Name = "label8";
           this.label8.Size = new System.Drawing.Size( 69, 28 );
           this.label8.TabIndex = 12;
           this.label8.Text = "IP и порт передачи :";
           // 
           // tbPortCMDGateIn
           // 
           this.tbPortCMDGateIn.Location = new System.Drawing.Point( 194, 19 );
           this.tbPortCMDGateIn.Name = "tbPortCMDGateIn";
           this.tbPortCMDGateIn.Size = new System.Drawing.Size( 47, 20 );
           this.tbPortCMDGateIn.TabIndex = 11;
           // 
           // label7
           // 
           this.label7.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.label7.Location = new System.Drawing.Point( 8, 11 );
           this.label7.Name = "label7";
           this.label7.Size = new System.Drawing.Size( 67, 28 );
           this.label7.TabIndex = 10;
           this.label7.Text = "IP и порт приема :";
           // 
           // label29
           // 
           this.label29.AutoSize = true;
           this.label29.Location = new System.Drawing.Point( 195, 60 );
           this.label29.Name = "label29";
           this.label29.Size = new System.Drawing.Size( 36, 13 );
           this.label29.TabIndex = 7;
           this.label29.Text = "Порт";
           // 
           // label30
           // 
           this.label30.AutoSize = true;
           this.label30.Location = new System.Drawing.Point( 117, 60 );
           this.label30.Name = "label30";
           this.label30.Size = new System.Drawing.Size( 19, 13 );
           this.label30.TabIndex = 6;
           this.label30.Text = "IP";
           // 
           // tbPortCMDGate
           // 
           this.tbPortCMDGate.Location = new System.Drawing.Point( 194, 37 );
           this.tbPortCMDGate.Name = "tbPortCMDGate";
           this.tbPortCMDGate.Size = new System.Drawing.Size( 47, 20 );
           this.tbPortCMDGate.TabIndex = 5;
           // 
           // tbIPCMDGate
           // 
           this.tbIPCMDGate.Location = new System.Drawing.Point( 89, 37 );
           this.tbIPCMDGate.Name = "tbIPCMDGate";
           this.tbIPCMDGate.Size = new System.Drawing.Size( 100, 20 );
           this.tbIPCMDGate.TabIndex = 4;
           // 
           // gbRepeateMode
           // 
           this.gbRepeateMode.Controls.Add( this.label28 );
           this.gbRepeateMode.Controls.Add( this.label25 );
           this.gbRepeateMode.Controls.Add( this.tbPortForRepeater );
           this.gbRepeateMode.Controls.Add( this.tbIPForRepeater );
           this.gbRepeateMode.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.gbRepeateMode.Location = new System.Drawing.Point( 3, 84 );
           this.gbRepeateMode.Name = "gbRepeateMode";
           this.gbRepeateMode.Size = new System.Drawing.Size( 250, 44 );
           this.gbRepeateMode.TabIndex = 26;
           this.gbRepeateMode.TabStop = false;
           this.gbRepeateMode.Text = "Адрес ретрансляции";
           // 
           // label28
           // 
           this.label28.AutoSize = true;
           this.label28.Location = new System.Drawing.Point( 198, 21 );
           this.label28.Name = "label28";
           this.label28.Size = new System.Drawing.Size( 36, 13 );
           this.label28.TabIndex = 3;
           this.label28.Text = "Порт";
           // 
           // label25
           // 
           this.label25.AutoSize = true;
           this.label25.Location = new System.Drawing.Point( 120, 22 );
           this.label25.Name = "label25";
           this.label25.Size = new System.Drawing.Size( 19, 13 );
           this.label25.TabIndex = 2;
           this.label25.Text = "IP";
           // 
           // tbPortForRepeater
           // 
           this.tbPortForRepeater.Location = new System.Drawing.Point( 145, 19 );
           this.tbPortForRepeater.Name = "tbPortForRepeater";
           this.tbPortForRepeater.Size = new System.Drawing.Size( 47, 20 );
           this.tbPortForRepeater.TabIndex = 1;
           // 
           // tbIPForRepeater
           // 
           this.tbIPForRepeater.Location = new System.Drawing.Point( 3, 18 );
           this.tbIPForRepeater.Name = "tbIPForRepeater";
           this.tbIPForRepeater.Size = new System.Drawing.Size( 111, 20 );
           this.tbIPForRepeater.TabIndex = 0;
           // 
           // chbIsRepeater
           // 
           this.chbIsRepeater.AutoSize = true;
           this.chbIsRepeater.Location = new System.Drawing.Point( 3, 68 );
           this.chbIsRepeater.Name = "chbIsRepeater";
           this.chbIsRepeater.Size = new System.Drawing.Size( 189, 17 );
           this.chbIsRepeater.TabIndex = 25;
           this.chbIsRepeater.Text = "Включить ретранляцию пакетов";
           this.chbIsRepeater.UseVisualStyleBackColor = true;
           this.chbIsRepeater.CheckedChanged += new System.EventHandler( this.chbIsRepeater_CheckedChanged );
           // 
           // label14
           // 
           this.label14.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
           this.label14.AutoSize = true;
           this.label14.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.label14.Location = new System.Drawing.Point( 138, 392 );
           this.label14.Name = "label14";
           this.label14.Size = new System.Drawing.Size( 72, 13 );
           this.label14.TabIndex = 24;
           this.label14.Text = "IP сервера";
           // 
           // tbIPServer
           // 
           this.tbIPServer.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
           this.tbIPServer.Enabled = false;
           this.tbIPServer.Location = new System.Drawing.Point( 16, 389 );
           this.tbIPServer.Name = "tbIPServer";
           this.tbIPServer.Size = new System.Drawing.Size( 116, 20 );
           this.tbIPServer.TabIndex = 23;
           // 
           // cbMemorizeInProfile
           // 
           this.cbMemorizeInProfile.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
           this.cbMemorizeInProfile.AutoSize = true;
           this.cbMemorizeInProfile.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.cbMemorizeInProfile.ForeColor = System.Drawing.Color.Maroon;
           this.cbMemorizeInProfile.Location = new System.Drawing.Point( 7, 458 );
           this.cbMemorizeInProfile.Name = "cbMemorizeInProfile";
           this.cbMemorizeInProfile.Size = new System.Drawing.Size( 174, 17 );
           this.cbMemorizeInProfile.TabIndex = 18;
           this.cbMemorizeInProfile.Text = "Запомнить в настройках";
           this.cbMemorizeInProfile.UseVisualStyleBackColor = true;
           // 
           // pnlServerSetup
           // 
           this.pnlServerSetup.BackColor = System.Drawing.Color.LightSalmon;
           this.pnlServerSetup.Controls.Add( this.label13 );
           this.pnlServerSetup.Controls.Add( this.tbConnectNumber );
           this.pnlServerSetup.Controls.Add( this.button2 );
           this.pnlServerSetup.Controls.Add( this.gbSynhro );
           this.pnlServerSetup.Location = new System.Drawing.Point( 6, 232 );
           this.pnlServerSetup.Name = "pnlServerSetup";
           this.pnlServerSetup.Size = new System.Drawing.Size( 215, 155 );
           this.pnlServerSetup.TabIndex = 17;
           // 
           // label13
           // 
           this.label13.Location = new System.Drawing.Point( 62, 108 );
           this.label13.Name = "label13";
           this.label13.Size = new System.Drawing.Size( 153, 44 );
           this.label13.TabIndex = 24;
           this.label13.Text = "Количество подключений к серверу удал. взаимодействия";
           // 
           // tbConnectNumber
           // 
           this.tbConnectNumber.AcceptsReturn = true;
           this.tbConnectNumber.Location = new System.Drawing.Point( 17, 120 );
           this.tbConnectNumber.Name = "tbConnectNumber";
           this.tbConnectNumber.Size = new System.Drawing.Size( 44, 20 );
           this.tbConnectNumber.TabIndex = 23;
           this.tbConnectNumber.Text = "1";
           this.tbConnectNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
           // 
           // gbSynhro
           // 
           this.gbSynhro.BackColor = System.Drawing.SystemColors.Info;
           this.gbSynhro.Controls.Add( this.label26 );
           this.gbSynhro.Controls.Add( this.tbPortPointForSerializeMesPan );
           this.gbSynhro.Controls.Add( this.label15 );
           this.gbSynhro.Controls.Add( this.tbIPPointForSerializeMesPan );
           this.gbSynhro.Dock = System.Windows.Forms.DockStyle.Top;
           this.gbSynhro.Location = new System.Drawing.Point( 0, 0 );
           this.gbSynhro.Name = "gbSynhro";
           this.gbSynhro.Size = new System.Drawing.Size( 215, 78 );
           this.gbSynhro.TabIndex = 0;
           this.gbSynhro.TabStop = false;
           this.gbSynhro.Text = "Параметры соединения для сериализация панели сообщений";
           // 
           // label26
           // 
           this.label26.AutoSize = true;
           this.label26.Location = new System.Drawing.Point( 117, 56 );
           this.label26.Name = "label26";
           this.label26.Size = new System.Drawing.Size( 76, 13 );
           this.label26.TabIndex = 4;
           this.label26.Text = "Порт-клиента";
           // 
           // tbPortPointForSerializeMesPan
           // 
           this.tbPortPointForSerializeMesPan.Location = new System.Drawing.Point( 4, 53 );
           this.tbPortPointForSerializeMesPan.Name = "tbPortPointForSerializeMesPan";
           this.tbPortPointForSerializeMesPan.Size = new System.Drawing.Size( 45, 20 );
           this.tbPortPointForSerializeMesPan.TabIndex = 3;
           this.tbPortPointForSerializeMesPan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
           // 
           // label15
           // 
           this.label15.AutoSize = true;
           this.label15.Location = new System.Drawing.Point( 117, 32 );
           this.label15.Name = "label15";
           this.label15.Size = new System.Drawing.Size( 61, 13 );
           this.label15.TabIndex = 2;
           this.label15.Text = "IP-клиента";
           // 
           // tbIPPointForSerializeMesPan
           // 
           this.tbIPPointForSerializeMesPan.Location = new System.Drawing.Point( 4, 29 );
           this.tbIPPointForSerializeMesPan.Name = "tbIPPointForSerializeMesPan";
           this.tbIPPointForSerializeMesPan.Size = new System.Drawing.Size( 107, 20 );
           this.tbIPPointForSerializeMesPan.TabIndex = 1;
           this.tbIPPointForSerializeMesPan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
           // 
           // gbRoleARM
           // 
           this.gbRoleARM.Controls.Add( this.rbClientSecond );
           this.gbRoleARM.Controls.Add( this.rbClient );
           this.gbRoleARM.Controls.Add( this.rbServer );
           this.gbRoleARM.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.gbRoleARM.Location = new System.Drawing.Point( 3, 3 );
           this.gbRoleARM.Name = "gbRoleARM";
           this.gbRoleARM.Size = new System.Drawing.Size( 215, 60 );
           this.gbRoleARM.TabIndex = 15;
           this.gbRoleARM.TabStop = false;
           this.gbRoleARM.Text = "Роль АРМ";
           // 
           // rbClientSecond
           // 
           this.rbClientSecond.AutoSize = true;
           this.rbClientSecond.Location = new System.Drawing.Point( 17, 37 );
           this.rbClientSecond.Name = "rbClientSecond";
           this.rbClientSecond.Size = new System.Drawing.Size( 134, 17 );
           this.rbClientSecond.TabIndex = 2;
           this.rbClientSecond.TabStop = true;
           this.rbClientSecond.Text = "Вторичный клиент";
           this.rbClientSecond.UseVisualStyleBackColor = true;
           this.rbClientSecond.CheckedChanged += new System.EventHandler( this.rbServer_CheckedChanged );
           // 
           // rbClient
           // 
           this.rbClient.AutoSize = true;
           this.rbClient.Location = new System.Drawing.Point( 125, 19 );
           this.rbClient.Name = "rbClient";
           this.rbClient.Size = new System.Drawing.Size( 67, 17 );
           this.rbClient.TabIndex = 1;
           this.rbClient.TabStop = true;
           this.rbClient.Text = "Клиент";
           this.rbClient.UseVisualStyleBackColor = true;
           this.rbClient.CheckedChanged += new System.EventHandler( this.rbServer_CheckedChanged );
           // 
           // rbServer
           // 
           this.rbServer.AutoSize = true;
           this.rbServer.Location = new System.Drawing.Point( 17, 19 );
           this.rbServer.Name = "rbServer";
           this.rbServer.Size = new System.Drawing.Size( 68, 17 );
           this.rbServer.TabIndex = 0;
           this.rbServer.TabStop = true;
           this.rbServer.Text = "Сервер";
           this.rbServer.UseVisualStyleBackColor = true;
           this.rbServer.CheckedChanged += new System.EventHandler( this.rbServer_CheckedChanged );
           // 
           // cbRemoutOn
           // 
           this.cbRemoutOn.AutoSize = true;
           this.cbRemoutOn.Location = new System.Drawing.Point( 437, 53 );
           this.cbRemoutOn.Name = "cbRemoutOn";
           this.cbRemoutOn.Size = new System.Drawing.Size( 219, 17 );
           this.cbRemoutOn.TabIndex = 12;
           this.cbRemoutOn.Text = "Включить удаленное взаимодействие";
           this.cbRemoutOn.UseVisualStyleBackColor = true;
           this.cbRemoutOn.CheckedChanged += new System.EventHandler( this.cbRemoutOn_CheckedChanged );
           // 
           // cbIsToolTipRefDesign
           // 
           this.cbIsToolTipRefDesign.AutoSize = true;
           this.cbIsToolTipRefDesign.Location = new System.Drawing.Point( 43, 100 );
           this.cbIsToolTipRefDesign.Name = "cbIsToolTipRefDesign";
           this.cbIsToolTipRefDesign.Size = new System.Drawing.Size( 323, 17 );
           this.cbIsToolTipRefDesign.TabIndex = 11;
           this.cbIsToolTipRefDesign.Text = "Показывать условное наименование устройств в секциях";
           this.cbIsToolTipRefDesign.UseVisualStyleBackColor = true;
           // 
           // cbIsShowTabForms
           // 
           this.cbIsShowTabForms.AutoSize = true;
           this.cbIsShowTabForms.Location = new System.Drawing.Point( 43, 123 );
           this.cbIsShowTabForms.Name = "cbIsShowTabForms";
           this.cbIsShowTabForms.Size = new System.Drawing.Size( 337, 17 );
           this.cbIsShowTabForms.TabIndex = 10;
           this.cbIsShowTabForms.Text = "Показывать заголовки активных вкладок на главной форме";
           this.cbIsShowTabForms.UseVisualStyleBackColor = true;
           // 
           // cbIsShowToolTip
           // 
           this.cbIsShowToolTip.AutoSize = true;
           this.cbIsShowToolTip.Location = new System.Drawing.Point( 43, 76 );
           this.cbIsShowToolTip.Name = "cbIsShowToolTip";
           this.cbIsShowToolTip.Size = new System.Drawing.Size( 353, 17 );
           this.cbIsShowToolTip.TabIndex = 9;
           this.cbIsShowToolTip.Text = "Показывать подсказку при наведении указателя мыши на блок";
           this.cbIsShowToolTip.UseVisualStyleBackColor = true;
           // 
           // label16
           // 
           this.label16.AutoSize = true;
           this.label16.Location = new System.Drawing.Point( 63, 52 );
           this.label16.Name = "label16";
           this.label16.Size = new System.Drawing.Size( 206, 13 );
           this.label16.TabIndex = 8;
           this.label16.Text = "Количество строк в панели сообщений";
           // 
           // nudStringsInPanMes
           // 
           this.nudStringsInPanMes.Location = new System.Drawing.Point( 11, 50 );
           this.nudStringsInPanMes.Maximum = new decimal( new int [ ] {
            20,
            0,
            0,
            0} );
           this.nudStringsInPanMes.Name = "nudStringsInPanMes";
           this.nudStringsInPanMes.Size = new System.Drawing.Size( 46, 20 );
           this.nudStringsInPanMes.TabIndex = 7;
           // 
           // pnlTop_pnl1
           // 
           this.pnlTop_pnl1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
           this.pnlTop_pnl1.Controls.Add( this.lbl_pnlTop_pnl1 );
           this.pnlTop_pnl1.Dock = System.Windows.Forms.DockStyle.Top;
           this.pnlTop_pnl1.Location = new System.Drawing.Point( 0, 0 );
           this.pnlTop_pnl1.Name = "pnlTop_pnl1";
           this.pnlTop_pnl1.Size = new System.Drawing.Size( 689, 34 );
           this.pnlTop_pnl1.TabIndex = 6;
           // 
           // lbl_pnlTop_pnl1
           // 
           this.lbl_pnlTop_pnl1.AutoSize = true;
           this.lbl_pnlTop_pnl1.Font = new System.Drawing.Font( "Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.lbl_pnlTop_pnl1.Location = new System.Drawing.Point( 190, 6 );
           this.lbl_pnlTop_pnl1.Name = "lbl_pnlTop_pnl1";
           this.lbl_pnlTop_pnl1.Size = new System.Drawing.Size( 147, 16 );
           this.lbl_pnlTop_pnl1.TabIndex = 0;
           this.lbl_pnlTop_pnl1.Text = "Параметры запуска";
           // 
           // tabPage2
           // 
           this.tabPage2.Controls.Add( this.pnl2 );
           this.tabPage2.Location = new System.Drawing.Point( 4, 4 );
           this.tabPage2.Name = "tabPage2";
           this.tabPage2.Padding = new System.Windows.Forms.Padding( 3 );
           this.tabPage2.Size = new System.Drawing.Size( 695, 583 );
           this.tabPage2.TabIndex = 1;
           this.tabPage2.Text = "Цвета";
           this.tabPage2.UseVisualStyleBackColor = true;
           // 
           // pnl2
           // 
           this.pnl2.Controls.Add( this.pnlColors_pnl2 );
           this.pnl2.Controls.Add( this.pnlTop_pnl2 );
           this.pnl2.Dock = System.Windows.Forms.DockStyle.Fill;
           this.pnl2.Location = new System.Drawing.Point( 3, 3 );
           this.pnl2.Name = "pnl2";
           this.pnl2.Size = new System.Drawing.Size( 689, 577 );
           this.pnl2.TabIndex = 3;
           // 
           // pnlColors_pnl2
           // 
           this.pnlColors_pnl2.Controls.Add( this.groupBox1 );
           this.pnlColors_pnl2.Dock = System.Windows.Forms.DockStyle.Fill;
           this.pnlColors_pnl2.Location = new System.Drawing.Point( 0, 26 );
           this.pnlColors_pnl2.Name = "pnlColors_pnl2";
           this.pnlColors_pnl2.Size = new System.Drawing.Size( 689, 551 );
           this.pnlColors_pnl2.TabIndex = 6;
           // 
           // groupBox1
           // 
           this.groupBox1.Controls.Add( this.panel3 );
           this.groupBox1.Controls.Add( this.panel2 );
           this.groupBox1.Controls.Add( this.panel1 );
           this.groupBox1.Controls.Add( this.btnChInfo_pnlColors_pnl2 );
           this.groupBox1.Controls.Add( this.btnChWarn_pnlColors_pnl2 );
           this.groupBox1.Controls.Add( this.btnChAvar_pnlColors_pnl2 );
           this.groupBox1.Controls.Add( this.label4 );
           this.groupBox1.Controls.Add( this.label3 );
           this.groupBox1.Controls.Add( this.label2 );
           this.groupBox1.Location = new System.Drawing.Point( 21, 22 );
           this.groupBox1.Name = "groupBox1";
           this.groupBox1.Size = new System.Drawing.Size( 341, 107 );
           this.groupBox1.TabIndex = 4;
           this.groupBox1.TabStop = false;
           this.groupBox1.Text = "Панель сообщений";
           // 
           // panel3
           // 
           this.panel3.BackColor = System.Drawing.Color.White;
           this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           this.panel3.Controls.Add( this.btnChInfoCC_pnlColors_pnl2 );
           this.panel3.Location = new System.Drawing.Point( 274, 67 );
           this.panel3.Name = "panel3";
           this.panel3.Size = new System.Drawing.Size( 46, 23 );
           this.panel3.TabIndex = 14;
           // 
           // btnChInfoCC_pnlColors_pnl2
           // 
           this.btnChInfoCC_pnlColors_pnl2.BackColor = System.Drawing.Color.Black;
           this.btnChInfoCC_pnlColors_pnl2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
           this.btnChInfoCC_pnlColors_pnl2.Location = new System.Drawing.Point( 4, 4 );
           this.btnChInfoCC_pnlColors_pnl2.Name = "btnChInfoCC_pnlColors_pnl2";
           this.btnChInfoCC_pnlColors_pnl2.Size = new System.Drawing.Size( 37, 12 );
           this.btnChInfoCC_pnlColors_pnl2.TabIndex = 0;
           this.btnChInfoCC_pnlColors_pnl2.UseVisualStyleBackColor = false;
           // 
           // panel2
           // 
           this.panel2.BackColor = System.Drawing.Color.White;
           this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           this.panel2.Controls.Add( this.btnChWarnCC_pnlColors_pnl2 );
           this.panel2.Location = new System.Drawing.Point( 274, 43 );
           this.panel2.Name = "panel2";
           this.panel2.Size = new System.Drawing.Size( 46, 23 );
           this.panel2.TabIndex = 13;
           // 
           // btnChWarnCC_pnlColors_pnl2
           // 
           this.btnChWarnCC_pnlColors_pnl2.BackColor = System.Drawing.Color.Black;
           this.btnChWarnCC_pnlColors_pnl2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
           this.btnChWarnCC_pnlColors_pnl2.Location = new System.Drawing.Point( 4, 4 );
           this.btnChWarnCC_pnlColors_pnl2.Name = "btnChWarnCC_pnlColors_pnl2";
           this.btnChWarnCC_pnlColors_pnl2.Size = new System.Drawing.Size( 37, 12 );
           this.btnChWarnCC_pnlColors_pnl2.TabIndex = 0;
           this.btnChWarnCC_pnlColors_pnl2.UseVisualStyleBackColor = false;
           // 
           // panel1
           // 
           this.panel1.BackColor = System.Drawing.Color.White;
           this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           this.panel1.Controls.Add( this.btnChAvarCC_pnlColors_pnl2 );
           this.panel1.Location = new System.Drawing.Point( 274, 19 );
           this.panel1.Name = "panel1";
           this.panel1.Size = new System.Drawing.Size( 46, 23 );
           this.panel1.TabIndex = 12;
           // 
           // btnChAvarCC_pnlColors_pnl2
           // 
           this.btnChAvarCC_pnlColors_pnl2.BackColor = System.Drawing.Color.Black;
           this.btnChAvarCC_pnlColors_pnl2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
           this.btnChAvarCC_pnlColors_pnl2.Location = new System.Drawing.Point( 4, 4 );
           this.btnChAvarCC_pnlColors_pnl2.Name = "btnChAvarCC_pnlColors_pnl2";
           this.btnChAvarCC_pnlColors_pnl2.Size = new System.Drawing.Size( 37, 12 );
           this.btnChAvarCC_pnlColors_pnl2.TabIndex = 0;
           this.btnChAvarCC_pnlColors_pnl2.UseVisualStyleBackColor = false;
           // 
           // btnChInfo_pnlColors_pnl2
           // 
           this.btnChInfo_pnlColors_pnl2.Location = new System.Drawing.Point( 223, 67 );
           this.btnChInfo_pnlColors_pnl2.Name = "btnChInfo_pnlColors_pnl2";
           this.btnChInfo_pnlColors_pnl2.Size = new System.Drawing.Size( 33, 23 );
           this.btnChInfo_pnlColors_pnl2.TabIndex = 11;
           this.btnChInfo_pnlColors_pnl2.Text = ">>";
           this.btnChInfo_pnlColors_pnl2.UseVisualStyleBackColor = true;
           this.btnChInfo_pnlColors_pnl2.Click += new System.EventHandler( this.btnChAvar_pnlColors_pnl2_Click );
           // 
           // btnChWarn_pnlColors_pnl2
           // 
           this.btnChWarn_pnlColors_pnl2.Location = new System.Drawing.Point( 223, 44 );
           this.btnChWarn_pnlColors_pnl2.Name = "btnChWarn_pnlColors_pnl2";
           this.btnChWarn_pnlColors_pnl2.Size = new System.Drawing.Size( 33, 23 );
           this.btnChWarn_pnlColors_pnl2.TabIndex = 10;
           this.btnChWarn_pnlColors_pnl2.Text = ">>";
           this.btnChWarn_pnlColors_pnl2.UseVisualStyleBackColor = true;
           this.btnChWarn_pnlColors_pnl2.Click += new System.EventHandler( this.btnChAvar_pnlColors_pnl2_Click );
           // 
           // btnChAvar_pnlColors_pnl2
           // 
           this.btnChAvar_pnlColors_pnl2.Location = new System.Drawing.Point( 223, 22 );
           this.btnChAvar_pnlColors_pnl2.Name = "btnChAvar_pnlColors_pnl2";
           this.btnChAvar_pnlColors_pnl2.Size = new System.Drawing.Size( 33, 23 );
           this.btnChAvar_pnlColors_pnl2.TabIndex = 9;
           this.btnChAvar_pnlColors_pnl2.Text = ">>";
           this.btnChAvar_pnlColors_pnl2.UseVisualStyleBackColor = true;
           this.btnChAvar_pnlColors_pnl2.Click += new System.EventHandler( this.btnChAvar_pnlColors_pnl2_Click );
           // 
           // label4
           // 
           this.label4.AutoSize = true;
           this.label4.Location = new System.Drawing.Point( 6, 73 );
           this.label4.Name = "label4";
           this.label4.Size = new System.Drawing.Size( 184, 13 );
           this.label4.TabIndex = 8;
           this.label4.Text = "Цвет информационных сообщений";
           // 
           // label3
           // 
           this.label3.AutoSize = true;
           this.label3.Location = new System.Drawing.Point( 6, 49 );
           this.label3.Name = "label3";
           this.label3.Size = new System.Drawing.Size( 196, 13 );
           this.label3.TabIndex = 7;
           this.label3.Text = "Цвет предупредительных сообщений";
           // 
           // label2
           // 
           this.label2.AutoSize = true;
           this.label2.Location = new System.Drawing.Point( 6, 27 );
           this.label2.Name = "label2";
           this.label2.Size = new System.Drawing.Size( 136, 13 );
           this.label2.TabIndex = 6;
           this.label2.Text = "Цвет аварийных событий";
           // 
           // pnlTop_pnl2
           // 
           this.pnlTop_pnl2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
           this.pnlTop_pnl2.Controls.Add( this.lbl_pnlTop_pnl2 );
           this.pnlTop_pnl2.Dock = System.Windows.Forms.DockStyle.Top;
           this.pnlTop_pnl2.Location = new System.Drawing.Point( 0, 0 );
           this.pnlTop_pnl2.Name = "pnlTop_pnl2";
           this.pnlTop_pnl2.Size = new System.Drawing.Size( 689, 26 );
           this.pnlTop_pnl2.TabIndex = 5;
           // 
           // lbl_pnlTop_pnl2
           // 
           this.lbl_pnlTop_pnl2.AutoSize = true;
           this.lbl_pnlTop_pnl2.Font = new System.Drawing.Font( "Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.lbl_pnlTop_pnl2.Location = new System.Drawing.Point( 190, 6 );
           this.lbl_pnlTop_pnl2.Name = "lbl_pnlTop_pnl2";
           this.lbl_pnlTop_pnl2.Size = new System.Drawing.Size( 50, 16 );
           this.lbl_pnlTop_pnl2.TabIndex = 0;
           this.lbl_pnlTop_pnl2.Text = "Цвета";
           // 
           // tabPage3
           // 
           this.tabPage3.Controls.Add( this.pnl3 );
           this.tabPage3.Location = new System.Drawing.Point( 4, 4 );
           this.tabPage3.Name = "tabPage3";
           this.tabPage3.Size = new System.Drawing.Size( 695, 583 );
           this.tabPage3.TabIndex = 2;
           this.tabPage3.Text = "Шрифт";
           this.tabPage3.UseVisualStyleBackColor = true;
           // 
           // pnl3
           // 
           this.pnl3.Controls.Add( this.pnlTop_pnl3 );
           this.pnl3.Dock = System.Windows.Forms.DockStyle.Fill;
           this.pnl3.Location = new System.Drawing.Point( 0, 0 );
           this.pnl3.Name = "pnl3";
           this.pnl3.Size = new System.Drawing.Size( 695, 583 );
           this.pnl3.TabIndex = 10;
           // 
           // pnlTop_pnl3
           // 
           this.pnlTop_pnl3.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
           this.pnlTop_pnl3.Controls.Add( this.lbl_pnlTop_pnl3 );
           this.pnlTop_pnl3.Dock = System.Windows.Forms.DockStyle.Top;
           this.pnlTop_pnl3.Location = new System.Drawing.Point( 0, 0 );
           this.pnlTop_pnl3.Name = "pnlTop_pnl3";
           this.pnlTop_pnl3.Size = new System.Drawing.Size( 695, 34 );
           this.pnlTop_pnl3.TabIndex = 6;
           // 
           // lbl_pnlTop_pnl3
           // 
           this.lbl_pnlTop_pnl3.AutoSize = true;
           this.lbl_pnlTop_pnl3.Font = new System.Drawing.Font( "Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.lbl_pnlTop_pnl3.Location = new System.Drawing.Point( 190, 6 );
           this.lbl_pnlTop_pnl3.Name = "lbl_pnlTop_pnl3";
           this.lbl_pnlTop_pnl3.Size = new System.Drawing.Size( 58, 16 );
           this.lbl_pnlTop_pnl3.TabIndex = 0;
           this.lbl_pnlTop_pnl3.Text = "Шрифт";
           // 
           // tabPage4
           // 
           this.tabPage4.Controls.Add( this.pnl4 );
           this.tabPage4.Location = new System.Drawing.Point( 4, 4 );
           this.tabPage4.Name = "tabPage4";
           this.tabPage4.Size = new System.Drawing.Size( 695, 583 );
           this.tabPage4.TabIndex = 3;
           this.tabPage4.Text = "Журнал событий";
           this.tabPage4.UseVisualStyleBackColor = true;
           // 
           // pnl4
           // 
           this.pnl4.Controls.Add( this.cbLogOnlyDisk );
           this.pnl4.Controls.Add( this.label10 );
           this.pnl4.Controls.Add( this.tbAliveInterval );
           this.pnl4.Controls.Add( this.lblLogPlace_pnl4 );
           this.pnl4.Controls.Add( this.groupBox3 );
           this.pnl4.Controls.Add( this.linklblLogPlace_pnl4 );
           this.pnl4.Controls.Add( this.lblLogMaxSize_pnl4 );
           this.pnl4.Controls.Add( this.linklblLogSize_pnl4 );
           this.pnl4.Controls.Add( this.pnlTop_pnl4 );
           this.pnl4.Dock = System.Windows.Forms.DockStyle.Fill;
           this.pnl4.Location = new System.Drawing.Point( 0, 0 );
           this.pnl4.Name = "pnl4";
           this.pnl4.Size = new System.Drawing.Size( 695, 583 );
           this.pnl4.TabIndex = 8;
           // 
           // cbLogOnlyDisk
           // 
           this.cbLogOnlyDisk.AutoSize = true;
           this.cbLogOnlyDisk.Location = new System.Drawing.Point( 27, 220 );
           this.cbLogOnlyDisk.Name = "cbLogOnlyDisk";
           this.cbLogOnlyDisk.Size = new System.Drawing.Size( 156, 17 );
           this.cbLogOnlyDisk.TabIndex = 17;
           this.cbLogOnlyDisk.Text = "Только дисковый журнал";
           this.cbLogOnlyDisk.UseVisualStyleBackColor = true;
           this.cbLogOnlyDisk.CheckedChanged += new System.EventHandler( this.cbLogOnlyDisk_CheckedChanged );
           // 
           // label10
           // 
           this.label10.AutoSize = true;
           this.label10.Location = new System.Drawing.Point( 115, 194 );
           this.label10.Name = "label10";
           this.label10.Size = new System.Drawing.Size( 305, 13 );
           this.label10.TabIndex = 16;
           this.label10.Text = "Интервал времени для проверки работоспособности (сек)";
           // 
           // tbAliveInterval
           // 
           this.tbAliveInterval.Location = new System.Drawing.Point( 28, 191 );
           this.tbAliveInterval.Name = "tbAliveInterval";
           this.tbAliveInterval.Size = new System.Drawing.Size( 79, 20 );
           this.tbAliveInterval.TabIndex = 15;
           // 
           // lblLogPlace_pnl4
           // 
           this.lblLogPlace_pnl4.AutoSize = true;
           this.lblLogPlace_pnl4.Location = new System.Drawing.Point( 168, 58 );
           this.lblLogPlace_pnl4.Name = "lblLogPlace_pnl4";
           this.lblLogPlace_pnl4.Size = new System.Drawing.Size( 0, 13 );
           this.lblLogPlace_pnl4.TabIndex = 14;
           // 
           // groupBox3
           // 
           this.groupBox3.Controls.Add( this.rbSaveAs_pnl4 );
           this.groupBox3.Controls.Add( this.rbClear_pnl4 );
           this.groupBox3.Location = new System.Drawing.Point( 28, 116 );
           this.groupBox3.Name = "groupBox3";
           this.groupBox3.Size = new System.Drawing.Size( 200, 65 );
           this.groupBox3.TabIndex = 13;
           this.groupBox3.TabStop = false;
           this.groupBox3.Text = "Действия при переполнении";
           // 
           // rbSaveAs_pnl4
           // 
           this.rbSaveAs_pnl4.AutoSize = true;
           this.rbSaveAs_pnl4.Location = new System.Drawing.Point( 11, 42 );
           this.rbSaveAs_pnl4.Name = "rbSaveAs_pnl4";
           this.rbSaveAs_pnl4.Size = new System.Drawing.Size( 181, 17 );
           this.rbSaveAs_pnl4.TabIndex = 1;
           this.rbSaveAs_pnl4.Text = "Сохранить под другим именем";
           this.rbSaveAs_pnl4.UseVisualStyleBackColor = true;
           this.rbSaveAs_pnl4.CheckedChanged += new System.EventHandler( this.radioButton1_CheckedChanged );
           // 
           // rbClear_pnl4
           // 
           this.rbClear_pnl4.AutoSize = true;
           this.rbClear_pnl4.Checked = true;
           this.rbClear_pnl4.Location = new System.Drawing.Point( 11, 19 );
           this.rbClear_pnl4.Name = "rbClear_pnl4";
           this.rbClear_pnl4.Size = new System.Drawing.Size( 155, 17 );
           this.rbClear_pnl4.TabIndex = 0;
           this.rbClear_pnl4.TabStop = true;
           this.rbClear_pnl4.Text = "Очистить без сохранения";
           this.rbClear_pnl4.UseVisualStyleBackColor = true;
           this.rbClear_pnl4.CheckedChanged += new System.EventHandler( this.radioButton1_CheckedChanged );
           // 
           // linklblLogPlace_pnl4
           // 
           this.linklblLogPlace_pnl4.AutoSize = true;
           this.linklblLogPlace_pnl4.Location = new System.Drawing.Point( 25, 58 );
           this.linklblLogPlace_pnl4.Name = "linklblLogPlace_pnl4";
           this.linklblLogPlace_pnl4.Size = new System.Drawing.Size( 131, 13 );
           this.linklblLogPlace_pnl4.TabIndex = 12;
           this.linklblLogPlace_pnl4.TabStop = true;
           this.linklblLogPlace_pnl4.Text = "Расположение журнала:";
           this.linklblLogPlace_pnl4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.linklblLogPlace_pnl4_LinkClicked );
           // 
           // lblLogMaxSize_pnl4
           // 
           this.lblLogMaxSize_pnl4.AutoSize = true;
           this.lblLogMaxSize_pnl4.Location = new System.Drawing.Point( 168, 84 );
           this.lblLogMaxSize_pnl4.Name = "lblLogMaxSize_pnl4";
           this.lblLogMaxSize_pnl4.Size = new System.Drawing.Size( 13, 13 );
           this.lblLogMaxSize_pnl4.TabIndex = 10;
           this.lblLogMaxSize_pnl4.Text = "0";
           // 
           // linklblLogSize_pnl4
           // 
           this.linklblLogSize_pnl4.AutoSize = true;
           this.linklblLogSize_pnl4.Location = new System.Drawing.Point( 25, 84 );
           this.linklblLogSize_pnl4.Name = "linklblLogSize_pnl4";
           this.linklblLogSize_pnl4.Size = new System.Drawing.Size( 127, 13 );
           this.linklblLogSize_pnl4.TabIndex = 9;
           this.linklblLogSize_pnl4.TabStop = true;
           this.linklblLogSize_pnl4.Text = "Размер журнала (байт):";
           this.linklblLogSize_pnl4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler( this.linklblLogSize_pnl4_LinkClicked );
           // 
           // pnlTop_pnl4
           // 
           this.pnlTop_pnl4.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
           this.pnlTop_pnl4.Controls.Add( this.lbl_pnlTop_pnl4 );
           this.pnlTop_pnl4.Dock = System.Windows.Forms.DockStyle.Top;
           this.pnlTop_pnl4.Location = new System.Drawing.Point( 0, 0 );
           this.pnlTop_pnl4.Name = "pnlTop_pnl4";
           this.pnlTop_pnl4.Size = new System.Drawing.Size( 695, 34 );
           this.pnlTop_pnl4.TabIndex = 6;
           // 
           // lbl_pnlTop_pnl4
           // 
           this.lbl_pnlTop_pnl4.AutoSize = true;
           this.lbl_pnlTop_pnl4.Font = new System.Drawing.Font( "Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.lbl_pnlTop_pnl4.Location = new System.Drawing.Point( 190, 6 );
           this.lbl_pnlTop_pnl4.Name = "lbl_pnlTop_pnl4";
           this.lbl_pnlTop_pnl4.Size = new System.Drawing.Size( 127, 16 );
           this.lbl_pnlTop_pnl4.TabIndex = 0;
           this.lbl_pnlTop_pnl4.Text = "Журнал событий";
           // 
           // tabPage5
           // 
           this.tabPage5.Controls.Add( this.pnl5 );
           this.tabPage5.Location = new System.Drawing.Point( 4, 4 );
           this.tabPage5.Name = "tabPage5";
           this.tabPage5.Size = new System.Drawing.Size( 695, 583 );
           this.tabPage5.TabIndex = 4;
           this.tabPage5.Text = "Безопасность";
           this.tabPage5.UseVisualStyleBackColor = true;
           // 
           // pnl5
           // 
           this.pnl5.Controls.Add( this.cbReqPass );
           this.pnl5.Controls.Add( this.pnlTop_pnl5 );
           this.pnl5.Dock = System.Windows.Forms.DockStyle.Fill;
           this.pnl5.Location = new System.Drawing.Point( 0, 0 );
           this.pnl5.Name = "pnl5";
           this.pnl5.Size = new System.Drawing.Size( 695, 583 );
           this.pnl5.TabIndex = 7;
           // 
           // cbReqPass
           // 
           this.cbReqPass.AutoSize = true;
           this.cbReqPass.Location = new System.Drawing.Point( 21, 54 );
           this.cbReqPass.Name = "cbReqPass";
           this.cbReqPass.Size = new System.Drawing.Size( 315, 17 );
           this.cbReqPass.TabIndex = 7;
           this.cbReqPass.Text = "Запрашивать пароль для выполнения опасных действий";
           this.cbReqPass.UseVisualStyleBackColor = true;
           // 
           // pnlTop_pnl5
           // 
           this.pnlTop_pnl5.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
           this.pnlTop_pnl5.Controls.Add( this.lbl_pnlTop_pnl5 );
           this.pnlTop_pnl5.Dock = System.Windows.Forms.DockStyle.Top;
           this.pnlTop_pnl5.Location = new System.Drawing.Point( 0, 0 );
           this.pnlTop_pnl5.Name = "pnlTop_pnl5";
           this.pnlTop_pnl5.Size = new System.Drawing.Size( 695, 34 );
           this.pnlTop_pnl5.TabIndex = 6;
           // 
           // lbl_pnlTop_pnl5
           // 
           this.lbl_pnlTop_pnl5.AutoSize = true;
           this.lbl_pnlTop_pnl5.Font = new System.Drawing.Font( "Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.lbl_pnlTop_pnl5.Location = new System.Drawing.Point( 190, 6 );
           this.lbl_pnlTop_pnl5.Name = "lbl_pnlTop_pnl5";
           this.lbl_pnlTop_pnl5.Size = new System.Drawing.Size( 107, 16 );
           this.lbl_pnlTop_pnl5.TabIndex = 0;
           this.lbl_pnlTop_pnl5.Text = "Безопасность";
           // 
           // tabPage6
           // 
           this.tabPage6.Controls.Add( this.pnl6 );
           this.tabPage6.Location = new System.Drawing.Point( 4, 4 );
           this.tabPage6.Name = "tabPage6";
           this.tabPage6.Size = new System.Drawing.Size( 695, 583 );
           this.tabPage6.TabIndex = 5;
           this.tabPage6.Text = "Параметры вывода значений";
           this.tabPage6.UseVisualStyleBackColor = true;
           // 
           // pnl6
           // 
           this.pnl6.Controls.Add( this.label6 );
           this.pnl6.Controls.Add( this.nudPrecesion );
           this.pnl6.Controls.Add( this.pnlTop_pnl6 );
           this.pnl6.Dock = System.Windows.Forms.DockStyle.Fill;
           this.pnl6.Location = new System.Drawing.Point( 0, 0 );
           this.pnl6.Name = "pnl6";
           this.pnl6.Size = new System.Drawing.Size( 695, 583 );
           this.pnl6.TabIndex = 11;
           // 
           // label6
           // 
           this.label6.AutoSize = true;
           this.label6.Location = new System.Drawing.Point( 68, 50 );
           this.label6.Name = "label6";
           this.label6.Size = new System.Drawing.Size( 212, 13 );
           this.label6.TabIndex = 8;
           this.label6.Text = "Точность значений с плавающей точкой";
           // 
           // nudPrecesion
           // 
           this.nudPrecesion.Location = new System.Drawing.Point( 15, 48 );
           this.nudPrecesion.Maximum = new decimal( new int [ ] {
            5,
            0,
            0,
            0} );
           this.nudPrecesion.Name = "nudPrecesion";
           this.nudPrecesion.Size = new System.Drawing.Size( 47, 20 );
           this.nudPrecesion.TabIndex = 7;
           this.nudPrecesion.Validated += new System.EventHandler( this.nudPrecesion_Validated );
           // 
           // pnlTop_pnl6
           // 
           this.pnlTop_pnl6.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
           this.pnlTop_pnl6.Controls.Add( this.lbl_pnlTop_pnl6 );
           this.pnlTop_pnl6.Dock = System.Windows.Forms.DockStyle.Top;
           this.pnlTop_pnl6.Location = new System.Drawing.Point( 0, 0 );
           this.pnlTop_pnl6.Name = "pnlTop_pnl6";
           this.pnlTop_pnl6.Size = new System.Drawing.Size( 695, 34 );
           this.pnlTop_pnl6.TabIndex = 6;
           // 
           // lbl_pnlTop_pnl6
           // 
           this.lbl_pnlTop_pnl6.AutoSize = true;
           this.lbl_pnlTop_pnl6.Font = new System.Drawing.Font( "Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
           this.lbl_pnlTop_pnl6.Location = new System.Drawing.Point( 145, 3 );
           this.lbl_pnlTop_pnl6.Name = "lbl_pnlTop_pnl6";
           this.lbl_pnlTop_pnl6.Size = new System.Drawing.Size( 220, 16 );
           this.lbl_pnlTop_pnl6.TabIndex = 0;
           this.lbl_pnlTop_pnl6.Text = "Параметры вывода значений";
           // 
           // frmCustom
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.ClientSize = new System.Drawing.Size( 951, 609 );
           this.Controls.Add( this.splitContainer1 );
           this.Controls.Add( this.splitter1 );
           this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
           this.MaximizeBox = false;
           this.MinimizeBox = false;
           this.Name = "frmCustom";
           this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
           this.Text = "Настройка";
           this.Load += new System.EventHandler( this.frmCustom_Load );
           this.Activated += new System.EventHandler( this.frmCustom_Activated );
           this.splitContainer1.Panel1.ResumeLayout( false );
           this.splitContainer1.Panel2.ResumeLayout( false );
           this.splitContainer1.ResumeLayout( false );
           this.tabControl1.ResumeLayout( false );
           this.tabPage1.ResumeLayout( false );
           this.pnl1.ResumeLayout( false );
           this.pnl1.PerformLayout( );
           this.groupBox4.ResumeLayout( false );
           this.groupBox4.PerformLayout( );
           this.pnlRemExchange.ResumeLayout( false );
           this.pnlRemExchange.PerformLayout( );
           this.groupBox2.ResumeLayout( false );
           this.groupBox2.PerformLayout( );
           this.gbCMDGate.ResumeLayout( false );
           this.gbCMDGate.PerformLayout( );
           this.gbRepeateMode.ResumeLayout( false );
           this.gbRepeateMode.PerformLayout( );
           this.pnlServerSetup.ResumeLayout( false );
           this.pnlServerSetup.PerformLayout( );
           this.gbSynhro.ResumeLayout( false );
           this.gbSynhro.PerformLayout( );
           this.gbRoleARM.ResumeLayout( false );
           this.gbRoleARM.PerformLayout( );
           ( ( System.ComponentModel.ISupportInitialize ) ( this.nudStringsInPanMes ) ).EndInit( );
           this.pnlTop_pnl1.ResumeLayout( false );
           this.pnlTop_pnl1.PerformLayout( );
           this.tabPage2.ResumeLayout( false );
           this.pnl2.ResumeLayout( false );
           this.pnlColors_pnl2.ResumeLayout( false );
           this.groupBox1.ResumeLayout( false );
           this.groupBox1.PerformLayout( );
           this.panel3.ResumeLayout( false );
           this.panel2.ResumeLayout( false );
           this.panel1.ResumeLayout( false );
           this.pnlTop_pnl2.ResumeLayout( false );
           this.pnlTop_pnl2.PerformLayout( );
           this.tabPage3.ResumeLayout( false );
           this.pnl3.ResumeLayout( false );
           this.pnlTop_pnl3.ResumeLayout( false );
           this.pnlTop_pnl3.PerformLayout( );
           this.tabPage4.ResumeLayout( false );
           this.pnl4.ResumeLayout( false );
           this.pnl4.PerformLayout( );
           this.groupBox3.ResumeLayout( false );
           this.groupBox3.PerformLayout( );
           this.pnlTop_pnl4.ResumeLayout( false );
           this.pnlTop_pnl4.PerformLayout( );
           this.tabPage5.ResumeLayout( false );
           this.pnl5.ResumeLayout( false );
           this.pnl5.PerformLayout( );
           this.pnlTop_pnl5.ResumeLayout( false );
           this.pnlTop_pnl5.PerformLayout( );
           this.tabPage6.ResumeLayout( false );
           this.pnl6.ResumeLayout( false );
           this.pnl6.PerformLayout( );
           ( ( System.ComponentModel.ISupportInitialize ) ( this.nudPrecesion ) ).EndInit( );
           this.pnlTop_pnl6.ResumeLayout( false );
           this.pnlTop_pnl6.PerformLayout( );
           this.ResumeLayout( false );
        }
        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.SplitContainer splitContainer1;
		 private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
       private System.Windows.Forms.TabControl tabControl1;
       private System.Windows.Forms.TabPage tabPage1;
       private System.Windows.Forms.TabPage tabPage2;
       private System.Windows.Forms.Panel pnl1;
       private System.Windows.Forms.Label label17;
       private System.Windows.Forms.TextBox tbDataReNew;
       private System.Windows.Forms.Panel pnlRemExchange;
       private System.Windows.Forms.Label label14;
       private System.Windows.Forms.TextBox tbIPServer;
       private System.Windows.Forms.CheckBox cbMemorizeInProfile;
       private System.Windows.Forms.Panel pnlServerSetup;
       private System.Windows.Forms.Button button2;
       private System.Windows.Forms.GroupBox gbSynhro;
       private System.Windows.Forms.Label label15;
       private System.Windows.Forms.TextBox tbIPPointForSerializeMesPan;
       private System.Windows.Forms.GroupBox gbRoleARM;
       private System.Windows.Forms.RadioButton rbClient;
       private System.Windows.Forms.RadioButton rbServer;
       private System.Windows.Forms.CheckBox cbRemoutOn;
       private System.Windows.Forms.CheckBox cbIsToolTipRefDesign;
       private System.Windows.Forms.CheckBox cbIsShowTabForms;
       private System.Windows.Forms.CheckBox cbIsShowToolTip;
       private System.Windows.Forms.Label label16;
       private System.Windows.Forms.NumericUpDown nudStringsInPanMes;
       private System.Windows.Forms.Panel pnlTop_pnl1;
       private System.Windows.Forms.Label lbl_pnlTop_pnl1;
       private System.Windows.Forms.Panel pnl2;
       private System.Windows.Forms.Panel pnlColors_pnl2;
       private System.Windows.Forms.GroupBox groupBox1;
       private System.Windows.Forms.Panel panel3;
       private System.Windows.Forms.Button btnChInfoCC_pnlColors_pnl2;
       private System.Windows.Forms.Panel panel2;
       private System.Windows.Forms.Button btnChWarnCC_pnlColors_pnl2;
       private System.Windows.Forms.Panel panel1;
       private System.Windows.Forms.Button btnChAvarCC_pnlColors_pnl2;
       private System.Windows.Forms.Button btnChInfo_pnlColors_pnl2;
       private System.Windows.Forms.Button btnChWarn_pnlColors_pnl2;
       private System.Windows.Forms.Button btnChAvar_pnlColors_pnl2;
       private System.Windows.Forms.Label label4;
       private System.Windows.Forms.Label label3;
       private System.Windows.Forms.Label label2;
       private System.Windows.Forms.Panel pnlTop_pnl2;
       private System.Windows.Forms.Label lbl_pnlTop_pnl2;
       private System.Windows.Forms.TabPage tabPage3;
       private System.Windows.Forms.Panel pnl3;
       private System.Windows.Forms.Panel pnlTop_pnl3;
       private System.Windows.Forms.Label lbl_pnlTop_pnl3;
       private System.Windows.Forms.TabPage tabPage4;
       private System.Windows.Forms.Panel pnl4;
       private System.Windows.Forms.CheckBox cbLogOnlyDisk;
       private System.Windows.Forms.Label label10;
       private System.Windows.Forms.TextBox tbAliveInterval;
       private System.Windows.Forms.Label lblLogPlace_pnl4;
       private System.Windows.Forms.GroupBox groupBox3;
       private System.Windows.Forms.RadioButton rbSaveAs_pnl4;
       private System.Windows.Forms.RadioButton rbClear_pnl4;
       private System.Windows.Forms.LinkLabel linklblLogPlace_pnl4;
       private System.Windows.Forms.Label lblLogMaxSize_pnl4;
       private System.Windows.Forms.LinkLabel linklblLogSize_pnl4;
       private System.Windows.Forms.Panel pnlTop_pnl4;
       private System.Windows.Forms.Label lbl_pnlTop_pnl4;
       private System.Windows.Forms.TabPage tabPage5;
       private System.Windows.Forms.Panel pnl5;
       private System.Windows.Forms.CheckBox cbReqPass;
       private System.Windows.Forms.Panel pnlTop_pnl5;
       private System.Windows.Forms.Label lbl_pnlTop_pnl5;
       private System.Windows.Forms.TabPage tabPage6;
       private System.Windows.Forms.Panel pnl6;
       private System.Windows.Forms.Label label6;
       private System.Windows.Forms.NumericUpDown nudPrecesion;
       private System.Windows.Forms.Panel pnlTop_pnl6;
       private System.Windows.Forms.Label lbl_pnlTop_pnl6;
       private System.Windows.Forms.Label label13;
       private System.Windows.Forms.TextBox tbConnectNumber;
       private System.Windows.Forms.Label label26;
       private System.Windows.Forms.TextBox tbPortPointForSerializeMesPan;
       private System.Windows.Forms.RadioButton rbClientSecond;
       private System.Windows.Forms.GroupBox gbRepeateMode;
       private System.Windows.Forms.CheckBox chbIsRepeater;
       private System.Windows.Forms.Label label28;
       private System.Windows.Forms.Label label25;
       private System.Windows.Forms.TextBox tbPortForRepeater;
       private System.Windows.Forms.TextBox tbIPForRepeater;
       private System.Windows.Forms.GroupBox gbCMDGate;
       private System.Windows.Forms.Label label29;
       private System.Windows.Forms.Label label30;
       private System.Windows.Forms.TextBox tbPortCMDGate;
       private System.Windows.Forms.TextBox tbIPCMDGate;
       private System.Windows.Forms.GroupBox groupBox2;
       private System.Windows.Forms.TextBox tbPortNumOut;
       private System.Windows.Forms.Label label5;
       private System.Windows.Forms.TextBox tbPortNumIn;
       private System.Windows.Forms.Label label1;
       private System.Windows.Forms.TextBox tbPortCMDGateIn;
       private System.Windows.Forms.Label label7;
       private System.Windows.Forms.Label label8;
       private System.Windows.Forms.CheckBox chbIsRetransmittingCMD;
       private System.Windows.Forms.TextBox tbIPCMDGateIn;
       private System.Windows.Forms.GroupBox groupBox4;
       private System.Windows.Forms.Label label9;
       private System.Windows.Forms.TextBox tbIPFCForCMD;
       private System.Windows.Forms.Label label27;
       private System.Windows.Forms.TextBox tbPortFCForCMD;
    }
}