using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NormalModeLibrary.Sources
{
    public class Configuration : BaseObjectCollection
    {
        internal Configuration()
        {
            ActiveTime = TimeMode.AllTime;
            IsActive = true;
            Place = Places.None;
        }
        public override void ParseXml( XElement xnode )
        {
            ActiveTime = GetTimeMode( xnode.Attribute( "activeTime" ).Value );
            IsActive = bool.Parse( xnode.Attribute( "isActive" ).Value );
            Place = GetPlace( xnode.Attribute( "place" ).Value );

            IEnumerable<XElement> nodes = xnode.Elements( "Panel" );
            foreach ( XElement node in nodes )
            {
                Panel panel = new Panel();
                panel.ParseXml( node );
                Collection.Add( panel );
            }
        }
        public override XElement CreateXml()
        {
            XElement node = new XElement( "Configuration" );
            node.Add( new XAttribute( "activeTime", ActiveTime.ToString() ) );
            node.Add( new XAttribute( "isActive", IsActive ) );
            node.Add( new XAttribute( "place", Place ) );

            foreach ( Panel panel in Collection )
            {
                if ( panel.Collection.Count > 0 )
                    node.Add( panel.CreateXml() );
            }
            return node;
        }
        internal override string GetTreeNodeText()
        {
            return string.Format( "Активное время: {0} (Место: {1})", ActiveTime.ToString(), Place );
        }
        internal override BaseObject Copy()
        {
            Configuration copy = new Configuration();
            copy.ActiveTime = ActiveTime;
            copy.IsActive = IsActive;
            copy.Place = Place;
            foreach ( BaseObject bo in Collection )
                copy.Collection.Add( bo.Copy() );
            return copy;
        }

        public TimeMode ActiveTime { get; internal set; }
        public Places Place { get; internal set; }
        public Boolean IsActive { get; internal set; }

        internal static String[] GetTimeModeNames()
        {
            string[] names = new string[5];
            names[0] = TimeMode.AllTime.ToString();
            names[1] = TimeMode.Morning.ToString();
            names[2] = TimeMode.Day.ToString();
            names[3] = TimeMode.Evening.ToString();
            names[4] = TimeMode.Night.ToString();
            return names;
        }
        internal static String[] GetPlaceNames()
        {
            string[] names = new string[3];
            names[0] = Places.None.ToString();
            names[1] = Places.MainMnemo.ToString();
            names[2] = Places.FastAccess.ToString();
            return names;
        }
        internal static TimeMode GetTimeMode( String name )
        {
            return (TimeMode)Enum.Parse( typeof( TimeMode ), name );
        }
        internal static Places GetPlace( String name )
        {
            return (Places)Enum.Parse( typeof( Places ), name );
        }
    }
}
