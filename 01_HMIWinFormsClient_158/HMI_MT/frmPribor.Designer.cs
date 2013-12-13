namespace HMI_MT
{
	partial class frmPribor
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( frmPribor ) );
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.íàñòðîèòüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiCS = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiDopPnl = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.çàêðûòüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.pnlMin = new System.Windows.Forms.Panel();
			this.pnlBlinkMin = new System.Windows.Forms.Panel();
			this.lblMinVal = new System.Windows.Forms.Label();
			this.splitContainer4 = new System.Windows.Forms.SplitContainer();
			this.btnMin = new System.Windows.Forms.Button();
			this.lblDTFixMin = new System.Windows.Forms.Label();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.pnlCurVal = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblCapDim = new System.Windows.Forms.Label();
			this.pnlBlinkVal = new System.Windows.Forms.Panel();
			this.lblValue = new System.Windows.Forms.Label();
			this.pnlMax = new System.Windows.Forms.Panel();
			this.pnlBlinkMax = new System.Windows.Forms.Panel();
			this.lblMaxVal = new System.Windows.Forms.Label();
			this.splitContainer5 = new System.Windows.Forms.SplitContainer();
			this.btnMax = new System.Windows.Forms.Button();
			this.lblDTFixMax = new System.Windows.Forms.Label();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.timer1 = new System.Windows.Forms.Timer( this.components );
			this.Pribor = new Manometers.Thermometer();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.pnlMin.SuspendLayout();
			this.pnlBlinkMin.SuspendLayout();
			this.splitContainer4.Panel1.SuspendLayout();
			this.splitContainer4.SuspendLayout();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.pnlCurVal.SuspendLayout();
			this.panel1.SuspendLayout();
			this.pnlBlinkVal.SuspendLayout();
			this.pnlMax.SuspendLayout();
			this.pnlBlinkMax.SuspendLayout();
			this.splitContainer5.Panel2.SuspendLayout();
			this.splitContainer5.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add( this.Pribor );
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add( this.splitContainer2 );
			this.splitContainer1.Size = new System.Drawing.Size( 309, 413 );
			this.splitContainer1.SplitterDistance = 341;
			this.splitContainer1.TabIndex = 0;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.íàñòðîèòüToolStripMenuItem,
            this.tsmiCS,
            this.tsmiDopPnl,
            this.toolStripSeparator1,
            this.çàêðûòüToolStripMenuItem} );
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size( 211, 98 );
			// 
			// íàñòðîèòüToolStripMenuItem
			// 
			this.íàñòðîèòüToolStripMenuItem.Name = "íàñòðîèòüToolStripMenuItem";
			this.íàñòðîèòüToolStripMenuItem.Size = new System.Drawing.Size( 210, 22 );
			this.íàñòðîèòüToolStripMenuItem.Text = "Íàñòðîèòü";
			this.íàñòðîèòüToolStripMenuItem.Click += new System.EventHandler( this.íàñòðîèòüToolStripMenuItem_Click );
			// 
			// tsmiCS
			// 
			this.tsmiCS.CheckOnClick = true;
			this.tsmiCS.Name = "tsmiCS";
			this.tsmiCS.Size = new System.Drawing.Size( 210, 22 );
			this.tsmiCS.Text = "Ïîâåðõ âñåõ îêîí";
			this.tsmiCS.CheckStateChanged += new System.EventHandler( this.ïîâåðõÂñåõÎêîíToolStripMenuItem_CheckStateChanged );
			// 
			// tsmiDopPnl
			// 
			this.tsmiDopPnl.CheckOnClick = true;
			this.tsmiDopPnl.Name = "tsmiDopPnl";
			this.tsmiDopPnl.Size = new System.Drawing.Size( 210, 22 );
			this.tsmiDopPnl.Text = "Äîïîëíèòåëüíàÿ ïàíåëü";
			this.tsmiDopPnl.CheckStateChanged += new System.EventHandler( this.tsmiDopPnl_CheckStateChanged );
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 207, 6 );
			// 
			// çàêðûòüToolStripMenuItem
			// 
			this.çàêðûòüToolStripMenuItem.Name = "çàêðûòüToolStripMenuItem";
			this.çàêðûòüToolStripMenuItem.Size = new System.Drawing.Size( 210, 22 );
			this.çàêðûòüToolStripMenuItem.Text = "Çàêðûòü";
			this.çàêðûòüToolStripMenuItem.Click += new System.EventHandler( this.çàêðûòüToolStripMenuItem_Click );
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add( this.pnlMin );
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add( this.splitContainer3 );
			this.splitContainer2.Size = new System.Drawing.Size( 307, 66 );
			this.splitContainer2.SplitterDistance = 101;
			this.splitContainer2.TabIndex = 1;
			// 
			// pnlMin
			// 
			this.pnlMin.Controls.Add( this.pnlBlinkMin );
			this.pnlMin.Controls.Add( this.splitContainer4 );
			this.pnlMin.Controls.Add( this.lblDTFixMin );
			this.pnlMin.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMin.Location = new System.Drawing.Point( 0, 0 );
			this.pnlMin.Name = "pnlMin";
			this.pnlMin.Size = new System.Drawing.Size( 101, 66 );
			this.pnlMin.TabIndex = 0;
			// 
			// pnlBlinkMin
			// 
			this.pnlBlinkMin.AutoSize = true;
			this.pnlBlinkMin.Controls.Add( this.lblMinVal );
			this.pnlBlinkMin.Location = new System.Drawing.Point( 6, 26 );
			this.pnlBlinkMin.Name = "pnlBlinkMin";
			this.pnlBlinkMin.Size = new System.Drawing.Size( 54, 14 );
			this.pnlBlinkMin.TabIndex = 8;
			// 
			// lblMinVal
			// 
			this.lblMinVal.AutoSize = true;
			this.lblMinVal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblMinVal.Location = new System.Drawing.Point( 0, 0 );
			this.lblMinVal.Name = "lblMinVal";
			this.lblMinVal.Size = new System.Drawing.Size( 0, 13 );
			this.lblMinVal.TabIndex = 5;
			// 
			// splitContainer4
			// 
			this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitContainer4.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer4.Name = "splitContainer4";
			// 
			// splitContainer4.Panel1
			// 
			this.splitContainer4.Panel1.Controls.Add( this.btnMin );
			this.splitContainer4.Panel2Collapsed = true;
			this.splitContainer4.Size = new System.Drawing.Size( 101, 23 );
			this.splitContainer4.SplitterDistance = 76;
			this.splitContainer4.TabIndex = 5;
			// 
			// btnMin
			// 
			this.btnMin.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnMin.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
			this.btnMin.Location = new System.Drawing.Point( 0, 0 );
			this.btnMin.Name = "btnMin";
			this.btnMin.Size = new System.Drawing.Size( 101, 23 );
			this.btnMin.TabIndex = 17;
			this.btnMin.Text = "Ìèíèìóì";
			this.btnMin.UseVisualStyleBackColor = true;
			this.btnMin.Click += new System.EventHandler( this.btnMin_Click );
			// 
			// lblDTFixMin
			// 
			this.lblDTFixMin.AutoSize = true;
			this.lblDTFixMin.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblDTFixMin.Location = new System.Drawing.Point( 0, 53 );
			this.lblDTFixMin.Name = "lblDTFixMin";
			this.lblDTFixMin.Size = new System.Drawing.Size( 0, 13 );
			this.lblDTFixMin.TabIndex = 1;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add( this.pnlCurVal );
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add( this.pnlMax );
			this.splitContainer3.Size = new System.Drawing.Size( 202, 66 );
			this.splitContainer3.SplitterDistance = 99;
			this.splitContainer3.TabIndex = 0;
			// 
			// pnlCurVal
			// 
			this.pnlCurVal.Controls.Add( this.panel1 );
			this.pnlCurVal.Controls.Add( this.pnlBlinkVal );
			this.pnlCurVal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlCurVal.Location = new System.Drawing.Point( 0, 0 );
			this.pnlCurVal.Name = "pnlCurVal";
			this.pnlCurVal.Size = new System.Drawing.Size( 99, 66 );
			this.pnlCurVal.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.panel1.Controls.Add( this.lblCapDim );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point( 0, 0 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 99, 19 );
			this.panel1.TabIndex = 16;
			// 
			// lblCapDim
			// 
			this.lblCapDim.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblCapDim.AutoSize = true;
			this.lblCapDim.Font = new System.Drawing.Font( "Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
			this.lblCapDim.ForeColor = System.Drawing.Color.Yellow;
			this.lblCapDim.Location = new System.Drawing.Point( 27, 1 );
			this.lblCapDim.Name = "lblCapDim";
			this.lblCapDim.Size = new System.Drawing.Size( 0, 19 );
			this.lblCapDim.TabIndex = 0;
			// 
			// pnlBlinkVal
			// 
			this.pnlBlinkVal.Anchor = ( ( System.Windows.Forms.AnchorStyles ) ( ( System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.pnlBlinkVal.AutoSize = true;
			this.pnlBlinkVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlBlinkVal.Controls.Add( this.lblValue );
			this.pnlBlinkVal.Location = new System.Drawing.Point( 0, 23 );
			this.pnlBlinkVal.Name = "pnlBlinkVal";
			this.pnlBlinkVal.Size = new System.Drawing.Size( 99, 43 );
			this.pnlBlinkVal.TabIndex = 1;
			// 
			// lblValue
			// 
			this.lblValue.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblValue.AutoSize = true;
			this.lblValue.Font = new System.Drawing.Font( "Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
			this.lblValue.ForeColor = System.Drawing.Color.Blue;
			this.lblValue.Location = new System.Drawing.Point( 25, 3 );
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size( 0, 23 );
			this.lblValue.TabIndex = 2;
			// 
			// pnlMax
			// 
			this.pnlMax.Controls.Add( this.pnlBlinkMax );
			this.pnlMax.Controls.Add( this.splitContainer5 );
			this.pnlMax.Controls.Add( this.lblDTFixMax );
			this.pnlMax.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMax.Location = new System.Drawing.Point( 0, 0 );
			this.pnlMax.Name = "pnlMax";
			this.pnlMax.Size = new System.Drawing.Size( 99, 66 );
			this.pnlMax.TabIndex = 0;
			// 
			// pnlBlinkMax
			// 
			this.pnlBlinkMax.Controls.Add( this.lblMaxVal );
			this.pnlBlinkMax.Location = new System.Drawing.Point( 8, 26 );
			this.pnlBlinkMax.Name = "pnlBlinkMax";
			this.pnlBlinkMax.Size = new System.Drawing.Size( 52, 14 );
			this.pnlBlinkMax.TabIndex = 6;
			// 
			// lblMaxVal
			// 
			this.lblMaxVal.AutoSize = true;
			this.lblMaxVal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblMaxVal.Location = new System.Drawing.Point( 0, 0 );
			this.lblMaxVal.Name = "lblMaxVal";
			this.lblMaxVal.Size = new System.Drawing.Size( 0, 13 );
			this.lblMaxVal.TabIndex = 5;
			// 
			// splitContainer5
			// 
			this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitContainer5.Location = new System.Drawing.Point( 0, 0 );
			this.splitContainer5.Name = "splitContainer5";
			this.splitContainer5.Panel1Collapsed = true;
			// 
			// splitContainer5.Panel2
			// 
			this.splitContainer5.Panel2.Controls.Add( this.btnMax );
			this.splitContainer5.Size = new System.Drawing.Size( 99, 23 );
			this.splitContainer5.SplitterDistance = 28;
			this.splitContainer5.TabIndex = 5;
			// 
			// btnMax
			// 
			this.btnMax.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnMax.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( ( byte ) ( 204 ) ) );
			this.btnMax.Location = new System.Drawing.Point( 0, 0 );
			this.btnMax.Name = "btnMax";
			this.btnMax.Size = new System.Drawing.Size( 99, 23 );
			this.btnMax.TabIndex = 3;
			this.btnMax.Text = "Ìàêñèìóì";
			this.btnMax.UseVisualStyleBackColor = true;
			this.btnMax.Click += new System.EventHandler( this.btnMin_Click );
			// 
			// lblDTFixMax
			// 
			this.lblDTFixMax.AutoSize = true;
			this.lblDTFixMax.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblDTFixMax.Location = new System.Drawing.Point( 0, 53 );
			this.lblDTFixMax.Name = "lblDTFixMax";
			this.lblDTFixMax.Size = new System.Drawing.Size( 0, 13 );
			this.lblDTFixMax.TabIndex = 1;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			// 
			// Pribor
			// 
			this.Pribor.ContextMenuStrip = this.contextMenuStrip1;
			this.Pribor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Pribor.Font = new System.Drawing.Font( "Microsoft Sans Serif", 11F );
			this.Pribor.Interval = 10F;
			this.Pribor.Location = new System.Drawing.Point( 0, 0 );
			this.Pribor.Max = 100F;
			this.Pribor.MaxArrowColor = System.Drawing.Color.Red;
			this.Pribor.Min = 0F;
			this.Pribor.MinArrowColor = System.Drawing.Color.Green;
			this.Pribor.Name = "Pribor";
			this.Pribor.Size = new System.Drawing.Size( 307, 339 );
			this.Pribor.StoredMax = 0F;
			this.Pribor.StoredMaxDate = new System.DateTime( ( ( long ) ( 0 ) ) );
			this.Pribor.StoredMin = 0F;
			this.Pribor.StoredMinDate = new System.DateTime( 2008, 3, 26, 8, 57, 49, 212 );
			this.Pribor.TabIndex = 0;
			this.Pribor.TextUnit = "°C";
			this.Pribor.Value = 0F;
			// 
			// frmPribor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 309, 413 );
			this.ControlBox = false;
			this.Controls.Add( this.splitContainer1 );
			this.Icon = ( ( System.Drawing.Icon ) ( resources.GetObject( "$this.Icon" ) ) );
			this.Name = "frmPribor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmPribor";
			this.ResizeEnd += new System.EventHandler( this.frmPribor_ResizeEnd );
			this.Load += new System.EventHandler( this.frmPribor_Load );
			this.splitContainer1.Panel1.ResumeLayout( false );
			this.splitContainer1.Panel2.ResumeLayout( false );
			this.splitContainer1.ResumeLayout( false );
			this.contextMenuStrip1.ResumeLayout( false );
			this.splitContainer2.Panel1.ResumeLayout( false );
			this.splitContainer2.Panel2.ResumeLayout( false );
			this.splitContainer2.ResumeLayout( false );
			this.pnlMin.ResumeLayout( false );
			this.pnlMin.PerformLayout();
			this.pnlBlinkMin.ResumeLayout( false );
			this.pnlBlinkMin.PerformLayout();
			this.splitContainer4.Panel1.ResumeLayout( false );
			this.splitContainer4.ResumeLayout( false );
			this.splitContainer3.Panel1.ResumeLayout( false );
			this.splitContainer3.Panel2.ResumeLayout( false );
			this.splitContainer3.ResumeLayout( false );
			this.pnlCurVal.ResumeLayout( false );
			this.pnlCurVal.PerformLayout();
			this.panel1.ResumeLayout( false );
			this.panel1.PerformLayout();
			this.pnlBlinkVal.ResumeLayout( false );
			this.pnlBlinkVal.PerformLayout();
			this.pnlMax.ResumeLayout( false );
			this.pnlMax.PerformLayout();
			this.pnlBlinkMax.ResumeLayout( false );
			this.pnlBlinkMax.PerformLayout();
			this.splitContainer5.Panel2.ResumeLayout( false );
			this.splitContainer5.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem íàñòðîèòüToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem çàêðûòüToolStripMenuItem;
		private System.Windows.Forms.ColorDialog colorDialog1;
		public System.Windows.Forms.ToolStripMenuItem tsmiCS;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Panel pnlMin;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.Panel pnlCurVal;
		private System.Windows.Forms.Panel pnlMax;
		private System.Windows.Forms.Label lblDTFixMin;
		private System.Windows.Forms.Label lblDTFixMax;
		private System.Windows.Forms.ToolStripMenuItem tsmiDopPnl;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Panel pnlBlinkVal;
        private System.Windows.Forms.Label lblValue;
		private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Button btnMin;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblCapDim;
		private System.Windows.Forms.SplitContainer splitContainer5;
		private System.Windows.Forms.Button btnMax;
        private System.Windows.Forms.Panel pnlBlinkMax;
        private System.Windows.Forms.Panel pnlBlinkMin;
        private System.Windows.Forms.Label lblMinVal;
        private System.Windows.Forms.Label lblMaxVal;
	}
}