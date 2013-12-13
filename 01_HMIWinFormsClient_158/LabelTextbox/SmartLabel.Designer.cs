namespace LabelTextbox
{
    partial class SmartLabel
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
			  this.components = new System.ComponentModel.Container();
			  this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip( this.components );
			  this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			  this.contextMenuStrip2.SuspendLayout();
			  this.SuspendLayout();
			  // 
			  // contextMenuStrip2
			  // 
			  this.contextMenuStrip2.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1} );
			  this.contextMenuStrip2.Name = "contextMenuStrip2";
			  this.contextMenuStrip2.Size = new System.Drawing.Size( 140, 26 );
			  // 
			  // toolStripMenuItem1
			  // 
			  this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			  this.toolStripMenuItem1.Size = new System.Drawing.Size( 139, 22 );
			  this.toolStripMenuItem1.Text = "Настроить";
			  // 
			  // SmartLabel
			  // 
			  this.ContextMenuStrip = this.contextMenuStrip2;
			  this.contextMenuStrip2.ResumeLayout( false );
			  this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}
