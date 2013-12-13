/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс, реализующий интерфейс IDSDecoy для работы с конфигурацией DataServer
 *		по протоколу TCP
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\DSDecoy\DSDecoy_TCP.cs
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
	public class DSDecoy_TCP : IDSDecoy
	{
		#region События
		#endregion

		#region Свойства
		#endregion

		#region public
		#endregion

		#region private
		#endregion

		#region конструктор(ы)
		public DSDecoy_TCP()
		{
			throw new Exception("Не реализовано");
		}
		#endregion


		#region public-методы
		/// <summary>
		/// предоставление клиенту значений тегов
		/// по запросу формата:
		/// devguid.gr.reg.bitmask#devguid.gr.reg.bitmask# ...
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <summary>
		/// получить данные с DataServer в виде строки
		/// </summary>
		/// <param name="str">строка запроса формата: devguid.gr.reg.bitmask#devguid.gr.reg.bitmask# ...</param>
		/// <returns></returns>
		public string GetDataServerDataAsString(string str)
		{
			//byte[] buffer = WCFproxy.GetDataServerData(str);//"4.3.25.0100#3.3.25.0100#5.3.25.0100"
			//string rez = Encoding.ASCII.GetString(buffer, 0, buffer.Length);

			//return rez;

			return null;
		}
		public MemoryStream GetDSValueAsByteBuffer(byte[] arr)
		{
			return null;	//cfg.GetArrayIDValueTags(Encoding.ASCII.GetString(arr, 0, (int)arr.Length));
		}

		public void RunCMD(byte numksdu, ushort numvtu, int tagguid, byte[] arr)
		{
			//RunCMD(numksdu, numvtu, tagguid, arr);
		}

		/// <summary>
		/// закрытие соединения
		/// </summary>
		public void CloseExchChannel()
		{
			throw new Exception("нет реализации метода");
		}
		#endregion

		#region private-методы
		#endregion

	}
}
