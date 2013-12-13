using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LabelTextbox
{
    public partial class ComboBoxListWindow : Form
    {
        readonly string[] arrStrCbEnum;
        readonly Dictionary<string, RadioButton> dictStrToRb;
        string caption;

        public ComboBoxListWindow( string paramName, string[] arrStrCbEnum, string activeStr )
        {
            InitializeComponent();
            dictStrToRb = new Dictionary<string, RadioButton>();
            caption = paramName;
            this.arrStrCbEnum = arrStrCbEnum;
            ActiveItemInCB = activeStr;
        }

        private void ComboBoxListWindowLoad( object sender, System.EventArgs e )
        {
            int j = 0;
            int cntItemInRow = 10;

            this.Cursor = Cursors.Hand;

            if ( arrStrCbEnum.Count() > 70 )
                cntItemInRow = 70;

            for ( int i = 0; i < arrStrCbEnum.Count(); i++ )
            {
                RadioButton rb = new RadioButton();
                rb.Text = arrStrCbEnum[i];
                rb.AutoSize = true;
                rb.Tag = i;

                if ( j > cntItemInRow )
                {
                    flpForEnum.SetFlowBreak( rb, true );
                    j = 0;
                }

                flpForEnum.Controls.Add( rb );
                rb.CheckedChanged += ( senderCheckBox, args ) => ActiveItemInCB = ( (RadioButton)senderCheckBox ).Text;

                if ( !dictStrToRb.ContainsKey( arrStrCbEnum[i] ) )
                    dictStrToRb.Add( arrStrCbEnum[i], rb );
                //else
                //    MessageBox.Show( "Ключ уже содержится в словаре. Ключ = " + arrstrCBEnum[i], "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                j++;
            }

            if ( dictStrToRb.ContainsKey( ActiveItemInCB ) )
                ( dictStrToRb[ActiveItemInCB] as RadioButton ).Checked = true;
        }

        /// <summary>
        /// текущий активный элемент перечисления
        /// </summary>
        public string ActiveItemInCB { get; set; }
    }
}
