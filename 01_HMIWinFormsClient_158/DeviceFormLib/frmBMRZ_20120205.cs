/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Форма для работы с блоком БМРЗ ВВ-14-31-12.                                                           
 *                                                                             
 *	Файл                     : frmBMRZVV14_31_12.cs                                         
 *	Тип конечного файла      : 
 *	версия ПО для разработки : С#, Framework 2.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : xx.04.2007                                       
 *	Дата (v1.0)              :                                                  
 *******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/

using System;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Calculator;
using System.Collections;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Globalization;
using LabelTextbox;
using CommonUtils;
using System.Linq;
using System.Xml.Linq;
using InterfaceLibrary;
using HMI_MT_Settings;
using DataBaseLib;
using OscillogramsLib;

namespace DeviceFormLib
{
	public partial class frmBMRZ : Form, IDeviceForm
	{
        #region поля и переменные
        //private Form parent;//MainForm
        XDocument xdoc;
        SortedList slFormElements; // список элементов, свойства которых нужно будет изменить (см. Device.cfg)
        int iFC;            // номер ФК целочисленный
        string strFC;       // номер ФК строка
        int iIDDev;         // номер устройства целочисленный
        string strIDDev;    // номер устройства строка
        //string nfXMLConfig; // имя файла с описанием 
        /// <summary>
        ///  имя папки с общими файлами типа устройства
        /// </summary>
        string folderDevDescrPattern;
        /// <summary>
        /// имя файла с описание формы
        /// </summary>
        string fileFrmTagsDescript;
        string nfXMLConfigFC; // имя файла с описанием ЩАСУ
        // массив дополнительных панелей
        ArrayList arDopPanel;

        ArrayList arrAvarSign = new ArrayList();
        ArrayList arrCurSign = new ArrayList();
        ArrayList arrSystemSign = new ArrayList();
        ArrayList arrStoreSign = new ArrayList();
        ArrayList arrMaxMeterSign = new ArrayList();
        ArrayList arrConfigSign = new ArrayList();
        ArrayList arrStatusDevCommand = new ArrayList();
        ArrayList arrStatusFCCommand = new ArrayList();

        ushort iclm = 16;  // число колонок в дампе
        SortedList slLocal;
        SortedList se = new SortedList();
        SortedList sl_tpnameUst = new SortedList();
        StringBuilder sbse = new StringBuilder();

        DataTable dtO;  // таблица с осциллограммами
        DataTable dtG;  // таблица с диаграммами
        DataTable dtA;  // таблица с авариями
        DataTable dtU;  // таблица с уставками

        SortedList slFLP = new SortedList();	// для хранения инф о FlowLayoutPanel

        OscDiagViewer oscdg;
        /// <summary>
        /// список тегов для подписки/отписки
        /// </summary>
	    List<ITag> taglist;
        /// <summary>
        /// список списков тегов по вкладкам
        /// </summary>
        SortedDictionary<string,List<ITag> > slTagListByTabPages = new SortedDictionary<string,List<ITag>>();

        /// <summary>
        /// список коэф трансф. из файла PrgDevCFG.cdp 
        /// для данного устройства
        /// </summary>
        private SortedList<string, string> slKoefRatioValue = new SortedList<string, string>();
        /// <summary>
        /// флаг - покаывать ли сообщение об отсутсвии архивных записей 
        /// или нет
        /// </summary>
        bool IsMesView = false;
        #endregion

        #region конструктор
        public frmBMRZ(/*Form MainForm linkMainForm,*/ int iFC, int iIDDev, /*int inumLoc,*/ string fXML, string ftagsxmldescript)
		{
			InitializeComponent();
			try
			{
                InitInterfaceElementsClick();

                //parent = linkMainForm;
                this.iFC = iFC;                 // номер ФК целочисленный
                strFC = iFC.ToString();         // номер ФК строка
                this.iIDDev = iIDDev;           // номер устройства целочисленный
                strIDDev = iIDDev.ToString();   // номер устройства строка
                folderDevDescrPattern = fXML;
                fileFrmTagsDescript = ftagsxmldescript;
                string TypeName = String.Empty;
                string nameR = String.Empty;
                string nameELowLevel = String.Empty;
                string nameEHighLevel = String.Empty;

                slFormElements = new SortedList();

                // формируем массив панелей и скрываем их
                arDopPanel = new ArrayList();
                arDopPanel.Add(this.pnlCurrent);
                arDopPanel.Add(this.pnlAvar);
                arDopPanel.Add(this.pnlSystem);
                arDopPanel.Add(this.pnlStore);
                arDopPanel.Add(this.pnlConfig);
                arDopPanel.Add(this.pnlOscDiag);
                arDopPanel.Add(this.pnlStatusSHASU);


                foreach (Panel p in arDopPanel)
                    p.Visible = false;

                /*
                 * вычислим название файла с описанием устройства
                 * для этого используем файл PrgDevCFG.cdp источника
                 */
                XElement xedev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription(0, "MOA_ECU", string.Format("{0}.{1}", strFC, strIDDev));
                xedev = xedev.Element("DescDev");   // подправили

                                TypeName = xedev.Element("TypeName").Value;
                                nameR = xedev.Element("nameR").Value;
                                nameELowLevel = xedev.Element("nameELowLevel").Value;
                                nameEHighLevel = xedev.Element("nameEHighLevel").Value;
                                this.Text = nameR + " ( ид.№ " + iIDDev.ToString() + " )" + xedev.Element("DescDev").Value;
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}

        /// <summary>
        /// в этой функции связываются клики на
        /// элементах интерфейса с кодом их обработки
        /// </summary>
        public void InitInterfaceElementsClick()
        {
            this.FormClosing +=new FormClosingEventHandler(frmBMRZ_FormClosing);
            tabPage1.Enter +=new EventHandler(tabPage1_Enter);
            tbpAvar.Enter += new EventHandler(tbpAvar_Enter);
            lstvAvar.ItemActivate +=new EventHandler(lstvAvar_ItemActivate);
            //btnReNew.Click += new EventHandler(btnReNew_Click);
            tabStore.Enter += new EventHandler(tabStore_Enter);
            tbpConfUst.Enter += new EventHandler(tbcConfig_Enter);
            //btnReadUstFC.Click += new EventHandler(btnReadUstFC_Click);
            lstvConfig.ItemActivate += new EventHandler(lstvConfig_ItemActivate);
            //btnReNewUstBD.Click += new EventHandler(btnReNewUstBD_Click);
            
            //btnWriteUst.Click += new EventHandler(btnWriteUst_Click);
            //btnResetValues.Click += new EventHandler(btnResetValues_Click);
            tabPage5.Enter += new EventHandler(tabPage5_Enter);
            tabSystem.Enter += new EventHandler(tabSystem_Enter);

            dtpStartData.ValueChanged += new EventHandler(dtpStartData_ValueChanged);
            dgvOscill.CellContentClick += new DataGridViewCellEventHandler(dgvOscill_CellContentClick);
            dgvDiag.CellContentClick += new DataGridViewCellEventHandler(dgvDiag_CellContentClick);
            //btnReNewOD.Click += new EventHandler(btnReNewOD_Click);
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
                switch(typetabpage)
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

        /// <summary>
        /// загрузка формы
        /// </summary>
        private void frmBMRZVV14_31_12_Load( object sender, EventArgs e )
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

            //this.Width = parent.Width;
            //this.Height = parent.Height;

            //tabControl1.Width = this.Width - 10;
            //tabControl1.Height = parent.ClientSize.Height - parent.statusStrip1.Height - pnlCurrent.Height - parent.menuStrip1.Height - parent.tabForms.Height - 10;//parent.panelMes.Height - 

            SetElementsFormFeatures( folderDevDescrPattern );
        }

        /// <summary>
        /// установить особенности элементов формы на основании Device.cfg (см. УСО А - 00)
        /// </summary>
        /// <param Name="pathtoDevCFG">путь к папке с описанием устройства - файл Device.cfg</param>
        private void SetElementsFormFeatures( string pathtoDevCFG )
        {
           xdoc = XDocument.Load( pathtoDevCFG + Path.DirectorySeparatorChar + "Device.cfg" );
           // ищем элементы формы по Name
           if ( String.IsNullOrEmpty( ( string ) xdoc.Element( "Device" ).Element( "DeviceFeatures" ).Element( "GDIFeatures" ) ) )
              return;

           IEnumerable<XElement> collNameEl = from tp in xdoc.Element( "Device" ).Element( "DeviceFeatures" ).Element( "GDIFeatures" ).Element( "ElementsAction" ).Elements( "Element" )
                                              select tp;

           // изменяемые элементы в список
           foreach ( XElement xecollNameEl in collNameEl )
              slFormElements.Add( xecollNameEl.Attribute( "Name" ).Value, null );

           // ищем эдементы списка на форме
           ControlCollection cc;
           cc = ( ControlCollection ) this.Controls;
           for ( int i = 0 ; i < cc.Count ; i++ )
              if ( slFormElements.Contains( cc[ i ].Name ) )
                 slFormElements[ cc[ i ].Name ] = cc[ i ];
              else
                 TestCCforElements( cc[ i ] );

           // изменяем свойства элементов
           foreach ( XElement xecollNameEl in collNameEl )
           {
              Control cntrl = ( Control ) slFormElements[ xecollNameEl.Attribute( "Name" ).Value ];
              switch ( xecollNameEl.Element( "Property" ).Attribute( "Name" ).Value )
              {
                 case "Enabled":
                    ( ( Control ) slFormElements[ xecollNameEl.Attribute( "Name" ).Value ] ).Enabled = Convert.ToBoolean( xecollNameEl.Element( "Property" ).Value );
                    break;
                 default:
                    break;
              }
           }
        }

        /// <summary>
        /// рекурсивный поиск элементов для включения в список на изменение свойств
        /// </summary>
        /// <param Name="cc"></param>  
        private void TestCCforElements( Control cc )
        {
           for ( int i = 0 ; i < cc.Controls.Count ; i++ )
              if ( slFormElements.Contains( cc.Controls[ i ].Name ) )
                 slFormElements[ cc.Controls[ i ].Name ] = cc.Controls[ i ];
              else
                 TestCCforElements( cc.Controls[ i ] );
        }

        /// <summary>
        /// закрытие формы
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void frmBMRZ_FormClosing(object sender, FormClosingEventArgs e)
        {
            // отписываемся от тегов
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(taglist);
        }
        #endregion

        /// <summary>
		  /// создание массива ArrayList с описанием переменных по содержимому файла XML
		  /// </summary>
		  /// <param Name="arrVar"> массив  ArrayList
		  ///фигуры</param>
		  /// <param Name="nameFile">имя файла XML
		  ///фигуры</param>
        private void CreateArrayList( ArrayList arrVar, string name_arrVar )
		  {
			  SortedList sl = new SortedList();
			  ArrayList alVal = new ArrayList();
                XDocument xd;
			try
			{
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

                        TypeOfTag ToT = TypeOfTag.no;
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
                                ToT = TypeOfTag.no;
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
                        for (int i = 0; i < tbkConfig.Controls.Count; i++)
                        {
                            if (tbkConfig.Controls[i] is TabPage && tbkConfig.Controls[i].Name == sbse.ToString())
                            {
                                tbkConfig.Controls[i].Text = xef.Element("value").Attribute("tag").Value;
                                sl_tpnameUst[sbse.ToString()] = tbkConfig.Controls[i];
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

                // подписываемся на обновление тегов с DataServer
                taglist = new List<ITag>();

                foreach (FormulaEvalNDS fe in arrVar)
                {
                    if (fe.LinkVariableNewDS == null)
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 428, string.Format("() : frmBMRZ.cs : CreateArrayList() : Нет привязки к тегу = {0}", fe.CaptionFE));               
                    else
                        taglist.Add(fe.LinkVariableNewDS);
                }

                if (!slTagListByTabPages.ContainsKey(name_arrVar))
                    slTagListByTabPages.Add(name_arrVar, taglist);

                HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags(taglist);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
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
      /// <summary>
      /// вход на Tab с текущей информацией
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      private void tabPage1_Enter( object sender, EventArgs e )
        {
           // скрываем/показываем нужную панель
            foreach( Panel p in arDopPanel )
                p.Visible = false;

            //-------------------------------------//
			pnlCurrent.Visible = true;

            pnlCurrent.Left = tabControl1.Left + 5;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlCurrent.Height;
            pnlCurrent.Parent = splitContainer1.Panel2;
            pnlCurrent.Dock = DockStyle.Fill;

            //pnlCurrent.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlCurrent.Width = tabControl1.Width - 10;
            
            Current_Analog_First.Left = 3;
            Current_Analog_First.Width = tabPage1.Width / 3 - 3 ;
            Current_Analog_First.Height = tabPage1.Height - 10;

            Current_DiscretIn.Left = Current_Analog_First.Left + Current_Analog_First.Width + 3 ;
            Current_DiscretIn.Height = tabPage1.Height - 10;
            Current_DiscretIn.Width = tabPage1.Width / 3 - 3;

            Current_DiscretOut.Left = Current_DiscretIn.Left + Current_DiscretIn.Width + 3;
            Current_DiscretOut.Height = tabPage1.Height - 10;
            Current_DiscretOut.Width = tabPage1.Width / 3 - 3;

            //-------------------------------------------------------------------
            //готовим инф. для отображения текущих значений аналоговых и дискретных сигналов
            // текущие аналоговые сигналы (рег. 0033 ...) с учетом коэффициента трансформации
            if( arrCurSign.Count != 0 )
                return;
				 CreateArrayList( arrCurSign, "arrCurSign" );

           // размещаем динамически на форме
            for( int i = 0; i < arrCurSign.Count; i++ )
            {
                FormulaEvalNDS ev = ( FormulaEvalNDS ) arrCurSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Analog:
							 usTB = new ctlLabelTextbox(ev);
							 usTB.LabelText = "";
							 usTB.Parent = (FlowLayoutPanel) slFLP[ev.ToP];
							 usTB.AutoSize = true;

							 Binding bnd = new Binding("Text",ev.LinkVariableNewDS,"ValueAsString", true);
							 bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
							 usTB.txtLabelText.DataBindings.Add(bnd);
                            try
			                {
                                ev.LinkVariableNewDS.BindindTag = bnd;
                            }
			                catch(Exception ex)
			                {
				                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			                }							 
							 usTB.Caption_Text = ev.CaptionFE;
							 usTB.Dim_Text = ev.Dim;
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar(ev);
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;

							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
							bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.checkBox1.Text = ev.CaptionFE;
							 break;
						 case TypeOfTag.no:
							 break;
						 default:
							 MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
							 break;
					 }
            }

			// открываем доп панели на pnlCurrent
            // === настройка нижней панели ========\\
            ArrayList tabforremove = new ArrayList( );
            for ( int i = 0 ; i < tcCurrentBottomPanel.TabPages.Count ; i++ )
            {
               TabPage tpt = tcCurrentBottomPanel.TabPages[ i ];
               GroupBox gbt;
               FlowLayoutPanel flpt;

               for ( int j = 0 ; j < tpt.Controls.Count ; j++ )
               {
                  if ( tpt.Controls[ j ] is GroupBox )
                  {
                     gbt = (GroupBox)tpt.Controls[j];
                     tpt.Text = gbt.Text;
                     for ( int jj = 0 ; jj < gbt.Controls.Count ; jj++ )
                        if ( gbt.Controls[ jj ] is FlowLayoutPanel )
                           if ( ( ( FlowLayoutPanel ) gbt.Controls[ jj ] ).Controls.Count == 0 )
                              tabforremove.Add( tpt );
                  }
               }
            }
			// удаляем пустые TabPage
            for ( int i = 0 ; i < tabforremove.Count ; i++ )
               tcCurrentBottomPanel.TabPages.Remove( (TabPage)tabforremove[i] );



            tabSystem_Enter( sender, e );
       }

      private void tabPage1_Leave(object sender, EventArgs e)
      {
          if (slTagListByTabPages.ContainsKey("arrCurSign"))
            // отписываемся от тегов
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(slTagListByTabPages["arrCurSign"]);
      }
        #endregion

        #region Tab с аварийной информацией
        /*=======================================================================*
        *   private void tbpAvar_Enter( object sender, EventArgs e )
        *       вход на Tab с аварийной информацией
        *=======================================================================*/
        private void tbpAvar_Enter( object sender, EventArgs e )
        {
           // устанавливаем пикеры для вывода аварийной информации за последние сутки
           dtpEndDateAvar.Value = DateTime.Now;
           dtpEndTimeAvar.Value = DateTime.Now;
           ;
           dtpStartDateAvar.Value = DateTime.Now;
           ;

           TimeSpan ts = new TimeSpan( 1, 0, 0, 0 );
           dtpStartDateAvar.Value = dtpStartDateAvar.Value - ts;
           dtpStartTimeAvar.Value = DateTime.Now;

            //-------------------------------------------------------------------
            // скрываем/показываем нужную панель
            foreach( Panel p in arDopPanel )
                p.Visible = false;
            pnlAvar.Visible = true;

            pnlAvar.Left = tabControl1.Left + 5;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlAvar.Height;
            pnlAvar.Parent = splitContainer1.Panel2;
            pnlAvar.Dock = DockStyle.Fill;

            //pnlAvar.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlAvar.Width = tabControl1.Width - 10;

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

            //-------------------------------------------------------------------
            //готовим инф. для отображения аналоговых и дискретных сигналов
            
            if( arrAvarSign.Count != 0 )
                return;

            lstvAvar.Items.Clear();
            
            AvarBD();

            CreateArrayList( arrAvarSign, "arrAvarSign" );
            
            // размещаем динамически на форме
            for( int i = 0; i < arrAvarSign.Count; i++ ) 
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrAvarSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Analog:
                             usTB = new ctlLabelTextbox(ev);
							 usTB.LabelText = "";
							 usTB.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 usTB.AutoSize = true;

							 Binding bnd = new Binding("Text",ev.LinkVariableNewDS,"ValueAsString", true);
							 bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
							 usTB.txtLabelText.DataBindings.Add(bnd);
							 ev.LinkVariableNewDS.BindindTag = bnd;
							 usTB.Caption_Text = ev.CaptionFE;
							 usTB.Dim_Text = ev.Dim;
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar(ev);
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;

							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
            				bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.checkBox1.Text = ev.CaptionFE;
							 break;
						 case TypeOfTag.no:
							 break;
						 default:
							 MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
							 break;
					 }
            }            
        }
        /*=======================================================================*
        *   private void lstvAvar_ItemActivate( object sender, EventArgs e )
        *       вывод информации при выборе конкретной аварии
        *=======================================================================*/
        private void lstvAvar_ItemActivate( object sender, EventArgs e )
        {
            if (lstvAvar.SelectedItems.Count == 0)
                return;
            
            #region старый код
		    //извлекаем данные по аварии

            //byte[] adata = CommonUtils.CommonUtils.GetBlockData(HMI_Settings.cstr, Convert.ToInt32(lstvAvar.SelectedItems[0].Tag));

            // вызываем процедуру разбора пакета с аварийной информацией из базы
            //ParseBDPacket( adata, 10280, iIDDev ); 
	        #endregion

            int id_block = (int)lstvAvar.SelectedItems[0].Tag;

            // номер устройства с цчетом фк
            int numdevfc = iFC * 256 + iIDDev;
            ArrayList arparam = new ArrayList();
            // номер арх записи в бд
            arparam.Add(id_block);
            // строка подключения
            byte[] str_cnt_in_bytes = Encoding.UTF8.GetBytes(HMI_Settings.cstr);
            arparam.Add(str_cnt_in_bytes);

            IRequestData req = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetData(0, (uint)numdevfc, "ArhivAvariBlockData", arparam, numdevfc);

            //byte[] adata = DataBaseReq.GetBlockData(HMI_Settings.cstr, (int)lstvConfig.SelectedItems[0].Tag);

            // вызываем процедуру разбора пакета из базы
            // SetArhivGroupInDev(iIDDev, 14);
            // ParseBDPacket(adata, 62000, iIDDev);
        }

        private void AvarBD( )
        {
            DataBaseReq dbs = new DataBaseReq(HMI_Settings.cstr, "ShowDataLog2");

            // входные параметры
            // 1. ip FC
            dbs.AddCMDParams(new DataBaseParameter("@IP", ParameterDirection.Input, SqlDbType.BigInt, 0));
            // 2. id устройства
            dbs.AddCMDParams(new DataBaseParameter("@id", ParameterDirection.Input, SqlDbType.Int, iFC * 256 + iIDDev));
            // 3. начальное время
            TimeSpan tss = new TimeSpan(0, dtpStartDateAvar.Value.Hour - dtpStartTimeAvar.Value.Hour, dtpStartDateAvar.Value.Minute - dtpStartTimeAvar.Value.Minute, dtpStartDateAvar.Value.Second - dtpStartTimeAvar.Value.Second);
            DateTime tim = dtpStartDateAvar.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_start", ParameterDirection.Input, SqlDbType.DateTime, tim));
            // 2. конечное время
            tss = new TimeSpan(0, dtpEndDateAvar.Value.Hour - dtpEndTimeAvar.Value.Hour, dtpEndDateAvar.Value.Minute - dtpEndTimeAvar.Value.Minute, dtpEndDateAvar.Value.Second - dtpEndTimeAvar.Value.Second);
            tim = dtpEndDateAvar.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_end", ParameterDirection.Input, SqlDbType.DateTime, tim));
            // 5. тип записи
            // информация по авариям
            int tbd = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Srabat, iFC, iIDDev);
            dbs.AddCMDParams(new DataBaseParameter("@type", ParameterDirection.Input, SqlDbType.Int, tbd));
            // 6. ид записи журнала
            dbs.AddCMDParams(new DataBaseParameter("@id_record", ParameterDirection.Input, SqlDbType.Int, 0));

            dbs.DoStoredProcedure();

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
                //DateTime t = (DateTime)dtA.Rows[curRow]["TimeBlock"];
                //ListViewItem li = new ListViewItem(t.ToShortDateString());
                //li.SubItems.Add(t.ToLongTimeString() + ":" + t.Millisecond);
                //li.Tag = dtA.Rows[curRow]["ID"];
                //lstvAvar.Items.Add(li);

                DateTime t = (DateTime)dtA.Rows[curRow]["TimeBlock"];
                ListViewItem li = new ListViewItem(CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t));
                //li.SubItems.Add(t.ToLongTimeString() + ":" + t.Millisecond);
                li.Tag = dtA.Rows[curRow]["ID"];
                lstvAvar.Items.Add(li);
            }
           }
        #region процедура разбора пакета с аварийной информацией из базы
        private void ParseBDPacket(byte[] pack, ushort adr , int dev)
        {
            //PrintHexDump( "LogHexPacket.dat" , pack);  // выведем в файл для контроля
            //parent.newKB.PacketToQueDev( pack, adr , iFC,dev); // 10280 пакет  по адресу  устройства
            //// объявить соответсвующую группу переменных архивной
            //SetArhivGroupInDev(dev, 8);
        }
        #endregion

        private void btnReNew_Click( object sender, EventArgs e )
        {
            IsMesView = true;
            AvarBD();
            tcAvarBottomPanel.SelectTab(0);//.TabPages[ 0 ].Select( );
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
        private void tbpAvar_Leave(object sender, EventArgs e)
        {
            if (slTagListByTabPages.ContainsKey("arrAvarSign"))
                // отписываемся от тегов
                HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(slTagListByTabPages["arrAvarSign"]);
        }
        #endregion

        #region вход на вкладку накопительной информации
        private void tabStore_Enter( object sender, EventArgs e )
        {
            // посылаем команду, отменяющую периодическое чтение накопительной информации и максметра
            /*byte[] paramss = new byte[2];
			paramss[0] =  paramss[1] = 0;

			parent.newKB.ExecuteCommand(iFC, iIDDev, "SPC", paramss, parent.toolStripProgressBar1 );
            lblIntervalReadStore1.Enabled = false;
            lblIntervalReadStore2.Enabled = false;
            tbIntervalReadStore.Enabled = false;

            parent.newKB.ExecuteCommand( iFC, iIDDev, "SPM", paramss, parent.toolStripProgressBar1 );
            lblIntervalReadMaxM1.Enabled = false;
            lblIntervalReadMaxM2.Enabled = false;
            tbIntervalReadMaxMeter.Enabled = false;*/

            // скрываем/показываем нужную панель
            foreach( Panel p in arDopPanel )
                p.Visible = false;
            pnlStore.Visible = true;

            pnlStore.Left = tabControl1.Left + 5;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlStore.Height;
            pnlStore.Parent = splitContainer1.Panel2;
            pnlStore.Dock = DockStyle.Fill;

            //pnlStore.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlStore.Width = tabControl1.Width;

            if( arrStoreSign.Count != 0 )
                return;

            CreateArrayList(arrStoreSign, "arrStoreSign");
           //--------------------
            
            // размещаем динамически на форме
            for( int i = 0; i < arrStoreSign.Count; i++ )
            {
                FormulaEvalNDS ev = ( FormulaEvalNDS ) arrStoreSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
					 CheckBoxVar chBV;
                ctlLabelTextbox usTB;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Analog:
                             usTB = new ctlLabelTextbox(ev);
							 usTB.lblCaption.Text = "";
							 usTB.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 usTB.AutoSize = true;

							 Binding bnd = new Binding("Text",ev.LinkVariableNewDS,"ValueAsString", true);
							 bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
							 usTB.txtLabelText.DataBindings.Add(bnd);
							 ev.LinkVariableNewDS.BindindTag = bnd;
							 usTB.Caption_Text = ev.CaptionFE;
							 usTB.Dim_Text = ev.Dim;
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar(ev);
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;
							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
							bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.checkBox1.Text = ev.CaptionFE;
							 break;
						 case TypeOfTag.no:
							 break;
						 default:
							 MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
							 break;
					 }					 
            }
        }
        private void tabStore_Leave(object sender, EventArgs e)
        {
            if (slTagListByTabPages.ContainsKey("arrStoreSign"))
                // отписываемся от тегов
                HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(slTagListByTabPages["arrStoreSign"]);
        }
        #endregion        

        #region Вкладка "Конфигурации и уставки"
        /// <summary>
        /// private void tbcConfig_Enter( object sender, EventArgs e )
        ///  вход на вкладку "Конфигурации и уставки"
        /// </summary>
        private void tbcConfig_Enter( object sender, EventArgs e )
        {
           // устанавливаем пикеры для изменения уставок за последние сутки
           dtpEndDateConfig.Value = DateTime.Now;
           dtpEndTimeConfig.Value = DateTime.Now;
           dtpStartDateConfig.Value = DateTime.Now;
           
           TimeSpan ts = new TimeSpan( 1, 0, 0, 0 );
           dtpStartDateConfig.Value = dtpStartDateConfig.Value - ts;
           dtpStartTimeConfig.Value = DateTime.Now;

            // скрываем/показываем нужную панель
            foreach( Panel p in arDopPanel )
                p.Visible = false;
            pnlConfig.Visible = true;

            pnlConfig.Left = tabControl1.Left + 5;

            pnlConfig.Left = tabControl1.Left + 5;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlConfig.Height;
            pnlConfig.Parent = splitContainer1.Panel2;
            pnlConfig.Dock = DockStyle.Fill;

            //pnlConfig.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlConfig.Width = tabControl1.Width;

            //-------------------------------------------------------------------
            //готовим инф. для отображения аналоговых и дискретных сигналов
            if( arrConfigSign.Count != 0 )
                return;

           lstvConfig.Items.Clear();
				
            UstavBD();

				 btnWriteUst.Enabled = false;
            CreateArrayList(arrConfigSign, "arrConfigSign");

            // для начала скрываем все tabpage
            for( int i = 0; i < tbkConfig.Controls.Count; i++ )
            {
                if( tbkConfig.Controls[i] is TabPage )
                {
                    tbkConfig.Controls.RemoveAt( i );
                    i--;
                }
            }

            // корректируем названия вкладок
            for( int i = 0; i < sl_tpnameUst.Count; i++ )
                tbkConfig.Controls.Add((Control) sl_tpnameUst.GetByIndex( i ) );

            // размещаем динамически на форме
            for( int i = 0; i < arrConfigSign.Count; i++ )
            {
                //FormulaEval ev = ( FormulaEval ) arrConfigSign[i];
                FormulaEvalNDS ev = (FormulaEvalNDS)arrConfigSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
                ComboBoxVar cBV;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Combo:
                             //       cBV = new ComboBoxVar((string[])( ( TagEval ) ( ( TagVal ) ev.arrTagVal[0] ).linkTagEval ).arrStrCB.ToArray(typeof(string)), 0);
                             //       cBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
                             //cBV.AutoSize = true;
                             //cBV.addrLinkVar = ev.addrVar;
                             //cBV.typetag = ev.tRezFormulaEval.TypeTag;
                             //ev.OnChangeValForm += cBV.LinkSetText;
                             //ev.FirstValue();

                             //cBV = new ComboBoxVar((string[])( ( TagEval ) ( ( TagVal ) ev.arrTagVal[0] ).linkTagEval ).arrStrCB.ToArray(typeof(string)), 0);
                             cBV = new ComboBoxVar(ev.LinkVariableNewDS.SlEnumsParty , 0);
                             cBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
                             cBV.AutoSize = true;
                             cBV.TypeView = TypeViewValue.Textbox;//.Combobox;

                             //Binding bndcb = new Binding("SelectedText", ev.LinkVariableNewDS, "ValueAsString", true);
                             ////Binding bndcb = new Binding("SelectedIndex", ev.LinkVariableNewDS., "ValueAsMemX", true);
                             //bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                             //cBV.cbVar.DataBindings.Add(bndcb);
                             //ev.LinkVariableNewDS.BindindTag = bndcb;
                             //cBV.lblCaption.Text = ev.CaptionFE;

                             Binding bndcb = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                             bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                             cBV.tbText.DataBindings.Add(bndcb);
                             ev.LinkVariableNewDS.BindindTag = bndcb;
                             cBV.lblCaption.Text = ev.CaptionFE;
                             break;

						 case TypeOfTag.Analog:
							 usTB = new ctlLabelTextbox(ev);
							 usTB.LabelText = "";
							 usTB.Parent = (FlowLayoutPanel) slFLP[ev.ToP];
							 usTB.AutoSize = true;

							 Binding bnd = new Binding("Text",ev.LinkVariableNewDS,"ValueAsString", true);
							 bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
							 usTB.txtLabelText.DataBindings.Add(bnd);
							 ev.LinkVariableNewDS.BindindTag = bnd;
							 usTB.Caption_Text = ev.CaptionFE;
							 usTB.Dim_Text = ev.Dim;
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar(ev);
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;

							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
							bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.checkBox1.Text = ev.CaptionFE;
							 break;
						 default:
							 MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
							 break;
					 }
            }
        }
        private void UstavBD( )
        {
            DataBaseReq dbs = new DataBaseReq(HMI_Settings.cstr, "ShowDataLog2");

            // входные параметры
            // 1. ip FC
            dbs.AddCMDParams(new DataBaseParameter("@IP", ParameterDirection.Input, SqlDbType.BigInt,0));
            // 2. id устройства
            dbs.AddCMDParams(new DataBaseParameter("@id",ParameterDirection.Input,SqlDbType.Int,iFC * 256 + iIDDev));
            // 3. начальное время
            TimeSpan tss = new TimeSpan(0, dtpStartDateConfig.Value.Hour - dtpStartTimeConfig.Value.Hour, dtpStartDateConfig.Value.Minute - dtpStartTimeConfig.Value.Minute, dtpStartDateConfig.Value.Second - dtpStartTimeConfig.Value.Second);
            DateTime tim = dtpStartDateConfig.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_start",ParameterDirection.Input,SqlDbType.DateTime,tim));
            // 2. конечное время
            tss = new TimeSpan(0, dtpEndDateConfig.Value.Hour - dtpEndTimeConfig.Value.Hour, dtpEndDateConfig.Value.Minute - dtpEndTimeConfig.Value.Minute, dtpEndDateConfig.Value.Second - dtpEndTimeConfig.Value.Second);
            tim = dtpEndDateConfig.Value - tss;
            dbs.AddCMDParams(new DataBaseParameter("@dt_end", ParameterDirection.Input, SqlDbType.DateTime, tim));
            // 5. тип записи
            // информация по уставкам
            int tbd = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Ustavki, iFC, iIDDev);
            dbs.AddCMDParams(new DataBaseParameter("@type", ParameterDirection.Input, SqlDbType.Int, tbd));
            // 6. ид записи журнала
            dbs.AddCMDParams(new DataBaseParameter("@id_record", ParameterDirection.Input, SqlDbType.Int, 0));

            dbs.DoStoredProcedure();

            // извлекаем данные по уставкам
            dtU = dbs.GetTableAsResultCMD();

            if (dtU.Rows.Count == 0 && IsMesView)
            {
                MessageBox.Show("Архивных данных по уставкам для этого устройства нет.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                IsMesView = false;
            }

            // заполняем ListView
            lstvConfig.Items.Clear();
            for (int curRow = 0; curRow < dtU.Rows.Count; curRow++)
            {
                //DateTime t = (DateTime)dtU.Rows[curRow]["TimeBlock"];
                //ListViewItem li = new ListViewItem(t.ToShortDateString());
                //li.SubItems.Add(t.ToLongTimeString() + ":" + t.Millisecond);
                //li.Tag = dtU.Rows[curRow]["ID"];
                //lstvConfig.Items.Add(li);

                DateTime t = (DateTime)dtU.Rows[curRow]["TimeBlock"];
                ListViewItem li = new ListViewItem(CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t));
                //li.SubItems.Add(t.ToLongTimeString() + ":" + t.Millisecond);
                li.Tag = dtU.Rows[curRow]["ID"];
                lstvConfig.Items.Add(li);
            }
        }

        private void btnReadUstFC_Click(object sender, EventArgs e)
        {
            btnWriteUst.Enabled = true;

            // правильная запись в журнал действий пользователя
            // номер устройства с цчетом фк
            int numdevfc = iFC * 256 + iIDDev;
            CommonUtils.CommonUtils.WriteEventToLog(7, numdevfc.ToString(), true);

            ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "IMP", new byte[] { });

            //if (parent.newKB.ExecuteCommand(iFC, iIDDev, "IMP", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
            //    parent.WriteEventToLog(35, "Команда \"IMP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false);

            tcUstConfigBottomPanel.SelectTab(0);
        }

        #region вывод информации при выборе конкретной записи по уставкам
        private void lstvConfig_ItemActivate(object sender, EventArgs e)
        {
            if (lstvConfig.SelectedItems.Count == 0)
                return;

            int id_block = (int)lstvConfig.SelectedItems[0].Tag;

            // номер устройства с цчетом фк
            int numdevfc = iFC * 256 + iIDDev;
            ArrayList arparam = new ArrayList();
            // номер арх записи в бд
            arparam.Add(id_block);
            // строка подключения
            byte[] str_cnt_in_bytes = Encoding.UTF8.GetBytes(HMI_Settings.cstr);
            arparam.Add(str_cnt_in_bytes);

            IRequestData req = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetData(0, (uint)numdevfc, "ArhivUstavkiBlockData", arparam, numdevfc);

            //byte[] adata = DataBaseReq.GetBlockData(HMI_Settings.cstr, (int)lstvConfig.SelectedItems[0].Tag);

           // вызываем процедуру разбора пакета из базы
           // SetArhivGroupInDev(iIDDev, 14);
           // ParseBDPacket(adata, 62000, iIDDev);

           btnWriteUst.Enabled = true;
        }
        #endregion

        private void btnReNewUstBD_Click( object sender, EventArgs e )
      {
        IsMesView = true;
         UstavBD( );
         tcUstConfigBottomPanel.SelectTab( 0 );
      }
      /// <summary>
		/// private void btnWriteUst_Click( object sender, EventArgs e )
		/// запись уставок
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>  
		private void btnWriteUst_Click( object sender, EventArgs e )
        {
            MessageBox.Show("Выполнение действия запрещено");

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
            //foreach (DataSource aFC in parent.KB)
            ////foreach( FC aFC in parent.KB )
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
            //    //System.Buffer.BlockCopy( ( byte[] ) slLocal[62000] , 6, memX, 0, (( byte[] ) slLocal[62000] ).Length - 6 );
            //    System.Buffer.BlockCopy( ( byte[] ) slLocal[62000], 6, memX, 0, lenpack - 6 );

            ////memDevBlock.Read( memX, 0, lenpack - 6 );

            //for( int i = 0; i < tbkConfig.Controls.Count; i++ )
            //{
            //    if( tbkConfig.Controls[i] is TabPage )
            //    {
            //        tp = ( TabPage ) tbkConfig.Controls[i];
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
            //                           if ( !CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 ) )
            //                              return;
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
            //            if ( !CommonUtils.CommonUtils.StrToBCD_Field( ultb, memX, 62000 ) )
            //               return;
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

            //if( parent.newKB.ExecuteCommand( iFC, iIDDev, "WCP", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent ) )
            //   parent.WriteEventToLog(35, "Команда \"WCP\" ушла в сеть. Устройство - " + iIDDev.ToString(), this.Name, true);//, true, false );
            //    // документирование действия пользователя
            //parent.WriteEventToLog(6, iIDDev.ToString(), this.Name, true);//, true, false );			//"выдана команда WCP - запись уставок."
            //isUstChange = false;
        }		

		private void btnResetValues_Click( object sender, EventArgs e )
		{
            //MessageBox.Show("Выполнение действия запрещено");
            btnWriteUst.Enabled = false;
            IDevice thisDev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device(0, (uint)(iFC * 256 + iIDDev));
            foreach ( ITag tag in thisDev.GetRtuTags() )
                tag.SetDefaultValue();
            //parent.newKB.ResetGroup(iFC, iIDDev, 14);
		}
        private void tbpConfUst_Leave(object sender, EventArgs e)
        {
            if (slTagListByTabPages.ContainsKey("arrConfigSign"))
                // отписываемся от тегов
                HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(slTagListByTabPages["arrConfigSign"]);
        }

        #endregion

        #region Вкладка осциллограммы и диаграммы
        private void tabPage5_Enter( object sender, EventArgs e )
        {
           if (dgvOscill.RowCount != 0 || dgvDiag.RowCount != 0)
              return;

           // устанавливаем пикеры для изменения набора осциллограмм и диаграмм за последние сутки
           dtpEndData.Value = DateTime.Now;
           dtpEndTime.Value = DateTime.Now;
           dtpStartData.Value = DateTime.Now;

           TimeSpan ts = new TimeSpan( 1, 0, 0, 0 );
           dtpStartData.Value = dtpStartData.Value - ts;
           dtpStartTime.Value = DateTime.Now;

            // скрываем/показываем нужную панель
            foreach( Panel p in arDopPanel )
                p.Visible = false;
            pnlOscDiag.Visible = true;

            pnlOscDiag.Left = tabControl1.Left + 5;

            pnlOscDiag.Left = tabControl1.Left + 5;
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlOscDiag.Height;
            pnlOscDiag.Parent = splitContainer1.Panel2;
            pnlOscDiag.Dock = DockStyle.Fill;

            //pnlOscDiag.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlOscDiag.Width = tabControl1.Width - 10;
				
			  //остальные размеры
            tabPage5.Height = tabControl1.Height;
            splitContainer_OscDiag.Height = tabPage5.Height;//tabControl1            
            dgvOscill.Height = splitContainer_OscDiag.Panel1.Height - 2 * btnUnionOsc.Height;
            dgvDiag.Height = splitContainer_OscDiag.Panel1.Height - 2 * btnUnionOsc.Height;
        }

        private void OscBD( )
        {
            if (oscdg == null)
                oscdg = new OscDiagViewer();

            oscdg.IdFC = this.iFC;
            oscdg.IdDev = this.iIDDev;
            oscdg.DTStartData = dtpStartData.Value;
            oscdg.DTStartTime = dtpStartTime.Value;
            oscdg.DTEndData = dtpEndData.Value;
            oscdg.DTEndTime = dtpEndTime.Value;
            oscdg.TypeRec = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Osc, iFC, iIDDev);            

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

        // получение диаграмм из базы
        private void DiagBD( )
        {
            if (oscdg == null)
                oscdg = new OscDiagViewer();

            oscdg.IdFC = this.iFC;
            oscdg.IdDev = this.iIDDev;
            oscdg.DTStartData = dtpStartData.Value;
            oscdg.DTStartTime = dtpStartTime.Value;
            oscdg.DTEndData = dtpEndData.Value;
            oscdg.DTEndTime = dtpEndTime.Value;
            oscdg.TypeRec = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(TypeBlockData4Req.TypeBlockData4Req_Diagramm, iFC, iIDDev);

            // извлекаем данные по осциллограммам
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
            }
        }

        void dgvDiag_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        // изменение пикеров - в сочетании с кнопкой обновить
        private void dtpStartData_ValueChanged(object sender, EventArgs e)
        {
            // выводим результаты запроса
            dgvOscill.Rows.Clear();
            //OscBD();
            //DiagBD();
        }

        ArrayList asb = new ArrayList();    // для хранения имен файлов в случае для объединения осциллограмм

        // кнопка чтение одной осциллограммы 
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

            /*
             * первый аргумент номер DS,
             * сейсчас для отработки механизма задана константа (0)
             * в дальнейшем нужно придумать механизм когда на данном этапе
             * будет известен реальный номер DS
             */
            oscdg.ShowOSCDg( 0 ,dtO, ide);
        }

        // объединяем осциллограммы
        private void button4_Click(object sender, EventArgs e)
        {
        //    if (oscdg == null)
        //       oscdg = new OscDiagViewer(parent);

        //    oscdg.ClearArrayIDE();

        //    // перечисляем записи в таблице dbO, смотрим отмеченные
        //    for (int curRow = 0; curRow < dtO.Rows.Count; curRow++)
        //       if ((bool)dgvOscill[0, curRow].Value == true)
        //          oscdg.AddIde2ArrayIde(((int)dtO.Rows[curRow]["ID"]));
            
        //   oscdg.ShowUnionOSCDg(dtO);
        //}

        //// кнопка чтение одной диаграммы
        //private void dgvDiag_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    DataGridViewCell de;
        //    char[] sep = { ' ' };
        //    string[] sp;
        //    StringBuilder sb;
        //    if (e.ColumnIndex == 0)
        //    {
        //        dgvDiag[e.ColumnIndex, e.RowIndex].Value = (bool)dgvDiag[e.ColumnIndex, e.RowIndex].Value ? false : true;
        //        btnUnionDiag.Enabled = true;
        //        return;
        //    }
        //    else if (e.ColumnIndex != 5)
        //        return;

        //    btnUnionDiag.Enabled = false;
        //    // сбрасываем все флажки
        //    for (int i = 0; i < dtG.Rows.Count; i++)
        //        dgvDiag[0, i].Value = false;
        //    try
        //    {
        //        de = dgvDiag["clmIDDiag", e.RowIndex];
        //    }
        //    catch
        //    {
        //        MessageBox.Show("dgvDiag_CellContentClick - исключение");
        //        return;
        //    }
        //    int ide = (int)de.Value;

        //    oscdg.ShowOSCDg(dtG, ide);
        }

        // объединяем диаграммы
        private void btnUnionDiag_Click(object sender, EventArgs e)
        {
            //if (oscdg == null)
            //   oscdg = new OscDiagViewer(parent);

            //oscdg.ClearArrayIDE();

            //// перечисляем записи в таблице dbO, смотрим отмеченные
            //for (int curRow = 0; curRow < dtG.Rows.Count; curRow++)
            //   if ((bool)dgvDiag[0, curRow].Value == true)
            //      oscdg.AddIde2ArrayIde(((int)dtG.Rows[curRow]["ID"]));

            //oscdg.ShowUnionOSCDg(dtG);
        }

        // кнопка Обновить
        private void btnReNewOD_Click( object sender, EventArgs e )
        {
            DiagBD();
            OscBD();                                  
        }
        #endregion

        #region вход на вкладку с системной информацией
        private void tabSystem_Enter( object sender, EventArgs e )
        {
            // скрываем/показываем нужную панель
            foreach( Panel p in arDopPanel )
                p.Visible = false;
            pnlSystem.Visible = true;

            pnlSystem.Left = tabControl1.Left + 5;

            splitContainer1.Panel2Collapsed = false;
            splitContainer1.SplitterDistance = splitContainer1.Height - pnlSystem.Height;
            pnlSystem.Parent = splitContainer1.Panel2;
            pnlSystem.Dock = DockStyle.Fill;

            //pnlSystem.Top = tabControl1.Top + tabControl1.Height + 5;
            //pnlSystem.Width = tabControl1.Width;

            gbVizov.Width = tabSystem.Width / 2 - 8;
            gbTest.Width = tabSystem.Width / 2 - 8;

            if( arrSystemSign.Count != 0 )
                return;
            //-------------------------------------------------------------------
             CreateArrayList(arrSystemSign, "arrSystemSign");

            // размещаем динамически на форме
            for( int i = 0; i < arrSystemSign.Count; i++ )
            {
                FormulaEvalNDS ev = (FormulaEvalNDS)arrSystemSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Analog:
                             usTB = new ctlLabelTextbox(ev);
							 usTB.LabelText = "";
							 usTB.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 usTB.AutoSize = true;
							 Binding bnd = new Binding("Text",ev.LinkVariableNewDS,"ValueAsString", true);
							 bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
							 usTB.txtLabelText.DataBindings.Add(bnd);
							 ev.LinkVariableNewDS.BindindTag = bnd;
							 usTB.Caption_Text = ev.CaptionFE;
							 usTB.Dim_Text = ev.Dim;
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar(ev);
							 chBV.checkBox1.Text = "";
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;
							 Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
							bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
							chBV.checkBox1.DataBindings.Add(bndCB);
							ev.LinkVariableNewDS.BindindTag = bndCB;
							chBV.checkBox1.Text = ev.CaptionFE;
							 break;
						 case TypeOfTag.no:
							 break;
						 default:
							 MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
							 break;
					 }					 
            }
        }
        private void tabSystem_Leave(object sender, EventArgs e)
        {
            if (slTagListByTabPages.ContainsKey("arrSystemSign"))
                // отписываемся от тегов
                HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(slTagListByTabPages["arrSystemSign"]);
        }
        #endregion

        //для потокобезопасного вызова процедуры
        delegate void SetLVCallback( ListViewItem li, bool actDellstV );

        // actDellstV - действия с ListView : false - не трогать; true - очистить;
        public void LinkSetLV( object Value, bool actDellstV )
        {
        }

        //для потокобезопасного вызова процедуры
        private void SetLV( ListViewItem li, bool actDellstV )
        {
        }

        private void button1_Click( object sender, EventArgs e )
        {
            ReNew(); 
        }

        private void ReNew( ) 
        {
        }

        private void btnPrint_Click( object sender, EventArgs e )
        {
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
            //parent.mnuPrint_Click( sender, e );
        }

        /// <summary>
        /// PrintArr()
        ///     Печать массива переменных
        /// </summary>
        private void PrintArr()
        {
            //   StringBuilder sb = new StringBuilder();
            //   float f_val;
            //   int i_val;
            //   string t_val = "";
            //   ArrayList arCurPrt = new ArrayList();

            //   object val;

            //   // определяем активную вкладку
            //   TabPage tp_sel = tabControl1.SelectedTab;

            //   sb.Length = 0;

            //   switch (tp_sel.Text)
            //   {
            //      case "Текущая информация":
            //         // формируем заголовок листинга
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (Текущая информация)");
            //         sb.Append("\n========================================================================\n");
            //         sb.Append(" \n \n ");
            //         arCurPrt = arrCurSign;
            //         break;
            //      case "Информация об авариях":
            //         // формируем заголовок листинга
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (Информация об авариях)");
            //         sb.Append("\n========================================================================\n");

            //         sb.Append(" \n \n ");
            //         arCurPrt = arrAvarSign;
            //         break;
            //      case "Накопительная информация":
            //         // формируем заголовок листинга
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (Накопительная информация)");
            //         sb.Append("\n========================================================================\n");

            //         sb.Append(" \n \n ");
            //         arCurPrt = arrStoreSign;
            //         break;
            //      case "Конфигурация и уставки":
            //         // формируем заголовок листинга
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (Конфигурация и уставки)");
            //         sb.Append("\n========================================================================\n");

            //         sb.Append(" \n \n ");
            //         arCurPrt = arrConfigSign;
            //         break;
            //      case "Система":
            //         // формируем заголовок листинга
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (Система)");
            //         sb.Append("\n========================================================================\n");

            //         sb.Append(" \n \n ");
            //         arCurPrt = arrSystemSign;
            //         break;
            //      case "Состояние устройства и команд":
            //         // формируем заголовок листинга
            //         sb.Append("========================================================================\n");
            //         sb.Append(this.Text + " (Состояние устройства и команд)");
            //         sb.Append("\n========================================================================\n");
            //         sb.Append(" \n \n ");
            //         arCurPrt = arrStatusDevCommand;
            //         break;
            //      default:
            //         break;
            //   }

            //   //MessageBox.Show(arCurPrt.Count.ToString());

            //   string strm = String.Empty;

            //   for (int i = 0; i < arCurPrt.Count; i++)
            //      try
            //      {

            //          FormulaEvalNDS ev = (FormulaEvalNDS)arCurPrt[i];

            //         switch (ev.ToT)
            //         {
            //            case TypeOfTag.Analog:
            //               val = ev.tRezFormulaEval.Value;

            //               if (val is float)
            //               {
            //                  f_val = (float)ev.tRezFormulaEval.Value;
            //                  t_val = f_val.ToString("F2"); // две цифры после запятой
            //               }
            //               else if (val is short)
            //               {
            //                  i_val = (Int16)ev.tRezFormulaEval.Value;
            //                  t_val = i_val.ToString();
            //               }
            //               else if (val is int)
            //               {
            //                  i_val = (Int32)ev.tRezFormulaEval.Value;
            //                  t_val = i_val.ToString();
            //               }
            //               else if (val is string)
            //               {
            //                  t_val = (string)ev.tRezFormulaEval.Value;
            //               }

            //               //sb.Append(ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE);
            //               strm = ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE;
            //               break;
            //            case TypeOfTag.Discret:
            //               //sb.Append(ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE);
            //               strm = ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE;
            //               break;
            //            case TypeOfTag.Combo:
            //               //sb.Append(ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE);
            //               strm = ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE;
            //               break;
            //            default:
            //               continue;
            //         }

            //         string news = String.Empty;
            //         for (int ii = 0; ii < strm.Length; ii++)
            //         {
            //            if (strm[ii] != '\0')
            //               news += strm[ii];
            //         }
            //         sb.Append(news);
            //         sb.Append(" \n ");
            //      }
            //      catch (Exception ex)
            //      {
            //         MessageBox.Show(ex.Message);
            //      }
            //   parent.prt.rtbText.AppendText(sb.ToString());
            //}

            //#region формирование строки для печати для отдельной переменной - старый код - закомментировано
            ///// <summary>
            ///// sbForSimpleVar(StringBuilder sb)
            /////     формирование строки для печати для отдельной переменной
            ///// </summary>
            ////private void sbForSimpleVar(StringBuilder sb, FormulaEval b_xxx)
            ////{
            ////    float f_val;
            ////    int i_val;
            ////    string t_val = "";
            ////    FormulaEval ev = ( FormulaEval ) b_xxx;
            ////    object val;

            ////    if( b_xxx == null )
            ////        return;

            ////    switch( ev.ToT )
            ////    {
            ////        case TypeOfTag.no:
            ////            val = ev.tRezFormulaEval.Value;

            ////            if( val is float )
            ////            {
            ////                f_val = ( float ) ev.tRezFormulaEval.Value;
            ////                t_val = f_val.ToString( "F2" ); // две цифры после запятой
            ////            }
            ////            else if( val is short )
            ////            {
            ////                i_val = ( Int16 ) ev.tRezFormulaEval.Value;
            ////                t_val = i_val.ToString();
            ////            }
            ////            else if( val is ushort )
            ////            {
            ////                i_val = ( UInt16 ) ev.tRezFormulaEval.Value;
            ////                t_val = i_val.ToString();
            ////            }
            ////            else if( val is int )
            ////            {
            ////                i_val = ( Int32 ) ev.tRezFormulaEval.Value;
            ////                t_val = i_val.ToString();
            ////            }
            ////            else if( val is string )
            ////            {
            ////                t_val = ( string ) ev.tRezFormulaEval.Value;
            ////            }

            ////            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
            ////            break;
            ////        case TypeOfTag.Analog:
            ////            val = ev.tRezFormulaEval.Value;

            ////            if( val is float )
            ////            {
            ////                f_val = ( float ) ev.tRezFormulaEval.Value;
            ////                t_val = f_val.ToString( "F2" ); // две цифры после запятой
            ////            }
            ////            else if( val is short )
            ////            {
            ////                i_val = ( Int16 ) ev.tRezFormulaEval.Value;
            ////                t_val = i_val.ToString();
            ////            }
            ////            else if( val is int )
            ////            {
            ////                i_val = ( Int32 ) ev.tRezFormulaEval.Value;
            ////                t_val = i_val.ToString();
            ////            }
            ////            else if( val is string )
            ////            {
            ////                t_val = ( string ) ev.tRezFormulaEval.Value;
            ////            }

            ////            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + t_val + " \t " + ev.tRezFormulaEval.DimIE );
            ////            break;
            ////        case TypeOfTag.Discret:
            ////            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
            ////            break;
            ////        case TypeOfTag.Combo:
            ////            sb.Append( ev.tRezFormulaEval.CaptionIE + " \t = " + ev.tRezFormulaEval.Value.ToString() + " \t " + ev.tRezFormulaEval.DimIE );
            ////            break;
            ////        default:
            ////            break;
            ////    }
            ////    sb.Append( " \n " );
            ////}		 
            //#endregion
        }
    }
}
        