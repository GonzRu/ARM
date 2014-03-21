using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HMI_MT
{
    public partial class PrintHMI : Form
    {
        private MainForm parent;

        public PrintHMI( )
        {
            InitializeComponent();
        }
        public PrintHMI(MainForm linkMainForm)
		{
			InitializeComponent();
            parent = linkMainForm;
        }

        private void printDocument_PrintPage( object sender, System.Drawing.Printing.PrintPageEventArgs e )
        {
            char[] param = { '\n' };
            string[] lines = rtbText.Lines;

            int x = 20;
            int y = 20;
            foreach (string line in lines)
            {
                e.Graphics.DrawString( line, new Font( "Arial", 6 ), Brushes.Black, x,y );
                y += 15;
            }
        }

        private void btnPrint_Click( object sender, EventArgs e )
        {
            try
            {
                printDocument.Print();
            }
            catch 
            {
                MessageBox.Show("Проблемы с печатью");
            }
            
        }

        private void button1_Click( object sender, EventArgs e )
        {
            this.Close();
        }
    }
}