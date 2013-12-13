/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Интерфейс для унификации обмена с поставщиком данных (по сети, каналам Pipe)
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\InterfaceLibraryCFG\IProviderCustomer.cs
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
using InterfaceLibrary;

namespace ProviderCustomerExchangeLib
{
	public delegate void ByteArrayPacketAppearance(byte[] pq);
    /// <summary>
    /// потеря связи с DS
    /// </summary>
    /// <param name="state"></param>
    public delegate void DSCommunicationLoss(bool state);

	public interface IProviderCustomer
	{
		/// <summary>
		/// очередь пакетов от источника данных 
		/// на обработку DataServer
		/// </summary>
		PacketQueque NetPackQ
		{
			set;
			get;
		}

		/// <summary>
		/// событие появления данных на входе клиента(потребителя)
		/// </summary>
		event ByteArrayPacketAppearance OnByteArrayPacketAppearance;

        /// <summary>
        /// событие потери связи с DataServer
        /// </summary>
        event DSCommunicationLoss OnDSCommunicationLoss;

		/// <summary>
		/// посылка данных поставщику
		/// </summary>
		/// <param name="pq"></param>
		void SendData(byte[] pq);
	    /// <summary>
        /// посылка запроса на
        /// осциллограмму поставщику данных
        /// </summary>
        /// <param name="pq"></param>
        void SendOcsReq(byte[] pq);
        /// <summary>
        /// посылка запроса на
        /// архивные данные
        /// </summary>
        /// <param name="pq"></param>
        void SendArhivReq(byte[] pq);
        /// <summary>
        /// послать запрос на 
        /// выполнение команды
        /// </summary>
        /// <param name="pq"></param>
        void SendRunCMD(byte[] pq);
    }
}
