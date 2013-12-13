namespace LabelTextbox
{
    partial class ComboBoxListWindow
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
            if ( disposing && ( components != null ) )
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
            this.flpForEnum = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpForEnum
            // 
            this.flpForEnum.AutoScroll = true;
            this.flpForEnum.AutoSize = true;
            this.flpForEnum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpForEnum.Location = new System.Drawing.Point(0, 0);
            this.flpForEnum.Name = "flpForEnum";
            this.flpForEnum.Size = new System.Drawing.Size(204, 216);
            this.flpForEnum.TabIndex = 0;
            // 
            // ComboBoxListWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 216);
            this.Controls.Add(this.flpForEnum);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(220, 700);
            this.MinimumSize = new System.Drawing.Size(130, 100);
            this.Name = "ComboBoxListWindow";
            this.Load += new System.EventHandler(this.ComboBoxListWindowLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpForEnum;
    }
}