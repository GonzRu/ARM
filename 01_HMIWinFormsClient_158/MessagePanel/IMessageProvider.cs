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

        #region Квитирование

        /// <summary>
        /// Квитирование одного сообщения
        /// </summary>
        bool KvotMessage(TableEventLogAlarm msg, string comment);

        /// <summary>
        /// Квитирование всех сообщений
        /// </summary>
        bool KvotAllMessages(string comment);

        /// <summary>
        /// Квитирование всех сообщений устройства
        /// </summary>
        bool KvotDeviceMessages(UInt32 deviceGuid, string comment);
        
        /// <summary>
        /// Квитирование всех сообщений в период между startDate и endDate
        /// </summary>
        bool KvotMessagesInTimePeriod(DateTime startDate, DateTime endDate, string comment);

        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Получить или задать максимальное количество запрашиваемых сообщений
        /// </summary>
        int MessageCount { get; set; }

        /// <summary>
        /// Общее количество сообщений
        /// </summary>
        int TotalMessagesCount { get; }
        #endregion
    }
}
