/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Интерфейс унифицирующий доступ к конфигурации конкретного источника
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\ISourceConfiguration.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 11.11.2011 
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

namespace InterfaceLibrary
{
    public interface ISourceConfiguration
    {
        /// <summary>
        /// имя источника
        /// (атрибут nameSourceDriver = ...)
        /// </summary>
        string NameSource { get; }
        /// <summary>
        /// путь к файлу описания конфигурации источника
        /// </summary>
        string Path2src_prgdevcfg { get; }
        /// <summary>
        /// Создать и инициализировать конфигурацию источника
        /// </summary>   
        void InitSrcConfiguration(string path2SrcPrgDevSfg, XElement xe_Src, string UniDS_GUID);
        /// <summary>
        /// получить ссылку на устройство
        /// по строке идентификации, специфичной для устройства
        /// например fc.dev
        /// </summary>
        /// <param name="uniDevGuid"></param>
        /// <returns></returns>
        IDevice GetDeviceLinkBySrcSpecificStrDescribe(int uniDevGuid);
        /// <summary>
        /// Получить список устройств для этого источника
        /// </summary>
        /// <returns></returns>
        List<IDevice> GetDeviceList4TheSource();
        /// <summary>
        /// функция получения правильного TagGUID:
        ///		TagGUID = адр. рег сдвинуть влево на 8 бит 
        ///		в младшие 4 бита поместить маску 
        ///		битового поля (если она есть)
        /// </summary>
        /// <param name="strtemp"></param>
        /// <returns></returns>
        uint GetTagGUID(string srcs_pecific_describe);
    }
}
