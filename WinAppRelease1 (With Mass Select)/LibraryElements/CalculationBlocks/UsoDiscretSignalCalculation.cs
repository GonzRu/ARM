using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    public class UsoDiscretSignalCalculation: ElementCalculation, IFigureResult, IStateProtocol, IAdjustment
    {
        public UsoDiscretSignalCalculation()
        {
            Records.Add(new SignalMatchRecord("Срабатывание"));
            Records.Add(new SignalMatchRecord("Неисправность"));
            Records.Add(new SignalMatchRecord("Обрыв ВОК"));
            Records.Add(new SignalMatchRecord("Вкл./Выкл."));

            Records.Add(new DataRecord("StateProtocol", DataRecord.RecordTypes.StateProtocol) { Value = ProtocolStatus.Bad });
            Records.Add(new DataRecord("IsAdjustment", DataRecord.RecordTypes.Boolean) {Value = false});
            Records.Add(new DataRecord("Цвет состояния \"Исправен\"", DataRecord.RecordTypes.Color) {Value = Color.Green});
            Records.Add(new DataRecord("Цвет состояния \"Срабатывание\"", DataRecord.RecordTypes.Color) { Value = Color.Red });
            Records.Add(new DataRecord("Цвет состояния \"Не исправен\"", DataRecord.RecordTypes.Color) { Value = Color.Yellow });
            Records.Add(new DataRecord("Цвет состояния \"Обрыв ВОД\"", DataRecord.RecordTypes.Color) { Value = Color.Yellow });
            Records.Add(new DataRecord("Цвет состояния \"Отключен\"", DataRecord.RecordTypes.Color) { Value = Color.Gray });
            Records.Add(new DataRecord("Цвет неизвестного состояния", DataRecord.RecordTypes.Color) { Value = Color.DarkGray });
            Records.Add(new DataRecord("Нет связи", DataRecord.RecordTypes.Color) { Value = Color.Gainsboro });
        }

        /// <summary>
        /// Отрисовка элемента расчетных данных
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        public void DrawElement( Graphics graphics, Rectangle rectangle )
        {
            var analogValueResult = GetAnalogResultValue();

            //Signal color
            var sb = new SolidBrush(this.GetResult(analogValueResult));
            //оконтовка
            var pn = new Pen( Color.Black );          

            //Draw signal
            graphics.FillEllipse( sb, rectangle );
            //Draw signal (оконтовка)
            graphics.DrawEllipse( pn, rectangle );

            if (analogValueResult == 6)
            {
                DrawBreakLines(graphics, rectangle);
            }

            pn.Dispose( );
            sb.Dispose( );
        }

        private void DrawBreakLines(Graphics graphics, Rectangle rectangle)
        {
            /*
             * Данный способ перечеркивает весь прямоугольник, а не эллипс
             */
            //Point leftPoint = new Point(rectangle.Left, rectangle.Top + rectangle.Height/2);
            //Point upPoint = new Point(rectangle.Left + rectangle.Width/2, rectangle.Top);
            //Point rightPoint = new Point(rectangle.Right, rectangle.Top + rectangle.Height/2);
            //Point downPoint = new Point(rectangle.Left + rectangle.Width/2, rectangle.Bottom);

            //Pen pen = new Pen(Color.Black);

            //graphics.DrawLine(pen, leftPoint, rightPoint);
            //graphics.DrawLine(pen, upPoint, downPoint);


            /*
             * Данный способо перечеркивает эллипс, а не прямоугольник.
             * Для этого делается следующее:
             *      1. Система координат в центре прямоугльника.
             *      2. Следовательно, уравнение эллипса - x*x/a + y*y/b = 1, где a и b - большая и малая полуось соответственно.
             *      3. Ищется угол (phi) между осью абсцисс и прямой, проходящей через начало координат и углом прямоугольника.
             *      4. Ищется длина радиус-вектора от начала координат до точки пересечения эллипса с прямой, проходящей через
             *          начало координат и угол прямоугольника.
             *      5. Находятся координаты точки пересечения
             *      6. Находим координаты всех точек пересечения в исходной системе координат
             */

            if (rectangle.Width != 15 && rectangle.Height != 15)
            {

            }

            // Координаты центра в исходной системе координат
            int Cx = rectangle.Left + rectangle.Width / 2;
            int Cy = rectangle.Top + rectangle.Height / 2;

            double a = rectangle.Width / 2.0; // большая полуось
            double b = rectangle.Height / 2.0; // малая полуось

            // Угол, между осью абсцисс и радиус-вектором от центра координат до угла прямоугольника
            double phi = Math.Atan(b / a);

            // Длина радиус-вектора от центра координат до точки пересечения эллипса с с прямой, проходящей через центр координат и угол прямоугольника
            double a1 = Math.Pow(b * Math.Cos(phi), 2);
            double a2 = Math.Pow(a * Math.Sin(phi), 2);
            double r = a * b / (Math.Sqrt(a1 + a2));

            // Находим координаты точки пересечения в новой системе координат
            double Ay = r * Math.Sin(phi);
            double Ax = r * Math.Cos(phi);

            // Находим координаты точек пересечения в исходной системе координат
            Point leftUpPoint = new Point((int)(Cx - Ax), (int)(Cy - Ay));
            Point leftDownPoint = new Point((int)(Cx - Ax), (int)(Cy + Ay));
            Point rightUpPoint = new Point((int)(Cx + Ax), (int)(Cy - Ay));
            Point rightDownPoint = new Point((int)(Cx + Ax), (int)(Cy + Ay));

            Pen pen = new Pen(Color.Black);

            graphics.DrawLine(pen, leftUpPoint, rightDownPoint);
            graphics.DrawLine(pen, leftDownPoint, rightUpPoint);
        }        

        public override string ToString( ) { return "UsoDiscretSignalCalculation"; }

        private int GetAnalogResultValue()
        {
            // Дискрет "Срабатывание"
            int discretSrabat = GetRecord("Срабатывание").Value.Equals(0) ? 0 : 1;
            // Дискрет "Неисправность"
            int discretFault = GetRecord("Неисправность").Value.Equals(0) ? 0 : 1;
            // Дискрет "Обрыв ВОК"
            int discretBreakVod = GetRecord("Обрыв ВОК").Value.Equals(0) ? 0 : 1;
            // Дискрет "Вкл./Выкл."
            int discretOnOff = GetRecord("Вкл./Выкл.").Value.Equals(0) ? 0 : 1;

            return discretSrabat + 2 * discretFault + 4 * discretBreakVod + 8 * discretOnOff;
        }

        /// <summary>
        /// Получить результат расчетов
        /// </summary>
        /// <returns>Результат расчетов</returns>
        private Color GetResult(int analogValueResult)
        {
            if (StateProtocol == ProtocolStatus.Bad) return (Color)GetRecord("Нет связи").Value;

            // устройство ОВОД - привязывается одна целочисленная переменная со значениями
            //00h - "исправен" - зеленый
            //01h - "сработал" - красный
            //02h - "не исправен" - желтый
            //06h - "обрыв ВОД" - желтый с перекрестием
            //08h - "отключен" - серый
            switch (analogValueResult)
            {
                case 0: return (Color)GetRecord("Цвет состояния \"Исправен\"").Value;
                case 1: return (Color)GetRecord("Цвет состояния \"Срабатывание\"").Value;
                case 2: return (Color)GetRecord("Цвет состояния \"Не исправен\"").Value;
                case 6: return (Color)GetRecord("Цвет состояния \"Обрыв ВОД\"").Value;
                case 8: return (Color)GetRecord("Цвет состояния \"Отключен\"").Value;
                default: return (Color)GetRecord("Цвет неизвестного состояния").Value;
            }            
        }

        /// <summary>
        /// Состояние протокола
        /// </summary>
        public ProtocolStatus StateProtocol { get { return (ProtocolStatus)GetRecord( "StateProtocol" ).Value; } set { GetRecord( "StateProtocol" ).Value = value; } }
        /// <summary>
        /// Корректировка тэгов
        /// </summary>
        public Boolean IsAdjustment { get { return Convert.ToBoolean( GetRecord( "IsAdjustment" ).Value ); } set { GetRecord( "IsAdjustment" ).Value = value; } }
    }
}
