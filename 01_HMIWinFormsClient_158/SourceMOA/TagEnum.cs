/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс для определения перечислимого тега устройства
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\TagEnum.cs
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
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using InterfaceLibrary;

namespace SourceMOA
{
	public class TagEnum : Tag
	{
        public TagEnum(XElement xetag)
        {
            TypeOfTagHMI = TypeOfTag.Combo;

            // CBItemsList
            if ((from xe in xetag.Element("Configurator_level_Describe").Elements() where xe.Name == "CBItemsList" select xe).Count() != 0)
            {
                var xe_enums = xetag.Element("Configurator_level_Describe").Element("CBItemsList").Elements("CBItem");
                foreach (XElement xe_enum in xe_enums)
                {
                    slEnumsParty.Add(int.Parse(xe_enum.Attribute("intvalue").Value), xe_enum.Value);
                }
            }    
        }

		/// <summary>
		/// установить значение тега
		/// </summary>
        public override void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq)
        {
            this.SetValue( memX, dt, vq, TypeOfTag.Combo );
		}

        /// <summary>
        /// установить значение тега
        /// </summary>     
        public override void SetValueAsObject(object tagValueAsObject, DateTime dt, VarQualityNewDs vq)
        {
            this.SetValueAsObject(tagValueAsObject, dt, vq, TypeOfTag.Combo);
        }

        /// <summary>
        /// установить значение тега
        /// </summary>
        protected override void SetValue( byte[] memX, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag )
        {
            try
            {
                var tagEnumValueAsSingle = BitConverter.ToSingle( memX, 0 );
                var tagEnumValueAsInt32 = Convert.ToInt32(tagEnumValueAsSingle);

                // Проверяем, произошло ли изменение значения тега или его качества
                string newValueAsString = String.Empty;
                if (slEnumsParty.ContainsKey(tagEnumValueAsInt32))
                    newValueAsString = slEnumsParty[tagEnumValueAsInt32];
                //if (ValueAsString == newValueAsString && DataQuality == vq)
                //    return;

                ValueAsString = newValueAsString;

                if ( this.BindindTag != null )
                    this.BindindTag.ReadValue();

                base.SetValue( memX, dt, vq, typeOfTag );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }

        /// <summary>
        /// установить значение тега
        /// </summary>
        protected override void SetValueAsObject(object tagValueAsObject, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag)
        {
            try
            {
                if (!(tagValueAsObject is Boolean) && !(tagValueAsObject is Single))
                {
                    Console.WriteLine("Для TagEnum отброшено значение: " + tagValueAsObject.ToString() + " " + tagValueAsObject.GetType());
                    return;
                }

                // Делаем преобразование в Single, так как от нового ФК могут приходить Boolean дл типа BitEnum.
                if (tagValueAsObject is Boolean)
                    tagValueAsObject = (Boolean)tagValueAsObject ? (Single)1 : (Single)0;

                string newValueAsString = String.Empty;
                var indexenum = Convert.ToInt32((Single)tagValueAsObject);
                if (slEnumsParty.ContainsKey(indexenum))
                    newValueAsString = slEnumsParty[indexenum];
                //if (ValueAsString == newValueAsString && DataQuality == vq)
                //    return;

                // Заполняем ValueAsString
                ValueAsString = newValueAsString;

                if (BindindTag != null)
                    BindindTag.ReadValue();

                base.SetValueAsObject(tagValueAsObject, dt, vq, typeOfTag);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// установить значение тега
        /// по умолчанию (для его сброса)
        /// </summary>
        public override void SetDefaultValue()
        {
            SetValueAsObject((Single)0, DateTime.Now, VarQualityNewDs.vqUndefined);
        }
	}
}
