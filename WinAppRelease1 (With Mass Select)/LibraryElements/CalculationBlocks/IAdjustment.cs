
using System;
namespace LibraryElements.CalculationBlocks
{
    /// <summary>
    /// Интерфейс корректировки тэгов
    /// </summary>
    public interface IAdjustment
    {
        /// <summary>
        /// Корректировка тэгов
        /// </summary>
        Boolean IsAdjustment { get; set; }
    }
}
