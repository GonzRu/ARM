namespace HMI_MT
{
    partial class ActUserCurrentMess
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
			  System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ActUserCurrentMess ) );
			  this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			  this.lstvOutMes = new System.Windows.Forms.ListView();
			  this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			  this.chDateTime = new System.Windows.Forms.ColumnHeader();
			  this.chCodEvent = new System.Windows.Forms.ColumnHeader();
			  this.chTextDescribe = new System.Windows.Forms.ColumnHeader();
			  this.chComment = new System.Windows.Forms.ColumnHeader();
			  this.chFormWindow = new System.Windows.Forms.ColumnHeader();
			  this.chArhivBD = new System.Windows.Forms.ColumnHeader();
			  this.chWriteToLogARM = new System.Windows.Forms.ColumnHeader();
			  this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			  this.ïå÷àòüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			  this.splitContainer1.Panel1.SuspendLayout();
			  this.splitContainer1.SuspendLayout();
			  this.menuStrip1.SuspendLayout();
			  this.SuspendLayout();
			  // 
			  // splitContainer1
			  // 
			  this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			  this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			  this.splitContainer1.Location = new System.Drawing.Point( 0, 24 );
			  this.splitContainer1.Name = "splitContainer1";
			  this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			  // 
			  // splitContainer1.Panel1
			  // 
			  this.splitContainer1.Panel1.Controls.Add( this.lstvOutMes );
			  this.splitContainer1.Size = new System.Drawing.Size( 1156, 603 );
			  this.splitContainer1.SplitterDistance = 571;
			  this.splitContainer1.TabIndex = 0;
			  // 
			  // lstvOutMes
			  // 
			  this.lstvOutMes.BackColor = System.Drawing.SystemColors.Window;
			  this.lstvOutMes.BorderStyle = System.Windows.Forms.BorderStyle.None;
			  this.lstvOutMes.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.chDateTime,
            this.chCodEvent,
            this.chTextDescribe,
            this.chComment,
            this.chFormWindow,
            this.chArhivBD,
            this.chWriteToLogARM} );
			  this.lstvOutMes.Dock = System.Windows.Forms.DockStyle.Fill;
			  this.lstvOutMes.GridLines = true;
			  this.lstvOutMes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			  this.lstvOutMes.Location = new System.Drawing.Point( 0, 0 );
			  this.lstvOutMes.Name = "lstvOutMes";
			  this.lstvOutMes.Size = new System.Drawing.Size( 1154, 569 );
			  this.lstvOutMes.TabIndex = 0;
			  this.lstvOutMes.UseCompatibleStateImageBehavior = false;
			  this.lstvOutMes.View = System.Windows.Forms.View.Details;
			  // 
			  // columnHeader1
			  // 
			  this.columnHeader1.DisplayIndex = 1;
			  this.columnHeader1.Width = 1;
			  // 
			  // chDateTime
			  // 
			  this.chDateTime.DisplayIndex = 0;
			  this.chDateTime.Text = "Äàòà-Âğåìÿ";
			  this.chDateTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			  this.chDateTime.Width = 200;
			  // 
			  // chCodEvent
			  // 
			  this.chCodEvent.Text = "Êîä ñîáûòèÿ";
			  this.chCodEvent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			  this.chCodEvent.Width = 80;
			  // 
			  // chTextDescribe
			  // 
			  this.chTextDescribe.Text = "Îïèñàíèå ñîáûòèÿ";
			  this.chTextDescribe.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			  this.chTextDescribe.Width = 316;
			  // 
			  // chComment
			  // 
			  this.chComment.Text = "Êîììåíòàğèé";
			  this.chComment.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			  this.chComment.Width = 185;
			  // 
			  // chFormWindow
			  // 
			  this.chFormWindow.Text = "Îêíî";
			  this.chFormWindow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			  this.chFormWindow.Width = 110;
			  // 
			  // chArhivBD
			  // 
			  this.chArhivBD.Text = "Àğõèâèğîâàíèå â ÁÄ";
			  this.chArhivBD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			  this.chArhivBD.Width = 130;
			  // 
			  // chWriteToLogARM
			  // 
			  this.chWriteToLogARM.Text = "Çàïèñü â æóğíàë ÀĞÌ";
			  this.chWriteToLogARM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			  this.chWriteToLogARM.Width = 130;
			  // 
			  // menuStrip1
			  // 
			  this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.ïå÷àòüToolStripMenuItem} );
			  this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
			  this.menuStrip1.Name = "menuStrip1";
			  this.menuStrip1.Size = new System.Drawing.Size( 1156, 24 );
			  this.menuStrip1.TabIndex = 1;
			  this.menuStrip1.Text = "menuStrip1";
			  // 
			  // ïå÷àòüToolStripMenuItem
			  // 
			  this.ïå÷àòüToolStripMenuItem.Name = "ïå÷àòüToolStripMenuItem";
			  this.ïå÷àòüToolStripMenuItem.Size = new System.Drawing.Size( 56, 20 );
			  this.ïå÷àòüToolStripMenuItem.Text = "Ïå÷àòü";
			  this.ïå÷àòüToolStripMenuItem.Click += new System.EventHandler( this.ïå÷àòüToolStripMenuItem_Click );
			  // 
			  // ActUserCurrentMess
			  // 
			  this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			  this.ClientSize = new System.Drawing.Size( 1156, 627 );
			  this.ControlBox = false;
			  this.Controls.Add( this.splitContainer1 );
			  this.Controls.Add( this.menuStrip1 );
			  this.DoubleBuffered = true;
			  this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			  this.Icon = ( ( System.Drawing.Icon ) ( resources.GetObject( "$this.Icon" ) ) );
			  this.MaximizeBox = false;
			  this.MinimizeBox = false;
			  this.Name = "ActUserCurrentMess";
			  this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			  this.Text = "Òåêóùèå ñîîáùåíèÿ î äåéñòâèÿõ ïîëüçîâàòåëÿ";
			  this.splitContainer1.Panel1.ResumeLayout( false );
			  this.splitContainer1.ResumeLayout( false );
			  this.menuStrip1.ResumeLayout( false );
			  this.menuStrip1.PerformLayout();
			  this.ResumeLayout( false );
			  this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lstvOutMes;
        private System.Windows.Forms.ColumnHeader chDateTime;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader chTextDescribe;
        private System.Windows.Forms.ColumnHeader chCodEvent;
        private System.Windows.Forms.ColumnHeader chFormWindow;
        private System.Windows.Forms.ColumnHeader chWriteToLogARM;
        private System.Windows.Forms.ColumnHeader chArhivBD;
        private System.Windows.Forms.ColumnHeader chComment;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ïå÷àòüToolStripMenuItem;

    }
}