using System;
using System.Collections.Generic;
using System.Xml.Linq;

using InterfaceLibrary;

using LibraryElements;

namespace HelperLibrary
{
    public class Simulation
    {
        private class Element
        {
            private readonly string mask;
            public Element( string guid, string mask, string value )
            {
                this.Guid = guid;
                this.mask = mask;
                this.Value = value;
            }

            public string Guid { get; private set; }
            public string Result { get { return string.Format( "0.{0}.{1}", Guid, mask ); } }
            public string Value { get; private set; }
        }

        private readonly List<Element> elements = new List<Element>();

        private void ReadElementValues()
        {
            var simNode = XElement.Load( Environment.CurrentDirectory + @"\Project\Simulation.xml" );
            
            foreach ( var xNode in simNode.Elements( "Element" ) )
            {
                var child = xNode.Attribute( "guid" );
                if ( child == null ) continue;

                foreach ( var xParam in xNode.Elements( "Parameter" ) )
                {
                    var xAttr1 = xParam.Attribute( "mask" );
                    var xAttr2 = xParam.Attribute( "value" );
                    if ( xAttr1 == null || xAttr2 == null ) continue;

                    elements.Add( new Element( child.Value, xAttr1.Value, xAttr2.Value ) );
                }
            }
        }
        private void CheckRegionOnDemo( BaseRegion region )
        {
            var idp = region as IDynamicParameters;
            if ( idp != null && idp.Parameters != null )
            {
                foreach ( var element in elements )
                    if ( idp.Parameters.DeviceGuid == uint.Parse( element.Guid ) )
                        region.IsDemonstration = true;
            }
        }
        private void RegionLinking( BaseRegion region )
        {
            var cr = region as CalculationRegion;
            if ( cr != null && region.IsDemonstration )
            {
                cr.LinkSetTextStatusDev( string.Empty, "5", TypeOfTag.Analog );
                foreach ( var element in elements )
                    region.LinkSetText( element.Result, element.Value, TypeOfTag.Analog );
            }
        }

        public void Parse( IEnumerable<BaseRegion> regions )
        {
            if ( regions == null )
                return;

            this.ReadElementValues();

            foreach ( var region in regions )
            {
                this.CheckRegionOnDemo( region );
                this.RegionLinking( region );
            }
        }
        public void Parse( BaseRegion region )
        {
            if ( region == null )
                return;

            this.ReadElementValues();

            this.CheckRegionOnDemo( region );
            this.RegionLinking( region );
        }
    }
}
