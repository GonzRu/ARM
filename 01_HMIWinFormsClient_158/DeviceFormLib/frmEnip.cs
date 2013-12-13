using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Calculator;
using LabelTextbox;
using CRZADevices;
using CommonUtils;
using System.Xml.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using FileManager;
using LibraryElements;
using WindowsForms;
using Structure;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace HMI_MT
{
    public partial class frmEnip : frmBMRZbase
    {
        // сортированный список с именами панелей и фреймов
        SortedList DevPanelTypes;

        public frmEnip( MainForm linkMainForm, int iFC, int iIDDev, string fxml )
         : base( linkMainForm, iFC, iIDDev, fxml )
        {
            InitializeComponent();

            #region переупорядочим вкладки, отодвинув базовые назад
		    ArrayList artp = new ArrayList();

            foreach (TabPage tp in tc_Main_frmBMRZbase.TabPages)
            {
                artp.Add(tp);
            }

            int i = artp.Count - 1;

            tc_Main_frmBMRZbase.Multiline = true;  // отображение корешков в несколько рядов

            foreach (TabPage tp in artp)
            {
                tc_Main_frmBMRZbase.TabPages[i] = tp;
                i--;
            }
	        #endregion
        }

        private void frmEnip_Load(object sender, EventArgs e)
        {
            tabpageControl.Enter += new EventHandler(tabpageControl_Enter);
            slTPtoArrVars.Add(tabpageControl.Text, new ArrayList());

            #region по источнику и номеру устройства опрделяем пути к файлам
            path2PrgDevCFG = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";
            XDocument xdoc = XDocument.Load(path2PrgDevCFG);
            IEnumerable<XElement> xes = xdoc.Descendants("FC");
            var xe = (from nn in
                          (from n in xes
                           where n.Attribute("numFC").Value == StrFC
                           select n).Descendants("Device")
                      where nn.Element("NumDev").Value == IIDDev.ToString()//(IIDDev - (256 * IFC)).ToString()
                      select nn).First();

            path2DeviceCFG = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + xe.Element("nameR").Value + Path.DirectorySeparatorChar + "Device.cfg";
            path2FrmDev = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + xe.Element("nameR").Value + Path.DirectorySeparatorChar + "frm" + xe.Element("nameELowLevel").Value + ".xml";
            if (!File.Exists(path2DeviceCFG))
            {
                MessageBox.Show("Файл =" + path2DeviceCFG + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!File.Exists(path2FrmDev))
            {
                MessageBox.Show("Файл =" + path2FrmDev + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            // формируем сортированный список с панелями
            xdoc = XDocument.Load(path2DeviceCFG);
            DevPanelTypes = new SortedList();

            if (!String.IsNullOrEmpty((string)xdoc.Element("Device").Element("TypeOfPanelSections")))
            {
                IEnumerable<XElement> etypes = xdoc.Element("Device").Element("TypeOfPanelSections").Elements("TypeOfPanel");

                foreach (XElement xr in etypes)
                    // определим вариант формата секции TypeOfPanel
                    if ((string)xr.Element("Name") == null)
                        DevPanelTypes.Add(xr.Value, String.Empty);
                    else
                        DevPanelTypes.Add(xr.Element("Name").Value, xr.Element("Caption").Value);
            }
            else
                MessageBox.Show("Типы панелей в файле Device.cfg отсутсвуют", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            GetCCforFLP((ControlCollection)this.Controls);

            // заголовок формы
            //this.Text = xe.Element( "nameR" ).Value + " ( ид.№ " + this.IIDDev.ToString( ) + " )"; // + " " + rr.cwInfo.strRefDesign+ " ( ид.№ " + rr.cwInfo.idDev + " ) - яч. № " + rr.cwInfo.nLoc

            // создаем нижние панели
            //CreateTabPanel();
        }

        private void btnCloseOut1_Click(object sender, EventArgs e)
        {
            byte[] memXOut = new byte[2];

               memXOut[0] = 0;
               memXOut[1] = 1;

               if (parent.newKB.ExecuteCommand(2, 14, "CLS", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                   parent.WriteEventToLog(35, "Команда \"CLS\" ушла в сеть. Устройство - 0.8"
                     , this.Name, true);
        }

        private void btnCloseOut2_Click(object sender, EventArgs e)
        {
            byte[] memXOut = new byte[2];

            memXOut[0] = 0;
            memXOut[1] = 2;

            if (parent.newKB.ExecuteCommand(2, 14, "CLS", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                parent.WriteEventToLog(35, "Команда \"CLS\" ушла в сеть. Устройство - 0.8"
                  , this.Name, true);
        }

        #region Обработчики входов на вкладки
        #region Текущая
        private void tabpageControl_Enter(object sender, EventArgs e)
        {
            /*
            * скрываем панели
            */
            //foreach (UserControl p in arDopPanel)
            //    p.Visible = false;

            //pnlCurrent.Visible = true;

            TabPage tp_this = (TabPage)sender;
            ArrayList arrVars = (ArrayList)slTPtoArrVars[tp_this.Text];
            if (arrVars.Count != 0)
                return;

            PrepareTabPagesForGroup(tp_this.Text, tp_this, ref arrVars, null/*pnlTPControl*/);
            slTPtoArrVars[tp_this.Text] = arrVars; // ref не отрабатывает (?)
            //PrepareAdditionalFLP(pnlCurrent.Controls);

        }
        #endregion
        #endregion
    }
}
