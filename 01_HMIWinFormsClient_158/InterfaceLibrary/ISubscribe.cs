/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс для подписки на обновление тегов с DataServer
 *                                                                             
 *	Файл                     : X:\Projects\01_HMIWinFormsClient\InterfaceLibrary\ISubscribe.cs                                      
 *	Тип конечного файла      : 
 *	версия ПО для разработки : С#, Framework 2.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 19.02.2012                                       
 *	Дата (v1.0)              :                                                  
 *******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Text;

namespace InterfaceLibrary
{
    public interface ISubscribe
    {
        /// <summary>
        /// подписка тега на
        /// обновление
        /// </summary>
        /// <param name="taglist"></param>
        void SubscribeTagReNew( );
        /// <summary>
        /// отписка тега от
        /// обновления
        /// </summary>
        /// <param name="taglist"></param>
        void UnSubscribeTagReNew();
        /// <summary>
        /// отписка тега от
        /// обновления c установкой 
        /// значения по умолчанию (обнулением)
        /// </summary>
        /// <param name="taglist"></param>
        void UnSubscribeTagReNewAndClear();
    }
} 
