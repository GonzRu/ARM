using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LibraryElements
{
   public class PrimRectangle : Figure
   {
      public PrimRectangle() : base(300,300)
      {
         this.IsSelected = true;
         this.IsModify = true;
      }
      public override Element CopyElement()
      {
          var create = new PrimRectangle();
          create.CopyElement( this );
          return create;
      }
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         Pen pn = CreateViewPenElement(this.ElementColor);
         g.DrawRectangle(pn, elem_rec);
         pn.Dispose();
         
         DrawSelected(g);
      }
   }
}
