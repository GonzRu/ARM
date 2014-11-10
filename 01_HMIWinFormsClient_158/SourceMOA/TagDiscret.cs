/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс для определения дискретного тега устройства
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\SourceMOA\TagDiscret.cs
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
using System.Globalization;
using InterfaceLibrary;

namespace SourceMOA
{
	public class TagDiscret : Tag
    {
        #region Constructors

        public TagDiscret()
        {
            TypeOfTagHMI = TypeOfTag.Discret;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Строковое представление значения тега
        /// </summary>
        public override string ValueAsString
        {
            get { return BitConverter.ToBoolean(ValueAsMemX, 0).ToString(CultureInfo.InvariantCulture); }
        }

        #endregion

        #region Public metods

        /// <summary>
		/// установить значение тега
		/// </summary>
		public override void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq)
		{
			this.SetValue( memX, dt, vq, TypeOfTag.Discret );
		}

        /// <summary>
        /// установить значение тега
        /// </summary>     
        public override void SetValueAsObject(object tagValueAsObject, DateTime dt, VarQualityNewDs vq)
        {
            this.SetValueAsObject(tagValueAsObject, dt, vq, TypeOfTag.Discret);
        }

        /// <summary>
        /// установить значение тега
        /// </summary>
        protected override void SetValue( byte[] memX, DateTime dt, VarQualityNewDs vq, TypeOfTag typeOfTag )
        {
            try
            {
                var tmp = BitConverter.ToBoolean( memX, 0 );
                if ( IsInverse && ( this.DataQuality == VarQualityNewDs.vqGood ) )
                    tmp = !tmp;

                memX = BitConverter.GetBytes( tmp );

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
                if (!(tagValueAsObject is Boolean))
                {
#if DEBUG
                    Console.WriteLine(String.Format("Для TagDiscret ({0}.{1}.{2}) отброшено значение: {3} {4}", Device.UniDS_GUID, Device.UniObjectGUID, TagGUID, tagValueAsObject.ToString(), tagValueAsObject.GetType()));
#endif
                    return;
                }

                Boolean tmp = (Boolean) tagValueAsObject;
                if (IsInverse && (this.DataQuality == VarQualityNewDs.vqGood))
                    tmp = !tmp;

                if (BindindTag != null)
                    BindindTag.ReadValue();

                base.SetValueAsObject(tmp, dt, vq, typeOfTag);
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
            SetValueAsObject(false, DateTime.Now, VarQualityNewDs.vqUndefined);
        }

        #endregion
    }
}
