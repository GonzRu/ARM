/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс доступа к  ECU
 *                                                                             
 *	Файл                     : X:\Projects\40_Tumen_GPP09\Client\InterfaceLibrary\IECU.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 15.12.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InterfaceLibrary
{
    /// <summary>
    /// интерфейс доступа к контроллеру 
    /// собирающему инф с устройств
    /// </summary>
    public interface IECU
    {
        /// <summary>
        /// строка описания ECU для представления в интерфейсе
        /// </summary>
        string StrECUDescription{get;}
        /// <summary>
        /// Список устройств ECU
        /// </summary>
        List<IDevice> GetLisEcutDevices();
        /// <summary>
        ///	добавить устройство в список устройств этого ECU
        /// </summary>
        /// <param name="dev"></param>
        void AddDevices(IDevice dev);
    }
}
