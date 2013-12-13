/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс для реализации асинхронного обмена по протоколу TCP
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ProviderCustomerExchangeLib\ClientServerOnTCPSockets.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 26.08.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется материал статьи-http://msdn.microsoft.com/en-us/library/bew39x2a.aspx
 *#############################################################################*/

using System;
using System.IO;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Xml.Linq;

namespace ProviderCustomerExchangeLib
{
	/// <summary>
	/// Состояние сокета
	/// </summary>
	public class StateObject
	{
		/// <summary>
		/// Клиентский сокет
		/// </summary>
		public Socket workSocket = null;
		/// <summary>
		/// размер буфера для получения фрагмента данных пакета
		/// </summary>
		public const int lenTmpBuf = 100;
		/// <summary>
		/// буфер для получения фрагмента данных пакета
		/// </summary>
        public byte[] buffer = new byte[lenTmpBuf];
		/// <summary>
		/// поток для сборки полного пакета
		/// </summary>
		public MemoryStream msNetPacket = new MemoryStream();
        /// <summary>
        /// общая длина получаемых данных
        /// </summary>
        public int lendata = 0;
        /// <summary>
        /// накопистельная длина получаемых данных
        /// </summary>
        public int lenReceivedData = 0;
	}

	public class ClientServerOnTCPSockets : IProviderCustomer
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
		/// клиентский сокет
		/// </summary>
		Socket client;
		/// <summary>
		/// класс состояния клиента
		/// </summary>
		//StateObject state;
		/// <summary>
		/// ip для подключения к серверу
		/// </summary>
		IPAddress tcpserver_ip = IPAddress.Loopback;
		/// <summary>
		/// порт для подключения к серверу
		/// </summary>
		int tcpserver_port = 0;
		/// <summary>
		/// таймер перезапуска
		/// </summary>
		System.Timers.Timer tmrReconnectTCPClientToTCPServer;
		/// <summary>
		/// тайм на ошибки соединения - перезапуск соединения
		/// </summary>
		double rwTimeout = 5000;
        ///// <summary>
        ///// размер пакета для обмена с источником данных
        ///// </summary>
        //int packetExchangeSize = 1024;
		/// <summary>
		/// массив для входных пакетов
		/// </summary>
		ArrayForExchange arrForReceiveData;
		/// <summary>
		/// массив для выходных пакетов
		/// </summary>
		ArrayForExchange arrForSendData = new ArrayForExchange();
		#endregion

		#region конструктор(ы)		
		/// <summary>
		/// конструктор класса для обмена по tcp\ip
		/// </summary>
		/// <param name="srcinfo">инф для настройки соединения tcp\ip</param>
		public ClientServerOnTCPSockets(XElement srcinfo)
		{
			CreateConnect2Server(srcinfo);

			arrForReceiveData = new ArrayForExchange();
			arrForReceiveData.packetAppearance += new ByteArrayPacketAppearance(arrForReceiveData_packetAppearance);
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
		/// посылка данных поставщику
		/// </summary>
		/// <param name="pq"></param>
		public void SendData(byte[] pq)
		{
            if(client.Connected)
			    Send(client, pq) ;
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
				//прочитаем ip-адрес сервера
                if (!IPAddress.TryParse(srcinfo.Element("IPAddress").Attribute("value").Value, out tcpserver_ip))//.Element("CustomiseDriverInfo")
				{
					tcpserver_ip = IPAddress.Loopback;
					Console.WriteLine();
					TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 126, "Не задано значение ip-адреса сервера .\n IP-адрес по умолчанию : ." + tcpserver_ip.ToString());
				}

				//прочитаем номер порта
                if (!int.TryParse(srcinfo.Element("Port").Attribute("value").Value, out tcpserver_port))//.Element("CustomiseDriverInfo")
				{
					tcpserver_port = 28080;
					TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 133, "Не задано значение порта для входящих соединений TCP-сервера.\n Порт по умолчанию : ." + tcpserver_port.ToString());
				}

				// инициализируем таймер для ошибок соединения
                if (!double.TryParse(srcinfo.Element("RWTimeout").Value, out rwTimeout))//.Element("CustomiseDriverInfo")
				{
					rwTimeout = 5000;
					TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 140, "Не задано значение таймаута для ошибок соединений.\n Значение по умолчанию : ." + rwTimeout.ToString());
				}

				// инициализируем размер пакета для обмена с источником
                //if (!int.TryParse(srcinfo.Element("PacketExchangeSize").Value, out packetExchangeSize))//.Element("CustomiseDriverInfo")
                //{
                //    packetExchangeSize = 1024;
                //    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 140, "Не задано значение размера пакета для обмена с источником.\n Значение по умолчанию : ." + packetExchangeSize.ToString());
                //}

				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 194, 
					string.Format("{0} : {1} : {2} : Информация о соединении : tcpserver_ip = {3}, tcpserver_port = {4}, таймер реакции на ошибки соединения = {5}",
					DateTime.Now.ToString(), "ClientServerOnTCPSockets.cs", "ReadCFGInfo()", tcpserver_ip, tcpserver_port, rwTimeout)); //, размер пакетов для обмена = {6}, packetExchangeSize
				
				tmrReconnectTCPClientToTCPServer = new System.Timers.Timer();
				tmrReconnectTCPClientToTCPServer.Elapsed += new System.Timers.ElapsedEventHandler(tmrReconnectTCPClientToTCPServer_Elapsed);
				tmrReconnectTCPClientToTCPServer.Interval = rwTimeout;
				tmrReconnectTCPClientToTCPServer.Stop();
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}

		/// <summary>
		/// создать соединение
		/// </summary>
		private void CreateConnection()
		{
			if (client != null)
				CloseConnection(client);

			try
			{
				IPEndPoint remoteEP = new IPEndPoint(tcpserver_ip, tcpserver_port);//

				client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				client.BeginConnect(remoteEP , new AsyncCallback(ConnectCallback), client);

				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 236, string.Format("{0} : {1} : {2} : Запрос на установку соединения : tcpserver_ip = {3}; tcpserver_port = {4}", DateTime.Now.ToString(), "ClientServerOnTCPSockets.cs", "CreateConnection()", tcpserver_ip, tcpserver_port));
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 258, string.Format("{0} : {1} : {2} : Ошибка запроса на установку соединения : tcpserver_ip = {3}; tcpserver_port = {4}", DateTime.Now.ToString(), "ClientServerOnTCPSockets.cs", "CreateConnection()", tcpserver_ip, tcpserver_port));
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				CloseConnection(client);
				tmrReconnectTCPClientToTCPServer.Start();
			}
		}

		/// <summary>
		/// закрыть соединение
		/// </summary>
		/// <param name="tc"></param>
		private void CloseConnection(Socket tc)
		{
            try
            {
                if (tc != null)
                {
                    //if (tc.Connected)
                    //{
                    //    tc.Shutdown(SocketShutdown.Both);
                    //    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 275, string.Format("{0} : {1} : {2} : Соединение закрыто.", DateTime.Now.ToString(), "ClientServerOnTCPSockets.cs", "CloseConnection()"));
                    //}

                    tc.Close();
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 275, string.Format("{0} : {1} : {2} : Соединение закрыто.", DateTime.Now.ToString(), "ClientServerOnTCPSockets.cs", "CloseConnection()"));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
		}

		/// <summary>
		/// асинхронное ожидание соединения
		/// </summary>
		/// <param name="ar"></param>
		private void ConnectCallback(IAsyncResult ar)
		{
			try
			{
				Socket client = (Socket)ar.AsyncState;

				client.EndConnect(ar);

				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 296, string.Format("{0} : {1} : {2} : Соединение установлено.", DateTime.Now.ToString(), "ClientServerOnTCPSockets.cs", "ConnectCallback()"));

				// соединение выполнено, ждем пакет
				ReceivePacket(client);
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				CloseConnection(client);
				tmrReconnectTCPClientToTCPServer.Start();
			}
		}

		/// <summary>
		/// асинхронное чтение пакета
		/// </summary>
		/// <param name="client"></param>
		private void ReceivePacket(Socket client)
		{
			try
			{
				StateObject state = new StateObject();
				state.workSocket = client;
    			state.msNetPacket = new MemoryStream();

				client.BeginReceive(state.buffer, 0, state.buffer.Length, 0, new AsyncCallback(ReceiveCallback), state);
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				CloseConnection(client);
				tmrReconnectTCPClientToTCPServer.Start();
			}
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			StateObject state = (StateObject)ar.AsyncState;

			Socket client = state.workSocket;

			MemoryStream ms = new MemoryStream();

			try
			{
				int bytesRead = client.EndReceive(ar);

                if (bytesRead < 100)
                    bytesRead = bytesRead;

				if (bytesRead > 0)
				{
					// данных м.б. много поэтому здесь нужен код для из вычитывания из сокета
					ms.Write(state.buffer, 0, bytesRead);                    

                    BinaryReader br = new BinaryReader(ms);
                    ms.Position = 0;

                    // определяем длину данных пакета клиента не учитывающую длину самого пакета
                    state.lendata = 0;
                    state.lendata = br.ReadInt32();

                    // определяемся с объемом читаемых данных
                    if (state.lendata > bytesRead - 4)
                    {
                        // запишем содержимое посылки исключая длину
                        state.msNetPacket.Write(br.ReadBytes(bytesRead - 4), 0, bytesRead - 4);

                        // данные придется читать в несколько заходов
                        // state.msfullrezbuffer.Write(state.buffer,0,bytesRead-4);
                        state.lenReceivedData = bytesRead - 4;
                        client.BeginReceive(state.buffer, 0,  StateObject.lenTmpBuf, 0, new AsyncCallback(ReadCallbackChunkOfData), state);
                    }
                    else
                    {
                        // запишем содержимое посылки исключая длину
                        state.msNetPacket.Write(br.ReadBytes(state.lendata), 0, state.lendata);

                        /*
                         * все данные прочитаны
                         *  на обработку 
                         * ... и чистим
                         */
                        state.msNetPacket.Position = 0;
                        ParseNetPacket(state.msNetPacket);

                        if ((bytesRead - 4) == 96)
                            bytesRead = bytesRead;

                        Console.WriteLine("ReceiveCallback() : Received {0} bytes from server.", state.msNetPacket.Length);
                        ClearStateData(state);
                    }
                }
				else
				{
                    // все данные получены - придем ли мы сюда и чего делать
                    // а пока
                    // чистим буфера для последующего приема данных                    
                    ClearStateData(state);					
				}
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				CloseConnection(client);
				tmrReconnectTCPClientToTCPServer.Start();
			}
		}

        /// <summary>
        /// получение порций данных длинного запроса
        /// </summary>
        /// <param name="ar"></param>
        private void ReadCallbackChunkOfData(IAsyncResult ar)
        {
            StateObject state = null;

            try
            {
                // получить объект состояния и указатель на сокет
                state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // прочитать данные с клиентского сокета
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // запишем содержимое посылки в результат исключая длину
                    state.msNetPacket.Write(state.buffer, 0, bytesRead);

                    state.lenReceivedData += bytesRead;

                    // на получение следующей порции данных запроса
                    if (state.lendata > state.lenReceivedData)
                    {
                        // вычитываем дальше
                        if (state.lendata - state.lenReceivedData > StateObject.lenTmpBuf)
                            handler.BeginReceive(state.buffer, 0, StateObject.lenTmpBuf, 0, new AsyncCallback(ReadCallbackChunkOfData), state);
                        else
                            handler.BeginReceive(state.buffer, 0, state.lendata - state.lenReceivedData, 0, new AsyncCallback(ReadCallbackChunkOfData), state);
                    }
                    else
                    {
                        /*
                         * все данные прочитаны
                         *  на обработку 
                         * ... и чистим
                         */
                         state.msNetPacket.Position = 0;
                         ParseNetPacket(state.msNetPacket);
                         Console.WriteLine("ReadCallbackChunkOfData() : Received {0} bytes from server.", state.msNetPacket.Length);
                         ClearStateData(state);
                    }
                }
                else // все данные прочитаны
                    ClearStateData(state);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                CloseConnection(client);
                tmrReconnectTCPClientToTCPServer.Start();
            }
        }
        /// <summary>
        /// Очистить рабочие структуры данных
        /// </summary>
        private void ClearStateData(StateObject state)
        {
            try
            {
                // чистим буфера для последующего приема данных                    
                state.lenReceivedData = 0;
                state.lendata = 0;
                state.buffer = new byte[StateObject.lenTmpBuf];
                state.msNetPacket = new MemoryStream();

                state.workSocket.BeginReceive(state.buffer, 0, StateObject.lenTmpBuf, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                CloseConnection(client);
                tmrReconnectTCPClientToTCPServer.Start();
            }
        }

		/// <summary>
		/// разобрать буфер на пары длина+пакет и отправить конфигурации на дальнейший разбор
		/// </summary>
		/// <param name="ms"></param>
		private void ParseNetPacket(MemoryStream ms)
		{
			ms.Position = 0;
			BinaryReader br = new BinaryReader(ms);
			
			int lenfragment = 0;
			int lenfull = 0;
			
			try
			{
                //do
                //{
                    //lenfragment = br.ReadInt32();
                    //byte[] buf = new byte[lenfragment];
                    //buf = br.ReadBytes(lenfragment);
                    //arrForReceiveData.Add(buf);
                    //lenfull += lenfragment + 4;
                //} while (lenfull < ms.Length);

                arrForReceiveData.Add(ms.ToArray());

			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
                throw new Exception(ex.Message,ex);
			}
		}

        /// <summary>
        /// функция посылки запроса DataServer
        /// </summary>
        /// <param name="client"></param>
        /// <param name="byteData"></param>
		private void Send(Socket client, byte[] byteData)
		{
			try
			{
                /*
                 * оборачиваем запрос в длину - 4 байта+
                 * 
                 */
                 int byteData_Length = byteData.Length;
                 MemoryStream msreq = new MemoryStream();
                 BinaryWriter bw = new BinaryWriter(msreq);
                 bw.Write(BitConverter.GetBytes(byteData_Length));
                 bw.Write(byteData);

                 if (client.Connected)
                    client.BeginSend(msreq.ToArray(), 0, (int)msreq.Length, 0, new AsyncCallback(SendCallback), client);
                 else
                     throw new Exception(@"(516) : X:\Projects\01_HMIWinFormsClient\ProviderCustomerExchangeLib\ClientServerOnTCPSockets.cs : Send() : Отсутсвует соединение с сокетом");
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				CloseConnection(client);
				tmrReconnectTCPClientToTCPServer.Start();
			}
		}

		private void SendCallback(IAsyncResult ar)
		{
            int bytesSent = 0;

			try
			{
				Socket client = (Socket)ar.AsyncState;

                if (client.Connected)
                {
                    bytesSent = client.EndSend(ar);
                    Console.WriteLine("Send {0} bytes to server.", bytesSent);
                }
                //else
                //    throw new Exception(@"(532) : X:\Projects\01_HMIWinFormsClient\ProviderCustomerExchangeLib\ClientServerOnTCPSockets.cs : SendCallback() : Отсутсвует соединение с сокетом");
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				CloseConnection(client);
				tmrReconnectTCPClientToTCPServer.Start();
			}
		}

		/// <summary>
		/// срабатывание таймера на перезапуск соединения
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void tmrReconnectTCPClientToTCPServer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			tmrReconnectTCPClientToTCPServer.Stop();
			CreateConnection();
		}

		void arrForReceiveData_packetAppearance(byte[] pq)
		{
			try
			{
				if (OnByteArrayPacketAppearance != null)
					OnByteArrayPacketAppearance(pq);
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw new Exception(ex.Message,ex);
			}
		}
		#endregion
	}
}
