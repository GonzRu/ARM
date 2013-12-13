/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс содержащий логику заполнения формы
 *                                                                             
 *	Файл                     : X:\Projects\33_Virica\Server_new_Interface\crza\CRZADevices\CRZADevices\CRZADeviceEv.cs                                         
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 20.12.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InterfaceLibrary;
using System.Xml.Linq;
using System.IO;
using Calculator;
using LabelTextbox;
using DebugStatisticLibrary;

namespace DeviceFormLib
{
    public class frmEngine
    {
        //public event ChangeConfigUst OnChangeConfigUst;

        /// <summary>
        /// Тексе в заголовок формы
        /// </summary>
        public string CaptionForm
        {
            get { return captionForm;}
        }
        string captionForm = string.Empty;
        /// <summary>
        /// устройство для этой формы
        /// </summary>
        public IDevice Dev
        {
            get{return dev;}
        }
        IDevice dev = null;

        /// <summary>
        /// уник номер DS
        /// </summary>        
        public uint UniDs { get; private set; }
        /// <summary>
        /// уник номер устр
        /// </summary>
        public uint UniDev { get; private set; }


        /// <summary>
        /// файл с описанием формы
        /// </summary>
        string strnamef = string.Empty;
        /// <summary>
        /// xml-документ для описания формы
        /// </summary>
        public XDocument xdocFrm4Device;
        ///// <summary>
        ///// общий список тегов для подписки/отписки
        ///// </summary>
        //List<ITag> taglist;
        /// <summary>
        /// список тегов аварии для подписки/отписки
        /// </summary>
        List<ITag> taglistAvarSrabat;
        /// <summary>
        /// список тегов аварии для подписки/отписки
        /// </summary>
        List<ITag> taglistAvarPusk;
        /// <summary>
        /// сортированный список flp с именами и условными обозначениями, т.е. пара (Name, Caption)
        /// </summary>
        SortedList slnflps = new SortedList();
        /// <summary>
        /// сортированный список для связи flp с определенынм именем т.е. пара (Caption, mtraflp)
        /// </summary>
        readonly SortedList<string, MTRANamedFLPanel> slFlpByName = new SortedList<string, MTRANamedFLPanel>();
        /// <summary>
        /// для хранения инф о FlowLayoutPanel, т.е. пара (flp.Name, ссылка на FLP)
        /// </summary>
        readonly SortedList slFlp = new SortedList();
        /// <summary>
        /// список TabPages для возможного размещения 
        /// иерархии подгрупп некоторой головной группы, 
        /// например уставки, аварии и т.п.
        /// </summary>
        readonly SortedList<string, TabPage> slTabPagesByName = new SortedList<string, TabPage>();
        /// <summary>
        /// тип блока
        /// </summary>
        string typeName;
        /// <summary>
        /// счетчик FlowLayoutPanel
        /// для формирования уник имени
        /// </summary>
        int counflp;
        /// <summary>
        /// ссылка на панель
        /// на кот может располагаться tabcontrol
        /// при автоматич формир вкладок
        /// </summary>
        Panel PnlMain;
        Form parent;
        /// <summary>
        /// ссылка на TabPage c уставками
        /// </summary>
        TabPage tpConfig;

        public frmEngine (UInt32 unids, UInt32 unidev, Form parentfrm)
        {
            UniDs = unids;
            UniDev = unidev;
            parent = parentfrm;

			try
			{
                dev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device(unids, unidev);

                if ( dev == null )
                    throw new Exception(
                        string.Format(
                            @"(87) : X:\Projects\01_HMIWinFormsClient\DeviceFormLib\frmEngine.cs : frmEngine() : Нет связанного устройства с данной формой unids ={0}; unidev = {1}",
                            unids.ToString(), unidev.ToString() ) );
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }
        /// <summary>
        /// инициализироватть форму устройства
        /// </summary>
        public void InitFrm(Form form, TabControl tabControl, Panel pnlmain)
        {
            try
            {
                PnlMain = pnlmain;

                GetCCforFLP(form.Controls);

                foreach (TabPage tp in tabControl.TabPages)
                    slTabPagesByName.Add(tp.Text, tp);

                /*
                 * вычислим название файла с описанием устройства
                 * для этого используем файл PrgDevCFG.cdp источника
                 */
                XElement xedev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription( 0, "MOA_ECU", (int)this.UniDev );

                //XElement xedev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription((int)uniDs, "MOA_ECU", string.Format("{0}.{1}", uniDs,uniDev));

                if (xedev == null)
                    throw new Exception(string.Format("(157) : frm4Device.cs : InitFrm() : Попытка открыть форму для несуществующего устройства  = {0}", string.Format("{0}.{1}", this.UniDs, this.UniDev)));

                typeName = xedev.Attribute("TypeName").Value;

                xedev = xedev.Element("DescDev");   // подправили

                #region открываем файл формы
                strnamef = Path.GetDirectoryName( HMI_MT_Settings.HMI_Settings.PathToConfigurationFile ) +
                           "\\Configuration\\0#DataServer\\Devices\\" + this.UniDev.ToString() + '@' + typeName + ".cfg";

                if (!File.Exists(strnamef))
                    throw new Exception(string.Format(@"(161) : X:\Projects\01_HMIWinFormsClient\DeviceFormLib\frmEngine.cs : frmEngine() : Несуществующий файл описания формы = {0}", strnamef));

                this.xdocFrm4Device = XDocument.Load( strnamef );
                #endregion

                if (slnflps.Count == 0)
                    FormTabPage();
                else
                    PlaceTagsOnFlps();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// инициализироватть форму устройства
        /// без размещения виз компонент для тегов
        /// </summary>
        public void InitFrmLight(Form form, TabControl tabControl, Panel pnlmain)
        {
            try
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Запуск отложенной инициализации панелей." );
                PnlMain = pnlmain;

                GetCCforFLP(form.Controls);

                foreach (TabPage tp in tabControl.TabPages)
                    slTabPagesByName.Add(tp.Text, tp);

                XElement xedev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription(0, "MOA_ECU", (int)this.UniDev);

                //XElement xedev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription((int)uniDs, "MOA_ECU", string.Format("{0}.{1}", uniDs,uniDev));

                if (xedev == null)
                    throw new Exception(string.Format("(157) : frm4Device.cs : InitFrm() : Попытка открыть форму для несуществующего устройства  = {0}", string.Format("{0}.{1}", this.UniDs, this.UniDev)));

                typeName = xedev.Attribute("TypeName").Value;

                xedev = xedev.Element("DescDev");   // подправили

                #region открываем файл формы
                strnamef = Path.GetDirectoryName( HMI_MT_Settings.HMI_Settings.PathToConfigurationFile ) +
                           "\\Configuration\\0#DataServer\\Devices\\" + this.UniDev.ToString() + '@' + typeName + ".cfg";

                if (!File.Exists(strnamef))
                    throw new Exception(string.Format(@"(161) : X:\Projects\01_HMIWinFormsClient\DeviceFormLib\frmEngine.cs : frmEngine() : Несуществующий файл описания формы = {0}", strnamef));

                this.xdocFrm4Device = XDocument.Load(strnamef);
                #endregion

                DebugStatistics.WindowStatistics.AddStatistic( "Запуск отложенной инициализации панелей заверен." );
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// Разместить теги для конкретной группы 
        /// верхнего уровня
        /// </summary>
        public void PlaceTagsOnTPFlps( string groupname)
        {
            DebugStatistics.WindowStatistics.AddStatistic( "Постраение панели." );

            try
            {
                /*
                 * в файле формы анализируем группы,
                 * размещаем теги
                 */

                IEnumerable<XElement> xe_Groups = this.xdocFrm4Device.Element( "Device" ).Element( "Groups" ).Elements( "Group" );

                foreach (XElement xe_tabpage in xe_Groups)
                {
                    if (xe_tabpage.Attribute("enable").Value.ToLower() == "false")
                        continue;

                    if (xe_tabpage.Attribute("Name").Value.ToLower() != groupname.ToLower())
                        continue;

                    if ((from xa in xe_tabpage.Attributes() where xa.Name == "TypeOfPanel" select xa).Count() != 0)
                    {
                        if (xe_tabpage.Attributes("TypeOfPanel").Count() != 0)
                            if (slFlpByName.ContainsKey(xe_tabpage.Attribute("TypeOfPanel").Value))
                                CreateTagsInFlp(xe_tabpage);
                    }
                    else if (slTabPagesByName.ContainsKey(xe_tabpage.Attribute("Name").Value))
                    {
                        IEnumerable<XElement> xe_SubGrs = from xe in xe_tabpage.Elements() where xe.Name == "Group" select xe;

                        if (xe_SubGrs.Count() != 0)
                            CreateSubgroup(slTabPagesByName[xe_tabpage.Attribute("Name").Value], xe_SubGrs);
                        continue;
                    }

                    IEnumerable<XElement> xe_SubGroups = from xe in xe_tabpage.Elements() where xe.Name == "Group" select xe;

                    if (xe_SubGroups.Count() != 0)
                        CreateSubgroup(xe_SubGroups);
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
            DebugStatistics.WindowStatistics.AddStatistic( "Постраение панели завершено." );
        }

        /// <summary>
        /// Разместить теги на предварительно подготовленной форме
        /// </summary>
        private void PlaceTagsOnFlps()
        {
            try
            {
                /*
                 * в файле формы анализируем группы,
                 * размещаем теги
                 */

                IEnumerable<XElement> xe_Groups = this.xdocFrm4Device.Element( "Device" ).Element( "Groups" ).Elements( "Group" );//this.xdocFrm4Device.Element("MTRADeviceForm").Element("frame").Element("Groups").Elements("Group");

                foreach (XElement xe_tabpage in xe_Groups)
                {
                    if (xe_tabpage.Attribute("enable").Value.ToLower() == "false")
                        continue;


                    if ((from xa in xe_tabpage.Attributes() where xa.Name == "TypeOfPanel" select xa).Count() != 0)
                    {
                        if (xe_tabpage.Attributes("TypeOfPanel").Count() != 0)
                            if (slFlpByName.ContainsKey(xe_tabpage.Attribute("TypeOfPanel").Value))
                                CreateTagsInFlp(xe_tabpage);
                    }
                    else if (slTabPagesByName.ContainsKey(xe_tabpage.Attribute("Name").Value))
                    {
                        IEnumerable<XElement> xe_SubGrs = from xe in xe_tabpage.Elements() where xe.Name == "Group" select xe;

                        if (xe_SubGrs.Count() != 0)
                            CreateSubgroup(slTabPagesByName[xe_tabpage.Attribute("Name").Value], xe_SubGrs);
                        continue;
                    }

                    IEnumerable<XElement> xe_SubGroups = from xe in xe_tabpage.Elements() where xe.Name == "Group" select xe;

                    if (xe_SubGroups.Count() != 0)
                        CreateSubgroup(xe_SubGroups);
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// сформировать систему вкладок
        /// отражающую структуру логических групп устройства
        /// </summary>
        private void FormTabPage()
        {
            TabControl tcMain = new TabControl();
            tcMain.Parent = PnlMain;
            tcMain.Dock = DockStyle.Fill;
            try
            {
                /*
                 * открываем файл, 
                 * формируем вкладки,
                 * размещаем теги
                 */

                string strnamef = Path.GetDirectoryName(HMI_MT_Settings.HMI_Settings.PathToConfigurationFile) + "\\Configuration\\0#DataServer\\Devices\\" + this.UniDev.ToString() + "@frm" + typeName + ".xml";

                if (!File.Exists(strnamef))
                    throw new Exception(string.Format("(132) : frm4Device.cs : FormTabPage() : Несуществующий файл описания формы = {0}", strnamef));

                XDocument xdoc_frm4Device = XDocument.Load(strnamef);
                IEnumerable<XElement> xe_Groups = xdoc_frm4Device.Element("MTRADeviceForm").Element("frame").Element("Groups").Elements("Group");

                foreach (XElement xe_tabpage in xe_Groups)
                {
                    if (xe_tabpage.Attribute("enable").Value.ToLower() == "false")
                        continue;

                    TabPage tp = new TabPage(xe_tabpage.Attribute("Name").Value);
                    tcMain.TabPages.Add(tp);

                    IEnumerable<XElement> xe_SubGroups = from xe in xe_tabpage.Elements() where xe.Name == "Group" select xe;
                    IEnumerable<XElement> xe_Tags = from xe in xe_tabpage.Elements() where xe.Name == "TagGuid" select xe;

                    if (xe_SubGroups.Count() != 0)
                        CreateSubgroup(tp, xe_SubGroups);
                    else if (xe_Tags.Count() != 0)
                        CreateTagsInTP(tp, xe_tabpage, xe_Tags);
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// сформировать подгруппы
        /// из логической иерархии групп
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="xe_SubGroups"></param>
        public void CreateSubgroup(TabPage tp, IEnumerable<XElement> xe_SubGroups)
        {
            try
            {
                TabControl tc = new TabControl();
                tc.Parent = tp;
                tc.Dock = DockStyle.Fill;

                foreach (XElement xe_tabpage in xe_SubGroups)
                {
                    if (xe_tabpage.Attribute("enable").Value.ToLower() == "false")
                        continue;

                    TabPage tpp = new TabPage(xe_tabpage.Attribute("Name").Value);
                    tc.TabPages.Add(tpp);

                    IEnumerable<XElement> xe_SubSubGroups = from xe in xe_tabpage.Elements() where xe.Name == "Group" select xe;
                    IEnumerable<XElement> xe_Tags = from xe in xe_tabpage.Elements() where xe.Name == "TagGuid" select xe;

                    if (xe_SubSubGroups.Count() != 0)
                    {
                        CreateSubgroup(tpp, xe_SubSubGroups);
                    }
                    else if (xe_Tags.Count() != 0)
                        CreateTagsInTP(tpp, xe_tabpage, xe_Tags);

                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// сформировать подгруппы
        /// из логической иерархии групп
        /// и разместить их на предопределенной flp
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="xe_SubGroups"></param>
        private void CreateSubgroup(IEnumerable<XElement> xe_SubGroups)
        {
            try
            {
                foreach (XElement xe_tabpage in xe_SubGroups)
                {
                    if (xe_tabpage.Attribute("enable").Value.ToLower() == "false")
                        continue;

                    if (xe_tabpage.Attributes("TypeOfPanel").Count() != 0)
                        if (slFlpByName.ContainsKey(xe_tabpage.Attribute("TypeOfPanel").Value))
                            CreateTagsInFlp(xe_tabpage);

                    IEnumerable<XElement> xe_SubSubGroups = from xe in xe_tabpage.Elements() where xe.Name == "Group" select xe; //xe_tabpage.Elements("Group");

                    if (xe_SubSubGroups.Count() != 0)
                        CreateSubgroup(xe_SubSubGroups);
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// разместить теги на вкладке
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="xe_Tags"></param>
        private void CreateTagsInTP(TabPage tp, XElement xe_tabpage, IEnumerable<XElement> xe_Tags)
        {
            try
            {
                FlowLayoutPanel flp = new FlowLayoutPanel();
                counflp++;
                flp.Name = "flp" + counflp.ToString();
                flp.FlowDirection = FlowDirection.TopDown;
                this.slFlp.Add(flp.Name, flp);
                tp.BackColor = SystemColors.Control;
                flp.Parent = tp;
                flp.Dock = DockStyle.Fill;
                ArrayList arrvar = new ArrayList();

                CreateArrayList(ref arrvar, xe_tabpage, xe_Tags);

                // размещаем динамически на форме
                for (int i = 0; i < arrvar.Count; i++)
                {
                    FormulaEvalNDS ev = (FormulaEvalNDS)arrvar[i];
                    // смотрим категорию вкладки для размещения тега и его тип
                    ComboBoxVar cBV;
                    CheckBoxVar chBV;
                    ctlLabelTextbox usTB;
                    Binding bnd;
                    switch (ev.ToT)
                    {
                        case TypeOfTag.Combo:
                            cBV = new ComboBoxVar(ev.LinkVariableNewDS.SlEnumsParty, 0, ev);
                            cBV.Parent = (MTRANamedFLPanel)slFlpByName[xe_tabpage.Attribute("TypeOfPanel").Value];
                            cBV.AutoSize = true;
                            cBV.TypeView = TypeViewValue.Textbox;//.Combobox;

                            Binding bndcb = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                            cBV.tbText.DataBindings.Add(bndcb);
                            ev.LinkVariableNewDS.BindindTag = bndcb;
                            cBV.lblCaption.Text = ev.CaptionFE;
                            break;

                        case TypeOfTag.String:
                        case TypeOfTag.Analog:
                            usTB = new ctlLabelTextbox(ev);
                            usTB.LabelText = "";
                            usTB.Parent = (MTRANamedFLPanel)slFlpByName[xe_tabpage.Attribute("TypeOfPanel").Value];
                            usTB.AutoSize = true;

                            bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                            bnd.Parse += new ConvertEventHandler(usTB.bnd_Parse);
                            usTB.txtLabelText.DataBindings.Add(bnd);
                            ev.LinkVariableNewDS.BindindTag = bnd;
                            usTB.Caption_Text = ev.NameFE;//ev.CaptionFE;;
                            usTB.Dim_Text = ev.Dim;
                            break;
                        case TypeOfTag.DateTime:
                            try
                            {
                                usTB = new ctlLabelTextbox(ev);
                                usTB.LabelText = "";
                                usTB.Parent = (MTRANamedFLPanel)slFlpByName[xe_tabpage.Attribute("TypeOfPanel").Value];
                                usTB.AutoSize = true;

                                bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                                bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                                bnd.Parse += new ConvertEventHandler(usTB.bnd_Parse);
                                usTB.txtLabelText.DataBindings.Add(bnd);
                                ev.LinkVariableNewDS.BindindTag = bnd;
                                usTB.Caption_Text = ev.NameFE;//ev.CaptionFE;;
                                usTB.Dim_Text = ev.Dim;
                            }
                            catch (Exception ex)
                            {
                                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                            }
                            break;
                        case TypeOfTag.Discret:
                            chBV = new CheckBoxVar(ev);
                            chBV.checkBox1.Text = "";
                            chBV.Parent = (FlowLayoutPanel)flp;// slFLP[ev.ToP];
                            chBV.AutoSize = true;

                            Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                            bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                            chBV.checkBox1.DataBindings.Add(bndCB);
                            ev.LinkVariableNewDS.BindindTag = bndCB;
                            chBV.checkBox1.Text = ev.NameFE;//ev.CaptionFE;;
                            break;
                        case TypeOfTag.NaN:
                            break;
                        default:
                            MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// разместить теги на flp
        /// </summary>
        /// <param name="xe_tabpage"></param>
        private void CreateTagsInFlp(XElement xe_tabpage)
        {
            try
            {
                ArrayList arrvar = new ArrayList();               

                if (xe_tabpage.Elements("Tags").Count() == 0)
                    return;

                IEnumerable<XElement> xe_Tags = from xe in xe_tabpage.Element("Tags").Elements() where xe.Name == "TagGuid" select xe;

                if (xe_Tags.Count() == 0)
                    return;

                CreateArrayList(ref arrvar, xe_tabpage, xe_Tags);

                // размещаем динамически на форме
                for (int i = 0; i < arrvar.Count; i++)
                {
                    FormulaEvalNDS ev = (FormulaEvalNDS)arrvar[i];
                    // смотрим категорию вкладки для размещения тега и его тип
                    CheckBoxVar chBV;
                    ctlLabelTextbox usTB;
                    ComboBoxVar cBV;
                    switch (ev.ToT)
                    {
                        case TypeOfTag.Combo:
                            cBV = new ComboBoxVar(ev.LinkVariableNewDS.SlEnumsParty, 0,ev);
                            cBV.Parent = (MTRANamedFLPanel)slFlpByName[xe_tabpage.Attribute("TypeOfPanel").Value];
                            cBV.AutoSize = true;
                            cBV.TypeView = TypeViewValue.Textbox;//.Combobox; //текстовый режим

                            Binding bndcb = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                            cBV.tbText.DataBindings.Add(bndcb);
                            ev.LinkVariableNewDS.BindindTag = bndcb;
                            cBV.lblCaption.Text = ev.NameFE;//ev.CaptionFE;
                            //if ( !string.IsNullOrEmpty( ev.Dim ) )
                            //    cBV.lblCaption.Text += '(' + ev.Dim + ')';
                            cBV.tbText.ReadOnly = true;
                            break;

                        case TypeOfTag.String:
                        case TypeOfTag.Analog:
                            try
                            {
                                usTB = new ctlLabelTextbox(ev);
                                usTB.LabelText = "";
                                usTB.Parent = (MTRANamedFLPanel)slFlpByName[xe_tabpage.Attribute("TypeOfPanel").Value];
                                usTB.AutoSize = true;

                                Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                                bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                                bnd.Parse += new ConvertEventHandler(usTB.bnd_Parse);
                                usTB.txtLabelText.DataBindings.Add(bnd);
                                ev.LinkVariableNewDS.BindindTag = bnd;
                                usTB.Caption_Text =  ev.NameFE;//ev.CaptionFE;;
                                usTB.Dim_Text = ev.Dim;
                                usTB.txtLabelText.ReadOnly = true;
                            }
                            catch (Exception ex)
                            {
                                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                            }
                            break;
                        case TypeOfTag.DateTime:
                            try
                            {
                                usTB = new ctlLabelTextbox(ev);
                                usTB.LabelText = "";
                                usTB.Parent = (MTRANamedFLPanel)slFlpByName[xe_tabpage.Attribute("TypeOfPanel").Value];
                                usTB.AutoSize = true;

                                Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                                bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                                bnd.Parse += new ConvertEventHandler(usTB.bnd_Parse);
                                usTB.txtLabelText.DataBindings.Add(bnd);
                                ev.LinkVariableNewDS.BindindTag = bnd;
                                usTB.Caption_Text = ev.NameFE;//ev.CaptionFE;;
                                usTB.Dim_Text = ev.Dim;
                                usTB.txtLabelText.ReadOnly = true;
                            }
                            catch (Exception ex)
                            {
                                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                            }
                            break;
                        case TypeOfTag.Discret:
                            try
                            {
                                chBV = new CheckBoxVar(ev);
                                chBV.checkBox1.Text = "";
                                chBV.Parent = (MTRANamedFLPanel)slFlpByName[xe_tabpage.Attribute("TypeOfPanel").Value];
                                chBV.AutoSize = true;

                                Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                                bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                                chBV.checkBox1.DataBindings.Add(bndCB);
                                ev.LinkVariableNewDS.BindindTag = bndCB;
                                chBV.checkBox1.Text =  ev.NameFE;//ev.CaptionFE;;
                            }
                            catch (Exception ex)
                            {
                                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                            }
                            break;
                        case TypeOfTag.NaN:
                            break;
                        default:
                            MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }            
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// создать массив формул для привязки виз элементов
        /// </summary>
        /// <param name="xe_Tags"></param>
        private void CreateArrayList(ref ArrayList arrVar, XElement xe_tabpage, IEnumerable<XElement> xe_Tags)
        {
            try
            {
                SortedList sl = new SortedList();
                StringBuilder frmtgs = new StringBuilder();

                foreach (XElement xef in xe_Tags)
                {
                    if (xef.Attribute("enable").Value.ToLower() == "false")
                        continue;

                    // формируем элементы формулы
                    sl["formula"] = "0";    //xef.Attribute("express").Value;
                    
                    XElement xelem = xef.Element( "gui_variables_describe" );
                    if ( xelem != null )
                    {
                        xelem = xelem.Element( "var_title" );
                        if ( xelem != null )
                        {
                            if (string.IsNullOrEmpty( xelem.Value ))
                                continue;

                            sl["caption"] = xelem.Value; //.Attribute("Caption").Value;
                        }
                        else sl["caption"] = "Unknown";
                    }

                    xelem = xef.Element( "gui_variables_describe" );
                    if ( xelem != null )
                    {
                        xelem = xelem.Element( "UOM" );
                        if ( xelem != null )
                            sl["dim"] = xelem.Value; //xef.Attribute("Dim").Value;
                        else sl["dim"] = string.Empty;
                    }
                    // панель для размещения
                    sl["TypeOfPanel"] = "no";

                    var xe_grs = xe_tabpage.Attributes("TypeOfPanel");
                    if (xe_grs.Count() != 0)
                        sl["TypeOfPanel"] = xe_grs.First().Value;
                    xe_grs = xef.Attributes("TypeOfPanel");
                    if (xe_grs.Count() != 0)
                        sl["TypeOfPanel"] = xe_grs.First().Value;

                    TypeOfTag ToT = TypeOfTag.NaN;
                    string ToP = "";

                    // тип тега ?
                    ITag tag = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Tag(this.UniDs, this.UniDev, uint.Parse(xef.Attribute("value").Value));// xef.Attribute("TypeOfTag").Value;

                    xelem = xef.Element( "gui_variables_describe" );
                    if ( xelem != null )
                    {
                        xelem = xelem.Element( "gui_param1" );
                        if ( xelem != null && xelem.Value.Trim() == "1" )
                        {
                            if ( tag != null )
                                tag.IsInverse = true;
                        }
                        else
                            if ( tag != null )
                                tag.IsInverse = false;
                    }

                    var itagDim = tag as ITagDim;
                    if ( itagDim != null )
                    {
                        var node = xef.Element( "HMI_Format_describe" );
                        if (node != null)
                        {
                            node = node.Element( "HMIPosPoint" );
                            if ( node != null )
                            {
                                ushort tmp;
                                ushort.TryParse( node.Value, out tmp );
                                itagDim.ValueDim = tmp;
                            }
                        }
                    }

                    //if (xef.Element("gui_variables_describe").Element("gui_param1").Value.Trim() == "1")
                    //    tag.IsInverse = true;

                    if ( tag != null )
                        sl["TypeOfTag"] = tag.TypeOfTagHMI;
                    else sl["TypeOfTag"] = "fail";

                    switch ((string)sl["TypeOfTag"])
                    {
                        case "String":
                            ToT = TypeOfTag.String;
                            break;
                        case "Analog":
                            ToT = TypeOfTag.Analog;
                            break;
                        case "Discret":
                            ToT = TypeOfTag.Discret;
                            break;
                        case "Combo":
                            ToT = TypeOfTag.Combo;
                            break;
                        case "DateTime":
                            ToT = TypeOfTag.DateTime;
                            break;
                        case "No":
                            ToT = TypeOfTag.NaN;
                            break;
                        default:
                            MessageBox.Show("Нет такого типа сигнала");
                            if ( tag == null )
                            {
                                var str = string.Format( "*********** \"{0}\" GUID: {1} ***********",
                                                         sl["caption"], xef.Attribute( "value" ).Value );
                                throw new ArgumentNullException( str, "Tag is not exist" );
                            }
                            break;
                    }

                    ToP = (string)sl["TypeOfPanel"];

                    frmtgs.Length = 0;

                    frmtgs.Append( this.UniDs );
                    frmtgs.Append( "." );
                    frmtgs.Append( this.UniDev );
                    frmtgs.Append( "." );
                    frmtgs.Append( tag.TagGUID );

                    arrVar.Add(new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, frmtgs.ToString(), sl["caption"].ToString(), sl["dim"].ToString(), ToT, ToP));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// добавление новых FLP в список (для возможности последующего обращения по именам)
        /// </summary>
        /// <param Name="cc"></param>
        private void GetCCforFLP(Control.ControlCollection cc)
        {
            try
            {
                for (int i = 0; i < cc.Count; i++)
                {
                    if (cc[i] is FlowLayoutPanel)
                    {
                        FlowLayoutPanel flp = (FlowLayoutPanel)cc[i];
                        if (this.slFlp.ContainsKey(flp.Name))
                            continue;

                        this.slFlp[flp.Name] = flp;
                    }
                    else if (cc[i] is MTRANamedFLPanel)
                    {
                        MTRANamedFLPanel flp = (MTRANamedFLPanel)cc[i];
                        if (this.slFlp.ContainsKey(flp.Name))
                            continue;

                        this.slFlp[flp.Name] = flp;

                        if (slnflps.ContainsKey(flp.Name))
                            continue;

                        if (slnflps.ContainsKey(flp.Name))
                            throw new Exception(string.Format("(715) : frm4Device.cs : GetCCforFLP() : Запись с таким ключом уже существует  slnflps = {0}", flp.Name));
                        slnflps.Add(flp.Name, flp.Caption);

                        if (slFlpByName.ContainsKey(flp.Caption))
                            throw new Exception(string.Format("(719) : frm4Device.cs : GetCCforFLP() : Запись с таким ключом уже существует slFlpByName = {0}", flp.Caption));
                        slFlpByName.Add(flp.Caption, flp);
                    }
                    else
                        TestCCforFLP(cc[i]);
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        private void TestCCforFLP(Control cc)
        {
            try
            {
                for ( int i = 0; i < cc.Controls.Count; i++ )
                {
                    if ( cc.Controls[i] is MTRANamedFLPanel )
                    {
                        MTRANamedFLPanel flp = (MTRANamedFLPanel)cc.Controls[i];
                        if ( this.slFlp.ContainsKey( flp.Name ) )
                            continue;

                        this.slFlp.Add( flp.Name, flp );

                        if ( slnflps == null )
                            continue;

                        if ( slnflps.ContainsKey( flp.Name ) )
                            continue;

                        if ( slnflps.ContainsKey( flp.Name ) )
                            throw new Exception(
                                string.Format(
                                    "(715) : frm4Device.cs : GetCCforFLP() : Запись с таким ключом уже существует  slnflps = {0}",
                                    flp.Name ) );
                        slnflps.Add( flp.Name, flp.Caption );

                        if ( slFlpByName.ContainsKey( flp.Caption ) )
                            throw new Exception(
                                string.Format(
                                    "(719) : frm4Device.cs : GetCCforFLP() : Запись с таким ключом уже существует slFlpByName = {0}",
                                    flp.Caption ) );
                        slFlpByName.Add( flp.Caption, flp );
                    }
                    else if ( cc.Controls[i] is HelperControlsLibrary.OperationalControl )
                    {
                        var cntrl = (HelperControlsLibrary.OperationalControl)cc.Controls[i];
                        var flp = (MTRANamedFLPanel)cntrl.GetPanel();
                        if ( this.slFlp.ContainsKey( flp.Name ) )
                            continue;
                        this.slFlp[flp.Name] = flp;
                        slFlpByName.Add( flp.Caption, flp );
                    }
                    else if ( cc.Controls[i] is FlowLayoutPanel )
                    {
                        FlowLayoutPanel flp = (FlowLayoutPanel)cc.Controls[i];
                        if ( this.slFlp.ContainsKey( flp.Name ) )
                            continue;

                        this.slFlp[flp.Name] = flp;
                    }
                    else
                    {
                        TestCCforFLP( cc.Controls[i] );
                    }
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }

        // создать теги на вклдаке авар информации
        /// <summary>
        /// создать теги на вклдаке авар информации
        /// </summary>
        /// <param name="ngrsrabat"></param>
        /// <param name="ngrpusk"></param>
        public void CreateTP4AvarData(Int32 ngrsrabat, GroupBox gbsrabat, Int32 ngrpusk, GroupBox gbpusk, out string namesrabat)
        {
            namesrabat = string.Empty; 
            
            try
			{
                IEnumerable<XElement> xe_Groups = this.xdocFrm4Device.Element("MTRADeviceForm").Element("frame").Element("Groups").Descendants("Group");


                foreach (XElement xe_Group in xe_Groups)
                    if (ngrsrabat.ToString() == xe_Group.Attribute("GroupGUID").Value)
                    {
                        CreateAvarTagsInFlp(xe_Group, taglistAvarSrabat, "Срабатывание");
                        namesrabat = xe_Group.Attribute("Name").Value;
                    }

                foreach (XElement xe_Group in xe_Groups)
                    if (ngrpusk.ToString() == xe_Group.Attribute("GroupGUID").Value)
                    {
                        CreateAvarTagsInFlp(xe_Group, taglistAvarPusk, "Пуск");
                        namesrabat = xe_Group.Attribute("Name").Value;
                    }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }

        /// <summary>
        /// разместить теги на flp c авариями
        /// </summary>
        /// <param name="xe_tabpage"></param>
        private void CreateAvarTagsInFlp(XElement xe_tabpage, List<ITag> taglistAvar, string nameFlpSrabat)
        {
            try
            {
                ArrayList arrvar = new ArrayList();


                IEnumerable<XElement> xe_Tags = from xe in xe_tabpage.Element("Tags").Elements() where xe.Name == "TagGuid" select xe;

                if (xe_Tags.Count() == 0)
                    return;

                CreateArrayList(ref arrvar, xe_tabpage, xe_Tags);

                if (((MTRANamedFLPanel)slFlpByName[nameFlpSrabat]).Controls.Count != 0)
                    ((MTRANamedFLPanel)slFlpByName[nameFlpSrabat]).Controls.Clear();

                // размещаем динамически на форме
                for (int i = 0; i < arrvar.Count; i++)
                {
                    FormulaEvalNDS ev = (FormulaEvalNDS)arrvar[i];
                    // смотрим категорию вкладки для размещения тега и его тип
                    ComboBoxVar cBV;
                    CheckBoxVar chBV;
                    ctlLabelTextbox usTB;
                    Binding bnd;

                    switch (ev.ToT)
                    {
                        case TypeOfTag.Combo:
                            cBV = new ComboBoxVar(ev.LinkVariableNewDS.SlEnumsParty, 0, ev);
                            //cBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                            SetParent(cBV, (MTRANamedFLPanel)slFlpByName[nameFlpSrabat]);//
                            cBV.AutoSize = true;
                            cBV.TypeView = TypeViewValue.Textbox;//.Combobox;

                            Binding bndcb = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                            cBV.tbText.DataBindings.Add(bndcb);
                            ev.LinkVariableNewDS.BindindTag = bndcb;
                            cBV.lblCaption.Text = ev.CaptionFE;
                            break;

                        case TypeOfTag.String:
                        case TypeOfTag.Analog:
                            usTB = new ctlLabelTextbox(ev);
                            usTB.LabelText = "";
                            SetParent(usTB, (MTRANamedFLPanel)slFlpByName[nameFlpSrabat]);//xe_tabpage.Attribute("TypeOfPanel").Value
                            usTB.AutoSize = true;

                            bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                            bnd.Parse += new ConvertEventHandler(usTB.bnd_Parse);
                            usTB.txtLabelText.DataBindings.Add(bnd);
                            ev.LinkVariableNewDS.BindindTag = bnd;
                            usTB.Caption_Text =  ev.NameFE;//ev.CaptionFE;;
                            usTB.Dim_Text = ev.Dim;
                            break;
                        case TypeOfTag.DateTime:
                            try
                            {
                                usTB = new ctlLabelTextbox(ev);
                                usTB.LabelText = "";
                                usTB.Parent = (MTRANamedFLPanel)slFlpByName[xe_tabpage.Attribute("TypeOfPanel").Value];
                                usTB.AutoSize = true;

                                bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                                bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                                bnd.Parse += new ConvertEventHandler(usTB.bnd_Parse);
                                usTB.txtLabelText.DataBindings.Add(bnd);
                                ev.LinkVariableNewDS.BindindTag = bnd;
                                usTB.Caption_Text = ev.NameFE;//ev.CaptionFE;;
                                usTB.Dim_Text = ev.Dim;
                            }
                            catch (Exception ex)
                            {
                                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                            }
                            break;

                        case TypeOfTag.Discret:
                            chBV = new CheckBoxVar(ev);
                            chBV.checkBox1.Text = "";
                            SetParent(chBV, (MTRANamedFLPanel)slFlpByName[nameFlpSrabat]);//xe_tabpage.Attribute("TypeOfPanel").Value
                            //chBV.Parent = (MTRANamedFLPanel)slFlpByName[xe_tabpage.Attribute("TypeOfPanel").Value];
                            chBV.AutoSize = true;

                            Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                            bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                            chBV.checkBox1.DataBindings.Add(bndCB);
                            ev.LinkVariableNewDS.BindindTag = bndCB;
                            chBV.checkBox1.Text =  ev.NameFE;//ev.CaptionFE;;
                            break;
                        case TypeOfTag.NaN:
                            break;
                        default:
                            MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        delegate void SetUsTBParent(Control usTB, MTRANamedFLPanel mTRANamedFLPanel);
        private void SetParent(Control usTB, MTRANamedFLPanel mTRANamedFLPanel)//ctlLabelTextbox 
        {
            try
            {
                if (mTRANamedFLPanel.InvokeRequired)
                {
                    SetUsTBParent d = new SetUsTBParent(SetFLPParent);

                    try
                    {
                        parent.Invoke(d, new object[] { usTB, mTRANamedFLPanel });
                    }
                    catch
                    {

                    }
                }
                else
                {
                    usTB.Parent = mTRANamedFLPanel;
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        public void SetFLPParent(Control usTB, MTRANamedFLPanel mTRANamedFLPanel)
        {
            usTB.Parent = mTRANamedFLPanel;
        }
        //*****************

        /// <summary>
        /// создать теги на вклдаке
        /// </summary>
        public void CreateTPData( TabPage tab, Panel panel, string name, Boolean isClickable )
        {
            //var xeGroups = xdocFrm4Device.Element( "MTRADeviceForm" ).Element( "frame" ).Element( "Groups" ).Descendants( "Group" );
            var xeGroups = xdocFrm4Device.Element( "Device" ).Element( "Groups" ).Descendants( "Group" );

            foreach ( XElement xeGroup in xeGroups )
                if ( xeGroup.Attributes( "TypeOfPanel" ).Count() != 0 )
                    if ( name == xeGroup.Attribute( "TypeOfPanel" ).Value )
                        CreateSubgroup4Ust( tab, xeGroup.Elements( "Group" ), panel, isClickable );
        }

        // создать теги на вклдаке уставок и конфигурации
        /// <summary>
        /// сформировать панель для уставок        
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="xe_SubGroups"></param>
        private void CreateSubgroup4Ust(TabPage tp, IEnumerable<XElement> xe_SubGroups, Panel pnlprnt, bool isClickable)
        {
            try
            {
                TabControl tc = new TabControl();
                tc.Parent = pnlprnt;
                tc.Dock = DockStyle.Fill;
                tpConfig = tp; // ссылка на форму с уставками для определения изменений

                foreach (XElement xe_tabpage in xe_SubGroups)
                {
                    if (xe_tabpage.Attribute("enable").Value.ToLower() == "false")
                        continue;

                    TabPage tpp = new TabPage(xe_tabpage.Attribute("Name").Value);
                    //tpp.Leave += new EventHandler(tpp_Leave);
                    tc.TabPages.Add(tpp);

                    IEnumerable<XElement> xe_SubSubGroups = from xe in xe_tabpage.Elements() where xe.Name == "Group" select xe;
                    if (xe_SubSubGroups.Count() != 0)
                    {
                        CreateSubgroupUst(tpp, xe_SubSubGroups, isClickable);
                        continue;
                    }

                    IEnumerable<XElement> xe_Tags = null;
                    if (xe_tabpage.Elements("Tags").Count() != 0)
                        xe_Tags = from xe in xe_tabpage.Element("Tags").Elements() where xe.Name == "TagGuid" select xe;

                    if (xe_Tags == null)
                        continue;

                    if (xe_Tags.Count() != 0)
                        CreateTagsInEmptyTP(tpp, xe_tabpage, xe_Tags, UserCntrlRWAccess.ReadWrite, isClickable);
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        ///// <summary>
        ///// действия при покидании вкладки с подгруппой уставок
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void tpp_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (GetList4WriteUst().Count > 0) 
        //         if (OnChangeConfigUst != null)
        //            OnChangeConfigUst();
        //    }
        //    catch(Exception ex)
        //    {
        //        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
        //    }
        //}
        /// <summary>
        /// Проверка на факт изменения 
        /// уставок и получение списка
        /// тегов измененных уставок
        /// </summary>
        public List<ITag> GetList4WriteUst()
        {
            try
            {
                return CommonUtils.CommonUtils.GetListModifiedHMIUserControls(tpConfig);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                return new List<ITag>();
            }
        }

        /// <summary>
        /// сформировать подгруппы
        /// из логической иерархии групп
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="xe_SubGroups"></param>
        public void CreateSubgroupUst(TabPage tp, IEnumerable<XElement> xe_SubGroups, bool isClickable)
        {
            try
            {
                TabControl tc = new TabControl();
                tc.Parent = tp;
                tc.Dock = DockStyle.Fill;

                foreach (XElement xe_tabpage in xe_SubGroups)
                {
                    if (xe_tabpage.Attribute("enable").Value.ToLower() == "false")
                        continue;

                    TabPage tpp = new TabPage(xe_tabpage.Attribute("Name").Value);
                    tc.TabPages.Add(tpp);

                    IEnumerable<XElement> xe_SubSubGroups = from xe in xe_tabpage.Elements() where xe.Name == "Group" select xe;
                    if (xe_SubSubGroups.Count() != 0)
                    {
                        CreateSubgroupUst(tpp, xe_SubSubGroups, isClickable);
                        continue;
                    }

                    IEnumerable<XElement> xe_Tags = null;
                    if (xe_tabpage.Elements("Tags").Count() != 0)
                        xe_Tags = from xe in xe_tabpage.Element("Tags").Elements() where xe.Name == "TagGuid" select xe;

                    if (xe_Tags == null)
                        continue;

                    if (xe_Tags.Count() != 0)
                        CreateTagsInEmptyTP(tpp, xe_tabpage, xe_Tags, UserCntrlRWAccess.ReadWrite, isClickable);

                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// разместить теги на вкладке
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="xe_Tags"></param>
        private void CreateTagsInEmptyTP(TabPage tp, XElement xe_tabpage, IEnumerable<XElement> xe_Tags, UserCntrlRWAccess rwAccess, bool isClickable)
        {
            try
            {
                FlowLayoutPanel flp = new FlowLayoutPanel();
                counflp++;
                flp.Name = "flp" + counflp.ToString();
                flp.FlowDirection = FlowDirection.TopDown;
                this.slFlp.Add(flp.Name, flp);
                tp.BackColor = SystemColors.Control;
                flp.Parent = tp;
                flp.Dock = DockStyle.Fill;
                ArrayList arrvar = new ArrayList();

                CreateArrayList(ref arrvar, xe_tabpage, xe_Tags);

                // размещаем динамически на форме
                for (int i = 0; i < arrvar.Count; i++)
                {
                    FormulaEvalNDS ev = (FormulaEvalNDS)arrvar[i];
                    // смотрим категорию вкладки для размещения тега и его тип
                    ComboBoxVar cBV;
                    CheckBoxVar chBV;
                    ctlLabelTextbox usTB;
                    Binding bnd;

                    switch (ev.ToT)
                    {
                        case TypeOfTag.Combo:
                            cBV = new ComboBoxVar(ev.LinkVariableNewDS.SlEnumsParty, 0, ev);
                            cBV.Parent = (FlowLayoutPanel)flp;
                            cBV.AutoSize = true;

                            if ( !isClickable )
                                cBV.TypeView = TypeViewValue.Textbox; //.Combobox; //по умолчанию режим выбора
                            cBV.tbText.ReadOnly = !isClickable;

                            Binding bndcb = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                            cBV.tbText.DataBindings.Add(bndcb);
                            ev.LinkVariableNewDS.BindindTag = bndcb;
                            cBV.lblCaption.Text = ev.CaptionFE;
                            break;

                        case TypeOfTag.String:
                        case TypeOfTag.Analog:
                            usTB = new ctlLabelTextbox(ev);
                            usTB.LabelText = "";
                            usTB.Parent = (FlowLayoutPanel)flp;
                            usTB.AutoSize = true;

                            bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                            bnd.Parse += new ConvertEventHandler(usTB.bnd_Parse);
                            usTB.txtLabelText.DataBindings.Add(bnd);
                            ev.LinkVariableNewDS.BindindTag = bnd;
                            usTB.Caption_Text =  ev.NameFE;//ev.CaptionFE;;
                            usTB.Dim_Text = ev.Dim;
                            //usTB.txtLabelText.ReadOnly = (rwAccess == UserCntrlRWAccess.ReadOnly) ? true : false;
                            usTB.txtLabelText.ReadOnly = !isClickable;
                            break;
                        case TypeOfTag.DateTime:
                            try
                            {
                                usTB = new ctlLabelTextbox(ev);
                                usTB.LabelText = "";
                                usTB.Parent = (FlowLayoutPanel)flp;
                                //usTB.Parent = (MTRANamedFLPanel)slFlpByName[xe_tabpage.Attribute("TypeOfPanel").Value];
                                usTB.AutoSize = true;

                                bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                                bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                                bnd.Parse += new ConvertEventHandler(usTB.bnd_Parse);
                                usTB.txtLabelText.DataBindings.Add(bnd);
                                ev.LinkVariableNewDS.BindindTag = bnd;
                                usTB.Caption_Text = ev.NameFE;//ev.CaptionFE;;
                                usTB.Dim_Text = ev.Dim;

                                usTB.txtLabelText.ReadOnly = !isClickable;
                            }
                            catch (Exception ex)
                            {
                                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                            }
                            break;

                        case TypeOfTag.Discret:
                            chBV = new CheckBoxVar(ev);
                            chBV.IsClickable = isClickable;
                            chBV.CheckBox_Text = "";
                            chBV.Parent = (FlowLayoutPanel)flp;// slFLP[ev.ToP];
                            chBV.AutoSize = true;

                            Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                            bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                            chBV.checkBox1.DataBindings.Add(bndCB);
                            ev.LinkVariableNewDS.BindindTag = bndCB;
                            chBV.CheckBox_Text =  ev.NameFE;//ev.CaptionFE;;
                            break;
                        case TypeOfTag.NaN:
                            break;
                        default:
                            MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
             }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        //*****************

        internal ITag GetTag( string guid ) { return HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Tag( this.UniDs, this.UniDev, uint.Parse( guid ) ); }
    }

    public class FrmEngineNew
    {
        internal class TagDescription
        {
            public String Title;
            public String Uom;
            public UInt16 PosPoint;
            public ITag Source;
        }
        private enum GroupSearchIdentificator
        {
            Name,
            TypeOfPanel
        }

        readonly UInt32 uniDs, uniDev;
        readonly SortedList<string, Panel> flPanels = new SortedList<string, Panel>();
        readonly IList<Control> controls = new List<Control>();
        readonly IDevice device;

        public FrmEngineNew( UInt32 uniDs, UInt32 uniDev, Form form )
        { 
            this.uniDs = uniDs;
            this.uniDev = uniDev;

            try
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация средств расставления сигналов." );

                this.device = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device( uniDs, uniDev );
                if ( this.device == null )
                    throw new Exception(
                        string.Format( "FrmEngine: Нет связанного устройства с данной формой unids = {0}; unidev = {1}",
                                       uniDs.ToString( CultureInfo.InvariantCulture ),
                                       uniDev.ToString( CultureInfo.InvariantCulture ) ) );

                CollectPanels( form.Controls );

                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация средств расставления сигналов завершена." );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        public void InitFrm( Form form, TabControl tabControl ) { }
        public void PlaceTagsOnPanels( string groupName, bool isClickable = false )
        {
            try
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Сбор данных панели." );

                controls.Clear();
                var group = GetGroupByName( this.device.GetGroupHierarchy(), groupName );
                if ( group != null )
                {
                    CollectPanelTags( group, isClickable );
                    ParseGroups( group.SubGroupsList, isClickable );
                }

                DebugStatistics.WindowStatistics.AddStatistic( "Сбор данных панели завершен." );
            }
            catch ( Exception ex )
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Ошибка сбора данных панели." );
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }

            try
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Расстановка данных на пенель." );
                
                foreach ( var flPanel in flPanels )
                    foreach ( var control in controls )
                        if ( flPanel.Key == control.Tag.ToString() )
                            flPanel.Value.Controls.Add( control );

                DebugStatistics.WindowStatistics.AddStatistic( "Расстановка данных на пенель завершена." );
            }
            catch ( Exception ex )
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Ошибка расстановки данных на пенель." );
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        public void CreateAutoTabPage( Control control, string groupName, bool isClickable )
        {
            DebugStatistics.WindowStatistics.AddStatistic( "Постраение автоматической панели." );
            try
            {
                control.Controls.Clear();

                var group = GetGroupByName( this.device.GetGroupHierarchy(), groupName );
                if ( group != null && group.SubGroupsList.Count > 0 )
                {
                    var tabControl = new TabControl { Dock = DockStyle.Fill };
                    control.Controls.Add( tabControl );

                    foreach ( var subGroup in group.SubGroupsList )
                    {
                        var result = CreateAutoContol( subGroup, isClickable );
                        if ( result == null ) continue;
                        tabControl.TabPages.Add( (TabPage)result );
                    }
                }
            }
            catch ( Exception ex )
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Ошибка постраение автоматической панели." );
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }

            DebugStatistics.WindowStatistics.AddStatistic( "Постраение автоматической панели завершено." );
        }
        public IEnumerable<ITag> GetChangedTags(Control control)
        {
            try
            {
                return CommonUtils.CommonUtils.GetListModifiedHMIUserControls( control );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                return new List<ITag>();
            }
        }

        private void CollectPanelTags( IGroup group, bool isClickable )
        {
            var tagDescriptions = CollectTagDescriptions( group, this.uniDs, this.uniDev );
            if ( tagDescriptions == null ) return;

            foreach ( var fends in CollectFormulaEvalTags( tagDescriptions ) )
            {
                var panelControl = CreatePanelControl( fends, isClickable );
                if ( panelControl == null ) continue;

                panelControl.Tag = group.NameGroupPanel;
                controls.Add( panelControl );
            }
        }
        private void ParseGroups( IEnumerable<IGroup> groups, bool isClickable )
        {
            foreach ( var group in groups )
            {
                if (!group.IsEnable) continue;

                CollectPanelTags( group, isClickable );
                ParseGroups( group.SubGroupsList, isClickable );
            }
        }
        private void CollectPanels( Control.ControlCollection collection )
        {
            try
            {
                for ( var i = 0; i < collection.Count; i++ )
                {
                    if ( collection[i] is MTRANamedFLPanel )
                    {
                        var panel = (MTRANamedFLPanel)collection[i];
                        this.AddPanelToSortingList( panel );
                    }
                    if ( collection[i] is HelperControlsLibrary.OperationalControl )
                    {
                        var cntrl = (HelperControlsLibrary.OperationalControl)collection[i];
                        var panel = (MTRANamedFLPanel)cntrl.GetPanel();
                        this.AddPanelToSortingList( panel );
                    }
                    else
                        this.CollectPanels( collection[i].Controls );
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        private void AddPanelToSortingList( MTRANamedFLPanel panel )
        {
            if ( this.flPanels.ContainsKey( panel.Caption ) ) return;
            this.flPanels[panel.Caption] = panel;
        }
        private Control CreateAutoContol( IGroup group, bool isClickable )
        {
            if ( !group.IsEnable )
                return null;

            var tabPage = new TabPage( group.NameGroup );

            if ( group.SubGroupTagsList.Count > 0 )
            {
                var stack = SetTagsToAutoPage( group, isClickable );
                tabPage.Controls.Add( stack );
            }

            if ( group.SubGroupsList.Count > 0 )
            {
                var tabControl = new TabControl { Dock = DockStyle.Fill };
                tabPage.Controls.Add( tabControl );

                foreach ( var subGroup in group.SubGroupsList )
                {
                    var result = CreateAutoContol( subGroup, isClickable );
                    if ( result == null )
                        continue;
                    tabControl.TabPages.Add( (TabPage)result );
                }
            }
            return tabPage;
        }
        
        private FlowLayoutPanel SetTagsToAutoPage( IGroup group, bool isClickable )
        {
            var stack = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, AutoSize = true };

            var tagDescriptions = CollectTagDescriptions( group, this.uniDs, this.uniDev );
            if ( tagDescriptions != null )
            {
                foreach ( var fends in CollectFormulaEvalTags( tagDescriptions ) )
                {
                    var panelControl = CreatePanelControl( fends, isClickable );
                    if ( panelControl != null ) stack.Controls.Add( panelControl );
                }
            }

            return stack;
        }
        private static Control CreatePanelControl( FormulaEvalNDS fends, bool isClickable )
        {
            if ( string.IsNullOrEmpty( fends.NameFE ) ) return null;

            switch ( fends.ToT )
            {
                case TypeOfTag.Combo:
                    {
                        var cbv = new ComboBoxVar( fends.LinkVariableNewDS.SlEnumsParty, 0, fends )
                                      {
                                              AutoSize = true,
                                              lblCaption = { Text = fends.NameFE },
                                              TypeView = ( !isClickable ) ? TypeViewValue.Textbox : TypeViewValue.Combobox,
                                              //по умолчанию режим выбора
                                              tbText = { ReadOnly = !isClickable }
                                      };

                        var binding = new Binding( "Text", fends.LinkVariableNewDS, "ValueAsString", true );
                        binding.Format += cbv.bnd_Format;
                        cbv.tbText.DataBindings.Add( binding );
                        fends.LinkVariableNewDS.BindindTag = binding;

                        return cbv;
                    }
                case TypeOfTag.String:
                case TypeOfTag.Analog:
                    {
                        var ustb = new ctlLabelTextbox( fends )
                                       {
                                               LabelText = string.Empty,
                                               AutoSize = true,
                                               Caption_Text = fends.NameFE,
                                               Dim_Text = fends.Dim,
                                               txtLabelText = { ReadOnly = !isClickable }
                                       };

                        var binding = new Binding( "Text", fends.LinkVariableNewDS, "ValueAsString", true );
                        binding.Format += ustb.bnd_Format;
                        binding.Parse += ustb.bnd_Parse;
                        ustb.txtLabelText.DataBindings.Add( binding );
                        fends.LinkVariableNewDS.BindindTag = binding;

                        return ustb;
                    }
                case TypeOfTag.DateTime:
                    {
                        var ustb = new ctlLabelTextbox( fends )
                                       {
                                               LabelText = string.Empty,
                                               AutoSize = true,
                                               Caption_Text = fends.NameFE,
                                               Dim_Text = fends.Dim,
                                               txtLabelText = { ReadOnly = !isClickable }
                                       };

                        var binding = new Binding( "Text", fends.LinkVariableNewDS, "ValueAsString", true );
                        binding.Format += ustb.bnd_Format;
                        binding.Parse += ustb.bnd_Parse;
                        ustb.txtLabelText.DataBindings.Add( binding );
                        fends.LinkVariableNewDS.BindindTag = binding;

                        return ustb;
                    }
                case TypeOfTag.Discret:
                    {
                        var chbv = new CheckBoxVar( fends )
                                       {
                                               checkBox1 = { Text = string.Empty },
                                               AutoSize = true,
                                               CheckBox_Text = fends.NameFE,
                                               IsClickable = isClickable
                                       };

                        var binding = new Binding( "Checked", fends.LinkVariableNewDS, "ValueAsString", true );
                        binding.Format += chbv.bnd_Format;
                        chbv.checkBox1.DataBindings.Add( binding );
                        fends.LinkVariableNewDS.BindindTag = binding;

                        return chbv;
                    }
                default:
                    return null;
            }
        }
        private static IGroup GetGroup( IEnumerable<IGroup> groups, string name, GroupSearchIdentificator identificator )
        {
            foreach ( var group in groups )
            {
                switch ( identificator )
                {
                    case GroupSearchIdentificator.Name:
                        if ( group.IsEnable &&
                             group.NameGroup.ToLower( ) == name.ToLower( ) ) return group;
                        break;
                    case GroupSearchIdentificator.TypeOfPanel:
                        if ( group.IsEnable &&
                             group.NameGroupPanel.ToLower( ) == name.ToLower( ) ) return group;
                        break;
                }

                var searchGroup = GetGroup( group.SubGroupsList, name, identificator );
                if ( searchGroup != null ) return searchGroup;
            }

            return null;
        }
        private static IGroup GetGroupByName( IEnumerable<IGroup> groups, string groupName ) { return GetGroup( groups, groupName, GroupSearchIdentificator.Name ); }
        internal static IGroup GetGroupByTypeOfPanel( IEnumerable<IGroup> groups, string panelName ) { return GetGroup( groups, panelName, GroupSearchIdentificator.TypeOfPanel ); }
        internal static IEnumerable<TagDescription> CollectTagDescriptions( IGroup group, uint uniDs, uint uniDev )
        {
            var xTags = group.GroupXElement.Element( "Tags" );
            if ( xTags == null ) return null;

            var tagDescriptions = new List<TagDescription>( );
            foreach ( var xTagDescription in xTags.Elements( ) )
            {
                try
                {
                    var xmlAttribute = xTagDescription.Attribute( "enable" );
                    if ( xmlAttribute == null ||
                         xmlAttribute.Value.ToLower( ) == "false" ) continue;

                    xmlAttribute = xTagDescription.Attribute( "value" );
                    if ( xmlAttribute == null ) continue;

                    var tagDescription = new TagDescription { Source = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Tag( uniDs, uniDev, uint.Parse( xmlAttribute.Value ) ) };
                    if ( tagDescription.Source == null ) continue;

                    var xElement = xTagDescription.Element( "gui_variables_describe" );
                    if ( xElement != null )
                    {
                        var xElem = xElement.Element( "var_title" );
                        tagDescription.Title = ( xElem != null ) ? xElem.Value : tagDescription.Source.TagName;
                        xElem = xElement.Element( "UOM" );
                        tagDescription.Uom = ( xElem != null ) ? xElem.Value : tagDescription.Source.Unit;
                    }
                    else
                    {
                        tagDescription.Title = tagDescription.Source.TagName;
                        tagDescription.Uom = tagDescription.Source.Unit;
                    }

                    xElement = xTagDescription.Element( "HMI_Format_describe" );
                    if ( xElement != null )
                    {
                        xElement = xElement.Element( "HMIPosPoint" );
                        if ( xElement != null )
                        {
                            ushort tmp;
                            ushort.TryParse( xElement.Value, out tmp );
                            tagDescription.PosPoint = tmp;
                        }
                    }

                    tagDescriptions.Add( tagDescription );
                }
                catch ( Exception ex )
                {
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                }
            }

            return tagDescriptions;
        }
        private static IEnumerable<FormulaEvalNDS> CollectFormulaEvalTags( IEnumerable<TagDescription> descriptions )
        {
            var list = new List<FormulaEvalNDS>( );

            foreach ( var description in descriptions )
            {
                var dsGuid = description.Source.Device.UniDS_GUID;
                var devGuid = description.Source.Device.UniObjectGUID;
                var tagGuid = description.Source.TagGUID;

                var itagDim = description.Source as ITagDim;
                if ( itagDim != null ) itagDim.ValueDim = description.PosPoint;

                if ( description.Source.TypeOfTagHMI == TypeOfTag.NaN )
                {
                        MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                        throw new ArgumentNullException( string.Format( "*********** \"{0}\" DS: {1} Device: {2} GUID: {3} ***********", description.Title, dsGuid, devGuid, tagGuid ), "Tag is not exist" );
                }
                list.Add( new FormulaEvalNDS( HMI_MT_Settings.HMI_Settings.CONFIGURATION, string.Format( "{0}.{1}.{2}", dsGuid, devGuid, tagGuid ), description.Title, description.Uom, description.Source.TypeOfTagHMI, string.Empty ) );
            }

            return list;
        }
    }

    public class FrmEngineNew2
    {
        public enum Category
        {
            NaN,
            Crush,
            Ustavki
        }
        public class TagDescription
        {
            public String Title;
            public String Uom;
            public ITag Source;
            public FormulaEvalNDS Formula;
            public Boolean IsChange;

            public void CreateFormulaTag( )
            {
                if ( this.Source == null ) throw new ArgumentNullException( string.Format( "Нет сигнала: {0}", this.Title ) );

                var dsGuid = this.Source.Device.UniDS_GUID;
                var devGuid = this.Source.Device.UniObjectGUID;
                var tagGuid = this.Source.TagGUID;

                if ( this.Source.TypeOfTagHMI == TypeOfTag.NaN )
                {
                    throw new ArgumentNullException(
                        string.Format(
                            "\n*********** Нет сигнала: \"{0}\" DS: {1} Device: {2} GUID: {3} Тип: {4} ***********\n",
                            this.Title, dsGuid, devGuid, tagGuid, this.Source.TypeOfTagHMI ), "Tag is not exist" );
                }

                this.Formula = new FormulaEvalNDS( HMI_MT_Settings.HMI_Settings.CONFIGURATION,
                                                   string.Format( "{0}.{1}.{2}", dsGuid, devGuid, tagGuid ),
                                                   this.Title, this.Uom, this.Source.TypeOfTagHMI, string.Empty );
            }
            public String Result
            {
                get
                {
                    return ( this.Source == null )
                               ? "0.0.0"
                               : string.Format( "{0}.{1}.{2}", this.Source.Device.UniDS_GUID,
                                                this.Source.Device.UniObjectGUID, this.Source.TagGUID );
                } 
            }
            public static String NodeTitle( TagDescription description )
            {
                return ( string.IsNullOrEmpty( description.Uom ) )
                           ? string.Format( "{0}:   {1}", description.Title, description.Source.ValueAsString )
                           : string.Format( "{0}:   {1} ({2})", description.Title, description.Source.ValueAsString, description.Uom );
            }
        }
        public class GroupDescription
        {
            public readonly List<TagDescription> Tags = new List<TagDescription>();
            public readonly List<GroupDescription> Groups = new List<GroupDescription>();
            public String Name;
            public Category Category;
        }

        readonly UInt32 uniDs, uniDev;
        private readonly List<GroupDescription> groupDescriptions = new List<GroupDescription>();

        public FrmEngineNew2( UInt32 uniDs, UInt32 uniDev )
        {
            this.uniDs = uniDs;
            this.uniDev = uniDev;

            try
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация средств расставления сигналов." );

                var device = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device( uniDs, uniDev );
                if ( device == null )
                    throw new Exception(
                        string.Format( "FrmEngine: Нет связанного устройства с данной формой unids = {0}; unidev = {1}",
                                       uniDs.ToString( CultureInfo.InvariantCulture ),
                                       uniDev.ToString( CultureInfo.InvariantCulture ) ) );

                CollectGroups( device.GetGroupHierarchy(), groupDescriptions );

                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация средств расставления сигналов завершена." );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        
        public IEnumerable<GroupDescription> Groups { get { return groupDescriptions; } }

        private void CollectGroups( IEnumerable<IGroup> groups, ICollection<GroupDescription> descriptions, Category type = Category.NaN )
        {
            foreach ( var group in groups )
            {
                if ( !group.IsEnable ) continue;

                var description = new GroupDescription { Name = @group.NameGroup };

                var xElem = @group.GroupXElement.Attribute( "category" );
                if ( xElem != null ) description.Category = (Category)Enum.Parse( typeof( Category ), xElem.Value );
                else description.Category = type;

                CollectGroups( group.SubGroupsList, description.Groups, description.Category );
                CollectTags( group, description.Tags );
                descriptions.Add( description );
            }
        }
        private void CollectTags( IGroup group, ICollection<TagDescription> descriptions )
        {
            var xTags = group.GroupXElement.Element( "Tags" );
            if ( xTags == null ) return;

            foreach ( var xTagDescription in xTags.Elements( ) )
            {
                try
                {
                    var xmlAttribute = xTagDescription.Attribute( "enable" );
                    if ( xmlAttribute == null || !bool.Parse( xmlAttribute.Value ) ) continue;

                    xmlAttribute = xTagDescription.Attribute( "value" );
                    if ( xmlAttribute == null ) continue;

                    var source = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Tag( uniDs, uniDev, uint.Parse( xmlAttribute.Value ) );
                    if ( source == null ) continue;

                    var tag = new TagDescription { Source = source };
                    
                    var xElement = xTagDescription.Element( "gui_variables_describe" );
                    if ( xElement != null )
                    {
                        var xElem = xElement.Element( "var_title" );
                        tag.Title = ( xElem != null ) ? xElem.Value : tag.Source.TagName;
                        xElem = xElement.Element( "UOM" );
                        tag.Uom = ( xElem != null ) ? xElem.Value : tag.Source.Unit;
                    }
                    else
                    {
                        tag.Title = tag.Source.TagName;
                        tag.Uom = tag.Source.Unit;
                    }

                    xElement = xTagDescription.Element( "HMI_Format_describe" );
                    if ( xElement != null )
                    {
                        xElement = xElement.Element( "HMIPosPoint" );
                        if ( xElement != null )
                        {
                            ushort tmp;
                            ushort.TryParse( xElement.Value, out tmp );
                            
                            var itagDim = tag.Source as ITagDim;
                            if ( itagDim != null ) itagDim.ValueDim = tmp;
                        }
                    }

                    tag.CreateFormulaTag( );
                    descriptions.Add( tag );
                }
                catch ( Exception ex )
                {
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                }
            }
        }
    }
}
