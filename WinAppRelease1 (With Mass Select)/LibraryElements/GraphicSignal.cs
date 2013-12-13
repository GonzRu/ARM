using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using LibraryElements.Sources;

using Structure;

namespace LibraryElements
{
    public class GraphicSignal : Figure
    {
        private readonly List<PointF> hystory = new List<PointF>();

        public GraphicSignal() : base( 1600, 1600 )
        {
            ElementModel = Model.Dynamic;
            ElementName = "GraphicSignal";
            IsSelected = true;
            IsModify = true;
            
            elem_rec.Size = new Size( 100, 50 );
            TagMatchRecord = new SignalMatchRecord( ElementName );
        }
        /// <summary>
        /// Коллизия попадания курсора мыши
        /// в область изменения
        /// </summary>
        /// <param name="pnt">курсор мыши(X,Y)</param>
        /// <returns>true если курсор попал в обасть для изменения размера</returns>
        public override bool ResizeCollision( Point pnt )
        {
            return false;
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        public override Element CopyElement()
        {
            var create = new GraphicSignal();
            create.CopyElement( this );
            return create;
        }
        /// <summary>
        /// Метод отрисовки
        /// </summary>
        public override void DrawElement( Graphics g )
        {
            //Текст
            using ( var sb = CreateViewBrushElement( ElementColor ) )
            {
                //CreateNewSize( g );
                //g.DrawString( CreateFullString( Text, this.TagMatchRecord.Value.ToString(), Uom, VerticalView ),
                //              TextFont, sb, elem_rec, tStrform );



            }

            DrawSelected( g );
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="_original">Элемент на основе которого делается копия</param>
        public override void CopyElement( Element _original )
        {
            base.CopyElement( _original );
            var origin = (GraphicSignal)_original;

            TagMatchRecord = new SignalMatchRecord( origin.TagMatchRecord );
        }

        /// <summary>
        /// Тег привязки к данным
        /// </summary>
        public SignalMatchRecord TagMatchRecord { get; set; }
    }
}
