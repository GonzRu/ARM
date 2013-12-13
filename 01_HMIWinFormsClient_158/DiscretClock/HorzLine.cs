////////////////////////////////////////////////////////////////
//  
////////////////////////////////////////////////////////////////
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

//==============================================================
namespace Egida
{
    /// <summary>
    /// Вертикальная линия
    /// </summary>
    internal class HorzLine : Primitive
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
        private readonly PointF[] points = { new PointF(10f, 0f),
                                             new PointF(50f, 0f),
                                             new PointF(60f, 10f),
                                             new PointF(50f, 20f),
                                             new PointF(10f, 20f),
                                             new PointF(0f, 10f) };
        #endregion Константы
    }
}

//==============================================================