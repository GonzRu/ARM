/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс общедоступных полей проекта
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\HMI_MT\HMI_Settings.cs
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
using System.Collections.Specialized;
using System.Drawing;
using System.Xml.Linq;
using InterfaceLibrary;
using System.Windows.Forms;
using DebugStatisticLibrary;

namespace HMI_MT_Settings
{
    /// <summary>
    /// HMI_Settings
    /// </summary>
    public static class HMI_Settings
    {
        /// <summary>
        /// Proxy для роутера
        /// </summary>
        //public static DSRouterClient WCFproxy;
        public static IDSRouter WCFproxy;

        /// <summary>
        /// Конфигурация текущего проекта
        /// </summary>
        public static IConfiguration CONFIGURATION;
        /// <summary>
        /// путь к файлу проекта Project.cfg 
        /// </summary>
        public static string PathToPrjFile;
        /// <summary>
        /// xml-представление файла PathToPrjFile
        /// </summary>
        public static XDocument XDoc4PathToPrjFile;
        /// <summary>
        /// путь к файлу конфигурации клиента Configuration.cfg 
        /// </summary>
        public static string PathToConfigurationFile;
        /// <summary>
        /// xml-представление файла PathToPrjFile
        /// </summary>
        public static XDocument XDoc4PathToConfigurationFile;
        /// <summary>
        /// путь к файлу описания адаптеров PanelState.xml
        /// для панели состояний устройств ПТК 
        /// </summary>
        public static string PathPanelState_xml;
        /// <summary>
        /// xml-представление файла PathPanelState_xml
        /// </summary>
        public static XDocument XDoc4PathPanelState_xml;
        /// <summary>
        /// ссылка на главную форму
        /// </summary>
        public static Form Link2MainForm;
        /// <summary>
        /// ip адрес DS для его перезапуска
        /// </summary>
        public static string IPADDRES_SERVER;
        /// <summary>
        /// Url адрес для подключения к Ауре
        /// </summary>
        public static string AuraUrl;
        /// <summary>
        /// Коэффициент масштабирования схемы
        /// </summary>
        public static PointF SchemaSize = new PointF( 1.0f, 1.0f );
        /// <summary>
        /// Релятивная ссылка на файл для панели диагностики
        /// </summary>
        public static String DiagnosticSchema;
        /// <summary>
        /// Релятивная ссылка на файл для мнемосхемы
        /// </summary>
        public static String MainMnenoSchema;
        
        #region  Информация о пользователе
        public static ArrayList alMenu;
        public static int UserID;      // идентификатор пользователя
        public static string UserName = ""; // имя пользователя
        public static string UserPassword = ""; // пароль пользователя
        public static string UserComment = "";  // комментарий
        public static int IDRights;        // идентификатор группы прав
        public static string GroupName = "";    // название группы прав
        public static string GroupComment = ""; // комментарий группы прав
        public static string UserRight = "";   // права пользователя
        public static ArrayList arrlUserMenu = new ArrayList();
        public static string UserMenu
        {
            set
            {
                char[] delim = { ';' };
                string[] miAD = value.Split(delim);
                // заполняем массив
                uint itmp;
                string stmp;

                arrlUserMenu.Clear();
                for (int i = 0; i < miAD.Length; i++)
                {
                    stmp = miAD[i];
                    if (stmp == "")
                        continue;
                    char ctmp = stmp[0];
                    if (!Char.IsDigit(ctmp))
                        continue;
                    itmp = Convert.ToUInt32(stmp);
                    arrlUserMenu.Add(itmp);
                }
            }
        }
        public static bool isReqPassword
        {
            get
            {
                return HMI_Settings.isRegPass;
            }
            set
            {
                HMI_Settings.isRegPass = value;
            }
        }
        /// <summary>
        /// текущее время компьютера (синхронизированное с ФК)
        /// </summary>
        public static DateTime CurrentDateTime;
        public static StringDictionary sdUserRightsFull = new StringDictionary();  // все доступные права с распределением их по битам
        public static StringDictionary sdUserRights = new StringDictionary();      // права данного пользователя - идентификация прав по их названию
        #endregion

        #region подписка\отписка тегов на обновление по содержимому control'a
        public enum SubscribeAction
        {
            Subscribe = 0,
            UnSubscribe = 1,
            UnSubscribeAndClear = 2,
        }
        /// <summary>
        /// Подписаться\отписаться на теги участвующие 
        /// в формировании интерфейсных элементов формы(панели и т.п.)
        /// </summary>
        /// <param Name="cc"></param>
        public static void HMIControlsTagReNew(Control cntrl, SubscribeAction sa)
        {
            if (cntrl == null)
                return;

            var list = new List<ITag>();
            CollectTags( cntrl.Controls, list );

            HmiTagsSubScribes( list, sa );
        }
        public static void HmiTagsSubScribes(List<ITag> tags, SubscribeAction subscribeAction )
        {
            try
            {
                switch ( subscribeAction )
                {
                    case SubscribeAction.Subscribe:
                        DebugStatistics.WindowStatistics.AddStatistic( "Запуск подписки тэгов." );

                        CONFIGURATION.CfgReqEntry.SubscribeTags( tags );

                        DebugStatistics.WindowStatistics.AddStatistic( "Запуск подписки тэгов завершен." );
                        break;

                    case SubscribeAction.UnSubscribe:
                        DebugStatistics.WindowStatistics.AddStatistic( "Запуск отпыски тэгов." );

                        CONFIGURATION.CfgReqEntry.UnSubscribeTags( tags );

                        //foreach ( ISubscribe hmielements in lstSubscribeControls )
                        //    hmielements.UnSubscribeTagReNew();

                        DebugStatistics.WindowStatistics.AddStatistic( "Запуск отписки тэгов завершен." );
                        break;

                    case SubscribeAction.UnSubscribeAndClear:
                        DebugStatistics.WindowStatistics.AddStatistic( "Запуск отпыски тэгов." );

                        foreach ( var tag in tags ) tag.SetDefaultValue( );
                        CONFIGURATION.CfgReqEntry.UnSubscribeTags( tags );

                        DebugStatistics.WindowStatistics.AddStatistic( "Запуск отписки тэгов завершен." );
                        break;
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }

        private static void CollectTags( Control.ControlCollection controls, IList<ITag> list )
        {
            for (var i=0; i < controls.Count; i++ )
            {
                if ( controls[i] is IHMITagAccess )
                    list.Add( ((IHMITagAccess)controls[i]).LinkedTag );
                else CollectTags( controls[i].Controls, list );
            }
        }

        public static TabPage SetTagsSubscribe4TPCurrent(TabPage tp)
        {
            if (tp.Tag == null)
                tp.Tag = false;

            if (((bool)tp.Tag) == false)
            {
                HMIControlsTagReNew(tp, SubscribeAction.Subscribe);
                tp.Tag = true;
            }
            return tp;
        }
        #endregion

        #region от старого варианта ПТК - необходимо пересмотреть и удалить лишнее
        /// <summary>
        /// запрос пароля для опасных действий
        /// </summary>
        public static bool isRegPass;	// 
        //журнал событий
        public static string pathLogEvent_pnl4;	// путь к журналу событий
        public static int sizeLog_pnl4;			// размер журнала
        public static string whatToDoLog_pnl4;			// что делать с файлом журнала при его переполнении
        /// <summary>
        /// показывать подсказку при наведении мыши на блок
        /// </summary>
        public static bool IsShowToolTip;
        /// <summary>
        /// показывать условные обозначения устройств вместо названий блоков
        /// </summary>
        public static bool IsToolTipRefDesign;
        /// <summary>
        /// показывать tabControl на главной форме
        /// </summary>
        public static bool IsShowTabForms;
        // точность значений с плавающей точкой
        public static string Precision;
        // ведение локального журнала только на диске, без окна текущих сообщений
        public static bool LogOnlyDisk;
        // Роль АРМ - TCP-сервер или TCP-клиент (вторичный)
        public static bool IsTCPServer;
        public static bool IsTCPClient;
        public static bool IsUDPSecondClient;
        // порт для входящих соединений
        public static int PORTin;
        // порт для исходящих соединений
        public static int PORTout;
        /// <summary>
        /// тип запускаемой системы - локальная или удаленная - уточняется при загрузке MainForm
        /// </summary>
        //public static bool IsLocalSystem;
        /// <summary>
        /// Необходимость ввода логина и пароля при входе в АРМ
        /// </summary>
        public static bool isNeedLoginAndPassword = true;
        // IP-адрес клиента - используется для ping в сервере
        public static string IPADDRES_CLIENT;
        // количество подключений
        public static int NUMBER_CONNECTING;
        // интервал обновления
        //public static int IntervalDataReNew;
        // ip клиента для сериализации панели сообщений
        public static string IPPointForSerializeMesPan;
        // порт клиента для сериализации панели сообщений
        public static uint PortPointForSerializeMesPan;
        // список серверов, которым посылаются теги (сервера OPC, тег менеджер)
        public static SortedList slOPCTaglServers = new SortedList();
        /// <summary>
        /// строка соединения с БД - SqlProviderPTK
        /// </summary>
        public static string ProviderPtkSql;
        /// <summary>
        /// строка соединения с БД - OleProviderPTK
        /// </summary>
        public static string ProviderPtkOleSql;
        /// <summary>
        /// путь к файлу проекта PrgDevCFG.cdp
        /// </summary>
        public static string PathToPrgDevCFG_cdp;
        /// <summary>
        /// xml-представление файла PathToPrgDevCFG
        /// </summary>
        public static XDocument XDoc4PathToPrgDevCFG;
        /// <summary>
        /// имя канала для транспортного уровня 
        /// </summary>      
        //public static string PipeName;
        /// <summary>
        /// Показывать стандартные кнопки управления окном?
        /// </summary>      
        public static string ViewBtn4MainWindow;
        /// <summary>
        /// Скрывать строку статуса Windows?
        /// </summary>      
        public static string HideWindowLineStatus;
        /// <summary>
        /// Факт наличия активной антенны GPS
        /// </summary>
        public static bool IsGPSActive;
        /// <summary>
        /// Код состояния антенны GPS
        /// </summary>
        public static byte GPSActiveCode;
        /// <summary>
        /// Строка описывающая состояние GPS
        /// </summary>
        public static string GPSActiveCodeMessage;
        /// <summary>
        /// класс (используемый в TCP-клиенте) с таблицами для организации целевого запроса данных с сервера
        /// </summary>
        //public static ClientDataForExchange ClientDFE; 
        #endregion
    }
}
