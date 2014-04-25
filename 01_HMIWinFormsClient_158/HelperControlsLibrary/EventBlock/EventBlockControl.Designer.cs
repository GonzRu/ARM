namespace HelperControlsLibrary
{
    partial class EventBlockControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelBottomEventBlock1 = new PanelBottomEventBlock();
            this.lstvEventBlock = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTime_logSystemFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chText_logSystemFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelBottomEventBlock1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lstvEventBlock, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(952, 516);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panelBottomEventBlock1
            // 
            this.panelBottomEventBlock1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBottomEventBlock1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottomEventBlock1.Location = new System.Drawing.Point(3, 444);
            this.panelBottomEventBlock1.Name = "panelBottomEventBlock1";
            this.panelBottomEventBlock1.Size = new System.Drawing.Size(946, 69);
            this.panelBottomEventBlock1.TabIndex = 0;
            // 
            // lstvEventBlock
            // 
            this.lstvEventBlock.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.chTime_logSystemFull,
            this.chText_logSystemFull});
            this.lstvEventBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvEventBlock.GridLines = true;
            this.lstvEventBlock.Location = new System.Drawing.Point(3, 3);
            this.lstvEventBlock.Name = "lstvEventBlock";
            this.lstvEventBlock.Size = new System.Drawing.Size(946, 435);
            this.lstvEventBlock.TabIndex = 1;
            this.lstvEventBlock.UseCompatibleStateImageBehavior = false;
            this.lstvEventBlock.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Width = 1;
            // 
            // chTime_logSystemFull
            // 
            this.chTime_logSystemFull.Text = "Время";
            this.chTime_logSystemFull.Width = 150;
            // 
            // chText_logSystemFull
            // 
            this.chText_logSystemFull.Text = "Текст";
            this.chText_logSystemFull.Width = 1400;
            // 
            // EventBlockControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EventBlockControl";
            this.Size = new System.Drawing.Size(952, 516);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView lstvEventBlock;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader chTime_logSystemFull;
        private System.Windows.Forms.ColumnHeader chText_logSystemFull;
        private PanelBottomEventBlock panelBottomEventBlock1;
    }
}
