/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: активные команды ПТК
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\CommandLib\MOAActiveCmd.cs
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
using System.Collections.Generic;
using System.Text;
using InterfaceLibrary;

namespace CommandLib
{
    public class ActiveCmd : IActiveCommands
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
        /// ICommand - ссылка на экземпляр класса команды
        /// </summary>
        private Dictionary<string, ICommand> dlistActiveCmd = new Dictionary<string,ICommand>();
		#endregion

		#region конструктор(ы)
		#endregion

        #region public-методы реализации интерфейса IActiveCommands
        /// <summary>
        /// добавить команду к активным командам ПТК
        /// </summary>
        /// <param name="cmd"></param>
        public void AddCmd(ICommand cmd)
        {
        	try
			{
                string key = string.Format("{0}.{1}.{2}", cmd.DS.ToString(), cmd.ObjUni.ToString(), cmd.CmdName);

                if (!dlistActiveCmd.ContainsKey(key))
                    dlistActiveCmd.Add(key, cmd);

                // теперь новую команду нужно передать своему dataserver для выполнения
                IDataServer ds = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServer((uint)cmd.DS);
                ds.ExecuteCMD(cmd);
                cmd.ResultTriggering = CommandResult._2_CMDSend2DS;
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
         }
        /// <summary>
        /// удалить команду из списка акт команд
        /// </summary>
        /// <param name="cmd"></param>
        public void RemoveCmd(string key, byte returncode)
        {
            // удаляем команду из списка активных команд ПТК
            //string key = string.Format("{0}.{1}.{2}", cmd.DS.ToString(), cmd.ObjUni.ToString(), cmd.CmdName);
            try
			{
                ICommand cmd = null;

                if (dlistActiveCmd.ContainsKey(key))
                    cmd = dlistActiveCmd[key];
                else
                    throw new Exception(string.Format("(80) : ActiveCmd.cs : RemoveCmd() : Попытка удаления несуществующей команды из списка активных команды : {0}", key));

                // извещаем конфигурацию о завершении команды
                cmd.CMD_Executed(returncode);

                dlistActiveCmd.Remove(key);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }
        #endregion

        #region public-методы
        #endregion

        #region private-методы
        #endregion
    }
}
