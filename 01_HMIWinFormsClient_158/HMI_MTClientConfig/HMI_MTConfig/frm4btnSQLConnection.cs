using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace HMI_MTConfig
{
   public partial class frm4btnSQLConnection : CustomizeForm
    {
        #region Данные для Отмены
       /// <summary>
       /// таблица со старыми параметрами SQL-соединения 
       /// (для восстановления)
       /// </summary>
       DataTable dtSQLParamsOld;
       string strOldTypeCnt = string.Empty;
        #endregion

       /// <summary>
       /// таблица с параметрами SQL-соединения
       /// </summary>
       DataTable dtSQLParams;

       /// <summary>
       /// StringBuilder для использования вместо String
       /// там где это возможно
       /// </summary>
       StringBuilder sb = new StringBuilder();

        public frm4btnSQLConnection()
        {
            InitializeComponent();
            InitForm();
        }

        /// <summary>
        /// Формирование формы для исходной группы параметров
        /// </summary>
        private void InitForm()
        {
            dtSQLParams = new DataTable();

            dtSQLParams.Columns.Add("Target", typeof(System.String));
            dtSQLParams.Columns["Target"].Caption = "Назначение";

            dtSQLParams.Columns.Add("CaptionValue", typeof(System.String));
            dtSQLParams.Columns["CaptionValue"].Caption = "Значение";

            dgwSQLParams.DataSource = dtSQLParams;

            foreach (DataGridViewColumn dgvc in dgwSQLParams.Columns)
                dgvc.HeaderText = dtSQLParams.Columns[dgvc.Index].Caption;

            #region таблица со старыми параметрами SQL-соединения 
            dtSQLParamsOld = new DataTable();

            dtSQLParamsOld.Columns.Add("Target", typeof(System.String));
            dtSQLParamsOld.Columns["Target"].Caption = "Назначение";

            dtSQLParamsOld.Columns.Add("CaptionValue", typeof(System.String));
            dtSQLParamsOld.Columns["CaptionValue"].Caption = "Значение";            
            #endregion

             #region заполняем форму
            // определяем текущий тип аутентификации
            if (!String.IsNullOrWhiteSpace((string)Program.xdoc_Project_cfg.Element("Project").Element("ConnectionString").Element("TypeConnection").Attribute("TypeAuthentication")))
            {
                sb.Clear();
                sb.Append(Program.xdoc_Project_cfg.Element("Project").Element("ConnectionString").Element("TypeConnection").Attribute("TypeAuthentication").Value);
                
                cbTypeAutent.SelectedItem = sb.ToString();
                strOldTypeCnt = sb.ToString();
            }
            else
            {
                SetError("Некорректное определение секции с описанием параметров соединения к БД");
                Close();
            }
            #endregion
        }

        private void ShowParams4TypeAutentif(string p)
        {
            switch(p)
            {
                case "SQL":                    
                    break;
                case "Windows":
                    break;
                default:
                    SetError("Неправильное значение типа аутентификации - " + p);
                    return;
            }

            XElement xe = Program.xdoc_Project_cfg.Element("Project").Element("ConnectionString").Element("TypeConnection").Element(p);
            IEnumerable<XElement> xes = xe.Elements();
            foreach( XElement xesingle in xes )
            {
                if (String.IsNullOrWhiteSpace((string)xesingle.Attribute("sqlname")))
                    continue;

                // заполняем таблицы
                AddRowInDT(xesingle);
            }
        }
        private void AddRowInDT(XElement xesingle)
        {
            DataRow dr = dtSQLParams.NewRow();

            dr["Target"] = xesingle.Attribute("sqlname").Value;
            dr["CaptionValue"] = xesingle.Attribute("value").Value;

            dtSQLParams.Rows.Add(dr);

            #region аналогично таблица со стар параметрами
            DataRow drr = dtSQLParamsOld.NewRow();

            drr["Target"] = xesingle.Attribute("sqlname").Value;
            drr["CaptionValue"] = xesingle.Attribute("value").Value;

            dtSQLParamsOld.Rows.Add(drr);            
            #endregion
        }

        private void cbTypeAutent_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtSQLParams.Rows.Clear();
            object selecti = (sender as ComboBox).SelectedItem;
            ShowParams4TypeAutentif(selecti.ToString());
        }

        private void cbTypeAutent_Click(object sender, EventArgs e)
        {
            IsDgwEdit = true;
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
            SaveValue(cbTypeAutent.SelectedItem.ToString(), dtSQLParams);
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
            SaveValue(strOldTypeCnt,dtSQLParamsOld);
        }
       /// <summary>
       /// общая функция для сохранения значений (новых или старых)
       /// </summary>
       /// <param name="typeCnt"></param>
        public void SaveValue(string typeCnt, DataTable dt)
        {
            XElement xe = Program.xdoc_Project_cfg.Element("Project").Element("ConnectionString").Element("TypeConnection").Element(typeCnt);
            IEnumerable<XElement> xes = xe.Elements();
            Program.xdoc_Project_cfg.Element("Project").Element("ConnectionString").Element("TypeConnection").Attribute("TypeAuthentication").Value = cbTypeAutent.SelectedItem.ToString();

            foreach (XElement xesingle in xes)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (xesingle.Attribute("sqlname").Value == dr["Target"].ToString())
                        xesingle.Attribute("value").Value = dr["CaptionValue"].ToString();
                }
            }
            IsDgwEdit = false;
         }

        private void dgwSQLParams_MouseClick(object sender, MouseEventArgs e)
        {
            IsDgwEdit = true;
        }

        private void btnVerifaySQLServers_Click(object sender, EventArgs e)
        {
            // создаем новый процесс
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            string path2exefile = AppDomain.CurrentDomain.BaseDirectory + "\\TestSQLConnection\\Microsoft.Data.ConnectionUI.Sample.exe";
            if (!File.Exists(path2exefile))
                throw new Exception("Файл " + path2exefile + " не найден.");

            proc.StartInfo.FileName = path2exefile;
            proc.Start();
        }
    }
}
