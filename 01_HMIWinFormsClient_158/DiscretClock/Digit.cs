////////////////////////////////////////////////////////////////
//  Цифровые часы
//  Цифра
////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

//==============================================================
namespace Egida
{
    /// <summary>
    /// Цифра
    /// </summary>
    internal class Digit
    {
        #region Функции
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        internal Digit()
        {
            valuePrimitives.Add(0, new List<int> { 0, 1, 2, 4, 5, 6, 7, 8, 9 });
            valuePrimitives.Add(1, new List<int> { 2, 5 });
            valuePrimitives.Add(2, new List<int> { 0, 2, 3, 4, 6 });
            valuePrimitives.Add(3, new List<int> { 0, 2, 3, 5, 6 });
            valuePrimitives.Add(4, new List<int> { 1, 2, 3, 5 });
            valuePrimitives.Add(5, new List<int> { 0, 1, 3, 5, 6 });
            valuePrimitives.Add(6, new List<int> { 0, 1, 3, 4, 5, 6 });
            valuePrimitives.Add(7, new List<int> { 0, 2, 5 });
            valuePrimitives.Add(8, new List<int> { 0, 1, 2, 3, 4, 5, 6 });
            valuePrimitives.Add(9, new List<int> { 0, 1, 2, 3, 5, 6 });
        }

        /// <summary>
        /// Вывод цифры
        /// </summary>
        /// <param name="g">
        /// Объект для вывода графики
        /// </param>
        /// <param name="shift">
        /// Смещения
        /// </param>
        internal void Paint(Graphics g, PointF shift)
        {
            Debug.Assert(g != null, "g != null");

            List<int> selectPrimitives = valuePrimitives[Value];
            for (int i = 0; i < primitives.Length; ++i)
            {
                PointF primitiveShift = primitiveShifts[i];
                var realShift = new PointF(shift.X + primitiveShift.X,
                                           shift.Y + primitiveShift.Y);

                Primitive primitive = primitives[i];
                primitive.Select = selectPrimitives.Contains(i);
                primitive.Paint(g, realShift);

            }
        }
        #endregion Функции

        #region Свойства
        /// <summary>
        /// Значение
        /// </summary>
        public int Value
        {
            get 
            { 
                return _value;
            }
            set 
            {
                Debug.Assert((value >= 0) && (value < 10));

                _value = value;
            }
        }
        #endregion Свойства

        #region Поля
        /// <summary>
        /// Значение
        /// </summary>
        private int _value = 0;
        /// <summary>
        /// Соотношения значений и выбранных примитивов
        /// </summary>
        private Dictionary<int, List<int>> valuePrimitives = new Dictionary<int, List<int>>();
        #endregion Поля

        #region Константы
        /// <summary>
        /// Примитивы
        /// </summary>
        private readonly Primitive[] primitives = { new HorzLine(), 
                                                    new VertLine(),
                                                    new VertLine(),
                                                    new HorzLine(),
                                                    new VertLine(),
                                                    new VertLine(),
                                                    new HorzLine() };
        /// <summary>
        /// Смещения
        /// </summary>
        private readonly PointF[] primitiveShifts = { new PointF(20f, 0f),
                                                      new PointF(0f, 10f),
                                                      new PointF(80f, 10f),
                                                      new PointF(20f, 65f),
                                                      new PointF(0f, 80f),
                                                      new PointF(80f, 80f),
                                                      new PointF(20f, 130f) };
        #endregion Константы
    }
}

//==============================================================
