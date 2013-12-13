using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace ProviderCustomerExchangeLib
{
   public delegate void ErrorSended(string error_source);
   public delegate void PacketSended();

   /// <summary>
   /// класс организующий отправку данных по TCP-соединению
   /// </summary>
   public class SendBlockData
   {
      public event ErrorSended OnErrorSended;
      /// <summary>
      /// Событие, посылаемое при длительной передаче
      /// (напр. при отправке осциллограммы) с тем чтобы 
      /// сбрасывать счетчик попыток посылки данных count
      /// </summary>
      public event PacketSended OnPacketSended;

      public TcpClient clientSocket;
      NetworkStream writerStream = null;

      /// <summary>
      /// максимальный размер фрагмента отправляемого по сети
      /// </summary>
      const ushort max_size_for_tcpsend = 1000;
      /// <summary>
      /// длина заголовка пакета
      /// </summary>
      const byte packet_header_length = 12;
      /// <summary>
      /// резервный байт
      /// </summary>
      byte reserve = 0;
      /// <summary>
      /// серия пакета - номер серии формируется по кольцу в диапазоне 0-255
      /// </summary>
      ushort Series_number
      {
         get
         {
            return series_number;
         }
         set
         {
            series_number = value;
            series_number = series_number >= 255 ? (ushort)0 : series_number;
         }
      }
      ushort series_number = 0;
      /// <summary>
      /// длина серии - полного 
      /// нефрагмантерованного пакета
      /// </summary>
      uint series_length = 0;
      /// <summary>
      /// номер пакета в серии
      /// </summary>
      ushort series_fragment_number = 0;
      /// <summary>
      /// длина очередного пакета, 
      /// отправляемого в сеть
      /// </summary>
      ushort series_fragment_length = 0;
      /// <summary>
      /// массив для квитка о получении пакета от клиента
      /// </summary>
      byte[] receipt;

      /// <summary>
      /// интервал таймера ожидания операций обмена
      /// </summary>
      public string TCPRWTimeout 
      {
         set 
         {
            if (!int.TryParse(value, out tmr_receipt_interval))
            {
               TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 87, DateTime.Now.ToString() + " (87) : ClientServerDataExchange.cs : TCPRWTimeout : ОШИБКА - попытка установить некорректное значение таймаута на оперции чтения записи по NCH-соединению - " + value);
               tmr_receipt_interval = 10000;
            }
         }
      }
      int tmr_receipt_interval;

      public SendBlockData(TcpClient client)
      {
         clientSocket = client;
      }

      /// <summary>
      /// подготовить среду для обмена
      /// </summary>
      public void PrepareExch()
      {
         writerStream = clientSocket.GetStream();
      }
      /// <summary>
      /// прочитать байт
      /// </summary>
      public void ReadByte()
      {
         try 
         {
            if (writerStream.CanTimeout)
               writerStream.ReadTimeout = writerStream.WriteTimeout = tmr_receipt_interval;

            writerStream.ReadByte();
         }
         catch (Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 115, DateTime.Now.ToString() + " (115) : ClientServerDataExchange.cs : ReadByte() : ОШИБКА " + ex.Message);

            // вызов события о неудачной передаче
            if (OnErrorSended != null)
               OnErrorSended("writerStream");
         }
      }
      /// <summary>
      /// посылка байта
      /// </summary>
      /// <param name="b"></param>
      public void WriteByte(byte b)
      {
         try 
         {
            if (writerStream.CanTimeout)
               writerStream.ReadTimeout = writerStream.WriteTimeout = tmr_receipt_interval;

            writerStream.WriteByte(b);
         }
         catch (Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 136, DateTime.Now.ToString() + " (136) : ClientServerDataExchange.cs : WriteByte() : ОШИБКА " + ex.Message);

            // вызов события о неудачной передаче
            if (OnErrorSended != null)
               OnErrorSended("writerStream");
         }
      }
      /// <summary>
      /// посылка данных - результат посылки через событие
      /// </summary>
      /// <param name="arr4Send"></param>
      public void Write(byte[] arrwithlen)
      {
         Series_number++;
         series_fragment_number = 0;
         series_length = (uint)arrwithlen.Length;

         MemoryStream ms_series = new MemoryStream(arrwithlen);

         if (ms_series.Length == 0)
            SendEmptyPacket();
         else
            SendNonEmptyPacket(ms_series);
      }

      /// <summary>
      /// подготовка непостоянной части заголовка для пустого пакета
      /// </summary>
      private void SendEmptyPacket()
      { 
         byte[] packet = new byte[packet_header_length];
         PrepareConstHeaderPacket(packet);

         series_length = 0;
         series_fragment_number = 0;

         Buffer.BlockCopy(BitConverter.GetBytes(series_length), 0, packet, 4, 4);
         Buffer.BlockCopy(BitConverter.GetBytes(series_fragment_number), 0, packet, 2, 2);

         // вместо длины пакета серии размещаем 0;
         ushort series_fragment_length_0 = 0;
         Buffer.BlockCopy(BitConverter.GetBytes(series_fragment_length_0), 0, packet, 10, 2);

         SendPacket(packet);
      }

      /// <summary>
      /// подготовка постоянной для всех пакетов части заголовка
      /// </summary>
      private void PrepareConstHeaderPacket(byte[] packet)
      {
         packet[0] = packet_header_length;
         packet[1] = reserve;
         Buffer.BlockCopy(BitConverter.GetBytes(Series_number), 0, packet, 2, 2);
         Buffer.BlockCopy(BitConverter.GetBytes(series_length), 0, packet, 4, 4);
      }

      /// <summary>
      /// подготовка непостоянной части заголовка для непустого пакета
      /// </summary>
      private void SendNonEmptyPacket(MemoryStream ms_series)
      {
         byte[] packet = new byte[0];
         series_length = Convert.ToUInt32(ms_series.Length);
         series_fragment_number = 1;

         uint ms_series_Position = 0;
         while ((ms_series.Length - ms_series_Position) > max_size_for_tcpsend)
         {
            packet = new byte[packet_header_length + max_size_for_tcpsend];
            PrepareConstHeaderPacket(packet);

            Buffer.BlockCopy(BitConverter.GetBytes(series_fragment_number), 0, packet, 8, 2);
            series_fragment_length = max_size_for_tcpsend;
            Buffer.BlockCopy(BitConverter.GetBytes(series_fragment_length), 0, packet, 10, 2);

            Buffer.BlockCopy(ms_series.ToArray(),(int) ms_series_Position, packet, packet_header_length, series_fragment_length);

            SendPacket(packet);

            ms_series_Position += max_size_for_tcpsend;
            series_fragment_number++;
         }

         // если нужно, дописываем
         if ((ms_series.Length - ms_series_Position) < max_size_for_tcpsend)
         {
            packet = new byte[packet_header_length + (ms_series.Length - ms_series_Position)];
            PrepareConstHeaderPacket(packet);

            Buffer.BlockCopy(BitConverter.GetBytes(series_fragment_number), 0, packet, 8, 2);
            series_fragment_length = Convert.ToUInt16((ms_series.Length - ms_series_Position));
            Buffer.BlockCopy(BitConverter.GetBytes(series_fragment_length), 0, packet, 10, 2);

            Buffer.BlockCopy(ms_series.ToArray(), (int)ms_series_Position, packet, packet_header_length, (int)(ms_series.Length - ms_series_Position));

            ms_series_Position += series_fragment_length;

            SendPacket(packet);
         }
      }

      /// <summary>
      /// Отправка пакета в сеть
      /// </summary>
      /// <returns></returns>
      private void SendPacket(byte[] packet)
      {
         try
         {
            if (writerStream.CanTimeout)
               writerStream.ReadTimeout = writerStream.WriteTimeout = tmr_receipt_interval;

            this.writerStream.Write(packet, 0, packet.Length);

			TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 113, DateTime.Now.ToString() + " : (225) : ClientServerDataExchange.cs : послано байт = " + packet.Length.ToString());
            //Console.WriteLine(DateTime.Now.ToString() + " : (225) : ClientServerDataExchange.cs : послано байт = " + packet.Length.ToString());

            // ждем квиток - номер серии, номер пакета
            receipt = new byte[4];

            if (writerStream.CanTimeout)
               writerStream.ReadTimeout = writerStream.WriteTimeout = tmr_receipt_interval;

            this.writerStream.Read(receipt, 0, receipt.Length);

            if (OnPacketSended != null)
               OnPacketSended();

            ushort receipt_series_number = BitConverter.ToUInt16(receipt,0);
            ushort receipt_series_fragment_number = BitConverter.ToUInt16(receipt, 2);

            if (!((series_number == receipt_series_number) && (series_fragment_number == receipt_series_fragment_number)))
               throw new Exception("(237) : ClientServerDataExchange.cs : Нарушен порядок передачи пакетов.");
         }
         catch (Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 113, DateTime.Now.ToString() + " (113) : ClientServerDataExchange.cs : SendPacket() : ОШИБКА " + ex.Message);

            // вызов события о неудачной передаче
            if (OnErrorSended != null)
               OnErrorSended("writerStream");
         }
      }

      /// <summary>
      /// закрытие и унтичтожение объекта
      /// </summary>
      public void Close()
      {
         this.Dispose();
      }

         #region уничтожение объекта
         /// <summary>
         /// флаг для определения того, 
         /// вызывался ли метод Dispose()
         /// </summary>
         private bool isdisposed = false;

         public void Dispose()
         {
            // вызываем метод с кодом очистки
            DeleteThis(true);

            // подавляем финализацию
            GC.SuppressFinalize(this);
         }

         /// <summary>
         /// Код очистки
         /// </summary>
         /// <param name="whois">true - означает, что очистку инициировало приложение, а не сборщик мусора</param>
         private void DeleteThis(bool whois)
         {
            // проверка на то, выполнялась ли очистка
            if (!isdisposed)
            {
               // если whois == true, освобождаем все управляемые ресурсы
               if (whois)
               {
                  // здесь осущ очистку всех управляемых ресурсов
                  // ...
                  if (writerStream != null)
                  {
                     writerStream.Close();
                  }
               }
               // здесь осущ очистку всех НЕуправляемых ресурсов
               // ...
            }
            isdisposed = true;
         }
         ~SendBlockData()
         {
            // вызываем метод очистки, 
            // false - означает, что очистка была инициирована сборщиком мусора
            DeleteThis(false);
         }
         #endregion
   }

   /// <summary>
   /// класс организующий получение данных по TCP-соединению
   /// </summary>
   /// 
   public class ReceiveBlockData
   {
      public event ErrorSended OnErrorSended;

      TcpClient clientSocket;
      BinaryReader readerStream = null;

      /// <summary>
      /// интервал таймера ожидания операций обмена
      /// </summary>
      public string TCPRWTimeout
      {
         set
         {
            if (!int.TryParse(value, out tmr_receipt_interval))
            {
               TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 87, DateTime.Now.ToString() + " (87) : ClientServerDataExchange.cs : TCPRWTimeout : ОШИБКА - попытка установить некорректное значение таймаута на оперции чтения записи по NCH-соединению - " + value);
               tmr_receipt_interval = 10000;
            }
         }
      }
      int tmr_receipt_interval;

      public ReceiveBlockData(TcpClient client)
      {
         clientSocket = client;         
      }

      /// <summary>
      /// подготовить среду для обмена
      /// </summary>
       public void PrepareExch()
      {
         readerStream = new BinaryReader(clientSocket.GetStream());
      }
      /// <summary>
       /// чтение данных в MemoryStream
      /// </summary>
      /// <returns></returns>
       public MemoryStream Read()
       {
          MemoryStream ms = new MemoryStream();
		  byte[] buffer_for_header;
		  byte[] buffer_for_packet_data;

          try
          {
                // заводим таймер на ошибку чтения
                if (readerStream.BaseStream.CanTimeout)
                   readerStream.BaseStream.ReadTimeout = readerStream.BaseStream.WriteTimeout = tmr_receipt_interval;

				buffer_for_header = new byte[4];

				// читаем длину пакета
				readerStream.Read(buffer_for_header, 0, 4);
				ushort fragment_length = BitConverter.ToUInt16(buffer_for_header, 0);

                // запись содержимого пакета в поток
                buffer_for_packet_data = new byte[fragment_length];

                // заводим таймер на ошибку чтения
                if (readerStream.BaseStream.CanTimeout)
                   readerStream.BaseStream.ReadTimeout = readerStream.BaseStream.WriteTimeout = tmr_receipt_interval;

                readerStream.Read(buffer_for_packet_data, 0, fragment_length);

                ms.Write(buffer_for_packet_data, 0, fragment_length);
          }
          catch (Exception ex)
          {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
             if (OnErrorSended != null)
                OnErrorSended("readerStream");
          }
          ms.Position = 0;

		  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 113, DateTime.Now.ToString() + " : Program.cs : ClientServerDataExchange : принято байт ИТОГО = " + ms.Length.ToString());         

          return ms;
       }
      
      /// <summary>
      /// закрытие и унтичтожение объекта
      /// </summary>
      public void Close()
      {
         this.Dispose();
      }

      #region уничтожение объекта
         /// <summary>
         /// флаг для определения того, 
         /// вызывался ли метод Dispose()
         /// </summary>
         private bool isdisposed = false;

         public void Dispose()
         {
            // вызываем метод с кодом очистки
            DeleteThis(true);

            // подавляем финализацию
            GC.SuppressFinalize(this);
         }

         /// <summary>
         /// Код очистки
         /// </summary>
         /// <param name="whois">true - означает, что очистку инициировало приложение, а не сборщик мусора</param>
         private void DeleteThis(bool whois)
         {
            // проверка на то, выполнялась ли очистка
            if (!isdisposed)
            {
               // если whois == true, освобождаем все управляемые ресурсы
               if (whois)
               {
                  // здесь осущ очистку всех управляемых ресурсов
                  // ...
                  if (readerStream != null)
                  {
                     readerStream.Close();
                  }
               }
               // здесь осущ очистку всех НЕуправляемых ресурсов
               // ...
            }
            isdisposed = true;
         }
         ~ReceiveBlockData()
         {
            // вызываем метод очистки, 
            // false - означает, что очистка была инициирована сборщиком мусора
            DeleteThis(false);
         }
         #endregion
   }
}
