using System;
using System.Drawing;

using Structure;

namespace LibraryElements
{
    /// <summary>
    /// Класс текста блока
    /// </summary>
    public class BlockText : Figure, IBaseFormText
    {
        private Font tFont;
        private StringFormat tStrform;

        public BlockText( ) : this( 500, 500 )
        {
            ElementColor = Color.LightGray;
        }
        protected BlockText( int maxX, int maxY ) : base( maxX, maxY )
        {
            this.tStrform = new StringFormat( StringFormatFlags.NoClip )
                { 
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };
            this.tFont = new Font( "Tahoma", 10 );

            Text = "Unknown";
            Group = "Unknown group";

            ElementModel = Model.Static;
            ElementName = "BlockText";
            ElementColor = Color.LightGray;
            IsSelected = true;
            IsModify = true;
            elem_rec.Size = new Size( 100, 30 );
        }
        private void DrawBody( Graphics g, Rectangle rec )
        {
            //фон
            var sb = CreateViewBrushElement( ElementColor );
            g.FillRectangle( sb, elem_rec );
            //******

            //Текст
            sb = CreateViewBrushElement( Color.Black );
            g.DrawString( Text, this.tFont, sb, rec, this.tStrform );
            //******

            //оконтовка
            var pn = CreateViewPenElement( Color.Black );
            g.DrawRectangle( pn, elem_rec );
            //******

            pn.Dispose();
            sb.Dispose();
        }
        
        public override Element CopyElement( )
        {
            var create = new BlockText( );
            create.CopyElement( this );
            return create;
        }
        /// <summary>
        /// Метод отрисовки
        /// </summary>
        public override void DrawElement( Graphics g )
        {
            DrawBody( g, elem_rec );
            DrawSelected( g );
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="_original">Элемент на основе которого делается копия</param>
        public override void CopyElement( Element _original )
        {
            base.CopyElement( _original );
            var block = (BlockText)_original;

            this.tFont = block.tFont;
            this.tStrform = block.tStrform;
            Text = block.Text;
            Group = block.Group;
        }

        /// <summary>
        /// Получить или задать текст
        /// </summary>
        public String Text { get; set; }
        /// <summary>
        /// Получить или задать группу
        /// </summary>
        public String Group { get; set; }
    }
    /// <summary>
    /// Кнопка схемы
    /// </summary>
    public class SchemaButton : BlockText
    {
        public SchemaButton( ) : base( 500, 500 )
        {
            ElementModel = Model.Dynamic;
            ElementName = "SchemaButton";
            IsSelected = true;
            IsModify = true;
        }
        public override Element CopyElement( )
        {
            var create = new SchemaButton( );
            create.CopyElement( this );
            return create;
        }
    }
}
