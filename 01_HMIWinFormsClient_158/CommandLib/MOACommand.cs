/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Команда уровня HMI (ориентация на MOA)
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\CommandLib\MOACommand.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 18.11.2011 
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
using System.Text;
using InterfaceLibrary;
using System.Windows.Forms;

namespace CommandLib
{
    public class MOACommand : ICommand
    {
        #region События
        /// <summary>
        /// событие завершения команды
        /// </summary>
        public event CmdExecuted OnCmdExecuted; 
		#endregion

		#region Свойства
		#endregion

		#region public
		#endregion

		#region private
        /// <summary>
        /// родит форма для диалог окна с процессом выполнения команды
        /// </summary>
        Form parentfrm = null;
		#endregion

		#region конструктор(ы)
		#endregion

        #region public-методы реализации интерфейса ICommand
        /// <summary>
        /// класс длительной операции 
        /// с выводом неблокирующего диалового окна
        /// </summary>
        public ILongTimeAction Lta { get {return lta;} set { lta = value;}}
        ILongTimeAction lta = null;
        /// <summary>
        /// номер DS
        /// </summary>
        public uint DS { get{return dS;} }
        uint dS = 0xffffffff;
        /// <summary>
        /// уник номер устройства
        /// </summary>
        public uint ObjUni {get{return objUni;}}
         uint objUni = 0;
        /// <summary>
        /// имя команды
        /// </summary>
        public string CmdName { get { return cmdName; } }
        string cmdName= string.Empty;
        /// <summary>
        /// диспетчерское имя команды
        /// (для использования в интерфейсе)
        /// </summary>
        public string CmdDispatcherName 
        { get{return cmdDispatcherName;} }
        string cmdDispatcherName = string.Empty;
        /// <summary>
        /// параметры команды
        /// </summary>
        public byte[] Alparams { get { return alparams; } }
        byte[] alparams = new byte[]{};
        /// <summary>
        /// значения отражающие результат 
        /// запуска-стадии выполнения команды
        /// </summary>
        public CommandResult ResultTriggering
        {
            get { return resultTriggering; }
            set { resultTriggering = value;}
        }
        CommandResult resultTriggering = CommandResult._0_UNDEFINED;
        /// <summary>
        /// инициализация команды
        /// </summary>
        /// <param name="arrParams">набор параметров</param>
        public void Init(ArrayList arrParams)
        {
            ArrayList arrThr = arrParams;//(ArrayList)e.Argument;
			try
			{
                dS = (uint)arrThr[0];
                objUni = (uint)arrThr[1];
                cmdName = (string)arrThr[2];
                alparams = (byte[])arrThr[3];
                parentfrm = (Form)arrThr[4];

                // выясним диспетчерское название команды
                IDevice dev;
                IDeviceCommand dc;

                if (dS == 0 && objUni == 0)
                {
                    // команда всем ECU?
                    cmdDispatcherName = CmdName;                
                }
                else
                {
                    dev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device(dS, objUni);
                    dc = dev.GetDeviceCommandByShortName(CmdName);
                    cmdDispatcherName = dc.CmdDispatcherName;

                    if (string.IsNullOrWhiteSpace(CmdDispatcherName) )
                        cmdDispatcherName = CmdName;                

                    if (parentfrm != null)
                    {
                        lta = new CommonUtils.LongTimeAction();
                        lta.Start(parentfrm, string.Format("Команда {0} запущена на выполнение", CmdDispatcherName), "Выполнение команды", "Закрыть");
                    }
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
                resultTriggering = CommandResult._1_FAIL_TRIGGERING;  // запрос не выполнен
                return;
			}

             resultTriggering = CommandResult._3_QUERY_ACCEPTANCE;  // запрос принят
        }
        /// <summary>
        /// завершение команды в источнике
        /// </summary>
        public void CMD_Executed(byte returncode)
        { 
            switch(returncode)
            {
                case 0:
                    this.ResultTriggering = CommandResult._4_SUCCESS_TRIGGERING;
                break;
                default:
                    this.ResultTriggering= CommandResult._1_FAIL_TRIGGERING;
                break;
            }
            if (OnCmdExecuted != null)
                OnCmdExecuted(this);
            
            // регистрационные действия по выполнению команды
            AnalizeCMDRez();
        }

        /// <summary>
        /// анализ результата выполнения
        /// команды и выполнение соотв действий
        /// (запись в журнал ПТК и т.п.)
        /// </summary>
        public void AnalizeCMDRez()
        {
            switch (ResultTriggering)
            {
                case CommandResult._4_SUCCESS_TRIGGERING:
                    CommonUtils.CommonUtils.WriteEventToLog(35, string.Format("Команда {0} ушла в сеть. Устройство - DS={1}.ObjUni={2}", this.CmdName, this.DS, this.ObjUni), true);
                    break;
                case CommandResult._1_FAIL_TRIGGERING:
                    break;
                case CommandResult._0_UNDEFINED:
                    break;
                default:
                    break;
            }
        }
		#endregion

		#region public-методы
		#endregion

		#region private-методы
		#endregion
    }
}
