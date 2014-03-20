/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс описания устройства
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\Device.cs
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
using HMI_MT_Settings;

namespace SourceMOA
{
	public class Device : IDevice
	{
		#region Свойства
	    /// <summary>
	    /// строка описания для представления устройства в HMI
	    /// с точки зрения физич. расположения
	    /// </summary>
        public string StrDescriptionAsPhysicalDevice { get; private set; }
	    /// <summary>
	    /// строка описания для представления устройсва в HMI
	    /// с точки зрения логич. расположения
	    /// </summary>
        public string StrDescriptionAsLogicalDevice { get; private set; }

        public String Name { get; private set; }
	    public String TypeName { get; private set; }
        public String Description { get; private set; }
        public String Version { get; private set; }

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
            get { return typeTagPriorityView; }
            set { typeTagPriorityView = value; }
        }
        TypeViewTag typeTagPriorityView = TypeViewTag.ADC;
		#endregion

		#region private
		/// <summary>
		/// список тегов устройства
		/// </summary>
		SortedList<uint, Tag> slDeviceTags = new SortedList<uint, Tag>();
		/// <summary>
		/// список иерархии групп устройства и распределения тегов по ним
		/// </summary>
		List<IGroup> listDevGroupsHierarchy = new List<IGroup>();
		/// <summary>
		/// линейный список (без иерархии) групп устройства и распределения тегов по ним (если они есть)
		/// </summary>
		List<IGroup> listDevGroupsWOHierarchy = new List<IGroup>();
		#endregion

		#region конструктор(ы)
		public Device(XElement xeObj, string path2SrcPrgDevCfg, uint uniDS_GUID)
		{
			try
			{
                xESsectionDescribe = xeObj;
                this.uniDS_GUID = uniDS_GUID;
				uniObjectGUID = uint.Parse(xeObj.Attribute("objectGUID").Value);
			    TypeName = xeObj.Attribute( "TypeName" ).Value;
                Description = xeObj.Element( "DescDev" ).Element( "DescDev" ).Value;
                // строки для представления в интерфейсе
                StrDescriptionAsPhysicalDevice = string.Format( "{0}@{1}", uniObjectGUID, TypeName );
                StrDescriptionAsLogicalDevice = string.Format( "{0}@{1}", UniObjectGUID, Description );

			    var path2f = string.Format( @"{0}\Configuration\{1}#DataServer\Devices\{2}@{3}.cfg",
			                                Path.GetDirectoryName( HMI_Settings.PathToPrjFile ), uniDS_GUID,
			                                UniObjectGUID.ToString( "D3" ), TypeName );

                if ( !File.Exists( path2f ) )
                    throw new Exception( string.Format( "(77) : Device.cs : Device() : Ошибка открытия файла {0}", path2f ) );

                var xdoc_dev = new DeviceXDocument( path2f );
                Name = xdoc_dev.GetDescriptInfo().Element( "DeviceType" ).Value;
			    Version = xdoc_dev.GetDescriptInfo().Element( "DeviceVersion" ).Value;

			    try
			    {
                    CreateSpecificValues4Device( xdoc_dev.GetSpecificDeviceValues() );
			    }
			    catch ( Exception ex )
			    {
			        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(
			            new Exception( String.Format( "***{0}@{1}***:{2}", UniObjectGUID.ToString( "D3" ), TypeName, ex.Message ) ) );
			    }
                

				//CreateListTagsDevice(xdoc_dev);
                CreateListTagsDevice(xdoc_dev.GetTagsList());

				//listDevGroupsHierarchy = CreateGroupHierarchy(xdoc_dev.Element("Device").Element("Groups"));
                listDevGroupsHierarchy = CreateGroupHierarchy(xdoc_dev.GetGroupsSection());

                //if (xdoc_dev.Element("Device").Elements("Commands").Count() != 0)
                if (xdoc_dev.CommandsSectionExist())
                    //lstDeviceCommand = CreateCmdList(xdoc_dev.Element("Device").Element("Commands"));
                    lstDeviceCommand = CreateCmdList(xdoc_dev.GetCommandsSection());
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}
        /// <summary>
        /// создать класс со специф параметрами устройсва
        /// </summary>
        /// <param name="xElement"></param>
        private void CreateSpecificValues4Device(XElement xElement)
        {
            if ( xElement == null )
                return;//throw new Exception("(160) : Device.cs : CreateSpecificValues4Device() : В xml-файле описания устройства отсутсвует секция <SpecificDeviceValues>");

            var node = xElement.Element( "TypeOfBlocks" );
            if ( node == null )
                throw new Exception( "В файле описания отсудствует ветка \"TypeOfBlocks\"" );

            IEnumerable<XElement> xe_TypeOfBlocks = node.Elements( "TypeOfBlock" );

            MOASpecificDeviceValue sdv = new MOASpecificDeviceValue();

            foreach ( XElement xe_TypeOfBlock in xe_TypeOfBlocks )
                sdv.SetValueByName( xe_TypeOfBlock.Attribute( "type" ).Value,
                                    xe_TypeOfBlock.Attribute( "value" ).Value );

            if (xElement.Element("Ustavki") != null)
            if ( xElement.Element( "Ustavki" ).Elements( "BlockAddresses" ).Count() != 0 )
                if (
                    xElement.Element( "Ustavki" ).Elements( "BlockAddresses" ).Elements( "BlockAddress" ).Count() != 0 )
                {
                    IEnumerable<XElement> xe_UstavkiBlockAddresses =
                        xElement.Element( "Ustavki" ).Element( "BlockAddresses" ).Elements( "BlockAddress" );

                    foreach ( XElement xe_UstavkiBlockAddress in xe_UstavkiBlockAddresses )
                        sdv.AddBlockAdress4Ustavki(
                            uint.Parse( xe_UstavkiBlockAddress.Attribute( "value" ).Value ), null );
                }

            this.specificDeviceValue = sdv;
        }
		#endregion					

		#region public-методы
		#endregion

        #region public-методы реализации интерфейса IDevice
        /// <summary>
        /// ссылка на класс со специфич параметрами устройства
        /// </summary>
        public ISpecificDeviceValue SpecificDeviceValue
        {
            get { return specificDeviceValue; }
        }
        ISpecificDeviceValue specificDeviceValue;

		/// <summary>
		/// извлечь тег
		/// </summary>
		/// <param name="tagGuid"></param>
		/// <returns></returns>
		public ITag GetTag(UInt32 tagGuid)
        {
		    return this.slDeviceTags.ContainsKey(tagGuid) ? this.slDeviceTags[tagGuid] : null;
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
                
                foreach( IDeviceCommand idc in lstDeviceCommand )
                    if (idc.CmdName.Replace('\"', ' ').Trim() == cmdname)
                        dc = idc;
            return dc;
        }
		#endregion
			
		#region private-методы
		/// <summary>
		/// заполнить список тегов устройства
		/// </summary>
		/// <param name="xeObj">секция описания источника</param>
		/// <param name="path2SrcPrgDevCfg">путь к файлу PrgDevCfg источника</param>
        //private void CreateListTagsDevice(XDocument xdoc_dev)
        //{
        //    TagsFactory tf = new TagsFactory();
        //    uint tmp = 0;
        //    try
        //    {
        //        foreach (var xe_tag in xdoc_dev.Element("Device").Element("Tags").Elements("Tag"))
        //        {
        //            tmp = uint.Parse(xe_tag.Attribute("TagGUID").Value);
        //            slDeviceTags.Add(tmp, tf.CreateTag(xe_tag, this));
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
        //    }
        //}
        private void CreateListTagsDevice(List<XElement> taglist)
        {
            TagsFactory tf = new TagsFactory();
            uint tmp = 0;
            try
            {
                foreach (var xe_tag in taglist)
                {
                    tmp = uint.Parse(xe_tag.Attribute("TagGUID").Value);
                    slDeviceTags.Add(tmp, tf.CreateTag(xe_tag, this));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
		/// <summary>
		/// создать структуру с иерархией групп 
		/// и входящими в них тегами 
		/// </summary>
		/// <param name="xdoc_dev"></param>
		private List<IGroup> CreateGroupHierarchy(XElement xeGroupHierarchy)
		{
			// список групп первого уровня
			List<IGroup> RTUGroupsList = new List<IGroup>();

			try
			{
				listDevGroupsWOHierarchy = new List<IGroup>();

				GroupFactory gfact = new GroupFactory();

				IEnumerable<XElement> xe_groups = xeGroupHierarchy.Elements("Group");

				// создаем список групп первого уровня
				foreach (XElement xe_group in xe_groups)
				{
					IGroup ng = gfact.CreategGroup(xe_group, this);
					RTUGroupsList.Add(ng);
					listDevGroupsWOHierarchy.Add(ng);
				}

				foreach (var group in RTUGroupsList)
				{
					CreateSubItemInGroup(group, gfact);
				}
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}

            return RTUGroupsList;
		}

		private void CreateSubItemInGroup(IGroup group, GroupFactory gfact)
		{
			IEnumerable<XElement> xe_groups = group.GroupXElement.Elements("Group");

			foreach (var xe_group in xe_groups)
			{
				IGroup gr = gfact.CreategGroup(xe_group, this);

				group.SubGroupsList.Add(gr);
				listDevGroupsWOHierarchy.Add(gr);

				IEnumerable<XElement> xe_subgroups = xe_group.Elements("Group");
				if (xe_subgroups.Count() != 0)
					CreateSubItemInGroup(gr, gfact);
			}
		}

        private List<IDeviceCommand> CreateCmdList(XElement xe_cmds)
        {
            List<IDeviceCommand> lstcmds = new List<IDeviceCommand>();
            try
            {
                IEnumerable<XElement> xe_commands = xe_cmds.Elements("Command");
                
                foreach( XElement xe_command in xe_commands )
                {
                    DeviceCommand dc = new DeviceCommand();
                    dc.Init(xe_command);
                    lstDeviceCommand.Add(dc);
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return lstDeviceCommand;
        }
		#endregion
	}
}
