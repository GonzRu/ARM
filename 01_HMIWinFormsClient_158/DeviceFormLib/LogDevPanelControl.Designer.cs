namespace DeviceFormLib
{
   partial class LogDevPanelControl
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
            this.pnlLogDev = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.tbNumRTU = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbNum2Header = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbNumByteInEachEventRecord = new System.Windows.Forms.TextBox();
            this.tbNumEvent = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbReasonUnload = new System.Windows.Forms.TextBox();
            this.blbChIdFormat = new System.Windows.Forms.Label();
            this.tbChIdFrmt = new System.Windows.Forms.TextBox();
            this.lblNumUVS = new System.Windows.Forms.Label();
            this.tbNumUVS = new System.Windows.Forms.TextBox();
            this.lblNameDev = new System.Windows.Forms.Label();
            this.tbNameDev = new System.Windows.Forms.TextBox();
            this.lblDateTimeWrFileClockRTU = new System.Windows.Forms.Label();
            this.tbDateTimeWrFileClockRTU = new System.Windows.Forms.TextBox();
            this.btnReNewLogDev = new System.Windows.Forms.Button();
            this.gbEndTime = new System.Windows.Forms.GroupBox();
            this.dtpendtime = new System.Windows.Forms.DateTimePicker();
            this.dtpenddata = new System.Windows.Forms.DateTimePicker();
            this.gbStartTime = new System.Windows.Forms.GroupBox();
            this.dtpstarttime = new System.Windows.Forms.DateTimePicker();
            this.dtpstartdata = new System.Windows.Forms.DateTimePicker();
            this.pnlLogDev.SuspendLayout();
            this.gbEndTime.SuspendLayout();
            this.gbStartTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLogDev
            // 
            this.pnlLogDev.BackColor = System.Drawing.SystemColors.Control;
            this.pnlLogDev.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlLogDev.Controls.Add(this.label2);
            this.pnlLogDev.Controls.Add(this.tbNumRTU);
            this.pnlLogDev.Controls.Add(this.label7);
            this.pnlLogDev.Controls.Add(this.tbNum2Header);
            this.pnlLogDev.Controls.Add(this.label1);
            this.pnlLogDev.Controls.Add(this.label6);
            this.pnlLogDev.Controls.Add(this.tbNumByteInEachEventRecord);
            this.pnlLogDev.Controls.Add(this.tbNumEvent);
            this.pnlLogDev.Controls.Add(this.label5);
            this.pnlLogDev.Controls.Add(this.tbReasonUnload);
            this.pnlLogDev.Controls.Add(this.blbChIdFormat);
            this.pnlLogDev.Controls.Add(this.tbChIdFrmt);
            this.pnlLogDev.Controls.Add(this.lblNumUVS);
            this.pnlLogDev.Controls.Add(this.tbNumUVS);
            this.pnlLogDev.Controls.Add(this.lblNameDev);
            this.pnlLogDev.Controls.Add(this.tbNameDev);
            this.pnlLogDev.Controls.Add(this.lblDateTimeWrFileClockRTU);
            this.pnlLogDev.Controls.Add(this.tbDateTimeWrFileClockRTU);
            this.pnlLogDev.Controls.Add(this.btnReNewLogDev);
            this.pnlLogDev.Controls.Add(this.gbEndTime);
            this.pnlLogDev.Controls.Add(this.gbStartTime);
            this.pnlLogDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLogDev.Location = new System.Drawing.Point(0, 0);
            this.pnlLogDev.Name = "pnlLogDev";
            this.pnlLogDev.Size = new System.Drawing.Size(800, 150);
            this.pnlLogDev.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(498, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "№ уст";
            // 
            // tbNumRTU
            // 
            this.tbNumRTU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNumRTU.Location = new System.Drawing.Point(461, 58);
            this.tbNumRTU.Name = "tbNumRTU";
            this.tbNumRTU.Size = new System.Drawing.Size(36, 20);
            this.tbNumRTU.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(498, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Байт во 2-й части загол.";
            // 
            // tbNum2Header
            // 
            this.tbNum2Header.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNum2Header.Location = new System.Drawing.Point(382, 109);
            this.tbNum2Header.Name = "tbNum2Header";
            this.tbNum2Header.Size = new System.Drawing.Size(56, 20);
            this.tbNum2Header.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(667, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 35);
            this.label1.TabIndex = 15;
            this.label1.Text = "Число байт в записи события";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(667, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Число событий";
            // 
            // tbNumByteInEachEventRecord
            // 
            this.tbNumByteInEachEventRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNumByteInEachEventRecord.Location = new System.Drawing.Point(618, 35);
            this.tbNumByteInEachEventRecord.Name = "tbNumByteInEachEventRecord";
            this.tbNumByteInEachEventRecord.Size = new System.Drawing.Size(43, 20);
            this.tbNumByteInEachEventRecord.TabIndex = 14;
            // 
            // tbNumEvent
            // 
            this.tbNumEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNumEvent.Location = new System.Drawing.Point(618, 6);
            this.tbNumEvent.Name = "tbNumEvent";
            this.tbNumEvent.Size = new System.Drawing.Size(43, 20);
            this.tbNumEvent.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(667, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 33);
            this.label5.TabIndex = 13;
            this.label5.Text = "Причина выгрузки файла из памяти RTU";
            // 
            // tbReasonUnload
            // 
            this.tbReasonUnload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReasonUnload.Location = new System.Drawing.Point(618, 63);
            this.tbReasonUnload.Name = "tbReasonUnload";
            this.tbReasonUnload.Size = new System.Drawing.Size(43, 20);
            this.tbReasonUnload.TabIndex = 12;
            // 
            // blbChIdFormat
            // 
            this.blbChIdFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.blbChIdFormat.AutoSize = true;
            this.blbChIdFormat.Location = new System.Drawing.Point(498, 86);
            this.blbChIdFormat.Name = "blbChIdFormat";
            this.blbChIdFormat.Size = new System.Drawing.Size(133, 13);
            this.blbChIdFormat.TabIndex = 11;
            this.blbChIdFormat.Text = "Числ. идентиф. формата";
            // 
            // tbChIdFrmt
            // 
            this.tbChIdFrmt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbChIdFrmt.Location = new System.Drawing.Point(382, 83);
            this.tbChIdFrmt.Name = "tbChIdFrmt";
            this.tbChIdFrmt.Size = new System.Drawing.Size(56, 20);
            this.tbChIdFrmt.TabIndex = 10;
            // 
            // lblNumUVS
            // 
            this.lblNumUVS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNumUVS.AutoSize = true;
            this.lblNumUVS.Location = new System.Drawing.Point(425, 61);
            this.lblNumUVS.Name = "lblNumUVS";
            this.lblNumUVS.Size = new System.Drawing.Size(38, 13);
            this.lblNumUVS.TabIndex = 9;
            this.lblNumUVS.Text = "№ увс";
            // 
            // tbNumUVS
            // 
            this.tbNumUVS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNumUVS.Location = new System.Drawing.Point(383, 58);
            this.tbNumUVS.Name = "tbNumUVS";
            this.tbNumUVS.Size = new System.Drawing.Size(36, 20);
            this.tbNumUVS.TabIndex = 8;
            // 
            // lblNameDev
            // 
            this.lblNameDev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNameDev.AutoSize = true;
            this.lblNameDev.Location = new System.Drawing.Point(498, 35);
            this.lblNameDev.Name = "lblNameDev";
            this.lblNameDev.Size = new System.Drawing.Size(89, 13);
            this.lblNameDev.TabIndex = 7;
            this.lblNameDev.Text = "Имя устройства";
            // 
            // tbNameDev
            // 
            this.tbNameDev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNameDev.Location = new System.Drawing.Point(382, 32);
            this.tbNameDev.Name = "tbNameDev";
            this.tbNameDev.Size = new System.Drawing.Size(115, 20);
            this.tbNameDev.TabIndex = 6;
            // 
            // lblDateTimeWrFileClockRTU
            // 
            this.lblDateTimeWrFileClockRTU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateTimeWrFileClockRTU.Location = new System.Drawing.Point(498, 6);
            this.lblDateTimeWrFileClockRTU.Name = "lblDateTimeWrFileClockRTU";
            this.lblDateTimeWrFileClockRTU.Size = new System.Drawing.Size(114, 26);
            this.lblDateTimeWrFileClockRTU.TabIndex = 5;
            this.lblDateTimeWrFileClockRTU.Text = "Время и дата записи (по часам RTU)";
            // 
            // tbDateTimeWrFileClockRTU
            // 
            this.tbDateTimeWrFileClockRTU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDateTimeWrFileClockRTU.Location = new System.Drawing.Point(382, 6);
            this.tbDateTimeWrFileClockRTU.Name = "tbDateTimeWrFileClockRTU";
            this.tbDateTimeWrFileClockRTU.Size = new System.Drawing.Size(115, 20);
            this.tbDateTimeWrFileClockRTU.TabIndex = 4;
            // 
            // btnReNewLogDev
            // 
            this.btnReNewLogDev.Location = new System.Drawing.Point(301, 6);
            this.btnReNewLogDev.Name = "btnReNewLogDev";
            this.btnReNewLogDev.Size = new System.Drawing.Size(75, 129);
            this.btnReNewLogDev.TabIndex = 3;
            this.btnReNewLogDev.Text = "Обновить";
            this.btnReNewLogDev.UseVisualStyleBackColor = true;
            // 
            // gbEndTime
            // 
            this.gbEndTime.Controls.Add(this.dtpendtime);
            this.gbEndTime.Controls.Add(this.dtpenddata);
            this.gbEndTime.Location = new System.Drawing.Point(5, 74);
            this.gbEndTime.Name = "gbEndTime";
            this.gbEndTime.Size = new System.Drawing.Size(290, 61);
            this.gbEndTime.TabIndex = 2;
            this.gbEndTime.TabStop = false;
            this.gbEndTime.Text = "Время конца выборки";
            // 
            // dtpendtime
            // 
            this.dtpendtime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpendtime.Location = new System.Drawing.Point(185, 23);
            this.dtpendtime.Name = "dtpendtime";
            this.dtpendtime.ShowUpDown = true;
            this.dtpendtime.Size = new System.Drawing.Size(70, 20);
            this.dtpendtime.TabIndex = 1;
            // 
            // dtpenddata
            // 
            this.dtpenddata.Location = new System.Drawing.Point(16, 23);
            this.dtpenddata.Name = "dtpenddata";
            this.dtpenddata.Size = new System.Drawing.Size(130, 20);
            this.dtpenddata.TabIndex = 0;
            // 
            // gbStartTime
            // 
            this.gbStartTime.Controls.Add(this.dtpstarttime);
            this.gbStartTime.Controls.Add(this.dtpstartdata);
            this.gbStartTime.Location = new System.Drawing.Point(5, 3);
            this.gbStartTime.Name = "gbStartTime";
            this.gbStartTime.Size = new System.Drawing.Size(290, 61);
            this.gbStartTime.TabIndex = 1;
            this.gbStartTime.TabStop = false;
            this.gbStartTime.Text = "Время начала выборки";
            // 
            // dtpstarttime
            // 
            this.dtpstarttime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpstarttime.Location = new System.Drawing.Point(189, 23);
            this.dtpstarttime.Name = "dtpstarttime";
            this.dtpstarttime.ShowUpDown = true;
            this.dtpstarttime.Size = new System.Drawing.Size(70, 20);
            this.dtpstarttime.TabIndex = 1;
            // 
            // dtpstartdata
            // 
            this.dtpstartdata.Location = new System.Drawing.Point(16, 23);
            this.dtpstartdata.Name = "dtpstartdata";
            this.dtpstartdata.Size = new System.Drawing.Size(130, 20);
            this.dtpstartdata.TabIndex = 0;
            // 
            // LogDevPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlLogDev);
            this.MinimumSize = new System.Drawing.Size(800, 150);
            this.Name = "LogDevPanelControl";
            this.Size = new System.Drawing.Size(800, 150);
            this.pnlLogDev.ResumeLayout(false);
            this.pnlLogDev.PerformLayout();
            this.gbEndTime.ResumeLayout(false);
            this.gbStartTime.ResumeLayout(false);
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel pnlLogDev;
      private System.Windows.Forms.Button btnReNewLogDev;
      private System.Windows.Forms.GroupBox gbEndTime;
      private System.Windows.Forms.DateTimePicker dtpendtime;
      private System.Windows.Forms.DateTimePicker dtpenddata;
      private System.Windows.Forms.GroupBox gbStartTime;
      private System.Windows.Forms.DateTimePicker dtpstarttime;
      private System.Windows.Forms.DateTimePicker dtpstartdata;
      private System.Windows.Forms.Label lblNumUVS;
      private System.Windows.Forms.TextBox tbNumUVS;
      private System.Windows.Forms.Label lblNameDev;
      private System.Windows.Forms.TextBox tbNameDev;
      private System.Windows.Forms.Label lblDateTimeWrFileClockRTU;
      private System.Windows.Forms.TextBox tbDateTimeWrFileClockRTU;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.TextBox tbNum2Header;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TextBox tbNumEvent;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox tbReasonUnload;
      private System.Windows.Forms.Label blbChIdFormat;
      private System.Windows.Forms.TextBox tbChIdFrmt;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox tbNumByteInEachEventRecord;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox tbNumRTU;
   }
}
