using System;
using System.Drawing;

using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    public class KeyCalculation : ElementCalculation, IFigureResult, IStateProtocol, IAdjustment
    {
        private Rectangle elementRectangle;
        private int stposX, stposY, posX, posY;

        public KeyCalculation( )
        {
            Records.Add( new SignalMatchRecord( "On" ) );
            Records.Add( new SignalMatchRecord( "Off" ) );

            Records.Add( new DataRecord( "StateProtocol", DataRecord.RecordTypes.StateProtocol ) { Value = ProtocolStatus.Bad } );
            Records.Add( new DataRecord( "IsAdjustment", DataRecord.RecordTypes.Boolean ) { Value = false } );
            Records.Add( new DataRecord( "IsSingleSignal", DataRecord.RecordTypes.Boolean ) { Value = false } );
            Records.Add( new DataRecord( "SetSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Red } );
            Records.Add( new DataRecord( "NoSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Green } );
            Records.Add( new DataRecord( "UnknownSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Yellow } );
            Records.Add( new DataRecord( "LostSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.DarkGray } );

            Records.Add( new DataRecord( "Rotate", DataRecord.RecordTypes.Rotate) { Value = DrawRotate.Up } );
        }
        /// <summary>
        /// Отрисовка элемента расчетных данных
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        public void DrawElement( Graphics graphics, Rectangle rectangle )
        {
            this.elementRectangle = rectangle;

            // Костыль! Стало нужно все зарисовывать, так как в связи с последними изменениями - при изменении значения тега
            // теперь перерисовывается не вся мнемосхема, а только один элемент. В связи с чем для данного элемента могут
            // оставаться артефакты при смене состояния
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(192, 192, 192)), this.elementRectangle);

            // Отрисовка элемента расчетных данных
            switch ( (DrawRotate)GetRecord( "Rotate" ).Value )
            {
                case DrawRotate.Up: this.DrawUp( graphics ); break;
                case DrawRotate.Down: this.DrawDown( graphics ); break;
                case DrawRotate.Left: this.DrawLeft( graphics ); break;
                case DrawRotate.Right: this.DrawRight( graphics ); break;
            }
        }
        public override string ToString( ) { return "KeyCalculation"; }

        /// <summary>
        /// Получить результат расчетов
        /// </summary>
        /// <returns>Результат расчетов</returns>
        private Color GetResult( )
        {
            if ( StateProtocol == ProtocolStatus.Bad )
                return (Color)GetRecord( "LostSignalColor" ).Value;

            if ( Convert.ToBoolean( GetRecord( "IsSingleSignal" ).Value ) )
                return ( !GetRecord( "On" ).Value.Equals( 0 ) )
                           ? (Color)GetRecord( "SetSignalColor" ).Value
                           : (Color)GetRecord( "NoSignalColor" ).Value;

            if ( !GetRecord( "On" ).Value.Equals( 0 ) && GetRecord( "Off" ).Value.Equals( 0 ) )
                return (Color)GetRecord( "SetSignalColor" ).Value;
            if ( GetRecord( "On" ).Value.Equals( 0 ) && !GetRecord( "Off" ).Value.Equals( 0 ) )
                return (Color)GetRecord( "NoSignalColor" ).Value;

            return (Color)GetRecord( "UnknownSignalColor" ).Value;                
        }
        /// <summary>
        /// Отрисовка положения вверх
        /// </summary>      
        private void DrawUp( Graphics graphics )
        {
            var result = this.GetResult( );
            this.DrawVerticalConnectors( graphics, result );

            if ( StateProtocol == ProtocolStatus.Bad )
            {
                this.DrawHorisontalSignalConnector( graphics, result );
                return;
            }

            if ( Convert.ToBoolean( GetRecord( "IsSingleSignal" ).Value ) )
            {
                if ( !GetRecord( "On" ).Value.Equals( 0 ) ) this.DrawVerticalSignalConnector( graphics, result );
                else this.DrawHorisontalSignalConnector( graphics, result );
            }
            else
            {
                if ( !GetRecord( "On" ).Value.Equals( 0 ) && GetRecord( "Off" ).Value.Equals( 0 ) )
                {
                    this.DrawVerticalSignalConnector( graphics, result );
                    return;
                }
                if ( GetRecord( "On" ).Value.Equals( 0 ) && !GetRecord( "Off" ).Value.Equals( 0 ) )
                    this.DrawHorisontalSignalConnector( graphics, result );
                else
                {
                    this.DrawVerticalSignalConnector( graphics, result );
                    this.DrawHorisontalSignalConnector( graphics, result );
                }
            }
        }
        /// <summary>
        /// Отрисовка положения вниз
        /// </summary>
        private void DrawDown( Graphics graphics )
        {
            //всего только два положения
            //вертикально и горизонтально
            this.DrawUp( graphics );
        }
        /// <summary>
        /// Отрисовка положения влево
        /// </summary>
        private void DrawLeft( Graphics graphics )
        {
            var result = this.GetResult( );
            this.DrawHorisontalConnectors( graphics, result );

            if ( StateProtocol == ProtocolStatus.Bad )
            {
                this.DrawVerticalSignalConnector( graphics, result );
                return;
            }

            if ( Convert.ToBoolean( GetRecord( "IsSingleSignal" ).Value ) )
            {
                if ( !GetRecord( "On" ).Value.Equals( 0 ) ) this.DrawHorisontalSignalConnector( graphics, result );
                else this.DrawVerticalSignalConnector( graphics, result );
            }
            else
            {
                if ( !GetRecord( "On" ).Value.Equals( 0 ) && GetRecord( "Off" ).Value.Equals( 0 ) )
                {
                    this.DrawHorisontalSignalConnector( graphics, result );
                    return;
                }
                if ( GetRecord( "On" ).Value.Equals( 0 ) && !GetRecord( "Off" ).Value.Equals( 0 ) ) this.DrawVerticalSignalConnector( graphics, result );
                else
                {
                    this.DrawHorisontalSignalConnector( graphics, result );
                    this.DrawVerticalSignalConnector( graphics, result );
                }
            }
        }
        /// <summary>
        /// Отрисовка положения вправо
        /// </summary>
        private void DrawRight( Graphics graphics )
        {
            //всего только два положения
            //вертикально и горизонтально
            this.DrawLeft( graphics );
        }
        /// <summary>
        /// Отрисовка состовляющей элемента в горизонтальном виде
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="color">Цвет отрисовки</param>
        private void DrawHorisontalConnectors( Graphics graphics, Color color )
        {
            //соединитель слева (-)
            this.stposX = this.elementRectangle.X;
            this.stposY = this.elementRectangle.Y + this.elementRectangle.Height / 2;
            this.posX = this.elementRectangle.X + this.elementRectangle.Width / 4;
            this.posY = this.stposY;

            var pn = new Pen( Color.Black );
            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );

            //соединитель справа (-)
            this.stposX = this.elementRectangle.Right - this.elementRectangle.Width / 4;
            this.posX = this.elementRectangle.Right;

            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );
            pn.Dispose();

            //соединитель ключа слева (|)
            this.stposX = this.elementRectangle.X + this.elementRectangle.Width / 4;
            this.stposY = this.elementRectangle.Y + this.elementRectangle.Height / 4;
            this.posX = this.stposX;
            this.posY = this.elementRectangle.Bottom - this.elementRectangle.Height / 4;

            pn = new Pen( color, 2 );
            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );

            //соединитель ключа справа (|)
            this.stposX = this.elementRectangle.Right - this.elementRectangle.Width / 4;
            this.posX = this.stposX;

            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );
            pn.Dispose();
        }
        /// <summary>
        /// Отрисовка состовляющей элемента в вертикальном виде
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="color">Цвет отрисовки</param>      
        private void DrawVerticalConnectors( Graphics graphics, Color color )
        {
            //соединитель сверху (|)
            this.stposX = this.elementRectangle.X + this.elementRectangle.Width / 2;
            this.stposY = this.elementRectangle.Y;
            this.posX = this.stposX;
            this.posY = this.elementRectangle.Y + this.elementRectangle.Height / 4;

            var pn = new Pen( Color.Black );
            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );

            //соединитель снизу (|)
            this.stposY = this.elementRectangle.Bottom - this.elementRectangle.Height / 4;
            this.posY = this.elementRectangle.Bottom;

            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );
            pn.Dispose();

            //соединитель ключа сверху (-)
            this.stposX = this.elementRectangle.X + this.elementRectangle.Width / 4;
            this.stposY = this.elementRectangle.Y + this.elementRectangle.Height / 4;
            this.posX = this.elementRectangle.Right - this.elementRectangle.Width / 4;
            this.posY = this.stposY;

            pn = new Pen( color, 2 );
            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );

            //соединитель ключа снизу (-)
            this.stposY = this.elementRectangle.Bottom - this.elementRectangle.Height / 4;
            this.posY = this.stposY;

            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );
            pn.Dispose();
        }
        /// <summary>
        /// Отрисовка положения ключа в горизонтальном виде
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="color">Цвет отрисовки</param>
        private void DrawHorisontalSignalConnector( Graphics graphics, Color color )
        {
            this.stposX = this.elementRectangle.X + this.elementRectangle.Width / 4 + this.elementRectangle.Width / 8;
            this.stposY = this.elementRectangle.Y + this.elementRectangle.Height / 2;
            this.posX = this.elementRectangle.Right - this.elementRectangle.Width / 4 - this.elementRectangle.Width / 8;
            this.posY = this.stposY;

            var pn = new Pen( color, 2 );
            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );
            pn.Dispose();
        }
        /// <summary>
        /// Отрисовка положения ключа в вертикальном виде
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="color">Цвет отрисовки</param>      
        private void DrawVerticalSignalConnector( Graphics graphics, Color color )
        {
            this.stposX = this.elementRectangle.X + this.elementRectangle.Width / 2;
            this.stposY = this.elementRectangle.Y + this.elementRectangle.Height / 4 + this.elementRectangle.Height / 8;
            this.posX = this.stposX;
            this.posY = this.elementRectangle.Bottom - this.elementRectangle.Height / 4 - this.elementRectangle.Height / 8;

            var pn = new Pen( color, 2 );
            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );
            pn.Dispose();
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