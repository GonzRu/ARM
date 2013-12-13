namespace HMI
{
	partial class MainMnemo
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.controlSwitch1 = new Mnemo.ControlSwitch();
            this.line0_01 = new Mnemo.Line0_0();
            this.SuspendLayout();
            // 
            // controlSwitch1
            // 
            this.controlSwitch1.BackColor = System.Drawing.Color.Transparent;
            this.controlSwitch1.Location = new System.Drawing.Point(90, 115);
            this.controlSwitch1.MaximumSize = new System.Drawing.Size(35, 35);
            this.controlSwitch1.MinimumSize = new System.Drawing.Size(20, 20);
            this.controlSwitch1.Name = "controlSwitch1";
            this.controlSwitch1.Size = new System.Drawing.Size(25, 25);
            this.controlSwitch1.TabIndex = 0;
            // 
            // line0_01
            // 
            this.line0_01.BackColor = System.Drawing.Color.Transparent;
            this.line0_01.DLine = Mnemo.DirectLine.Horizontal;
            this.line0_01.LineW = 1;
            this.line0_01.Location = new System.Drawing.Point(237, 134);
            this.line0_01.MaximumSize = new System.Drawing.Size(1600, 1);
            this.line0_01.MinimumSize = new System.Drawing.Size(6, 6);
            this.line0_01.Name = "line0_01";
            this.line0_01.Size = new System.Drawing.Size(84, 6);
            this.line0_01.TabIndex = 2;
            // 
            // MainMnemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(696, 629);
            this.Controls.Add(this.line0_01);
            this.Controls.Add(this.controlSwitch1);
            this.Name = "MainMnemo";
            this.Text = "Главная мнемосхема";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

		}

		#endregion

		private Mnemo.ControlSwitch controlSwitch1;
		private Mnemo.Line0_0 line0_01;
	}
}