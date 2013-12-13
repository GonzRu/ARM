namespace HMI_MT
{
	partial class dlgCreateEditCalculatedTag
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
			this.tbFormula = new System.Windows.Forms.TextBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnUndo = new System.Windows.Forms.Button();
			this.btnDoMemoryFormula = new System.Windows.Forms.Button();
			this.btnDefineOperation = new System.Windows.Forms.Button();
			this.btnDefintConstant = new System.Windows.Forms.Button();
			this.btnTagSelect = new System.Windows.Forms.Button();
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
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tbFormula);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btnClose);
			this.splitContainer1.Panel2.Controls.Add(this.btnUndo);
			this.splitContainer1.Panel2.Controls.Add(this.btnDoMemoryFormula);
			this.splitContainer1.Panel2.Controls.Add(this.btnDefineOperation);
			this.splitContainer1.Panel2.Controls.Add(this.btnDefintConstant);
			this.splitContainer1.Panel2.Controls.Add(this.btnTagSelect);
			this.splitContainer1.Size = new System.Drawing.Size(675, 58);
			this.splitContainer1.SplitterDistance = 32;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 0;
			// 
			// tbFormula
			// 
			this.tbFormula.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbFormula.Location = new System.Drawing.Point(0, 0);
			this.tbFormula.Name = "tbFormula";
			this.tbFormula.Size = new System.Drawing.Size(675, 20);
			this.tbFormula.TabIndex = 0;
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(537, 3);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(131, 23);
			this.btnClose.TabIndex = 5;
			this.btnClose.Text = "Выйти без сохранения";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnUndo
			// 
			this.btnUndo.Location = new System.Drawing.Point(328, 3);
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Size = new System.Drawing.Size(75, 23);
			this.btnUndo.TabIndex = 4;
			this.btnUndo.Text = "Очистить";
			this.btnUndo.UseVisualStyleBackColor = true;
			this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
			// 
			// btnDoMemoryFormula
			// 
			this.btnDoMemoryFormula.Location = new System.Drawing.Point(409, 3);
			this.btnDoMemoryFormula.Name = "btnDoMemoryFormula";
			this.btnDoMemoryFormula.Size = new System.Drawing.Size(122, 23);
			this.btnDoMemoryFormula.TabIndex = 3;
			this.btnDoMemoryFormula.Text = "Запомнить формулу";
			this.btnDoMemoryFormula.UseVisualStyleBackColor = true;
			this.btnDoMemoryFormula.Click += new System.EventHandler(this.btnDoMemoryFormula_Click);
			// 
			// btnDefineOperation
			// 
			this.btnDefineOperation.Location = new System.Drawing.Point(198, 3);
			this.btnDefineOperation.Name = "btnDefineOperation";
			this.btnDefineOperation.Size = new System.Drawing.Size(124, 23);
			this.btnDefineOperation.TabIndex = 2;
			this.btnDefineOperation.Text = "Выбрать операцию";
			this.btnDefineOperation.UseVisualStyleBackColor = true;
			this.btnDefineOperation.Click += new System.EventHandler(this.btnDefineOperation_Click);
			// 
			// btnDefintConstant
			// 
			this.btnDefintConstant.Location = new System.Drawing.Point(88, 3);
			this.btnDefintConstant.Name = "btnDefintConstant";
			this.btnDefintConstant.Size = new System.Drawing.Size(105, 23);
			this.btnDefintConstant.TabIndex = 1;
			this.btnDefintConstant.Text = "Задать константу";
			this.btnDefintConstant.UseVisualStyleBackColor = true;
			this.btnDefintConstant.Click += new System.EventHandler(this.btnDefintConstant_Click);
			// 
			// btnTagSelect
			// 
			this.btnTagSelect.Location = new System.Drawing.Point(0, 3);
			this.btnTagSelect.Name = "btnTagSelect";
			this.btnTagSelect.Size = new System.Drawing.Size(82, 23);
			this.btnTagSelect.TabIndex = 0;
			this.btnTagSelect.Text = "Выбрать тег";
			this.btnTagSelect.UseVisualStyleBackColor = true;
			this.btnTagSelect.Click += new System.EventHandler(this.btnTagSelect_Click);
			// 
			// dlgCreateEditCalculatedTag
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(675, 58);
			this.ControlBox = false;
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "dlgCreateEditCalculatedTag";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Формула";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox tbFormula;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnUndo;
		private System.Windows.Forms.Button btnDoMemoryFormula;
		private System.Windows.Forms.Button btnDefineOperation;
		private System.Windows.Forms.Button btnDefintConstant;
		private System.Windows.Forms.Button btnTagSelect;
	}
}