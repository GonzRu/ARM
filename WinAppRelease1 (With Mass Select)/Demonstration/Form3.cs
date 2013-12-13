using System;
using System.Windows.Forms;

namespace Demonstration
{
    public partial class Form3 : Form
    {
        private readonly String[] elements = new[] { "Element1", "Element2", "Element3" };
        
        public Form3()
        {
            InitializeComponent();

            var newRow = new DataGridViewRow( );
            newRow.Cells.Add( new DataGridViewTextBoxCell { Value = "Test Column 01" } );
            var cell = new DataGridViewComboBoxCell();
            newRow.Cells.Add( cell );
            foreach ( var value in elements ) cell.Items.Add( value );
            cell.Value = elements[0];
            newRow.Cells.Add( new DataGridViewTextBoxCell { Value = "Test Column 03" } );

            dataGridView1.Rows.Add( newRow );
        }

        private void ButtonClick( object sender, EventArgs e )
        {
            var button = (Button)sender;
            dataGridView1.Rows[0].Cells[1].Value = button.Tag.ToString();
        }
    }

    public class MyClass : SplitContainer
    {
        public MyClass()
        {
            this.SplitterMoving += delegate( object sender, SplitterCancelEventArgs args )
                                       {
                                           
                                       };
        }
    }
}
