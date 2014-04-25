using System;
using System.Windows.Forms;

namespace HelperControlsLibrary
{
    public partial class ReadWriteUstavkyControl : UserControl
    {
        public ReadWriteUstavkyControl( ) { InitializeComponent( ); }

        public void ResetButtons()
        {
            btnFix4Change.Checked = false;
        }

        #region Handlers
        private void BtnFix4ChangeCheckedChanged( object sender, EventArgs e )
        {
            var cb = (CheckBox)sender;

            if (cb.Checked)
                cb.Text = "Откл. режим задания уставок";
            else
                cb.Text = "Вкл. режим задания уставок";

            this.btnWriteUst.Enabled = cb.Checked;
        }

        private void BtnWriteUstClick(object sender, EventArgs e)
        {
            /*ResetButtons( );*/
        }

        private void BtnResetValuesClick(object sender, EventArgs e)
        {
            ResetButtons();
        }

        private void BtnReadUstFcClick(object sender, EventArgs e)
        {
            ResetButtons();
        }
        #endregion
    }
}
