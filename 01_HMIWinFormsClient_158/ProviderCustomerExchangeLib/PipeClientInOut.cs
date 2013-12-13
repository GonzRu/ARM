/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс для организации обмена посредством каналов
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\uvs_SAF\PipeClientInOut.cs                                        
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 15.07.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Threading;
using System.ComponentModel;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace ProviderCustomerExchangeLib
{
   public class PipeClientInOut
   {
	   #region События
	   #endregion

	   #region Свойства
		   /// <summary>
		  /// состояние соединения по каналу
		  /// </summary>
		  public bool IsPipeConnected
		  {
			 get { return isPipeConnected; }
			 set { isPipeConnected = value; }
		  }
		  bool isPipeConnected;

		  /// <summary>
		  /// признак ошибки при обмене
		  /// </summary>
		  public bool IsExchangeError
		  {
			 get { return isExchangeError; }
			 set { isExchangeError = value; }
		  }
		  bool isExchangeError;      
	   #endregion

	   #region public
      /// <summary>
      /// ссылка на массив для принимаемых пакетов
      /// </summary>
      public ArrayForExchange arrForReseivePackets;
	   #endregion

	   #region private
      /// <summary>
      /// интервал ожидания соединеня с pipe-сервером
      /// </summary>
      int connectInterval;
      string pipenameInOut = string.Empty;

      NamedPipeClientStream pipeClientInOut;

		/// <summary>
		/// направление канала
		/// </summary>
	  string chanelDirection = string.Empty;

      /// <summary>
      /// таймер на перезапуск соединения с pipe-сервером
      /// </summary>
      System.Timers.Timer tmrReRunClient;
	   #endregion

	   #region конструктор(ы)
	   #endregion


	   #region public-методы
	   #endregion

	   #region private-методы
	   #endregion




      public PipeClientInOut(string pipeName) 
      {
         // инициализируем таймер на перезауск соединения с pipe-сервером
         tmrReRunClient = new System.Timers.Timer();
         tmrReRunClient.Interval = 10000;
         tmrReRunClient.Elapsed += new System.Timers.ElapsedEventHandler(tmrReRunClient_Elapsed);
         tmrReRunClient.Stop();

         // имя канала
         pipenameInOut = pipeName;
         isPipeConnected = false;
      }

      /// <summary>
      /// старт pipe-клиента
      /// </summary>
      /// <param Name="intervalForConnect">интервал ожидания соединения с pipe-сервером</param>
      public void Start(int intervalForConnect, string chaneldirection)
      {
         connectInterval = intervalForConnect;
		 chanelDirection = chaneldirection;

         try
         {
            // создаем канал, сообщаем о нем уровню обмена
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 100, DateTime.Now.ToString() + " (100) : HMI.PipeClientInOut.cs : Start : создание канала : " );

			switch (chaneldirection)
			{
				case "In":
					pipeClientInOut = new NamedPipeClientStream(".", pipenameInOut, PipeDirection.InOut, PipeOptions.Asynchronous);
					break;
				case "Out":
					pipeClientInOut = new NamedPipeClientStream(".", pipenameInOut, PipeDirection.InOut, PipeOptions.Asynchronous);
					break;
				default:
					break;
			}

           //ждем коннекта
            WaitForPipeConnect(intervalForConnect);
          }
         catch (Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 108, DateTime.Now.ToString() + " (108) (== ОШИБКА ===) : HMI_MT.PipeClientInOut.cs : Start : ошибка при установке соединения с pipe-сервером : " + ex.Message);
            ReStartPipeClient();
         }
      }

      /// <summary>
      /// закрыть канал при выходе из приложения
      /// </summary>
      public void CloseAndDisconnectPipeClient()
      {
         try
         {
            // формируем длину пакета перед данными пакета - длина == -1, 
            // т.е. служ пакет
            byte[] pq_with_len = new byte[1 + 4];
            int lenService = -1;
            byte[] lenpackEx = BitConverter.GetBytes(lenService);
            Buffer.BlockCopy(lenpackEx, 0, pq_with_len, 0, lenpackEx.Length);
            Buffer.BlockCopy(new byte[1] { 0x01 }, 0, pq_with_len, lenpackEx.Length, 1);
            pipeClientInOut.BeginWrite(pq_with_len, 0, (int)pq_with_len.Length, new AsyncCallback(AsyncPipeWrite), null);
         }catch(Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 123, DateTime.Now.ToString() + " (123) (== ОШИБКА ===) : HMI.PipeClientInOut.cs : CloseAndDisconnectPipeClient() : ОШИБКА : " + ex.Message);
         }
      }

      private void WaitForPipeConnect(int intervalForConnect)
      {
         //ждем коннекта
         try
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 117, DateTime.Now.ToString() + " (117) : HMI.PipeClientInOut.cs : WaitForPiPeConnect : ожидание соединения с pipe-сервером в течении : " + intervalForConnect.ToString() + "ms");
            
            pipeClientInOut.Connect(intervalForConnect);

            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 121, DateTime.Now.ToString() + " (121) : HMI.PipeClientInOut.cs : WaitForPiPeConnect : соединение с pipe-сервером установлено: ");

			pipeClientInOut.ReadMode = PipeTransmissionMode.Message;//;.Byte

            // запустим процесс обмена - начнем с чтения длины пакета
            ReadLenPack();
         }
         catch
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 152, DateTime.Now.ToString() + " (152) (== ОШИБКА ===) : HMI.PipeClientInOut.cs : WaitForPiPeConnect : ошибка при установке соединения с pipe-сервером : время ожидания истекло.");
            ReStartPipeClient();
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 154, DateTime.Now.ToString() + " (154) : HMI.PipeClientInOut.cs : WaitForPiPeConnect : СТАРТ-таймер переустановки соединения с pipe-сервером : ReStartPipeClient()");
         }
      }

      void tmrReRunClient_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {
         tmrReRunClient.Stop();
         TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 107, DateTime.Now.ToString() + " (107) : HMI.PipeClientInOut.cs : tmrReRunClient_Elapsed : СТОП-таймер переустановки соединения с pipe-сервером : tmrReRunClient.Stop()");

         TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 168, DateTime.Now.ToString() + " (168) : HMI.PipeClientInOut.cs : ReStartPipeClient() : запуск процесса соединения с pipe-сервером : connectInterval = " + connectInterval.ToString());
         Start(connectInterval, chanelDirection);
      }

      /// <summary>
      /// рестарт соединения с pipe-сервером
      /// </summary>
      private void ReStartPipeClient() 
      {
         if (pipeClientInOut != null)
            pipeClientInOut.Close();
         
         tmrReRunClient.Start();
         TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 176, DateTime.Now.ToString() + " (176) : HMI.PipeClientInOut.cs : ReStartPipeClient : СТАРТ-таймер переустановки соединения с pipe-сервером : tmrReRunClient.Start()");
      }

      byte[] bytelenpack = new byte[4];
      int lenpack = 0;  // длина пакета от сервера (3->2)

      /// <summary>
      /// чтение длины входящего пакета
      /// </summary>
      void ReadLenPack()
      {
         // читаем длину пакета
         try
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 162, DateTime.Now.ToString() + " (162) : HMI_MT.PipeClientInOut.cs : ReadLenPack() : Начало чтения длины пакета : ");

            // читаем длину пакета
            pipeClientInOut.BeginRead(bytelenpack, 0, 4, new AsyncCallback(AsyncPipeBeginReadLenData), null);
         }
         catch (Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 169, DateTime.Now.ToString() + " (169) (== ОШИБКА ===) : HMI_MT.PipeClientInOut.cs : ReadLenPack() : ошибка при чтении длины пакета : " + ex.Message);

            ReStartPipeClient();
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 172, DateTime.Now.ToString() + " (172) : HMI.PipeClientInOut.cs : ReadLenPack() : СТАРТ-таймер переустановки соединения с pipe-сервером : ReStartPipeClient()");
         }
      }

      /// <summary>
      /// асинхронное чтение длины пакета от сервера
      /// </summary>
      /// <param Name="result"></param>
      void AsyncPipeBeginReadLenData(IAsyncResult result)
      {         
         try
         {
            pipeClientInOut.EndRead(result);

            lenpack = BitConverter.ToInt32(bytelenpack, 0);

            // читаем пакет 
			if (lenpack != 0)
			{
				MemoryStream msread = ReadStream(lenpack);

				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 191, DateTime.Now.ToString() + " (101) : HMI_MT.PipeClientInOut.cs : AsyncPipeBeginReadLenData() : прочитали пакет - длина = " + msread.Length.ToString());

				// d
				// размещаем прочитанный пакет в массиве байт
				arrForReseivePackets.Add(msread.ToArray());				
			}

			ReadLenPack();
         }
         catch (Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 208, DateTime.Now.ToString() + " (208) (== ОШИБКА ===) : HMI_MT.PipeClientInOut.cs : AsyncPipeBeginReadLenData : ошибка при попытке чтения длины пакета : /n" + ex.Message);
             ReStartPipeClient();
         }
      }

      /// <summary>
      /// буфер для приема пакетов от pipe-сервера
      /// </summary>
      byte[] buffer = new byte[0x1000];

      private MemoryStream ReadStream(int lenpacket)
      {
		  try
		  {

         if (!pipeClientInOut.IsConnected)
         {
            // ПЕРЕЗАПУСК соединения 
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 246, DateTime.Now.ToString() + " (246) : HMI_MT.PipeClientInOut.cs : ReadStream :  pipeClientInOut.IsConnected = false.");

            ReStartPipeClient();
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 236, DateTime.Now.ToString() + " (236) : HMI.PipeClientInOut.cs : ReadStream : СТАРТ-таймер переустановки соединения с pipe-сервером : ReStartPipeClient()");

            return null;
         }

         MemoryStream ms = new MemoryStream();
         TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 251, DateTime.Now.ToString() + " (251) : HMI_MT.PipeClientInOut.cs : ReadStream :  получение пакета данных : заявленная длина = " + lenpacket.ToString());

         // теперь читаем блоками по 4 Кб

         do
         {
            if (lenpacket > 0)
               ms.Write(buffer, 0, pipeClientInOut.Read(buffer, 0, buffer.Length));
            else
            {
               TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 260, DateTime.Now.ToString() + " (260) : HMI_MT.PipeClientInOut.cs : ReadStream :  получение пакета данных : lenpacket = 0, переход на чтение длины след. пакета. " );
               break;
            }
         } while (!pipeClientInOut.IsMessageComplete);

         return ms;
		  }
		  catch (Exception ex)
		  {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			  return null;
		  }
      }

      /// <summary>
      /// вызывается при появлении пакетов в очереди для передачи по каналу pipe-серверу
      /// обратный ход пакетов к DataServer
      /// </summary>
      /// <param Name="pq"></param>
      public void byteQueque_packetAppearance(byte[] pq)
      {
         try
         {
            // формируем длину пакета перед данными пакета
			 byte[] pq_with_len = new byte[pq.Length + 4];
			 byte[] lenpackEx = BitConverter.GetBytes(pq.Length);
			 Buffer.BlockCopy(lenpackEx, 0, pq_with_len, 0, lenpackEx.Length);
			 Buffer.BlockCopy(pq, 0, pq_with_len, lenpackEx.Length, pq.Length);
			 pipeClientInOut.BeginWrite(pq_with_len, 0, (int)pq_with_len.Length, new AsyncCallback(AsyncPipeWrite), null);
         }
         catch (Exception err)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 195, DateTime.Now.ToString() + " (195) : HMI_MT.ClientDataForExchange.cs : byteQueque_packetAppearance : неудачная посылка пакета : " + err.Message);
            throw new Exception("Исключение при попытке посылки пакета");
         }
      }

      /// <summary>
      /// конец записи - начало чтения данных от сервера
      /// </summary>
      /// <param Name="result"></param>
      void AsyncPipeWrite(IAsyncResult result)
      {
         TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 1, DateTime.Now.ToString() + " (1) : Точка = 7 -> 6(2) = (PipeClientInOut.cs::AsyncPipeBeginReadLenData()) ");

         try
         {
            pipeClientInOut.EndWrite(result);

            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 294, DateTime.Now.ToString() + " (294) : HMI_MT.ClientDataForExchange.cs : AsyncPipeWrite : " + " пакет послан. Ждем ответ от pipe-сервера. ");

            pipeClientInOut.Flush();

            // читаем длину очередного пакета
            //ReadLenPack();
         }
         catch (Exception err)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 803, DateTime.Now.ToString() + "(803)  (== ОШИБКА ===) : HMI_MT.ClientDataForExchange.cs : AsyncPipeWrite : ошибка : " + err.Message);
            ReStartPipeClient();
         }
      }
   }
}
