using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using Structure;

namespace LibraryElements
{
   /// <summary>
   /// Класс элемент ZElement
   /// Поворот: вниз, влево
   /// </summary>
   public class ZElement : Rotate
   {
      #region Class Methods
      public ZElement(DrawRotate _rot)
      {
         this.IsSelected = true;
         this.IsModify = true;

         rotfig = _rot;
      }
      #endregion
      
      #region Override Method
      /// <summary>
      /// Отрисовка положения по умолчанию
      /// </summary>
      protected override void Draw_Down(Graphics g)
      {
         int x1, y1, x2, y2;
         Pen pn = CreateViewPenElement(Color.Black);

         x1 = elem_rec.X + elem_rec.Width / 3;
         y1 = elem_rec.Y;
         x2 = (elem_rec.Right - elem_rec.Width / 3) - x1;
         y2 = elem_rec.Height;
         g.DrawRectangle(pn, x1, y1, x2, y2);

         using (GraphicsPath path = new GraphicsPath())
         {
            x1 = elem_rec.X + elem_rec.Width / 6;
            y1 = elem_rec.Y;
            x2 = x1;
            y2 = elem_rec.Y + elem_rec.Height / 3;
            path.AddLine(x1, y1, x2, y2);

            x1 = x2;
            y1 = y2;
            x2 = elem_rec.Right - elem_rec.Width / 6;
            y2 = elem_rec.Bottom - elem_rec.Height / 3;
            path.AddLine(x1, y1, x2, y2);

            x1 = x2;
            y1 = y2;
            x2 = elem_rec.Right - elem_rec.Width / 6;
            y2 = elem_rec.Bottom;
            path.AddLine(x1, y1, x2, y2);

            g.DrawPath(pn, path);
         }
         pn.Dispose();
      }
      /// <summary>
      /// Отрисовка положения влево
      /// </summary>
      protected override void Draw_Left(Graphics g)
      {
         int x1, y1, x2, y2;
         Pen pn = CreateViewPenElement(Color.Black);

         x1 = elem_rec.X;
         y1 = elem_rec.Y + elem_rec.Height / 3;
         x2 = elem_rec.Width;
         y2 = (elem_rec.Bottom - elem_rec.Height / 3) - y1;
         g.DrawRectangle(pn, x1, y1, x2, y2);

         using (GraphicsPath path = new GraphicsPath())
         {
            x1 = elem_rec.X;
            y1 = elem_rec.Bottom - elem_rec.Height / 6;
            x2 = elem_rec.X + elem_rec.Width / 3;
            y2 = y1;
            path.AddLine(x1, y1, x2, y2);

            x1 = x2;
            y1 = y2;
            x2 = elem_rec.Right - elem_rec.Width / 3;
            y2 = elem_rec.Y + elem_rec.Height / 6;
            path.AddLine(x1, y1, x2, y2);

            x1 = x2;
            y1 = y2;
            x2 = elem_rec.Right;
            y2 = elem_rec.Y + elem_rec.Height / 6;
            path.AddLine(x1, y1, x2, y2);

            g.DrawPath(pn, path);
         }
         pn.Dispose();
      }
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         //масштабирование
         Matrix mtx = new Matrix();
         mtx.Scale(Scale.X, Scale.Y);
         g.Transform = mtx;

         this.DrawMethod(g);

         DrawSelected(g);
         g.ResetTransform();
      }
      #endregion
   }
}
