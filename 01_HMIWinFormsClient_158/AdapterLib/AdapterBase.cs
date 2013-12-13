/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Базовый класс всех адаптеров
 *	            для для вычисления тегов по заданной при создании адаптера формуле
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\AdapterLib\AdapterBase.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 23.03.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * для порождения адаптеров определенного типа используется фабрика адаптеров
 * X:\Projects\99_MTRAhmi\Client\crza\CRZADevices\CRZADevices\AdapterFactory.cs
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using InterfaceLibrary;

namespace AdapterLib
{	
	public class AdapterBase
	{
		public delegate void ValueChange(object sender, string value);
		/// <summary>
		/// GUID устройства (если есть)
		/// -1 - если данный адаптер вычисляет теги разл устройств
		/// </summary>
		public int DevGuid = -1;
		/// <summary>
		/// событие по изменению значения адаптера
		/// </summary>
		public virtual event ValueChange AdapterValueChange;
		/// <summary>
		/// Параметр для присвоения любых значений
		/// для придания адаптеру доп. свойств
		/// </summary>
		public object Tag
		{
			get { return tag; }
			set { tag = value; }
		}
		object tag;

		/// <summary>
		/// формула, вычисляющая значение тега
		/// </summary>
		protected FormulaEvalEv4Adapter FEvalEv;

		/// <summary>
		/// тип адаптера
		/// </summary>
	    public	string Typeadapter;
		/// <summary>
		/// имя адаптера для ссылки извне на него
		/// </summary>
		public string Name;
		/// <summary>
		///  диспетчерское название значения 
		///  формируемого адаптером
		/// </summary>
		public string Caption;
		/// <summary>
		/// название размерности значения
		///  формируемого адаптером
		/// </summary>
		public string Dim;
		/// <summary>
		/// комментарий к значению 
		///  формируемого адаптером
		/// </summary>
		public string Commentary;
		/// <summary>
		/// формула по которой формируется 
		/// значение адаптера
		/// </summary>
		public string Express;
		/// <summary>
		/// xml-секция с описанием задания адаптеру:
		/// формулы, тегов для привязки
		/// </summary>
		public XElement XeRawDescribe;
		/// <summary>
		/// виртуальная функция
		/// вызываемая при изменении значения адаптера
		/// </summary>
		/// <param name="valForm"></param>
		public virtual void FEvalEv_OnChangeValForm(object valForm)
		{
		}
		/// <summary>
		/// значение адаптера в виде строки
		/// </summary>
		public string ValueAsString = string.Empty;
		/// <summary>
		/// функция инициализации
		/// адаптера определенного типа
		/// </summary>
		/// <param name="xeinit"></param>
		/// <param name="kb"></param>
        public virtual void Init(XElement xeinit)//, ArrayList kb
		{
			try 
			{
				XeRawDescribe = xeinit;
				Typeadapter = xeinit.Attribute("typeadapter").Value;
				Name = xeinit.Attribute("name").Value;
				Caption = xeinit.Attribute("Caption").Value;
				Dim = xeinit.Attribute("Dim").Value;
				Commentary = xeinit.Attribute("commentary").Value;
				Express = xeinit.Attribute("express").Value;

				// создаем формулу для вычисления и привязываемся к обновлению значения формулы
				ArrayList alstr = new ArrayList();
				IEnumerable<XElement> xevalues = xeinit.Elements("value");
				foreach (XElement xevalue in xevalues)
					alstr.Add(xevalue.Attribute("tag").Value);

                // проверим корректность элементов формулы в текущем контексте исполнения
                if (FormulaElementsIsCorrect(alstr))
                {
                    FEvalEv = new FormulaEvalEv4Adapter(alstr, Express);//kb, 
                    FEvalEv.OnChangeValForm += new ChangeValForm(FEvalEv_OnChangeValForm);
                    FEvalEv.FirstEval();
                }
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}

        /// <summary>
        /// проверка существования устройств и тегов
        /// </summary>
        /// <param name="alstr"></param>
        /// <returns></returns>
        private bool FormulaElementsIsCorrect(ArrayList alstr)
        {
            bool rez = false;
            uint unidev = 0;
            ITag tag = null;

			try
			{
                foreach (string str in alstr)
				    {
					    if (str.Contains("="))
					        continue;

                        string[] strarr = str.Split(new char[]{'.'});

                        // старое или новое описание
                        if (strarr.Length == 3)
                        {   // новое описание ds.dev.tagguid
                            unidev = uint.Parse(strarr[1]);

                            IDevice dev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device(0, unidev);

                            if (dev == null)
                                throw new Exception(string.Format(@"(171) : X:\Projects\01_HMIWinFormsClient\AdapterLib\AdapterBase.cs : FormulaElementsIsCorrect() : Некорректный идентификатор тега = {0}", str));

                            tag = dev.GetTag(uint.Parse(strarr[2]));

                            if (tag == null)
                                throw new Exception(string.Format(@"(175) : X:\Projects\01_HMIWinFormsClient\AdapterLib\AdapterBase.cs : FormulaElementsIsCorrect() : Некорректный идентификатор тега = {0}", str));

                            rez = true;
                        }
                        else if (strarr.Length == 5)
                        {   // старое описание fc.dev.gr.adrtag.bitmalk
                             unidev = uint.Parse(strarr[0]) * 256 + uint.Parse(strarr[1]);

                            IDevice dev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device(0, unidev);

                            if ( dev == null)
                                throw new Exception(string.Format(@"(187) : X:\Projects\01_HMIWinFormsClient\AdapterLib\AdapterBase.cs : FormulaElementsIsCorrect() : Некорректный идентификатор тега = {0}", str));

                            tag = dev.GetTag(HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetTagGUID(0, "MOA_ECU", string.Format("{0}.{1}", strarr[3],strarr[4])));

                            if (tag == null)
                                throw new Exception(string.Format(@"(193) : X:\Projects\01_HMIWinFormsClient\AdapterLib\AdapterBase.cs : FormulaElementsIsCorrect() : Некорректный идентификатор тега = {0}", str));

                            rez = true;
                        }
                        else
                            throw new Exception(string.Format(@"(181) : X:\Projects\01_HMIWinFormsClient\AdapterLib\AdapterBase.cs : FormulaElementsIsCorrect() : Некорректный идентификатор тега = {0}", str));
                    }
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

            return rez;
        }

        /// <summary>
        /// функция инициализации
        /// адаптера определенного типа
        /// </summary>
        /// <param name="xeinit"></param>
        /// <param name="kb"></param>
        public virtual void Init(XElement xeinit, string devGuid)//ArrayList kb, 
        {
            if (!int.TryParse(devGuid, out DevGuid))
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(System.Diagnostics.TraceEventType.Error, 145, "(145) AdapterBase.cs : Init() : некорректный devGuid = " + devGuid);

            Init(xeinit);//, kb
        }

		/// <summary>
		/// принудительное вычисление формулы, связанной с адаптером адаптера 
		/// </summary>
		public void DoEval()
		{
			//FEvalEv.FirstEval();
		}
	}
}
