/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс описания логической группы устройства
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\IGroup.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 14.11.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace InterfaceLibrary
{
    public interface IGroup
    {
        Boolean IsEnable { get; set; }
        /// <summary>
        /// название группы
        /// </summary>
        string NameGroup { get; set; }
        /// <summary>
        /// название панели группы присоединения к форме
        /// </summary>
        string NameGroupPanel { get; set; }
        /// <summary>
        /// уник номер группы
        /// </summary>
        string GroupGUID { get; set; }
        /// <summary>
        /// устройство, которому принадлежит эта группа
        /// </summary>
        IDevice Device4ThisGroup { get; set; }
        /// <summary>
        /// список подгрупп
        /// </summary>
        List<IGroup> SubGroupsList { get; set; }
        /// <summary>
        /// список TagGUID тегов
        /// </summary>
        ArrayList SubGroupTagsList { get; set; }
        /// <summary>
        /// секция с содержимым группы в xml-файле конфигурации
        /// </summary>
        XElement GroupXElement { get; set; }
    }
}
