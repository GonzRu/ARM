namespace HMI_MT
{
	partial class dlgDeleteTagsFromCMP
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.btnDeleteTAgs = new System.Windows.Forms.Button();
			this.flp4DeleteTags = new System.Windows.Forms.FlowLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.AutoScroll = true;
			this.splitContainer1.Panel1.Controls.Add(this.flp4DeleteTags);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btnDeleteTAgs);
			this.splitContainer1.Size = new System.Drawing.Size(188, 266);
			this.splitContainer1.SplitterDistance = 230;
			this.splitContainer1.TabIndex = 0;
			// 
			// btnDeleteTAgs
			// 
			this.btnDeleteTAgs.BackColor = System.Drawing.Color.LightSalmon;
			this.btnDeleteTAgs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnDeleteTAgs.Location = new System.Drawing.Point(0, 0);
			this.btnDeleteTAgs.Name = "btnDeleteTAgs";
			this.btnDeleteTAgs.Size = new System.Drawing.Size(188, 32);
			this.btnDeleteTAgs.TabIndex = 0;
			this.btnDeleteTAgs.Text = "Закрыть";
			this.btnDeleteTAgs.UseVisualStyleBackColor = false;
			this.btnDeleteTAgs.Click += new System.EventHandler(this.btnDeleteTAgs_Click);
			// 
			// flp4DeleteTags
			// 
			this.flp4DeleteTags.AutoScroll = true;
			this.flp4DeleteTags.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flp4DeleteTags.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flp4DeleteTags.Location = new System.Drawing.Point(0, 0);
			this.flp4DeleteTags.Name = "flp4DeleteTags";
			this.flp4DeleteTags.Size = new System.Drawing.Size(188, 230);
			this.flp4DeleteTags.TabIndex = 0;
			// 
			// dlgDeleteTagsFromCMP
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(188, 266);
			this.ControlBox = false;
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "dlgDeleteTagsFromCMP";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Удаление тегов";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button btnDeleteTAgs;
		private System.Windows.Forms.FlowLayoutPanel flp4DeleteTags;

	}
}