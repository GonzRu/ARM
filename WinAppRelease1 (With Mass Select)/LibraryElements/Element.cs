using System;
using System.Drawing;

using Structure;

namespace LibraryElements
{
   /// <summary>
   /// Общие элементы об'ектов
   /// </summary>
   public abstract class Element
   {
      #region Class Parameters
      public static Color BackGroundColor = Color.FromArgb( 192, 192, 192 );
      /// <summary>
      /// Имя элемента
      /// </summary>
      private string elem_name;
      /// <summary>
      /// Значение "true" если фигура движеться за мышью
      /// </summary>
      private bool elem_dragged;
      /// <summary>
      /// Значение "true" если фигура меняет свой размер
      /// </summary>
      private bool elem_modify;
      /// <summary>
      /// Значение "true" если фигура выделена
      /// </summary>
      private bool elem_selected;
      /// <summary>
      /// Значение true - если элемент можно вращать
      /// </summary>
      private bool elem_enable_rotate;
      /// <summary>
      /// Уровень элемента (для отображения слоев)
      /// </summary>
      private int elem_level;
      /// <summary>
      /// Масштаб фигуры
      /// (100% - 1.0f, 50% - 0.5f)
      /// </summary>
      private PointF elem_scale;
      /// <summary>
      /// Модель элемента
      /// </summary>
      private Model model;
      /// <summary>
      /// Цвет
      /// </summary>
      private Color elem_color;
      /// <summary>
      /// Смещение относительно мыши
      /// </summary>
      protected Point elem_mouseoffset;
      #endregion

      #region Abstract Method
      public abstract Element CopyElement();
      /// <summary>
      /// Смещение элемента
      /// </summary>
      /// <param name="_shift_x">Смещение по x</param>
      /// <param name="_shift_y">Смещение по y</param>
      public abstract void MoveElementtoShift( int _shift_x, int _shift_y );
      #endregion

      #region Virtual methods
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public virtual void DrawElement(Graphics g) { }
      /// <summary>
      /// Задать величину смещения
      /// относительно мыши
      /// </summary>
      public virtual void MouseOffSet(Point _pnt) { }
      /// <summary>
      /// Метод отрисовки выделения элемента
      /// </summary>
      protected virtual void DrawSelected(Graphics _g) { }
      /// <summary>
      /// Копирование элемента
      /// </summary>
      /// <param name="_original">Элемент на основе которого делается копия</param>
      public virtual void CopyElement(Element _original)
      {
         elem_name = _original.elem_name;
         elem_enable_rotate = _original.elem_enable_rotate;
         elem_scale = _original.elem_scale;
         elem_color = _original.elem_color;

         elem_selected = false;
         elem_modify = false;
      }
      /// <summary>
      /// Коллизия попадания мыши по элементу
      /// </summary>
      /// <param name="_pnt">курсор мыши(X,Y)</param>
      /// <returns>true если курсор на елементе</returns>
      public virtual bool Collision(Rectangle _pnt) { return false; }
      /// <summary>
      /// Коллизия попадания курсора мыши
      /// в область изменения
      /// </summary>
      /// <param name="pnt">курсор мыши(X,Y)</param>
      /// <returns>true если курсор попал в обасть для изменения размера</returns>
      public virtual bool ResizeCollision(Point pnt) { return false; }
      /// <summary>
      /// Конвертация под нужный размер окна
      /// </summary>
      /// <param name="_factor_x">значение для окна по X</param>
      /// <param name="_factor_y">значение для окна по Y</param>
      public virtual void ConverttoNewSize(float _factor_x, float _factor_y) { }
      #endregion

      #region General methods
      /// <summary>
      /// Получить имя элемента (задание значения типа protected)
      /// </summary>
      public String ElementName
      {
         get { return elem_name; }
         protected set { elem_name = value; }
      }
      /// <summary>
      /// Получить или задать режим модификации элемента
      /// </summary>
      public Boolean IsModify
      {
         get { return elem_modify; }
         set { elem_modify = value; }
      }
      /// <summary>
      /// Получить или задать режим перетаскивания элемента
      /// </summary>
      public Boolean IsDragged
      {
         get { return elem_dragged; }
         set { elem_dragged = value; }
      }
      /// <summary>
      /// Получить или задать режим выбора элемента
      /// </summary>
      public Boolean IsSelected
      {
         get { return elem_selected; }
         set { elem_selected = value; }
      }
      /// <summary>
      /// Получить или задать масштаб фигуры
      /// диапозон: от 0.1 до 1.0
      /// </summary>
      public PointF Scale
      {
         get { return elem_scale; }
         set { elem_scale = value; }
      }
      /// <summary>
      /// Получить уровень расположения элемента
      /// диапозон: от 0 до N (задание значения типа protected)
      /// </summary>
      public int Level
      {
         get { return elem_level; }
         protected set { elem_level = value; }
      }
      /// <summary>
      /// Получить модель элемента (задание значения типа protected)
      /// </summary>
      public Model ElementModel
      {
         get { return model; }
         protected set { model = value; }
      }
      /// <summary>
      /// Получить или задать цвет
      /// </summary>
      public Color ElementColor
      {
         get { return elem_color; }
         set { elem_color = value; }
      }
      #endregion
   }
}
