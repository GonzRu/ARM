namespace HelperControlsLibrary
{
   partial class OscDiagPanelControl
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
            this.pnlOscDiag = new System.Windows.Forms.Panel();
            this.btnReNewOD = new System.Windows.Forms.Button();
            this.gbEndTime = new System.Windows.Forms.GroupBox();
            this.dtpendtime = new System.Windows.Forms.DateTimePicker();
            this.dtpenddata = new System.Windows.Forms.DateTimePicker();
            this.gbStartTime = new System.Windows.Forms.GroupBox();
            this.dtpstarttime = new System.Windows.Forms.DateTimePicker();
            this.dtpstartdata = new System.Windows.Forms.DateTimePicker();
            this.pnlOscDiag.SuspendLayout();
            this.gbEndTime.SuspendLayout();
            this.gbStartTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOscDiag
            // 
            this.pnlOscDiag.BackColor = System.Drawing.SystemColors.Control;
            this.pnlOscDiag.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlOscDiag.Controls.Add(this.btnReNewOD);
            this.pnlOscDiag.Controls.Add(this.gbEndTime);
            this.pnlOscDiag.Controls.Add(this.gbStartTime);
            this.pnlOscDiag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOscDiag.Location = new System.Drawing.Point(0, 0);
            this.pnlOscDiag.Name = "pnlOscDiag";
            this.pnlOscDiag.Size = new System.Drawing.Size(958, 110);
            this.pnlOscDiag.TabIndex = 8;
            // 
            // btnReNewOD
            // 
            this.btnReNewOD.Location = new System.Drawing.Point(626, 3);
            this.btnReNewOD.Name = "btnReNewOD";
            this.btnReNewOD.Size = new System.Drawing.Size(75, 61);
            this.btnReNewOD.TabIndex = 3;
            this.btnReNewOD.Text = "Обновить";
            this.btnReNewOD.UseVisualStyleBackColor = true;
            // 
            // gbEndTime
            // 
            this.gbEndTime.Controls.Add(this.dtpendtime);
            this.gbEndTime.Controls.Add(this.dtpenddata);
            this.gbEndTime.Location = new System.Drawing.Point(330, 3);
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
            this.gbStartTime.Location = new System.Drawing.Point(34, 3);
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
            this.dtpstartdata.Location = new System.Drawing.Point(20, 23);
            this.dtpstartdata.Name = "dtpstartdata";
            this.dtpstartdata.Size = new System.Drawing.Size(130, 20);
            this.dtpstartdata.TabIndex = 0;
            // 
            // OscDiagPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlOscDiag);
            this.Name = "OscDiagPanelControl";
            this.Size = new System.Drawing.Size(958, 110);
            this.pnlOscDiag.ResumeLayout(false);
            this.gbEndTime.ResumeLayout(false);
            this.gbStartTime.ResumeLayout(false);
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel pnlOscDiag;
      private System.Windows.Forms.Button btnReNewOD;
      private System.Windows.Forms.GroupBox gbEndTime;
      private System.Windows.Forms.DateTimePicker dtpendtime;
      private System.Windows.Forms.DateTimePicker dtpenddata;
      private System.Windows.Forms.GroupBox gbStartTime;
      private System.Windows.Forms.DateTimePicker dtpstarttime;
      private System.Windows.Forms.DateTimePicker dtpstartdata;
   }
}
