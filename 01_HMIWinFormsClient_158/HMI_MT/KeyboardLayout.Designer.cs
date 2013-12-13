namespace HMI_MT
{
    partial class KeyboardLayout
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cbCurLen = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Язык ввода : ";
            // 
            // cbCurLen
            // 
            this.cbCurLen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurLen.FormattingEnabled = true;
            this.cbCurLen.Items.AddRange(new object[] {
            "RU",
            "EN"});
            this.cbCurLen.Location = new System.Drawing.Point(86, 3);
            this.cbCurLen.Name = "cbCurLen";
            this.cbCurLen.Size = new System.Drawing.Size(45, 21);
            this.cbCurLen.TabIndex = 1;
            // 
            // KeyboardLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbCurLen);
            this.Controls.Add(this.label1);
            this.Name = "KeyboardLayout";
            this.Size = new System.Drawing.Size(137, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbCurLen;
    }
}
