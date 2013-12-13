/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Интерфейсный класс для индивидуализации процесса разбора пакетов
 *				от различных поставщиков
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ProviderCustomerExchangeLib\IPacketParser.cs
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
	/// <summary>
	/// интерфейс для индивидуализации 
	/// процесса разбора пакетов для 
	/// разных поставщиков
	/// </summary>
	interface IPacketParser
	{
		/// <summary>
		/// инициалзизация класс разбора
		/// </summary>
		void Init(IConfiguration srcCfg);
		void byteQueque_packetAppearance(Queue<byte[]> pq);
	}
}
