namespace HMI_MTConfig
{
    partial class frm4btnSQLConnection
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.cbTypeAutent = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.dgwSQLParams = new System.Windows.Forms.DataGridView();
			this.btnVerifaySQLServers = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgwSQLParams)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btnVerifaySQLServers);
			this.splitContainer1.Size = new System.Drawing.Size(292, 266);
			this.splitContainer1.SplitterDistance = 207;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer2
			// 
			this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer2.IsSplitterFixed = true;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.cbTypeAutent);
			this.splitContainer2.Panel1.Controls.Add(this.label1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.dgwSQLParams);
			this.splitContainer2.Size = new System.Drawing.Size(292, 207);
			this.splitContainer2.SplitterDistance = 120;
			this.splitContainer2.TabIndex = 0;
			// 
			// cbTypeAutent
			// 
			this.cbTypeAutent.FormattingEnabled = true;
			this.cbTypeAutent.Items.AddRange(new object[] {
            "Windows",
            "SQL"});
			this.cbTypeAutent.Location = new System.Drawing.Point(3, 31);
			this.cbTypeAutent.Name = "cbTypeAutent";
			this.cbTypeAutent.Size = new System.Drawing.Size(112, 21);
			this.cbTypeAutent.TabIndex = 1;
			this.cbTypeAutent.SelectedIndexChanged += new System.EventHandler(this.cbTypeAutent_SelectedIndexChanged);
			this.cbTypeAutent.Click += new System.EventHandler(this.cbTypeAutent_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(115, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Тип аутентификации:";
			// 
			// dgwSQLParams
			// 
			this.dgwSQLParams.AllowUserToAddRows = false;
			this.dgwSQLParams.AllowUserToDeleteRows = false;
			this.dgwSQLParams.AllowUserToOrderColumns = true;
			this.dgwSQLParams.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgwSQLParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgwSQLParams.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgwSQLParams.Location = new System.Drawing.Point(0, 0);
			this.dgwSQLParams.Name = "dgwSQLParams";
			this.dgwSQLParams.Size = new System.Drawing.Size(166, 205);
			this.dgwSQLParams.TabIndex = 0;
			this.dgwSQLParams.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgwSQLParams_MouseClick);
			// 
			// btnVerifaySQLServers
			// 
			this.btnVerifaySQLServers.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnVerifaySQLServers.AutoSize = true;
			this.btnVerifaySQLServers.Location = new System.Drawing.Point(136, 19);
			this.btnVerifaySQLServers.Name = "btnVerifaySQLServers";
			this.btnVerifaySQLServers.Size = new System.Drawing.Size(143, 23);
			this.btnVerifaySQLServers.TabIndex = 0;
			this.btnVerifaySQLServers.Text = "Доступные SQL-сервера";
			this.btnVerifaySQLServers.UseVisualStyleBackColor = true;
			this.btnVerifaySQLServers.Click += new System.EventHandler(this.btnVerifaySQLServers_Click);
			// 
			// frm4btnSQLConnection
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frm4btnSQLConnection";
			this.Text = "frm4btnSQLConnection";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgwSQLParams)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgwSQLParams;
        private System.Windows.Forms.ComboBox cbTypeAutent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnVerifaySQLServers;
    }
}