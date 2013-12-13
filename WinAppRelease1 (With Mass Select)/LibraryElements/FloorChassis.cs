using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using Structure;

namespace LibraryElements
{
   /// <summary>
   /// Класс элемент раз'единитель
   /// </summary>
   public class FloorChassis : Rotate
   {
      #region Class Method
      public FloorChassis(DrawRotate _rot) : this(_rot, false) { }
      public FloorChassis(DrawRotate _rot, bool _mirror) : base(_rot, _mirror)
      {
         this.IsSelected = true;
         this.IsModify = true;
      }
      #endregion

      #region Override Methods
      protected override void Draw_Down(Graphics g, bool _mirror)
      {
         if (_mirror)
         {
            Draw_Up(g, false);
            return;
         }

         GraphicsPath path;
         PointF[] line1, line2, line3;
         int stposX;
         int stposY;
         int posX;
         int posY;

         using (path = new GraphicsPath())
         {
            stposX = elem_rec.X + elem_rec.Width / 2;
            stposY = elem_rec.Y;
            posX = stposX;
            posY = elem_rec.Y + elem_rec.Height / 4;
            path.AddLine(stposX, stposY, posX, posY);
            line1 = path.PathPoints;
         }
         //----------------------------------------------
         using (path = new GraphicsPath())
         {
            stposX = elem_rec.X + elem_rec.Width / 6;
            stposY = elem_rec.Y + elem_rec.Height / 4;
            posX = elem_rec.X + elem_rec.Width / 2;
            posY = elem_rec.Y + elem_rec.Height / 2;
            path.AddLine(stposX, stposY, posX, posY);

            stposX = posX;
            stposY = posY;
            posX = elem_rec.Right - elem_rec.Width / 6;
            posY = elem_rec.Y + elem_rec.Height / 4;
            path.AddLine(stposX, stposY, posX, posY);
            line2 = path.PathPoints;
         }
         //----------------------------------------------
         using (path = new GraphicsPath())
         {
            stposX = elem_rec.X + elem_rec.Width / 6;
            stposY = elem_rec.Y + elem_rec.Height / 2;
            posX = elem_rec.X + elem_rec.Width / 2;
            posY = elem_rec.Bottom - elem_rec.Height / 4;
            path.AddLine(stposX, stposY, posX, posY);

            stposX = posX;
            stposY = posY;
            posX = elem_rec.Right - elem_rec.Width / 6;
            posY = elem_rec.Y + elem_rec.Height / 2;
            path.AddLine(stposX, stposY, posX, posY);
            line3 = path.PathPoints;
         }
         //---------------------------------------------- 
         Pen pn = CreateViewPenElement(this.ElementColor);
         g.DrawLines(pn, line1);
         g.DrawLines(pn, line2);
         g.DrawLines(pn, line3);
         pn.Dispose();

         //белая полоска для закраски линии
         stposX = elem_rec.X + elem_rec.Width / 2;
         stposY = elem_rec.Y + elem_rec.Height / 2;
         posX = stposX;
         posY = elem_rec.Bottom - elem_rec.Height / 4 - elem_rec.Height / 15;

         pn = CreateViewPenElement( BackGroundColor );
         g.DrawLine(pn, stposX, stposY, posX, posY);
         pn.Dispose();
      }
      protected override void Draw_Up(Graphics g, bool _mirror)
      {
         if (_mirror)
         {
            Draw_Down(g, false);
            return;
         }

         GraphicsPath path;
         PointF[] line1, line2, line3;
         int stposX;
         int stposY;
         int posX;
         int posY;

         using (path = new GraphicsPath())
         {
            stposX = elem_rec.X + elem_rec.Width / 2;
            stposY = elem_rec.Bottom - elem_rec.Height / 4;
            posX = stposX;
            posY = elem_rec.Bottom;
            path.AddLine(stposX, stposY, posX, posY);
            line1 = path.PathPoints;
         }
         //----------------------------------------------
         using (path = new GraphicsPath())
         {
            stposX = elem_rec.X + elem_rec.Width / 6;
            stposY = elem_rec.Bottom - elem_rec.Height / 4;
            posX = elem_rec.X + elem_rec.Width / 2;
            posY = elem_rec.Y + elem_rec.Height / 2;
            path.AddLine(stposX, stposY, posX, posY);

            stposX = posX;
            stposY = posY;
            posX = elem_rec.Right - elem_rec.Width / 6;
            posY = elem_rec.Bottom - elem_rec.Height / 4;
            path.AddLine(stposX, stposY, posX, posY);
            line2 = path.PathPoints;
         }
         //----------------------------------------------
         using (path = new GraphicsPath())
         {
            stposX = elem_rec.X + elem_rec.Width / 6;
            stposY = elem_rec.Y + elem_rec.Height / 2;
            posX = elem_rec.X + elem_rec.Width / 2;
            posY = elem_rec.Y + elem_rec.Height / 4;
            path.AddLine(stposX, stposY, posX, posY);

            stposX = posX;
            stposY = posY;
            posX = elem_rec.Right - elem_rec.Width / 6;
            posY = elem_rec.Y + elem_rec.Height / 2;
            path.AddLine(stposX, stposY, posX, posY);
            line3 = path.PathPoints;
         }
         //---------------------------------------------- 
         Pen pn = CreateViewPenElement(this.ElementColor);
         g.DrawLines(pn, line1);
         g.DrawLines(pn, line2);
         g.DrawLines(pn, line3);
         pn.Dispose();

         //белая полоска для закраски линии
         stposX = elem_rec.X + elem_rec.Width / 2;
         stposY = elem_rec.Y + elem_rec.Height / 2;
         posX = stposX;
         posY = elem_rec.Y + elem_rec.Height / 4 + elem_rec.Height / 15;

         pn = CreateViewPenElement( BackGroundColor );
         g.DrawLine(pn, stposX, stposY, posX, posY);
         pn.Dispose();
      }
      protected override void Draw_Left(Graphics g, bool _mirror)
      {
         if (_mirror)
         {
            Draw_Right(g, false);
            return;
         }

         GraphicsPath path;
         PointF[] line1, line2, line3;
         int stposX;
         int stposY;
         int posX;
         int posY;

         using (path = new GraphicsPath())
         {
            stposX = elem_rec.Right - elem_rec.Width / 4;
            stposY = elem_rec.Y + elem_rec.Height / 2;
            posX = elem_rec.Right;
            posY = stposY;
            path.AddLine(stposX, stposY, posX, posY);
            line1 = path.PathPoints;
         }
         //----------------------------------------------
         using (path = new GraphicsPath())
         {
            stposX = elem_rec.Right - elem_rec.Width / 4;
            stposY = elem_rec.Bottom - elem_rec.Height / 6;
            posX = elem_rec.X + elem_rec.Width / 2;
            posY = elem_rec.Y + elem_rec.Height / 2;
            path.AddLine(stposX, stposY, posX, posY);

            stposX = posX;
            stposY = posY;
            posX = elem_rec.Right - elem_rec.Width / 4;
            posY = elem_rec.Y + elem_rec.Height / 6;
            path.AddLine(stposX, stposY, posX, posY);

            line2 = path.PathPoints;
         }
         //----------------------------------------------
         using (path = new GraphicsPath())
         {
            stposX = elem_rec.X + elem_rec.Width / 2;
            stposY = elem_rec.Bottom - elem_rec.Height / 6;
            posX = elem_rec.X + elem_rec.Width / 4;
            posY = elem_rec.Y + elem_rec.Height / 2;
            path.AddLine(stposX, stposY, posX, posY);

            stposX = posX;
            stposY = posY;
            posX = elem_rec.X + elem_rec.Width / 2;
            posY = elem_rec.Y + elem_rec.Height / 6;
            path.AddLine(stposX, stposY, posX, posY);
            
            line3 = path.PathPoints;
         }
         //---------------------------------------------- 
         Pen pn = CreateViewPenElement(this.ElementColor);
         g.DrawLines(pn, line1);
         g.DrawLines(pn, line2);
         g.DrawLines(pn, line3);
         pn.Dispose();

         //белая полоска для закраски линии
         stposX = elem_rec.X + elem_rec.Width / 2;
         stposY = elem_rec.Y + elem_rec.Height / 2;
         posX = elem_rec.X + elem_rec.Width / 4 + elem_rec.Width / 15;
         posY = stposY;

         pn = CreateViewPenElement( BackGroundColor );
         g.DrawLine(pn, stposX, stposY, posX, posY);
         pn.Dispose();
      }
      protected override void Draw_Right(Graphics g, bool _mirror)
      {
         if (_mirror)
         {
            Draw_Left(g, false);
            return;
         }

         GraphicsPath path;
         PointF[] line1, line2, line3;
         int stposX;
         int stposY;
         int posX;
         int posY;

         using (path = new GraphicsPath())
         {
            stposX = elem_rec.X + elem_rec.Width / 4;
            stposY = elem_rec.Y + elem_rec.Height / 2;
            posX = elem_rec.X;
            posY = stposY;
            path.AddLine(stposX, stposY, posX, posY);
            line1 = path.PathPoints;
         }
         //----------------------------------------------
         using (path = new GraphicsPath())
         {
            stposX = elem_rec.X + elem_rec.Width / 4;
            stposY = elem_rec.Bottom - elem_rec.Height / 6;
            posX = elem_rec.X + elem_rec.Width / 2;
            posY = elem_rec.Y + elem_rec.Height / 2;
            path.AddLine(stposX, stposY, posX, posY);

            stposX = posX;
            stposY = posY;
            posX = elem_rec.X + elem_rec.Width / 4;
            posY = elem_rec.Y + elem_rec.Height / 6;
            path.AddLine(stposX, stposY, posX, posY);

            line2 = path.PathPoints;
         }
         //----------------------------------------------
         using (path = new GraphicsPath())
         {
            stposX = elem_rec.X + elem_rec.Width / 2;
            stposY = elem_rec.Bottom - elem_rec.Height / 6;
            posX = elem_rec.Right - elem_rec.Width / 4;
            posY = elem_rec.Y + elem_rec.Height / 2;
            path.AddLine(stposX, stposY, posX, posY);

            stposX = posX;
            stposY = posY;
            posX = elem_rec.X + elem_rec.Width / 2;
            posY = elem_rec.Y + elem_rec.Height / 6;
            path.AddLine(stposX, stposY, posX, posY);

            line3 = path.PathPoints;
         }
         //---------------------------------------------- 
         Pen pn = CreateViewPenElement(this.ElementColor);
         g.DrawLines(pn, line1);
         g.DrawLines(pn, line2);
         g.DrawLines(pn, line3);
         pn.Dispose();

         //белая полоска для закраски линии
         stposX = elem_rec.X + elem_rec.Width / 2;
         stposY = elem_rec.Y + elem_rec.Height / 2;
         posX = elem_rec.Right - elem_rec.Width / 4 - elem_rec.Width / 15;
         posY = stposY;

         pn = CreateViewPenElement( BackGroundColor );
         g.DrawLine(pn, stposX, stposY, posX, posY);
         pn.Dispose();
      }
      public override Element CopyElement()
      {
          var create = new FloorChassis( DrawRotate.Down );
          create.CopyElement( this );
          return create;
      }
      public override void DrawElement(Graphics g)
      {
         this.DrawMethod(g);

         DrawSelected(g);
      }
      #endregion
   }
}
