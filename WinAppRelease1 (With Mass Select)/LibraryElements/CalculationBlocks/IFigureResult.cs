using System.Drawing;

namespace LibraryElements.CalculationBlocks
{
    interface IFigureResult
    {
        /// <summary>
        /// Отрисовка элемента расчетных данных
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        void DrawElement( Graphics graphics, Rectangle rectangle );
    }
}