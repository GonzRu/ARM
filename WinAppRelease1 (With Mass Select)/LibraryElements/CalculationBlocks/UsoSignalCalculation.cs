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
        /// ��������� �������� ��������� ������
        /// </summary>
        /// <param name="graphics">����������� ��������</param>
        /// <param name="rectangle">������� ��������</param>
        public void DrawElement( Graphics graphics, Rectangle rectangle )
        {
            //Signal color
            var sb = new SolidBrush( this.GetResult( ) );
            //���������
            var pn = new Pen( Color.Black );

            //Draw signal
            graphics.FillEllipse( sb, rectangle );
            //Draw signal (���������)
            graphics.DrawEllipse( pn, rectangle );

            pn.Dispose( );
            sb.Dispose( );
        }
        public override string ToString( ) { return "UsoSignalCalculation"; }
        /// <summary>
        /// �������� ��������� ��������
        /// </summary>
        /// <returns>��������� ��������</returns>
        private Color GetResult( )
        {
            if ( StateProtocol == ProtocolStatus.Bad ) return (Color)GetRecord( "LostSignalColor" ).Value;

            // ���������� ���� - ������������� ���� ������������� ���������� �� ����������
            //00h - "��������" - �������
            //01h - "��������" - �������
            //02h - "�� ��������" - ������
            //06h - "����� ���" - ������ � ������������
            //08h - "��������" - �����
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
        /// ��������� ���������
        /// </summary>
        public ProtocolStatus StateProtocol { get { return (ProtocolStatus)GetRecord( "StateProtocol" ).Value; } set { GetRecord( "StateProtocol" ).Value = value; } }
        /// <summary>
        /// ������������� �����
        /// </summary>
        public Boolean IsAdjustment { get { return Convert.ToBoolean( GetRecord( "IsAdjustment" ).Value ); } set { GetRecord( "IsAdjustment" ).Value = value; } }
    }
}