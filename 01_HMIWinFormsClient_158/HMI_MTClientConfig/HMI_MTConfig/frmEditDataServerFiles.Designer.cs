namespace HMI_MTConfig
{
	partial class frmEditDataServerFiles
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnSameExch = new System.Windows.Forms.Button();
			this.btnReNew = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblInfoIP = new System.Windows.Forms.Label();
			this.lblInfoFileName = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSaveFile = new System.Windows.Forms.Button();
			this.btnLoadPrjDevCfg_cdp = new System.Windows.Forms.Button();
			this.btnLoadProject_cfg = new System.Windows.Forms.Button();
			this.rtbEdit = new System.Windows.Forms.RichTextBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
			this.splitContainer1.Panel1MinSize = 200;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.rtbEdit);
			this.splitContainer1.Size = new System.Drawing.Size(750, 590);
			this.splitContainer1.SplitterDistance = 200;
			this.splitContainer1.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnSameExch);
			this.groupBox1.Controls.Add(this.btnReNew);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.btnSaveFile);
			this.groupBox1.Controls.Add(this.btnLoadPrjDevCfg_cdp);
			this.groupBox1.Controls.Add(this.btnLoadProject_cfg);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 590);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Доступные файлы для редактирования";
			// 
			// btnSameExch
			// 
			this.btnSameExch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnSameExch.Location = new System.Drawing.Point(9, 204);
			this.btnSameExch.Name = "btnSameExch";
			this.btnSameExch.Size = new System.Drawing.Size(185, 23);
			this.btnSameExch.TabIndex = 5;
			this.btnSameExch.Text = "Сохранить изменения";
			this.btnSameExch.UseVisualStyleBackColor = true;
			this.btnSameExch.Click += new System.EventHandler(this.btnSameExch_Click);
			// 
			// btnReNew
			// 
			this.btnReNew.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnReNew.Location = new System.Drawing.Point(9, 146);
			this.btnReNew.Name = "btnReNew";
			this.btnReNew.Size = new System.Drawing.Size(185, 52);
			this.btnReNew.TabIndex = 4;
			this.btnReNew.Text = "Обновить (при внешнем редактировании)";
			this.btnReNew.UseVisualStyleBackColor = true;
			this.btnReNew.Click += new System.EventHandler(this.btnReNew_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lblInfoIP);
			this.groupBox2.Controls.Add(this.lblInfoFileName);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox2.Location = new System.Drawing.Point(3, 487);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(194, 100);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Информация о файле";
			// 
			// lblInfoIP
			// 
			this.lblInfoIP.AutoSize = true;
			this.lblInfoIP.Location = new System.Drawing.Point(128, 48);
			this.lblInfoIP.Name = "lblInfoIP";
			this.lblInfoIP.Size = new System.Drawing.Size(0, 13);
			this.lblInfoIP.TabIndex = 3;
			// 
			// lblInfoFileName
			// 
			this.lblInfoFileName.AutoSize = true;
			this.lblInfoFileName.Location = new System.Drawing.Point(128, 26);
			this.lblInfoFileName.Name = "lblInfoFileName";
			this.lblInfoFileName.Size = new System.Drawing.Size(0, 13);
			this.lblInfoFileName.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(106, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "ip-адрес Dataserver:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(102, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Загруженый файл:";
			// 
			// btnSaveFile
			// 
			this.btnSaveFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveFile.Location = new System.Drawing.Point(9, 233);
			this.btnSaveFile.Name = "btnSaveFile";
			this.btnSaveFile.Size = new System.Drawing.Size(185, 23);
			this.btnSaveFile.TabIndex = 2;
			this.btnSaveFile.Text = "Сохранить текущий файл";
			this.btnSaveFile.UseVisualStyleBackColor = true;
			this.btnSaveFile.Click += new System.EventHandler(this.btnLoadFile_Click);
			// 
			// btnLoadPrjDevCfg_cdp
			// 
			this.btnLoadPrjDevCfg_cdp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadPrjDevCfg_cdp.Location = new System.Drawing.Point(6, 80);
			this.btnLoadPrjDevCfg_cdp.Name = "btnLoadPrjDevCfg_cdp";
			this.btnLoadPrjDevCfg_cdp.Size = new System.Drawing.Size(188, 23);
			this.btnLoadPrjDevCfg_cdp.TabIndex = 1;
			this.btnLoadPrjDevCfg_cdp.Text = "Загрузить PrjDevCfg.cdp";
			this.btnLoadPrjDevCfg_cdp.UseVisualStyleBackColor = true;
			this.btnLoadPrjDevCfg_cdp.Click += new System.EventHandler(this.btnLoadFile_Click);
			// 
			// btnLoadProject_cfg
			// 
			this.btnLoadProject_cfg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadProject_cfg.Location = new System.Drawing.Point(6, 51);
			this.btnLoadProject_cfg.Name = "btnLoadProject_cfg";
			this.btnLoadProject_cfg.Size = new System.Drawing.Size(188, 23);
			this.btnLoadProject_cfg.TabIndex = 0;
			this.btnLoadProject_cfg.Text = "Загрузить Project.cfg";
			this.btnLoadProject_cfg.UseVisualStyleBackColor = true;
			this.btnLoadProject_cfg.Click += new System.EventHandler(this.btnLoadFile_Click);
			// 
			// rtbEdit
			// 
			this.rtbEdit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbEdit.Location = new System.Drawing.Point(0, 0);
			this.rtbEdit.Name = "rtbEdit";
			this.rtbEdit.Size = new System.Drawing.Size(546, 590);
			this.rtbEdit.TabIndex = 0;
			this.rtbEdit.Text = "";
			// 
			// frmEditDataServerFiles
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(750, 590);
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmEditDataServerFiles";
			this.Text = "Редактирование конф. файлов DataServer";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnSaveFile;
		private System.Windows.Forms.Button btnLoadPrjDevCfg_cdp;
		private System.Windows.Forms.Button btnLoadProject_cfg;
		private System.Windows.Forms.RichTextBox rtbEdit;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label lblInfoIP;
		private System.Windows.Forms.Label lblInfoFileName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnReNew;
		private System.Windows.Forms.Button btnSameExch;
	}
}