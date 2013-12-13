using System;
using System.Xml.Linq;

using LibraryElements.CalculationBlocks;

namespace FileManager
{
    public class BuildFormula : LinqStream
    {
        public const string FormulaBlock = "Behaviour of the block.xml";
        public const string BlockImage = "Device.bmp";

        ElementCalculation calculation;

        /// <summary>
        /// Разбор данных из файла
        /// </summary>
        public void ParceDataFromFile()
        {
            if ( error_flag ) return;

            xroot = xdoc.Element( "namespace" );
            if (xroot != null)
                ParceDataFromNode( xroot.Element( "formulas" ) );
        }
        /// <summary>
        /// Разбор данных из ветки
        /// </summary>
        /// <param name="data">Ветвь с данными</param>
        public void ParceDataFromNode( XElement data )
        {
            if ( error_flag || data == null )
                return;

            var xAttr = data.Attribute( "calculation" );
            var calcName = ( xAttr == null ) ? "NaN" : xAttr.Value;

            calculation = ElementCalculation.DefineElement( calcName );

            if ( this.calculation == null )
                return;

            this.calculation.ReadXRecords( data );
        }
        public CalculationContext GetData() { return ( calculation == null ) ? null : new CalculationContext( calculation ); }
    }
    public class CreateFormula : LinqStream
    {
        public void CreateCompleateTree( CalculationContext calculation )
        {
            var ns = CreateDeclaration();
            ns.Add( GetCreateNode( calculation ) );
        }
        public XElement GetCreateNode( CalculationContext calculation )
        {
            var root = new XElement( "formulas" );
            root.Add( new XAttribute( "calculation", ( calculation == null ) ? "NaN" : calculation.Context.ToString() ) );

            if ( calculation != null )
                root.Add( calculation.Context.CreateXRecordsNode() );

            return root;
        }
        public static XElement CreateEmptyNode( )
        {
            var root = new XElement( "formulas" );
            root.Add( new XAttribute( "calculation", "NaN" ) );
            return root;
        }
    }
}
