using System;
using System.Drawing;

using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    public class BlockSignalCalculation : ElementCalculation, IFigureResult, IStateProtocol, IAdjustment
    {
        private readonly Font font;
        private readonly StringFormat format;

        public BlockSignalCalculation( )
        {
            this.format = new StringFormat( StringFormatFlags.NoClip )
                         {
                             LineAlignment = StringAlignment.Center,
                             Alignment = StringAlignment.Center
                         };
            this.font = new Font( "Tahoma", 10 );

            Records.Add( new SignalMatchRecord( "On" ) );
            Records.Add( new SignalMatchRecord( "Off" ) );

            Records.Add( new DataRecord( "StateProtocol", DataRecord.RecordTypes.StateProtocol ) { Value = ProtocolStatus.Bad } );
            Records.Add( new DataRecord( "IsAdjustment", DataRecord.RecordTypes.Boolean ) { Value = false } );
            Records.Add( new DataRecord( "IsSingleSignal", DataRecord.RecordTypes.Boolean ) { Value = false } );
            Records.Add( new DataRecord( "SetSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Red } );
            Records.Add( new DataRecord( "NoSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Green } );
            Records.Add( new DataRecord( "UnknownSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Yellow } );
            Records.Add( new DataRecord( "LostSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.DarkGray } );
            Records.Add( new DataRecord( "BackgroundColor", DataRecord.RecordTypes.Color ) { Value = Color.LightGray } );
            Records.Add( new DataRecord( "TextColor", DataRecord.RecordTypes.Color ) { Value = Color.Black } );

            Records.Add( new DataRecord( "EmulateSignal", DataRecord.RecordTypes.Boolean ) { Value = false } );
            Records.Add( new DataRecord( "Text", DataRecord.RecordTypes.Text ) { Value = "Unknown" } );
        }
        /// <summary>
        /// Отрисовка элемента расчетных данных
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        public void DrawElement( Graphics graphics, Rectangle rectangle )
        {
            this.DrawBody( graphics, rectangle );
            this.DrawSignal( graphics, rectangle );
        }
        public override string ToString( ) { return "BlockSignalCalculation"; }
        /// <summary>
        /// Получить результат расчетов
        /// </summary>
        /// <returns>Результат расчетов</returns>
        private Color GetResult( )
        {
            if ( Convert.ToBoolean( GetRecord( "EmulateSignal" ).Value ) )
                return (Color)GetRecord( "SetSignalColor" ).Value;

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
        private Rectangle GetSignalCorrectText( Rectangle rectangle )
        {
            //Text
            return new Rectangle
                       {
                           Location = rectangle.Location,
                           Size = new Size( rectangle.Width - 10, rectangle.Height )
                       };
        }
        private void DrawBody( Graphics graphics, Rectangle rectangle )
        {
            //фон
            var sb = new SolidBrush( (Color)GetRecord( "BackgroundColor" ).Value );
            graphics.FillRectangle( sb, rectangle );

            //Текст
            sb = new SolidBrush( (Color)GetRecord( "TextColor" ).Value );
            graphics.DrawString( GetRecord( "Text" ).Value.ToString(), this.font, sb,
                                 this.GetSignalCorrectText( rectangle ), this.format );

            //оконтовка
            var pn = new Pen( Color.Black );
            graphics.DrawRectangle( pn, rectangle );

            pn.Dispose();
            sb.Dispose();
        }
        private void DrawSignal( Graphics graphics, Rectangle rectangle )
        {
            var rec = new Rectangle
                          {
                              Location =
                                  new Point( rectangle.Right - 15, rectangle.Y + rectangle.Height / 2 - 5 ),
                              Size = new Size( 10, 10 )
                          };

            //Signal color
            var sb = new SolidBrush( this.GetResult( ) );
            //оконтовка
            var pn = new Pen( Color.Black );

            //Draw signal
            graphics.FillEllipse( sb, rec );
            //Draw signal (оконтовка)
            graphics.DrawEllipse( pn, rec );

            pn.Dispose();
            sb.Dispose();
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