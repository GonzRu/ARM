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
   public partial class OpenErrorLogForm : Form, IErrorLog
   {
      bool records;
      public OpenErrorLogForm()
      {
         InitializeComponent();
         records = false;
      }
      public void SetErrorRecord(TreeNode _error_node)
      {
         this.treeView1.Nodes.Add(_error_node);
         records = true;
      }
      public bool RecordsExist
      {
         get { return records; }
      }

      private void button1_Click(object sender, EventArgs e)
      {
         //base.Close();
         base.Dispose();
      }
   }
}