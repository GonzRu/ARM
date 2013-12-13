using System;
using System.Drawing;

namespace WindowsForms
{
    public class BlockPanel : BasePanelCollection
    {
        public BlockPanel( String path, PointF scale )
        {
            LoadingScheme( path );
            GetRegions( scale );

            // сортировка списка элементов
            if ( Elements != null )
                Elements.Sort( ListCompare );
        }
    }
}
