namespace HMI_MT
{
   partial class frm2Panels
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
      private void InitializeComponent( )
      {
         this.splitContainer1 = new System.Windows.Forms.SplitContainer( );
         this.splitContainer1.SuspendLayout( );
         this.SuspendLayout( );
         // 
         // splitContainer1
         // 
         this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.None;//.FixedSingle;
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
         this.splitContainer1.Name = "splitContainer1";
         this.splitContainer1.Size = new System.Drawing.Size( 1016, 734 );
         this.splitContainer1.SplitterDistance = 510;
         this.splitContainer1.TabIndex = 0;
         // 
         // frm2Panels
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size( 1016, 734 );
         this.Controls.Add( this.splitContainer1 );
         this.Name = "frm2Panels";
         this.Text = "frm2Panels";
         this.Shown += new System.EventHandler( this.frm2Panels_Shown );
         this.Controls.SetChildIndex( this.splitContainer1, 0 );
         this.splitContainer1.ResumeLayout( false );
         this.ResumeLayout( false );
         this.PerformLayout( );

      }

      #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
   }
}