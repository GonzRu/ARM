/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Интерфейс взаимодействия с описанием DataServer - 
 *	компонента зона ответсвенности которого отдельный объект (подстанция), 
 *	Элементы (устройства) этого объекта представляются разными источниками.
 *	Каждый источник и его устройства описаны формальным образом и объединены 
 *	заданием конфигурационной информации.
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\IDataServer.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 14.11.2011 
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
using System.Xml.Linq;

namespace InterfaceLibrary
{
       /// <summary>
        /// потеря связи с DS
        /// </summary>
        /// <param name="state"></param>
    public delegate void DSCommunicationLoss4Client(bool state);

    public interface IDataServer
    {
        /// <summary>
        /// событие потери связи с DataServer
        /// </summary>
        event DSCommunicationLoss4Client OnDSCommunicationLoss4Client;
        /// <summary>
        /// Уникальный id данного DS
        /// </summary>
        UInt32 UniDS_GUID{get;}
        /// <summary>
        /// получить ссылку на конфигурацию источника по его имени
        /// </summary>
        /// <param name="namesrccfg"></param>
        /// <returns></returns>
        ISourceConfiguration GetSrcCfgByName(string namesrccfg);
        /// <summary>
        /// ссылка на класс управлениния 
        /// актуальностью запросов к DataServer
        /// </summary>
        IRequestEntry ReqEntry{get;}
        /// <summary>
        /// создать конфигурацию DataServer'а
        /// </summary>
        void CreateConfigurationDS(string path2prgcfg,XElement DataServerXMLDescribe);
        /// <summary>
        /// получить xml-описание конфигурации 
        /// источников данного DS
        /// </summary>
        /// <returns></returns>
        XElement GetDataServerXMLDescribe();
        /// <summary>
        /// список объектов данного DS, поддерживающего интерфейс 
        ///		IDevice, т. е. имеющего состояние в виде набора тегов
        /// </summary>
        /// <returns></returns>
        List<IDevice> GetListDevice();
        /// <summary>
        /// разобрать ответ от DataServer
        /// </summary>
        /// <param name="data"></param>
        void ParseData(byte[] data);
        /// <summary>
        /// выполнить команду
        /// </summary>
        /// <param name="cmd"></param>
        void ExecuteCMD(ICommand cmd);
        /// <summary>
        /// выполнить запрос на данные
        /// </summary>
        /// <param name="req"></param>
        void ExecuteRequest(IRequestData req);
        /// <summary>
        /// инициировать процесс чтения осциллограммы osc с 
        /// текущего DS
        /// </summary>
        /// <param name="osc"></param>
        void ReadOsc(IOscillogramma osc);
    }
}
