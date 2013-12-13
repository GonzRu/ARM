namespace HMI_MTConfig
{
   partial class Form1
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
		  this.btnGentralCustomise = new System.Windows.Forms.Button();
		  this.btnControlDiagOsc = new System.Windows.Forms.Button();
		  this.btnEditDataServerFiles = new System.Windows.Forms.Button();
		  this.btnSecurity = new System.Windows.Forms.Button();
		  this.btnCustomTcpIp = new System.Windows.Forms.Button();
		  this.btnSQLConnection = new System.Windows.Forms.Button();
		  this.btnMnemo = new System.Windows.Forms.Button();
		  this.btnPublicPrjCaption = new System.Windows.Forms.Button();
		  this.splitContainer2 = new System.Windows.Forms.SplitContainer();
		  this.splitContainer3 = new System.Windows.Forms.SplitContainer();
		  this.btnCaption = new System.Windows.Forms.Button();
		  this.btnCancel = new System.Windows.Forms.Button();
		  this.btnApply = new System.Windows.Forms.Button();
		  this.btnSaveAndExit = new System.Windows.Forms.Button();
		  this.btnOk = new System.Windows.Forms.Button();
		  ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
		  this.splitContainer1.Panel1.SuspendLayout();
		  this.splitContainer1.Panel2.SuspendLayout();
		  this.splitContainer1.SuspendLayout();
		  ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
		  this.splitContainer2.Panel1.SuspendLayout();
		  this.splitContainer2.Panel2.SuspendLayout();
		  this.splitContainer2.SuspendLayout();
		  ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
		  this.splitContainer3.Panel1.SuspendLayout();
		  this.splitContainer3.SuspendLayout();
		  this.SuspendLayout();
		  // 
		  // splitContainer1
		  // 
		  this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		  this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		  this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		  this.splitContainer1.IsSplitterFixed = true;
		  this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		  this.splitContainer1.Name = "splitContainer1";
		  // 
		  // splitContainer1.Panel1
		  // 
		  this.splitContainer1.Panel1.Controls.Add(this.btnGentralCustomise);
		  this.splitContainer1.Panel1.Controls.Add(this.btnControlDiagOsc);
		  this.splitContainer1.Panel1.Controls.Add(this.btnEditDataServerFiles);
		  this.splitContainer1.Panel1.Controls.Add(this.btnSecurity);
		  this.splitContainer1.Panel1.Controls.Add(this.btnCustomTcpIp);
		  this.splitContainer1.Panel1.Controls.Add(this.btnSQLConnection);
		  this.splitContainer1.Panel1.Controls.Add(this.btnMnemo);
		  this.splitContainer1.Panel1.Controls.Add(this.btnPublicPrjCaption);
		  // 
		  // splitContainer1.Panel2
		  // 
		  this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
		  this.splitContainer1.Size = new System.Drawing.Size(642, 542);
		  this.splitContainer1.SplitterDistance = 204;
		  this.splitContainer1.TabIndex = 0;
		  // 
		  // btnGentralCustomise
		  // 
		  this.btnGentralCustomise.Dock = System.Windows.Forms.DockStyle.Top;
		  this.btnGentralCustomise.Location = new System.Drawing.Point(0, 196);
		  this.btnGentralCustomise.Name = "btnGentralCustomise";
		  this.btnGentralCustomise.Size = new System.Drawing.Size(202, 23);
		  this.btnGentralCustomise.TabIndex = 25;
		  this.btnGentralCustomise.Text = "Общие настройки";
		  this.btnGentralCustomise.UseVisualStyleBackColor = true;
		  this.btnGentralCustomise.Click += new System.EventHandler(this.btnLeft_Click);
		  // 
		  // btnControlDiagOsc
		  // 
		  this.btnControlDiagOsc.Dock = System.Windows.Forms.DockStyle.Top;
		  this.btnControlDiagOsc.Location = new System.Drawing.Point(0, 173);
		  this.btnControlDiagOsc.Name = "btnControlDiagOsc";
		  this.btnControlDiagOsc.Size = new System.Drawing.Size(202, 23);
		  this.btnControlDiagOsc.TabIndex = 24;
		  this.btnControlDiagOsc.Text = "Осциллограммы и диаграммы";
		  this.btnControlDiagOsc.UseVisualStyleBackColor = true;
		  this.btnControlDiagOsc.Click += new System.EventHandler(this.btnLeft_Click);
		  // 
		  // btnEditDataServerFiles
		  // 
		  this.btnEditDataServerFiles.Dock = System.Windows.Forms.DockStyle.Top;
		  this.btnEditDataServerFiles.Location = new System.Drawing.Point(0, 131);
		  this.btnEditDataServerFiles.Name = "btnEditDataServerFiles";
		  this.btnEditDataServerFiles.Size = new System.Drawing.Size(202, 42);
		  this.btnEditDataServerFiles.TabIndex = 23;
		  this.btnEditDataServerFiles.Text = "Редактировать файлы DataServer";
		  this.btnEditDataServerFiles.UseVisualStyleBackColor = true;
		  this.btnEditDataServerFiles.Click += new System.EventHandler(this.btnLeft_Click);
		  // 
		  // btnSecurity
		  // 
		  this.btnSecurity.Dock = System.Windows.Forms.DockStyle.Top;
		  this.btnSecurity.Location = new System.Drawing.Point(0, 108);
		  this.btnSecurity.Name = "btnSecurity";
		  this.btnSecurity.Size = new System.Drawing.Size(202, 23);
		  this.btnSecurity.TabIndex = 4;
		  this.btnSecurity.Text = "Безопасность";
		  this.btnSecurity.UseVisualStyleBackColor = true;
		  this.btnSecurity.Click += new System.EventHandler(this.btnLeft_Click);
		  // 
		  // btnCustomTcpIp
		  // 
		  this.btnCustomTcpIp.Dock = System.Windows.Forms.DockStyle.Top;
		  this.btnCustomTcpIp.Location = new System.Drawing.Point(0, 85);
		  this.btnCustomTcpIp.Name = "btnCustomTcpIp";
		  this.btnCustomTcpIp.Size = new System.Drawing.Size(202, 23);
		  this.btnCustomTcpIp.TabIndex = 3;
		  this.btnCustomTcpIp.Text = "IP-адреса, порты";
		  this.btnCustomTcpIp.UseVisualStyleBackColor = true;
		  this.btnCustomTcpIp.Click += new System.EventHandler(this.btnLeft_Click);
		  // 
		  // btnSQLConnection
		  // 
		  this.btnSQLConnection.Dock = System.Windows.Forms.DockStyle.Top;
		  this.btnSQLConnection.Location = new System.Drawing.Point(0, 55);
		  this.btnSQLConnection.Name = "btnSQLConnection";
		  this.btnSQLConnection.Size = new System.Drawing.Size(202, 30);
		  this.btnSQLConnection.TabIndex = 2;
		  this.btnSQLConnection.Text = "Настройки соединения с БД";
		  this.btnSQLConnection.UseVisualStyleBackColor = true;
		  this.btnSQLConnection.Click += new System.EventHandler(this.btnLeft_Click);
		  // 
		  // btnMnemo
		  // 
		  this.btnMnemo.Dock = System.Windows.Forms.DockStyle.Top;
		  this.btnMnemo.Location = new System.Drawing.Point(0, 32);
		  this.btnMnemo.Name = "btnMnemo";
		  this.btnMnemo.Size = new System.Drawing.Size(202, 23);
		  this.btnMnemo.TabIndex = 1;
		  this.btnMnemo.Text = "Мнемосхема";
		  this.btnMnemo.UseVisualStyleBackColor = true;
		  this.btnMnemo.Click += new System.EventHandler(this.btnLeft_Click);
		  // 
		  // btnPublicPrjCaption
		  // 
		  this.btnPublicPrjCaption.Dock = System.Windows.Forms.DockStyle.Top;
		  this.btnPublicPrjCaption.Location = new System.Drawing.Point(0, 0);
		  this.btnPublicPrjCaption.Name = "btnPublicPrjCaption";
		  this.btnPublicPrjCaption.Size = new System.Drawing.Size(202, 32);
		  this.btnPublicPrjCaption.TabIndex = 0;
		  this.btnPublicPrjCaption.Text = "Общие названия проекта";
		  this.btnPublicPrjCaption.UseVisualStyleBackColor = true;
		  this.btnPublicPrjCaption.Click += new System.EventHandler(this.btnLeft_Click);
		  // 
		  // splitContainer2
		  // 
		  this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		  this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
		  this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
		  this.splitContainer2.IsSplitterFixed = true;
		  this.splitContainer2.Location = new System.Drawing.Point(0, 0);
		  this.splitContainer2.Name = "splitContainer2";
		  this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
		  // 
		  // splitContainer2.Panel1
		  // 
		  this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
		  // 
		  // splitContainer2.Panel2
		  // 
		  this.splitContainer2.Panel2.Controls.Add(this.btnCancel);
		  this.splitContainer2.Panel2.Controls.Add(this.btnApply);
		  this.splitContainer2.Panel2.Controls.Add(this.btnSaveAndExit);
		  this.splitContainer2.Panel2.Controls.Add(this.btnOk);
		  this.splitContainer2.Size = new System.Drawing.Size(434, 542);
		  this.splitContainer2.SplitterDistance = 515;
		  this.splitContainer2.SplitterWidth = 2;
		  this.splitContainer2.TabIndex = 0;
		  // 
		  // splitContainer3
		  // 
		  this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
		  this.splitContainer3.Location = new System.Drawing.Point(0, 0);
		  this.splitContainer3.Name = "splitContainer3";
		  this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
		  // 
		  // splitContainer3.Panel1
		  // 
		  this.splitContainer3.Panel1.BackColor = System.Drawing.Color.LightSalmon;
		  this.splitContainer3.Panel1.Controls.Add(this.btnCaption);
		  this.splitContainer3.Size = new System.Drawing.Size(432, 513);
		  this.splitContainer3.SplitterDistance = 27;
		  this.splitContainer3.TabIndex = 1;
		  // 
		  // btnCaption
		  // 
		  this.btnCaption.AutoSize = true;
		  this.btnCaption.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		  this.btnCaption.Dock = System.Windows.Forms.DockStyle.Fill;
		  this.btnCaption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		  this.btnCaption.Location = new System.Drawing.Point(0, 0);
		  this.btnCaption.Name = "btnCaption";
		  this.btnCaption.Size = new System.Drawing.Size(432, 27);
		  this.btnCaption.TabIndex = 0;
		  this.btnCaption.UseVisualStyleBackColor = true;
		  // 
		  // btnCancel
		  // 
		  this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
		  this.btnCancel.Location = new System.Drawing.Point(320, 0);
		  this.btnCancel.Name = "btnCancel";
		  this.btnCancel.Size = new System.Drawing.Size(75, 23);
		  this.btnCancel.TabIndex = 4;
		  this.btnCancel.Text = "Отменить";
		  this.btnCancel.UseVisualStyleBackColor = true;
		  this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
		  // 
		  // btnApply
		  // 
		  this.btnApply.Dock = System.Windows.Forms.DockStyle.Left;
		  this.btnApply.Location = new System.Drawing.Point(245, 0);
		  this.btnApply.Name = "btnApply";
		  this.btnApply.Size = new System.Drawing.Size(75, 23);
		  this.btnApply.TabIndex = 3;
		  this.btnApply.Text = "Применить";
		  this.btnApply.UseVisualStyleBackColor = true;
		  this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
		  // 
		  // btnSaveAndExit
		  // 
		  this.btnSaveAndExit.AutoSize = true;
		  this.btnSaveAndExit.Dock = System.Windows.Forms.DockStyle.Left;
		  this.btnSaveAndExit.Location = new System.Drawing.Point(132, 0);
		  this.btnSaveAndExit.Name = "btnSaveAndExit";
		  this.btnSaveAndExit.Size = new System.Drawing.Size(113, 23);
		  this.btnSaveAndExit.TabIndex = 2;
		  this.btnSaveAndExit.Text = "Сохранить и выйти";
		  this.btnSaveAndExit.UseVisualStyleBackColor = true;
		  this.btnSaveAndExit.Click += new System.EventHandler(this.btnSaveAndExit_Click);
		  // 
		  // btnOk
		  // 
		  this.btnOk.AutoSize = true;
		  this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
		  this.btnOk.Dock = System.Windows.Forms.DockStyle.Left;
		  this.btnOk.Location = new System.Drawing.Point(0, 0);
		  this.btnOk.Name = "btnOk";
		  this.btnOk.Size = new System.Drawing.Size(132, 23);
		  this.btnOk.TabIndex = 0;
		  this.btnOk.Text = "Выйти без сохранения";
		  this.btnOk.UseVisualStyleBackColor = true;
		  this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
		  // 
		  // Form1
		  // 
		  this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		  this.ClientSize = new System.Drawing.Size(642, 542);
		  this.ControlBox = false;
		  this.Controls.Add(this.splitContainer1);
		  this.MinimumSize = new System.Drawing.Size(650, 550);
		  this.Name = "Form1";
		  this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		  this.Text = "Настройки клиента ПТК \"Эгида\"";
		  this.splitContainer1.Panel1.ResumeLayout(false);
		  this.splitContainer1.Panel2.ResumeLayout(false);
		  ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
		  this.splitContainer1.ResumeLayout(false);
		  this.splitContainer2.Panel1.ResumeLayout(false);
		  this.splitContainer2.Panel2.ResumeLayout(false);
		  this.splitContainer2.Panel2.PerformLayout();
		  ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
		  this.splitContainer2.ResumeLayout(false);
		  this.splitContainer3.Panel1.ResumeLayout(false);
		  this.splitContainer3.Panel1.PerformLayout();
		  ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
		  this.splitContainer3.ResumeLayout(false);
		  this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.Button btnSecurity;
      private System.Windows.Forms.Button btnCustomTcpIp;
      private System.Windows.Forms.Button btnSQLConnection;
      private System.Windows.Forms.Button btnMnemo;
      private System.Windows.Forms.Button btnPublicPrjCaption;
      private System.Windows.Forms.SplitContainer splitContainer2;
      private System.Windows.Forms.SplitContainer splitContainer3;
      private System.Windows.Forms.Button btnCaption;
      private System.Windows.Forms.Button btnSaveAndExit;
      private System.Windows.Forms.Button btnOk;
      private System.Windows.Forms.Button btnApply;
      private System.Windows.Forms.Button btnCancel;
	  private System.Windows.Forms.Button btnEditDataServerFiles;
	  private System.Windows.Forms.Button btnControlDiagOsc;
	  private System.Windows.Forms.Button btnGentralCustomise;
   }
}

