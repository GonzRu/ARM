using System.Windows.Forms;

namespace HelperControlsLibrary
{
   public partial class OscDiagPanelControl : UserControl
   {
      #region свойства
      public Button btnReNew
      {
         get
         {
            return btnReNewOD;
         }
      }
      public DateTimePicker dtpStartData
      {
         get{ return dtpstartdata; }
         set{ dtpstartdata = value; }
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
      #endregion

      ErrorProvider erp = new ErrorProvider( );

      public OscDiagPanelControl( )
      {
         InitializeComponent( );
      }
   }
}
