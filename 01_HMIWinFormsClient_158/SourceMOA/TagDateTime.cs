/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс для определения дата/время тега устройства
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\TagDateTime.cs
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
using InterfaceLibrary;

namespace SourceMOA
{
 	public class TagDateTime : Tag
    {
        #region Constructors

        public TagDateTime()
        {
            TypeOfTagHMI = TypeOfTag.DateTime;
            DefValue = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(DateTime.MinValue);
        }

        #endregion

        #region Public properties


        #endregion

        #region Public metods

        /// <summary>
 	    /// установить значение тега
 	    /// </summary>
 	    public override void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq)
        {
            this.SetValue( memX, dt, vq, TypeOfTag.DateTime );
		}

        /// <summary>
        /// установить значение тега
        /// </summary>     
        public override void SetValueAsObject(object tagValueAsObject, DateTime dt, VarQualityNewDs vq)
        {
            this.SetValueAsObject(tagValueAsObject, dt, vq, TypeOfTag.DateTime);
        }

        /// <summary>
        /// установить значение тега
        /// </summary>
        protected override void SetValue( byte[] memX, DateTime dtt, VarQualityNewDs vq, TypeOfTag typeOfTag )
        {
            try
            {
                // Проверяем, произошло ли изменение значения тега или его качества
                var dt = new DateTime( BitConverter.ToInt64( memX, 0 ) );
                string newValueAsString = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat( dt );
                //if (this.ValueAsString == newValueAsString && this.DataQuality == vq)
                //    return;

                ValueAsString = newValueAsString;

                if ( this.BindindTag != null )
                    this.BindindTag.ReadValue();

                base.SetValue( memX, dtt, vq, typeOfTag );

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
                string newValueAsString = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat((DateTime)tagValueAsObject);
                //if (this.ValueAsString == newValueAsString && this.DataQuality == vq)
                //    return;

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
            SetValueAsObject(DateTime.MinValue, DateTime.Now, VarQualityNewDs.vqUndefined);
        }

        #endregion
    }
}
