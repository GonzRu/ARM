/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика компонентов запросов уровня DataServer
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\ClientWPF\RequestFactory.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 27.10.2011 
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
using BlockDataComposer;
using InterfaceLibrary;
namespace RequsEtntryLib
{
	public class RequestFactory
	{
			public IRequestEntry CreateRequestEntry(string typereq, IBlockDataComposer bcd)
			{
				IRequestEntry reqentr = null;

				try
				{
					switch (typereq)
					{
						case "ordinal":
							reqentr = new RequestEntry(bcd);
							break;
						default:
							throw new Exception(string.Format("Тип компонента запросов {0} не поддерживается", typereq));
					}
				}
				catch(Exception ex)
				{
					TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
				}
				return reqentr;
			}
		}
	}