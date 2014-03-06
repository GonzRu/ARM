using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Media;

namespace NormalModeLibrary.Sources
{
    public class DigitalSignal : BaseSignal
    {
        Color value = GetDefaultColorOff();

        internal DigitalSignal()
        {
            SignalOn = GetDefaultColorOn();
            SignalOff = GetDefaultColorOff();
        }
        public override void ParseXml( System.Xml.Linq.XElement xnode )
        {
            base.ParseXml( xnode );

            SignalOn = GetColorValue( xnode.Attribute( "signalOn" ).Value);
            SignalOff = GetColorValue( xnode.Attribute( "signalOff" ).Value );
        }
        public override XElement CreateXml()
        {
            XElement node = base.CreateXml();
            node.Add( new XAttribute( "signalOn", SetColorValue( SignalOn ) ) );
            node.Add( new XAttribute( "signalOff", SetColorValue( SignalOff ) ) );
            return node;
        }
        public override bool SetValue( object value, bool quality )
        {
            if ( !quality )
            {
                this.value = GetDefaultColorOff();
                return true;
            }

            bool tmp = false;
            bool res = bool.TryParse( value.ToString(), out tmp );
            if ( res )
            {
                Color clr = ( tmp ) ? SignalOn : SignalOff;
                if ( clr != this.value )
                    this.value = clr;
                else res = false;
            }
            return res;
        }
        internal override string GetTreeNodeText()
        {
            return string.Format( "Дискретный сигнал: {0}", Caption );
        }
        internal override BaseObject Copy()
        {
            DigitalSignal copy = new DigitalSignal();
            copy.Caption = Caption;
            copy.Commentary = Commentary;
            copy.Dim = Dim;
            copy.dsGuid = dsGuid;
            copy.Guid = Guid;
            copy.objectGuid = objectGuid;
            copy.SignalOff = SignalOff;
            copy.SignalOn = SignalOn;
            copy.type = type;
            copy.value = value;
            copy.FontSize = FontSize;
            return copy;
        }

        public Color SignalOn { get; internal set; }
        public Color SignalOff { get; internal set; }
        #warning 123
        //public override Object Value { get { return new SolidColorBrush( value ); } }
        public override Object Value { get { return value == SignalOn; } }

        internal static Color GetDefaultColorOn()
        {
            return Color.FromRgb( 255, 0, 0 );
        }
        internal static Color GetDefaultColorOff()
        {
            return Color.FromRgb( 128, 128, 128 );
        }
        private static String SetColorValue( Color color )
        {
            return string.Format( "{0} {1} {2}", color.R, color.G, color.B );
        }
        private static Color GetColorValue(string value)
        {
            string[] clrs = value.Split( ' ' );
            return Color.FromRgb( Convert.ToByte( clrs[0] ), Convert.ToByte( clrs[1] ), Convert.ToByte( clrs[2] ) );
        }
    }
}
