/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика очереди активных команд ПТК
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\CommandLib\ActiveCommandsFactory.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 18.11.2011 
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
using InterfaceLibrary;

namespace CommandLib
{
    public class ActiveCommandsFactory
    {
        public IActiveCommands CreateActiveCommands(string typeactiveCmd)
        {
            IActiveCommands activecmd = null;

            try
            {
                switch (typeactiveCmd)
                {
                    case "moaactivecmd":
                        activecmd = new ActiveCmd();
                        break;
                    default:
                        throw new Exception(string.Format("Тип списка акт команд {0} не поддерживается", typeactiveCmd));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return activecmd;
        }
    }
}
