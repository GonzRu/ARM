/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс для обработки входных пакетов для источника данных МОА
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ProviderCustomerExchangeLib\PacketParser_udp_MOA.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 17.10.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using InterfaceLibrary;

namespace ProviderCustomerExchangeLib
{
   public class PacketParser_udp_MOA : IPacketParser
   {
      /// <summary>
      /// локальная очередь - создается для быстрого копирования
      /// входной очереди byteQueque
      /// </summary>
      Queue<byte[]> netPackQLoc;
      /// <summary>
      /// поток, обрабатывающий входную очередь
      /// </summary>
      BackgroundWorker bcwQ;
	   /// <summary>
	   /// конфигурация текущего DataServer
	   /// </summary>
	  IConfiguration srcCfg;

	  /// <summary>
	  /// инициалзизация класс разбора
	  /// </summary>
	  public void Init(IConfiguration srcCfg)
      {
		  this.srcCfg = srcCfg;

         netPackQLoc = new Queue<byte[]>();

         bcwQ = new BackgroundWorker();
         bcwQ.DoWork += new DoWorkEventHandler(bcwQ_DoWork);
      }

      public void byteQueque_packetAppearance(Queue<byte[]> pq)
      {
         lock (pq)
         {
            // обработка считанных пакетов в отд. потоке
            if (!bcwQ.IsBusy)
            {
               // быстро копируем в новую очередь
               try
               {
                  netPackQLoc = new Queue<byte[]>(pq.ToArray());
               }
               catch(Exception ex)
               {
                  System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " : (126) PacketHandler.cs : byteQueque_packetAppearance  : ОШИБКА: " + ex.Message);
               }

               bcwQ.RunWorkerAsync(netPackQLoc);
               // и чистим входную
               pq.Clear();
            }
            else
               return;
         }
      }

      /// <summary>
      /// извлекает пакеты из очереди netPackQLoc
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void bcwQ_DoWork(object sender, DoWorkEventArgs e)
      {
         short lenpack = 0;
         short numdev = 0;
         int nf = 0;
         int nd = 0;

		Queue<byte[]> NetPackQ = (Queue<byte[]>)e.Argument; 

		try
		{

			if (NetPackQ.Count == 0)
				return;
				
			do
			{
				using (MemoryStream msDev = new MemoryStream((byte[])NetPackQ.Dequeue()))
					using (	BinaryReader binReader = new BinaryReader(msDev))
						{
							lenpack = (short)binReader.ReadInt16();
							// номер устройства в пакет с учетом ФК, поэтому вычленяем его
							numdev = binReader.ReadInt16();
							nf = numdev / 256;
							nd = numdev % 256;

							binReader.BaseStream.Position -= 4;

							/*
							 *  находим устройство и передаем ему пакет на "раздергивание".
							 *  Для старых проектов objectGUID = DevGUID
							 */

							//IDevice dev = srcCfg.SlObjectConfiguration[nf].GetDevice(nd);

							//if (dev == null)
							//    continue;

							//dev.ParsePacketRawData(binReader);

							

							// передаем пакет на разбор устройству

							//foreach (MTDevice mt in MTDev)
							//{
							//   string nameMTBMRz = mt.ToString();
							//   if (nameMTBMRz == "FC_net")
							//      nameMTBMRz = "FC_net";
							//   if (nf == 1 )//&& nd == 0
							//      nf = 1;
								
							//   if (mt.NFC == nf && mt.NDev == nd)
							//   {
							//      mt.parse_memdev(binReader);
							//      break;
							//   }
							//}
						}
			} while (NetPackQ.Count > 0);
		}
		catch(Exception ex)
		{
			TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			NetPackQ.Clear();
		}
      }
   }
}