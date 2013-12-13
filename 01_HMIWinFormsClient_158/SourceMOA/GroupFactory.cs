/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика для создания групп устройства
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\uvs_SAF\GroupFactory.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 25.08.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using InterfaceLibrary;


namespace SourceMOA
{
	public class GroupFactory
	{
		public IGroup CreategGroup(XElement xe_group, IDevice dev)
		{
			IGroup group = new Group();
			group.NameGroup = xe_group.Attribute("Name").Value;
			group.GroupGUID = xe_group.Attribute("GroupGUID").Value;
		    group.IsEnable = bool.Parse( xe_group.Attribute( "enable" ).Value );
		    var xGroupPanel = xe_group.Attribute( "TypeOfPanel" );
		    group.NameGroupPanel = ( xGroupPanel != null ) ? xGroupPanel.Value : string.Empty;
			group.SubGroupsList = new List<IGroup>();
			group.SubGroupTagsList = new ArrayList();
			group.GroupXElement = new XElement(xe_group);
			group.Device4ThisGroup = dev;

			IEnumerable<XElement> xe_tags = xe_group.Elements("TagGuid");
            if ( xe_tags.Count() != 0 )
                foreach ( XElement xe_tag in xe_tags )
                    group.SubGroupTagsList.Add( xe_tag.Value );
            else
            {
                XElement node = xe_group.Element( "Tags" );
                if ( node != null )
                {
                    xe_tags = node.Elements( "TagGuid" );
                    if ( xe_tags.Count() != 0 )
                        foreach ( XElement xe_tag in xe_tags )
                            group.SubGroupTagsList.Add( xe_tag.Attribute( "value" ).Value );
                }
            }

            return group;
		}
	}
}
