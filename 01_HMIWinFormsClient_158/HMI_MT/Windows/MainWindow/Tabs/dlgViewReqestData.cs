using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InterfaceLibrary;
namespace HMI_MT
{
    public partial class dlgViewReqestData : Form
    {
        /// <summary>
        /// таблица для храниния 
        /// статистики запросов
        /// тегов
        /// </summary>
        DataTable dtreqinfo;

        public dlgViewReqestData()
        {
            InitializeComponent();

            dtreqinfo = new System.Data.DataTable();
            DataColumn dc = new System.Data.DataColumn("Идентификатор тега", typeof(string));//idTag
            dtreqinfo.Columns.Add(dc);
            dc = new System.Data.DataColumn("Счетчик использования", typeof(Int32));//tagRegCount
            dtreqinfo.Columns.Add(dc);
            dgwRequstInfo.DataSource = dtreqinfo;
                
            for (int i = 0; i < dgwRequstInfo.Columns.Count; i++)
            {
                dgwRequstInfo.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            //ArrayList ads = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServerNumbers();//.GetDataServer()

            //for (int i = 0; i < ads.Count; i++)
            //{
            //    IDataServer ds = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServer((uint)ads[0]);
            //    ds.ReqEntry.OnChangeRequestTags += new ChangeRequestTags(ReqEntry_OnChangeRequestTags);
            //}
        }

        /// <summary>
        /// обработчик события изменения списка подписанных тегов
        /// </summary>
        /// <param name="countChange"></param>
        /// <param name="htReqList"></param>
        void ReqEntry_OnChangeRequestTags(int countChange, Hashtable htReqList)
        {
            lblCntAll.Text = htReqList.Count.ToString();            

            dtreqinfo.Rows.Clear();

            foreach( string key in htReqList.Keys )
			{
			 
                DataRow dr = dtreqinfo.NewRow();
                dr[0] = key;
                dr[1] = htReqList[key];

                dtreqinfo.Rows.Add(dr);
			}
        }

        private void dlgViewReqestData_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ArrayList ads = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServerNumbers();//.GetDataServer()

            //for (int i = 0; i < ads.Count; i++)
            //{
            //    IDataServer ds = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServer((uint)ads[0]);
            //    ds.ReqEntry.OnChangeRequestTags -= new ChangeRequestTags(ReqEntry_OnChangeRequestTags);
            //}
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {

            dtreqinfo.Rows.Clear();

            int cnt = 0;

            ArrayList ads = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServerNumbers();

            for (int i = 0; i < ads.Count; i++)
            {
                IDataServer ds = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServer((uint)ads[0]);
                Hashtable dshasht = ds.ReqEntry.GetTagsReqList();
                foreach (string key in dshasht.Keys)
                {

                    DataRow dr = dtreqinfo.NewRow();
                    dr[0] = key;
                    dr[1] = dshasht[key];

                    dtreqinfo.Rows.Add(dr);
                }
                cnt += dshasht.Count;
            }

            lblCntAll.Text = cnt.ToString();
        }
    }
}
