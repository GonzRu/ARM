namespace HelperControlsLibrary
{
    partial class ReadWriteUstavkyControl
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
            this.btnResetValues = new System.Windows.Forms.Button();
            this.btnFix4Change = new System.Windows.Forms.CheckBox();
            this.btnWriteUst = new System.Windows.Forms.Button();
            this.btnReadUstFC = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnResetValues
            // 
            this.btnResetValues.Location = new System.Drawing.Point(149, 33);
            this.btnResetValues.Name = "btnResetValues";
            this.btnResetValues.Size = new System.Drawing.Size(140, 23);
            this.btnResetValues.TabIndex = 16;
            this.btnResetValues.Text = "Очистить поля формы";
            this.btnResetValues.UseVisualStyleBackColor = true;
            this.btnResetValues.Click += new System.EventHandler(this.BtnResetValuesClick);
            // 
            // btnFix4Change
            // 
            this.btnFix4Change.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnFix4Change.AutoSize = true;
            this.btnFix4Change.Location = new System.Drawing.Point(3, 33);
            this.btnFix4Change.Name = "btnFix4Change";
            this.btnFix4Change.Size = new System.Drawing.Size(140, 23);
            this.btnFix4Change.TabIndex = 5;
            this.btnFix4Change.Text = "Режим задания уставок";
            this.btnFix4Change.UseVisualStyleBackColor = true;
            this.btnFix4Change.CheckedChanged += new System.EventHandler(this.BtnFix4ChangeCheckedChanged);
            // 
            // btnWriteUst
            // 
            this.btnWriteUst.AutoSize = true;
            this.btnWriteUst.Enabled = false;
            this.btnWriteUst.Location = new System.Drawing.Point(149, 3);
            this.btnWriteUst.Name = "btnWriteUst";
            this.btnWriteUst.Size = new System.Drawing.Size(140, 24);
            this.btnWriteUst.TabIndex = 2;
            this.btnWriteUst.Text = "Запись уставок";
            this.btnWriteUst.UseVisualStyleBackColor = true;
            this.btnWriteUst.Click += new System.EventHandler(this.BtnWriteUstClick);
            // 
            // btnReadUstFC
            // 
            this.btnReadUstFC.AutoSize = true;
            this.btnReadUstFC.Location = new System.Drawing.Point(3, 3);
            this.btnReadUstFC.Name = "btnReadUstFC";
            this.btnReadUstFC.Size = new System.Drawing.Size(140, 24);
            this.btnReadUstFC.TabIndex = 0;
            this.btnReadUstFC.Text = "Чтение уставок";
            this.btnReadUstFC.UseVisualStyleBackColor = true;
            this.btnReadUstFC.Click += new System.EventHandler(this.BtnReadUstFcClick);
            // 
            // ReadWriteUstavkyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnResetValues);
            this.Controls.Add(this.btnWriteUst);
            this.Controls.Add(this.btnFix4Change);
            this.Controls.Add(this.btnReadUstFC);
            this.Name = "ReadWriteUstavkyControl";
            this.Size = new System.Drawing.Size(293, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnWriteUst;
        public System.Windows.Forms.Button btnReadUstFC;
        public System.Windows.Forms.Button btnResetValues;
        public System.Windows.Forms.CheckBox btnFix4Change;
    }
}
