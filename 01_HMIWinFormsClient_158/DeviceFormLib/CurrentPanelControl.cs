using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceFormLib
{
   public partial class CurrentPanelControl : UserControl
   {
      public Panel PnlImgDev
      {
         get
         {
            return pnlSwitchPosition;
         }
      }
      public CurrentPanelControl( )
      {
         InitializeComponent( );
      }
   }
}
