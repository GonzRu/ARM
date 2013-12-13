/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс CurrentModePanels для поддержки панелей норм реж для отдельной формы
 *                                                                             
 *	Файл                     : X:\Projects\99_MTRAhmi\Client\HMI_MT\CurrentModePanels.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 05.05.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;
//using CRZADevices;
namespace HMI_MT
{
	public class CurrentModePanels
	{
		/// <summary>
		/// список панелей frmSmartPanel
		/// </summary>
		List<frmSmartPanel> listSmartPanels = new List<frmSmartPanel>();
		/// <summary>
		/// список секций с описаниями панелей из файла CurrentModePanel.xml
		/// </summary>
		SortedDictionary<string, XElement> slDescSmartPanels = new SortedDictionary<string, XElement>();
		/// <summary>
		/// xml-файл CurrentModePanel.xml
		/// </summary>
		XDocument xdocCMP;
		/// <summary>
		/// секция в которой размещаются CurModeParamsPanel 
		/// для тек пользователя и формы
		/// </summary>
		XElement xeSection4CurrentUserAndForm;
		/// <summary>
		/// путь к файлу с описанием панелей
		/// с привязкой к тек пользователю и 
		/// определенной форме
		/// </summary>
		string Path2CurrentModePanel;

		StringBuilder sbTmp = new StringBuilder();
		/// <summary>
		/// форма на кот создаются панели
		/// </summary>
		Form frmParent;
		/// <summary>
		/// текущий пользователь
		/// </summary>
		string userCurrent = string.Empty;

		/// <summary>
		/// конструктор для размещ панелей тек режима
		/// </summary>
		/// <param name="frm"></param>
		public  CurrentModePanels( string curuser, Form frm )
		{
			frmParent = frm;
			userCurrent = curuser;
			CreateCurrentModePanels(frmParent);
		}
		/// <summary>
		/// сохранение конфигурации панелей 
		/// </summary>
		public void SavePanels()
		{ 
			if (!File.Exists(Path2CurrentModePanel))
				return;

			foreach( frmSmartPanel fsp in listSmartPanels )
			{
				XElement xesp;
				if (!slDescSmartPanels.ContainsKey(fsp.Name))
					break;
				
				xesp = slDescSmartPanels[fsp.Name];

				xesp.Attribute("Width").Value = fsp.Width.ToString();
				xesp.Attribute("Height").Value = fsp.Height.ToString();
				xesp.Attribute("Left").Value = fsp.Left.ToString();
				xesp.Attribute("Top").Value = fsp.Top.ToString();
			}

			if (xdocCMP != null)
				xdocCMP.Save(Path2CurrentModePanel);
		}

		private void CreateCurrentModePanels(Form fct2d)
		{
			sbTmp.Clear();
			sbTmp.Append(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "CurrentModePanel.xml");
			
			if (!File.Exists(sbTmp.ToString()))
				return;

			Path2CurrentModePanel = sbTmp.ToString();

			/*
			 * файл с панелями тек режима существует:
			 * создаем и размещаем их
			 */
			xdocCMP = XDocument.Load(sbTmp.ToString());

			IEnumerable<XElement> xepnls = GetListPanels4CurUserAndReqForm(xdocCMP);

         if (xepnls != null)
			   // создаем формы для панелей
			   foreach( XElement xepnl in xepnls )
				   CreateCMP(xepnl);
		}
		/// <summary>
		/// Получить список секций с опис панелей тек реж
		/// </summary>
		/// <param name="xdocCMP"></param>
		/// <returns></returns>
		private IEnumerable<XElement> GetListPanels4CurUserAndReqForm(XDocument xdocCMP)
		{
			IEnumerable<XElement> xeusers = xdocCMP.Element("MTRA").Element("CurrentModeParameters").Elements("User");

			foreach (XElement xeuser in xeusers)
			{
				if (xeuser.Attribute("name").Value == userCurrent)
				{
					IEnumerable<XElement> xecfgs = xeuser.Elements("Configuration");
					foreach (XElement xecfg in xecfgs)
						if (xecfg.Attribute("isActive").Value == "true")
						{
							IEnumerable<XElement> xefrms = xecfg.Elements("FormForPlacement");
							foreach (XElement xefrm in xefrms)
								if (xefrm.Attribute("name").Value == frmParent.Name)
								{
									// запомнили секцию куда можно писать новые панели и редактировать существующие
									xeSection4CurrentUserAndForm = xefrm;
									return xefrm.Elements("CurModeParamsPanel").Where(x => x.Attribute("isThisPanelVisible").Value == "true").Select(x => x).ToList();
								}
						}
				}
			}
			return null;
		}
		/// <summary>
		/// Создание панели 
		/// текущего режима
		/// </summary>
		private void CreateCMP(XElement xecmp)
		{
			try
			{
                //frmSmartPanel fspanel = new frmSmartPanel(frmParent,int.Parse(xecmp.Attribute("LinkedDevGUID").Value),this);

                //fspanel.Name = xecmp.Attribute("name").Value;
                //fspanel.Owner = frmParent;
                //fspanel.ControlBox = false;
                //fspanel.ShowIcon = false;
                //fspanel.StartPosition = FormStartPosition.Manual;
                //fspanel.AutoSize = false;
                //fspanel.FormBorderStyle = FormBorderStyle.Sizable;//.FixedDialog;
                //fspanel.ShowInTaskbar = false;
                //fspanel.FormClosing += new FormClosingEventHandler(fspanel_FormClosing);
                //XElement xedev = CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG(int.Parse(xecmp.Attribute("LinkedDevGUID").Value));
                //if (xedev != null)
                //    fspanel.Text = xedev.Element("DescDev").Value;

                //fspanel.Width = Convert.ToInt32(xecmp.Attribute("Width").Value);
                //fspanel.Height = Convert.ToInt32(xecmp.Attribute("Height").Value);
                //fspanel.Left = Convert.ToInt32(xecmp.Attribute("Left").Value);
                //fspanel.Top = Convert.ToInt32(xecmp.Attribute("Top").Value);

                //FillCMPPanel(xecmp, fspanel);
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}

		/// <summary>
		/// заполнить панель содержимым секции CurModeParamsPanel
		/// </summary>
		/// <param name="xecmp"></param>
		/// <param name="fspanel"></param>
		private void FillCMPPanel(XElement xecmp, frmSmartPanel fspanel)
		{
            try
            {
                throw new Exception(string.Format("Заглушка : Вызов функции {0}.{1}", @"X:\Projects\40_Tumen_GPP09\Client\HMI_MT\CurrentModePanels.cs", "FillCMPPanel())"));
			}
			catch(Exception ex)
			{
			   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

            //fspanel.flp.Controls.Clear();

            //IEnumerable<XElement> xeformulas = xecmp.Element("Adapters").Elements("formula");

            //foreach (XElement xeformula in xeformulas)
            //{
            //    CRZADevices.AdapterBase abase = (CRZADevices.AdapterBase)new CRZADevices.AdapterFactoryImplementation().Make(xeformula.Attribute("typeadapter").Value);

            //    abase.Init(xeformula, Configurator.KB, xecmp.Attribute("LinkedDevGUID").Value);

            //    LabelTextbox.MTRAUserControl mtraUc = (LabelTextbox.MTRAUserControl)new LabelTextbox.MTRAUserControlFactoryImplementation().Make(abase);
            //    mtraUc.Init(fspanel.flp);

            //    //IEnumerable<XElement> xevalues = xeformula.Elements("value");

            //    // формируем список тегов для обновления с сервера
            //    if (HMI_Settings.ClientDFE != null)
            //        HMI_Settings.ClientDFE.AddArrTags(fspanel.Text, abase);

            //    abase.DoEval();
            //}

            //fspanel.Show();
            //AddPanel2List(fspanel);

            //if (slDescSmartPanels.ContainsKey(fspanel.Name))
            //    slDescSmartPanels[fspanel.Name] = xecmp;
            //else
            //    slDescSmartPanels.Add(fspanel.Name, xecmp);
		}

		public void AddPanel2List(frmSmartPanel fspanel)
		{
			if (CMPPanelExist(fspanel.Name))
				return;

			listSmartPanels.Add(fspanel);
		}
		/// <summary>
		/// проверка на существование панели в списке
		/// </summary>
		/// <param name="namePanel"></param>
		/// <returns></returns>
		public bool CMPPanelExist(string namePanel)
		{
			// проверим на существование панели в списке загруженных панелей
			foreach (frmSmartPanel fsp in listSmartPanels)
				if (fsp.Name == namePanel)
					return true;
			// а может она просто невидима - тогда смотрим ее в xeSection4CurrentUserAndForm - секция для тек формы и польз куда можно писать новые панели и редактировать существующие
			IEnumerable<XElement>  xehides = xeSection4CurrentUserAndForm.Elements("CurModeParamsPanel").Where(x => (x.Attribute("name").Value == namePanel) && (x.Attribute("isThisPanelVisible").Value == "false")).Select(x => x).ToList();
			if (xehides.Count() > 0)
				return true;

			return false;
		}

		void fspanel_FormClosing(object sender, FormClosingEventArgs e)
		{			
                        try
            {
                throw new Exception(string.Format("Заглушка : Вызов функции {0}.{1}", @"X:\Projects\40_Tumen_GPP09\Client\HMI_MT\CurrentModePanels.cs", "fspanel_FormClosing())"));
			}
			catch(Exception ex)
			{
			   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

            //// удаляем ссылки на теги
            //if (HMI_Settings.ClientDFE != null)
            //    HMI_Settings.ClientDFE.RemoveAdapterBaseRefToPageTags((sender as Form).Text);
		}

		public void DoCMPVisible(string namePanel)
		{
			foreach (frmSmartPanel fsp in listSmartPanels)
				if (fsp.Name == namePanel)
				{
					fsp.Visible = true;
					return;
				}

			// если мы здесь, значит имеем дело со скрытой панелью
			XElement xehide = xeSection4CurrentUserAndForm.Elements("CurModeParamsPanel").Where(x => (x.Attribute("name").Value == namePanel) && (x.Attribute("isThisPanelVisible").Value == "false")).Select(x => x).Single();
			xehide.Attribute("isThisPanelVisible").Value = "true";
			xdocCMP.Save(Path2CurrentModePanel);
			CreateCMP(xehide);
		}

		/// <summary>
		/// сохранить новую или существующую панель для текущего пользователя и формы
		/// </summary>
		public void SaveNewPanels(int devguid, /*TCRZAVariable TCRZAVar,*/ frmSmartPanel fspLink)	//List<TCRZAVariable> lstTCRZAVars
		{
                        try
            {
                throw new Exception(string.Format("Заглушка : Вызов функции {0}.{1}", @"X:\Projects\40_Tumen_GPP09\Client\HMI_MT\CurrentModePanels.cs", "SaveNewPanels())"));
			}
			catch(Exception ex)
			{
			   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

			//CreateSection4NewPanels(devguid, CreateFormulaSection(TCRZAVar), fspLink);
		}

		//private void CreateSection4NewPanels(int devguid, TCRZAVariable tv, frmSmartPanel fspLink)//List<TCRZAVariable> lstTCRZAVars
		private void CreateSection4NewPanels(int devguid, XElement xe_section, frmSmartPanel fspLink)//List<TCRZAVariable> lstTCRZAVars
		{
			XElement xe;
			XElement xeadapters = null;

			if (!slDescSmartPanels.ContainsKey(devguid.ToString()))
			{
				xe = new XElement("CurModeParamsPanel");

				xe.Add(new XAttribute("name", devguid.ToString()));
				xe.Add(new XAttribute("isThisPanelVisible", "true"));
				xe.Add(new XAttribute("type", "Linked"));
				xe.Add(new XAttribute("LinkedDevGUID", devguid.ToString()));
				xe.Add(new XAttribute("isCaptionVisible", "true"));
				xe.Add(new XAttribute("Caption", ""));
				xe.Add(new XAttribute("Width", fspLink.Width));
				xe.Add(new XAttribute("Height", fspLink.Height));
				xe.Add(new XAttribute("Left", fspLink.Left));
				xe.Add(new XAttribute("Top", fspLink.Top));

				xeadapters = new XElement("Adapters");
				xeadapters.Add(new XAttribute("isSorted", "false"));
				xeadapters.Add(new XAttribute("typeSort", "None"));

				//xeadapters.Add(CreateFormulaSection(tv));
				// добавим секцию с опианием формулы
				xeadapters.Add(xe_section);				

				xe.Add(xeadapters);
				xeSection4CurrentUserAndForm.Add(xe);
				if (slDescSmartPanels.ContainsKey(devguid.ToString()))
					slDescSmartPanels[devguid.ToString()] = xe;

				AddPanel2List(fspLink);
			}
			else 
			{
				slDescSmartPanels[devguid.ToString()].Element("Adapters").Add(xe_section);	//CreateFormulaSection(tv)
				xe = slDescSmartPanels[devguid.ToString()];
			}
			FillCMPPanel(xe, fspLink);
			xdocCMP.Save(Path2CurrentModePanel);
		}

		private XElement CreateFormulaSection(/*TCRZAVariable tv*/)
		{
                    try
            {
                throw new Exception(string.Format("Заглушка : Вызов функции {0}.{1}", @"X:\Projects\40_Tumen_GPP09\Client\HMI_MT\CurrentModePanels.cs", "CreateFormulaSection())"));
			}
			catch(Exception ex)
			{
			   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

			StringBuilder sbid = new StringBuilder();

			XElement xe_formula = new XElement("formula");

            //if (tv is TBitFieldVariable)
            //    xe_formula.Add(new XAttribute("typeadapter", "BitFieldAdapter"));
            //else
            //    xe_formula.Add(new XAttribute("typeadapter", "FloatFieldAdapter"));

            //xe_formula.Add(new XAttribute("name", tv.Name));
            //xe_formula.Add(new XAttribute("Caption", tv.Name));

            //if (tv.Dim != null)
            //    xe_formula.Add(new XAttribute("Dim", tv.Dim));
            //else
            //    xe_formula.Add(new XAttribute("Dim", string.Empty));

            //xe_formula.Add(new XAttribute("commentary", ""));
            //xe_formula.Add(new XAttribute("express", "0"));

            //XElement xe_value = new XElement("value");
            //xe_value.Add(new XAttribute("id", "0"));
            //sbid.Clear();
            //sbid.Append(tv.Group.Device.NumFC + "." + tv.Group.Device.NumDev + "." + tv.Group.Id + "." + tv.RegInDev + "." + tv.bitMask);
            //xe_value.Add(new XAttribute("tag", sbid.ToString()));

            //xe_formula.Add(xe_value);

			return xe_formula;
		}

		/// <summary>
		/// удалить панель тек режима
		/// </summary>
		/// <param name="devguid"></param>
		/// <param name="fsp"></param>
		public void DeleteCMP(int devguid, frmSmartPanel fsp)
		{
			slDescSmartPanels.Remove(devguid.ToString());
			listSmartPanels.Remove(fsp);
			fsp.Close();

			IEnumerable<XElement> xes = GetListPanels4CurUserAndReqForm(xdocCMP);
			foreach( XElement xe in xes )
			{
				if (xe.Attribute("LinkedDevGUID").Value == devguid.ToString())
				{
					xe.Remove(); 
					if (xdocCMP != null)
						xdocCMP.Save(Path2CurrentModePanel);
					break;
				}
			}
		}

		/// <summary>
		/// скрыть панель тек режима
		/// </summary>
		/// <param name="devguid"></param>
		/// <param name="fsp"></param>
		public void HideCMP(int devguid, frmSmartPanel fsp)
		{
			slDescSmartPanels.Remove(devguid.ToString());
			listSmartPanels.Remove(fsp);
			fsp.Close();

			IEnumerable<XElement> xes = GetListPanels4CurUserAndReqForm(xdocCMP);
			foreach (XElement xe in xes)
			{
				if (xe.Attribute("LinkedDevGUID").Value == devguid.ToString())
				{
					xe.Attribute("isThisPanelVisible").Value = "false";

					if (xdocCMP != null)
						xdocCMP.Save(Path2CurrentModePanel);

					break;
				}
			}
		}
		/// <summary>
		/// удаление тегов
		/// </summary>
		internal void DeleteTags(int devguid, frmSmartPanel fsp)
		{
			dlgDeleteTagsFromCMP dlgdeltags = new dlgDeleteTagsFromCMP();
			dlgdeltags.Top = fsp.Top;
			dlgdeltags.Left = fsp.Left + fsp.Width;
			XElement xeCMP = GetCMPDescByDevGuid(devguid);

			try 
			{
				dlgdeltags.Init(xeCMP);
			}catch
			{
				return;
			}
			
			dlgdeltags.ShowDialog();

			fsp.flp.Controls.Clear();

			FillCMPPanel(xeCMP,fsp);
		}

		private XElement GetCMPDescByDevGuid(int devguid)
		{
			return xeSection4CurrentUserAndForm.Elements("CurModeParamsPanel").Where(x => (x.Attribute("name").Value == devguid.ToString())).Select(x => x).Single();
		}

		internal void CreateCalculatedTag(int devguid, frmSmartPanel frmSmartPanel)
		{
			dlgCreateEditCalculatedTag dlgCECT = new dlgCreateEditCalculatedTag();
			dlgCECT.ShowDialog();

			if (dlgCECT.xe_formula == null)
				return;

			CreateSection4NewPanels(devguid, dlgCECT.xe_formula, frmSmartPanel);
		}
	}
}