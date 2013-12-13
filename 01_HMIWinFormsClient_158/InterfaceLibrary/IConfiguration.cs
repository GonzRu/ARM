/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Интерфейс для взаимодействия с конфигурацией DataServer'ов
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\InterfaceLibrary\IConfiguration.cs
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
using System.Xml.Linq;
using System.Windows.Forms;

namespace InterfaceLibrary
{
    /// <summary>
    /// потеря связи с DS
    /// (событие от конфигурации)
    /// </summary>
    /// <param name="state"></param>
    public delegate void ConfigDSCommunicationLoss4Client(bool state);

    /// <summary>
    /// значения отражающие результат 
    /// запуска-стадии выполнения команды
    /// </summary>
    public enum CommandResult
    {
        _0_UNDEFINED = 0,
        _1_FAIL_TRIGGERING = 1,      // неудачный запуск команды
        _2_CMDSend2DS = 2,           // принятие конфигурацией запроса на выполнение команды
        _3_QUERY_ACCEPTANCE = 3,     // команда передана целевому DS
        _4_SUCCESS_TRIGGERING = 4    // удачный запуск команды
    }
    /// <summary>
    /// запрашиваемый тип записи
    /// </summary>
    public enum TypeBlockData4Req
    {
        /// <summary>
        /// неизвестный тип записи
        /// </summary>
        TypeBlockData4Req_Unknown = 0,
        /// <summary>
        /// запрос типа записи уставок
        /// </summary>
        TypeBlockData4Req_Ustavki = 1,

        /// <summary>
        /// запрос типа записи аварий
        /// </summary>
        TypeBlockData4Req_Srabat = 2,

        /// <summary>
        /// запрос типа записи осциллограмм
        /// </summary>
        TypeBlockData4Req_Osc = 3,

        /// <summary>
        /// запрос типа записи диаграмм
        /// </summary>
        TypeBlockData4Req_Diagramm = 4,

        /// <summary>
        /// запрос типа записи журнала событий блока
        /// </summary>
        TypeBlockData_LogEventBlock = 5,
    }

    ///// <summary>
    ///// типы записей в базе для запроса 
    ///// по типам блоков
    ///// </summary>
    //public enum TypeBlockData
    //{
    //    /// <summary>
    //    /// неизвестный тип записи
    //    /// </summary>
    //    TypeBlockData_Unknown = 0,
    //    /// <summary>
    //    /// ид типа записи уставок
    //    /// </summary>
    //    TypeBlockData_Ustavki = 1,

    //    /// <summary>
    //    /// ид типа записи аварий
    //    /// </summary>
    //    TypeBlockData_Srabat = 2,

    //    /// <summary>
    //    /// ид типа записи осциллограмм БМРЗ (ид = 4, расширение .osc)
    //    /// </summary>
    //    TypeBlockData_OscBMRZ = 4,

    //    /// <summary>
    //    /// ид типа записи диаграмм (ид = 5, расширение .dgm)
    //    /// </summary>
    //    TypeBlockData_Diagramm = 5,

    //    /// <summary>
    //    /// ид типа записи осциллограммы сириус (ид = 8, расширение .trd)
    //    /// </summary>
    //    TypeBlockData_OscSirius = 8,
    //    /// <summary>
    //    /// ид типа записи журнала событий блока
    //    /// </summary>
    //    TypeBlockData_LogEventBlock = 9,
    //    /// <summary>
    //    /// ид типа записи осциллограммы экра (ид = 10, расширение .dfr)
    //    /// </summary>
    //    TypeBlockData_OscEkra = 10,
    //    /// <summary>
    //    /// ид типа записи осциллограммы Бреслера (ид = 11, расширение .brs)
    //    /// </summary>
    //    TypeBlockData_OscBresler = 11,
    //}

    /// <summary>
    /// интерфейс доступа к конфигурации
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// событие от конфигурации
        /// потери связи с DataServer
        /// </summary>
        event ConfigDSCommunicationLoss4Client OnConfigDSCommunicationLoss4Client;
        /// <summary>
        /// ссылка на класс диспетчеризации 
        /// запросов для подписки/отписки на теги 
        /// конкретному  DataServer 
        /// </summary>
        ICfgReqEntry CfgReqEntry{get;}
        /// <summary>
        /// создать конфигурацию на основе 
        /// файлов конфигурации источника
        /// </summary>
        void CreateConfiguration(string path2prgdevcfg);
        /// <summary>
        /// получить ссылку на DataServer
        /// по его уникальному номеру
        /// </summary>
        /// <param name="unidsGuid">уник номер DataServer</param>
        /// <returns></returns>
        IDataServer GetDataServer(uint unidsGuid);
        /// <summary>
        /// возвратить массив уник номеров DataServer:
        /// </summary>
        /// <returns></returns>
        ArrayList GetDataServerNumbers();
        /// <summary>
        /// возвратить XElement - секцию с описанием DataServer
        /// </summary>
        /// <param name="dsnumber">уник номер DataServer</param>
        /// <returns></returns>
        XElement GetDataServerDescription(uint dsnumber);
        /// <summary>
        /// возвратить список устройств указанного DataServer
        /// </summary>
        /// <param name="dsnumber">уник номер DataServer</param>
        /// <returns></returns>
        List<IDevice> GetDataServerDevices(uint dsnumber);
        /// <summary>
        /// ссылка на тег 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="uniDevGuid"></param>
        /// <param name="uniTagGuid"></param>
        ITag GetLink2Tag(uint ds, uint uniDevGuid, uint uniTagGuid);
        /// <summary>
        /// получить ссылку на устройство
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="uniDevGuid"></param>
        /// <returns></returns>
        IDevice GetLink2Device(uint ds, uint uniDevGuid);
        /// <summary>
        /// извлечь описание устройства в конфиг файле источника
        /// по правилам нотации источника
        /// </summary>
        /// <param name="unids_guid">уник ном ds</param>
        /// <param name="src_name">имя источника</param>
        /// <param name="uniDevGuid">идентифиц устр в источнике</param>
        /// <returns></returns>
        XElement GetDeviceXMLDescription(int unids_guid,string src_name, int uniDevGuid);
        /// <summary>
        /// функция вычисления TagGUID 
        /// для тегов конкретного источника, если он
        /// формируется по определенному алгоритму
        /// </summary>
        /// <param name="unids_guid"></param>
        /// <param name="src_name"></param>
        /// <param name="idreg"></param>
        /// <param name="bitMsk"></param>
        /// <returns></returns>
        uint GetTagGUID(int unids_guid, string src_name, string str_dev_ident_in_src_notation);
        /// <summary>
        /// запустить команду верхнего уровня и передает устройству на низний уровень
        /// </summary>
        /// <param name="ds">уник номер DataServer</param>
        /// <param name="objectGuid">уник номер объекта</param>
        /// <param name="cmd">имя команды</param>
        /// <param name="parameters">массив параметров</param>
        /// <param name="parentfrm">родительская форма для случая когда нужно показывать окно выполнения команды, может
        /// быть null</param>
        /// <returns></returns>
        ICommand ExecuteCommand(uint ds, uint objectGuid, string cmd, byte[] parameters, Form parentfrm);
        /// <summary>
        /// активные (запущенные команды)
        /// </summary>
        IActiveCommands ActiveCommands{get;}
        /// <summary>
        /// установить элемент списка значений 
        /// типов архивных блоков
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetTypeBlockArchivData(string name, string value);
        /// <summary>
        /// получить значение элемента списка значений 
        /// типов архивных блоков
        /// </summary>
        /// <param name="name"></param>
        string GetTypeBlockArchivData(string name);
        /// <summary>
        /// запросить данные (уставки, аварии, осциллограммы) из БД
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="objectGuid"></param>
        /// <param name="comment"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        //IRequestData GetData(uint ds, uint objectGuid, string comment, ArrayList arparams);
        /// <summary>
        /// перегруженный вариант запроса данных (уставки, аварии) из БД
        /// клиент ждет специфич ответа по запросу от DS
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="objectGuid"></param>
        /// <param name="comment"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        IRequestData GetData(uint ds, uint objectGuid, string comment, ArrayList arparams, Int32 uniGuidRequest);
        /// <summary>
        /// активные осциллограммы (фрагменты для сборки)
        /// </summary>
        IActiveOscillograms ActiveOscillograms { get; }
        /// <summary>
        /// запрос на чтение осциллограммы/диаграммы
        /// </summary>
        /// <param name="idrec"></param>
        IOscillogramma GetOscData(UInt32 ds, UInt32 idrec);
    }
}
