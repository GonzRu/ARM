namespace HMI_MTConfig
{
	partial class frm4btnGentralCustomise
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
			this.chbMinMaxMainWindow = new System.Windows.Forms.CheckBox();
			this.chbHideWindowsStatus = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// chbMinMaxMainWindow
			// 
			this.chbMinMaxMainWindow.AutoSize = true;
			this.chbMinMaxMainWindow.Location = new System.Drawing.Point(24, 17);
			this.chbMinMaxMainWindow.Name = "chbMinMaxMainWindow";
			this.chbMinMaxMainWindow.Size = new System.Drawing.Size(294, 17);
			this.chbMinMaxMainWindow.TabIndex = 0;
			this.chbMinMaxMainWindow.Text = "Показывать стандартные кнопки управления окном";
			this.chbMinMaxMainWindow.UseVisualStyleBackColor = true;
			this.chbMinMaxMainWindow.CheckedChanged += new System.EventHandler(this.chbMinMaxMainWindow_CheckedChanged);
			// 
			// chbHideWindowsStatus
			// 
			this.chbHideWindowsStatus.AutoSize = true;
			this.chbHideWindowsStatus.Location = new System.Drawing.Point(24, 40);
			this.chbHideWindowsStatus.Name = "chbHideWindowsStatus";
			this.chbHideWindowsStatus.Size = new System.Drawing.Size(202, 17);
			this.chbHideWindowsStatus.TabIndex = 1;
			this.chbHideWindowsStatus.Text = "Скрывать строку статуса Windows";
			this.chbHideWindowsStatus.UseVisualStyleBackColor = true;
			this.chbHideWindowsStatus.CheckedChanged += new System.EventHandler(this.chbMinMaxMainWindow_CheckedChanged);
			// 
			// frm4btnGentralCustomise
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(352, 297);
			this.Controls.Add(this.chbHideWindowsStatus);
			this.Controls.Add(this.chbMinMaxMainWindow);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frm4btnGentralCustomise";
			this.Text = "frmGeneralCustomise";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chbMinMaxMainWindow;
		private System.Windows.Forms.CheckBox chbHideWindowsStatus;
	}
}