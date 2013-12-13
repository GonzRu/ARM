using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Structure;

namespace LibraryElements
{
   /// <summary>
   /// ����� ������
   /// </summary>
   public abstract class Figure : Element
   {
      #region Class Parameters
      /// <summary>
      /// �������� ������� � ������ ������
      /// </summary>
      protected Rectangle elem_rec;
      /// <summary>
      /// ����������� ������ ������������ �������� �� X (const)
      /// </summary>
      private const int MinSizeElementX = 10;
      /// <summary>
      /// ����������� ������ ������������ �������� �� Y (const)
      /// </summary>
      private const int MinSizeElementY = 10;
      /// <summary>
      /// ������������ ������ ������������ �������� �� X (not const)
      /// </summary>
      private /*readonly*/ int MaxSizeElementX;
      /// <summary>
      /// ������������ ������ ������������ �������� �� Y (not const)
      /// </summary>
      private /*readonly*/ int MaxSizeElementY;
      #endregion

      #region Class Methods
      public Figure() : this(40, 40) { }
      protected Figure(int _maxsizeX, int _maxsizeY)
      {
         ElementModel = Model.Static;
         ElementName = "Figure";
         IsDragged = false;
         IsModify = false;
         IsSelected = false;
         ElementColor = Color.Black;
         Scale = new PointF(1.0f, 1.0f);
         Level = 1;
         elem_rec = new Rectangle(0, 0, 30, 30);

         MaxSizeElementX = _maxsizeX;
         MaxSizeElementY = _maxsizeY;
      }
      /// <summary>
      /// ������ ��������� ������ ������ ������������ �������� ���������
      /// </summary>
      protected Point GetFigureCenter()
      {
         return new Point(elem_rec.X + elem_rec.Width / 2, elem_rec.Y + elem_rec.Height / 2);
      }
      /// <summary>
      /// �������� ����
      /// </summary>
      /// <param name="_color">����</param>
      protected Pen CreateViewPenElement(Color _color)
      {
         return new Pen(_color);
      }
      /// <summary>
      /// �������� ����
      /// </summary>
      /// <param name="_color">����</param>
      /// <param name="_size">������</param>
      protected Pen CreateViewPenElement(Color _color, int _size)
      {
         if (_size < 1)
            return CreateViewPenElement( _color );
         else
            return new Pen(_color, _size);
      }
      /// <summary>
      /// �������� �����
      /// </summary>
      /// <param name="_color">����</param>
      protected SolidBrush CreateViewBrushElement(Color _color)
      {
         return new SolidBrush(_color);
      }
      /// <summary>
      /// ����� ����������� �������������
      /// �� ���������� �����������
      /// </summary>
      protected void NoImage(Graphics g)
      {
         int x1, y1, x2, y2;

         var pnts = GetPoints();
         var pn = new Pen(Color.Red, 2);
         x1 = pnts[0].X;
         y1 = pnts[0].Y;
         x2 = pnts[2].X;
         y2 = pnts[2].Y;
         g.DrawLine(pn, x1, y1, x2, y2);

         x1 = pnts[1].X;
         y1 = pnts[1].Y;
         x2 = pnts[3].X;
         y2 = pnts[3].Y;
         g.DrawLine(pn, x1, y1, x2, y2);

         pn = new Pen(Color.Black, 2);
         x1 = pnts[0].X;
         y1 = pnts[0].Y;
         x2 = pnts[1].X;
         y2 = pnts[1].Y;
         g.DrawLine(pn, x1, y1, x2, y2);
         x1 = pnts[1].X;
         y1 = pnts[1].Y;
         x2 = pnts[2].X;
         y2 = pnts[2].Y;
         g.DrawLine(pn, x1, y1, x2, y2);
         x1 = pnts[2].X;
         y1 = pnts[2].Y;
         x2 = pnts[3].X;
         y2 = pnts[3].Y;
         g.DrawLine(pn, x1, y1, x2, y2);
         x1 = pnts[3].X;
         y1 = pnts[3].Y;
         x2 = pnts[0].X;
         y2 = pnts[0].Y;
         g.DrawLine(pn, x1, y1, x2, y2);

         pn.Dispose();
      }
      /// <summary>
      /// ������ ����� � ������ �������� �� ����
      /// </summary>
      /// <returns>���������� �����</returns>
      private Point[] GetPoints()
      {
         int pnt_x, pnt_y;
         Point ul, ur, dl, dr; //���������� ����� � ������ ��������
         PointF center = this.GetFigureCenter();
         
         //���� ����������
         var rectangle = MathMethods.GetDegrees(Math.Atan2(elem_rec.Height / 2, elem_rec.Width / 2));

         //��������� ���������� �� ������ ������� ���� �� ������
         pnt_x = elem_rec.Right - Convert.ToInt32(center.X);
         pnt_y = elem_rec.Bottom - Convert.ToInt32(center.Y);
         var tmp = new Point(pnt_x, pnt_y);
         var radius = MathMethods.GetRadius(tmp.X, tmp.Y);

         var rad = MathMethods.GetRadians(rectangle + 180/* + this.AngleRotate*/);
         pnt_x = MathMethods.GetCoordX(radius, rad) + Convert.ToInt32(center.X);
         pnt_y = MathMethods.GetCoordY(radius, rad) + Convert.ToInt32(center.Y);
         ul = new Point(Convert.ToInt32(pnt_x), Convert.ToInt32(pnt_y));

         rad = MathMethods.GetRadians(360 - rectangle/* + this.AngleRotate*/);
         pnt_x = MathMethods.GetCoordX(radius, rad) + Convert.ToInt32(center.X);
         pnt_y = MathMethods.GetCoordY(radius, rad) + Convert.ToInt32(center.Y);
         ur = new Point(Convert.ToInt32(pnt_x), Convert.ToInt32(pnt_y));


         rad = MathMethods.GetRadians(180 - rectangle/* + this.AngleRotate*/);
         pnt_x = MathMethods.GetCoordX(radius, rad) + Convert.ToInt32(center.X);
         pnt_y = MathMethods.GetCoordY(radius, rad) + Convert.ToInt32(center.Y);
         dl = new Point(Convert.ToInt32(pnt_x), Convert.ToInt32(pnt_y));


         rad = MathMethods.GetRadians(rectangle/* + this.AngleRotate*/);
         pnt_x = MathMethods.GetCoordX(radius, rad) + Convert.ToInt32(center.X);
         pnt_y = MathMethods.GetCoordY(radius, rad) + Convert.ToInt32(center.Y);
         dr = new Point(Convert.ToInt32(pnt_x), Convert.ToInt32(pnt_y));

         Point[] pnts = { ul, ur, dr, dl };
         return pnts;
      }
      #endregion

      #region Figure Methods
      /// <summary>
      /// �������� ������� � ������ ��������
      /// </summary>
      public Rectangle GetPosition()
      {
         return this.elem_rec;
      }
      /// <summary>
      /// ������� ��������� ��������
      /// </summary>
      /// <param name="_pnt">���������� ������</param>
      public void SetPosition(Point _pnt)
      {
         if (IsSelected)
         {
            elem_rec.X = Convert.ToInt32((_pnt.X / Scale.X + elem_mouseoffset.X / Scale.X));
            elem_rec.Y = Convert.ToInt32((_pnt.Y / Scale.Y + elem_mouseoffset.Y / Scale.Y));
         }
         else
         {
            elem_rec.X = Convert.ToInt32(_pnt.X / Scale.X);
            elem_rec.Y = Convert.ToInt32(_pnt.Y / Scale.Y);
         }
      }
      /// <summary>
      /// ������� ������� ��������
      /// </summary>
      /// <param name="_size">������ ������</param>
      public void SetSize(Size _size)
      {
         if (_size.Width >= MinSizeElementX && _size.Width <= MaxSizeElementX)
            elem_rec.Width = Convert.ToInt32(_size.Width * Scale.X);
         if (_size.Height >= MinSizeElementY && _size.Height <= MaxSizeElementY)
            elem_rec.Height = Convert.ToInt32(_size.Height * Scale.Y);
      }
      /// <summary>
      /// ����� �������� �� ����
      /// </summary>
      public void ResetMouseOffset()
      {
         elem_mouseoffset = new Point(0, 0);
      }
      /// <summary>
      /// ��������� ����� ������������ ������ ��������
      /// </summary>
      /// <param name="_maxsize">������������ ������</param>
      protected void SetMaxSize(Size _maxsize)
      {
         //��������� �� �������� �� ������������ ������ ������ ������������
         if (_maxsize.Width > MinSizeElementX)
         {
            this.MaxSizeElementX = _maxsize.Width;
            if (elem_rec.Width > _maxsize.Width)
               elem_rec.Width = _maxsize.Width;
         }
         if (_maxsize.Height > MinSizeElementY)
         {
            this.MaxSizeElementY = _maxsize.Height;
            if (elem_rec.Height > _maxsize.Height)
               elem_rec.Height = _maxsize.Height;
         }
      }
      #endregion

      #region Properties
      /// <summary>
      /// �������� ������������ ������ �������� �� X
      /// </summary>
      public int MaxSizeX
      {
         get { return MaxSizeElementX; }
      }
      /// <summary>
      /// �������� ������������ ������ �������� �� Y
      /// </summary>
      public int MaxSizeY
      {
         get { return MaxSizeElementY; }
      }
      /// <summary>
      /// �������� ����������� ������ �������� �� X
      /// </summary>
      public int MinSizeX
      {
         get { return MinSizeElementX; }
      }
      /// <summary>
      /// �������� ����������� ������ �������� �� Y
      /// </summary>
      public int MinSizeY
      {
         get { return MinSizeElementY; }
      }
      #endregion

      #region Override Methods
      /// <summary>
      /// ����� ��������� ��������� ��������
      /// </summary>
      protected override void DrawSelected(Graphics _g)
      {
         int _x1 = 0, _y1 = 0, _x2 = 0, _y2 = 0;
         if (IsSelected)
         {
            var selrect = elem_rec;

            var pn = new Pen(Color.Red, 2);
            var sb = new SolidBrush(Color.Blue);
            _g.DrawRectangle(pn, selrect);

            _g.FillRectangle(sb, selrect.X - 2, selrect.Y - 2, 5, 5);
            _g.FillRectangle(sb, selrect.Right - 2, selrect.Y - 2, 5, 5);
            _g.FillRectangle(sb, selrect.X - 2, selrect.Bottom - 2, 5, 5);

            pn = new Pen(Color.Blue, 1);
            _x1 = selrect.Right - 5;
            _y1 = selrect.Bottom;
            _x2 = selrect.Right;
            _y2 = selrect.Bottom - 5;
            _g.DrawLine(pn, _x1, _y1, _x2, _y2);

            _x1 -= 3; _y2 -= 3;
            _g.DrawLine(pn, _x1, _y1, _x2, _y2);

            _x1 -= 3; _y2 -= 3;
            _g.DrawLine(pn, _x1, _y1, _x2, _y2);

            pn.Dispose();
            sb.Dispose();
         }
      }
      /// <summary>
      /// �������� ��������� ���� �� ��������
      /// </summary>
      /// <param name="_pnt">������ ����(X,Y)</param>
      /// <returns>true ���� ������ �� ��������</returns>
      public override bool Collision(Rectangle _pnt)
      {
         bool flag;
         var pnts = this.GetPoints();

         //���������� ����� � ������ ���������������
         for (int i = 0; i < pnts.Count(); i++ )
         {
            pnts[i].X = Convert.ToInt32(pnts[i].X * Scale.X);
            pnts[i].Y = Convert.ToInt32(pnts[i].Y * Scale.Y);
         }

         var grpath = new GraphicsPath();
         grpath.AddPolygon(pnts);
         flag = grpath.IsVisible(_pnt.Location);
         grpath.Dispose();

         if (!flag)
         {
            bool flag2 = false;
            foreach (Point pnt in pnts)
            {
               if (_pnt.Contains(pnt))
                  flag2 = true;
               else
               {
                  flag2 = false;
                  break;
               }
            }

            if (!flag && flag2)
               flag = true;
         }

         return flag;
      }
      /// <summary>
      /// �������� ��������� ������� ����
      /// � ������� ���������
      /// </summary>
      /// <param name="pnt">������ ����(X,Y)</param>
      /// <returns>true ���� ������ ����� � ������ ��� ��������� �������</returns>
      public override bool ResizeCollision(Point pnt)
      {
         var collrect = elem_rec;

         var x1 = Convert.ToInt32((collrect.Right - 9) * Scale.X);
         var y1 = Convert.ToInt32((collrect.Bottom - 9) * Scale.Y);
         var w = Convert.ToInt32(9 * Scale.X);
         var h = Convert.ToInt32(9 * Scale.Y);

         var pntRec = new Rectangle(pnt.X, pnt.Y, 2, 2);
         var resizeRec = new Rectangle(x1, y1, w, h);
         var flag = resizeRec.Contains(pntRec);
         return flag;
      }
      /// <summary>
      /// ����������� ��������
      /// </summary>
      /// <param name="_original">������� �� ������ �������� �������� �����</param>
      public override void CopyElement(Element _original)
      {
         base.CopyElement(_original);

         var tmp = (Figure)_original;

         var center = tmp.GetFigureCenter();
         //this.elem_rec = new Rectangle(center.X, center.Y, tmp.elem_rec.Width, tmp.elem_rec.Height);
         this.elem_rec = new Rectangle(tmp.elem_rec.X, tmp.elem_rec.Y, tmp.elem_rec.Width, tmp.elem_rec.Height);

         this.MaxSizeElementX = tmp.MaxSizeElementX;
         this.MaxSizeElementY = tmp.MaxSizeElementY;
      }
      /// <summary>
      /// ������ �������� ��������
      /// ������������ ����
      /// </summary>
      public sealed override void MouseOffSet(Point _pnt)
      {
         var elpos = new Point(elem_rec.X, elem_rec.Y);
         elem_mouseoffset = new Point((int)(elpos.X * Scale.X - _pnt.X), (int)(elpos.Y * Scale.Y - _pnt.Y));
      }
      /// <summary>
      /// ����������� ��� ������ ������ ����
      /// </summary>
      /// <param name="_factor">�������� ��� ����</param>
      public override void ConverttoNewSize(float _factor_x, float _factor_y)
      {
         int x = Convert.ToInt32(elem_rec.X * _factor_x);
         int y = Convert.ToInt32(elem_rec.Y * _factor_y);
         int w = Convert.ToInt32(elem_rec.Width * _factor_x);
         int h = Convert.ToInt32(elem_rec.Height * _factor_y);

         if (w > MaxSizeElementX) w = MaxSizeElementX;
         if (h > MaxSizeElementY) h = MaxSizeElementY;

         elem_rec = new Rectangle(x, y, w, h);
      }
      public sealed override void MoveElementtoShift(int _shift_x, int _shift_y)
      {
         this.elem_rec.X += _shift_x;
         this.elem_rec.Y += _shift_y;
      }
      #endregion    
   }
}
