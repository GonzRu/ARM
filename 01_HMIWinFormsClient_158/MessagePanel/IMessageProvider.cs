using System;
using System.Collections.Generic;
using MessagePanel.MessagePanelService;

namespace MessagePanel
{
    public interface IMessageProvider
    {
        #region Events
        /// <summary>
        /// Событие обновления списка сообщений
        /// </summary>
        event Action MessagesUpdated;

        /// <summary>
        /// События появления сообщений, на которые пользователь должен обратить внимание
        /// </summary>
        event Action AlarmMessagesAppeared;

        #endregion

        #region Metods
        /// <summary>
        /// Запускает работу объекта.
        /// </summary>
        void StartWork(int userId);

        /// <summary>
        /// Получить список сообщений
        /// </summary>
        List<TableEventLogAlarm> GetMessages();
        #endregion

        #region Properties
        /// <summary>
        /// Получить или задать максимальное количество запрашиваемых сообщений
        /// </summary>
        int MessageCount { get; set; }
        #endregion
    }
}
