using System;

namespace DataBaseFilesLibrary
{
    class SqlTransationException : Exception
    {
        public SqlTransationException( string text ) : base( text ) { }
    }
}
