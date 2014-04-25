namespace HelperControlsLibrary
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnMaxmeterReset = new System.Windows.Forms.Button();
            this.btnMaxmeterRead = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnMaxmeterReset);
            this.groupBox2.Controls.Add(this.btnMaxmeterRead);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
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
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.groupBox2);
            this.Name = "MaxMeterControl";
            this.Size = new System.Drawing.Size(317, 56);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button btnMaxmeterReset;
        public System.Windows.Forms.Button btnMaxmeterRead;
        public System.Windows.Forms.GroupBox groupBox2;
    }
}
