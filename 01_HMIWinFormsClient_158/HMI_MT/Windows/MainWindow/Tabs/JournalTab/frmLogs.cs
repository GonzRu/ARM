using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using CommonUtils;
using HMI_MT_Settings;
using InterfaceLibrary;
using MessagePanel.MessagePanelService;
using OscillogramsLib;

namespace HMI_MT
{
    public partial class frmLogs : Form
    {

       #region private-����� ������
         private MainForm parent;
         private int sortColumn = -1;	// ��� ���������� � ListView
         private ListView lstvCurrent;
         OscDiagViewer oscdg;
         DataTable dtO;  // ������� � ���������������
         DataTable dtO_100;  // ������� � ��������������� ����-100
         DataTable dtOE;  // ������� � ��������������� ���
         DataTable dtG;  // ������� � �����������
         DataTable dtA;   // ������� � ��������
         DataTable dt;   
         DataTable dtLSF;   // ������� �� �������� ���������
         XDocument xdoc;
         XDocument xdoc_Dev;
       #endregion

       #region ������������
        /// <summary>
        /// �����������
        /// </summary>
        public frmLogs( )
        {
            InitializeComponent();
        }
        public frmLogs( MainForm linkMainForm )
        {
            InitializeComponent();
            parent = linkMainForm;
        }
       #endregion

        /// <summary>
        /// �������� �����
        /// </summary>
        private void frmLogs_Load( object sender, EventArgs e )
        {
            // �������� ������� � ����������� �������� ����������
            this.Width = parent.Width;
            tabControl1.Height = ClientSize.Height -  pnlSelect.Height;
            chEventDescript.Width = parent.Width - columnHeader1.Width - chLocalTimeFix.Width - chIdBlock.Width - chBlockName.Width - chBlockComment.Width - chAckStatus.Width - parent.Width / 60;//20lstvEvent.Width
            chActionName.Width = ( parent.Width - columnHeader2.Width - chUserName.Width - chArmName.Width - chBlockNameUser.Width - chComment.Width - chServerTime.Width - chLocalTime.Width ) * 2 / 3 - parent.Width / 70;
            chComment.Width = parent.Width - columnHeader2.Width - chUserName.Width - chArmName.Width - chBlockNameUser.Width - chActionName.Width - chServerTime.Width - chLocalTime.Width - parent.Width / 70;

            lbl_ch1.Enabled = false;
            lbl_ch2.Enabled = false;
            lbl_ch3.Enabled = false;
            nudMin.Enabled = false;
            nudSec.Enabled = false;
            // ������������� ������ ��� ������ ���������� �� ��������� �����
            // ������� �������� ������� ��������� ����� � ��
            if (!parent.isBDConnection)
            {
                MessageBox.Show("��� ����� � ����� ������");
                return;
            }
            dtpEndData.Value = DateTime.Now;
            dtpEndTime.Value = DateTime.Now;
            dtpStartData.Value = DateTime.Now;

            TimeSpan ts = new TimeSpan( 1, 0, 0, 0 );
            dtpStartData.Value = dtpStartData.Value - ts;
            dtpStartTime.Value = DateTime.Now;

            timer1.Stop();

            #region messagesTab
            tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;
            tableLayoutPanel1.RowStyles[1].Height = 0;

            tableLayoutPanel1.RowStyles[2].SizeType = SizeType.AutoSize;
            #endregion
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Application.DoEvents();

            ShowMessagesCountNumericUpDown.Value = HMI_Settings.MessageProvider.MessageCount; // ������� DisplayMessages();
        }

        #region MessagesTab metods
        private void MessagesUpdatedhandler()
        {
            DisplayMessages();
        }

        private void DisplayMessages()
        {
            messagesListView.Items.Clear();

            var messages = HMI_Settings.MessageProvider.GetMessages();
            if (messages == null)
            {
                MessagesCountLabel.Text = "�� ��������";
                MessageBox.Show("�� ������� �������� ������...", "������", MessageBoxButtons.OK);
                return;
            }            

            lta = new LongTimeAction();
            lta.Start(this);

            int i = 1;
            List<ListViewItem> listViewItems = new List<ListViewItem>();
            List<int> devices = new List<int>();
            foreach (var message in messages)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.SubItems.Add(i.ToString());
                listViewItem.SubItems.Add(message.LocalTime.ToString());
                listViewItem.SubItems.Add(message.BlockName);
                listViewItem.SubItems.Add(message.Text);
                listViewItem.SubItems.Add(message.Comment);
                listViewItem.Tag = message;

                // ���� ������� ���������, �� ������� ���� ���������
                if (!devices.Contains(message.AdditionalID))
                    devices.Add(message.AdditionalID);

                i++;
                listViewItems.Add(listViewItem);                
            }

            // ���������� ������������ ����
            messagesListView.Items.AddRange(listViewItems.ToArray());
            messagesListView.ContextMenu = new ContextMenu();
            messagesListView.ContextMenu.MenuItems.Add("�����������",
                                                       (sender, args) =>
                                                           {
                                                               KvitSelectedMessages();
                                                               DisplayMessages();
                                                           });

            // ���������� ����������� ComboBox �� ������ ���������, �� ������� ���� ���������
            foreach (var device in devices)
            {
                var dev = HMI_Settings.CONFIGURATION.GetLink2Device(0, (uint) device);

                if (dev != null)
                    DeviceTypesComboBox.Items.Add(new Tuple<string, uint>(String.Format("{0} {1}@{2}", dev.Description, dev.UniObjectGUID, dev.TypeName), (uint)device));
            }

            DeviceTypesComboBox.DisplayMember = "Item1";
            DeviceTypesComboBox.ValueMember = "Item2";

            MessagesCountLabel.Text = HMI_Settings.MessageProvider.TotalMessagesCount.ToString();

            lta.Stop();
        }

        private void KvitSelectedMessages()
        {
            string comment = null;
            List<TableEventLogAlarm> messages = new List<TableEventLogAlarm>();

            foreach (var item in messagesListView.SelectedItems)
            {
                TableEventLogAlarm message = (item as ListViewItem).Tag as TableEventLogAlarm;

                if (message.ReceiptComment == true && comment == null)
                {
                    var commentWindow = new GetCommentWindow();
                    var result = commentWindow.ShowDialog();

                    if (result == DialogResult.Cancel)
                        return;

                    if (result == DialogResult.OK)
                        comment = commentWindow.Comment;
                }

                messages.Add(message);
            }

            new KvitWindow(messages, comment).ShowDialog();
        }

        private void KvitAllMessages()
        {
            string comment = null;
            List<TableEventLogAlarm> messages = new List<TableEventLogAlarm>();

            foreach (var item in messagesListView.Items)
            {
                TableEventLogAlarm message = (item as ListViewItem).Tag as TableEventLogAlarm;

                if (message.ReceiptComment == true && comment == null)
                {
                    var commentWindow = new GetCommentWindow();
                    var result = commentWindow.ShowDialog();

                    if (result == DialogResult.Cancel)
                        return;

                    if (result == DialogResult.OK)
                        comment = commentWindow.Comment;
                }

                messages.Add(message);
            }

            new KvitWindow(messages, comment).ShowDialog();
        }

        private void KvitMessagesByDeviceGuid()
        {
            if (DeviceTypesComboBox.SelectedItem == null)
                return;

            uint deviceGuid = ((Tuple<string, uint>)DeviceTypesComboBox.SelectedItem).Item2;


            string comment = null;
            List<TableEventLogAlarm> messages = new List<TableEventLogAlarm>();

            foreach (var item in messagesListView.Items)
            {
                TableEventLogAlarm message = (item as ListViewItem).Tag as TableEventLogAlarm;

                if (message.ReceiptComment == true && comment == null)
                {
                    var commentWindow = new GetCommentWindow();
                    var result = commentWindow.ShowDialog();

                    if (result == DialogResult.Cancel)
                        return;

                    if (result == DialogResult.OK)
                        comment = commentWindow.Comment;
                }

                if (message.AdditionalID == deviceGuid)
                    messages.Add(message);
            }

            new KvitWindow(messages, comment).ShowDialog();
        }

        #region Handlers
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;

            if (tabControl.SelectedTab != null)
                if (tabControl.SelectedTab.Text == "���������")
                {
                    tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Absolute;
                    tableLayoutPanel1.RowStyles[1].Height = 0;

                    tableLayoutPanel1.RowStyles[2].SizeType = SizeType.AutoSize;
                }
                else
                {
                    tableLayoutPanel1.RowStyles[1].SizeType = SizeType.AutoSize;

                    tableLayoutPanel1.RowStyles[2].SizeType = SizeType.Absolute;
                    tableLayoutPanel1.RowStyles[2].Height = 0;
                }
        }

        private void kvitSelectMsgButton_Click(object sender, EventArgs e)
        {
            KvitSelectedMessages();

            DisplayMessages();            
        }

        private void kvitAllButton_Click(object sender, EventArgs e)
        {
            KvitAllMessages();

            DisplayMessages();
        }

        private void kvitByDeviceTypeButton_Click(object sender, EventArgs e)
        {
            KvitMessagesByDeviceGuid();

            DisplayMessages();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            DisplayMessages();
        }

        private void autoUpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoUpdateCheckBox.Checked)
            {
                HMI_Settings.MessageProvider.MessagesUpdated += MessagesUpdatedhandler;
                updateButton.Enabled = false;
            }
            else
            {
                HMI_Settings.MessageProvider.MessagesUpdated -= MessagesUpdatedhandler;
                updateButton.Enabled = true;
            }
        }


        private void ShowMessagesCountNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            HMI_Settings.MessageProvider.MessageCount = (int)ShowMessagesCountNumericUpDown.Value;

            DisplayMessages();
        }
        #endregion

        #endregion MessagesTab

        /// <summary>
        /// private void EventBD( )
        ///     ������ ������� �� ���� ����������� �������� ���������    
        /// </summary>
        private void EventBD( )
        {
            // ��������� ����� ���������� � ���������� ������ �� ����� *.config
           SqlConnection asqlconnect = new SqlConnection(  HMI_Settings.ProviderPtkSql );
            try
            {
                asqlconnect.Open();
            } catch
            {
                MessageBox.Show("��� ����� � ��");
                return;
            }

            // ������������ ������ ��� ������ �������� ���������
            SqlCommand cmd = new SqlCommand( "ShowEventLog", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // ������� ���������
            // 1. ��������� �����
            SqlParameter dtMim = new SqlParameter();
            dtMim.ParameterName = "@Dt_start";
            dtMim.SqlDbType = SqlDbType.DateTime;
            TimeSpan tss = new TimeSpan( 0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second );
            DateTime tim = dtpStartData.Value - tss;
            dtMim.Value = tim;
            dtMim.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( dtMim );

            // 2. �������� �����
            SqlParameter dtMax = new SqlParameter();
            dtMax.ParameterName = "@Dt_end";
            dtMax.SqlDbType = SqlDbType.DateTime;
            tss = new TimeSpan( 0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second );
            tim = dtpEndData.Value - tss;
            dtMax.Value = tim;
            dtMax.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( dtMax );

            // 3. ������������� ����� (��� �������������)
            SqlParameter idBlock = new SqlParameter();
            idBlock.ParameterName = "@Id";
            idBlock.SqlDbType = SqlDbType.Int;
            idBlock.Value = 0;
            idBlock.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( idBlock );
            
            // 4. �������� �������
            SqlParameter strName = new SqlParameter();
            strName.ParameterName = "@Name";
            strName.SqlDbType = SqlDbType.Text;
            strName.Value = "";//SqlString.Null;
            strName.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( strName );

            // ���������� DataSet
            DataSet aDS = new DataSet( "ptk" );
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            //aSDA.sq
            try
            {
                aSDA.Fill(aDS);
            }
            catch (SqlException sex)
            {
                MessageBox.Show("�������� ������ ����������.\n������:" + sex.Message + "\n��������� ������.", "������ ������� � ���� ������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                asqlconnect.Close();
                aSDA.Dispose();
                aDS.Dispose();
                return;
            }


            asqlconnect.Close();

            //PrintDataSet( aDS );

            // ��������� ������ �� �������
            dt = aDS.Tables[0];

            LinkSetLV( null, true );    // ������� ListView ��� ����������  

            StringBuilder ts = new StringBuilder();
            for( int curRow = 0; curRow < dt.Rows.Count; curRow++ )
            {
                ListViewItem li = new ListViewItem();
                li.SubItems.Clear();
                DateTime t = ( DateTime ) dt.Rows[curRow]["LocalTime"];
					 li.Tag = t;	// ��� ����������
                //li.SubItems.Add( t.ToShortDateString() + " : " + t.ToLongTimeString() + "." + t.Millisecond );
                string tmpTs = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t);
                //li.SubItems.Add(t.ToShortDateString() + " : " + t.ToLongTimeString() + "." + t.Millisecond);
                li.SubItems.Add(tmpTs);
                
                ts.Length = 0; 
                ts.Append( dt.Rows[curRow]["BlockId"] );
                li.SubItems.Add( ts.ToString() );
                
                ts.Length = 0;
                ts.Append( dt.Rows[curRow]["BlockName"] );
                li.SubItems.Add( ts.ToString() );

                ts.Length = 0;
                ts.Append( dt.Rows[curRow]["Comment"] );
                li.SubItems.Add( ts.ToString() );
                
                ts.Length = 0;
                ts.Append( dt.Rows[curRow]["EventText"] );
                li.SubItems.Add( ts.ToString() );
                
                ts.Length = 0;
                ts.Append( dt.Rows[curRow]["Status"] );
                li.SubItems.Add( ts.ToString() );

                LinkSetLV( li, false );
            }
			  // ������������ ListView � �����
				CommonUtils.CommonUtils.DrawAsZebra( lstvEvent );
				aSDA.Dispose();
				aDS.Dispose();
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

            if( lstvEvent.InvokeRequired )
            {
                if( !actDellstV )
                    SetLV( li, actDellstV );
                else
                    SetLV( null, actDellstV );
            }
            else
            {
                if( !actDellstV )
                    lstvEvent.Items.Add( li );
                else
                    lstvEvent.Items.Clear();
            }
        }

        /*==========================================================================*
        * private void SetText(ListViewItem li)
        * //��� ����������������� ������ ���������
        *==========================================================================*/
        private void SetLV( ListViewItem li, bool actDellstV )
        {
            if( lstvEvent.InvokeRequired )
            {
                SetLVCallback d = new SetLVCallback( SetLV );
                this.Invoke( d, new object[] { li, actDellstV } );
            }
            else
            {
                if( !actDellstV )
                    lstvEvent.Items.Add( li );
                else
                    lstvEvent.Items.Clear();
            }
        }


        /// <summary>
        /// private void checkBox1_CheckedChanged( object sender, EventArgs e )
        /// </summary>
        private void checkBox1_CheckedChanged( object sender, EventArgs e )
        {
            if( cbPeriodRenew.Checked )
            {
                lbl_ch1.Enabled = true;
                lbl_ch2.Enabled = true;
                lbl_ch3.Enabled = true;
                nudMin.Enabled = true;
                nudSec.Enabled = true;
                // �������� ������ �������������� ������
                //MessageBox.Show("�� �����������");
                if( nudMin.Value == 0 && nudSec.Value == 0 )
                    timer1.Interval = 5000;
                else
                    timer1.Interval = (int)( nudMin.Value * 60000 + nudSec.Value * 1000 );

                timer1.Start();

            }
            else
            {
                lbl_ch1.Enabled = false;
                lbl_ch2.Enabled = false;
                lbl_ch3.Enabled = false;
                nudMin.Enabled = false;
                nudSec.Enabled = false;
                // ��������� ������ �������������� ������
                //MessageBox.Show( "�� �����������" );
                timer1.Stop();
            }
        }
        /// <summary>
        /// private void tpLogActionUsers_Enter( object sender, EventArgs e )
        /// </summary>
        private void tpLogActionUsers_Enter( object sender, EventArgs e )
        {
            sortColumn = -1;
            //cbEvent.Enabled = false;
            //lblEvent.Enabled = false;
            // ����������� ������ �� ��������� ������������
            //UserBD();
            lstvCurrent = lstvUserAction;
        }

        #region ���� �� ������� � ���������� �������� � ���������������
        /// <summary>
        /// private void tpLogSummaryAvarOsc_Enter( object sender, EventArgs e )
        /// </summary>
        private void tpLogSummaryAvarOsc_Enter( object sender, EventArgs e )
        {
            tpLogSummaryAvarOsc.Width = tabControl1.Width;
            gbAvar.Width = tpLogSummaryAvarOsc.Width / 2;
        }
        #endregion 

        private void timer1_Tick( object sender, EventArgs e )
        {
            // ���������
            EventBD();
        }

        CommonUtils.LongTimeAction lta;

        private void button1_Click( object sender, EventArgs e )
        {
           lta = new LongTimeAction();
           lta.Start(this);
           //BackgroundWorker bgwBackGround = new BackgroundWorker();
           //bgwBackGround.DoWork += new DoWorkEventHandler(bgwBackGround_DoWork);
           //bgwBackGround.RunWorkerAsync();
           // ������� ���������� �������
           try
           {
              switch (tabControl1.SelectedTab.Text)
              {
                 case "������� ��� � ���":
                    lstvEvent.Items.Clear();
                    EventBD();
                    break;
                 case "�������� �������������":
                    lstvUserAction.Items.Clear();
                    UserBD();
                    break;
                 case "������� ������ ������ � ������������":
                    dgvOscill.Rows.Clear();
                    dgvAvar.Rows.Clear();
                    AvarBD();
                    OscBD();
                    //OscBD10();
                    //DiagBD();
                    break;
                 case "������� ��������� ������":
                    lstvLogSystemFull.Items.Clear();
                    LogSystemFullBD();
                    break;
              }
           }
           catch (Exception ex)
           {
              TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 443, DateTime.Now.ToString() + " (443) : frmLogs.cs : bgwBackGround_DoWork() :  ������ ��� ������������ ������: " + ex.Message);
              //this.Close();
           }
           finally 
           {
              lta.Stop();
           }
        }

        private void ������ToolStripMenuItem1_Click( object sender, EventArgs e )
        {
            StringBuilder sb = new StringBuilder();
            ListViewItem li = new ListViewItem();

            sb.Length = 0;
            switch( lstvCurrent.Name )
            {
                case "lstvEvent":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( " (������� ��� � ���)" );
                    sb.Append( "\n========================================================================\n" );
                    sb.Append( " \n \n " );

                    for( int i = 0; i < lstvEvent.Items.Count; i++ )
                    {
                        li = lstvEvent.Items[i];
                        for( int j = 0; j < li.SubItems.Count; j++ )
                            sb.Append( li.SubItems[j].Text + " \t " );
                        sb.Append( " \n " );
                        parent.prt.rtbText.AppendText( sb.ToString() );
                        sb.Length = 0;
                    }
                    break;
                case "lstvUserAction":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( " (�������� �������������)" );
                    sb.Append( "\n========================================================================\n" );
                    sb.Append( " \n \n " );

                    for( int i = 0; i < lstvUserAction.Items.Count; i++ )
                    {
                        li = lstvUserAction.Items[i];
                        for( int j = 0; j < li.SubItems.Count; j++ )
                            sb.Append( li.SubItems[j].Text + " \t " );
                        sb.Append( " \n " );
                        parent.prt.rtbText.AppendText( sb.ToString() );
                        sb.Length = 0;
                    }
                    break;
                default:
                    return;
            }
            parent.RibbonMenuButtonPrintClick( sender, e );
        }

        private void tpLogEventOKU_RZA_Enter( object sender, EventArgs e )
        {
			  sortColumn = -1;
           lstvCurrent = lstvEvent;
        }

        private void mnuPageSetup_Click( object sender, EventArgs e )
        {
        }

        private void mnuPrintPreview_Click( object sender, EventArgs e )
        {
            StringBuilder sb = new StringBuilder();
            ListViewItem li = new ListViewItem();

            sb.Length = 0;
            switch( lstvCurrent.Name )
            {
                case "lstvEvent":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( " (������� ��� � ���)" );
                    sb.Append( "\n========================================================================\n" );
                    sb.Append( " \n \n " );

                    for( int i = 0; i < lstvEvent.Items.Count; i++ )
                    {
                        li = lstvEvent.Items[i];
                        for( int j = 0; j < li.SubItems.Count; j++ )
                            sb.Append( li.SubItems[j].Text + " \t " );
                        sb.Append( " \n " );
                        parent.prt.rtbText.AppendText( sb.ToString() );
                        sb.Length = 0;
                    }
                    break;
                case "lstvUserAction":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( " (�������� �������������)" );
                    sb.Append( "\n========================================================================\n" );
                    sb.Append( " \n \n " );

                    for( int i = 0; i < lstvUserAction.Items.Count; i++ )
                    {
                        li = lstvUserAction.Items[i];
                        for( int j = 0; j < li.SubItems.Count; j++ )
                            sb.Append( li.SubItems[j].Text + " \t " );
                        sb.Append( " \n " );
                        parent.prt.rtbText.AppendText( sb.ToString() );
                        sb.Length = 0;
                    }
                    break;
                case "lstvLogSystemFull":
                    // ��������� ��������� ��������
                    sb.Append( "========================================================================\n" );
                    sb.Append( " (������� ��������� ������)" );
                    sb.Append( "\n========================================================================\n" );
                    sb.Append( " \n \n " );

                    for ( int i = 0 ; i < lstvLogSystemFull.Items.Count ; i++ )
                    {
                       li = lstvLogSystemFull.Items[ i ];
                       for ( int j = 0 ; j < li.SubItems.Count ; j++ )
                          sb.Append( li.SubItems[ j ].Text + " \t " );
                       sb.Append( " \n " );
                       parent.prt.rtbText.AppendText( sb.ToString( ) );
                       sb.Length = 0;
                    }
                    break;
                default:
                    return;
            }
        }
        /// <summary>
        /// private void EventBD( )
        ///     ������ ������� �� ���� ����������� �������� ���������    
        /// </summary>
        private void AvarBD( )
        {
            dgvAvar.Rows.Clear();
            // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            try
            {
                asqlconnect.Open();
            }
            catch 
            {
               System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + "���������� � : frmLogs : AvarBD() " );
                return;
            }

            // ������������ ������ ��� ������ �������� ���������
            SqlCommand cmd = new SqlCommand( "ShowDataLog2", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // ������� ���������
            // 1. ip FC
            SqlParameter pipFC = new SqlParameter();
            pipFC.ParameterName = "@IP";
            pipFC.SqlDbType = SqlDbType.BigInt;
            pipFC.Value = 0;
            pipFC.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pipFC );
            // 2. id ����������
            SqlParameter pidBlock = new SqlParameter();
            pidBlock.ParameterName = "@id";
            pidBlock.SqlDbType = SqlDbType.Int;
            pidBlock.Value = 0;
            pidBlock.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pidBlock );

            // 3. ��������� �����
            SqlParameter dtMim = new SqlParameter();
            dtMim.ParameterName = "@dt_start";
            dtMim.SqlDbType = SqlDbType.DateTime;
            TimeSpan tss = new TimeSpan( 0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second );
            DateTime tim = dtpStartData.Value - tss;
            dtMim.Value = tim;
            dtMim.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( dtMim );

            // 2. �������� �����
            SqlParameter dtMax = new SqlParameter();
            dtMax.ParameterName = "@dt_end";
            dtMax.SqlDbType = SqlDbType.DateTime;
            tss = new TimeSpan( 0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second );
            tim = dtpEndData.Value - tss;
            dtMax.Value = tim;
            dtMax.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( dtMax );

            // 5. ��� ������
            SqlParameter ptypeRec = new SqlParameter();
            ptypeRec.ParameterName = "@type";
            ptypeRec.SqlDbType = SqlDbType.Int;

            ptypeRec.Value = 2; // ���������� �� �������
            ptypeRec.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( ptypeRec );
            // 6. �� ������ �������
            SqlParameter pid = new SqlParameter();
            pid.ParameterName = "@id_record";
            pid.SqlDbType = SqlDbType.Int;
            pid.Value = 0;
            pid.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pid );

            // ���������� DataSet
            DataSet aDS = new DataSet( "ptk" );
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            //aSDA.sq            
            try
            {
                aSDA.Fill(aDS, "TbAlarm");
            }
            catch (SqlException sex)
            {
                MessageBox.Show("�������� ������ ����������.\n������:" + sex.Message + "\n��������� ������.", "������ ������� � ���� ������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                asqlconnect.Close();
                aSDA.Dispose();
                aDS.Dispose();
                return;
            }
            asqlconnect.Close();

            //PrintDataSet( aDS );
            // ��������� ������ �� ������
            dtA = aDS.Tables["TbAlarm"];
            for( int curRow = 0; curRow < dtA.Rows.Count; curRow++ )
            {
                int i = dgvAvar.Rows.Add();   // ����� ������

                var devGuid = uint.Parse(dtA.Rows[curRow]["BlockID"].ToString());
                var device = HMI_Settings.CONFIGURATION.GetLink2Device(0, devGuid);

                // ��� �����
                dgvAvar["clmBlockName", i].Value = dtA.Rows[curRow]["BlockName"];

                // �������������
                if (HMI_Settings.IsDebugMode)
                    dgvAvar["clmComment", i].Value = String.Format("({0}) {1} ({2})", devGuid, device.Description, device.TypeName);
                else
                    dgvAvar["clmComment", i].Value = device.Description;

                // ����� �����
                DateTime tmpT = ( DateTime ) dtA.Rows[curRow]["TimeBlock"];
                string tmpTs = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(tmpT);
                dgvAvar["clmBlockTime", i].Value = tmpTs;
                
                // ID ������
                dgvAvar["clmIDAvar", i].Value = dtA.Rows[curRow]["ID"];

                // Guid �����
                dgvAvar["clmBlockId", i].Value = dtA.Rows[curRow]["BlockID"];
            }
				aSDA.Dispose();
				aDS.Dispose();
        }

        #region ������������ � �������� ������������ � ��������
        ArrayList asb = new ArrayList();    // ��� �������� ���� ������ � ������ ����������� ������������
        short cntSelectOSC = 0;             // ����� ���������� ������������/��������

		/// <summary>
		/// ������ ��������� ������������ ������ ������������
		/// � �������� ��� ��������� ����� ������ - 
		/// ����� ������ - ���������� � ������ ����� Project.cfg
		/// </summary>
        private void OscBD()
        {
			XDocument xdoc_Project_cfg = XDocument.Load(HMI_Settings.PathToPrjFile);
			IEnumerable<XElement> xes =  xdoc_Project_cfg.Element("Project").Element("OscDiagInSummaryLog").Elements();

			foreach (XElement xe in xes)
			{
				if (xe.Attribute("isenable").Value.ToLower() == "false")
					continue;

                //TypeBlockData tbd;
                //int tbd;
                switch (xe.Attribute("type").Value)
                {
                    case "OscBMRZ":
                        //tbd = TypeBlockData.TypeBlockData_OscBMRZ;
                        GetOscDiagList(4, out dtO);
                        break;
                    case "OscBMRZ_100":
                        //tbd = TypeBlockData.TypeBlockData_OscBMRZ;
                        GetOscDiagList(8, out dtO_100);
                        break;
                    case "OscSirius":
                        //tbd = TypeBlockData.TypeBlockData_OscSirius;
                        //GetOscDiagList(8, out dtO);
                        break;
                    case "OscEkra":
                        //tbd = TypeBlockData.TypeBlockData_OscEkra;
                        GetOscDiagList(10, out dtOE);
                        break;
                    case "OscBresler":
                        //tbd = TypeBlockData.TypeBlockData_OscBresler;
                        GetOscDiagList(11, out dtO);
                        break;
                    case "Diagramm":
                        //tbd = TypeBlockData.TypeBlockData_Diagramm;
                        GetOscDiagList(5, out dtG);
                        break;
                    default:
                        throw new Exception("������������� ������� ���� ���������� �� ��������������");
                }
			}
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param Name="tbd"></param>
        void GetOscDiagList(int tbd, out DataTable dtog)
        {
            if (oscdg == null)
                oscdg = new OscDiagViewer();//parent

            oscdg.DTStartData = dtpStartData.Value;
            oscdg.DTStartTime = dtpStartTime.Value;
            oscdg.DTEndData = dtpEndData.Value;
            oscdg.DTEndTime = dtpEndTime.Value;
            oscdg.TypeRec = tbd;

            // ��������� ������ �� �������������� ��� ����������
            dtog = oscdg.Do_SQLProc();

            for (int curRow = 0; curRow < dtog.Rows.Count; curRow++)
            {
                int i = dgvOscill.Rows.Add();   // ����� ������

                var devGuid = uint.Parse(dtog.Rows[curRow]["BlockID"].ToString());
                var device = HMI_Settings.CONFIGURATION.GetLink2Device(0, devGuid);

                // ��� �����
                dgvOscill["clmBlockNameOsc", i].Value = dtog.Rows[curRow]["BlockName"];

                // �������������
                if (HMI_Settings.IsDebugMode)
                    dgvOscill["clmCommentOsc", i].Value = String.Format("({0}) {1} ({2})", devGuid, device.Description, device.TypeName);
                else
                    dgvOscill["clmCommentOsc", i].Value = device.Description;

                // ����� �����
                DateTime tmpT = (DateTime)dtog.Rows[curRow]["TimeBlock"];
                string tmpTs = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(tmpT);
                dgvOscill["clmBlockTimeOsc", i].Value = tmpTs;

                // ������������� �������������
                dgvOscill["clmID", i].Value = dtog.Rows[curRow]["ID"];

                //dgvOscill["clmViewOsc", i].Value = ; //"�������������";

                if ((int)dtog.Rows[curRow]["TypeID"] == 5)
                    dgvOscill["clmViewOsc", i].Value = "���������";
                else
                    dgvOscill["clmViewOsc", i].Value = "�������������";
            }
        }

        private void UserBD()
        {
           #region ��� ��� OleDB
           //xdoc = XDocument.Load(HMI_Settings.PathToPrjFile);

           //string Provider = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Provider").Attribute("value").Value;
           //string Data_Source = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Data_Source").Attribute("value").Value;
           //string Initial_Catalog = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Initial_Catalog").Attribute("value").Value;
           //string Persist_Security_Info = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Persist_Security_Info").Attribute("value").Value;
           //string User_ID = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("User_ID").Attribute("value").Value;
           //string Password = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Password").Attribute("value").Value;
           //string Anything_else = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Anything_else").Attribute("value").Value;

           //// ��� ������ �� udl-����� - ��� �������, ���������� ��� ���������
           ////cstr = "Provider=SQLOLEDB.1;Password=12345;Persist Security Info=True;User ID=asu;Initial Catalog=ptk304;Data Source=192.168.240.1";

           //string cstr = "Provider=" + Provider
           //                + ";Data Source=" + Data_Source
           //                + ";Initial Catalog=" + Initial_Catalog
           //                + ";Persist Security Info=" + Persist_Security_Info
           //                + ";User ID=" + User_ID
           //                + ";Password=" + Password
           //               + "; " + Anything_else;

           //// ��� �������� �� ������� ������ ����������
           //Console.WriteLine("OleDbConnection: " + cstr);

           //OleDbConnection oledbconnect = new OleDbConnection(cstr);

           //try
           //{
           //   oledbconnect.Open();
           //}
           //catch
           //{
           //   MessageBox.Show("��� ����� � �� (oledbconnect).\n��������� ������ ����������� (���� .udl)");
           //   return;
           //} 

           // ������������ ������ ��� ������ �������� ���������
           //OleDbCommand cmd = new OleDbCommand("ShowUserLog", oledbconnect);
           //cmd.CommandType = CommandType.StoredProcedure;
           #endregion

           #region ��� ��� SqlDBConnection
           //// ��������� ����� ���������� � ���������� ������ �� ����� *.config
           //SqlConnection asqlconnect = new SqlConnection(HMI_Settings.cstr);
           //try
           //{
           //   asqlconnect.Open();
           //}
           //catch
           //{
           //   MessageBox.Show("��� ����� � ��");
           //   return;
           //}

           //// ������������ ������ ��� ������ �������� ���������
           //SqlCommand cmd = new SqlCommand("ShowUserLog", asqlconnect);
           //cmd.CommandType = CommandType.StoredProcedure;

           //// ������� ���������
           //// 1. ��������� �����
           ////OleDbParameter dtMim = new OleDbParameter();
           //SqlParameter dtMim = new SqlParameter();
           //dtMim.ParameterName = "@MinDate";
           //dtMim.SqlDbType = SqlDbType.DateTime;//.DBTimeStamp;   //OleDbType
           //TimeSpan tss = new TimeSpan(0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second);
           //DateTime tim = dtpStartData.Value - tss;
           //dtMim.Value = tim;
           //dtMim.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(dtMim);

           //// 2. �������� �����
           ////OleDbParameter dtMax = new OleDbParameter();
           //SqlParameter dtMax = new SqlParameter();
           //dtMax.ParameterName = "@MaxDate";
           //dtMax.SqlDbType = SqlDbType.DateTime;//.DBTimeStamp;//OleDbType
           //tss = new TimeSpan(0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second);
           //tim = dtpEndData.Value - tss;
           //dtMax.Value = tim;
           //dtMax.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(dtMax);

           //// 3. ������������� ������������
           ////OleDbParameter idUser = new OleDbParameter();
           //SqlParameter idUser = new SqlParameter();
           //idUser.ParameterName = "@UserId";
           //idUser.SqlDbType = SqlDbType.Int;//OleDbType
           //idUser.Value = 0;
           //idUser.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(idUser);

           //// 4. ������������� �����
           ////OleDbParameter idBlock = new OleDbParameter();
           //SqlParameter idBlock = new SqlParameter();
           //idBlock.ParameterName = "@BlockId";
           //idBlock.SqlDbType = SqlDbType.Int;//OleDbType
           //idBlock.Value = 0;
           //idBlock.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(idBlock);

           //// 5. ������������� ���
           ////OleDbParameter idArm = new OleDbParameter();
           //SqlParameter idArm = new SqlParameter();
           //idArm.ParameterName = "@ArmId";
           //idArm.SqlDbType = SqlDbType.Int;
           //idArm.Value = 0;
           //idArm.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(idArm);

           //// 6. ������������� ��������
           ////OleDbParameter idAction = new OleDbParameter();
           //SqlParameter idAction = new SqlParameter();
           //idAction.ParameterName = "@Action";
           //idAction.SqlDbType = SqlDbType.Int;
           //idAction.Value = 0;
           //idAction.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(idAction);

           //// ���������� DataSet
           //DataSet aDS = new DataSet("ptk");
           //SqlDataAdapter aSDA = new SqlDataAdapter();
           //aSDA.SelectCommand = cmd;

           //try
           //{
           //   aSDA.Fill(aDS);
           //}
           //catch (SqlException sex)
           //{
           //   MessageBox.Show("�������� ������ ����������.\n������:" + sex.Message + "\n��������� ������.", "������ ������� � ���� ������", MessageBoxButtons.OK, MessageBoxIcon.Error);
           //   asqlconnect.Close();
           //   aSDA.Dispose();
           //   aDS.Dispose();
           //   return;
           //}

           //asqlconnect.Close();

           ////PrintDataSet( aDS );

           //// ��������� ������ �� �������������
           //DataTable dt = aDS.Tables[0];

           //LinkSetLVU(null, true);    // ������� ListView ��� ����������   
           #endregion

           dt = new DataTable();
           // 1. ��������� �����
           TimeSpan tss = new TimeSpan(0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second);
           DateTime tim_start = dtpStartData.Value - tss;
           // 2. �������� �����           
           tss = new TimeSpan(0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second);
           DateTime tim_end = dtpEndData.Value - tss;

           using (SqlConnection UserBDSqlConnection = new SqlConnection(HMI_Settings.ProviderPtkSql))
           {
              SqlCommand sc = UserBDSqlConnection.CreateCommand();
              sc.CommandText = "SELECT * FROM vUserLog WHERE LocalTime BETWEEN '" + tim_start.ToString() + "' AND '" + tim_end.ToString() + "' ORDER BY LocalTime DESC";
              SqlDataAdapter sda = new SqlDataAdapter(sc);
              sda.Fill(dt);
           }

           StringBuilder ts = new StringBuilder();
           for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
           {
              ListViewItem li = new ListViewItem();
              li.SubItems.Clear();

              // ����� �������� ������� ��
              ts.Length = 0;
              DateTime t = (DateTime)dt.Rows[curRow]["LocalTime"];
              li.Tag = t;	// ��� ����������
              string tmpTs = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t);
              //li.SubItems.Add(t.ToShortDateString() + " : " + t.ToLongTimeString() + "." + t.Millisecond);
              li.SubItems.Add(tmpTs);

              // ����� �������� ������� Logger'��					
              ts.Length = 0;
              t = (DateTime)dt.Rows[curRow]["ServerTime"];
              tmpTs = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t);
              //li.SubItems.Add(t.ToShortDateString() + " : " + t.ToLongTimeString() + "." + t.Millisecond);
              li.SubItems.Add(tmpTs);

              ts.Length = 0;
              ts.Append(dt.Rows[curRow]["ActionName"]);
              li.SubItems.Add(ts.ToString());

              ts.Length = 0;
              ts.Append(dt.Rows[curRow]["UserName"]);
              li.SubItems.Add(ts.ToString());

              ts.Length = 0;
              ts.Append(dt.Rows[curRow]["ArmName"]);
              li.SubItems.Add(ts.ToString());

              ts.Length = 0;
              ts.Append(dt.Rows[curRow]["BlockName"]);
              li.SubItems.Add(ts.ToString());

              ts.Length = 0;
              ts.Append(dt.Rows[curRow]["Comment"]);
              li.SubItems.Add(ts.ToString());

              LinkSetLVU(li, false);
           }

           // ������������ ListView � �����
           CommonUtils.CommonUtils.DrawAsZebra(lstvUserAction);
           
           #region ��� ��� SqlDBConnection
           //aSDA.Dispose();
           //aDS.Dispose();
           #endregion
        }

        /// <summary>
        /// ����� ������ ������������� ��� ��������� �� �������� ������ ������������/�������� � ������
        /// </summary>
        /// 
        private void dgvOscill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3)
                return;

           string ifa = String.Empty;         // ��� �����
           DataGridViewCell de;
           char[] sep = { ' ' };
           int ide = 0; // ����� �����
           string strType = String.Empty; // ��� ����� - ���. ��� ����.

           try
           {
              de = dgvOscill["clmID", e.RowIndex];
              ide = (int)de.Value;
              de = dgvOscill["clmViewOsc", e.RowIndex];    // ��� - ������������� ��� ���������
              strType = (string)de.Value;
           }
           catch
           {
              System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + "���������� � : frmLogs : dgvOscill_CellContentClick() ");
              return;
           }

           /*
            * ������ �������� ����� DS,
            * ������� ��� ��������� ��������� ������ ��������� (0)
            * � ���������� ����� ��������� �������� ����� �� ������ �����
            * ����� �������� �������� ����� DS
            */
           // ���� ����� ������ ������������� ������ ���� (dtO)
           oscdg.ShowOSCDg(0, dtO, ide);
           oscdg.ShowOSCDg(0, dtO_100, ide);
           oscdg.ShowOSCDg(0, dtOE, ide);
           oscdg.ShowOSCDg(0, dtG, ide); 
           #region MyRegion
		   // �� ide ����� ������ � dto, ������� ���� � �������������� (����������), �������� � ����, ��������� fastview

           //oscdg.CntSelectOSC = 1;   // ���� � ���� ������� ����� �������� ������ � ����� �������������� ��� ����������

           //oscdg.ShowOSCDg(dtO, ide);
           //oscdg.ShowOSCDg(dtOE, ide);

	       #endregion
       }
        #endregion

       /*==========================================================================*
			*   private void void LinkSetText(object Value)
			*      ��� ����������������� ������ ���������
			*==========================================================================*/
        delegate void SetLVUCallback( ListViewItem li, bool actDellstV );

       // actDellstV - �������� � ListView : false - �� �������; true - ��������;
        public void LinkSetLVU( object Value, bool actDellstV )
        {
            if( !( Value is ListViewItem ) && !actDellstV )
                return;   // ������������� ������ ����� ����������

            ListViewItem li = null;
            if( !actDellstV )
                li = ( ListViewItem ) Value;

            if(lstvUserAction.InvokeRequired )
            {
                if( !actDellstV )
                    SetLVU( li, actDellstV );
                else
                    SetLVU( null, actDellstV );
            }
            else
            {
                if( !actDellstV )
                    lstvUserAction.Items.Add( li );
                else
                    lstvUserAction.Items.Clear();
            }
        }

        /*==========================================================================*
        * private void SetText(ListViewItem li)
        * //��� ����������������� ������ ���������
        *==========================================================================*/
        private void SetLVU( ListViewItem li, bool actDellstV )
        {
            if( lstvUserAction.InvokeRequired )
            {
                SetLVUCallback d = new SetLVUCallback( SetLVU );
                this.Invoke( d, new object[] { li, actDellstV } );
            }
            else
            {
                if( !actDellstV )
                    lstvUserAction.Items.Add( li );
                else
                    lstvUserAction.Items.Clear();
            }
        }

        private DataGridViewCell GetDataGridViewCell(string nameColumn, int rowindex)
        {
            DataGridViewCell de = null;
            try
            {
                de = dgvAvar[nameColumn, rowindex];
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                        @"(1103) : {0} : X:\Projects\40_Tumen_GPP09\Client\HMI_MT\frmLogs.cs : GetDataGridViewCell() : ������ : {1}",
                        DateTime.Now.ToString(), ex.Message));
                //System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + "���������� � : frmLogs : dgvAvar_CellContentClick() : " + ex.Message);
            }
            return de;
        }

        /// <summary>
       /// ����� ���������� ������ ���������� ����� ��� ���������
       /// </summary>
       /// <param Name="sender"></param>
       /// <param Name="e"></param> 
       private void dgvAvar_CellContentClick( object sender, DataGridViewCellEventArgs e )
        {
            if (e.ColumnIndex != 3) return;
            
            // ����� ���������� � ������ ��

            int numdevfc = Convert.ToInt32(GetDataGridViewCell("clmBlockId", e.RowIndex).Value.ToString());
            
            numdevfc = Convert.ToInt32(numdevfc);
            #warning DsGuid = 0
            IDeviceForm frm = DevicesLibrary.DeviceFormFactory.CreateForm( this, 0, (uint)numdevfc, parent.arrFrm );
            frm.ActivateAndShowTreeGroupWithCategory(HelperControlsLibrary.Category.Crush);

            #region ��������� ������ �� ������ � ������������
            // ����� ����� iFC * 256 + iIDDev
            int ide = (int)GetDataGridViewCell("clmIDAvar", e.RowIndex).Value;

            ArrayList arparam = new ArrayList();
            // ����� ��� ������ � ��
            arparam.Add(ide);
            // ������ �����������
            byte[] str_cnt_in_bytes = Encoding.UTF8.GetBytes(HMI_Settings.ProviderPtkSql);
            arparam.Add(str_cnt_in_bytes);

            IRequestData req = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetData(0, (uint)numdevfc, "ArhivAvariBlockData", arparam, ide);
            req.OnReqExecuted += new ReqExecuted(frm.reqAvar_OnReqExecuted);       
            #endregion
        }

        #region ���� �� ������� �������� �������
			/// <summary>
		  /// private void tpLogSystemFull_Enter( object sender, EventArgs e )
		  /// ���� �� ������� �������� �������
		 /// </summary>
		 /// <param Name="sender"></param>
		 /// <param Name="e"></param>
		 private void tpLogSystemFull_Enter( object sender, EventArgs e )
		 {
			 sortColumn = -1;
			 //LogSystemFullBD();
          lstvCurrent = lstvLogSystemFull;
		 }
		 private void LogSystemFullBD( )
		 {
			 // ��������� ����� ���������� � ���������� ������ �� ����� *.config
			 //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
          SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
			 try
			 {
				 asqlconnect.Open();
			 } catch
			 {
				 MessageBox.Show( "��� ����� � ��" );
				 return;
			 }

			 // ������������ ������ ��� ������ �������� ���������
			 SqlCommand cmd = new SqlCommand( "ShowUnionLog", asqlconnect );
			 cmd.CommandType = CommandType.StoredProcedure;

			 // ������� ���������
			 // 1. ��������� �����
			 SqlParameter dtMim = new SqlParameter();
			 dtMim.ParameterName = "@StartData";
			 dtMim.SqlDbType = SqlDbType.DateTime;
			 TimeSpan tss = new TimeSpan( 0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second );
			 DateTime tim = dtpStartData.Value - tss;
			 dtMim.Value = tim;
			 dtMim.Direction = ParameterDirection.Input;
			 cmd.Parameters.Add( dtMim );

			 // 2. �������� �����
			 SqlParameter dtMax = new SqlParameter();
			 dtMax.ParameterName = "@EndData";
			 dtMax.SqlDbType = SqlDbType.DateTime;
			 tss = new TimeSpan( 0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second );
			 tim = dtpEndData.Value - tss;
			 dtMax.Value = tim;
			 dtMax.Direction = ParameterDirection.Input;
			 cmd.Parameters.Add( dtMax );

			 // ���������� DataSet
			 DataSet aDS = new DataSet( "ptk" );
			 SqlDataAdapter aSDA = new SqlDataAdapter();
			 aSDA.SelectCommand = cmd;

			 //aSDA.sq
             try
             {
                 aSDA.Fill(aDS);
             }
             catch (SqlException sex)
             {
                 MessageBox.Show("�������� ������ ����������.\n������:" + sex.Message + "\n��������� ������.", "������ ������� � ���� ������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 asqlconnect.Close();
                 aSDA.Dispose();
                 aDS.Dispose();
                 return;
             }
			 asqlconnect.Close();

			 //PrintDataSet( aDS );

			 // ��������� ������ �� ��������
			 dtLSF = aDS.Tables[0];

			 LinkSetLVLSF( null, true );    // ������� ListView ��� ����������  

			 StringBuilder ts = new StringBuilder();
			 for( int curRow = 0 ; curRow < dtLSF.Rows.Count ; curRow++ )
			 {
				 ListViewItem li = new ListViewItem();
				 li.SubItems.Clear();

				 //ts.Length = 0;
				 //ts.Append( dtLSF.Rows[curRow]["ID"] );
				 //li.SubItems.Add( ts.ToString() );

				 ts.Length = 0;
				 DateTime t = ( DateTime ) dtLSF.Rows[curRow]["TIME"];
				 li.Tag = t;
                 string tmpTs = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t);
				 //li.SubItems.Add( t.ToShortDateString() + " : " + t.ToLongTimeString() + "." + t.Millisecond );
                 li.SubItems.Add(tmpTs);

				 ts.Length = 0;
				 ts.Append( dtLSF.Rows[curRow]["ObjName"] );
				 li.SubItems.Add( ts.ToString() );

				 ts.Length = 0;
				 ts.Append( dtLSF.Rows[curRow]["Comment"] );
				 li.SubItems.Add( ts.ToString() );

				 ts.Length = 0;
				 ts.Append( dtLSF.Rows[curRow]["Text"] );
				 li.SubItems.Add( ts.ToString() );

				 ts.Length = 0;
				 ts.Append( dtLSF.Rows[curRow]["Source"] );
				 li.SubItems.Add( ts.ToString() );

				 //ts.Length = 0;
				 //ts.Append( dtLSF.Rows[curRow]["TypeLog"] );
				 //li.SubItems.Add( ts.ToString() );

				 LinkSetLVLSF( li, false );
			 }
			 // ������������ ListView � �����
			 CommonUtils.CommonUtils.DrawAsZebra( lstvEvent );
			 aSDA.Dispose();
			 aDS.Dispose();
		 }
		 /*==========================================================================*
  *   private void void LinkSetText(object Value)
  *      ��� ����������������� ������ ���������
  *==========================================================================*/
		 delegate void SetLVLSFCallback( ListViewItem li, bool actDellstV );

		 // actDellstV - �������� � ListView : false - �� �������; true - ��������;
		 public void LinkSetLVLSF( object Value, bool actDellstV )
		 {
			 if( !( Value is ListViewItem ) && !actDellstV )
				 return;   // ������������� ������ ����� ����������

			 ListViewItem li = null;
			 if( !actDellstV )
				 li = ( ListViewItem ) Value;

			 if( lstvLogSystemFull.InvokeRequired )
			 {
				 if( !actDellstV )
					 SetLVLSF( li, actDellstV );
				 else
					 SetLVLSF( null, actDellstV );
			 }
			 else
			 {
				 if( !actDellstV )
					 lstvLogSystemFull.Items.Add( li );
				 else
					 lstvLogSystemFull.Items.Clear();
			 }
		 }

		 /*==========================================================================*
		 * private void SetText(ListViewItem li)
		 * //��� ����������������� ������ ���������
		 *==========================================================================*/
		 private void SetLVLSF( ListViewItem li, bool actDellstV )
		 {
			 if( lstvLogSystemFull.InvokeRequired )
			 {
				 SetLVLSFCallback d = new SetLVLSFCallback( SetLVLSF );
				 this.Invoke( d, new object[] { li, actDellstV } );
			 }
			 else
			 {
				 if( !actDellstV )
					 lstvLogSystemFull.Items.Add( li );
				 else
					 lstvLogSystemFull.Items.Clear();
			 }
		 }
        #endregion

		 /// <summary>
		 /// private void lstvEvent_ColumnClick( object sender, ColumnClickEventArgs e )
		 /// ��������� click'a �� ��������� listview
		 /// </summary>
		 /// <param Name="sender"></param>
		 /// <param Name="e"></param>
		 private void lstvEvent_ColumnClick( object sender, ColumnClickEventArgs e )
		 {
			 // ���������� ��������� �� ������� � ��������� ��������� ��������
			 if( e.Column != sortColumn )
			 {
				 sortColumn = e.Column;	// ��������� ���������� ������ �������
                 lstvEvent.Sorting = System.Windows.Forms.SortOrder.Ascending;	//������� ���������� �� ���������
			 }
			 else
			 {
				 // ������ ������� ����������
                 if (lstvEvent.Sorting == System.Windows.Forms.SortOrder.Ascending)
                     lstvEvent.Sorting = System.Windows.Forms.SortOrder.Descending;
				 else
                     lstvEvent.Sorting = System.Windows.Forms.SortOrder.Ascending;
			 }
			 // ����� ������ ������ ����������
			 lstvEvent.Sort();

			 lstvEvent.ListViewItemSorter = new ListViewItemComparer(e.Column, lstvEvent.Sorting);
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param Name="sender"></param>
		 /// <param Name="e"></param>
		 private void lstvUserAction_ColumnClick( object sender, ColumnClickEventArgs e )
		 {
			 // ���������� ��������� �� ������� � ��������� ��������� ��������
			 if( e.Column != sortColumn )
			 {
				 sortColumn = e.Column;	// ��������� ���������� ������ �������
                 lstvUserAction.Sorting = System.Windows.Forms.SortOrder.Ascending;	//������� ���������� �� ���������
			 }
			 else
			 {
				 // ������ ������� ����������
                 if (lstvUserAction.Sorting == System.Windows.Forms.SortOrder.Ascending)
                     lstvUserAction.Sorting = System.Windows.Forms.SortOrder.Descending;
				 else
                     lstvUserAction.Sorting = System.Windows.Forms.SortOrder.Ascending;
			 }
			 // ����� ������ ������ ����������
			 lstvUserAction.Sort();

			 lstvUserAction.ListViewItemSorter = new ListViewItemComparer( e.Column, lstvUserAction.Sorting );
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param Name="sender"></param>
		 /// <param Name="e"></param>
		 private void lstvLogSystemFull_ColumnClick( object sender, ColumnClickEventArgs e )
		 {
			 // ���������� ��������� �� ������� � ��������� ��������� ��������
			 if( e.Column != sortColumn )
			 {
				 sortColumn = e.Column;	// ��������� ���������� ������ �������
                 lstvLogSystemFull.Sorting = System.Windows.Forms.SortOrder.Ascending;	//������� ���������� �� ���������
			 }
			 else
			 {
				 // ������ ������� ����������
                 if (lstvLogSystemFull.Sorting == System.Windows.Forms.SortOrder.Ascending)
                     lstvLogSystemFull.Sorting = System.Windows.Forms.SortOrder.Descending;
				 else
                     lstvLogSystemFull.Sorting = System.Windows.Forms.SortOrder.Ascending;
			 }
			 // ����� ������ ������ ����������
			 lstvLogSystemFull.Sort();

			 lstvLogSystemFull.ListViewItemSorter = new ListViewItemComparer( e.Column, lstvLogSystemFull.Sorting );
		 }

		 private void frmLogs_FormClosing( object sender, FormClosingEventArgs e )
		 {
		 }

       private void btnListView2File_Click( object sender, EventArgs e )
       {
          StringBuilder sb = new StringBuilder( );
          ListViewItem li = new ListViewItem( );
          parent.prt.rtbText.Clear();
          sb.Length = 0;

          switch ( lstvCurrent.Name )
          {
             case "lstvEvent":
                // ��������� ��������� ��������
                sb.Append( "========================================================================\n" );
                sb.Append( " (������� ��� � ���)" );
                sb.Append( "\n========================================================================\n" );
                sb.Append( " \n \n " );

                for ( int i = 0 ; i < lstvEvent.Items.Count ; i++ )
                {
                   li = lstvEvent.Items[ i ];
                   for ( int j = 0 ; j < li.SubItems.Count ; j++ )
                      sb.Append( li.SubItems[ j ].Text + " \t " );
                   sb.Append( " \n " );
                   parent.prt.rtbText.AppendText( sb.ToString( ) );
                   sb.Length = 0;
                }                
                break;
             case "lstvUserAction":
                // ��������� ��������� ��������
                sb.Append( "========================================================================\n" );
                sb.Append( " (�������� �������������)" );
                sb.Append( "\n========================================================================\n" );
                sb.Append( " \n \n " );

                for ( int i = 0 ; i < lstvUserAction.Items.Count ; i++ )
                {
                   li = lstvUserAction.Items[ i ];
                   for ( int j = 0 ; j < li.SubItems.Count ; j++ )
                      sb.Append( li.SubItems[ j ].Text + " \t " );
                   sb.Append( " \n " );
                   parent.prt.rtbText.AppendText( sb.ToString( ) );
                   sb.Length = 0;
                }
                break;
             case "lstvLogSystemFull":
                // ��������� ��������� ��������
                sb.Append( "========================================================================\n" );
                sb.Append( " (������� ��������� ������)" );
                sb.Append( "\n========================================================================\n" );
                sb.Append( " \n \n " );

                for ( int i = 0 ; i < lstvLogSystemFull.Items.Count ; i++ )
                {
                   li = lstvLogSystemFull.Items[ i ];
                   for ( int j = 0 ; j < li.SubItems.Count ; j++ )
                      sb.Append( li.SubItems[ j ].Text + " \t " );
                   sb.Append( " \n " );
                   parent.prt.rtbText.AppendText( sb.ToString( ) );
                   sb.Length = 0;
                }
                break;
             default:
                return;
          }
          SaveFileDialog saveFileDialog1 = new SaveFileDialog( );
          saveFileDialog1.AddExtension = true;
          saveFileDialog1.CheckFileExists = false;
          saveFileDialog1.DefaultExt = "log";
          saveFileDialog1.FileName = ((TabPage)lstvCurrent.Parent).Text;
          saveFileDialog1.Filter = "��������� � �������(*.log)|*.log|��� �����(*.*)|*.*";
          saveFileDialog1.InitialDirectory = Application.StartupPath;

          if ( DialogResult.OK != saveFileDialog1.ShowDialog( ) )
             return;

          parent.prt.rtbText.SaveFile( saveFileDialog1.FileName, RichTextBoxStreamType.PlainText );

          // ����� �������� ������� � xml ����
          DataSet data = new DataSet( );
          DataTable nt = new DataTable( ( ( TabPage ) lstvCurrent.Parent ).Text );
          // ���������� ������� �������
          for ( int i = 0 ; i < lstvCurrent.Columns.Count ; i++ )
          {
             DataColumn dtc = new DataColumn( lstvCurrent.Columns[i].Text);
             nt.Columns.Add( dtc );
          }

          for ( int i = 0 ; i < lstvCurrent.Items.Count ; i++ )
          {
             li = lstvCurrent.Items[ i ];
             DataRow dr = nt.NewRow();
             for ( int j = 0 ; j < li.SubItems.Count ; j++ )
                dr[nt.Columns[j]] = li.SubItems[j].Text;
             nt.Rows.Add( dr );
          }

          data.Tables.Add( nt );

          data.WriteXml( Path.GetFileNameWithoutExtension(saveFileDialog1.FileName)  + ".xml" );
       }
	 }

	class ListViewItemComparer : IComparer
	{
		private int col;
        private System.Windows.Forms.SortOrder order;

		public ListViewItemComparer( )
		{
			col = 0;
            order = System.Windows.Forms.SortOrder.Ascending;
		}
        public ListViewItemComparer(int column, System.Windows.Forms.SortOrder order)
		{
			col = column;
			this.order = order;
		}
		public int Compare( object x, object y)
		{
			int returnVal;// = -1
			string[] strDT;
			char[] arrch = { '.', ':' };

			// ���������� �������� �� ������������ �������� ������
			try
			{
				strDT = ( ( ( ListViewItem ) x ).SubItems[col].Text ).Split( arrch );
				DateTime fData = new DateTime( Convert.ToInt32( strDT[2] ), Convert.ToInt32( strDT[1] ), Convert.ToInt32( strDT[0] ), Convert.ToInt32( strDT[3] ), Convert.ToInt32( strDT[4] ), Convert.ToInt32( strDT[5] ) );
				//DateTime secData = new DateTime();
				//DateTime firstData = DateTime.Parse( ( ( ListViewItem ) x ).SubItems[col].Text );
				//DateTime secData = DateTime.Parse( ( ( ListViewItem ) y ).SubItems[col].Text );
				DateTime firstData = ( DateTime )( ( ( ListViewItem ) x ).Tag);
				DateTime secData = ( DateTime ) ( ( ( ListViewItem ) y ).Tag );

				returnVal = DateTime.Compare( firstData , secData);
			} catch
			{
				returnVal = string.Compare( ( ( ListViewItem ) x ).SubItems[col].Text, ( ( ListViewItem ) y ).SubItems[col].Text );
			}
			if( order == System.Windows.Forms.SortOrder.Descending )
				returnVal *= -1;
			return returnVal;
		}
	}
}