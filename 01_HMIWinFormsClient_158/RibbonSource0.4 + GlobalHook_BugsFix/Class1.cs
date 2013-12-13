using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace System.Windows.Forms
{
    public class RibbonRed : RibbonProfesionalRendererColorTable
    {
        public RibbonRed()
        {
            ButtonSelectedBgOut = Color.DarkRed;
            ButtonSelectedBgCenter = Color.PaleVioletRed;
            ButtonSelectedBorderOut = Color.Red;
            ButtonSelectedBorderIn = Color.Red;
            ButtonSelectedGlossyNorth = Color.Pink;
            ButtonSelectedGlossySouth = Color.LightPink;
        }
    }

    public class RibbonGreen : RibbonProfesionalRendererColorTable
    {
        public RibbonGreen()
        {
            ButtonSelectedBgOut = Color.Green;
            ButtonSelectedBgCenter = Color.Green;
            ButtonSelectedBorderOut = Color.Green;
            ButtonSelectedBorderIn = Color.Green;
            ButtonSelectedGlossyNorth = Color.Green;
            ButtonSelectedGlossySouth = Color.Green;
        }
    }

    public class RibbonYellow : RibbonProfesionalRendererColorTable
    {
        public RibbonYellow()
        {
            ButtonSelectedBgOut = Color.Yellow;
            ButtonSelectedBgCenter = Color.Yellow;
            ButtonSelectedBorderOut = Color.Yellow;
            ButtonSelectedBorderIn = Color.Yellow;
            ButtonSelectedGlossyNorth = Color.Yellow;
            ButtonSelectedGlossySouth = Color.Yellow;
        }
    }
}