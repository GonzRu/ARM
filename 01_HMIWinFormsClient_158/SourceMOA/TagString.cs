/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс для определения строгового тега устройства
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\TagString.cs
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
using System.Text;
using InterfaceLibrary;

namespace SourceMOA
{
	public class TagString : Tag
	{
        public TagString()
        {
            TypeOfTagHMI = TypeOfTag.String;
        }

		/// <summary>
		/// установить значение тега
		/// </summary>
        public override void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq)
        {
            this.SetValue( memX, dt, vq, TypeOfTag.String );
		}

        /// <summary>
        /// установить значение тега
        /// </summary>     
        public override void SetValueAsObject(object tagValueAsObject, DateTime dt, VarQualityNewDs vq)
        {
            this.SetValueAsObject(tagValueAsObject, dt, vq, TypeOfTag.String);
        }

        /// <summary>
        /// установить значение тега
        /// </summary>
        protected override void SetValue( byte[] memX, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag )
        {
            try
            {
                // Проверяем, произошло ли изменение значения тега или его качества
                var enco = Encoding.GetEncoding( Tag.StringValueEncoding );
                string newValueAsString = enco.GetString( memX );
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
                // Проверяем, произошло ли изменение значения тега или его качества
                //if (ValueAsString == tagValueAsObject.ToString() && DataQuality == vq)
                //    return;

                ValueAsString = tagValueAsObject.ToString();

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
            try
            {
                string defvalue = string.Empty;
                this.ValueAsString = defvalue;

                System.Text.Encoding enco = Encoding.GetEncoding(Tag.StringValueEncoding);

                byte[] memx = enco.GetBytes(defvalue);

                if (this.BindindTag != null)
                    this.BindindTag.ReadValue();

                //base.SetValue(memx);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
	}
}
