using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonUtils
{
    public partial class dlgAcition4LargeFile : Form
    {
        public long FSize = 0;

        public dlgAcition4LargeFile()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "btnDelete":
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    break;
                case "btnSaveAs":
                    this.DialogResult = System.Windows.Forms.DialogResult.No;
                    break;
                case "btnNoAction":
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
                default:
                break;
            }
        }

        private void dlgAcition4LargeFile_Shown(object sender, EventArgs e)
        {
            lblSize.Text = FSize.ToString();
        }
    }
}
