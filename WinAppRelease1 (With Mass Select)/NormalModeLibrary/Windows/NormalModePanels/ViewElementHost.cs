using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using NormalModeLibrary.Sources;
using NormalModeLibrary.ViewModel;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace NormalModeLibrary.Windows
{
    public class ViewElementHost : ElementHost, INormalModePanel
    {
        #region private
        private bool _canMove;
        private Point _mouseDownLocation;
        #endregion

        #region Properties
        public Places Place { get; set; }

        public PanelViewModel Component
        {
            get
            {
                TableControl tc = (TableControl)Child;
                return (PanelViewModel)tc.DataContext;
            }
            set
            {
                TableControl tc = (TableControl)Child;
                tc.DataContext = value;
            }
        }
        #endregion

        #region Constructors
        public ViewElementHost()
        {
            TableControl tc = new TableControl(this);
            Child = tc;

            ControlMoverOrResizer.Init(this);
        }

        #endregion

        #region Public metods
        public void ActivatedComponent()
        {
            if (Component != null)
            {
                this.Text = Component.Caption;

                this.Top = Component.Top;
                this.Left = Component.Left;
                this.Width = Component.Width;
                this.Height = Component.Height;

                if (Component.IsAutomaticaly)
                    Visible = false;
            }
        }

        public void DeactivatedComponent()
        {
            
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
                if (!Component.IsAutomaticaly)
                {
                    Visible = true;
                }
        }

        public void SetOwner(Form owner)
        {
            owner.Controls[0].Controls.Add(this);
        }
        #endregion
    }
}
