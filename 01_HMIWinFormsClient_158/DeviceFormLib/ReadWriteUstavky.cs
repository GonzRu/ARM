using System;
using System.Windows.Forms;

namespace DeviceFormLib
{
    public partial class ReadWriteUstavkyControl : UserControl
    {
        public ReadWriteUstavkyControl() { InitializeComponent(); }
        private void ResetButtons() { btnFix4Change.Checked = false; }
        private void BtnFix4ChangeCheckedChanged( object sender, EventArgs e )
        {
            var cb = (CheckBox)sender;
            this.btnWriteUst.Enabled = cb.Checked;
        }
        private void BtnWriteUstClick( object sender, EventArgs e ) { ResetButtons(); }
        private void BtnResetValuesClick( object sender, EventArgs e ) { ResetButtons(); }
    }
}
