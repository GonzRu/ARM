using System.Windows.Forms;

namespace DeviceFormLib.BlockTabs
{
    public partial class EventBlockControl : UserControl
    {
        public EventBlockControl( int uniDevId )
        {
            InitializeComponent();
            panelBottomEventBlock1.InitPanel( uniDevId, lstvEventBlock );
        }
    }
}
