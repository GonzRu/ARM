/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Фабрика для создания объекта, взаимодействующего с конфигурацией DataServer
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\uvs_SAF\DSDecoyFactory.cs
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

namespace DSDecoy
{
	public class DSDecoyFactory
	{
		public IDSDecoy CreateDSDecoy(string typeDsDecoy, string ip = null)
		{
		    IDSDecoy dsDecoy;

		    switch ( typeDsDecoy )
		    {
		        case "TCP":
		            dsDecoy = new DSDecoy_TCP();
		            break;
		        case "WCF":
		            if ( string.IsNullOrEmpty( ip ) )
		                throw new Exception( "Не указан IP адресс для подключения" );
		            dsDecoy = new DSDecoy_WCF( ip );
		            break;
		        default:
		            throw new Exception( string.Format( "Тип \"{0}\" взаимодействия с DataServer не поддерживается",
		                                                typeDsDecoy ) );
		    }
		    return dsDecoy;
		}
	}
}
