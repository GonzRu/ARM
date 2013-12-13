using System;
using System.Xml.Linq;

namespace HelperLibrary
{
    public static class ContentHelper
    {
        /// <summary>
        /// Чтение контекста комманд меню
        /// </summary>
        /// <param name="node">Xml ветвь</param>
        /// <returns>Контейнер данных о комманде</returns>
        public static CommandContent<byte[]> CreateCommandMenuContent( XElement node )
        {
            var xCommand = node.Attribute( "command" );
            if ( xCommand == null ) return null;
            var xContext = node.Attribute( "context" );
            if ( xContext == null ) return null;
            var xCommandCode = node.Attribute( "code" );
            if ( xCommandCode == null ) return null;
            var xCommandParameter = node.Attribute( "parameter" );

            return new CommandContent<byte[]>
                       {
                               Command = xCommand.Value,
                               Context = xContext.Value,
                               Code = uint.Parse( xCommandCode.Value ),
                               Parameter = ( xCommandParameter == null ) ? new byte[] { } : new[] { Convert.ToByte( xCommandParameter.Value ) }
                       };
        }
        /// <summary>
        /// Чтение контекста контекстного меню
        /// </summary>
        /// <param name="node">Xml ветвь</param>
        /// <returns>Контейнер данных о комманде</returns>
        public static CommandContent<String> CreateContextMenuContent( XElement node )
        {
            var xCommand = node.Attribute( "command" );
            if ( xCommand == null ) return null;
            var xContext = node.Attribute( "context" );
            if ( xContext == null ) return null;
            var xCommandCode = node.Attribute( "code" );
            if ( xCommandCode == null ) return null;
            var xCommandParameter = node.Attribute( "parameter" );

            return new CommandContent<string>
                       {
                               Command = xCommand.Value,
                               Context = xContext.Value,
                               Code = uint.Parse( xCommandCode.Value ),
                               Parameter = ( xCommandParameter == null ) ? "normal" : xCommandParameter.Value
                       };
        }
    }
}