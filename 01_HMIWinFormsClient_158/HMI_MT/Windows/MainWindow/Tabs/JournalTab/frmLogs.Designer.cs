namespace HMI_MT
{
    partial class frmLogs
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
           if (oscdg != null)
              oscdg.Dispose();

            if( disposing && ( components != null ) )
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
        private void InitializeComponent( )
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpMessages = new System.Windows.Forms.TabPage();
            this.messagesListView = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpLogEventOKU_RZA = new System.Windows.Forms.TabPage();
            this.lstvEvent = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLocalTimeFix = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chIdBlock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBlockName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBlockComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chEventDescript = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAckStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpLogActionUsers = new System.Windows.Forms.TabPage();
            this.lstvUserAction = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLocalTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chServerTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chActionName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chArmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBlockNameUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpLogSummaryAvarOsc = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gbAvar = new System.Windows.Forms.GroupBox();
            this.dgvAvar = new System.Windows.Forms.DataGridView();
            this.clmBlockName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmJumpAvarForm = new System.Windows.Forms.DataGridViewButtonColumn();
            this.clmIDAvar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmIdFcBlock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbOscill = new System.Windows.Forms.GroupBox();
            this.dgvOscill = new System.Windows.Forms.DataGridView();
            this.clmBlockNameOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCommentOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockTimeOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmViewOsc = new System.Windows.Forms.DataGridViewButtonColumn();
            this.clmID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpLogSystemFull = new System.Windows.Forms.TabPage();
            this.lstvLogSystemFull = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTime_logSystemFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chObjName_logSystemFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chComment_logSystemFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chText_logSystemFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSousce_logSystemFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlSelect = new System.Windows.Forms.Panel();
            this.btnListView2File = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_ch2 = new System.Windows.Forms.Label();
            this.nudMin = new System.Windows.Forms.NumericUpDown();
            this.lbl_ch3 = new System.Windows.Forms.Label();
            this.nudSec = new System.Windows.Forms.NumericUpDown();
            this.lbl_ch1 = new System.Windows.Forms.Label();
            this.cbPeriodRenew = new System.Windows.Forms.CheckBox();
            this.gbEndTime = new System.Windows.Forms.GroupBox();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEndData = new System.Windows.Forms.DateTimePicker();
            this.gbStartTime = new System.Windows.Forms.GroupBox();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtpStartData = new System.Windows.Forms.DateTimePicker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.системаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPageSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPrintPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ShowMessagesCountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.MessagesCountLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.autoUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.kvitirovanieGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.kvitByDeviceTypeButton = new System.Windows.Forms.Button();
            this.DeviceTypesComboBox = new System.Windows.Forms.ComboBox();
            this.kvitAllButton = new System.Windows.Forms.Button();
            this.kvitSelectMsgButton = new System.Windows.Forms.Button();
            this.frmLogsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.frmLogsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1.SuspendLayout();
            this.tpMessages.SuspendLayout();
            this.tpLogEventOKU_RZA.SuspendLayout();
            this.tpLogActionUsers.SuspendLayout();
            this.tpLogSummaryAvarOsc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbAvar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvar)).BeginInit();
            this.gbOscill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOscill)).BeginInit();
            this.tpLogSystemFull.SuspendLayout();
            this.pnlSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSec)).BeginInit();
            this.gbEndTime.SuspendLayout();
            this.gbStartTime.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShowMessagesCountNumericUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.kvitirovanieGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frmLogsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frmLogsBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpMessages);
            this.tabControl1.Controls.Add(this.tpLogEventOKU_RZA);
            this.tabControl1.Controls.Add(this.tpLogActionUsers);
            this.tabControl1.Controls.Add(this.tpLogSummaryAvarOsc);
            this.tabControl1.Controls.Add(this.tpLogSystemFull);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 7);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1472, 565);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpMessages
            // 
            this.tpMessages.Controls.Add(this.messagesListView);
            this.tpMessages.Location = new System.Drawing.Point(4, 22);
            this.tpMessages.Name = "tpMessages";
            this.tpMessages.Size = new System.Drawing.Size(1464, 539);
            this.tpMessages.TabIndex = 4;
            this.tpMessages.Text = "Сообщения";
            this.tpMessages.UseVisualStyleBackColor = true;
            // 
            // messagesListView
            // 
            this.messagesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeaderNumber,
            this.columnHeaderDate,
            this.columnHeaderSource,
            this.columnHeaderText,
            this.columnHeaderComment});
            this.messagesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messagesListView.FullRowSelect = true;
            this.messagesListView.GridLines = true;
            this.messagesListView.Location = new System.Drawing.Point(0, 0);
            this.messagesListView.Name = "messagesListView";
            this.messagesListView.Size = new System.Drawing.Size(1464, 539);
            this.messagesListView.TabIndex = 0;
            this.messagesListView.UseCompatibleStateImageBehavior = false;
            this.messagesListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = 1;
            // 
            // columnHeaderNumber
            // 
            this.columnHeaderNumber.Text = "№";
            this.columnHeaderNumber.Width = 30;
            // 
            // columnHeaderDate
            // 
            this.columnHeaderDate.Text = "Дата и Время";
            this.columnHeaderDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderDate.Width = 160;
            // 
            // columnHeaderSource
            // 
            this.columnHeaderSource.Text = "Источник";
            this.columnHeaderSource.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeaderSource.Width = 300;
            // 
            // columnHeaderText
            // 
            this.columnHeaderText.Text = "Текст сообщения";
            this.columnHeaderText.Width = 300;
            // 
            // columnHeaderComment
            // 
            this.columnHeaderComment.Text = "Комментарий";
            this.columnHeaderComment.Width = 313;
            // 
            // tpLogEventOKU_RZA
            // 
            this.tpLogEventOKU_RZA.BackColor = System.Drawing.SystemColors.Control;
            this.tpLogEventOKU_RZA.Controls.Add(this.lstvEvent);
            this.tpLogEventOKU_RZA.Location = new System.Drawing.Point(4, 22);
            this.tpLogEventOKU_RZA.Name = "tpLogEventOKU_RZA";
            this.tpLogEventOKU_RZA.Padding = new System.Windows.Forms.Padding(3);
            this.tpLogEventOKU_RZA.Size = new System.Drawing.Size(1464, 539);
            this.tpLogEventOKU_RZA.TabIndex = 0;
            this.tpLogEventOKU_RZA.Text = "События ОКУ и РЗА";
            this.tpLogEventOKU_RZA.Enter += new System.EventHandler(this.tpLogEventOKU_RZA_Enter);
            // 
            // lstvEvent
            // 
            this.lstvEvent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.chLocalTimeFix,
            this.chIdBlock,
            this.chBlockName,
            this.chBlockComment,
            this.chEventDescript,
            this.chAckStatus});
            this.lstvEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvEvent.GridLines = true;
            this.lstvEvent.HideSelection = false;
            this.lstvEvent.Location = new System.Drawing.Point(3, 3);
            this.lstvEvent.Name = "lstvEvent";
            this.lstvEvent.Size = new System.Drawing.Size(1458, 533);
            this.lstvEvent.TabIndex = 0;
            this.lstvEvent.UseCompatibleStateImageBehavior = false;
            this.lstvEvent.View = System.Windows.Forms.View.Details;
            this.lstvEvent.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstvEvent_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 1;
            // 
            // chLocalTimeFix
            // 
            this.chLocalTimeFix.Text = "Время события (локальное)";
            this.chLocalTimeFix.Width = 180;
            // 
            // chIdBlock
            // 
            this.chIdBlock.Text = "Идент. блока";
            this.chIdBlock.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chIdBlock.Width = 1;
            // 
            // chBlockName
            // 
            this.chBlockName.Text = "Название блока";
            this.chBlockName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chBlockName.Width = 100;
            // 
            // chBlockComment
            // 
            this.chBlockComment.Text = "Комментарий (к блоку)";
            this.chBlockComment.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chBlockComment.Width = 150;
            // 
            // chEventDescript
            // 
            this.chEventDescript.Text = "Описание события";
            this.chEventDescript.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chEventDescript.Width = 200;
            // 
            // chAckStatus
            // 
            this.chAckStatus.Text = "Признак квитирования";
            this.chAckStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chAckStatus.Width = 150;
            // 
            // tpLogActionUsers
            // 
            this.tpLogActionUsers.BackColor = System.Drawing.SystemColors.Control;
            this.tpLogActionUsers.Controls.Add(this.lstvUserAction);
            this.tpLogActionUsers.Location = new System.Drawing.Point(4, 22);
            this.tpLogActionUsers.Name = "tpLogActionUsers";
            this.tpLogActionUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tpLogActionUsers.Size = new System.Drawing.Size(1464, 539);
            this.tpLogActionUsers.TabIndex = 1;
            this.tpLogActionUsers.Text = "Действия пользователей";
            this.tpLogActionUsers.Enter += new System.EventHandler(this.tpLogActionUsers_Enter);
            // 
            // lstvUserAction
            // 
            this.lstvUserAction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstvUserAction.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.chLocalTime,
            this.chServerTime,
            this.chActionName,
            this.chUserName,
            this.chArmName,
            this.chBlockNameUser,
            this.chComment});
            this.lstvUserAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvUserAction.GridLines = true;
            this.lstvUserAction.Location = new System.Drawing.Point(3, 3);
            this.lstvUserAction.MultiSelect = false;
            this.lstvUserAction.Name = "lstvUserAction";
            this.lstvUserAction.Size = new System.Drawing.Size(1458, 533);
            this.lstvUserAction.TabIndex = 0;
            this.lstvUserAction.UseCompatibleStateImageBehavior = false;
            this.lstvUserAction.View = System.Windows.Forms.View.Details;
            this.lstvUserAction.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstvUserAction_ColumnClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 1;
            // 
            // chLocalTime
            // 
            this.chLocalTime.Text = "Время АРМ";
            this.chLocalTime.Width = 150;
            // 
            // chServerTime
            // 
            this.chServerTime.Text = "Время Сервера";
            this.chServerTime.Width = 1;
            // 
            // chActionName
            // 
            this.chActionName.Text = "Действие";
            this.chActionName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chActionName.Width = 360;
            // 
            // chUserName
            // 
            this.chUserName.Text = "Пользователь";
            this.chUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chUserName.Width = 150;
            // 
            // chArmName
            // 
            this.chArmName.Text = "Рабочее место";
            this.chArmName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chArmName.Width = 100;
            // 
            // chBlockNameUser
            // 
            this.chBlockNameUser.Text = "Объект";
            this.chBlockNameUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chBlockNameUser.Width = 100;
            // 
            // chComment
            // 
            this.chComment.Text = "Комментарий";
            this.chComment.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chComment.Width = 150;
            // 
            // tpLogSummaryAvarOsc
            // 
            this.tpLogSummaryAvarOsc.BackColor = System.Drawing.SystemColors.Control;
            this.tpLogSummaryAvarOsc.Controls.Add(this.splitContainer1);
            this.tpLogSummaryAvarOsc.Location = new System.Drawing.Point(4, 22);
            this.tpLogSummaryAvarOsc.Name = "tpLogSummaryAvarOsc";
            this.tpLogSummaryAvarOsc.Padding = new System.Windows.Forms.Padding(3);
            this.tpLogSummaryAvarOsc.Size = new System.Drawing.Size(1464, 539);
            this.tpLogSummaryAvarOsc.TabIndex = 2;
            this.tpLogSummaryAvarOsc.Text = "Сводный список аварий и осциллограмм";
            this.tpLogSummaryAvarOsc.Enter += new System.EventHandler(this.tpLogSummaryAvarOsc_Enter);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gbAvar);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbOscill);
            this.splitContainer1.Size = new System.Drawing.Size(1458, 533);
            this.splitContainer1.SplitterDistance = 737;
            this.splitContainer1.TabIndex = 1;
            // 
            // gbAvar
            // 
            this.gbAvar.Controls.Add(this.dgvAvar);
            this.gbAvar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAvar.Location = new System.Drawing.Point(0, 0);
            this.gbAvar.Name = "gbAvar";
            this.gbAvar.Size = new System.Drawing.Size(737, 533);
            this.gbAvar.TabIndex = 1;
            this.gbAvar.TabStop = false;
            this.gbAvar.Text = "Аварии";
            // 
            // dgvAvar
            // 
            this.dgvAvar.AllowUserToAddRows = false;
            this.dgvAvar.AllowUserToDeleteRows = false;
            this.dgvAvar.AllowUserToResizeRows = false;
            this.dgvAvar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAvar.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvAvar.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvAvar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvar.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmBlockName,
            this.clmComment,
            this.clmBlockTime,
            this.clmJumpAvarForm,
            this.clmIDAvar,
            this.clmIdFcBlock,
            this.clmBlockId});
            this.dgvAvar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAvar.Location = new System.Drawing.Point(3, 16);
            this.dgvAvar.MultiSelect = false;
            this.dgvAvar.Name = "dgvAvar";
            this.dgvAvar.ReadOnly = true;
            this.dgvAvar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAvar.Size = new System.Drawing.Size(731, 514);
            this.dgvAvar.TabIndex = 0;
            this.dgvAvar.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAvar_CellContentClick);
            // 
            // clmBlockName
            // 
            this.clmBlockName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.clmBlockName.FillWeight = 181.2614F;
            this.clmBlockName.HeaderText = "Тип блока";
            this.clmBlockName.Name = "clmBlockName";
            this.clmBlockName.ReadOnly = true;
            this.clmBlockName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmBlockName.Width = 65;
            // 
            // clmComment
            // 
            this.clmComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmComment.FillWeight = 11.05808F;
            this.clmComment.HeaderText = "Присоединение";
            this.clmComment.Name = "clmComment";
            this.clmComment.ReadOnly = true;
            this.clmComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clmBlockTime
            // 
            this.clmBlockTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.clmBlockTime.FillWeight = 101.5228F;
            this.clmBlockTime.HeaderText = "Время блока";
            this.clmBlockTime.Name = "clmBlockTime";
            this.clmBlockTime.ReadOnly = true;
            this.clmBlockTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmBlockTime.Width = 79;
            // 
            // clmJumpAvarForm
            // 
            this.clmJumpAvarForm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmJumpAvarForm.FillWeight = 106.1577F;
            this.clmJumpAvarForm.HeaderText = "Просмотр";
            this.clmJumpAvarForm.Name = "clmJumpAvarForm";
            this.clmJumpAvarForm.ReadOnly = true;
            this.clmJumpAvarForm.Text = "Просмотр";
            this.clmJumpAvarForm.UseColumnTextForButtonValue = true;
            // 
            // clmIDAvar
            // 
            this.clmIDAvar.Name = "clmIDAvar";
            this.clmIDAvar.ReadOnly = true;
            this.clmIDAvar.Visible = false;
            // 
            // clmIdFcBlock
            // 
            this.clmIdFcBlock.Name = "clmIdFcBlock";
            this.clmIdFcBlock.ReadOnly = true;
            this.clmIdFcBlock.Visible = false;
            // 
            // clmBlockId
            // 
            this.clmBlockId.HeaderText = "clmBlockId";
            this.clmBlockId.Name = "clmBlockId";
            this.clmBlockId.ReadOnly = true;
            this.clmBlockId.Visible = false;
            // 
            // gbOscill
            // 
            this.gbOscill.Controls.Add(this.dgvOscill);
            this.gbOscill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOscill.Location = new System.Drawing.Point(0, 0);
            this.gbOscill.Name = "gbOscill";
            this.gbOscill.Size = new System.Drawing.Size(717, 533);
            this.gbOscill.TabIndex = 2;
            this.gbOscill.TabStop = false;
            this.gbOscill.Text = "Осциллограммы и диаграммы";
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
            this.clmBlockNameOsc,
            this.clmCommentOsc,
            this.clmBlockTimeOsc,
            this.clmViewOsc,
            this.clmID});
            this.dgvOscill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOscill.Location = new System.Drawing.Point(3, 16);
            this.dgvOscill.MultiSelect = false;
            this.dgvOscill.Name = "dgvOscill";
            this.dgvOscill.ReadOnly = true;
            this.dgvOscill.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOscill.Size = new System.Drawing.Size(711, 514);
            this.dgvOscill.TabIndex = 0;
            this.dgvOscill.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOscill_CellContentClick);
            // 
            // clmBlockNameOsc
            // 
            this.clmBlockNameOsc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.clmBlockNameOsc.HeaderText = "Тип блока";
            this.clmBlockNameOsc.Name = "clmBlockNameOsc";
            this.clmBlockNameOsc.ReadOnly = true;
            this.clmBlockNameOsc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmBlockNameOsc.Width = 65;
            // 
            // clmCommentOsc
            // 
            this.clmCommentOsc.HeaderText = "Присоединение";
            this.clmCommentOsc.Name = "clmCommentOsc";
            this.clmCommentOsc.ReadOnly = true;
            this.clmCommentOsc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clmBlockTimeOsc
            // 
            this.clmBlockTimeOsc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.clmBlockTimeOsc.HeaderText = "Время блока";
            this.clmBlockTimeOsc.Name = "clmBlockTimeOsc";
            this.clmBlockTimeOsc.ReadOnly = true;
            this.clmBlockTimeOsc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmBlockTimeOsc.Width = 79;
            // 
            // clmViewOsc
            // 
            this.clmViewOsc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmViewOsc.HeaderText = "Просмотр";
            this.clmViewOsc.Name = "clmViewOsc";
            this.clmViewOsc.ReadOnly = true;
            // 
            // clmID
            // 
            this.clmID.HeaderText = "Идентификатор";
            this.clmID.Name = "clmID";
            this.clmID.ReadOnly = true;
            this.clmID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmID.Visible = false;
            // 
            // tpLogSystemFull
            // 
            this.tpLogSystemFull.BackColor = System.Drawing.SystemColors.Control;
            this.tpLogSystemFull.Controls.Add(this.lstvLogSystemFull);
            this.tpLogSystemFull.Location = new System.Drawing.Point(4, 22);
            this.tpLogSystemFull.Name = "tpLogSystemFull";
            this.tpLogSystemFull.Padding = new System.Windows.Forms.Padding(3);
            this.tpLogSystemFull.Size = new System.Drawing.Size(1464, 539);
            this.tpLogSystemFull.TabIndex = 3;
            this.tpLogSystemFull.Text = "Сводный системный журнал";
            this.tpLogSystemFull.Enter += new System.EventHandler(this.tpLogSystemFull_Enter);
            // 
            // lstvLogSystemFull
            // 
            this.lstvLogSystemFull.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.chTime_logSystemFull,
            this.chObjName_logSystemFull,
            this.chComment_logSystemFull,
            this.chText_logSystemFull,
            this.chSousce_logSystemFull});
            this.lstvLogSystemFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvLogSystemFull.GridLines = true;
            this.lstvLogSystemFull.Location = new System.Drawing.Point(3, 3);
            this.lstvLogSystemFull.Name = "lstvLogSystemFull";
            this.lstvLogSystemFull.Size = new System.Drawing.Size(1458, 533);
            this.lstvLogSystemFull.TabIndex = 0;
            this.lstvLogSystemFull.UseCompatibleStateImageBehavior = false;
            this.lstvLogSystemFull.View = System.Windows.Forms.View.Details;
            this.lstvLogSystemFull.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstvLogSystemFull_ColumnClick);
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
            // chObjName_logSystemFull
            // 
            this.chObjName_logSystemFull.Text = "Объект";
            this.chObjName_logSystemFull.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chObjName_logSystemFull.Width = 100;
            // 
            // chComment_logSystemFull
            // 
            this.chComment_logSystemFull.Text = "Комментарий";
            this.chComment_logSystemFull.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chComment_logSystemFull.Width = 300;
            // 
            // chText_logSystemFull
            // 
            this.chText_logSystemFull.Text = "Текст";
            this.chText_logSystemFull.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chText_logSystemFull.Width = 150;
            // 
            // chSousce_logSystemFull
            // 
            this.chSousce_logSystemFull.Text = "Источник";
            this.chSousce_logSystemFull.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chSousce_logSystemFull.Width = 150;
            // 
            // pnlSelect
            // 
            this.pnlSelect.BackColor = System.Drawing.Color.LightCoral;
            this.pnlSelect.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSelect.Controls.Add(this.btnListView2File);
            this.pnlSelect.Controls.Add(this.button1);
            this.pnlSelect.Controls.Add(this.lbl_ch2);
            this.pnlSelect.Controls.Add(this.nudMin);
            this.pnlSelect.Controls.Add(this.lbl_ch3);
            this.pnlSelect.Controls.Add(this.nudSec);
            this.pnlSelect.Controls.Add(this.lbl_ch1);
            this.pnlSelect.Controls.Add(this.cbPeriodRenew);
            this.pnlSelect.Controls.Add(this.gbEndTime);
            this.pnlSelect.Controls.Add(this.gbStartTime);
            this.pnlSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSelect.Location = new System.Drawing.Point(3, 578);
            this.pnlSelect.Name = "pnlSelect";
            this.pnlSelect.Size = new System.Drawing.Size(1472, 104);
            this.pnlSelect.TabIndex = 1;
            // 
            // btnListView2File
            // 
            this.btnListView2File.AutoSize = true;
            this.btnListView2File.Location = new System.Drawing.Point(786, 42);
            this.btnListView2File.Name = "btnListView2File";
            this.btnListView2File.Size = new System.Drawing.Size(185, 23);
            this.btnListView2File.TabIndex = 13;
            this.btnListView2File.Text = "Вывод текущего журнала в файл";
            this.btnListView2File.UseVisualStyleBackColor = true;
            this.btnListView2File.Click += new System.EventHandler(this.btnListView2File_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(621, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Обновить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_ch2
            // 
            this.lbl_ch2.AutoSize = true;
            this.lbl_ch2.Enabled = false;
            this.lbl_ch2.Location = new System.Drawing.Point(915, 17);
            this.lbl_ch2.Name = "lbl_ch2";
            this.lbl_ch2.Size = new System.Drawing.Size(30, 13);
            this.lbl_ch2.TabIndex = 7;
            this.lbl_ch2.Text = "мин.";
            // 
            // nudMin
            // 
            this.nudMin.Enabled = false;
            this.nudMin.Location = new System.Drawing.Point(872, 13);
            this.nudMin.Name = "nudMin";
            this.nudMin.Size = new System.Drawing.Size(37, 20);
            this.nudMin.TabIndex = 6;
            // 
            // lbl_ch3
            // 
            this.lbl_ch3.AutoSize = true;
            this.lbl_ch3.Enabled = false;
            this.lbl_ch3.Location = new System.Drawing.Point(991, 17);
            this.lbl_ch3.Name = "lbl_ch3";
            this.lbl_ch3.Size = new System.Drawing.Size(28, 13);
            this.lbl_ch3.TabIndex = 5;
            this.lbl_ch3.Text = "сек.";
            // 
            // nudSec
            // 
            this.nudSec.Enabled = false;
            this.nudSec.Location = new System.Drawing.Point(945, 13);
            this.nudSec.Name = "nudSec";
            this.nudSec.Size = new System.Drawing.Size(37, 20);
            this.nudSec.TabIndex = 4;
            // 
            // lbl_ch1
            // 
            this.lbl_ch1.AutoSize = true;
            this.lbl_ch1.Enabled = false;
            this.lbl_ch1.Location = new System.Drawing.Point(781, 18);
            this.lbl_ch1.Name = "lbl_ch1";
            this.lbl_ch1.Size = new System.Drawing.Size(80, 13);
            this.lbl_ch1.TabIndex = 3;
            this.lbl_ch1.Text = "с интервалом ";
            // 
            // cbPeriodRenew
            // 
            this.cbPeriodRenew.AutoSize = true;
            this.cbPeriodRenew.Location = new System.Drawing.Point(620, 17);
            this.cbPeriodRenew.Name = "cbPeriodRenew";
            this.cbPeriodRenew.Size = new System.Drawing.Size(155, 17);
            this.cbPeriodRenew.TabIndex = 2;
            this.cbPeriodRenew.Text = "Периодически обновлять";
            this.cbPeriodRenew.UseVisualStyleBackColor = true;
            this.cbPeriodRenew.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // gbEndTime
            // 
            this.gbEndTime.Controls.Add(this.dtpEndTime);
            this.gbEndTime.Controls.Add(this.dtpEndData);
            this.gbEndTime.Location = new System.Drawing.Point(316, 4);
            this.gbEndTime.Name = "gbEndTime";
            this.gbEndTime.Size = new System.Drawing.Size(290, 61);
            this.gbEndTime.TabIndex = 1;
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
            // 
            // dtpEndData
            // 
            this.dtpEndData.Location = new System.Drawing.Point(16, 23);
            this.dtpEndData.Name = "dtpEndData";
            this.dtpEndData.Size = new System.Drawing.Size(130, 20);
            this.dtpEndData.TabIndex = 0;
            // 
            // gbStartTime
            // 
            this.gbStartTime.Controls.Add(this.dtpStartTime);
            this.gbStartTime.Controls.Add(this.dtpStartData);
            this.gbStartTime.Location = new System.Drawing.Point(7, 4);
            this.gbStartTime.Name = "gbStartTime";
            this.gbStartTime.Size = new System.Drawing.Size(290, 61);
            this.gbStartTime.TabIndex = 0;
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
            // 
            // dtpStartData
            // 
            this.dtpStartData.Location = new System.Drawing.Point(20, 23);
            this.dtpStartData.Name = "dtpStartData";
            this.dtpStartData.Size = new System.Drawing.Size(130, 20);
            this.dtpStartData.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.системаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1016, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // системаToolStripMenuItem
            // 
            this.системаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPageSetup,
            this.mnuPrintPreview,
            this.mnuPrint});
            this.системаToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.системаToolStripMenuItem.MergeIndex = 1;
            this.системаToolStripMenuItem.Name = "системаToolStripMenuItem";
            this.системаToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.системаToolStripMenuItem.Text = "Система";
            // 
            // mnuPageSetup
            // 
            this.mnuPageSetup.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.mnuPageSetup.MergeIndex = 1;
            this.mnuPageSetup.Name = "mnuPageSetup";
            this.mnuPageSetup.Size = new System.Drawing.Size(233, 22);
            this.mnuPageSetup.Text = "Параметры страницы";
            this.mnuPageSetup.Click += new System.EventHandler(this.mnuPageSetup_Click);
            // 
            // mnuPrintPreview
            // 
            this.mnuPrintPreview.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.mnuPrintPreview.MergeIndex = 1;
            this.mnuPrintPreview.Name = "mnuPrintPreview";
            this.mnuPrintPreview.Size = new System.Drawing.Size(233, 22);
            this.mnuPrintPreview.Text = "Предварительный просмотр";
            this.mnuPrintPreview.Click += new System.EventHandler(this.mnuPrintPreview_Click);
            // 
            // mnuPrint
            // 
            this.mnuPrint.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.mnuPrint.MergeIndex = 1;
            this.mnuPrint.Name = "mnuPrint";
            this.mnuPrint.Size = new System.Drawing.Size(233, 22);
            this.mnuPrint.Text = "Печать";
            this.mnuPrint.Click += new System.EventHandler(this.печатьToolStripMenuItem1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlSelect, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1478, 791);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCoral;
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.kvitirovanieGroupBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 688);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1472, 100);
            this.panel1.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ShowMessagesCountNumericUpDown);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.MessagesCountLabel);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(1025, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(328, 80);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Количество сообщений";
            // 
            // ShowMessagesCountNumericUpDown
            // 
            this.ShowMessagesCountNumericUpDown.Location = new System.Drawing.Point(179, 46);
            this.ShowMessagesCountNumericUpDown.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.ShowMessagesCountNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ShowMessagesCountNumericUpDown.Name = "ShowMessagesCountNumericUpDown";
            this.ShowMessagesCountNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.ShowMessagesCountNumericUpDown.TabIndex = 4;
            this.ShowMessagesCountNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ShowMessagesCountNumericUpDown.ValueChanged += new System.EventHandler(this.ShowMessagesCountNumericUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Показывать сообщений:";
            // 
            // MessagesCountLabel
            // 
            this.MessagesCountLabel.AutoSize = true;
            this.MessagesCountLabel.Location = new System.Drawing.Point(176, 21);
            this.MessagesCountLabel.Name = "MessagesCountLabel";
            this.MessagesCountLabel.Size = new System.Drawing.Size(29, 13);
            this.MessagesCountLabel.TabIndex = 1;
            this.MessagesCountLabel.Text = "NaN";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Всего сообщений:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.autoUpdateCheckBox);
            this.groupBox2.Controls.Add(this.updateButton);
            this.groupBox2.Location = new System.Drawing.Point(801, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 80);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Обновление";
            // 
            // autoUpdateCheckBox
            // 
            this.autoUpdateCheckBox.AutoSize = true;
            this.autoUpdateCheckBox.Location = new System.Drawing.Point(6, 21);
            this.autoUpdateCheckBox.Name = "autoUpdateCheckBox";
            this.autoUpdateCheckBox.Size = new System.Drawing.Size(160, 17);
            this.autoUpdateCheckBox.TabIndex = 2;
            this.autoUpdateCheckBox.Text = "Автоматически обновлять";
            this.autoUpdateCheckBox.UseVisualStyleBackColor = true;
            this.autoUpdateCheckBox.CheckedChanged += new System.EventHandler(this.autoUpdateCheckBox_CheckedChanged);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(11, 44);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(160, 23);
            this.updateButton.TabIndex = 1;
            this.updateButton.Text = "Обновить";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // kvitirovanieGroupBox
            // 
            this.kvitirovanieGroupBox.Controls.Add(this.groupBox1);
            this.kvitirovanieGroupBox.Controls.Add(this.kvitAllButton);
            this.kvitirovanieGroupBox.Controls.Add(this.kvitSelectMsgButton);
            this.kvitirovanieGroupBox.Location = new System.Drawing.Point(9, 3);
            this.kvitirovanieGroupBox.Name = "kvitirovanieGroupBox";
            this.kvitirovanieGroupBox.Size = new System.Drawing.Size(769, 80);
            this.kvitirovanieGroupBox.TabIndex = 0;
            this.kvitirovanieGroupBox.TabStop = false;
            this.kvitirovanieGroupBox.Text = "Квитирование";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.kvitByDeviceTypeButton);
            this.groupBox1.Controls.Add(this.DeviceTypesComboBox);
            this.groupBox1.Location = new System.Drawing.Point(376, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 51);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Квитирование по устройствам";
            // 
            // kvitByDeviceTypeButton
            // 
            this.kvitByDeviceTypeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kvitByDeviceTypeButton.Location = new System.Drawing.Point(294, 17);
            this.kvitByDeviceTypeButton.Name = "kvitByDeviceTypeButton";
            this.kvitByDeviceTypeButton.Size = new System.Drawing.Size(87, 23);
            this.kvitByDeviceTypeButton.TabIndex = 1;
            this.kvitByDeviceTypeButton.Text = "Квитировать";
            this.kvitByDeviceTypeButton.UseVisualStyleBackColor = true;
            this.kvitByDeviceTypeButton.Click += new System.EventHandler(this.kvitByDeviceTypeButton_Click);
            // 
            // DeviceTypesComboBox
            // 
            this.DeviceTypesComboBox.FormattingEnabled = true;
            this.DeviceTypesComboBox.Location = new System.Drawing.Point(6, 19);
            this.DeviceTypesComboBox.Name = "DeviceTypesComboBox";
            this.DeviceTypesComboBox.Size = new System.Drawing.Size(282, 21);
            this.DeviceTypesComboBox.TabIndex = 0;
            // 
            // kvitAllButton
            // 
            this.kvitAllButton.Location = new System.Drawing.Point(258, 33);
            this.kvitAllButton.Name = "kvitAllButton";
            this.kvitAllButton.Size = new System.Drawing.Size(101, 23);
            this.kvitAllButton.TabIndex = 1;
            this.kvitAllButton.Text = "Квитировать всё";
            this.kvitAllButton.UseVisualStyleBackColor = true;
            this.kvitAllButton.Click += new System.EventHandler(this.kvitAllButton_Click);
            // 
            // kvitSelectMsgButton
            // 
            this.kvitSelectMsgButton.Location = new System.Drawing.Point(20, 33);
            this.kvitSelectMsgButton.Name = "kvitSelectMsgButton";
            this.kvitSelectMsgButton.Size = new System.Drawing.Size(217, 23);
            this.kvitSelectMsgButton.TabIndex = 0;
            this.kvitSelectMsgButton.Text = "Квитировать выделенные сообщения";
            this.kvitSelectMsgButton.UseVisualStyleBackColor = true;
            this.kvitSelectMsgButton.Click += new System.EventHandler(this.kvitSelectMsgButton_Click);
            // 
            // frmLogsBindingSource
            // 
            this.frmLogsBindingSource.DataSource = typeof(HMI_MT.frmLogs);
            // 
            // frmLogsBindingSource1
            // 
            this.frmLogsBindingSource1.DataSource = typeof(HMI_MT.frmLogs);
            // 
            // frmLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1478, 791);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmLogs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ведомости и журналы";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogs_FormClosing);
            this.Load += new System.EventHandler(this.frmLogs_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpMessages.ResumeLayout(false);
            this.tpLogEventOKU_RZA.ResumeLayout(false);
            this.tpLogActionUsers.ResumeLayout(false);
            this.tpLogSummaryAvarOsc.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbAvar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvar)).EndInit();
            this.gbOscill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOscill)).EndInit();
            this.tpLogSystemFull.ResumeLayout(false);
            this.pnlSelect.ResumeLayout(false);
            this.pnlSelect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSec)).EndInit();
            this.gbEndTime.ResumeLayout(false);
            this.gbStartTime.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShowMessagesCountNumericUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.kvitirovanieGroupBox.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frmLogsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frmLogsBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpLogEventOKU_RZA;
        private System.Windows.Forms.TabPage tpLogActionUsers;
        private System.Windows.Forms.TabPage tpLogSummaryAvarOsc;
        private System.Windows.Forms.Panel pnlSelect;
        private System.Windows.Forms.ListView lstvEvent;
        private System.Windows.Forms.ColumnHeader chLocalTimeFix;
        private System.Windows.Forms.ColumnHeader chIdBlock;
        private System.Windows.Forms.ColumnHeader chBlockName;
        private System.Windows.Forms.ColumnHeader chBlockComment;
        private System.Windows.Forms.ColumnHeader chEventDescript;
        private System.Windows.Forms.ColumnHeader chAckStatus;
        private System.Windows.Forms.GroupBox gbEndTime;
        private System.Windows.Forms.GroupBox gbStartTime;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.DateTimePicker dtpEndData;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.DateTimePicker dtpStartData;
        private System.Windows.Forms.Label lbl_ch2;
        private System.Windows.Forms.NumericUpDown nudMin;
        private System.Windows.Forms.Label lbl_ch3;
        private System.Windows.Forms.NumericUpDown nudSec;
        private System.Windows.Forms.Label lbl_ch1;
		 private System.Windows.Forms.CheckBox cbPeriodRenew;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem системаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuPrint;
        private System.Windows.Forms.ToolStripMenuItem mnuPageSetup;
        private System.Windows.Forms.ToolStripMenuItem mnuPrintPreview;
        private System.Windows.Forms.BindingSource frmLogsBindingSource;
        private System.Windows.Forms.ListView lstvUserAction;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader chActionName;
        private System.Windows.Forms.ColumnHeader chUserName;
        private System.Windows.Forms.ColumnHeader chArmName;
        private System.Windows.Forms.ColumnHeader chBlockNameUser;
        private System.Windows.Forms.ColumnHeader chComment;
        private System.Windows.Forms.ColumnHeader chServerTime;
        private System.Windows.Forms.ColumnHeader chLocalTime;
		 private System.Windows.Forms.TabPage tpLogSystemFull;
		 private System.Windows.Forms.ListView lstvLogSystemFull;
		 private System.Windows.Forms.ColumnHeader columnHeader3;
		 private System.Windows.Forms.ColumnHeader chTime_logSystemFull;
		 private System.Windows.Forms.ColumnHeader chObjName_logSystemFull;
		 private System.Windows.Forms.ColumnHeader chComment_logSystemFull;
		 private System.Windows.Forms.ColumnHeader chText_logSystemFull;
		 private System.Windows.Forms.ColumnHeader chSousce_logSystemFull;
       private System.Windows.Forms.Button btnListView2File;
       private System.Windows.Forms.BindingSource frmLogsBindingSource1;
       private System.Windows.Forms.SplitContainer splitContainer1;
       private System.Windows.Forms.GroupBox gbAvar;
       private System.Windows.Forms.DataGridView dgvAvar;
       private System.Windows.Forms.GroupBox gbOscill;
       private System.Windows.Forms.DataGridView dgvOscill;
       private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
       private System.Windows.Forms.TabPage tpMessages;
       private System.Windows.Forms.ListView messagesListView;
       private System.Windows.Forms.ColumnHeader columnHeaderNumber;
       private System.Windows.Forms.ColumnHeader columnHeaderDate;
       private System.Windows.Forms.ColumnHeader columnHeaderSource;
       private System.Windows.Forms.ColumnHeader columnHeaderText;
       private System.Windows.Forms.ColumnHeader columnHeaderComment;
       private System.Windows.Forms.ColumnHeader columnHeader4;
       private System.Windows.Forms.Panel panel1;
       private System.Windows.Forms.GroupBox kvitirovanieGroupBox;
       private System.Windows.Forms.GroupBox groupBox1;
       private System.Windows.Forms.Button kvitByDeviceTypeButton;
       private System.Windows.Forms.ComboBox DeviceTypesComboBox;
       private System.Windows.Forms.Button kvitAllButton;
       private System.Windows.Forms.Button kvitSelectMsgButton;
       private System.Windows.Forms.CheckBox autoUpdateCheckBox;
       private System.Windows.Forms.Button updateButton;
       private System.Windows.Forms.GroupBox groupBox2;
       private System.Windows.Forms.GroupBox groupBox3;
       private System.Windows.Forms.Label label3;
       private System.Windows.Forms.Label MessagesCountLabel;
       private System.Windows.Forms.Label label1;
       private System.Windows.Forms.NumericUpDown ShowMessagesCountNumericUpDown;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockNameOsc;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmCommentOsc;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockTimeOsc;
       private System.Windows.Forms.DataGridViewButtonColumn clmViewOsc;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmID;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockName;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmComment;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockTime;
       private System.Windows.Forms.DataGridViewButtonColumn clmJumpAvarForm;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmIDAvar;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmIdFcBlock;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockId;
    }
}