using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HMI_MT
{
    public partial class ActUserCurrentMess : Form
    {
        private MainForm parent;
        public ActUserCurrentMess( )
        {
            InitializeComponent();
        }
        public ActUserCurrentMess( MainForm f)
        {
            InitializeComponent();
            parent = f;
        }

        /// <summary>
        ///     ViewEventToLog - ������������ ������ � ListView �����
        /// </summary>
        /// <param Name="timeEvent"> ����� ������� (��� �� ������� � ��)</param>
        /// <param Name="codEvent"> ��� ������� ��� ������������� (��� �� ������� � ��)</param>
        /// <param Name="TextMesFromBD"> ����� � �������� ������������ �� ��</param>
        /// <param Name="sComment"> ���. ����������� � �������</param>
        /// <param Name="oF"> ������ �� �������� ������� - �����</param>        
        /// <param Name="wrToBDLog"> ������� ������������� ������������� � ��</param>
        /// <param Name="showInArmLog"> ������� ������������� ������ � ������� ���'�</param>
        public void ViewEventToLog( string timeEvent, int codEvent, string TextMesFromBD, string sComment, string oF, bool wrToBDLog, bool showInArmLog ) 
        {
            ListViewItem li = new ListViewItem();
            li.SubItems.Add( timeEvent );
            li.SubItems.Add( codEvent.ToString() );
            if( TextMesFromBD == "" )
                TextMesFromBD = " " ;
            li.SubItems.Add( TextMesFromBD );
            if( sComment == "" )
                sComment = " ";
            li.SubItems.Add( sComment );
            li.SubItems.Add( oF );
            li.SubItems.Add( wrToBDLog.ToString() );
            li.SubItems.Add( showInArmLog.ToString() );
            
            //lstvOutMes.Items.Add( li );

            LinkSetLV( li, false );
        }
        /*==========================================================================*
          *   private void void LinkSetText(object Value)
          *      ��� ����������������� ������ ���������
          *==========================================================================*/
        delegate void SetLVCallback( ListViewItem li, bool actDellstV );

        // actDellstV - �������� � ListView : false - �� �������; true - ��������;
        public void LinkSetLV( object Value, bool actDellstV )
        {
            if( !( Value is ListViewItem ) && !actDellstV )
                return;   // ������������� ������ ����� ����������

            ListViewItem li = null;
            if( !actDellstV )
                li = ( ListViewItem ) Value;

            if( lstvOutMes.InvokeRequired )
            {
                if( !actDellstV )
                    SetLV( li, actDellstV );
                else
                    SetLV( null, actDellstV );
            }
            else
            {
                if( !actDellstV )
                    lstvOutMes.Items.Add( li );
                else
                    lstvOutMes.Items.Clear();
            }
        }

        /*==========================================================================*
        * private void SetText(ListViewItem li)
        * //��� ����������������� ������ ���������
        *==========================================================================*/
        private void SetLV( ListViewItem li, bool actDellstV )
        {
            if( lstvOutMes.InvokeRequired )
            {
                SetLVCallback d = new SetLVCallback( SetLV );
                this.Invoke( d, new object[] { li, actDellstV } );
            }
            else
            {
                if( !actDellstV )
                    lstvOutMes.Items.Add( li );
                else
                    lstvOutMes.Items.Clear();
            }
        }

        private void menuStrip1_ItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {

        }

        private void ������ToolStripMenuItem_Click( object sender, EventArgs e )
        {
            StringBuilder sb = new StringBuilder();
            ListViewItem li = new ListViewItem();

            // ��������� ��������� ��������
            sb.Append( "========================================================================\n" );
            sb.Append( " (C�������� �������� ������ ������)" );
            sb.Append( "\n========================================================================\n" );
            sb.Append( " \n \n " );

            for( int i = 0; i < lstvOutMes.Items.Count; i++ )
            {
                li = lstvOutMes.Items[i];
                
                for( int j = 0; j < li.SubItems.Count; j++ )
                    sb.Append( li.SubItems[j].Text + " \t " );
                sb.Append( " \n " );
                parent.prt.rtbText.AppendText( sb.ToString() );
                sb.Length = 0;
            }
            parent.RibbonMenuButtonPrintClick( sender, e );
        }
    }
}