using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HMI_MT
{
	public partial class dlgCreateEditCalculatedTag : Form
	{
		/// <summary>
		/// класс для создания формулы
		/// </summary>
		CMPFormula cmpFormula;

		public XElement xe_formula;

		public dlgCreateEditCalculatedTag()
		{
			InitializeComponent();

			cmpFormula = new CMPFormula();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// кнопка выбор тега из конфигурации
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnTagSelect_Click(object sender, EventArgs e)
		{
			AddElementToRez(cmpFormula.SelectTag());
		}

		/// <summary>
		/// кнопка выбор константы из конфигурации
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDefintConstant_Click(object sender, EventArgs e)
		{
			string rezconst = cmpFormula.SelectConst();

			float rezconst2fl = 0;
			// проверки
			if (string.IsNullOrWhiteSpace(rezconst))
				return;

			if (float.TryParse(rezconst, out rezconst2fl))
				AddElementToRez(rezconst);
			else
				MessageBox.Show("Константа задана неверно", "Ввод константы", MessageBoxButtons.OK,MessageBoxIcon.Error);
		}

		/// <summary>
		/// кнопка выбор операции
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDefineOperation_Click(object sender, EventArgs e)
		{
			AddElementToRez(cmpFormula.SelectOperation());
		}

		/// <summary>
		/// добавление элемента в строку результата
		/// </summary>
		/// <param name="newelem"></param>
		private void AddElementToRez(string newelem)
		{
			tbFormula.Text += newelem + " ";
		}

		private void btnDoMemoryFormula_Click(object sender, EventArgs e)
		{
			xe_formula = cmpFormula.ApplyFormula();
			Close();
		}

		private void btnUndo_Click(object sender, EventArgs e)
		{
			tbFormula.Clear();
			cmpFormula.ClearFormula();
		}
	}
}
