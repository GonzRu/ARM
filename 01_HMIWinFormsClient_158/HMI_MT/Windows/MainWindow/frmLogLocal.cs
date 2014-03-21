using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HMI_MT_Settings;

namespace HMI_MT
{
	public partial class frmLogLocal : Form
	{
		public frmLogLocal( )
		{
			InitializeComponent();
		}

		private void frmLogLocal_Load( object sender, EventArgs e )
		{
			FileInfo fi = new FileInfo(HMI_Settings.pathLogEvent_pnl4 + "\\HMI_MT.log");
			//FileInfo fio = new FileInfo(HMI_Settings.pathLogEvent_pnl4 + "\\~HMI_MT.log" );

			//if( fio.Exists )
			//   fio.Delete();

			fi.CopyTo(HMI_Settings.pathLogEvent_pnl4 + "\\~HMI_MT.log", true );
			richTextBox1.LoadFile( HMI_Settings.pathLogEvent_pnl4 + "\\~HMI_MT.log", RichTextBoxStreamType.PlainText);
		}
	}
}