namespace WindowsForms
{
   partial class MnemoSchemaForm
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
         this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.contextMenuStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // contextMenuStrip1
         // 
         this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator1,
            this.propertiesToolStripMenuItem});
         this.contextMenuStrip1.Name = "contextMenuStrip1";
         this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
         this.contextMenuStrip1.Size = new System.Drawing.Size(147, 120);
         // 
         // cutToolStripMenuItem
         // 
         this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
         this.cutToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
         this.cutToolStripMenuItem.Text = "Вырезать";
         this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
         // 
         // copyToolStripMenuItem
         // 
         this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
         this.copyToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
         this.copyToolStripMenuItem.Text = "Копировать";
         this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
         // 
         // pasteToolStripMenuItem
         // 
         this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
         this.pasteToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
         this.pasteToolStripMenuItem.Text = "Вставить";
         this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
         // 
         // deleteToolStripMenuItem
         // 
         this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
         this.deleteToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
         this.deleteToolStripMenuItem.Text = "Удалить";
         this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
         // 
         // toolStripSeparator1
         // 
         this.toolStripSeparator1.Name = "toolStripSeparator1";
         this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
         // 
         // propertiesToolStripMenuItem
         // 
         this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
         this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
         this.propertiesToolStripMenuItem.Text = "Свойства";
         this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.ClientSize = new System.Drawing.Size(640, 480);
         this.MinimumSize = new System.Drawing.Size(320, 240);
         this.Name = "Form1";
         this.Text = "The Empty Blank";
         this.GotFocus += new System.EventHandler(this.Form1_GotFocus);
         this.contextMenuStrip1.ResumeLayout(false);
         this.ResumeLayout(false);

      }
      #endregion

      private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
      private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
   }
}

