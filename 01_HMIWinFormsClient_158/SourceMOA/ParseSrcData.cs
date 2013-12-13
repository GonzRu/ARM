/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класса ParseSrcData,
 *	            для разбора данных от DataServer
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\ParseSrcData.cs
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using InterfaceLibrary;

namespace SourceMOA
{
	public class ParseSrcData
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
		#endregion

		#region public-методы
		#endregion

		#region private-методы
		#endregion


		public ParseSrcData()
		{

		}

		/// <summary>
		/// Разобрать пакет с данными устройств
		/// </summary>
		/// <param name="ms">пакет с данными для разбора</param>
		public void ParsePacket(MemoryStream ms)
		{
			BinaryReader br = new BinaryReader(ms);

			try
			{

			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				TraceSourceLib.TraceSourceDiagMes.WriteDump(TraceEventType.Error, 157, string.Format("{0} : {1} : {2} : Некорректный пакет.", DateTime.Now.ToString(), "ParseSrcData.cs", "ParsePacket()"),ms);
			}
		}
	}
}
