using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HMI_MTConfig
{
	public partial class frm4btnGentralCustomise : CustomizeForm
	{
		string chbMinMaxMainWindow_oldvalue;
		string chbHideWindowsStatus_oldvalue;

		public frm4btnGentralCustomise()
		{
			InitializeComponent();
			InitForm();
		}

		private void InitForm()
		{
			chbMinMaxMainWindow.Checked = Convert.ToBoolean(Program.xdoc_Project_cfg.Element("Project").Element("ViewBtn4MainWindow").Value);
			chbMinMaxMainWindow_oldvalue = Program.xdoc_Project_cfg.Element("Project").Element("ViewBtn4MainWindow").Value;
			chbHideWindowsStatus.Checked = Convert.ToBoolean(Program.xdoc_Project_cfg.Element("Project").Element("HideWindowLineStatus").Value);
			chbHideWindowsStatus_oldvalue = Program.xdoc_Project_cfg.Element("Project").Element("HideWindowLineStatus").Value;
		}

		public override void AplayChangesCF()
		{
			if (!IsDgwEdit)
				return;

			SaveNewValue();
		}

		/// <summary>
		/// сохранить новые значения
		/// </summary>
		public override void SaveNewValue()
		{
			Program.xdoc_Project_cfg.Element("Project").Element("ViewBtn4MainWindow").Value = chbMinMaxMainWindow.Checked.ToString().ToLower();
			Program.xdoc_Project_cfg.Element("Project").Element("HideWindowLineStatus").Value = chbHideWindowsStatus.Checked.ToString().ToLower();

			IsDgwEdit = false;
		}
	
		/// <summary>
		/// Отменить из главного окна
		/// </summary>
		public override void CancelChangesCF()
		{
			SaveOldValue();
		}
		/// <summary>
		/// Собственно действие по Отменить
		/// </summary>
		private void SaveOldValue()
		{
			Program.xdoc_Project_cfg.Element("Project").Element("ViewBtn4MainWindow").Value = chbMinMaxMainWindow_oldvalue;
			Program.xdoc_Project_cfg.Element("Project").Element("HideWindowLineStatus").Value = chbHideWindowsStatus_oldvalue;

			IsDgwEdit = false;
		}

		private void chbMinMaxMainWindow_CheckedChanged(object sender, EventArgs e)
		{
			IsDgwEdit = true;
		}
	}
}
