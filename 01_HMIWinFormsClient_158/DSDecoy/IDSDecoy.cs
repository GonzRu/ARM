/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Интерфейс, задающий функции для работы с конфигурацией DataServer
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\DSDecoy\IDSDecoy.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 11.07.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;

namespace DSDecoy
{
	public interface IDSDecoy
	{
		/// <summary>
		/// предоставление клиенту значений тегов
		/// по запросу формата:
		/// devguid.gr.reg.bitmask#devguid.gr.reg.bitmask# ...
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		string GetDataServerDataAsString(string str);
		/// <summary>
		/// извлечение значений из DataServer в виде массива байт
		/// подробности в файле Протокол взаимодействия HMI-клиента и DataServer.doc
		/// </summary>
		/// <param name="arr">запрос в виде массива байт</param>
		/// <returns>результат в виде массива байт</returns>
		MemoryStream GetDSValueAsByteBuffer(byte[] arr);
		/// <summary>
		/// Выполнить команду
		/// </summary>
		/// <param name="numksdu">номер ECU</param>
		/// <param name="numvtu">номер RTU</param>
		/// <param name="tagguid">уник номер тега</param>
		/// <param name="arr">массив доп параметров - пока пустой - не испольуется</param>
		void RunCMD(byte numksdu, ushort numvtu, int tagguid, byte[] arr);
		/// <summary>
		/// закрытие канала обмена
		/// </summary>
		void CloseExchChannel();
	}
}
