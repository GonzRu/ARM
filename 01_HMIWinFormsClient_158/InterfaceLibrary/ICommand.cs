/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Интерфейс представляющий активную команду
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\ICommand.cs
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
using System.Text;
using System.Windows.Forms;

namespace InterfaceLibrary
{
    public delegate void CmdExecuted(ICommand cmd);

    public interface ICommand
    {
        /// <summary>
        /// событие завершения команды
        /// </summary>
        event CmdExecuted OnCmdExecuted;
        /// <summary>
        /// значения отражающие результат 
        /// запуска-стадии выполнения команды
        /// </summary>
        CommandResult ResultTriggering { get;set; }
        /// <summary>
        /// номер DS
        /// </summary>
        uint DS { get; }
        /// <summary>
        /// уник номер устройства
        /// </summary>
        uint ObjUni {get;}
        /// <summary>
        /// имя команды
        /// </summary>
        string CmdName { get; }
        /// <summary>
        /// диспетчерское имя команды
        /// (для использования в интерфейсе)
        /// </summary>
        string CmdDispatcherName { get; }
        /// <summary>
        /// параметры команды
        /// </summary>
        byte[] Alparams{ get; }
        /// <summary>
        /// инициализация команды
        /// </summary>
        /// <param name="arrParams">набор параметров</param>
        void Init(ArrayList arrParams);
        /// <summary>
        /// завершение команды в источнике
        /// </summary>
        void CMD_Executed(byte returncode);
        /// <summary>
        /// анализ результата выполнения
        /// команды и выполнение соотв действий
        /// (запись в журнал ПТК)
        /// </summary>
        void AnalizeCMDRez();
        /// <summary>
        /// класс длительной операции 
        /// с выводом неблокирующего диалового окна
        /// </summary>
        ILongTimeAction Lta {get; set;}
    }
}
