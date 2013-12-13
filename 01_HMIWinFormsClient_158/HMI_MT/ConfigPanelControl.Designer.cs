namespace HMI_MT
{
   partial class ConfigPanelControl
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
         this.pnlConfig = new System.Windows.Forms.Panel();
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.splitContainer2 = new System.Windows.Forms.SplitContainer();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.btnReNewUstBD = new System.Windows.Forms.Button();
         this.splitContainer3 = new System.Windows.Forms.SplitContainer();
         this.groupBox10 = new System.Windows.Forms.GroupBox();
         this.dtpStartTimeConfig = new System.Windows.Forms.DateTimePicker();
         this.dtpStartDateConfig = new System.Windows.Forms.DateTimePicker();
         this.groupBox11 = new System.Windows.Forms.GroupBox();
         this.dtpEndTimeConfig = new System.Windows.Forms.DateTimePicker();
         this.dtpEndDateConfig = new System.Windows.Forms.DateTimePicker();
         this.groupBox9 = new System.Windows.Forms.GroupBox();
         this.btnWriteUst = new System.Windows.Forms.Button();
         this.btnReadUstFC = new System.Windows.Forms.Button();
         this.btnResetValues = new System.Windows.Forms.Button();
         this.pnlConfig.SuspendLayout();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         this.splitContainer2.Panel1.SuspendLayout();
         this.splitContainer2.Panel2.SuspendLayout();
         this.splitContainer2.SuspendLayout();
         this.groupBox1.SuspendLayout();
         this.splitContainer3.Panel1.SuspendLayout();
         this.splitContainer3.Panel2.SuspendLayout();
         this.splitContainer3.SuspendLayout();
         this.groupBox10.SuspendLayout();
         this.groupBox11.SuspendLayout();
         this.groupBox9.SuspendLayout();
         this.SuspendLayout();
         // 
         // pnlConfig
         // 
         this.pnlConfig.BackColor = System.Drawing.Color.LightSalmon;
         this.pnlConfig.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.pnlConfig.Controls.Add( this.splitContainer1 );
         this.pnlConfig.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pnlConfig.Location = new System.Drawing.Point( 0, 0 );
         this.pnlConfig.Name = "pnlConfig";
         this.pnlConfig.Size = new System.Drawing.Size( 1286, 111 );
         this.pnlConfig.TabIndex = 6;
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
         this.splitContainer1.Name = "splitContainer1";
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add( this.splitContainer2 );
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add( this.btnResetValues );
         this.splitContainer1.Size = new System.Drawing.Size( 1282, 107 );
         this.splitContainer1.SplitterDistance = 1087;
         this.splitContainer1.TabIndex = 15;
         // 
         // splitContainer2
         // 
         this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer2.Location = new System.Drawing.Point( 0, 0 );
         this.splitContainer2.Name = "splitContainer2";
         // 
         // splitContainer2.Panel1
         // 
         this.splitContainer2.Panel1.Controls.Add( this.groupBox1 );
         // 
         // splitContainer2.Panel2
         // 
         this.splitContainer2.Panel2.Controls.Add( this.groupBox9 );
         this.splitContainer2.Size = new System.Drawing.Size( 1087, 107 );
         this.splitContainer2.SplitterDistance = 755;
         this.splitContainer2.TabIndex = 13;
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add( this.btnReNewUstBD );
         this.groupBox1.Controls.Add( this.splitContainer3 );
         this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupBox1.Location = new System.Drawing.Point( 0, 0 );
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size( 755, 107 );
         this.groupBox1.TabIndex = 19;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Хронология уставок в базе данных";
         // 
         // btnReNewUstBD
         // 
         this.btnReNewUstBD.Dock = System.Windows.Forms.DockStyle.Right;
         this.btnReNewUstBD.Location = new System.Drawing.Point( 665, 16 );
         this.btnReNewUstBD.Name = "btnReNewUstBD";
         this.btnReNewUstBD.Size = new System.Drawing.Size( 87, 88 );
         this.btnReNewUstBD.TabIndex = 19;
         this.btnReNewUstBD.Text = "Обновить";
         this.btnReNewUstBD.UseVisualStyleBackColor = true;
         // 
         // splitContainer3
         // 
         this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Left;
         this.splitContainer3.Location = new System.Drawing.Point( 3, 16 );
         this.splitContainer3.Name = "splitContainer3";
         // 
         // splitContainer3.Panel1
         // 
         this.splitContainer3.Panel1.Controls.Add( this.groupBox10 );
         // 
         // splitContainer3.Panel2
         // 
         this.splitContainer3.Panel2.Controls.Add( this.groupBox11 );
         this.splitContainer3.Size = new System.Drawing.Size( 656, 88 );
         this.splitContainer3.SplitterDistance = 218;
         this.splitContainer3.TabIndex = 1;
         // 
         // groupBox10
         // 
         this.groupBox10.Controls.Add( this.dtpStartTimeConfig );
         this.groupBox10.Controls.Add( this.dtpStartDateConfig );
         this.groupBox10.Location = new System.Drawing.Point( 0, 0 );
         this.groupBox10.Name = "groupBox10";
         this.groupBox10.Size = new System.Drawing.Size( 221, 56 );
         this.groupBox10.TabIndex = 17;
         this.groupBox10.TabStop = false;
         this.groupBox10.Text = "Время начала выборки";
         // 
         // dtpStartTimeConfig
         // 
         this.dtpStartTimeConfig.Format = System.Windows.Forms.DateTimePickerFormat.Time;
         this.dtpStartTimeConfig.Location = new System.Drawing.Point( 128, 18 );
         this.dtpStartTimeConfig.Name = "dtpStartTimeConfig";
         this.dtpStartTimeConfig.ShowUpDown = true;
         this.dtpStartTimeConfig.Size = new System.Drawing.Size( 87, 20 );
         this.dtpStartTimeConfig.TabIndex = 1;
         // 
         // dtpStartDateConfig
         // 
         this.dtpStartDateConfig.Format = System.Windows.Forms.DateTimePickerFormat.Short;
         this.dtpStartDateConfig.Location = new System.Drawing.Point( 6, 17 );
         this.dtpStartDateConfig.Name = "dtpStartDateConfig";
         this.dtpStartDateConfig.Size = new System.Drawing.Size( 99, 20 );
         this.dtpStartDateConfig.TabIndex = 0;
         // 
         // groupBox11
         // 
         this.groupBox11.Controls.Add( this.dtpEndTimeConfig );
         this.groupBox11.Controls.Add( this.dtpEndDateConfig );
         this.groupBox11.Location = new System.Drawing.Point( 0, 0 );
         this.groupBox11.Name = "groupBox11";
         this.groupBox11.Size = new System.Drawing.Size( 215, 56 );
         this.groupBox11.TabIndex = 16;
         this.groupBox11.TabStop = false;
         this.groupBox11.Text = "Время конца выборки";
         // 
         // dtpEndTimeConfig
         // 
         this.dtpEndTimeConfig.Format = System.Windows.Forms.DateTimePickerFormat.Time;
         this.dtpEndTimeConfig.Location = new System.Drawing.Point( 117, 17 );
         this.dtpEndTimeConfig.Name = "dtpEndTimeConfig";
         this.dtpEndTimeConfig.ShowUpDown = true;
         this.dtpEndTimeConfig.Size = new System.Drawing.Size( 88, 20 );
         this.dtpEndTimeConfig.TabIndex = 1;
         // 
         // dtpEndDateConfig
         // 
         this.dtpEndDateConfig.Format = System.Windows.Forms.DateTimePickerFormat.Short;
         this.dtpEndDateConfig.Location = new System.Drawing.Point( 6, 17 );
         this.dtpEndDateConfig.Name = "dtpEndDateConfig";
         this.dtpEndDateConfig.Size = new System.Drawing.Size( 92, 20 );
         this.dtpEndDateConfig.TabIndex = 0;
         // 
         // groupBox9
         // 
         this.groupBox9.Controls.Add( this.btnWriteUst );
         this.groupBox9.Controls.Add( this.btnReadUstFC );
         this.groupBox9.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupBox9.Location = new System.Drawing.Point( 0, 0 );
         this.groupBox9.MinimumSize = new System.Drawing.Size( 423, 114 );
         this.groupBox9.Name = "groupBox9";
         this.groupBox9.Size = new System.Drawing.Size( 423, 114 );
         this.groupBox9.TabIndex = 12;
         this.groupBox9.TabStop = false;
         this.groupBox9.Text = "Чтение\\запись уставок";
         // 
         // btnWriteUst
         // 
         this.btnWriteUst.AutoSize = true;
         this.btnWriteUst.Location = new System.Drawing.Point( 155, 19 );
         this.btnWriteUst.Name = "btnWriteUst";
         this.btnWriteUst.Size = new System.Drawing.Size( 111, 24 );
         this.btnWriteUst.TabIndex = 2;
         this.btnWriteUst.Text = "Запись уставок";
         this.btnWriteUst.UseVisualStyleBackColor = true;
         // 
         // btnReadUstFC
         // 
         this.btnReadUstFC.AutoSize = true;
         this.btnReadUstFC.Location = new System.Drawing.Point( 6, 19 );
         this.btnReadUstFC.Name = "btnReadUstFC";
         this.btnReadUstFC.Size = new System.Drawing.Size( 143, 24 );
         this.btnReadUstFC.TabIndex = 0;
         this.btnReadUstFC.Text = "Чтение уставок";
         this.btnReadUstFC.UseVisualStyleBackColor = true;
         // 
         // btnResetValues
         // 
         this.btnResetValues.Dock = System.Windows.Forms.DockStyle.Fill;
         this.btnResetValues.Location = new System.Drawing.Point( 0, 0 );
         this.btnResetValues.Name = "btnResetValues";
         this.btnResetValues.Size = new System.Drawing.Size( 191, 107 );
         this.btnResetValues.TabIndex = 15;
         this.btnResetValues.Text = "Очистить поля формы";
         this.btnResetValues.UseVisualStyleBackColor = true;
         // 
         // ConfigPanelControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add( this.pnlConfig );
         this.Name = "ConfigPanelControl";
         this.Size = new System.Drawing.Size( 1286, 111 );
         this.pnlConfig.ResumeLayout( false );
         this.splitContainer1.Panel1.ResumeLayout( false );
         this.splitContainer1.Panel2.ResumeLayout( false );
         this.splitContainer1.ResumeLayout( false );
         this.splitContainer2.Panel1.ResumeLayout( false );
         this.splitContainer2.Panel2.ResumeLayout( false );
         this.splitContainer2.ResumeLayout( false );
         this.groupBox1.ResumeLayout( false );
         this.splitContainer3.Panel1.ResumeLayout( false );
         this.splitContainer3.Panel2.ResumeLayout( false );
         this.splitContainer3.ResumeLayout( false );
         this.groupBox10.ResumeLayout( false );
         this.groupBox11.ResumeLayout( false );
         this.groupBox9.ResumeLayout( false );
         this.groupBox9.PerformLayout();
         this.ResumeLayout( false );

      }

      #endregion

      public System.Windows.Forms.Panel pnlConfig;
      private System.Windows.Forms.SplitContainer splitContainer1;
      public System.Windows.Forms.Button btnResetValues;
      private System.Windows.Forms.SplitContainer splitContainer2;
      private System.Windows.Forms.GroupBox groupBox9;
      public System.Windows.Forms.Button btnWriteUst;
      public System.Windows.Forms.Button btnReadUstFC;
      private System.Windows.Forms.GroupBox groupBox1;
      public System.Windows.Forms.Button btnReNewUstBD;
      private System.Windows.Forms.SplitContainer splitContainer3;
      private System.Windows.Forms.GroupBox groupBox10;
      public System.Windows.Forms.DateTimePicker dtpStartTimeConfig;
      public System.Windows.Forms.DateTimePicker dtpStartDateConfig;
      private System.Windows.Forms.GroupBox groupBox11;
      public System.Windows.Forms.DateTimePicker dtpEndTimeConfig;
      public System.Windows.Forms.DateTimePicker dtpEndDateConfig;
   }
}
