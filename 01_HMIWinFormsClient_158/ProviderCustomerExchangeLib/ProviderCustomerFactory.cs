/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика для создания среды обмена данными между поставщиком и потребителем(DataServer)
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ProviderCustomerExchangeLib\ProviderCustomerFactory.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 26.08.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Xml.Linq;
using InterfaceLibrary;
using ProviderCustomerExchangeLib.WCF;

namespace ProviderCustomerExchangeLib
{
	public class ProviderCustomerFactory
	{
		/// <summary>
		/// создать канал обмена данными
		/// </summary>
		/// <param name="reqstr">тип канала - pipe/tcp/etc</param>
		/// <returns></returns>
		/// <param name="srcinfo">информация из конфиг файла для организации соединения по TCP\IP</param>
		/// <returns></returns>
		public IProviderCustomer CreateProviderConsumerChanel(string reqstr, XElement srcinfo, IConfiguration srcCfg)
		{
			IProviderCustomer provconsch = null;
			try
			{

				switch (reqstr)
				{
                    case "WCF":
					case "wcf":
                        provconsch = new ClientServerOnWCF(srcinfo);
						break;
					case "pipe":
						provconsch = new ClientServerOnPipes();
						break;
                    case "TCPServer":
					case "tcp":
						provconsch = new ClientServerOnTCPSockets(srcinfo);
						break;
					case "udp_MOA":
						provconsch = new ClientServerOn_MOA_UDPSockets(srcinfo);
						// создаем экземпляры классов очереди и разборщика пакетов
						PacketQueque PQueque = new PacketQueque();
						provconsch.NetPackQ = PQueque;
						IPacketParser PParser = (IPacketParser)new PacketParser_udp_MOA();
						PParser.Init(srcCfg);
						PQueque.packetAppearance += new PacketAppearance(PParser.byteQueque_packetAppearance);
						break;						
					default:
						throw new Exception(string.Format("ProviderCustomerFactory.cs - {0} - неизвестный запрашиваемый тип канала обмена данными.", reqstr));
				}
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}


			return provconsch;
		}
	}
}
