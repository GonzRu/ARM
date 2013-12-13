using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NMLS = NormalModeLibrary.Sources;

namespace NormalModeLibrary.Windows
{
    internal partial class PanelWindow : Form
    {
        public PanelWindow()
        {
            InitializeComponent();
            numericUpDown1.Maximum = numericUpDown2.Maximum = numericUpDown3.Maximum =
                numericUpDown4.Maximum = numericUpDown5.Maximum = decimal.MaxValue;
        }
        public void SetPanel( NMLS.Panel panel )
        {
            this.Tag = panel;
            textBox1.Text = panel.Caption;
            checkBox1.Checked = panel.IsVisible;
            switch ( panel.Type )
            {
                case NMLS.Panel.LinkType.Free: radioButton1.Checked = true; break;
                case NMLS.Panel.LinkType.Named: radioButton2.Checked = true; break;
                default: break;
            }
            numericUpDown5.Value = panel.ObjectGuid;
            checkBox2.Checked = panel.IsCaptionVisible;
            numericUpDown3.Value = panel.Width;
            numericUpDown4.Value = panel.Height;
            numericUpDown1.Value = panel.Left;
            numericUpDown2.Value = panel.Top;
        }
        public void ApplyData()
        {
            if ( this.Tag == null )
                this.Tag = new NMLS.Panel();

            NMLS.Panel panel = (NMLS.Panel)this.Tag;
            panel.Caption = textBox1.Text;
            panel.IsVisible = checkBox1.Checked;
            panel.Type = ( radioButton1.Checked ) ? NMLS.Panel.LinkType.Free : NMLS.Panel.LinkType.Named;
            panel.ObjectGuid = (uint)numericUpDown5.Value;
            panel.IsCaptionVisible = checkBox2.Checked;
            panel.Width = (int)numericUpDown3.Value;
            panel.Height = (int)numericUpDown4.Value;
            panel.Left = (int)numericUpDown1.Value;
            panel.Top = (int)numericUpDown2.Value;
        }
        public NMLS.Panel GetPanel()
        {
            ApplyData();
            return (NMLS.Panel)this.Tag;
        }
    }
}
