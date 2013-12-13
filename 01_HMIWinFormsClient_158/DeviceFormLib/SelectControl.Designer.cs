namespace DeviceFormLib
{
    partial class SelectControl
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.grbDTStart = new System.Windows.Forms.GroupBox();
            this.dtpStartTimeAvar = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDateAvar = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grbDTFin = new System.Windows.Forms.GroupBox();
            this.dtpEndTimeAvar = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDateAvar = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grbDTStart.SuspendLayout();
            this.grbDTFin.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(580, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(108, 42);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Обновить";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // grbDTStart
            // 
            this.grbDTStart.Controls.Add(this.dtpStartTimeAvar);
            this.grbDTStart.Controls.Add(this.dtpStartDateAvar);
            this.grbDTStart.Controls.Add(this.label2);
            this.grbDTStart.Controls.Add(this.label1);
            this.grbDTStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.grbDTStart.ForeColor = System.Drawing.Color.Black;
            this.grbDTStart.Location = new System.Drawing.Point(3, 3);
            this.grbDTStart.Name = "grbDTStart";
            this.grbDTStart.Size = new System.Drawing.Size(286, 42);
            this.grbDTStart.TabIndex = 9;
            this.grbDTStart.TabStop = false;
            this.grbDTStart.Text = "Время начала выборки:";
            // 
            // dtpStartTimeAvar
            // 
            this.dtpStartTimeAvar.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTimeAvar.Location = new System.Drawing.Point(197, 15);
            this.dtpStartTimeAvar.Name = "dtpStartTimeAvar";
            this.dtpStartTimeAvar.ShowUpDown = true;
            this.dtpStartTimeAvar.Size = new System.Drawing.Size(78, 20);
            this.dtpStartTimeAvar.TabIndex = 3;
            // 
            // dtpStartDateAvar
            // 
            this.dtpStartDateAvar.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDateAvar.Location = new System.Drawing.Point(48, 15);
            this.dtpStartDateAvar.Name = "dtpStartDateAvar";
            this.dtpStartDateAvar.Size = new System.Drawing.Size(94, 20);
            this.dtpStartDateAvar.TabIndex = 2;
            this.dtpStartDateAvar.Value = new System.DateTime(2007, 4, 25, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Время:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Дата:";
            // 
            // grbDTFin
            // 
            this.grbDTFin.Controls.Add(this.dtpEndTimeAvar);
            this.grbDTFin.Controls.Add(this.dtpEndDateAvar);
            this.grbDTFin.Controls.Add(this.label4);
            this.grbDTFin.Controls.Add(this.label3);
            this.grbDTFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.grbDTFin.ForeColor = System.Drawing.Color.Black;
            this.grbDTFin.Location = new System.Drawing.Point(295, 3);
            this.grbDTFin.Name = "grbDTFin";
            this.grbDTFin.Size = new System.Drawing.Size(279, 42);
            this.grbDTFin.TabIndex = 10;
            this.grbDTFin.TabStop = false;
            this.grbDTFin.Text = "Время конца выборки:";
            // 
            // dtpEndTimeAvar
            // 
            this.dtpEndTimeAvar.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTimeAvar.Location = new System.Drawing.Point(196, 15);
            this.dtpEndTimeAvar.Name = "dtpEndTimeAvar";
            this.dtpEndTimeAvar.ShowUpDown = true;
            this.dtpEndTimeAvar.Size = new System.Drawing.Size(75, 20);
            this.dtpEndTimeAvar.TabIndex = 3;
            // 
            // dtpEndDateAvar
            // 
            this.dtpEndDateAvar.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDateAvar.Location = new System.Drawing.Point(48, 14);
            this.dtpEndDateAvar.Name = "dtpEndDateAvar";
            this.dtpEndDateAvar.Size = new System.Drawing.Size(93, 20);
            this.dtpEndDateAvar.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(147, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Время:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Дата:";
            // 
            // SelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.grbDTFin);
            this.Controls.Add(this.grbDTStart);
            this.Name = "SelectControl";
            this.Size = new System.Drawing.Size(691, 53);
            this.Load += new System.EventHandler(this.SelectUserControlLoad);
            this.grbDTStart.ResumeLayout(false);
            this.grbDTStart.PerformLayout();
            this.grbDTFin.ResumeLayout(false);
            this.grbDTFin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbDTStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grbDTFin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Button btnUpdate;
        internal System.Windows.Forms.DateTimePicker dtpStartTimeAvar;
        internal System.Windows.Forms.DateTimePicker dtpStartDateAvar;
        internal System.Windows.Forms.DateTimePicker dtpEndTimeAvar;
        internal System.Windows.Forms.DateTimePicker dtpEndDateAvar;
    }
}
