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
using System.Globalization;
using System.Threading;

namespace HMI_MT
{
   public partial class frmBresler : frmBMRZbase
   {
      // нижние панели
      CurrentPanelControl pnlCurrent;
      OscDiagPanelControl pnlOscDiag;

      DataTable dtO;    // таблица с осциллограммами
      DataTable dtG;    // таблица с диаграммами

      SortedList slLocal;

      public frmBresler()
      {
         InitializeComponent();
      }

      public frmBresler(MainForm linkMainForm, int iFC, int iIDDev, string fxml)
         : base( linkMainForm, iFC, iIDDev, fxml )
      {
         InitializeComponent( );

         //переупорядочим вкладки, отодвинув базовые назад
         ArrayList artp = new ArrayList( );

         foreach ( TabPage tp in tc_Main_frmBMRZbase.TabPages )
         {
            artp.Add( tp );
         }

         int i = artp.Count - 1;

         tc_Main_frmBMRZbase.Multiline = true;  // отображение корешков в несколько рядов

         foreach ( TabPage tp in artp )
         {
            tc_Main_frmBMRZbase.TabPages [ i ] = tp;
            i--;
         }
      }

      private void frmBresler_Load(object sender, EventArgs e)
      {
         tabpageControl.Enter += new EventHandler(tabpageControl_Enter);
         slTPtoArrVars.Add(tabpageControl.Text, new ArrayList());

         #region по источнику и номеру устройства опрделяем пути к файлам
         path2PrgDevCFG = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";
         XDocument xdoc = XDocument.Load(path2PrgDevCFG);
         IEnumerable<XElement> xes = xdoc.Descendants("PS");
         var xe = (from nn in
                      (from n in xes
                       where n.Attribute("numFC").Value == StrFC
                       select n).Descendants("Device")
                   where nn.Element("NumDev").Value == IIDDev.ToString()  //( IIDDev - ( 256 * IFC ) )
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
         CreateTabPanel();
      }

      #region Обработчики входов на вкладки
      #region Текущая
      void tabpageControl_Enter(object sender, EventArgs e)
      {
         /*
          * скрываем панели
          */
         foreach (UserControl p in arDopPanel)
            p.Visible = false;

         //pnlCurrent.Visible = true;

         TabPage tp_this = (TabPage)sender;
         ArrayList arrVars = (ArrayList)slTPtoArrVars[tp_this.Text];
         if (arrVars.Count != 0)
            return;

         PrepareTabPagesForGroup(tp_this.Text, tp_this, ref arrVars, null/*pnlTPControl*/ );
         slTPtoArrVars[tp_this.Text] = arrVars; // ref не отрабатывает (?)
         PrepareAdditionalFLP(pnlCurrent.Controls);
      }

      /// <summary>
      /// сформировать виз элементы на доп панелях MTRANamedFLPanel, кот принадлежат некоторому контролу
      /// </summary>
      private void PrepareAdditionalFLP(Control.ControlCollection cntrlCC)
      {
         // новые flp нужно добавить в список, скорее всего они там уже есть
         GetCCforFLP(cntrlCC);

         /* 
          * читаем файл с доп панелями, имя файла или файлов
          * извлекаем из frmxxx.xml,
          * формируем arrList для каждой из них, 
          * добавляем их в slTPtoArrVars
          * создаем виз элементы для формул
          * и отображаем
          */
         if (!File.Exists(path2FrmDev))
            throw new Exception("Файл не найден : " + path2FrmDev);

         XDocument xdoc_frm = XDocument.Load(path2FrmDev);
         string faddname = String.Empty;

         if (!String.IsNullOrEmpty((string)xdoc_frm.Element("MT").Element("FileAdditionalFLP")))
            faddname = xdoc_frm.Element("MT").Element("FileAdditionalFLP").Element("Name").Value;
         else
         {
            MessageBox.Show("В файле описания формы нет ссылки на доп панель", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }

         faddname = Path.GetDirectoryName(path2FrmDev) + Path.DirectorySeparatorChar + faddname;

         if (!File.Exists(faddname))
         {
            MessageBox.Show("Файл с описанием доп панели не найден : " + faddname, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }

         XDocument xdoc_addflp;
         try
         {
            xdoc_addflp = XDocument.Load(faddname);
         }
         catch (Exception e)
         {
            throw new Exception("Ошибка в формате xml-документа: " + faddname);
         }

         IEnumerable<XElement> flpframes = xdoc_addflp.Element("MT").Element("AdditionalFLP").Elements("FLPframe");

         foreach (XElement flpframe in flpframes)
         {
            // формируем массив формул
            ArrayList arrf = GetArrFrmls(flpframe, String.Empty);

            // отображаем
            PlaceVisElemOnForm("", flpframe.Attribute("MTFLPNameR").Value, arrf);
         }
      }
      #endregion

      #region Диаграммы и осщиллограммы
      private void tabPageOscDiag_Enter(object sender, EventArgs e)
      {
         foreach (UserControl p in arDopPanel)
            p.Visible = false;

         pnlOscDiag.Visible = true;

         //DiagBD();
         OscBD();
      }

      void btnReNewOscDg_Click(object sender, EventArgs e)
      {
         //DiagBD();
         OscBD();
      }

      #region Осциллограммы и диаграммы (запросы из БД)
      /// <summary>
      /// получение осциллограмм из базы
      /// </summary>
      public void OscBD()
      {
         dgvOscill.Rows.Clear();
         // получение строк соединения и поставщика данных из файла *.config
         //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
         SqlConnection asqlconnect = new SqlConnection(HMI_Settings.cstr);
         try
         {
            asqlconnect.Open();
         }
         catch (SqlException ex)
         {
            string errorMes = "";
            // интеграция всех возвращаемых ошибок
            foreach (SqlError connectError in ex.Errors)
               errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ")" + Environment.NewLine;
            parent.WriteEventToLog(21, "Нет связи с БД (OscBD): " + errorMes, this.Name, false);//, true, false ); // событие нет связи с БД
            System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " : frmBMRZ : Нет связи с БД (OscBD)");
            asqlconnect.Close();
            return;
         }
         catch (Exception ex)
         {
            MessageBox.Show("Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            asqlconnect.Close();
            return;
         }
         // формирование данных для вызова хранимой процедуры
         SqlCommand cmd = new SqlCommand("ShowDataLog", asqlconnect);
         cmd.CommandType = CommandType.StoredProcedure;

         // входные параметры
         // 1. ip FC
         SqlParameter pipFC = new SqlParameter();
         pipFC.ParameterName = "@IP";
         pipFC.SqlDbType = SqlDbType.BigInt;
         pipFC.Value = 0;
         pipFC.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(pipFC);
         // 2. id устройства
         SqlParameter pidBlock = new SqlParameter();
         pidBlock.ParameterName = "@id";
         pidBlock.SqlDbType = SqlDbType.Int;
         pidBlock.Value = IFC * 256 + IIDDev;//IIDDev;;
         pidBlock.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(pidBlock);

         // 3. начальное время
         SqlParameter dtMim = new SqlParameter();
         dtMim.ParameterName = "@dt_start";
         dtMim.SqlDbType = SqlDbType.DateTime;
         TimeSpan tss = new TimeSpan(0, pnlOscDiag.dtpStartData.Value.Hour - pnlOscDiag.dtpStartTime.Value.Hour, pnlOscDiag.dtpStartData.Value.Minute - pnlOscDiag.dtpStartTime.Value.Minute, pnlOscDiag.dtpStartData.Value.Second - pnlOscDiag.dtpStartTime.Value.Second);
         DateTime tim = pnlOscDiag.dtpStartData.Value - tss;
         dtMim.Value = tim;
         dtMim.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(dtMim);

         // 2. конечное время
         SqlParameter dtMax = new SqlParameter();
         dtMax.ParameterName = "@dt_end";
         dtMax.SqlDbType = SqlDbType.DateTime;
         tss = new TimeSpan(0, pnlOscDiag.dtpEndData.Value.Hour - pnlOscDiag.dtpEndTime.Value.Hour, pnlOscDiag.dtpEndData.Value.Minute - pnlOscDiag.dtpEndTime.Value.Minute, pnlOscDiag.dtpEndData.Value.Second - pnlOscDiag.dtpEndTime.Value.Second);
         tim = pnlOscDiag.dtpEndData.Value - tss;
         dtMax.Value = tim;
         dtMax.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(dtMax);

         // 5. тип записи
         SqlParameter ptypeRec = new SqlParameter();
         ptypeRec.ParameterName = "@type";
         ptypeRec.SqlDbType = SqlDbType.Int;

         ptypeRec.Value = TypeBlockData.TypeBlockData_OscBresler; // информация по осциллограммам Бреслер
         ptypeRec.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(ptypeRec);
         // 6. ид записи журнала
         SqlParameter pid = new SqlParameter();
         pid.ParameterName = "@id_record";
         pid.SqlDbType = SqlDbType.Int;
         pid.Value = 0;
         pid.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(pid);

         // заполнение DataSet
         DataSet aDS = new DataSet("ptk");
         SqlDataAdapter aSDA = new SqlDataAdapter();
         aSDA.SelectCommand = cmd;

         //aSDA.sq
         aSDA.Fill(aDS, "TbOscill");//TbAlarm

         asqlconnect.Close();

         //PrintDataSet( aDS );
         // извлекаем данные по осциллограммам
         dtO = aDS.Tables["TbOscill"];//TbAlarm
         for (int curRow = 0; curRow < dtO.Rows.Count; curRow++)
         {
            int i = dgvOscill.Rows.Add();   // номер строки
            dgvOscill["clmChBoxOsc", i].Value = false;
            dgvOscill["clmBlockNameOsc", i].Value = dtO.Rows[curRow]["BlockName"];
            dgvOscill["clmBlockIdOsc", i].Value = dtO.Rows[curRow]["BlockID"];
            dgvOscill["clmBlockTimeOsc", i].Value = dtO.Rows[curRow]["TimeBlock"];
            dgvOscill["clmCommentOsc", i].Value = dtO.Rows[curRow]["Comment"];
            dgvOscill["clmID", i].Value = dtO.Rows[curRow]["ID"];
         }
         aSDA.Dispose();
         aDS.Dispose();
      }

      private void dgvOscill_CellContentClick(object sender, DataGridViewCellEventArgs e)
      {
         byte[] arrO = null;
         string ifa;         // имя файла
         DataGridViewCell de;
         char[] sep = { ' ' };
         string[] sp;
         StringBuilder sb;
         if (e.ColumnIndex == 0)
         {
            dgvOscill[e.ColumnIndex, e.RowIndex].Value = (bool)dgvOscill[e.ColumnIndex, e.RowIndex].Value ? false : true;
            btnUnionOsc.Enabled = true;
            return;
         }
         else if (e.ColumnIndex != 5)
            return;

         btnUnionOsc.Enabled = false;
         // сбрасываем все флажки
         for (int i = 0; i < dtO.Rows.Count; i++)
            dgvOscill[0, i].Value = false;

         try
         {
            de = dgvOscill["clmID", e.RowIndex];
         }
         catch
         {
            MessageBox.Show("dgvOscill_CellContentClick - исключение");
            return;
         }
         int ide = (int)de.Value;

         // по ide найти запись в dto, извлечь блок с осциллограммой (диаграммой), записать в файл, запустить fastview
         // перечисляем записи в dto
         int curRow;

         for (curRow = 0; curRow < dtO.Rows.Count; curRow++)
         {
            if (ide == ((int)dtO.Rows[curRow]["ID"]))
            {
               arrO = (byte[])dtO.Rows[curRow]["Data"];
               break;
            }
         }
         // записываем массив байт в файл
         // формируем имя файла в зависимости от типа - диаграмма или осциллограмма
         #region старый код
         //ifa = (string)dtO.Rows[curRow]["BlockName"] + ".trd";

         //// удаляем пробелы
         //sp = ifa.Split(sep);
         //sb = new StringBuilder();
         //for (int i = 0; i < sp.Length; i++)
         //{
         //   sb.Append(sp[i]);
         //}         
         #endregion

         #region Осциллограмма Сириус - формирование имени файла
         ifa = (string)dtO.Rows[curRow]["BlockName"] + "_#_" + Convert.ToString(dtO.Rows[curRow]["BlockID"]) + "_#Tblock_" + Convert.ToString(dtO.Rows[curRow]["TimeBlock"]) + "_#TFC_" + Convert.ToString(dtO.Rows[curRow]["TimeFC"]);

         // удаляем пробелы
         sp = ifa.Split(sep);
         sb = new StringBuilder();
         for (int i = 0; i < sp.Length; i++)
         {
            sb.Append(sp[i]);
         }
         string sbb = sb.ToString();
         sb.Length = 0;
         for (int i = 0; i < sbb.Length; i++)
         {
            if (sbb[i] == '.' || sbb[i] == '/' || sbb[i] == '\\' || sbb[i] == '/' || sbb[i] == ':')
               sb.Append('_');
            else
               sb.Append(sbb[i]);
         }
         sb.Append(".brs");
         #endregion

         #region запись в файл, запуск OscView - Старый код
         //FileStream f = File.Create(Environment.CurrentDirectory.ToString() + "\\" + sb.ToString());
         //f.Write(arrO, 0, arrO.Length);
         //f.Close();
         //// запускаем fastview
         //Process prc = new Process();
         //prc.StartInfo.FileName = Environment.CurrentDirectory.ToString() + "\\OscView\\OscView.exe  ";
         //prc.StartInfo.Arguments = sb.ToString();
         //prc.Start(); 
         #endregion

         #region запись в файл, запуск OscView
         FileStream f = null;
         try
         {
            f = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + sb.ToString());
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.ToString() + "\nФайл : " + AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + sb.ToString(), this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
         }

         f.Write(arrO, 0, arrO.Length);
         f.Close();
         // запускаем fastview

         Process prc = new Process();
         prc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\OscView\\OscView.exe  ";
         prc.StartInfo.Arguments = "\"" + AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + sb.ToString() + "\"";
         prc.Start();
         #endregion
      }
      #endregion
      #endregion
      #endregion

      #region Формирование нижних (доп) панелей
      void CreateTabPanel()
      {
         if (arDopPanel == null)
            arDopPanel = new ArrayList();

         #region Текущая - контроль
         pnlCurrent = new CurrentPanelControl();
         SplitContMain.Panel2.Controls.Add(pnlCurrent);
         pnlCurrent.Dock = DockStyle.Fill;
         arDopPanel.Add(pnlCurrent);

         DinamicControl rr;
         /* 
          * создадим динамический элемент для его размещения на панели pnl
         */
         int xx = pnlCurrent.PnlImgDev.Width;
         int yy = pnlCurrent.PnlImgDev.Height;

         ArrayList arrFE = new ArrayList();
         CommonUtils.CommonUtils.CreateDevImg4Panel(out rr, parent.KB, IFC, IIDDev, pnlCurrent.PnlImgDev, ref arrFE); //, xed, ControlSizeVariant.SizeofControl 
         #endregion

         #region Осциллограммы и диаграммы
         pnlOscDiag = new OscDiagPanelControl();
         SplitContMain.Panel2.Controls.Add(pnlOscDiag);
         pnlOscDiag.btnReNew.Click += new EventHandler(btnReNewOscDg_Click);
         pnlOscDiag.Dock = DockStyle.Fill;
         arDopPanel.Add(pnlOscDiag);
         #endregion

         foreach (UserControl p in arDopPanel)
            p.Visible = false;
      }
      #endregion

   }
}
