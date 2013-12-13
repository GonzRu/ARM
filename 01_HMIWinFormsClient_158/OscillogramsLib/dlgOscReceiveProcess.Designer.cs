namespace OscillogramsLib
{
   partial class dlgOscReceiveProcess
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
         this.panel1 = new System.Windows.Forms.Panel();
         this.lblOSCTime = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.lblOSCPercent = new System.Windows.Forms.Label();
         this.button1 = new System.Windows.Forms.Button();
         this.panel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel1.Controls.Add(this.button1);
         this.panel1.Controls.Add(this.lblOSCPercent);
         this.panel1.Controls.Add(this.lblOSCTime);
         this.panel1.Controls.Add(this.label4);
         this.panel1.Controls.Add(this.label1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(331, 120);
         this.panel1.TabIndex = 0;
         // 
         // lblOSCTime
         // 
         this.lblOSCTime.AutoSize = true;
         this.lblOSCTime.Location = new System.Drawing.Point(275, 51);
         this.lblOSCTime.Name = "lblOSCTime";
         this.lblOSCTime.Size = new System.Drawing.Size(17, 17);
         this.lblOSCTime.TabIndex = 5;
         this.lblOSCTime.Text = "0";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(18, 51);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(251, 17);
         this.label4.TabIndex = 4;
         this.label4.Text = "Время от начала чтения (сек.): ";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
         this.label1.Location = new System.Drawing.Point(10, 8);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(312, 19);
         this.label1.TabIndex = 0;
         this.label1.Text = "Чтение осциллограммы (диаграммы):";
         // 
         // lblOSCPercent
         // 
         this.lblOSCPercent.AutoSize = true;
         this.lblOSCPercent.Location = new System.Drawing.Point(142, 34);
         this.lblOSCPercent.Name = "lblOSCPercent";
         this.lblOSCPercent.Size = new System.Drawing.Size(20, 17);
         this.lblOSCPercent.TabIndex = 6;
         this.lblOSCPercent.Text = "--";
         // 
         // button1
         // 
         this.button1.AutoSize = true;
         this.button1.Location = new System.Drawing.Point(120, 84);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(89, 27);
         this.button1.TabIndex = 7;
         this.button1.Text = "Прервать";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.btnOSCReceiveCancel_Click);
         // 
         // dlgOscReceiveProcess
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(331, 120);
         this.ControlBox = false;
         this.Controls.Add(this.panel1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "dlgOscReceiveProcess";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "dlgOscReceiveProcess";
         this.TopMost = true;
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.dlgOscReceiveProcess_FormClosing);
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label lblOSCTime;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label lblOSCPercent;
      private System.Windows.Forms.Button button1;
   }
}