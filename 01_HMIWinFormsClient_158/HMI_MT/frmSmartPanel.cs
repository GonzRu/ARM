using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HMI_MT
{
   public partial class frmSmartPanel : Form
   {
      public FlowLayoutPanel flp;
      //public Panel pnlCaption;
      //public Label lblCaption;
      public SplitContainer sc;
      Form parent;

      public frmSmartPanel(Form parent)
      {
         InitializeComponent();
         this.parent = parent;
         //this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
         DoubleBuffered = true;

         sc = new SplitContainer();
         sc.Parent = this;
         sc.Dock = DockStyle.Fill;
         sc.Orientation = Orientation.Horizontal;

         //lblCaption = new Label();
         //lblCaption.AutoSize = true;
         //lblCaption.Text = "...";
         //lblCaption.Parent = sc.Panel1;
         //lblCaption.Top = 5;
         //lblCaption.Left = 0;
         //lblCaption.Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold);
         ////lblCaption.MouseDown += new MouseEventHandler(lblCaption_MouseDown);

         sc.SplitterDistance = 1;//// lblCaption.Height;
         sc.Panel1.BackColor = Color.DarkOrange;
         sc.BorderStyle = BorderStyle.FixedSingle;
         sc.IsSplitterFixed = true;
         sc.SplitterWidth = 1;
         sc.Panel1Collapsed = true;
         //sc.Panel1.MouseDown += new MouseEventHandler(sc_MouseDown);
         //this.MouseDown += new MouseEventHandler(SmartPanel_MouseDown);
         flp = new FlowLayoutPanel();
         flp.Parent = sc.Panel2;
         flp.BorderStyle = BorderStyle.FixedSingle;
         flp.Dock = DockStyle.Fill;
         flp.BackColor = Color.Transparent;
         flp.FlowDirection = FlowDirection.TopDown;
         //flp.MouseDown += flp_MouseDown;
         flp.SizeChanged += new EventHandler(flp_SizeChanged);
      }
      private void flp_SizeChanged(object sender, EventArgs ea)
      {
         //this.Height = flp.Height + sc.Panel1.Height;
         //sc.SplitterDistance = lblCaption.Height;
      }

   }
}
