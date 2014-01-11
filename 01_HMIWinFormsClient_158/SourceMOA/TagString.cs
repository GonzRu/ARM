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
		/// <param name="memX"></param>
        public override void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq)
        {
            this.SetValue( memX, dt, vq, TypeOfTag.String );
		}
        /// <summary>
        /// установить значение тега
        /// </summary>
        /// <param name="memX"></param>
        protected override void SetValue( byte[] memX, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag )
        {
            try
            {
                TimeStamp = dt;
                DataQuality = vq;

                var enco = Encoding.GetEncoding( Tag.StringValueEncoding );
                ValueAsString = enco.GetString( memX );

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
        /// по умолчанию (для его сброса)
        /// </summary>
        /// <param name="memX"></param>
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
