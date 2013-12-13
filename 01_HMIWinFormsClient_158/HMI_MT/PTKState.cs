/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс PTKState - для хранения состояния различных компонентов ПТК
 *                                                                             
 *	Файл                     : X:\Projects\99_MTRAhmi\Client\HMI_MT\PTKState.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 21.03.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
 * Особенности реализации:
 * Используется паттерн Одиночка (Singlenton)
 *#############################################################################*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using AdapterLib;

namespace HMI_MT
{
	public sealed class PTKState
	{
		#region События
		#endregion

		#region Свойства
		#endregion

		#region public
		#endregion

		#region private
		/// <summary>
		/// список <DevGuid, ссылка на список <Название сигнала, ссылка на адаптер для привязки к изменениям>>
		/// </summary>
        Dictionary<string, Dictionary<string, AdapterBase>> listDevByDevGuid = new Dictionary<string, Dictionary<string, AdapterBase>>();
		#endregion

		#region конструктор(ы)
		public static PTKState Iinstance { get { return InstanceHolder._instance; } }

		private class InstanceHolder
		{
			static InstanceHolder() { }
			internal static readonly PTKState _instance = new PTKState();
		}

		/// <summary>
		/// закрытый конструктор
		/// по умолчанию
		/// </summary>
		private PTKState(){}
		#endregion

		/// <summary>
		/// инициализация экземпляра класса состояния ПТК
		/// </summary>
		/// <param name="path2PrgDevCFG_cdp"></param>
		/// <param name="path2PanelStateFile"></param>
		/// <param name="kb"></param>
        public void InitPTKStateInfo()///*string path2PrgDevCFG_cdp,*/ string path2PanelStateFile/*, ArrayList kb*/
		{
			StringBuilder sbFullTag = new StringBuilder();

			try
			{
                if (!File.Exists(HMI_MT_Settings.HMI_Settings.PathPanelState_xml))// path2PanelStateFile
					throw new Exception("(70) : PTKState.cs : InitPTKStateInfo() : Ошибка открытия файла PanelState.xml для отображения состояния ПТК");

				//XDocument xdPanelStateFile = XDocument.Load(path2PanelStateFile);

                XElement xetest = HMI_MT_Settings.HMI_Settings.XDoc4PathPanelState_xml.Element("MT").Element("PTKDeviceState");//xdPanelStateFile

				if (xetest == null)
					throw new Exception("(131) : PTKState.cs : InitPTKStateInfo() : Нет секции описания PTKDeviceState в файле PanelState.xml");

				IEnumerable<XElement> xedevs = xetest.Elements("Device");
				foreach (XElement xedev in xedevs)
				{
                    Dictionary<string, AdapterBase> listLinkToAdapters = new Dictionary<string, AdapterBase>();

                    IEnumerable<XElement> xeformulas = xedev.Elements("formula");
                    int numdev = (int.Parse(xedev.Attribute("DevGUID").Value)) % 256;
                    int fc = (int.Parse(xedev.Attribute("DevGUID").Value)) / 256;

                    foreach (XElement xeformula in xeformulas)
                    {
                        AdapterBase abase = (AdapterBase)new AdapterFactoryImplementation().Make(xeformula.Attribute("typeadapter").Value);

                        abase.Init(xeformula, xedev.Attribute("DevGUID").Value);//kb, 

                        if (!listLinkToAdapters.ContainsKey(xeformula.Attribute("name").Value))
                            listLinkToAdapters.Add(xeformula.Attribute("name").Value, abase);
                        else
                            System.Windows.Forms.MessageBox.Show("Ошибка в конфигурации панели состояний - устройство : " + xeformula.Attribute("name").Value);
                    }

                    listDevByDevGuid.Add(xedev.Attribute("DevGUID").Value, listLinkToAdapters);
				}


                //XDocument xd = XDocument.Load(path2PrgDevCFG_cdp);
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}

        //public AdapterBase GetAdapter4Link(string name4Link, string signalname)
        //{
        //    try
        //    {
        //        if (listDevByDevGuid.ContainsKey(name4Link))
        //        {
        //            Dictionary<string, AdapterBase> listLinkToAdapters = listDevByDevGuid[name4Link];

        //            if (listLinkToAdapters.ContainsKey(signalname))
        //                return listLinkToAdapters[signalname];
        //        }
        //    }
        //    catch (Exception ex)
        //    {				
        //        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
        //    }

        //    return null;
        //}

		public string GetValueAsString(string name4Link, string signalname)
		{
            try
            {
                if (listDevByDevGuid.ContainsKey(name4Link))
                {
                    Dictionary<string, AdapterBase> listLinkToAdapters = listDevByDevGuid[name4Link];

                    if (listLinkToAdapters.ContainsKey(signalname))
                        return listLinkToAdapters[signalname].ValueAsString;
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

			return string.Empty;
		}

		public bool IsAdapterExist(string name4Link, string signalname)
		{
            try
            {
                if (listDevByDevGuid.ContainsKey(name4Link))
                {
                    Dictionary<string, AdapterBase> listLinkToAdapters = listDevByDevGuid[name4Link];

                    if (listLinkToAdapters.ContainsKey(signalname))
                        return true;
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

			return false;
		}
	}
}
