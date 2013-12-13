using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LibraryElements;
using FileManager;
using Structure;

namespace ElementEditor
{
   public partial class Form1 : Form
   {
      #region Parameters
      const String str1 = "Были внесены изменения, сохраним?";
      const String str2 = "Новый элемент";
      const String str3 = "Ошибка прав доступа.\nНет возможности сохранить файл с описанием";
      const String str4 = "Ошибка прав доступа.\nНет возможности прочитать файл с описанием";

      GraphicTabPage mytabpage;
      Model selmodel;
      #endregion

      #region Class Methods
      public Form1()
      {
         InitializeComponent();

         InitFileDialog(openFileDialog1);
         InitFileDialog(saveFileDialog1);

         saveFileDialog2.InitialDirectory = Environment.CurrentDirectory;
         saveFileDialog2.RestoreDirectory = true;
         saveFileDialog2.FileName = String.Empty;
         saveFileDialog2.Filter = "Emf files|*.emf";
         saveFileDialog2.FilterIndex = 1;
      }
      /// <summary>
      /// Проверка уже выделенных вариантов отображения
      /// и установка возможностей изменять элементы
      /// </summary>
      private void ReSetCanvasEvents()
      {
         foreach (GraphicTabPage gtp in this.tabControl1.TabPages)
         {
            if (checkBox2.Checked) gtp.ApplyEvents();
            else gtp.CancelEvents();
         }
      }
      /// <summary>
      /// Запуск приложения (все выключено)
      /// </summary>
      private void StartWindow()
      {
         this.button2.Enabled = false;
         this.button3.Enabled = false;
         //this.button4.Enabled = false;//cancel
         this.button5.Enabled = false;
         this.checkBox2.Enabled = false;
         this.textBox2.Enabled = false;
         this.comboBox2.Enabled = false;
         
         this.textBox1.Text = String.Empty;
         this.textBox2.Text = String.Empty;
         EditElement(false);

         this.checkBox1.Checked = false;
         this.checkBox2.Checked = false;
         this.checkedListBox1.Items.Clear();
         this.tabControl1.TabPages.Clear();
         this.comboBox1.SelectedIndex = -1;
         this.comboBox2.SelectedIndex = -1;
         pictureBox2.BackgroundImage = null;
      }
      /// <summary>
      /// Новый элемент отображения
      /// </summary>
      private void NewElement()
      {
         this.button2.Enabled = true;
         this.button3.Enabled = true;
         this.checkBox2.Enabled = true;
         this.checkBox1.Checked = true; //choise group
         this.checkBox2.Checked = true; //edit element

         this.textBox1.Text = str2;
         this.comboBox1.SelectedIndex = -1;
         this.comboBox2.SelectedIndex = -1;
      }
      /// <summary>
      /// Выход из программы
      /// </summary>
      private void CloseWindow()
      {
         base.Close();
      }
      /// <summary>
      /// Выбор какой элемент создается
      /// </summary>
      private void CreateNewElement()
      {
         ElementModel frm = new ElementModel();
         DialogResult res = frm.ShowDialog();

         if (res == DialogResult.OK)
         {
            selmodel = frm.GetModel();
            StartWindow();
            NewElement();
         }
         else StartWindow();
         frm.Dispose();
      }
      /// <summary>
      /// Закрузка элементов редактора
      /// </summary>
      private void LoadElements()
      {
         comboBox1.Items.Clear();
         comboBox1.Items.Add(NameComponents.SingleLine_name);
         comboBox1.Items.Add(NameComponents.PolyLine_name);
         comboBox1.Items.Add(NameComponents.Ellipse_name);
         comboBox1.Items.Add(NameComponents.Rectangle_name);
         comboBox1.Items.Add(NameComponents.FillEllipse_name);
         comboBox1.Items.Add(NameComponents.FillRectangle_name);
         comboBox1.Items.Add(NameComponents.Arc_name + NameComponents.Down_prefix);
         comboBox1.Items.Add(NameComponents.Arc_name + NameComponents.Up_prefix);
         comboBox1.Items.Add(NameComponents.Arc_name + NameComponents.Left_prefix);
         comboBox1.Items.Add(NameComponents.Arc_name + NameComponents.Right_prefix);
      }
      /// <summary>
      /// Проверка статуса изменения
      /// </summary>
      /// <returns></returns>
      private bool ChangeStatus()
      {
         bool change = false;

         foreach (GraphicTabPage gtp in tabControl1.TabPages)
            if (gtp.Change) change = true;

         return change;
      }
      /// <summary>
      /// Редактирование елемента
      /// </summary>
      private void EditElement(bool _flag)
      {
         this.checkedListBox1.Enabled = _flag;
         this.button1.Enabled = _flag;
         this.checkBox1.Enabled = _flag;
         this.textBox1.Enabled = _flag;
         this.comboBox1.Enabled = _flag;

         if (checkBox1.Checked) comboBox2.Enabled = _flag;
         else textBox2.Enabled = _flag;
      }
      /// <summary>
      /// Взять имя категории
      /// </summary>
      /// <returns>строка с именем категории или пустая строка</returns>
      private String GetCategoryString()
      {
         if (checkBox1.Checked)
         {
            if (this.comboBox2.SelectedIndex >= 0)
               return (String)this.comboBox2.SelectedItem;
            else
               return String.Empty;
         }
         else
         {
            if (this.textBox2.Text.Length > 0)
               return this.textBox2.Text;
            else
               return String.Empty;
         }
      }
      /// <summary>
      /// Заполнение структуры данными
      /// </summary>
      //private ElemParam GetElementStructure()
      //{
      //   ElemParam elem = new ElemParam();
      //   elem.ElemName = this.textBox1.Text;
      //   elem.ElemModel = selmodel;
      //   elem.ElemCategory = GetCategoryString();

      //   elem.ElemWidth = Convert.ToInt32(this.numericUpDown1.Value);
      //   elem.ElemHeight = Convert.ToInt32(this.numericUpDown2.Value);

      //   return elem;
      //}
      /// <summary>
      /// Инициализация окна сохранения
      /// </summary>
      private void InitFileDialog(FileDialog _filedialog)
      {
         FileDialog filedialog = _filedialog;
         filedialog.InitialDirectory = Environment.CurrentDirectory;
         filedialog.RestoreDirectory = true;
         filedialog.FileName = String.Empty;
         filedialog.Filter = ProgrammExtensions.GetDescriptionFilter() + ProgrammExtensions.GetAnyFilesFilter();
         filedialog.FilterIndex = 1;
      }
      #endregion

      #region Drawing creation
      private void CreateViewDescription()
      {
         //Пишем имя файла
         if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
         
         //Готовим класс для записи данных в файл
         CreaterDescription creater = new CreaterDescription();

         //Передаем классу имя панели и список элем.-в из чего состоит картинка
         foreach (GraphicTabPage page in this.tabControl1.TabPages)
            creater.CreateImageDescription(page.Text, page.PicList);
         
         //Передаем классу данные элемента
         creater.CreateDescription(this.textBox1.Text);
         //Сохраняем описание элемента
         if (!creater.SaveFile(saveFileDialog1.FileName))
            MessageBox.Show(str3, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

      }
      #endregion

      #region Drawing loading
      private void LoadViewDescription()
      {
         //if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

         //BuilderDescription build = new BuilderDescription(openFileDialog1.FileName);
         //if (build.Error_Status)   //Если нет возможности прочитать файл
         //{                         //сообщаем об этом и уходим
         //   MessageBox.Show(str4, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
         //   return;
         //}

         //build.LoadDescription();

         ////тут оформить распределение данных по форме
      }
      #endregion

      #region Change of an existing drawing
      #endregion

      #region Events
      private void Form1_Load(object sender, EventArgs e)
      {
         LoadElements();
         StartWindow();
      }      
      private void newElementToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (ChangeStatus())
         {
            DialogResult res = MessageBox.Show(str1, "Atention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
               //if (!editflag)
               //   CreateElement();
               //else
               //   SaveChange();
            }//if
         }//if
         CreateNewElement();
      }
      private void openElementToolStripMenuItem_Click(object sender, EventArgs e)
      {
         LoadViewDescription();
      }
      private void closeElementToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (ChangeStatus())
         {
            DialogResult res = MessageBox.Show(str1, "Atention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
               //if (editflag)
               //   SaveChange();
            }
         }
         StartWindow();
      }
      private void exitToolStripMenuItem_Click(object sender, EventArgs e)
      {
         CloseWindow();
      }
      private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
      {
         AboutBox1 frm = new AboutBox1();
         frm.ShowDialog();
      }
      
      private void button1_Click(object sender, EventArgs e)
      {//Select component
         foreach (GraphicTabPage tab in this.tabControl1.TabPages)
         {
            switch ((string)comboBox1.SelectedItem)
            {
               case "Arc (вниз)":
                  {
                     tab.Comp_choise = Components.Arc;
                     tab.Rotate_choise = DrawRotate.Down;
                  } break;
               case "Arc (вверх)":
                  {
                     tab.Comp_choise = Components.Arc;
                     tab.Rotate_choise = DrawRotate.Up;
                  }
                  break;
               case "Arc (влево)":
                  {
                     tab.Comp_choise = Components.Arc;
                     tab.Rotate_choise = DrawRotate.Left;
                  }
                  break;
               case "Arc (вправо)":
                  {
                     tab.Comp_choise = Components.Arc;
                     tab.Rotate_choise = DrawRotate.Right;
                  }
                  break;
               case "Ellipse": tab.Comp_choise = Components.Ellipse; break;
               case "Rectangle": tab.Comp_choise = Components.Rectangle; break;
               case "SingleLine": tab.Comp_choise = Components.SingleLine; break;
               case "PolyLine": tab.Comp_choise = Components.PolyLine; break;
               case "FillEllipse": tab.Comp_choise = Components.FillEllipse; break;
               case "FillRectangle": tab.Comp_choise = Components.FillRectangle; break;
               default: tab.Comp_choise = Components.None; break;
            }//switch
         }//foreach
      }
      private void button2_Click(object sender, EventArgs e)
      {//Preview
         if (this.tabControl1.Controls.Count == 0) return;
         GraphicTabPage tab = (GraphicTabPage)this.tabControl1.SelectedTab;
         List<Element> tmplst = tab.PicList;
         Bitmap bmp = new Bitmap(40, 40);
         Graphics grps = Graphics.FromImage(bmp);
         grps.Clear(pictureBox2.BackColor);

         foreach (Element elem in tmplst)
         {
            elem.Scale = new PointF(0.2f, 0.2f);
            elem.DrawElement(grps);
            elem.Scale = new PointF(1.0f, 1.0f);
         }
         pictureBox2.BackgroundImage = bmp;
      }
      private void button3_Click(object sender, EventArgs e)
      {//Save or save changed
         CreateViewDescription();
      }      
      private void button4_Click(object sender, EventArgs e)
      {//Exit program
         CloseWindow();
      }
      private void button5_Click(object sender, EventArgs e)
      {//Delete select image
         GraphicTabPage gtp = (GraphicTabPage)this.tabControl1.SelectedTab;
         gtp.DeleteImage();
      }      

      private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
      {
         if (((CheckBox)sender).Checked)
         {
            this.textBox2.Enabled = false;
            this.comboBox2.Enabled = true;
         }
         else
         {
            this.textBox2.Enabled = true;
            this.comboBox2.Enabled = false;
         }
      }
      private void checkBox2_CheckedChanged(object sender, System.EventArgs e)
      {
         if (((CheckBox)sender).Checked)
            EditElement(true);
         else
            EditElement(false);

         ReSetCanvasEvents();
      }

      private void checkedListBox1_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
      {
         bool flag = false;
         //string selname;
         //взяли отмеченную строку (имя) из списка
         string selturnstatus = (string)checkedListBox1.SelectedItem;

         //Если устанавливается отметка и строка не отмеченна
         if (e.NewValue == CheckState.Checked && e.CurrentValue == CheckState.Unchecked)
         {
            //смотрим есть ли уже панелька с таким именем или нет
            //если да - устанавливаем флаг в значение true
            for (int i = 0; i < this.tabControl1.Controls.Count; i++)
            {
               if (this.tabControl1.Controls[i].Text == selturnstatus)
                  flag = true;
            }
            //если такой панель еще нет
            if (!flag)
            {
               //создаем панель
               mytabpage = new GraphicTabPage(selturnstatus);
               this.tabControl1.Controls.Add(mytabpage);

               //if (editflag && elemsreader != null)
               //{
               //   selname = (string)comboBox1.SelectedItem;
               //   mytabpage.PicList = elemsreader.LoadImage(selname, selturnstatus);
               //}
            }
         }
         //Если снимается отметка и строка отмеченна
         if (e.NewValue == CheckState.Unchecked && e.CurrentValue == CheckState.Checked)
         {
            //ищем панель по имени и удаляем
            for (int k = 0; k < this.tabControl1.Controls.Count; k++)
            {
               if (selturnstatus == this.tabControl1.Controls[k].Text)
                  this.tabControl1.Controls.RemoveAt(k);
            }
         }         
      }
      
      private void addToolStripMenuItem_Click(object sender, EventArgs e)
      {
        Form2 frm = new Form2();

        if (frm.ShowDialog() == DialogResult.OK)
        {
          this.checkedListBox1.Items.Add(frm.NewTabName, false);
        }
        
        frm.Dispose();
      }
      private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
      {
        if (this.checkedListBox1.SelectedIndex >= 0)
        {
          this.checkedListBox1.SetItemChecked(this.checkedListBox1.SelectedIndex, false);
          this.checkedListBox1.Items.RemoveAt(this.checkedListBox1.SelectedIndex);
        }
      }
      private void editToolStripMenuItem_Click(object sender, EventArgs e)
      {

      }
      private void exportToolStripMenuItem_Click(object sender, EventArgs e)
      {
        if (this.tabControl1.TabCount == 0) return;

        if (this.saveFileDialog2.ShowDialog() == DialogResult.OK)
        {
          GraphicTabPage gtp = (GraphicTabPage)this.tabControl1.SelectedTab;
          Export expt = new Export(gtp.PicList);
          expt.SaveImage(this.saveFileDialog2.FileName);
        }

      }      
      #endregion
   }
}
