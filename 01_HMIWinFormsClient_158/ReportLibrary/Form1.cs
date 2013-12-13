using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Reporting.WinForms;

namespace ReportLibrary
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var row = DataSet.DataTable1.NewRow();
            row[0] = "group1";
            row[1] = "user1";
            DataSet.DataTable1.Rows.Add( row );
            row = DataSet.DataTable1.NewRow();
            row[0] = "group1";
            row[1] = "user";
            DataSet.DataTable1.Rows.Add( row );

            var row2 = DataSet.DataTable2.NewRow();
            row2[0] = "group1";
            row2[1] = "name1";
            row2[2] = "value1";
            DataSet.DataTable2.Rows.Add( row2 );
            row2 = DataSet.DataTable2.NewRow();
            row2[0] = "group1";
            row2[1] = "name2";
            row2[2] = "value2";
            DataSet.DataTable2.Rows.Add( row2 );
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }
    }
}