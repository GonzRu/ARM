using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    public class DiscretPlata2signalsCalculation : ElementCalculation, IFigureResult, IStateProtocol, IAdjustment
    {
        private const int RIGHT_PADDING = 10;
        private const int DIVIDE_LINE_PADDING = 10;

        private readonly Font TitleFont;
        private readonly Font TextFont;
        private readonly StringFormat format;

        public DiscretPlata2signalsCalculation()
        {
            this.format = new StringFormat(StringFormatFlags.NoClip)
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            this.TitleFont = new Font("Tahoma", 10, FontStyle.Bold);
            this.TextFont = new Font("Tahoma", 10, FontStyle.Italic);

            Records.Add(new SignalMatchRecord("StateSignal"));
            Records.Add(new SignalMatchRecord("Input1"));
            Records.Add(new SignalMatchRecord("Input2"));

            Records.Add(new DataRecord("StateProtocol", DataRecord.RecordTypes.StateProtocol)
            {
                Value = ProtocolStatus.Bad
            });
            Records.Add(new DataRecord("IsAdjustment", DataRecord.RecordTypes.Boolean) {Value = false});
            Records.Add(new DataRecord("SetSignalColor", DataRecord.RecordTypes.Color) {Value = Color.Red});
            Records.Add(new DataRecord("NoSignalColor", DataRecord.RecordTypes.Color) {Value = Color.Green});
            Records.Add(new DataRecord("UnknownSignalColor", DataRecord.RecordTypes.Color) {Value = Color.Yellow});
            Records.Add(new DataRecord("LostSignalColor", DataRecord.RecordTypes.Color) {Value = Color.DarkGray});
            Records.Add(new DataRecord("BackgroundColor", DataRecord.RecordTypes.Color) {Value = Color.LightGray});
            Records.Add(new DataRecord("TitleTextColor", DataRecord.RecordTypes.Color) {Value = Color.Black});
            Records.Add(new DataRecord("TextColor", DataRecord.RecordTypes.Color) { Value = Color.Black });

            Records.Add(new DataRecord("DivideLineColor", DataRecord.RecordTypes.Color) { Value = Color.Black });

            Records.Add(new DataRecord("TitleText", DataRecord.RecordTypes.Text) {Value = "Unknown"});
            Records.Add(new DataRecord("Text_0-0", DataRecord.RecordTypes.Text) { Value = "Unknown" });
            Records.Add(new DataRecord("Text_0-1", DataRecord.RecordTypes.Text) { Value = "Unknown" });
            Records.Add(new DataRecord("Text_1-0", DataRecord.RecordTypes.Text) { Value = "Unknown" });
            Records.Add(new DataRecord("Text_1-1", DataRecord.RecordTypes.Text) { Value = "Unknown" });
        }

        /// <summary>
        /// Отрисовка элемента расчетных данных
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        public void DrawElement(Graphics graphics, Rectangle rectangle)
        {
            this.DrawBody(graphics, rectangle);
            this.DrawStateIndicator(graphics, rectangle);
            this.DrawSignalText(graphics, rectangle);
        }

        public override string ToString()
        {
            return "DiscretPlataInput2signals";
        }

        /// <summary>
        /// Получить результат расчетов
        /// </summary>
        private Color GetResult()
        {
            if (StateProtocol == ProtocolStatus.Bad)
                return (Color) GetRecord("LostSignalColor").Value;

            return (!GetRecord("StateSignal").Value.Equals(0))
                    ? (Color) GetRecord("SetSignalColor").Value
                    : (Color) GetRecord("NoSignalColor").Value;
        }

        private Rectangle GetSignalCorrectText(Rectangle rectangle)
        {
            //Text
            return new Rectangle
            {
                Location = rectangle.Location,
                Size = new Size(rectangle.Width - RIGHT_PADDING, rectangle.Height)
            };
        }

        private void DrawBody(Graphics graphics, Rectangle rectangle)
        {
            //фон
            var sb = new SolidBrush((Color) GetRecord("BackgroundColor").Value);
            graphics.FillRectangle(sb, rectangle);

            //горизонтальная линия, разграничивающая заголовок от значений
            Pen pen = new Pen((Color) GetRecord("DivideLineColor").Value);
            graphics.DrawLine(pen,
                rectangle.Left + DIVIDE_LINE_PADDING, 
                rectangle.Top + rectangle.Height/2,
                rectangle.Right - RIGHT_PADDING - DIVIDE_LINE_PADDING,
                rectangle.Top + rectangle.Height/2);

            //Текст
            sb = new SolidBrush((Color) GetRecord("TitleTextColor").Value);
            Rectangle newRec = new Rectangle(rectangle.Location, new Size(rectangle.Width, rectangle.Height/2));
            graphics.DrawString(GetRecord("TitleText").Value.ToString(), this.TitleFont, sb,
                this.GetSignalCorrectText(newRec), this.format);

            //оконтовка
            var pn = new Pen(Color.Black);
            graphics.DrawRectangle(pn, rectangle);

            pn.Dispose();
            sb.Dispose();
        }

        private void DrawStateIndicator(Graphics graphics, Rectangle rectangle)
        {
            var rec = new Rectangle
            {
                Location =
                    new Point(rectangle.Right - 15, rectangle.Y + rectangle.Height/2 - 5),
                Size = new Size(10, 10)
            };

            //Signal color
            var sb = new SolidBrush(this.GetResult());
            //оконтовка
            var pn = new Pen(Color.Black);

            //Draw signal
            graphics.FillEllipse(sb, rec);
            //Draw signal (оконтовка)
            graphics.DrawEllipse(pn, rec);

            pn.Dispose();
            sb.Dispose();
        }

        private void DrawSignalText(Graphics graphics, Rectangle rectangle)
        {
            var rec = new Rectangle()
            {
                Location = new Point(rectangle.Left, rectangle.Top + rectangle.Height/2),
                Size = new Size(rectangle.Width, rectangle.Height/2)
            };

            string text;
            if (StateProtocol == ProtocolStatus.Bad)
                text = "....";
            else
            if (GetRecord("Input1").Value.Equals(0))
                if (GetRecord("Input2").Value.Equals(0))
                    text = GetRecord("Text_0-0").Value.ToString();
                else
                    text = GetRecord("Text_0-1").Value.ToString();
            else
                if (GetRecord("Input2").Value.Equals(0))
                    text = GetRecord("Text_1-0").Value.ToString();
                else
                    text = GetRecord("Text_1-1").Value.ToString();

            var brush = new SolidBrush((Color) GetRecord("TextColor").Value);

            graphics.DrawString(text, TextFont, brush, rec, format);
        }

        /// <summary>
        /// Состояние протокола
        /// </summary>
        public ProtocolStatus StateProtocol
        {
            get { return (ProtocolStatus) GetRecord("StateProtocol").Value; }
            set { GetRecord("StateProtocol").Value = value; }
        }

        /// <summary>
        /// Корректировка тэгов
        /// </summary>
        public Boolean IsAdjustment
        {
            get { return Convert.ToBoolean(GetRecord("IsAdjustment").Value); }
            set { GetRecord("IsAdjustment").Value = value; }
        }

        /// <summary>
        /// Block text
        /// </summary>
        public string Text
        {
            get { return GetRecord("TitleText").Value.ToString(); }
            set { GetRecord("TitleText").Value = value; }
        }
    }
}
