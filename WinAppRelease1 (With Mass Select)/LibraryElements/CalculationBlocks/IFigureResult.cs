using System.Drawing;

namespace LibraryElements.CalculationBlocks
{
    interface IFigureResult
    {
        /// <summary>
        /// ��������� �������� ��������� ������
        /// </summary>
        /// <param name="graphics">����������� ��������</param>
        /// <param name="rectangle">������� ��������</param>
        void DrawElement( Graphics graphics, Rectangle rectangle );
    }
}