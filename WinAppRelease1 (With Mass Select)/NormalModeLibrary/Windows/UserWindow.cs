using System.Windows.Forms;

using NormalModeLibrary.Sources;

namespace NormalModeLibrary.Windows
{
    internal partial class UserWindow : Form
    {
        public UserWindow()
        {
            InitializeComponent();

            comboBox1.Items.AddRange( Sources.Configuration.GetTimeModeNames() );
            comboBox1.SelectedIndex = 0;
        }
        public void SetUser( User user )
        {
            this.Tag = user;
            textBox1.Text = user.Login;
            comboBox1.SelectedItem = user.ActiveTime.ToString();
        }
        public void ApplyData()
        {
            if ( this.Tag == null )
                this.Tag = new User();

            User user = (User)this.Tag;
            user.Login = textBox1.Text;
            user.ActiveTime = Configuration.GetTimeMode( comboBox1.SelectedItem.ToString() );
        }
        public User GetUser()
        {
            ApplyData();
            return (User)this.Tag;
        }
    }
}
