using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NormalModeLibrary.Sources;

namespace NormalModeLibrary.Windows
{
    internal partial class ConfigurationWindow : Form
    {
        public ConfigurationWindow()
        {
            InitializeComponent();

            comboBox1.Items.AddRange( Sources.Configuration.GetTimeModeNames() );
            comboBox2.Items.AddRange( Sources.Configuration.GetPlaceNames() );
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = 0;
        }
        public void SetConfiguration( Configuration configuration )
        {
            this.Tag = configuration;
            comboBox1.SelectedItem = configuration.ActiveTime;
            comboBox2.SelectedItem = configuration.Place;
            checkBox1.Checked = configuration.IsActive;
        }
        public void ApplyData()
        {
            if ( this.Tag == null )
                this.Tag = new Configuration();

            Configuration configuration = (Configuration)this.Tag;
            configuration.ActiveTime = Configuration.GetTimeMode( comboBox1.SelectedItem.ToString() );
            configuration.Place = Configuration.GetPlace( comboBox2.SelectedItem.ToString() );
            configuration.IsActive = checkBox1.Checked;
        }
        public Configuration GetConfiguration()
        {
            ApplyData();
            return (Configuration)this.Tag;
        }
    }
}
