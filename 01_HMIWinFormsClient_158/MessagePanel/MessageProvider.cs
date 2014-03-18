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
