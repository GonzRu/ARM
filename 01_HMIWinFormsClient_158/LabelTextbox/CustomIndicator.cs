using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Calculator;
using InterfaceLibrary;

namespace LabelTextbox
{
    internal enum StatusInd
    {
        Undefined,
        PermLight, // ����� ���������
        Flash, // ������ ������
        Err // ������ ��������
    }

    public partial class CustomIndicator : UserControl, ISubscribe
    {
        private bool V_Ind;
        private bool V_IndFlash;
        private bool V_IndErr;

        // ���� ����������
        public Brush ColorInd;
        private StatusInd status = StatusInd.Undefined;

        // ��� �������
        public int counter = 0; // ������� ������� �����
        private int N_intervalErr = 1; // ������� �������
        private int N_intervalFlash = 2; // ��������� �������
        // ���������� ��������� � �������
        public bool V_IndPrev;
        public bool V_IndFlashPrev;
        public bool V_IndErrPrev;

        public delegate void ChangeIndode();

        public event ChangeIndode OnChangeIndode; //������� ��� ��������� ������ ��������� ��� ������������� �������

        // ������ ����� ��������� � ������ ���������
        private List<ITag> lstLinkTags = new List<ITag>();

        public CustomIndicator()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ColorInd = Brushes.Red;
        }

        //public void LinkSetText_Ind( object Value, string format )
        //{
        //    RezFormulaEval tmpRezFormulaEval;

        //    if( !( Value is RezFormulaEval ) )
        //        return;   // � ������� ������������� ������ ����� ����������

        //    tmpRezFormulaEval = ( RezFormulaEval ) Value;
        //    object val = tmpRezFormulaEval.Value;

        //    if( val is Boolean )
        //    {
        //        V_Ind = Convert.ToBoolean( val );
        //        // �������� ������� ��� ������������� ���������
        //        if (V_Ind)
        //            if (OnChangeIndode != null)
        //                OnChangeIndode();

        //    }
        // Invalidate();
        //}

        #region ���������� ���������� ��������/�������
        public void SetSubscribeLink2Tag( ITag tag ) { lstLinkTags.Add( tag ); }

        /// <summary>
        /// �������� ������� ���� ��
        /// ����������
        /// </summary>
        /// <param name="taglist"></param>
        public void SubscribeTagReNew()
        {
            // ������������� �� ����
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags( lstLinkTags );
        }

        /// <summary>
        /// ������� ������� ���� ��
        /// ���������� 
        /// </summary>
        /// <param name="taglist"></param>
        public void UnSubscribeTagReNew() { HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags( lstLinkTags ); }

        /// <summary>
        /// ������� ���� ��
        /// ���������� c ���������� 
        /// �������� �� ��������� (����������)
        /// </summary>
        /// <param name="taglist"></param>     
        public void UnSubscribeTagReNewAndClear()
        {
            // ������������ �� �����
            foreach ( ITag linkedTag in lstLinkTags )
                linkedTag.SetDefaultValue(); // ������������� � �������� �� ��������� � ... ������������

            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags( lstLinkTags );
        }
        #endregion

        public bool LinkSetText_Ind( string strtagident, object valTag, TypeOfTag type )
        {
            try
            {
                bool v = Convert.ToBoolean( valTag );
                if ( V_Ind != v )
                {
                    V_Ind = v;
                    if ( OnChangeIndode != null )
                        OnChangeIndode();
                    Invalidate();
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
            return true;
        }

        public bool LinkSetText_IndFlash( string strtagident, object valTag, TypeOfTag type )
        {
            try
            {
                bool v = Convert.ToBoolean( valTag );
                if ( V_IndFlash != v )
                {
                    V_IndFlash = v;
                    if ( OnChangeIndode != null )
                        OnChangeIndode();
                    Invalidate();
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
            return true;
        }

        public bool LinkSetText_IndErr( string strtagident, object valTag, TypeOfTag type )
        {
            try
            {
                bool v = Convert.ToBoolean( valTag );
                if ( V_IndErr != v )
                {
                    V_IndErr = v;
                    if ( OnChangeIndode != null )
                        OnChangeIndode();
                    Invalidate();
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
            return true;
        }

        public bool LinkSetText_IndA( string strtagident, object val )
        {
            try
            {
                uint uv = Convert.ToUInt32( val );
                bool v = ( uv == 0 ) ? false : true;
                return LinkSetText_Ind( strtagident, v, TypeOfTag.NaN );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
            return true;
        }

        public bool LinkSetText_IndFlashA( string strtagident, object val )
        {
            try
            {
                uint uv = Convert.ToUInt32( val );
                bool v = ( uv == 0 ) ? false : true;
                return LinkSetText_IndFlash( strtagident, v, TypeOfTag.NaN );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
            return true;
        }

        #region ������ ���
        //public void LinkSetText_IndFlash( object Value, string format )
        //{
        //    RezFormulaEval tmpRezFormulaEval;

        //    if( !( Value is RezFormulaEval ) )
        //        return;   // � ������� ������������� ������ ����� ����������

        //    tmpRezFormulaEval = ( RezFormulaEval ) Value;
        //    object val = tmpRezFormulaEval.Value;

        //    if( val is Boolean )
        //    {
        //        V_IndFlash = Convert.ToBoolean( val );
        //        // �������� ������� ��� ������������� ���������
        //        if (V_IndFlash)
        //            if (OnChangeIndode != null)
        //                OnChangeIndode();
        //    }         
        //    Invalidate();
        //}
        //public void LinkSetText_IndErr( object Value, string format )
        //{
        //    RezFormulaEval tmpRezFormulaEval;

        //    if( !( Value is RezFormulaEval ) )
        //        return;   // � ������� ������������� ������ ����� ����������

        //    tmpRezFormulaEval = ( RezFormulaEval ) Value;
        //    object val = tmpRezFormulaEval.Value;

        //    if( val is Boolean )
        //    {
        //        V_IndErr = Convert.ToBoolean( val );
        //        // �������� ������� ��� ������������� ���������
        //        if (V_IndErr)
        //            if (OnChangeIndode != null)
        //                OnChangeIndode();
        //        Invalidate();
        //    }
        //}
        //public void LinkSetText_IndA( object Value, string format )
        //{
        //   RezFormulaEval tmpRezFormulaEval;

        //   if( !( Value is RezFormulaEval ) )
        //      return;   // � ������� ������������� ������ ����� ����������

        //   tmpRezFormulaEval = ( RezFormulaEval ) Value;
        //   object val = tmpRezFormulaEval.Value;

        //   if( val is UInt32 )
        //   {
        //      UInt32 tva = Convert.ToUInt32( val );

        //      V_Ind = tva == 0 ? true : false;

        //      // �������� ������� ��� ������������� ���������
        //      if( V_Ind )
        //         if( OnChangeIndode != null )
        //            OnChangeIndode( );
        //   }
        //   Invalidate( );
        //}

        //public void LinkSetText_IndFlashA( object Value, string format )
        //{
        //   RezFormulaEval tmpRezFormulaEval;

        //   if( !( Value is RezFormulaEval ) )
        //      return;   // � ������� ������������� ������ ����� ����������

        //   tmpRezFormulaEval = ( RezFormulaEval ) Value;
        //   object val = tmpRezFormulaEval.Value;

        //   if( val is UInt32 )
        //   {
        //      UInt32 tva = Convert.ToUInt32( val );

        //      V_IndFlash = tva != 0 ? true : false;

        //      // �������� ������� ��� ������������� ���������
        //      if( V_IndFlash )
        //         if( OnChangeIndode != null )
        //            OnChangeIndode( );
        //   }
        //   Invalidate( );
        //}
        #endregion

        public void Renew() { panel1.Invalidate(); }

        private void panel1_Paint( object sender, PaintEventArgs e )
        {
            //this.panel1.Invalidate();
            base.OnPaint( e );
            // ��������� � ����������� �� ���������
            if ( V_IndErr )
            {
                this.status = StatusInd.Err;
            }
            else if ( V_IndFlash && !V_IndErr )
            {
                status = StatusInd.Flash;
            }
            else if ( V_Ind && !V_IndErr && !V_IndFlash )
            {
                status = StatusInd.PermLight;
            }
            else if ( !V_Ind && !V_IndErr && !V_IndFlash )
                status = StatusInd.Undefined;

            using ( Pen pw1 = new Pen( Color.Black, 2 ) )
            {
                //��������/������� � ����������� �� ���������
                switch ( status )
                {
                    case StatusInd.Err:
                        if ( counter < N_intervalErr )
                        {
                            counter++;
                            if ( V_IndErrPrev )
                                break;
                            e.Graphics.DrawEllipse( pw1,
                                                    new Rectangle( new Point( 0, 0 ),
                                                                   new Size(
                                                                       panel1.Width - Convert.ToInt32( pw1.Width ),
                                                                       panel1.Height - Convert.ToInt32( pw1.Width ) ) ) );
                            e.Graphics.FillEllipse( ColorInd,
                                                    new Rectangle( new Point( 0, 0 ),
                                                                   new Size(
                                                                       panel1.Width - Convert.ToInt32( pw1.Width ),
                                                                       panel1.Height - Convert.ToInt32( pw1.Width ) ) ) );
                            return; //Brushes.Red
                        }
                        else if ( counter == N_intervalErr )
                        {
                            counter = 0;
                            V_IndErrPrev = !V_IndErrPrev;
                        }
                        break;
                    case StatusInd.Flash:
                        if ( counter < N_intervalFlash )
                        {
                            counter++;
                            if ( V_IndFlashPrev )
                                break;
                            e.Graphics.DrawEllipse( pw1,
                                                    new Rectangle( new Point( 0, 0 ),
                                                                   new Size(
                                                                       panel1.Width - Convert.ToInt32( pw1.Width ),
                                                                       panel1.Height - Convert.ToInt32( pw1.Width ) ) ) );
                            e.Graphics.FillEllipse( ColorInd,
                                                    new Rectangle( new Point( 0, 0 ),
                                                                   new Size(
                                                                       panel1.Width - Convert.ToInt32( pw1.Width ),
                                                                       panel1.Height - Convert.ToInt32( pw1.Width ) ) ) );
                            return; //Brushes.Red
                        }
                        else if ( counter == N_intervalFlash )
                        {
                            counter = 0;
                            V_IndFlashPrev = !V_IndFlashPrev;
                        }
                        break;
                    case StatusInd.PermLight:
                        e.Graphics.DrawEllipse( pw1,
                                                new Rectangle( new Point( 0, 0 ),
                                                               new Size( panel1.Width - Convert.ToInt32( pw1.Width ),
                                                                         panel1.Height - Convert.ToInt32( pw1.Width ) ) ) );
                        e.Graphics.FillEllipse( ColorInd,
                                                new Rectangle( new Point( 0, 0 ),
                                                               new Size( panel1.Width - Convert.ToInt32( pw1.Width ),
                                                                         panel1.Height - Convert.ToInt32( pw1.Width ) ) ) );
                        return; //Brushes.Red
                    default:
                        // �� ���������
                        e.Graphics.DrawEllipse( pw1,
                                                new Rectangle( new Point( 0, 0 ),
                                                               new Size( panel1.Width - Convert.ToInt32( pw1.Width ),
                                                                         panel1.Height - Convert.ToInt32( pw1.Width ) ) ) );
                        e.Graphics.FillEllipse( Brushes.Gray,
                                                new Rectangle( new Point( 0, 0 ),
                                                               new Size( panel1.Width - Convert.ToInt32( pw1.Width ),
                                                                         panel1.Height - Convert.ToInt32( pw1.Width ) ) ) );
                        return;
                        //break;
                }
                e.Graphics.DrawEllipse( pw1,
                                        new Rectangle( new Point( 0, 0 ),
                                                       new Size( panel1.Width - Convert.ToInt32( pw1.Width ),
                                                                 panel1.Height - Convert.ToInt32( pw1.Width ) ) ) );
                e.Graphics.FillEllipse( Brushes.Gray,
                                        new Rectangle( new Point( 0, 0 ),
                                                       new Size( panel1.Width - Convert.ToInt32( pw1.Width ),
                                                                 panel1.Height - Convert.ToInt32( pw1.Width ) ) ) );
            }
        }
    }
}
