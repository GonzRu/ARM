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
   public partial class frm4btnCustomTcpIp : CustomizeForm
    {
        #region Данные для Отмены
        /// <summary>
        /// таблица со старыми параметрами ФК 
        /// (для восстановления)
        /// </summary>
        DataTable dtFCsOld;
        string OldAddressTcpClient = string.Empty;
        #endregion

        /// <summary>
        /// таблица ФК
        /// </summary>
        DataTable dtFCs;

        public frm4btnCustomTcpIp()
        {
            InitializeComponent();
            InitForm();
        }
       /// <summary>
      /// Формирование формы для исходной группы параметров
      /// </summary>
        private void InitForm()
        {
            #region если мы сервер, то необходимо скрыть поле с его адресом, т.к. оно нужно только клиенту
            XElement xe = Program.xdoc_Project_cfg.Element("Project").Element("ARMRole");//.Element("Mnemo").Element("Mnemolevel2").Element("FileName");

            if (xe.Attribute("role").Value == "IsTCPServer")
            {
                label1.Visible = false;
                tbDataserverIP.Visible = false;
            }            
            #endregion

            dtFCs = new DataTable();

            dtFCs.Columns.Add("describe", typeof(System.String));
            dtFCs.Columns["describe"].Caption = "Описание";

            dtFCs.Columns.Add("enable", typeof(System.String));
            dtFCs.Columns["enable"].Caption = "Возможность работы с ФК";

            dtFCs.Columns.Add("numFC", typeof(System.String));
            dtFCs.Columns["numFC"].Caption = "Номер ФК";

            dtFCs.Columns.Add("fcadr", typeof(System.String));
            dtFCs.Columns["fcadr"].Caption = "Адрес ФК";

            dgwDescribeSources.DataSource = dtFCs;

            foreach (DataGridViewColumn dgvc in dgwDescribeSources.Columns)
                dgvc.HeaderText = dtFCs.Columns[dgvc.Index].Caption;

            #region заполняем таблицу с адресами ФК
            IEnumerable<XElement> xefcs = Program.xdoc_PrgDevCFG_cdp.Element("MT").Element("Configuration").Elements("FC");
            foreach( XElement xefc in xefcs )
            {
                DataRow dr = dtFCs.NewRow();

                dr["describe"] = xefc.Attribute("describe").Value;
                dr["enable"] = xefc.Attribute("enable").Value;
                dr["numFC"] = xefc.Attribute("numFC").Value;
                dr["fcadr"] = xefc.Attribute("fcadr").Value;

                dtFCs.Rows.Add(dr);

                // аналогично копию таблицы для восстановления      
                dtFCsOld = new DataTable();
                dtFCsOld.Columns.Add("describe", typeof(System.String));
                dtFCsOld.Columns.Add("enable", typeof(System.String));
                dtFCsOld.Columns.Add("numFC", typeof(System.String));
                dtFCsOld.Columns.Add("fcadr", typeof(System.String));

                DataRow drr = dtFCsOld.NewRow();

                drr["describe"] = xefc.Attribute("describe").Value;
                drr["enable"] = xefc.Attribute("enable").Value;
                drr["numFC"] = xefc.Attribute("numFC").Value;
                drr["fcadr"] = xefc.Attribute("fcadr").Value;

                dtFCsOld.Rows.Add(drr);
             }
            #endregion      

 			CustomizeTCPClientUIElements();

			splitContainer1.Panel2Collapsed = false;
        }

       /// <summary>
       /// Настройка textbox c IP-адресом сервера
       /// </summary>
        private void CustomizeTCPClientUIElements()
        {
            tbDataserverIP.Text = Program.xdoctcpclient.Element("Project").Element("NetInterface").Element("IPAddress").Attribute("value").Value;
            OldAddressTcpClient = tbDataserverIP.Text;
        }

        private void dgwDescribeSources_MouseClick(object sender, MouseEventArgs e)
        {
            IsDgwEdit = true;
        }

        private void tbDataserverIP_MouseClick(object sender, MouseEventArgs e)
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
            SaveValue(tbDataserverIP.Text, dtFCs);
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
            SaveValue(OldAddressTcpClient, dtFCsOld);
        }
              /// <summary>
       /// общая функция для сохранения значений (новых или старых)
       /// </summary>
       /// <param name="typeCnt"></param>
        public void SaveValue(string tcpclientaddress, DataTable dt)
        {
            IEnumerable<XElement> xefcs = Program.xdoc_PrgDevCFG_cdp.Element("MT").Element("Configuration").Elements("FC");
            foreach (XElement xefc in xefcs)
                foreach (DataRow dr in dt.Rows)
                {
                    if (xefc.Attribute("numFC").Value == dr["numFC"].ToString())
                    {
                        xefc.Attribute("describe").Value = dr["describe"].ToString();
                        xefc.Attribute("enable").Value = dr["enable"].ToString();
                        xefc.Attribute("numFC").Value = dr["numFC"].ToString();
                        xefc.Attribute("fcadr").Value = dr["fcadr"].ToString();
                    }
                }

            Program.xdoctcpclient.Element("Project").Element("NetInterface").Element("IPAddress").Attribute("value").Value = tcpclientaddress;

            IsDgwEdit = false;
        }
    }
}
