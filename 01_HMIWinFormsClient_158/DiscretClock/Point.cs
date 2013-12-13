////////////////////////////////////////////////////////////////
//  Цифровые часы
//  Точка
////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

//==============================================================
namespace Egida
{
    /// <summary>
    /// Вертикальная линия
    /// </summary>
    internal class Point : Primitive
    {
        #region Функции
        /// <summary>
        /// Вывод вертикальной линии
        /// </summary>
        /// <param name="g">
        /// Объект для вывода графики
        /// </param>
        /// <param name="shift">
        /// Смещение
        /// </param>
        internal override void Paint(Graphics g, PointF shift)
        {
            Debug.Assert(g != null, "g != null");

            PointF[] realPoints = points.Select(n => new PointF(n.X + shift.X, n.Y + shift.Y)).ToArray();

            if (Select)
            {
                var brush = new SolidBrush(SelectedColor);
                g.FillPolygon(brush, realPoints);
            }
            else
            {
                var brush = new SolidBrush(BackColor);
                g.FillPolygon(brush, realPoints);
            }

            var pen = new Pen(BorderColor);
            g.DrawPolygon(pen, realPoints);
        }
        #endregion Функции

        #region Константы
        /// <summary>
        /// Точки
        /// </summary>
        private readonly PointF[] points = { new PointF(0f, 0f),
                                             new PointF(20f, 0f),
                                             new PointF(20f, 20f),
                                             new PointF(0f, 20f) };
        #endregion Константы
    }
}

//==============================================================