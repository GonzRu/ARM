using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NormalModeLibrary.Windows
{
    static public class NormalModePanelFactory
    {
        static public INormalModePanel CreatePanel(string panelType)
        {
            switch (panelType)
            {
                case "windowAndWPFPanel":
                    return new ViewWindow();
                case "controlAndWPFPanel":
                    return new ViewElementHost();
                case "controlAndWinFormPanel":
                    return new ViewUserControl();
            }

            return null;
        }
    }
}
