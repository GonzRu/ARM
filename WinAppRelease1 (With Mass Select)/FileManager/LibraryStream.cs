using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

using LibraryElements;
using LibraryElements.CalculationBlocks;
using LibraryElements.Sources;

using Structure;

namespace FileManager
{
    /// <summary>
    /// Класс файлового потока
    /// </summary>
    public class SchemasStream : LinqXmlMethods
    {
        readonly string sturtupFolder;
        int winWidth, winHeight;

        IProcess form;
        IErrorLog errorLogForm;

        Figure osElement;
        IElementLine iosLine;

        Rectangle elemrect;

        #region Class Methods
        public SchemasStream()
        {
            sturtupFolder = Application.StartupPath + Path.DirectorySeparatorChar + "Project";
        }
        /// <summary>
        /// Форма демонстрирующая процесс загрузки или сохранения
        /// </summary>
        /// <param name="load">форма загрузки</param>
        public void SetProcessForm( IProcess load )
        {
            form = load;
        }
        /// <summary>
        /// Форма демонстрирующая отчет об ошибках чтения
        /// </summary>
        /// <param name="errorLog">форма ошибок</param>
        public void SetErrorForm( IErrorLog errorLog )
        {
            this.errorLogForm = errorLog;
        }
        /// <summary>
        /// Чтение аттрибутов
        /// </summary>
        /// <param name="element">ветвь</param>
        /// <returns>строка с аттрибутами</returns>
        private string CreateAttributeString( XElement element )
        {
            var atrstr = String.Empty;
            var datas = element.Attributes();

            foreach ( var atr in datas )
            {
                atrstr += atr.Name + "=";
                atrstr += atr.Value + " ";
            }

            return atrstr;
        }
        /// <summary>
        /// Чтение ветвей элемента
        /// </summary>
        /// <param name="node">ветвь</param>
        /// <returns>ветвь TreeNode</returns>
        private TreeNode CreateNodeString( XElement node )
        {
            TreeNode node1, node2;
            string nodestr = String.Empty;
            string atrstr = String.Empty;
            XElement xelem = node;

            nodestr = "<" + xelem.Name;

            if ( xelem.HasAttributes )
            {
                atrstr = " " + CreateAttributeString( xelem );
                nodestr += atrstr;
            }

            if ( xelem.HasAttributes )
            {
                if ( !xelem.HasElements ) nodestr += "/>";
                if ( xelem.HasElements ) nodestr += ">";
            }
            else nodestr += ">";

            if ( !xelem.HasElements && xelem.Value != null && xelem.Value != "" )
                nodestr += xelem.Value + "</" + xelem.Name + ">";

            node1 = new TreeNode( nodestr );

            if ( xelem.HasElements )
            {
                IEnumerable<XNode> xnodes = node.Nodes();
                foreach ( XNode xnode in xnodes )
                {
                    node2 = CreateNodeString( (XElement)xnode/*XDocument.Parse(xnode.ToString())*/);
                    node1.Nodes.Add( node2 );
                }

            }

            return node1;
        }
        /// <summary>
        /// Добавление разобранной ветви в окно отчета
        /// </summary>
        private void AddErrorFigureRecord( XElement element )
        {
            if ( this.errorLogForm == null ) return;

            var root = new TreeNode( "Element" );
            foreach ( var selem in element.Elements() )
                root.Nodes.Add( CreateNodeString( selem ) );
            this.errorLogForm.SetErrorRecord( root );
        }
        #endregion

        #region Save
        public void AddDatas( ref List<Element> list, Size winsz )
        {
            this.AddDatas( ref list, winsz, String.Empty );
        }
        public void AddDatas( ref List<Element> list, Size winsz, String caption )
        {
            if ( error_flag ) return;
            if ( form != null ) form.SetMaxValue( list.Count );

            xroot = CreateDeclaration();
            xroot.Add( SaveWindowInformation( caption, winsz ) );

            //Save Elements
            foreach ( var search in list )
            {
                if ( search is Figure )
                {
                    PrepareSaveFigure( (Figure)search );
                    if ( form != null ) form.SetPerformStep();
                }
                if ( search is Line )
                {
                    PrepareSaveLine( (Line)search );
                    if ( form != null ) form.SetPerformStep();
                }
            }
        }

        #region Save Methods
        /// <summary>
        /// Запись служебной информации
        /// </summary>
        /// <param name="caption">Имя схемы</param>
        /// <param name="winsz">Размер окна</param>
        private XElement SaveWindowInformation( String caption, Size winsz )
        {
            XElement xnode = new XElement( "window" );
            XElement xnode2 = new XElement( "win_size" );
            XAttribute xatr = new XAttribute( "width", winsz.Width.ToString() );
            xnode2.Add( xatr );
            xatr = new XAttribute( "height", winsz.Height.ToString() );
            xnode2.Add( xatr );
            XElement xnode3 = SaveSchemVersion();
            XElement xnode4 = new XElement( "mneno_caption", caption );
            xnode.Add( xnode2 );
            xnode.Add( xnode3 );
            xnode.Add( xnode4 );

            return xnode;
        }
        /// <summary>
        /// Запись версии схемы
        /// </summary>
        private XElement SaveSchemVersion()
        {
            XElement xnode = new XElement( "schema" );
            XAttribute xatr = new XAttribute( "version", Assembly.GetExecutingAssembly().GetName().Version.ToString() );
            xnode.Add( xatr );
            return xnode;
        }
        /// <summary>
        /// Подготовка к записи фигуры
        /// </summary>
        /// <param name="elem">интерфейс фигуры</param>
        private void PrepareSaveFigure( Figure elem )
        {
            this.osElement = elem;
            var xnode = new XElement( "element" );
            xroot.Add( xnode );

            xnode.Add( SaveFigureLocation( ) );
            xnode.Add( SaveFigureType( ) );
            xnode.Add( SaveFigureStyle( ) );

            var calc = osElement as ICalculationContext;
            if ( calc != null )
            {
                var param = osElement as IDynamicParameters;
                using ( var createCalculation = new CreateFormula( ) )
                {
                    if ( param != null && !param.Parameters.ExternalDescription )
                        xnode.Add( createCalculation.GetCreateNode( calc.CalculationContext ) );
                    else xnode.Add( CreateFormula.CreateEmptyNode( ) );
                }
            }
        }
        /// <summary>
        /// Подготовка к записи линии
        /// </summary>
        /// <param name="elem">линия</param>
        private void PrepareSaveLine( Line elem )
        {
            this.iosLine = elem;
            var xnode = new XElement( "line_element" );
            xroot.Add( xnode );

            xnode.Add( SaveLines() );

            var calc = iosLine as ICalculationContext;
            if ( calc != null )
                using ( var createCalculation = new CreateFormula() )
                {
                    xnode.Add( createCalculation.GetCreateNode( calc.CalculationContext ) );
                }
        }
        /// <summary>
        /// Запись динамических данных элемента
        /// </summary>
        /// <param name="parameters">Параметры элемента</param>
        private object[] SaveFigureDynamicElements( DynamicParameters parameters )
        {
            var ParamNode = new XElement( "parameters" );
            ParamNode.Add( new XAttribute( "dsGuid", parameters.DsGuid ) );
            ParamNode.Add( new XAttribute( "devGuid", parameters.DeviceGuid ) );
            ParamNode.Add( new XAttribute( "cell", parameters.Cell ) );
            ParamNode.Add( new XAttribute( "type", parameters.Type ) );
            ParamNode.Add( new XAttribute( "tooltip", parameters.ToolTipMessage ) );
            ParamNode.Add( new XAttribute( "extern", parameters.ExternalDescription ) );

            var commandNode = new XElement("command",
                                           new XAttribute("dsGuid", parameters.DsGuidForCommandBinding),
                                           new XAttribute("devGuid", parameters.DeviceGuidForCommandBinding),
                                           new XAttribute("commandGuid", parameters.CommandGuidForCommandBinding));

            var externalProgramNode = new XElement("ExternalProgram",
                                                   new XAttribute("IsExecExternalProgram", parameters.IsExecExternalProgram),
                                                   new XAttribute("PathToExternalProgram", parameters.PathToExternalProgram));

            return new object[] {ParamNode, commandNode, externalProgramNode};
        }
        /// <summary>
        /// Запись типа фигуры
        /// </summary>
        private XElement SaveFigureType()
        {
            XElement xnode = null, xnode2 = null;

            if ( this.osElement is StaticElement )
            {
                xnode = this.CreateElementType( "Static_Element", "static" );
                var img = ( (StaticElement)this.osElement ).GetImage();
                xnode2 = ( img == null ) ? new XElement( "image", "null" ) : new XElement( "image", img.Path );
                xnode.Add( xnode2 );
                return xnode;
            }
            if ( this.osElement is DynamicElement )
            {
                xnode = this.CreateElementType( "Dinamic_Element", "dynamic" );

                xnode.Add(SaveFigureDynamicElements(((IDynamicParameters)this.osElement).Parameters));

                return xnode;
            }
            if ( this.osElement is Ground )
            {
                xnode = this.CreateElementType( "Ground", "static",
                                                ( (Rotate)this.osElement ).TurnPosition.ToString(),
                                                ( (Rotate)this.osElement ).Mirror );
                return xnode;
            }
            if ( this.osElement is FloorChassis )
            {
                xnode = this.CreateElementType( "FloorChassis", "static",
                                                ( (Rotate)this.osElement ).TurnPosition.ToString(),
                                                ( (Rotate)this.osElement ).Mirror );
                return xnode;
            }
            if ( this.osElement is TrunkPoint )
            {
                xnode = this.CreateElementType( "TrunkPoint", "static" );
                return xnode;
            }
            if ( this.osElement is FormText )
            {
                xnode = this.CreateElementType( "FormText", "static" );
                xnode2 = SaveTextGeneral( (IFormText)this.osElement );
                xnode.Add( xnode2 );
                xnode2 = SaveTextOther( (IFormText)this.osElement );
                xnode.Add( xnode2 );
                return xnode;
            }
            if ( this.osElement is PrimEllipse )
            {
                xnode = this.CreateElementType( "Ellipse", "static" );
                return xnode;
            }
            if ( this.osElement is PrimRectangle )
            {
                xnode = this.CreateElementType( "Rectangle", "static" );
                return xnode;
            }
            if ( this.osElement is PrimTriangle )
            {
                xnode = this.CreateElementType( "Triangle", "static",
                                                ( (Rotate)this.osElement ).TurnPosition.ToString(),
                                                ( (Rotate)this.osElement ).Mirror );
                return xnode;
            }
            if ( this.osElement is PrimArc )
            {
                xnode = this.CreateElementType( "Arc", "static",
                                                ( (Rotate)this.osElement ).TurnPosition.ToString(),
                                                ( (Rotate)this.osElement ).Mirror );
                return xnode;
            }
            if ( this.osElement is BlockText )
            {
                xnode = this.CreateElementType( "BlockText", "static" );

                if ( this.osElement is SchemaButton )
                    xnode = this.CreateElementType( "SchemaButton", "dynamic" );

                xnode2 = this.SaveBlockText( (BlockText)this.osElement );
                xnode.Add( xnode2 );

                return xnode;
            }

            return null;
        }
        /// <summary>
        /// Запись стиля фигуры
        /// </summary>
        private XElement SaveFigureStyle()
        {
            XElement xnode = new XElement( "style" );
            XAttribute xatr = new XAttribute( "color", ConvertMethods.GetColorString( this.osElement.ElementColor ) );
            xnode.Add( xatr );

            return xnode;
        }
        /// <summary>
        /// Запись данных блока диагностики
        /// </summary>
        /// <param name="block">Елемент</param>
        private XElement SaveBlockText( BlockText block )
        {
            var xnode = new XElement( "data" );
            xnode.Add( new XAttribute( "name", block.Text ) );
            xnode.Add( new XAttribute( "group", block.Group ) );
            return xnode;
        }
        /// <summary>
        /// Запись местоположения фигуры
        /// </summary>
        private XElement SaveFigureLocation()
        {
            var xnode = new XElement( "rect" );
            xnode.Add( new XAttribute( "x", this.osElement.GetPosition().X ) );
            xnode.Add( new XAttribute( "y", this.osElement.GetPosition().Y ) );
            xnode.Add( new XAttribute( "width", this.osElement.GetPosition().Width ) );
            xnode.Add( new XAttribute( "height", this.osElement.GetPosition().Height ) );
            return xnode;
        }
        /// <summary>
        /// Запись текста
        /// </summary>
        private XElement SaveTextGeneral( IFormText figure )
        {
            var xnode = new XElement( "text" );
            xnode.Add( new XAttribute( "txt", figure.Text ) );
            xnode.Add( new XAttribute( "font", figure.TextFont.Name ) );
            xnode.Add( new XAttribute( "size", Convert.ToInt32( figure.TextFont.Size ) ) );
            return xnode;
        }
        /// <summary>
        /// Запись текста
        /// </summary>
        private XElement SaveTextOther( IFormText figure )
        {
            var xnode = new XElement( "other" );
            xnode.Add( new XAttribute( "color", ConvertMethods.GetColorString( figure.ElementColor ) ) );
            xnode.Add( new XAttribute( "vertical", figure.VerticalView ) );
            return xnode;
        }
        /// <summary>
        /// Запись ломаной или простой линий
        /// </summary>
        private XElement SaveLines()
        {
            var xnode = new XElement( "points" );
            var xnode2 = new XElement( "line" );
            xnode2.Add( new XAttribute( "line_points", this.iosLine.GetPoints() ) );
            xnode.Add( xnode2 );

            xnode2 = new XElement( "type" );
            if ( this.iosLine is Trunk )
            {
                xnode2.Add( new XAttribute( "figure_name", "Trunk" ) );
                xnode2.Add( new XAttribute( "figure_type", "dinamic" ) );
            }
            else
            {
                if ( this.iosLine is PolyLine )
                    xnode2.Add( new XAttribute( "figure_name", "PolyLine" ) );
                else 
                    xnode2.Add( new XAttribute( "figure_name", "SingleLine" ) );

                xnode2.Add( new XAttribute( "figure_type", "static" ) );
            }
            xnode.Add( xnode2 );

            xnode2 = new XElement( "style" );
            xnode2.Add( new XAttribute( "color", ConvertMethods.GetColorString( this.iosLine.ElementColor ) ) );
            xnode2.Add( new XAttribute( "width", this.iosLine.Thickness ) );
            xnode2.Add( new XAttribute( "ln_style", this.iosLine.LineStyle.ToString() ) );
            xnode.Add( xnode2 );

            return xnode;
        }
        #endregion
        #endregion

        #region Load
        public void ReadDatas( ref List<Element> list, ref int width, ref int height )
        {
            if ( error_flag ) return;

            ReadWindowInformation();
            width = this.winWidth;
            height = this.winHeight;

            this.ReadDatas( ref list );
        }
        public void ReadDatas( ref List<Element> list )
        {
            if ( error_flag ) return;
            var xNames = xdoc.Element( "namespace" );
            if ( xNames == null ) return;

            var xFigures = xNames.Elements( "element" );
            var xLines = xNames.Elements( "line_element" );

            if ( form != null )
                form.SetMaxValue( xFigures.Count() + xLines.Count() );

            #region Figures
            foreach ( var xFigure in xFigures )
            {
                ReadElement( xFigure );
                if ( this.osElement != null )
                {
                    this.osElement.SetPosition( elemrect.Location );
                    this.osElement.SetSize( elemrect.Size );
                    list.Add( this.osElement );

                    if ( form != null ) form.SetPerformStep();
                }
                else
                    if ( form != null ) form.SetNewError();
            }
            #endregion

            #region Lines
            foreach ( var xLine in xLines )
            {
                ReadLine( xLine );

                if ( this.iosLine != null )
                {
                    list.Add( (Element)this.iosLine );
                    if ( form != null ) form.SetPerformStep();
                }
                else
                    if ( form != null ) form.SetNewError();
            }
            #endregion
        }
       /// <summary>
        /// Взять название схемы
        /// </summary>
        /// <returns>имя схемы</returns>
        public String GetMnenoCaption()
        {
            var mnenoCaption = String.Empty;
            var xNode = xdoc.Element( "namespace" );
            if ( xNode == null ) return mnenoCaption;
            xNode = xNode.Element( "window" );
            if ( xNode == null ) return mnenoCaption;
            xNode = xNode.Element( "mneno_caption" );
            return ( xNode == null ) ? mnenoCaption : xNode.Value;
        }

        #region Load Methods
        /// <summary>
        /// Чтение служебной информации
        /// </summary>
        private void ReadWindowInformation()
        {
            var xNode = xdoc.Element( "namespace" );
            if ( xNode == null )
            {
                winWidth = 800;
                winHeight = 600;
                return;
            }
            xNode = xNode.Element( "window" );
            if ( xNode == null )
            {
                winWidth = 800;
                winHeight = 600;
                return;
            }
            xNode = xNode.Element( "win_size" );
            if ( xNode == null )
            {
                winWidth = 800;
                winHeight = 600;
                return;
            }

            var xAttr1 = xNode.Attribute( "width" );
            var xAttr2 = xNode.Attribute( "height" );
            if ( xAttr1 == null || xAttr2 == null )
            {
                winWidth = 800;
                winHeight = 600;
            }
            else
            {
                winWidth = Convert.ToInt32( xAttr1.Value );
                winHeight = Convert.ToInt32( xAttr2.Value );;
            }
        }
        /// <summary>
        /// Создание элемента
        /// </summary>
        /// <param name="type">тип элемента</param>
        /// <param name="turn">поворот фигуры</param>
        /// <param name="mirror">зеркальный поворот фигуры</param>
        /// <returns>true - если фигура создана</returns>
        private bool CreateElement( string type, string turn, bool mirror )
        {
            var readturn = ConvertMethods.GetTurnPosition( turn );
            this.osElement = null;

            switch ( type )
            {
                case "Static_Element":
                    this.osElement = new StaticElement();
                    break;
                case "Dinamic_Element":
                    this.osElement = new DynamicElement();
                    break;
                case "Ground":
                    this.osElement = new Ground( readturn, mirror );
                    break;
                case "FloorChassis":
                    this.osElement = new FloorChassis( readturn, mirror );
                    break;
                case "TrunkPoint":
                    this.osElement = new TrunkPoint();
                    break;
                case "FormText":
                    this.osElement = new FormText();
                    break;
                case "Ellipse":
                    this.osElement = new PrimEllipse();
                    break;
                case "Rectangle":
                    this.osElement = new PrimRectangle();
                    break;
                case "Triangle":
                    this.osElement = new PrimTriangle( readturn, mirror );
                    break;
                case "Arc":
                    this.osElement = new PrimArc( readturn, mirror );
                    break;
                case "SchemaButton":
                    this.osElement = new SchemaButton();
                    break;
                case "BlockText":
                    this.osElement = new BlockText();
                    break;
            }

            if ( this.osElement != null )
            {
                osElement.IsSelected = false;
                osElement.IsModify = false;
            }
            return this.osElement != null;
        }
        /// <summary>
        /// Чтение фигур из файла
        /// </summary>
        private void ReadElement( XElement node )
        {
            string ftype = string.Empty, fturn = string.Empty;
            var fmirror = false;
            var tmp = node.Element( "type" );

            var xattr = tmp.Attribute( "figure_name" );
            if ( xattr != null ) ftype = xattr.Value;
            xattr = tmp.Attribute( "turn" );
            if ( xattr != null ) fturn = xattr.Value;
            xattr = tmp.Attribute( "mirror" );
            if ( xattr != null ) fmirror = ConvertMethods.GetBooleanStatus( xattr.Value );

            if ( !CreateElement( ftype, fturn, fmirror ) )
            {
                MessageError( "Данного элемента в библиотеке не существует: " + ftype );
                this.AddErrorFigureRecord( node );
                return;
            }

            foreach ( var elem in node.Elements() )
                switch ( elem.Name.ToString() )
                {
                    case "rect": ReadRectAtributes( elem ); break;
                    case "type": ReadType( elem ); break;
                    case "style":
                        {
                            osElement.ElementColor = ConvertMethods.GetParseColor( elem.Attribute( "color" ).Value );
                        }
                        break;
                    case "formulas":
                        try
                        {
                            ChoiseFormulaType(elem, osElement);
                        }
                        catch (Exception ex)
                        {
                            MessageError(string.Format("Type: {0} Message: {1}", ftype, ex.Message));
                        }
                        break;
                }
        }
        /// <summary>
        /// Чтение данных о расположении фигуры
        /// </summary>
        /// <param name="node">ветвь xml с данными</param>
        private void ReadRectAtributes( XElement node )
        {
            int x = 0, y = 0, w = 30, h = 30;
            var xAttr = node.Attribute( "x" );
            if (xAttr != null) x = Convert.ToInt32( xAttr.Value );
            xAttr = node.Attribute( "y" );
            if (xAttr != null) y = Convert.ToInt32( xAttr.Value );
            xAttr = node.Attribute( "width" );
            if (xAttr != null) w = Convert.ToInt32( xAttr.Value );
            xAttr = node.Attribute( "height" );
            if (xAttr != null) h = Convert.ToInt32( xAttr.Value );

            elemrect = new Rectangle( x, y, w, h );
        }
        /// <summary>
        /// Чтение текста
        /// </summary>
        /// <param name="node">ветвь xml с данными</param>
        private void ReadTextAtributes( XElement node )
        {
            var txtclr = new Color();
            string txtstr = String.Empty, txtfont = String.Empty, txtuom = string.Empty;
            var txtvert = false;
            var txtsize = 0;

            var iFText = (IFormText)this.osElement;

            //чтение атрибутов текста
            foreach ( var selem in node.Elements() )
            {
                if ( selem.Name == "text" )
                {
                    txtstr = selem.Attribute( "txt" ).Value;
                    txtfont = selem.Attribute( "font" ).Value;
                    txtsize = Convert.ToInt32( selem.Attribute( "size" ).Value );
                }
                if ( selem.Name == "other" )
                {
                    txtclr = ConvertMethods.GetParseColor( selem.Attribute( "color" ).Value );
                    txtvert = ConvertMethods.GetBooleanStatus( selem.Attribute( "vertical" ).Value );
                }
            }

            iFText.TextFont = new Font( txtfont, txtsize );
            iFText.Text = txtstr;
            iFText.ElementColor = txtclr;
            iFText.VerticalView = txtvert;
        }
        /// <summary>
        /// Чтение динамических данных фигуры
        /// </summary>
        /// <param name="node">ветвь xml с данными</param>
        private void ReadType( XElement node )
        {
            if ( this.osElement is IDynamicParameters )
            {
                var osParams = ( (IDynamicParameters)this.osElement ).Parameters;
                var xElement = node.Element( "parameters" );
                if ( xElement != null )
                {
                    var xAttribute = xElement.Attribute( "dsGuid" );
                    if ( xAttribute != null )
                    {
                        uint ds;
                        uint.TryParse( xAttribute.Value, out ds );
                        osParams.DsGuid = ds;
                    }
                    
                    xAttribute = xElement.Attribute( "devGuid" );
                    if ( xAttribute != null )
                    {
                        uint dev;
                        uint.TryParse( xAttribute.Value, out dev );
                        osParams.DeviceGuid = dev;
                    }
                    
                    xAttribute = xElement.Attribute( "cell" );
                    if ( xAttribute != null )
                    {
                        uint cell;
                        uint.TryParse( xAttribute.Value, out cell );
                        osParams.Cell = cell;
                    }
                    
                    xAttribute = xElement.Attribute( "type" );
                    if ( xAttribute != null )
                        osParams.Type = ( string.IsNullOrEmpty( xAttribute.Value ) ) ? string.Empty : xAttribute.Value;
                    
                    xAttribute = xElement.Attribute( "tooltip" );
                    if ( xAttribute != null )
                        osParams.ToolTipMessage = ( string.IsNullOrEmpty( xAttribute.Value ) ) ? string.Empty : xAttribute.Value;

                    xAttribute = xElement.Attribute( "extern" );
                    if ( xAttribute != null )
                        osParams.ExternalDescription = !( string.IsNullOrEmpty( xAttribute.Value ) ) && ConvertMethods.GetBooleanStatus( xAttribute.Value );
                }
                else
                {
                    xElement = node.Element( "strNameBlock" );
                    if ( xElement != null )
                        osParams.Type = ( string.IsNullOrEmpty( xElement.Value ) ) ? string.Empty : xElement.Value;

                    xElement = node.Element( "Name" );
                    if ( xElement != null )
                        osParams.ToolTipMessage = ( string.IsNullOrEmpty( xElement.Value ) ) ? string.Empty : xElement.Value;

                    uint fk = 0, device = 0, cell = 0;
                    xElement = node.Element( "nFC" );
                    if ( xElement != null )
                        uint.TryParse( xElement.Value, out fk );

                    xElement = node.Element( "idDev" );
                    if ( xElement != null )
                        uint.TryParse( xElement.Value, out device );

                    xElement = node.Element( "nLoc" );
                    if ( xElement != null )
                        uint.TryParse( xElement.Value, out cell );

                    osParams.DsGuid = 0;
                    osParams.DeviceGuid = fk * 256 + device;
                    osParams.Cell = cell;
                }

                #region command section
                osParams.DsGuidForCommandBinding = 0;
                osParams.DeviceGuidForCommandBinding = 0;
                osParams.CommandGuidForCommandBinding = 0;

                xElement = node.Element("command");
                if (xElement != null)
                {
                    var xAttribute = xElement.Attribute("dsGuid");
                    if (xAttribute != null)
                    {
                        uint ds;
                        uint.TryParse(xAttribute.Value, out ds);
                        osParams.DsGuidForCommandBinding = ds;
                    }

                    xAttribute = xElement.Attribute("devGuid");
                    if (xAttribute != null)
                    {
                        uint dev;
                        uint.TryParse(xAttribute.Value, out dev);
                        osParams.DeviceGuidForCommandBinding = dev;
                    }

                    xAttribute = xElement.Attribute("commandGuid");
                    if (xAttribute != null)
                    {
                        uint command;
                        uint.TryParse(xAttribute.Value, out command);
                        osParams.CommandGuidForCommandBinding = command;
                    }
                }
                #endregion

                #region Exec external programm section
                osParams.IsExecExternalProgram = false;
                osParams.PathToExternalProgram = String.Empty;

                xElement = node.Element("ExternalProgram");
                if (xElement != null)
                {
                    var IsExecExternalProgramAttribute = xElement.Attribute("IsExecExternalProgram");
                    if (IsExecExternalProgramAttribute != null)
                    {
                        bool IsExecExternalProgram;
                        if (bool.TryParse(IsExecExternalProgramAttribute.Value, out IsExecExternalProgram))
                            osParams.IsExecExternalProgram = IsExecExternalProgram;
                    }

                    var PathToExternalProgramAttribute = xElement.Attribute("PathToExternalProgram");
                    if (PathToExternalProgramAttribute != null)
                    {
                        osParams.PathToExternalProgram = PathToExternalProgramAttribute.Value;
                    }
                }
                #endregion
            }
            if ( this.osElement is StaticElement )
            {
                var xImage = node.Element( "image" );
                if ( xImage == null ) return;

                try
                {
                    var img = WorkFile.ReadImageFile( xImage.Value );
                    ( (StaticElement)this.osElement ).SetImage( new ImageData( img, xImage.Value ) );
                }
                catch ( Exception )
                {
                    MessageError( "Не верно указан путь до изображения" );
                }
            }

            if ( osElement is IFormText )
                ReadTextAtributes( node );

            if ( this.osElement is BlockText )
            {
                var block = (BlockText)this.osElement;
                var xnode = node.Element( "data" );
                if ( xnode != null )
                {
                    var xattr = xnode.Attribute( "name" );
                    if ( xattr != null )
                        block.Text = xattr.Value;
                    xattr = xnode.Attribute( "group" );
                    if ( xattr != null )
                        block.Group = xattr.Value;
                }
            }

        }
        private void ChoiseFormulaType( XElement node, Element element )
        {
            var iCalcData = element as ICalculationContext;
            if ( iCalcData == null ) return;
            var iDynamic = element as IDynamicParameters;

            using ( var build = new BuildFormula() )
            {
                if ( element is Line || iDynamic == null || !iDynamic.Parameters.ExternalDescription )
                    build.ParceDataFromNode( node );
                else
                {
                    var path = sturtupFolder + Path.DirectorySeparatorChar + iDynamic.Parameters.Type + Path.DirectorySeparatorChar + BuildFormula.FormulaBlock;
                    if ( !WorkFile.CheckExistFile( path ) )
                    {
                        MessageError( "Не найден файл описания поведения устройства: " + path );
                        return;
                    }

                    build.LoadFile( path );
                    build.ParceDataFromFile();
                }

                iCalcData.CalculationContext = build.GetData();
            }
        }
        /// <summary>
        /// Чтение данных о линии
        /// </summary>
        private void ReadLine( XElement node )
        {
            var xPoints = node.Element( "points" );
            // Поиск в ветве "type" и создание элемента
            var xType = xPoints.Element( "type" );
            var fname = xType.Attribute( "figure_name" ).Value;

            if ( !CreateLine( fname ) )
            {
                //error_form.SetErrorRecord((TreeNode)_xml_elem);
                return;
            }

            // Чтение данных линии
            if ( this.iosLine != null )
            {
                foreach ( var selem in xPoints.Elements() )
                {
                    switch ( selem.Name.ToString() )
                    {
                        case "line": this.iosLine.SetReadPoints( selem.Attribute( "line_points" ).Value ); break;
                        case "style": ParseLineAtributes( selem, iosLine ); break;
                        default: continue;
                    }
                }

                var xFormulas = node.Element( "formulas" );
                if (this.iosLine is ICalculationContext && xFormulas != null)
                    try
                    {
                        ChoiseFormulaType( xFormulas, (Element)iosLine );
                    }
                    catch ( Exception ex )
                    {
                        MessageError( string.Format( "Type: {0} Message: {1}", fname, ex.Message ) );
                    }
            }
        }
        /// <summary>
        /// Создание линии
        /// </summary>
        /// <param name="type">тип линии</param>
        /// <returns>true если линия создана</returns>
        private bool CreateLine( string type )
        {
            this.iosLine = null;

            switch ( type )
            {
                case "SingleLine":
                    this.iosLine = new Line();
                    break;
                case "PolyLine":
                    this.iosLine = new PolyLine();
                    break;
                case "Trunk":
                    this.iosLine = new Trunk();
                    break;
            }

            if ( iosLine != null )
                ( (Element)iosLine ).IsSelected = false;

            return iosLine != null;
        }
        #endregion
        #endregion
    }
}
