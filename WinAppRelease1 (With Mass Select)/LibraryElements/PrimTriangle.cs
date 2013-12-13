using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using Structure;

namespace LibraryElements
{
   /// <summary>
   /// Класс элемент треугольник
   /// Поворот: вверх, вниз
   /// </summary>
   public class PrimTriangle : Rotate
   {
      #region Parameters
      int stposX, stposY, posX, posY;
      #endregion

      #region Class Methods
      public PrimTriangle(DrawRotate _rot) : this(_rot, false) { }
      public PrimTriangle(DrawRotate _rot, bool _mirror) : base(_rot, _mirror)
      {
         this.stposX = this.stposY = this.posX = this.posY = 0;

         this.IsSelected = true;
         this.IsModify = true;
      }
      #endregion

      #region Override Methods
      /// <summary>
      /// Отрисовка положения вверх
      /// </summary>
      protected override void Draw_Up(Graphics g, bool _mirror)
      {
         if (_mirror)
         {
            Draw_Down(g, false);
            return;
         }

         Pen pn = CreateViewPenElement(this.ElementColor);

         stposX = elem_rec.X;
         stposY = elem_rec.Bottom;
         posX = elem_rec.X + elem_rec.Width / 2;
         posY = elem_rec.Y;
         g.DrawLine(pn, stposX, stposY, posX, posY);

         stposX = posX;
         stposY = posY;
         posX = elem_rec.Right;
         posY = elem_rec.Bottom;
         g.DrawLine(pn, stposX, stposY, posX, posY);

         stposX = posX;
         stposY = posY;
         posX = elem_rec.X;
         posY = elem_rec.Bottom;
         g.DrawLine(pn, stposX, stposY, posX, posY);

         //белая полоса,чтоб скрыть линию
         //CreateViewPenElement(Color.White, 1);
         //stposX = elem_rec.X + elem_rec.Width / 2;
         //stposY = elem_rec.Y;
         //posX = stposX;
         //posY = elem_rec.Bottom;
         //g.DrawLine(pn, stposX, stposY, posX, posY);

         pn.Dispose();
      }
      /// <summary>
      /// Отрисовка положения вниз
      /// </summary>
      protected override void Draw_Down(Graphics g, bool _mirror)
      {
         if (_mirror)
         {
            Draw_Up(g, false);
            return;
         }

         Pen pn = CreateViewPenElement(this.ElementColor);

         stposX = elem_rec.X;
         stposY = elem_rec.Y;
         posX = elem_rec.X + elem_rec.Width / 2;
         posY = elem_rec.Bottom;
         g.DrawLine(pn, stposX, stposY, posX, posY);

         stposX = posX;
         stposY = posY;
         posX = elem_rec.Right;
         posY = elem_rec.Y;
         g.DrawLine(pn, stposX, stposY, posX, posY);

         stposX = posX;
         stposY = posY;
         posX = elem_rec.X;
         posY = elem_rec.Y;
         g.DrawLine(pn, stposX, stposY, posX, posY);

         //белая полоса,чтоб скрыть линию
         //CreateViewPenElement(Color.White, 1);
         //stposX = elem_rec.X + elem_rec.Width / 2;
         //stposY = elem_rec.Y;
         //posX = stposX;
         //posY = elem_rec.Bottom;
         //g.DrawLine(pn, stposX, stposY, posX, posY);

         pn.Dispose();
      }
      /// <summary>
      /// Отрисовка положения влево
      /// </summary>
      protected override void Draw_Left( Graphics g, bool _mirror )
      {
          if ( _mirror )
          {
              Draw_Right( g, false );
              return;
          }

          Pen pn = CreateViewPenElement( this.ElementColor );

          stposX = elem_rec.Right;
          stposY = elem_rec.Y;
          posX = elem_rec.X;
          posY = elem_rec.Y + elem_rec.Height / 2;
          g.DrawLine( pn, stposX, stposY, posX, posY );

          stposX = posX;
          stposY = posY;
          posX = elem_rec.Right;
          posY = elem_rec.Bottom;
          g.DrawLine( pn, stposX, stposY, posX, posY );

          stposX = posX;
          stposY = posY;
          posX = elem_rec.Right;
          posY = elem_rec.Y;
          g.DrawLine( pn, stposX, stposY, posX, posY );

          pn.Dispose();
      }
      /// <summary>
      /// Отрисовка положения вправо
      /// </summary>
      protected override void Draw_Right( Graphics g, bool _mirror )
      {
          if ( _mirror )
          {
              Draw_Left( g, false );
              return;
          }

          Pen pn = CreateViewPenElement( this.ElementColor );

          stposX = elem_rec.X;
          stposY = elem_rec.Y;
          posX = elem_rec.Right;
          posY = elem_rec.Y + elem_rec.Height / 2;
          g.DrawLine( pn, stposX, stposY, posX, posY );

          stposX = posX;
          stposY = posY;
          posX = elem_rec.X;
          posY = elem_rec.Bottom;
          g.DrawLine( pn, stposX, stposY, posX, posY );

          stposX = posX;
          stposY = posY;
          posX = elem_rec.X;
          posY = elem_rec.Y;
          g.DrawLine( pn, stposX, stposY, posX, posY );

          pn.Dispose();
      }
      public override Element CopyElement()
      {
          var create = new PrimTriangle( DrawRotate.Down );
          create.CopyElement( this );
          return create;
      }
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         this.DrawMethod(g);

         DrawSelected(g);
      }
      #endregion
   }
}
