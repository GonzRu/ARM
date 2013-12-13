/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс для реализации асинхронного обмена по протоколу UDP (MOA)
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ProviderCustomerExchangeLib\ClientServerOn_MOA_UDPSockets.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 23.09.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации: 
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Xml.Linq;
using NetManager;

namespace ProviderCustomerExchangeLib
{
	public class ClientServerOn_MOA_UDPSockets : IProviderCustomer
	{
		#region События
		#endregion

		#region Свойства
		PacketQueque netPackQ;
		public PacketQueque NetPackQ
		{
			set
			{
				netPackQ = value;
			}
			get
			{
				return netPackQ;
			}
		}
		#endregion

		#region public
		#endregion

		#region private
		/// <summary>
		/// массив для входных пакетов
		/// </summary>
		//ArrayForExchange arrForReceiveData;
		/// <summary>
		/// порт для получения данных от сервера
		/// по UDP
		/// </summary>
		int udpserver_port = 0;
		private ChatUdpListener _listener;
		/// <summary>
		/// StringBuilder для формирования ip-адреса UDP-пакетов
		/// </summary>
		StringBuilder adr = new StringBuilder();
		/// <summary>
		/// список новеров фк и их ip-адресов
		/// </summary>
		Dictionary<string, int> dictECUIPAdresses = new Dictionary<string, int>();
		/// <summary>
		/// поток в памяти для приема информации последовательностей, формируемых ФК
		/// </summary>
		private BinaryWriter foutMemCurrent = new BinaryWriter(new MemoryStream());
		#endregion

		#region конструктор(ы)
		/// <summary>
		/// конструктор класса для обмена по udp 
		/// c  ист. данных МОА
		/// </summary>
		/// <param name="srcinfo">инф для настройки соединения tcp\ip</param>
		public ClientServerOn_MOA_UDPSockets(XElement srcinfo)
		{
			CreateConnect2Server(srcinfo);

			//arrForReceiveData = new ArrayForExchange();
			//arrForReceiveData.packetAppearance += new ByteArrayPacketAppearance(arrForReceiveData_packetAppearance);
		}
		#endregion					

		#region public-методы
		#endregion

		#region public-методы реализации интерфейса IProviderCustomer
		/// <summary>
		/// событие появления данных на входе клиента(потребителя)
		/// </summary>
		public event ByteArrayPacketAppearance OnByteArrayPacketAppearance;
        /// <summary>
        /// событие потери связи с DataServer
        /// </summary>
        public event DSCommunicationLoss OnDSCommunicationLoss;

		/// <summary>
		/// посылка данных поставщику
		/// </summary>
		/// <param name="pq"></param>
		public void SendData(byte[] pq)
		{
			//Send(client, pq);
		}
        /// <summary>
        /// посылка запроса на
        /// осциллограмму поставщику данных
        /// </summary>
        /// <param name="pq"></param>
        public void SendOcsReq(byte[] pq)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// посылка запроса на
        /// архивные данные
        /// </summary>
        /// <param name="pq"></param>
        public void SendArhivReq(byte[] pq)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// послать запрос на 
        /// выполнение команды
        /// </summary>
        /// <param name="pq"></param>
        public void SendRunCMD(byte[] pq)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        #endregion

		#region private-методы
		/// <summary>
		/// установка связи с сервером
		/// </summary>
		private void CreateConnect2Server(XElement srcinfo)
		{
			ReadCFGInfo(srcinfo);

			CreateConnection();
		}

		/// <summary>
		/// читаем настройки соединения из конф файла
		/// </summary>
		private void ReadCFGInfo(XElement srcinfo)
		{
			try
			{
				//прочитаем номер порта
				if (!int.TryParse(srcinfo.Element("CustomiseDriverInfo").Element("Port").Attribute("value").Value, out udpserver_port))
				{
					udpserver_port = 20000;
					TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 110, "Не задано значение порта для входящих соединений TCP-сервера.\n Порт по умолчанию : ." + udpserver_port.ToString());
				}

				// сформируем список с адресами фк, от которых ждем пакетов
				CreateListECUAdresses(srcinfo);
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}

		/// <summary>
		/// Сформировать список адресов ФК
		/// от которых ждем пакеты
		/// </summary>
		/// <param name="srcinfo"></param>
		private void CreateListECUAdresses(XElement srcinfo)
		{
			try
			{
				// путь к файлу конфигурации источника
				string path2SrcPrgDevCfg = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar
												+ "Project" + Path.DirectorySeparatorChar
												+ srcinfo.Attribute("nameSourceDriver").Value + Path.DirectorySeparatorChar
												+ "PrgDevCFG.cdp";

				if (!File.Exists(path2SrcPrgDevCfg))
					throw new Exception("(141) : ClientServerOn_MOA_UDPSockets.cs : CreateListECUAdresses() : Ошибка открытия файла : " + path2SrcPrgDevCfg);

				XDocument xdoc_SrcPrgDevCfg = XDocument.Load(path2SrcPrgDevCfg);

				var xe_xml_ecus = xdoc_SrcPrgDevCfg.Descendants("SourceECU");

				foreach( XElement xe_ecu in xe_xml_ecus )
				{
					dictECUIPAdresses.Add(xe_ecu.Attribute("fcadr").Value,int.Parse(xe_ecu.Attribute("numFC").Value) );
				}
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}

		/// <summary>
		/// создать соединение
		/// </summary>
		private void CreateConnection()
		{
			try
			{
				_listener = new ChatUdpListener((int)udpserver_port);
			}
			catch
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 110, "ClientServerOn_MOA_UDPSockets.cs : ClientServerOn_MOA_UDPSockets.CreateConnection() : Неправильный номер UDP-порта для получения данных от ФК: " + udpserver_port.ToString());
				
				return;
			}

			_listener.NewMessage += new EventHandler(_listener_NewMessage);
		}

		/// <summary>
		/// Обработчик события UdpListener.NewMessage -
		/// выделение пакета, ip-адрес в начало пакета, 
		/// пакет на обработку
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _listener_NewMessage(object sender, EventArgs e)
		{
			byte[] ipadrinbyte;	// адрес фк от которого пришел пакет

			try
			{
				#region пакет от нужного ФК?
				adr.Length = 0;
				adr.Append((e as NewMessage).EndPoint.ToString());
				char[] delim = { ':' };

				string[] stdelim = (adr.ToString()).Split(delim);
				adr.Length = 0;
				adr.Append(stdelim[0]);

				IPAddress ipudpserver = IPAddress.Loopback;
				// сворачиваем адрес и в начало пакета - передаем на верхний уровень
				if (!IPAddress.TryParse(adr.ToString(), out ipudpserver))
				{
					TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 163, "ClientServerOn_MOA_UDPSockets.cs : ClientServerOn_MOA_UDPSockets.CreateConnection() : Неправильный ip-адрес: " + adr.ToString());
					return;
				}

				if (!dictECUIPAdresses.ContainsKey(ipudpserver.ToString()))
					return;
				else
					ipadrinbyte = ipudpserver.GetAddressBytes();
				#endregion

				byte[] dataNetRaw = (e as NewMessage).Message;

				BinaryReader fin = new BinaryReader(new MemoryStream(dataNetRaw));
				fin.BaseStream.Position = 0;
				if (fin.BaseStream.Length == 0)
					return;

				// читаем последовательности из потока: читаем заголовок первой последовательности , потом ее пакеты.
				// проверка номера протокола 0x7aa7 (для 304 и вырицы) и 0xnna7 (для более поздних проектов, nn - номер фк):
				byte[] protocol = new byte[2];
				protocol = fin.ReadBytes(2);

				// время пакета и данных в нем - его мы бераем в качестве метки времени тегов
				UInt32 m_uiTemp = fin.ReadUInt32();		// читаем количество секунд - по этому времени ФК осуществляем синхронизацию
				byte uiTemp2 = fin.ReadByte();           // читаем количество сотых долей от начала секунды

				// номер последовательности
				ushort usNumSeq = fin.ReadByte();
				ushort usNumMes = fin.ReadUInt16();	// номер сообщения

				// далее можно читать пакеты из тела сообщения
				ushort usLenPack;
				do
				{
					// читаем длину очередного фрагмента
					usLenPack = fin.ReadUInt16();	// число байт в очередном фрагменте исходного пакета

					if (usLenPack == 0 || (fin.PeekChar() == -1))   // если длина пакета 0 или поток пустой, то выходим
						break;

					// сдвинулись обратно чтобы вернуться к длине
					fin.BaseStream.Position -= 2;

					//читать тело пакета с длиной usLenPack байт
					//byte[] bufPack = new byte[usLenPack];
					//bufPack = fin.ReadBytes(usLenPack);

					//foutMemCurrent.Write(bufPack, 0, usLenPack);	// - 2
					// записать метку времени в поток за всеми данными, не учитывая ее в длине usLenPack
					//foutMemCurrent.Write(m_uiTemp);
					//foutMemCurrent.Write(uiTemp2);

					byte[] copyBlockDev = new byte[usLenPack + 5]; //5 - метка времени  lenpack
					fin.Read(copyBlockDev, 0, usLenPack);	// читаем пакет lenpack в copyBlockDev
					//fin.Read(copyBlockDev, usLenPack, 5);

					// формируем метку времени в copyBlockDev
					Buffer.BlockCopy(BitConverter.GetBytes(m_uiTemp),0,copyBlockDev,usLenPack,4);
					copyBlockDev[usLenPack + 4] = uiTemp2;					

					// и ставим в очередь на обработку
					NetPackQ.Add(copyBlockDev);

				} while (true);

				// даем команду обработать накопивлиеся пакеты в очереди
				NetPackQ.ParsePackInQueque();

				//byte[] dataNet = new byte[foutMemCurrent.BaseStream.Length + 4];//dataNetRaw.Length

				//foutMemCurrent.BaseStream.Position = 0;

				//foutMemCurrent.BaseStream.Read(dataNet,4,(int)foutMemCurrent.BaseStream.Length);

				////Buffer.BlockCopy(foutMemCurrent.BaseStream., 0, dataNet, 4, dataNetRaw.Length); // копируем оставляя место под номер фк

				//Buffer.BlockCopy(BitConverter.GetBytes((int)dictECUIPAdresses[adr.ToString()]), 0, dataNet, 0, 4);

				//ParseNetPacket(dataNet);
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}

		///// <summary>
		///// отправить пакет -> конфигурации для обработки
		///// </summary>
		///// <param name="ms"></param>
		//private void ParseNetPacket(byte [] datap)
		//{
		//    try
		//    {
		//        arrForReceiveData.Add(datap);
		//    }
		//    catch (Exception ex)
		//    {
		//        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
		//    }
		//}

		/// <summary>
		/// закрыть соединение
		/// </summary>
		/// <param name="tc"></param>
		private void CloseConnection(Socket tc)
		{
		}

		private void Send(Socket client, byte[] byteData)
		{
			try
			{
				//client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				//CloseConnection(client);
				//tmrReconnectTCPClientToTCPServer.Start();
			}
		}

		//void arrForReceiveData_packetAppearance(byte[] pq)
		//{
		//    try
		//    {
		//        if (OnByteArrayPacketAppearance != null)
		//            OnByteArrayPacketAppearance(pq);
		//    }
		//    catch (Exception ex)
		//    {
		//        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
		//    }
		//}
		#endregion
	}
}
