using System;
using System.Drawing;
using System.Windows.Forms;

using LibraryElements;

namespace WindowsForms
{
   public class Form4_mod : Form4
   {
      #region Properties
      ColorDialog clrdial1, clrdial2;
      TabPage tab;
      Label label1, label2, label3;
      Button btn1, btn2;
      #endregion

      #region Class Methods
      public Form4_mod(object _elem, int frameMaxX, int frameMaxY)
          : base(_elem, frameMaxX, frameMaxY)
      {
         Initialization();
         InitPage_mod();
      }
      private void Initialization()
      {
         clrdial1 = new ColorDialog();
         clrdial2 = new ColorDialog();

         btn1 = new Button();
         btn1.Location = new Point(110, 73);
         btn1.Name = "button4";
         btn1.Size = new Size(75, 23);
         btn1.TabIndex = 1;
         btn1.Text = "Изменить";
         btn1.UseVisualStyleBackColor = true;
         btn1.Click += new EventHandler(this.Btn1Click);

         btn2 = new Button();
         btn2.Location = new Point(110, 106);
         btn2.Name = "button5";
         btn2.Size = new Size(75, 23);
         btn2.TabIndex = 2;
         btn2.Text = "Изменить";
         btn2.UseVisualStyleBackColor = true;
         btn2.Click += new EventHandler(this.Btn2Click);

         label1 = new Label();
         label1.AutoSize = true;
         label1.Location = new Point(43, 46);
         label1.Name = "label13";
         label1.Size = new Size(133, 13);
         label1.TabIndex = 0;
         label1.Text = "Фон выбранной фигуры:";

         label2 = new Label();
         label2.AutoSize = true;
         label2.Location = new Point(30, 75);
         label2.Name = "label14";
         label2.Size = new Size(133, 13);
         label2.TabIndex = 0;
         label2.Text = "Верхний цвет:";

         label3 = new Label();
         label3.AutoSize = true;
         label3.Location = new Point(30, 108);
         label3.Name = "label15";
         label3.Size = new Size(133, 13);
         label3.TabIndex = 0;
         label3.Text = "Нижний цвет:";

         tab = new TabPage();
         tab.Controls.Add(btn1);
         tab.Controls.Add(label1);
         tab.Controls.Add(btn2);
         tab.Controls.Add(label2);
         tab.Controls.Add(label3);
         tab.Location = new Point(4, 22);
         tab.Name = "tabPage3";
         tab.Size = new Size(216, 303);
         tab.TabIndex = 4;
         tab.Text = "Фон фигуры";
         tab.UseVisualStyleBackColor = true;

         this.tabControl1.Controls.Add(tab);
      }
      private void InitPage_mod()
      {
         if (SelectElement is EditorFillEllipse)
         {
             clrdial1.Color = ( (EditorFillEllipse)SelectElement ).GetUpColor();
             clrdial2.Color = ( (EditorFillEllipse)SelectElement ).GetDownColor();
         }
         if ( SelectElement is EditorFillRectangle )
         {
             clrdial1.Color = ( (EditorFillRectangle)SelectElement ).GetUpColor();
             clrdial2.Color = ( (EditorFillRectangle)SelectElement ).GetDownColor();
         }
      }
      protected override void ToElement()
      {
         base.ToElement();

         if ( SelectElement is EditorFillEllipse )
             ( (EditorFillEllipse)SelectElement ).SetColor( clrdial1.Color, clrdial2.Color );

         if ( SelectElement is EditorFillRectangle )
             ( (EditorFillRectangle)SelectElement ).SetColor( clrdial1.Color, clrdial2.Color );
      }
      #endregion

      #region Events
      private void Btn1Click(object sender, EventArgs e)
      {
         clrdial1.ShowDialog();
      }
      private void Btn2Click(object sender, EventArgs e)
      {
         clrdial2.ShowDialog();
      }
      protected override void Button2Click(object sender, EventArgs e)
      {
         ToElement();
         Close();
      }
      #endregion
   }
}
