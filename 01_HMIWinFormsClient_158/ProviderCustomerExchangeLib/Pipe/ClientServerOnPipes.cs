/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс для организации обмена поставщика и потебителя 
 *				посредством каналов (pipe)
 *                                                                             
 *	Файл                     : ClientServerOnPipes.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ProviderCustomerExchangeLib
{
	public class ClientServerOnPipes : IProviderCustomer
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
		/// Класс чтения из канала для обмена данными
		/// </summary>
		PipeClientInOut pcIORead;
		/// <summary>
		/// Класс записи в канал для обмена данными
		/// </summary>
		PipeClientInOut pcIOWrite;

		/// <summary>
		/// интервал ожидания соединения с pipe-сервером
		/// </summary>
		int intervalForConnect = 5000;

		/// <summary>
		/// массив для входных пакетов - в нее кладет пакеты объект класса канала PipeServerInOut
		/// а читает объект класса ServerDataForExchange
		/// </summary>
		ArrayForExchange arrForReceiveData;

		/// <summary>
		/// массив для выходных пакетов - в нее кладет пакеты объект класса ServerDataForExchange
		/// а читает объект класса канала PipeServerInOut 
		/// </summary>
		ArrayForExchange arrForSendData = new ArrayForExchange();

		/// <summary>
		/// имя канала на получение  данных от источника данных
		/// </summary>
		string pipeNameRead = "PipeWriteForClientCADAC";//"Pipe_ClientCADAC_DataServer";
		/// <summary>
		/// имя канала на отправку данных источнику данных
		/// </summary>
		string pipeNameWrite = "PipeReadForClientCADAC";
		#endregion

		#region конструктор(ы)
		public ClientServerOnPipes()
		{
			CreateConnect2Pipe();
		}
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
			pcIOWrite.byteQueque_packetAppearance(pq);
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

		#region public-методы
		#endregion

		#region private-методы
		private void CreateConnect2Pipe()
		{
			StartPipeClientRead(pipeNameRead, "In");
			StartPipeClientWrite(pipeNameWrite, "Out");
		}
		private void StartPipeClientRead(string pipeName, string chanelDirection)
		{
			TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 232, DateTime.Now.ToString() + " : MainWindow.xaml.cs : StartPipeClientRead :");

			// создаем канал для чтения
			pcIORead = new PipeClientInOut(pipeName);
			pcIORead.Start(intervalForConnect, chanelDirection);

			/* 
			 * создаем объекты классов вх. и вых массивов для того, чтобы класс ServerDataForExchange 
			 * обменивался данными через них
			 * 
			 * arrForReceiveData - массив входных пакетов - в него кладет пакеты объект класса канала PipeServerInOut
			 * а читает объект класса ServerDataForExchange
			 */

			arrForReceiveData = new ArrayForExchange();
			// привязка к событию появления данных в массиве вх пакетовX:\Projects\38_DS4BlockingPrg\DataServer\uvs_SAF\Class1.cs
			//arrForReceiveData.packetAppearance += new ByteArrayPacketAppearance(this.byteQueque_packetAppearance);
			arrForReceiveData.packetAppearance += new ByteArrayPacketAppearance(arrForReceiveData_packetAppearance);
			pcIORead.arrForReseivePackets = arrForReceiveData;
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
			}
		}

		private void StartPipeClientWrite(string pipeName, string chanelDirection)
		{
			TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 254, DateTime.Now.ToString() + " : MainWindow.xaml.cs : StartPipeClientWrite :");

			// создаем канал для записи данных
			pcIOWrite = new PipeClientInOut(pipeName);
			pcIOWrite.Start(intervalForConnect, chanelDirection);

			/* 
			 * создаем объекты классов вх. и вых массивов для того, чтобы класс ServerDataForExchange 
			 * обменивался данными через них
			 * 
			 * arrForSendData - массив выходных пакетов - в него кладет пакеты объект класса канала ClientDataForExchange
			 * а читает объект класса PipeClientInOut и отправлет pipe-серверу
			 */

			arrForSendData = new ArrayForExchange();
			// привязка к событию появления данных в массиве вых пакетов
			arrForSendData.packetAppearance += new ByteArrayPacketAppearance(pcIOWrite.byteQueque_packetAppearance);
		}

		#endregion


	}
}
