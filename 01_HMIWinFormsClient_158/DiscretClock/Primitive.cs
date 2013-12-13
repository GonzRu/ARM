////////////////////////////////////////////////////////////////
//  Цифровые часы
//  Базовый класс примитива
////////////////////////////////////////////////////////////////
using System.Drawing;

//==============================================================
namespace Egida
{
    /// <summary>
    /// Базовый класс примитива
    /// </summary>
    internal abstract class Primitive
    {
        /// <summary>
        /// Функция вывода
        /// </summary>
        /// <param name="g">
        /// Объект для вывода графика
        /// </param>
        /// <param name="shift">
        /// Смещение
        /// </param>
        internal abstract void Paint(Graphics g, PointF shift);

        /// <summary>
        /// Цвет границы
        /// </summary>
        internal Color BorderColor { get { return borderColor; } set { borderColor = value; } }
        /// <summary>
        /// Цвет фона
        /// </summary>
        internal Color BackColor { get { return backColor; } set { backColor = value; } }
        /// <summary>
        /// Выделенный цвет
        /// </summary>
        internal Color SelectedColor { get { return selectedColor; } set { selectedColor = value; } }
        /// <summary>
        /// Выделение
        /// </summary>
        public bool Select { get; set; }

        /// <summary>
        /// Цвет границы
        /// </summary>
        private Color borderColor = Color.Gray;
        /// <summary>
        /// Цвет фона
        /// </summary>
        private Color backColor = Color.Silver;
        /// <summary>
        /// Цвет выделенного примитива
        /// </summary>
        private Color selectedColor = Color.Green;
    }
}

//==============================================================