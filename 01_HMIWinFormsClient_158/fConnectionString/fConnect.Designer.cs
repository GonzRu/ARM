﻿namespace fConnectionString
{
    partial class fConnect
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
            if ( disposing && (components != null) )
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
           this.groupBox1 = new System.Windows.Forms.GroupBox();
           this.comboBox2 = new System.Windows.Forms.ComboBox();
           this.button1 = new System.Windows.Forms.Button();
           this.label1 = new System.Windows.Forms.Label();
           this.groupBox2 = new System.Windows.Forms.GroupBox();
           this.checkBox1 = new System.Windows.Forms.CheckBox();
           this.textBox2 = new System.Windows.Forms.TextBox();
           this.textBox1 = new System.Windows.Forms.TextBox();
           this.label3 = new System.Windows.Forms.Label();
           this.label2 = new System.Windows.Forms.Label();
           this.radioButton2 = new System.Windows.Forms.RadioButton();
           this.radioButton1 = new System.Windows.Forms.RadioButton();
           this.groupBox3 = new System.Windows.Forms.GroupBox();
           this.button2 = new System.Windows.Forms.Button();
           this.comboBox1 = new System.Windows.Forms.ComboBox();
           this.panel1 = new System.Windows.Forms.Panel();
           this.pictureBox3 = new System.Windows.Forms.PictureBox();
           this.pictureBox2 = new System.Windows.Forms.PictureBox();
           this.pictureBox1 = new System.Windows.Forms.PictureBox();
           this.button4 = new System.Windows.Forms.Button();
           this.button3 = new System.Windows.Forms.Button();
           this.groupBox1.SuspendLayout();
           this.groupBox2.SuspendLayout();
           this.groupBox3.SuspendLayout();
           this.panel1.SuspendLayout();
           ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
           ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
           ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
           this.SuspendLayout();
           // 
           // groupBox1
           // 
           this.groupBox1.Controls.Add(this.comboBox2);
           this.groupBox1.Controls.Add(this.button1);
           this.groupBox1.Controls.Add(this.label1);
           this.groupBox1.Location = new System.Drawing.Point(12, 12);
           this.groupBox1.Name = "groupBox1";
           this.groupBox1.Size = new System.Drawing.Size(324, 65);
           this.groupBox1.TabIndex = 1;
           this.groupBox1.TabStop = false;
           this.groupBox1.Text = "MS SQL Server";
           // 
           // comboBox2
           // 
           this.comboBox2.FormattingEnabled = true;
           this.comboBox2.Location = new System.Drawing.Point(6, 30);
           this.comboBox2.Name = "comboBox2";
           this.comboBox2.Size = new System.Drawing.Size(201, 21);
           this.comboBox2.TabIndex = 3;
           this.comboBox2.SelectionChangeCommitted += new System.EventHandler(this.comboBox2_TextChanged);
           this.comboBox2.Leave += new System.EventHandler(this.comboBox2_TextChanged);
           this.comboBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox2_KeyDown);
           // 
           // button1
           // 
           this.button1.Location = new System.Drawing.Point(213, 28);
           this.button1.Name = "button1";
           this.button1.Size = new System.Drawing.Size(105, 23);
           this.button1.TabIndex = 2;
           this.button1.Text = "Получить список";
           this.button1.UseVisualStyleBackColor = true;
           this.button1.Click += new System.EventHandler(this.button1_Click);
           // 
           // label1
           // 
           this.label1.AutoSize = true;
           this.label1.Location = new System.Drawing.Point(3, 16);
           this.label1.Name = "label1";
           this.label1.Size = new System.Drawing.Size(77, 13);
           this.label1.TabIndex = 0;
           this.label1.Text = "Имя сервера:";
           // 
           // groupBox2
           // 
           this.groupBox2.Controls.Add(this.checkBox1);
           this.groupBox2.Controls.Add(this.textBox2);
           this.groupBox2.Controls.Add(this.textBox1);
           this.groupBox2.Controls.Add(this.label3);
           this.groupBox2.Controls.Add(this.label2);
           this.groupBox2.Controls.Add(this.radioButton2);
           this.groupBox2.Controls.Add(this.radioButton1);
           this.groupBox2.Location = new System.Drawing.Point(12, 83);
           this.groupBox2.Name = "groupBox2";
           this.groupBox2.Size = new System.Drawing.Size(324, 147);
           this.groupBox2.TabIndex = 2;
           this.groupBox2.TabStop = false;
           this.groupBox2.Text = "Вход на сервер";
           // 
           // checkBox1
           // 
           this.checkBox1.AutoSize = true;
           this.checkBox1.Location = new System.Drawing.Point(147, 117);
           this.checkBox1.Name = "checkBox1";
           this.checkBox1.Size = new System.Drawing.Size(118, 17);
           this.checkBox1.TabIndex = 5;
           this.checkBox1.Text = "Сохранить пароль";
           this.checkBox1.UseVisualStyleBackColor = true;
           // 
           // textBox2
           // 
           this.textBox2.Location = new System.Drawing.Point(147, 91);
           this.textBox2.Name = "textBox2";
           this.textBox2.PasswordChar = '*';
           this.textBox2.Size = new System.Drawing.Size(171, 20);
           this.textBox2.TabIndex = 4;
           // 
           // textBox1
           // 
           this.textBox1.Location = new System.Drawing.Point(147, 65);
           this.textBox1.Name = "textBox1";
           this.textBox1.Size = new System.Drawing.Size(171, 20);
           this.textBox1.TabIndex = 3;
           // 
           // label3
           // 
           this.label3.AutoSize = true;
           this.label3.Location = new System.Drawing.Point(35, 94);
           this.label3.Name = "label3";
           this.label3.Size = new System.Drawing.Size(48, 13);
           this.label3.TabIndex = 3;
           this.label3.Text = "Пароль:";
           // 
           // label2
           // 
           this.label2.AutoSize = true;
           this.label2.Location = new System.Drawing.Point(35, 68);
           this.label2.Name = "label2";
           this.label2.Size = new System.Drawing.Size(106, 13);
           this.label2.TabIndex = 2;
           this.label2.Text = "Имя пользователя:";
           // 
           // radioButton2
           // 
           this.radioButton2.AutoSize = true;
           this.radioButton2.Location = new System.Drawing.Point(9, 42);
           this.radioButton2.Name = "radioButton2";
           this.radioButton2.Size = new System.Drawing.Size(274, 17);
           this.radioButton2.TabIndex = 1;
           this.radioButton2.TabStop = true;
           this.radioButton2.Text = "Использовать проверку подлинности SQL Server";
           this.radioButton2.UseVisualStyleBackColor = true;
           this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
           // 
           // radioButton1
           // 
           this.radioButton1.AutoSize = true;
           this.radioButton1.Location = new System.Drawing.Point(9, 19);
           this.radioButton1.Name = "radioButton1";
           this.radioButton1.Size = new System.Drawing.Size(263, 17);
           this.radioButton1.TabIndex = 0;
           this.radioButton1.TabStop = true;
           this.radioButton1.Text = "Использовать проверку подлинности Windows";
           this.radioButton1.UseVisualStyleBackColor = true;
           this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
           // 
           // groupBox3
           // 
           this.groupBox3.Controls.Add(this.button2);
           this.groupBox3.Controls.Add(this.comboBox1);
           this.groupBox3.Location = new System.Drawing.Point(12, 236);
           this.groupBox3.Name = "groupBox3";
           this.groupBox3.Size = new System.Drawing.Size(324, 56);
           this.groupBox3.TabIndex = 3;
           this.groupBox3.TabStop = false;
           this.groupBox3.Text = "База данных";
           // 
           // button2
           // 
           this.button2.Location = new System.Drawing.Point(175, 19);
           this.button2.Name = "button2";
           this.button2.Size = new System.Drawing.Size(143, 23);
           this.button2.TabIndex = 1;
           this.button2.Text = "Проверить подключение";
           this.button2.UseVisualStyleBackColor = true;
           this.button2.Click += new System.EventHandler(this.button2_Click);
           // 
           // comboBox1
           // 
           this.comboBox1.FormattingEnabled = true;
           this.comboBox1.Location = new System.Drawing.Point(6, 19);
           this.comboBox1.Name = "comboBox1";
           this.comboBox1.Size = new System.Drawing.Size(163, 21);
           this.comboBox1.TabIndex = 0;
           this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
           // 
           // panel1
           // 
           this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
           this.panel1.Controls.Add(this.pictureBox3);
           this.panel1.Controls.Add(this.pictureBox2);
           this.panel1.Controls.Add(this.pictureBox1);
           this.panel1.Controls.Add(this.button4);
           this.panel1.Controls.Add(this.button3);
           this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
           this.panel1.Location = new System.Drawing.Point(0, 298);
           this.panel1.Name = "panel1";
           this.panel1.Size = new System.Drawing.Size(349, 49);
           this.panel1.TabIndex = 4;
           // 
           // pictureBox3
           // 
           this.pictureBox3.Image = global::fConnectionString.Properties.Resources.imac;
           this.pictureBox3.Location = new System.Drawing.Point(3, 3);
           this.pictureBox3.Name = "pictureBox3";
           this.pictureBox3.Size = new System.Drawing.Size(47, 43);
           this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
           this.pictureBox3.TabIndex = 4;
           this.pictureBox3.TabStop = false;
           // 
           // pictureBox2
           // 
           this.pictureBox2.Image = global::fConnectionString.Properties.Resources.user_blue;
           this.pictureBox2.Location = new System.Drawing.Point(56, 3);
           this.pictureBox2.Name = "pictureBox2";
           this.pictureBox2.Size = new System.Drawing.Size(47, 43);
           this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
           this.pictureBox2.TabIndex = 3;
           this.pictureBox2.TabStop = false;
           // 
           // pictureBox1
           // 
           this.pictureBox1.Image = global::fConnectionString.Properties.Resources.DB;
           this.pictureBox1.Location = new System.Drawing.Point(109, 3);
           this.pictureBox1.Name = "pictureBox1";
           this.pictureBox1.Size = new System.Drawing.Size(47, 43);
           this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
           this.pictureBox1.TabIndex = 2;
           this.pictureBox1.TabStop = false;
           // 
           // button4
           // 
           this.button4.Location = new System.Drawing.Point(180, 14);
           this.button4.Name = "button4";
           this.button4.Size = new System.Drawing.Size(75, 23);
           this.button4.TabIndex = 1;
           this.button4.Text = "ОК";
           this.button4.UseVisualStyleBackColor = true;
           this.button4.Click += new System.EventHandler(this.button4_Click);
           // 
           // button3
           // 
           this.button3.Location = new System.Drawing.Point(261, 14);
           this.button3.Name = "button3";
           this.button3.Size = new System.Drawing.Size(75, 23);
           this.button3.TabIndex = 0;
           this.button3.Text = "Отмена";
           this.button3.UseVisualStyleBackColor = true;
           this.button3.Click += new System.EventHandler(this.button3_Click);
           // 
           // fConnect
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.ClientSize = new System.Drawing.Size(349, 347);
           this.Controls.Add(this.panel1);
           this.Controls.Add(this.groupBox3);
           this.Controls.Add(this.groupBox2);
           this.Controls.Add(this.groupBox1);
           this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
           this.MaximumSize = new System.Drawing.Size(355, 371);
           this.MinimumSize = new System.Drawing.Size(355, 371);
           this.Name = "fConnect";
           this.ShowInTaskbar = false;
           this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
           this.Text = "Параметры подключения к БД";
           this.TopMost = true;
           this.Load += new System.EventHandler(this.Form1_Load);
           this.Shown += new System.EventHandler(this.fConnect_Shown);
           this.Activated += new System.EventHandler(this.fConnect_Activated);
           this.groupBox1.ResumeLayout(false);
           this.groupBox1.PerformLayout();
           this.groupBox2.ResumeLayout(false);
           this.groupBox2.PerformLayout();
           this.groupBox3.ResumeLayout(false);
           this.panel1.ResumeLayout(false);
           ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
           ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
           ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
           this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;

    }
}

