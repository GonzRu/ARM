﻿using System;
using System.Windows.Forms;

namespace ReportLibrary
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles( );
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new Form1( ) );
        }
    }
}