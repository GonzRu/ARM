namespace HMI_MTConfig
{
    partial class frm4btnMnemo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
           this.label1 = new System.Windows.Forms.Label();
           this.lblMnemoCurrent = new System.Windows.Forms.Label();
           this.btnChangeMnemo = new System.Windows.Forms.Button();
           this.SuspendLayout();
           // 
           // label1
           // 
           this.label1.AutoSize = true;
           this.label1.Location = new System.Drawing.Point(12, 9);
           this.label1.Name = "label1";
           this.label1.Size = new System.Drawing.Size(158, 13);
           this.label1.TabIndex = 0;
           this.label1.Text = "Файл текущей мнемосхемы: ";
           // 
           // lblMnemoCurrent
           // 
           this.lblMnemoCurrent.AutoSize = true;
           this.lblMnemoCurrent.Location = new System.Drawing.Point(176, 9);
           this.lblMnemoCurrent.Name = "lblMnemoCurrent";
           this.lblMnemoCurrent.Size = new System.Drawing.Size(0, 13);
           this.lblMnemoCurrent.TabIndex = 1;
           // 
           // btnChangeMnemo
           // 
           this.btnChangeMnemo.AutoSize = true;
           this.btnChangeMnemo.Location = new System.Drawing.Point(15, 39);
           this.btnChangeMnemo.Name = "btnChangeMnemo";
           this.btnChangeMnemo.Size = new System.Drawing.Size(128, 23);
           this.btnChangeMnemo.TabIndex = 2;
           this.btnChangeMnemo.Text = "Выбрать мнемосхему";
           this.btnChangeMnemo.UseVisualStyleBackColor = true;
           this.btnChangeMnemo.Click += new System.EventHandler(this.btnChangeMnemo_Click);
           // 
           // frm4btnMnemo
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.ClientSize = new System.Drawing.Size(292, 266);
           this.Controls.Add(this.btnChangeMnemo);
           this.Controls.Add(this.lblMnemoCurrent);
           this.Controls.Add(this.label1);
           this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
           this.Name = "frm4btnMnemo";
           this.Text = "frm4btnMnemo";
           this.ResumeLayout(false);
           this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMnemoCurrent;
        private System.Windows.Forms.Button btnChangeMnemo;
    }
}