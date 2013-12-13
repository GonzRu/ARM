/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика классов представления инф об осциллограммах
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\OscillogramsLib\OscFactory.cs
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
using InterfaceLibrary;

namespace OscillogramsLib
{
    public class OscFactory
    {
        public IOscillogramma CreateOscillogramm(UInt32 ds, string typeosc, UInt32 identInBD)
        {
            IOscillogramma osc = null;

            try
            {
                switch (typeosc)
                {
                    case "simple":
                        osc = new SimpleOsc( ds, identInBD);                        
                        break;
                    default:
                        throw new Exception(string.Format("Тип осциллограммы {0} не поддерживается", typeosc));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return osc;
        }
    }
}
