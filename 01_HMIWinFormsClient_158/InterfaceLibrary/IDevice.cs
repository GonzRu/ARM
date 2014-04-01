/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Интерфейс взаимодействия с устройством - некоторым объектом, 
 *	            обладающим конфигурацией (набором тегов)
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\IDevice.cs
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
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace InterfaceLibrary
{
    /// <summary>
    /// Тип представления тега - 
    /// в кодах АЦП, первичное, вторичное.
    /// значения перечисления являются степенью 2 
    /// и формируют маску, для того чтобы отображать, например
    /// архивную информацию первичных или вторичных значениях. 
    /// Это на тот случай, когда устройство постоянно передает 
    /// уставки текущие, а нам нужно смотреть архивные 
    /// </summary>
    public enum TypeViewTag
    {
        ADC = 0,        // коды АЦП
        PRIMARY = 1,    // первичные
        SECONDARY = 2   // вторичные
    }
    /// <summary>
    /// интерфейс доступа к устройству
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// строка описания для представления устройства в HMI
        /// с точки зрения физич. расположения
        /// </summary>
        string StrDescriptionAsPhysicalDevice{get;}
        /// <summary>
        /// строка описания для представления устройсва в HMI
        /// с точки зрения логич. расположения
        /// </summary>
        string StrDescriptionAsLogicalDevice { get; }

        String Name { get; }
        String TypeName { get; }
        String Description { get;  }
        String Version { get; }

        /// <summary>
        /// ссылка на класс со специфич параметрами устройства
        /// </summary>
        ISpecificDeviceValue SpecificDeviceValue {get;}
        /// <summary>
        /// уникальный номер DS
        /// кот. принадлежит данное устройство
        /// </summary>
        uint UniDS_GUID { get; }
        /// <summary>
        /// уникальный (в пределах DS)
        /// номер объекта
        /// </summary>
        uint UniObjectGUID { get; set; }
        /// <summary>
        /// секция описания устройсва в
        /// файле PrgDevCFG.cdp
        /// </summary>
        XElement XESsectionDescribe { get; }
        /// <summary>
        /// список групп/подгрупп устройства
        /// со списком тегов
        /// </summary>
        /// <returns></returns>
        List<IGroup> GetGroupHierarchy();
        /// <summary>
        /// список тегов устройства
        /// </summary>
        /// <returns></returns>
        List<ITag> GetRtuTags();
        /// <summary>
        /// получить список команд устройства
        /// </summary>
        /// <returns></returns>
        List<IDeviceCommand> GetListDeviceCommands();
        /// <summary>
        /// извлечь команду по 
        /// краткому имени
        /// </summary>
        /// <param name="cmdname"></param>
        /// <returns></returns>
        IDeviceCommand GetDeviceCommandByShortName(string cmdname);
        /// <summary>
        /// извлечь конкретный тег
        /// </summary>
        /// <param name="tagGuid"></param>
        /// <returns></returns>
        ITag GetTag(UInt32 tagGuid);
        /// <summary>
        /// разобрать пакет с данными 
        /// от устройства с учетом специфики
        /// </summary>
        void ParsePacketRawData(BinaryReader binReader);
        /// <summary>
        /// приоритетный режим отображения тегов -
        /// в первичных значениях, вторичных и т.п.
        /// </summary>
        TypeViewTag TypeTagPriorityView {get;set; }

        #region Представление устройства в пользовательском интерфейсе
        /// <summary>
        /// Показывать ли группы и теги устройства
        /// </summary>
        bool ShowGroupsAndTags { get; }

        /// <summary>
        /// Показывать ли осцилограммы устройства
        /// </summary>
        bool ShowOscilograms { get; }

        /// <summary>
        /// Показывать ли диаграммы устройства
        /// </summary>
        bool ShowDiagrams { get; }

        /// <summary>
        /// Показывать ли события устройства
        /// </summary>
        bool ShowEvents { get; }

        /// <summary>
        /// Показывать ли пользовательские файлы
        /// </summary>
        bool ShowUserFiles { get; }

        /// <summary>
        /// Показывать ли команды устройства
        /// </summary>
        bool ShowCommands { get; }
        #endregion Представление устройства в пользовательском интерфейсе
    }
}
