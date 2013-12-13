using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinAppRelease1
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MDIParent1() );
        }
    }
}