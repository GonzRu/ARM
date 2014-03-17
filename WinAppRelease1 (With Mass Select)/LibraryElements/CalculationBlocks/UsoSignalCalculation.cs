using System;
using System.Drawing;

using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    public class UsoSignalCalculation : ElementCalculation, IFigureResult, IStateProtocol, IAdjustment
    {
        public UsoSignalCalculation( )
        {
            Records.Add( new SignalMatchRecord( "AnalogSignal" ) );
            Records.Add( new DataRecord( "StateProtocol", DataRecord.RecordTypes.StateProtocol ) { Value = ProtocolStatus.Bad } );
            Records.Add( new DataRecord( "IsAdjustment", DataRecord.RecordTypes.Boolean ) { Value = false } );
            Records.Add( new DataRecord( "SetSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Red } );
            Records.Add( new DataRecord( "NoSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Green } );
            Records.Add( new DataRecord( "UnknownSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.Yellow } );
            Records.Add( new DataRecord( "LostSignalColor", DataRecord.RecordTypes.Color ) { Value = Color.DarkGray } );
        }
        /// <summary>
        /// Отрисовка элемента расчетных данных
        /// </summary>
        /// <param name="graphics">Графический контекст</param>
        /// <param name="rectangle">Размеры элемента</param>
        public void DrawElement( Graphics graphics, Rectangle rectangle )
        {
            //Signal color
            var sb = new SolidBrush( this.GetResult( ) );
            //оконтовка
            var pn = new Pen( Color.Black );

            //Draw signal
            graphics.FillEllipse( sb, rectangle );
            //Draw signal (оконтовка)
            graphics.DrawEllipse( pn, rectangle );

            pn.Dispose( );
            sb.Dispose( );
        }
        public override string ToString( ) { return "UsoSignalCalculation"; }
        /// <summary>
        /// Получить результат расчетов
        /// </summary>
        /// <returns>Результат расчетов</returns>
        private Color GetResult( )
        {
            if ( StateProtocol == ProtocolStatus.Bad ) return (Color)GetRecord( "LostSignalColor" ).Value;

            // устройство ОВОД - привязывается одна целочисленная переменная со значениями
            //00h - "исправен" - зеленый
            //01h - "сработал" - красный
            //02h - "не исправен" - желтый
            //06h - "обрыв ВОД" - желтый с перекрестием
            //08h - "отключен" - серый
            switch ((int)Double.Parse(GetRecord("AnalogSignal").Value.ToString()))
            {
                case 0: return (Color)GetRecord("NoSignalColor").Value;
                case 1: return (Color)GetRecord("SetSignalColor").Value;
                case 2:
                case 6: return (Color)GetRecord("UnknownSignalColor").Value;
                default: return (Color)GetRecord("LostSignalColor").Value;
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