/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс описывающий группу в устройстве с учетом иерархии вложенности
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\uvs_SAF\Group.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 24.08.2011 
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
using System.Xml.Linq;
using System.Text;
using InterfaceLibrary;

namespace SourceMOA
{
	public class Group : IGroup
	{
        public Boolean IsEnable { get; set; }
	    /// <summary>
	    /// устройство, которому принадлежит эта группа
	    /// </summary>
        public IDevice Device4ThisGroup { get; set; }
	    /// <summary>
	    /// название группы
	    /// </summary>
        public string NameGroup { get; set; }
	    /// <summary>
	    /// название панели группы присоединения к форме
	    /// </summary>
        public string NameGroupPanel { get; set; }
	    /// <summary>
	    /// уник номер группы
	    /// </summary>
        public string GroupGUID { get; set; }
	    /// <summary>
	    /// список подгрупп
	    /// </summary>
        public List<IGroup> SubGroupsList { get; set; }
	    /// <summary>
	    /// список TagGUID тегов
	    /// </summary>
        public ArrayList SubGroupTagsList { get; set; }
	    /// <summary>
	    /// секция с содержимым группы в xml-файле конфигурации
	    /// </summary>
        public XElement GroupXElement { get; set; }
	}
}
