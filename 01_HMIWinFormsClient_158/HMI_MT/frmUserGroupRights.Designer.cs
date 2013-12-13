namespace HMI_MT
{
    partial class frmUserGroupRights
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserGroupRights));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tbUsers = new System.Windows.Forms.TabControl();
			this.tabUsers = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.btnAddUser = new System.Windows.Forms.Button();
			this.gbEditUserParameters = new System.Windows.Forms.GroupBox();
			this.pnlHelp = new System.Windows.Forms.Panel();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.btnDelUser = new System.Windows.Forms.Button();
			this.tbComment = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbInNewPassConfirm = new System.Windows.Forms.TextBox();
			this.tbInNewPass = new System.Windows.Forms.TextBox();
			this.lbNewPassConf = new System.Windows.Forms.Label();
			this.lbInNewPass = new System.Windows.Forms.Label();
			this.tbInOldPass = new System.Windows.Forms.TextBox();
			this.lbInOldPass = new System.Windows.Forms.Label();
			this.cbUserGroup = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnChUserPass = new System.Windows.Forms.Button();
			this.tbUserName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSaveChange = new System.Windows.Forms.Button();
			this.dgvUsers = new System.Windows.Forms.DataGridView();
			this.clmUsersName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmUserGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmDateGentration = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmChangeUser = new System.Windows.Forms.DataGridViewButtonColumn();
			this.clmIdUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmUserPass = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbGroupRights = new System.Windows.Forms.TabPage();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.dgvGroups = new System.Windows.Forms.DataGridView();
			this.clmGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmGroupComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmGroupCreateData = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmGroupEditData = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmGroupRight = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmIdGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmGroupMenuRight = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.splitContainer4 = new System.Windows.Forms.SplitContainer();
			this.gbRights = new System.Windows.Forms.GroupBox();
			this.flpGroupRights = new System.Windows.Forms.FlowLayoutPanel();
			this.splitContainer5 = new System.Windows.Forms.SplitContainer();
			this.gbCustomRight = new System.Windows.Forms.GroupBox();
			this.dgvRights = new System.Windows.Forms.DataGridView();
			this.clmRightsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.clmVedenieLoga = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.clmOutInLog = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.clmIdRightRecord = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.gbMenuTreeview = new System.Windows.Forms.GroupBox();
			this.treeViewHideMenu = new System.Windows.Forms.TreeView();
			this.contextMenuStripMenuTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiAccessOn = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAccessOff = new System.Windows.Forms.ToolStripMenuItem();
			this.btnGroupDel = new System.Windows.Forms.Button();
			this.btnGroupcreate = new System.Windows.Forms.Button();
			this.btnGroupSaveChange = new System.Windows.Forms.Button();
			this.tbUsers.SuspendLayout();
			this.tabUsers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.gbEditUserParameters.SuspendLayout();
			this.pnlHelp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
			this.tbGroupRights.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
			this.splitContainer4.Panel1.SuspendLayout();
			this.splitContainer4.Panel2.SuspendLayout();
			this.splitContainer4.SuspendLayout();
			this.gbRights.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
			this.splitContainer5.Panel1.SuspendLayout();
			this.splitContainer5.Panel2.SuspendLayout();
			this.splitContainer5.SuspendLayout();
			this.gbCustomRight.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvRights)).BeginInit();
			this.gbMenuTreeview.SuspendLayout();
			this.contextMenuStripMenuTreeView.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbUsers
			// 
			this.tbUsers.Controls.Add(this.tabUsers);
			this.tbUsers.Controls.Add(this.tbGroupRights);
			this.tbUsers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbUsers.Location = new System.Drawing.Point(0, 0);
			this.tbUsers.Name = "tbUsers";
			this.tbUsers.SelectedIndex = 0;
			this.tbUsers.Size = new System.Drawing.Size(1037, 598);
			this.tbUsers.TabIndex = 0;
			// 
			// tabUsers
			// 
			this.tabUsers.BackColor = System.Drawing.SystemColors.Control;
			this.tabUsers.Controls.Add(this.splitContainer1);
			this.tabUsers.Location = new System.Drawing.Point(4, 22);
			this.tabUsers.Name = "tabUsers";
			this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
			this.tabUsers.Size = new System.Drawing.Size(1029, 572);
			this.tabUsers.TabIndex = 0;
			this.tabUsers.Text = "Пользователи";
			this.tabUsers.Enter += new System.EventHandler(this.tabUsers_Enter);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.btnAddUser);
			this.splitContainer1.Panel1.Controls.Add(this.gbEditUserParameters);
			this.splitContainer1.Panel1.Controls.Add(this.dgvUsers);
			this.splitContainer1.Size = new System.Drawing.Size(1023, 566);
			this.splitContainer1.SplitterDistance = 509;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 0;
			// 
			// btnAddUser
			// 
			this.btnAddUser.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnAddUser.Location = new System.Drawing.Point(583, 484);
			this.btnAddUser.Name = "btnAddUser";
			this.btnAddUser.Size = new System.Drawing.Size(440, 23);
			this.btnAddUser.TabIndex = 10;
			this.btnAddUser.Text = "Добавить пользователя";
			this.btnAddUser.UseVisualStyleBackColor = true;
			this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
			// 
			// gbEditUserParameters
			// 
			this.gbEditUserParameters.Controls.Add(this.pnlHelp);
			this.gbEditUserParameters.Controls.Add(this.btnDelUser);
			this.gbEditUserParameters.Controls.Add(this.tbComment);
			this.gbEditUserParameters.Controls.Add(this.label3);
			this.gbEditUserParameters.Controls.Add(this.tbInNewPassConfirm);
			this.gbEditUserParameters.Controls.Add(this.tbInNewPass);
			this.gbEditUserParameters.Controls.Add(this.lbNewPassConf);
			this.gbEditUserParameters.Controls.Add(this.lbInNewPass);
			this.gbEditUserParameters.Controls.Add(this.tbInOldPass);
			this.gbEditUserParameters.Controls.Add(this.lbInOldPass);
			this.gbEditUserParameters.Controls.Add(this.cbUserGroup);
			this.gbEditUserParameters.Controls.Add(this.label2);
			this.gbEditUserParameters.Controls.Add(this.btnChUserPass);
			this.gbEditUserParameters.Controls.Add(this.tbUserName);
			this.gbEditUserParameters.Controls.Add(this.label1);
			this.gbEditUserParameters.Controls.Add(this.btnSaveChange);
			this.gbEditUserParameters.Dock = System.Windows.Forms.DockStyle.Top;
			this.gbEditUserParameters.Location = new System.Drawing.Point(583, 0);
			this.gbEditUserParameters.Name = "gbEditUserParameters";
			this.gbEditUserParameters.Size = new System.Drawing.Size(440, 484);
			this.gbEditUserParameters.TabIndex = 1;
			this.gbEditUserParameters.TabStop = false;
			this.gbEditUserParameters.Text = "Изменение параметров пользователя";
			// 
			// pnlHelp
			// 
			this.pnlHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlHelp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlHelp.Controls.Add(this.textBox1);
			this.pnlHelp.Location = new System.Drawing.Point(6, 19);
			this.pnlHelp.Name = "pnlHelp";
			this.pnlHelp.Size = new System.Drawing.Size(431, 149);
			this.pnlHelp.TabIndex = 18;
			this.pnlHelp.Visible = false;
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(429, 147);
			this.textBox1.TabIndex = 0;
			this.textBox1.TabStop = false;
			this.textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// btnDelUser
			// 
			this.btnDelUser.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnDelUser.Location = new System.Drawing.Point(3, 435);
			this.btnDelUser.Name = "btnDelUser";
			this.btnDelUser.Size = new System.Drawing.Size(434, 23);
			this.btnDelUser.TabIndex = 8;
			this.btnDelUser.Text = "Удалить пользователя";
			this.btnDelUser.UseVisualStyleBackColor = true;
			this.btnDelUser.Click += new System.EventHandler(this.btnDelUser_Click);
			// 
			// tbComment
			// 
			this.tbComment.Location = new System.Drawing.Point(129, 232);
			this.tbComment.Name = "tbComment";
			this.tbComment.Size = new System.Drawing.Size(297, 20);
			this.tbComment.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(25, 235);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(77, 13);
			this.label3.TabIndex = 13;
			this.label3.Text = "Комментарий";
			// 
			// tbInNewPassConfirm
			// 
			this.tbInNewPassConfirm.Location = new System.Drawing.Point(255, 366);
			this.tbInNewPassConfirm.Name = "tbInNewPassConfirm";
			this.tbInNewPassConfirm.Size = new System.Drawing.Size(170, 20);
			this.tbInNewPassConfirm.TabIndex = 5;
			this.tbInNewPassConfirm.UseSystemPasswordChar = true;
			this.tbInNewPassConfirm.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbInNewPassConfirm_KeyDown);
			// 
			// tbInNewPass
			// 
			this.tbInNewPass.Location = new System.Drawing.Point(255, 340);
			this.tbInNewPass.Name = "tbInNewPass";
			this.tbInNewPass.Size = new System.Drawing.Size(171, 20);
			this.tbInNewPass.TabIndex = 4;
			this.tbInNewPass.UseSystemPasswordChar = true;
			// 
			// lbNewPassConf
			// 
			this.lbNewPassConf.AutoSize = true;
			this.lbNewPassConf.Location = new System.Drawing.Point(126, 369);
			this.lbNewPassConf.Name = "lbNewPassConf";
			this.lbNewPassConf.Size = new System.Drawing.Size(111, 13);
			this.lbNewPassConf.TabIndex = 10;
			this.lbNewPassConf.Text = "Подтвердите новый:";
			// 
			// lbInNewPass
			// 
			this.lbInNewPass.AutoSize = true;
			this.lbInNewPass.Location = new System.Drawing.Point(126, 340);
			this.lbInNewPass.Name = "lbInNewPass";
			this.lbInNewPass.Size = new System.Drawing.Size(87, 13);
			this.lbInNewPass.TabIndex = 9;
			this.lbInNewPass.Text = "Введите новый:";
			// 
			// tbInOldPass
			// 
			this.tbInOldPass.Location = new System.Drawing.Point(254, 314);
			this.tbInOldPass.Name = "tbInOldPass";
			this.tbInOldPass.Size = new System.Drawing.Size(171, 20);
			this.tbInOldPass.TabIndex = 3;
			this.tbInOldPass.UseSystemPasswordChar = true;
			this.tbInOldPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbInOldPass_KeyDown);
			// 
			// lbInOldPass
			// 
			this.lbInOldPass.AutoSize = true;
			this.lbInOldPass.Location = new System.Drawing.Point(126, 317);
			this.lbInOldPass.Name = "lbInOldPass";
			this.lbInOldPass.Size = new System.Drawing.Size(92, 13);
			this.lbInOldPass.TabIndex = 7;
			this.lbInOldPass.Text = "Введите старый:";
			// 
			// cbUserGroup
			// 
			this.cbUserGroup.FormattingEnabled = true;
			this.cbUserGroup.Location = new System.Drawing.Point(130, 396);
			this.cbUserGroup.Name = "cbUserGroup";
			this.cbUserGroup.Size = new System.Drawing.Size(296, 21);
			this.cbUserGroup.TabIndex = 6;
			this.cbUserGroup.TextChanged += new System.EventHandler(this.cbUserGroup_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(26, 399);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(97, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Изменить группу:";
			// 
			// btnChUserPass
			// 
			this.btnChUserPass.Location = new System.Drawing.Point(129, 268);
			this.btnChUserPass.Name = "btnChUserPass";
			this.btnChUserPass.Size = new System.Drawing.Size(296, 23);
			this.btnChUserPass.TabIndex = 2;
			this.btnChUserPass.Text = "Изменить пароль";
			this.btnChUserPass.UseVisualStyleBackColor = true;
			this.btnChUserPass.Click += new System.EventHandler(this.btnChUserPass_Click);
			// 
			// tbUserName
			// 
			this.tbUserName.Location = new System.Drawing.Point(129, 200);
			this.tbUserName.Name = "tbUserName";
			this.tbUserName.ReadOnly = true;
			this.tbUserName.Size = new System.Drawing.Size(297, 20);
			this.tbUserName.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 203);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(103, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Имя пользователя";
			// 
			// btnSaveChange
			// 
			this.btnSaveChange.BackColor = System.Drawing.Color.Bisque;
			this.btnSaveChange.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnSaveChange.Location = new System.Drawing.Point(3, 458);
			this.btnSaveChange.Name = "btnSaveChange";
			this.btnSaveChange.Size = new System.Drawing.Size(434, 23);
			this.btnSaveChange.TabIndex = 9;
			this.btnSaveChange.Text = "Сохранить изменения";
			this.btnSaveChange.UseVisualStyleBackColor = false;
			this.btnSaveChange.Click += new System.EventHandler(this.btnSaveChange_Click);
			// 
			// dgvUsers
			// 
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvUsers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvUsers.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmUsersName,
            this.clmComment,
            this.clmUserGroup,
            this.clmDateGentration,
            this.clmChangeUser,
            this.clmIdUser,
            this.clmUserPass});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Bisque;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvUsers.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Left;
			this.dgvUsers.GridColor = System.Drawing.SystemColors.Control;
			this.dgvUsers.Location = new System.Drawing.Point(0, 0);
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvUsers.Size = new System.Drawing.Size(583, 509);
			this.dgvUsers.TabIndex = 0;
			this.dgvUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
			// 
			// clmUsersName
			// 
			this.clmUsersName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
			this.clmUsersName.DefaultCellStyle = dataGridViewCellStyle1;
			this.clmUsersName.HeaderText = "Пользователь";
			this.clmUsersName.Name = "clmUsersName";
			this.clmUsersName.ReadOnly = true;
			this.clmUsersName.Width = 105;
			// 
			// clmComment
			// 
			this.clmComment.HeaderText = "Комментарий";
			this.clmComment.Name = "clmComment";
			this.clmComment.ReadOnly = true;
			// 
			// clmUserGroup
			// 
			this.clmUserGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.clmUserGroup.HeaderText = "Группа";
			this.clmUserGroup.Name = "clmUserGroup";
			this.clmUserGroup.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.clmUserGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.clmUserGroup.Width = 48;
			// 
			// clmDateGentration
			// 
			this.clmDateGentration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.clmDateGentration.HeaderText = "Дата создания";
			this.clmDateGentration.Name = "clmDateGentration";
			this.clmDateGentration.ReadOnly = true;
			this.clmDateGentration.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.clmDateGentration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.clmDateGentration.Width = 90;
			// 
			// clmChangeUser
			// 
			this.clmChangeUser.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.clmChangeUser.HeaderText = "";
			this.clmChangeUser.Name = "clmChangeUser";
			this.clmChangeUser.Width = 5;
			// 
			// clmIdUser
			// 
			this.clmIdUser.HeaderText = "";
			this.clmIdUser.Name = "clmIdUser";
			this.clmIdUser.ReadOnly = true;
			this.clmIdUser.Visible = false;
			// 
			// clmUserPass
			// 
			this.clmUserPass.HeaderText = "";
			this.clmUserPass.Name = "clmUserPass";
			this.clmUserPass.Visible = false;
			// 
			// tbGroupRights
			// 
			this.tbGroupRights.BackColor = System.Drawing.SystemColors.Control;
			this.tbGroupRights.Controls.Add(this.splitContainer2);
			this.tbGroupRights.Location = new System.Drawing.Point(4, 22);
			this.tbGroupRights.Name = "tbGroupRights";
			this.tbGroupRights.Padding = new System.Windows.Forms.Padding(3);
			this.tbGroupRights.Size = new System.Drawing.Size(1029, 572);
			this.tbGroupRights.TabIndex = 1;
			this.tbGroupRights.Text = "Группы и права групп";
			this.tbGroupRights.Paint += new System.Windows.Forms.PaintEventHandler(this.tbGroupRights_Paint);
			this.tbGroupRights.Enter += new System.EventHandler(this.tbGroupRights_Enter);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(3, 3);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.btnGroupDel);
			this.splitContainer2.Panel2.Controls.Add(this.btnGroupcreate);
			this.splitContainer2.Panel2.Controls.Add(this.btnGroupSaveChange);
			this.splitContainer2.Size = new System.Drawing.Size(1023, 566);
			this.splitContainer2.SplitterDistance = 377;
			this.splitContainer2.TabIndex = 0;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.dgvGroups);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
			this.splitContainer3.Size = new System.Drawing.Size(1023, 377);
			this.splitContainer3.SplitterDistance = 341;
			this.splitContainer3.TabIndex = 3;
			// 
			// dgvGroups
			// 
			this.dgvGroups.AllowUserToAddRows = false;
			this.dgvGroups.AllowUserToDeleteRows = false;
			this.dgvGroups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvGroups.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dgvGroups.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dgvGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmGroupName,
            this.clmGroupComment,
            this.clmGroupCreateData,
            this.clmGroupEditData,
            this.clmGroupRight,
            this.clmIdGroup,
            this.clmGroupMenuRight});
			this.dgvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvGroups.Location = new System.Drawing.Point(0, 0);
			this.dgvGroups.Name = "dgvGroups";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Linen;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.BlanchedAlmond;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvGroups.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvGroups.Size = new System.Drawing.Size(341, 377);
			this.dgvGroups.TabIndex = 1;
			this.dgvGroups.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGroups_CellClick);
			// 
			// clmGroupName
			// 
			this.clmGroupName.HeaderText = "Имя группы";
			this.clmGroupName.Name = "clmGroupName";
			// 
			// clmGroupComment
			// 
			this.clmGroupComment.HeaderText = "Комментарий";
			this.clmGroupComment.Name = "clmGroupComment";
			// 
			// clmGroupCreateData
			// 
			this.clmGroupCreateData.HeaderText = "Дата создания группы";
			this.clmGroupCreateData.Name = "clmGroupCreateData";
			this.clmGroupCreateData.ReadOnly = true;
			// 
			// clmGroupEditData
			// 
			this.clmGroupEditData.HeaderText = "Дата последнего редактирования группы";
			this.clmGroupEditData.Name = "clmGroupEditData";
			this.clmGroupEditData.ReadOnly = true;
			// 
			// clmGroupRight
			// 
			this.clmGroupRight.HeaderText = "";
			this.clmGroupRight.Name = "clmGroupRight";
			this.clmGroupRight.Visible = false;
			// 
			// clmIdGroup
			// 
			this.clmIdGroup.HeaderText = "";
			this.clmIdGroup.Name = "clmIdGroup";
			this.clmIdGroup.Visible = false;
			// 
			// clmGroupMenuRight
			// 
			this.clmGroupMenuRight.HeaderText = "";
			this.clmGroupMenuRight.Name = "clmGroupMenuRight";
			this.clmGroupMenuRight.Visible = false;
			// 
			// splitContainer4
			// 
			this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer4.Location = new System.Drawing.Point(0, 0);
			this.splitContainer4.Name = "splitContainer4";
			// 
			// splitContainer4.Panel1
			// 
			this.splitContainer4.Panel1.Controls.Add(this.gbRights);
			// 
			// splitContainer4.Panel2
			// 
			this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
			this.splitContainer4.Size = new System.Drawing.Size(678, 377);
			this.splitContainer4.SplitterDistance = 226;
			this.splitContainer4.TabIndex = 0;
			// 
			// gbRights
			// 
			this.gbRights.Controls.Add(this.flpGroupRights);
			this.gbRights.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbRights.Location = new System.Drawing.Point(0, 0);
			this.gbRights.Name = "gbRights";
			this.gbRights.Size = new System.Drawing.Size(226, 377);
			this.gbRights.TabIndex = 2;
			this.gbRights.TabStop = false;
			this.gbRights.Text = "Права группы";
			// 
			// flpGroupRights
			// 
			this.flpGroupRights.AutoScroll = true;
			this.flpGroupRights.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.flpGroupRights.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpGroupRights.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpGroupRights.Location = new System.Drawing.Point(3, 16);
			this.flpGroupRights.Name = "flpGroupRights";
			this.flpGroupRights.Size = new System.Drawing.Size(220, 358);
			this.flpGroupRights.TabIndex = 0;
			// 
			// splitContainer5
			// 
			this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer5.Location = new System.Drawing.Point(0, 0);
			this.splitContainer5.Name = "splitContainer5";
			// 
			// splitContainer5.Panel1
			// 
			this.splitContainer5.Panel1.Controls.Add(this.gbCustomRight);
			this.splitContainer5.Panel1Collapsed = true;
			// 
			// splitContainer5.Panel2
			// 
			this.splitContainer5.Panel2.Controls.Add(this.gbMenuTreeview);
			this.splitContainer5.Size = new System.Drawing.Size(448, 377);
			this.splitContainer5.SplitterDistance = 149;
			this.splitContainer5.TabIndex = 0;
			// 
			// gbCustomRight
			// 
			this.gbCustomRight.Controls.Add(this.dgvRights);
			this.gbCustomRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbCustomRight.Location = new System.Drawing.Point(0, 0);
			this.gbCustomRight.Name = "gbCustomRight";
			this.gbCustomRight.Size = new System.Drawing.Size(149, 100);
			this.gbCustomRight.TabIndex = 3;
			this.gbCustomRight.TabStop = false;
			this.gbCustomRight.Text = "Настройка прав (для всех пользователей)";
			// 
			// dgvRights
			// 
			this.dgvRights.AllowUserToAddRows = false;
			this.dgvRights.AllowUserToDeleteRows = false;
			this.dgvRights.AllowUserToResizeRows = false;
			this.dgvRights.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvRights.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dgvRights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvRights.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmRightsName,
            this.clmVedenieLoga,
            this.clmOutInLog,
            this.clmIdRightRecord});
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Linen;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvRights.DefaultCellStyle = dataGridViewCellStyle4;
			this.dgvRights.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvRights.Location = new System.Drawing.Point(3, 16);
			this.dgvRights.MultiSelect = false;
			this.dgvRights.Name = "dgvRights";
			this.dgvRights.Size = new System.Drawing.Size(143, 81);
			this.dgvRights.TabIndex = 0;
			// 
			// clmRightsName
			// 
			this.clmRightsName.HeaderText = "Название права";
			this.clmRightsName.Name = "clmRightsName";
			// 
			// clmVedenieLoga
			// 
			this.clmVedenieLoga.HeaderText = "Ведение лога";
			this.clmVedenieLoga.Name = "clmVedenieLoga";
			this.clmVedenieLoga.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// clmOutInLog
			// 
			this.clmOutInLog.HeaderText = "Вывод лога в журнал";
			this.clmOutInLog.Name = "clmOutInLog";
			this.clmOutInLog.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// clmIdRightRecord
			// 
			this.clmIdRightRecord.HeaderText = "";
			this.clmIdRightRecord.Name = "clmIdRightRecord";
			this.clmIdRightRecord.Visible = false;
			// 
			// gbMenuTreeview
			// 
			this.gbMenuTreeview.Controls.Add(this.treeViewHideMenu);
			this.gbMenuTreeview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gbMenuTreeview.Location = new System.Drawing.Point(0, 0);
			this.gbMenuTreeview.Name = "gbMenuTreeview";
			this.gbMenuTreeview.Size = new System.Drawing.Size(448, 377);
			this.gbMenuTreeview.TabIndex = 3;
			this.gbMenuTreeview.TabStop = false;
			this.gbMenuTreeview.Text = "Настройка доступа к системе меню";
			// 
			// treeViewHideMenu
			// 
			this.treeViewHideMenu.CheckBoxes = true;
			this.treeViewHideMenu.ContextMenuStrip = this.contextMenuStripMenuTreeView;
			this.treeViewHideMenu.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewHideMenu.Location = new System.Drawing.Point(3, 16);
			this.treeViewHideMenu.Name = "treeViewHideMenu";
			this.treeViewHideMenu.Size = new System.Drawing.Size(442, 358);
			this.treeViewHideMenu.TabIndex = 0;
			this.treeViewHideMenu.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewHideMenu_AfterCheck);
			this.treeViewHideMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewHideMenu_AfterCheck);
			// 
			// contextMenuStripMenuTreeView
			// 
			this.contextMenuStripMenuTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAccessOn,
            this.tsmiAccessOff});
			this.contextMenuStripMenuTreeView.Name = "contextMenuStripMenuTreeView";
			this.contextMenuStripMenuTreeView.Size = new System.Drawing.Size(141, 48);
			// 
			// tsmiAccessOn
			// 
			this.tsmiAccessOn.Name = "tsmiAccessOn";
			this.tsmiAccessOn.Size = new System.Drawing.Size(140, 22);
			this.tsmiAccessOn.Text = "Разрешить";
			// 
			// tsmiAccessOff
			// 
			this.tsmiAccessOff.Name = "tsmiAccessOff";
			this.tsmiAccessOff.Size = new System.Drawing.Size(140, 22);
			this.tsmiAccessOff.Text = "Запретить";
			// 
			// btnGroupDel
			// 
			this.btnGroupDel.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnGroupDel.Location = new System.Drawing.Point(0, 46);
			this.btnGroupDel.Name = "btnGroupDel";
			this.btnGroupDel.Size = new System.Drawing.Size(1023, 23);
			this.btnGroupDel.TabIndex = 3;
			this.btnGroupDel.Text = "Удаление группы";
			this.btnGroupDel.UseVisualStyleBackColor = true;
			this.btnGroupDel.Click += new System.EventHandler(this.btnGroupDel_Click);
			// 
			// btnGroupcreate
			// 
			this.btnGroupcreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnGroupcreate.Location = new System.Drawing.Point(0, 23);
			this.btnGroupcreate.Name = "btnGroupcreate";
			this.btnGroupcreate.Size = new System.Drawing.Size(1023, 23);
			this.btnGroupcreate.TabIndex = 2;
			this.btnGroupcreate.Text = "Создать группу";
			this.btnGroupcreate.UseVisualStyleBackColor = true;
			this.btnGroupcreate.Click += new System.EventHandler(this.btnGroupcreate_Click);
			// 
			// btnGroupSaveChange
			// 
			this.btnGroupSaveChange.AutoSize = true;
			this.btnGroupSaveChange.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnGroupSaveChange.Location = new System.Drawing.Point(0, 0);
			this.btnGroupSaveChange.Name = "btnGroupSaveChange";
			this.btnGroupSaveChange.Size = new System.Drawing.Size(1023, 23);
			this.btnGroupSaveChange.TabIndex = 0;
			this.btnGroupSaveChange.Text = "Сохранить изменения";
			this.btnGroupSaveChange.UseVisualStyleBackColor = true;
			this.btnGroupSaveChange.Click += new System.EventHandler(this.btnGroupSaveChange_Click);
			// 
			// frmUserGroupRights
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1037, 598);
			this.Controls.Add(this.tbUsers);
			this.Name = "frmUserGroupRights";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Управление доступом";
			this.Load += new System.EventHandler(this.frmUserGroupRights_Load);
			this.tbUsers.ResumeLayout(false);
			this.tabUsers.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.gbEditUserParameters.ResumeLayout(false);
			this.gbEditUserParameters.PerformLayout();
			this.pnlHelp.ResumeLayout(false);
			this.pnlHelp.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
			this.tbGroupRights.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).EndInit();
			this.splitContainer4.Panel1.ResumeLayout(false);
			this.splitContainer4.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
			this.splitContainer4.ResumeLayout(false);
			this.gbRights.ResumeLayout(false);
			this.splitContainer5.Panel1.ResumeLayout(false);
			this.splitContainer5.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
			this.splitContainer5.ResumeLayout(false);
			this.gbCustomRight.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvRights)).EndInit();
			this.gbMenuTreeview.ResumeLayout(false);
			this.contextMenuStripMenuTreeView.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbUsers;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tbGroupRights;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.GroupBox gbEditUserParameters;
        private System.Windows.Forms.Button btnSaveChange;
        private System.Windows.Forms.ComboBox cbUserGroup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChUserPass;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbInNewPassConfirm;
        private System.Windows.Forms.TextBox tbInNewPass;
        private System.Windows.Forms.Label lbNewPassConf;
        private System.Windows.Forms.Label lbInNewPass;
        private System.Windows.Forms.TextBox tbInOldPass;
        private System.Windows.Forms.Label lbInOldPass;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmUsersName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmUserGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDateGentration;
        private System.Windows.Forms.DataGridViewButtonColumn clmChangeUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmIdUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmUserPass;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnDelUser;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnGroupSaveChange;
        private System.Windows.Forms.Button btnGroupcreate;
        private System.Windows.Forms.Button btnGroupDel;
		 private System.Windows.Forms.ContextMenuStrip contextMenuStripMenuTreeView;
		 private System.Windows.Forms.ToolStripMenuItem tsmiAccessOn;
       private System.Windows.Forms.ToolStripMenuItem tsmiAccessOff;
       private System.Windows.Forms.Panel pnlHelp;
       private System.Windows.Forms.TextBox textBox1;
       private System.Windows.Forms.SplitContainer splitContainer3;
       private System.Windows.Forms.SplitContainer splitContainer4;
       private System.Windows.Forms.SplitContainer splitContainer5;
       private System.Windows.Forms.DataGridView dgvGroups;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmGroupName;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmGroupComment;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmGroupCreateData;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmGroupEditData;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmGroupRight;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmIdGroup;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmGroupMenuRight;
       private System.Windows.Forms.GroupBox gbRights;
       private System.Windows.Forms.FlowLayoutPanel flpGroupRights;
       private System.Windows.Forms.GroupBox gbCustomRight;
       private System.Windows.Forms.DataGridView dgvRights;
       private System.Windows.Forms.DataGridViewTextBoxColumn clmRightsName;
       private System.Windows.Forms.DataGridViewCheckBoxColumn clmVedenieLoga;
       private System.Windows.Forms.DataGridViewCheckBoxColumn clmOutInLog;
       private System.Windows.Forms.DataGridViewComboBoxColumn clmIdRightRecord;
       private System.Windows.Forms.GroupBox gbMenuTreeview;
       private System.Windows.Forms.TreeView treeViewHideMenu;
    }
}