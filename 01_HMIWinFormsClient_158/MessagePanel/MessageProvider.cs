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
            return _messagePanelServerProvider.Kvoting(msg, comment, _userID, KVOT_ONE);
        }

        /// <summary>
        /// Квитирование всех сообщений
        /// </summary>
        public bool KvotAllMessages(string comment)
        {
            return _messagePanelServerProvider.AllKvoting(comment, _userID, KVOT_ALL);
        }

        /// <summary>
        /// Квитирование всех сообщений устройства
        /// </summary>
        public bool KvotDeviceMessages(UInt32 deviceGuid, string comment)
        {
            return _messagePanelServerProvider.KvotingEventDevice(comment, _userID, KVOT_DEVICE, (int)deviceGuid);
        }

        /// <summary>
        /// Квитирование всех сообщений в период между startDate и endDate
        /// </summary>
        public bool KvotMessagesInTimePeriod(DateTime startDate, DateTime endDate, string comment)
        {
            return _messagePanelServerProvider.KvotingEventTime(comment, _userID, KVOT_ALL, startDate, endDate);
        }
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Получить или задать максимальное количество запрашиваемых сообщений
        /// </summary>
        public int MessageCount { get; set; }
        #endregion

        #endregion

        #region Private Metods
        private void InitMessagePanelServerProvider()
        {
            //_messagePanelServerProvider = new DataChannelClient("ServicePoint1");

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            _messagePanelServerProvider = new DataChannelClient(binding, new EndpointAddress("net.tcp://192.168.240.35:15100/Services"));
        }

        private void InitTimers()
        {
            _periodicUpdateMessagesTimer.Elapsed += PeriodicUpdateMessagesTimerOnElapsed;
            _periodicUpdateMessagesTimer.Interval = 5000;
        }
        #endregion

        #region Handlers
        private void PeriodicUpdateMessagesTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _periodicUpdateMessagesTimer.Stop();

            if (_messagePanelServerProvider.State == CommunicationState.Opened || _messagePanelServerProvider.State == CommunicationState.Created || _messagePanelServerProvider.State == CommunicationState.Opening)
            if (_messagePanelServerProvider.NeedUpDate(_messages == null ? 0 : _messages.Count))
            {

                try
                {
                    var a = _messagePanelServerProvider.GetEventLogAlarm(MessageCount, _userID, MAGIC_CONST);
                    _messages = new List<TableEventLogAlarm>(a.Tela);

                    if (MessagesUpdated != null)
                        MessagesUpdated();

                    if (AlarmMessagesAppeared != null)
                        AlarmMessagesAppeared();
                }
                catch (TimeoutException ex)
                {
                }
            }

            _periodicUpdateMessagesTimer.Start();
        }
        #endregion
    }
}
