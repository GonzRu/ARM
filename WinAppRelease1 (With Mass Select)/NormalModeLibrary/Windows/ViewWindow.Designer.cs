namespace NormalModeLibrary.Windows
{
    partial class ViewWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
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
        private void InitializeComponent()
        {
            this.ControlGroupBox = new System.Windows.Forms.GroupBox();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.HideButton = new System.Windows.Forms.Button();
            this.ConfigButton = new System.Windows.Forms.Button();
            this.AlarmButton = new System.Windows.Forms.Button();
            this.ControlGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ControlGroupBox
            // 
            this.ControlGroupBox.Controls.Add(this.HideButton);
            this.ControlGroupBox.Controls.Add(this.ConfigButton);
            this.ControlGroupBox.Controls.Add(this.AlarmButton);
            this.ControlGroupBox.Location = new System.Drawing.Point(3, 182);
            this.ControlGroupBox.Name = "ControlGroupBox";
            this.ControlGroupBox.Size = new System.Drawing.Size(257, 110);
            this.ControlGroupBox.TabIndex = 3;
            this.ControlGroupBox.TabStop = false;
            this.ControlGroupBox.Text = "Управление";
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(3, 3);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(267, 173);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = null;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.elementHost1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ControlGroupBox, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(273, 304);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // HideButton
            // 
            this.HideButton.Location = new System.Drawing.Point(9, 16);
            this.HideButton.Name = "HideButton";
            this.HideButton.Size = new System.Drawing.Size(236, 23);
            this.HideButton.TabIndex = 0;
            this.HideButton.Text = "Скрыть";
            this.HideButton.UseVisualStyleBackColor = true;
            // 
            // ConfigButton
            // 
            this.ConfigButton.Location = new System.Drawing.Point(9, 45);
            this.ConfigButton.Name = "ConfigButton";
            this.ConfigButton.Size = new System.Drawing.Size(236, 23);
            this.ConfigButton.TabIndex = 4;
            this.ConfigButton.Text = "Настроить";
            this.ConfigButton.UseVisualStyleBackColor = true;
            // 
            // AlarmButton
            // 
            this.AlarmButton.Location = new System.Drawing.Point(9, 74);
            this.AlarmButton.Name = "AlarmButton";
            this.AlarmButton.Size = new System.Drawing.Size(236, 30);
            this.AlarmButton.TabIndex = 5;
            this.AlarmButton.Text = "Тревога принята";
            this.AlarmButton.UseVisualStyleBackColor = true;
            // 
            // ViewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 304);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Signals";
            this.Shown += new System.EventHandler(this.ViewWindow_Shown);
            this.ControlGroupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ControlGroupBox;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button HideButton;
        private System.Windows.Forms.Button ConfigButton;
        private System.Windows.Forms.Button AlarmButton;

    }
}