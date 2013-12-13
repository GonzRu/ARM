using System;
using System.Drawing;

using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    public class TransformatorCalculation : ElementCalculation, IFigureResult, IStateProtocol, IAdjustment
    {
        public TransformatorCalculation( )
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
        }
        /// <summary>
        /// ��������� �������� ��������� ������
        /// </summary>
        /// <param name="graphics">����������� ��������</param>
        /// <param name="rectangle">������� ��������</param>
        public void DrawElement( Graphics graphics, Rectangle rectangle )
        {
            using ( var pn = new Pen( this.GetResult( ) ) )
            {
                graphics.DrawEllipse( pn, rectangle );
            }
        }
        public override string ToString() { return "TransformatorCalculation"; }
        /// <summary>
        /// �������� ��������� ��������
        /// </summary>
        /// <returns>��������� ��������</returns>
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
        /// ��������� ���������
        /// </summary>
        public ProtocolStatus StateProtocol { get { return (ProtocolStatus)GetRecord( "StateProtocol" ).Value; } set { GetRecord( "StateProtocol" ).Value = value; } }
        /// <summary>
        /// ������������� �����
        /// </summary>
        public Boolean IsAdjustment { get { return Convert.ToBoolean( GetRecord( "IsAdjustment" ).Value ); } set { GetRecord( "IsAdjustment" ).Value = value; } }
    }
}