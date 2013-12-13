/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс для определения аналогового тега устройства
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\TagAnalog.cs
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
	public class TagAnalog : Tag, ITagDim
	{
        /// <summary>
        /// Кол-во знаков после запятой
        /// </summary>
        public ushort ValueDim { get; set; }

        public TagAnalog()
        {
            TypeOfTagHMI = TypeOfTag.Analog;          
            DefValue = "0";
            ValueDim = 2;
        }
	    /// <summary>
	    /// установить значение тега
	    /// </summary>
	    /// <param name="memX"></param>
	    /// <param name="dt"></param>
	    /// <param name="vq"></param>        
        public override void SetValue( byte[] memX, DateTime dt, VarQualityNewDs vq )
        {
            this.SetValue( memX, dt, vq, TypeOfTag.Analog );
        }
	    /// <summary>
	    /// установить значение тега
	    /// </summary>
	    /// <param name="memX"></param>
	    /// <param name="dt"></param>
	    /// <param name="vq"></param>
        /// <param name="typeOfTag"> </param>
        protected override void SetValue( byte[] memX, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag )
        {
            try
            {
                TimeStamp = dt;
                DataQuality = vq;

                ValueAsString = memX.Length == 8
                                    ? Convert.ToSingle( BitConverter.ToDouble( memX, 0 ) ).ToString(
                                        string.Format( "F{0}", this.ValueDim ) )
                                    : BitConverter.ToSingle( memX, 0 ).ToString( string.Format( "F{0}", this.ValueDim ) );

				if (BindindTag != null)
					BindindTag.ReadValue();

                base.SetValue( memX, dt, vq, typeOfTag );
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
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
                const Single defvalue = 0;
                ValueAsString = defvalue.ToString("F2");

                //byte[] memx = new byte[4];

                ValueAsMemX = BitConverter.GetBytes(defvalue);

                if (BindindTag != null)
                    BindindTag.ReadValue();

                //base.SetValue(memx);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
	}
}
