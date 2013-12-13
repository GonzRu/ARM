/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс компонента запросов уровня конфигурации
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\ICfgReqEntry.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 15.11.2011 
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

namespace InterfaceLibrary
{
    public interface ICfgReqEntry
    {
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
    }
}
