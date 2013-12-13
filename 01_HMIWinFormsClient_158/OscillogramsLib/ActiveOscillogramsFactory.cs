/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика очереди активных осциллограмм ПТК
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\OscillogramsLib\ActiveOscillogramsFactory.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InterfaceLibrary;

namespace OscillogramsLib
{
    public class ActiveOscillogramsFactory
    {
        public IActiveOscillograms CreateActiveOscillograms(string typeactiveOsc)
        {
            IActiveOscillograms activeosc = null;

            try
            {
                switch (typeactiveOsc)
                {
                    case "moasimpleosc":
                        activeosc = new ActiveOscillograms();
                        break;
                    default:
                        throw new Exception(string.Format("Тип списка акт осциллограмм {0} не поддерживается", typeactiveOsc));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return activeosc;
        }
    
    }
}
