using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Demonstration
{
    public partial class Form2 : Form
    {
        private readonly XDocument newXdoc;
        private readonly XElement @namespace;

        public Form2()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Mneno files|*.mnm|Xml files|*.xml";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            newXdoc = new XDocument(new XDeclaration( "1.0", "utf-8", "yes" ) );
            @namespace = new XElement( "namespace" );
            newXdoc.Add( @namespace );
        }
        private static XElement WindowParameters( XContainer node )
        {
            var xWinNode = node.Element( "window" );
            if ( xWinNode == null )
                throw new ArgumentNullException( "Window block not found" );

            var xNode = xWinNode.Element( "schema" );
            if ( xNode == null )
                throw new ArgumentNullException( "\"schema\" block not found" );

            xNode.ReplaceAttributes( new XAttribute( "version", "2.0.0.0" ) );

            return xWinNode;
        }
        private static void CreateParameters( XContainer node, string @extern )
        {
            var xNode = node.Element( "strNameBlock" );
            if ( xNode == null )
                throw new ArgumentNullException( "Type.strNameBlock is null" );
            var type = xNode.Value;
            
            xNode = node.Element( "DescDev" );
            if ( xNode == null )
                throw new ArgumentNullException( "Type.DescDev is null" );
            var toolTip = xNode.Value;

            var xFc = node.Element( "nFC" );
            if ( xFc == null )
                throw new ArgumentNullException( "Type.nFC is null" );
            var fc = Convert.ToUInt32( xFc.Value );
            
            var xNdev = node.Element( "idDev" );
            if ( xNdev == null )
                throw new ArgumentNullException( "Type.idDev is null" );
            var nDev = Convert.ToUInt32( xNdev.Value );

            var xNSection = node.Element( "nSection" );
            if ( xNSection == null )
                throw new ArgumentNullException( "Type.nSection is null" );
            var nSection = xNSection.Value;

            var xParams = new XElement( "parameters" );
            xParams.Add( new XAttribute( "dsGuid", 0 ) );
            xParams.Add( new XAttribute( "devGuid", ( fc == 0 ) ? 256 + nDev : 512 * fc + nDev ) );
            xParams.Add( new XAttribute( "cell", nSection ) );
            xParams.Add( new XAttribute( "type", type ) );
            xParams.Add( new XAttribute( "tooltip", toolTip ) );
            xParams.Add( new XAttribute( "extern", @extern ) );
            node.ReplaceNodes( xParams );
        }
        private static void CreateFormula( XContainer typeNode, XContainer formulaNode )
        {
        
        }
        private static List<Object> ParseLines( XContainer node )
        {
            var newLines = new List<Object>();
            var xLines = node.Elements( "line_element" );

            foreach ( var xLine in xLines )
            {
                var xPoints = xLine.Element( "points" );
                if ( xPoints == null )
                    throw new Exception( xLine.ToString() );
                var xType = xPoints.Element( "type" );
                if ( xType == null )
                    throw new Exception( xLine.ToString() );
                var xAttr = xType.Attribute( "figure_name" );
                if ( xAttr == null )
                    throw new Exception( xLine.ToString() );

                if ( xAttr.Value == "Trunk" )
                {
                    xType.RemoveNodes();
                    var xFormula = xLine.Element( "formulas" );
                    if (xFormula == null) continue;


                }
                
                newLines.Add( xLine );
            }


            return newLines;
        }
        private static List<Object> ParseFigures( XContainer node )
        {
            var newFigures = new List<Object>();
            var xFigures = node.Elements( "element" );
            
            foreach ( var xFigure in xFigures )
            {
                var xType = xFigure.Element( "type" );
                if ( xType == null )
                    throw new Exception( xFigure.ToString() );
                var xAttr = xType.Attribute( "figure_name" );
                if ( xAttr == null )
                    throw new Exception( xFigure.ToString() );

                switch ( xAttr.Value )
                {
                    case "Dinamic_Element":
                        {
                            var @extern = bool.FalseString;
                            
                            var xFormula = xFigure.Element( "formulas" );
                            if ( xFormula == null )
                                xFigure.Add( new XElement( "formulas", new XAttribute( "calculation", "NaN" ) ) );
                            else
                            {
                                var xExtern = xFormula.Attribute( "extern" );
                                if ( xExtern != null )
                                    @extern = xExtern.Value;

                                CreateFormula( xType, xFormula );
                            }
                            CreateParameters( xType, @extern );
                        }
                        break;
                    case "Transformator":
                        {

                        }
                        break;
                    case "Key": break;
                    case "BlockKey":
                        break;
                    case "BaseBlock":
                        break;
                    case "TextSignal":
                        break;
                }

                newFigures.Add( xFigure );
            }

            return newFigures;
        }
        private void Button1Click( object sender, EventArgs e )
        {

        }
        private void Button2Click( object sender, EventArgs e )
        {
            if ( openFileDialog1.ShowDialog( this ) != DialogResult.OK )
                return;

            try
            {
                var xDoc = XDocument.Load( openFileDialog1.FileName );
                var nSpace = xDoc.Element( "namespace" );
                if ( nSpace == null )
                    throw new Exception( "Xml body not found" );
                
                @namespace.Add( WindowParameters( nSpace ) );
                var lines = ParseLines( nSpace );
                if ( lines != null ) @namespace.Add( lines );
                
                var figures = ParseFigures( nSpace );
                if ( figures != null ) @namespace.Add( figures );
                
                newXdoc.Save( System.IO.Path.GetDirectoryName( openFileDialog1.FileName ) +
                              System.IO.Path.DirectorySeparatorChar +
                              System.IO.Path.GetFileNameWithoutExtension( openFileDialog1.FileName ) + "_new.xml" );
            }
            catch ( Exception ex )
            {
                MessageBox.Show( this, ex.Message, "Xml Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }
    }
}
