using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Data;

namespace LabelTextbox
{
    public partial class SmartPanel : Panel
    {
		 [DllImport("user32", CharSet = CharSet.Auto)] 
		internal extern static bool PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam); 
		[DllImport("user32", CharSet = CharSet.Auto)] 
		internal extern static bool ReleaseCapture();
		 [DllImport( "user32", CharSet = CharSet.Auto )]
		 internal extern static bool SetCapture( ); 
		const uint WM_SYSCOMMAND = 0x0112; 
		const uint DOMOVE = 0xF012; 

		 public FlowLayoutPanel flp;
       private DataGridView dgv;
       //private void 
		 public Label lblCaption;
		 public SplitContainer sc;

       /// <summary>
       /// таблица с информацией о тегах, прикрепляется к datagridview
       /// </summary>
       public DataTable dtTags;

		 Form parent;
		 

        public SmartPanel(Form parent)
        {
            InitializeComponent();
				this.parent = parent;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
				DoubleBuffered = true;

				sc = new SplitContainer();
				sc.Parent = this;
				sc.Dock = DockStyle.Fill;
				sc.Orientation = Orientation.Horizontal;

				lblCaption = new Label();
				lblCaption.AutoSize = true;
				lblCaption.Text = "...";
				lblCaption.Parent = sc.Panel1;
				lblCaption.Top = 5;
				lblCaption.Left = 0;
				lblCaption.Font = new Font( SystemFonts.DefaultFont, FontStyle.Bold );
				lblCaption.MouseDown += new MouseEventHandler( lblCaption_MouseDown );

				sc.SplitterDistance = lblCaption.Height;
				sc.Panel1.BackColor = Color.DarkOrange;
				sc.BorderStyle = BorderStyle.FixedSingle;
				sc.IsSplitterFixed = true;
				sc.SplitterWidth = 1;
				sc.Panel1.MouseDown += new MouseEventHandler( sc_MouseDown );
	 		   this.MouseDown += new MouseEventHandler(SmartPanel_MouseDown);

            

           // flp = new FlowLayoutPanel();
           // flp.Parent = sc.Panel2;
           // flp.BorderStyle = BorderStyle.FixedSingle;
           // flp.Dock = DockStyle.Fill;
           // flp.BackColor = Color.Transparent;
           // flp.MouseDown += flp_MouseDown;
           //flp.SizeChanged += new EventHandler(flp_SizeChanged);

            dgv = new DataGridView();
            dgv.Parent = sc.Panel2;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.Dock = DockStyle.Fill;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersVisible = false;
            dgv.RowHeadersVisible = false;

            this.Dock = DockStyle.Fill;

           dtTags = new DataTable();   //"TableTagsNNP*" + parent.Name + "*" + csw.GetFK().ToString() + "*" + csw.GetDevice().ToString() + "*"
           dtTags.Columns.Add("MainTagDescribe", typeof(System.String));	// строка идентификации тега в конфигурации
           dtTags.Columns.Add("TagName", typeof(System.String));	// название сигнала
           dtTags.Columns.Add("Value", typeof(System.String));	// значение сигнала
           dtTags.Columns.Add("Dimension", typeof(System.String));	// размерность сигнала

        }
		 private void flp_SizeChanged(object sender, EventArgs ea )
		 {
			 this.Height = flp.Height + sc.Panel1.Height;
			 sc.SplitterDistance = lblCaption.Height;
		 }
		 void lblCaption_MouseDown( object sender, MouseEventArgs e )
		 {
			SmartPanel_MouseDown( sender, e );
		 }
		 void sc_MouseDown( object sender, MouseEventArgs e )
		 {
			 SmartPanel_MouseDown( sender, e );
		 }
        //void flp_MouseDown( object sender, MouseEventArgs e )
        //{
        //   SmartPanel_MouseDown( sender, e );
        //}
		 private void SmartPanel_MouseDown( object sender, MouseEventArgs e )
		 {
			 ReleaseCapture();
			 PostMessage( this.Handle, WM_SYSCOMMAND, DOMOVE, 0 );
		 }
		 private void SmartPanel_Paint(object sender, PaintEventArgs pea )
		 {
		 }
    }
}
