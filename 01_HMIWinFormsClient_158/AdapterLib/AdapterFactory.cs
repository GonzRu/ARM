/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: фабрика порождающая адаптеры
 *                                                                             
 *	Файл                     : X:\Projects\99_MTRAhmi\Client\crza\CRZADevices\CRZADevices\AdapterFactory.cs                               
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 23.03.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 *
 *#############################################################################*/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace AdapterLib
{
	public interface AdapterFactory
	{
		Adapter Make(string name);
	}

	public interface Adapter
	{
		void Init(XElement xeinit);//, ArrayList kb
	}

	public class AdapterFactoryImplementation : AdapterFactory
	{
		public Adapter Make(string name)
		{ 
			switch(name)
			{
				case "BitField2IntAdapter":
					//return new BitField2IntAdapter();
				case "BitFieldAdapter":
					return new BitFieldAdapter();
				case "FloatFieldAdapter":
					//return new FloatFieldAdapter();					
				default:
					throw new Exception("=== Тип Адаптера " + name + " не поддерживается ===");
			}
		}
	}
}
