using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;

namespace NormalModeLibrary.Sources
{
    public class User : BaseObjectCollection
    {
        internal User()
        {
            ActiveTime = TimeMode.AllTime;
            Login = string.Empty;
        }
        public override void ParseXml( XElement xnode )
        {
            Login = xnode.Attribute( "name" ).Value;
            ActiveTime = Sources.Configuration.GetTimeMode( xnode.Attribute( "activeTime" ).Value );
            
            IEnumerable<XElement> nodes = xnode.Elements( "Configuration" );
            foreach ( XElement node in nodes )
            {
                Configuration config = new Configuration();
                config.ParseXml( node );
                Collection.Add( config );
            }
        }
        public override XElement CreateXml()
        {
            XElement node = new XElement( "User" );
            node.Add( new XAttribute( "name", Login ) );
            node.Add( new XAttribute( "activeTime", ActiveTime.ToString() ) );

            foreach ( Configuration cfg in Collection )
                node.Add( cfg.CreateXml() );

            return node;
        }
        internal override string GetTreeNodeText()
        {
            return string.Format( "Логин: {0} (Активное время: {1})", Login, ActiveTime.ToString() );
        }
        internal override BaseObject Copy()
        {
            User copy = new User();
            copy.ActiveTime = ActiveTime;
            copy.Login = Login;
            foreach ( BaseObject bo in Collection )
                copy.Collection.Add( bo.Copy() );
            return copy;
        }

        public String Login { get; internal set; }
        public TimeMode ActiveTime { get; internal set; }
    }
}
