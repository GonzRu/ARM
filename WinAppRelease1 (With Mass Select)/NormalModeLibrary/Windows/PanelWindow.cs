using System.Windows.Forms;

using NMLS = NormalModeLibrary.Sources;

namespace NormalModeLibrary.Windows
{
    internal partial class PanelWindow : Form
    {
        public PanelWindow()
        {
            InitializeComponent();
        }
        public void SetPanel( NMLS.Panel panel )
        {
            this.Tag = panel;
            textBox1.Text = panel.Caption;
            checkBox2.Checked = panel.IsCaptionVisible;

            if (panel.IsVisible)
                if (panel.IsAutomaticaly)
                    workModeComboBox.SelectedIndex = 0;
                else
                    workModeComboBox.SelectedIndex = 1;
            else
                workModeComboBox.SelectedIndex = 2;
        }
        public void ApplyData()
        {
            if ( this.Tag == null )
                this.Tag = new NMLS.Panel();

            NMLS.Panel panel = (NMLS.Panel)this.Tag;
            panel.Caption = textBox1.Text;
            panel.IsCaptionVisible = checkBox2.Checked;

            if (workModeComboBox.SelectedIndex == 2)
                panel.IsVisible = false;
            else
            {
                panel.IsVisible = true;
                panel.IsAutomaticaly = (workModeComboBox.SelectedIndex == 0) ? true : false;
            }
        }
        public NMLS.Panel GetPanel()
        {
            ApplyData();
            return (NMLS.Panel)this.Tag;
        }
    }
}
