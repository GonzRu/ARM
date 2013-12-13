using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LibraryElements
{
   public class PrimEllipse : Figure
   {
      public PrimEllipse() : base(300, 300)
      {
         this.IsSelected = true;
         this.IsModify = true;
      }

      #region Override Methods
      public override Element CopyElement()
      {
          var create = new PrimEllipse();
          create.CopyElement( this );
          return create;
      }
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         Pen pn = CreateViewPenElement(this.ElementColor);
         g.DrawEllipse(pn, elem_rec);
         pn.Dispose();

         DrawSelected(g);       
      }
      #endregion
   }
}
