namespace HMI_MT
{
	partial class dlgTagBrowser
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgTagBrowser));
			this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.gb3_LowLevel = new System.Windows.Forms.GroupBox();
			this.rtbNetLevel = new System.Windows.Forms.RichTextBox();
			this.gb2_HiLevel = new System.Windows.Forms.GroupBox();
			this.rtbASULevel = new System.Windows.Forms.RichTextBox();
			this.gb1_CurrentConfig = new System.Windows.Forms.GroupBox();
			this.treeViewKB = new System.Windows.Forms.TreeView();
			this.gb1_pnl1_PropertiesItem = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chbEmulateValue = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rbBitValue_1 = new System.Windows.Forms.RadioButton();
			this.rbBitValue_0 = new System.Windows.Forms.RadioButton();
			this.tbAnalogValue = new System.Windows.Forms.TextBox();
			this.btnSetEmulValue = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.gb3_LowLevel.SuspendLayout();
			this.gb2_HiLevel.SuspendLayout();
			this.gb1_CurrentConfig.SuspendLayout();
			this.gb1_pnl1_PropertiesItem.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.gb3_LowLevel);
			this.splitContainer1.Panel1.Controls.Add(this.gb2_HiLevel);
			this.splitContainer1.Panel1.Controls.Add(this.gb1_CurrentConfig);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btnClose);
			this.splitContainer1.Panel2MinSize = 35;
			this.splitContainer1.Size = new System.Drawing.Size(936, 674);
			this.splitContainer1.SplitterDistance = 638;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 1;
			// 
			// gb3_LowLevel
			// 
			this.gb3_LowLevel.Controls.Add(this.rtbNetLevel);
			this.gb3_LowLevel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gb3_LowLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.gb3_LowLevel.Location = new System.Drawing.Point(649, 0);
			this.gb3_LowLevel.Name = "gb3_LowLevel";
			this.gb3_LowLevel.Size = new System.Drawing.Size(283, 634);
			this.gb3_LowLevel.TabIndex = 5;
			this.gb3_LowLevel.TabStop = false;
			this.gb3_LowLevel.Text = "Сетевой уровень";
			// 
			// rtbNetLevel
			// 
			this.rtbNetLevel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbNetLevel.Location = new System.Drawing.Point(3, 18);
			this.rtbNetLevel.Name = "rtbNetLevel";
			this.rtbNetLevel.Size = new System.Drawing.Size(277, 613);
			this.rtbNetLevel.TabIndex = 0;
			this.rtbNetLevel.Text = "";
			// 
			// gb2_HiLevel
			// 
			this.gb2_HiLevel.Controls.Add(this.rtbASULevel);
			this.gb2_HiLevel.Dock = System.Windows.Forms.DockStyle.Left;
			this.gb2_HiLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.gb2_HiLevel.Location = new System.Drawing.Point(319, 0);
			this.gb2_HiLevel.Name = "gb2_HiLevel";
			this.gb2_HiLevel.Size = new System.Drawing.Size(330, 634);
			this.gb2_HiLevel.TabIndex = 4;
			this.gb2_HiLevel.TabStop = false;
			this.gb2_HiLevel.Text = "Уровень АСУ";
			// 
			// rtbASULevel
			// 
			this.rtbASULevel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbASULevel.Location = new System.Drawing.Point(3, 18);
			this.rtbASULevel.Name = "rtbASULevel";
			this.rtbASULevel.Size = new System.Drawing.Size(324, 613);
			this.rtbASULevel.TabIndex = 0;
			this.rtbASULevel.Text = "";
			// 
			// gb1_CurrentConfig
			// 
			this.gb1_CurrentConfig.Controls.Add(this.treeViewKB);
			this.gb1_CurrentConfig.Controls.Add(this.gb1_pnl1_PropertiesItem);
			this.gb1_CurrentConfig.Dock = System.Windows.Forms.DockStyle.Left;
			this.gb1_CurrentConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.gb1_CurrentConfig.Location = new System.Drawing.Point(0, 0);
			this.gb1_CurrentConfig.Name = "gb1_CurrentConfig";
			this.gb1_CurrentConfig.Size = new System.Drawing.Size(319, 634);
			this.gb1_CurrentConfig.TabIndex = 3;
			this.gb1_CurrentConfig.TabStop = false;
			this.gb1_CurrentConfig.Text = "Текущая конфигурация";
			// 
			// treeViewKB
			// 
			this.treeViewKB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewKB.Location = new System.Drawing.Point(3, 18);
			this.treeViewKB.Name = "treeViewKB";
			this.treeViewKB.Size = new System.Drawing.Size(313, 510);
			this.treeViewKB.TabIndex = 1;
			this.treeViewKB.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewKB_NodeMouseClick);
			this.treeViewKB.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeViewKB_MouseClick);
			// 
			// gb1_pnl1_PropertiesItem
			// 
			this.gb1_pnl1_PropertiesItem.Controls.Add(this.groupBox1);
			this.gb1_pnl1_PropertiesItem.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.gb1_pnl1_PropertiesItem.Location = new System.Drawing.Point(3, 528);
			this.gb1_pnl1_PropertiesItem.Name = "gb1_pnl1_PropertiesItem";
			this.gb1_pnl1_PropertiesItem.Size = new System.Drawing.Size(313, 103);
			this.gb1_pnl1_PropertiesItem.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chbEmulateValue);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.tbAnalogValue);
			this.groupBox1.Controls.Add(this.btnSetEmulValue);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(313, 103);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Значение тега";
			// 
			// chbEmulateValue
			// 
			this.chbEmulateValue.AutoSize = true;
			this.chbEmulateValue.Location = new System.Drawing.Point(7, 51);
			this.chbEmulateValue.Name = "chbEmulateValue";
			this.chbEmulateValue.Size = new System.Drawing.Size(183, 20);
			this.chbEmulateValue.TabIndex = 5;
			this.chbEmulateValue.Text = "Эмулировать значение";
			this.chbEmulateValue.UseVisualStyleBackColor = true;
			this.chbEmulateValue.CheckedChanged += new System.EventHandler(this.chbEmulateValue_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rbBitValue_1);
			this.groupBox2.Controls.Add(this.rbBitValue_0);
			this.groupBox2.Location = new System.Drawing.Point(191, 9);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(116, 34);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Дискр. сигнал";
			// 
			// rbBitValue_1
			// 
			this.rbBitValue_1.AutoSize = true;
			this.rbBitValue_1.Location = new System.Drawing.Point(55, 14);
			this.rbBitValue_1.Name = "rbBitValue_1";
			this.rbBitValue_1.Size = new System.Drawing.Size(33, 20);
			this.rbBitValue_1.TabIndex = 1;
			this.rbBitValue_1.TabStop = true;
			this.rbBitValue_1.Text = "1";
			this.rbBitValue_1.UseVisualStyleBackColor = true;
			// 
			// rbBitValue_0
			// 
			this.rbBitValue_0.AutoSize = true;
			this.rbBitValue_0.Location = new System.Drawing.Point(16, 13);
			this.rbBitValue_0.Name = "rbBitValue_0";
			this.rbBitValue_0.Size = new System.Drawing.Size(33, 20);
			this.rbBitValue_0.TabIndex = 0;
			this.rbBitValue_0.TabStop = true;
			this.rbBitValue_0.Text = "0";
			this.rbBitValue_0.UseVisualStyleBackColor = true;
			// 
			// tbAnalogValue
			// 
			this.tbAnalogValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.tbAnalogValue.Location = new System.Drawing.Point(49, 20);
			this.tbAnalogValue.Name = "tbAnalogValue";
			this.tbAnalogValue.Size = new System.Drawing.Size(136, 22);
			this.tbAnalogValue.TabIndex = 1;
			// 
			// btnSetEmulValue
			// 
			this.btnSetEmulValue.BackColor = System.Drawing.Color.LightCoral;
			this.btnSetEmulValue.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnSetEmulValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnSetEmulValue.Location = new System.Drawing.Point(3, 77);
			this.btnSetEmulValue.Name = "btnSetEmulValue";
			this.btnSetEmulValue.Size = new System.Drawing.Size(307, 23);
			this.btnSetEmulValue.TabIndex = 0;
			this.btnSetEmulValue.Text = "Задать значение";
			this.btnSetEmulValue.UseVisualStyleBackColor = false;
			this.btnSetEmulValue.Click += new System.EventHandler(this.btnSetEmulValue_Click);
			// 
			// btnClose
			// 
			this.btnClose.AutoSize = true;
			this.btnClose.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnClose.Location = new System.Drawing.Point(0, 2);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(932, 29);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Закрыть";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
			// 
			// dlgTagBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(936, 674);
			this.Controls.Add(this.splitContainer1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "dlgTagBrowser";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Обозреватель тегов";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.dlgTagBrowser_Load);
			this.SizeChanged += new System.EventHandler(this.dlgTagBrowser_SizeChanged);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.gb3_LowLevel.ResumeLayout(false);
			this.gb2_HiLevel.ResumeLayout(false);
			this.gb1_CurrentConfig.ResumeLayout(false);
			this.gb1_pnl1_PropertiesItem.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.GroupBox gb1_CurrentConfig;
		private System.Windows.Forms.TreeView treeViewKB;
      private System.Windows.Forms.Panel gb1_pnl1_PropertiesItem;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox gb3_LowLevel;
		private System.Windows.Forms.GroupBox gb2_HiLevel;
		private System.Windows.Forms.RichTextBox rtbNetLevel;
      private System.Windows.Forms.RichTextBox rtbASULevel;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Button btnSetEmulValue;
      private System.Windows.Forms.TextBox tbAnalogValue;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.RadioButton rbBitValue_1;
      private System.Windows.Forms.RadioButton rbBitValue_0;
      private System.Windows.Forms.CheckBox chbEmulateValue;
	}
}