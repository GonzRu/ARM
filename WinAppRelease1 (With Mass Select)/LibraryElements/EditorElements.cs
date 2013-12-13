using System;
using System.Collections.Generic;
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
   public class EditorArc : Rotate
   {
      #region Class Methods
      public EditorArc(DrawRotate _rot) : this(_rot, false) { }
      public EditorArc(DrawRotate _rot, bool _mirror) : base(_rot, _mirror, 200, 200)
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
          var create = new EditorArc( DrawRotate.Down );
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
   /// <summary>
   /// Класс элемент закрашенный Элипс
   /// </summary>
   public class EditorFillEllipse : Figure, IEditorColor
   {
      #region Parameters
      Color upcolor, downcolor;
      #endregion

      #region Class Methods
      public EditorFillEllipse() : base(200, 200)
      {
         this.IsSelected = true;
         this.IsModify = true;
         
         upcolor = Color.White;
      }
      /// <summary>
      /// Закрашивание фигуры разными цветами
      /// </summary>
      /// <param name="up">цвет верхней границы</param>
      /// <param name="down">цвет нижней границы</param>
      public void SetColor(Color up, Color down)
      {
         upcolor = up;
         downcolor = down;
      }
      public Color GetUpColor()
      {
         return upcolor;
      }
      public Color GetDownColor()
      {
         return downcolor;
      }
      #endregion

      #region Override Methods
      public override Element CopyElement()
      {
          var create = new EditorFillEllipse();
          create.CopyElement( this );
          return create;
      }
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         using (LinearGradientBrush tmp = new LinearGradientBrush(elem_rec, upcolor, downcolor, LinearGradientMode.Vertical))
         {
            g.FillEllipse(tmp, elem_rec);
            using (Pen tmp2 = CreateViewPenElement(this.ElementColor))
            {
               g.DrawEllipse(tmp2, elem_rec);
            }
         }

         DrawSelected(g);
      }
      /// <summary>
      /// Копирование элемента
      /// </summary>
      /// <param name="_original">Элемент на основе которого делается копия</param>
      public override void CopyElement(Element _original)
      {
         base.CopyElement(_original);

         upcolor = ((EditorFillEllipse)_original).upcolor;
      }
      #endregion
   }
   /// <summary>
   /// Класс элемент закрашенный Прямоугольник
   /// </summary>
   public class EditorFillRectangle : Figure, IEditorColor
   {
      #region Parameters
      Color upcolor, downcolor;
      #endregion

      #region Class Methods
      public EditorFillRectangle() : base (200,200)
      {
         this.IsSelected = true;
         this.IsModify = true;

         upcolor = Color.White;
      }
      /// <summary>
      /// Закрашивание фигуры разными цветами
      /// </summary>
      /// <param name="up">цвет верхней границы</param>
      /// <param name="down">цвет нижней границы</param>
      public void SetColor(Color up, Color down)
      {
         upcolor = up;
         downcolor = down;
      }
      public Color GetUpColor()
      {
         return upcolor;
      }
      public Color GetDownColor()
      {
         return downcolor;
      }
      #endregion

      #region Override Methods
      public override Element CopyElement()
      {
          var create = new EditorFillRectangle();
          create.CopyElement( this );
          return create;
      }
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         using (LinearGradientBrush tmp = new LinearGradientBrush(elem_rec, upcolor, downcolor, LinearGradientMode.Vertical))
         {
            g.FillRectangle(tmp, elem_rec);
            using (Pen tmp2 = CreateViewPenElement(this.ElementColor))
            {
               g.DrawRectangle(tmp2, elem_rec);
            }
         }

         DrawSelected(g);
      }
      /// <summary>
      /// Копирование элемента
      /// </summary>
      /// <param name="_original">Элемент на основе которого делается копия</param>
      public override void CopyElement(Element _original)
      {
         base.CopyElement(_original);

         upcolor = ((EditorFillRectangle)_original).upcolor;
      }
      #endregion
   }
}
