/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика для создания отдельных устройств
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\DeviceFactory.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 25.10.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using InterfaceLibrary;

namespace SourceMOA
{
		public class DeviceFactory
		{
			public IDevice CreateDevice(XElement xeObj, string path2SrcPrgDevCfg, uint UniDS_GUID)
			{
				IDevice device = null;

				try
				{
					switch (xeObj.Name.LocalName)
					{
						case "SourceECU":
							device = new ECU(xeObj, path2SrcPrgDevCfg, UniDS_GUID);
							break;
						case "Device":
							device = new Device(xeObj, path2SrcPrgDevCfg, UniDS_GUID);
							break;							
						default:
                            throw new Exception(string.Format("Тип устройства {0} не поддерживается", xeObj.Name.LocalName));
					}
				}
				catch(Exception ex)
				{
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex); 
                    throw new Exception(string.Format(@"(51) : {0} : X:\Projects\01_HMIWinFormsClient\SourceMOA\DeviceFactory.cs : CreateDevice() : ОШИБКА : {1}", DateTime.Now.ToString(), ex.Message));
				}
				return device;
			}
		}
}
