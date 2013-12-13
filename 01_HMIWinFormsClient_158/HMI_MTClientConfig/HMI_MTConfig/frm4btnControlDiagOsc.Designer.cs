namespace HMI_MTConfig
{
	partial class frm4btnControlDiagOsc
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
			this.dgwOscDiagSupport = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgwOscDiagSupport)).BeginInit();
			this.SuspendLayout();
			// 
			// dgwOscDiagSupport
			// 
			this.dgwOscDiagSupport.AllowUserToAddRows = false;
			this.dgwOscDiagSupport.AllowUserToDeleteRows = false;
			this.dgwOscDiagSupport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgwOscDiagSupport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgwOscDiagSupport.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgwOscDiagSupport.Location = new System.Drawing.Point(0, 0);
			this.dgwOscDiagSupport.Name = "dgwOscDiagSupport";
			this.dgwOscDiagSupport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgwOscDiagSupport.Size = new System.Drawing.Size(647, 416);
			this.dgwOscDiagSupport.TabIndex = 0;
			this.dgwOscDiagSupport.Click += new System.EventHandler(this.dgwOscDiagSupport_Click);
			// 
			// frm4btnControlDiagOsc
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(647, 416);
			this.Controls.Add(this.dgwOscDiagSupport);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frm4btnControlDiagOsc";
			this.Text = "frm4btnControlDiagOsc";
			((System.ComponentModel.ISupportInitialize)(this.dgwOscDiagSupport)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgwOscDiagSupport;
	}
}