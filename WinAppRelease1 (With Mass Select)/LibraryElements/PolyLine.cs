using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using Structure;

namespace LibraryElements
{
   /// <summary>
   /// Класс ломаная линия
   /// </summary>
   public class PolyLine : Line
   {
      #region ClassParameters
      private readonly List<LinePoints> listLines;
      private int selline;
      #endregion

      #region Class Methods
      public PolyLine()
      {
         this.listLines = new List<LinePoints>();
         selline = 0;
      }
      /// <summary>
      /// Замена линии при движении точки за мышью
      /// </summary>
      /// <param name="_pnt">координаты мыши</param>
      private void SwapLine(PointF _pnt)
      {
         LinePoints ln_next;
         ln_line = this.listLines[selline];

         if (sel_point == SelectPoint.Start)
         {
            ln_line.Start = _pnt;
            this.listLines.RemoveAt(selline);
            this.listLines.Insert(selline, ln_line);
            return;
         }
         if (sel_point == SelectPoint.Finish)
         {
            ln_line.Finish = _pnt;
            this.listLines.RemoveAt(selline);
            this.listLines.Insert(selline, ln_line);
            return;
         }


         if (sel_point == SelectPoint.StartIntermediate)
         {
            if (selline - 1 >= 0)
            {
               ln_next = this.listLines[selline - 1];
               ln_next.Finish = _pnt;
               this.listLines.RemoveAt(selline-1);
               this.listLines.Insert(selline-1, ln_next);
            }
            ln_line.Start = _pnt;
            this.listLines.RemoveAt(selline);
            this.listLines.Insert(selline, ln_line);
            return;
         }
         if (sel_point == SelectPoint.FinishIntermediate)
         {
            if (selline + 1 <= this.listLines.Count - 1)
            {
               ln_next = this.listLines[selline + 1];
               ln_next.Start = _pnt;
               this.listLines.RemoveAt(selline+1);
               this.listLines.Insert(selline+1, ln_next);
            }
            ln_line.Finish = _pnt;
            this.listLines.RemoveAt(selline);
            this.listLines.Insert(selline, ln_line);
            return;
         }
      }
      /// <summary>
      /// Прекращение добавления новых точек к рисуемой линии
      /// </summary>
      public void ClosePoly()
      {
         ln_mod = LineStatus.Close;

         if (this.listLines.Count >= 1)
         {
            ln_line.Start = this.listLines[this.listLines.Count - 1].Start;
            ln_line.Finish = this.listLines[this.listLines.Count - 1].Finish;
         }
         else
         {
            ln_mod = LineStatus.Add;
         }
      }
      public List<LinePoints> IgetAllPoints()
      {
          return this.listLines;
      }
      #endregion

      #region Override Methods
      public override String GetPoints()
      {
         string str = null;

         for (int i = 0; i < this.listLines.Count; i++)
         {
            if (i == 0)
               str = "M" + this.listLines[i].Start.X.ToString() + ":" + this.listLines[i].Start.Y.ToString();
            
            str += " L" + this.listLines[i].Finish.X.ToString() + ":" + this.listLines[i].Finish.Y.ToString();

            if (i == this.listLines.Count - 1)
               str += " Z";
         }

         return str;         
      }
      public override void SetReadPoints(string _parse_str)
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
                  {
                     obj = Convert.ChangeType(str, typeof(float));
                     point.Y = (float)obj;
                     str = null;
                     this.AddPoint(point);
                     this.ClosePoly();
                  }
                  break;
               case 'L':
                  {
                     obj = Convert.ChangeType(str, typeof(float));
                     point.Y = (float)obj;
                     str = null;
                     this.AddPoint(point);
                  }
                  break;
               case ':': obj = Convert.ChangeType(str, typeof(float)); point.X = (float)obj; str = null; break;
               default: str += symbol; break;
            }//switch.
         }//for
      }
      
      /// <summary>
      /// Установить флаг для точки следовать за мышью 
      /// </summary>
      public override void SetPointtoMouse()
      {
         base.SetPointtoMouse();

         if (sel_point == SelectPoint.StartIntermediate || sel_point == SelectPoint.FinishIntermediate)
            ln_mod = LineStatus.Intermediate;
      }
      /// <summary>
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         if (!CloseLine)
            base.DrawElement(g);

         if (this.listLines.Count > 0)
         {
            using (GraphicsPath gpath = new GraphicsPath())
            {
               for (int i = 0; i < this.listLines.Count; i++)
                  gpath.AddLine(this.listLines[i].Start, this.listLines[i].Finish);
               
               using (Pen pn = new Pen(this.ElementColor, ln_thickness))
               {
                  pn.DashStyle = this.ln_style;
                  g.DrawPath(pn, gpath); 
               }
            }
         }

         DrawSelected(g);
      }
      /// <summary>
      /// Метод прикрепления точки линии курсору
      /// </summary>
      /// <param name="_pnt">Координаты XY</param>
      public override bool PointOnMouse(Point _pnt)
      {
         switch (ln_mod)
         {
            case LineStatus.Add: 
               ln_line.Finish = _pnt;
               return true;//break;
            case LineStatus.PointStart:
               {
                  this.SwapLine(_pnt);
               }
               return true;//break;
            case LineStatus.PointFinish:
               {
                  this.SwapLine(_pnt);
               }
               return true;//break;
            case LineStatus.Intermediate:
               {
                  this.SwapLine(_pnt);
               }
               return true;//break;
            default: return false;//break;
         }
      }
      /// <summary>
      /// координаты новой точки
      /// </summary>
      /// <param name="_pos">Координаты XY</param>
      public override void AddPoint(PointF _pnt)
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
                  this.listLines.Add(ln_line);

                  ln_line.Start = _pnt;
               }
               break;
            case LineStatus.PointStart:
               {
                  this.SwapLine(_pnt);
                  ln_mod = LineStatus.Close;
               }
               break;
            case LineStatus.PointFinish:
               {
                  this.SwapLine(_pnt);
                  ln_mod = LineStatus.Close;
               }
               break;
            case LineStatus.Intermediate:
               {
                  this.SwapLine(_pnt);
                  ln_mod = LineStatus.Close;
               }
               break;
            default: break;
         }
      }
      /// <summary>
      /// Коллизия попадания мыши по элементу
      /// </summary>
      /// <param name="_pnt">курсор мыши(X,Y)</param>
      /// <returns>true если курсор на елементе</returns>
      public override bool Collision(Rectangle _pnt)
      {
         LinePoints tmp = ln_line;

         if (CheckAllPointsinRect(_pnt))
            return true;

         for (int i = 0; i < this.listLines.Count; i++)
         {
            ln_line = this.listLines[i];

            if (base.Collision(_pnt))
               return true;
         }
         ln_line = tmp;
         return false;
      }
      /// <summary>
      /// Задать величину смещения
      /// относительно мыши
      /// </summary>
      public override void MouseOffSet(Point _pnt)
      {
         if (this.listLines.Count != 0)
         {
            int x = Convert.ToInt32(this.listLines[0].Start.X * Scale.X - _pnt.X);
            int y = Convert.ToInt32(this.listLines[0].Start.Y * Scale.Y - _pnt.Y);
            elem_mouseoffset = new Point(x, y);
         }
      }
      /// <summary>
      /// Смещение линии за мышью
      /// </summary>
      /// <param name="_pnt">Координаты XY</param>
      public override void MoveLine(Point _pnt)
      {
         LinePoints tmp;
         int sx=0, sy=0;
         int x, y, x1, y1, x2, y2, w, h;

         if (IsSelected)
         {
            x = _pnt.X;
            y = _pnt.Y;

            for (int i = 0; i < this.listLines.Count; i++)
            {
               tmp = this.listLines[i];
               w = Convert.ToInt32(tmp.Finish.X - tmp.Start.X);
               h = Convert.ToInt32(tmp.Finish.Y - tmp.Start.Y);

               if (i == 0)
               {
                  x1 = Convert.ToInt32((x / Scale.X + elem_mouseoffset.X / Scale.X));
                  y1 = Convert.ToInt32((y / Scale.Y + elem_mouseoffset.Y / Scale.Y));
                  x2 = Convert.ToInt32(x1 + w);
                  y2 = Convert.ToInt32(y1 + h);
                  sx = x2;
                  sy = y2;
               }
               else
               {
                  x1 = sx;
                  y1 = sy;
                  x2 = Convert.ToInt32(x1 + w);
                  y2 = Convert.ToInt32(y1 + h);
                  sx = x2;
                  sy = y2;
               }
               if (ln_line.Start == tmp.Start && ln_line.Finish == tmp.Finish)
               {
                  ln_line.Start = new PointF(x1, y1);
                  ln_line.Finish = new PointF(x2, y2);
               }

               tmp.Start = new PointF(x1, y1);
               tmp.Finish = new PointF(x2, y2);
               this.listLines.RemoveAt(i);
               this.listLines.Insert(i, tmp);
            }//for
         }//if (isSelected)
      }
      /// <summary>
      /// Коллизия попадания курсора мыши
      /// в область изменения
      /// </summary>
      /// <param name="pnt">курсор мыши(X,Y)</param>
      /// <returns>true если курсор попал в обасть для изменения размера</returns>
      public override bool ResizeCollision(Point pnt)
      {
         bool flag = false;
         this.sel_point = SelectPoint.None;
         int x1, y1, x2, y2, w, h;         

         if (ln_mod == LineStatus.Add)
            return false;


         for (int i = 0; i < this.listLines.Count; i++)
         {
            x1 = Convert.ToInt32((this.listLines[i].Start.X - 2) * Scale.X);
            y1 = Convert.ToInt32((this.listLines[i].Start.Y - 2) * Scale.Y);
            x2 = Convert.ToInt32((this.listLines[i].Finish.X - 2) * Scale.X);
            y2 = Convert.ToInt32((this.listLines[i].Finish.Y - 2) * Scale.Y);
            w = Convert.ToInt32(4 * Scale.X);
            h = Convert.ToInt32(4 * Scale.Y);

            Rectangle rec = new Rectangle(pnt.X - 1, pnt.Y - 1, 2, 2);
            Rectangle resize_rec = new Rectangle(x1, y1, w, h);
            flag = resize_rec.Contains(rec);
         
            if (flag)
            {
               if (i == 0)
               {
                  sel_point = SelectPoint.Start;
                  selline = i;
               }
               else
               {
                  if (ln_line.Start == this.listLines[i].Start && ln_line.Finish == this.listLines[i].Finish)
                  {
                     sel_point = SelectPoint.StartIntermediate;
                     selline = i;
                  }
               }
            }
            else
            {
               resize_rec = new Rectangle(x2, y2, 4, 4);
               flag = resize_rec.Contains(rec);
               if (flag)
               {
                  if (i == this.listLines.Count - 1)
                  {
                     sel_point = SelectPoint.Finish;
                     selline = i;
                  }
                  else
                  {
                     if (ln_line.Start == this.listLines[i].Start && ln_line.Finish == this.listLines[i].Finish)
                     {
                        sel_point = SelectPoint.FinishIntermediate;
                        selline = i;
                     }
                  }
               }
            }//if_else
         }//for

         if (sel_point == SelectPoint.None)
            return false;
         else
            return true;
      }
      public override Element CopyElement()
      {
          var create = new PolyLine();
          create.CopyElement( this );
          return create;
      }
      /// <summary>
      /// Копирование элемента
      /// </summary>
      /// <param name="_original">Элемент на основе которого делается копия</param>
      public override void CopyElement(Element _original)
      {
          base.CopyElement(_original);

         PolyLine tmp = (PolyLine)_original;
         LinePoints tmpln = new LinePoints();

         this.Scale = tmp.Scale;
         this.ln_thickness = tmp.ln_thickness;
         this.ln_mod = tmp.ln_mod;

         this.ln_style = tmp.ln_style;

         this.IsDragged = false;
         this.IsSelected = false;
         
         for (int i = 0; i < tmp.listLines.Count; i++)
         {            
            //tmpln.Start = new PointF(tmp.listLines[i].Start.X + 10, tmp.listLines[i].Start.Y + 10);
            //tmpln.Finish = new PointF(tmp.listLines[i].Finish.X + 10, tmp.listLines[i].Finish.Y + 10);
            tmpln.Start = new PointF(tmp.listLines[i].Start.X, tmp.listLines[i].Start.Y);
            tmpln.Finish = new PointF(tmp.listLines[i].Finish.X, tmp.listLines[i].Finish.Y);
            this.listLines.Add(tmpln);
         }
      }
      /// <summary>
      /// Конвертация под нужный размер окна
      /// </summary>
      /// <param name="_factor_x">значение для окна по X</param>
      /// <param name="_factor_y">значение для окна по Y</param>
      public override void ConverttoNewSize(float _factor_x, float _factor_y)
      {
         float x1, y1, x2, y2;
         LinePoints tmp = new LinePoints();

         base.ConverttoNewSize(_factor_x, _factor_y);

         for(int i=0; i < this.listLines.Count; i++)
         {
            x1 = this.listLines[i].Start.X * _factor_x;
            y1 = this.listLines[i].Start.Y * _factor_y;
            x2 = this.listLines[i].Finish.X * _factor_x;
            y2 = this.listLines[i].Finish.Y * _factor_y;

            tmp.Start = new PointF(x1,y1);
            tmp.Finish = new PointF(x2,y2);

            this.listLines.RemoveAt(i);
            this.listLines.Insert(i, tmp);
         }
      }

      public override void MoveElementtoShift(int _shift_x, int _shift_y)
      {
         float x1, y1, x2, y2;
         LinePoints tmp = new LinePoints();

         for (int i = 0; i < this.listLines.Count; i++)
         {
            x1 = this.listLines[i].Start.X + _shift_x;
            y1 = this.listLines[i].Start.Y + _shift_y;
            x2 = this.listLines[i].Finish.X + _shift_x;
            y2 = this.listLines[i].Finish.Y + _shift_y;
            tmp.Start = new PointF(x1,y1);
            tmp.Finish = new PointF(x2,y2);
            
            this.listLines.RemoveAt(i);
            this.listLines.Insert(i, tmp);
         }
      }

      /// <summary>
      /// Метод отрисовки выделения элемента
      /// </summary>
      protected override void DrawSelected(Graphics g)
      {
         int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
         
         if (IsSelected)
         {
            foreach (LinePoints _ln in this.listLines)
            {
               x1 = Convert.ToInt32(_ln.Start.X - 2);
               y1 = Convert.ToInt32(_ln.Start.Y - 2);
               x2 = Convert.ToInt32(_ln.Finish.X - 2);
               y2 = Convert.ToInt32(_ln.Finish.Y - 2);

               Rectangle st_rec = new Rectangle(x1, y1, 4, 4);
               Rectangle fin_rec = new Rectangle(x2, y2, 4, 4);

               using (SolidBrush sb = new SolidBrush(Color.Red))
               {
                  g.FillRectangle(sb, st_rec);
                  g.FillRectangle(sb, fin_rec);
               }              
            }//foreach
         }
         base.DrawSelected(g);
      }
      /// <summary>
      /// Проверка входят ли все точки линии в прямоугольник выделения
      /// </summary>
      /// <param name="_rec">прямоугольник выделения</param>
      /// <returns>true если все точки попали в выделение</returns>
      protected override bool CheckAllPointsinRect(Rectangle _rec)
      {
         LinePoints tmp;
         Point pnt = new Point();
         bool flag = false;

         for (int i = 0; i < this.listLines.Count; i++)
         {
            tmp = this.listLines[i];

            pnt.X = Convert.ToInt32(tmp.Start.X);
            pnt.Y = Convert.ToInt32(tmp.Start.Y);
            if (_rec.Contains(pnt))
               flag = true;
            else
               flag = false;

            if (!flag) return flag;

            pnt.X = Convert.ToInt32(tmp.Finish.X);
            pnt.Y = Convert.ToInt32(tmp.Finish.Y);
            if (_rec.Contains(pnt))
               flag = true;
            else
               flag = false;

            if (!flag) return flag;            
         }
         return flag;
      }
      #endregion
   }
}