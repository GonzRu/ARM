using System;
using System.Drawing;

using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    public class SiriusCalculation : ElementCalculation, IFigureResult, IStateProtocol, IAdjustment
    {
        public SiriusCalculation( )
        {
            Records.Add( new SignalMatchRecord( "RPV" ) );
            Records.Add( new SignalMatchRecord( "RPO" ) );
            Records.Add( new SignalMatchRecord( "Call" ) );

            Records.Add( new DataRecord( "StateProtocol", DataRecord.RecordTypes.StateProtocol ) { Value = ProtocolStatus.Bad } );
            Records.Add( new DataRecord( "IsAdjustment", DataRecord.RecordTypes.Boolean ) { Value = false } );
            Records.Add( new DataRecord( "CallColor", DataRecord.RecordTypes.Color ) { Value = Color.Aqua } );
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
            BmrzCalculation.DrawRectangleBody( graphics, rectangle, Brushes.DarkGray );

            if ( StateProtocol == ProtocolStatus.Bad )
            {
                BmrzCalculation.DrawRectangleBody( graphics, rectangle,
                                                   new SolidBrush( (Color)GetRecord( "NoConnectBodySignalColor" ).Value ) );
                BmrzCalculation.DrawSimbolConnection( graphics, rectangle,
                                                      new SolidBrush( (Color)GetRecord( "NoConnectSignalColor" ).Value ), '!' );
                return;
            }

            var newRectangle = rectangle;
            if ( !GetRecord( "Call" ).Value.Equals( 0 ) )
            {
                graphics.FillRectangle( new SolidBrush( (Color)GetRecord( "CallColor" ).Value ), newRectangle );
                newRectangle = new Rectangle( newRectangle.X + 3, newRectangle.Y + 3,
                                              newRectangle.Width - 6, newRectangle.Height - 6 );
            }

            var defaultImageRecord = GetRecord( "DefaultImage" );
            if ( defaultImageRecord.Value != null )
            {
                BmrzCalculation.DrawDefaultSignal( graphics, newRectangle, (ImageData)defaultImageRecord.Value );
                return;
            }

            if ( !GetRecord( "RPV" ).Value.Equals( 0 ) && GetRecord( "RPO" ).Value.Equals( 0 ) )
            {
                BmrzCalculation.DrawRectangleBody( graphics, newRectangle,
                                                   new SolidBrush( (Color)GetRecord( "SetSignalColor" ).Value ) );
                BmrzCalculation.DrawOnSignal( graphics, newRectangle, (DrawRotate)GetRecord( "Rotate" ).Value );
            }
            if ( GetRecord( "RPV" ).Value.Equals( 0 ) && !GetRecord( "RPO" ).Value.Equals( 0 ) )
            {
                BmrzCalculation.DrawRectangleBody( graphics, newRectangle,
                                                   new SolidBrush( (Color)GetRecord( "NoSignalColor" ).Value ) );
                BmrzCalculation.DrawOffSignal( graphics, newRectangle, (DrawRotate)GetRecord( "Rotate" ).Value );
            }
            if ( GetRecord( "RPV" ).Value.Equals( GetRecord( "RPO" ).Value ) )
            {
                BmrzCalculation.DrawRectangleBody( graphics, newRectangle,
                                                   new SolidBrush( (Color)GetRecord( "UndefineBodySignalColor" ).Value ) );
                BmrzCalculation.DrawSimbolConnection( graphics, newRectangle,
                                                      new SolidBrush( (Color)GetRecord( "UndefineSignalColor" ).Value ),
                                                      '?' );
            }
        }
        public override string ToString( ) { return "SiriusCalculation"; }

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