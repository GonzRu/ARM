using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace LabelTextbox
{
	public partial class Clock : UserControl
	{
		public Clock( )
		{
			InitializeComponent();
		}

		private void timer1_Tick( object sender, EventArgs e )
		{
			label1.Text = DateTime.Now.ToLongTimeString();
		}
	}
}
