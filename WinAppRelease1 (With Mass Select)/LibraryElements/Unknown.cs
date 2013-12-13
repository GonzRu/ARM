using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LibraryElements
{
   public class Unknown1 : Figure
   {
      #region Class Methods
      public Unknown1()
      {
         this.IsSelected = true;
         this.IsModify = true;
      }
      /// <summary>
      /// Отрисовка "тела"
      /// </summary>
      private void DrawBody(Graphics g)
      {
         int x1=0, y1=0, x2=0, y2=0;

         x1 = elem_rec.X + elem_rec.Width / 2;
         x2 = x1;
         y1 = elem_rec.Y;
         y2 = elem_rec.Bottom;
         g.DrawLine(fig_pen, x1, y1, x2, y2);

         g.DrawRectangle(fig_pen, elem_rec);
      }
      #endregion

      #region Override Methods
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         //масштабирование
         mtx.Reset();
         mtx.Scale(Scale, Scale);
         g.Transform = mtx;

         CreateViewPenElement(Color.Black, 1);
         DrawBody(g);
         DisposeViewElement();

         DrawSelected(g);        
         g.ResetTransform();
      }
      /// <summary>
      /// Копирование элемента
      /// </summary>
      /// <param name="_original">Элемент на основе которого делается копия</param>
      public override void CopyElement(Element _original)
      {
         base.CopyElement(_original);
      }
      #endregion
   }
}
