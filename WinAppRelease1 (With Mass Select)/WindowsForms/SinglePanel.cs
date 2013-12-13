using System.Drawing;
using System.Windows.Forms;

using HelperLibrary;
using LibraryElements;

namespace WindowsForms
{
    public class SinglePanel : BasePanel
    {
        readonly Element element;

        public SinglePanel( Element element, int width, int height )
        {
            this.element = element;
            ClientSize = new Size( width, height );

            ( (Figure)element ).SetPosition( new Point( 0, 0 ) );
            ( (Figure)element ).SetSize( new Size( width, height ) );
            element.IsSelected = false;

            Core = BaseRegion.GetRegion( element );
            if ( Core != null )
                Core.ChangeValue += delegate { Refresh( ); };
        }
        /// <summary>
        /// Отрисовка элемента
        /// </summary>
        protected sealed override void OnPaint( PaintEventArgs e )
        {
            base.OnPaint( e );
            element.DrawElement( e.Graphics );
        }
        /// <summary>
        /// Движение мыши над элементом
        /// </summary>
        protected override void OnMouseMove( MouseEventArgs e )
        {
            if ( Core != null && ( e.Location.X / Core.Scale.X <= Core.Position.Left || e.Location.Y / Core.Scale.Y <= Core.Position.Top ||
                 e.Location.X / Core.Scale.X >= Core.Position.Right || e.Location.Y / Core.Scale.Y >= Core.Position.Bottom ) )
                Core.ToolTip.Active = false;

            base.OnMouseMove( e );
        }
    }
}
