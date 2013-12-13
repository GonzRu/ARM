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
   public partial class LogDevPanelControl : UserControl
   {
      #region свойства
      public Button btnReNew
      {
         get
         {
            return btnReNewLogDev;
         }
      }
      public DateTimePicker dtpStartData
      {
         get
         {
            return dtpstartdata;
         }
         set
         {
            dtpstartdata = value;
         }
      }
      public DateTimePicker dtpStartTime
      {
         get
         {
            return dtpstarttime;
         }
         set
         {
            dtpstarttime = value;
         }
      }
      public DateTimePicker dtpEndData
      {
         get
         {
            return dtpenddata;
         }
         set
         {
            dtpenddata = value;
         }
      }
      public DateTimePicker dtpEndTime
      {
         get
         {
            return dtpendtime;
         }
         set
         {
            dtpendtime = value;
         }
      }
      public TextBox TbChIdFrmt
      {
         get
         {
            return tbChIdFrmt;
         }
      }
      public TextBox TbDateTimeWrFileClockRTU
      {
         get
         {
            return tbDateTimeWrFileClockRTU;
         }
      }
      public TextBox TbNameDev
      {
         get
         {
            return tbNameDev;
         }
      }
      public TextBox TbNum2Header
      {
         get
         {
            return tbNum2Header;
         }
      }
      public TextBox TbNumByteInEachEventRecord
      {
         get
         {
            return tbNumByteInEachEventRecord;
         }
      }
      public TextBox TbNumEvent
      {
         get
         {
            return tbNumEvent;
         }
      }
      public TextBox TbNumUVS
      {
         get
         {
            return tbNumUVS;
         }
      }
      public TextBox TbNumRTU
      {
         get
         {
            return tbNumRTU;
         }
      }
      public TextBox TbReasonUnload
      {
         get
         {
            return tbReasonUnload;
         }
      }
      #endregion

      public LogDevPanelControl( )
      {
         InitializeComponent( );
      }
   }
}
