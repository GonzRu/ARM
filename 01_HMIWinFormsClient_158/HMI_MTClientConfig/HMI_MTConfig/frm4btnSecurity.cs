using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HMI_MTConfig
{
   public partial class frm4btnSecurity : CustomizeForm
    {
        #region Данные для Отмены
        string strOldUserUId = string.Empty;
        #endregion

       /// <summary>
        /// StringBuilder для использования вместо String
        /// там где это возможно
        /// </summary>
        StringBuilder sb = new StringBuilder();

        public frm4btnSecurity()
        {
            InitializeComponent();
            InitForm();
        }

        /// <summary>
        /// Формирование формы для исходной группы параметров
        /// </summary>
        private void InitForm()
        {
            if (!String.IsNullOrWhiteSpace((string)Program.xdoc_Project_cfg.Element("Project").Element("IpForMACResolving").Attribute("mac")))
            {
                sb.Clear();
                sb.Append(Program.xdoc_Project_cfg.Element("Project").Element("IpForMACResolving").Attribute("mac").Value);

                tbUserUId.Text = sb.ToString();
                strOldUserUId = sb.ToString();
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
            Program.xdoc_Project_cfg.Element("Project").Element("IpForMACResolving").Attribute("mac").Value = tbUserUId.Text;
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
            Program.xdoc_Project_cfg.Element("Project").Element("IpForMACResolving").Attribute("mac").Value = strOldUserUId;
            IsDgwEdit = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetError("Не реализовано");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetError("Не реализовано");
        }

        private void tbUserUId_MouseClick(object sender, MouseEventArgs e)
        {
            IsDgwEdit = true;
        }
    }
}
