using System;

namespace HelperLibrary
{
    /// <summary>
    /// Контейнер контекста комманд
    /// </summary>
    public class CommandContent<T>
    {
        /// <summary>
        /// Команда
        /// </summary>
        public String Command { get; set; }
        /// <summary>
        /// Подпись
        /// </summary>
        public String Context { get; set; }
        /// <summary>
        /// Код команды
        /// </summary>
        public UInt32 Code { get; set; }
        /// <summary>
        /// Дополнительные параметры
        /// </summary>
        public T Parameter { get; set; }

        /// <summary>
        /// DataServer Guid
        /// </summary>
        public uint DsGuid { get; set; }

        /// <summary>
        /// Device Guid
        /// </summary>
        public uint DevGuid { get; set; }
    }
}