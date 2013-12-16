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
      private const string _XStr = "X: ";
      private const string _YStr = "Y: ";
      private const string _zoomStr = "Zoom: ";
      private const string _changeStatusNoStr = "Изменений нет";
      private const string _changeStatusYesStr = "Были внесены изменения";
       private const string _countOfSelectedElementsStr = "Выделено элементов: ";
      private StatusStrip sb_strip;
      private ToolStripStatusLabel sb_changeStatusLabel;
      private ToolStripStatusLabel sb_ZoomLabel;
      private ToolStripStatusLabel sb_ZoomValueLabel;
      private ToolStripStatusLabel sb_CursorPositionXLabel;
      private ToolStripStatusLabel sb_CursorPositionYLabel;
       private ToolStripStatusLabel sb_CountOfSelectedElementsLabel;
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
         if (this.sb_changeStatusLabel.Text != _changeStatusNoStr)
            this.sb_changeStatusLabel.Text = _changeStatusNoStr;
      }
      public void SetLabel1Change()
      {
         if(this.sb_changeStatusLabel.Text != _changeStatusYesStr)
            this.sb_changeStatusLabel.Text = _changeStatusYesStr;
      }
      private void InitLabels()
      {
         this.sb_changeStatusLabel = new ToolStripStatusLabel();
         this.sb_changeStatusLabel.AutoSize = false;
         this.sb_changeStatusLabel.Name = "toolStripStatusLabel11";
         this.sb_changeStatusLabel.Size = new System.Drawing.Size(200, 17);
         this.sb_changeStatusLabel.Text = _changeStatusNoStr;
         this.sb_changeStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

         this.sb_ZoomLabel = new ToolStripStatusLabel();
         this.sb_ZoomLabel.Name = "toolStripStatusLabel2";
         this.sb_ZoomLabel.Size = new System.Drawing.Size(433, 17);
         this.sb_ZoomLabel.Spring = true;

         this.sb_ZoomValueLabel = new ToolStripStatusLabel();
         this.sb_ZoomValueLabel.AutoSize = false;
         this.sb_ZoomValueLabel.Name = "toolStripStatusLabel3";
         this.sb_ZoomValueLabel.Size = new System.Drawing.Size(90, 17);
         this.sb_ZoomValueLabel.Text = _zoomStr + "100%";
         this.sb_ZoomValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

         this.sb_CursorPositionXLabel = new ToolStripStatusLabel();
         this.sb_CursorPositionXLabel.AutoSize = false;
         this.sb_CursorPositionXLabel.Name = "toolStripStatusLabel4";
         this.sb_CursorPositionXLabel.Size = new System.Drawing.Size(50, 17);
         this.sb_CursorPositionXLabel.Text = _XStr;
         this.sb_CursorPositionXLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

         this.sb_CursorPositionYLabel = new ToolStripStatusLabel();
         this.sb_CursorPositionYLabel.AutoSize = false;
         this.sb_CursorPositionYLabel.Name = "toolStripStatusLabel3";
         this.sb_CursorPositionYLabel.Size = new System.Drawing.Size(50, 17);
         this.sb_CursorPositionYLabel.Text = _YStr;
         this.sb_CursorPositionYLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

          this.sb_CountOfSelectedElementsLabel = new ToolStripStatusLabel();
          this.sb_CountOfSelectedElementsLabel.AutoSize = true;
          this.sb_CountOfSelectedElementsLabel.Name = "CountOfSelectedElementsLabel";
          this.sb_CountOfSelectedElementsLabel.Size = new System.Drawing.Size(200, 17);
          this.sb_CountOfSelectedElementsLabel.Text = _countOfSelectedElementsStr + "0";
          this.sb_CountOfSelectedElementsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

      }
      private void AddtoStrip()
      {
         sb_strip.Items.Add(sb_changeStatusLabel);
         sb_strip.Items.Add(sb_CountOfSelectedElementsLabel);
         sb_strip.Items.Add(sb_ZoomLabel);
         sb_strip.Items.Add(sb_ZoomValueLabel);
         sb_strip.Items.Add(sb_CursorPositionXLabel);
         sb_strip.Items.Add(sb_CursorPositionYLabel);
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить или задать текст для "status"
      /// </summary>
      public String ChangeStatusLabelText
      {
         get { return this.sb_changeStatusLabel.Text; }
         set { this.sb_changeStatusLabel.Text = value; }
      }
      /// <summary>
      /// Получить или задать текст для "Zoom"
      /// </summary>
      public String ZoomValueLabelText
      {
         get { return this.sb_ZoomValueLabel.Text; }
         set { this.sb_ZoomValueLabel.Text = _zoomStr + value; }
      }
      /// <summary>
      /// Получить или задать текст для "X"
      /// </summary>
      public String CursorPositionXLabelText
      {
         get { return this.sb_CursorPositionXLabel.Text; }
         set { this.sb_CursorPositionXLabel.Text = _XStr + value; }
      }
      /// <summary>
      /// Получить или задать текст для "Y"
      /// </summary>
      public String CursorPositionYLabelText
      {
         get { return this.sb_CursorPositionYLabel.Text; }
         set { this.sb_CursorPositionYLabel.Text = _YStr + value; }
      }

       /// <summary>
       /// Получить или задать текст для "Выделено элементов:"
       /// </summary>
       public String CountOfSelectedelementsLabelText
       {
           get { return this.sb_CountOfSelectedElementsLabel.Text;  }
           set { this.sb_CountOfSelectedElementsLabel.Text = _countOfSelectedElementsStr + value.ToString();  }
       }

       #endregion
   }
}
