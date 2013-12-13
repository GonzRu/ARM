using System;
using System.Windows.Forms;

namespace DebugStatisticLibrary
{
    public interface IDebugStatistics
    {
        void AddStatistic( String message );
    }

    public partial class DebugStatistics : Form, IDebugStatistics
    {
        private static IDebugStatistics window;

        private DebugStatistics()
        {
            InitializeComponent();

            Closed += ( sender, args ) => { window = null; };
        }
        public void AddStatistic( String message )
        {
            // если раскомментировать, то будет выводиться наглядная статистика
            // так как проект не собрать под релизом (из-за того, что проект использует старые dll не через референс), для скрытия, приходиться комментировать данный кусок кода
//#if DEBUG
//            richTextBox1.Text += string.Format( "Время действия: {0}\nСообщение статистики: {1}\n\n", DateTime.Now, message );
//            richTextBox1.Focus( );
//            richTextBox1.Select( richTextBox1.Text.Length, 0 );
//            richTextBox1.Refresh( );

//            if (!Visible) Show( );
//#endif
        }

        public static IDebugStatistics WindowStatistics
        {
            get { return window ?? ( window = new DebugStatistics( ) ); }
        }
    }
}
