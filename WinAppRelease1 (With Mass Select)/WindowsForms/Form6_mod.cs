using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LibraryElements;
using Structure;

namespace WindowsForms
{
   class Form6_mod : Form6
   {
      #region Parameters
      const string str_save = "Saving...";
      const string str_save_label = "Элементов сохранено:";
      #endregion

      #region Class Methods
      public Form6_mod()
      {
         this.Text = str_save;
         this.label1.Text = str_save_label;
         this.label3.Visible = false;
         this.label4.Visible = false;
      }
      #endregion
   }
}
