using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using LibraryElements;
using Structure;
using FileManager;

namespace WindowsForms
{
   public partial class MnemoSchemaForm : Form
   {
      #region Class Parameters
      const int shiftw = 35, shifth = 55; //смещение чтобы было видно красныю рамку
      const string str = "Постройте схему в пределах рабочей области";
      string filename;            //путь до открытого файла
      string mnenocaption;        //имя(описание) схемы
      int win_scale;              //размер рабочей области
      bool create_change;         //внесение изменений
      Selected select;            //выделение

      #region Form
      //рабочие переменные
      Point pointXY;              //позиционные координаты
      Size sizeXY;                //размер
      Size windsize;              //размер рабочей области
      BarsMenu.ToolBar orgtb;     //тулбар панель
      BarsMenu.StatusBar orgsb;   //статусбар панель
      #endregion

      #region Elements
      //работа с фигурами
      bool createlineflag;        //флаг создания линии
      List<Element> list;         //список фигур
      List<Element> selected_list;//список выделенных фигур
      List<Element> copy_list;    //список скопированных фигур
      Figure selected_elem;       //вспомогательный об'ект фигура
      Line select_line;           //вспомогательный об'ект линия
      #endregion
      #endregion

      #region Class Methods
      public MnemoSchemaForm(BarsMenu.ToolBar _orgtb, BarsMenu.StatusBar _orgsb)
      {
         InitializeComponent();
         InitElements();
         SetStyle(ControlStyles.UserPaint, true);
         SetStyle(ControlStyles.AllPaintingInWmPaint, true);
         SetStyle(ControlStyles.DoubleBuffer, true);

         orgtb = _orgtb;
         orgsb = _orgsb;
      }
      /// <summary>
      /// Действия при потепе/захвате фокуса окна-потомка
      /// </summary>
      private void Form1_GotFocus(object sender, EventArgs e)
      {
         if (this.create_change)
            orgsb.SetLabel1Change();
         else
            orgsb.SetLabel1TextDefault();

         orgsb.Label3_Text = win_scale.ToString() + "%";
      }
      /// <summary>
      /// Отрисовка размера рабочей области
      /// </summary>
      private void DrawField(Graphics g)
      {         
         Rectangle wind = new Rectangle(0, 0, windsize.Width, windsize.Height);
         StringFormat strform = new StringFormat();
         PointF pnt = new PointF();         
         
         g.DrawRectangle(new Pen(Color.Red, 1), wind);
         
         pnt.X = 5;
         pnt.Y = wind.Bottom;
         g.DrawString(str, new Font("Verdana", 8), new SolidBrush(Color.Red), pnt, strform);
         
         pnt.X = wind.Right;
         pnt.Y = 5;
         strform.FormatFlags = StringFormatFlags.DirectionVertical;
         g.DrawString(str, new Font("Verdana", 8), new SolidBrush(Color.Red), pnt, strform);
      }
      /// <summary>
      /// Инициализация переменных
      /// </summary>
      private void InitElements()
      {
         ClearObjects();
         create_change = false;
         mnenocaption = String.Empty;
         win_scale = 100;//100%


         select = new Selected();
         list = new List<Element>();
         selected_list = new List<Element>();
         copy_list = new List<Element>();
      }
      /// <summary>
      /// Очистка вспомогательных элементов
      /// </summary>
      private void ClearObjects()
      {
         createlineflag = false;
         if (selected_elem != null)
            selected_elem.IsSelected = false;
         if (select_line != null)
            select_line.IsSelected = false;
      }
      /// <summary>
      /// Установка маркера о внесении изменений
      /// </summary>
      private void SetChange()
      {
         if (create_change == false)
         {
            create_change = true;
            orgsb.SetLabel1Change();
         }
      }
      /// <summary>
      /// Проверка на выбор фигуры
      /// </summary>
      /// <returns></returns>
      private bool SelectFigure()
      {
         bool flag = true;
         switch (orgtb.MenuItem)
         {
            case SelectMenuItems.None:
               flag = false;
               break;
            case SelectMenuItems.ItemMenuSingleLine:
               flag = false;
               break;
            case SelectMenuItems.ItemMenuPolyLine:
               flag = false;
               break;
            case SelectMenuItems.ItemMenuTrunk:
               flag = false;
               break;
            default: flag = true; break;
         }
         return flag;
      }
      /// <summary>
      /// Создание элементов
      /// </summary>
      /// <param name="_pos">позиция мыши куда будет установлен элемент</param>
      private void CreateChoise(Point _pos)
      {
         ClearObjects();
         pointXY = _pos;
         sizeXY = new Size(0, 0);

         //выбор рисуемого элемента
         switch (orgtb.MenuItem)
         {
            case SelectMenuItems.ItemMenuSingleLine:
               {
                  select_line = new Line();
                  orgtb.MenuItem = SelectMenuItems.None;
                  createlineflag = true;
               }
               break;
            case SelectMenuItems.ItemMenuPolyLine:
               {
                  select_line = new PolyLine();
                  orgtb.MenuItem = SelectMenuItems.None;
                  createlineflag = true;
               }
               break;
            case SelectMenuItems.ItemMenuTrunk:
               {
                  select_line = new Trunk();
                  orgtb.MenuItem = SelectMenuItems.None;
                  createlineflag = true;
               }
               break;
            case SelectMenuItems.ItemMenuGrounding:
               {
                  if (orgtb.MenuModify == SelectMenuModify.Up)
                  {
                     selected_elem = new Ground(DrawRotate.Up);
                     selected_elem.SetPosition(pointXY);
                  }
                  if (orgtb.MenuModify == SelectMenuModify.Down)
                  {
                     selected_elem = new Ground(DrawRotate.Down);
                     selected_elem.SetPosition(pointXY);
                  }
                  if (orgtb.MenuModify == SelectMenuModify.Left)
                  {
                     selected_elem = new Ground(DrawRotate.Left);
                     selected_elem.SetPosition(pointXY);
                  }
                  if (orgtb.MenuModify == SelectMenuModify.Right)
                  {
                     selected_elem = new Ground(DrawRotate.Right);
                     selected_elem.SetPosition(pointXY);
                  }
               }
               break;
            case SelectMenuItems.ItemMenuFloorChassis:
               {
                  if (orgtb.MenuModify == SelectMenuModify.Up)
                  {
                     selected_elem = new FloorChassis(DrawRotate.Up);
                     selected_elem.SetPosition(pointXY);
                  }
                  if (orgtb.MenuModify == SelectMenuModify.Down)
                  {
                     selected_elem = new FloorChassis(DrawRotate.Down);
                     selected_elem.SetPosition(pointXY);
                  }
               }
               break;
            case SelectMenuItems.ItemMenuTriangle:
               {
                  if (orgtb.MenuModify == SelectMenuModify.Up)
                  {
                     selected_elem = new PrimTriangle(DrawRotate.Up);
                     selected_elem.SetPosition(pointXY);
                  }
                  if (orgtb.MenuModify == SelectMenuModify.Down)
                  {
                     selected_elem = new PrimTriangle(DrawRotate.Down);
                     selected_elem.SetPosition(pointXY);
                  }
               }
               break;
            case SelectMenuItems.ItemMenuArc:
               {
                  if (orgtb.MenuModify == SelectMenuModify.Up)
                  {
                     selected_elem = new PrimArc(DrawRotate.Up);
                     selected_elem.SetPosition(pointXY);
                  }
                  if (orgtb.MenuModify == SelectMenuModify.Down)
                  {
                     selected_elem = new PrimArc(DrawRotate.Down);
                     selected_elem.SetPosition(pointXY);
                  }
               }
               break;
            case SelectMenuItems.ItemMenuTrunkPoint:
               {
                  selected_elem = new TrunkPoint();
                  selected_elem.SetPosition(pointXY);
               }
               break;
            case SelectMenuItems.ItemMenuStaticElement:
               {
                  selected_elem = new StaticElement();
                  selected_elem.SetPosition(pointXY);
               }
               break;
            case SelectMenuItems.ItemMenuDinamicElement:
               {
                  selected_elem = new DynamicElement();
                  selected_elem.SetPosition(pointXY);
               }
               break;
            case SelectMenuItems.ItemMenuText:
               {
                  selected_elem = new FormText();
                  selected_elem.SetPosition(pointXY);
               }
               break;
            case SelectMenuItems.ItemMenuEllipse:
               {
                  selected_elem = new PrimEllipse();
                  selected_elem.SetPosition(pointXY);
               }
               break;
            case SelectMenuItems.ItemMenuRectangle:
               {
                  selected_elem = new PrimRectangle();
                  selected_elem.SetPosition(pointXY);
               }
               break;
            case SelectMenuItems.SchemaButton:
               {
                   selected_elem = new SchemaButton();
                   selected_elem.SetPosition(pointXY);
               }
               break;
             case SelectMenuItems.ItemMenuBlockText:
               {
                   selected_elem = new BlockText();
                   selected_elem.SetPosition(pointXY);
               }
               break;
         }
      }
      /// <summary>
      /// Сортировка элементов по уровню отображения
      /// </summary>
      private static int ListCompare(Element elem1, Element elem2)
      {
         if (elem1.Level > elem2.Level) return 0;
         else
            if (elem1.Level < elem2.Level) return -1;
            else return 1;
      }
      #endregion

      #region Form Methods
      /// <summary>
      /// Метод сохранения данных
      /// </summary>
      public void SaveMethod()
      {
         SaveProgressForm frm = new SaveProgressForm();
         frm.Show();

         SchemasStream file = new SchemasStream();
         file.SetProcessForm(frm);
         if (this.mnenocaption != String.Empty)
           file.AddDatas(ref this.list, this.windsize, this.mnenocaption);
         else
           file.AddDatas(ref this.list, this.windsize);
         file.SaveFile(this.filename);

         create_change = false;
         orgsb.SetLabel1TextDefault();

         frm.Close();
         frm.Dispose();
      }
      /// <summary>
      /// Метод сохранения данных
      /// </summary>
      /// <param name="_strname">имя и путь файла</param>
      public void SaveMethod(string _strname)
      {
         SaveProgressForm frm = new SaveProgressForm();
         frm.Show();

         SchemasStream file = new SchemasStream();
         file.SetProcessForm(frm);
         if (this.mnenocaption != String.Empty)
           file.AddDatas(ref this.list, this.windsize, this.mnenocaption);
         else
           file.AddDatas(ref this.list, this.windsize);
         file.SaveFile(_strname);

         create_change = false;
         orgsb.SetLabel1TextDefault();
         this.filename = _strname;

         frm.Close();
         frm.Dispose();
      }
      /// <summary>
      /// Метод чтения данных
      /// </summary>
      /// <param name="_strname">имя и путь файла</param>
      public void OpenMethod(string _strname)
      {
         int _winWidth = 640, _winHeight = 480;
         this.filename = _strname;

         Form7 frmlog = new Form7();
         OpenProgressForm frm = new OpenProgressForm();
         frm.Show();

         SchemasStream file = new SchemasStream();
         file.SetProcessForm(frm);
         file.SetErrorForm(frmlog);
         file.LoadFile(_strname);
         file.ReadDatas(ref this.list, ref _winWidth, ref _winHeight);
         mnenocaption = file.GetMnenoCaption();
         file.Dispose();

         frm.Close();
         frm.Dispose();

         if (frmlog.RecordsExist) frmlog.Show();

         this.MaximumSize = new Size(_winWidth + shiftw, _winHeight + shifth);
         this.ClientSize = new Size(_winWidth, _winHeight);
         this.windsize = new Size(_winWidth, _winHeight);

         #region действие: сортировка списка элементов
         list.Sort(ListCompare);
         #endregion
      }
      /// <summary>
      /// Экспорт данных
      /// </summary>
      /// <param name="_strname">имя и путь файла</param>
      public void ExportMethod(string _strname)
      {
      }
      /// <summary>
      /// Метод собирания схемы в единое целое
      /// </summary>
      /// <param name="_strname">имя и путь файла</param>
      public void BuildMethod(string _strname)
      {
         if (this.create_change || string.IsNullOrEmpty( this.filename ))
         {
            MessageBox.Show("Сохраните схему, прежде чем продолжить", "Внимание",
               MessageBoxButtons.OK, MessageBoxIcon.Warning);            
            return;
         }

         MessageBox.Show("Процесс собирания схемы займет какое-то время...");

         OpenProgressForm frm = new OpenProgressForm();
         frm.Show();

         List<Element> buildlst = new List<Element>();
         SchemasStream file = new SchemasStream();
         file.SetProcessForm(frm);
         file.LoadFile(this.filename);
         file.ReadDatas(ref buildlst);
         file.Dispose();
         frm.Close();
         frm.Dispose();

         foreach (Element elem in buildlst)
         {
             var tmp = elem as IDynamicParameters;
             if ( tmp != null )
                 tmp.Parameters.ExternalDescription = false;
         }

         frm = new SaveProgressForm();
         frm.Show();
         file = new SchemasStream();
         file.SetProcessForm(frm);
         file.AddDatas(ref buildlst, this.windsize, mnenocaption);
         file.SaveFile(_strname);
         frm.Close();
         frm.Dispose();

         MessageBox.Show("Процесс сборки заверщен.");
      }
      /// <summary>
      /// Предварительный просмотр схемы
      /// </summary>
      public void PreviewMethod()
      {
         if (this.filename == String.Empty)
         {
            MessageBox.Show("Перед продолжением сохраните схему", "Внимание",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
         }

         List<Element> tmplst = new List<Element>();
         string title;

         OpenProgressForm frmload = new OpenProgressForm();
         frmload.Show();

         SchemasStream file = new SchemasStream();
         file.SetProcessForm(frmload);
         file.LoadFile(this.filename);
         file.ReadDatas(ref tmplst);
         title = file.GetMnenoCaption();

         Form frm = new Form();
         frm.AutoScroll = true;
         frm.Text = (title == string.Empty) ? "Предпросмотр" : "Предпросмотр ( " + title + " )";
         frm.Size = this.Size;
         file.Dispose();

         frmload.Close();
         frmload.Dispose();

         SchemaPanel sp = new SchemaPanel(tmplst, this.windsize.Width, this.windsize.Height, this.mnenocaption, new PointF( 1.0f, 1.0f ) );
         //sp.BackColor = Color.FromArgb( 80, 80, 80 );
         sp.BackColor = Color.FromArgb( 192, 192, 192 );
         frm.Controls.Add(sp);
         frm.Show();
      }
      public void Convertto1024()
      {         
         if (this.windsize.Width == 1280 && this.windsize.Height == 1024)
         {  //from 1280 to 1024 (+-)
            SetChange();
            ConvertSchem(0.8f, 0.75f);
            this.windsize = new Size(1024, 768);
            this.Refresh();
         }
         if (this.windsize.Width == 1600 && this.windsize.Height == 1200)
         {  //from 1600 to 1024 (+)
            SetChange();
            ConvertSchem(0.64f, 0.64f);
            this.windsize = new Size(1024, 768);
            //ConvertSchem(1.05f, 0.875f);
            //this.windsize = new Size(1024, 768);
            this.Refresh();
         }

         //testing
         //if (this.windsize.Width == 800 && this.windsize.Height == 1200)
         //{
         //   SetChange();
         //   ConvertSchem(0.85f, 0.64f);
         //   this.windsize = new Size(683, 768);
         //   //ConvertSchem(1.05f, 0.875f);
         //   //this.windsize = new Size(1024, 768);
         //   this.Refresh();
         //}
      }
      public void Convertto1280()
      {
         if (this.windsize.Width == 1024 && this.windsize.Height == 768)
         {  //from 1024 to 1280 (+-)
            SetChange();
            ConvertSchem(1.25f, 1.333f);
            this.windsize = new Size(1280, 1024);
            this.Refresh();
         }
         if (this.windsize.Width == 1600 && this.windsize.Height == 1200)
         {  //from 1600 to 1280 (+)
            SetChange();
            ConvertSchem(0.8f, 0.853f);
            this.windsize = new Size(1280, 1024);
            this.Refresh();
         }         
      }
      private void ConvertSchem(float _conv_scale_x, float _conv_scale_y)
      {
         foreach (Element _elem in list)
         {
            _elem.ConverttoNewSize(_conv_scale_x, _conv_scale_y);
            _elem.Scale = new PointF(1.0f, 1.0f);
         }
         this.orgsb.Label3_Text = "100%";
         this.win_scale = 100;
      }
      /// <summary>
      /// Сохранение схемы как подложки, без активных элементов
      /// </summary>
      /// <param name="strName">имя и путь файла</param>
      /// <param name="background">Фон подложки</param>
      public void SaveSubStrateMethod( string strName, Color background )
      {
          Bitmap bitmap = new Bitmap( windsize.Width, windsize.Height );
          // Создаем объект Graphics для вычисления высоты и ширины текста.
          Graphics graphics = Graphics.FromImage( bitmap );
          // Задаем цвет фона.
          graphics.Clear( background );
          // Задаем параметры анти-алиасинга
          graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
          graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

          foreach ( Element elem in list )
              if ( !( elem is DynamicElement ) )
                  elem.DrawElement( graphics );

          graphics.Flush();

          bitmap.Save( strName );
      }
      /// <summary>
      /// Сохранение схемы как подложки, без активных элементов
      /// </summary>
      /// <param name="strName">имя и путь файла</param>
      public void SaveSubStrateMethod( string strName )
      { 
          SaveSubStrateMethod( strName, Color.White );
      }
      #endregion

      #region Override Methods
      protected override void OnPaint(PaintEventArgs e)
      {
         //рабочее поле
         this.DrawField(e.Graphics);

         #region действие: отрисовка элементов
         foreach (Element _searchelem in list)
         {
            _searchelem.DrawElement(e.Graphics);
         }
         
         //вывод на экран еще не добавленную в список линию при ее создании
         if (select_line != null)
            select_line.DrawElement(e.Graphics);
         #endregion

         #region действие: отрисовка области выделения
         select.DrawSelectRectangle(e.Graphics);
         #endregion
      }
      protected override void OnMouseDown(MouseEventArgs e)
      {
         #region действие: сортировка списка элементов
         list.Sort(ListCompare);
         #endregion

         #region действие: создание элемента
         if (e.Button == MouseButtons.Left && orgtb.MenuItem != SelectMenuItems.None && win_scale == 100)
         {
            CreateChoise(e.Location);
            return;
         }
         #endregion

         #region действие: установка начальной точки области выделения
         if (!createlineflag)
         {
            select.SetMouseDown(e.Location);
            select.Active = true;
         }
         else 
            select.Active = false;
         #endregion

         #region действие: вывод элементу Context Menu
         if (e.Button == MouseButtons.Right && select_line == null)
         {
            contextMenuStrip1.Show(this, e.Location);
         }
         #endregion

         #region действие: изменение размера фигуры
         if (e.Button == MouseButtons.Left)
         {
            if (selected_list.Count == 1 && selected_list[0] is Figure && win_scale == 100)
            {
               if (selected_list[0].ResizeCollision(e.Location))
               {
                  pointXY.X = ((Figure)selected_list[0]).GetPosition().X;
                  pointXY.Y = ((Figure)selected_list[0]).GetPosition().Y;
                  selected_list[0].IsModify = true;
                  selected_list[0].IsDragged = false;
                  select.Active = false;
                  return;
               }
            }
         }
         #endregion

         #region действие: перетаскивание выбранной точки
         if (e.Button == MouseButtons.Left)
         {
            if (selected_list.Count == 1 && selected_list[0] is Line && win_scale == 100)
            {
               if (selected_list[0].ResizeCollision(e.Location))
               {
                  ((Line)selected_list[0]).SetPointtoMouse();
                  selected_list[0].IsModify = true;
                  select.Active = false;
                  return;
               }
            }
         }
         #endregion

         #region действие: перемещение объекта
         if (e.Button == MouseButtons.Left)
         {
            bool flag = false;

            for (int i = 0; i < selected_list.Count; i++)
            {
               if (flag)
               {
                  selected_list[i].IsDragged = true;
                  selected_list[i].MouseOffSet(e.Location);
                  select.Active = false;
               }
               if (!flag && selected_list[i].Collision(select.Area))
               {
                  flag = true;
                  i = -1;
               }
            }

            if (!flag)
            {
               foreach(Element _elem in selected_list)
               {
                  _elem.IsDragged = false;
                  _elem.IsSelected = false;
               }
            }
            return;
         }
         #endregion      
      }
      protected override void OnMouseUp(MouseEventArgs e)
      {         
         #region Figures
         #region действие: добавление созданного элемента в список
         if (e.Button == MouseButtons.Left && win_scale == 100 && SelectFigure())
         {
            if (selected_elem != null)
            {
               list.Add(selected_elem);
               selected_elem = null;
               SetChange();
            }
            orgtb.MenuItem = SelectMenuItems.None;            
         }
         #endregion
         Invalidate();
         #endregion

         #region Create line and add new point
         if (e.Button == MouseButtons.Left && win_scale == 100)
         {
            #region Add Point
            if (select_line != null)
               select_line.AddPoint(e.Location);
            #endregion

            #region Create Single Line
            if (select_line != null && select_line.CloseLine && createlineflag)
            {
               list.Add(select_line);
               orgtb.MenuItem = SelectMenuItems.None;
               createlineflag = false;
               select_line = null;
            }
            #endregion
         }
         #endregion
         #region Close & Create PolyLine
         if (e.Button == MouseButtons.Right && win_scale == 100)
         {
            if (select_line != null && createlineflag)
            {
               if (select_line is PolyLine)
               {
                  ((PolyLine)select_line).ClosePoly();

                  //Create PolyLine
                  if (select_line.CloseLine && createlineflag)
                  {
                     list.Add(select_line);
                     orgtb.MenuItem = SelectMenuItems.None;
                     createlineflag = false;
                     select_line = null;
                  }
               }
            }
         }
         #endregion

         #region действие: отмена статуса активности выделения
         if (select.Active)
         {
            select.Active = false;
         }
         #endregion

         #region действие: работа над элементами
         if (e.Button == MouseButtons.Left)
         {
            if (selected_list.Count > 0)
               selected_list.Clear();

            foreach (Element _elem in list)
            {
               #region действие: выбор элементов
               if (_elem.Collision(select.Area))
               {
                  _elem.IsSelected = true;
                  _elem.IsDragged = false;
                  _elem.IsModify = false;
                  selected_list.Add(_elem);
               }
               else
               {
                  if (_elem.IsDragged)
                  {
                     _elem.IsDragged = false;
                     _elem.IsModify = false;
                     selected_list.Add(_elem);
                  }
                  else
                  {
                     _elem.IsSelected = false;
                     _elem.IsDragged = false;
                     _elem.IsModify = false;
                  }
               }
               #endregion
            }//foreach
            Invalidate();
         }
         #endregion
      }
      protected override void OnMouseMove(MouseEventArgs e)
      {
         #region Parameters
         orgsb.Label4_Text = e.X.ToString();
         orgsb.Label5_Text = e.Y.ToString();
         #endregion

         #region действие: установка конечной точки выделения
         select.SetMouseMove(e.Location);
         Invalidate();
         #endregion
         

         #region действие: перемещение элементов
         if (e.Button == MouseButtons.Left)
         {
            foreach (Element _elem in selected_list)
            {
               if (_elem is Figure && _elem.IsSelected && _elem.IsDragged)
               {
                  ((Figure)_elem).SetPosition(e.Location);
                  SetChange();
                  Invalidate();
               }

               if (_elem is Line && _elem.IsSelected && _elem.IsDragged)
               {
                  ((Line)_elem).MoveLine(e.Location);
                  SetChange();
                  Invalidate();
               }
            }
         }
         #endregion
         #region действие: изменение размера элемента
         if (e.Button == MouseButtons.Left)
         {
            if (selected_list.Count == 1 && selected_list[0] is Figure && selected_list[0].IsModify)
            {
               sizeXY.Width = e.Location.X - pointXY.X;
               sizeXY.Height = e.Location.Y - pointXY.Y;
               ((Figure)selected_list[0]).SetSize(sizeXY);
               SetChange();
               Invalidate();
            }
         }
         #endregion      
      
         #region Move line point
         if (select_line != null && !select_line.IsDragged && win_scale == 100)
         {
            if (select_line.PointOnMouse(e.Location))
            {
               SetChange();
               Invalidate();
               return;
            }
         }
         if (selected_list.Count == 1 && selected_list[0] is Line && selected_list[0].IsModify)
         {
            if (((Line)selected_list[0]).PointOnMouse(e.Location))
            {
               SetChange();
               Invalidate();
            }
         }
         #endregion
      }
      protected override void OnKeyDown(KeyEventArgs e)
      {
         #region действие: Увеличить
         if (e.KeyCode == Keys.PageUp && list.Count != 0 && !createlineflag)
         {
            if (win_scale == 250) return;   //ограничение на максимальное увеличение

            foreach (Element _elem in list)
            {
               PointF pnt = _elem.Scale;
               _elem.Scale = new PointF(pnt.X + 0.1f, pnt.Y + 0.1f);
            }
            win_scale += 10;
            this.Refresh();
         }
         #endregion

         #region действие: Уменьшить
         if (e.KeyCode == Keys.PageDown && list.Count != 0 && !createlineflag)
         {
            if (win_scale == 40) return;    //ограничение на максимальное уменьшение

            foreach (Element _elem in list)
            {
               PointF pnt = _elem.Scale;
               _elem.Scale = new PointF(pnt.X - 0.1f, pnt.Y - 0.1f);
            }
            win_scale -= 10;
            this.Refresh();
         }
         #endregion         

         orgsb.Label3_Text = win_scale.ToString() + "%";
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить полный путь до файла
      /// </summary>
      public bool FileNameExist
      {
         get 
         {
            if (filename != null)
               return true;
            else
               return false;
         }
      }
      /// <summary>
      /// Получить или задать имя(описание) схемы
      /// </summary>
      public String MnenoCaption
      {
         get { return mnenocaption; }
         set
         {
            mnenocaption = value;
            SetChange();
         }
      }
      /// <summary>
      /// Получить признак внесения изменений
      /// </summary>
      public bool ExistChange
      {
         get { return create_change; }
      }
      /// <summary>
      /// Получить или задать размер рабочей области
      /// </summary>
      public Size WorkSpaceSize
      {
         get { return this.windsize; }
         set { this.windsize = value; }
      }
      /// <summary>
      /// Получить коэфициент смешения рабочей области по ширине
      /// </summary>
      public Int32 WidthShift
      {
         get { return shiftw; }
      }
      /// <summary>
      /// Получить коэфициент смешения рабочей области по высоте
      /// </summary>
      public Int32 HeightShift
      {
         get { return shifth; }
      }
      #endregion

      #region ToolBar Menu Methods
      public void SelectedAll()
      {
         selected_list.Clear();

         foreach (Element _elem in list)         
         {
            _elem.IsSelected = true;
            _elem.IsDragged = false;
            _elem.IsModify = false;
            selected_list.Add(_elem);
         }
         this.Refresh();
      }
      #endregion

      #region Context Menu Items
      public void cutToolStripMenuItem_Click(object sender, EventArgs e)
      {
          copyToolStripMenuItem_Click( sender, e );
          deleteToolStripMenuItem_Click( sender, e );

          SetChange();
      }
      public void copyToolStripMenuItem_Click(object sender, EventArgs e)
      {
          foreach ( Element _elem in selected_list )
              copy_list.Add( _elem.CopyElement() );
      }
      public void pasteToolStripMenuItem_Click(object sender, EventArgs e)
      {
          foreach ( Element _elem in selected_list )
          {
              _elem.IsSelected = false;
          }
          selected_list.Clear();

          foreach ( Element _elem in copy_list )
          {
              _elem.IsSelected = true;
              list.Add( _elem );
              selected_list.Add( _elem );
          }
          copy_list.Clear();
          this.Refresh();

          SetChange();
      }
      public void deleteToolStripMenuItem_Click(object sender, EventArgs e)
      {
          for ( int i = 0; i < list.Count; i++ )
          {
              if ( list[i].IsSelected )
              {
                  selected_elem = null;
                  select_line = null;
                  list.RemoveAt( i );
                  i = -1;
              }
          }
          selected_list.Clear();
          this.Refresh();

          SetChange();
      }      
      private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (selected_list.Count != 1) return;

         
         if (selected_list[0] is Line)
         {
            LinesPropertiesForm frm = new LinesPropertiesForm(selected_list[0], this.windsize.Width, this.windsize.Height);
            if (frm.ShowDialog() == DialogResult.OK)
               SetChange();
            this.Refresh();
            return;
         }

         //if (selected_list[0] is FormText)
         //{
         //   Form5 frm = new Form5((FormText)selected_list[0]);
         //   if (frm.ShowDialog() == DialogResult.OK)
         //      SetChange();
         //}//if
         //else
         //{
         //   Form4 frm = new Form4(selected_list[0]);
         //   if (frm.ShowDialog() == DialogResult.OK)
         //      SetChange();
         //}

         CommonPropertiesForm form = new CommonPropertiesForm( selected_list[0], this.windsize.Width, this.windsize.Height );
         if ( form.ShowDialog() == DialogResult.OK )
             SetChange();        
      }
      #endregion
   }
}