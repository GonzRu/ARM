namespace HMI_MT
{
	partial class dlgOptionsFormEditor
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
			if( disposing && ( components != null ) )
			{
				components.Dispose();
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.gbTypeVisualisation = new System.Windows.Forms.GroupBox();
			this.rbHide = new System.Windows.Forms.RadioButton();
			this.rbAutoVisible = new System.Windows.Forms.RadioButton();
			this.rbNonVisible = new System.Windows.Forms.RadioButton();
			this.rbAlwaysVisible = new System.Windows.Forms.RadioButton();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.treeViewCfg4Cmp = new System.Windows.Forms.TreeView();
			this.dgvCMP = new System.Windows.Forms.DataGridView();
			this.btnGist = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.gbTypeVisualisation.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCMP)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.gbTypeVisualisation);
			this.splitContainer1.Panel1Collapsed = true;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(292, 672);
			this.splitContainer1.SplitterDistance = 103;
			this.splitContainer1.TabIndex = 5;
			// 
			// gbTypeVisualisation
			// 
			this.gbTypeVisualisation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.gbTypeVisualisation.Controls.Add(this.rbHide);
			this.gbTypeVisualisation.Controls.Add(this.rbAutoVisible);
			this.gbTypeVisualisation.Controls.Add(this.rbNonVisible);
			this.gbTypeVisualisation.Controls.Add(this.rbAlwaysVisible);
			this.gbTypeVisualisation.Location = new System.Drawing.Point(12, 5);
			this.gbTypeVisualisation.Name = "gbTypeVisualisation";
			this.gbTypeVisualisation.Size = new System.Drawing.Size(268, 92);
			this.gbTypeVisualisation.TabIndex = 6;
			this.gbTypeVisualisation.TabStop = false;
			this.gbTypeVisualisation.Text = "Тип отображения";
			// 
			// splitContainer2
			// 
			this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.btnGist);
			this.splitContainer2.Panel2.Controls.Add(this.btnOK);
			this.splitContainer2.Panel2.Controls.Add(this.btnCancel);
			this.splitContainer2.Size = new System.Drawing.Size(292, 672);
			this.splitContainer2.SplitterDistance = 632;
			this.splitContainer2.TabIndex = 0;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.treeViewCfg4Cmp);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.dgvCMP);
			this.splitContainer3.Panel2Collapsed = true;
			this.splitContainer3.Size = new System.Drawing.Size(288, 628);
			this.splitContainer3.SplitterDistance = 252;
			this.splitContainer3.TabIndex = 0;
			// 
			// treeViewCfg4Cmp
			// 
			this.treeViewCfg4Cmp.CheckBoxes = true;
			this.treeViewCfg4Cmp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewCfg4Cmp.Location = new System.Drawing.Point(0, 0);
			this.treeViewCfg4Cmp.Name = "treeViewCfg4Cmp";
			this.treeViewCfg4Cmp.Size = new System.Drawing.Size(288, 628);
			this.treeViewCfg4Cmp.TabIndex = 0;
			// 
			// dgvCMP
			// 
			this.dgvCMP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCMP.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvCMP.Location = new System.Drawing.Point(0, 0);
			this.dgvCMP.Name = "dgvCMP";
			this.dgvCMP.Size = new System.Drawing.Size(96, 100);
			this.dgvCMP.TabIndex = 0;
			// 
			// btnGist
			// 
			this.btnGist.Enabled = false;
			this.btnGist.Location = new System.Drawing.Point(10, 6);
			this.btnGist.Name = "btnGist";
			this.btnGist.Size = new System.Drawing.Size(104, 23);
			this.btnGist.TabIndex = 5;
			this.btnGist.Text = "Гистерезис";
			this.btnGist.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(120, 6);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "Принять";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(201, 6);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Отменить";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// dlgOptionsFormEditor
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 672);
			this.ControlBox = false;
			this.Controls.Add(this.splitContainer1);
			this.MinimumSize = new System.Drawing.Size(300, 680);
			this.Name = "dlgOptionsFormEditor";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Выбор тегов для контроля";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.gbTypeVisualisation.ResumeLayout(false);
			this.gbTypeVisualisation.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvCMP)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.GroupBox gbTypeVisualisation;
		private System.Windows.Forms.RadioButton rbAutoVisible;
		private System.Windows.Forms.RadioButton rbAlwaysVisible;
		private System.Windows.Forms.RadioButton rbNonVisible;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnGist;
		private System.Windows.Forms.RadioButton rbHide;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.TreeView treeViewCfg4Cmp;
		private System.Windows.Forms.DataGridView dgvCMP;
	}
}