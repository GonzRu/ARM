namespace HMI_MT
{
   partial class NewMainMnemo
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip_USO_HANDSET = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_usohandset_On = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_usohandset_Off = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_USO_HANDSET.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_USO_HANDSET
            // 
            this.contextMenuStrip_USO_HANDSET.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_usohandset_On,
            this.toolStripMenuItem_usohandset_Off});
            this.contextMenuStrip_USO_HANDSET.Name = "contextMenuStrip1";
            this.contextMenuStrip_USO_HANDSET.Size = new System.Drawing.Size(153, 70);
            // 
            // toolStripMenuItem_usohandset_On
            // 
            this.toolStripMenuItem_usohandset_On.Name = "toolStripMenuItem_usohandset_On";
            this.toolStripMenuItem_usohandset_On.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_usohandset_On.Text = "Замкнуть";
            // 
            // toolStripMenuItem_usohandset_Off
            // 
            this.toolStripMenuItem_usohandset_Off.Name = "toolStripMenuItem_usohandset_Off";
            this.toolStripMenuItem_usohandset_Off.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem_usohandset_Off.Text = "Разомкнуть";
            // 
            // NewMainMnemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Name = "NewMainMnemo";
            this.ShowIcon = false;
            this.Text = "NewMainMnemo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewMainMnemoFormClosing);
            this.Load += new System.EventHandler(this.NewMainMnemoLoad);
            this.Shown += new System.EventHandler(this.NewMainMnemoShown);
            this.contextMenuStrip_USO_HANDSET.ResumeLayout(false);
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ContextMenuStrip contextMenuStrip_USO_HANDSET;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_usohandset_On;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_usohandset_Off;
   }
}