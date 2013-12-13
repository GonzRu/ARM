/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс описания устройства сбора информации с устройств
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\ECU.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 25.03.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using InterfaceLibrary;
using System.Diagnostics;

namespace SourceMOA
{
	public class ECU : IECU, IDevice
	{
		#region События
		#endregion

		#region Свойства
        /// <summary>
        /// строка описания ECU для представления в интерфейсе
        /// </summary>
        public string StrECUDescription 
        {
            get{ return strECUDescription;}
        }
         string strECUDescription = string.Empty;

         /// <summary>
         /// строка описания для представления устройства в HMI
         /// с точки зрения физич. расположения
         /// </summary>
         public string StrDescriptionAsPhysicalDevice
         {
            get{return strDescriptionAsPhysicalDevice;}
         }
         string strDescriptionAsPhysicalDevice = string.Empty;
         /// <summary>
         /// строка описания для представления устройсва в HMI
         /// с точки зрения логич. расположения
         /// </summary>
         public string StrDescriptionAsLogicalDevice 
         {
            get{return strDescriptionAsLogicalDevice;}
         }
	    public string Name { get { throw new NotImplementedException(); } }
	    public string TypeName { get { throw new NotImplementedException(); } }
	    public string Description { get { throw new NotImplementedException(); } }
	    public string Version { get { throw new NotImplementedException(); } }
	    string strDescriptionAsLogicalDevice = string.Empty;

		/// <summary>
		/// уникальный (в пределах DS)
		/// номер объекта
		/// </summary>
		public uint UniObjectGUID 
		{
			get { return uniObjectGUID; }
			set { uniObjectGUID = value; } 
		}
		uint uniObjectGUID;
		/// <summary>
		/// секция описания устройсва в
		/// файле PrgDevCFG.cdp
		/// </summary>
		public XElement XESsectionDescribe
		{
			get { return xESsectionDescribe; }
		}
		XElement xESsectionDescribe;
		/// <summary>
		/// уник номер DS
		/// </summary>
		public uint UniDS_GUID
		{
			get { return uniDS_GUID; }
		}
		uint uniDS_GUID;
        /// <summary>
        /// приоритетный режим отображения тегов -
        /// в первичных значениях, вторичных и т.п.
        /// </summary>
        public TypeViewTag TypeTagPriorityView 
        { 
            get{ return typeTagPriorityView; }
            set{ typeTagPriorityView = value;}
        }
        TypeViewTag typeTagPriorityView = TypeViewTag.ADC;
		#endregion

		#region public
		public void AddDevices(IDevice dev)
		{
			if (dev != null)
				slEcuDevices.Add(dev.UniObjectGUID, dev);
		}
		/// <summary>
		/// извлечь тег
		/// </summary>
		/// <param name="tagGuid"></param>
		/// <returns></returns>
		public ITag GetTag(UInt32 tagGuid)
		{
			return null;
		}

		#endregion

		#region private
		/// <summary>
		/// список устройств этого ECU
		/// key - objectGUID
		/// value - ссылка на устройство
		/// </summary>
		SortedList<uint, IDevice> slEcuDevices = new SortedList<uint, IDevice>();
		/// <summary>
		/// список иерархии групп устройства и распределения тегов по ним
		/// </summary>
		List<IGroup> listDevGroupsHierarchy = new List<IGroup>();
		/// <summary>
		/// список тегов устройства
		/// </summary>
		SortedList<uint, Tag> slDeviceTags = new SortedList<uint, Tag>();
		#endregion

		#region конструктор(ы)
		public ECU(XElement xeObj, string path2SrcPrgDevCfg, uint uniDS_GUID)
		{
			try
			{
				uniObjectGUID = uint.Parse(xeObj.Attribute("objectGUID").Value);
				xESsectionDescribe = xeObj;
                strDescriptionAsPhysicalDevice = strECUDescription = string.Format("{0}#{1}", xeObj.Attribute("objectGUID").Value, xeObj.Attribute("describe").Value); //, xeObj.Attribute("fcadr").Value
				this.uniDS_GUID = uniDS_GUID;

				//CreateListTagsDevice(xdoc_dev);
				//listDevGroupsHierarchy = CreateGroupHierarchy(xdoc_dev.Element("Device").Element("Groups"));
                
                //if (xdoc_dev.Element("Device").Elements("Commands").Count() != 0)
                //    lstDeviceCommand = CreateCmdList(xdoc_dev.Element("Device").Element("Commands"));
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}			
		}
		#endregion
					

		#region public-методы
		#endregion

		#region public-методы реализации интерфейса xxx
        /// <summary>
        /// ссылка на класс со специфич параметрами устройства
        /// </summary>
        public ISpecificDeviceValue SpecificDeviceValue 
        { 
            get {return specificDeviceValue;}
        }
        ISpecificDeviceValue specificDeviceValue;

		/// <summary>
		/// Список устройств ECU
		/// </summary>
		public List<IDevice> GetLisEcutDevices()
		{
			List<IDevice> lstdevs = new List<IDevice>();			
			
			try
			{
                foreach( KeyValuePair<uint, IDevice> kvp in slEcuDevices )
                    lstdevs.Add(kvp.Value);
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

			return lstdevs;
		}
		/// <summary>
		/// список групп/подгрупп устройства
		/// со списком тегов
		/// </summary>
		/// <returns></returns>
		public List<IGroup> GetGroupHierarchy()
		{
			return listDevGroupsHierarchy;
		}
		/// <summary>
		/// список тегов устройства
		/// </summary>
		/// <returns></returns>
		public List<ITag> GetRtuTags()
		{
			List<ITag> lsttags = new List<ITag>();

			foreach (ITag tag in this.slDeviceTags.Values)
				lsttags.Add(tag);

			return lsttags;
		}
		/// <summary>
		/// разобрать пакет с данными 
		/// от устройства с учетом специфики
		/// </summary>
		public void ParsePacketRawData(BinaryReader binReader)
		{ 
		}
        List<IDeviceCommand> lstDeviceCommand = new List<IDeviceCommand>();
        /// <summary>
        /// получить список команд устройства
        /// </summary>
        /// <returns></returns>
        public List<IDeviceCommand> GetListDeviceCommands()
        {
            return lstDeviceCommand;
        }
        /// <summary>
        /// извлечь команду по 
        /// краткому имени
        /// </summary>
        /// <param name="cmdname"></param>
        /// <returns></returns>
        public IDeviceCommand GetDeviceCommandByShortName(string cmdname)
        {
            IDeviceCommand dc = null;

            foreach (IDeviceCommand idc in lstDeviceCommand)
                if (idc.CmdName == cmdname)
                    dc = idc;

            return dc;
        }

		#endregion
		
		#region private-методы
		#endregion

	}
}
