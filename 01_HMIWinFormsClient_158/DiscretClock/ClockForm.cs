using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Egida
{
    public partial class ClockForm : Form
    {
        private readonly string path = Environment.CurrentDirectory + @"\Project\Project.cfg";
        private System.Drawing.Point loc;

        public ClockForm() { InitializeComponent(); }

        private void ClockFormLoad( object sender, EventArgs e )
        {
            try
            {
                var doc = XElement.Load( path );
                doc = doc.Element( "Clock" );
                if ( doc == null )
                    loc = new System.Drawing.Point( 100, 50 );
                else
                {
                    var xattr1 = doc.Attribute( "left" );
                    var xattr2 = doc.Attribute( "top" );
                    var x = ( xattr1 != null ) ? int.Parse( xattr1.Value ) : 100;
                    var y = ( xattr2 != null ) ? int.Parse( xattr2.Value ) : 50;
                    loc = new System.Drawing.Point( x, y );
                }
            }
            catch
            {
                loc = new System.Drawing.Point( 100, 50 );
            }

            if (Screen.PrimaryScreen.Bounds.Width - 224 < loc.X)
                loc.X = 0;

            if (Screen.PrimaryScreen.Bounds.Height - 46 < loc.Y)
                loc.Y = 0;
        }

        private void ClockFormShown( object sender, EventArgs e ) { Location = loc; }

        private void ClockFormFormClosing( object sender, FormClosingEventArgs e )
        {
            if ( loc == Location )
                return;

            try
            {
                var doc = XElement.Load( path );
                var elem = doc.Element( "Clock" );
                var objs = new object[] { new XAttribute( "left", Location.X ), new XAttribute( "top", Location.Y ) };

                if ( elem == null )
                    doc.Add( new XElement( "Clock", objs ) );
                else
                    elem.ReplaceAttributes( objs );

                doc.Save( path );
            }
            catch
            {
                MessageBox.Show( "Ошибка записи данных расположения часов", "Ошибка", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error );
            }
        }
    }
}
