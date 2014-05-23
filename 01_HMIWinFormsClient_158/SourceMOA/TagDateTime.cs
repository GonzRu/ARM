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
        public TagDateTime()
        {
            TypeOfTagHMI = TypeOfTag.DateTime;
            DefValue = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(DateTime.MinValue);
        }

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
                TimeStamp = dtt;
                DataQuality = vq;

                //DateTime dt = DateTime.FromBinary(BitConverter.ToInt64(memX,0));
                var dt = new DateTime( BitConverter.ToInt64( memX, 0 ) );
                this.ValueAsString = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat( dt );

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
                TimeStamp = dt;
                DataQuality = vq;

                ValueAsString = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat((DateTime)tagValueAsObject);

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
                DateTime defvalue = DateTime.MinValue;

                this.ValueAsString = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(defvalue);

                //byte[] memx = new byte[8];

                this.ValueAsMemX = BitConverter.GetBytes(defvalue.Ticks);

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
