namespace DeviceFormLib
{
   partial class SrabatPanelControl
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
            this.pnlAvar = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnReNew = new System.Windows.Forms.Button();
            this.grbDTStart = new System.Windows.Forms.GroupBox();
            this.dtpStartTimeAvar = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDateAvar = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grbDTFin = new System.Windows.Forms.GroupBox();
            this.dtpEndTimeAvar = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDateAvar = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlAvar.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grbDTStart.SuspendLayout();
            this.grbDTFin.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAvar
            // 
            this.pnlAvar.BackColor = System.Drawing.Color.LightSalmon;
            this.pnlAvar.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlAvar.Controls.Add(this.groupBox1);
            this.pnlAvar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAvar.Location = new System.Drawing.Point(0, 0);
            this.pnlAvar.Name = "pnlAvar";
            this.pnlAvar.Size = new System.Drawing.Size(679, 74);
            this.pnlAvar.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.btnReNew);
            this.groupBox1.Controls.Add(this.grbDTStart);
            this.groupBox1.Controls.Add(this.grbDTFin);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(675, 70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры выборки из базы данных:";
            // 
            // btnReNew
            // 
            this.btnReNew.Location = new System.Drawing.Point(583, 19);
            this.btnReNew.Name = "btnReNew";
            this.btnReNew.Size = new System.Drawing.Size(84, 42);
            this.btnReNew.TabIndex = 11;
            this.btnReNew.Text = "Обновить";
            this.btnReNew.UseVisualStyleBackColor = true;
            // 
            // grbDTStart
            // 
            this.grbDTStart.Controls.Add(this.dtpStartTimeAvar);
            this.grbDTStart.Controls.Add(this.dtpStartDateAvar);
            this.grbDTStart.Controls.Add(this.label2);
            this.grbDTStart.Controls.Add(this.label1);
            this.grbDTStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.grbDTStart.ForeColor = System.Drawing.Color.Black;
            this.grbDTStart.Location = new System.Drawing.Point(6, 19);
            this.grbDTStart.Name = "grbDTStart";
            this.grbDTStart.Size = new System.Drawing.Size(286, 42);
            this.grbDTStart.TabIndex = 9;
            this.grbDTStart.TabStop = false;
            this.grbDTStart.Text = "Время начала выборки:";
            // 
            // dtpStartTimeAvar
            // 
            this.dtpStartTimeAvar.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTimeAvar.Location = new System.Drawing.Point(197, 15);
            this.dtpStartTimeAvar.Name = "dtpStartTimeAvar";
            this.dtpStartTimeAvar.ShowUpDown = true;
            this.dtpStartTimeAvar.Size = new System.Drawing.Size(78, 20);
            this.dtpStartTimeAvar.TabIndex = 3;
            // 
            // dtpStartDateAvar
            // 
            this.dtpStartDateAvar.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDateAvar.Location = new System.Drawing.Point(48, 15);
            this.dtpStartDateAvar.Name = "dtpStartDateAvar";
            this.dtpStartDateAvar.Size = new System.Drawing.Size(94, 20);
            this.dtpStartDateAvar.TabIndex = 2;
            this.dtpStartDateAvar.Value = new System.DateTime(2007, 4, 25, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Время:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Дата:";
            // 
            // grbDTFin
            // 
            this.grbDTFin.Controls.Add(this.dtpEndTimeAvar);
            this.grbDTFin.Controls.Add(this.dtpEndDateAvar);
            this.grbDTFin.Controls.Add(this.label4);
            this.grbDTFin.Controls.Add(this.label3);
            this.grbDTFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.grbDTFin.ForeColor = System.Drawing.Color.Black;
            this.grbDTFin.Location = new System.Drawing.Point(298, 19);
            this.grbDTFin.Name = "grbDTFin";
            this.grbDTFin.Size = new System.Drawing.Size(279, 42);
            this.grbDTFin.TabIndex = 10;
            this.grbDTFin.TabStop = false;
            this.grbDTFin.Text = "Время конца выборки:";
            // 
            // dtpEndTimeAvar
            // 
            this.dtpEndTimeAvar.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTimeAvar.Location = new System.Drawing.Point(196, 15);
            this.dtpEndTimeAvar.Name = "dtpEndTimeAvar";
            this.dtpEndTimeAvar.ShowUpDown = true;
            this.dtpEndTimeAvar.Size = new System.Drawing.Size(75, 20);
            this.dtpEndTimeAvar.TabIndex = 3;
            // 
            // dtpEndDateAvar
            // 
            this.dtpEndDateAvar.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDateAvar.Location = new System.Drawing.Point(48, 14);
            this.dtpEndDateAvar.Name = "dtpEndDateAvar";
            this.dtpEndDateAvar.Size = new System.Drawing.Size(93, 20);
            this.dtpEndDateAvar.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(147, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Время:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Дата:";
            // 
            // SrabatPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.pnlAvar);
            this.Name = "SrabatPanelControl";
            this.Size = new System.Drawing.Size(679, 74);
            this.pnlAvar.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.grbDTStart.ResumeLayout(false);
            this.grbDTStart.PerformLayout();
            this.grbDTFin.ResumeLayout(false);
            this.grbDTFin.PerformLayout();
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel pnlAvar;
      private System.Windows.Forms.GroupBox groupBox1;
      public System.Windows.Forms.Button btnReNew;
      private System.Windows.Forms.GroupBox grbDTStart;
      public System.Windows.Forms.DateTimePicker dtpStartTimeAvar;
      public System.Windows.Forms.DateTimePicker dtpStartDateAvar;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.GroupBox grbDTFin;
      public System.Windows.Forms.DateTimePicker dtpEndTimeAvar;
      public System.Windows.Forms.DateTimePicker dtpEndDateAvar;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label3;
   }
}
