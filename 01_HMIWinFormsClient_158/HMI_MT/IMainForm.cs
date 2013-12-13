using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMI_MT
{
    interface IMainForm
    {
        PrintHMI PRT { get; }
        int Width { get; }
        int Height { get; }
        bool isBDConnection { get; }

        /// <summary>
        /// Print
        /// </summary>
        void MenuButtonPrintClick( object sender, EventArgs e );
        /// <summary>
        /// Page setup
        /// </summary>
        void MenuButtonPageSetupClick( object sender, EventArgs e );
        /// <summary>
        /// Preview page
        /// </summary>
        void MenuButtonPreviewPageClick( object sender, EventArgs e );
    }
}
