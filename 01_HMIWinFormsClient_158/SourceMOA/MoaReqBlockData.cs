/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Реализация класса запроса архивных данных
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\SourceMOA\MoaReqBlockData.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 26.11.2011 
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

namespace SourceMOA
{
    public class MoaReqBlockData : IRequestData
    {
        /// <summary>
        /// уник номер запроса
        /// если -1, то отслеживать выполнение запроса не нужно
        /// </summary>
        public Int32 UniGuidRequest { get{ return uniGuidRequest;} }
        Int32 uniGuidRequest = -1;

        /// <summary>
        /// код ошибки запроса
        /// </summary>
        public Int32 CodeReqError { get{return codeReqError;} set{codeReqError = value;} }
        Int32 codeReqError = -1;

        /// <summary>
        /// событие завершения запроса
        /// </summary>
        public event ReqExecuted OnReqExecuted;
        /// <summary>
        /// номер DS
        /// </summary>
        public uint DS
        { get { return dS; } }
        uint dS = 0xffffffff;
        /// <summary>
        /// уник номер устройства
        /// </summary>
        public uint ObjUni { get { return objUni; } }
        uint objUni = 0xffffffff;
        /// <summary>
        /// номер группы устройства
        /// </summary>
        public uint NumGroup { get { return numGroup; } }
        UInt16 numGroup = 0xffff;
        /// <summary>
        /// комментарий к запросу
        /// </summary>
        public string Comment { get { return comment; } }
        string comment = string.Empty;
        /// <summary>
        /// параметры команды
        /// </summary>
        public ArrayList ReqParams { get { return reqParams; } }
        ArrayList reqParams = new ArrayList();
        /// <summary>
        /// параметры запроса 
        /// в виде массива байт 
        /// (интерпритация в конечной точке приема)
        /// </summary>
        public byte[] ReqParamsAsByteAray 
        { 
            get{return reqParamsAsByteAray;}
            set{ reqParamsAsByteAray = value;}
        }
        byte[] reqParamsAsByteAray = null;

        public MoaReqBlockData(UInt16 numarchGroup)
        {
            numGroup = numarchGroup;
        }
        /// <summary>
        /// инициализация запроса
        /// </summary>
        /// <param name="arrParams">набор параметров</param>
        public void Init(ArrayList arrParams)
        {
            ArrayList arrThr = arrParams;
            try
            {
                dS = (uint)arrThr[0];
                objUni = (uint)arrThr[1];
                reqParams = (ArrayList)arrThr[2];
                uniGuidRequest = (int)arrThr[3];
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// завершение запроса в источнике
        /// </summary>
        public void REQ_Executed(byte returncode)
        {
            codeReqError = Convert.ToInt32(returncode);

            if (OnReqExecuted != null)
                OnReqExecuted(this);
        }
        /// <summary>
        /// анализ результата выполнения
        /// запроса и выполнение соотв действий
        /// (запись в журнал ПТК)
        /// </summary>
        public void AnalizeREQRez()
        {
        }
    }
}
