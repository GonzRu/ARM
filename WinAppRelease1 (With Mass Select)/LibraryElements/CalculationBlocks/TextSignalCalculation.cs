using System;
using System.Drawing;

using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    /// <summary>
    /// Шаблон нового расчетного элемента взамен TextSignal
    /// </summary>
    public class TextSignalCalculation : ElementCalculation, IFigureResult, IStateProtocol, IAdjustment
    {
        private readonly Font font;
        private readonly StringFormat format;

        public TextSignalCalculation( )
        {
            this.format = new StringFormat( StringFormatFlags.NoClip )
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            this.font = new Font( "Tahoma", 10 );

            Records.Add( new SignalMatchRecord( "OnOff" ) );

            Records.Add( new DataRecord( "StateProtocol", DataRecord.RecordTypes.StateProtocol ) { Value = ProtocolStatus.Bad } );
            Records.Add( new DataRecord( "IsAdjustment", DataRecord.RecordTypes.Boolean ) { Value = false } );
            Records.Add( new DataRecord( "BackgroundColor", DataRecord.RecordTypes.Color ) { Value = Color.LightGray } );
            Records.Add( new DataRecord( "TextColor", DataRecord.RecordTypes.Color ) { Value = Color.Black } );
            Records.Add( new DataRecord( "Text", DataRecord.RecordTypes.Text ) { Value = "Unknown" } );
            Records.Add( new DataRecord( "Uom", DataRecord.RecordTypes.Text ) { Value = string.Empty } );
            Records.Add( new DataRecord( "IsVerticalView", DataRecord.RecordTypes.Boolean ) { Value = false } );
        }
        public void DrawElement( Graphics graphics, Rectangle rectangle )
        {
            //фон
            var sb = new SolidBrush( (Color)GetRecord( "BackgroundColor" ).Value );
            graphics.FillRectangle( sb, rectangle );

            //Текст
            sb = ( StateProtocol == ProtocolStatus.Bad )
                     ? new SolidBrush( Color.Gray )
                     : new SolidBrush( (Color)GetRecord( "TextColor" ).Value );

            var isVertical = Convert.ToBoolean( GetRecord( "IsVerticalView" ).Value );
            var @string = CreateFullString( GetRecord( "Text" ).Value.ToString( ),
                                            GetRecord( "OnOff" ).Value.ToString( ),
                                            GetRecord( "Uom" ).Value.ToString( ),
                                            isVertical );

            this.format.Alignment = isVertical ? StringAlignment.Center : StringAlignment.Near;

            graphics.DrawString( @string, this.font, sb, rectangle, this.format );

            //оконтовка
            var pn = new Pen( Color.Black );
            graphics.DrawRectangle( pn, rectangle );

            pn.Dispose( );
            sb.Dispose( );
        }
        public override string ToString( ) { return "TextSignalCalculation"; }

        /// <summary>
        /// Состояние протокола
        /// </summary>
        public ProtocolStatus StateProtocol { get { return (ProtocolStatus)GetRecord( "StateProtocol" ).Value; } set { GetRecord( "StateProtocol" ).Value = value; } }
        /// <summary>
        /// Корректировка тэгов
        /// </summary>
        public Boolean IsAdjustment { get { return Convert.ToBoolean( GetRecord( "IsAdjustment" ).Value ); } set { GetRecord( "IsAdjustment" ).Value = value; } }

        private static String CreateString( String title, String uom )
        {
            return ( string.IsNullOrEmpty( uom ) ) ? title : string.Format( "{0} ({1})", title, uom );
        }
        private static String CreateFullString( String title, String value, String uom, bool vertical )
        {
            return ( vertical )
                       ? string.Format( "{0}\n{1}", CreateString( title, uom ), value )
                       : string.Format( "{0}: {1}", title, CreateString( value, uom ) );
        }
    }
}
