namespace HMI_MT
{
   partial class ViewSingleAvar4Sirius
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.tabpageSrabat = new System.Windows.Forms.TabPage();
         ( (System.ComponentModel.ISupportInitialize) ( this.erp ) ).BeginInit();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         this.tc_Main.SuspendLayout();
         ( (System.ComponentModel.ISupportInitialize) ( this.errorProvider1 ) ).BeginInit();
         this.SuspendLayout();
         // 
         // splitContainer1
         // 
         // 
         // tc_Main
         // 
         this.tc_Main.Controls.Add( this.tabpageSrabat );
         this.tc_Main.Enter += new System.EventHandler( this.tabpageSrabat_Enter );
         // 
         // tabpageSrabat
         // 
         this.tabpageSrabat.Location = new System.Drawing.Point( 4, 22 );
         this.tabpageSrabat.Name = "tabpageSrabat";
         this.tabpageSrabat.Size = new System.Drawing.Size( 996, 593 );
         this.tabpageSrabat.TabIndex = 0;
         this.tabpageSrabat.Text = "Срабатывание";
         this.tabpageSrabat.UseVisualStyleBackColor = true;
         // 
         // ViewSingleAvar4Sirius
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size( 1008, 730 );
         this.Location = new System.Drawing.Point( 0, 0 );
         this.Name = "ViewSingleAvar4Sirius";
         this.Text = "ViewSingleAvar4Sirius";
         this.Load += new System.EventHandler( this.ViewSingleAvar4Sirius_Load );
         ( (System.ComponentModel.ISupportInitialize) ( this.erp ) ).EndInit();
         this.splitContainer1.Panel1.ResumeLayout( false );
         this.splitContainer1.ResumeLayout( false );
         this.tc_Main.ResumeLayout( false );
         ( (System.ComponentModel.ISupportInitialize) ( this.errorProvider1 ) ).EndInit();
         this.ResumeLayout( false );
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TabPage tabpageSrabat;
   }
}