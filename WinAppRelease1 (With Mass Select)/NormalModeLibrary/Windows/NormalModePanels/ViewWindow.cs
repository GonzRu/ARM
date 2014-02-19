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
using NormalModeLibrary.ViewModel;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace NormalModeLibrary.Windows
{
    public partial class ViewWindow : Form, INormalModePanel
    {
        delegate void CheckRangeDelegate();

        internal Boolean IsEditable { get; set; }
        public Places Place { get; set; }

        private bool _isAlarmMode = false;

        #region Constructors
        public ViewWindow()
        {
            InitializeComponent();
            elementHost1.Child = new TableControl();
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

                if (Component.IsAutomaticaly)
                    needToShow = false;

                this.Top = Component.Top;
                this.Left = Component.Left;
                this.Width = Component.Width;
                this.Height = Component.Height;

                foreach (var viewModel in Component.Collection)
                {
                    var analogViewModel = viewModel as AnalogViewModel;
                    if (analogViewModel != null)
                        analogViewModel.OutOfRangeEvent += OutOfRangeEvent;
                }

                if (needToShow)
                    Show();
            }
        }
        public void DeactivatedComponent()
        {
            this.Close();
        }

        public void SetOnEditMode()
        {
            ((TableControl)elementHost1.Child).mainListBox.Background = System.Windows.Media.Brushes.Yellow;

            foreach (var viewModel in Component.Collection)
            {
                var analogViewModel = viewModel as AnalogViewModel;
                if (analogViewModel != null)
                    analogViewModel.OutOfRangeEvent -= OutOfRangeEvent;
            }
        }

        public void SetOffEditMode()
        {
            ((TableControl)elementHost1.Child).mainListBox.Background = System.Windows.Media.Brushes.White;

            foreach (var viewModel in Component.Collection)
            {
                var analogViewModel = viewModel as AnalogViewModel;
                if (analogViewModel != null)
                    analogViewModel.OutOfRangeEvent += OutOfRangeEvent;
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

        public ViewModel.PanelViewModel Component
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

        public void SetOwner(Form owner)
        {
            Owner = owner;
        }

        public bool IsEmpty()
        {
            if (Component.Collection.Count == 0)
                return true;

            if (Component.Collection.Count == 1)
                return Component.Collection.First() is CaptionViewModel;

            return false;
        }
        #endregion

        #region Private Metods
        private void OutOfRangeEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ViewWindow_Shown( object sender, EventArgs e )
        {
        }
        #endregion

        #region Override Metdods
        protected override void OnClosing(CancelEventArgs e)
        {
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