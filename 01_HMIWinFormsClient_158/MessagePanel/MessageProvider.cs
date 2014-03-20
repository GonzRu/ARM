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
        /// Общее количество сообщений
        /// </summary>
        private int _totalMessageCount;
        #endregion

        #region Constructor
        public MessageProvider()
        {
            InitMessagePanelServerProvider();

            InitTimers();

            MessageCount = 100;
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
            _periodicUpdateMessagesTimer.Start();
            _userID = userId;
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
        public int MessageCount { get; set; }
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
                _messagePanelServerProvider = new DataChannelClient(binding, new EndpointAddress("net.tcp://192.168.240.35:15100/Services"));
                _messagePanelServerProvider.Open();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Иницилизирует таймеры
        /// </summary>
        private void InitTimers()
        {
            _periodicUpdateMessagesTimer.Elapsed += PeriodicUpdateMessagesTimerOnElapsed;
            _periodicUpdateMessagesTimer.Interval = 5000;
        }

        /// <summary>
        /// Получает сообщения
        /// </summary>
        private void GetMessagesFromServer()
        {
            try
            {
                if (_messagePanelServerProvider.NeedUpDate(_messages == null ? 0 : _totalMessageCount))
                {
                    // Получение количества сообщений
                    var countRows = _messagePanelServerProvider.CountRowsData();
                    _totalMessageCount = countRows.Failure + countRows.Info + countRows.Undefined + countRows.Warning;

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
