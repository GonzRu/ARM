using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NormalModeLibrary.Sources;

namespace NormalModeLibrary.Windows
{
    internal partial class SignalWindow : Form
    {
        public SignalWindow()
        {
            InitializeComponent();

            comboBox1.Items.AddRange( BaseSignal.GetSignalTypeNames() );
            comboBox1.SelectedIndex = 0;
            colorDialog1.Color = GetWinFormColor( DigitalSignal.GetDefaultColorOn() );
            colorDialog2.Color = GetWinFormColor( DigitalSignal.GetDefaultColorOff() );
            label6.ForeColor = colorDialog1.Color;
            label7.ForeColor = colorDialog2.Color;
            numericUpDown1.Minimum = numericUpDown2.Minimum = numericUpDown3.Minimum = numericUpDown4.Minimum = decimal.MinValue;
            numericUpDown1.Maximum = numericUpDown2.Maximum = numericUpDown3.Maximum = numericUpDown4.Maximum = decimal.MaxValue;
            numericUpDown1.Value = (decimal)BaseRange.DefaultRangeMinValue;
            numericUpDown2.Value = (decimal)BaseRange.DefaultRangeMaxValue;
            numericUpDown3.Value = (decimal)HysteresisRange.DefaultRangeMinHysteresis;
            numericUpDown4.Value = (decimal)HysteresisRange.DefaultRangeMaxHysteresis;
        }
        public void SetSignal( BaseSignal signal )
        {
            this.Tag = signal;
            Sources.BaseSignal.SignalType signalType = BaseSignal.SignalType.Unknown;
            
            if ( signal is AnalogSignal ) signalType = BaseSignal.SignalType.Analog;
            else if ( signal is DigitalSignal ) signalType = BaseSignal.SignalType.Discret;

            switch ( signalType )
            {
                case BaseSignal.SignalType.Analog:
                    comboBox1.SelectedIndex = 0;
                    comboBox1.Enabled = false;

                    numericUpDown1.Value = (decimal)((AnalogSignal)signal).Range.RangeMinValue;
                    numericUpDown2.Value = (decimal)((AnalogSignal)signal).Range.RangeMaxValue;
                    numericUpDown3.Value = (decimal)((AnalogSignal)signal).Range.RangeMinHysteresis;
                    numericUpDown4.Value = (decimal)((AnalogSignal)signal).Range.RangeMaxHysteresis;
                    break;
                case BaseSignal.SignalType.Discret:
                    comboBox1.SelectedIndex = 1;
                    comboBox1.Enabled = false;

                    textBox2.Enabled = false;

                    colorDialog1.Color = GetWinFormColor( ( (DigitalSignal)signal ).SignalOn );
                    colorDialog2.Color = GetWinFormColor( ( (DigitalSignal)signal ).SignalOff );
                    break;
                default:
                    throw new NotImplementedException();
            }

            textBox1.Text = signal.Caption;
            textBox2.Text = signal.Dim;
            textBox3.Text = signal.Commentary;
            textBox4.Text = signal.Guid.ToString();

            textBox4.Enabled = false;

        }
        public void ApplyData()
        {
            Sources.BaseSignal.SignalType signalType = Sources.BaseSignal.GetSignalType( comboBox1.SelectedItem.ToString() );
            
            switch ( signalType )
            {
                case Sources.BaseSignal.SignalType.Discret:
                    if ( this.Tag == null )
                        this.Tag = new DigitalSignal();
                    
                    DigitalSignal digit = (DigitalSignal)this.Tag;
                    digit.Caption = textBox1.Text;
                    digit.Dim = textBox2.Text;
                    digit.Commentary = textBox3.Text;
                    digit.Guid = uint.Parse( textBox4.Text );
                    digit.SignalOn = GetWPFFormColor( colorDialog1.Color );
                    digit.SignalOff = GetWPFFormColor( colorDialog2.Color );
                    break;

                case Sources.BaseSignal.SignalType.Analog:
                    if (this.Tag == null)
                        this.Tag = new AnalogSignal();

                    AnalogSignal analog = (AnalogSignal)this.Tag;
                    analog.Caption = textBox1.Text;
                    analog.Dim = textBox2.Text;
                    analog.Commentary = textBox3.Text;
                    analog.Guid = uint.Parse( textBox4.Text );
                    analog.Range.RangeMinValue = (double)numericUpDown1.Value;
                    analog.Range.RangeMaxValue = (double)numericUpDown2.Value;
                    analog.Range.RangeMinHysteresis = (double)numericUpDown3.Value;
                    analog.Range.RangeMaxHysteresis = (double)numericUpDown4.Value;
                    break;

                default: break;
            }
        }
        public BaseSignal GetSignal()
        {
            ApplyData();
            return (BaseSignal)this.Tag;
        }

        private void comboBox1_SelectedIndexChanged( object sender, EventArgs e )
        {
            string type = ( (ComboBox)sender ).SelectedItem.ToString();
            switch ( BaseSignal.GetSignalType( type ) )
            {
                case BaseSignal.SignalType.Discret:
                    {
                        groupBox1.Enabled = true;
                        groupBox2.Enabled = false;
                    }
                    break;
                case BaseSignal.SignalType.Analog:
                    {
                        groupBox1.Enabled = false;
                        groupBox2.Enabled = true;
                    }
                    break;
                default:
                    {
                        groupBox1.Enabled = false;
                        groupBox2.Enabled = false;
                    }
                    break;
            }
        }
        private void button3_Click( object sender, EventArgs e )
        {
            if ( colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                label6.ForeColor = colorDialog1.Color;
        }
        private void button4_Click( object sender, EventArgs e )
        {
            if ( colorDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                label7.ForeColor = colorDialog2.Color;
        }

        private static System.Drawing.Color GetWinFormColor( System.Windows.Media.Color color )
        {
            return System.Drawing.Color.FromArgb( color.R, color.G, color.B );
        }
        private static System.Windows.Media.Color GetWPFFormColor( System.Drawing.Color color )
        {
            return System.Windows.Media.Color.FromRgb( color.R, color.G, color.B );
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BaseSignal signal = (BaseSignal)this.Tag;

            signal.Caption = textBox1.Text;
            signal.Dim = textBox2.Text;
            signal.Commentary = textBox3.Text;

            BaseSignal.SignalType signalType = BaseSignal.SignalType.Unknown;
            if (signal is AnalogSignal)
                signalType = BaseSignal.SignalType.Analog;
            else if (signal is DigitalSignal)
                signalType = BaseSignal.SignalType.Discret;

            switch (signalType)
            {
                case BaseSignal.SignalType.Analog:
                    var analogSignal = signal as AnalogSignal;

                    analogSignal.Range.RangeMinValue = (double)numericUpDown1.Value;
                    analogSignal.Range.RangeMaxValue = (double)numericUpDown2.Value;
                    analogSignal.Range.RangeMinHysteresis = (double)numericUpDown3.Value;
                    analogSignal.Range.RangeMaxHysteresis = (double)numericUpDown4.Value;
                    break;
                case BaseSignal.SignalType.Discret:
                    var discretSignal = signal as DigitalSignal;

                    discretSignal.SignalOn = GetWPFFormColor(colorDialog1.Color);
                    discretSignal.SignalOff = GetWPFFormColor(colorDialog2.Color);
                    break;
            }

            NormalModeLibrary.ComponentFactory.Factory.SaveXml();
            Close();
        }
    }
}
