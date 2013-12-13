using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HMI_MT
{
   public class TextBoxEx : TextBox
   {
      protected override void WndProc(ref Message m) 
      {
         if (m.Msg == 0xa1 || m.Msg == 0x201) 
            return; 
         
         base.WndProc (ref m); 
      } 
   }
}
