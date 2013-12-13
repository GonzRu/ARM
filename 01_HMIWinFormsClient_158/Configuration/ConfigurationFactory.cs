/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика конфигураций
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\Configuration\ConfigurationFactory.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 11.11.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/


using System;
using InterfaceLibrary;

namespace Configuration
{
    public class ConfigurationFactory
    {
        public IConfiguration CreateConfiguration(string nameConfiguration, string path2prgcfg)//
        {
             IConfiguration configuration = null;

            try
            {
                switch (nameConfiguration)
                {
                    case "OnlyMOACfg":
                        configuration = new Configuration();
                        configuration.CreateConfiguration(path2prgcfg);
                        break;
                    default:
                        throw new Exception(string.Format("Конфигурация {0} не поддерживается",0));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                configuration = null;
            }

            return configuration;
        }
    }
}
