using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsForms
{
   public partial class Form13 : Form
   {
      public Form13()
      {
         InitializeComponent();
      }
      /// <summary>
      /// Получить или задать имя описания схемы
      /// </summary>
      public String MnenoCaption
      {
         get { return this.textBox1.Text; }
         set { this.textBox1.Text = value; }
      }
   }
}
