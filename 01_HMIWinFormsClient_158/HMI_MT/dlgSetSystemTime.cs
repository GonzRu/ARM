using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using InterfaceLibrary;

namespace HMI_MT
{
	public partial class dlgSetSystemTime : Form
	{
		[DllImport( "kernel32.dll" )]
		private extern static uint SetSystemTime( ref SYSTEMTIME lpSystemTime );
		SYSTEMTIME systemTime;

		MainForm parent;
		public dlgSetSystemTime(MainForm mf )
		{
			InitializeComponent();
			parent = mf;
		}

		private void dlgSetSystemTime_Activated( object sender, EventArgs e )
		{
			tbCurData.Text = dtpData.Text = DateTime.Now.ToLongDateString();
			tbCurrTime.Text = dtpTime.Text = DateTime.Now.ToLongTimeString();
			timer1.Enabled = true;
		}

		private void button2_Click( object sender, EventArgs e )
		{
			Close();
		}

		private void button1_Click( object sender, EventArgs e )
		{
			// посылаем команду установки часов ФК (ПТК)
			DateTime dt = dtpData.Value.AddHours(dtpTime.Value.Hour).AddMinutes(dtpTime.Value.Minute).AddSeconds(dtpTime.Value.Second);
			DateTime newDT = dt.ToUniversalTime();
			DateTime oldDt = DateTime.Now.ToUniversalTime();

			TimeSpan tsp = new TimeSpan();
			//DateTime mdt = new DateTime( 1970, 1, 1, 0, 0, 0, 0 );
			//tsp = dt - mdt;

			tsp = newDT - oldDt;

			Int32 i32dut = Convert.ToInt32( tsp.TotalSeconds );//


			byte[] paramss = { 0,0,0,0,0,0,0,0 };
			byte[] pte = BitConverter.GetBytes(i32dut);
			Array.Reverse( pte );
			Array.Copy( pte, paramss, 4 );

            //parent.newKB.ExecuteCommand( 0, 0, "GMT", String.Empty, paramss, parent.toolStripProgressBar1, parent.statusStrip1, parent );
            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, 0, "GMT", paramss, this);

			systemTime.wYear = ( ushort ) newDT.Year;
			systemTime.wMonth = ( ushort ) newDT.Month;
			systemTime.wDayOfWeek = ( ushort ) newDT.DayOfWeek;
			systemTime.wDay = ( ushort ) newDT.Day;
			systemTime.wHour = ( ushort ) newDT.Hour;
			systemTime.wMinute = ( ushort ) newDT.Minute;
			systemTime.wSecond = ( ushort ) newDT.Second;
			systemTime.wMilliseconds = ( ushort ) newDT.Millisecond;

			SetSystemTime( ref systemTime );

			MessageBox.Show("Запрос на установку времени послан", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

			Close();
		}

		private void timer1_Tick( object sender, EventArgs e )
		{
			tbCurrTime.Text = DateTime.Now.ToLongTimeString();	//dtpTime.Text = 
		}

		private void dlgSetSystemTime_FormClosing( object sender, FormClosingEventArgs e )
		{
			timer1.Enabled = false;
		}

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
	}
}