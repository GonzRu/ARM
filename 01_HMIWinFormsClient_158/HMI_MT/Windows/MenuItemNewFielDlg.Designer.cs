namespace HMI_MT
{
    partial class MenuItemNewFielDlg
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb1600_1280 = new System.Windows.Forms.RadioButton();
            this.rb1280_1024 = new System.Windows.Forms.RadioButton();
            this.rb1024_768 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb1600_1280);
            this.groupBox1.Controls.Add(this.rb1280_1024);
            this.groupBox1.Controls.Add(this.rb1024_768);
            this.groupBox1.Location = new System.Drawing.Point(39, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(129, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Разрешение экрана";
            // 
            // rb1600_1280
            // 
            this.rb1600_1280.AutoSize = true;
            this.rb1600_1280.Location = new System.Drawing.Point(22, 69);
            this.rb1600_1280.Name = "rb1600_1280";
            this.rb1600_1280.Size = new System.Drawing.Size(78, 17);
            this.rb1600_1280.TabIndex = 2;
            this.rb1600_1280.TabStop = true;
            this.rb1600_1280.Text = "1600x1200";
            this.rb1600_1280.UseVisualStyleBackColor = true;
            // 
            // rb1280_1024
            // 
            this.rb1280_1024.AutoSize = true;
            this.rb1280_1024.Location = new System.Drawing.Point(22, 46);
            this.rb1280_1024.Name = "rb1280_1024";
            this.rb1280_1024.Size = new System.Drawing.Size(78, 17);
            this.rb1280_1024.TabIndex = 1;
            this.rb1280_1024.TabStop = true;
            this.rb1280_1024.Text = "1280x1024";
            this.rb1280_1024.UseVisualStyleBackColor = true;
            // 
            // rb1024_768
            // 
            this.rb1024_768.AutoSize = true;
            this.rb1024_768.Location = new System.Drawing.Point(22, 23);
            this.rb1024_768.Name = "rb1024_768";
            this.rb1024_768.Size = new System.Drawing.Size(72, 17);
            this.rb1024_768.TabIndex = 0;
            this.rb1024_768.TabStop = true;
            this.rb1024_768.Text = "1024x768";
            this.rb1024_768.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.Location = new System.Drawing.Point(12, 165);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Применить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(103, 165);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Отменить";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MenuItemNewFielDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 212);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MenuItemNewFielDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Новая мнемосхема";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton rb1600_1280;
        public System.Windows.Forms.RadioButton rb1280_1024;
        public System.Windows.Forms.RadioButton rb1024_768;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}