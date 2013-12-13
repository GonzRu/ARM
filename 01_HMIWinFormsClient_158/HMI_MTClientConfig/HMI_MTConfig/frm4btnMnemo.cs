using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace HMI_MTConfig
{
   public partial class frm4btnMnemo : CustomizeForm
    {
       #region Данные для Отмены
       /// <summary>
       /// имя старой мнемосхемы
       /// </summary>
       string name4oldmnemo = string.Empty;       
       #endregion

        public frm4btnMnemo()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            XElement xe = Program.xdoc_Project_cfg.Element("Project").Element("Mnemoshems");//.Element("Mnemo").Element("Mnemolevel2").Element("FileName");
           
           if (!String.IsNullOrWhiteSpace((string)xe))
           {
              lblMnemoCurrent.Text = Program.xdoc_Project_cfg.Element("Project").Element("Mnemoshems").Element("Mnemo").Element("Mnemolevel2").Element("FileName").Value;

                string stringForSave = lblMnemoCurrent.Text;
                if (stringForSave.Contains("MnemoSchemas"))
                    stringForSave = stringForSave.Remove(0, 12);
                if (stringForSave.Contains("\\"))
                    stringForSave = stringForSave.Remove(0, 1);

              name4oldmnemo = lblMnemoCurrent.Text;
           } 
        }

        private void btnChangeMnemo_Click(object sender, EventArgs e)
        {
           IsDgwEdit = true;

           OpenFileDialog ofd = new OpenFileDialog();
           ofd.DefaultExt = "mnm";
           ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + "MnemoSchemas";
           ofd.Filter = "файлы мнемосхемы (*.mnm)|*.mnm";
		   switch (ofd.ShowDialog())
		   { 
			   case DialogResult.OK:
				   break;
			   default:
				   return;
		   }

           FileInfo fi = new FileInfo(ofd.FileName);
           lblMnemoCurrent.Text =  fi.Name;
        }

       /// <summary>
       /// Применить из главного окна
       /// </summary>
        public override void AplayChangesCF()
        {
           SaveNewValue();
        }
       /// <summary>
       /// Собственно действие по Применить
       /// </summary>
        public override void SaveNewValue()
        {
           Program.xdoc_Project_cfg.Element("Project").Element("Mnemoshems").Element("Mnemo").Element("Mnemolevel2").Element("FileName").Value = "MnemoSchemas" + Path.DirectorySeparatorChar + lblMnemoCurrent.Text;           
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
            Program.xdoc_Project_cfg.Element("Project").Element("Mnemoshems").Element("Mnemo").Element("Mnemolevel2").Element("FileName").Value = "MnemoSchemas" + Path.DirectorySeparatorChar + name4oldmnemo;
            IsDgwEdit = false;
        }
    }
}
