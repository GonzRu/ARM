namespace DeviceFormLib
{
   partial class CurrentPanelControl
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
         if ( disposing && ( components != null ) )
         {
            components.Dispose( );
         }
         base.Dispose( disposing );
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent( )
      {
            this.components = new System.ComponentModel.Container();
            this.pnlCurrent = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtraflpSvetodiods = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mtraflpTumblers = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pnlSwitchPosition = new System.Windows.Forms.Panel();
            this.pnlCurrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCurrent
            // 
            this.pnlCurrent.BackColor = System.Drawing.Color.LightSalmon;
            this.pnlCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCurrent.Controls.Add(this.splitContainer1);
            this.pnlCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCurrent.Location = new System.Drawing.Point(0, 0);
            this.pnlCurrent.Name = "pnlCurrent";
            this.pnlCurrent.Size = new System.Drawing.Size(740, 150);
            this.pnlCurrent.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(738, 148);
            this.splitContainer1.SplitterDistance = 578;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(578, 148);
            this.splitContainer2.SplitterDistance = 411;
            this.splitContainer2.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.mtraflpSvetodiods);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 148);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Светодиоды";
            // 
            // mtraflpSvetodiods
            // 
            this.mtraflpSvetodiods.AutoScroll = true;
            this.mtraflpSvetodiods.BackColor = System.Drawing.SystemColors.Control;
            this.mtraflpSvetodiods.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraflpSvetodiods.Caption = "Светодиоды";
            this.mtraflpSvetodiods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraflpSvetodiods.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraflpSvetodiods.Location = new System.Drawing.Point(3, 16);
            this.mtraflpSvetodiods.Name = "mtraflpSvetodiods";
            this.mtraflpSvetodiods.Size = new System.Drawing.Size(405, 129);
            this.mtraflpSvetodiods.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.mtraflpTumblers);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(163, 148);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Тумблеры";
            // 
            // mtraflpTumblers
            // 
            this.mtraflpTumblers.AutoScroll = true;
            this.mtraflpTumblers.BackColor = System.Drawing.SystemColors.Control;
            this.mtraflpTumblers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraflpTumblers.Caption = "Тумблеры";
            this.mtraflpTumblers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraflpTumblers.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraflpTumblers.Location = new System.Drawing.Point(3, 16);
            this.mtraflpTumblers.Name = "mtraflpTumblers";
            this.mtraflpTumblers.Size = new System.Drawing.Size(157, 129);
            this.mtraflpTumblers.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.pnlSwitchPosition);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(156, 148);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Положение выключателя";
            // 
            // pnlSwitchPosition
            // 
            this.pnlSwitchPosition.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlSwitchPosition.BackColor = System.Drawing.Color.Transparent;
            this.pnlSwitchPosition.Location = new System.Drawing.Point(27, 29);
            this.pnlSwitchPosition.Name = "pnlSwitchPosition";
            this.pnlSwitchPosition.Size = new System.Drawing.Size(95, 105);
            this.pnlSwitchPosition.TabIndex = 3;
            // 
            // CurrentPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlCurrent);
            this.Name = "CurrentPanelControl";
            this.Size = new System.Drawing.Size(740, 150);
            this.pnlCurrent.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel pnlCurrent;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.SplitContainer splitContainer2;
      private System.Windows.Forms.GroupBox groupBox1;
      private LabelTextbox.MTRANamedFLPanel mtraflpSvetodiods;
      private System.Windows.Forms.GroupBox groupBox2;
      private LabelTextbox.MTRANamedFLPanel mtraflpTumblers;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.Panel pnlSwitchPosition;
   }
}
