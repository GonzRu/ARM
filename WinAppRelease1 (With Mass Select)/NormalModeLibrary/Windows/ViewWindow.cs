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

        public ViewWindow()
        {
            InitializeComponent();
            elementHost1.Child = new TableControl();
        }
        //public Form CreateNewHandle()
        //{
            
        //}
        public void ActivatedComponent()
        {
            if ( Component != null )
            {
                this.Text = Component.Caption;

                if ( IsEditable || Component.IsAutomaticaly )
                {
                    this.ControlBox = false;
                    this.tableLayoutPanel1.RowStyles[1].Height = 30;
                }
                else
                {
                    this.ControlBox = true;
                    this.tableLayoutPanel1.RowStyles[1].Height = 0;
                }

                if ( IsEditable )
                {
                    FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                    this.button1.Enabled = false;
                }
                else
                {
                    foreach ( ViewModel.BaseSignalViewModel vmSignal in Component.Collection )
                    {
                        ViewModel.IOutOfRangeHandler ioorh = vmSignal as ViewModel.IOutOfRangeHandler;
                        if ( ioorh != null && !ioorh.IsOutOfRangeEvent )
                            ioorh.OutOfRangeEvent += new EventHandler( ViewWindow_OutOfRangeEvent );
                    }

                    FormBorderStyle = ( Component.IsCaptionVisible ) ? System.Windows.Forms.FormBorderStyle.FixedSingle : System.Windows.Forms.FormBorderStyle.None;

                    this.button1.Enabled = true;
                    //this.Show(); //для создания дескриптора окна

                    if ( !Component.IsVisible || Component.IsAutomaticaly )
                        this.Hide(); //скрываем, после создания дескриптора
                }

                this.Top = Component.Top;
                this.Left = Component.Left;
                this.Width = Component.Width;
                this.Height = Component.Height;
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
        }
        private void ViewWindow_OutOfRangeEvent( object sender, EventArgs e )
        {
            NormalModeLibrary.Sources.OutOfRangeEventArgs orea = (NormalModeLibrary.Sources.OutOfRangeEventArgs)e;

            if ( orea.OutOfRange )
            {
                this.Invoke( (CheckRangeDelegate)delegate { if ( !this.Visible ) this.Show(); } );

                try
                {
                    if ( SoundSystem.System.IsPlaying )
                        return;
                    SoundSystem.System.Play();
                }
                catch ( SoundSystemException ex )
                {
                    Console.WriteLine( "=================" );
                    Console.WriteLine( ex.Message );
                    Console.WriteLine( string.Format( "Load file: {0}; Sound location: {1}", ex.IsLoadCompleted, ex.SoundLocation ) );
                    Console.WriteLine( string.Format( "Source: {0}", ex.Source ) );
                    Console.WriteLine( "=================" );
                }
                catch
                {
                    throw;
                }
            }
            else if ( !Component.IsAutomaticaly )
                SoundSystem.System.Stop();
        }
        protected override void OnClosing( CancelEventArgs e )
        {
            base.OnClosing( e );

            SoundSystem.System.Stop();
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
        internal Boolean IsEditable { get; set; }
        internal Places Place { get; set; }

        private void ViewWindow_Shown( object sender, EventArgs e )
        {
            ActivatedComponent();
        }
        private void button1_Click( object sender, EventArgs e )
        {
            SoundSystem.System.Stop();
            this.Hide();
        }
    }
}