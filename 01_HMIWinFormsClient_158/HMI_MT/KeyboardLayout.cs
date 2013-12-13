using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HMI_MT
{
    public partial class KeyboardLayout : UserControl
    {
        public KeyboardLayout()
        {
            InitializeComponent();

            LoadUC();
        }

        public void LoadUC()
        {
            cbCurLen.SelectedIndexChanged += new EventHandler(cbCurLen_SelectedIndexChanged);

            // установим текущий язык ввода
            InputLanguage defIL = InputLanguage.DefaultInputLanguage;
            InputLanguage curIL = InputLanguage.CurrentInputLanguage;

            switch (curIL.Culture.IetfLanguageTag)
            {
                case "en-US":
                    cbCurLen.SelectedItem = "EN";
                    break;
                case "ru-RU":
                    cbCurLen.SelectedItem = "RU";
                    break;
                default:
                    MessageBox.Show("Неизвестная раскладка клавиатуры : " + curIL.Culture.EnglishName, "Предупреждение авторизации", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
            #warning Принудительная установка английского языка по-умолчанию
            cbCurLen.SelectedItem = "EN";
            cbCurLen.SelectedIndexChanged += new EventHandler(cbCurLen_SelectedIndexChanged);
        }

        /// <summary>
        /// установим выбранный язык в качестве языка ввода
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void cbCurLen_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((sender as ComboBox).SelectedItem.ToString())
            {
                case "EN":
                    InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                    break;
                case "RU":
                    InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("ru-RU"));
                    break;
                default:
                    break;
            }
        }
    }
}
