using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

using LibraryElements.Sources;
using Structure;

namespace LibraryElements.CalculationBlocks
{
    public abstract class ElementCalculation
    {
        protected readonly List<DataRecord> Records = new List<DataRecord>( );

        /// <summary>
        /// ����� ������ � �������
        /// </summary>
        /// <param name="name">��� ������</param>
        /// <returns>������ ��� null</returns>
        protected DataRecord GetRecord( String name ) { return this.Records.FirstOrDefault( record => record.Name == name ); }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="original">�������� ��� �����������</param>
        private void Copy( ElementCalculation original )
        {
            this.Records.Clear( );
            foreach ( var record in original.Records )
                this.Records.Add( DataRecord.GetCopy( record ) );
        }
        /// <summary>
        /// ���������� ��������
        /// </summary>
        /// <param name="format">������ ������������ ���</param>
        /// <param name="value">��������</param>
        public bool SetSignalValue( String format, Object value )
        {
            // ������� raw ��������
            foreach ( SignalMatchRecord tag in this.GetTags( ) )
                if ( tag.Result.Equals( format ) && !tag.Value.Equals( value ) )
                {
                    tag.Value = value;
                    return true;
                }

            return false;
        }
        /// <summary>
        /// �������� ������ ��� ��������
        /// </summary>
        /// <returns>������ �����</returns>
        public IEnumerable<DataRecord> GetTags( )
        {
            return ( from rec in this.Records
                     where rec.RecordType == DataRecord.RecordTypes.Tag
                     select rec ).ToArray( );
        }
        /// <summary>
        /// �������� �������������� ������
        /// </summary>
        /// <returns>������ �������������� ������</returns>
        public IEnumerable<DataRecord> GetOptions( )
        {
            return ( from rec in this.Records
                     where rec.RecordType != DataRecord.RecordTypes.Tag
                     select rec ).ToArray( );
        }

        /// <summary>
        /// ������� xml ����� ������
        /// </summary>
        /// <returns>Xml �����</returns>
        public XElement CreateXRecordsNode( )
        {
            var xNode = new XElement( "records" );
            foreach ( SignalMatchRecord record in this.GetTags( ) )
                xNode.Add( CreateSignalMatch( record ) );

            foreach ( var option in this.GetOptions( ) )
                xNode.Add( CreateXRecord( option ) );

            return xNode;
        }
        /// <summary>
        /// ��������� ������ �� ������ xml ������
        /// </summary>
        /// <param name="data">Xml �����</param>
        public void ReadXRecords( XElement data )
        {
            if ( data == null ) return;
            var xRecords = data.Element( "records" );
            if ( xRecords == null ) return;

            this.Records.Clear( );
            var nodes = xRecords.Elements( "record" );
            foreach ( var node in nodes )
                this.Records.Add( ReadDataRecord( node ) );
        }
        
        /// <summary>
        /// ��������� ����� ��������� ������
        /// </summary>
        /// <param name="original">��������</param>
        /// <returns>����� ��������� ������</returns>
        public static ElementCalculation GetCopy( ElementCalculation original )
        {
            ElementCalculation element = null;

            if ( original is BmrzCalculation ) element = new BmrzCalculation( );
            if ( original is SiriusCalculation ) element = new SiriusCalculation( );
            if ( original is KeyCalculation ) element = new KeyCalculation( );
            if ( original is BlockSignalCalculation ) element = new BlockSignalCalculation( );
            if ( original is TransformatorCalculation ) element = new TransformatorCalculation( );
            if ( original is TrunkCalculation ) element = new TrunkCalculation( );
            if ( original is ImageCalculation ) element = new ImageCalculation( );
            if ( original is UsoSignalCalculation ) element = new UsoSignalCalculation( );
            if ( original is TextSignalCalculation ) element = new TextSignalCalculation( );
            
            if ( element != null ) element.Copy( original );
            return element;
        }
        /// <summary>
        /// ����������� ���� ��������� ������
        /// </summary>
        /// <param name="name">��� ����</param>
        /// <returns>��� ������</returns>
        public static ElementCalculation DefineElement( String name )
        {
            switch ( name )
            {
                case "BmrzCalculation":
                    return new BmrzCalculation( );
                case "SiriusCalculation":
                    return new SiriusCalculation( );
                case "EkraCalculation":
                    return new EkraCalculation();
                case "BreslerCalculation":
                    return new BreslerCalculation();
                case "KeyCalculation":
                    return new KeyCalculation( );
                case "BlockSignalCalculation":
                    return new BlockSignalCalculation( );
                case "TransformatorCalculation":
                    return new TransformatorCalculation( );
                case "TrunkCalculation":
                    return new TrunkCalculation( );
                case "ImageCalculation":
                    return new ImageCalculation( );
                case "UsoSignalCalculation":
                    return new UsoSignalCalculation( );
                case "TextSignalCalculation":
                    return new TextSignalCalculation( );
                default:
                    return null;
            }
        }
        /// <summary>
        /// �������� ������� ������������
        /// </summary>
        /// <param name="record">������ � �������</param>
        /// <returns>Xml �����</returns>
        public static XElement CreateSignalMatch( SignalMatchRecord record )
        {
            var node = CreateXRecord( record );
            var link = new XElement( "link" );
            link.Add( new XAttribute( "dsGuid", record.DsGuid ) );
            link.Add( new XAttribute( "devGuid", record.DevGuid ) );
            link.Add( new XAttribute( "tagGuid", record.TagGuid ) );
            node.Add( link );
            return node;
        }
        /// <summary>
        /// �������� xml ����� �� ������ ������
        /// </summary>
        /// <param name="record">������ � �������</param>
        /// <returns>Xml �����</returns>
        private static XElement CreateXRecord( DataRecord record )
        {
            string value;
            switch ( record.RecordType )
            {
                case DataRecord.RecordTypes.Tag: value = string.Empty; break;
                case DataRecord.RecordTypes.Image: value = ( record.Value == null ) ? string.Empty : ( (ImageData)record.Value ).Path; break;
                case DataRecord.RecordTypes.Color: value = ConvertMethods.GetColorString( (Color)record.Value ); break;
                default: value = record.Value.ToString( ); break;
            }

            var node = new XElement( "record", value );
            node.Add( new XAttribute( "name", record.Name ) );
            node.Add( new XAttribute( "type", record.RecordType ) );
            return node;
        }
        /// <summary>
        /// ��������� �������������
        /// </summary>
        /// <param name="node">Xml ����� �������</param>
        /// <returns>�������������</returns>
        public static DataRecord ReadDataRecord( XElement node )
        {   
            var type = DataRecord.RecordTypes.Unknown;
            var name = "Unknown";

            var xAttr = node.Attribute( "type" );
            if ( xAttr != null ) type = (DataRecord.RecordTypes)Enum.Parse( typeof ( DataRecord.RecordTypes ), xAttr.Value );
            
            xAttr = node.Attribute( "name" );
            if ( xAttr != null ) name = xAttr.Value;

            switch ( type )
            {
                case DataRecord.RecordTypes.Text: return new DataRecord( name, type ) { Value = node.Value };
                case DataRecord.RecordTypes.Color: return new DataRecord( name, type ) { Value = ConvertMethods.GetParseColor( node.Value ) };
                case DataRecord.RecordTypes.Rotate: return new DataRecord( name, type ) { Value = Enum.Parse( typeof ( DrawRotate ), node.Value ) };
                case DataRecord.RecordTypes.Boolean: return new DataRecord( name, type ) { Value = ConvertMethods.GetBooleanStatus( node.Value ) };
                case DataRecord.RecordTypes.StateProtocol: return new DataRecord( name, type ) { Value = Enum.Parse( typeof ( ProtocolStatus ), node.Value ) };
                case DataRecord.RecordTypes.Image:
                    {
                        var image = new DataRecord( name, type );
                        if ( !string.IsNullOrEmpty( node.Value ) )
                            image.Value = new ImageData( WorkFile.ReadImageFile( node.Value ), node.Value );
                        return image;
                    }
                case DataRecord.RecordTypes.Tag:
                    {
                        var xLink = node.Element( "link" );
                        if ( xLink != null )
                        {
                            string dsGuid = "0", devGuid = "0", tagGuid = "0";
                            xAttr = xLink.Attribute( "dsGuid" );
                            if ( xAttr != null ) dsGuid = xAttr.Value;
                            xAttr = xLink.Attribute( "devGuid" );
                            if ( xAttr != null ) devGuid = xAttr.Value;
                            xAttr = xLink.Attribute( "tagGuid" );
                            if ( xAttr != null ) tagGuid = xAttr.Value;
                            return new SignalMatchRecord( name, dsGuid, devGuid, tagGuid );
                        }

                        throw new Exception( "������ ��� ������� ������� ������ ����� ���������" );
                    }
                default: throw new Exception( "������ ��� ������� ������� ������ ����� ���������" );
            }
        }
    }
}