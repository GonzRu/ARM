using System.Drawing;

namespace LibraryElements.CalculationBlocks
{
    interface IColorResult
    {
        /// <summary>
        /// Получить результат расчетов
        /// </summary>
        /// <returns>Результат расчетов</returns>
        Color GetResult();
    }
}