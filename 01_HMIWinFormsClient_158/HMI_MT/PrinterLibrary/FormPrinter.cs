using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace PrinterLibrary
{
    public class FormPrinter : IFormPrint
    {

        public bool Print()
        {
            return false;
        }

        public FontDialog FontDialog { get; private set; }

        static FormPrinter() { Settings = new PrinterSettings(); }
        public static PrinterSettings Settings { get; private set; }
    }
}
