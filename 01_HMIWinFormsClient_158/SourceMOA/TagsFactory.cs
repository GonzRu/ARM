/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика для создания тегов в соответсвии с типом
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\TagsFactory.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 26.10.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Xml.Linq;
using InterfaceLibrary;

namespace SourceMOA
{
	public class TagsFactory
	{
		public Tag CreateTag(XElement xeTag, IDevice device)
		{
			Tag tag = null;

			try
			{
			    var xElement1 = xeTag.Element( "Configurator_level_Describe" );
			    if ( xElement1 != null )
			    {
			        var element1 = xElement1.Element( "Type" );
			        if ( element1 != null )
			            switch (element1.Value)
			            {
			                case "Analog":
			                    tag = new TagAnalog();
			                    break;
			                case "Discret":
			                    tag = new TagDiscret();
			                    break;
			                case "Enum":
			                    tag = new TagEnum(xeTag);
			                    break;
			                case "DateTime":
                            case "DataTime":
			                    tag = new TagDateTime();
			                    break;
			                case "String":
			                    tag = new TagString();
			                    break;
			                default:
			                    var xElement = xElement1;
			                    {
			                        var element = xElement.Element( "Type" );
			                        if ( element != null )
			                            throw new Exception(string.Format("Тип тега {0} не поддерживается", element.Value));
			                    }
			                    break;
			            }
			    }

			    if ( tag != null )
			        tag.FillTagGenerality(xeTag, device);
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
			return tag;
		}
	}
}
