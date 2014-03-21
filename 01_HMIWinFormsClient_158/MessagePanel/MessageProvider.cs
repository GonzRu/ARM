using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Timers;
using MessagePanel.MessagePanelService;

namespace MessagePanel
{
    /// <summary>
    /// Класс для работы с неквитированными сообщениями
    /// </summary>
    public class MessageProvider : IMessageProvider
    {
        #region Private Constants
        /// <summary>
        /// Число, отправляемое сервису, но которое похоже ни на что не влияет
        /// </summary>
        private const int MAGIC_CONST = 7;

        /// <summary>
        /// Константы необходимые методам сервиса
        /// </summary>
        private const int KVOT_ONE = 0;
        private const int KVOT_DEVICE = 1;
        private const int KVOT_DATE = 2;
        private const int KVOT_ALL = 3;

        /// <summary>
        /// Количество запрашиваемых сообщений по-умолчанию
        /// </summary>
        private const int DEFAULT_MESSAGES_COUNT = 100;

        /// <summary>
        /// Интервал срабатывания таймера по-умолчанию
        /// </summary>
        private const int DEFAULT_TIMER_INTERVAL = 5000;
        #endregion

        #region Private Fields
        /// <summary>
        /// Класс для взаимодействия с сервером
        /// </summary>
        private DataChannelClient _messagePanelServerProvider;

        /// <summary>
        /// Таймер для постоянного запроса обновлений сообщений
        /// </summary>
        readonly private Timer _periodicUpdateMessagesTimer = new Timer();

        /// <summary>
        /// Список сообщений
        /// </summary>
        private List<TableEventLogAlarm> _messages;

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        private int _userID;

        /// <summary>
        /// Флаг, показывающий были ли уже попытки соединиться с сервером
        /// </summary>
        private bool _isFirstReconectFault = true;

        /// <summary>
        /// Количество запрашиваемых сообщений
        /// </summary>
        private int _shownMessagesCoount;

        /// <summary>
        /// IP-адрес сервера
        /// </summary>
        private string _serverIP;
        #endregion

        #region Constructor
        public MessageProvider(string serverIP)
        {
            _serverIP = serverIP;

            InitMessagePanelServerProvider();

            InitTimers();

            MessageCount = DEFAULT_MESSAGES_COUNT;
        }
        #endregion

        #region IMessageProvider

        #region Events
        /// <summary>
        /// Событие обновления списка сообщений
        /// </summary>
        public event Action MessagesUpdated;

        /// <summary>
        /// События появления сообщений, на которые пользователь должен обратить внимание
        /// </summary>
        public event Action AlarmMessagesAppeared;

        #endregion

        #region Metods
        /// <summary>
        /// Запускает работу объекта.
        /// </summary>
        public void StartWork(int userId)
        {
            _messages = null;
            _periodicUpdateMessagesTimer.Stop();

            _userID = userId;
            _periodicUpdateMessagesTimer.Start();           
        }

        /// <summary>
        /// Получить список сообщений
        /// </summary>
        public List<TableEventLogAlarm> GetMessages()
        {
            return _messages;
        }

        #region Квитирование
        /// <summary>
        /// Квитирование одного сообщения
        /// </summary>
        public bool KvotMessage(TableEventLogAlarm msg, string comment)
        {
            bool result = false;

            _periodicUpdateMessagesTimer.Stop();

            try
            {
                result = _messagePanelServerProvider.Kvoting(msg, comment, _userID, KVOT_ONE);

                GetMessagesFromServer();
            }
            catch
            {
                RestoreConnection();
            }

            _periodicUpdateMessagesTimer.Start();

            return result;
        }

        /// <summary>
        /// Квитирование всех сообщений
        /// </summary>
        public bool KvotAllMessages(string comment)
        {
            bool result = false;

            _periodicUpdateMessagesTimer.Stop();

            try
            {
                result = _messagePanelServerProvider.AllKvoting(comment, _userID, KVOT_ALL);

                GetMessagesFromServer();
            }
            catch
            {
                RestoreConnection();
            }

            _periodicUpdateMessagesTimer.Start();

            return result;
        }

        /// <summary>
        /// Квитирование всех сообщений устройства
        /// </summary>
        public bool KvotDeviceMessages(UInt32 deviceGuid, string comment)
        {
            bool result = false;

            _periodicUpdateMessagesTimer.Stop();

            try
            {
                result = _messagePanelServerProvider.KvotingEventDevice(comment, _userID, KVOT_DEVICE, (int) deviceGuid);

                GetMessagesFromServer();
            }
            catch
            {
                RestoreConnection();
            }

            _periodicUpdateMessagesTimer.Start();

            return result;
        }

        /// <summary>
        /// Квитирование всех сообщений в период между startDate и endDate
        /// </summary>
        public bool KvotMessagesInTimePeriod(DateTime startDate, DateTime endDate, string comment)
        {
            bool result = false;

            _periodicUpdateMessagesTimer.Stop();

            try
            {
                result = _messagePanelServerProvider.KvotingEventTime(comment, _userID, KVOT_DATE, startDate, endDate);

                GetMessagesFromServer();
            }
            catch
            {
                RestoreConnection();
            }

            _periodicUpdateMessagesTimer.Start();

            return result;
        }
        #endregion Квитирование

        #endregion

        #region Properties
        /// <summary>
        /// Получить или задать максимальное количество запрашиваемых сообщений
        /// </summary>
        public int MessageCount
        {
            get { return _shownMessagesCoount; }
            set
            {
                _shownMessagesCoount = value;
                _messages = null;
                GetMessagesFromServer();
            }
        }

        /// <summary>
        /// Общее количество сообщений
        /// </summary>
        public int TotalMessagesCount { get; private set; }
        #endregion

        #endregion

        #region Private Metods
        /// <summary>
        /// Устанавливает соединение с сервисом
        /// </summary>
        private void InitMessagePanelServerProvider()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            try
            {
                _messagePanelServerProvider = new DataChannelClient(binding, new EndpointAddress(String.Format("net.tcp://{0}:15100/Services", _serverIP)));
                _messagePanelServerProvider.Open();

                Console.WriteLine("MessagePanel: Установлено соединение с сервером сообщений.");
                _isFirstReconectFault = true;
            }
            catch (Exception)
            {
                if (_isFirstReconectFault)
                {
                    Console.WriteLine("MessagePanel: Не удалось установить соединение с сервером сообщений.");
                    _isFirstReconectFault = false;
                }
            }
        }

        /// <summary>
        /// Иницилизирует таймеры
        /// </summary>
        private void InitTimers()
        {
            _periodicUpdateMessagesTimer.Elapsed += PeriodicUpdateMessagesTimerOnElapsed;
            _periodicUpdateMessagesTimer.Interval = DEFAULT_TIMER_INTERVAL;
        }

        /// <summary>
        /// Получает сообщения
        /// </summary>
        private void GetMessagesFromServer()
        {
            try
            {
                if (_messagePanelServerProvider.NeedUpDate(_messages == null ? 0 : TotalMessagesCount))
                {
                    // Получение количества сообщений
                    var countRows = _messagePanelServerProvider.CountRowsData();
                    TotalMessagesCount = countRows.Failure + countRows.Info + countRows.Undefined + countRows.Warning;

                    // Получений необходимого количества сообщений
                    var a = _messagePanelServerProvider.GetEventLogAlarm(MessageCount, _userID, MAGIC_CONST);
                    _messages = new List<TableEventLogAlarm>(a.Tela);

                    if (MessagesUpdated != null)
                        MessagesUpdated();

                    if (AlarmMessagesAppeared != null)
                        AlarmMessagesAppeared();
                }
            }
            catch (Exception)
            {
                _messages = null;
                RestoreConnection();
            }
        }

        /// <summary>
        /// Восстанавливает соединение с сервисом
        /// </summary>
        private void RestoreConnection()
        {
            InitMessagePanelServerProvider();
        }
        #endregion

        #region Handlers
        private void PeriodicUpdateMessagesTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _periodicUpdateMessagesTimer.Stop();

            GetMessagesFromServer();

            _periodicUpdateMessagesTimer.Start();
        }
        #endregion
    }
}
