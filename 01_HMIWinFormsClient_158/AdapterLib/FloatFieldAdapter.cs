/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Адаптер для вычисления значения с плавающей точкой по формуле
 *                                                                             
 *	Файл                     : X:\Projects\99_MTRAhmi\Client\crza\CRZADevices\CRZADevices\FloatFieldAdapter.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 13.04.2011 
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
using System.Xml.Linq;

namespace AdapterLib
{
	public class FloatFieldAdapter : AdapterBase, Adapter
	{
		#region Свойства
		#endregion

		#region public
		public override event ValueChange AdapterValueChange;
		public Single Value = 0;
		#endregion

		#region private
		#endregion
			#region конструкторы
		public FloatFieldAdapter() 
		{
		}
		#endregion	
		/// <summary>
		/// реакция на событие вычисление нового значения 
		/// привязанной к тегу формулы
		/// </summary>
		/// <param Name="valForm"></param>
		public override void FEvalEv_OnChangeValForm(object valForm)
		{
			try
			{
				Value = Convert.ToSingle(valForm);
				ValueAsString = Convert.ToString(Value);

				if (AdapterValueChange != null)
					AdapterValueChange(this, Convert.ToString(Value));
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}
	}
}
