using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

using Structure;

namespace BarsMenu
{
   public class ElementConMenu : ContextMenuStrip
   {
      #region Properties
      private ToolStripMenuItem ecm_menuitem;
      private ToolStripSeparator ecm_separator;
      #endregion

      #region Class Methods
      public ElementConMenu()
      {
         this.RenderMode = ToolStripRenderMode.System;
         this.Size = new Size(120, 140);
      }
      public void CreateItem(string _menu_name)
      {
         if (_menu_name == null || _menu_name == String.Empty) return;

         ecm_menuitem = new ToolStripMenuItem(_menu_name);
         ecm_menuitem.Name = _menu_name;
         ecm_menuitem.Click += new EventHandler(Event_menuitem);
         this.Items.Add(ecm_menuitem);
      }
      public void CreateSeparator()
      {
         ecm_separator = new ToolStripSeparator();
         this.Items.Add(ecm_separator);
      }
      #endregion

      #region Events
      private void Event_menuitem(object sender, EventArgs e) { }
      #endregion
   }
}
