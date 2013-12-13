using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;

namespace HMI_MTConfig
{
	public partial class frm4btnControlDiagOsc : CustomizeForm
	{
		IEnumerable<XElement> xes;
		DataTable dtNew;
		DataTable dtOld;
		public frm4btnControlDiagOsc()
		{
			InitializeComponent();
			InitForm();
		}

		private void InitForm()
		{
			try 
			{
				xes = Program.xdoc_Project_cfg.Element("Project").Element("OscDiagInSummaryLog").Elements();
				dtNew = new DataTable();
				dtNew.Columns.Add("Выбор", typeof(bool));
				dtNew.Columns.Add("Тип", typeof(string));
				dtNew.Columns.Add("Ид записи в БД", typeof(string));
				dtNew.Columns.Add("Расширение файла", typeof(string));
				dtNew.Columns.Add("Название", typeof(string));

				foreach (XElement xe in xes)
				{
					DataRow dr = dtNew.NewRow();
					dr["Выбор"] = xe.Attribute("isenable").Value;
					dr["Тип"] = xe.Attribute("type").Value;
					dr["Ид записи в БД"] = xe.Attribute("value").Value;
					dr["Расширение файла"] = xe.Attribute("fileExtension").Value;
					dr["Название"] = xe.Attribute("CaptionInButton").Value;
					dtNew.Rows.Add(dr);
				}

				dgwOscDiagSupport.DataSource = dtNew;

				foreach (DataGridViewColumn dgvclmn in dgwOscDiagSupport.Columns)
					if (dgvclmn.Name != "Выбор")
						dgvclmn.ReadOnly = true;

				dtOld = dtNew.Copy();
			}catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "frm4btnControlDiagOsc.cs : InitForm()",MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
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
			foreach( XElement xe in xes )
			{
				foreach (DataRow dr in dtNew.Rows)
				{
					if (dr["Тип"] == xe.Attribute("type").Value)
						xe.Attribute("isenable").Value = dr["Выбор"].ToString().ToLower();
				}
			}
			
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
			foreach (XElement xe in xes)
			{
				foreach (DataRow dr in dtOld.Rows)
				{
					if (dr["Тип"] == xe.Attribute("type").Value)
						xe.Attribute("isenable").Value = dr["Выбор"].ToString().ToLower();
				}
			}

			IsDgwEdit = false;
		}

		private void dgwOscDiagSupport_Click(object sender, EventArgs e)
		{
			IsDgwEdit = true;
		}
	}
}
