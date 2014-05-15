/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс для организации взаимодействия с классом описания команды
 *                                                                             
 *	Файл                     : X:\Projects\01_HMIWinFormsClient\InterfaceLibrary\IDeviceCommand.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 15.02.2012
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace InterfaceLibrary
{
    #warning need to move to another file
    public interface IParameter
    {
        /// <summary>
        /// Parameter name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Parameter value
        /// </summary>
        UInt32 Value { get; }

        /// <summary>
        /// Init Parameter from XML element
        /// </summary>
        /// <param name="xeinit"></param>
        void Init(XElement xeinit);
    }

    public interface IDeviceCommand
    {
        /// <summary>
        /// краткое имя команды
        /// </summary>
        string CmdName{get;}
        /// <summary>
        /// диспетчерское имя команды
        /// для использования в HMI
        /// </summary>
        string CmdDispatcherName{get;}
        /// <summary>
        /// инициализация команды
        /// на основе содержимого 
        /// xml-секции команды в файле
        /// описания устройства
        /// </summary>
        /// <param name="xeinit"></param>
        void Init(XElement xeinit);

        #region New parameters for telemechanica
        /// <summary>
        /// Command address
        /// </summary>
        string IECAddress { get; }

        /// <summary>
        /// Command parameters list
        /// </summary>
        List<IParameter> Parameters { get; }
        #endregion
    }
}
