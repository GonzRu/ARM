using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LibraryElements
{
   /// <summary>
   /// Класс элемент емкость
   /// </summary>
   public class Capacity : Figure
   {
      public Capacity()
      {
         this.IsSelected = true;
         this.IsModify = true;
      }
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         int x1, y1, x2, y2;

         CreateViewPenElement(Color.Black, 2);

         //масштабирование
         mtx.Reset();
         mtx.Scale(Scale, Scale);
         g.Transform = mtx;

         //белая полоса,чтоб скрыть линию
         x1 = elem_rec.X;
         y1 = elem_rec.Y + elem_rec.Height / 2;
         x2 = elem_rec.Right;
         y2 = y1;
         g.DrawLine(new Pen(Color.White, 1), x1, y1, x2, y2);

         x1 = elem_rec.X + elem_rec.Width / 3;
         y1 = elem_rec.Y + elem_rec.Height / 5;
         x2 = x1;
         y2 = elem_rec.Bottom - elem_rec.Height / 5;
         g.DrawLine(fig_pen, x1, y1, x2, y2);

         x1 = elem_rec.Right - elem_rec.Width / 3;
         x2 = x1;
         g.DrawLine(fig_pen, x1, y1, x2, y2);
         DisposeViewElement();

         CreateViewPenElement(Color.Black, 1);
         x1 = elem_rec.X;
         y1 = elem_rec.Y + elem_rec.Height / 2;
         x2 = elem_rec.X + elem_rec.Width / 3;
         y2 = y1;
         g.DrawLine(fig_pen, x1, y1, x2, y2);

         x1 = elem_rec.Right - elem_rec.Width / 3;
         x2 = elem_rec.Right;
         g.DrawLine(fig_pen, x1, y1, x2, y2);


         DrawSelected(g);
         DisposeViewElement();

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
   }
}
