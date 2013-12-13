////////////////////////////////////////////////////////////////
//  Цифровые часы
////////////////////////////////////////////////////////////////
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

//==============================================================
namespace Egida
{
    /// <summary>
    /// Цифровые часы
    /// </summary>
    public partial class DiscretClock : UserControl
    {
        /// <summary>
        /// Цифры
        /// </summary>
        private readonly Digit[] digits = { new Digit(), 
                                            new Digit(),
                                            new Digit(),
                                            new Digit(),
                                            new Digit(),
                                            new Digit() };
        /// <summary>
        /// Смещение
        /// </summary>
        private readonly PointF[] digitShifts = { new PointF(0f, 0f),
                                                  new PointF(110f, 0f),
                                                  new PointF(250, 0f),
                                                  new PointF(360f, 0f),
                                                  new PointF(500f, 0f),
                                                  new PointF(610f, 0f) };
        /// <summary>
        /// Точки
        /// </summary>
        private readonly Point[] points = { new Point(),
                                            new Point(),
                                            new Point(),
                                            new Point() };
        /// <summary>
        /// Смещения точек
        /// </summary>
        private readonly PointF[] pointShifts = { new PointF(220f, 40f),
                                                  new PointF(220f, 80f),
                                                  new PointF(470f, 40f),
                                                  new PointF(470f, 80f) };

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public DiscretClock()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Функция вызывается при перерисовке формы
        /// </summary>
        /// <param name="sender">
        /// Объект, из которого вызывается функция
        /// </param>
        /// <param name="e">
        /// Параметры вызова
        /// </param>
        private void DiscretClock_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            //  Выставление масштабирования
            var scaleX = Width / 720f;
            var scaleY = Height / 150f;
            g.ScaleTransform(scaleX, scaleY);

            //  Задание часов
            digits[0].Value = DateTime.Now.Hour / 10;
            digits[1].Value = DateTime.Now.Hour % 10;

            //  Задание минут
            digits[2].Value = DateTime.Now.Minute / 10;
            digits[3].Value = DateTime.Now.Minute % 10;

            //  Задание секунд
            digits[4].Value = DateTime.Now.Second / 10;
            digits[5].Value = DateTime.Now.Second % 10;

            //  Вывод точек
            for (var i = 0; i < points.Length; ++i)
            {
                var point = points[i];
                var pointShift = pointShifts[i];

                point.Paint(g, pointShift);
            }

            //  Вывод цифр
            for (var i = 0; i < digits.Length; ++i)
            {
                var digit = digits[i];
                var digitShift = digitShifts[i];

                digit.Paint(g, digitShift);
            }
        }
        /// <summary>
        /// Функция вызывается при изменении размера формы
        /// </summary>
        /// <param name="sender">
        /// Объект, из которого вызывается функция
        /// </param>
        /// <param name="e">
        /// Параметры вывода
        /// </param>
        private void DiscretClock_Resize(object sender, EventArgs e)
        {
            Refresh();
        }
        /// <summary>
        /// Функция вызывается при срабатывании таймера
        /// </summary>
        /// <param name="sender">
        /// Объект, из которого вызывается функция
        /// </param>
        /// <param name="e">
        /// Параметры вызова
        /// </param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            //  Мигание точек
            foreach (var point in points)
                point.Select = !point.Select;

            Refresh();
        }
    }
}

//==============================================================