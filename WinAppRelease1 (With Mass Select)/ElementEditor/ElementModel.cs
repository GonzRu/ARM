using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Structure;

namespace ElementEditor
{
   public partial class ElementModel : Form
   {
      private static String str1 = "Статический элемент";
      private static String str2 = "Динамический элемент";

      public ElementModel()
      {
         InitializeComponent();

         comboBox1.Items.Add(str1);
         comboBox1.Items.Add(str2);
      }
      public Model GetModel()
      {
         if (this.comboBox1.SelectedIndex == 0) return Model.Static;
         if (this.comboBox1.SelectedIndex == 1) return Model.Dynamic;

         return Model.Static;
      }
   }
}
