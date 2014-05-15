/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика конфигураций источников
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\Configuration\SourceConfigurationFactory.cs
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
using System.Collections.Generic;
using System.Text;
using InterfaceLibrary;
using SourceMOA;
using System.IO;
using System.Xml.Linq;

namespace SourceConfigurationLib
{
    public class SourceConfigurationFactory
    {
        /// <summary>
        /// фабрика для создания конигурации источника
        /// </summary>
        /// <param name="path2prgdevcfg">путь к конф файлу, где описаны источники</param>
        /// <param name="nameSourceConfiguration">имя источника, для вычисления пути к его конфиг файлу</param>
        /// <returns></returns>
        public ISourceConfiguration CreateSourceConfiguration(string path2prgcfg, XElement xe_src , string stringUniDS_GUID)
        {
            ISourceConfiguration sourceConfiguration = null;

            try
            {
                switch (xe_src.Element("SourceDriver").Attribute("nameSourceDriver").Value)
                {
                    case "MOA_ECU":
                        sourceConfiguration = new MOAConfiguration();
                        sourceConfiguration.InitSrcConfiguration(path2prgcfg, xe_src, stringUniDS_GUID);
                        break;
                    default:
                        throw new Exception(string.Format("Конфигурация источника типа {0} не поддерживается", xe_src.Attribute("name").Value));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return sourceConfiguration;
        }
    }
}
