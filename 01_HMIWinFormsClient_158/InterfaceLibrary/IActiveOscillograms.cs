/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс поддержки отслеживания процесса формирования осциллограммы
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\IActiveOscillograms.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 28.11.2011 
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

namespace InterfaceLibrary
{
    public interface IActiveOscillograms
    {
        /// <summary>
        /// добавить осциллограмму к активным осциллограммам ПТК
        /// </summary>
        /// <param name="cmd"></param>
        void AddOsc(IOscillogramma osc);
        /// <summary>
        /// удалить осциллограмму из списка акт осциллограмм
        /// </summary>
        /// <param name="cmd"></param>
        void RemoveOsc(UInt32 ident_in_bd, byte[] content);
    }
}
