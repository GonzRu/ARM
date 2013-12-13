namespace DeviceFormLib
{
	partial class frmBMRZ
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
         //if (oscdg != null)
         //   oscdg.Dispose();

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
            this.pnlCurrent = new System.Windows.Forms.Panel();
            this.tcCurrentBottomPanel = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.gbRegStatus = new System.Windows.Forms.GroupBox();
            this.CurrentStatusReg = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.gbControlProgUst = new System.Windows.Forms.GroupBox();
            this.CurrentControlProgUst = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.gbCounters = new System.Windows.Forms.GroupBox();
            this.CurrentCounters = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.gbDirection_P = new System.Windows.Forms.GroupBox();
            this.CurrentDirection_P = new System.Windows.Forms.FlowLayoutPanel();
            this.btnReadResurs = new System.Windows.Forms.Button();
            this.pnlAvar = new System.Windows.Forms.Panel();
            this.tcAvarBottomPanel = new System.Windows.Forms.TabControl();
            this.tabPage14 = new System.Windows.Forms.TabPage();
            this.Avar_BottomPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage13 = new System.Windows.Forms.TabPage();
            this.btnReNew = new System.Windows.Forms.Button();
            this.grbDTStart = new System.Windows.Forms.GroupBox();
            this.dtpStartTimeAvar = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDateAvar = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grbDTFin = new System.Windows.Forms.GroupBox();
            this.dtpEndTimeAvar = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDateAvar = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlSystem = new System.Windows.Forms.Panel();
            this.System_BottomPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlStore = new System.Windows.Forms.Panel();
            this.tcStoreBottomPanel = new System.Windows.Forms.TabControl();
            this.tabPage15 = new System.Windows.Forms.TabPage();
            this.Store_BottomPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage16 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnReadMaxMeterBlock = new System.Windows.Forms.Button();
            this.btnResetMaxMeter = new System.Windows.Forms.Button();
            this.btnReadMaxMeterFC = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnReadStoreBlock = new System.Windows.Forms.Button();
            this.btnResetStore = new System.Windows.Forms.Button();
            this.btnReadStoreFC = new System.Windows.Forms.Button();
            this.pnlConfig = new System.Windows.Forms.Panel();
            this.btnResetValues = new System.Windows.Forms.Button();
            this.tcUstConfigBottomPanel = new System.Windows.Forms.TabControl();
            this.tabPage17 = new System.Windows.Forms.TabPage();
            this.Config_BottomPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage19 = new System.Windows.Forms.TabPage();
            this.btnReNewUstBD = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.dtpStartTimeConfig = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDateConfig = new System.Windows.Forms.DateTimePicker();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.dtpEndTimeConfig = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDateConfig = new System.Windows.Forms.DateTimePicker();
            this.tabPage18 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnFix4Change = new System.Windows.Forms.CheckBox();
            this.btnWriteUst = new System.Windows.Forms.Button();
            this.btnReadUstFC = new System.Windows.Forms.Button();
            this.pnlOscDiag = new System.Windows.Forms.Panel();
            this.ctlLabelTextbox1 = new LabelTextbox.ctlLabelTextbox();
            this.btnReNewOD = new System.Windows.Forms.Button();
            this.gbEndTime = new System.Windows.Forms.GroupBox();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEndData = new System.Windows.Forms.DateTimePicker();
            this.gbStartTime = new System.Windows.Forms.GroupBox();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtpStartData = new System.Windows.Forms.DateTimePicker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.системаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPageSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlStatusSHASU = new System.Windows.Forms.Panel();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.tbIntervalReadMaxMeter = new System.Windows.Forms.TextBox();
            this.lblIntervalReadMaxM2 = new System.Windows.Forms.Label();
            this.lblIntervalReadMaxM1 = new System.Windows.Forms.Label();
            this.cbPeriodReadMaxMeter = new System.Windows.Forms.CheckBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.lblIntervalReadStore2 = new System.Windows.Forms.Label();
            this.tbIntervalReadStore = new System.Windows.Forms.TextBox();
            this.lblIntervalReadStore1 = new System.Windows.Forms.Label();
            this.cbPeriodReadStore = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.Current_Analog_First = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.Current_Analog_Second = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer13 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Current_DiscretIn = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer15 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Current_DiscretOut = new System.Windows.Forms.FlowLayoutPanel();
            this.gbVizov = new System.Windows.Forms.GroupBox();
            this.System_Vizov_vkl = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpAvar = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lstvAvar = new System.Windows.Forms.ListView();
            this.clmnData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmnTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer16 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.Avar_AS_PPZ = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.Avar_AS_PSZ = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.Avar_MaxMin = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.Avar_AS_AsPZ = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.Avar_AS_AsSZ = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.Avar_DS_In = new System.Windows.Forms.FlowLayoutPanel();
            this.Avar_DS_InChange = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.Avar_DS_Out = new System.Windows.Forms.FlowLayoutPanel();
            this.Avar_DS_OutChange = new System.Windows.Forms.FlowLayoutPanel();
            this.tabStore = new System.Windows.Forms.TabPage();
            this.splitContainer10 = new System.Windows.Forms.SplitContainer();
            this.splitContainer11 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.Store_I_IntegralOtkl = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer17 = new System.Windows.Forms.SplitContainer();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Store_I_lastOtkl = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.Store_Maxmeter = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.Store_CountEvent = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpConfUst = new System.Windows.Forms.TabPage();
            this.splitContainer12 = new System.Windows.Forms.SplitContainer();
            this.lstvConfig = new System.Windows.Forms.ListView();
            this.clmnHDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmnhTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbkConfig = new System.Windows.Forms.TabControl();
            this.tbpUst_0 = new System.Windows.Forms.TabPage();
            this.Config_Ustavki_0 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpUst_1 = new System.Windows.Forms.TabPage();
            this.Config_Ustavki_1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpUst_2 = new System.Windows.Forms.TabPage();
            this.Config_Ustavki_2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpUst_3 = new System.Windows.Forms.TabPage();
            this.Config_Ustavki_3 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpUst_4 = new System.Windows.Forms.TabPage();
            this.Config_Ustavki_4 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpUst_5 = new System.Windows.Forms.TabPage();
            this.Config_Ustavki_5 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpUst_6 = new System.Windows.Forms.TabPage();
            this.Config_Ustavki_6 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpUst_7 = new System.Windows.Forms.TabPage();
            this.Config_Ustavki_7 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpUst_8 = new System.Windows.Forms.TabPage();
            this.Config_Ustavki_8 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbpUst_9 = new System.Windows.Forms.TabPage();
            this.Config_Ustavki_9 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.splitContainer_OscDiag = new System.Windows.Forms.SplitContainer();
            this.btnUnionOsc = new System.Windows.Forms.Button();
            this.dgvOscill = new System.Windows.Forms.DataGridView();
            this.clmChBoxOsc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmBlockNameOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockIdOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCommentOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockTimeOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmViewOsc = new System.Windows.Forms.DataGridViewButtonColumn();
            this.clmID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.btnUnionDiag = new System.Windows.Forms.Button();
            this.dgvDiag = new System.Windows.Forms.DataGridView();
            this.clmChBoxDiag = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmBlockNameDiag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockIdDiag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCommentDiag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockTimeDiag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmViewDiag = new System.Windows.Forms.DataGridViewButtonColumn();
            this.clmIDDiag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.tabSystem = new System.Windows.Forms.TabPage();
            this.splitContainer14 = new System.Windows.Forms.SplitContainer();
            this.gbTest = new System.Windows.Forms.GroupBox();
            this.System_Test = new System.Windows.Forms.FlowLayoutPanel();
            this.tabpageEventBlock = new System.Windows.Forms.TabPage();
            this.lstvEventBlock = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTime_logSystemFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chText_logSystemFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageInfo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.PanelInfoTextBox = new System.Windows.Forms.TextBox();
            this.rtbInfo = new System.Windows.Forms.RichTextBox();
            this.pnlCurrent.SuspendLayout();
            this.tcCurrentBottomPanel.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.gbRegStatus.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.gbControlProgUst.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.gbCounters.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.gbDirection_P.SuspendLayout();
            this.pnlAvar.SuspendLayout();
            this.tcAvarBottomPanel.SuspendLayout();
            this.tabPage14.SuspendLayout();
            this.tabPage13.SuspendLayout();
            this.grbDTStart.SuspendLayout();
            this.grbDTFin.SuspendLayout();
            this.pnlSystem.SuspendLayout();
            this.pnlStore.SuspendLayout();
            this.tcStoreBottomPanel.SuspendLayout();
            this.tabPage15.SuspendLayout();
            this.tabPage16.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.pnlConfig.SuspendLayout();
            this.tcUstConfigBottomPanel.SuspendLayout();
            this.tabPage17.SuspendLayout();
            this.tabPage19.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.tabPage18.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.pnlOscDiag.SuspendLayout();
            this.gbEndTime.SuspendLayout();
            this.gbStartTime.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.pnlStatusSHASU.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer13)).BeginInit();
            this.splitContainer13.Panel1.SuspendLayout();
            this.splitContainer13.Panel2.SuspendLayout();
            this.splitContainer13.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer15)).BeginInit();
            this.splitContainer15.Panel1.SuspendLayout();
            this.splitContainer15.Panel2.SuspendLayout();
            this.splitContainer15.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbVizov.SuspendLayout();
            this.tbpAvar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer16)).BeginInit();
            this.splitContainer16.Panel1.SuspendLayout();
            this.splitContainer16.Panel2.SuspendLayout();
            this.splitContainer16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.groupBox18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            this.groupBox19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).BeginInit();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.Panel2.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            this.tabStore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer10)).BeginInit();
            this.splitContainer10.Panel1.SuspendLayout();
            this.splitContainer10.Panel2.SuspendLayout();
            this.splitContainer10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).BeginInit();
            this.splitContainer11.Panel1.SuspendLayout();
            this.splitContainer11.Panel2.SuspendLayout();
            this.splitContainer11.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer17)).BeginInit();
            this.splitContainer17.Panel1.SuspendLayout();
            this.splitContainer17.Panel2.SuspendLayout();
            this.splitContainer17.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tbpConfUst.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer12)).BeginInit();
            this.splitContainer12.Panel1.SuspendLayout();
            this.splitContainer12.Panel2.SuspendLayout();
            this.splitContainer12.SuspendLayout();
            this.tbkConfig.SuspendLayout();
            this.tbpUst_0.SuspendLayout();
            this.tbpUst_1.SuspendLayout();
            this.tbpUst_2.SuspendLayout();
            this.tbpUst_3.SuspendLayout();
            this.tbpUst_4.SuspendLayout();
            this.tbpUst_5.SuspendLayout();
            this.tbpUst_6.SuspendLayout();
            this.tbpUst_7.SuspendLayout();
            this.tbpUst_8.SuspendLayout();
            this.tbpUst_9.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_OscDiag)).BeginInit();
            this.splitContainer_OscDiag.Panel1.SuspendLayout();
            this.splitContainer_OscDiag.Panel2.SuspendLayout();
            this.splitContainer_OscDiag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOscill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiag)).BeginInit();
            this.tabSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer14)).BeginInit();
            this.splitContainer14.Panel1.SuspendLayout();
            this.splitContainer14.SuspendLayout();
            this.gbTest.SuspendLayout();
            this.tabpageEventBlock.SuspendLayout();
            this.tabPageInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCurrent
            // 
            this.pnlCurrent.BackColor = System.Drawing.SystemColors.Control;
            this.pnlCurrent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlCurrent.Controls.Add(this.tcCurrentBottomPanel);
            this.pnlCurrent.Controls.Add(this.btnReadResurs);
            this.pnlCurrent.Location = new System.Drawing.Point(16, 647);
            this.pnlCurrent.Name = "pnlCurrent";
            this.pnlCurrent.Size = new System.Drawing.Size(1130, 86);
            this.pnlCurrent.TabIndex = 1;
            // 
            // tcCurrentBottomPanel
            // 
            this.tcCurrentBottomPanel.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tcCurrentBottomPanel.Controls.Add(this.tabPage4);
            this.tcCurrentBottomPanel.Controls.Add(this.tabPage10);
            this.tcCurrentBottomPanel.Controls.Add(this.tabPage11);
            this.tcCurrentBottomPanel.Controls.Add(this.tabPage12);
            this.tcCurrentBottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcCurrentBottomPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tcCurrentBottomPanel.Location = new System.Drawing.Point(0, 0);
            this.tcCurrentBottomPanel.Name = "tcCurrentBottomPanel";
            this.tcCurrentBottomPanel.SelectedIndex = 0;
            this.tcCurrentBottomPanel.Size = new System.Drawing.Size(1021, 82);
            this.tcCurrentBottomPanel.TabIndex = 5;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.gbRegStatus);
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1013, 56);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "tabPage4";
            // 
            // gbRegStatus
            // 
            this.gbRegStatus.BackColor = System.Drawing.SystemColors.Control;
            this.gbRegStatus.Controls.Add(this.CurrentStatusReg);
            this.gbRegStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRegStatus.Location = new System.Drawing.Point(3, 3);
            this.gbRegStatus.Name = "gbRegStatus";
            this.gbRegStatus.Size = new System.Drawing.Size(1007, 50);
            this.gbRegStatus.TabIndex = 0;
            this.gbRegStatus.TabStop = false;
            this.gbRegStatus.Text = "Регистр статуса";
            // 
            // CurrentStatusReg
            // 
            this.CurrentStatusReg.AutoScroll = true;
            this.CurrentStatusReg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrentStatusReg.Location = new System.Drawing.Point(3, 16);
            this.CurrentStatusReg.Margin = new System.Windows.Forms.Padding(1);
            this.CurrentStatusReg.Name = "CurrentStatusReg";
            this.CurrentStatusReg.Size = new System.Drawing.Size(1001, 31);
            this.CurrentStatusReg.TabIndex = 2;
            // 
            // tabPage10
            // 
            this.tabPage10.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage10.Controls.Add(this.gbControlProgUst);
            this.tabPage10.Location = new System.Drawing.Point(4, 4);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(1013, 56);
            this.tabPage10.TabIndex = 1;
            this.tabPage10.Text = "tabPage10";
            // 
            // gbControlProgUst
            // 
            this.gbControlProgUst.AutoSize = true;
            this.gbControlProgUst.BackColor = System.Drawing.SystemColors.Control;
            this.gbControlProgUst.Controls.Add(this.CurrentControlProgUst);
            this.gbControlProgUst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbControlProgUst.Location = new System.Drawing.Point(3, 3);
            this.gbControlProgUst.Name = "gbControlProgUst";
            this.gbControlProgUst.Size = new System.Drawing.Size(1007, 50);
            this.gbControlProgUst.TabIndex = 2;
            this.gbControlProgUst.TabStop = false;
            this.gbControlProgUst.Text = "Управление программой уставок";
            // 
            // CurrentControlProgUst
            // 
            this.CurrentControlProgUst.AutoScroll = true;
            this.CurrentControlProgUst.AutoSize = true;
            this.CurrentControlProgUst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrentControlProgUst.Location = new System.Drawing.Point(3, 16);
            this.CurrentControlProgUst.Name = "CurrentControlProgUst";
            this.CurrentControlProgUst.Size = new System.Drawing.Size(1001, 31);
            this.CurrentControlProgUst.TabIndex = 0;
            // 
            // tabPage11
            // 
            this.tabPage11.BackColor = System.Drawing.Color.LightSalmon;
            this.tabPage11.Controls.Add(this.gbCounters);
            this.tabPage11.Location = new System.Drawing.Point(4, 4);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new System.Drawing.Size(1013, 56);
            this.tabPage11.TabIndex = 2;
            this.tabPage11.Text = "tabPage11";
            // 
            // gbCounters
            // 
            this.gbCounters.AutoSize = true;
            this.gbCounters.BackColor = System.Drawing.SystemColors.Control;
            this.gbCounters.Controls.Add(this.CurrentCounters);
            this.gbCounters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCounters.Location = new System.Drawing.Point(0, 0);
            this.gbCounters.Name = "gbCounters";
            this.gbCounters.Size = new System.Drawing.Size(1013, 56);
            this.gbCounters.TabIndex = 4;
            this.gbCounters.TabStop = false;
            this.gbCounters.Text = "Счетчики";
            // 
            // CurrentCounters
            // 
            this.CurrentCounters.AutoScroll = true;
            this.CurrentCounters.AutoSize = true;
            this.CurrentCounters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrentCounters.Location = new System.Drawing.Point(3, 16);
            this.CurrentCounters.Name = "CurrentCounters";
            this.CurrentCounters.Size = new System.Drawing.Size(1007, 37);
            this.CurrentCounters.TabIndex = 0;
            // 
            // tabPage12
            // 
            this.tabPage12.BackColor = System.Drawing.Color.LightSalmon;
            this.tabPage12.Controls.Add(this.gbDirection_P);
            this.tabPage12.Location = new System.Drawing.Point(4, 4);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Size = new System.Drawing.Size(1013, 56);
            this.tabPage12.TabIndex = 3;
            this.tabPage12.Text = "tabPage12";
            // 
            // gbDirection_P
            // 
            this.gbDirection_P.AutoSize = true;
            this.gbDirection_P.BackColor = System.Drawing.SystemColors.Control;
            this.gbDirection_P.Controls.Add(this.CurrentDirection_P);
            this.gbDirection_P.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDirection_P.Location = new System.Drawing.Point(0, 0);
            this.gbDirection_P.Name = "gbDirection_P";
            this.gbDirection_P.Size = new System.Drawing.Size(1013, 56);
            this.gbDirection_P.TabIndex = 3;
            this.gbDirection_P.TabStop = false;
            this.gbDirection_P.Text = "Направл. мощности";
            // 
            // CurrentDirection_P
            // 
            this.CurrentDirection_P.AutoScroll = true;
            this.CurrentDirection_P.AutoSize = true;
            this.CurrentDirection_P.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrentDirection_P.Location = new System.Drawing.Point(3, 16);
            this.CurrentDirection_P.Name = "CurrentDirection_P";
            this.CurrentDirection_P.Size = new System.Drawing.Size(1007, 37);
            this.CurrentDirection_P.TabIndex = 0;
            // 
            // btnReadResurs
            // 
            this.btnReadResurs.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnReadResurs.Location = new System.Drawing.Point(1021, 0);
            this.btnReadResurs.Name = "btnReadResurs";
            this.btnReadResurs.Size = new System.Drawing.Size(105, 82);
            this.btnReadResurs.TabIndex = 4;
            this.btnReadResurs.Text = "Ресурс силового выключателя";
            this.btnReadResurs.UseVisualStyleBackColor = true;
            this.btnReadResurs.Visible = false;
            // 
            // pnlAvar
            // 
            this.pnlAvar.BackColor = System.Drawing.SystemColors.Control;
            this.pnlAvar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlAvar.Controls.Add(this.tcAvarBottomPanel);
            this.pnlAvar.Location = new System.Drawing.Point(4, 382);
            this.pnlAvar.Name = "pnlAvar";
            this.pnlAvar.Size = new System.Drawing.Size(1013, 80);
            this.pnlAvar.TabIndex = 2;
            // 
            // tcAvarBottomPanel
            // 
            this.tcAvarBottomPanel.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tcAvarBottomPanel.Controls.Add(this.tabPage14);
            this.tcAvarBottomPanel.Controls.Add(this.tabPage13);
            this.tcAvarBottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcAvarBottomPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tcAvarBottomPanel.Location = new System.Drawing.Point(0, 0);
            this.tcAvarBottomPanel.Name = "tcAvarBottomPanel";
            this.tcAvarBottomPanel.SelectedIndex = 0;
            this.tcAvarBottomPanel.Size = new System.Drawing.Size(1009, 76);
            this.tcAvarBottomPanel.TabIndex = 7;
            // 
            // tabPage14
            // 
            this.tabPage14.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage14.Controls.Add(this.Avar_BottomPanel);
            this.tabPage14.Location = new System.Drawing.Point(4, 4);
            this.tabPage14.Name = "tabPage14";
            this.tabPage14.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage14.Size = new System.Drawing.Size(1001, 50);
            this.tabPage14.TabIndex = 1;
            this.tabPage14.Text = "Общие данные по аварии";
            // 
            // Avar_BottomPanel
            // 
            this.Avar_BottomPanel.AutoScroll = true;
            this.Avar_BottomPanel.BackColor = System.Drawing.SystemColors.Control;
            this.Avar_BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Avar_BottomPanel.Location = new System.Drawing.Point(3, 3);
            this.Avar_BottomPanel.Name = "Avar_BottomPanel";
            this.Avar_BottomPanel.Size = new System.Drawing.Size(995, 44);
            this.Avar_BottomPanel.TabIndex = 7;
            // 
            // tabPage13
            // 
            this.tabPage13.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage13.Controls.Add(this.btnReNew);
            this.tabPage13.Controls.Add(this.grbDTStart);
            this.tabPage13.Controls.Add(this.grbDTFin);
            this.tabPage13.Location = new System.Drawing.Point(4, 4);
            this.tabPage13.Name = "tabPage13";
            this.tabPage13.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage13.Size = new System.Drawing.Size(1001, 50);
            this.tabPage13.TabIndex = 0;
            this.tabPage13.Text = "Параметры выборки из базы данных";
            // 
            // btnReNew
            // 
            this.btnReNew.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnReNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnReNew.Location = new System.Drawing.Point(923, 3);
            this.btnReNew.Name = "btnReNew";
            this.btnReNew.Size = new System.Drawing.Size(75, 44);
            this.btnReNew.TabIndex = 8;
            this.btnReNew.Text = "Обновить";
            this.btnReNew.UseVisualStyleBackColor = true;
            this.btnReNew.Click += new System.EventHandler(this.btnReNew_Click);
            // 
            // grbDTStart
            // 
            this.grbDTStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grbDTStart.Controls.Add(this.dtpStartTimeAvar);
            this.grbDTStart.Controls.Add(this.dtpStartDateAvar);
            this.grbDTStart.Controls.Add(this.label2);
            this.grbDTStart.Controls.Add(this.label1);
            this.grbDTStart.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grbDTStart.ForeColor = System.Drawing.Color.Black;
            this.grbDTStart.Location = new System.Drawing.Point(348, 3);
            this.grbDTStart.Name = "grbDTStart";
            this.grbDTStart.Size = new System.Drawing.Size(286, 42);
            this.grbDTStart.TabIndex = 6;
            this.grbDTStart.TabStop = false;
            this.grbDTStart.Text = "Время начала выборки";
            // 
            // dtpStartTimeAvar
            // 
            this.dtpStartTimeAvar.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTimeAvar.Location = new System.Drawing.Point(197, 15);
            this.dtpStartTimeAvar.Name = "dtpStartTimeAvar";
            this.dtpStartTimeAvar.ShowUpDown = true;
            this.dtpStartTimeAvar.Size = new System.Drawing.Size(78, 20);
            this.dtpStartTimeAvar.TabIndex = 3;
            // 
            // dtpStartDateAvar
            // 
            this.dtpStartDateAvar.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDateAvar.Location = new System.Drawing.Point(48, 15);
            this.dtpStartDateAvar.Name = "dtpStartDateAvar";
            this.dtpStartDateAvar.Size = new System.Drawing.Size(94, 20);
            this.dtpStartDateAvar.TabIndex = 2;
            this.dtpStartDateAvar.Value = new System.DateTime(2007, 4, 25, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Время:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Дата:";
            // 
            // grbDTFin
            // 
            this.grbDTFin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grbDTFin.Controls.Add(this.dtpEndTimeAvar);
            this.grbDTFin.Controls.Add(this.dtpEndDateAvar);
            this.grbDTFin.Controls.Add(this.label4);
            this.grbDTFin.Controls.Add(this.label3);
            this.grbDTFin.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grbDTFin.ForeColor = System.Drawing.Color.Black;
            this.grbDTFin.Location = new System.Drawing.Point(640, 3);
            this.grbDTFin.Name = "grbDTFin";
            this.grbDTFin.Size = new System.Drawing.Size(279, 42);
            this.grbDTFin.TabIndex = 7;
            this.grbDTFin.TabStop = false;
            this.grbDTFin.Text = "Время конца выборки";
            // 
            // dtpEndTimeAvar
            // 
            this.dtpEndTimeAvar.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTimeAvar.Location = new System.Drawing.Point(196, 15);
            this.dtpEndTimeAvar.Name = "dtpEndTimeAvar";
            this.dtpEndTimeAvar.ShowUpDown = true;
            this.dtpEndTimeAvar.Size = new System.Drawing.Size(75, 20);
            this.dtpEndTimeAvar.TabIndex = 3;
            // 
            // dtpEndDateAvar
            // 
            this.dtpEndDateAvar.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDateAvar.Location = new System.Drawing.Point(48, 14);
            this.dtpEndDateAvar.Name = "dtpEndDateAvar";
            this.dtpEndDateAvar.Size = new System.Drawing.Size(93, 20);
            this.dtpEndDateAvar.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(147, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 14);
            this.label4.TabIndex = 1;
            this.label4.Text = "Время:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "Дата:";
            // 
            // pnlSystem
            // 
            this.pnlSystem.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlSystem.BackColor = System.Drawing.Color.LightSalmon;
            this.pnlSystem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSystem.Controls.Add(this.System_BottomPanel);
            this.pnlSystem.Location = new System.Drawing.Point(76, 566);
            this.pnlSystem.Name = "pnlSystem";
            this.pnlSystem.Size = new System.Drawing.Size(914, 80);
            this.pnlSystem.TabIndex = 3;
            // 
            // System_BottomPanel
            // 
            this.System_BottomPanel.BackColor = System.Drawing.SystemColors.Control;
            this.System_BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.System_BottomPanel.Location = new System.Drawing.Point(0, 0);
            this.System_BottomPanel.Name = "System_BottomPanel";
            this.System_BottomPanel.Size = new System.Drawing.Size(910, 76);
            this.System_BottomPanel.TabIndex = 0;
            // 
            // pnlStore
            // 
            this.pnlStore.BackColor = System.Drawing.SystemColors.Control;
            this.pnlStore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlStore.Controls.Add(this.tcStoreBottomPanel);
            this.pnlStore.Location = new System.Drawing.Point(22, 278);
            this.pnlStore.Name = "pnlStore";
            this.pnlStore.Size = new System.Drawing.Size(1002, 87);
            this.pnlStore.TabIndex = 4;
            // 
            // tcStoreBottomPanel
            // 
            this.tcStoreBottomPanel.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tcStoreBottomPanel.Controls.Add(this.tabPage15);
            this.tcStoreBottomPanel.Controls.Add(this.tabPage16);
            this.tcStoreBottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcStoreBottomPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tcStoreBottomPanel.Location = new System.Drawing.Point(0, 0);
            this.tcStoreBottomPanel.Name = "tcStoreBottomPanel";
            this.tcStoreBottomPanel.SelectedIndex = 0;
            this.tcStoreBottomPanel.Size = new System.Drawing.Size(998, 83);
            this.tcStoreBottomPanel.TabIndex = 8;
            // 
            // tabPage15
            // 
            this.tabPage15.BackColor = System.Drawing.Color.LightSalmon;
            this.tabPage15.Controls.Add(this.Store_BottomPanel);
            this.tabPage15.Location = new System.Drawing.Point(4, 4);
            this.tabPage15.Name = "tabPage15";
            this.tabPage15.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage15.Size = new System.Drawing.Size(990, 57);
            this.tabPage15.TabIndex = 0;
            this.tabPage15.Text = "Общие данные";
            this.tabPage15.UseVisualStyleBackColor = true;
            // 
            // Store_BottomPanel
            // 
            this.Store_BottomPanel.AutoScroll = true;
            this.Store_BottomPanel.BackColor = System.Drawing.SystemColors.Control;
            this.Store_BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Store_BottomPanel.Location = new System.Drawing.Point(3, 3);
            this.Store_BottomPanel.Name = "Store_BottomPanel";
            this.Store_BottomPanel.Size = new System.Drawing.Size(984, 51);
            this.Store_BottomPanel.TabIndex = 7;
            // 
            // tabPage16
            // 
            this.tabPage16.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage16.Controls.Add(this.groupBox7);
            this.tabPage16.Controls.Add(this.groupBox6);
            this.tabPage16.Location = new System.Drawing.Point(4, 4);
            this.tabPage16.Name = "tabPage16";
            this.tabPage16.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage16.Size = new System.Drawing.Size(990, 57);
            this.tabPage16.TabIndex = 1;
            this.tabPage16.Text = "Управление накопителем и максметром";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnReadMaxMeterBlock);
            this.groupBox7.Controls.Add(this.btnResetMaxMeter);
            this.groupBox7.Controls.Add(this.btnReadMaxMeterFC);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox7.Location = new System.Drawing.Point(635, 3);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(352, 51);
            this.groupBox7.TabIndex = 7;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Максметр";
            // 
            // btnReadMaxMeterBlock
            // 
            this.btnReadMaxMeterBlock.AutoSize = true;
            this.btnReadMaxMeterBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnReadMaxMeterBlock.Location = new System.Drawing.Point(151, 19);
            this.btnReadMaxMeterBlock.Name = "btnReadMaxMeterBlock";
            this.btnReadMaxMeterBlock.Size = new System.Drawing.Size(100, 23);
            this.btnReadMaxMeterBlock.TabIndex = 2;
            this.btnReadMaxMeterBlock.Text = "Чтение (блок)";
            this.btnReadMaxMeterBlock.UseVisualStyleBackColor = true;
            // 
            // btnResetMaxMeter
            // 
            this.btnResetMaxMeter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnResetMaxMeter.Location = new System.Drawing.Point(257, 19);
            this.btnResetMaxMeter.Name = "btnResetMaxMeter";
            this.btnResetMaxMeter.Size = new System.Drawing.Size(75, 23);
            this.btnResetMaxMeter.TabIndex = 1;
            this.btnResetMaxMeter.Text = "Сброс";
            this.btnResetMaxMeter.UseVisualStyleBackColor = true;
            // 
            // btnReadMaxMeterFC
            // 
            this.btnReadMaxMeterFC.AutoSize = true;
            this.btnReadMaxMeterFC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnReadMaxMeterFC.Location = new System.Drawing.Point(6, 19);
            this.btnReadMaxMeterFC.Name = "btnReadMaxMeterFC";
            this.btnReadMaxMeterFC.Size = new System.Drawing.Size(139, 23);
            this.btnReadMaxMeterFC.TabIndex = 0;
            this.btnReadMaxMeterFC.Text = "Чтение (память ФК)";
            this.btnReadMaxMeterFC.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.btnReadStoreBlock);
            this.groupBox6.Controls.Add(this.btnResetStore);
            this.groupBox6.Controls.Add(this.btnReadStoreFC);
            this.groupBox6.Location = new System.Drawing.Point(284, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(345, 54);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Накопитель";
            // 
            // btnReadStoreBlock
            // 
            this.btnReadStoreBlock.AutoSize = true;
            this.btnReadStoreBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnReadStoreBlock.Location = new System.Drawing.Point(151, 19);
            this.btnReadStoreBlock.Name = "btnReadStoreBlock";
            this.btnReadStoreBlock.Size = new System.Drawing.Size(100, 23);
            this.btnReadStoreBlock.TabIndex = 2;
            this.btnReadStoreBlock.Text = "Чтение (блок)";
            this.btnReadStoreBlock.UseVisualStyleBackColor = true;
            // 
            // btnResetStore
            // 
            this.btnResetStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnResetStore.Location = new System.Drawing.Point(257, 19);
            this.btnResetStore.Name = "btnResetStore";
            this.btnResetStore.Size = new System.Drawing.Size(75, 23);
            this.btnResetStore.TabIndex = 1;
            this.btnResetStore.Text = "Сброс";
            this.btnResetStore.UseVisualStyleBackColor = true;
            // 
            // btnReadStoreFC
            // 
            this.btnReadStoreFC.AutoSize = true;
            this.btnReadStoreFC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnReadStoreFC.Location = new System.Drawing.Point(6, 19);
            this.btnReadStoreFC.Name = "btnReadStoreFC";
            this.btnReadStoreFC.Size = new System.Drawing.Size(139, 23);
            this.btnReadStoreFC.TabIndex = 0;
            this.btnReadStoreFC.Text = "Чтение (память ФК)";
            this.btnReadStoreFC.UseVisualStyleBackColor = true;
            // 
            // pnlConfig
            // 
            this.pnlConfig.BackColor = System.Drawing.SystemColors.Control;
            this.pnlConfig.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlConfig.Controls.Add(this.btnResetValues);
            this.pnlConfig.Controls.Add(this.tcUstConfigBottomPanel);
            this.pnlConfig.Location = new System.Drawing.Point(31, 520);
            this.pnlConfig.Name = "pnlConfig";
            this.pnlConfig.Size = new System.Drawing.Size(986, 80);
            this.pnlConfig.TabIndex = 5;
            // 
            // btnResetValues
            // 
            this.btnResetValues.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnResetValues.Location = new System.Drawing.Point(898, 0);
            this.btnResetValues.Name = "btnResetValues";
            this.btnResetValues.Size = new System.Drawing.Size(84, 76);
            this.btnResetValues.TabIndex = 14;
            this.btnResetValues.Text = "Очистить поля формы";
            this.btnResetValues.UseVisualStyleBackColor = true;
            this.btnResetValues.Click += new System.EventHandler(this.btnResetValues_Click);
            // 
            // tcUstConfigBottomPanel
            // 
            this.tcUstConfigBottomPanel.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tcUstConfigBottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tcUstConfigBottomPanel.Controls.Add(this.tabPage17);
            this.tcUstConfigBottomPanel.Controls.Add(this.tabPage19);
            this.tcUstConfigBottomPanel.Controls.Add(this.tabPage18);
            this.tcUstConfigBottomPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tcUstConfigBottomPanel.Location = new System.Drawing.Point(4, 2);
            this.tcUstConfigBottomPanel.Name = "tcUstConfigBottomPanel";
            this.tcUstConfigBottomPanel.SelectedIndex = 0;
            this.tcUstConfigBottomPanel.Size = new System.Drawing.Size(884, 76);
            this.tcUstConfigBottomPanel.TabIndex = 11;
            // 
            // tabPage17
            // 
            this.tabPage17.BackColor = System.Drawing.Color.LightSalmon;
            this.tabPage17.Controls.Add(this.Config_BottomPanel);
            this.tabPage17.Location = new System.Drawing.Point(4, 4);
            this.tabPage17.Name = "tabPage17";
            this.tabPage17.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage17.Size = new System.Drawing.Size(876, 50);
            this.tabPage17.TabIndex = 0;
            this.tabPage17.Text = "Общие данные по уставкам";
            this.tabPage17.UseVisualStyleBackColor = true;
            // 
            // Config_BottomPanel
            // 
            this.Config_BottomPanel.AutoScroll = true;
            this.Config_BottomPanel.BackColor = System.Drawing.SystemColors.Control;
            this.Config_BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_BottomPanel.Location = new System.Drawing.Point(3, 3);
            this.Config_BottomPanel.Name = "Config_BottomPanel";
            this.Config_BottomPanel.Size = new System.Drawing.Size(870, 44);
            this.Config_BottomPanel.TabIndex = 8;
            // 
            // tabPage19
            // 
            this.tabPage19.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage19.Controls.Add(this.btnReNewUstBD);
            this.tabPage19.Controls.Add(this.groupBox10);
            this.tabPage19.Controls.Add(this.groupBox11);
            this.tabPage19.Location = new System.Drawing.Point(4, 4);
            this.tabPage19.Name = "tabPage19";
            this.tabPage19.Size = new System.Drawing.Size(876, 50);
            this.tabPage19.TabIndex = 2;
            this.tabPage19.Text = "Анализ уставок в СДХ";
            // 
            // btnReNewUstBD
            // 
            this.btnReNewUstBD.Location = new System.Drawing.Point(443, 7);
            this.btnReNewUstBD.Name = "btnReNewUstBD";
            this.btnReNewUstBD.Size = new System.Drawing.Size(75, 40);
            this.btnReNewUstBD.TabIndex = 17;
            this.btnReNewUstBD.Text = "Обновить";
            this.btnReNewUstBD.UseVisualStyleBackColor = true;
            this.btnReNewUstBD.Click += new System.EventHandler(this.btnReNewUstBD_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.dtpStartTimeConfig);
            this.groupBox10.Controls.Add(this.dtpStartDateConfig);
            this.groupBox10.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox10.Location = new System.Drawing.Point(0, 0);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(221, 50);
            this.groupBox10.TabIndex = 16;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Время начала выборки";
            // 
            // dtpStartTimeConfig
            // 
            this.dtpStartTimeConfig.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTimeConfig.Location = new System.Drawing.Point(128, 18);
            this.dtpStartTimeConfig.Name = "dtpStartTimeConfig";
            this.dtpStartTimeConfig.ShowUpDown = true;
            this.dtpStartTimeConfig.Size = new System.Drawing.Size(87, 20);
            this.dtpStartTimeConfig.TabIndex = 1;
            // 
            // dtpStartDateConfig
            // 
            this.dtpStartDateConfig.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDateConfig.Location = new System.Drawing.Point(6, 17);
            this.dtpStartDateConfig.Name = "dtpStartDateConfig";
            this.dtpStartDateConfig.Size = new System.Drawing.Size(99, 20);
            this.dtpStartDateConfig.TabIndex = 0;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.dtpEndTimeConfig);
            this.groupBox11.Controls.Add(this.dtpEndDateConfig);
            this.groupBox11.Location = new System.Drawing.Point(226, 0);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(211, 50);
            this.groupBox11.TabIndex = 15;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Время конца выборки";
            // 
            // dtpEndTimeConfig
            // 
            this.dtpEndTimeConfig.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTimeConfig.Location = new System.Drawing.Point(117, 17);
            this.dtpEndTimeConfig.Name = "dtpEndTimeConfig";
            this.dtpEndTimeConfig.ShowUpDown = true;
            this.dtpEndTimeConfig.Size = new System.Drawing.Size(88, 20);
            this.dtpEndTimeConfig.TabIndex = 1;
            // 
            // dtpEndDateConfig
            // 
            this.dtpEndDateConfig.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDateConfig.Location = new System.Drawing.Point(6, 17);
            this.dtpEndDateConfig.Name = "dtpEndDateConfig";
            this.dtpEndDateConfig.Size = new System.Drawing.Size(92, 20);
            this.dtpEndDateConfig.TabIndex = 0;
            // 
            // tabPage18
            // 
            this.tabPage18.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage18.Controls.Add(this.groupBox9);
            this.tabPage18.Location = new System.Drawing.Point(4, 4);
            this.tabPage18.Name = "tabPage18";
            this.tabPage18.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage18.Size = new System.Drawing.Size(876, 50);
            this.tabPage18.TabIndex = 1;
            this.tabPage18.Text = "Настройка/анализ уставок в устройстве";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.btnFix4Change);
            this.groupBox9.Controls.Add(this.btnWriteUst);
            this.groupBox9.Controls.Add(this.btnReadUstFC);
            this.groupBox9.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox9.Location = new System.Drawing.Point(449, 3);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(424, 44);
            this.groupBox9.TabIndex = 11;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Чтение\\запись уставок";
            // 
            // btnFix4Change
            // 
            this.btnFix4Change.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnFix4Change.AutoSize = true;
            this.btnFix4Change.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFix4Change.Location = new System.Drawing.Point(155, 18);
            this.btnFix4Change.Name = "btnFix4Change";
            this.btnFix4Change.Size = new System.Drawing.Size(140, 23);
            this.btnFix4Change.TabIndex = 0;
            this.btnFix4Change.Text = "Режим задания уставок";
            this.btnFix4Change.UseVisualStyleBackColor = true;
            // 
            // btnWriteUst
            // 
            this.btnWriteUst.AutoSize = true;
            this.btnWriteUst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnWriteUst.Location = new System.Drawing.Point(301, 17);
            this.btnWriteUst.Name = "btnWriteUst";
            this.btnWriteUst.Size = new System.Drawing.Size(111, 24);
            this.btnWriteUst.TabIndex = 2;
            this.btnWriteUst.Text = "Запись уставок";
            this.btnWriteUst.UseVisualStyleBackColor = true;
            // 
            // btnReadUstFC
            // 
            this.btnReadUstFC.AutoSize = true;
            this.btnReadUstFC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnReadUstFC.Location = new System.Drawing.Point(6, 17);
            this.btnReadUstFC.Name = "btnReadUstFC";
            this.btnReadUstFC.Size = new System.Drawing.Size(143, 24);
            this.btnReadUstFC.TabIndex = 0;
            this.btnReadUstFC.Text = "Чтение уставок";
            this.btnReadUstFC.UseVisualStyleBackColor = true;
            this.btnReadUstFC.Click += new System.EventHandler(this.btnReadUstFC_Click);
            // 
            // pnlOscDiag
            // 
            this.pnlOscDiag.BackColor = System.Drawing.SystemColors.Control;
            this.pnlOscDiag.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlOscDiag.Controls.Add(this.ctlLabelTextbox1);
            this.pnlOscDiag.Controls.Add(this.btnReNewOD);
            this.pnlOscDiag.Controls.Add(this.gbEndTime);
            this.pnlOscDiag.Controls.Add(this.gbStartTime);
            this.pnlOscDiag.Location = new System.Drawing.Point(8, 192);
            this.pnlOscDiag.Name = "pnlOscDiag";
            this.pnlOscDiag.Size = new System.Drawing.Size(1000, 80);
            this.pnlOscDiag.TabIndex = 7;
            // 
            // ctlLabelTextbox1
            // 
            this.ctlLabelTextbox1.Caption_Text = "Caption";
            this.ctlLabelTextbox1.Dim_Text = "Dim";
            this.ctlLabelTextbox1.IsChange = false;
            this.ctlLabelTextbox1.LabelText = "Caption";
            this.ctlLabelTextbox1.Location = new System.Drawing.Point(816, 12);
            this.ctlLabelTextbox1.Name = "ctlLabelTextbox1";
            this.ctlLabelTextbox1.Position = LabelTextbox.ctlLabelTextbox.PositionEnum.Right;
            this.ctlLabelTextbox1.Size = new System.Drawing.Size(163, 33);
            this.ctlLabelTextbox1.TabIndex = 4;
            this.ctlLabelTextbox1.TextboxMargin = 0;
            this.ctlLabelTextbox1.TextboxText = "";
            this.ctlLabelTextbox1.Visible = false;
            // 
            // btnReNewOD
            // 
            this.btnReNewOD.Location = new System.Drawing.Point(626, 12);
            this.btnReNewOD.Name = "btnReNewOD";
            this.btnReNewOD.Size = new System.Drawing.Size(75, 52);
            this.btnReNewOD.TabIndex = 3;
            this.btnReNewOD.Text = "Обновить";
            this.btnReNewOD.UseVisualStyleBackColor = true;
            this.btnReNewOD.Click += new System.EventHandler(this.btnReNewOD_Click);
            // 
            // gbEndTime
            // 
            this.gbEndTime.Controls.Add(this.dtpEndTime);
            this.gbEndTime.Controls.Add(this.dtpEndData);
            this.gbEndTime.Location = new System.Drawing.Point(330, 3);
            this.gbEndTime.Name = "gbEndTime";
            this.gbEndTime.Size = new System.Drawing.Size(290, 61);
            this.gbEndTime.TabIndex = 2;
            this.gbEndTime.TabStop = false;
            this.gbEndTime.Text = "Время конца выборки";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTime.Location = new System.Drawing.Point(185, 23);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.ShowUpDown = true;
            this.dtpEndTime.Size = new System.Drawing.Size(70, 20);
            this.dtpEndTime.TabIndex = 1;
            this.dtpEndTime.ValueChanged += new System.EventHandler(this.dtpStartData_ValueChanged);
            // 
            // dtpEndData
            // 
            this.dtpEndData.Location = new System.Drawing.Point(16, 23);
            this.dtpEndData.Name = "dtpEndData";
            this.dtpEndData.Size = new System.Drawing.Size(130, 20);
            this.dtpEndData.TabIndex = 0;
            this.dtpEndData.ValueChanged += new System.EventHandler(this.dtpStartData_ValueChanged);
            // 
            // gbStartTime
            // 
            this.gbStartTime.Controls.Add(this.dtpStartTime);
            this.gbStartTime.Controls.Add(this.dtpStartData);
            this.gbStartTime.Location = new System.Drawing.Point(34, 3);
            this.gbStartTime.Name = "gbStartTime";
            this.gbStartTime.Size = new System.Drawing.Size(290, 61);
            this.gbStartTime.TabIndex = 1;
            this.gbStartTime.TabStop = false;
            this.gbStartTime.Text = "Время начала выборки";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTime.Location = new System.Drawing.Point(189, 23);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Size = new System.Drawing.Size(70, 20);
            this.dtpStartTime.TabIndex = 1;
            this.dtpStartTime.ValueChanged += new System.EventHandler(this.dtpStartData_ValueChanged);
            // 
            // dtpStartData
            // 
            this.dtpStartData.Location = new System.Drawing.Point(20, 23);
            this.dtpStartData.Name = "dtpStartData";
            this.dtpStartData.Size = new System.Drawing.Size(130, 20);
            this.dtpStartData.TabIndex = 0;
            this.dtpStartData.ValueChanged += new System.EventHandler(this.dtpStartData_ValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.системаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 355);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(74, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // системаToolStripMenuItem
            // 
            this.системаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPageSetup,
            this.mnuPrintPreview,
            this.mnuPrint});
            this.системаToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.системаToolStripMenuItem.Name = "системаToolStripMenuItem";
            this.системаToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.системаToolStripMenuItem.Text = "Система";
            // 
            // mnuPageSetup
            // 
            this.mnuPageSetup.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.mnuPageSetup.Name = "mnuPageSetup";
            this.mnuPageSetup.Size = new System.Drawing.Size(233, 22);
            this.mnuPageSetup.Text = "Параметры страницы";
            this.mnuPageSetup.Click += new System.EventHandler(this.mnuPageSetup_Click);
            // 
            // mnuPrintPreview
            // 
            this.mnuPrintPreview.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.mnuPrintPreview.Name = "mnuPrintPreview";
            this.mnuPrintPreview.Size = new System.Drawing.Size(233, 22);
            this.mnuPrintPreview.Text = "Предварительный просмотр";
            this.mnuPrintPreview.Click += new System.EventHandler(this.mnuPrintPreview_Click);
            // 
            // mnuPrint
            // 
            this.mnuPrint.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.mnuPrint.Name = "mnuPrint";
            this.mnuPrint.Size = new System.Drawing.Size(233, 22);
            this.mnuPrint.Text = "Печать";
            this.mnuPrint.Click += new System.EventHandler(this.mnuPrint_Click);
            // 
            // pnlStatusSHASU
            // 
            this.pnlStatusSHASU.BackColor = System.Drawing.SystemColors.Control;
            this.pnlStatusSHASU.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlStatusSHASU.Controls.Add(this.groupBox12);
            this.pnlStatusSHASU.Controls.Add(this.groupBox13);
            this.pnlStatusSHASU.Location = new System.Drawing.Point(76, 466);
            this.pnlStatusSHASU.Name = "pnlStatusSHASU";
            this.pnlStatusSHASU.Size = new System.Drawing.Size(1002, 96);
            this.pnlStatusSHASU.TabIndex = 8;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.tbIntervalReadMaxMeter);
            this.groupBox12.Controls.Add(this.lblIntervalReadMaxM2);
            this.groupBox12.Controls.Add(this.lblIntervalReadMaxM1);
            this.groupBox12.Controls.Add(this.cbPeriodReadMaxMeter);
            this.groupBox12.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox12.Location = new System.Drawing.Point(298, 0);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(325, 92);
            this.groupBox12.TabIndex = 5;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Максметр";
            // 
            // tbIntervalReadMaxMeter
            // 
            this.tbIntervalReadMaxMeter.Location = new System.Drawing.Point(0, 0);
            this.tbIntervalReadMaxMeter.Name = "tbIntervalReadMaxMeter";
            this.tbIntervalReadMaxMeter.Size = new System.Drawing.Size(100, 20);
            this.tbIntervalReadMaxMeter.TabIndex = 0;
            // 
            // lblIntervalReadMaxM2
            // 
            this.lblIntervalReadMaxM2.AutoSize = true;
            this.lblIntervalReadMaxM2.Location = new System.Drawing.Point(250, 55);
            this.lblIntervalReadMaxM2.Name = "lblIntervalReadMaxM2";
            this.lblIntervalReadMaxM2.Size = new System.Drawing.Size(48, 13);
            this.lblIntervalReadMaxM2.TabIndex = 5;
            this.lblIntervalReadMaxM2.Text = "(x10сек)";
            // 
            // lblIntervalReadMaxM1
            // 
            this.lblIntervalReadMaxM1.AutoSize = true;
            this.lblIntervalReadMaxM1.Location = new System.Drawing.Point(10, 52);
            this.lblIntervalReadMaxM1.Name = "lblIntervalReadMaxM1";
            this.lblIntervalReadMaxM1.Size = new System.Drawing.Size(181, 13);
            this.lblIntervalReadMaxM1.TabIndex = 4;
            this.lblIntervalReadMaxM1.Text = "Интервал периодического чтения:";
            // 
            // cbPeriodReadMaxMeter
            // 
            this.cbPeriodReadMaxMeter.Location = new System.Drawing.Point(0, 0);
            this.cbPeriodReadMaxMeter.Name = "cbPeriodReadMaxMeter";
            this.cbPeriodReadMaxMeter.Size = new System.Drawing.Size(104, 24);
            this.cbPeriodReadMaxMeter.TabIndex = 6;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.lblIntervalReadStore2);
            this.groupBox13.Controls.Add(this.tbIntervalReadStore);
            this.groupBox13.Controls.Add(this.lblIntervalReadStore1);
            this.groupBox13.Controls.Add(this.cbPeriodReadStore);
            this.groupBox13.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox13.Location = new System.Drawing.Point(0, 0);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(298, 92);
            this.groupBox13.TabIndex = 4;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Накопитель";
            // 
            // lblIntervalReadStore2
            // 
            this.lblIntervalReadStore2.AutoSize = true;
            this.lblIntervalReadStore2.Location = new System.Drawing.Point(231, 55);
            this.lblIntervalReadStore2.Name = "lblIntervalReadStore2";
            this.lblIntervalReadStore2.Size = new System.Drawing.Size(51, 13);
            this.lblIntervalReadStore2.TabIndex = 6;
            this.lblIntervalReadStore2.Text = " (x10сек)";
            // 
            // tbIntervalReadStore
            // 
            this.tbIntervalReadStore.Location = new System.Drawing.Point(0, 0);
            this.tbIntervalReadStore.Name = "tbIntervalReadStore";
            this.tbIntervalReadStore.Size = new System.Drawing.Size(100, 20);
            this.tbIntervalReadStore.TabIndex = 7;
            // 
            // lblIntervalReadStore1
            // 
            this.lblIntervalReadStore1.AutoSize = true;
            this.lblIntervalReadStore1.Location = new System.Drawing.Point(6, 52);
            this.lblIntervalReadStore1.Name = "lblIntervalReadStore1";
            this.lblIntervalReadStore1.Size = new System.Drawing.Size(181, 13);
            this.lblIntervalReadStore1.TabIndex = 4;
            this.lblIntervalReadStore1.Text = "Интервал периодического чтения:";
            // 
            // cbPeriodReadStore
            // 
            this.cbPeriodReadStore.Location = new System.Drawing.Point(0, 0);
            this.cbPeriodReadStore.Name = "cbPeriodReadStore";
            this.cbPeriodReadStore.Size = new System.Drawing.Size(104, 24);
            this.cbPeriodReadStore.TabIndex = 8;
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(1008, 732);
            this.splitContainer1.SplitterDistance = 137;
            this.splitContainer1.TabIndex = 9;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tbpAvar);
            this.tabControl1.Controls.Add(this.tabStore);
            this.tabControl1.Controls.Add(this.tbpConfUst);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabSystem);
            this.tabControl1.Controls.Add(this.tabpageEventBlock);
            this.tabControl1.Controls.Add(this.tabPageInfo);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1008, 732);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.splitContainer3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1000, 706);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Текущая информация";
            this.tabPage1.Enter += new System.EventHandler(this.tabPage1_Enter);
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControl3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer13);
            this.splitContainer3.Size = new System.Drawing.Size(994, 700);
            this.splitContainer3.SplitterDistance = 193;
            this.splitContainer3.TabIndex = 3;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Controls.Add(this.tabPage9);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(193, 700);
            this.tabControl3.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.Current_Analog_First);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(185, 674);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Первичные";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Current_Analog_First
            // 
            this.Current_Analog_First.AutoScroll = true;
            this.Current_Analog_First.BackColor = System.Drawing.SystemColors.Control;
            this.Current_Analog_First.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Current_Analog_First.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Current_Analog_First.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Current_Analog_First.Location = new System.Drawing.Point(3, 3);
            this.Current_Analog_First.Name = "Current_Analog_First";
            this.Current_Analog_First.Size = new System.Drawing.Size(179, 668);
            this.Current_Analog_First.TabIndex = 2;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.Current_Analog_Second);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(185, 674);
            this.tabPage9.TabIndex = 1;
            this.tabPage9.Text = "Вторичные";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // Current_Analog_Second
            // 
            this.Current_Analog_Second.AutoScroll = true;
            this.Current_Analog_Second.BackColor = System.Drawing.SystemColors.Control;
            this.Current_Analog_Second.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Current_Analog_Second.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Current_Analog_Second.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Current_Analog_Second.Location = new System.Drawing.Point(3, 3);
            this.Current_Analog_Second.Name = "Current_Analog_Second";
            this.Current_Analog_Second.Size = new System.Drawing.Size(179, 668);
            this.Current_Analog_Second.TabIndex = 3;
            // 
            // splitContainer13
            // 
            this.splitContainer13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer13.Location = new System.Drawing.Point(0, 0);
            this.splitContainer13.Name = "splitContainer13";
            // 
            // splitContainer13.Panel1
            // 
            this.splitContainer13.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer13.Panel2
            // 
            this.splitContainer13.Panel2.Controls.Add(this.splitContainer15);
            this.splitContainer13.Size = new System.Drawing.Size(797, 700);
            this.splitContainer13.SplitterDistance = 255;
            this.splitContainer13.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Current_DiscretIn);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 700);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Дискретные входы:";
            // 
            // Current_DiscretIn
            // 
            this.Current_DiscretIn.AutoScroll = true;
            this.Current_DiscretIn.BackColor = System.Drawing.SystemColors.Control;
            this.Current_DiscretIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Current_DiscretIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Current_DiscretIn.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Current_DiscretIn.Location = new System.Drawing.Point(3, 16);
            this.Current_DiscretIn.Name = "Current_DiscretIn";
            this.Current_DiscretIn.Size = new System.Drawing.Size(249, 681);
            this.Current_DiscretIn.TabIndex = 4;
            // 
            // splitContainer15
            // 
            this.splitContainer15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer15.Location = new System.Drawing.Point(0, 0);
            this.splitContainer15.Name = "splitContainer15";
            // 
            // splitContainer15.Panel1
            // 
            this.splitContainer15.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer15.Panel2
            // 
            this.splitContainer15.Panel2.Controls.Add(this.gbVizov);
            this.splitContainer15.Size = new System.Drawing.Size(538, 700);
            this.splitContainer15.SplitterDistance = 269;
            this.splitContainer15.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Current_DiscretOut);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(269, 700);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Дискретные выходы:";
            // 
            // Current_DiscretOut
            // 
            this.Current_DiscretOut.AutoScroll = true;
            this.Current_DiscretOut.BackColor = System.Drawing.SystemColors.Control;
            this.Current_DiscretOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Current_DiscretOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Current_DiscretOut.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Current_DiscretOut.Location = new System.Drawing.Point(3, 16);
            this.Current_DiscretOut.Name = "Current_DiscretOut";
            this.Current_DiscretOut.Size = new System.Drawing.Size(263, 681);
            this.Current_DiscretOut.TabIndex = 5;
            // 
            // gbVizov
            // 
            this.gbVizov.Controls.Add(this.System_Vizov_vkl);
            this.gbVizov.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbVizov.Location = new System.Drawing.Point(0, 0);
            this.gbVizov.Name = "gbVizov";
            this.gbVizov.Size = new System.Drawing.Size(265, 700);
            this.gbVizov.TabIndex = 2;
            this.gbVizov.TabStop = false;
            this.gbVizov.Text = "Причина появления сигнала \"ВЫЗОВ\"";
            // 
            // System_Vizov_vkl
            // 
            this.System_Vizov_vkl.AutoScroll = true;
            this.System_Vizov_vkl.BackColor = System.Drawing.SystemColors.Control;
            this.System_Vizov_vkl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.System_Vizov_vkl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.System_Vizov_vkl.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.System_Vizov_vkl.Location = new System.Drawing.Point(3, 16);
            this.System_Vizov_vkl.Name = "System_Vizov_vkl";
            this.System_Vizov_vkl.Size = new System.Drawing.Size(259, 681);
            this.System_Vizov_vkl.TabIndex = 0;
            // 
            // tbpAvar
            // 
            this.tbpAvar.BackColor = System.Drawing.SystemColors.Control;
            this.tbpAvar.Controls.Add(this.splitContainer2);
            this.tbpAvar.Location = new System.Drawing.Point(4, 22);
            this.tbpAvar.Name = "tbpAvar";
            this.tbpAvar.Padding = new System.Windows.Forms.Padding(3);
            this.tbpAvar.Size = new System.Drawing.Size(1000, 706);
            this.tbpAvar.TabIndex = 1;
            this.tbpAvar.Text = "Информация об авариях";
            this.tbpAvar.Enter += new System.EventHandler(this.tbpAvar_Enter);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.Moccasin;
            this.splitContainer2.Panel1.Controls.Add(this.lstvAvar);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer2.Size = new System.Drawing.Size(994, 700);
            this.splitContainer2.SplitterDistance = 173;
            this.splitContainer2.TabIndex = 0;
            // 
            // lstvAvar
            // 
            this.lstvAvar.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstvAvar.AllowColumnReorder = true;
            this.lstvAvar.BackColor = System.Drawing.Color.LightCyan;
            this.lstvAvar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstvAvar.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmnData,
            this.clmnTime});
            this.lstvAvar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvAvar.FullRowSelect = true;
            this.lstvAvar.GridLines = true;
            this.lstvAvar.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvAvar.Location = new System.Drawing.Point(0, 0);
            this.lstvAvar.MultiSelect = false;
            this.lstvAvar.Name = "lstvAvar";
            this.lstvAvar.Size = new System.Drawing.Size(171, 698);
            this.lstvAvar.TabIndex = 0;
            this.lstvAvar.UseCompatibleStateImageBehavior = false;
            this.lstvAvar.View = System.Windows.Forms.View.Details;
            this.lstvAvar.ItemActivate += new System.EventHandler(this.lstvAvar_ItemActivate);
            // 
            // clmnData
            // 
            this.clmnData.Text = "Дата";
            this.clmnData.Width = 130;
            // 
            // clmnTime
            // 
            this.clmnTime.Text = "Комментарий";
            this.clmnTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clmnTime.Width = 90;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(815, 698);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage2.Controls.Add(this.splitContainer4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(807, 672);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Аналоговые сигналы";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer16);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer6);
            this.splitContainer4.Size = new System.Drawing.Size(799, 664);
            this.splitContainer4.SplitterDistance = 409;
            this.splitContainer4.TabIndex = 8;
            // 
            // splitContainer16
            // 
            this.splitContainer16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer16.Location = new System.Drawing.Point(0, 0);
            this.splitContainer16.Name = "splitContainer16";
            this.splitContainer16.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer16.Panel1
            // 
            this.splitContainer16.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer16.Panel2
            // 
            this.splitContainer16.Panel2.Controls.Add(this.groupBox8);
            this.splitContainer16.Size = new System.Drawing.Size(409, 664);
            this.splitContainer16.SplitterDistance = 512;
            this.splitContainer16.TabIndex = 1;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.groupBox14);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.groupBox15);
            this.splitContainer5.Size = new System.Drawing.Size(409, 512);
            this.splitContainer5.SplitterDistance = 204;
            this.splitContainer5.TabIndex = 0;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.Avar_AS_PPZ);
            this.groupBox14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox14.Location = new System.Drawing.Point(0, 0);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(204, 512);
            this.groupBox14.TabIndex = 0;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Признаки  пуска защиты:";
            // 
            // Avar_AS_PPZ
            // 
            this.Avar_AS_PPZ.AutoScroll = true;
            this.Avar_AS_PPZ.BackColor = System.Drawing.SystemColors.Control;
            this.Avar_AS_PPZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Avar_AS_PPZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Avar_AS_PPZ.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Avar_AS_PPZ.Location = new System.Drawing.Point(3, 16);
            this.Avar_AS_PPZ.Name = "Avar_AS_PPZ";
            this.Avar_AS_PPZ.Size = new System.Drawing.Size(198, 493);
            this.Avar_AS_PPZ.TabIndex = 2;
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.Avar_AS_PSZ);
            this.groupBox15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox15.Location = new System.Drawing.Point(0, 0);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(201, 512);
            this.groupBox15.TabIndex = 0;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Признаки  срабатывания защиты:";
            // 
            // Avar_AS_PSZ
            // 
            this.Avar_AS_PSZ.AutoScroll = true;
            this.Avar_AS_PSZ.BackColor = System.Drawing.SystemColors.Control;
            this.Avar_AS_PSZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Avar_AS_PSZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Avar_AS_PSZ.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Avar_AS_PSZ.Location = new System.Drawing.Point(3, 16);
            this.Avar_AS_PSZ.Name = "Avar_AS_PSZ";
            this.Avar_AS_PSZ.Size = new System.Drawing.Size(195, 493);
            this.Avar_AS_PSZ.TabIndex = 1;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.Avar_MaxMin);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(0, 0);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(409, 148);
            this.groupBox8.TabIndex = 0;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Фазы максимального и минимального тока:";
            // 
            // Avar_MaxMin
            // 
            this.Avar_MaxMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Avar_MaxMin.Location = new System.Drawing.Point(3, 16);
            this.Avar_MaxMin.Name = "Avar_MaxMin";
            this.Avar_MaxMin.Size = new System.Drawing.Size(403, 129);
            this.Avar_MaxMin.TabIndex = 0;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.groupBox16);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.groupBox17);
            this.splitContainer6.Size = new System.Drawing.Size(386, 664);
            this.splitContainer6.SplitterDistance = 193;
            this.splitContainer6.TabIndex = 0;
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.Avar_AS_AsPZ);
            this.groupBox16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox16.Location = new System.Drawing.Point(0, 0);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(193, 664);
            this.groupBox16.TabIndex = 0;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Пуск защиты:";
            // 
            // Avar_AS_AsPZ
            // 
            this.Avar_AS_AsPZ.AutoScroll = true;
            this.Avar_AS_AsPZ.BackColor = System.Drawing.SystemColors.Control;
            this.Avar_AS_AsPZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Avar_AS_AsPZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Avar_AS_AsPZ.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Avar_AS_AsPZ.Location = new System.Drawing.Point(3, 16);
            this.Avar_AS_AsPZ.Name = "Avar_AS_AsPZ";
            this.Avar_AS_AsPZ.Size = new System.Drawing.Size(187, 645);
            this.Avar_AS_AsPZ.TabIndex = 5;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.Avar_AS_AsSZ);
            this.groupBox17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox17.Location = new System.Drawing.Point(0, 0);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(189, 664);
            this.groupBox17.TabIndex = 0;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Срабатывание защиты:";
            // 
            // Avar_AS_AsSZ
            // 
            this.Avar_AS_AsSZ.AutoScroll = true;
            this.Avar_AS_AsSZ.BackColor = System.Drawing.SystemColors.Control;
            this.Avar_AS_AsSZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Avar_AS_AsSZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Avar_AS_AsSZ.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Avar_AS_AsSZ.Location = new System.Drawing.Point(3, 16);
            this.Avar_AS_AsSZ.Name = "Avar_AS_AsSZ";
            this.Avar_AS_AsSZ.Size = new System.Drawing.Size(183, 645);
            this.Avar_AS_AsSZ.TabIndex = 5;
            // 
            // tabPage7
            // 
            this.tabPage7.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage7.Controls.Add(this.splitContainer7);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(807, 672);
            this.tabPage7.TabIndex = 1;
            this.tabPage7.Text = "Дискретные сигналы";
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(3, 3);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.groupBox18);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.groupBox19);
            this.splitContainer7.Size = new System.Drawing.Size(801, 666);
            this.splitContainer7.SplitterDistance = 397;
            this.splitContainer7.TabIndex = 6;
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.splitContainer8);
            this.groupBox18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox18.Location = new System.Drawing.Point(0, 0);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(397, 666);
            this.groupBox18.TabIndex = 0;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Входы/изменения:";
            // 
            // splitContainer8
            // 
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.Location = new System.Drawing.Point(3, 16);
            this.splitContainer8.Name = "splitContainer8";
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.Avar_DS_In);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.Avar_DS_InChange);
            this.splitContainer8.Size = new System.Drawing.Size(391, 647);
            this.splitContainer8.SplitterDistance = 192;
            this.splitContainer8.TabIndex = 0;
            // 
            // Avar_DS_In
            // 
            this.Avar_DS_In.AutoScroll = true;
            this.Avar_DS_In.BackColor = System.Drawing.SystemColors.Control;
            this.Avar_DS_In.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Avar_DS_In.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Avar_DS_In.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Avar_DS_In.Location = new System.Drawing.Point(0, 0);
            this.Avar_DS_In.Name = "Avar_DS_In";
            this.Avar_DS_In.Size = new System.Drawing.Size(192, 647);
            this.Avar_DS_In.TabIndex = 1;
            // 
            // Avar_DS_InChange
            // 
            this.Avar_DS_InChange.AutoScroll = true;
            this.Avar_DS_InChange.BackColor = System.Drawing.SystemColors.Control;
            this.Avar_DS_InChange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Avar_DS_InChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Avar_DS_InChange.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Avar_DS_InChange.Location = new System.Drawing.Point(0, 0);
            this.Avar_DS_InChange.Name = "Avar_DS_InChange";
            this.Avar_DS_InChange.Size = new System.Drawing.Size(195, 647);
            this.Avar_DS_InChange.TabIndex = 2;
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.splitContainer9);
            this.groupBox19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox19.Location = new System.Drawing.Point(0, 0);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(400, 666);
            this.groupBox19.TabIndex = 0;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Выходы/изменения:";
            // 
            // splitContainer9
            // 
            this.splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer9.Location = new System.Drawing.Point(3, 16);
            this.splitContainer9.Name = "splitContainer9";
            // 
            // splitContainer9.Panel1
            // 
            this.splitContainer9.Panel1.Controls.Add(this.Avar_DS_Out);
            // 
            // splitContainer9.Panel2
            // 
            this.splitContainer9.Panel2.Controls.Add(this.Avar_DS_OutChange);
            this.splitContainer9.Size = new System.Drawing.Size(394, 647);
            this.splitContainer9.SplitterDistance = 202;
            this.splitContainer9.TabIndex = 0;
            // 
            // Avar_DS_Out
            // 
            this.Avar_DS_Out.AutoScroll = true;
            this.Avar_DS_Out.BackColor = System.Drawing.SystemColors.Control;
            this.Avar_DS_Out.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Avar_DS_Out.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Avar_DS_Out.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Avar_DS_Out.Location = new System.Drawing.Point(0, 0);
            this.Avar_DS_Out.Name = "Avar_DS_Out";
            this.Avar_DS_Out.Size = new System.Drawing.Size(202, 647);
            this.Avar_DS_Out.TabIndex = 1;
            // 
            // Avar_DS_OutChange
            // 
            this.Avar_DS_OutChange.AutoScroll = true;
            this.Avar_DS_OutChange.BackColor = System.Drawing.SystemColors.Control;
            this.Avar_DS_OutChange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Avar_DS_OutChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Avar_DS_OutChange.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Avar_DS_OutChange.Location = new System.Drawing.Point(0, 0);
            this.Avar_DS_OutChange.Name = "Avar_DS_OutChange";
            this.Avar_DS_OutChange.Size = new System.Drawing.Size(188, 647);
            this.Avar_DS_OutChange.TabIndex = 2;
            // 
            // tabStore
            // 
            this.tabStore.BackColor = System.Drawing.SystemColors.Control;
            this.tabStore.Controls.Add(this.splitContainer10);
            this.tabStore.Location = new System.Drawing.Point(4, 22);
            this.tabStore.Name = "tabStore";
            this.tabStore.Padding = new System.Windows.Forms.Padding(3);
            this.tabStore.Size = new System.Drawing.Size(1000, 706);
            this.tabStore.TabIndex = 2;
            this.tabStore.Text = "Накопительная информация";
            this.tabStore.Enter += new System.EventHandler(this.tabStore_Enter);
            // 
            // splitContainer10
            // 
            this.splitContainer10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer10.Location = new System.Drawing.Point(3, 3);
            this.splitContainer10.Name = "splitContainer10";
            // 
            // splitContainer10.Panel1
            // 
            this.splitContainer10.Panel1.Controls.Add(this.splitContainer11);
            // 
            // splitContainer10.Panel2
            // 
            this.splitContainer10.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer10.Size = new System.Drawing.Size(994, 700);
            this.splitContainer10.SplitterDistance = 466;
            this.splitContainer10.TabIndex = 0;
            // 
            // splitContainer11
            // 
            this.splitContainer11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer11.Location = new System.Drawing.Point(0, 0);
            this.splitContainer11.Name = "splitContainer11";
            this.splitContainer11.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer11.Panel1
            // 
            this.splitContainer11.Panel1.Controls.Add(this.groupBox4);
            // 
            // splitContainer11.Panel2
            // 
            this.splitContainer11.Panel2.Controls.Add(this.splitContainer17);
            this.splitContainer11.Size = new System.Drawing.Size(466, 700);
            this.splitContainer11.SplitterDistance = 172;
            this.splitContainer11.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox4.Controls.Add(this.Store_I_IntegralOtkl);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(466, 172);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Суммарный ток отключений";
            // 
            // Store_I_IntegralOtkl
            // 
            this.Store_I_IntegralOtkl.AutoScroll = true;
            this.Store_I_IntegralOtkl.BackColor = System.Drawing.SystemColors.Control;
            this.Store_I_IntegralOtkl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Store_I_IntegralOtkl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Store_I_IntegralOtkl.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Store_I_IntegralOtkl.Location = new System.Drawing.Point(3, 16);
            this.Store_I_IntegralOtkl.Name = "Store_I_IntegralOtkl";
            this.Store_I_IntegralOtkl.Size = new System.Drawing.Size(460, 153);
            this.Store_I_IntegralOtkl.TabIndex = 2;
            // 
            // splitContainer17
            // 
            this.splitContainer17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer17.Location = new System.Drawing.Point(0, 0);
            this.splitContainer17.Name = "splitContainer17";
            this.splitContainer17.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer17.Panel1
            // 
            this.splitContainer17.Panel1.Controls.Add(this.groupBox5);
            // 
            // splitContainer17.Panel2
            // 
            this.splitContainer17.Panel2.Controls.Add(this.groupBox20);
            this.splitContainer17.Size = new System.Drawing.Size(466, 524);
            this.splitContainer17.SplitterDistance = 156;
            this.splitContainer17.TabIndex = 7;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.Store_I_lastOtkl);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(466, 156);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Ток при последнем отключении";
            // 
            // Store_I_lastOtkl
            // 
            this.Store_I_lastOtkl.AutoScroll = true;
            this.Store_I_lastOtkl.BackColor = System.Drawing.SystemColors.Control;
            this.Store_I_lastOtkl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Store_I_lastOtkl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Store_I_lastOtkl.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Store_I_lastOtkl.Location = new System.Drawing.Point(3, 16);
            this.Store_I_lastOtkl.Name = "Store_I_lastOtkl";
            this.Store_I_lastOtkl.Size = new System.Drawing.Size(460, 137);
            this.Store_I_lastOtkl.TabIndex = 2;
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.Store_Maxmeter);
            this.groupBox20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox20.Location = new System.Drawing.Point(0, 0);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(466, 364);
            this.groupBox20.TabIndex = 1;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Максметр";
            // 
            // Store_Maxmeter
            // 
            this.Store_Maxmeter.AutoScroll = true;
            this.Store_Maxmeter.BackColor = System.Drawing.SystemColors.Control;
            this.Store_Maxmeter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Store_Maxmeter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Store_Maxmeter.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Store_Maxmeter.Location = new System.Drawing.Point(3, 16);
            this.Store_Maxmeter.Name = "Store_Maxmeter";
            this.Store_Maxmeter.Size = new System.Drawing.Size(460, 345);
            this.Store_Maxmeter.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.Store_CountEvent);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(524, 700);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Счетчики событий";
            // 
            // Store_CountEvent
            // 
            this.Store_CountEvent.AutoScroll = true;
            this.Store_CountEvent.BackColor = System.Drawing.SystemColors.Control;
            this.Store_CountEvent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Store_CountEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Store_CountEvent.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Store_CountEvent.Location = new System.Drawing.Point(3, 16);
            this.Store_CountEvent.Name = "Store_CountEvent";
            this.Store_CountEvent.Size = new System.Drawing.Size(518, 681);
            this.Store_CountEvent.TabIndex = 0;
            // 
            // tbpConfUst
            // 
            this.tbpConfUst.BackColor = System.Drawing.SystemColors.Control;
            this.tbpConfUst.Controls.Add(this.splitContainer12);
            this.tbpConfUst.Location = new System.Drawing.Point(4, 22);
            this.tbpConfUst.Name = "tbpConfUst";
            this.tbpConfUst.Padding = new System.Windows.Forms.Padding(3);
            this.tbpConfUst.Size = new System.Drawing.Size(1000, 706);
            this.tbpConfUst.TabIndex = 3;
            this.tbpConfUst.Text = "Конфигурация и уставки";
            this.tbpConfUst.Enter += new System.EventHandler(this.tbcConfig_Enter);
            // 
            // splitContainer12
            // 
            this.splitContainer12.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer12.Location = new System.Drawing.Point(3, 3);
            this.splitContainer12.Name = "splitContainer12";
            // 
            // splitContainer12.Panel1
            // 
            this.splitContainer12.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer12.Panel1.Controls.Add(this.lstvConfig);
            // 
            // splitContainer12.Panel2
            // 
            this.splitContainer12.Panel2.Controls.Add(this.tbkConfig);
            this.splitContainer12.Size = new System.Drawing.Size(994, 700);
            this.splitContainer12.SplitterDistance = 172;
            this.splitContainer12.TabIndex = 1;
            // 
            // lstvConfig
            // 
            this.lstvConfig.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstvConfig.BackColor = System.Drawing.Color.LightCyan;
            this.lstvConfig.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmnHDate,
            this.clmnhTime});
            this.lstvConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvConfig.FullRowSelect = true;
            this.lstvConfig.GridLines = true;
            this.lstvConfig.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvConfig.Location = new System.Drawing.Point(0, 0);
            this.lstvConfig.Name = "lstvConfig";
            this.lstvConfig.Size = new System.Drawing.Size(170, 698);
            this.lstvConfig.TabIndex = 0;
            this.lstvConfig.UseCompatibleStateImageBehavior = false;
            this.lstvConfig.View = System.Windows.Forms.View.Details;
            this.lstvConfig.ItemActivate += new System.EventHandler(this.lstvConfig_ItemActivate);
            // 
            // clmnHDate
            // 
            this.clmnHDate.Text = "Дата";
            this.clmnHDate.Width = 130;
            // 
            // clmnhTime
            // 
            this.clmnhTime.Text = "Комментарий";
            this.clmnhTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clmnhTime.Width = 90;
            // 
            // tbkConfig
            // 
            this.tbkConfig.Controls.Add(this.tbpUst_0);
            this.tbkConfig.Controls.Add(this.tbpUst_1);
            this.tbkConfig.Controls.Add(this.tbpUst_2);
            this.tbkConfig.Controls.Add(this.tbpUst_3);
            this.tbkConfig.Controls.Add(this.tbpUst_4);
            this.tbkConfig.Controls.Add(this.tbpUst_5);
            this.tbkConfig.Controls.Add(this.tbpUst_6);
            this.tbkConfig.Controls.Add(this.tbpUst_7);
            this.tbkConfig.Controls.Add(this.tbpUst_8);
            this.tbkConfig.Controls.Add(this.tbpUst_9);
            this.tbkConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbkConfig.Location = new System.Drawing.Point(0, 0);
            this.tbkConfig.Name = "tbkConfig";
            this.tbkConfig.SelectedIndex = 0;
            this.tbkConfig.Size = new System.Drawing.Size(816, 698);
            this.tbkConfig.TabIndex = 0;
            // 
            // tbpUst_0
            // 
            this.tbpUst_0.Controls.Add(this.Config_Ustavki_0);
            this.tbpUst_0.Location = new System.Drawing.Point(4, 22);
            this.tbpUst_0.Name = "tbpUst_0";
            this.tbpUst_0.Padding = new System.Windows.Forms.Padding(3);
            this.tbpUst_0.Size = new System.Drawing.Size(808, 672);
            this.tbpUst_0.TabIndex = 0;
            this.tbpUst_0.Text = "Программа 1";
            this.tbpUst_0.UseVisualStyleBackColor = true;
            // 
            // Config_Ustavki_0
            // 
            this.Config_Ustavki_0.AutoScroll = true;
            this.Config_Ustavki_0.BackColor = System.Drawing.SystemColors.Control;
            this.Config_Ustavki_0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Config_Ustavki_0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_Ustavki_0.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Config_Ustavki_0.Location = new System.Drawing.Point(3, 3);
            this.Config_Ustavki_0.Name = "Config_Ustavki_0";
            this.Config_Ustavki_0.Size = new System.Drawing.Size(802, 666);
            this.Config_Ustavki_0.TabIndex = 0;
            // 
            // tbpUst_1
            // 
            this.tbpUst_1.Controls.Add(this.Config_Ustavki_1);
            this.tbpUst_1.Location = new System.Drawing.Point(4, 22);
            this.tbpUst_1.Name = "tbpUst_1";
            this.tbpUst_1.Padding = new System.Windows.Forms.Padding(3);
            this.tbpUst_1.Size = new System.Drawing.Size(808, 672);
            this.tbpUst_1.TabIndex = 1;
            this.tbpUst_1.Text = "Программа 2";
            this.tbpUst_1.UseVisualStyleBackColor = true;
            // 
            // Config_Ustavki_1
            // 
            this.Config_Ustavki_1.AutoScroll = true;
            this.Config_Ustavki_1.BackColor = System.Drawing.SystemColors.Control;
            this.Config_Ustavki_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Config_Ustavki_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_Ustavki_1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Config_Ustavki_1.Location = new System.Drawing.Point(3, 3);
            this.Config_Ustavki_1.Name = "Config_Ustavki_1";
            this.Config_Ustavki_1.Size = new System.Drawing.Size(802, 666);
            this.Config_Ustavki_1.TabIndex = 0;
            // 
            // tbpUst_2
            // 
            this.tbpUst_2.Controls.Add(this.Config_Ustavki_2);
            this.tbpUst_2.Location = new System.Drawing.Point(4, 22);
            this.tbpUst_2.Name = "tbpUst_2";
            this.tbpUst_2.Padding = new System.Windows.Forms.Padding(3);
            this.tbpUst_2.Size = new System.Drawing.Size(808, 672);
            this.tbpUst_2.TabIndex = 2;
            this.tbpUst_2.Text = "АВР, РАВР и ВНР";
            this.tbpUst_2.UseVisualStyleBackColor = true;
            // 
            // Config_Ustavki_2
            // 
            this.Config_Ustavki_2.AutoScroll = true;
            this.Config_Ustavki_2.BackColor = System.Drawing.SystemColors.Control;
            this.Config_Ustavki_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_Ustavki_2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Config_Ustavki_2.Location = new System.Drawing.Point(3, 3);
            this.Config_Ustavki_2.Name = "Config_Ustavki_2";
            this.Config_Ustavki_2.Size = new System.Drawing.Size(802, 666);
            this.Config_Ustavki_2.TabIndex = 0;
            // 
            // tbpUst_3
            // 
            this.tbpUst_3.Controls.Add(this.Config_Ustavki_3);
            this.tbpUst_3.Location = new System.Drawing.Point(4, 22);
            this.tbpUst_3.Name = "tbpUst_3";
            this.tbpUst_3.Padding = new System.Windows.Forms.Padding(3);
            this.tbpUst_3.Size = new System.Drawing.Size(808, 672);
            this.tbpUst_3.TabIndex = 3;
            this.tbpUst_3.Text = "Автоматика, общие";
            this.tbpUst_3.UseVisualStyleBackColor = true;
            // 
            // Config_Ustavki_3
            // 
            this.Config_Ustavki_3.AutoScroll = true;
            this.Config_Ustavki_3.BackColor = System.Drawing.SystemColors.Control;
            this.Config_Ustavki_3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_Ustavki_3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Config_Ustavki_3.Location = new System.Drawing.Point(3, 3);
            this.Config_Ustavki_3.Name = "Config_Ustavki_3";
            this.Config_Ustavki_3.Size = new System.Drawing.Size(802, 666);
            this.Config_Ustavki_3.TabIndex = 0;
            // 
            // tbpUst_4
            // 
            this.tbpUst_4.Controls.Add(this.Config_Ustavki_4);
            this.tbpUst_4.Location = new System.Drawing.Point(4, 22);
            this.tbpUst_4.Name = "tbpUst_4";
            this.tbpUst_4.Padding = new System.Windows.Forms.Padding(3);
            this.tbpUst_4.Size = new System.Drawing.Size(808, 672);
            this.tbpUst_4.TabIndex = 4;
            this.tbpUst_4.Text = "Входы, Выходы";
            this.tbpUst_4.UseVisualStyleBackColor = true;
            // 
            // Config_Ustavki_4
            // 
            this.Config_Ustavki_4.AutoScroll = true;
            this.Config_Ustavki_4.BackColor = System.Drawing.SystemColors.Control;
            this.Config_Ustavki_4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_Ustavki_4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Config_Ustavki_4.Location = new System.Drawing.Point(3, 3);
            this.Config_Ustavki_4.Name = "Config_Ustavki_4";
            this.Config_Ustavki_4.Size = new System.Drawing.Size(802, 666);
            this.Config_Ustavki_4.TabIndex = 0;
            // 
            // tbpUst_5
            // 
            this.tbpUst_5.Controls.Add(this.Config_Ustavki_5);
            this.tbpUst_5.Location = new System.Drawing.Point(4, 22);
            this.tbpUst_5.Name = "tbpUst_5";
            this.tbpUst_5.Padding = new System.Windows.Forms.Padding(3);
            this.tbpUst_5.Size = new System.Drawing.Size(808, 672);
            this.tbpUst_5.TabIndex = 5;
            this.tbpUst_5.Text = "Вкладка 6";
            this.tbpUst_5.UseVisualStyleBackColor = true;
            // 
            // Config_Ustavki_5
            // 
            this.Config_Ustavki_5.AutoScroll = true;
            this.Config_Ustavki_5.BackColor = System.Drawing.SystemColors.Control;
            this.Config_Ustavki_5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_Ustavki_5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Config_Ustavki_5.Location = new System.Drawing.Point(3, 3);
            this.Config_Ustavki_5.Name = "Config_Ustavki_5";
            this.Config_Ustavki_5.Size = new System.Drawing.Size(802, 666);
            this.Config_Ustavki_5.TabIndex = 0;
            // 
            // tbpUst_6
            // 
            this.tbpUst_6.Controls.Add(this.Config_Ustavki_6);
            this.tbpUst_6.Location = new System.Drawing.Point(4, 22);
            this.tbpUst_6.Name = "tbpUst_6";
            this.tbpUst_6.Size = new System.Drawing.Size(808, 672);
            this.tbpUst_6.TabIndex = 6;
            this.tbpUst_6.Text = "Вкладка 7";
            this.tbpUst_6.UseVisualStyleBackColor = true;
            // 
            // Config_Ustavki_6
            // 
            this.Config_Ustavki_6.AutoScroll = true;
            this.Config_Ustavki_6.BackColor = System.Drawing.SystemColors.Control;
            this.Config_Ustavki_6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_Ustavki_6.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Config_Ustavki_6.Location = new System.Drawing.Point(0, 0);
            this.Config_Ustavki_6.Name = "Config_Ustavki_6";
            this.Config_Ustavki_6.Size = new System.Drawing.Size(808, 672);
            this.Config_Ustavki_6.TabIndex = 1;
            // 
            // tbpUst_7
            // 
            this.tbpUst_7.Controls.Add(this.Config_Ustavki_7);
            this.tbpUst_7.Location = new System.Drawing.Point(4, 22);
            this.tbpUst_7.Name = "tbpUst_7";
            this.tbpUst_7.Size = new System.Drawing.Size(808, 672);
            this.tbpUst_7.TabIndex = 7;
            this.tbpUst_7.Text = "Вкладка 8";
            this.tbpUst_7.UseVisualStyleBackColor = true;
            // 
            // Config_Ustavki_7
            // 
            this.Config_Ustavki_7.AutoScroll = true;
            this.Config_Ustavki_7.BackColor = System.Drawing.SystemColors.Control;
            this.Config_Ustavki_7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_Ustavki_7.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Config_Ustavki_7.Location = new System.Drawing.Point(0, 0);
            this.Config_Ustavki_7.Name = "Config_Ustavki_7";
            this.Config_Ustavki_7.Size = new System.Drawing.Size(808, 672);
            this.Config_Ustavki_7.TabIndex = 2;
            // 
            // tbpUst_8
            // 
            this.tbpUst_8.Controls.Add(this.Config_Ustavki_8);
            this.tbpUst_8.Location = new System.Drawing.Point(4, 22);
            this.tbpUst_8.Name = "tbpUst_8";
            this.tbpUst_8.Size = new System.Drawing.Size(808, 672);
            this.tbpUst_8.TabIndex = 8;
            this.tbpUst_8.Text = "Вкладка 9";
            this.tbpUst_8.UseVisualStyleBackColor = true;
            // 
            // Config_Ustavki_8
            // 
            this.Config_Ustavki_8.AutoScroll = true;
            this.Config_Ustavki_8.BackColor = System.Drawing.SystemColors.Control;
            this.Config_Ustavki_8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_Ustavki_8.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Config_Ustavki_8.Location = new System.Drawing.Point(0, 0);
            this.Config_Ustavki_8.Name = "Config_Ustavki_8";
            this.Config_Ustavki_8.Size = new System.Drawing.Size(808, 672);
            this.Config_Ustavki_8.TabIndex = 2;
            // 
            // tbpUst_9
            // 
            this.tbpUst_9.Controls.Add(this.Config_Ustavki_9);
            this.tbpUst_9.Location = new System.Drawing.Point(4, 22);
            this.tbpUst_9.Name = "tbpUst_9";
            this.tbpUst_9.Size = new System.Drawing.Size(808, 672);
            this.tbpUst_9.TabIndex = 9;
            this.tbpUst_9.Text = "Вкладка 10";
            this.tbpUst_9.UseVisualStyleBackColor = true;
            // 
            // Config_Ustavki_9
            // 
            this.Config_Ustavki_9.AutoScroll = true;
            this.Config_Ustavki_9.BackColor = System.Drawing.SystemColors.Control;
            this.Config_Ustavki_9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Config_Ustavki_9.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Config_Ustavki_9.Location = new System.Drawing.Point(0, 0);
            this.Config_Ustavki_9.Name = "Config_Ustavki_9";
            this.Config_Ustavki_9.Size = new System.Drawing.Size(808, 672);
            this.Config_Ustavki_9.TabIndex = 2;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage5.Controls.Add(this.splitContainer_OscDiag);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1000, 706);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Осциллограммы и диаграммы";
            this.tabPage5.Enter += new System.EventHandler(this.tabPage5_Enter);
            // 
            // splitContainer_OscDiag
            // 
            this.splitContainer_OscDiag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_OscDiag.Location = new System.Drawing.Point(3, 3);
            this.splitContainer_OscDiag.Name = "splitContainer_OscDiag";
            // 
            // splitContainer_OscDiag.Panel1
            // 
            this.splitContainer_OscDiag.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer_OscDiag.Panel1.Controls.Add(this.btnUnionOsc);
            this.splitContainer_OscDiag.Panel1.Controls.Add(this.dgvOscill);
            this.splitContainer_OscDiag.Panel1.Controls.Add(this.button2);
            // 
            // splitContainer_OscDiag.Panel2
            // 
            this.splitContainer_OscDiag.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer_OscDiag.Panel2.Controls.Add(this.btnUnionDiag);
            this.splitContainer_OscDiag.Panel2.Controls.Add(this.dgvDiag);
            this.splitContainer_OscDiag.Panel2.Controls.Add(this.button3);
            this.splitContainer_OscDiag.Size = new System.Drawing.Size(994, 700);
            this.splitContainer_OscDiag.SplitterDistance = 524;
            this.splitContainer_OscDiag.TabIndex = 0;
            // 
            // btnUnionOsc
            // 
            this.btnUnionOsc.AutoSize = true;
            this.btnUnionOsc.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnUnionOsc.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnUnionOsc.Enabled = false;
            this.btnUnionOsc.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUnionOsc.Location = new System.Drawing.Point(0, 671);
            this.btnUnionOsc.Name = "btnUnionOsc";
            this.btnUnionOsc.Size = new System.Drawing.Size(524, 29);
            this.btnUnionOsc.TabIndex = 2;
            this.btnUnionOsc.Text = "Объединить осциллограммы";
            this.btnUnionOsc.UseVisualStyleBackColor = false;
            this.btnUnionOsc.Visible = false;
            // 
            // dgvOscill
            // 
            this.dgvOscill.AllowUserToAddRows = false;
            this.dgvOscill.AllowUserToDeleteRows = false;
            this.dgvOscill.AllowUserToResizeRows = false;
            this.dgvOscill.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOscill.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvOscill.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOscill.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmChBoxOsc,
            this.clmBlockNameOsc,
            this.clmBlockIdOsc,
            this.clmCommentOsc,
            this.clmBlockTimeOsc,
            this.clmViewOsc,
            this.clmID});
            this.dgvOscill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOscill.Location = new System.Drawing.Point(0, 35);
            this.dgvOscill.MultiSelect = false;
            this.dgvOscill.Name = "dgvOscill";
            this.dgvOscill.ReadOnly = true;
            this.dgvOscill.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOscill.Size = new System.Drawing.Size(524, 665);
            this.dgvOscill.TabIndex = 1;
            // 
            // clmChBoxOsc
            // 
            this.clmChBoxOsc.HeaderText = "";
            this.clmChBoxOsc.Name = "clmChBoxOsc";
            this.clmChBoxOsc.ReadOnly = true;
            // 
            // clmBlockNameOsc
            // 
            this.clmBlockNameOsc.HeaderText = "Имя блока";
            this.clmBlockNameOsc.Name = "clmBlockNameOsc";
            this.clmBlockNameOsc.ReadOnly = true;
            // 
            // clmBlockIdOsc
            // 
            this.clmBlockIdOsc.HeaderText = "Идентификатор блока";
            this.clmBlockIdOsc.Name = "clmBlockIdOsc";
            this.clmBlockIdOsc.ReadOnly = true;
            this.clmBlockIdOsc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clmCommentOsc
            // 
            this.clmCommentOsc.HeaderText = "Комментарий";
            this.clmCommentOsc.Name = "clmCommentOsc";
            this.clmCommentOsc.ReadOnly = true;
            this.clmCommentOsc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clmBlockTimeOsc
            // 
            this.clmBlockTimeOsc.HeaderText = "Время блока";
            this.clmBlockTimeOsc.Name = "clmBlockTimeOsc";
            this.clmBlockTimeOsc.ReadOnly = true;
            // 
            // clmViewOsc
            // 
            this.clmViewOsc.HeaderText = "Просмотр";
            this.clmViewOsc.Name = "clmViewOsc";
            this.clmViewOsc.ReadOnly = true;
            this.clmViewOsc.Text = "Просмотр";
            // 
            // clmID
            // 
            this.clmID.HeaderText = "Идентификатор";
            this.clmID.Name = "clmID";
            this.clmID.ReadOnly = true;
            this.clmID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmID.Visible = false;
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Enabled = false;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(524, 35);
            this.button2.TabIndex = 0;
            this.button2.Text = "Осциллограммы";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // btnUnionDiag
            // 
            this.btnUnionDiag.AutoSize = true;
            this.btnUnionDiag.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnUnionDiag.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnUnionDiag.Enabled = false;
            this.btnUnionDiag.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUnionDiag.Location = new System.Drawing.Point(0, 671);
            this.btnUnionDiag.Name = "btnUnionDiag";
            this.btnUnionDiag.Size = new System.Drawing.Size(466, 29);
            this.btnUnionDiag.TabIndex = 3;
            this.btnUnionDiag.Text = "Объединить диаграммы";
            this.btnUnionDiag.UseVisualStyleBackColor = false;
            this.btnUnionDiag.Visible = false;
            // 
            // dgvDiag
            // 
            this.dgvDiag.AllowUserToAddRows = false;
            this.dgvDiag.AllowUserToDeleteRows = false;
            this.dgvDiag.AllowUserToResizeRows = false;
            this.dgvDiag.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDiag.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvDiag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiag.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmChBoxDiag,
            this.clmBlockNameDiag,
            this.clmBlockIdDiag,
            this.clmCommentDiag,
            this.clmBlockTimeDiag,
            this.clmViewDiag,
            this.clmIDDiag});
            this.dgvDiag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDiag.Location = new System.Drawing.Point(0, 35);
            this.dgvDiag.MultiSelect = false;
            this.dgvDiag.Name = "dgvDiag";
            this.dgvDiag.ReadOnly = true;
            this.dgvDiag.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDiag.Size = new System.Drawing.Size(466, 665);
            this.dgvDiag.TabIndex = 2;
            this.dgvDiag.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiag_CellContentClick);
            // 
            // clmChBoxDiag
            // 
            this.clmChBoxDiag.HeaderText = "";
            this.clmChBoxDiag.Name = "clmChBoxDiag";
            this.clmChBoxDiag.ReadOnly = true;
            this.clmChBoxDiag.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clmChBoxDiag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // clmBlockNameDiag
            // 
            this.clmBlockNameDiag.HeaderText = "Имя блока";
            this.clmBlockNameDiag.Name = "clmBlockNameDiag";
            this.clmBlockNameDiag.ReadOnly = true;
            // 
            // clmBlockIdDiag
            // 
            this.clmBlockIdDiag.HeaderText = "Идентификатор блока";
            this.clmBlockIdDiag.Name = "clmBlockIdDiag";
            this.clmBlockIdDiag.ReadOnly = true;
            this.clmBlockIdDiag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clmCommentDiag
            // 
            this.clmCommentDiag.HeaderText = "Комментарий";
            this.clmCommentDiag.Name = "clmCommentDiag";
            this.clmCommentDiag.ReadOnly = true;
            this.clmCommentDiag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clmBlockTimeDiag
            // 
            this.clmBlockTimeDiag.HeaderText = "Время блока";
            this.clmBlockTimeDiag.Name = "clmBlockTimeDiag";
            this.clmBlockTimeDiag.ReadOnly = true;
            // 
            // clmViewDiag
            // 
            this.clmViewDiag.HeaderText = "Просмотр";
            this.clmViewDiag.Name = "clmViewDiag";
            this.clmViewDiag.ReadOnly = true;
            this.clmViewDiag.Text = "Просмотр";
            // 
            // clmIDDiag
            // 
            this.clmIDDiag.HeaderText = "Идентификатор";
            this.clmIDDiag.Name = "clmIDDiag";
            this.clmIDDiag.ReadOnly = true;
            this.clmIDDiag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmIDDiag.Visible = false;
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button3.Dock = System.Windows.Forms.DockStyle.Top;
            this.button3.Enabled = false;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(0, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(466, 35);
            this.button3.TabIndex = 1;
            this.button3.Text = "Диаграммы";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // tabSystem
            // 
            this.tabSystem.BackColor = System.Drawing.SystemColors.Control;
            this.tabSystem.Controls.Add(this.splitContainer14);
            this.tabSystem.Location = new System.Drawing.Point(4, 22);
            this.tabSystem.Name = "tabSystem";
            this.tabSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tabSystem.Size = new System.Drawing.Size(1000, 706);
            this.tabSystem.TabIndex = 5;
            this.tabSystem.Text = "Система";
            this.tabSystem.Enter += new System.EventHandler(this.tabSystem_Enter);
            // 
            // splitContainer14
            // 
            this.splitContainer14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer14.Location = new System.Drawing.Point(3, 3);
            this.splitContainer14.Name = "splitContainer14";
            // 
            // splitContainer14.Panel1
            // 
            this.splitContainer14.Panel1.Controls.Add(this.gbTest);
            this.splitContainer14.Panel2Collapsed = true;
            this.splitContainer14.Size = new System.Drawing.Size(994, 700);
            this.splitContainer14.SplitterDistance = 576;
            this.splitContainer14.TabIndex = 2;
            // 
            // gbTest
            // 
            this.gbTest.Controls.Add(this.System_Test);
            this.gbTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTest.Location = new System.Drawing.Point(0, 0);
            this.gbTest.Name = "gbTest";
            this.gbTest.Size = new System.Drawing.Size(994, 700);
            this.gbTest.TabIndex = 2;
            this.gbTest.TabStop = false;
            this.gbTest.Text = "Результаты тестирования БМРЗ";
            // 
            // System_Test
            // 
            this.System_Test.AutoScroll = true;
            this.System_Test.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.System_Test.Dock = System.Windows.Forms.DockStyle.Fill;
            this.System_Test.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.System_Test.Location = new System.Drawing.Point(3, 16);
            this.System_Test.Name = "System_Test";
            this.System_Test.Size = new System.Drawing.Size(988, 681);
            this.System_Test.TabIndex = 0;
            // 
            // tabpageEventBlock
            // 
            this.tabpageEventBlock.BackColor = System.Drawing.SystemColors.Control;
            this.tabpageEventBlock.Controls.Add(this.lstvEventBlock);
            this.tabpageEventBlock.Location = new System.Drawing.Point(4, 22);
            this.tabpageEventBlock.Name = "tabpageEventBlock";
            this.tabpageEventBlock.Size = new System.Drawing.Size(1000, 706);
            this.tabpageEventBlock.TabIndex = 17;
            this.tabpageEventBlock.Text = "События блока";
            // 
            // lstvEventBlock
            // 
            this.lstvEventBlock.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.chTime_logSystemFull,
            this.chText_logSystemFull});
            this.lstvEventBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvEventBlock.GridLines = true;
            this.lstvEventBlock.Location = new System.Drawing.Point(0, 0);
            this.lstvEventBlock.Name = "lstvEventBlock";
            this.lstvEventBlock.Size = new System.Drawing.Size(1000, 706);
            this.lstvEventBlock.TabIndex = 1;
            this.lstvEventBlock.UseCompatibleStateImageBehavior = false;
            this.lstvEventBlock.View = System.Windows.Forms.View.Details;
            this.lstvEventBlock.Enter += new System.EventHandler(this.tabpageEventBlock_Enter);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Width = 1;
            // 
            // chTime_logSystemFull
            // 
            this.chTime_logSystemFull.Text = "Время";
            this.chTime_logSystemFull.Width = 150;
            // 
            // chText_logSystemFull
            // 
            this.chText_logSystemFull.Text = "Текст";
            this.chText_logSystemFull.Width = 1400;
            // 
            // tabPageInfo
            // 
            this.tabPageInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageInfo.Controls.Add(this.tableLayoutPanel1);
            this.tabPageInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageInfo.Name = "tabPageInfo";
            this.tabPageInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInfo.Size = new System.Drawing.Size(1000, 706);
            this.tabPageInfo.TabIndex = 18;
            this.tabPageInfo.Text = "Информация";
            this.tabPageInfo.Enter += new System.EventHandler(this.tabPageInfo_Enter);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(994, 700);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.PanelInfoTextBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rtbInfo, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(244, 694);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // PanelInfoTextBox
            // 
            this.PanelInfoTextBox.BackColor = System.Drawing.Color.LightSalmon;
            this.PanelInfoTextBox.Location = new System.Drawing.Point(3, 3);
            this.PanelInfoTextBox.Name = "PanelInfoTextBox";
            this.PanelInfoTextBox.ReadOnly = true;
            this.PanelInfoTextBox.Size = new System.Drawing.Size(238, 20);
            this.PanelInfoTextBox.TabIndex = 4;
            this.PanelInfoTextBox.Text = "Время последнего изменения:";
            // 
            // rtbInfo
            // 
            this.rtbInfo.BackColor = System.Drawing.Color.White;
            this.rtbInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbInfo.Location = new System.Drawing.Point(3, 29);
            this.rtbInfo.Name = "rtbInfo";
            this.rtbInfo.ReadOnly = true;
            this.rtbInfo.Size = new System.Drawing.Size(238, 662);
            this.rtbInfo.TabIndex = 6;
            this.rtbInfo.Text = "";
            // 
            // frmBMRZ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 732);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pnlConfig);
            this.Controls.Add(this.pnlStatusSHASU);
            this.Controls.Add(this.pnlStore);
            this.Controls.Add(this.pnlSystem);
            this.Controls.Add(this.pnlCurrent);
            this.Controls.Add(this.pnlOscDiag);
            this.Controls.Add(this.pnlAvar);
            this.MaximumSize = new System.Drawing.Size(1024, 768);
            this.MinimumSize = new System.Drawing.Size(1024, 766);
            this.Name = "frmBMRZ";
            this.Text = "БМРЗ-ВВ-14_31_12";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmBMRZVV14_31_12_Load);
            this.pnlCurrent.ResumeLayout(false);
            this.tcCurrentBottomPanel.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.gbRegStatus.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tabPage10.PerformLayout();
            this.gbControlProgUst.ResumeLayout(false);
            this.gbControlProgUst.PerformLayout();
            this.tabPage11.ResumeLayout(false);
            this.tabPage11.PerformLayout();
            this.gbCounters.ResumeLayout(false);
            this.gbCounters.PerformLayout();
            this.tabPage12.ResumeLayout(false);
            this.tabPage12.PerformLayout();
            this.gbDirection_P.ResumeLayout(false);
            this.gbDirection_P.PerformLayout();
            this.pnlAvar.ResumeLayout(false);
            this.tcAvarBottomPanel.ResumeLayout(false);
            this.tabPage14.ResumeLayout(false);
            this.tabPage13.ResumeLayout(false);
            this.grbDTStart.ResumeLayout(false);
            this.grbDTStart.PerformLayout();
            this.grbDTFin.ResumeLayout(false);
            this.grbDTFin.PerformLayout();
            this.pnlSystem.ResumeLayout(false);
            this.pnlStore.ResumeLayout(false);
            this.tcStoreBottomPanel.ResumeLayout(false);
            this.tabPage15.ResumeLayout(false);
            this.tabPage16.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.pnlConfig.ResumeLayout(false);
            this.tcUstConfigBottomPanel.ResumeLayout(false);
            this.tabPage17.ResumeLayout(false);
            this.tabPage19.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.tabPage18.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.pnlOscDiag.ResumeLayout(false);
            this.gbEndTime.ResumeLayout(false);
            this.gbStartTime.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlStatusSHASU.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.splitContainer13.Panel1.ResumeLayout(false);
            this.splitContainer13.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer13)).EndInit();
            this.splitContainer13.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer15.Panel1.ResumeLayout(false);
            this.splitContainer15.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer15)).EndInit();
            this.splitContainer15.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.gbVizov.ResumeLayout(false);
            this.tbpAvar.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer16.Panel1.ResumeLayout(false);
            this.splitContainer16.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer16)).EndInit();
            this.splitContainer16.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.groupBox16.ResumeLayout(false);
            this.groupBox17.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.groupBox18.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            this.groupBox19.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).EndInit();
            this.splitContainer9.ResumeLayout(false);
            this.tabStore.ResumeLayout(false);
            this.splitContainer10.Panel1.ResumeLayout(false);
            this.splitContainer10.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer10)).EndInit();
            this.splitContainer10.ResumeLayout(false);
            this.splitContainer11.Panel1.ResumeLayout(false);
            this.splitContainer11.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).EndInit();
            this.splitContainer11.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.splitContainer17.Panel1.ResumeLayout(false);
            this.splitContainer17.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer17)).EndInit();
            this.splitContainer17.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox20.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tbpConfUst.ResumeLayout(false);
            this.splitContainer12.Panel1.ResumeLayout(false);
            this.splitContainer12.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer12)).EndInit();
            this.splitContainer12.ResumeLayout(false);
            this.tbkConfig.ResumeLayout(false);
            this.tbpUst_0.ResumeLayout(false);
            this.tbpUst_1.ResumeLayout(false);
            this.tbpUst_2.ResumeLayout(false);
            this.tbpUst_3.ResumeLayout(false);
            this.tbpUst_4.ResumeLayout(false);
            this.tbpUst_5.ResumeLayout(false);
            this.tbpUst_6.ResumeLayout(false);
            this.tbpUst_7.ResumeLayout(false);
            this.tbpUst_8.ResumeLayout(false);
            this.tbpUst_9.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.splitContainer_OscDiag.Panel1.ResumeLayout(false);
            this.splitContainer_OscDiag.Panel1.PerformLayout();
            this.splitContainer_OscDiag.Panel2.ResumeLayout(false);
            this.splitContainer_OscDiag.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_OscDiag)).EndInit();
            this.splitContainer_OscDiag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOscill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiag)).EndInit();
            this.tabSystem.ResumeLayout(false);
            this.splitContainer14.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer14)).EndInit();
            this.splitContainer14.ResumeLayout(false);
            this.gbTest.ResumeLayout(false);
            this.tabpageEventBlock.ResumeLayout(false);
            this.tabPageInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Panel pnlCurrent;
        private System.Windows.Forms.Panel pnlAvar;
        private System.Windows.Forms.Panel pnlSystem;
        private System.Windows.Forms.Panel pnlStore;
        private System.Windows.Forms.Panel pnlConfig;
        private System.Windows.Forms.Panel pnlOscDiag;
        private System.Windows.Forms.GroupBox gbEndTime;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.DateTimePicker dtpEndData;
        private System.Windows.Forms.GroupBox gbStartTime;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.DateTimePicker dtpStartData;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem системаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuPageSetup;
        private System.Windows.Forms.ToolStripMenuItem mnuPrintPreview;
        private System.Windows.Forms.ToolStripMenuItem mnuPrint;
        private System.Windows.Forms.Button btnReNewOD;
		private System.Windows.Forms.Panel pnlStatusSHASU;
		private System.Windows.Forms.GroupBox groupBox12;
		private System.Windows.Forms.TextBox tbIntervalReadMaxMeter;
		private System.Windows.Forms.Label lblIntervalReadMaxM2;
		private System.Windows.Forms.Label lblIntervalReadMaxM1;
		private System.Windows.Forms.CheckBox cbPeriodReadMaxMeter;
		private System.Windows.Forms.GroupBox groupBox13;
		private System.Windows.Forms.Label lblIntervalReadStore2;
		private System.Windows.Forms.TextBox tbIntervalReadStore;
		private System.Windows.Forms.Label lblIntervalReadStore1;
		private System.Windows.Forms.CheckBox cbPeriodReadStore;
		private LabelTextbox.ctlLabelTextbox ctlLabelTextbox1;
      private System.Windows.Forms.FlowLayoutPanel System_BottomPanel;
      public System.Windows.Forms.Button btnReadResurs;
      private System.Windows.Forms.TabControl tcCurrentBottomPanel;
      private System.Windows.Forms.TabPage tabPage4;
      private System.Windows.Forms.TabPage tabPage10;
      private System.Windows.Forms.GroupBox gbControlProgUst;
      private System.Windows.Forms.FlowLayoutPanel CurrentControlProgUst;
      private System.Windows.Forms.TabPage tabPage11;
      private System.Windows.Forms.GroupBox gbCounters;
      private System.Windows.Forms.FlowLayoutPanel CurrentCounters;
      private System.Windows.Forms.TabPage tabPage12;
      private System.Windows.Forms.GroupBox gbDirection_P;
      private System.Windows.Forms.FlowLayoutPanel CurrentDirection_P;
      private System.Windows.Forms.GroupBox gbRegStatus;
      private System.Windows.Forms.FlowLayoutPanel CurrentStatusReg;
      private System.Windows.Forms.TabControl tcAvarBottomPanel;
      private System.Windows.Forms.TabPage tabPage13;
      private System.Windows.Forms.Button btnReNew;
      private System.Windows.Forms.GroupBox grbDTStart;
      private System.Windows.Forms.DateTimePicker dtpStartTimeAvar;
      private System.Windows.Forms.DateTimePicker dtpStartDateAvar;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.GroupBox grbDTFin;
      private System.Windows.Forms.DateTimePicker dtpEndTimeAvar;
      private System.Windows.Forms.DateTimePicker dtpEndDateAvar;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TabPage tabPage14;
      private System.Windows.Forms.FlowLayoutPanel Avar_BottomPanel;
      private System.Windows.Forms.TabControl tcStoreBottomPanel;
      private System.Windows.Forms.TabPage tabPage15;
      private System.Windows.Forms.FlowLayoutPanel Store_BottomPanel;
      private System.Windows.Forms.TabPage tabPage16;
      private System.Windows.Forms.GroupBox groupBox7;
      private System.Windows.Forms.Button btnReadMaxMeterBlock;
      private System.Windows.Forms.Button btnResetMaxMeter;
      private System.Windows.Forms.Button btnReadMaxMeterFC;
      private System.Windows.Forms.GroupBox groupBox6;
      private System.Windows.Forms.Button btnReadStoreBlock;
      private System.Windows.Forms.Button btnResetStore;
      private System.Windows.Forms.Button btnReadStoreFC;
      private System.Windows.Forms.TabControl tcUstConfigBottomPanel;
      private System.Windows.Forms.TabPage tabPage17;
      private System.Windows.Forms.FlowLayoutPanel Config_BottomPanel;
      private System.Windows.Forms.TabPage tabPage18;
      private System.Windows.Forms.GroupBox groupBox9;
      private System.Windows.Forms.Button btnWriteUst;
      private System.Windows.Forms.Button btnReadUstFC;
      private System.Windows.Forms.Button btnResetValues;
      private System.Windows.Forms.TabPage tabPage19;
      private System.Windows.Forms.GroupBox groupBox10;
      private System.Windows.Forms.DateTimePicker dtpStartTimeConfig;
      private System.Windows.Forms.DateTimePicker dtpStartDateConfig;
      private System.Windows.Forms.GroupBox groupBox11;
      private System.Windows.Forms.DateTimePicker dtpEndTimeConfig;
      private System.Windows.Forms.DateTimePicker dtpEndDateConfig;
      private System.Windows.Forms.Button btnReNewUstBD;
      private System.Windows.Forms.ToolTip toolTip1;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.TabControl tabControl1;
      private System.Windows.Forms.TabPage tabPage1;
      private System.Windows.Forms.SplitContainer splitContainer3;
      private System.Windows.Forms.TabControl tabControl3;
      private System.Windows.Forms.TabPage tabPage3;
      private System.Windows.Forms.FlowLayoutPanel Current_Analog_First;
      private System.Windows.Forms.TabPage tabPage9;
      private System.Windows.Forms.FlowLayoutPanel Current_Analog_Second;
      private System.Windows.Forms.SplitContainer splitContainer13;
      private System.Windows.Forms.FlowLayoutPanel Current_DiscretIn;
      private System.Windows.Forms.FlowLayoutPanel Current_DiscretOut;
      public System.Windows.Forms.TabPage tbpAvar;
      private System.Windows.Forms.SplitContainer splitContainer2;
      private System.Windows.Forms.ListView lstvAvar;
      private System.Windows.Forms.ColumnHeader clmnData;
      private System.Windows.Forms.ColumnHeader clmnTime;
      public System.Windows.Forms.TabControl tabControl2;
      private System.Windows.Forms.TabPage tabPage2;
      private System.Windows.Forms.SplitContainer splitContainer4;
      private System.Windows.Forms.SplitContainer splitContainer5;
      private System.Windows.Forms.GroupBox groupBox14;
      private System.Windows.Forms.FlowLayoutPanel Avar_AS_PPZ;
      private System.Windows.Forms.GroupBox groupBox15;
      private System.Windows.Forms.FlowLayoutPanel Avar_AS_PSZ;
      private System.Windows.Forms.SplitContainer splitContainer6;
      private System.Windows.Forms.GroupBox groupBox16;
      private System.Windows.Forms.FlowLayoutPanel Avar_AS_AsPZ;
      private System.Windows.Forms.GroupBox groupBox17;
      private System.Windows.Forms.FlowLayoutPanel Avar_AS_AsSZ;
      private System.Windows.Forms.TabPage tabPage7;
      private System.Windows.Forms.SplitContainer splitContainer7;
      private System.Windows.Forms.GroupBox groupBox18;
      private System.Windows.Forms.SplitContainer splitContainer8;
      private System.Windows.Forms.FlowLayoutPanel Avar_DS_In;
      private System.Windows.Forms.FlowLayoutPanel Avar_DS_InChange;
      private System.Windows.Forms.GroupBox groupBox19;
      private System.Windows.Forms.SplitContainer splitContainer9;
      private System.Windows.Forms.FlowLayoutPanel Avar_DS_Out;
      private System.Windows.Forms.FlowLayoutPanel Avar_DS_OutChange;
      private System.Windows.Forms.TabPage tabStore;
      private System.Windows.Forms.TabPage tbpConfUst;
      private System.Windows.Forms.SplitContainer splitContainer12;
      private System.Windows.Forms.ListView lstvConfig;
      private System.Windows.Forms.ColumnHeader clmnHDate;
      private System.Windows.Forms.ColumnHeader clmnhTime;
      private System.Windows.Forms.TabControl tbkConfig;
      private System.Windows.Forms.TabPage tbpUst_0;
      private System.Windows.Forms.FlowLayoutPanel Config_Ustavki_0;
      private System.Windows.Forms.TabPage tbpUst_1;
      private System.Windows.Forms.FlowLayoutPanel Config_Ustavki_1;
      private System.Windows.Forms.TabPage tbpUst_2;
      private System.Windows.Forms.FlowLayoutPanel Config_Ustavki_2;
      private System.Windows.Forms.TabPage tbpUst_3;
      private System.Windows.Forms.FlowLayoutPanel Config_Ustavki_3;
      private System.Windows.Forms.TabPage tbpUst_4;
      private System.Windows.Forms.FlowLayoutPanel Config_Ustavki_4;
      private System.Windows.Forms.TabPage tbpUst_5;
      private System.Windows.Forms.FlowLayoutPanel Config_Ustavki_5;
      private System.Windows.Forms.TabPage tbpUst_6;
      private System.Windows.Forms.FlowLayoutPanel Config_Ustavki_6;
      private System.Windows.Forms.TabPage tbpUst_7;
      private System.Windows.Forms.FlowLayoutPanel Config_Ustavki_7;
      private System.Windows.Forms.TabPage tbpUst_8;
      private System.Windows.Forms.FlowLayoutPanel Config_Ustavki_8;
      private System.Windows.Forms.TabPage tbpUst_9;
      private System.Windows.Forms.FlowLayoutPanel Config_Ustavki_9;
      private System.Windows.Forms.TabPage tabPage5;
      private System.Windows.Forms.SplitContainer splitContainer_OscDiag;
      private System.Windows.Forms.Button btnUnionOsc;
      private System.Windows.Forms.DataGridView dgvOscill;
      private System.Windows.Forms.DataGridViewCheckBoxColumn clmChBoxOsc;
      private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockNameOsc;
      private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockIdOsc;
      private System.Windows.Forms.DataGridViewTextBoxColumn clmCommentOsc;
      private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockTimeOsc;
      private System.Windows.Forms.DataGridViewButtonColumn clmViewOsc;
      private System.Windows.Forms.DataGridViewTextBoxColumn clmID;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.Button btnUnionDiag;
      private System.Windows.Forms.DataGridView dgvDiag;
      private System.Windows.Forms.DataGridViewCheckBoxColumn clmChBoxDiag;
      private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockNameDiag;
      private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockIdDiag;
      private System.Windows.Forms.DataGridViewTextBoxColumn clmCommentDiag;
      private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockTimeDiag;
      private System.Windows.Forms.DataGridViewButtonColumn clmViewDiag;
      private System.Windows.Forms.DataGridViewTextBoxColumn clmIDDiag;
      private System.Windows.Forms.Button button3;
      private System.Windows.Forms.TabPage tabSystem;
      private System.Windows.Forms.SplitContainer splitContainer14;
      private System.Windows.Forms.GroupBox gbTest;
      private System.Windows.Forms.FlowLayoutPanel System_Test;
      private System.Windows.Forms.SplitContainer splitContainer15;
      private System.Windows.Forms.GroupBox gbVizov;
      private System.Windows.Forms.FlowLayoutPanel System_Vizov_vkl;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.SplitContainer splitContainer16;
      private System.Windows.Forms.GroupBox groupBox8;
      private System.Windows.Forms.FlowLayoutPanel Avar_MaxMin;
      private System.Windows.Forms.SplitContainer splitContainer10;
      private System.Windows.Forms.SplitContainer splitContainer11;
      private System.Windows.Forms.GroupBox groupBox4;
      private System.Windows.Forms.FlowLayoutPanel Store_I_IntegralOtkl;
      private System.Windows.Forms.SplitContainer splitContainer17;
      private System.Windows.Forms.GroupBox groupBox5;
      private System.Windows.Forms.FlowLayoutPanel Store_I_lastOtkl;
      private System.Windows.Forms.GroupBox groupBox20;
      private System.Windows.Forms.FlowLayoutPanel Store_Maxmeter;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.FlowLayoutPanel Store_CountEvent;
      private System.Windows.Forms.CheckBox btnFix4Change;
      private System.Windows.Forms.TabPage tabPageInfo;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private System.Windows.Forms.TextBox PanelInfoTextBox;
      private System.Windows.Forms.RichTextBox rtbInfo;
      private System.Windows.Forms.TabPage tabpageEventBlock;
      private System.Windows.Forms.ListView lstvEventBlock;
      private System.Windows.Forms.ColumnHeader columnHeader3;
      private System.Windows.Forms.ColumnHeader chTime_logSystemFull;
      private System.Windows.Forms.ColumnHeader chText_logSystemFull;
	}
}