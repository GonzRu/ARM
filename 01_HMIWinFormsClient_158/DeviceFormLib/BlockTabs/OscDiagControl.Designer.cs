namespace DeviceFormLib.BlockTabs
{
    partial class OscDiagControl
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.selectUserControl3 = new DeviceFormLib.SelectControl();
            this.splitContainer_OscDiag = new System.Windows.Forms.SplitContainer();
            this.btnUnionOsc = new System.Windows.Forms.Button();
            this.dgvOscill = new System.Windows.Forms.DataGridView();
            this.clmChBoxOsc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmBlockNameOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockIdOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCommentOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockTimeOsc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmViewOsc = new System.Windows.Forms.DataGridViewButtonColumn();
            this.clmID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.btnUnionDiag = new System.Windows.Forms.Button();
            this.dgvDiag = new System.Windows.Forms.DataGridView();
            this.clmChBoxDiag = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmBlockNameDiag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockIdDiag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCommentDiag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlockTimeDiag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmViewDiag = new System.Windows.Forms.DataGridViewButtonColumn();
            this.clmIDDiag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_OscDiag)).BeginInit();
            this.splitContainer_OscDiag.Panel1.SuspendLayout();
            this.splitContainer_OscDiag.Panel2.SuspendLayout();
            this.splitContainer_OscDiag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOscill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiag)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 1;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Controls.Add(this.selectUserControl3, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.splitContainer_OscDiag, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(839, 476);
            this.tableLayoutPanel9.TabIndex = 2;
            // 
            // selectUserControl3
            // 
            this.selectUserControl3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectUserControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectUserControl3.Location = new System.Drawing.Point(3, 404);
            this.selectUserControl3.Name = "selectUserControl3";
            this.selectUserControl3.Size = new System.Drawing.Size(833, 69);
            this.selectUserControl3.TabIndex = 0;
            // 
            // splitContainer_OscDiag
            // 
            this.splitContainer_OscDiag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_OscDiag.Location = new System.Drawing.Point(3, 3);
            this.splitContainer_OscDiag.Name = "splitContainer_OscDiag";
            // 
            // splitContainer_OscDiag.Panel1
            // 
            this.splitContainer_OscDiag.Panel1.Controls.Add(this.btnUnionOsc);
            this.splitContainer_OscDiag.Panel1.Controls.Add(this.dgvOscill);
            this.splitContainer_OscDiag.Panel1.Controls.Add(this.button2);
            // 
            // splitContainer_OscDiag.Panel2
            // 
            this.splitContainer_OscDiag.Panel2.Controls.Add(this.btnUnionDiag);
            this.splitContainer_OscDiag.Panel2.Controls.Add(this.dgvDiag);
            this.splitContainer_OscDiag.Panel2.Controls.Add(this.button3);
            this.splitContainer_OscDiag.Size = new System.Drawing.Size(833, 395);
            this.splitContainer_OscDiag.SplitterDistance = 415;
            this.splitContainer_OscDiag.TabIndex = 2;
            // 
            // btnUnionOsc
            // 
            this.btnUnionOsc.AutoSize = true;
            this.btnUnionOsc.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnUnionOsc.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnUnionOsc.Enabled = false;
            this.btnUnionOsc.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUnionOsc.Location = new System.Drawing.Point(0, 366);
            this.btnUnionOsc.Name = "btnUnionOsc";
            this.btnUnionOsc.Size = new System.Drawing.Size(415, 29);
            this.btnUnionOsc.TabIndex = 2;
            this.btnUnionOsc.Text = "Объединить осциллограммы";
            this.btnUnionOsc.UseVisualStyleBackColor = false;
            this.btnUnionOsc.Visible = false;
            // 
            // dgvOscill
            // 
            this.dgvOscill.AllowUserToAddRows = false;
            this.dgvOscill.AllowUserToDeleteRows = false;
            this.dgvOscill.AllowUserToResizeRows = false;
            this.dgvOscill.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvOscill.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOscill.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmChBoxOsc,
            this.clmBlockNameOsc,
            this.clmBlockIdOsc,
            this.clmCommentOsc,
            this.clmBlockTimeOsc,
            this.clmViewOsc,
            this.clmID});
            this.dgvOscill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOscill.Location = new System.Drawing.Point(0, 35);
            this.dgvOscill.MultiSelect = false;
            this.dgvOscill.Name = "dgvOscill";
            this.dgvOscill.ReadOnly = true;
            this.dgvOscill.RowHeadersVisible = false;
            this.dgvOscill.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOscill.Size = new System.Drawing.Size(415, 360);
            this.dgvOscill.TabIndex = 1;
            // 
            // clmChBoxOsc
            // 
            this.clmChBoxOsc.HeaderText = "";
            this.clmChBoxOsc.Name = "clmChBoxOsc";
            this.clmChBoxOsc.ReadOnly = true;
            this.clmChBoxOsc.Width = 30;
            // 
            // clmBlockNameOsc
            // 
            this.clmBlockNameOsc.HeaderText = "Имя блока";
            this.clmBlockNameOsc.Name = "clmBlockNameOsc";
            this.clmBlockNameOsc.ReadOnly = true;
            // 
            // clmBlockIdOsc
            // 
            this.clmBlockIdOsc.HeaderText = "Идентификатор блока";
            this.clmBlockIdOsc.Name = "clmBlockIdOsc";
            this.clmBlockIdOsc.ReadOnly = true;
            // 
            // clmCommentOsc
            // 
            this.clmCommentOsc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmCommentOsc.HeaderText = "Комментарий";
            this.clmCommentOsc.Name = "clmCommentOsc";
            this.clmCommentOsc.ReadOnly = true;
            // 
            // clmBlockTimeOsc
            // 
            this.clmBlockTimeOsc.HeaderText = "Время блока";
            this.clmBlockTimeOsc.Name = "clmBlockTimeOsc";
            this.clmBlockTimeOsc.ReadOnly = true;
            // 
            // clmViewOsc
            // 
            this.clmViewOsc.HeaderText = "Просмотр";
            this.clmViewOsc.Name = "clmViewOsc";
            this.clmViewOsc.ReadOnly = true;
            this.clmViewOsc.Text = "Просмотр";
            this.clmViewOsc.UseColumnTextForButtonValue = true;
            // 
            // clmID
            // 
            this.clmID.HeaderText = "Идентификатор";
            this.clmID.Name = "clmID";
            this.clmID.ReadOnly = true;
            this.clmID.Visible = false;
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Enabled = false;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(415, 35);
            this.button2.TabIndex = 0;
            this.button2.Text = "Осциллограммы";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // btnUnionDiag
            // 
            this.btnUnionDiag.AutoSize = true;
            this.btnUnionDiag.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnUnionDiag.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnUnionDiag.Enabled = false;
            this.btnUnionDiag.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUnionDiag.Location = new System.Drawing.Point(0, 366);
            this.btnUnionDiag.Name = "btnUnionDiag";
            this.btnUnionDiag.Size = new System.Drawing.Size(414, 29);
            this.btnUnionDiag.TabIndex = 3;
            this.btnUnionDiag.Text = "Объединить диаграммы";
            this.btnUnionDiag.UseVisualStyleBackColor = false;
            this.btnUnionDiag.Visible = false;
            // 
            // dgvDiag
            // 
            this.dgvDiag.AllowUserToAddRows = false;
            this.dgvDiag.AllowUserToDeleteRows = false;
            this.dgvDiag.AllowUserToResizeRows = false;
            this.dgvDiag.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvDiag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiag.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmChBoxDiag,
            this.clmBlockNameDiag,
            this.clmBlockIdDiag,
            this.clmCommentDiag,
            this.clmBlockTimeDiag,
            this.clmViewDiag,
            this.clmIDDiag});
            this.dgvDiag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDiag.Location = new System.Drawing.Point(0, 35);
            this.dgvDiag.MultiSelect = false;
            this.dgvDiag.Name = "dgvDiag";
            this.dgvDiag.ReadOnly = true;
            this.dgvDiag.RowHeadersVisible = false;
            this.dgvDiag.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDiag.Size = new System.Drawing.Size(414, 360);
            this.dgvDiag.TabIndex = 2;
            // 
            // clmChBoxDiag
            // 
            this.clmChBoxDiag.HeaderText = "";
            this.clmChBoxDiag.Name = "clmChBoxDiag";
            this.clmChBoxDiag.ReadOnly = true;
            this.clmChBoxDiag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.clmChBoxDiag.Width = 30;
            // 
            // clmBlockNameDiag
            // 
            this.clmBlockNameDiag.HeaderText = "Имя блока";
            this.clmBlockNameDiag.Name = "clmBlockNameDiag";
            this.clmBlockNameDiag.ReadOnly = true;
            // 
            // clmBlockIdDiag
            // 
            this.clmBlockIdDiag.HeaderText = "Идентификатор блока";
            this.clmBlockIdDiag.Name = "clmBlockIdDiag";
            this.clmBlockIdDiag.ReadOnly = true;
            this.clmBlockIdDiag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clmCommentDiag
            // 
            this.clmCommentDiag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmCommentDiag.HeaderText = "Комментарий";
            this.clmCommentDiag.Name = "clmCommentDiag";
            this.clmCommentDiag.ReadOnly = true;
            this.clmCommentDiag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clmBlockTimeDiag
            // 
            this.clmBlockTimeDiag.HeaderText = "Время блока";
            this.clmBlockTimeDiag.Name = "clmBlockTimeDiag";
            this.clmBlockTimeDiag.ReadOnly = true;
            // 
            // clmViewDiag
            // 
            this.clmViewDiag.HeaderText = "Просмотр";
            this.clmViewDiag.Name = "clmViewDiag";
            this.clmViewDiag.ReadOnly = true;
            this.clmViewDiag.Text = "Просмотр";
            this.clmViewDiag.UseColumnTextForButtonValue = true;
            // 
            // clmIDDiag
            // 
            this.clmIDDiag.HeaderText = "Идентификатор";
            this.clmIDDiag.Name = "clmIDDiag";
            this.clmIDDiag.ReadOnly = true;
            this.clmIDDiag.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmIDDiag.Visible = false;
            // 
            // button3
            // 
            this.button3.AutoSize = true;
            this.button3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button3.Dock = System.Windows.Forms.DockStyle.Top;
            this.button3.Enabled = false;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(0, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(414, 35);
            this.button3.TabIndex = 1;
            this.button3.Text = "Диаграммы";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // OscDiagControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel9);
            this.Name = "OscDiagControl";
            this.Size = new System.Drawing.Size(839, 476);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.splitContainer_OscDiag.Panel1.ResumeLayout(false);
            this.splitContainer_OscDiag.Panel1.PerformLayout();
            this.splitContainer_OscDiag.Panel2.ResumeLayout(false);
            this.splitContainer_OscDiag.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_OscDiag)).EndInit();
            this.splitContainer_OscDiag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOscill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiag)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private SelectControl selectUserControl3;
        private System.Windows.Forms.Button btnUnionOsc;
        private System.Windows.Forms.DataGridView dgvOscill;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmChBoxOsc;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockNameOsc;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockIdOsc;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCommentOsc;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockTimeOsc;
        private System.Windows.Forms.DataGridViewButtonColumn clmViewOsc;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmID;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnUnionDiag;
        private System.Windows.Forms.DataGridView dgvDiag;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmChBoxDiag;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockNameDiag;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockIdDiag;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCommentDiag;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBlockTimeDiag;
        private System.Windows.Forms.DataGridViewButtonColumn clmViewDiag;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmIDDiag;
        private System.Windows.Forms.Button button3;
        protected internal System.Windows.Forms.SplitContainer splitContainer_OscDiag;
    }
}
