using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using LibraryElements;
using WindowsForms;
using Structure;

namespace ElementEditor
{
   /// <summary>
   /// Графическая закладка панели
   /// </summary>
   class GraphicTabPage : TabPage
   {
      #region Parameters
      int number;
      PictureBox picture;
      ContextMenuStrip conmenu;
      ToolStripMenuItem conmenuitem;
      bool changeflag;

      #region Elements
      Components component;
      DrawRotate rotating;
      Figure selected_element;
      Figure copy_element;
      Line select_line;
      Line copy_line;
      List<Element> editor_list;
      List<Element> select_list;
      Point mouselocation;
      Point pointXY;
      Size sizeXY;
      bool createlineflag;
      #endregion
      #endregion

      #region Class Methods
      public GraphicTabPage(string _tab_name)
      {
         InitContextMenu();
         InitCanvas();
         InitElements();

         this.Location = new System.Drawing.Point(4, 22);
         this.Name = "tabPage1";
         this.Padding = new System.Windows.Forms.Padding(3);
         this.Size = new System.Drawing.Size(258, 234);
         this.TabIndex = 0;
         this.Text = _tab_name;
         this.UseVisualStyleBackColor = true;

         this.Controls.Add(picture);
      }
      private void InitElements()
      {
         this.mouselocation = new Point();
         this.pointXY = new Point();
         this.sizeXY = new Size();
         this.editor_list = new List<Element>();
         this.select_list = new List<Element>();

         component = Components.None;
         copy_element = null;
         copy_line = null;
         selected_element = null;
         select_line = null;
         createlineflag = false;
         changeflag = false;
      }
      private void InitCanvas()
      {
         picture = new PictureBox();
         picture.Anchor = AnchorStyles.Top;
         picture.BackColor = SystemColors.Window;
         picture.BorderStyle = BorderStyle.FixedSingle;
         picture.Location = new Point(31, 17);
         picture.Name = "pictureBox";
         //picture.Size = new Size(102, 102);
         picture.ClientSize = new Size(201,201);
         picture.TabIndex = 0;
         picture.TabStop = false;

         SetStyle(ControlStyles.UserPaint, true);
         SetStyle(ControlStyles.AllPaintingInWmPaint, true);
         SetStyle(ControlStyles.DoubleBuffer, true);

         InitCanvasEvents();
      }
      private void InitCanvasEvents()
      {
         picture.Paint += new PaintEventHandler(pictureBox_Paint);
         ApplyEvents();
      }
      private void InitContextMenu()
      {
         //Menu
         conmenu = new ContextMenuStrip();
         conmenu.Name = "contextMenuStrip";
         conmenu.RenderMode = ToolStripRenderMode.System;
         conmenu.Size = new Size(147, 120);
         conmenu.ResumeLayout(false);

         AddContextItems();
      }
      private void AddContextItems()
      {
         conmenu.Items.Clear();

         //Menu Elements
         conmenuitem = new ToolStripMenuItem();
         conmenuitem.Name = "cutToolStripMenuItem";
         conmenuitem.Size = new Size(146, 22);
         conmenuitem.Text = "Выризать";
         conmenuitem.Click += new EventHandler(Cut_Click);
         conmenu.Items.Add(conmenuitem);

         conmenuitem = new ToolStripMenuItem();
         conmenuitem.Name = "copyToolStripMenuItem";
         conmenuitem.Size = new Size(146, 22);
         conmenuitem.Text = "Копировать";
         conmenuitem.Click += new EventHandler(Copy_Click);
         conmenu.Items.Add(conmenuitem);

         conmenuitem = new ToolStripMenuItem();
         conmenuitem.Name = "pasteToolStripMenuItem";
         conmenuitem.Size = new Size(146, 22);
         conmenuitem.Text = "Вставить";
         conmenuitem.Click += new EventHandler(Paste_Click);
         conmenu.Items.Add(conmenuitem);

         conmenuitem = new ToolStripMenuItem();
         conmenuitem.Name = "deleteToolStripMenuItem";
         conmenuitem.Size = new Size(146, 22);
         conmenuitem.Text = "Удалить";
         conmenuitem.Click += new EventHandler(Delete_Click);
         conmenu.Items.Add(conmenuitem);

         ToolStripSeparator tss = new ToolStripSeparator();
         tss.Size = new Size(143, 6);
         conmenu.Items.Add(tss);

         conmenuitem = new ToolStripMenuItem();
         conmenuitem.Name = "propertiesToolStripMenuItem";
         conmenuitem.Size = new Size(146, 22);
         conmenuitem.Text = "Свойства";
         conmenuitem.Click += new EventHandler(Properties_Click);
         conmenu.Items.Add(conmenuitem);
      }
      /// <summary>
      /// Добавление путктов в контекстменю
      /// для выбора выделенных элементов
      /// </summary>
      private void AddNewItemMenu()
      {
         int quant = 0;
         number = 0;
         AddContextItems();
         select_list.Clear();         

         foreach (Element _elem in editor_list)
         {
            if (_elem.IsSelected)
            {
               ToolStripSeparator tss = new ToolStripSeparator();
               conmenu.Items.Add(tss);

               conmenuitem = new ToolStripMenuItem();
               conmenuitem.Name = "DropSelectItems";
               conmenuitem.Size = new Size(146, 22);
               conmenuitem.Text = "Снять со всех выделение";
               conmenuitem.Click += new EventHandler(conmenuitem_Click);
               conmenu.Items.Add(conmenuitem);
               break;
            }
         }

         foreach (Element _elem in editor_list)
         {
            //if (quant > 4) return;//если фигур больше 5, больше не добавляем
            if (quant >= 4 && _elem.IsSelected)
               select_list.Add(_elem);

            if (quant < 4 && _elem.IsSelected)
            {
               conmenuitem = new ToolStripMenuItem();
               conmenuitem.Name = "SelectItem";
               conmenuitem.Size = new Size(146, 22);
               conmenuitem.Checked = true;
               conmenuitem.CheckOnClick = true;
               conmenuitem.Text = "Element " + number.ToString();
               conmenuitem.CheckedChanged += new EventHandler(conmenuitem_CheckedChanged);
               conmenu.Items.Add(conmenuitem);

               quant++;
               number++;
               select_list.Add(_elem);
            }
         }
      }
      private bool SelectFigure()
      {
         switch (component)
         {
            case Components.PolyLine: return false;
            case Components.SingleLine: return false;
            default: return true;
         }
      }
      private void CreateChoise(Point _pnt)
      {
         sizeXY.Width = 0;
         sizeXY.Height = 0;

         switch (component)
         {
            case Components.SingleLine:
               {
                  select_line = new Line();
                  component = Components.None;
                  createlineflag = true;
               }
               break;
            case Components.PolyLine:
               {
                  select_line = new PolyLine();
                  component = Components.None;
                  createlineflag = true;
               }
               break;
            case Components.Ellipse:
               {
                  selected_element = new PrimEllipse();
                  selected_element.SetPosition(_pnt);
               }
               break;
            case Components.Rectangle:
               {
                  selected_element = new PrimRectangle();
                  selected_element.SetPosition(_pnt);
               }
               break;
            case Components.Arc:
               {
                  selected_element = new EditorArc(rotating);
                  selected_element.SetPosition(_pnt);
               }
               break;
            case Components.FillEllipse:
               {
                  selected_element = new EditorFillEllipse();
                  selected_element.SetPosition(_pnt);
               }
               break;
            case Components.FillRectangle:
               {
                  selected_element = new EditorFillRectangle();
                  selected_element.SetPosition(_pnt);
               }
               break;
            default: break;
         }
      }
      public void DeleteImage()
      {
         editor_list.Clear();
         changeflag = true;
         picture.Refresh();
      }

      private void CopyElement()
      {
         if (selected_element != null)
         {
            if (selected_element is EditorArc)
            {
               EditorArc create = new EditorArc(DrawRotate.Down);
               create.CopyElement(selected_element);
               copy_element = create;
            }
            if (selected_element is PrimEllipse)
            {
               PrimEllipse create = new PrimEllipse();
               create.CopyElement(selected_element);
               copy_element = create;
            }
            if (selected_element is PrimRectangle)
            {
               PrimRectangle create = new PrimRectangle();
               create.CopyElement(selected_element);
               copy_element = create;
            }
         }
         if (select_line != null)
         {
            if (select_line is PolyLine)
            {
               PolyLine create = new PolyLine();
               create.CopyElement(select_line);
               copy_line = create;
            }
            else
            {
               Line create = new Line();
               create.CopyElement(select_line);
               copy_line = create;
            }
         }      
      }
      private void DeleteElement()
      {
         if (editor_list.Count != 0 && selected_element != null)
         {
            editor_list.Remove(selected_element);
            selected_element = null;
            picture.Refresh();
         }
         if (editor_list.Count != 0 && select_line != null)
         {
            editor_list.Remove(select_line);
            select_line = null;
            picture.Refresh();
         }
      }
      private void PasteElement()
      {
         if (copy_element != null)
         {
            editor_list.Add(copy_element);
            picture.Refresh();
         }
         if (copy_line != null)
         {
            editor_list.Add(copy_line);
            picture.Refresh();
         }
      }
      private void Properties()
      {
         if (selected_element != null && select_line == null)
         {
            if (selected_element is EditorFillEllipse || selected_element is EditorFillRectangle)
            {
               Form4_mod frm = new Form4_mod(selected_element);
               if (frm.ShowDialog() == DialogResult.OK)
               {
                  changeflag = true;
                  picture.Refresh();
               }
            }
            else
            {
               Form4 frm = new Form4(selected_element);
               if (frm.ShowDialog() == DialogResult.OK)
               {
                  changeflag = true;
                  picture.Refresh();
               }
            }//if_else
         }
         if (select_line != null && selected_element == null)
         {
            Form3 frm = new Form3(select_line);
            if (frm.ShowDialog() == DialogResult.OK)
            {
               changeflag = true;
               picture.Refresh();
            }
         }
      }
      /// <summary>
      /// Отключение возможности изменять нарисованные элементы
      /// </summary>
      public void CancelEvents()
      {
        if (picture != null)
        {
           picture.MouseDown -= new MouseEventHandler(pictureBox_MouseDown);
           picture.MouseUp -= new MouseEventHandler(pictureBox_MouseUp);
           picture.MouseMove -= new MouseEventHandler(pictureBox_MouseMove);          
        }
      }
      /// <summary>
      /// Включение возможности изменять нарисованные элементы
      /// </summary>
      public void ApplyEvents()
      {
        if (picture != null)
        {
           picture.MouseDown += new MouseEventHandler(pictureBox_MouseDown);
           picture.MouseUp += new MouseEventHandler(pictureBox_MouseUp);
           picture.MouseMove += new MouseEventHandler(pictureBox_MouseMove);          
        }
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить или назначить вариант компонента
      /// </summary>
      public Components Comp_choise
      {
         get { return component; }
         set { component = value; }
      }
      /// <summary>
      /// Получить или задать направление поворота компонента
      /// </summary>
      public DrawRotate Rotate_choise
      {
         get { return rotating; }
         set { rotating = value; }
      }
      /// <summary>
      /// Получить или задать список фигур
      /// </summary>
      public List<Element> PicList
      {
         get { return editor_list; }
         set { editor_list = value; picture.Invalidate(); }
      }
      /// <summary>
      /// Получить статус изменения фигур на холсте
      /// </summary>
      public Boolean Change
      {
         get { return changeflag; }
      }
      #endregion

      #region Events
      /// <summary>
      /// Снятие выделения со всех выбранных фигур.
      /// </summary>
      void conmenuitem_Click(object sender, EventArgs e)
      {
         foreach (Element _elem in select_list)
            _elem.IsSelected = false;

         select_line = null;
         selected_element = null;
         select_list.Clear();
         picture.Refresh();
      }
      /// <summary>
      /// Снимаем выделение с выбранных фигур.
      /// последнюю помещаем в selected_element.
      /// </summary>
      void conmenuitem_CheckedChanged(object sender, EventArgs e)
      {
         ToolStripMenuItem tmp = (ToolStripMenuItem)sender;
         tmp.Checked = false;
         string name = tmp.Text;
         int qunt = conmenu.Items.Count;
         int position = 8;

         for (int cm = position; cm < qunt; cm++)
         {
            if (conmenu.Items[cm].Text == name)
            {
               conmenu.Items.RemoveAt(cm);
               select_list[cm - position].IsSelected = false;
               select_list.RemoveAt(cm - position);
               picture.Refresh();
               break;
            }
         }

         if (select_list.Count == 0)//если елемент был последний
         {
            select_line = null;
            selected_element = null;
         }

         if (select_list.Count == 1)//если осталось одна фигура, выбираем ее.
         {
            if (select_list[0] is Figure)
            {
               selected_element = (Figure)select_list[0];
               select_line = null;
            }
            else
            {
               select_line = (Line)select_list[0];
               selected_element = null;
            }
            //select_list.Clear();
         }
      }
      //Canvas Events
      void pictureBox_Paint(object sender, PaintEventArgs e)
      {
         #region Draw Elements
         if (selected_element != null)
            selected_element.DrawElement(e.Graphics);
         if (select_line != null)               
            select_line.DrawElement(e.Graphics);

         foreach (Element _searchelem in editor_list)
         {
            _searchelem.DrawElement(e.Graphics);
         }
         #endregion
      }
      void pictureBox_MouseDown(object sender, MouseEventArgs e)
      {
         mouselocation = new Point(e.X, e.Y);

         #region Create Element
         if (e.Button == MouseButtons.Left && component != Components.None)
         {
            CreateChoise(mouselocation);
            return;
         }
         #endregion

         #region действие: вывод элементу Context Menu
         if (e.Button == MouseButtons.Right && !createlineflag)
         {
            AddNewItemMenu();
            //в связи с расположением холста (picture) в picture.Location = new Point(31, 17);
            //меню так же нужно подвинуть
            conmenu.Show(picture.Parent, new Point(e.X + 31, e.Y + 17));
         }
         #endregion

         if (e.Button == MouseButtons.Left && component == Components.None)
         {
            Rectangle rect = new Rectangle(e.X, e.Y, 2, 2);

            #region Изменение размера фигуры
            if (selected_element != null && selected_element.ResizeCollision(mouselocation))
            {
               pointXY.X = selected_element.GetPosition().X;
               pointXY.Y = selected_element.GetPosition().Y;
               selected_element.IsModify = true;
               selected_element.IsDragged = false;
               return;
            }
            #endregion
            #region действие: перемещение объекта
            if (selected_element != null)
            {
               if (selected_element.Collision(rect))
               {
                  selected_element.MouseOffSet(mouselocation);
                  selected_element.IsDragged = true;
                  selected_element.IsModify = false;
                  return;
               }
               else
               {
                  selected_element.IsSelected = false;
                  selected_element = null;
               }
            }
            #endregion
            #region действие: перемещение линии
            if (select_line != null && select_line.CloseLine && !select_line.ResizeCollision(mouselocation))
            {
               if (select_line.Collision(rect))
               {
                  select_line.MouseOffSet(mouselocation);
                  select_line.IsDragged = true;
                  return;
               }
               else
               {
                  select_line.IsSelected = false;
                  select_line = null;
               }
            }
            #endregion
            #region действие: перетаскивание выбранной точки
            if (select_line != null && select_line.IsSelected && select_line.ResizeCollision(mouselocation))
            {
               ((Line)select_line).SetPointtoMouse();
               return;
            }
            #endregion
            #region действие: выделение мышью объекта или снятие выделения
            foreach (Element _elem in editor_list)
            {
               if (_elem is Figure)
               {
                  if (_elem.Collision(rect) && !_elem.IsSelected)
                  {
                     _elem.MouseOffSet(mouselocation);
                     _elem.IsSelected = true;
                     _elem.IsDragged = true;
                     selected_element = (Figure)_elem;
                  }
                  else
                     _elem.IsSelected = false;
               }//if
               if (_elem is Line)
               {
                  if (_elem.Collision(rect) && !_elem.IsSelected && !_elem.ResizeCollision(mouselocation))
                  {
                     _elem.MouseOffSet(mouselocation);
                     _elem.IsSelected = true;
                     _elem.IsDragged = true;
                     select_line = (Line)_elem;
                  }
                  else
                     _elem.IsSelected = false;
               }
            }//foreach
            #endregion
         }

         picture.Refresh();
      }
      void pictureBox_MouseUp(object sender, MouseEventArgs e)
      {
         #region Create Element
         if (e.Button == MouseButtons.Left && component != Components.None && SelectFigure())
         {
            if (selected_element != null)
            {
               editor_list.Add(selected_element);
               component = Components.None;
               changeflag = true;
            }
         }
         #endregion
         #region Dragged and Resize Element
         if (e.Button == MouseButtons.Left && selected_element != null)
         {
            if (selected_element.IsDragged)
               selected_element.IsDragged = false;

            if (selected_element.IsModify)
               selected_element.IsModify = false;
         }
         #endregion

         #region Create line and add new point
         if (e.Button == MouseButtons.Left)
         {
            #region Add Point
            if (select_line != null)
               select_line.AddPoint(new Point(e.X, e.Y));
            #endregion

            #region Create Single Line
            if (select_line != null && select_line.CloseLine && createlineflag)
            {
               editor_list.Add(select_line);
               component = Components.None;
               createlineflag = false;
               changeflag = true;
            }
            #endregion
         }
         #endregion
         #region Close & Create PolyLine
         if (e.Button == MouseButtons.Right)
         {
            if (select_line != null && createlineflag)
            {
               if (select_line is PolyLine)
               {
                  ((PolyLine)select_line).ClosePoly();

                  //Create PolyLine
                  if (select_line.CloseLine && createlineflag)
                  {
                     editor_list.Add(select_line);
                     component = Components.None;
                     createlineflag = false;
                     changeflag = true;
                  }
               }
            }
         }
         #endregion
         #region Dragged Line
         if (e.Button == MouseButtons.Left && select_line != null)
         {
            if (select_line.IsDragged)
               select_line.IsDragged = false;
         }
         #endregion

         picture.Invalidate();
      }
      void pictureBox_MouseMove(object sender, MouseEventArgs e)
      {
         mouselocation = new Point(e.X, e.Y);

         #region Move Element
         if (e.Button == MouseButtons.Left && component == Components.None)
         {
            if (selected_element != null && selected_element.IsDragged)
            {
               selected_element.SetPosition(mouselocation);
               picture.Refresh();
               changeflag = true;
            }
         }
         #endregion
         #region ResizeBeginCreatedElement
         if (e.Button == MouseButtons.Left)
         {
            if (selected_element != null && selected_element.IsModify)
            {
               sizeXY.Width = mouselocation.X - pointXY.X;
               sizeXY.Height = mouselocation.Y - pointXY.Y;
               selected_element.SetSize(sizeXY);
               picture.Refresh();
               changeflag = true;
            }
         }
         #endregion

         #region Move line point
         if (select_line != null && !select_line.IsModify)
         {
            if (select_line.PointOnMouse(mouselocation))
            {
               picture.Invalidate();
               changeflag = true;
            }
         }
         #endregion
         #region Move line
         if (e.Button == MouseButtons.Left && component == Components.None)
         {
            if (select_line != null && select_line.IsDragged)
            {
               select_line.MoveLine(mouselocation);
               picture.Invalidate();
               changeflag = true;
            }
         }
         #endregion
      }
      //ContextMenu Events
      void Properties_Click(object sender, EventArgs e)
      {
         Properties();         
      }
      void Delete_Click(object sender, EventArgs e)
      {
         DeleteElement();
      }
      void Paste_Click(object sender, EventArgs e)
      {
         PasteElement();
      }
      void Copy_Click(object sender, EventArgs e)
      {
         CopyElement();
      }
      void Cut_Click(object sender, EventArgs e)
      {
         CopyElement();
         DeleteElement();
      }
      #endregion
   }
}
