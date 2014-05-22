using System;
using System.Drawing;
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
            Records.Add(new DataRecord("Исправен", DataRecord.RecordTypes.Color) {Value = Color.Green});
            Records.Add(new DataRecord("Срабатывание", DataRecord.RecordTypes.Color) {Value = Color.Red});
            Records.Add(new DataRecord("Не исправен", DataRecord.RecordTypes.Color) {Value = Color.Yellow});
            Records.Add(new DataRecord("Обрыв ВОД", DataRecord.RecordTypes.Color) {Value = Color.Yellow});
            Records.Add(new DataRecord("Отключен", DataRecord.RecordTypes.Color) { Value = Color.Gray });
            Records.Add(new DataRecord("Неизвестное состояние", DataRecord.RecordTypes.Color) { Value = Color.DarkGray });
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
            Point leftUpPoint = new Point(rectangle.Left, rectangle.Top);
            Point leftDownPoint = new Point(rectangle.Left, rectangle.Bottom);
            Point rightUpPoint = new Point(rectangle.Right, rectangle.Top);
            Point rightDownPoint = new Point(rectangle.Right, rectangle.Bottom);

            Pen pen = new Pen(Color.Black);

            graphics.DrawLine(pen, leftUpPoint, rightDownPoint);
            graphics.DrawLine(pen, leftDownPoint, rightUpPoint);
             */

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

            // Координаты центра в исходной системе координат
            int Cx = rectangle.Left + rectangle.Width/2;
            int Cy = rectangle.Top + rectangle.Height/2;

            int a = rectangle.Width/2; // большая полуось
            int b = rectangle.Height/2; // малая полуось

            // Угол, между осью абсцисс и радиус-вектором от центра координат до угла прямоугольника
            double phi = Math.Atan2(b, a);

            // Длина радиус-вектора от центра координат до точки пересечения эллипса с с прямой, проходящей через центр координат и угол прямоугольника
            double r = b/(Math.Sqrt(1 - Math.Pow(Math.E*Math.Cos(phi), 2)));

            // Находим координаты точки пересечения в новой системе координат
            double Ax = r*Math.Sin(phi);
            double Ay = r*Math.Cos(phi);

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
                case 0: return (Color)GetRecord("Исправен").Value;
                case 1: return (Color)GetRecord("Срабатывание").Value;
                case 2: return (Color)GetRecord("Не исправен").Value;
                case 6: return (Color)GetRecord("Обрыв ВОД").Value;
                case 8: return (Color)GetRecord("Отключен").Value;
                default: return (Color)GetRecord("Неизвестное состояние").Value;
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
