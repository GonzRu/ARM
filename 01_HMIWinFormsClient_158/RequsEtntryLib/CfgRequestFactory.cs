/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика компонентов запросов уровня конфигурации
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\RequsEtntryLib\CfgRequestFactory.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 15.11.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Text;
using BlockDataComposer;
using InterfaceLibrary;

namespace RequsEtntryLib
{
    public class CfgRequestFactory
    {
        public ICfgReqEntry CreateCfgRequestEntry(string typereq)
        {
            ICfgReqEntry reqentr = null;

            try
            {
                switch (typereq)
                {
                    case "ordinal":
                        reqentr = new CfgReqEntry();
                        break;
                    default:
                        throw new Exception(string.Format("Тип компонента запросов {0} не поддерживается", typereq));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
            return reqentr;
        }
    }
}
