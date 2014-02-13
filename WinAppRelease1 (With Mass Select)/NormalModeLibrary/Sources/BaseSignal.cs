using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NormalModeLibrary.Sources
{
    public abstract class BaseSignal : BaseObject
    {
        public enum SignalType
        {
            Analog,
            Discret,
            Enum,
            String,
            DateTime,
            Unknown
        }

        internal Panel.LinkType type = Panel.LinkType.Named;
        internal uint dsGuid = 0, objectGuid = 0;

        protected BaseSignal()
        {
            Caption = Dim = Commentary = string.Empty;
            FontSize = 10;
        }
        public override void ParseXml( System.Xml.Linq.XElement xnode )
        {
            Caption = xnode.Attribute( "caption" ).Value;
            Dim = xnode.Attribute( "dim" ).Value;
            Commentary = xnode.Attribute( "commentary" ).Value;
            FontSize = UInt16.Parse(xnode.Attribute("FontSize").Value);

            if ( type == Panel.LinkType.Free )
            {
                string[] strs = xnode.Attribute( "guid" ).Value.Split( '.' );
                dsGuid = uint.Parse( strs[0] );
                objectGuid = uint.Parse( strs[1] );
                Guid = uint.Parse( strs[2] );
            }
            else
                Guid = uint.Parse( xnode.Attribute( "guid" ).Value );
        }
        public override XElement CreateXml()
        {
            XElement node = new XElement( "Signal" );
            node.Add( new XAttribute( "adapter", GetSignalTypeName( this ) ) );

            if ( type == Panel.LinkType.Free )
                node.Add( new XAttribute( "guid", string.Format( "{0}.{1}.{2}", dsGuid, objectGuid, Guid ) ) );
            else
                node.Add( new XAttribute( "guid", Guid ) );
            
            node.Add( new XAttribute( "caption", Caption ) );
            node.Add( new XAttribute( "dim", Dim ) );
            node.Add( new XAttribute( "commentary", Commentary ) );
            node.Add( new XAttribute( "FontSize", FontSize ) );
            return node;
        }
        public abstract bool SetValue( object value, bool qualuty );

        public String Caption { get; internal set; }
        public String Dim { get; internal set; }
        public String Commentary { get; internal set; }
        public UInt32 Guid { get; internal set; }
        public UInt16 FontSize { get; internal set; }
        public abstract Object Value { get; }

        internal static SignalType GetSignalType( String name )
        {
            return (SignalType)Enum.Parse( typeof( SignalType ), name );
        }
        internal static BaseSignal CreateSignal( SignalType type )
        {
            switch ( type )
            {
                case SignalType.Analog: return new AnalogSignal();
                case SignalType.Discret: return new DigitalSignal();
                default: return null;
            }
        }
        internal static String[] GetSignalTypeNames()
        {
            string[] names = new string[5];
            names[0] = SignalType.Analog.ToString();
            names[1] = SignalType.Discret.ToString();
            names[2] = SignalType.Enum.ToString();
            names[3] = SignalType.String.ToString();
            names[4] = SignalType.DateTime.ToString();
            return names;
        }
        internal static Boolean CheckSignalType( String typeName )
        {
            var type = GetSignalType( typeName );
            switch ( type )
            {
                case SignalType.Analog:
                case SignalType.Discret: return true;
                default: return false;
            }
        }
        private static String GetSignalTypeName( BaseSignal signal )
        {
            if ( signal is AnalogSignal )
                return SignalType.Analog.ToString();
            else if ( signal is DigitalSignal )
                return SignalType.Discret.ToString();
            else return SignalType.Unknown.ToString();
        }
    }
}
