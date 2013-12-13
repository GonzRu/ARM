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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
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
		/// <param name="memX"></param>
        public override void SetValue(byte[] memX, DateTime dt, VarQualityNewDs vq)
        {
            this.SetValue( memX, dt, vq, TypeOfTag.Combo );
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

                var sind = BitConverter.ToSingle( memX, 0 );
                //var sind = Convert.ToInt32( memX[0] );

                var indexenum = Convert.ToInt32( sind );

                if ( slEnumsParty.ContainsKey( indexenum ) )
                    this.ValueAsString = slEnumsParty[indexenum];

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
                Single defvalue = 0;
                if (SlEnumsParty.Count >= 1)
                    this.ValueAsString = this.SlEnumsParty.ElementAt(0).Value;
                else
                    this.ValueAsString = defvalue.ToString();

                byte[] memx = new byte[4];

                memx = BitConverter.GetBytes(defvalue);

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
