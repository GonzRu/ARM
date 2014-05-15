/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс запросов на данные для клиента
 *                                                                             
 *	Файл                     : X:\Projects\33_Virica\Server_new_Interface\crza\CRZADevices\CRZADevices\CRZADeviceEv.cs                                         
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 07.02.2011 
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

namespace InterfaceLibrary
{
    public delegate void ReqExecuted(IRequestData req);

    public interface IRequestData
    {
        /// <summary>
        /// код ошибки запроса
        /// </summary>
        Int32 CodeReqError{get;set;}
        /// <summary>
        /// уник номер запроса
        /// если -1, то отслеживать выполнение запроса не нужно
        /// </summary>
        Int32 UniGuidRequest {get;}
        /// <summary>
        /// событие завершения запроса
        /// </summary>
        event ReqExecuted OnReqExecuted;
        /// <summary>
        /// номер DS
        /// </summary>
        uint DS { get; }
        /// <summary>
        /// уник номер устройства
        /// </summary>
        uint ObjUni { get; }
        /// <summary>
        /// уник номер устройства
        /// </summary>
        uint NumGroup { get; }
        /// <summary>
        /// комментарий к запросу
        /// </summary>
        string Comment { get; }
        /// <summary>
        /// параметры запроса
        /// </summary>
        ArrayList ReqParams { get; }
        /// <summary>
        /// параметры запроса 
        /// в виде массива байт 
        /// (интерпритация в конечной точке приема)
        /// </summary>
        byte[] ReqParamsAsByteAray { get; set; }
        /// <summary>
        /// инициализация команды
        /// </summary>
        /// <param name="arrParams">набор параметров</param>
        void Init(ArrayList arrParams);
        /// <summary>
        /// завершение запроса в источнике
        /// </summary>
        void REQ_Executed(byte returncode);
        /// <summary>
        /// анализ результата выполнения
        /// запроса и выполнение соотв действий
        /// (запись в журнал ПТК)
        /// </summary>
        void AnalizeREQRez();
    }
}
