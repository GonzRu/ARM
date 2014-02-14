﻿using System;
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
            this.Close();
        }

        public void SetOnEditMode()
        {
        }

        public void SetOffEditMode()
        {
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