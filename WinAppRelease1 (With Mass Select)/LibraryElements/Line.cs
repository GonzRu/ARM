using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using Structure;

namespace LibraryElements
{
   /// <summary>
   /// Класс одиночная линия
   /// </summary>
    public class Line : Element, IElementLine
   {
      #region ClassParameters
      protected LinePoints ln_line;
      protected LineStatus ln_mod;
      protected SelectPoint sel_point;

      protected DashStyle ln_style;
      protected int ln_thickness;
      #endregion

      #region Class Methods
      public Line()
      {
         this.Level = 0;
         this.Scale = new PointF(1.0f, 1.0f);
         this.IsDragged = false;
         this.IsSelected = true;
         this.ln_thickness = 1;
         this.ln_mod = LineStatus.None;
         this.sel_point = SelectPoint.None;
         this.ln_line = new LinePoints();
         this.elem_mouseoffset = new Point();

         this.ElementColor = DefaultColor();
         this.ln_style = DashStyle.Solid;
      }
      /// <summary>
      /// Проверка на попадание мышью в начальные
      /// или конечные координаты
      /// </summary>
      /// <param name="_pnt">курсор мыши(X,Y)</param>
      /// <returns>true если попали</returns>
      private bool CheckStartFinPoints(Point _pnt)
      {
         float x1, y1, x2, y2;
         x1 = ln_line.Start.X * Scale.X;
         y1 = ln_line.Start.Y * Scale.Y;
         x2 = ln_line.Finish.X * Scale.X;
         y2 = ln_line.Finish.Y * Scale.Y;

         if ((_pnt.X == x1) && (_pnt.Y == y1))
            return true;
         if ((_pnt.X == x2) && (_pnt.Y == y2))
            return true;
         if ((x1 == x2) && (y1 == y2))
            return true;

         return false;
      }
      /// <summary>
      /// Метод проверки линии в вертикальной плоскости
      /// </summary>
      /// <param name="_pnt">Координаты мыши</param>
      /// <param name="_stpnt">Начальные координаты линии</param>
      /// <param name="_fnpnt">Конечные координаты линии</param>
      /// <returns>true если точка лежит в диапозоне на линии</returns>
      private bool CheckVerticalPoints(Point _pnt, PointF _stpnt, PointF _fnpnt)
      {
         int shift = 10;
         double x, y, x1, y1, x2, y2;
         x = _pnt.X;
         y = _pnt.Y;
         x1 = Math.Round(_stpnt.X * Scale.X, 1);
         y1 = Math.Round(_stpnt.Y * Scale.Y, 1);
         x2 = Math.Round(_fnpnt.X * Scale.X, 1);
         y2 = Math.Round(_fnpnt.Y * Scale.Y, 1);

         if (x1 == x2 && (x > x1 - shift && x < x1 + shift))
         {
            if (y1 < y2)
               if (y1 < y && y < y2)
                  return true;

            if (y1 > y2)
               if (y2 < y && y < y1)
                  return true;
         }
         return false;
      }
      /// <summary>
      /// Метод проверки линии в горизонтальной плоскости
      /// </summary>
      /// <param name="_pnt">Координаты мыши</param>
      /// <param name="_stpnt">Начальные координаты линии</param>
      /// <param name="_fnpnt">Конечные координаты линии</param>
      /// <returns>true если точка лежит в диапозоне на линии</returns>
      private bool CheckHorisontalPoints(Point _pnt, PointF _stpnt, PointF _fnpnt)
      {
         int shift = 10;
         double x, y, x1, y1, x2, y2;
         x = _pnt.X;
         y = _pnt.Y;
         x1 = Math.Round(_stpnt.X * Scale.X, 1);
         y1 = Math.Round(_stpnt.Y * Scale.Y, 1);
         x2 = Math.Round(_fnpnt.X * Scale.X, 1);
         y2 = Math.Round(_fnpnt.Y * Scale.Y, 1);

         if (y1 == y2 && (y > y1 - shift && y < y1 + shift))
         {
            if (x1 < x2)
               if (x1 < x && x < x2)
                  return true;

            if (x1 > x2)
               if (x2 < x && x < x1)
                  return true;
         }
         return false;
      }
      /// <summary>
      /// Цвет линии по умолчанию
      /// </summary>
      protected Color DefaultColor()
      {
         return Color.Black;
      }
      /// <summary>
      /// Метод проверки принадлежности заданной точки линии
      /// "Коллинеарность векторов"
      /// </summary>
      /// <param name="_pnt">Координаты мыши</param>
      /// <param name="_stpnt">Начальные координаты линии</param>
      /// <param name="_fnpnt">Конечные координаты линии</param>
      /// <returns>true если точка лежит на линии</returns>
      protected Boolean Collinear(PointF _pnt, PointF _stpnt, PointF _fnpnt)
      {
         double x, y, x1, x2, y1, y2;
         double param1, param2;

         //проверка принадлежности точки линии
         x = Convert.ToDouble(_pnt.X);
         y = Convert.ToDouble(_pnt.Y);
         x1 = Convert.ToDouble(_stpnt.X * Scale.X);
         y1 = Convert.ToDouble(_stpnt.Y * Scale.Y);
         x2 = Convert.ToDouble(_fnpnt.X * Scale.X);
         y2 = Convert.ToDouble(_fnpnt.Y * Scale.Y);

         //Коллинеарность
         param1 = Math.Round(((x - x1) / (x2 - x1)), 1);
         param2 = Math.Round(((y - y1) / (y2 - y1)), 1);
         int tmp = param1.CompareTo(param2);

         if (tmp == 0)
            return true;
         else
            return false;
      }

      public PointF GetStartPoint()
      {
         return ln_line.Start;
      }
      public PointF GetFinishPoint()
      {
         return ln_line.Finish;
      }
      public void SetNewPoints(PointF _start, PointF _finish)
      {
         this.ln_line.Start = _start;
         this.ln_line.Finish = _finish;
      }
      #endregion

      #region Virtual Method
      public virtual String GetPoints()
      {
         string str;
         str = "M" + ln_line.Start.X.ToString() + ":" + ln_line.Start.Y.ToString();
         str += " L" + ln_line.Finish.X.ToString() + ":" + ln_line.Finish.Y.ToString() + " Z";
         return str;
      }
      /// <summary>
      /// Чтение точек из строки для простой линии
      /// </summary>
      /// <param name="_parse_str">строка точек</param>
      public virtual void SetReadPoints(string _parse_str)
      {
         char symbol;
         string str = null;
         PointF point = new PointF();
         object obj;

         for (int i = 0; i < _parse_str.Length; i++)
         {
            symbol = _parse_str[i];
            switch (symbol)
            {
               case 'M': break;
               case ' ': break;
               case 'Z':
               case 'L': obj = Convert.ChangeType(str, typeof(float)); point.Y = (float)obj; str = null; this.AddPoint(point); break;
               case ':': obj = Convert.ChangeType(str, typeof(float)); point.X = (float)obj; str = null; break;
               default: str += symbol; break;
            }
         }//for
      }
      /// <summary>
      /// Метод прикрепления точки линии курсору
      /// </summary>
      /// <param name="_pnt">Координаты XY</param>
      public virtual bool PointOnMouse(Point _pnt)
      {
         switch (ln_mod)
         {
            case LineStatus.Add: ln_line.Finish = _pnt; return true;//break;
            case LineStatus.PointStart: ln_line.Start = _pnt; return true;//break;
            case LineStatus.PointFinish: ln_line.Finish = _pnt; return true;//break;
            default: return false;//break;
         }
      }
      /// <summary>
      /// координаты новой точки
      /// </summary>
      /// <param name="_pos">Координаты XY</param>
      public virtual void AddPoint(PointF _pnt)
      {
         switch (ln_mod)
         {
            case LineStatus.None:
               {
                  ln_line.Start = _pnt;
                  ln_line.Finish = _pnt;
                  ln_mod = LineStatus.Add;
               }
               break;
            case LineStatus.Add:
               {
                  ln_line.Finish = _pnt;
                  ln_mod = LineStatus.Close;
               }
               break;
            case LineStatus.PointStart:
               {
                  ln_line.Start = _pnt;
                  ln_mod = LineStatus.Close;
               }
               break;
            case LineStatus.PointFinish:
               {
                  ln_line.Finish = _pnt;
                  ln_mod = LineStatus.Close;
               }
               break;
            default: break;
         }
      } 
      /// <summary>
      /// Смещение линии за мышью
      /// </summary>
      /// <param name="_pnt">Координаты XY</param>
      public virtual void MoveLine(Point _pnt)
      {
         int w, h, x, y, x1, y1, x2, y2;

         if (IsSelected)
         {
            x = _pnt.X;
            y = _pnt.Y;

            w = Convert.ToInt32(ln_line.Finish.X - ln_line.Start.X); //длинна
            h = Convert.ToInt32(ln_line.Finish.Y - ln_line.Start.Y); //высота

            x1 = Convert.ToInt32((x / Scale.X + elem_mouseoffset.X / Scale.X)); //координаты начала линии
            y1 = Convert.ToInt32((y / Scale.Y + elem_mouseoffset.Y / Scale.Y));
            x2 = Convert.ToInt32(x1 + w);    //координаты конца линии
            y2 = Convert.ToInt32(y1 + h);

            ln_line.Start = new PointF(x1, y1);
            ln_line.Finish = new PointF(x2, y2);
         }
      }
      /// <summary>
      /// Установить флаг для точки следовать за мышью 
      /// </summary>
      public virtual void SetPointtoMouse()
      {
         if (sel_point == SelectPoint.Start)
            ln_mod = LineStatus.PointStart;

         if (sel_point == SelectPoint.Finish)
            ln_mod = LineStatus.PointFinish;
      }
      /// <summary>
      /// Проверка входят ли все точки линии в прямоугольник выделения
      /// </summary>
      /// <param name="_rec">прямоугольник выделения</param>
      /// <returns>true если все точки попали в выделение</returns>
      protected virtual bool CheckAllPointsinRect(Rectangle _rec)
      {
         Point ln_pnt = new Point();
         ln_pnt.X = Convert.ToInt32(ln_line.Start.X);
         ln_pnt.Y = Convert.ToInt32(ln_line.Start.Y);

         if (_rec.Contains(ln_pnt))
         {
            ln_pnt.X = Convert.ToInt32(ln_line.Finish.X);
            ln_pnt.Y = Convert.ToInt32(ln_line.Finish.Y);
            if(_rec.Contains(ln_pnt))
               return true;
         }
         
         return false;
      }
      #endregion

      #region Override Methods
      /// <summary>
      /// Метод отрисовки выделения элемента
      /// </summary>
      protected override void DrawSelected(Graphics g)
      {
         if (IsSelected)
         {
            int x1 = Convert.ToInt32(ln_line.Start.X - 2);
            int y1 = Convert.ToInt32(ln_line.Start.Y - 2);
            int x2 = Convert.ToInt32(ln_line.Finish.X - 2);
            int y2 = Convert.ToInt32(ln_line.Finish.Y - 2);

            Rectangle st_rec = new Rectangle(x1, y1, 4, 4);
            Rectangle fin_rec = new Rectangle(x2, y2, 4, 4);

            using (SolidBrush sb = new SolidBrush(Color.Blue))
            {
               g.FillRectangle(sb, st_rec);
               g.FillRectangle(sb, fin_rec);
            }
         }
      }
      public override Element CopyElement()
      {
          var create = new Line();
          create.CopyElement( this );
          return create;
      }
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         using (Pen pn = new Pen(this.ElementColor, ln_thickness))
         {
            pn.DashStyle = this.ln_style;
            g.DrawLine(pn, ln_line.Start, ln_line.Finish);
         }

         DrawSelected(g);
      }
      /// <summary>
      /// Коллизия попадания мыши по элементу
      /// </summary>
      /// <param name="_pnt">курсор мыши(X,Y)</param>
      /// <returns>true если курсор на елементе</returns>
      public override bool Collision(Rectangle _pnt)
      {
         Point _pnt1 = new Point(_pnt.X, _pnt.Y);

         if (CheckAllPointsinRect(_pnt))
            return true;

         if (CheckStartFinPoints(_pnt1))
            return false;
         if (CheckHorisontalPoints(_pnt1, ln_line.Start, ln_line.Finish))
            return true;
         if (CheckVerticalPoints(_pnt1, ln_line.Start, ln_line.Finish))
            return true;

         return (this.Collinear(_pnt1, ln_line.Start, ln_line.Finish));
      }
      /// <summary>
      /// Коллизия попадания курсора мыши
      /// в область изменения
      /// </summary>
      /// <param name="pnt">курсор мыши(X,Y)</param>
      /// <returns>true если курсор попал в обасть для изменения размера</returns>
      public override bool ResizeCollision(Point pnt)
      {
         bool flag;
         this.sel_point = SelectPoint.None;

         if (ln_mod == LineStatus.Add)
            return false;

         int x1, y1, x2, y2, w, h;
         x1 = Convert.ToInt32((ln_line.Start.X - 2) * Scale.X);
         y1 = Convert.ToInt32((ln_line.Start.Y - 2) * Scale.Y);
         x2 = Convert.ToInt32((ln_line.Finish.X - 2) * Scale.X);
         y2 = Convert.ToInt32((ln_line.Finish.Y - 2) * Scale.Y);
         w = Convert.ToInt32(4 * Scale.X);
         h = Convert.ToInt32(4 * Scale.Y);

         Rectangle rec = new Rectangle(pnt.X - 1, pnt.Y - 1, 2, 2);
         Rectangle resize_rec = new Rectangle(x1, y1, w, h);
         flag = resize_rec.Contains(rec);

         if (flag)
         {
            sel_point = SelectPoint.Start;
         }
         else
         {
            resize_rec = new Rectangle(x2, y2, 4, 4);
            flag = resize_rec.Contains(rec);
            if (flag)
            {
               sel_point = SelectPoint.Finish;
            }
         }

         return flag;
      }
      /// <summary>
      /// Копирование элемента
      /// </summary>
      /// <param name="_original">Элемент на основе которого делается копия</param>
      public override void CopyElement(Element _original)
      {
          base.CopyElement(_original);

         Line tmp = (Line)_original;

         this.Scale = tmp.Scale;
         this.ln_thickness = tmp.ln_thickness;
         this.ln_mod = tmp.ln_mod;

         this.ln_style = tmp.ln_style;
         
         this.IsDragged = false;
         this.IsSelected = false;

         //this.ln_line.Start = new PointF(tmp.ln_line.Start.X + 10, tmp.ln_line.Start.Y + 10);
         //this.ln_line.Finish = new PointF(tmp.ln_line.Finish.X + 10, tmp.ln_line.Finish.Y + 10);
         this.ln_line.Start = new PointF(tmp.ln_line.Start.X, tmp.ln_line.Start.Y);
         this.ln_line.Finish = new PointF(tmp.ln_line.Finish.X, tmp.ln_line.Finish.Y);
      }
      /// <summary>
      /// Задать величину смещения
      /// относительно мыши
      /// </summary>
      public override void MouseOffSet(Point _pnt)
      {
         int x = Convert.ToInt32(ln_line.Start.X * Scale.X - _pnt.X);
         int y = Convert.ToInt32(ln_line.Start.Y * Scale.Y - _pnt.Y);
         elem_mouseoffset = new Point(x, y);
      }
      /// <summary>
      /// Конвертация под нужный размер окна
      /// </summary>
      /// <param name="_factor">значение для окна</param>
      public override void ConverttoNewSize(float _factor_x, float _factor_y)
      {
         float x1 = ln_line.Start.X * _factor_x;
         float y1 = ln_line.Start.Y * _factor_y;
         float x2 = ln_line.Finish.X * _factor_x;
         float y2 = ln_line.Finish.Y * _factor_y;

         ln_line.Start = new PointF(x1, y1);
         ln_line.Finish = new PointF(x2, y2);
      }

      public override void MoveElementtoShift(int _shift_x, int _shift_y)
      {
         this.ln_line.Start = new PointF(Convert.ToSingle(this.ln_line.Start.X + _shift_x),
                                         Convert.ToSingle(this.ln_line.Start.Y + _shift_y));
         this.ln_line.Finish = new PointF(Convert.ToSingle(this.ln_line.Finish.X + _shift_x),
                                          Convert.ToSingle(this.ln_line.Finish.Y + _shift_y));
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить статус закончиности линии
      /// </summary>
      public Boolean CloseLine
      {
         get
         {
            if (ln_mod == LineStatus.Close)
               return true;
            else
               return false;
         }
      }
      /// <summary>
      /// Стиль линии
      /// </summary>
      public DashStyle LineStyle
      {
         get { return ln_style; }
         set { ln_style = value; }
      }
      /// <summary>
      /// Получить или задать толщину линии
      /// </summary>
      public int Thickness
      {
         get { return ln_thickness; }
         set { ln_thickness = value; }
      }
      #endregion
   }
}
