namespace Egida
{
    partial class ClockForm
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
            this.discretClock1 = new Egida.DiscretClock();
            this.SuspendLayout();
            // 
            // discretClock1
            // 
            this.discretClock1.BackColor = System.Drawing.Color.Silver;
            this.discretClock1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.discretClock1.Location = new System.Drawing.Point(0, 0);
            this.discretClock1.Name = "discretClock1";
            this.discretClock1.Size = new System.Drawing.Size(244, 46);
            this.discretClock1.TabIndex = 0;
            // 
            // ClockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 46);
            this.Controls.Add(this.discretClock1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Location = new System.Drawing.Point(100, 100);
            this.Name = "ClockForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Текущее время";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClockFormFormClosing);
            this.Load += new System.EventHandler(this.ClockFormLoad);
            this.Shown += new System.EventHandler(this.ClockFormShown);
            this.ResumeLayout(false);

        }

        #endregion

        private DiscretClock discretClock1;
    }
}