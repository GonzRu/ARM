using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace HMI_MT
{
   public delegate void ByteArrayPacketAppearance(byte[] pq);
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
         arrbytePacket = new byte[bytearr.Length];
         Buffer.BlockCopy(bytearr,0,arrbytePacket,0,bytearr.Length);
         ParsePackInQueque();
      }

      /// <summary>
      /// добавить пакет для передачи с учетом его типа 
      /// и длины извлекаемой из пакета
      /// </summary>
      /// <param Name="bytearr"></param>
      public void Add(byte[] bytearr, ushort typearr, int len)
      {
         arrbytePacket = new byte[bytearr.Length];
         typeData = typearr;
         lengthData = len;

         Buffer.BlockCopy(bytearr, 0, arrbytePacket, 0, bytearr.Length);
         ParsePackTypeInQueque();
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
         OnPacketAppearance();
      }

      /// <summary>
      /// событие - очередь не пуста
      /// </summary>
      private void OnPacketAppearance() 
      {
         if (packetAppearance != null)
            packetAppearance(arrbytePacket);
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
         if (packetTypeAppearance != null)
            packetTypeAppearance(arrbytePacket, TypeData, LengthData);
      }
   }
}
