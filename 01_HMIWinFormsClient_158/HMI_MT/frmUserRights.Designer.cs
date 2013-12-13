namespace HMI_MT
{
    partial class frmUserRights
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
            this.splitContainer_UserRights = new System.Windows.Forms.SplitContainer();
            this.pnlRights = new System.Windows.Forms.Panel();
            this.lstvRignts = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.chBitNum = new System.Windows.Forms.ColumnHeader();
            this.chRightName = new System.Windows.Forms.ColumnHeader();
            this.chArchiv = new System.Windows.Forms.ColumnHeader();
            this.chToLog = new System.Windows.Forms.ColumnHeader();
            this.pnlGroups = new System.Windows.Forms.Panel();
            this.lstvGroups = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.chIdGroup = new System.Windows.Forms.ColumnHeader();
            this.chGroupName = new System.Windows.Forms.ColumnHeader();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.lstvUser = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.chIdUser = new System.Windows.Forms.ColumnHeader();
            this.chIdName = new System.Windows.Forms.ColumnHeader();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.btnDeleteGroup = new System.Windows.Forms.Button();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ñèñòåìàToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ïå÷àòüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer_UserRights.Panel1.SuspendLayout();
            this.splitContainer_UserRights.Panel2.SuspendLayout();
            this.splitContainer_UserRights.SuspendLayout();
            this.pnlRights.SuspendLayout();
            this.pnlGroups.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_UserRights
            // 
            this.splitContainer_UserRights.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer_UserRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_UserRights.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer_UserRights.Name = "splitContainer_UserRights";
            this.splitContainer_UserRights.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_UserRights.Panel1
            // 
            this.splitContainer_UserRights.Panel1.Controls.Add( this.pnlRights );
            this.splitContainer_UserRights.Panel1.Controls.Add( this.pnlGroups );
            this.splitContainer_UserRights.Panel1.Controls.Add( this.pnlUser );
            // 
            // splitContainer_UserRights.Panel2
            // 
            this.splitContainer_UserRights.Panel2.Controls.Add( this.btnSaveChanges );
            this.splitContainer_UserRights.Panel2.Controls.Add( this.btnDeleteGroup );
            this.splitContainer_UserRights.Panel2.Controls.Add( this.btnAddGroup );
            this.splitContainer_UserRights.Panel2.Controls.Add( this.btnDeleteUser );
            this.splitContainer_UserRights.Panel2.Controls.Add( this.btnAddUser );
            this.splitContainer_UserRights.Size = new System.Drawing.Size( 1041, 736 );
            this.splitContainer_UserRights.SplitterDistance = 572;
            this.splitContainer_UserRights.SplitterWidth = 6;
            this.splitContainer_UserRights.TabIndex = 0;
            // 
            // pnlRights
            // 
            this.pnlRights.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRights.Controls.Add( this.lstvRignts );
            this.pnlRights.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRights.Location = new System.Drawing.Point( 687, 0 );
            this.pnlRights.Name = "pnlRights";
            this.pnlRights.Size = new System.Drawing.Size( 354, 572 );
            this.pnlRights.TabIndex = 2;
            // 
            // lstvRignts
            // 
            this.lstvRignts.BackColor = System.Drawing.Color.Snow;
            this.lstvRignts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstvRignts.CheckBoxes = true;
            this.lstvRignts.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.chBitNum,
            this.chRightName,
            this.chArchiv,
            this.chToLog} );
            this.lstvRignts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvRignts.GridLines = true;
            this.lstvRignts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvRignts.Location = new System.Drawing.Point( 0, 0 );
            this.lstvRignts.Name = "lstvRignts";
            this.lstvRignts.Size = new System.Drawing.Size( 352, 570 );
            this.lstvRignts.TabIndex = 0;
            this.lstvRignts.UseCompatibleStateImageBehavior = false;
            this.lstvRignts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "";
            this.columnHeader3.Width = 20;
            // 
            // chBitNum
            // 
            this.chBitNum.Text = "Áèò";
            this.chBitNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chBitNum.Width = 40;
            // 
            // chRightName
            // 
            this.chRightName.Text = "Îïèñàíèå";
            this.chRightName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chRightName.Width = 179;
            // 
            // chArchiv
            // 
            this.chArchiv.Text = "Àðõèâ";
            this.chArchiv.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chToLog
            // 
            this.chToLog.Text = "Â æóðíàë";
            this.chToLog.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chToLog.Width = 65;
            // 
            // pnlGroups
            // 
            this.pnlGroups.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGroups.Controls.Add( this.lstvGroups );
            this.pnlGroups.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlGroups.Location = new System.Drawing.Point( 300, 0 );
            this.pnlGroups.Name = "pnlGroups";
            this.pnlGroups.Size = new System.Drawing.Size( 385, 572 );
            this.pnlGroups.TabIndex = 1;
            // 
            // lstvGroups
            // 
            this.lstvGroups.BackColor = System.Drawing.Color.Snow;
            this.lstvGroups.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstvGroups.CheckBoxes = true;
            this.lstvGroups.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.chIdGroup,
            this.chGroupName} );
            this.lstvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvGroups.GridLines = true;
            this.lstvGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvGroups.Location = new System.Drawing.Point( 0, 0 );
            this.lstvGroups.Name = "lstvGroups";
            this.lstvGroups.Size = new System.Drawing.Size( 383, 570 );
            this.lstvGroups.TabIndex = 0;
            this.lstvGroups.UseCompatibleStateImageBehavior = false;
            this.lstvGroups.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "";
            this.columnHeader2.Width = 20;
            // 
            // chIdGroup
            // 
            this.chIdGroup.Text = "Èäåíò-ð";
            this.chIdGroup.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chIdGroup.Width = 80;
            // 
            // chGroupName
            // 
            this.chGroupName.Text = "Èìÿ ãðóïïû";
            this.chGroupName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chGroupName.Width = 278;
            // 
            // pnlUser
            // 
            this.pnlUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUser.Controls.Add( this.lstvUser );
            this.pnlUser.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlUser.Location = new System.Drawing.Point( 0, 0 );
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size( 300, 572 );
            this.pnlUser.TabIndex = 0;
            // 
            // lstvUser
            // 
            this.lstvUser.BackColor = System.Drawing.Color.Snow;
            this.lstvUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstvUser.CheckBoxes = true;
            this.lstvUser.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.chIdUser,
            this.chIdName} );
            this.lstvUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvUser.GridLines = true;
            this.lstvUser.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvUser.Location = new System.Drawing.Point( 0, 0 );
            this.lstvUser.MultiSelect = false;
            this.lstvUser.Name = "lstvUser";
            this.lstvUser.Size = new System.Drawing.Size( 298, 570 );
            this.lstvUser.TabIndex = 0;
            this.lstvUser.UseCompatibleStateImageBehavior = false;
            this.lstvUser.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 20;
            // 
            // chIdUser
            // 
            this.chIdUser.Text = "Èäåíò-ð";
            this.chIdUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chIdUser.Width = 70;
            // 
            // chIdName
            // 
            this.chIdName.Text = "Èìÿ ïîëüçîâàòåëÿ";
            this.chIdName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chIdName.Width = 191;
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.BackColor = System.Drawing.Color.SandyBrown;
            this.btnSaveChanges.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSaveChanges.Location = new System.Drawing.Point( 0, 92 );
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size( 1041, 23 );
            this.btnSaveChanges.TabIndex = 4;
            this.btnSaveChanges.Text = "Ñîõðàíèòü èçìåíåíèÿ";
            this.btnSaveChanges.UseVisualStyleBackColor = false;
            // 
            // btnDeleteGroup
            // 
            this.btnDeleteGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDeleteGroup.Location = new System.Drawing.Point( 0, 69 );
            this.btnDeleteGroup.Name = "btnDeleteGroup";
            this.btnDeleteGroup.Size = new System.Drawing.Size( 1041, 23 );
            this.btnDeleteGroup.TabIndex = 3;
            this.btnDeleteGroup.Text = "Óäàëèòü ãðóïïó";
            this.btnDeleteGroup.UseVisualStyleBackColor = true;
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddGroup.Location = new System.Drawing.Point( 0, 46 );
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size( 1041, 23 );
            this.btnAddGroup.TabIndex = 2;
            this.btnAddGroup.Text = "Äîáàâèòü ãðóïïó";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDeleteUser.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
            this.btnDeleteUser.Location = new System.Drawing.Point( 0, 23 );
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size( 1041, 23 );
            this.btnDeleteUser.TabIndex = 1;
            this.btnDeleteUser.Text = "Óäàëèòü ïîëüçîâàòåëÿ";
            this.btnDeleteUser.UseVisualStyleBackColor = false;
            // 
            // btnAddUser
            // 
            this.btnAddUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddUser.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
            this.btnAddUser.Location = new System.Drawing.Point( 0, 0 );
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size( 1041, 23 );
            this.btnAddUser.TabIndex = 0;
            this.btnAddUser.Text = "Äîáàâèòü ïîëüçîâàòåëÿ";
            this.btnAddUser.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.ñèñòåìàToolStripMenuItem} );
            this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size( 1041, 24 );
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // ñèñòåìàToolStripMenuItem
            // 
            this.ñèñòåìàToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.ïå÷àòüToolStripMenuItem} );
            this.ñèñòåìàToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.ñèñòåìàToolStripMenuItem.Name = "ñèñòåìàToolStripMenuItem";
            this.ñèñòåìàToolStripMenuItem.Size = new System.Drawing.Size( 61, 20 );
            this.ñèñòåìàToolStripMenuItem.Text = "Ñèñòåìà";
            // 
            // ïå÷àòüToolStripMenuItem
            // 
            this.ïå÷àòüToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.ïå÷àòüToolStripMenuItem.Name = "ïå÷àòüToolStripMenuItem";
            this.ïå÷àòüToolStripMenuItem.Size = new System.Drawing.Size( 122, 22 );
            this.ïå÷àòüToolStripMenuItem.Text = "Ïå÷àòü";
            // 
            // frmUserRights
            // 
            this.ClientSize = new System.Drawing.Size( 543, 345 );
            this.Name = "frmUserRights";
            this.splitContainer_UserRights.Panel1.ResumeLayout( false );
            this.splitContainer_UserRights.Panel2.ResumeLayout( false );
            this.splitContainer_UserRights.ResumeLayout( false );
            this.pnlRights.ResumeLayout( false );
            this.pnlGroups.ResumeLayout( false );
            this.pnlUser.ResumeLayout( false );
            this.menuStrip1.ResumeLayout( false );
            this.menuStrip1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_UserRights;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Panel pnlRights;
        private System.Windows.Forms.Panel pnlGroups;
        private System.Windows.Forms.Panel pnlUser;
        private System.Windows.Forms.Button btnDeleteGroup;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ñèñòåìàToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ïå÷àòüToolStripMenuItem;
        private System.Windows.Forms.ListView lstvRignts;
        private System.Windows.Forms.ListView lstvGroups;
        private System.Windows.Forms.ListView lstvUser;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader chIdUser;
        private System.Windows.Forms.ColumnHeader chIdName;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader chIdGroup;
        private System.Windows.Forms.ColumnHeader chGroupName;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader chBitNum;
        private System.Windows.Forms.ColumnHeader chRightName;
        private System.Windows.Forms.ColumnHeader chArchiv;
        private System.Windows.Forms.ColumnHeader chToLog;
        private System.Windows.Forms.Button btnSaveChanges;
    }
}