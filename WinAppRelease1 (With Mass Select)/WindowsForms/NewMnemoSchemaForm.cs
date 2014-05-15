using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Structure;

namespace WindowsForms
{
   public partial class NewMnemoSchemaForm : Form
   {
      Sanction sanct;
      public NewMnemoSchemaForm()
      {
         InitializeComponent();
         sanct = Sanction.Unknown;
      }

      private void CustomSanction()
      {
         if (this.radioButton4.Checked)
         {
            this.numericUpDown1.Enabled = true;
            this.numericUpDown2.Enabled = true;
         }
         else
         {
            this.numericUpDown1.Enabled = false;
            this.numericUpDown2.Enabled = false;
         }
      }
      private void Choise()
      {
         if (this.radioButton1.Checked)
         {
            sanct = Sanction.Sanction_1024;
            return;
         }
         if (this.radioButton2.Checked)
         {
            sanct = Sanction.Sanction_1280;
            return;
         }
         if (this.radioButton3.Checked)
         {
            sanct = Sanction.Sanction_1600;
            return;
         }
         if (this.radioButton4.Checked)
         {
            sanct = Sanction.Custom;
         }
      }
      public int GetCustomSanctWidth()
      {
         return Convert.ToInt32(this.numericUpDown1.Value);
      }
      public int GetCustomSanctHeight()
      {
         return Convert.ToInt32(this.numericUpDown2.Value);
      }
      public Sanction WinSanction
      {
         get
         {
            Choise();
            return sanct;
         }
      }

      private void radioButton4_CheckedChanged(object sender, EventArgs e)
      {
         CustomSanction();
      }
   }
}