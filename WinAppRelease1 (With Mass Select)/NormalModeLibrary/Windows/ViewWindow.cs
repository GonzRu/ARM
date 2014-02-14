using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace NormalModeLibrary.Windows
{
    public partial class ViewWindow : Form
    {
        delegate void CheckRangeDelegate();

        internal Boolean IsEditable { get; set; }
        internal Places Place { get; set; }

        private bool _isAlarmMode = false;

        #region Constructors
        public ViewWindow()
        {
            InitializeComponent();
            elementHost1.Child = new TableControl(this);
            elementHost1.Child.MouseDown += ChildOnMouseDown;
        }
        #endregion

        #region Public Metods
        public void ActivatedComponent()
        {
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            if ( Component != null )
            {
                bool needToShow = true;

                this.Text = Component.Caption;
                this.tableLayoutPanel1.RowStyles[1].Height = 0;                

                foreach (ViewModel.BaseSignalViewModel vmSignal in Component.Collection)
                {
                    ViewModel.IOutOfRangeHandler ioorh = vmSignal as ViewModel.IOutOfRangeHandler;
                    if (ioorh != null && !ioorh.IsOutOfRangeEvent)
                        ioorh.OutOfRangeEvent += ViewWindow_OutOfRangeEvent;
                }

                if (Component.IsAutomaticaly)
                    needToShow = false;

                this.Top = Component.Top;
                this.Left = Component.Left;
                this.Width = Component.Width;
                this.Height = Component.Height;

                if (needToShow)
                    Show();
            }
        }
        public void DeactivatedComponent()
        {
            if ( !IsEditable && Component != null )
                foreach ( ViewModel.BaseSignalViewModel vmSignal in Component.Collection )
                {
                    ViewModel.IOutOfRangeHandler ioorh = vmSignal as ViewModel.IOutOfRangeHandler;
                    if ( ioorh != null && ioorh.IsOutOfRangeEvent )
                        ioorh.OutOfRangeEvent -= ViewWindow_OutOfRangeEvent;
                }

            this.Close();
        }

        public void SetOnEditMode()
        {
            foreach (ViewModel.BaseSignalViewModel vmSignal in Component.Collection)
            {
                ViewModel.IOutOfRangeHandler ioorh = vmSignal as ViewModel.IOutOfRangeHandler;
                if (ioorh != null && !ioorh.IsOutOfRangeEvent)
                    ioorh.OutOfRangeEvent -= ViewWindow_OutOfRangeEvent;
            }
        }

        public void SetOffEditMode()
        {
            foreach (ViewModel.BaseSignalViewModel vmSignal in Component.Collection)
            {
                ViewModel.IOutOfRangeHandler ioorh = vmSignal as ViewModel.IOutOfRangeHandler;
                if (ioorh != null && !ioorh.IsOutOfRangeEvent)
                    ioorh.OutOfRangeEvent += ViewWindow_OutOfRangeEvent;
            }
        }

        public void UpdateWorkMode()
        {
            if (Component.IsVisible)
            {
                if (Component.IsAutomaticaly)
                    Hide();
            }
            else
                Hide();
        }

        public void ShowIfNeed()
        {
            if (Component.IsVisible)
                if (!Component.IsAutomaticaly || _isAlarmMode)
                {
                    Show();
                    Application.OpenForms[0].Activate();
                }
        }

        internal ViewModel.PanelViewModel Component
        {
            get
            {
                TableControl tc = (TableControl)elementHost1.Child;
                return (ViewModel.PanelViewModel)tc.DataContext;
            }
            set
            {
                TableControl tc = (TableControl)elementHost1.Child;
                tc.DataContext = value;
            }
        }
        #endregion

        #region Private Metods
        private void ViewWindow_OutOfRangeEvent(object sender, EventArgs e)
        {
            NormalModeLibrary.Sources.OutOfRangeEventArgs orea = (NormalModeLibrary.Sources.OutOfRangeEventArgs)e;

            if (orea.OutOfRange)
            {
                if (!_isAlarmMode)
                {
                    //System.Windows.Threading.Dispatcher.CurrentDispatcher.
                    BeginInvoke(new System.Threading.ThreadStart(delegate
                    {
                        if (Component.IsAutomaticaly && !Visible)
                            Hide();
                    }));

                    _isAlarmMode = true;
                    this.Height += 31;
                    tableLayoutPanel1.RowStyles[1].Height = 31;

                    SoundSystem.System.Play();
                }
            }
        }

        private void ViewWindow_Shown( object sender, EventArgs e )
        {
            //ActivatedComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            SoundSystem.System.Stop();
            this.Hide();
        }

        private void AlarmButton_Click(object sender, EventArgs e)
        {
            this.Height -= 31;
            tableLayoutPanel1.RowStyles[1].Height = 0;
            SoundSystem.System.Stop();
            _isAlarmMode = false;

            if (Component.IsAutomaticaly)
                Hide();
        }
        #endregion

        #region Override Metdods
        protected override void OnClosing(CancelEventArgs e)
        {
            //if (!_isAlarmMode)
            //{
            //    Component.IsAutomaticaly = true;
            //    Hide();
            //}
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);

            Component.Width = this.Width;
            Component.Height = this.Height;
            Component.Top = this.Top;
            Component.Left = this.Left;

            NormalModeLibrary.ComponentFactory.Factory.SaveXml();

            Application.OpenForms[0].Activate();
        }

        public void ChildOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Capture = false;

            Message msg = Message.Create(Handle, 0xA1, (IntPtr) 0x2, IntPtr.Zero);
            base.WndProc(ref msg);
        }

        #endregion
    }
}