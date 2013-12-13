using System;
using System.Windows.Forms;

namespace DeviceFormLib
{
    public partial class SelectControl : UserControl
    {
        public SelectControl()
        {
            InitializeComponent();
        }
        private void SelectUserControlLoad( object sender, EventArgs e )
        {
            dtpStartDateAvar.Value = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0 );
            dtpStartDateAvar.Value = dtpStartDateAvar.Value.AddDays( -1 );
            dtpEndDateAvar.Value = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59 );
            dtpStartTimeAvar.Value = dtpStartDateAvar.Value;
            dtpEndTimeAvar.Value = dtpEndDateAvar.Value;
        }

        internal DateTime StartDateCollapsed
        {
            get
            {
                return new DateTime( dtpStartDateAvar.Value.Year, dtpStartDateAvar.Value.Month, dtpStartDateAvar.Value.Day,
                    dtpStartTimeAvar.Value.Hour, dtpStartTimeAvar.Value.Minute, dtpStartTimeAvar.Value.Second );
            }
        }
        internal DateTime EndTimeCollapsed
        {
            get
            {
                return new DateTime( dtpEndDateAvar.Value.Year, dtpEndDateAvar.Value.Month, dtpEndDateAvar.Value.Day, 
                    dtpEndTimeAvar.Value.Hour, dtpEndTimeAvar.Value.Minute, dtpEndTimeAvar.Value.Second );
            }
        }
    }
}
