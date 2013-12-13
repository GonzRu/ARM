/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Интерфейс комопнентов формирователя пакетов
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\BlockDataComposer\IBlockDataComposer.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 28.10.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProviderCustomerExchangeLib;
using InterfaceLibrary;

namespace BlockDataComposer
{
	/// <summary>
	/// перечисление определяющее тип пакетов для обмена
	/// </summary>
	public enum TYPEOFPACKET
	{ 
		RequestData = 1,            // запрос текущих данных
		CMD = 2,                    // команда
        RequestArchivalData = 6,   // запрос архивных данных
        RequestOsc = 7,             // запрос на получение осциллограммы
        RequestSpecificBankData = 8,        // запрос текущих данных с указанием номера банка данных (ацп-перв-втор и т.п.)
		testpacket = 255            // тестовый пакет
	}
	/// <summary>
	/// интерфейс компонента формирователя пакетов
	/// </summary>
	public interface IBlockDataComposer
	{
		/// <summary>
		/// сформировать и послать пакет запроса значений тегов
		/// </summary>
		/// <param name="ht"></param>
		void FormAndSaveReqPacket(Hashtable ht);
        /// <summary>
        /// сформировать и послать пакет запроса значений тегов
        /// из конкретного банка значений - ацп-перв-втор
        /// </summary>
        /// <param name="ht"></param>
        void FormAndSaveSpecificBankReqPacket(Hashtable ht);
		/// <summary>
		/// сформировать и послать пакет команды
		/// </summary>
		/// <param name="ht"></param>
		void FormAndSendCMDPacket(ICommand cnd);
        /// <summary>
        /// сформировать и послать запрос на архивные данные
        /// </summary>
        /// <param name="ht"></param>
        void FormAndSendRequestPacket(IRequestData req);
        /// <summary>
        /// сформировать и послать запрос на осциллограмму
        /// </summary>
        /// <param name="ht"></param>
        void FormAndSendOscRequestPacket(IOscillogramma osc);
    }
	/// <summary>
	/// фабрика компонентов формирователей пакетов
	/// </summary>
	public class BlockDataComposerFactory
	{
		public IBlockDataComposer CreateBlockDataComposer(string type_blockdatacomposer, IProviderCustomer provCust)
		{
			IBlockDataComposer blockDataComposer = null;

			try
			{
				switch (type_blockdatacomposer)
				{
					case "ordinal":
						blockDataComposer = new OrdinalBlokDataComposer(provCust);
						break;
					default:
						throw new Exception(string.Format("Тип формирователя пакетов {0} не поддерживается", type_blockdatacomposer));
				}
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
			return blockDataComposer;
		}
	}
}
