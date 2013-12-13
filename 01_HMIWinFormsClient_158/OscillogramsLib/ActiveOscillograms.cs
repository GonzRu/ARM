/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс представления активных осциллограмм
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\OscillogramsLib\ActiveOscillograms.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 28.11.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfaceLibrary;

namespace OscillogramsLib
{
    public class ActiveOscillograms : IActiveOscillograms
    {
		#region События
		#endregion

		#region Свойства
		#endregion

		#region public
		#endregion

		#region private
        /// <summary>
        /// Список активных команд:
        /// string - строка идентификации: ds.uniObjDev.idCMD
        /// IOscillogramma - ссылка на экземпляр класса осциллограмма
        /// </summary>
        private Dictionary<UInt32, IOscillogramma> dlistActiveOsc = new Dictionary<UInt32, IOscillogramma>();
        #endregion

		#region конструктор(ы)
		#endregion

        #region public-методы реализации интерфейса IActiveOscillograms
        /// <summary>
        /// добавить осциллограмму к активным осциллограммам ПТК
        /// </summary>
        /// <param name="cmd"></param>
        public void AddOsc(IOscillogramma osc)
        {
            try
            {
                if (!dlistActiveOsc.ContainsKey(osc.IdInBD))
                    dlistActiveOsc.Add(osc.IdInBD, osc);

                // теперь запрос на осциллограмму нужно передать своему dataserver для выполнения
                IDataServer ds = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServer((uint)osc.DS);
                ds.ReadOsc(osc);

            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// удалить осциллограмму из списка акт осциллограмм
        /// </summary>
        /// <param name="cmd"></param>
        public void RemoveOsc(UInt32 ident_in_bd, byte[] content)
        {
            try
            {
                IOscillogramma osc = null;

                if (dlistActiveOsc.ContainsKey(ident_in_bd))
                    osc = dlistActiveOsc[ident_in_bd];
                else
                    throw new Exception(string.Format("(87) : ActiveOscillograms.cs : RemoveOsc() : Попытка удаления несуществующей осциллограммы из списка активных осциллограмм : {0}", ident_in_bd));
                
                osc.ContentBlockOsc = new byte[content.Length];
                Buffer.BlockCopy(content, 0, osc.ContentBlockOsc, 0, content.Length); 

                // извещаем конфигурацию о получении осциллограмы
                osc.OSC_Received();

                dlistActiveOsc.Remove(ident_in_bd);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        #endregion

        #region public-методы
        #endregion

        #region private-методы
        #endregion
    }
}
