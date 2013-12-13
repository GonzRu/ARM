/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Форма для работы с блоком Дуга.                                                           
 *                                                                             
 *	Файл                     : frmDuga.cs                                         
 *	Тип конечного файла      : 
 *	версия ПО для разработки : С#, Framework 2.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : xx.04.2008                                       
 *	Дата (v1.0)              :                                                  
 *******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/

using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Calculator;
using System.Collections;
using System.IO;
using LabelTextbox;
using System.Linq;
using System.Xml.Linq;
using InterfaceLibrary;
using HMI_MT_Settings;
using DataBaseLib;
using OscillogramsLib;

namespace DeviceFormLib
{
    public partial class frmDuga : Form, IDeviceForm
	{
        /// <summary>
        /// Текущий TabPage для подписки/отписки тегов
        /// </summary>
        TabPage tpCurrent;
 
        int iFC;            // номер ФК целочисленный
        string strFC;       // номер ФК строка
        int iIDDev;         // номер устройства целочисленный
        string strIDDev;    // номер устройства строка
        int inumLoc;         // номер ячейки целочисленный

        /// <summary>
        /// имя файла с описание формы
        /// </summary>
        string fileFrmTagsDescript;
		string nfXMLConfigFC; // имя файла с описанием ЩАСУ
        // массив дополнительных панелей
        ArrayList arDopPanel;

        // нижние панели
        ConfigPanelControl pnlConfig;
        SrabatPanelControl pnlSrabat;
        //CurrentPanelControl pnlCurrent;
        OscDiagPanelControl pnlOscDiag;

        ArrayList arrAvarSign = new ArrayList();
        ArrayList arrCurSign = new ArrayList();
        ArrayList arrSystemSign = new ArrayList();
        ArrayList arrStoreSign = new ArrayList();
        ArrayList arrConfigSign = new ArrayList();

        SortedList se = new SortedList();
        SortedList sl_tpnameUst = new SortedList();
        StringBuilder sbse = new StringBuilder();

        DataTable dtO;  // таблица с осциллограммами
        DataTable dtG;  // таблица с диаграммами
        DataTable dtA;  // таблица с авариями
        DataTable dtU;  // таблица с уставками
        OscDiagViewer oscdg;
        /// <summary>
        /// флаг - покаывать ли сообщение об отсутсвии архивных записей 
        /// или нет
        /// </summary>
        bool IsMesView = false;

        /// <summary>
        /// для хранения имен файлов в случае для объединения осциллограмм
        /// </summary>
        ArrayList asb = new ArrayList();

        SortedList slFLP = new SortedList();	// для хранения инф о FlowLayoutPanel
        /// <summary>
        /// список коэф трансф. из файла PrgDevCFG.cdp 
        /// для данного устройства
        /// </summary>
        private SortedList<string, string> slKoefRatioValue = new SortedList<string, string>();
 
        #region конструктор
        public frmDuga(int iFC, int iIDDev, /*int inumLoc,*/ string fXML, string ftagsxmldescript)
		{
		    InitializeComponent();
            try
            {
                //InitInterfaceElementsClick();

                //parent = linkMainForm;
                this.iFC = iFC;                 // номер ФК целочисленный
                strFC = iFC.ToString();         // номер ФК строка
                this.iIDDev = iIDDev;           // номер устройства целочисленный
                strIDDev = iIDDev.ToString();   // номер устройства строка
                //this.inumLoc = inumLoc;         // номер ячейки целочисленный

                fileFrmTagsDescript = ftagsxmldescript;

                //Text += " ( " + strIDDev + " ) - яч. № " + strnumLoc;

                tabControl1.Controls.Add( new DataBaseFilesLibrary.TabPageDbFile( (uint)( 256 * iFC + iIDDev ) ) );
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

		}
        /// <summary>
        /// загрузка формы
        /// </summary>
        private void frmDuga_Load( object sender, EventArgs e )
        {
			  ControlCollection cc;
			  cc = ( ControlCollection ) this.Controls;
			  for( int i = 0 ; i < cc.Count ; i++ )
			  {
				  if( cc[i] is FlowLayoutPanel )
				  {
					  FlowLayoutPanel flp = ( FlowLayoutPanel ) cc[i];
					  slFLP[flp.Name] = flp;
				  }
				  else
				  {
					  TestCCforFLP( cc[i] );
				  }
			  }

              // создаем нижние панели
              CreateTabPanel();

              InitInterfaceElementsClick();
        }
        /// <summary>
        /// в этой функции связываются клики на
        /// элементах интерфейса с кодом их обработки
        /// </summary>
        public void InitInterfaceElementsClick()
        {
            try
            {
                tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);
                                          
                tabPage1.Enter += new EventHandler(tabPage1_Enter);
                tbpAvar.Enter += new EventHandler(tbpAvar_Enter);
                tabStore.Enter += new EventHandler(tabStore_Enter);
                tbpConfUst.Enter += new EventHandler(tbcConfig_Enter);

                this.FormClosing += new FormClosingEventHandler(frmDuga_FormClosing);
                
                tabPage5.Enter += new EventHandler(tabPage5_Enter);

                lstvAvar.ItemActivate += new EventHandler(lstvAvar_ItemActivate);
                pnlSrabat.btnReNew.Click += new EventHandler(btnReNew_Click);
                //btnReNew.Click += new EventHandler(btnReNew_Click);
                //btnReadUstFC.Click += new EventHandler(btnReadUstFC_Click);
                lstvConfig.ItemActivate += new EventHandler(lstvConfig_ItemActivate);
                pnlConfig.btnReadUstFC.Click += new EventHandler(btnReadUstFC_Click);
                ////pnlConfig.btnWriteUst.Click += new EventHandler(btnWriteConfig_Click);
                ////pnlConfig.btnResetValues.Click += new EventHandler(btnResetValues_Click);
                pnlConfig.btnReNewUstBD.Click += new EventHandler(btnReNewUstBD_Click);

                //dtpStartData.ValueChanged += new EventHandler(dtpStartData_ValueChanged);

                dgvOscill.CellContentClick += new DataGridViewCellEventHandler(dgvOscill_CellContentClick);
                dgvDiag.CellContentClick += new DataGridViewCellEventHandler(dgvDiag_CellContentClick);
                pnlOscDiag.btnReNew.Click += new EventHandler(btnReNewOscDg_Click);

                btnReadMaxMeterFC.Click += new EventHandler(btnReadMaxMeterFC_Click);
                btnResetMaxMeter.Click +=new EventHandler(btnResetMaxMeter_Click);
                btnReadStoreFC.Click +=new EventHandler(btnReadStoreFC_Click);

            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        void frmDuga_FormClosing(object sender, FormClosingEventArgs e)
        {
            // отписываемся от тегов
            HMI_MT_Settings.HMI_Settings.HMIControlsTagReNew(tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe);
        }

        /// <summary>
        /// реализация метода интерфейса
        /// IDeviceForm
        /// </summary>
        public void CreateDeviceForm()
        { }

        /// <summary>
        /// активировать определенную вкладку
        /// при открытии формы
        /// </summary>
        /// <param name="typetabpage"></param>
        public void ActivateTabPage(string typetabpage)
        {
            try
            {
                switch (typetabpage)
                {
                    case "Авария":
                        tabControl1.SelectedTab = tbpAvar;
                        break;
                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        #endregion

        #region Формирование нижних (доп) панелей
        void CreateTabPanel()
        {
            TimeSpan ts;
            try
            {
                if (arDopPanel == null)
                    arDopPanel = new ArrayList();

                #region Уставки
                pnlConfig = new ConfigPanelControl();
                splitContainer2.Panel2.Controls.Add(pnlConfig);
                ////формируем панель для уставок
                pnlConfig.Dock = DockStyle.Fill;
                pnlConfig.Visible = false;
                ////arDopPanel.Add(pnlConfig);
                #endregion

                #region Аварии-срабатывание
                pnlSrabat = new SrabatPanelControl();
                splitContainer51.Panel1.Controls.Add(pnlSrabat);
                pnlSrabat.Dock = DockStyle.Fill;
                pnlSrabat.Visible = true;
                //pnlSrabat.btnReNew.Click += new EventHandler(btnReNew_Click);
                //arDopPanel.Add(pnlSrabat);
                //lstvAvar.ItemActivate += new EventHandler( lstvAvar_ItemActivate );
                #endregion

                #region Осциллограммы и диаграммы
                pnlOscDiag = new OscDiagPanelControl();
                splitContainer24.Panel2.Controls.Add(pnlOscDiag);                
                pnlOscDiag.Dock = DockStyle.Fill;
                ////arDopPanel.Add(pnlOscDiag);
                #endregion

                #region Журнал событий блока
                //pnlLogDev = new LogDevPanelControl();
                //splitContainer1.Panel2.Controls.Add(pnlLogDev);
                //pnlLogDev.btnReNew.Click += new EventHandler(btnReNewLogDev_Click);
                //pnlLogDev.Dock = DockStyle.Fill;
                //arDopPanel.Add(pnlLogDev);
                #endregion

                #region События блока
                //pnlBottomEV = new PanelBottomEventBlock();
                //splitContainer1.Panel2.Controls.Add(pnlBottomEV);
                ////формируем панель для выборки диапазона событий блока
                //pnlBottomEV.Dock = DockStyle.Fill;
                //pnlBottomEV.InitPanel(IFC, IIDDev, lstvEventBlock);

                //arDopPanel.Add(pnlBottomEV);
                #endregion

                foreach (UserControl p in arDopPanel)
                    p.Visible = false;

                #region устанавливаем пикеры для вывода осциллограмм и диаграмм за последние сутки
                //pnlOscDiag.dtpEndData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                //pnlOscDiag.dtpEndTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                //pnlOscDiag.dtpStartData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                //ts = new TimeSpan(1, 0, 0, 0);
                //pnlOscDiag.dtpStartData.Value = pnlOscDiag.dtpStartData.Value - ts;
                //pnlOscDiag.dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                #endregion

                #region устанавливаем пикеры для вывода последнего журнала устройства из БД
                //pnlLogDev.dtpEndData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                //pnlLogDev.dtpEndTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                //pnlLogDev.dtpStartData.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                //ts = new TimeSpan(360, 0, 0, 0);
                //pnlLogDev.dtpStartData.Value = pnlLogDev.dtpStartData.Value - ts;
                //pnlLogDev.dtpStartTime.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                #endregion

                #region устанавливаем пикеры для вывода аварийной информации за последние сутки
                pnlSrabat.dtpEndDateAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                pnlSrabat.dtpEndTimeAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                pnlSrabat.dtpStartDateAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                ts = new TimeSpan(3, 0, 0, 0);
                pnlSrabat.dtpStartDateAvar.Value = pnlSrabat.dtpStartDateAvar.Value - ts;
                pnlSrabat.dtpStartTimeAvar.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                #endregion

                #region устанавливаем пикеры для изменения уставок за последние сутки
                pnlConfig.dtpEndDateConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                pnlConfig.dtpEndTimeConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, 999);
                pnlConfig.dtpStartDateConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

                ts = new TimeSpan(1, 0, 0, 0);
                pnlConfig.dtpStartDateConfig.Value = pnlConfig.dtpStartDateConfig.Value - ts;
                pnlConfig.dtpStartTimeConfig.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                #endregion
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        #endregion


		  /// <summary>
		  /// создание массива ArrayList с описанием переменных по содержимому файла XML
		  /// </summary>
		  /// <param name="arrVar"> массив  ArrayList
		  ///фигуры</param>
		  /// <param name="nameFile">имя файла XML
		  ///фигуры</param>
        private void CreateArrayList(ArrayList arrVar, string name_arrVar)
        {
            SortedList sl = new SortedList();
            ArrayList alVal = new ArrayList();
            XDocument xd;

            // чтение XML
            xd = name_arrVar == "arrStatusFCCommand" ? XDocument.Load(nfXMLConfigFC) : XDocument.Load(fileFrmTagsDescript);

            //вывод отладочный в файл
            FileStream fs = File.Create("bmrz.xio");
            StreamWriter sw = new StreamWriter(fs);

            string bitmask = String.Empty;
            StringBuilder frmtgs = new StringBuilder();

            XElement xegr = (from p in xd.Element("MT").Element("BMRZ").Element("frame").Elements()
                             where p.Name == name_arrVar
                             select p).Single();

            try
            {
                IEnumerable<XElement> xefs = xegr.Elements("formula");
                foreach (XElement xef in xefs)
                {
                    // формируем элементы формулы
                    sl["formula"] = xef.Attribute("express").Value;
                    sl["caption"] = xef.Attribute("Caption").Value;
                    sl["dim"] = xef.Attribute("Dim").Value;
                    sl["TypeOfTag"] = xef.Attribute("TypeOfTag").Value;
                    sl["TypeOfPanel"] = xef.Attribute("TypeOfPanel").Value;

                    if (!String.IsNullOrWhiteSpace((string)xef.Attribute("bitmask")))
                        bitmask = xef.Attribute("bitmask").Value;

                    TypeOfTag ToT = TypeOfTag.NaN;
                    string ToP = "";

                    sw.WriteLine(sl["caption"]);
                    sw.Flush();

                    switch ((string)sl["TypeOfTag"])
                    {
                        case "Analog":
                            ToT = TypeOfTag.Analog;
                            break;
                        case "Discret":
                            ToT = TypeOfTag.Discret;
                            break;
                        case "Combo":
                            ToT = TypeOfTag.Combo;
                            break;
                        case "No":
                            ToT = TypeOfTag.NaN;
                            break;
                        default:
                            MessageBox.Show("Нет такого типа сигнала");
                            break;
                    }
                    ToP = (string)sl["TypeOfPanel"];

                    // читаем теги
                    alVal.Clear();
                    IEnumerable<XElement> xfts = xef.Elements("value");
                    StringBuilder sbtag = new StringBuilder();

                    foreach (XElement xft in xfts)
                    {
                        sbtag.Clear();
                        sbtag.Append(xft.Attribute("tag").Value);
                        if (sbtag.ToString() == "external")
                        {
                            sbtag.Clear();
                            if (slKoefRatioValue.ContainsKey(xft.Attribute("id").Value))
                                sbtag.Append(slKoefRatioValue[xft.Attribute("id").Value]);
                            else
                                sbtag.Append(xft.Attribute("DefaultValue").Value);
                        }
                        alVal.Add(sbtag.ToString());
                    }

                    if (bitmask == "" || bitmask == null)	// bitmask - для работы с BCD-значениями уставок по маске
                    {
                        frmtgs.Length = 0;
                        for (int i = 0; i < alVal.Count; i++)
                        {
                            frmtgs.Append(i.ToString() + "(" + strFC + "." + strIDDev + (string)alVal[i] + ")");
                        }
                        arrVar.Add(new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, frmtgs.ToString(), sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP));

                    }
                    else
                    {
                        if (alVal.Count == 2)
                            arrVar.Add(new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + strFC + "." + strIDDev + (string)alVal[0] + ")1(" + strFC + "." + strIDDev + (string)alVal[1] + ")", sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP, bitmask));
                        else
                            arrVar.Add(new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + strFC + "." + strIDDev + (string)alVal[0] + ")", sl["formula"].ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP, bitmask));
                    }
                }

                xefs = xegr.Elements("simple_eval");
                foreach (XElement xef in xefs)
                {
                    sbse.Clear();
                    sbse.Append(xef.Attribute("name").Value);
                    se[sbse.ToString()] = xef.Element("value").Attribute("tag").Value;
                }

                xefs = xegr.Elements("name_tabpage_ust");
                foreach (XElement xef in xefs)
                {   // запоминем названия вкладок в уставках и конфигурации
                    sbse.Length = 0;
                    sbse.Append(xef.Attribute("name").Value);
                    for (int i = 0; i < tbcConfig.Controls.Count; i++)
                    {
                        if (tbcConfig.Controls[i] is TabPage && tbcConfig.Controls[i].Name == sbse.ToString())
                        {
                            tbcConfig.Controls[i].Text = xef.Element("value").Attribute("tag").Value;
                            sl_tpnameUst[sbse.ToString()] = tbcConfig.Controls[i];
                        }
                    }
                }
            }
            catch (XmlException ee)
            {
                System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " : frmBMRZ : CreateArrayList() : ОШИБКА :" + ee.Message);
                sw.Close();
                fs.Close();
            }
            sw.Close();
            fs.Close();

            //if (HMI_Settings.ClientDFE != null)
            //{ 
            //    switch(name_arrVar)
            //    {
            //      case "arrAvarSign":
            //            break;
            //      case "arrConfigSign":
            //            break;
            //              case "arrStatusDevCommand":
            //                  break;
            //              case "arrStatusFCCommand":
            //                  break;															 
            //      //case "arrStoreSign":
            //            //foreach (FormulaEval fe in arrVar)
            //            //   if (fe.addrVar < 60000)
            //            //      parent.ClientDFE.AddArrTags(this.Text, fe);
            //            //break;                        
            //      default:

            // подписываемся на обновление тегов с DataServer
            //taglist = new List<ITag>();

            //foreach (FormulaEvalNDS fe in arrVar)
            //    taglist.Add(fe.LinkVariableNewDS);

            //HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags(taglist);
            //      break;
            //    }                  
            //}
        }

		private void TestCCforFLP( Control cc )
		{
			for( int i = 0 ; i < cc.Controls.Count ; i++ )
			{
				if( cc.Controls[i] is FlowLayoutPanel )
				{
					FlowLayoutPanel flp = ( FlowLayoutPanel ) cc.Controls[i];
					slFLP[flp.Name] = flp;
				}
				else
				{
					TestCCforFLP( cc.Controls[i] );
				}
			}
		}

        #region вход на Tab с текущей информацией
        /*=======================================================================*
        *   private void tabPage1_Enter( object sender, EventArgs e )
        *       вход на Tab с текущей информацией
        *=======================================================================*/
        private void tabPage1_Enter( object sender, EventArgs e )
        {
            tabSystem_Enter( sender, e );
            //-------------------------------------------------------------------
            //готовим инф. для отображения текущих значений аналоговых и дискретных сигналов
            // текущие аналоговые сигналы (рег. 0033 ...) с учетом коэффициента трансформации
            if( arrCurSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabPage1);
                return;
            }

    		 CreateArrayList( arrCurSign, "arrCurSign" );

           // размещаем динамически на форме
            for( int i = 0; i < arrCurSign.Count; i++ )
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrCurSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch (ev.ToT)
                {
                    case TypeOfTag.String:
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox(ev);
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                        bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                        usTB.txtLabelText.DataBindings.Add(bnd);
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;
                        usTB.Dim_Text = ev.Dim;
                        break;
                    case TypeOfTag.Discret:
                        chBV = new CheckBoxVar(ev);
                        chBV.checkBox1.Text = "";
                        chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        chBV.AutoSize = true;

                        Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                        bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                        chBV.checkBox1.DataBindings.Add(bndCB);
                        ev.LinkVariableNewDS.BindindTag = bndCB;
                        chBV.checkBox1.Text = ev.CaptionFE;
                        break;
                    case TypeOfTag.NaN:
                        break;
                    default:
                        MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
			  // открываем доп панели на pnlCurrent
			  if (CurrentControlProgUst.Controls.Count != 0)
				  gbControlProgUst.Visible = true;
			  if (CurrentDirection_P.Controls.Count != 0)
				  gbDirection_P.Visible = true;

              tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabPage1);
        }
        private void tabSystem_Enter( object sender, EventArgs e )
        {
            if ( arrSystemSign.Count != 0 )
                return;
            //-------------------------------------------------------------------
            CreateArrayList( arrSystemSign, "arrSystemSign" );

            // размещаем динамически на форме
            for ( int i = 0; i < arrSystemSign.Count; i++ )
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrSystemSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch ( ev.ToT )
                {
                    case TypeOfTag.String:
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox( ev );
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding( "Text", ev.LinkVariableNewDS, "ValueAsString", true );
                        bnd.Format += new ConvertEventHandler( usTB.bnd_Format );
                        usTB.txtLabelText.DataBindings.Add( bnd );
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;
                        usTB.Dim_Text = ev.Dim;
                        break;
                    case TypeOfTag.Discret:
                        chBV = new CheckBoxVar( ev );
                        chBV.checkBox1.Text = "";
                        chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        chBV.AutoSize = true;

                        Binding bndCB = new Binding( "Checked", ev.LinkVariableNewDS, "ValueAsString", true );
                        bndCB.Format += new ConvertEventHandler( chBV.bnd_Format );
                        chBV.checkBox1.DataBindings.Add( bndCB );
                        ev.LinkVariableNewDS.BindindTag = bndCB;
                        chBV.checkBox1.Text = ev.CaptionFE;
                        break;
                    case TypeOfTag.NaN:
                        break;
                    default:
                        MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        break;
                }
            }
        }
        #endregion

        #region вход на Tab с аварийной информацией
        /*=======================================================================*
        *   private void tbpAvar_Enter( object sender, EventArgs e )
        *       вход на Tab с аварийной информацией
        *=======================================================================*/
        private void tbpAvar_Enter( object sender, EventArgs e )
        {
            tbpAvar.Height = tabControl1.Height;
            splitContainer1.Height = tabControl1.Height;
            tabControl2.Height = splitContainer1.Height;
            tabPage2.Height = tabControl2.Height;
            tabPage7.Height = tabControl2.Height;

            tbpAvar.Width = tabControl1.Width;
            splitContainer1.Width = tabControl1.Width;
            tabControl2.Width = splitContainer1.Width;
            tabPage2.Width = tabControl2.Width;
            tabPage7.Width = tabControl2.Width;

            lstvAvar.Items.Clear();
            
            AvarBD();
            //-------------------------------------------------------------------
            //готовим инф. для отображения аналоговых и дискретных сигналов
            
            if( arrAvarSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbpAvar);
                return;
            }


            CreateArrayList( arrAvarSign, "arrAvarSign" );
            
            // размещаем динамически на форме
            for( int i = 0; i < arrAvarSign.Count; i++ ) 
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrAvarSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch (ev.ToT)
                {
                    case TypeOfTag.String:
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox(ev);
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                        bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                        usTB.txtLabelText.DataBindings.Add(bnd);
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;
                        usTB.Dim_Text = ev.Dim;
                        break;
                    case TypeOfTag.Discret:
                        chBV = new CheckBoxVar(ev);
                        chBV.checkBox1.Text = "";
                        chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        chBV.AutoSize = true;

                        Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                        bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                        chBV.checkBox1.DataBindings.Add(bndCB);
                        ev.LinkVariableNewDS.BindindTag = bndCB;
                        chBV.checkBox1.Text = ev.CaptionFE;
                        break;
                    case TypeOfTag.NaN:
                        break;
                    default:
                        MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }

            tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbpAvar);                       
        }
        /*=======================================================================*
        *   private void lstvAvar_ItemActivate( object sender, EventArgs e )
        *       вывод информации при выборе конкретной аварии
        *=======================================================================*/
        private void lstvAvar_ItemActivate( object sender, EventArgs e )
        {
            //if( lstvAvar.SelectedItems.Count > 0 )
            //{
            //    // получение строк соединения и поставщика данных из файла *.config
            //    string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            //    SqlConnection asqlconnect = new SqlConnection(cnStr);
            //         try
            //         {
            //             asqlconnect.Open();
            //         } catch( SqlException ex )
            //         {
            //             string errorMes = "";
            //             // интеграция всех возвращаемых ошибок
            //             foreach( SqlError connectError in ex.Errors )
            //                 errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ")" + Environment.NewLine;
            //             //parent.WriteEventToLog( 21, "Нет связи с БД (lstvAvar_ItemActivate): " + errorMes, this.Name, false, true, false ); // событие нет связи с БД

            //             asqlconnect.Close();
            //             return;
            //         } catch( Exception ex )
            //         {
            //             MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );
            //             asqlconnect.Close();
            //             return;
            //         }
            //    // формирование данных для вызова хранимой процедуры
            //    SqlCommand cmd = new SqlCommand( "ShowDataLog", asqlconnect );
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    // входные параметры
            //    // 1. ip FC
            //    SqlParameter pipFC = new SqlParameter();
            //    pipFC.ParameterName = "@IP";
            //    pipFC.SqlDbType = SqlDbType.BigInt;
            //    pipFC.Value = 0;
            //    pipFC.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( pipFC );
            //    // 2. id устройства
            //    SqlParameter pidBlock = new SqlParameter();
            //    pidBlock.ParameterName = "@id";
            //    pidBlock.SqlDbType = SqlDbType.Int;
            //         pidBlock.Value = 0;
            //    pidBlock.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( pidBlock );
            //    // 3. время старт
            //    SqlParameter ptimeStart = new SqlParameter();
            //    ptimeStart.ParameterName = "@dt_start";
            //    ptimeStart.SqlDbType = SqlDbType.DateTime;
            //    ptimeStart.Value = DateTime.Now;
            //    ptimeStart.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( ptimeStart );
            //    // 4. время конец
            //    SqlParameter ptimeFin = new SqlParameter();
            //    ptimeFin.ParameterName = "@dt_end";
            //    ptimeFin.SqlDbType = SqlDbType.DateTime;
            //    ptimeFin.Value = DateTime.Now;
            //    ptimeFin.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( ptimeFin );
            //    // 5. тип записи
            //    SqlParameter ptypeRec = new SqlParameter();
            //    ptypeRec.ParameterName = "@type";
            //    ptypeRec.SqlDbType = SqlDbType.Int;
            //    ptypeRec.Value = 0;
            //    ptypeRec.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( ptypeRec );
            //    // 6. ид записи журнала
            //    SqlParameter pid = new SqlParameter();
            //    pid.ParameterName = "@id_record";
            //    pid.SqlDbType = SqlDbType.Int;
            //    pid.Value = lstvAvar.SelectedItems[0].Tag;
            //    pid.Direction = ParameterDirection.Input;
            //    cmd.Parameters.Add( pid );

            //    // заполнение DataSet
            //    DataSet aDS = new DataSet( "ptk" );
            //    SqlDataAdapter aSDA = new SqlDataAdapter();
            //    aSDA.SelectCommand = cmd;
                
            //    //aSDA.sq
            //    aSDA.Fill( aDS);//, "DataLog" 

            //    asqlconnect.Close();

            //         PrintDataSet( aDS );
            //    // извлекаем данные по аварии
            //    DataTable dt = aDS.Tables[0];
            //    byte[] adata = ( byte[] ) dt.Rows[0]["Data"];
                
            //    // вызываем процедуру разбора пакета с аварийной информацией из базы
            //    ParseBDPacket( adata, 60100, iIDDev );//10280
            //         aSDA.Dispose();
            //}
        }
        /// <summary>
        /// завершение запроса данных 
        /// по архивной аварии
        /// </summary>
        /// <param name="req"></param>
        public void reqAvar_OnReqExecuted(IRequestData req)
        {
            try
            {
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        #endregion

        #region контрольная печать DataSet
        static void PrintDataSet( DataSet ds )
        {
            // метод выполняет цикл по всем DataTable данного DataSet
            Console.WriteLine( "Таблицы в DataSet '{0}'. \n ", ds.DataSetName );
            foreach( DataTable dt in ds.Tables )
            {
                Console.WriteLine( "Таблица '{0}'. \n ", dt.TableName );
                // вывод имен столбцов
                for( int curCol = 0; curCol < dt.Columns.Count; curCol++ )
                    Console.Write( dt.Columns[curCol].ColumnName.Trim() + "\t" );
                Console.WriteLine( "\n-----------------------------------------------" );

                // вывод DataTable
                for( int curRow = 0; curRow < dt.Rows.Count; curRow++ )
                {
                    for( int curCol = 0; curCol < dt.Columns.Count; curCol++ )
                        Console.Write( dt.Rows[curRow][curCol].ToString() + "\t" );
                    Console.WriteLine();
                }
            }
        }
        #endregion

        #region вкладка аварийная информация
        private void AvarBD()
        {
            try
            {
                DataBaseReq dbs = new DataBaseReq(HMI_Settings.ProviderPtkSql, "ShowDataLog2");

                var uniDev = (uint)(iFC * 256 + iIDDev);

                // входные параметры
                // 1. ip FC
                dbs.AddCMDParams(new DataBaseParameter("@IP", ParameterDirection.Input, SqlDbType.BigInt, 0));
                // 2. id устройства
                dbs.AddCMDParams(new DataBaseParameter("@id", ParameterDirection.Input, SqlDbType.Int, uniDev));
                // 3. начальное время
                TimeSpan tss = new TimeSpan(0, pnlSrabat.dtpStartDateAvar.Value.Hour - pnlSrabat.dtpStartTimeAvar.Value.Hour, pnlSrabat.dtpStartDateAvar.Value.Minute - pnlSrabat.dtpStartTimeAvar.Value.Minute, pnlSrabat.dtpStartDateAvar.Value.Second - pnlSrabat.dtpStartTimeAvar.Value.Second);
                DateTime tim = pnlSrabat.dtpStartDateAvar.Value - tss;
                dbs.AddCMDParams(new DataBaseParameter("@dt_start", ParameterDirection.Input, SqlDbType.DateTime, tim));
                // 2. конечное время
                tss = new TimeSpan(0, pnlSrabat.dtpEndDateAvar.Value.Hour - pnlSrabat.dtpEndTimeAvar.Value.Hour, pnlSrabat.dtpEndDateAvar.Value.Minute - pnlSrabat.dtpEndTimeAvar.Value.Minute, pnlSrabat.dtpEndDateAvar.Value.Second - pnlSrabat.dtpEndTimeAvar.Value.Second);
                tim = pnlSrabat.dtpEndDateAvar.Value - tss;
                dbs.AddCMDParams(new DataBaseParameter("@dt_end", ParameterDirection.Input, SqlDbType.DateTime, tim));
                // 5. тип записи
                // информация по авариям
                int tbd = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Srabat, uniDev);

                dbs.AddCMDParams(new DataBaseParameter("@type", ParameterDirection.Input, SqlDbType.Int, tbd));
                // 6. ид записи журнала
                dbs.AddCMDParams(new DataBaseParameter("@id_record", ParameterDirection.Input, SqlDbType.Int, 0));

                //dbs.DoStoredProcedure();

                // извлекаем данные по авариям
                dtA = dbs.GetTableAsResultCMD();

                if (dtA.Rows.Count == 0 && IsMesView)
                {
                    MessageBox.Show("Архивных данных по авариям для этого устройства нет.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    IsMesView = false;
                }

                // заполняем ListView
                lstvAvar.Items.Clear();
                for (int curRow = 0; curRow < dtA.Rows.Count; curRow++)
                {
                    DateTime t = (DateTime)dtA.Rows[curRow]["TimeBlock"];
                    ListViewItem li = new ListViewItem(CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t));
                    li.SubItems.Add(dtA.Rows[curRow]["Arg1"].ToString());
                    li.Tag = dtA.Rows[curRow]["ID"];
                    lstvAvar.Items.Add(li);
                }

            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        private void btnReNew_Click(object sender, EventArgs e)
        {
            AvarBD();
        }
        #endregion
     
        #region вывод пакета pack в hex-виде в файд fn
      private void PrintHexDump( string fn , byte[] pack)
        {
            // выведем в файл - текущий каталог
            FileStream fs = new FileStream( fn, FileMode.Append );
            StreamWriter sw = new StreamWriter( fs );

            sw.Write( Environment.NewLine ); 
            sw.Write( "*****************************" ); sw.Write( Environment.NewLine );
            sw.Write( DateTime.Now.ToString() );
            sw.Write( Environment.NewLine);
            sw.Write( "*****************************" ); sw.Write( Environment.NewLine );
            int ii = 0;
            for( ushort i = 0; i < pack.Length; i++ )
            {
                // начинаем строку счетчиком адреса
                sw.Write( ii.ToString( "x" ) );
                sw.Write( "    " );
                for( int jj = 0; jj < 2; jj++ )
                {
                    for( int j = 0; j < 4; j++ )
                    {
                        try
                        {
                            sw.Write( pack[ii].ToString( "x" ) + " " );
                            ii++;
                        }
                        catch
                        {
									sw.Close();
									fs.Close();
                            return;
                        }
                    }
                    sw.Write( "    " );
                }
                sw.Write( Environment.NewLine ); sw.Write( Environment.NewLine );
            }
				sw.Close();
				fs.Close();
        }
        #endregion

        #region вход на вкладку накопительной информации
        private void tabStore_Enter( object sender, EventArgs e )
        {
            if( arrStoreSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabStore);
                return;
            }

            CreateArrayList(arrStoreSign, "arrStoreSign");
           //--------------------
            
            // размещаем динамически на форме
            for( int i = 0; i < arrStoreSign.Count; i++ )
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrStoreSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch (ev.ToT)
                {
                    case TypeOfTag.String:
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox(ev);
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                        bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                        usTB.txtLabelText.DataBindings.Add(bnd);
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;
                        usTB.Dim_Text = ev.Dim;
                        break;
                    case TypeOfTag.Discret:
                        chBV = new CheckBoxVar(ev);
                        chBV.checkBox1.Text = "";
                        chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        chBV.AutoSize = true;

                        Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                        bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                        chBV.checkBox1.DataBindings.Add(bndCB);
                        ev.LinkVariableNewDS.BindindTag = bndCB;
                        chBV.checkBox1.Text = ev.CaptionFE;
                        break;
                    case TypeOfTag.NaN:
                        break;
                    default:
                        MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabStore);
        }
        //private void btnReadStore_Click(object sender, EventArgs e)
        //{
        //    //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "IMC", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
        //    //       parent.WriteEventToLog( 35, "Команда \"IMC\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );
        //    //   // документирование действия пользователя
        //    //   parent.WriteEventToLog( 8, iIDDev.ToString(), this.Name, true, true, false );//"выдана команда IMC - чтение накопительной."
        //    //   //b_62132.FirstValue();
        //}

        private void btnReadStoreFC_Click(object sender, EventArgs e)
        {
            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            int uniDev = iFC * 256 + iIDDev;
            int numdevfc = uniDev;

            CommonUtils.CommonUtils.WriteEventToLog(8, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "IMC", new byte[] { }, this);
        }

        private void btnResetStore_Click(object sender, EventArgs e)
        {
            //if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b06_Reset_info, parent.UserRight ) )
            //    return;

            //DialogResult dr = MessageBox.Show( "Сбросить накопительную информацию блока?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
            //if( dr == DialogResult.Yes )
            //{
            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "CCD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //        parent.WriteEventToLog( 35, "Команда \"CCD\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );

            //      // документирование действия пользователя
            //    parent.WriteEventToLog( 9, iIDDev.ToString(), this.Name, true, true, false );//"выдана команда CCD - сброс накопительной."
            //}
        }

        private void btnResetStore_Click_1(object sender, EventArgs e)
        {

            if (CommonUtils.CommonUtils.IsUserActionBan(CommonUtils.CommonUtils.UserActionType.b06_Reset_info, HMI_MT_Settings.HMI_Settings.UserRight))
                return;

            DialogResult dr = MessageBox.Show("Сбросить накопительную информацию блока?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            int uniDev = iFC * 256 + iIDDev;
            int numdevfc = uniDev;

            CommonUtils.CommonUtils.WriteEventToLog(35, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "CCD", new byte[] { }, this);
        }

        private void btnReadMaxMeterFC_Click(object sender, EventArgs e)
        {
            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            int uniDev = iFC * 256 + iIDDev;
            int numdevfc = uniDev;

            CommonUtils.CommonUtils.WriteEventToLog(10, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "IMD", new byte[] { }, this);
        }

        //private void btnReadMaxMeter_Click(object sender, EventArgs e)
        //{
        //    //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "IMD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
        //    //       parent.WriteEventToLog( 35, "Команда \"IMD\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );
        //    //     // документирование действия пользователя
        //    //     parent.WriteEventToLog( 10, iIDDev.ToString(), this.Name, true, true, false );//"выдана команда IMD - чтение максметра."

        //}

        private void cbPeriodReadStore_CheckedChanged(object sender, EventArgs e)
        {
            //if( cbPeriodReadStore.Checked )
            //{
            //    lblIntervalReadStore1.Enabled = true;
            //    lblIntervalReadStore2.Enabled = true;
            //    tbIntervalReadStore.Enabled = true;
            //    tbIntervalReadStore.Text = "0";
            //}
            //else
            //{
            //    lblIntervalReadStore1.Enabled = false;
            //    lblIntervalReadStore2.Enabled = false;
            //    tbIntervalReadStore.Enabled = false;
            //    tbIntervalReadStore.Text = "0";
            //    // посылаем команду, отменяющую периодическое чтение накопительной информации
            //    byte[] paramss = { 0 };

            //    //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "SPC", String.Empty, paramss, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //    //         parent.WriteEventToLog( 35, "Команда \"SPC\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );
            //}
        }

        private void btnReadStoreBlock_Click(object sender, EventArgs e)
        {
            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RCD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "Команда \"RCD\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );
            //   // документирование действия пользователя
            //   parent.WriteEventToLog( 8, iIDDev.ToString(), this.Name, true, true, false );//"выдана команда RCD - чтение накопительной."
            //   //b_62132.FirstValue();
        }

        private void tbIntervalReadStore_KeyDown(object sender, KeyEventArgs e)
        {
            //if( e.KeyValue != (int)Keys.Enter )
            //    return;
            //// посылаем команду, устанавливающую периодическое чтение накопительной информации
            //byte[] paramss = new byte[2];
            //byte[] temp_paramss = { Byte.Parse( tbIntervalReadStore.Text ) };

            //if( temp_paramss.Length == 1 )
            //{
            //    paramss[0] = temp_paramss[0];
            //    paramss[1] = 0;
            //}
            //else if( temp_paramss.Length == 2 )
            //    paramss = temp_paramss;

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "SPC", String.Empty, paramss, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "Команда \"SPC\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );
        }

        private void btnReadMaxMeterBlock_Click(object sender, EventArgs e)
        {
            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RMD", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "Команда \"RMD\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );
        }

        private void btnResetMaxMeter_Click(object sender, EventArgs e)
        {
            if (CommonUtils.CommonUtils.IsUserActionBan(CommonUtils.CommonUtils.UserActionType.b06_Reset_info, HMI_MT_Settings.HMI_Settings.UserRight))
                return;

            DialogResult dr = MessageBox.Show("Сбросить максметр?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            // правильная запись в журнал действий пользователя - номер устройства с цчетом фк
            int uniDev = iFC * 256 + iIDDev;
            int numdevfc = uniDev;

            CommonUtils.CommonUtils.WriteEventToLog(35, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "CMD", new byte[] { }, this);
        }

        private void cbPeriodReadMaxMeter_CheckedChanged(object sender, EventArgs e)
        {
            //if( cbPeriodReadMaxMeter.Checked )
            //{
            //    lblIntervalReadMaxM1.Enabled = true;
            //    lblIntervalReadMaxM2.Enabled = true;
            //    tbIntervalReadMaxMeter.Enabled = true;
            //    tbIntervalReadMaxMeter.Text = "0";
            //}
            //else
            //{
            //    lblIntervalReadMaxM1.Enabled = false;
            //    lblIntervalReadMaxM2.Enabled = false;
            //    tbIntervalReadMaxMeter.Enabled = false;
            //    tbIntervalReadMaxMeter.Text = "0";
            //    // посылаем команду, отменяющую периодическое чтение максметра
            //    byte[] paramss = { 0 };

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "SPM", String.Empty, paramss, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //         parent.WriteEventToLog( 35, "Команда \"SPM\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );
            //}
        }

        private void tbIntervalReadMaxMeter_KeyDown(object sender, KeyEventArgs e)
        {
            //if( e.KeyValue != ( int ) Keys.Enter )
            //    return;
            //// посылаем команду, устанавливающую периодическое чтение максметра
            //byte[] paramss = new byte[2];
            //byte[] temp_paramss = { Byte.Parse( tbIntervalReadMaxMeter.Text ) };

            //if( temp_paramss.Length == 1 )
            //{
            //    paramss[0] = temp_paramss[0];
            //    paramss[1] = 0;
            //}
            //else if( temp_paramss.Length == 2 )
            //    paramss = temp_paramss;

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "SPM", String.Empty, paramss, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //           parent.WriteEventToLog( 35, "Команда \"SPM\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );
        }

        #endregion
        
        #region вход на вкладку "Конфигурации и уставки"
        /// <summary>
        /// private void tbcConfig_Enter( object sender, EventArgs e )
        ///  вход на вкладку "Конфигурации и уставки"
        /// </summary>
        private void tbcConfig_Enter( object sender, EventArgs e )
        {
            lstvConfig.Items.Clear();

            /*
             * скрываем панели
             */
            foreach (UserControl p in arDopPanel)
                p.Visible = false;
            pnlConfig.Visible = true;

            pnlConfig.btnWriteUst.Enabled = false;

            //-------------------------------------------------------------------
            //готовим инф. для отображения аналоговых и дискретных сигналов
            if( arrConfigSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbpConfUst);
                return;
            }

    		 //btnWriteUst.Enabled = false;
            CreateArrayList(arrConfigSign, "arrConfigSign");

            // для начала скрываем все tabpage
            for( int i = 0; i < tbcConfig.Controls.Count; i++ )
            {
                if( tbcConfig.Controls[i] is TabPage )
                {
                    tbcConfig.Controls.RemoveAt( i );
                    i--;
                }
            }

            // корректируем названия вкладок
            for( int i = 0; i < sl_tpnameUst.Count; i++ )
                tbcConfig.Controls.Add((Control) sl_tpnameUst.GetByIndex( i ) );

            // размещаем динамически на форме
            for( int i = 0; i < arrConfigSign.Count; i++ )
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrConfigSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                ComboBoxVar cBV;
					 switch( ev.ToT )
					 {
                     //    case TypeOfTag.Combo:
                     //       cBV = new ComboBoxVar((string[])( ( TagEval ) ( ( TagVal ) ev.arrTagVal[0] ).linkTagEval ).arrStrCB.ToArray(typeof(string)), 0);
                     //       cBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
                     //cBV.AutoSize = true;
                     //cBV.addrLinkVar = ev.addrVar;

                     //ev.OnChangeValForm += cBV.LinkSetText;
                     //ev.FirstValue();
                     //break;
                         case TypeOfTag.String:
                         case TypeOfTag.Analog:
                             usTB = new ctlLabelTextbox(ev);
                             usTB.LabelText = "";
                             usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                             usTB.AutoSize = true;

                             Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                             bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                             usTB.txtLabelText.DataBindings.Add(bnd);
                             ev.LinkVariableNewDS.BindindTag = bnd;
                             usTB.Caption_Text = ev.CaptionFE;
                             usTB.Dim_Text = ev.Dim;
                             break;
                         case TypeOfTag.Discret:
                             chBV = new CheckBoxVar(ev);
                             chBV.checkBox1.Text = "";
                             chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                             chBV.AutoSize = true;

                             Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                             bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                             chBV.checkBox1.DataBindings.Add(bndCB);
                             ev.LinkVariableNewDS.BindindTag = bndCB;
                             chBV.checkBox1.Text = ev.CaptionFE;
                             break;
                         case TypeOfTag.NaN:
                             break;
                         default:
                             MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                             break;
                     }
            }
            tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tbpConfUst);
        }

        private void UstavBD( )
        {
            DataBaseReq dbs = new DataBaseReq(HMI_Settings.ProviderPtkSql, "ShowDataLog2");
            var uniDev = (uint)(iFC * 256 + iIDDev);

            // входные параметры
            // 1. ip FC
            dbs.AddCMDParams(new DataBaseParameter("@IP", ParameterDirection.Input, SqlDbType.BigInt, 0));
            // 2. id устройства
            dbs.AddCMDParams(new DataBaseParameter("@id", ParameterDirection.Input, SqlDbType.Int, uniDev));
            // 3. начальное время
            TimeSpan tss = new TimeSpan(0, pnlConfig.dtpStartDateConfig.Value.Hour - pnlConfig.dtpStartTimeConfig.Value.Hour, pnlConfig.dtpStartDateConfig.Value.Minute - pnlConfig.dtpStartTimeConfig.Value.Minute, pnlConfig.dtpStartDateConfig.Value.Second - pnlConfig.dtpStartTimeConfig.Value.Second);
            DateTime tim = pnlConfig.dtpStartDateConfig.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_start", ParameterDirection.Input, SqlDbType.DateTime, tim));
            // 2. конечное время
            tss = new TimeSpan(0, pnlConfig.dtpEndDateConfig.Value.Hour - pnlConfig.dtpEndTimeConfig.Value.Hour, pnlConfig.dtpEndDateConfig.Value.Minute - pnlConfig.dtpEndTimeConfig.Value.Minute, pnlConfig.dtpEndDateConfig.Value.Second - pnlConfig.dtpEndTimeConfig.Value.Second);
            tim = pnlConfig.dtpEndDateConfig.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_end", ParameterDirection.Input, SqlDbType.DateTime, tim));
            // 5. тип записи
            // информация по уставкам
            int tbd = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Ustavki, uniDev);
            dbs.AddCMDParams(new DataBaseParameter("@type", ParameterDirection.Input, SqlDbType.Int, tbd));
            // 6. ид записи журнала
            dbs.AddCMDParams(new DataBaseParameter("@id_record", ParameterDirection.Input, SqlDbType.Int, 0));

            //dbs.DoStoredProcedure();

            // извлекаем данные по уставкам
            dtU = dbs.GetTableAsResultCMD();

            if (dtU.Rows.Count == 0 && IsMesView)
            {
                MessageBox.Show("Архивных данных для этого устройства нет.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsMesView = false;
            }

            // заполняем ListView
            lstvConfig.Items.Clear();
            for (int curRow = 0; curRow < dtU.Rows.Count; curRow++)
            {
                DateTime t = (DateTime)dtU.Rows[curRow]["TimeBlock"];
                ListViewItem li = new ListViewItem(CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t));
                li.SubItems.Add(dtU.Rows[curRow]["Arg1"].ToString());
                li.Tag = dtU.Rows[curRow]["ID"];
                lstvConfig.Items.Add(li);
            }
        }

        private void btnReadUstFC_Click(object sender, EventArgs e)
        {
            //   btnWriteUst.Enabled = true;

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "IMP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "Команда \"IMP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );

            //   // документирование действия пользователя
            //   parent.WriteEventToLog( 7, iIDDev.ToString(), this.Name, true, true, false );//"выдана команда IMP - чтения уставок."
            //   if( b_62002 != null )
            //       b_62002.FirstValue();
            //   if( b_62092 != null )
            //       b_62092.FirstValue();

            //pnlConfig.btnWriteUst.Enabled = true;

            // правильная запись в журнал действий пользователя
            // номер устройства с цчетом фк
            int numdevfc = iFC * 256 + iIDDev;

            CommonUtils.CommonUtils.WriteEventToLog(7, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "IMP", new byte[] { }, this);
        }

        private void btnReadUstBlock_Click(object sender, EventArgs e)
        {
            //   btnWriteUst.Enabled = true;

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "RCP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //       parent.WriteEventToLog( 35, "Команда \"RCP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );

            //   // документирование действия пользователя
            //   parent.WriteEventToLog( 7, iIDDev.ToString(), this.Name, true, true, false );//"выдана команда RCP - чтения уставок."
            //   if( b_62002 != null )
            //       b_62002.FirstValue();
            //   if( b_62092 != null )
            //       b_62092.FirstValue();
        }

        void btnReNewUstBD_Click(object sender, EventArgs e)
        {
            IsMesView = true;
            UstavBD();
        }

        private void lstvConfig_ItemActivate(object sender, EventArgs e)
        {
            try
            {
                if (lstvConfig.SelectedItems.Count == 0)
                    return;

                int id_block = (int)lstvConfig.SelectedItems[0].Tag;

                // номер устройства с цчетом фк
                int numdevfc = (int)(iFC * 256 + iIDDev);
                ArrayList arparam = new ArrayList();
                // номер арх записи в бд
                arparam.Add(id_block);
                // строка подключения
                byte[] str_cnt_in_bytes = Encoding.UTF8.GetBytes(HMI_Settings.ProviderPtkSql);
                arparam.Add(str_cnt_in_bytes);

                IRequestData req = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetData(0, (uint)numdevfc, "ArhivUstavkiBlockData", arparam, id_block);
                req.OnReqExecuted += new ReqExecuted(reqAvar_OnReqExecuted);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// private void btnWriteUst_Click( object sender, EventArgs e )
        /// запись уставок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>  
        private void btnWriteUst_Click(object sender, EventArgs e)
        {
            //  if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, parent.UserRight ) )
            //      return;

            //    if( parent.isReqPassword )
            //      if( !parent.CanAction() )
            //      {
            //          MessageBox.Show( "Выполнение действия запрещено" );
            //          return;
            //      }

            //DialogResult dr = MessageBox.Show( "Записать уставки?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
            //if( dr == DialogResult.No )
            //    return;

            //string stri;
            //TabPage tp;
            //ctlLabelTextbox ultb;
            //CheckBoxVar chbTmp;
            //ComboBoxVar cbTmp;

            //FlowLayoutPanel flp;
            //bool isUstChange = false;   // факт изменения уставок для последующего формирования команды
            //StringBuilder sb = new StringBuilder();
            //uint ainmemX;    // адрес в массиве memX
            //byte[] aTmp2 = new byte[2];

            //// найдем SortedList для нужного устройства
            //slLocal = new SortedList();
            //foreach( FC aFC in parent.KB )
            //    if( aFC.NumFC == iFC )
            //    {
            //        foreach( TCRZADirectDevice aDev in aFC )
            //            if( aDev.NumDev == iIDDev )
            //            {
            //                slLocal = aDev.CRZAMemDev;
            //                break;
            //            }
            //        break;
            //    }
            //// извлекаем пакет с уставками для корректировки
            ////BinaryReader memDevBlock = ( BinaryReader ) slLocal[62000];

            //// читаем данные в массив 
            //     //byte[] memXt = new byte[( ( byte[] ) slLocal[62000] ).Length];
            //    //System.Buffer.BlockCopy( brP, 6, memX, 0, brP.Length - 6 );

            //// читаем данные в массив 
            ////memDevBlock.BaseStream.Position = 0;
            //     int lenpack = 0;
            //     try
            //     {
            //         lenpack = BitConverter.ToInt16( ( byte[] ) slLocal[62000], 0 );
            //     } catch( ArgumentNullException ex )
            //     {
            //         MessageBox.Show( "Нет данных для записи. \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information );
            //         return;
            //     }

            //     short numdev = BitConverter.ToInt16( ( byte[] ) slLocal[62000] , 2 );

            //     ushort add10 = BitConverter.ToUInt16( ( byte[] ) slLocal[62000] , 4 );	//читаем адрес блока данных

            //    //int lenpack = ( short ) memDevBlock.ReadInt16();
            //    //short numdev = ( short ) memDevBlock.ReadUInt16();
            //    //ushort add10 = ( ushort ) memDevBlock.ReadInt16();	//читаем адрес блока данных

            //byte[] memX = new byte[lenpack - 6];
            //    System.Buffer.BlockCopy( ( byte[] ) slLocal[62000] , 6, memX, 0, (( byte[] ) slLocal[62000] ).Length - 6 );

            ////memDevBlock.Read( memX, 0, lenpack - 6 );

            //for( int i = 0; i < tbcConfig.Controls.Count; i++ )
            //{
            //    if( tbcConfig.Controls[i] is TabPage )
            //    {
            //        tp = ( TabPage ) tbcConfig.Controls[i];
            //        for( int j = 0; j < tp.Controls.Count; j++ )
            //        {
            //            if( tp.Controls[j] is FlowLayoutPanel )
            //            {
            //                flp = ( FlowLayoutPanel ) tp.Controls[j];
            //                for( int n = 0; n < flp.Controls.Count; n++ )
            //                {
            //                    if( flp.Controls[n] is ctlLabelTextbox )
            //                    {
            //                        ultb = ( ctlLabelTextbox ) flp.Controls[n];
            //                        if( ultb.isChange )
            //                        {
            //                                        CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 );
            //                                        //StrToBCD_Field( ultb, memX );
            //                                        isUstChange = true;	
            //                        }
            //                    }
            //                    else if( flp.Controls[n] is ComboBoxVar )
            //                    {
            //                        cbTmp = ( ComboBoxVar ) flp.Controls[n];
            //                        if( cbTmp.isChange )
            //                        {
            //                            isUstChange = true;
            //                            cbTmp.isChange = false;  // сбрасываем признак изменения у конкретного ComboBoxVar'а
            //                            // записываем изменения по ComboBoxVar'ам в исходный пакет (корректируем массив memX)
            //                            uint a = cbTmp.addrLinkVar; // адрес переменной
            //                            // получим значение
            //                            int st = cbTmp.cbVar.SelectedIndex;
            //                            byte[] bst = new byte[4];
            //                            bst = BitConverter.GetBytes(st);
            //                            Buffer.BlockCopy( bst, 0, aTmp2, 0, 2 );
            //                            Array.Reverse( aTmp2 );
            //                            // запоминаем изменения
            //                            ainmemX = ( a - 62000 ) * 2;
            //                            Buffer.BlockCopy( aTmp2, 0, memX, ( int ) ainmemX, 2 );
            //                        }
            //                    }
            //                    else if( flp.Controls[n] is CheckBoxVar )
            //                    {
            //                        chbTmp = ( CheckBoxVar ) flp.Controls[n];
            //                        if( chbTmp.isChange )
            //                        {
            //                            isUstChange = true;
            //                            chbTmp.isChange = false;    // сбрасываем признак изменения у конкретного CheckBoxVar'а
            //                            // извлечем битовое поле из исходного массива
            //                            ainmemX = ( chbTmp.addrLinkVar - 62000 ) * 2;   // это адрес
            //                            //aTmp2 = new byte[2];
            //                            Buffer.BlockCopy( memX, (int)ainmemX, aTmp2, 0, 2 );
            //                            string bitmask = chbTmp.addrLinkVarBitMask;
            //                            UInt16 ibitmask = Convert.ToUInt16( chbTmp.addrLinkVarBitMask, 16 );
            //                            Array.Reverse( aTmp2 );
            //                            UInt16 rezbit = BitConverter.ToUInt16(aTmp2,0);
            //                            if( chbTmp.checkBox1.Checked == true )
            //                                rezbit = Convert.ToUInt16( rezbit | ibitmask );
            //                            else
            //                            {
            //                                UInt16 ti = (UInt16)~ibitmask; //Convert.ToUInt16()
            //                                rezbit = Convert.ToUInt16( rezbit & ~ibitmask );
            //                            }
            //                            // записать на место
            //                            aTmp2 = BitConverter.GetBytes( rezbit );
            //                            Array.Reverse( aTmp2 );
            //                            Buffer.BlockCopy( aTmp2, 0, memX, ( int ) ainmemX, 2 );
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            ////------------------------------
            //// аналогично для панели уставок
            //        //for( int n = 0 ; n < pnlConfig.Controls.Count ; n++ )
            //    for( int n = 0 ; n < Config_BottomPanel.Controls.Count ; n++ )
            //            //if( pnlConfig.Controls[n] is ctlLabelTextbox )
            //        if( ( Config_BottomPanel.Controls[n] as ctlLabelTextbox ) != null)
            //        {
            //            ultb = ( ctlLabelTextbox ) Config_BottomPanel.Controls[n];
            //                if( ultb.Name == "ctlTimeUstavkiSbros" )
            //                    continue;

            //                if( ultb.isChange )
            //                {
            //                    CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 );
            //                    //StrToBCD_Field( ultb, memX );
            //                    isUstChange = true;						
            //                }
            //        }
            ////------------------------------
            //if( !isUstChange )
            //{
            //    MessageBox.Show("Уставки не изменялись. \nВыполнение команды отменено.","Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //// формируем пакет и команду для отправки изменения уставок
            //byte[] memXOut = new byte [memX.Length];
            //Buffer.BlockCopy( memX, 4, memXOut, 4, memX.Length - 4);  // Handle пока нулевой

            ////if( parent.newKB.ExecuteCommand( iFC, iIDDev, "WCP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            ////        parent.WriteEventToLog( 35, "Команда \"WCP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true, true, false );
            ////    // документирование действия пользователя
            ////    parent.WriteEventToLog( 6, iIDDev.ToString(), this.Name, true, true, false );			//"выдана команда WCP - запись уставок."
            //isUstChange = false;
        }
        //private void dtpStartDateConfig_ValueChanged(object sender, EventArgs e)
        //{
        //    UstavBD();
        //}
        #endregion

        #region Вкладка осциллограммы и диаграммы
        /// <summary>
        /// Вкладка осциллограммы и диаграммы
        /// </summary>
        private void tabPage5_Enter( object sender, EventArgs e )
        {
            if (dgvOscill.RowCount != 0 || dgvDiag.RowCount != 0)
                return;

            foreach (UserControl p in arDopPanel)
                p.Visible = false;

            pnlOscDiag.Visible = true;

            //DiagBD();
            OscBD();
        }
        /// <summary>
        /// получение осциллограмм из базы
        /// </summary>
        private void OscBD( )
        {
            if (oscdg == null)
                oscdg = new OscDiagViewer();

            oscdg.IdFC = iFC;
            oscdg.IdDev = iIDDev;
            oscdg.DTStartData = pnlOscDiag.dtpStartData.Value;
            oscdg.DTStartTime = pnlOscDiag.dtpStartData.Value;
            oscdg.DTEndData = pnlOscDiag.dtpEndData.Value;
            oscdg.DTEndTime = pnlOscDiag.dtpEndData.Value;
            oscdg.TypeRec = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Osc, (uint)(iFC * 256 + iIDDev) );// TypeBlockData.TypeBlockData_OscSirius;GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Osc, iFC, iIDDev);            

            // извлекаем данные по осциллограммам
            dtO = oscdg.Do_SQLProc();

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
        }
        /// <summary>
        /// получение диаграмм из базы
        /// </summary>
        private void DiagBD( )
        {
            if (oscdg == null)
                oscdg = new OscDiagViewer();

            oscdg.IdFC = iFC;
            oscdg.IdDev = iIDDev;
            oscdg.DTStartData = pnlOscDiag.dtpStartData.Value;
            oscdg.DTStartTime = pnlOscDiag.dtpStartData.Value;
            oscdg.DTEndData = pnlOscDiag.dtpEndData.Value;
            oscdg.DTEndTime = pnlOscDiag.dtpEndData.Value;
            oscdg.TypeRec = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Diagramm, (uint)(iFC * 256 + iIDDev) );

            // извлекаем данные по диаграммам
            dtG = oscdg.Do_SQLProc();
            for (int curRow = 0; curRow < dtG.Rows.Count; curRow++)
            {
                int i = dgvDiag.Rows.Add();   // номер строки
                dgvDiag["clmChBoxDiag", i].Value = false;
                dgvDiag["clmBlockNameDiag", i].Value = dtG.Rows[curRow]["BlockName"];
                dgvDiag["clmBlockIdDiag", i].Value = dtG.Rows[curRow]["BlockID"];
                dgvDiag["clmBlockTimeDiag", i].Value = dtG.Rows[curRow]["TimeBlock"];
                dgvDiag["clmCommentDiag", i].Value = dtG.Rows[curRow]["Comment"];
                dgvDiag["clmIDDiag", i].Value = dtG.Rows[curRow]["ID"];

                //dgvDiag["clmViewDiag", i].Value = "Диаграмма";
            }
        }
        private void btnReNewOscDg_Click(object sender, EventArgs e)
        {
            DiagBD();
            OscBD();
        }
        private void dgvOscill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewCell de;

            if (e.ColumnIndex == 0)
            {
                dgvOscill[e.ColumnIndex, e.RowIndex].Value = (bool)dgvOscill[e.ColumnIndex, e.RowIndex].Value ? false : true;
                btnUnionOsc.Enabled = true;
                return;
            }
            else if (e.ColumnIndex != 5)
                return;

            asb.Clear();
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

            oscdg.ShowOSCDg(0, dtO, ide);
        }
        private void dgvDiag_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewCell de;

            if (e.ColumnIndex == 0)
            {
                dgvDiag[e.ColumnIndex, e.RowIndex].Value = (bool)dgvDiag[e.ColumnIndex, e.RowIndex].Value ? false : true;
                btnUnionOsc.Enabled = true;
                return;
            }
            else if (e.ColumnIndex != 5)
                return;

            asb.Clear();
            btnUnionOsc.Enabled = false;

            // сбрасываем все флажки
            for (int i = 0; i < dtG.Rows.Count; i++)
                dgvDiag[0, i].Value = false;

            try
            {
                de = dgvDiag["clmIDDiag", e.RowIndex];
            }
            catch
            {
                MessageBox.Show("dgvDiag_CellContentClick - исключение");
                return;
            }

            int ide = (int)de.Value;

            /*
             * первый аргумент номер DS,
             * сейсчас для отработки механизма задана константа (0)
             * в дальнейшем нужно придумать механизм когда на данном этапе
             * будет известен реальный номер DS
             */
            oscdg.ShowOSCDg(0, dtG, ide);
        }
        /// <summary>
        /// объединяем осциллограммы
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            //ArrayList asb = new ArrayList();    // для хранения имен файлов
            //string ifa; ;
            //StringBuilder sb;
            //byte[] arrO = null;
            //char[] sep = { ' ', '-' };
            //string[] sp;

            //// перечисляем записи в таблице dbO, смотрим отмеченные, формируем файлы, вызываем fastview
            //for (int curRow = 0; curRow < dtO.Rows.Count; curRow++)
            //{
            //    if ((bool)dgvOscill[0, curRow].Value == true)
            //    {
            //        // формируем файл, запоминаем имя в массиве
            //        arrO = (byte[])dtO.Rows[curRow]["Data"];

            //        // формируем имя файла
            //        ifa = (string)dtO.Rows[curRow]["BlockName"] + "_" + curRow.ToString() + ".osc";

            //        // удаляем пробелы
            //        sp = ifa.Split(sep);
            //        sb = new StringBuilder();
            //        for (int i = 0; i < sp.Length; i++)
            //        {
            //            sb.Append(sp[i]);
            //        }
            //        // сохраняем имя в массиве
            //        asb.Add(sb);
            //        // создаем файл
            //        FileStream f = File.Create(Environment.CurrentDirectory.ToString() + "\\" + sb.ToString());
            //        f.Write(arrO, 0, arrO.Length);
            //        f.Close();
            //    }
            //}
            //// запускаем fastview
            //Process prc = new Process();
            //sb = new StringBuilder();
            //foreach (StringBuilder s in asb)
            //{
            //    sb.Append(s.ToString());
            //    sb.Append(" ");
            //}
            //prc.StartInfo.FileName = Environment.CurrentDirectory.ToString() + "\\Fastview\\fastview.exe";
            //prc.StartInfo.Arguments = "-o " + sb.ToString();//+ "/"
            //prc.Start();
        }
        /// <summary>
        /// объединяем диаграммы
        /// </summary>
        private void btnUnionDiag_Click(object sender, EventArgs e)
        {
            //ArrayList asb = new ArrayList();    // для хранения имен файлов
            //string ifa; ;
            //StringBuilder sb;
            //byte[] arrO = null;
            //char[] sep = { ' ', '-' };
            //string[] sp;

            //// перечисляем записи в таблице dbO, смотрим отмеченные, формируем файлы, вызываем fastview
            //for (int curRow = 0; curRow < dtG.Rows.Count; curRow++)
            //{
            //    if ((bool)dgvDiag[0, curRow].Value == true)
            //    {
            //        // формируем файл, запоминаем имя в массиве
            //        arrO = (byte[])dtG.Rows[curRow]["Data"];

            //        // формируем имя файла
            //        ifa = (string)dtG.Rows[curRow]["BlockName"] + "_" + curRow.ToString() + ".dgm";

            //        // удаляем пробелы
            //        sp = ifa.Split(sep);
            //        sb = new StringBuilder();
            //        for (int i = 0; i < sp.Length; i++)
            //        {
            //            sb.Append(sp[i]);
            //        }
            //        // сохраняем имя в массиве
            //        asb.Add(sb);
            //        // создаем файл
            //        FileStream f = File.Create(Environment.CurrentDirectory.ToString() + "\\" + sb.ToString());
            //        f.Write(arrO, 0, arrO.Length);
            //        f.Close();
            //    }
            //}
            //// запускаем fastview
            //Process prc = new Process();
            //sb = new StringBuilder();
            //foreach (StringBuilder s in asb)
            //{
            //    sb.Append(s.ToString());
            //    sb.Append(" ");
            //}
            //prc.StartInfo.FileName = Environment.CurrentDirectory.ToString() + "\\Fastview\\fastview.exe";
            //prc.StartInfo.Arguments = "-m " + sb.ToString();//+ "/"
            //prc.Start();
        }
        #endregion

        private void dtpStartData_ValueChanged( object sender, EventArgs e )
        {
            // выводим результаты запроса
            dgvOscill.Rows.Clear();
            OscBD();
            DiagBD();
        }


        /// <summary>
        /// mnuPrint_Click( object sender, EventArgs e )
        ///     Реализация пункта меню Параметры страницы
        /// </summary>
        private void mnuPageSetup_Click( object sender, EventArgs e )
        {
            //parent.mnuPageSetup_Click( sender, e );
        }
        /// <summary>
        /// mnuPrint_Click( object sender, EventArgs e )
        ///     Реализация пункта меню Предварительный просмотр
        /// </summary>
        private void mnuPrintPreview_Click( object sender, EventArgs e )
        {
            //PrintArr();
            //parent.mnuPrintPreview_Click( sender, e );
        }
        /// <summary>
        /// mnuPrint_Click( object sender, EventArgs e )
        ///     Реализация пункта меню Печать
        /// </summary>
        private void mnuPrint_Click( object sender, EventArgs e )
        {
            //PrintArr();
            //p//arent.mnuPrint_Click( sender, e );
        }
        /// <summary>
        /// PrintArr()
        ///     Печать массива переменных
        /// </summary>
        //private void PrintArr()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    float f_val;
        //    int i_val;
        //    string t_val = "";
        //    ArrayList arCurPrt = new ArrayList();

        //    object val;

        //    // определяем активную вкладку
        //    TabPage tp_sel = tabControl1.SelectedTab;

        //    sb.Length = 0;
            
        //    switch(tp_sel.Text)
        //    {
        //        case "Текущая информация":
        //            // формируем заголовок листинга
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (Текущая информация)" );
        //            sb.Append( "\n========================================================================\n" );
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrCurSign;
        //            break;
        //        case "Информация об авариях":
        //            // формируем заголовок листинга
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (Информация об авариях)" );
        //            sb.Append( "\n========================================================================\n" );
                    
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrAvarSign;
        //            break;
        //        case "Накопительная информация":
        //            // формируем заголовок листинга
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (Накопительная информация)" );
        //            sb.Append( "\n========================================================================\n" );
                    
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrStoreSign;
        //            break;
        //        case "Конфигурация и уставки":
        //            // формируем заголовок листинга
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (Конфигурация и уставки)" );
        //            sb.Append( "\n========================================================================\n" );
                    
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrConfigSign;
        //            break;
        //        case "Система":
        //            // формируем заголовок листинга
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (Система)" );
        //            sb.Append( "\n========================================================================\n" );
                    
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrSystemSign;
        //            break;
        //        case "Состояние устройства и команд":
        //            // формируем заголовок листинга
        //            sb.Append( "========================================================================\n" );
        //            sb.Append( this.Text + " (Состояние устройства и команд)" );
        //            sb.Append( "\n========================================================================\n" );
        //            sb.Append( " \n \n " );
        //            arCurPrt = arrStatusDevCommand;
        //            break;
        //        default:
        //            break;
        //    }                

        //    for( int i = 0; i < arCurPrt.Count; i++ )
        //    {
        //        FormulaEval ev = ( FormulaEval ) arCurPrt[i];

        //        switch(ev.ToT)
        //        {
        //            case TypeOfTag.Analog:
        //                val = ev.tRezFormulaEval.Value;

        //                if( val is float )
        //                {
        //                    f_val = ( float ) ev.tRezFormulaEval.Value;
        //                    t_val = f_val.ToString( "F2" ); // две цифры после запятой
        //                }
        //                else if( val is short )
        //                {
        //                    i_val = ( Int16 ) ev.tRezFormulaEval.Value;
        //                    t_val = i_val.ToString();
        //                }
        //                else if( val is int )
        //                {
        //                    i_val = ( Int32 ) ev.tRezFormulaEval.Value;
        //                    t_val = i_val.ToString();
        //                }
        //                else if( val is string )
        //                { 
        //                    t_val = (string) ev.tRezFormulaEval.Value;
        //                }
        
        //                sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
        //                break;
        //            case TypeOfTag.Discret:
        //                sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
        //                break;
        //            case TypeOfTag.Combo:
        //                sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
        //                break;
        //            default:
        //                continue;
        //        }
        //        sb.Append( " \n " );
        //    }
        //    parent.prt.rtbText.AppendText( sb.ToString() );            
        //}
        /// <summary>
        /// sbForSimpleVar(StringBuilder sb)
        ///     формирование строки для печати для отдельной переменной
        /// </summary>
        //private void sbForSimpleVar(StringBuilder sb, FormulaEval b_xxx)
        //{
        //    float f_val;
        //    int i_val;
        //    string t_val = "";
        //    FormulaEval ev = ( FormulaEval ) b_xxx;
        //    object val;

        //    if( b_xxx == null )
        //        return;

        //    switch( ev.ToT )
        //    {
        //        case TypeOfTag.NoN:
        //            val = ev.tRezFormulaEval.Value;

        //            if( val is float )
        //            {
        //                f_val = ( float ) ev.tRezFormulaEval.Value;
        //                t_val = f_val.ToString( "F2" ); // две цифры после запятой
        //            }
        //            else if( val is short )
        //            {
        //                i_val = ( Int16 ) ev.tRezFormulaEval.Value;
        //                t_val = i_val.ToString();
        //            }
        //            else if( val is ushort )
        //            {
        //                i_val = ( UInt16 ) ev.tRezFormulaEval.Value;
        //                t_val = i_val.ToString();
        //            }
        //            else if( val is int )
        //            {
        //                i_val = ( Int32 ) ev.tRezFormulaEval.Value;
        //                t_val = i_val.ToString();
        //            }
        //            else if( val is string )
        //            {
        //                t_val = ( string ) ev.tRezFormulaEval.Value;
        //            }

        //            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
        //            break;
        //        case TypeOfTag.Analog:
        //            val = ev.tRezFormulaEval.Value;

        //            if( val is float )
        //            {
        //                f_val = ( float ) ev.tRezFormulaEval.Value;
        //                t_val = f_val.ToString( "F2" ); // две цифры после запятой
        //            }
        //            else if( val is short )
        //            {
        //                i_val = ( Int16 ) ev.tRezFormulaEval.Value;
        //                t_val = i_val.ToString();
        //            }
        //            else if( val is int )
        //            {
        //                i_val = ( Int32 ) ev.tRezFormulaEval.Value;
        //                t_val = i_val.ToString();
        //            }
        //            else if( val is string )
        //            {
        //                t_val = ( string ) ev.tRezFormulaEval.Value;
        //            }

        //            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
        //            break;
        //        case TypeOfTag.Discret:
        //            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
        //            break;
        //        case TypeOfTag.Combo:
        //            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
        //            break;
        //        default:
        //            break;
        //    }
        //    sb.Append( " \n " );
        //}

        //private void btnResetValues_Click( object sender, EventArgs e )
        //{
        //    //btnWriteUst.Enabled = false;
        //    //parent.newKB.ResetGroup(iFC, iIDDev, 14);
        //}
		
		#region вход на вкладку с журналом
		private void tabLog_Enter( object sender, EventArgs e )
		{
			// принудительно читаем записи журнала
         //parent.newKB.ExecuteCommand( iFC, iIDDev, "IME", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent );
			//-------------------------------------------------------------------
			//готовим инф. для отображения текущих значений аналоговых и дискретных сигналов
			// текущие аналоговые сигналы (рег. 0033 ...) с учетом коэффициента трансформации
			if( arrSystemSign.Count != 0 )
            {
                tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabLog);
                return;
            }

			CreateArrayList( arrSystemSign, "arrSystemSign" );

			// размещаем динамически на форме
			for( int i = 0 ; i < arrSystemSign.Count ; i++ )
			{
                FormulaEvalNDS ev = (FormulaEvalNDS)arrSystemSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                switch (ev.ToT)
                {
                    case TypeOfTag.String:
                    case TypeOfTag.Analog:
                        usTB = new ctlLabelTextbox(ev);
                        usTB.LabelText = "";
                        usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        usTB.AutoSize = true;

                        Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                        bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                        usTB.txtLabelText.DataBindings.Add(bnd);
                        ev.LinkVariableNewDS.BindindTag = bnd;
                        usTB.Caption_Text = ev.CaptionFE;
                        usTB.Dim_Text = ev.Dim;
                        break;
                    case TypeOfTag.Discret:
                        chBV = new CheckBoxVar(ev);
                        chBV.checkBox1.Text = "";
                        chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                        chBV.AutoSize = true;

                        Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                        bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                        chBV.checkBox1.DataBindings.Add(bndCB);
                        ev.LinkVariableNewDS.BindindTag = bndCB;
                        chBV.checkBox1.Text = ev.CaptionFE;
                        break;
                    case TypeOfTag.NaN:
                        break;
                    default:
                        MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            tpCurrent = HMI_MT_Settings.HMI_Settings.SetTagsSubscribe4TPCurrent(tabLog);            		
		}
        private void button4_Click_1(object sender, EventArgs e)
        {
            // принудительно читаем записи журнала
            // parent.newKB.ExecuteCommand( iFC, iIDDev, "IME", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent );
        }
        #endregion

      private void btnAck_Click( object sender, EventArgs e )
      {
         //if( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b02_ACK_Signaling, parent.UserRight ) )
         //   return;

         //ConfirmCommand dlg = new ConfirmCommand( );
         //dlg.label1.Text = "Квитировать?";

         //if( !( DialogResult.OK == dlg.ShowDialog( ) ) )
         //   return;
         //// выполняем действия по квитированию
         //Console.WriteLine( "Поступила команда \"Квитировать\" для устройства: {0}; id: {1}", "Дуга", iIDDev );
         //// запись в журнал
         ////parent.WriteEventToLog( 20, strIDDev, "Дуга", true, true, false );

         ////if( parent.newKB.ExecuteCommand( iFC, iIDDev, "ECC", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
         ////   parent.WriteEventToLog( 35, "Команда \"Квитировать\" ушла в сеть. Устройство - "
         ////      + strFC + "." + strIDDev, "Дуга", true, true, false );
      }
      /// <summary>
      /// для отслеживания смены 
      /// вкладок, чтобы корректировать подписку
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tabControl1_Selected(object sender, TabControlEventArgs e)
      {
          HMI_MT_Settings.HMI_Settings.HMIControlsTagReNew(tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe);
          tpCurrent.Tag = false; // признак отписки от тегов для данной TabPage
          tpCurrent = e.TabPage;  // запомним новую текущую вкладку
      }
      /// <summary>
      /// Идентификатор блока
      /// </summary>
      public uint Guid { get { return (uint)( iFC * 256 + iIDDev ); } }
    }
}
        