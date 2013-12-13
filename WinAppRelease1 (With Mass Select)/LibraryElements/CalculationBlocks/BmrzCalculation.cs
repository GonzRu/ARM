using System;
using System.Drawing;
using System.Globalization;

using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    public class BmrzCalculation : ElementCalculation, IFigureResult, IStateProtocol, IAdjustment
    {
        public BmrzCalculation( )
        {
            Records.Add( new SignalMatchRecord( "RPV" ) );
            Records.Add( new SignalMatchRecord( "RPO" ) );
            Records.Add( new SignalMatchRecord( "Call" ) );
            Records.Add( new SignalMatchRecord( "Local" ) );

            Records.Add( new DataRecord( "StateProtocol", DataRecord.RecordTypes.StateProtocol ) { Value = ProtocolStatus.Bad } );
            Records.Add( new DataRecord( "IsAdjustment", DataRecord.RecordTypes.Boolean ) { Value = false } );
            Records.Add( new DataRecord( "CallColor", DataRecord.RecordTypes.Color ) { Value = Color.Aqua } );
            Records.Add( new DataRecord( "LocalColor", DataRecord.RecordTypes.Color ) { Value = Color.White } );
            Records.Add( new DataRecord( "SetSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Red } );
            Records.Add( new DataRecord( "NoSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Green } );
            Records.Add( new DataRecord( "UndefineBodySignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Yellow } );
            Records.Add( new DataRecord( "UndefineSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Black } );
            Records.Add( new DataRecord( "NoConnectBodySignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Gray } );
            Records.Add( new DataRecord( "NoConnectSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Red } );

            Records.Add( new DataRecord( "DefaultImage", DataRecord.RecordTypes.Image ) );
            Records.Add( new DataRecord( "Rotate", DataRecord.RecordTypes.Rotate ) { Value = DrawRotate.Up } );
        }
        /// <summary>
        /// Отрисовка элемента расчетных данных
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        public void DrawElement( Graphics graphics, Rectangle rectangle )
        {
            DrawRectangleBody( graphics, rectangle, Brushes.DarkGray );
            
            if ( StateProtocol == ProtocolStatus.Bad )
            {
                DrawRectangleBody( graphics, rectangle,
                                   new SolidBrush( (Color)GetRecord( "NoConnectBodySignalColor" ).Value ) );
                DrawSimbolConnection( graphics, rectangle,
                                      new SolidBrush( (Color)GetRecord( "NoConnectSignalColor" ).Value ), '!' );
                return;
            }

            var newRectangle = rectangle;
            if ( !GetRecord( "Call" ).Value.Equals( 0 ) )
                graphics.FillRectangle( new SolidBrush( (Color)GetRecord( "CallColor" ).Value ), newRectangle );
            if ( !GetRecord( "Local" ).Value.Equals( 0 ) )
                graphics.FillRectangle( new SolidBrush( (Color)GetRecord( "LocalColor" ).Value ), newRectangle );
            if ( !GetRecord( "Call" ).Value.Equals( 0 ) || !GetRecord( "Local" ).Value.Equals( 0 ) )
                newRectangle = new Rectangle( newRectangle.X + 3, newRectangle.Y + 3,
                                              newRectangle.Width - 6, newRectangle.Height - 6 );
            
            var defaultImageRecord = GetRecord( "DefaultImage" );
            if ( defaultImageRecord.Value != null )
            {
                DrawDefaultSignal( graphics, newRectangle, (ImageData)defaultImageRecord.Value );
                return;
            }

            if ( !GetRecord( "RPV" ).Value.Equals( 0 ) && GetRecord( "RPO" ).Value.Equals( 0 ) )
            {
                DrawRectangleBody( graphics, newRectangle, new SolidBrush( (Color)GetRecord( "SetSignalColor" ).Value ) );
                DrawOnSignal( graphics, newRectangle, (DrawRotate)GetRecord( "Rotate" ).Value );
            }
            if ( GetRecord( "RPV" ).Value.Equals( 0 ) && !GetRecord( "RPO" ).Value.Equals( 0 ) )
            {
                DrawRectangleBody( graphics, newRectangle, new SolidBrush( (Color)GetRecord( "NoSignalColor" ).Value ) );
                DrawOffSignal( graphics, newRectangle, (DrawRotate)GetRecord( "Rotate" ).Value );
            }

            if ( GetRecord( "RPV" ).Value.Equals( GetRecord( "RPO" ).Value ) )
            {
                DrawRectangleBody( graphics, newRectangle,
                                   new SolidBrush( (Color)GetRecord( "UndefineBodySignalColor" ).Value ) );
                DrawSimbolConnection( graphics, newRectangle,
                                      new SolidBrush( (Color)GetRecord( "UndefineSignalColor" ).Value ), '?' );
            }
        }
        public override string ToString( ) { return "BmrzCalculation"; }

        /// <summary>
        /// Состояние протокола
        /// </summary>
        public ProtocolStatus StateProtocol { get { return (ProtocolStatus)GetRecord( "StateProtocol" ).Value; } set { GetRecord( "StateProtocol" ).Value = value; } }
        /// <summary>
        /// Корректировка тэгов
        /// </summary>
        public Boolean IsAdjustment { get { return Convert.ToBoolean( GetRecord( "IsAdjustment" ).Value ); } set { GetRecord( "IsAdjustment" ).Value = value; } }

        /// <summary>
        /// Отрисовка сигнала вкл. блока
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        /// <param name="rotate">Положение</param>
        internal static void DrawOnSignal( Graphics graphics, Rectangle rectangle, DrawRotate rotate )
        {
            using ( var pen = new Pen( Color.Black, 1f ) )
            {
                int x1 = rectangle.X + rectangle.Width / 2,
                    y1 = rectangle.Y + rectangle.Height / 15,
                    x2 = rectangle.Right - rectangle.Width / 2,
                    y2 = rectangle.Bottom - rectangle.Height / 15;

                if ( rotate == DrawRotate.Left || rotate == DrawRotate.Right )
                {
                    x1 = rectangle.X + rectangle.Width / 15;
                    y1 = rectangle.Y + rectangle.Height / 2;
                    x2 = rectangle.Right - rectangle.Width / 15;
                    y2 = rectangle.Bottom - rectangle.Height / 2;
                }
                graphics.DrawLine( pen, x1, y1, x2, y2 );
            }
        }
        /// <summary>
        /// Отрисовка сигнала выкл. блока
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        /// <param name="rotate">Положение</param>
        internal static void DrawOffSignal( Graphics graphics, Rectangle rectangle, DrawRotate rotate )
        {
            using ( var pen = new Pen( Color.Black, 1f ) )
            {
                int x1 = rectangle.X + rectangle.Width / 15,
                    y1 = rectangle.Y + rectangle.Height / 2,
                    x2 = rectangle.Right - rectangle.Width / 15,
                    y2 = rectangle.Bottom - rectangle.Height / 2;

                if ( rotate == DrawRotate.Left || rotate == DrawRotate.Right )
                {
                    x1 = rectangle.X + rectangle.Width / 2;
                    y1 = rectangle.Y + rectangle.Height / 15;
                    x2 = rectangle.Right - rectangle.Width / 2;
                    y2 = rectangle.Bottom - rectangle.Height / 15;
                }
                graphics.DrawLine( pen, x1, y1, x2, y2 );
            }
        }
        /// <summary>
        /// Отрисовка сигнала блока по умолчанию
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        /// <param name="imagePack">Упакованные данные изображения</param>
        internal static void DrawDefaultSignal( Graphics graphics, Rectangle rectangle, ImageData imagePack )
        {
            if ( imagePack == null || imagePack.Image == null )
            {
                DrawRectangleBody( graphics, rectangle, Brushes.Gray );
                DrawSimbolConnection( graphics, rectangle, Brushes.Red, '!' );
            }
            else
                graphics.DrawImage( imagePack.Image, rectangle );
        }
        /// <summary>
        /// Отрисовка символьного сигнала работы блока
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        /// <param name="simbolBrush">Цвет символа</param>
        /// <param name="simbol">Символ</param>
        internal static void DrawSimbolConnection( Graphics graphics, Rectangle rectangle, Brush simbolBrush, char simbol )
        {
            float fontSize = ( rectangle.Width <= rectangle.Height ) ? rectangle.Width / 2 : rectangle.Height / 2;

            using ( var font = new Font( "Arial Black", fontSize ) )
            {
                var textSize = graphics.MeasureString( simbol.ToString( CultureInfo.InvariantCulture ), font );
                var posX = rectangle.X + Convert.ToInt32( rectangle.Width / 2 ) -
                           Convert.ToInt32( textSize.Width / 2 );
                var posY = rectangle.Y + Convert.ToInt32( rectangle.Height / 2 ) -
                           Convert.ToInt32( textSize.Height / 2 );
                graphics.DrawString( simbol.ToString( CultureInfo.InvariantCulture ), font, simbolBrush, posX, posY );
            }
        }
        /// <summary>
        /// Отрисовка тела блока
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        /// <param name="bodyBrush">Цвет фона</param>
        internal static void DrawRectangleBody( Graphics graphics, Rectangle rectangle, Brush bodyBrush )
        {
            using ( var pen = new Pen( Color.Black, 1f ) )
            {
                graphics.FillRectangle( bodyBrush, rectangle );
                graphics.DrawRectangle( pen, rectangle );
            }
        }
    }
}