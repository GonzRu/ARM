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
        public bool IsInverse { get; set; }

        /// <summary>
        /// привязка для обновления
        /// </summary>
        public Binding BindindTag { get; set; }

        /// <summary>
        /// ссылка на устройство, содержащее тег
        /// </summary>
        public IDevice Device { get { return device; } }
        IDevice device;

        /// <summary>
        /// уник номер тега
        /// </summary>
        public uint TagGUID { get; protected set; }

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
        public string TagName { get; protected set; }

        /// <summary>
        ///  тип тега - 
        /// </summary>
        public string Type { get; protected set; }

        /// <summary>
        ///  тип-вид тега:
        ///  Analog, Discret, Combo
        /// </summary>
        public TypeOfTag TypeOfTagHMI { get; set; }

        /// <summary>
        ///  единица измерения
        /// </summary>
        public string Unit { get; protected set; }

        /// <summary>
        ///  нижняя граница значения
        /// </summary>
        public string MinValue { get; protected set; }

        /// <summary>
        ///  верхняя граница значения
        /// </summary>
        public string MaxValue { get; protected set; }

        /// <summary>
        ///  значение по умолчанию
        /// </summary>
        public string DefValue { get; set; }

        /// <summary>
        ///  доступ по чтению записи
        /// </summary>
        public string AccessToValue { get; protected set; }

        /// <summary>
        ///  значение как строка
        /// </summary>
        public abstract string ValueAsString { get; }

        /// <summary>
        ///  значение как массив байт
        /// </summary>
        public byte[] ValueAsMemX { get; set; }

        /// <summary>
        ///  метка времени
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        ///  качество тега
        /// </summary>
        public VarQualityNewDs DataQuality { get; set; }

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
        public bool IsHMIChange { get; set; }

        public Binding bnd;

        #endregion

        /// <summary>
        /// код кодировки символов для строк
        /// </summary>
        static public ushort StringValueEncoding = 866;   // по умолчанию

        #region Implementation ITag

        /// <summary>
        /// установить значение тега
        /// </summary>
        public abstract void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq);

        /// <summary>
        /// установить значение тега
        /// </summary>
        protected virtual void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag)
        {
            TimeStamp = dt;
            DataQuality = vq;

            ValueAsMemX = new byte[memX.Length];

            Buffer.BlockCopy(memX, 0, ValueAsMemX, 0, memX.Length);

            if (OnChangeVar != null)
                OnChangeVar(new Tuple<string, byte[], DateTime, VarQualityNewDs>(ValueAsString, ValueAsMemX, TimeStamp, DataQuality));

            if (OnChangeValue != null)
                OnChangeValue(new Tuple<string, byte[], DateTime, VarQualityNewDs>(ValueAsString, ValueAsMemX, TimeStamp, DataQuality), typeOfTag);
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
            TimeStamp = dt;
            DataQuality = vq;

            ValueAsMemX = ConverObjectToByteArray(tagValueAsObject);

            if (OnChangeVar != null)
                OnChangeVar(new Tuple<string, byte[], DateTime, VarQualityNewDs>(ValueAsString, ValueAsMemX, TimeStamp, DataQuality));

            if (OnChangeValue != null)
                OnChangeValue(new Tuple<string, byte[], DateTime, VarQualityNewDs>(ValueAsString, ValueAsMemX, TimeStamp, DataQuality), typeOfTag);
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
                this.TagGUID = uint.Parse(xe_t.Attribute("TagGUID").Value);
                this.AccessToValue = xe_tag.Element("AccessToValue").Value;
                this.DefValue = xe_tag.Element("DefValue").Value;
                this.MaxValue = xe_tag.Element("MaxValue").Value;
                this.MinValue = xe_tag.Element("MinValue").Value;
                this.TagName = xe_tag.Element("TagName").Value;
                this.Type = xe_tag.Element("Type").Value;
                this.Unit = xe_tag.Elements("Unit").Count() == 0 ? string.Empty : xe_tag.Element("Unit").Value;

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
