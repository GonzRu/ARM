/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика создания экземпляров DataServer
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\DataServersLib\DataServerFactory.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 14.11.2011 
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
using System.Xml.Linq;

namespace DataServersLib
{
    public class DataServerFactory
    {
        public IDataServer CreateDataServer(string path2prgcfg,string nameDataServer, XElement DataServerXMLDescribe)
        {
            IDataServer dataServer = null;

            try
            {
                switch (nameDataServer)
                {
                    case "version_1":
                        dataServer = new DataServer();
                        dataServer.CreateConfigurationDS(path2prgcfg,DataServerXMLDescribe);
                        break;
                    default:
                        throw new Exception(string.Format("Конфигурация {0} не поддерживается", 0));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                dataServer = null;
            }

            return dataServer;
        }
    }
}
