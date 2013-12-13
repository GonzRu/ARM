using System;
using System.Windows.Forms;
using Structure;

namespace BarsMenu
{
   /// <summary>
   /// Класс элементов ToolBar'a
   /// </summary>
   public class ToolBar
   {
      #region Parameters
      const string figure_name = "Элементы";      
      string sel_cu_name;
      string sel_cu_sub_name;
      SelectMenuItems item_menu;
      SelectMenuModify item_modify;

      ToolStrip toolspanel;
      ToolStripDropDownButton db_elements;
      
      #region Elements of menu
      ToolStripMenuItem mi_elem_menu1;
      ToolStripMenuItem mi_elem_menu2;
      ToolStripMenuItem mi_elem_menu4;
      ToolStripMenuItem mi_elem_menu5;
      ToolStripMenuItem mi_elem_menu6;
      ToolStripMenuItem mi_elem_menu7;
      ToolStripMenuItem mi_elem_menu8;
      ToolStripMenuItem mi_elem_menu9;
      ToolStripMenuItem mi_elem_menu10;

      ToolStripMenuItem mi_elem_menu1_1;
      ToolStripMenuItem mi_elem_menu1_2;
      ToolStripMenuItem mi_elem_menu1_3;
      ToolStripMenuItem mi_elem_menu1_4;
      ToolStripMenuItem mi_elem_menu1_5;
      ToolStripMenuItem mi_elem_menu1_6;
      ToolStripMenuItem mi_elem_menu5_1;
      ToolStripMenuItem mi_elem_menu5_2;
      #endregion
      #endregion

      #region Class Methods
      public ToolBar(ToolStrip _toolbar)
      {
         toolspanel = _toolbar;
         sel_cu_name = sel_cu_sub_name = null;
         InitMenu();

         CreateElements();
         CreateMenuItems_LevelOne();
         CreateSubMenuItems_LevelTwo();
         CreateSubMenuItems_LevelThree();
         InsertMenuItems();
         
         toolspanel.Items.Add(db_elements);

         CreateCustomItems();         
      }
      /// <summary>
      /// Инициализация начальных значений меню
      /// </summary>
      private void InitMenu()
      {
         item_menu = SelectMenuItems.None;
         item_modify = SelectMenuModify.None;
      }
      /// <summary>
      /// Создание выпадающих меню
      /// </summary>
      private void CreateElements()
      {
          this.db_elements = new ToolStripDropDownButton
              {
                  Image = Resource1.Figures.ToBitmap(),
                  ImageTransparentColor = System.Drawing.Color.Magenta,
                  Name = "toolStripDropDownButton1",
                  Size = new System.Drawing.Size( 87, 22 ),
                  Text = figure_name
              };
      }
      /// <summary>
      /// Создание пунктов меню (Уровень один)
      /// </summary>
      private void CreateMenuItems_LevelOne()
      {
         //основное меню
         this.mi_elem_menu1 = new ToolStripMenuItem();
         this.mi_elem_menu1.Name = "menu1ToolStripMenuItem";
         this.mi_elem_menu1.Size = new System.Drawing.Size(211, 22);
         this.mi_elem_menu1.Text = "Графические приметивы";

         this.mi_elem_menu2 = new ToolStripMenuItem();
         this.mi_elem_menu2.Name = "menu2ToolStripMenuItem";
         this.mi_elem_menu2.Size = new System.Drawing.Size(211, 22);
         this.mi_elem_menu2.Text = "Разъединитель";
         this.mi_elem_menu2.Click += new EventHandler(mi_elem_menu2_Click);

         this.mi_elem_menu4 = new ToolStripMenuItem();
         this.mi_elem_menu4.Name = "menu4ToolStripMenuItem";
         this.mi_elem_menu4.Size = new System.Drawing.Size(211, 22);
         this.mi_elem_menu4.Text = "Элемент с заземлением";
         this.mi_elem_menu4.Click += new EventHandler(mi_elem_menu4_Click);

         this.mi_elem_menu5 = new ToolStripMenuItem();
         this.mi_elem_menu5.Name = "menu5ToolStripMenuItem";
         this.mi_elem_menu5.Size = new System.Drawing.Size(211, 22);
         this.mi_elem_menu5.Text = "Элементы шины";

         this.mi_elem_menu6 = new ToolStripMenuItem();
         this.mi_elem_menu6.Name = "menu6ToolStripMenuItem";
         this.mi_elem_menu6.Size = new System.Drawing.Size(211, 22);
         this.mi_elem_menu6.Text = "Динамический элемент";
         this.mi_elem_menu6.Click += new EventHandler(mi_elem_menu6_Click);

         this.mi_elem_menu7 = new ToolStripMenuItem();
         this.mi_elem_menu7.Name = "menu7ToolStripMenuItem";
         this.mi_elem_menu7.Size = new System.Drawing.Size(211, 22);
         this.mi_elem_menu7.Text = "Статический элемент";
         this.mi_elem_menu7.Click += new EventHandler(mi_elem_menu7_Click);

         this.mi_elem_menu8 = new ToolStripMenuItem();
         this.mi_elem_menu8.Name = "menu8ToolStripMenuItem";
         this.mi_elem_menu8.Size = new System.Drawing.Size(211, 22);
         this.mi_elem_menu8.Text = "Текст";
         this.mi_elem_menu8.Click += new EventHandler(mi_elem_menu8_Click);

         this.mi_elem_menu9 = new ToolStripMenuItem();
         this.mi_elem_menu9.Name = "menu9ToolStripMenuItem";
         this.mi_elem_menu9.Size = new System.Drawing.Size( 211, 22 );
         this.mi_elem_menu9.Text = "Кнопка";
         this.mi_elem_menu9.Click += new EventHandler( mi_elem_menu9_Click );
         
          this.mi_elem_menu10 = new ToolStripMenuItem();
         this.mi_elem_menu10.Name = "menu10ToolStripMenuItem";
         this.mi_elem_menu10.Size = new System.Drawing.Size( 211, 22 );
         this.mi_elem_menu10.Text = "Текстовый блок";
         this.mi_elem_menu10.Click += new EventHandler( mi_elem_menu10_Click );
      }
      /// <summary>
      /// Создание подпунктов меню (Уровень два)
      /// </summary>
      private void CreateSubMenuItems_LevelTwo()
      {
         this.mi_elem_menu1_1 = new ToolStripMenuItem();
         this.mi_elem_menu1_1.Name = "menu11ToolStripMenuItem";
         this.mi_elem_menu1_1.Size = new System.Drawing.Size(175, 22);
         this.mi_elem_menu1_1.Text = "Одиночная линия";
         this.mi_elem_menu1_1.Click += new EventHandler(mi_elem_menu1_1_Click);

         this.mi_elem_menu1_2 = new ToolStripMenuItem();
         this.mi_elem_menu1_2.Name = "menu12ToolStripMenuItem";
         this.mi_elem_menu1_2.Size = new System.Drawing.Size(175, 22);
         this.mi_elem_menu1_2.Text = "Ломаная линия";
         this.mi_elem_menu1_2.Click += new EventHandler(mi_elem_menu1_2_Click);

         this.mi_elem_menu1_3 = new ToolStripMenuItem();
         this.mi_elem_menu1_3.Name = "menu13ToolStripMenuItem";
         this.mi_elem_menu1_3.Size = new System.Drawing.Size(175, 22);
         this.mi_elem_menu1_3.Text = "Эллипс/круг";
         this.mi_elem_menu1_3.Click += new EventHandler(mi_elem_menu1_3_Click);

         this.mi_elem_menu1_4 = new ToolStripMenuItem();
         this.mi_elem_menu1_4.Name = "menu14ToolStripMenuItem";
         this.mi_elem_menu1_4.Size = new System.Drawing.Size(175, 22);
         this.mi_elem_menu1_4.Text = "Прямоугольник/квадрат";
         this.mi_elem_menu1_4.Click += new EventHandler(mi_elem_menu1_4_Click);

         this.mi_elem_menu1_5 = new ToolStripMenuItem();
         this.mi_elem_menu1_5.Name = "menu15ToolStripMenuItem";
         this.mi_elem_menu1_5.Size = new System.Drawing.Size(175, 22);
         this.mi_elem_menu1_5.Text = "Треугольник";
         this.mi_elem_menu1_5.Click += new EventHandler(mi_elem_menu1_5_Click);

         this.mi_elem_menu1_6 = new ToolStripMenuItem();
         this.mi_elem_menu1_6.Name = "menu16ToolStripMenuItem";
         this.mi_elem_menu1_6.Size = new System.Drawing.Size(175, 22);
         this.mi_elem_menu1_6.Text = "Дуга";
         this.mi_elem_menu1_6.Click += new EventHandler(mi_elem_menu1_6_Click);

         this.mi_elem_menu5_1 = new ToolStripMenuItem();
         this.mi_elem_menu5_1.Name = "menu51ToolStripMenuItem";
         this.mi_elem_menu5_1.Size = new System.Drawing.Size(123, 22);
         this.mi_elem_menu5_1.Text = "Шина";
         this.mi_elem_menu5_1.Click += new EventHandler(mi_elem_menu5_1_Click);

         this.mi_elem_menu5_2 = new ToolStripMenuItem();
         this.mi_elem_menu5_2.Name = "menu52ToolStripMenuItem";
         this.mi_elem_menu5_2.Size = new System.Drawing.Size(123, 22);
         this.mi_elem_menu5_2.Text = "Соединитель";
         this.mi_elem_menu5_2.Click += new EventHandler(mi_elem_menu5_2_Click);
      }
      /// <summary>
      /// Создание подпунктов меню (Уровень три)
      /// </summary>      
      private void CreateSubMenuItems_LevelThree() { }
      /// <summary>
      /// Подключение всех пунктов меню к выпадающему меню
      /// </summary>
      private void InsertMenuItems()
      {
         //подключение подпунктов(уровень два)
         this.mi_elem_menu1.DropDownItems.Add(mi_elem_menu1_1);
         this.mi_elem_menu1.DropDownItems.Add(mi_elem_menu1_2);
         this.mi_elem_menu1.DropDownItems.Add(mi_elem_menu1_3);
         this.mi_elem_menu1.DropDownItems.Add(mi_elem_menu1_4);
         this.mi_elem_menu1.DropDownItems.Add(mi_elem_menu1_5);
         this.mi_elem_menu1.DropDownItems.Add(mi_elem_menu1_6);

         this.mi_elem_menu5.DropDownItems.Add(mi_elem_menu5_1);
         this.mi_elem_menu5.DropDownItems.Add(mi_elem_menu5_2);

         //подключение к основному выподающему меню
         this.db_elements.DropDownItems.Add(mi_elem_menu1);
         this.db_elements.DropDownItems.Add(mi_elem_menu2);
         this.db_elements.DropDownItems.Add(mi_elem_menu4);
         this.db_elements.DropDownItems.Add(mi_elem_menu5);
         this.db_elements.DropDownItems.Add(mi_elem_menu6);
         this.db_elements.DropDownItems.Add(mi_elem_menu7);
         this.db_elements.DropDownItems.Add(mi_elem_menu8);
         this.db_elements.DropDownItems.Add(mi_elem_menu9);
         this.db_elements.DropDownItems.Add(mi_elem_menu10);
      }
      /// <summary>
      /// Создание списка меню сторонних производителей
      /// </summary>
      private void CreateCustomItems()
      {
         //cu_builder = new ToolBarBuilder();
         //cu_builder.CreateMenu(toolspanel);

         //if (!cu_builder.Error_Status)
         //   for (int i = 4; i < toolspanel.Items.Count; i++)
         //   {
         //      ToolStripItem tmp = toolspanel.Items[i];
         //      if (tmp is ToolStripDropDownButton && tmp.Text != figure_name)
         //         AddCustomEvents((ToolStripDropDownButton)tmp);
         //   }            
      }
      /// <summary>
      /// Добавляем реакцию на нажатие меню
      /// </summary>
      private void AddCustomEvents(ToolStripDropDownButton _menu)
      {
         //foreach (ToolStripMenuItem menu in _menu.DropDownItems)
         //{
         //   if (menu.DropDownItems.Count == 0)
         //      menu.Click += new EventHandler(cu_elem_menu_Click);
            
         //   if (menu.DropDownItems.Count > 0)
         //      AddCustomSubEvents(menu.DropDownItems);
         //}
      }
      /// <summary>
      /// Добавляем реакцию на нажатие подменю
      /// </summary>
      /// <param name="_menu">подсписок елемента меню</param>
      private void AddCustomSubEvents(ToolStripItemCollection _menu)
      {
         //foreach (ToolStripMenuItem _sub in _menu)
         //   _sub.Click += new EventHandler(cu_elem_menu_Click);
      }
      #endregion

      #region Properties SelectedMenu
      public SelectMenuItems MenuItem
      {
         get { return item_menu; }
         set { item_menu = value; }
      }
      public SelectMenuModify MenuModify
      {
         get { return item_modify; }
         set { item_modify = value; }
      }
      public String MenuName
      {
         get { return sel_cu_name; }
      }
      public String SubMenuName
      {
         get { return sel_cu_sub_name; }
      }
      #endregion

      #region Events
      void mi_elem_menu8_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuText;
      }
      void mi_elem_menu9_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.SchemaButton;
      }      
      void mi_elem_menu6_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuDinamicElement;
      }
      void mi_elem_menu7_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuStaticElement;
      }
      void mi_elem_menu5_2_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuTrunkPoint;
      }
      void mi_elem_menu5_1_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuTrunk;
      }
      void mi_elem_menu4_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuGrounding;
         item_modify = SelectMenuModify.Up;
      }
      void mi_elem_menu2_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuFloorChassis;
         item_modify = SelectMenuModify.Up;
      }
      void mi_elem_menu1_1_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuSingleLine;
      }
      void mi_elem_menu1_2_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuPolyLine;
      }
      void mi_elem_menu1_3_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuEllipse;
      }
      void mi_elem_menu1_4_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuRectangle;
      }
      void mi_elem_menu1_5_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuTriangle;
         item_modify = SelectMenuModify.Up;
      }      
      void mi_elem_menu1_6_Click(object sender, EventArgs e)
      {
         item_menu = SelectMenuItems.ItemMenuArc;
         item_modify = SelectMenuModify.Up;
      }
      void mi_elem_menu10_Click( object sender, EventArgs e )
      {
          item_menu = SelectMenuItems.ItemMenuBlockText;
      }
      #endregion

      #region Custom Events
      void cu_elem_menu_Click(object sender, EventArgs e)
      {
         //задается в классе ToolBarBuilder при создании категории элементов
         //const string name = "toolStripDropDownButton";
         //item_menu = SelectMenuItems.Custom;

         //if (name == ((ToolStripMenuItem)sender).OwnerItem.Name)
         //{
         //   sel_cu_name = ((ToolStripMenuItem)sender).Text;
         //   sel_cu_sub_name = null;
         //}
         //else
         //{
         //   sel_cu_name = ((ToolStripMenuItem)sender).OwnerItem.Text;
         //   sel_cu_sub_name = ((ToolStripMenuItem)sender).Text;
         //}
      }
      #endregion
   }
}
