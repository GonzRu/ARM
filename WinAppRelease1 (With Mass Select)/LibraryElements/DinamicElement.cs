using System;

namespace LibraryElements
{
    /// <summary>
    /// Класс динамических параметров
    /// </summary>
    public class DynamicParameters
    {
        public DynamicParameters() { ToolTipMessage = Type = String.Empty; }

        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="original">Элемент на основе которого делается копия</param>
        public void CopyElement( DynamicParameters original )
        {
            ToolTipMessage = original.ToolTipMessage;
            Type = original.Type;
            DsGuid = original.DsGuid;
            DeviceGuid = original.DeviceGuid;
            Cell = original.Cell;
            ExternalDescription = original.ExternalDescription;
        }

        /// <summary>
        /// Получить или задать более детальныю подсказку для элемента.
        /// (Если подсказка не задана, подставляется стандартная подсказка)
        /// </summary>
        public string ToolTipMessage { get; set; }
        /// <summary>
        /// Получить или задать тип привязки
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Получить или задать номер 
        /// </summary>
        public uint DsGuid { get; set; }
        /// <summary>
        /// Получить или задать номер девайса
        /// </summary>
        public uint DeviceGuid { get; set; }
        /// <summary>
        /// Получить или задать номер клетки
        /// </summary>
        public uint Cell { get; set; }
        /// <summary>
        /// Получить или задать признак описания поведения вне элемента
        /// </summary>
        public bool ExternalDescription { get; set; }
    }
}