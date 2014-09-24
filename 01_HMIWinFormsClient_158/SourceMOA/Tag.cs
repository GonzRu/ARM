/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Базовый класс для определения тега устройства
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\Tag.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using InterfaceLibrary;
using System.Windows.Forms;

namespace SourceMOA
{
    public abstract class Tag : ITag
    {
        #region События

        // объявляем событие
        public event ChVarNewDs OnChangeVar;
        public event ChVarNewDsNew OnChangeValue;

        #endregion

        #region Constructor

        protected Tag()
        {
            SetDefaultValue();
        }

        #endregion

        #region Свойства

        /// <summary>
        /// признак инверсии для логич тегов
        /// </summary>
        public bool IsInverse
        {
            get { return isInverse; }
            set { isInverse = value; }
        }
        bool isInverse = false;

        /// <summary>
        /// привязка для обновления
        /// </summary>
        public Binding BindindTag
        {
            get { return bindindTag; }
            set { bindindTag = value; }
        }
        Binding bindindTag;

        /// <summary>
        /// ссылка на устройство, содержащее тег
        /// </summary>
        public IDevice Device { get { return device; } }
        IDevice device;

        /// <summary>
        /// уник номер тега
        /// </summary>
        public uint TagGUID
        {
            get { return tagGUID; }
        }
        uint tagGUID = 0;

        /// <summary>
        ///  доступность тега - 
        ///  готовность к обработке
        /// </summary>
        public string TagEnable
        {
            get { return tagEnable; }
        }
        string tagEnable = string.Empty;
        public bool IsEnable { get { return Convert.ToBoolean(tagEnable); } }

        /// <summary>
        ///  название тега		
        /// </summary>
        public string TagName
        {
            get { return tagName; }
        }
        string tagName = string.Empty;

        /// <summary>
        ///  тип тега - 
        /// </summary>
        public string Type
        {
            get { return type; }
        }
        string type = string.Empty;

        /// <summary>
        ///  тип-вид тега:
        ///  Analog, Discret, Combo
        /// </summary>
        public TypeOfTag TypeOfTagHMI { get; set; }

        /// <summary>
        ///  единица измерения
        /// </summary>
        public string Unit
        {
            get { return unit; }
        }
        string unit = string.Empty;

        /// <summary>
        ///  нижняя граница значения
        /// </summary>
        public string MinValue
        {
            get { return minValue; }
        }
        string minValue = string.Empty;

        /// <summary>
        ///  верхняя граница значения
        /// </summary>
        public string MaxValue
        {
            get { return maxValue; }
        }
        string maxValue = string.Empty;

        /// <summary>
        ///  значение по умолчанию
        /// </summary>
        public string DefValue
        {
            get { return defValue; }
            set { defValue = value; }
        }
        string defValue = string.Empty;

        /// <summary>
        ///  доступ по чтению записи
        /// </summary>
        public string AccessToValue
        {
            get { return accessToValue; }
        }
        string accessToValue = string.Empty;

        /// <summary>
        ///  значение как строка
        /// </summary>
        public string ValueAsString
        {
            get
            {
                return valueAsString;
            }
            set
            {
                valueAsString = value;
            }
        }
        string valueAsString = String.Empty;

        /// <summary>
        ///  значение как массив байт
        /// </summary>
        public byte[] ValueAsMemX
        {
            get
            {
                return valueAsMemX;
            }
            set
            {
                valueAsMemX = value;
            }
        }
        byte[] valueAsMemX;

        /// <summary>
        ///  метка времени
        /// </summary>
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }
        DateTime timeStamp = DateTime.MinValue;

        /// <summary>
        ///  качество тега
        /// </summary>
        public VarQualityNewDs DataQuality
        {
            get { return dataQuality; }
            set { dataQuality = value; }
        }
        VarQualityNewDs dataQuality = VarQualityNewDs.vqUndefined;

        /// <summary>
        /// список членов перечисления для 
        /// типов enum
        /// </summary>
        public SortedList<int, string> SlEnumsParty { get { return slEnumsParty; } }
        protected SortedList<int, string> slEnumsParty = new SortedList<int, string>();

        /// <summary>
        /// признак изменения тега
        /// со стороны HMI
        /// (уставки)
        /// </summary>
        public bool IsHMIChange
        {
            get { return isHMIChange; }
            set { isHMIChange = value; }
        }
        bool isHMIChange = false;

        #endregion

        #region public

        public Binding bnd;

        #endregion

        /// <summary>
        /// код кодировки символов для строк
        /// </summary>
        static public ushort StringValueEncoding = 866;   // по умолчанию

        #region public-методы реализации интерфейса ...

        /// <summary>
        /// установить значение тега
        /// </summary>
        public abstract void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq);

        /// <summary>
        /// установить значение тега
        /// </summary>
        protected virtual void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag)
        {
            timeStamp = dt;
            dataQuality = vq;

            valueAsMemX = new byte[memX.Length];

            Buffer.BlockCopy(memX, 0, valueAsMemX, 0, memX.Length);

            if (ValueAsString == string.Empty)
                ValueAsString = string.Empty;

            if (OnChangeVar != null)
                OnChangeVar(new Tuple<string, byte[], DateTime, VarQualityNewDs>(ValueAsString, ValueAsMemX, timeStamp, DataQuality));

            if (OnChangeValue != null)
                OnChangeValue(new Tuple<string, byte[], DateTime, VarQualityNewDs>(ValueAsString, ValueAsMemX, timeStamp, DataQuality), typeOfTag);
        }

        /// <summary>
        /// установить значение тега
        /// </summary>
        public abstract void SetValueAsObject(object tagValueAsObject, DateTime dt, VarQualityNewDs vq);

        /// <summary>
        /// установить значение тега
        /// </summary>
        protected virtual void SetValueAsObject(object tagValueAsObject, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag)
        {
            timeStamp = dt;
            dataQuality = vq;

            valueAsMemX = ConverObjectToByteArray(tagValueAsObject);

            if (OnChangeVar != null)
                OnChangeVar(new Tuple<string, byte[], DateTime, VarQualityNewDs>(ValueAsString, ValueAsMemX, timeStamp, DataQuality));

            if (OnChangeValue != null)
                OnChangeValue(new Tuple<string, byte[], DateTime, VarQualityNewDs>(ValueAsString, ValueAsMemX, timeStamp, DataQuality), typeOfTag);
        }

        /// <summary>
        /// установить значение тега
        /// по умолчанию (для его сброса)
        /// </summary>
        public virtual void SetDefaultValue()
        {
        }

        #endregion

        #region public-методы

        /// <summary>
        /// Заполнить общую часть описания тега
        /// (для всех типов тегов)
        /// </summary>
        public void FillTagGenerality(XElement xe_t, IDevice device)
        {
            try
            {
                this.device = device;
                var xe_tag = xe_t.Element("Configurator_level_Describe");
                this.tagGUID = uint.Parse(xe_t.Attribute("TagGUID").Value);
                this.accessToValue = xe_tag.Element("AccessToValue").Value;
                this.defValue = xe_tag.Element("DefValue").Value;
                this.maxValue = xe_tag.Element("MaxValue").Value;
                this.minValue = xe_tag.Element("MinValue").Value;
                this.tagName = xe_tag.Element("TagName").Value;
                this.type = xe_tag.Element("Type").Value;
                this.unit = xe_tag.Elements("Unit").Count() == 0 ? string.Empty : xe_tag.Element("Unit").Value;

                var tagAttr = xe_t.Attribute("TagEnable");
                if (tagAttr != null)
                    this.tagEnable = tagAttr.Value;
                else
                {
                    var tagNode = xe_tag.Element("TagEnable");
                    if (tagNode != null)
                        this.tagEnable = tagNode.Value;
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        #endregion

        #region Private-методы

        /// <summary>
        /// Преобразует объект в массив байтов
        /// </summary>
        private byte[] ConverObjectToByteArray(object tagValueAsObject)
        {
            // Перевод значения тега в byte []
            byte[] byteArrTagValue = null;

            if (tagValueAsObject is Boolean)
                byteArrTagValue = BitConverter.GetBytes((Boolean)tagValueAsObject);
            else if (tagValueAsObject is DateTime)
                byteArrTagValue = BitConverter.GetBytes(((DateTime)tagValueAsObject).Ticks);
            else if (tagValueAsObject is Single)
                byteArrTagValue = BitConverter.GetBytes((Single)tagValueAsObject);
            else if (tagValueAsObject is Int32)
            {
                Int32 v = (Int32)tagValueAsObject;
                Single s = (Single)v;
                byteArrTagValue = BitConverter.GetBytes(s);
            }
            else if (tagValueAsObject is String)
            {
                var encoder = System.Text.Encoding.GetEncoding(SourceMOA.Tag.StringValueEncoding);

                byteArrTagValue = encoder.GetBytes(tagValueAsObject.ToString());
            }

            return byteArrTagValue;
        }

        #endregion
    }
}
