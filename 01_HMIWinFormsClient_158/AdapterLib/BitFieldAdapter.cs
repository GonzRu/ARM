/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Адаптер для вычисления битового значения по формуле
 *                                                                             
 *	Файл                     : X:\Projects\99_MTRAhmi\Client\crza\CRZADevices\CRZADevices\BitFieldAdapter.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 24.03.2011 
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
	public class BitFieldAdapter : AdapterBase, Adapter
	{
		#region Свойства
		#endregion

		#region public
		public override event ValueChange AdapterValueChange;
		private	bool Value = false;
		#endregion

		#region private
		#endregion

		#region конструкторы
		public BitFieldAdapter() 
		{
			/*  
				<formula typeadapter = "BitFieldAdapter" name = "GPSb08" Caption ="GPSb08" Dim = "" commentary = "рег бит флагов ФК - состояние GPS - бит 8" express = "0">
					<value id = "0" tag = "0.34.3.24.0100"/>		
				</formula>
			 */
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
                bool tmp = false, res = bool.TryParse( valForm.ToString(), out tmp );
                if ( valForm.ToString().Contains( "Есть" ) )
                    tmp = true;

                //обновление при разности результата
                //if ( res && Value != tmp )
                //{
                //    Value = tmp;
                //    ValueAsString = Convert.ToString( Value );
                    
                //    if (AdapterValueChange != null)
                //        AdapterValueChange( this, Convert.ToString( Value ) );
                //}

                //постоянное обновление
                Value = tmp;
                ValueAsString = Value.ToString();
                if (AdapterValueChange != null)
                    AdapterValueChange(this, Convert.ToString(Value));
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                Value = false;
                ValueAsString = Boolean.FalseString;
			}
		}
	}
}
