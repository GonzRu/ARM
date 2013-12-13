namespace DeviceFormLib
{
    partial class MaxMeterControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNakopitelReset = new System.Windows.Forms.Button();
            this.btnNakopitelRead = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnMaxmeterReset = new System.Windows.Forms.Button();
            this.btnMaxmeterRead = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnNakopitelReset);
            this.groupBox1.Controls.Add(this.btnNakopitelRead);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(312, 49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Накопитель:";
            // 
            // btnNakopitelReset
            // 
            this.btnNakopitelReset.Location = new System.Drawing.Point(161, 19);
            this.btnNakopitelReset.Name = "btnNakopitelReset";
            this.btnNakopitelReset.Size = new System.Drawing.Size(146, 23);
            this.btnNakopitelReset.TabIndex = 1;
            this.btnNakopitelReset.Text = "Сброс";
            this.btnNakopitelReset.UseVisualStyleBackColor = true;
            // 
            // btnNakopitelRead
            // 
            this.btnNakopitelRead.Location = new System.Drawing.Point(6, 19);
            this.btnNakopitelRead.Name = "btnNakopitelRead";
            this.btnNakopitelRead.Size = new System.Drawing.Size(146, 23);
            this.btnNakopitelRead.TabIndex = 0;
            this.btnNakopitelRead.Text = "Чтение";
            this.btnNakopitelRead.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnMaxmeterReset);
            this.groupBox2.Controls.Add(this.btnMaxmeterRead);
            this.groupBox2.Location = new System.Drawing.Point(321, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(312, 49);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Максметр:";
            // 
            // btnMaxmeterReset
            // 
            this.btnMaxmeterReset.Location = new System.Drawing.Point(161, 19);
            this.btnMaxmeterReset.Name = "btnMaxmeterReset";
            this.btnMaxmeterReset.Size = new System.Drawing.Size(146, 23);
            this.btnMaxmeterReset.TabIndex = 1;
            this.btnMaxmeterReset.Text = "Сброс";
            this.btnMaxmeterReset.UseVisualStyleBackColor = true;
            // 
            // btnMaxmeterRead
            // 
            this.btnMaxmeterRead.Location = new System.Drawing.Point(6, 19);
            this.btnMaxmeterRead.Name = "btnMaxmeterRead";
            this.btnMaxmeterRead.Size = new System.Drawing.Size(146, 23);
            this.btnMaxmeterRead.TabIndex = 0;
            this.btnMaxmeterRead.Text = "Чтение";
            this.btnMaxmeterRead.UseVisualStyleBackColor = true;
            // 
            // MaxMeterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MaxMeterControl";
            this.Size = new System.Drawing.Size(638, 58);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button btnNakopitelReset;
        internal System.Windows.Forms.Button btnNakopitelRead;
        internal System.Windows.Forms.Button btnMaxmeterReset;
        internal System.Windows.Forms.Button btnMaxmeterRead;
        internal System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.GroupBox groupBox2;
    }
}
