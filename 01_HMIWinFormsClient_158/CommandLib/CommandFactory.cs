/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика команд
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\CommandLib\CommandFactory.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 07.02.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using InterfaceLibrary;

namespace CommandLib
{
    public class CommandFactory
    {
        public ICommand CreateCommand(string typeCmd, ArrayList arrParams)
        {
            ICommand cmd = null;

            try
            {
                switch (typeCmd)
                {
                    case "moacmd":
                        cmd = new MOACommand();
                        cmd.Init(arrParams);
                        break;
                    default:
                        throw new Exception(string.Format("Тип команды {0} не поддерживается", typeCmd));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return cmd;
        }
    }
}
