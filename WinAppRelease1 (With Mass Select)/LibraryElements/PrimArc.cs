using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using Structure;

namespace LibraryElements
{
   /// <summary>
   /// Класс элемент EditorArc(Дуга)
   /// Поворот: вверх, вниз, влево, вправо
   /// </summary>
   public class PrimArc : Rotate
   {
      #region Class Methods
      public PrimArc(DrawRotate _rot) : this(_rot, false) { }
      public PrimArc(DrawRotate _rot, bool _mirror) : base(_rot, _mirror, 200, 200)
      {
         this.IsSelected = true;
         this.IsModify = true;
      }
      #endregion

      #region Override Method
      /// <summary>
      /// Отрисовка положения по умолчанию
      /// </summary>
      protected override void Draw_Down(Graphics g, bool _mirror)
      {
         if (_mirror)
         {
            Draw_Up(g, false);
            return;
         }

         Pen pn = CreateViewPenElement(this.ElementColor);
         g.DrawArc(pn, elem_rec, 0, 180);
         pn.Dispose();
      }
      /// <summary>
      /// Отрисовка положения влево
      /// </summary>
      protected override void Draw_Left(Graphics g, bool _mirror)
      {
         if (_mirror)
         {
            Draw_Right(g, false);
            return;
         }

         Pen pn = CreateViewPenElement(this.ElementColor);
         g.DrawArc(pn, elem_rec, -90, -180);
         pn.Dispose();
      }
      /// <summary>
      /// Отрисовка положения вправо
      /// </summary>
      protected override void Draw_Right(Graphics g, bool _mirror)
      {
         if (_mirror)
         {
            Draw_Left(g, false);
            return;
         }

         Pen pn = CreateViewPenElement(this.ElementColor);
         g.DrawArc(pn, elem_rec, -90, 180);
         pn.Dispose();
      }
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
         g.DrawArc(pn, elem_rec, 0, -180);
         pn.Dispose();
      }
      public override Element CopyElement()
      {
          var create = new PrimArc( DrawRotate.Down );
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
