using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HMI_MT
{
	public partial class frmHelp : Form
	{
		public frmHelp( )
		{
			InitializeComponent();
		}

		private void frmHelp_Load( object sender, EventArgs e )
		{
			richTextBox1.LoadFile( "по+пяю.rtf" );
		}
	}
}