/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс для работы с отдельныым тегом устройства
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\ITag.cs
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
using System.Windows.Forms;

namespace InterfaceLibrary
{
    public interface ITag
    {
        #region Events

        // объявляем событие
        event ChVarNewDs OnChangeVar;
        event ChVarNewDsNew OnChangeValue;

        #endregion

        #region Properties

        /// <summary>
        /// привязка для обновления
        /// </summary>
        Binding BindindTag { get; set; }

        /// <summary>
        ///  доступность тега - 
        ///  готовность к обработке
        /// </summary>
        string TagEnable { get; }

        /// <summary>
        ///  доступность тега - 
        ///  готовность к обработке
        /// </summary>
        bool IsEnable { get; }

        /// <summary>
        ///  название тега		
        /// </summary>
        string TagName { get; }

        /// <summary>
        /// уник номер тега
        /// </summary>
        uint TagGUID { get; }

        /// <summary>
        ///  тип тега - 
        /// </summary>
        string Type { get; }

        /// <summary>
        ///  тип-вид тега:
        ///  Analog, Discret, Combo, DateTime
        /// </summary>
        TypeOfTag TypeOfTagHMI { get; set; }

        /// <summary>
        /// ссылка на устройство
        /// </summary>
        IDevice Device { get; }

        /// <summary>
        ///  единица измерения
        /// </summary>
        string Unit { get; }

        /// <summary>
        ///  нижняя граница значения
        /// </summary>
        string MinValue { get; }

        /// <summary>
        ///  верхняя граница значения
        /// </summary>
        string MaxValue { get; }

        /// <summary>
        ///  значение по умолчанию
        /// </summary>
        string DefValue { get; }

        /// <summary>
        ///  доступ по чтению записи
        /// </summary>
        string AccessToValue { get; }

        /// <summary>
        ///  значение как строка
        /// </summary>
        string ValueAsString { get; }

        /// <summary>
        ///  значение как массив байт
        /// </summary>
        byte[] ValueAsMemX { get; }

        /// <summary>
        ///  метка времени
        /// </summary>
        DateTime TimeStamp { get; set; }

        /// <summary>
        ///  качество тега
        /// </summary>
        VarQualityNewDs DataQuality { get; set; }

        /// <summary>
        /// список членов перечисления для 
        /// типов enum
        /// </summary>
        SortedList<int, string> SlEnumsParty { get; }

        /// <summary>
        /// признак инверсии для логич тегов
        /// </summary>
        bool IsInverse { get; set; }

        /// <summary>
        /// признак изменения тега со стороны HMI (уставки)
        /// </summary>
        bool IsHMIChange { get; set; }

        #endregion

        #region Metods

        /// <summary>
        /// установить значение тега
        /// </summary>
        void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq);

        /// <summary>
        /// установить значение тега
        /// </summary>
        void SetValueAsObject(object tagValueAsObject, DateTime dt, VarQualityNewDs vq);

        /// <summary>
        /// установить значение тега
        /// по умолчанию (для его сброса)
        /// </summary>
        void SetDefaultValue();


        #endregion
    }

    public interface ITagDim
    {
        /// <summary>
        /// Кол-во знаков после запятой
        /// </summary>
        ushort ValueDim { get; set; }
    }
}
