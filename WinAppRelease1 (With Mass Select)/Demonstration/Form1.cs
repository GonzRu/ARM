using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demonstration
{
    public partial class Form1 : Form
    {
        private readonly List<PointF> history = new List<PointF>();
        private readonly Random random = new Random();
        private readonly Timer createTimer, shiftTimer;
        private float lastY = 150;

        public Form1()
        {
            InitializeComponent();
            createTimer = new Timer { Interval = 250, Enabled = true };
            shiftTimer = new Timer { Interval = 100, Enabled = true };
            createTimer.Tick += delegate
            {
                lock ( this.history )
                {
                    lastY = random.Next( 110, 190 );
                    this.history.Add( new PointF( 400, lastY ) );

                }
            };
            shiftTimer.Tick += delegate 
            {

                lock ( this.history )
                {
                    this.history.RemoveAll( x => x.X <= 100 );
                    this.history.Add( new PointF( 400, lastY ) );

                    for ( var i = 0; i < this.history.Count; i++ )
                        this.history[i] = new PointF( this.history[i].X - 1, this.history[i].Y );
                }
                Refresh();
            };
        }
        private void Form1FormClosing( object sender, FormClosingEventArgs e )
        {
            createTimer.Dispose();
            shiftTimer.Dispose();
            history.Clear();
        }
        protected override void OnPaint( PaintEventArgs e )
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            lock ( history )
            {
                if (history.Count < 2) return;

                using ( var pen = new Pen( Color.Red, 1 ) )
                {
                    e.Graphics.DrawLines( pen, history.ToArray() );
                }
            }

            using ( var pen = new Pen( Color.Black, 1 ) )
            {
                e.Graphics.DrawLine( pen, 100, 200, 400, 200 );
                e.Graphics.DrawLine( pen, 100, 100, 100, 200 );
            }

            //const int time = 60;
            //const int dist = 6;
            //const int length = time * dist;

            base.OnPaint( e );
        }

        private void Form1Load( object sender, EventArgs e )
        {
            var form = new Form3();
            form.Show( );
        }
    }
}