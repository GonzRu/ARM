/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: интерфейс запроса данных (из БД)
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\RequsEtntryLib\RequestDataFactory.cs
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
using SourceMOA;

namespace RequsEtntryLib
{
    public class RequestDataFactory
    {
        public IRequestData CreateRequestData(string typeCmd, ArrayList arrParams)
        {
            IRequestData req = null;
            /*
             * определение реального номера группы 
             * по уставкам и авариям происходит на сервере
             * и соответсвенно посылка результата обратно
             */
            try
            {
                switch (typeCmd)
                {
                    case "ArhivUstavkiBlockData":                        
                        req = new MoaReqBlockData(0xfffe);                        //14
                        req.Init(arrParams);
                        break;
                    case "ArhivAvariBlockData":
                        req = new MoaReqBlockData(0xfffd); //8
                        req.Init(arrParams);
                        break;
                    //case "ArhivAvariBlockData_BMRZ100":
                    //    req = new MoaReqBlockData();//0xffff
                    //    req.Init(arrParams);
                    //    break;
                    default:
                        throw new Exception(string.Format("Тип архивного запроса {0} не поддерживается", typeCmd));
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return req;
        }
    }
}
