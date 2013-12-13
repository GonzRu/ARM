using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace BarsMenu
{
   /// <summary>
   /// Класс элементов StatusBar'a
   /// </summary>
   public class StatusBar
   {
      #region Parameters
      private const string str1 = "X: ";
      private const string str2 = "Y: ";
      private const string str3 = "Zoom: ";
      private const string str4 = "Изменений нет";
      private const string str5 = "Были внесены изменения";
      private StatusStrip sb_strip;
      private ToolStripStatusLabel sb_label1;
      private ToolStripStatusLabel sb_label2;
      private ToolStripStatusLabel sb_label3;
      private ToolStripStatusLabel sb_label4;
      private ToolStripStatusLabel sb_label5;
      #endregion

      #region Class Methods
      public StatusBar(StatusStrip _strip)
      {
         sb_strip = _strip;
         InitLabels();
         AddtoStrip();
      }
      public void SetLabel1TextDefault()
      {
         if (this.sb_label1.Text != str4)
            this.sb_label1.Text = str4;
      }
      public void SetLabel1Change()
      {
         if(this.sb_label1.Text != str5)
            this.sb_label1.Text = str5;
      }
      private void InitLabels()
      {
         this.sb_label1 = new ToolStripStatusLabel();
         this.sb_label1.AutoSize = false;
         this.sb_label1.Name = "toolStripStatusLabel1";
         this.sb_label1.Size = new System.Drawing.Size(200, 17);
         this.sb_label1.Text = str4;
         this.sb_label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

         this.sb_label2 = new ToolStripStatusLabel();
         this.sb_label2.Name = "toolStripStatusLabel2";
         this.sb_label2.Size = new System.Drawing.Size(433, 17);
         this.sb_label2.Spring = true;

         this.sb_label3 = new ToolStripStatusLabel();
         this.sb_label3.AutoSize = false;
         this.sb_label3.Name = "toolStripStatusLabel3";
         this.sb_label3.Size = new System.Drawing.Size(90, 17);
         this.sb_label3.Text = str3 + "100%";
         this.sb_label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

         this.sb_label4 = new ToolStripStatusLabel();
         this.sb_label4.AutoSize = false;
         this.sb_label4.Name = "toolStripStatusLabel4";
         this.sb_label4.Size = new System.Drawing.Size(50, 17);
         this.sb_label4.Text = str1;
         this.sb_label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

         this.sb_label5 = new ToolStripStatusLabel();
         this.sb_label5.AutoSize = false;
         this.sb_label5.Name = "toolStripStatusLabel3";
         this.sb_label5.Size = new System.Drawing.Size(50, 17);
         this.sb_label5.Text = str2;
         this.sb_label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      }
      private void AddtoStrip()
      {
         sb_strip.Items.Add(sb_label1);
         sb_strip.Items.Add(sb_label2);
         sb_strip.Items.Add(sb_label3);
         sb_strip.Items.Add(sb_label4);
         sb_strip.Items.Add(sb_label5);
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить или задать текст для "status"
      /// </summary>
      public String Label1_Text
      {
         get { return this.sb_label1.Text; }
         set { this.sb_label1.Text = value; }
      }
      /// <summary>
      /// Получить или задать текст для "Zoom"
      /// </summary>
      public String Label3_Text
      {
         get { return this.sb_label3.Text; }
         set { this.sb_label3.Text = str3 + value; }
      }
      /// <summary>
      /// Получить или задать текст для "X"
      /// </summary>
      public String Label4_Text
      {
         get { return this.sb_label4.Text; }
         set { this.sb_label4.Text = str1 + value; }
      }
      /// <summary>
      /// Получить или задать текст для "Y"
      /// </summary>
      public String Label5_Text
      {
         get { return this.sb_label5.Text; }
         set { this.sb_label5.Text = str2 + value; }
      }
      #endregion
   }
}
