using System;
using System.Collections.Generic;
using System.Drawing;

using LibraryElements;

namespace WindowsForms
{
    public class SchemaPanel : BasePanelCollection
    {
        public SchemaPanel( String path, PointF scale )
        {
            LoadingScheme( path );
            GetRegions( scale );

            // сортировка списка элементов
            if ( Elements != null )
                Elements.Sort( ListCompare );
        }
        public SchemaPanel( List<Element> elements, int width, int height, String captionOfSchema, PointF scale )
        {
            Elements = elements;
            CaptionOfSchema = captionOfSchema;
            GetRegions( scale );

            ClientSize = new Size( width, height );

            // сортировка списка элементов
            if ( elements != null )
                elements.Sort( ListCompare );
        }
    }
}
