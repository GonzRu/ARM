using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Calculator;
using DebugStatisticLibrary;
using HMI_MT_Settings;

using InterfaceLibrary;

namespace HelperControlsLibrary
{
    /// <summary>
    /// Контрол индикатора
    /// </summary>
    public partial class IndicatorControl : UserControl, IHMITagAccess
    {
        private bool flag;
        private readonly Timer timer = new Timer( );
        private delegate void ActivateTimerDelegate( Timer actTimer, Int32 interval );
        private delegate void DeactivateTimerDelegate( Timer actTimer, Color color );
        private readonly ActivateTimerDelegate activateTimerDelegate;
        private readonly DeactivateTimerDelegate deactivateTimerDelegate;

        public IndicatorControl( )
        {
            InitializeComponent( );

            activateTimerDelegate = ActivateTimer;
            deactivateTimerDelegate = DeactivateTimer;
            timer.Interval = 1000;
            timer.Tick += TimerOnTick;

            SignalOnColor = Color.Red;
            panel1.BackColor = SignalOffColor = Color.LightGray;
        }
        public void BindingSignal( String caption, UInt32 dsGuid, UInt32 devGuid, UInt32 tagGuid )
        {
            var efv = new FormulaEvalNds( HMI_Settings.CONFIGURATION, string.Format( "0({0}.{1}.{2})", dsGuid, devGuid, tagGuid ), string.Empty, string.Empty );

            if ( efv.LinkVariableNewDs == null )
            {
                DebugStatistics.WindowStatistics.AddStatistic( string.Format( "Ошибка в процессе привязывания индикатора к сигналу (TagGuid:{0})", tagGuid ) );
                return;
            }
            LinkedTag = efv.LinkVariableNewDs;
            VisibleText = caption;

            InitSize( );

            efv.OnChangeValFormTI += delegate( string strtagident, object valTag, TypeOfTag type )
                                         {
                                             if ( Disposing ) return false;

                                             try
                                             {
                                                 switch ( UInt16.Parse( valTag.ToString( ) ) )
                                                 {
                                                     case 1:
                                                     case 3:
                                                     case 5:
                                                     case 7: /*Признак неисправности канала. Соответствующий ему СД мигает с частотой порядка 5 Гц (быстрое мигание)*/
                                                         Invoke( activateTimerDelegate, timer, 500 );
                                                         break;

                                                     case 2:
                                                     case 6: /*Признак получения каналом аварийного сигнала. Соот-ветствующий ему СД мигает с частотой порядка 2,5 Гц (медленное мигание) до получения сигнала квитирования*/
                                                         Invoke( activateTimerDelegate, timer, 1000 );
                                                         break;

                                                     case 4: /*Признак наличия сигнала в канале. Соответствующий ему СД светится*/
                                                         Invoke( deactivateTimerDelegate, timer, SignalOnColor );
                                                         break;

                                                     default: /*Признак отсутствия сигнала в канале. Соответствующий ему СД не светится*/
                                                         Invoke( deactivateTimerDelegate, timer, SignalOffColor );
                                                         break;
                                                 }
                                             }
                                             catch
                                             {
                                                 DebugStatistics.WindowStatistics.AddStatistic( string.Format( "Ошибка чтения значения для индикатора (DsGuid:{0} DevGuid:{1} TagGuid:{2})", dsGuid, devGuid, tagGuid ) );
                                                 Invoke( deactivateTimerDelegate, timer, Color.Black );
                                                 return false;
                                             }

                                             return true;
                                         };
        }
        private void InitSize()
        {
            var gr = CreateGraphics( );
            var size = gr.MeasureString( VisibleText, label1.Font );
            Size = new Size( (int)( panel1.Size.Width + size.Width + 10 ), Size.Height );
        }
        private void TimerOnTick( object sender, EventArgs eventArgs )
        {
            panel1.BackColor = flag ? SignalOnColor : SignalOffColor;
            flag = !flag;
            panel1.Refresh( );
        }
        private void DeactivateTimer( Timer actTimer, Color color )
        {
            if ( actTimer.Enabled ) actTimer.Stop( );
            if ( panel1.BackColor.Equals( color ) ) return;
            
            panel1.BackColor = color;
            panel1.Refresh( );
        }
        private static void ActivateTimer( Timer actTimer, Int32 interval )
        {
            if ( actTimer.Enabled ) return;
            actTimer.Interval = interval;
            actTimer.Start( );
        }

        public Color SignalOnColor { get; set; }
        public Color SignalOffColor { get; set; }

        public ITag LinkedTag { get; private set; }
        public List<ITag> LinkedTags { get { return new List<ITag> { LinkedTag }; } }
        public bool IsChange { get; set; }
        public string VisibleText { get { return label1.Text; } private set { label1.Text = value; } }
    }
}