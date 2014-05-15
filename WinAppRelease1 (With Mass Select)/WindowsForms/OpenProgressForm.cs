using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LibraryElements;
using Structure;

namespace WindowsForms
{
   public partial class OpenProgressForm : Form, IProcess
   {
      #region Parameters
      int error_elements, compleated_elements;
      const string str_load = "Loading...";      
      const string str_load_label = "Элементов загружено:";
      const string str_fail = "Проигнорировано:";
      #endregion

      #region Class Methods
      public OpenProgressForm()
      {
         InitializeComponent();
         error_elements = compleated_elements = 0;

         this.Text = str_load;
         label1.Text = str_load_label;
         label3.Text = str_fail;
      }
      
      public void SetMaxValue(int _quant)
      {
         this.progressBar1.Value = 0;
         this.progressBar1.Minimum = 0;
         this.progressBar1.Maximum = _quant;
      }
      public void SetPerformStep()
      {
         this.progressBar1.PerformStep();
         compleated_elements++;

         SetValues();
      }
      public void SetNewError()
      {
         error_elements++;

         SetValues();
      }
      private void SetValues()
      {
         this.label2.Text = compleated_elements.ToString() + " из " + progressBar1.Maximum.ToString();
         this.label4.Text = error_elements.ToString();
         if (this.error_elements > 0)
            this.label4.ForeColor = Color.Red;
         this.Refresh();
      }
      #endregion
   }
}