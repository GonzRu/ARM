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
        public override void SetValue( byte[] memX, DateTime dt, VarQualityNewDs vq )
        {
            this.SetValue( memX, dt, vq, TypeOfTag.Analog );
        }

        /// <summary>
        /// установить значение тега
        /// </summary>     
        public override void SetValueAsObject(object tagValueAsObject, DateTime dt, VarQualityNewDs vq)
        {
            this.SetValueAsObject(tagValueAsObject, dt, vq, TypeOfTag.Analog);
        }

	    /// <summary>
	    /// установить значение тега
	    /// </summary>
        protected override void SetValue( byte[] memX, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag )
        {
            try
            {
                // Проверяем, произошло ли изменение значения тега или его качества
                string newValueAsString = memX.Length == 8
                                    ? Convert.ToSingle( BitConverter.ToDouble( memX, 0 ) ).ToString(
                                        string.Format( "F{0}", this.ValueDim ) )
                                    : BitConverter.ToSingle( memX, 0 ).ToString( string.Format( "F{0}", this.ValueDim ) );
                //if (this.ValueAsString == newValueAsString && DataQuality == vq)
                //    return;

                ValueAsString = newValueAsString;

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
        /// </summary>
        protected override void SetValueAsObject(object tagValueAsObject, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag)
        {
            try
            {
                if (!(tagValueAsObject is Single))
                {
#if DEBUG
                    Console.WriteLine(String.Format("Для TagAnalog ({0}.{1}.{2}) отброшено значение: {3} {4}", Device.UniDS_GUID, Device.UniObjectGUID, TagGUID, tagValueAsObject.ToString(), tagValueAsObject.GetType()));
#endif
                    return;
                }

                // Проверяем, произошло ли изменение значения тега или его качества
                string newValueAsString = ((Single) tagValueAsObject).ToString(String.Format("N{0}", this.ValueDim));
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
            SetValueAsObject((Single)0, DateTime.Now, VarQualityNewDs.vqUndefined);
        }
	}
}
