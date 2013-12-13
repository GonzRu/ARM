using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelperControlsLibrary
{
    public partial class OperationalControl : UserControl
    {
        private readonly Size singleButtonSize = new Size( 110, 23 );
        private readonly Size doubleButtonSize = new Size( 226, 23 );
        private bool isSingleButton;

        public OperationalControl()
        {
            InitializeComponent();
            button1.Size = button2.Size = singleButtonSize;
        }
        public Control GetPanel() { return mtraNamedFLPanel1; }

        ///<remarks>
        /// Переопределение имени потому,
        /// что имена добавляются в Dictionary,
        /// для помещение туда тэгов по их именам
        ///</remarks>
        public new String Name { get { return mtraNamedFLPanel1.Name; } set { mtraNamedFLPanel1.Name = value; } }
        public String Header { get { return groupBox1.Text; } set { groupBox1.Text = value; } }
        public String Caption { get { return mtraNamedFLPanel1.Caption; } set { mtraNamedFLPanel1.Caption = value; } }
        public String ButtonOneHeader { get { return button1.Text; } set { button1.Text = value; } }
        public String ButtonTwoHeader { get { return button2.Text; } set { button2.Text = value; } }
        public Boolean IsSingleButton
        {
            get { return isSingleButton; }
            set
            {
                isSingleButton = value;
                if ( value )
                {
                    button1.Size = doubleButtonSize;
                    button2.Visible = false;
                }
                else
                {
                    button1.Size = singleButtonSize;
                    button2.Visible = true;
                }
            }
        }

        public event EventHandler ButtonOneClick { add { button1.Click += value; } remove { button1.Click -= value; } }
        public event EventHandler ButtonTwoClick { add { button2.Click += value; } remove { button2.Click -= value; } }
    }
}
