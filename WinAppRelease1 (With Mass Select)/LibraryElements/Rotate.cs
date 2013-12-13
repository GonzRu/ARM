using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Structure;

namespace LibraryElements
{
   public abstract class Rotate : Figure
   {
      #region Parameters
      bool mirror;
      DrawRotate rotfig;
      #endregion

      #region Class Methods
      protected Rotate(DrawRotate _rot, bool _mirror)
      {
         mirror = _mirror;
         rotfig = _rot;
      }
      protected Rotate(DrawRotate _rot, bool _mirror, int _Width, int _Height) : base(_Width, _Height)
      {
         mirror = _mirror;
         rotfig = _rot;
      }
      /// <summary>
      /// Выбор отривовки варианта фигуры
      /// </summary>
      protected void DrawMethod(Graphics g)
      {
         switch (this.rotfig)
         {
            case DrawRotate.Up: this.Draw_Up(g, this.mirror); break;
            case DrawRotate.Down: this.Draw_Down(g, this.mirror); break;
            case DrawRotate.Left: this.Draw_Left(g, this.mirror); break;
            case DrawRotate.Right: this.Draw_Right(g, this.mirror); break;
         }
      }
      #endregion

      #region Virtual Methods
      /// <summary>
      /// Отрисовка положения вверх
      /// </summary>
      protected virtual void Draw_Up(Graphics g, bool _mirror) { NoImage(g); }
      /// <summary>
      /// Отрисовка положения вниз
      /// </summary>      
      protected virtual void Draw_Down(Graphics g, bool _mirror) { NoImage(g); }
      /// <summary>
      /// Отрисовка положения влево
      /// </summary>
      protected virtual void Draw_Left(Graphics g, bool _mirror) { NoImage(g); }
      /// <summary>
      /// Отрисовка положения вправо
      /// </summary>      
      protected virtual void Draw_Right(Graphics g, bool _mirror) { NoImage(g); }
      #endregion

      #region Override Methods
      /// <summary>
      /// Копирование элемента
      /// </summary>
      /// <param name="_original">Элемент на основе которого делается копия</param>
      public override void CopyElement(Element _original)
      {
         base.CopyElement(_original);
         this.rotfig = ((Rotate)_original).rotfig;
         this.mirror = ((Rotate)_original).mirror;
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить вариант положения фигуры
      /// </summary>
      public DrawRotate TurnPosition
      {
         get { return this.rotfig; }
         set { this.rotfig = value; }
      }
      public Boolean Mirror
      {
         get { return this.mirror; }
         set { this.mirror = value; }
      }
      #endregion
   }
}
