using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommonUtils
{
   public partial class dlgLongTimeAction : Form
   {
       public string PreLabelStr
       {
           set
           {
               this.prelabel.Text = value;
           }
       }

      public string ExplanationStr 
      {
         set
         { 
            var lblsize = TextRenderer.MeasureText(value,lblExplanation.Font);
            this.Width = lblsize.Width + 10;
            this.lblExplanation.Text = value;
            this.splitContainer1.SplitterDistance = this.Height - this.Height/15;
         }
      }
      public string PostLabelStr
      {
          set
          {
              this.postlabel.Text = value;
          }
      }

      public string DialogWindowCaption
      {
          set { this.Text = value; }
      }

      public string BtnCaption
      {
          set {  this.btnCancel.Text = value; }
      }

      public dlgLongTimeAction()
      {
         InitializeComponent();
      }

      private void dlgLongTimeAction_DoubleClick(object sender, EventArgs e)
      {
          return;
      }

      private void btnCancel_Click(object sender, EventArgs e)
      {
          Close();
      }
   }
}
