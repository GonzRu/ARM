using System.Drawing;

using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    public class ImageCalculation : ElementCalculation, IFigureResult, IStateProtocol
    {
        public ImageCalculation( )
        {
            Records.Add( new DataRecord( "StateProtocol", DataRecord.RecordTypes.StateProtocol ) { Value = ProtocolStatus.Bad } );
            Records.Add( new DataRecord( "NoConnectBodySignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Gray } );
            Records.Add( new DataRecord( "NoConnectSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Red } );

            Records.Add( new DataRecord( "Image", DataRecord.RecordTypes.Image ) );
        }
        /// <summary>
        /// Отрисовка элемента расчетных данных
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        public void DrawElement( Graphics graphics, Rectangle rectangle )
        {
            if ( StateProtocol == ProtocolStatus.Bad )
            {
                BmrzCalculation.DrawRectangleBody( graphics, rectangle,
                                                   new SolidBrush( (Color)GetRecord( "NoConnectBodySignalColor" ).Value ) );
                BmrzCalculation.DrawSimbolConnection( graphics, rectangle,
                                                      new SolidBrush( (Color)GetRecord( "NoConnectSignalColor" ).Value ),
                                                      '!' );
                return;
            }

            var defaultImageRecord = GetRecord( "Image" );
            if ( defaultImageRecord.Value != null )
                BmrzCalculation.DrawDefaultSignal( graphics, rectangle, (ImageData)defaultImageRecord.Value );
            else
            {
                BmrzCalculation.DrawRectangleBody( graphics, rectangle, Brushes.CornflowerBlue );
                BmrzCalculation.DrawOffSignal( graphics, rectangle, DrawRotate.Up );
            }
        }
        public override string ToString( ) { return "ImageCalculation"; }

        /// <summary>
        /// Состояние протокола
        /// </summary>
        public ProtocolStatus StateProtocol { get { return (ProtocolStatus)GetRecord( "StateProtocol" ).Value; } set { GetRecord( "StateProtocol" ).Value = value; } }
    }
}