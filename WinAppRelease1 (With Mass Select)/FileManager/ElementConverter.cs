using System;
using System.IO;
using System.Xml.Linq;

namespace FileManager
{
    public class ElementConverter : LinqStream
    {
        private enum BlockType
        {
            NaN,
            Bmrz,
            Sirius,
            Key,
            DataBus,
            Transformator
        }

        private String blockName;
        private XElement result;

        public new void LoadFile( String path )
        {
            this.blockName = Path.GetDirectoryName( path );

            base.LoadFile( path );
        }
        public void ConvertSchema( ) { if ( Error_Status ) return; }
        public void ConvertBehavoir( )
        {
            if ( Error_Status ) return;

            var xElement = xdoc.Element( "namespace" );
            if ( xElement == null ) throw new ArgumentNullException( "namespace" );
            xElement = xElement.Element( "formulas" );
            if ( xElement == null ) throw new ArgumentNullException( "formulas" );

            result = this.ConvertBehavoir( xElement, GetBlockType( this.blockName ) );

            if ( result == null )
            {
                this.IsConvert = false;
                return;
            }
        }


        private XElement ConvertBehavoir( XElement node, BlockType blockType, bool isAdjustment = true )
        {
            var xElement = node.Element( "tags" );
            if ( xElement == null ) throw new ArgumentNullException( "tags" );

            switch ( blockType )
            {
                case BlockType.Bmrz:
                    break;
                case BlockType.Sirius:
                    {
                        var rotate = string.Empty;
                        var images = node.Element( "images" );
                        if ( images.ToString( ).Contains( "signal_off_horisontal" ) ) rotate = "Up";
                        if ( images.ToString( ).Contains( "signal_oon_horisontal" ) ) rotate = "Left";


                    }
                    break;
                case BlockType.Key:
                    break;
                case BlockType.DataBus:
                    break;
                case BlockType.Transformator:
                    break;
                default:
                    this.IsConvert = false;
                    break;
            }

            return null;
        }

        public Boolean IsConvert { get; private set; }

        private static BlockType GetBlockType( String name )
        {
            if ( name.Contains( "Bmrz" ) ) return BlockType.Bmrz;
            if ( name.Contains( "Sirius" ) ) return BlockType.Sirius;
            if ( name.Contains( "Key" ) ) return BlockType.Key;
            if ( name.Contains( "Trunk" ) ) return BlockType.DataBus;
            return name.Contains( "Transformator" ) ? BlockType.Transformator : BlockType.NaN;
        }
    }
}
