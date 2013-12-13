namespace HMI_MT
{
   partial class frmOvod_SP
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent( )
      {
         this.components = new System.ComponentModel.Container();
         this.tabpageControl = new System.Windows.Forms.TabPage();
         this.splitContainer2 = new System.Windows.Forms.SplitContainer();
         this.splitContainer4 = new System.Windows.Forms.SplitContainer();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.mtraNPInfo = new LabelTextbox.MTRANamedFLPanel(this.components);
         this.panel1 = new System.Windows.Forms.Panel();
         this.label5 = new System.Windows.Forms.Label();
         this.button6 = new System.Windows.Forms.Button();
         this.label4 = new System.Windows.Forms.Label();
         this.button5 = new System.Windows.Forms.Button();
         this.label3 = new System.Windows.Forms.Label();
         this.button4 = new System.Windows.Forms.Button();
         this.label2 = new System.Windows.Forms.Label();
         this.button3 = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.button2 = new System.Windows.Forms.Button();
         this.splitContainer5 = new System.Windows.Forms.SplitContainer();
         this.groupBox4 = new System.Windows.Forms.GroupBox();
         this.mtraVOD = new LabelTextbox.MTRANamedFLPanel(this.components);
         this.splitContainer6 = new System.Windows.Forms.SplitContainer();
         this.groupBox5 = new System.Windows.Forms.GroupBox();
         this.mtraIn = new LabelTextbox.MTRANamedFLPanel(this.components);
         this.groupBox6 = new System.Windows.Forms.GroupBox();
         this.mtraOut = new LabelTextbox.MTRANamedFLPanel(this.components);
         this.label6 = new System.Windows.Forms.Label();
         this.button7 = new System.Windows.Forms.Button();
         ((System.ComponentModel.ISupportInitialize)(this.erp)).BeginInit();
         this.tc_Main.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
         this.tabpageControl.SuspendLayout();
         this.splitContainer2.Panel1.SuspendLayout();
         this.splitContainer2.Panel2.SuspendLayout();
         this.splitContainer2.SuspendLayout();
         this.splitContainer4.Panel1.SuspendLayout();
         this.splitContainer4.Panel2.SuspendLayout();
         this.splitContainer4.SuspendLayout();
         this.groupBox3.SuspendLayout();
         this.panel1.SuspendLayout();
         this.splitContainer5.Panel1.SuspendLayout();
         this.splitContainer5.Panel2.SuspendLayout();
         this.splitContainer5.SuspendLayout();
         this.groupBox4.SuspendLayout();
         this.splitContainer6.Panel1.SuspendLayout();
         this.splitContainer6.Panel2.SuspendLayout();
         this.splitContainer6.SuspendLayout();
         this.groupBox5.SuspendLayout();
         this.groupBox6.SuspendLayout();
         this.SuspendLayout();
         // 
         // tc_Main
         // 
         this.tc_Main.Controls.Add(this.tabpageControl);
         this.tc_Main.SelectedIndexChanged += new System.EventHandler(this.tc_Main_SelectedIndexChanged);
         this.tc_Main.Controls.SetChildIndex(this.tabpageControl, 0);
         //this.tc_Main.Controls.SetChildIndex(this.tabStatusDev_Command, 0);
         // 
         // tabStatusDev_Command
         // 
         //this.tabStatusDev_Command.BackColor = System.Drawing.Color.Transparent;
         //this.tabStatusDev_Command.UseVisualStyleBackColor = true;
         // 
         // tabpageControl
         // 
         this.tabpageControl.Controls.Add(this.splitContainer2);
         this.tabpageControl.Location = new System.Drawing.Point(4, 22);
         this.tabpageControl.Name = "tabpageControl";
         this.tabpageControl.Size = new System.Drawing.Size(1004, 598);
         this.tabpageControl.TabIndex = 8;
         this.tabpageControl.Text = "Текущие_значения";
         this.tabpageControl.UseVisualStyleBackColor = true;
         // 
         // splitContainer2
         // 
         this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer2.Location = new System.Drawing.Point(0, 0);
         this.splitContainer2.Name = "splitContainer2";
         // 
         // splitContainer2.Panel1
         // 
         this.splitContainer2.Panel1.Controls.Add(this.splitContainer4);
         // 
         // splitContainer2.Panel2
         // 
         this.splitContainer2.Panel2.Controls.Add(this.splitContainer5);
         this.splitContainer2.Size = new System.Drawing.Size(1004, 598);
         this.splitContainer2.SplitterDistance = 265;
         this.splitContainer2.TabIndex = 2;
         // 
         // splitContainer4
         // 
         this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer4.Location = new System.Drawing.Point(0, 0);
         this.splitContainer4.Name = "splitContainer4";
         this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
         // 
         // splitContainer4.Panel1
         // 
         this.splitContainer4.Panel1.Controls.Add(this.groupBox3);
         // 
         // splitContainer4.Panel2
         // 
         this.splitContainer4.Panel2.Controls.Add(this.panel1);
         this.splitContainer4.Size = new System.Drawing.Size(265, 598);
         this.splitContainer4.SplitterDistance = 310;
         this.splitContainer4.TabIndex = 0;
         // 
         // groupBox3
         // 
         this.groupBox3.Controls.Add(this.mtraNPInfo);
         this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupBox3.Location = new System.Drawing.Point(0, 0);
         this.groupBox3.Name = "groupBox3";
         this.groupBox3.Size = new System.Drawing.Size(265, 310);
         this.groupBox3.TabIndex = 1;
         this.groupBox3.TabStop = false;
         this.groupBox3.Text = "ОВОД-МД ид.уст. №";
         // 
         // mtraNPInfo
         // 
         this.mtraNPInfo.AutoScroll = true;
         this.mtraNPInfo.BackColor = System.Drawing.SystemColors.Control;
         this.mtraNPInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.mtraNPInfo.Caption = "";
         this.mtraNPInfo.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mtraNPInfo.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
         this.mtraNPInfo.Location = new System.Drawing.Point(3, 16);
         this.mtraNPInfo.Name = "mtraNPInfo";
         this.mtraNPInfo.Size = new System.Drawing.Size(259, 291);
         this.mtraNPInfo.TabIndex = 0;
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.SystemColors.Control;
         this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel1.Controls.Add(this.label6);
         this.panel1.Controls.Add(this.button7);
         this.panel1.Controls.Add(this.label5);
         this.panel1.Controls.Add(this.button6);
         this.panel1.Controls.Add(this.label4);
         this.panel1.Controls.Add(this.button5);
         this.panel1.Controls.Add(this.label3);
         this.panel1.Controls.Add(this.button4);
         this.panel1.Controls.Add(this.label2);
         this.panel1.Controls.Add(this.button3);
         this.panel1.Controls.Add(this.label1);
         this.panel1.Controls.Add(this.button2);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(265, 284);
         this.panel1.TabIndex = 2;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(34, 119);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(75, 13);
         this.label5.TabIndex = 9;
         this.label5.Text = "Отключен = 8";
         // 
         // button6
         // 
         this.button6.BackColor = System.Drawing.Color.Silver;
         this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.button6.Location = new System.Drawing.Point(13, 117);
         this.button6.Name = "button6";
         this.button6.Size = new System.Drawing.Size(15, 15);
         this.button6.TabIndex = 8;
         this.button6.UseVisualStyleBackColor = false;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(34, 93);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(86, 13);
         this.label4.TabIndex = 7;
         this.label4.Text = "Обрыв ВОД = 6";
         // 
         // button5
         // 
         this.button5.BackColor = System.Drawing.Color.Yellow;
         this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.button5.Location = new System.Drawing.Point(13, 91);
         this.button5.Margin = new System.Windows.Forms.Padding(0);
         this.button5.Name = "button5";
         this.button5.Size = new System.Drawing.Size(15, 15);
         this.button5.TabIndex = 6;
         this.button5.Text = "х";
         this.button5.UseVisualStyleBackColor = false;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(34, 65);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(90, 13);
         this.label3.TabIndex = 5;
         this.label3.Text = "Не исправен = 2";
         // 
         // button4
         // 
         this.button4.BackColor = System.Drawing.Color.Yellow;
         this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.button4.Location = new System.Drawing.Point(13, 65);
         this.button4.Name = "button4";
         this.button4.Size = new System.Drawing.Size(15, 15);
         this.button4.TabIndex = 4;
         this.button4.UseVisualStyleBackColor = false;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(34, 40);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(73, 13);
         this.label2.TabIndex = 3;
         this.label2.Text = "Сработал = 1";
         // 
         // button3
         // 
         this.button3.BackColor = System.Drawing.Color.Red;
         this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.button3.Location = new System.Drawing.Point(13, 39);
         this.button3.Name = "button3";
         this.button3.Size = new System.Drawing.Size(15, 15);
         this.button3.TabIndex = 2;
         this.button3.UseVisualStyleBackColor = false;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(34, 15);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(75, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Исправен = 0";
         // 
         // button2
         // 
         this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
         this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.button2.Location = new System.Drawing.Point(13, 13);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(15, 15);
         this.button2.TabIndex = 0;
         this.button2.UseVisualStyleBackColor = false;
         // 
         // splitContainer5
         // 
         this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer5.Location = new System.Drawing.Point(0, 0);
         this.splitContainer5.Name = "splitContainer5";
         // 
         // splitContainer5.Panel1
         // 
         this.splitContainer5.Panel1.Controls.Add(this.groupBox4);
         // 
         // splitContainer5.Panel2
         // 
         this.splitContainer5.Panel2.Controls.Add(this.splitContainer6);
         this.splitContainer5.Size = new System.Drawing.Size(735, 598);
         this.splitContainer5.SplitterDistance = 340;
         this.splitContainer5.TabIndex = 0;
         // 
         // groupBox4
         // 
         this.groupBox4.Controls.Add(this.mtraVOD);
         this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupBox4.Location = new System.Drawing.Point(0, 0);
         this.groupBox4.Name = "groupBox4";
         this.groupBox4.Size = new System.Drawing.Size(340, 598);
         this.groupBox4.TabIndex = 0;
         this.groupBox4.TabStop = false;
         this.groupBox4.Text = "ВОД";
         // 
         // mtraVOD
         // 
         this.mtraVOD.AutoScroll = true;
         this.mtraVOD.BackColor = System.Drawing.SystemColors.Control;
         this.mtraVOD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.mtraVOD.Caption = "";
         this.mtraVOD.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mtraVOD.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
         this.mtraVOD.Location = new System.Drawing.Point(3, 16);
         this.mtraVOD.Name = "mtraVOD";
         this.mtraVOD.Size = new System.Drawing.Size(334, 579);
         this.mtraVOD.TabIndex = 0;
         // 
         // splitContainer6
         // 
         this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer6.Location = new System.Drawing.Point(0, 0);
         this.splitContainer6.Name = "splitContainer6";
         // 
         // splitContainer6.Panel1
         // 
         this.splitContainer6.Panel1.Controls.Add(this.groupBox5);
         // 
         // splitContainer6.Panel2
         // 
         this.splitContainer6.Panel2.Controls.Add(this.groupBox6);
         this.splitContainer6.Size = new System.Drawing.Size(391, 598);
         this.splitContainer6.SplitterDistance = 205;
         this.splitContainer6.TabIndex = 0;
         // 
         // groupBox5
         // 
         this.groupBox5.Controls.Add(this.mtraIn);
         this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupBox5.Location = new System.Drawing.Point(0, 0);
         this.groupBox5.Name = "groupBox5";
         this.groupBox5.Size = new System.Drawing.Size(205, 598);
         this.groupBox5.TabIndex = 0;
         this.groupBox5.TabStop = false;
         this.groupBox5.Text = "Входы";
         // 
         // mtraIn
         // 
         this.mtraIn.AutoScroll = true;
         this.mtraIn.BackColor = System.Drawing.SystemColors.Control;
         this.mtraIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.mtraIn.Caption = "";
         this.mtraIn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mtraIn.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
         this.mtraIn.Location = new System.Drawing.Point(3, 16);
         this.mtraIn.Name = "mtraIn";
         this.mtraIn.Size = new System.Drawing.Size(199, 579);
         this.mtraIn.TabIndex = 0;
         // 
         // groupBox6
         // 
         this.groupBox6.Controls.Add(this.mtraOut);
         this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupBox6.Location = new System.Drawing.Point(0, 0);
         this.groupBox6.Name = "groupBox6";
         this.groupBox6.Size = new System.Drawing.Size(182, 598);
         this.groupBox6.TabIndex = 0;
         this.groupBox6.TabStop = false;
         this.groupBox6.Text = "Выходы";
         // 
         // mtraOut
         // 
         this.mtraOut.AutoScroll = true;
         this.mtraOut.BackColor = System.Drawing.SystemColors.Control;
         this.mtraOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.mtraOut.Caption = "";
         this.mtraOut.Dock = System.Windows.Forms.DockStyle.Fill;
         this.mtraOut.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
         this.mtraOut.Location = new System.Drawing.Point(3, 16);
         this.mtraOut.Name = "mtraOut";
         this.mtraOut.Size = new System.Drawing.Size(176, 579);
         this.mtraOut.TabIndex = 0;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(34, 145);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(136, 13);
         this.label6.TabIndex = 11;
         this.label6.Text = "Нет связи с устройством";
         // 
         // button7
         // 
         this.button7.BackColor = System.Drawing.Color.Black;
         this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.button7.Location = new System.Drawing.Point(13, 143);
         this.button7.Name = "button7";
         this.button7.Size = new System.Drawing.Size(15, 15);
         this.button7.TabIndex = 10;
         this.button7.UseVisualStyleBackColor = false;
         // 
         // frmOvod_SP
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1016, 734);
         this.Location = new System.Drawing.Point(0, 0);
         this.Name = "frmOvod_SP";
         this.Text = "frmOvod_SP";
         this.Load += new System.EventHandler(this.frmOvod_SP_Load);
         ((System.ComponentModel.ISupportInitialize)(this.erp)).EndInit();
         this.tc_Main.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
         this.tabpageControl.ResumeLayout(false);
         this.splitContainer2.Panel1.ResumeLayout(false);
         this.splitContainer2.Panel2.ResumeLayout(false);
         this.splitContainer2.ResumeLayout(false);
         this.splitContainer4.Panel1.ResumeLayout(false);
         this.splitContainer4.Panel2.ResumeLayout(false);
         this.splitContainer4.ResumeLayout(false);
         this.groupBox3.ResumeLayout(false);
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.splitContainer5.Panel1.ResumeLayout(false);
         this.splitContainer5.Panel2.ResumeLayout(false);
         this.splitContainer5.ResumeLayout(false);
         this.groupBox4.ResumeLayout(false);
         this.splitContainer6.Panel1.ResumeLayout(false);
         this.splitContainer6.Panel2.ResumeLayout(false);
         this.splitContainer6.ResumeLayout(false);
         this.groupBox5.ResumeLayout(false);
         this.groupBox6.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TabPage tabpageControl;
      private System.Windows.Forms.SplitContainer splitContainer2;
      private System.Windows.Forms.SplitContainer splitContainer4;
      private System.Windows.Forms.GroupBox groupBox3;
      private LabelTextbox.MTRANamedFLPanel mtraNPInfo;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Button button6;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Button button5;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Button button4;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button button3;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.SplitContainer splitContainer5;
      private System.Windows.Forms.GroupBox groupBox4;
      private System.Windows.Forms.SplitContainer splitContainer6;
      private System.Windows.Forms.GroupBox groupBox5;
      private System.Windows.Forms.GroupBox groupBox6;
      private LabelTextbox.MTRANamedFLPanel mtraVOD;
      private LabelTextbox.MTRANamedFLPanel mtraIn;
      private LabelTextbox.MTRANamedFLPanel mtraOut;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Button button7;
   }
}