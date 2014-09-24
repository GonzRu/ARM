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
        /// ��������� �������� ��������� ������
        /// </summary>
        /// <param name="graphics">����������� ��������</param>
        /// <param name="rectangle">������� ��������</param>
        public void DrawElement( Graphics graphics, Rectangle rectangle )
        {
            this.elementRectangle = rectangle;

            // �������! ����� ����� ��� ������������, ��� ��� � ����� � ���������� ����������� - ��� ��������� �������� ����
            // ������ ���������������� �� ��� ����������, � ������ ���� �������. � ����� � ��� ��� ������� �������� �����
            // ���������� ��������� ��� ����� ���������
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(192, 192, 192)), this.elementRectangle);

            // ��������� �������� ��������� ������
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
        /// ��������� ��������� �����
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
        /// ��������� ��������� ����
        /// </summary>
        private void DrawDown( Graphics graphics )
        {
            //����� ������ ��� ���������
            //����������� � �������������
            this.DrawUp( graphics );
        }
        /// <summary>
        /// ��������� ��������� �����
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
        /// ��������� ��������� ������
        /// </summary>
        private void DrawRight( Graphics graphics )
        {
            //����� ������ ��� ���������
            //����������� � �������������
            this.DrawLeft( graphics );
        }
        /// <summary>
        /// ��������� ������������ �������� � �������������� ����
        /// </summary>
        /// <param name="graphics">����������� ��������</param>
        /// <param name="color">���� ���������</param>
        private void DrawHorisontalConnectors( Graphics graphics, Color color )
        {
            //����������� ����� (-)
            this.stposX = this.elementRectangle.X;
            this.stposY = this.elementRectangle.Y + this.elementRectangle.Height / 2;
            this.posX = this.elementRectangle.X + this.elementRectangle.Width / 4;
            this.posY = this.stposY;

            var pn = new Pen( Color.Black );
            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );

            //����������� ������ (-)
            this.stposX = this.elementRectangle.Right - this.elementRectangle.Width / 4;
            this.posX = this.elementRectangle.Right;

            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );
            pn.Dispose();

            //����������� ����� ����� (|)
            this.stposX = this.elementRectangle.X + this.elementRectangle.Width / 4;
            this.stposY = this.elementRectangle.Y + this.elementRectangle.Height / 4;
            this.posX = this.stposX;
            this.posY = this.elementRectangle.Bottom - this.elementRectangle.Height / 4;

            pn = new Pen( color, 2 );
            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );

            //����������� ����� ������ (|)
            this.stposX = this.elementRectangle.Right - this.elementRectangle.Width / 4;
            this.posX = this.stposX;

            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );
            pn.Dispose();
        }
        /// <summary>
        /// ��������� ������������ �������� � ������������ ����
        /// </summary>
        /// <param name="graphics">����������� ��������</param>
        /// <param name="color">���� ���������</param>      
        private void DrawVerticalConnectors( Graphics graphics, Color color )
        {
            //����������� ������ (|)
            this.stposX = this.elementRectangle.X + this.elementRectangle.Width / 2;
            this.stposY = this.elementRectangle.Y;
            this.posX = this.stposX;
            this.posY = this.elementRectangle.Y + this.elementRectangle.Height / 4;

            var pn = new Pen( Color.Black );
            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );

            //����������� ����� (|)
            this.stposY = this.elementRectangle.Bottom - this.elementRectangle.Height / 4;
            this.posY = this.elementRectangle.Bottom;

            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );
            pn.Dispose();

            //����������� ����� ������ (-)
            this.stposX = this.elementRectangle.X + this.elementRectangle.Width / 4;
            this.stposY = this.elementRectangle.Y + this.elementRectangle.Height / 4;
            this.posX = this.elementRectangle.Right - this.elementRectangle.Width / 4;
            this.posY = this.stposY;

            pn = new Pen( color, 2 );
            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );

            //����������� ����� ����� (-)
            this.stposY = this.elementRectangle.Bottom - this.elementRectangle.Height / 4;
            this.posY = this.stposY;

            graphics.DrawLine( pn, this.stposX, this.stposY, this.posX, this.posY );
            pn.Dispose();
        }
        /// <summary>
        /// ��������� ��������� ����� � �������������� ����
        /// </summary>
        /// <param name="graphics">����������� ��������</param>
        /// <param name="color">���� ���������</param>
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
        /// ��������� ��������� ����� � ������������ ����
        /// </summary>
        /// <param name="graphics">����������� ��������</param>
        /// <param name="color">���� ���������</param>      
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
        /// ��������� ���������
        /// </summary>
        public ProtocolStatus StateProtocol { get { return (ProtocolStatus)GetRecord( "StateProtocol" ).Value; } set { GetRecord( "StateProtocol" ).Value = value; } }
        /// <summary>
        /// ������������� �����
        /// </summary>
        public Boolean IsAdjustment { get { return Convert.ToBoolean( GetRecord( "IsAdjustment" ).Value ); } set { GetRecord( "IsAdjustment" ).Value = value; } }
    }
}