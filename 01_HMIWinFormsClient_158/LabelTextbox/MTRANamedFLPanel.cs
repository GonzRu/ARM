using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace LabelTextbox
{
   public partial class MTRANamedFLPanel : FlowLayoutPanel
   {
      string caption = String.Empty;
      /// <summary>
      /// Понятное название FlowLayoutPanel для осмысленной привязки в интерфейсе
      /// </summary>
      public string Caption
      {
         get
         {
            return caption;
         }
         set
         {
            caption = value;
         }
      }

      public MTRANamedFLPanel( )
      {
         InitializeComponent( );
         InitFLP( );
      }

      public MTRANamedFLPanel( IContainer container )
      {
         container.Add( this );

         InitializeComponent( );
         InitFLP( );
      }

      private void InitFLP( )
      {
         this.AutoScroll = true;
         this.BackColor = SystemColors.Control;
         this.BorderStyle = BorderStyle.FixedSingle;
         this.Dock = DockStyle.Fill;
         this.FlowDirection = FlowDirection.TopDown;
      }
   }
}
