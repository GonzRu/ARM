using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HMI_MT
{
    public partial class frmNormalMode : Form
    {
        //MainMnemo prnt;
        
        public frmNormalMode()
        {
            InitializeComponent();
        }

        //public frmNormalMode(MainMnemo f)
        //{
        //    InitializeComponent();
        //    prnt = f;
        //}

        private void frmNormalMode_MouseLeave(object sender, EventArgs e)
        {
            this.Text = "";
        }

        private void frmNormalMode_MouseEnter(object sender, EventArgs e)
        {
            this.Text = "111";
        }

        private void listView1_MouseEnter(object sender, EventArgs e)
        {
            this.Text = "111";
            this.TopMost = true;
        }

        private void listView1_MouseLeave(object sender, EventArgs e)
        {
            //this.Text = "";
            //prnt.Invalidate();
        }
    }
}