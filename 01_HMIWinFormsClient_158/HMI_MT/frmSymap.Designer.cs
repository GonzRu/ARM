namespace HMI_MT
{
	partial class frmSymap
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tbpPacketViewer = new System.Windows.Forms.TabPage();
			this.panel3 = new System.Windows.Forms.Panel();
			this.splitContainer8 = new System.Windows.Forms.SplitContainer();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.rbClm16 = new System.Windows.Forms.RadioButton();
			this.rbClm8 = new System.Windows.Forms.RadioButton();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label14 = new System.Windows.Forms.Label();
			this.cbEncode = new System.Windows.Forms.ComboBox();
			this.btnOutToFile = new System.Windows.Forms.Button();
			this.btnPrint = new System.Windows.Forms.Button();
			this.cbAvailablePackets = new System.Windows.Forms.ComboBox();
			this.lstvDump = new System.Windows.Forms.ListView();
			this.btnOutInSingleWindow = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tbpPacketViewer.SuspendLayout();
			this.panel3.SuspendLayout();
			this.splitContainer8.Panel2.SuspendLayout();
			this.splitContainer8.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add( this.tbpPacketViewer );
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point( 0, 0 );
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size( 1233, 811 );
			this.tabControl1.TabIndex = 3;
			// 
			// tbpPacketViewer
			// 
			this.tbpPacketViewer.BackColor = System.Drawing.Color.Transparent;
			this.tbpPacketViewer.Controls.Add( this.panel3 );
			this.tbpPacketViewer.Controls.Add( this.lstvDump );
			this.tbpPacketViewer.Controls.Add( this.btnOutInSingleWindow );
			this.tbpPacketViewer.Location = new System.Drawing.Point( 4, 22 );
			this.tbpPacketViewer.Name = "tbpPacketViewer";
			this.tbpPacketViewer.Padding = new System.Windows.Forms.Padding( 3 );
			this.tbpPacketViewer.Size = new System.Drawing.Size( 1225, 785 );
			this.tbpPacketViewer.TabIndex = 3;
			this.tbpPacketViewer.Text = "Просмотр пакетов";
			this.tbpPacketViewer.UseVisualStyleBackColor = true;
			this.tbpPacketViewer.Enter += new System.EventHandler( this.tbpPacketViewer_Enter );
			// 
			// panel3
			// 
			this.panel3.Controls.Add( this.splitContainer8 );
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point( 784, 3 );
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size( 438, 779 );
			this.panel3.TabIndex = 8;
			// 
			// splitContainer8
			// 
			this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer8.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer8.Name = "splitContainer8";
			// 
			// splitContainer8.Panel2
			// 
			this.splitContainer8.Panel2.Controls.Add( this.groupBox5 );
			this.splitContainer8.Panel2.Controls.Add( this.groupBox6 );
			this.splitContainer8.Size = new System.Drawing.Size( 438, 779 );
			this.splitContainer8.SplitterDistance = 54;
			this.splitContainer8.TabIndex = 0;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add( this.rbClm16 );
			this.groupBox5.Controls.Add( this.rbClm8 );
			this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox5.Location = new System.Drawing.Point( 0, 129 );
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size( 380, 89 );
			this.groupBox5.TabIndex = 8;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Число колонок дампа";
			// 
			// rbClm16
			// 
			this.rbClm16.AutoSize = true;
			this.rbClm16.Checked = true;
			this.rbClm16.Location = new System.Drawing.Point( 19, 52 );
			this.rbClm16.Name = "rbClm16";
			this.rbClm16.Size = new System.Drawing.Size( 82, 17 );
			this.rbClm16.TabIndex = 1;
			this.rbClm16.TabStop = true;
			this.rbClm16.Tag = "16";
			this.rbClm16.Text = "16 колонок";
			this.rbClm16.UseVisualStyleBackColor = true;
			this.rbClm16.CheckedChanged += new System.EventHandler( this.rbClm16_CheckedChanged );
			// 
			// rbClm8
			// 
			this.rbClm8.AutoSize = true;
			this.rbClm8.Location = new System.Drawing.Point( 18, 23 );
			this.rbClm8.Name = "rbClm8";
			this.rbClm8.Size = new System.Drawing.Size( 76, 17 );
			this.rbClm8.TabIndex = 0;
			this.rbClm8.Tag = "8";
			this.rbClm8.Text = "8 колонок";
			this.rbClm8.UseVisualStyleBackColor = true;
			this.rbClm8.CheckedChanged += new System.EventHandler( this.rbClm16_CheckedChanged );
			// 
			// groupBox6
			// 
			this.groupBox6.BackColor = System.Drawing.Color.LightPink;
			this.groupBox6.Controls.Add( this.button1 );
			this.groupBox6.Controls.Add( this.label14 );
			this.groupBox6.Controls.Add( this.cbEncode );
			this.groupBox6.Controls.Add( this.btnOutToFile );
			this.groupBox6.Controls.Add( this.btnPrint );
			this.groupBox6.Controls.Add( this.cbAvailablePackets );
			this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox6.Location = new System.Drawing.Point( 0, 0 );
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size( 380, 129 );
			this.groupBox6.TabIndex = 7;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Пакет для отображения:";
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Moccasin;
			this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.button1.Location = new System.Drawing.Point( 3, 36 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 374, 23 );
			this.button1.TabIndex = 5;
			this.button1.Text = "Обновить";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler( this.button1_Click );
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point( 7, 43 );
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size( 110, 13 );
			this.label14.TabIndex = 4;
			this.label14.Text = "Выбрать кодировку:";
			// 
			// cbEncode
			// 
			this.cbEncode.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.cbEncode.FormattingEnabled = true;
			this.cbEncode.Location = new System.Drawing.Point( 3, 59 );
			this.cbEncode.Name = "cbEncode";
			this.cbEncode.Size = new System.Drawing.Size( 374, 21 );
			this.cbEncode.TabIndex = 3;
			this.cbEncode.SelectedIndexChanged += new System.EventHandler( this.cbEncode_SelectedIndexChanged );
			// 
			// btnOutToFile
			// 
			this.btnOutToFile.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnOutToFile.Location = new System.Drawing.Point( 3, 80 );
			this.btnOutToFile.Name = "btnOutToFile";
			this.btnOutToFile.Size = new System.Drawing.Size( 374, 23 );
			this.btnOutToFile.TabIndex = 2;
			this.btnOutToFile.Text = "Вывод в файл";
			this.btnOutToFile.UseVisualStyleBackColor = true;
			// 
			// btnPrint
			// 
			this.btnPrint.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnPrint.Location = new System.Drawing.Point( 3, 103 );
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size( 374, 23 );
			this.btnPrint.TabIndex = 1;
			this.btnPrint.Text = "Печать";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Click += new System.EventHandler( this.btnPrint_Click );
			// 
			// cbAvailablePackets
			// 
			this.cbAvailablePackets.Dock = System.Windows.Forms.DockStyle.Top;
			this.cbAvailablePackets.FormattingEnabled = true;
			this.cbAvailablePackets.Location = new System.Drawing.Point( 3, 16 );
			this.cbAvailablePackets.Name = "cbAvailablePackets";
			this.cbAvailablePackets.Size = new System.Drawing.Size( 374, 21 );
			this.cbAvailablePackets.TabIndex = 0;
			this.cbAvailablePackets.SelectedIndexChanged += new System.EventHandler( this.cbAvailablePackets_SelectedIndexChanged );
			// 
			// lstvDump
			// 
			this.lstvDump.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lstvDump.Dock = System.Windows.Forms.DockStyle.Left;
			this.lstvDump.GridLines = true;
			this.lstvDump.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstvDump.Location = new System.Drawing.Point( 78, 3 );
			this.lstvDump.Name = "lstvDump";
			this.lstvDump.Size = new System.Drawing.Size( 706, 779 );
			this.lstvDump.TabIndex = 1;
			this.lstvDump.UseCompatibleStateImageBehavior = false;
			this.lstvDump.View = System.Windows.Forms.View.Details;
			// 
			// btnOutInSingleWindow
			// 
			this.btnOutInSingleWindow.BackColor = System.Drawing.Color.FromArgb( ( ( int ) ( ( ( byte ) ( 255 ) ) ) ), ( ( int ) ( ( ( byte ) ( 255 ) ) ) ), ( ( int ) ( ( ( byte ) ( 128 ) ) ) ) );
			this.btnOutInSingleWindow.Dock = System.Windows.Forms.DockStyle.Left;
			this.btnOutInSingleWindow.Location = new System.Drawing.Point( 3, 3 );
			this.btnOutInSingleWindow.Name = "btnOutInSingleWindow";
			this.btnOutInSingleWindow.Size = new System.Drawing.Size( 75, 779 );
			this.btnOutInSingleWindow.TabIndex = 0;
			this.btnOutInSingleWindow.Text = "Вывод в отдельное окно";
			this.btnOutInSingleWindow.UseVisualStyleBackColor = false;
			this.btnOutInSingleWindow.Visible = false;
			// 
			// frmSymap
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 1233, 811 );
			this.Controls.Add( this.tabControl1 );
			this.Name = "frmSymap";
			this.Text = "frmSymap";
			this.Shown += new System.EventHandler( this.frmSymap_Shown );
			this.tabControl1.ResumeLayout( false );
			this.tbpPacketViewer.ResumeLayout( false );
			this.panel3.ResumeLayout( false );
			this.splitContainer8.Panel2.ResumeLayout( false );
			this.splitContainer8.ResumeLayout( false );
			this.groupBox5.ResumeLayout( false );
			this.groupBox5.PerformLayout();
			this.groupBox6.ResumeLayout( false );
			this.groupBox6.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tbpPacketViewer;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.SplitContainer splitContainer8;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.RadioButton rbClm16;
		private System.Windows.Forms.RadioButton rbClm8;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.ComboBox cbEncode;
		private System.Windows.Forms.Button btnOutToFile;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.ComboBox cbAvailablePackets;
		private System.Windows.Forms.ListView lstvDump;
		private System.Windows.Forms.Button btnOutInSingleWindow;

	}
}