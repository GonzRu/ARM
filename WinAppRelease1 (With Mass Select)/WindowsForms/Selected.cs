using System.Drawing;

namespace WindowsForms
{
   /// <summary>
   /// Выделение объектов
   /// </summary>
   class Selected
   {
      #region Parameters
      private Pen sel_pn;
      private SolidBrush sel_sb;
      private Rectangle sel_rec;
      private Point sel_down;
      private Point sel_move;
      private bool sel_active;
      #endregion

      #region Class Methods
      public Selected()
      {
         sel_sb = new SolidBrush(Color.FromArgb(130, 147, 201, 255));
         sel_pn = new Pen(Color.FromArgb(0, 105, 210), 1.7f);
         sel_rec = new Rectangle();
         sel_down = new Point();
         sel_move = new Point();
         sel_active = false;        
      }
      /// <summary>
      /// Определение нового размера прямоугольника выделения
      /// </summary>
      private void InitNewRect()
      {
         if (sel_move.X > sel_down.X)
         {
            sel_rec.X = sel_down.X;
            sel_rec.Width = sel_move.X - sel_down.X;
         }
         else
         {
            sel_rec.X = sel_move.X;
            sel_rec.Width = sel_down.X - sel_move.X;
         }
         if (sel_move.Y > sel_down.Y)
         {
            sel_rec.Y = sel_down.Y;
            sel_rec.Height = sel_move.Y - sel_down.Y;
         }
         else
         {
            sel_rec.Y = sel_move.Y;
            sel_rec.Height = sel_down.Y - sel_move.Y;
         }
      }
      /// <summary>
      /// Отрисовка выделения
      /// </summary>
      public void DrawSelectRectangle(Graphics g)
      {
         if (sel_active)
         {
            var ln1 = new Point(sel_down.X, sel_down.Y);
            var ln2 = new Point(sel_move.X, sel_down.Y);
            var ln3 = new Point(sel_move.X, sel_move.Y);
            var ln4 = new Point(sel_down.X, sel_move.Y);            
            Point[] pnts = {ln1,ln2,ln3,ln4};

            g.FillPolygon(sel_sb, pnts);
            g.DrawPolygon(sel_pn, pnts);
         }
      }
      /// <summary>
      /// Установить координаты нажатой мыши
      /// </summary>
      /// <param name="_pnt">точка</param>
      public void SetMouseDown(Point _pnt)
      {
         sel_rec.X = _pnt.X;
         sel_rec.Y = _pnt.Y;
         sel_rec.Width = 2;
         sel_rec.Height = 2;
         sel_down = _pnt;
         sel_move = _pnt;
      }
      /// <summary>
      /// Установить координаты мыши при движении
      /// </summary>
      /// <param name="_pnt">точка</param>
      public void SetMouseMove(Point _pnt)
      {
         if (sel_active)
         {
            sel_move = _pnt;
            InitNewRect();
         }
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить или задать статус активности выделения
      /// </summary>
      public bool Active
      {
         get { return sel_active; }
         set { sel_active = value; }
      }
      /// <summary>
      /// Получить выделенную область
      /// </summary>      
      public Rectangle Area
      {
         get { return sel_rec; }
      }
      #endregion
   }
}
