using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace ProviderCustomerExchangeLib
{
   //public delegate void ByteArrayPacketAppearance(byte[] pq);
   public delegate void ByteTypeArrayPacketAppearance(byte[] pq, ushort typepacket, int len);

   public class ArrayForExchange
   {
      public event ByteArrayPacketAppearance packetAppearance;

      public event ByteTypeArrayPacketAppearance packetTypeAppearance;

      /// <summary>
      /// тип данных текущего пакета pq
      /// </summary>
      public ushort TypeData 
      {
         get { return typeData; }
         set { typeData = value; }
      }
      ushort typeData = 0;
      
      /// <summary>
      /// длина данных извлекаемая из текущего пакета pq
      /// </summary>
      public int LengthData
      {
         get { return lengthData; }
         set { lengthData = value; }
      }
      int lengthData = 0;

      byte[] arrbytePacket;

      /// <summary>
      /// Конструктор
      /// </summary>
      public ArrayForExchange()
      {
      }

      /// <summary>
      /// добавить пакет для передачи
      /// </summary>
      /// <param Name="bytearr"></param>
      public void Add(byte [] bytearr) 
      {
            try
            {
                 arrbytePacket = new byte[bytearr.Length];
                 Buffer.BlockCopy(bytearr,0,arrbytePacket,0,bytearr.Length);
                 ParsePackInQueque();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw new Exception(ex.Message,ex);
            }

      }

      /// <summary>
      /// добавить пакет для передачи с учетом его типа 
      /// и длины извлекаемой из пакета
      /// </summary>
      /// <param Name="bytearr"></param>
      public void Add(byte[] bytearr, ushort typearr, int len)
      {
            try
            {
                 arrbytePacket = new byte[bytearr.Length];
                 typeData = typearr;
                 lengthData = len;

                 Buffer.BlockCopy(bytearr, 0, arrbytePacket, 0, bytearr.Length);
                 ParsePackTypeInQueque();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw new Exception(ex.Message,ex);
            }
     }

      /// <summary>
      /// извлечь элемент из очереди
      /// </summary>
      /// <returns></returns>
      public byte[] Get( )
      {
         return arrbytePacket;
      }

      /// <summary>
      /// запустить процесс обработки пакетов
      /// </summary>
      public void ParsePackInQueque()
      {
            try
            {
                OnPacketAppearance();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw new Exception(ex.Message,ex);
            }
      }

      /// <summary>
      /// событие - очередь не пуста
      /// </summary>
      private void OnPacketAppearance() 
      {
            try
            {
                 if (packetAppearance != null)
                    packetAppearance(arrbytePacket);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw new Exception(ex.Message,ex);
            }
      }

      /// <summary>
      /// запустить процесс обработки пакетов
      /// </summary>
      public void ParsePackTypeInQueque()
      {
         OnPacketTypeAppearance();
      }

      /// <summary>
      /// событие - очередь не пуста
      /// </summary>
      private void OnPacketTypeAppearance()
      {
            try
            {
                if (packetTypeAppearance != null)
                    packetTypeAppearance(arrbytePacket, TypeData, LengthData);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
      }
   }
}
