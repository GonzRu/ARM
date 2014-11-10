/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс работы с компонентом запросов уровня отдельного DataServer
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\IRequestEntry.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 14.11.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using InterfaceLibrary;

namespace InterfaceLibrary
{
    /// <summary>
    /// делегат для события 
    /// изменения списка тегов
    ///  для подписки
    /// </summary>
    /// <param name="countChange">величина изменения (со знаком)</param>
    /// <param name="htReqList">список тегов и счетчик запросов к каждому из них</param>
    public delegate void ChangeRequestTags(int countChange, Hashtable htReqList);
	public interface IRequestEntry
	{
        /// <summary>
        /// событие изменения 
        /// контекста тегов для подписки
        /// </summary>
        event ChangeRequestTags OnChangeRequestTags;
        /// <summary>
        /// получить текущий список
        /// тегов запрашиваемых с сервера
        /// </summary>
        /// <returns></returns>
        Hashtable GetTagsReqList();
		/// <summary>
		/// подписаться на обновление тегов
		/// </summary>
		/// <param name="?"></param>
		void SubscribeTags(List<ITag> lstTags);
		/// <summary>
		/// отписаться от обновления тегов
		/// </summary>
		/// <param name="?"></param>
        void UnSubscribeTags(List<ITag> lstTags);
        /// <summary>
        /// подписаться на обновление тега
        /// </summary>
        /// <param name="?"></param>
        void SubscribeTag(ITag Tag);
        /// <summary>
        /// отписаться от обновления тега
        /// </summary>
        /// <param name="?"></param>
        void UnSubscribeTag(ITag Tag);
        /// <summary>
		/// обновить инфо запросив данные по активным тегам
		/// </summary>
		void UpdateHMIInfo();

        /// <summary>
        /// Получить ссылку на осциллограмму
        /// </summary>
        string GetOscillogramAsUrlById(UInt16 dsGuid, Int32 oscGuid);
        /// <summary>
        /// Получить содержимое архива с осциллограммами и его имя
        /// </summary>
        Tuple<byte[], string> GetOscillogramAsByteArray(UInt16 dsGuid, Int32 oscGuid);
	}
}
