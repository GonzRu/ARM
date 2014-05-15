using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsForms
{
   public partial class EditMnemoCaptionForm : Form
   {
      public EditMnemoCaptionForm()
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
