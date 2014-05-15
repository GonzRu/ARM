using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LibraryElements;

namespace WindowsForms
{
  public partial class Form5 : Form
  {
    private FormText orgtext;

    #region Class Methods
    public Form5(FormText _obj)
    {
      InitializeComponent();
      orgtext = _obj;
      SetTextParameters();
    }
    private void SetTextParameters()
    {
       this.textBox1.Text = orgtext.Text;
       this.textBox1.Font = orgtext.TextFont;
       this.textBox1.ForeColor = orgtext.ElementColor;

       this.fontDialog1.Font = orgtext.TextFont;
       this.colorDialog1.Color = orgtext.ElementColor;
       this.checkBox1.Checked = orgtext.VerticalView;
    }

    private void button2_Click(object sender, EventArgs e)
    {
      base.Close();
    }
    private void button3_Click(object sender, EventArgs e)
    {
      if (this.fontDialog1.ShowDialog() == DialogResult.OK)
      {
        this.textBox1.Font = this.fontDialog1.Font;
      }
    }
    private void button4_Click(object sender, EventArgs e)
    {
      if (this.colorDialog1.ShowDialog() == DialogResult.OK)
      {
        this.textBox1.ForeColor = this.colorDialog1.Color;
      }
    }
    private void button1_Click(object sender, EventArgs e)
    {
       orgtext.Text = this.textBox1.Text;
       orgtext.TextFont = this.textBox1.Font;
       orgtext.ElementColor = this.textBox1.ForeColor;
       orgtext.VerticalView = this.checkBox1.Checked;
    }
    #endregion
  }
}