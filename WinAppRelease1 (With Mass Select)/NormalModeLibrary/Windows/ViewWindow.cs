using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            elementHost1.Child = new TableControl();
        }
        #endregion

        #region Public Metods
        public void ActivatedComponent()
        {
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

            if ( Component != null )
            {
                bool needToShow = true;

                this.Text = Component.Caption;
                this.tableLayoutPanel1.RowStyles[1].Height = 0;

                //if ( IsEditable || Component.IsAutomaticaly )
                //{
                //    this.ControlBox = false;
                //    this.tableLayoutPanel1.RowStyles[1].Height = 30;
                //}
                //else
                //{
                //    this.ControlBox = true;
                //    this.tableLayoutPanel1.RowStyles[1].Height = 0;
                //}
                

                foreach (ViewModel.BaseSignalViewModel vmSignal in Component.Collection)
                {
                    ViewModel.IOutOfRangeHandler ioorh = vmSignal as ViewModel.IOutOfRangeHandler;
                    if (ioorh != null && !ioorh.IsOutOfRangeEvent)
                        ioorh.OutOfRangeEvent += ViewWindow_OutOfRangeEvent;
                }

                this.HideButton.Enabled = true;
                    //this.Show(); //для создания дескриптора окна

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
                        Show();
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
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                    new System.Threading.ThreadStart(delegate
                    {
                        if (!this.Visible) this.ShowDialog();
                        this.tableLayoutPanel1.RowStyles[1].Height = 30;
                        
                    }));

                try
                {
                    if (SoundSystem.System.IsPlaying)
                        return;
                    SoundSystem.System.Play();
                }
                catch (SoundSystemException ex)
                {
                    Console.WriteLine("=================");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(string.Format("Load file: {0}; Sound location: {1}", ex.IsLoadCompleted, ex.SoundLocation));
                    Console.WriteLine(string.Format("Source: {0}", ex.Source));
                    Console.WriteLine("=================");
                }
                catch
                {
                    throw;
                }
            }
            else
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                    new System.Threading.ThreadStart(delegate
                    {
                        if (!Component.IsAutomaticaly)
                            SoundSystem.System.Stop();
                    }));
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
        #endregion

        #region Override Metdods
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            SoundSystem.System.Stop();
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
        #endregion
    }
}