namespace LabelTextbox
{
    partial class ComboBoxVar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.lblCaption = new System.Windows.Forms.Label();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.cbVar = new System.Windows.Forms.ComboBox();
            this.tbText = new LabelTextbox.TextBoxEx();
            this.btnToEnumFrm = new System.Windows.Forms.Button();
            this.pnlContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Location = new System.Drawing.Point(154, 5);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(43, 13);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "Caption";
            // 
            // pnlContainer
            // 
            this.pnlContainer.AutoSize = true;
            this.pnlContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlContainer.Controls.Add(this.cbVar);
            this.pnlContainer.Controls.Add(this.tbText);
            this.pnlContainer.Controls.Add(this.btnToEnumFrm);
            this.pnlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(151, 26);
            this.pnlContainer.TabIndex = 5;
            // 
            // cbVar
            // 
            this.cbVar.FormattingEnabled = true;
            this.cbVar.Location = new System.Drawing.Point(0, 1);
            this.cbVar.MinimumSize = new System.Drawing.Size(80, 0);
            this.cbVar.Name = "cbVar";
            this.cbVar.Size = new System.Drawing.Size(115, 21);
            this.cbVar.TabIndex = 7;
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(3, 2);
            this.tbText.MinimumSize = new System.Drawing.Size(80, 4);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(112, 20);
            this.tbText.TabIndex = 6;
            this.tbText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnToEnumFrm
            // 
            this.btnToEnumFrm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToEnumFrm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnToEnumFrm.Location = new System.Drawing.Point(113, 0);
            this.btnToEnumFrm.Name = "btnToEnumFrm";
            this.btnToEnumFrm.Size = new System.Drawing.Size(35, 23);
            this.btnToEnumFrm.TabIndex = 5;
            this.btnToEnumFrm.Text = "...";
            this.btnToEnumFrm.UseVisualStyleBackColor = true;
            this.btnToEnumFrm.Click += new System.EventHandler(this.btnToEnumFrm_Click);
            // 
            // ComboBoxVar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.lblCaption);
            this.Name = "ComboBoxVar";
            this.Size = new System.Drawing.Size(345, 26);
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.Button btnToEnumFrm;
        public System.Windows.Forms.ComboBox cbVar;
        //private System.Windows.Forms.TextBox tbText;
        public TextBoxEx tbText;
    }
}
