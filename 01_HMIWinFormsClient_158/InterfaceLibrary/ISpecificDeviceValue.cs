/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс класса специфических параметров устройства
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\ISpecificDeviceValue.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 25.11.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/


using System;
using System.Collections.Generic;
using System.Text;

namespace InterfaceLibrary
{
    public interface ISpecificDeviceValue
    {
        /// <summary>
        /// получить значение специфич параметра 
        /// устройства по имени параметра
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetValueByName(string name);
        /// <summary>
        /// установить значение специфич параметра 
        /// устройства по имени параметра
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        void SetValueByName(string name, string value);
        /// <summary>
        /// получить список адресов блоков уставок и доп параметров к ним
        /// </summary>
        /// <returns></returns>
        SortedList<UInt32,object> GetListUstavkiAddresses();
        /// <summary>
        /// установить список адресов блоков уставок и доп параметров к ним
        /// </summary>
        /// <returns></returns>
        void AddBlockAdress4Ustavki(uint adr, object specificparameters);
    }
}
