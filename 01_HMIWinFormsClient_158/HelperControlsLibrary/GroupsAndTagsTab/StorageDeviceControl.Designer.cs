namespace HelperControlsLibrary
{
    partial class StorageDeviceControl
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
            this.btnStorageReset = new System.Windows.Forms.Button();
            this.btnStorageRead = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStorageReset);
            this.groupBox1.Controls.Add(this.btnStorageRead);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(312, 49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Накопитель:";
            // 
            // btnStorageReset
            // 
            this.btnStorageReset.Location = new System.Drawing.Point(161, 19);
            this.btnStorageReset.Name = "btnStorageReset";
            this.btnStorageReset.Size = new System.Drawing.Size(146, 23);
            this.btnStorageReset.TabIndex = 1;
            this.btnStorageReset.Text = "Сброс";
            this.btnStorageReset.UseVisualStyleBackColor = true;
            // 
            // btnStorageRead
            // 
            this.btnStorageRead.Location = new System.Drawing.Point(6, 19);
            this.btnStorageRead.Name = "btnStorageRead";
            this.btnStorageRead.Size = new System.Drawing.Size(146, 23);
            this.btnStorageRead.TabIndex = 0;
            this.btnStorageRead.Text = "Чтение";
            this.btnStorageRead.UseVisualStyleBackColor = true;
            // 
            // StorageDeviceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.groupBox1);
            this.Name = "StorageDeviceControl";
            this.Size = new System.Drawing.Size(316, 56);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button btnStorageReset;
        public System.Windows.Forms.Button btnStorageRead;
        public System.Windows.Forms.GroupBox groupBox1;
    }
}
