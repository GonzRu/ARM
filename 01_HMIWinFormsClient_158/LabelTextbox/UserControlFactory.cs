/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика для создания элементов интерфейса, связанных с данными
 *                                                                             
 *	Файл                     : X:\Projects\99_MTRAhmi\Client\LabelTextbox\UserControlFactory.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AdapterLib;

namespace LabelTextbox
{
	public interface MTRAUserControlFactory
	{
		MTRAUserControl Make(AdapterBase aBase);
	}

	public interface MTRAUserControl
	{
		/// <summary>
		/// Инициализация пользовательского элемента
		/// </summary>
		/// <param name="parentControl">родительский контрол</param>
		void Init(Control parentControl);
	}

	public class MTRAUserControlFactoryImplementation : MTRAUserControlFactory
	{
		public MTRAUserControl Make(AdapterBase aBase)
		{
			try
			{
				switch (aBase.Typeadapter)
				{
					//case "BitField2IntAdapter":
					//    return new CheckBoxVar();
					case "BitFieldAdapter":
						CheckBoxVar chb = new LabelTextbox.CheckBoxVar();
						aBase.AdapterValueChange += chb.AdapterValueChange;
						//chb.Parent = fspanel.flp;
						chb.CheckBox_Text = aBase.Caption;
						return chb;
					case "FloatFieldAdapter":
						ctlLabelTextbox ctlTB = new LabelTextbox.ctlLabelTextbox();
						aBase.AdapterValueChange += ctlTB.AdapterValueChange;
						ctlTB.Caption_Text = aBase.Caption;
						ctlTB.Dim_Text = "(" + aBase.Dim + ")";
						return ctlTB;
					default:
						throw new Exception("(40) UserControlFactory.cs : Make() === Тип Адаптера " + aBase.Typeadapter + " не поддерживается ===");
				}
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
				throw new Exception(ex.Message);
			}
		}
	}
}
