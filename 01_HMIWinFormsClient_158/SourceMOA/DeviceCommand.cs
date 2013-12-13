/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс команды устройства
 *                                                                             
 *	Файл                     : X:\Projects\01_HMIWinFormsClient\SourceMOA\DeviceCommand.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 15.03.2012
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
using InterfaceLibrary;
using System.Xml.Linq;

namespace SourceMOA
{
    public class DeviceCommand : IDeviceCommand
    {
        /// <summary>
        /// краткое имя команды
        /// </summary>
        public string CmdName { get {return cmdName;} }
        string cmdName = string.Empty;
        /// <summary>
        /// диспетчерское имя команды
        /// для использования в HMI
        /// </summary>
        public string CmdDispatcherName { get{return cmdDispatcherName;} }
        string cmdDispatcherName = string.Empty;
        /// <summary>
        /// инициализация команды
        /// на основе содержимого 
        /// xml-секции команды в файле
        /// описания устройства
        /// </summary>
        /// <param name="xeinit"></param>
        public void Init(XElement xeinit)
        {
            cmdName = xeinit.Attribute("name").Value;
            cmdDispatcherName = xeinit.Element("CMDDescription").Value;
        }
    }
}
