namespace HMI_MTConfig
{
   partial class frm4btnPublicPrjCaption
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
         this.dgwPrjCaption = new System.Windows.Forms.DataGridView();
         ((System.ComponentModel.ISupportInitialize)(this.dgwPrjCaption)).BeginInit();
         this.SuspendLayout();
         // 
         // dgwPrjCaption
         // 
         this.dgwPrjCaption.AllowUserToAddRows = false;
         this.dgwPrjCaption.AllowUserToDeleteRows = false;
         this.dgwPrjCaption.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
         this.dgwPrjCaption.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         this.dgwPrjCaption.Dock = System.Windows.Forms.DockStyle.Fill;
         this.dgwPrjCaption.Location = new System.Drawing.Point(0, 0);
         this.dgwPrjCaption.Name = "dgwPrjCaption";
         this.dgwPrjCaption.Size = new System.Drawing.Size(292, 266);
         this.dgwPrjCaption.TabIndex = 0;
         this.dgwPrjCaption.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgwPrjCaption_MouseClick);
         // 
         // frm4btnPublicPrjCaption
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(292, 266);
         this.ControlBox = false;
         this.Controls.Add(this.dgwPrjCaption);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "frm4btnPublicPrjCaption";
         this.Text = "frm4btnPublicPrjCaption";
         //this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm4btnPublicPrjCaption_FormClosing);
         ((System.ComponentModel.ISupportInitialize)(this.dgwPrjCaption)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.DataGridView dgwPrjCaption;


   }
}