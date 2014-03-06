using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NormalModeLibrary.Sources
{
    public class Panel : BaseObjectCollection
    {
        public enum LinkType
        {
            Free,
            Named
        }

        internal Panel()
        {
            Caption = "New panel";
            IsVisible = IsCaptionVisible = true;
            IsAutomaticaly = false;
            Width = Height = 200;
        }
        public override void ParseXml( System.Xml.Linq.XElement xnode )
        {
            IsAutomaticaly = bool.Parse( xnode.Attribute( "isAutomaticaly" ).Value );
            IsVisible = bool.Parse( xnode.Attribute( "isVisible" ).Value );
            Type = GetType( xnode.Attribute( "type" ).Value );
            DsGuid = uint.Parse( xnode.Attribute( "dsGuid" ).Value );
            ObjectGuid = uint.Parse( xnode.Attribute( "objectGuid" ).Value );
            IsCaptionVisible = bool.Parse( xnode.Attribute( "isCaptionVisible" ).Value );
            Caption = xnode.Attribute( "caption" ).Value;
            Left = int.Parse( xnode.Attribute( "left" ).Value );
            Top = int.Parse( xnode.Attribute( "top" ).Value );            
            Width = int.Parse( xnode.Attribute( "width" ).Value );
            Height = int.Parse( xnode.Attribute( "height" ).Value );

            IEnumerable<XElement> nodes = xnode.Element( "Adapters" ).Elements( "Signal" );
            foreach ( XElement node in nodes )
            {
                BaseSignal signal = BaseSignal.CreateSignal( BaseSignal.GetSignalType( node.Attribute( "adapter" ).Value ) );
                if ( signal != null )
                {
                    signal.type = Type;
                    signal.dsGuid = DsGuid;
                    signal.objectGuid = ObjectGuid;
                    
                    signal.ParseXml( node );
                    Collection.Add( signal );
                }
            }
        }
        public override XElement CreateXml()
        {
            XElement node = new XElement( "Panel" );
            node.Add( new XAttribute( "isAutomaticaly", IsAutomaticaly ) );
            node.Add( new XAttribute( "isVisible", IsVisible ) );
            node.Add( new XAttribute( "type", Type ) );
            node.Add( new XAttribute( "dsGuid", DsGuid ) );
            node.Add( new XAttribute( "objectGuid", ObjectGuid ) );
            node.Add( new XAttribute( "isCaptionVisible", IsCaptionVisible ) );
            node.Add( new XAttribute( "caption", Caption ) );
            node.Add( new XAttribute( "left", Left ) );
            node.Add( new XAttribute( "top", Top ) );
            node.Add( new XAttribute( "width", Width ) );
            node.Add( new XAttribute( "height", Height ) );

            XElement node2 = new XElement("Adapters");
            node.Add(node2);

            foreach ( BaseSignal signal in Collection )
            {
                signal.type = Type;
                signal.dsGuid = DsGuid;
                signal.objectGuid = ObjectGuid;
                node2.Add( signal.CreateXml() );
            }
            return node;
        }
        internal override string GetTreeNodeText()
        {
            return string.Format( "Панель: {0}", Caption);
        }
        internal override BaseObject Copy()
        {
            Panel copy = new Panel();
            copy.Caption = Caption;
            copy.DsGuid = DsGuid;
            copy.Height = Height;
            copy.IsAutomaticaly = IsAutomaticaly;
            copy.IsCaptionVisible = IsCaptionVisible;
            copy.IsVisible = IsVisible;
            copy.Left = Left;
            copy.ObjectGuid = ObjectGuid;
            copy.Top = Top;
            copy.Type = Type;
            copy.Width = Width;
            foreach ( BaseObject bo in Collection )
                copy.Collection.Add( bo.Copy() );
            return copy;
        }

        public String Caption { get; internal set; }
        public LinkType Type { get; internal set; }
        public UInt32 DsGuid { get; internal set; }
        public UInt32 ObjectGuid { get; internal set; }
        public Boolean IsVisible { get; internal set; }
        public Boolean IsCaptionVisible { get; internal set; }
        public Boolean IsAutomaticaly { get; internal set; }
        public Int32 Left { get; internal set; }
        public Int32 Top { get; internal set; }
        public Int32 Width { get; internal set; }
        public Int32 Height { get; internal set; }

        private static LinkType GetType( String name )
        {
            return (LinkType)Enum.Parse( typeof( LinkType ), name );
        }
    }
}
