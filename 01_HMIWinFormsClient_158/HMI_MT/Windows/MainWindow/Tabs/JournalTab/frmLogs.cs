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

       #region private-члены класса
         private MainForm parent;
         private int sortColumn = -1;	// для сортировки в ListView
         private ListView lstvCurrent;
         OscDiagViewer oscdg;
         DataTable dtO;  // таблица с осциллограммами
         DataTable dtO_100;  // таблица с осциллограммами бмрз-100
         DataTable dtOE;  // таблица с осциллограммами экр
         DataTable dtG;  // таблица с диаграммами
         DataTable dtA;   // таблица с авариями
         DataTable dt;   
         DataTable dtLSF;   // таблица со сводными событиями
         XDocument xdoc;
         XDocument xdoc_Dev;
       #endregion

       #region Конструкторы
        /// <summary>
        /// конструктор
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
        /// загрузка формы
        /// </summary>
        private void frmLogs_Load( object sender, EventArgs e )
        {
            // приводим размеры в соответсвие текущему разрешению
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
            // устанавливаем пикеры для вывода информации за последние сутки
            // сначала проверим текущее состояние связи с БД
            if (!parent.isBDConnection)
            {
                MessageBox.Show("Нет связи с базой данных");
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

            ShowMessagesCountNumericUpDown.Value = HMI_Settings.MessageProvider.MessageCount; // Вызовет DisplayMessages();
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
                MessagesCountLabel.Text = "не доступно";
                MessageBox.Show("Не удалось получить данные...", "Ошибка", MessageBoxButtons.OK);
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

                // Сбор номеров устройств, от которых есть сообщения
                if (!devices.Contains(message.AdditionalID))
                    devices.Add(message.AdditionalID);

                i++;
                listViewItems.Add(listViewItem);                
            }

            // Построение контекстного меню
            messagesListView.Items.AddRange(listViewItems.ToArray());
            messagesListView.ContextMenu = new ContextMenu();
            messagesListView.ContextMenu.MenuItems.Add("Квитировать",
                                                       (sender, args) =>
                                                           {
                                                               KvitSelectedMessages();
                                                               DisplayMessages();
                                                           });

            // Построение содержимого ComboBox на основе устройств, от которых есть сообщения
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
                if (tabControl.SelectedTab.Text == "Сообщения")
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
        ///     запрос событий из базы посредством хранимой процедуры    
        /// </summary>
        private void EventBD( )
        {
            // получение строк соединения и поставщика данных из файла *.config
           SqlConnection asqlconnect = new SqlConnection(  HMI_Settings.ProviderPtkSql );
            try
            {
                asqlconnect.Open();
            } catch
            {
                MessageBox.Show("Нет связи с БД");
                return;
            }

            // формирование данных для вызова хранимой процедуры
            SqlCommand cmd = new SqlCommand( "ShowEventLog", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // входные параметры
            // 1. начальное время
            SqlParameter dtMim = new SqlParameter();
            dtMim.ParameterName = "@Dt_start";
            dtMim.SqlDbType = SqlDbType.DateTime;
            TimeSpan tss = new TimeSpan( 0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second );
            DateTime tim = dtpStartData.Value - tss;
            dtMim.Value = tim;
            dtMim.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( dtMim );

            // 2. конечное время
            SqlParameter dtMax = new SqlParameter();
            dtMax.ParameterName = "@Dt_end";
            dtMax.SqlDbType = SqlDbType.DateTime;
            tss = new TimeSpan( 0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second );
            tim = dtpEndData.Value - tss;
            dtMax.Value = tim;
            dtMax.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( dtMax );

            // 3. идентификатор блока (для конкретизации)
            SqlParameter idBlock = new SqlParameter();
            idBlock.ParameterName = "@Id";
            idBlock.SqlDbType = SqlDbType.Int;
            idBlock.Value = 0;
            idBlock.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( idBlock );
            
            // 4. название события
            SqlParameter strName = new SqlParameter();
            strName.ParameterName = "@Name";
            strName.SqlDbType = SqlDbType.Text;
            strName.Value = "";//SqlString.Null;
            strName.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( strName );

            // заполнение DataSet
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
                MessageBox.Show("Архивные данные недоступны.\nОшибка:" + sex.Message + "\nПовторите запрос.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                asqlconnect.Close();
                aSDA.Dispose();
                aDS.Dispose();
                return;
            }


            asqlconnect.Close();

            //PrintDataSet( aDS );

            // извлекаем данные по авариям
            dt = aDS.Tables[0];

            LinkSetLV( null, true );    // очищаем ListView для обновления  

            StringBuilder ts = new StringBuilder();
            for( int curRow = 0; curRow < dt.Rows.Count; curRow++ )
            {
                ListViewItem li = new ListViewItem();
                li.SubItems.Clear();
                DateTime t = ( DateTime ) dt.Rows[curRow]["LocalTime"];
					 li.Tag = t;	// для сортировки
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
			  // раскращиваем ListView в зебру
				CommonUtils.CommonUtils.DrawAsZebra( lstvEvent );
				aSDA.Dispose();
				aDS.Dispose();
        }
        /*==========================================================================*
          *   private void void LinkSetText(object Value)
          *      для потокобезопасного вызова процедуры
          *==========================================================================*/
        delegate void SetLVCallback( ListViewItem li, bool actDellstV );

        // actDellstV - действия с ListView : false - не трогать; true - очистить;
        public void LinkSetLV( object Value, bool actDellstV )
        {
            if( !( Value is ListViewItem ) && !actDellstV )
                return;   // сгенерировать ошибку через исключение

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
        * //для потокобезопасного вызова процедуры
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
                // включаем таймер периодического чтения
                //MessageBox.Show("Не реализовано");
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
                // отключаем таймер периодического чтения
                //MessageBox.Show( "Не реализовано" );
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
            // запрашиваем данные по действиям пользователя
            //UserBD();
            lstvCurrent = lstvUserAction;
        }

        #region Вход на вкладку с суммарными авариями и осциллограммами
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
            // обновляем
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
           // выводим результаты запроса
           try
           {
              switch (tabControl1.SelectedTab.Text)
              {
                 case "События ОКУ и РЗА":
                    lstvEvent.Items.Clear();
                    EventBD();
                    break;
                 case "Действия пользователей":
                    lstvUserAction.Items.Clear();
                    UserBD();
                    break;
                 case "Сводный список аварий и осциллограмм":
                    dgvOscill.Rows.Clear();
                    dgvAvar.Rows.Clear();
                    AvarBD();
                    OscBD();
                    //OscBD10();
                    //DiagBD();
                    break;
                 case "Сводный системный журнал":
                    lstvLogSystemFull.Items.Clear();
                    LogSystemFullBD();
                    break;
              }
           }
           catch (Exception ex)
           {
              TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 443, DateTime.Now.ToString() + " (443) : frmLogs.cs : bgwBackGround_DoWork() :  ошибка при формировании таблиц: " + ex.Message);
              //this.Close();
           }
           finally 
           {
              lta.Stop();
           }
        }

        private void печатьToolStripMenuItem1_Click( object sender, EventArgs e )
        {
            StringBuilder sb = new StringBuilder();
            ListViewItem li = new ListViewItem();

            sb.Length = 0;
            switch( lstvCurrent.Name )
            {
                case "lstvEvent":
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( " (События ОКУ и РЗА)" );
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
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( " (Действия пользователей)" );
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
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( " (События ОКУ и РЗА)" );
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
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( " (Действия пользователей)" );
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
                    // формируем заголовок листинга
                    sb.Append( "========================================================================\n" );
                    sb.Append( " (Сводный системный журнал)" );
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
        ///     запрос событий из базы посредством хранимой процедуры    
        /// </summary>
        private void AvarBD( )
        {
            dgvAvar.Rows.Clear();
            // получение строк соединения и поставщика данных из файла *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            try
            {
                asqlconnect.Open();
            }
            catch 
            {
               System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + "исключение в : frmLogs : AvarBD() " );
                return;
            }

            // формирование данных для вызова хранимой процедуры
            SqlCommand cmd = new SqlCommand( "ShowDataLog2", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // входные параметры
            // 1. ip FC
            SqlParameter pipFC = new SqlParameter();
            pipFC.ParameterName = "@IP";
            pipFC.SqlDbType = SqlDbType.BigInt;
            pipFC.Value = 0;
            pipFC.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pipFC );
            // 2. id устройства
            SqlParameter pidBlock = new SqlParameter();
            pidBlock.ParameterName = "@id";
            pidBlock.SqlDbType = SqlDbType.Int;
            pidBlock.Value = 0;
            pidBlock.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pidBlock );

            // 3. начальное время
            SqlParameter dtMim = new SqlParameter();
            dtMim.ParameterName = "@dt_start";
            dtMim.SqlDbType = SqlDbType.DateTime;
            TimeSpan tss = new TimeSpan( 0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second );
            DateTime tim = dtpStartData.Value - tss;
            dtMim.Value = tim;
            dtMim.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( dtMim );

            // 2. конечное время
            SqlParameter dtMax = new SqlParameter();
            dtMax.ParameterName = "@dt_end";
            dtMax.SqlDbType = SqlDbType.DateTime;
            tss = new TimeSpan( 0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second );
            tim = dtpEndData.Value - tss;
            dtMax.Value = tim;
            dtMax.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( dtMax );

            // 5. тип записи
            SqlParameter ptypeRec = new SqlParameter();
            ptypeRec.ParameterName = "@type";
            ptypeRec.SqlDbType = SqlDbType.Int;

            ptypeRec.Value = 2; // информация по авариям
            ptypeRec.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( ptypeRec );
            // 6. ид записи журнала
            SqlParameter pid = new SqlParameter();
            pid.ParameterName = "@id_record";
            pid.SqlDbType = SqlDbType.Int;
            pid.Value = 0;
            pid.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pid );

            // заполнение DataSet
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
                MessageBox.Show("Архивные данные недоступны.\nОшибка:" + sex.Message + "\nПовторите запрос.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                asqlconnect.Close();
                aSDA.Dispose();
                aDS.Dispose();
                return;
            }
            asqlconnect.Close();

            //PrintDataSet( aDS );
            // извлекаем данные по аварии
            dtA = aDS.Tables["TbAlarm"];
            for( int curRow = 0; curRow < dtA.Rows.Count; curRow++ )
            {
                int i = dgvAvar.Rows.Add();   // номер строки

                var devGuid = uint.Parse(dtA.Rows[curRow]["BlockID"].ToString());
                var device = HMI_Settings.CONFIGURATION.GetLink2Device(0, devGuid);

                // Тип блока
                dgvAvar["clmBlockName", i].Value = dtA.Rows[curRow]["BlockName"];

                // Присоединение
                if (HMI_Settings.IsDebugMode)
                    dgvAvar["clmComment", i].Value = String.Format("({0}) {1} ({2})", devGuid, device.Description, device.TypeName);
                else
                    dgvAvar["clmComment", i].Value = device.Description;

                // Время блока
                DateTime tmpT = ( DateTime ) dtA.Rows[curRow]["TimeBlock"];
                string tmpTs = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(tmpT);
                dgvAvar["clmBlockTime", i].Value = tmpTs;
                
                // ID аварии
                dgvAvar["clmIDAvar", i].Value = dtA.Rows[curRow]["ID"];

                // Guid блока
                dgvAvar["clmBlockId", i].Value = dtA.Rows[curRow]["BlockID"];
            }
				aSDA.Dispose();
				aDS.Dispose();
        }

        #region формирование и просмотр осциллограмм и диаграмм
        ArrayList asb = new ArrayList();    // для хранения имен файлов в случае объединения осциллограмм
        short cntSelectOSC = 0;             // число выделенных осциллограмм/диаграмм

		/// <summary>
		/// единая процедура формирования списка осциллограмм
		/// и диаграмм для различных типов блоков - 
		/// каких именно - определено в конфиг файле Project.cfg
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
                        throw new Exception("Осциллограммы данного типа устройства не поддерживаются");
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

            // извлекаем данные по осциллограммам или диаграммам
            dtog = oscdg.Do_SQLProc();

            for (int curRow = 0; curRow < dtog.Rows.Count; curRow++)
            {
                int i = dgvOscill.Rows.Add();   // номер строки

                var devGuid = uint.Parse(dtog.Rows[curRow]["BlockID"].ToString());
                var device = HMI_Settings.CONFIGURATION.GetLink2Device(0, devGuid);

                // Тип блока
                dgvOscill["clmBlockNameOsc", i].Value = dtog.Rows[curRow]["BlockName"];

                // Присоединение
                if (HMI_Settings.IsDebugMode)
                    dgvOscill["clmCommentOsc", i].Value = String.Format("({0}) {1} ({2})", devGuid, device.Description, device.TypeName);
                else
                    dgvOscill["clmCommentOsc", i].Value = device.Description;

                // Время блока
                DateTime tmpT = (DateTime)dtog.Rows[curRow]["TimeBlock"];
                string tmpTs = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(tmpT);
                dgvOscill["clmBlockTimeOsc", i].Value = tmpTs;

                // Идентификатор осциллограммы
                dgvOscill["clmID", i].Value = dtog.Rows[curRow]["ID"];

                //dgvOscill["clmViewOsc", i].Value = ; //"Осциллограмма";

                if ((int)dtog.Rows[curRow]["TypeID"] == 5)
                    dgvOscill["clmViewOsc", i].Value = "Диаграмма";
                else
                    dgvOscill["clmViewOsc", i].Value = "Осциллограмма";
            }
        }

        private void UserBD()
        {
           #region код для OleDB
           //xdoc = XDocument.Load(HMI_Settings.PathToPrjFile);

           //string Provider = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Provider").Attribute("value").Value;
           //string Data_Source = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Data_Source").Attribute("value").Value;
           //string Initial_Catalog = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Initial_Catalog").Attribute("value").Value;
           //string Persist_Security_Info = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Persist_Security_Info").Attribute("value").Value;
           //string User_ID = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("User_ID").Attribute("value").Value;
           //string Password = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Password").Attribute("value").Value;
           //string Anything_else = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Element("Anything_else").Attribute("value").Value;

           //// это строка из udl-файла - она рабочая, привледена для сравнения
           ////cstr = "Provider=SQLOLEDB.1;Password=12345;Persist Security Info=True;User ID=asu;Initial Catalog=ptk304;Data Source=192.168.240.1";

           //string cstr = "Provider=" + Provider
           //                + ";Data Source=" + Data_Source
           //                + ";Initial Catalog=" + Initial_Catalog
           //                + ";Persist Security Info=" + Persist_Security_Info
           //                + ";User ID=" + User_ID
           //                + ";Password=" + Password
           //               + "; " + Anything_else;

           //// для контроля на консоль строка соединения
           //Console.WriteLine("OleDbConnection: " + cstr);

           //OleDbConnection oledbconnect = new OleDbConnection(cstr);

           //try
           //{
           //   oledbconnect.Open();
           //}
           //catch
           //{
           //   MessageBox.Show("Нет связи с БД (oledbconnect).\nПроверьте строку подключения (файл .udl)");
           //   return;
           //} 

           // формирование данных для вызова хранимой процедуры
           //OleDbCommand cmd = new OleDbCommand("ShowUserLog", oledbconnect);
           //cmd.CommandType = CommandType.StoredProcedure;
           #endregion

           #region код для SqlDBConnection
           //// получение строк соединения и поставщика данных из файла *.config
           //SqlConnection asqlconnect = new SqlConnection(HMI_Settings.cstr);
           //try
           //{
           //   asqlconnect.Open();
           //}
           //catch
           //{
           //   MessageBox.Show("Нет связи с БД");
           //   return;
           //}

           //// формирование данных для вызова хранимой процедуры
           //SqlCommand cmd = new SqlCommand("ShowUserLog", asqlconnect);
           //cmd.CommandType = CommandType.StoredProcedure;

           //// входные параметры
           //// 1. начальное время
           ////OleDbParameter dtMim = new OleDbParameter();
           //SqlParameter dtMim = new SqlParameter();
           //dtMim.ParameterName = "@MinDate";
           //dtMim.SqlDbType = SqlDbType.DateTime;//.DBTimeStamp;   //OleDbType
           //TimeSpan tss = new TimeSpan(0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second);
           //DateTime tim = dtpStartData.Value - tss;
           //dtMim.Value = tim;
           //dtMim.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(dtMim);

           //// 2. конечное время
           ////OleDbParameter dtMax = new OleDbParameter();
           //SqlParameter dtMax = new SqlParameter();
           //dtMax.ParameterName = "@MaxDate";
           //dtMax.SqlDbType = SqlDbType.DateTime;//.DBTimeStamp;//OleDbType
           //tss = new TimeSpan(0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second);
           //tim = dtpEndData.Value - tss;
           //dtMax.Value = tim;
           //dtMax.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(dtMax);

           //// 3. идентификатор пользователя
           ////OleDbParameter idUser = new OleDbParameter();
           //SqlParameter idUser = new SqlParameter();
           //idUser.ParameterName = "@UserId";
           //idUser.SqlDbType = SqlDbType.Int;//OleDbType
           //idUser.Value = 0;
           //idUser.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(idUser);

           //// 4. идентификатор блока
           ////OleDbParameter idBlock = new OleDbParameter();
           //SqlParameter idBlock = new SqlParameter();
           //idBlock.ParameterName = "@BlockId";
           //idBlock.SqlDbType = SqlDbType.Int;//OleDbType
           //idBlock.Value = 0;
           //idBlock.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(idBlock);

           //// 5. идентификатор АРМ
           ////OleDbParameter idArm = new OleDbParameter();
           //SqlParameter idArm = new SqlParameter();
           //idArm.ParameterName = "@ArmId";
           //idArm.SqlDbType = SqlDbType.Int;
           //idArm.Value = 0;
           //idArm.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(idArm);

           //// 6. идентификатор действия
           ////OleDbParameter idAction = new OleDbParameter();
           //SqlParameter idAction = new SqlParameter();
           //idAction.ParameterName = "@Action";
           //idAction.SqlDbType = SqlDbType.Int;
           //idAction.Value = 0;
           //idAction.Direction = ParameterDirection.Input;
           //cmd.Parameters.Add(idAction);

           //// заполнение DataSet
           //DataSet aDS = new DataSet("ptk");
           //SqlDataAdapter aSDA = new SqlDataAdapter();
           //aSDA.SelectCommand = cmd;

           //try
           //{
           //   aSDA.Fill(aDS);
           //}
           //catch (SqlException sex)
           //{
           //   MessageBox.Show("Архивные данные недоступны.\nОшибка:" + sex.Message + "\nПовторите запрос.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
           //   asqlconnect.Close();
           //   aSDA.Dispose();
           //   aDS.Dispose();
           //   return;
           //}

           //asqlconnect.Close();

           ////PrintDataSet( aDS );

           //// извлекаем данные по пользователью
           //DataTable dt = aDS.Tables[0];

           //LinkSetLVU(null, true);    // очищаем ListView для обновления   
           #endregion

           dt = new DataTable();
           // 1. начальное время
           TimeSpan tss = new TimeSpan(0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second);
           DateTime tim_start = dtpStartData.Value - tss;
           // 2. конечное время           
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

              // время фиксации события ФК
              ts.Length = 0;
              DateTime t = (DateTime)dt.Rows[curRow]["LocalTime"];
              li.Tag = t;	// для сортировки
              string tmpTs = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t);
              //li.SubItems.Add(t.ToShortDateString() + " : " + t.ToLongTimeString() + "." + t.Millisecond);
              li.SubItems.Add(tmpTs);

              // время фиксации события Logger'ом					
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

           // раскращиваем ListView в зебру
           CommonUtils.CommonUtils.DrawAsZebra(lstvUserAction);
           
           #region код для SqlDBConnection
           //aSDA.Dispose();
           //aDS.Dispose();
           #endregion
        }

        /// <summary>
        /// выбор данных осциллограммы или диаграммы из сводного списка осциллограмм/диаграмм и аварий
        /// </summary>
        /// 
        private void dgvOscill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3)
                return;

           string ifa = String.Empty;         // имя файла
           DataGridViewCell de;
           char[] sep = { ' ' };
           int ide = 0; // номер блока
           string strType = String.Empty; // тип блока - осц. или диаг.

           try
           {
              de = dgvOscill["clmID", e.RowIndex];
              ide = (int)de.Value;
              de = dgvOscill["clmViewOsc", e.RowIndex];    // тип - осциллограмма или диаграмма
              strType = (string)de.Value;
           }
           catch
           {
              System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + "исключение в : frmLogs : dgvOscill_CellContentClick() ");
              return;
           }

           /*
            * первый аргумент номер DS,
            * сейсчас для отработки механизма задана константа (0)
            * в дальнейшем нужно придумать механизм когда на данном этапе
            * будет известен реальный номер DS
            */
           // пока можно только осциллограммы старых БМРЗ (dtO)
           oscdg.ShowOSCDg(0, dtO, ide);
           oscdg.ShowOSCDg(0, dtO_100, ide);
           oscdg.ShowOSCDg(0, dtOE, ide);
           oscdg.ShowOSCDg(0, dtG, ide); 
           #region MyRegion
		   // по ide найти запись в dto, извлечь блок с осциллограммой (диаграммой), записать в файл, запустить fastview

           //oscdg.CntSelectOSC = 1;   // пока в этой вкладке можно работать только с одной осциллограммой или диаграммой

           //oscdg.ShowOSCDg(dtO, ide);
           //oscdg.ShowOSCDg(dtOE, ide);

	       #endregion
       }
        #endregion

       /*==========================================================================*
			*   private void void LinkSetText(object Value)
			*      для потокобезопасного вызова процедуры
			*==========================================================================*/
        delegate void SetLVUCallback( ListViewItem li, bool actDellstV );

       // actDellstV - действия с ListView : false - не трогать; true - очистить;
        public void LinkSetLVU( object Value, bool actDellstV )
        {
            if( !( Value is ListViewItem ) && !actDellstV )
                return;   // сгенерировать ошибку через исключение

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
        * //для потокобезопасного вызова процедуры
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
                        @"(1103) : {0} : X:\Projects\40_Tumen_GPP09\Client\HMI_MT\frmLogs.cs : GetDataGridViewCell() : ОШИБКА : {1}",
                        DateTime.Now.ToString(), ex.Message));
                //System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + "исключение в : frmLogs : dgvAvar_CellContentClick() : " + ex.Message);
            }
            return de;
        }

        /// <summary>
       /// выбор конкретной аварии некоторого блока для просмотра
       /// </summary>
       /// <param Name="sender"></param>
       /// <param Name="e"></param> 
       private void dgvAvar_CellContentClick( object sender, DataGridViewCellEventArgs e )
        {
            if (e.ColumnIndex != 3) return;
            
            // номер устройства с цчетом фк

            int numdevfc = Convert.ToInt32(GetDataGridViewCell("clmBlockId", e.RowIndex).Value.ToString());
            
            numdevfc = Convert.ToInt32(numdevfc);
            #warning DsGuid = 0
            IDeviceForm frm = DevicesLibrary.DeviceFormFactory.CreateForm( this, 0, (uint)numdevfc, parent.arrFrm );
            frm.ActivateAndShowTreeGroupWithCategory(HelperControlsLibrary.Category.Crush);

            #region извлекаем данные по аварии в конфигурацию
            // идент блока iFC * 256 + iIDDev
            int ide = (int)GetDataGridViewCell("clmIDAvar", e.RowIndex).Value;

            ArrayList arparam = new ArrayList();
            // номер арх записи в бд
            arparam.Add(ide);
            // строка подключения
            byte[] str_cnt_in_bytes = Encoding.UTF8.GetBytes(HMI_Settings.ProviderPtkSql);
            arparam.Add(str_cnt_in_bytes);

            IRequestData req = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetData(0, (uint)numdevfc, "ArhivAvariBlockData", arparam, ide);
            req.OnReqExecuted += new ReqExecuted(frm.reqAvar_OnReqExecuted);       
            #endregion
        }

        #region вход на вкладку сводного журнала
			/// <summary>
		  /// private void tpLogSystemFull_Enter( object sender, EventArgs e )
		  /// вход на вкладку сводного журнала
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
			 // получение строк соединения и поставщика данных из файла *.config
			 //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
          SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
			 try
			 {
				 asqlconnect.Open();
			 } catch
			 {
				 MessageBox.Show( "Нет связи с БД" );
				 return;
			 }

			 // формирование данных для вызова хранимой процедуры
			 SqlCommand cmd = new SqlCommand( "ShowUnionLog", asqlconnect );
			 cmd.CommandType = CommandType.StoredProcedure;

			 // входные параметры
			 // 1. начальное время
			 SqlParameter dtMim = new SqlParameter();
			 dtMim.ParameterName = "@StartData";
			 dtMim.SqlDbType = SqlDbType.DateTime;
			 TimeSpan tss = new TimeSpan( 0, dtpStartData.Value.Hour - dtpStartTime.Value.Hour, dtpStartData.Value.Minute - dtpStartTime.Value.Minute, dtpStartData.Value.Second - dtpStartTime.Value.Second );
			 DateTime tim = dtpStartData.Value - tss;
			 dtMim.Value = tim;
			 dtMim.Direction = ParameterDirection.Input;
			 cmd.Parameters.Add( dtMim );

			 // 2. конечное время
			 SqlParameter dtMax = new SqlParameter();
			 dtMax.ParameterName = "@EndData";
			 dtMax.SqlDbType = SqlDbType.DateTime;
			 tss = new TimeSpan( 0, dtpEndData.Value.Hour - dtpEndTime.Value.Hour, dtpEndData.Value.Minute - dtpEndTime.Value.Minute, dtpEndData.Value.Second - dtpEndTime.Value.Second );
			 tim = dtpEndData.Value - tss;
			 dtMax.Value = tim;
			 dtMax.Direction = ParameterDirection.Input;
			 cmd.Parameters.Add( dtMax );

			 // заполнение DataSet
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
                 MessageBox.Show("Архивные данные недоступны.\nОшибка:" + sex.Message + "\nПовторите запрос.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 asqlconnect.Close();
                 aSDA.Dispose();
                 aDS.Dispose();
                 return;
             }
			 asqlconnect.Close();

			 //PrintDataSet( aDS );

			 // извлекаем данные по событиям
			 dtLSF = aDS.Tables[0];

			 LinkSetLVLSF( null, true );    // очищаем ListView для обновления  

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
			 // раскращиваем ListView в зебру
			 CommonUtils.CommonUtils.DrawAsZebra( lstvEvent );
			 aSDA.Dispose();
			 aDS.Dispose();
		 }
		 /*==========================================================================*
  *   private void void LinkSetText(object Value)
  *      для потокобезопасного вызова процедуры
  *==========================================================================*/
		 delegate void SetLVLSFCallback( ListViewItem li, bool actDellstV );

		 // actDellstV - действия с ListView : false - не трогать; true - очистить;
		 public void LinkSetLVLSF( object Value, bool actDellstV )
		 {
			 if( !( Value is ListViewItem ) && !actDellstV )
				 return;   // сгенерировать ошибку через исключение

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
		 * //для потокобезопасного вызова процедуры
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
		 /// обработка click'a на заголовке listview
		 /// </summary>
		 /// <param Name="sender"></param>
		 /// <param Name="e"></param>
		 private void lstvEvent_ColumnClick( object sender, ColumnClickEventArgs e )
		 {
			 // сравниваем совпадает ли столбец с последним выбранным столбцом
			 if( e.Column != sortColumn )
			 {
				 sortColumn = e.Column;	// установка сортировки нового столбца
                 lstvEvent.Sorting = System.Windows.Forms.SortOrder.Ascending;	//порядок сортировки по умолчанию
			 }
			 else
			 {
				 // меняем порядок сортировки
                 if (lstvEvent.Sorting == System.Windows.Forms.SortOrder.Ascending)
                     lstvEvent.Sorting = System.Windows.Forms.SortOrder.Descending;
				 else
                     lstvEvent.Sorting = System.Windows.Forms.SortOrder.Ascending;
			 }
			 // вызов метода ручной сортировки
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
			 // сравниваем совпадает ли столбец с последним выбранным столбцом
			 if( e.Column != sortColumn )
			 {
				 sortColumn = e.Column;	// установка сортировки нового столбца
                 lstvUserAction.Sorting = System.Windows.Forms.SortOrder.Ascending;	//порядок сортировки по умолчанию
			 }
			 else
			 {
				 // меняем порядок сортировки
                 if (lstvUserAction.Sorting == System.Windows.Forms.SortOrder.Ascending)
                     lstvUserAction.Sorting = System.Windows.Forms.SortOrder.Descending;
				 else
                     lstvUserAction.Sorting = System.Windows.Forms.SortOrder.Ascending;
			 }
			 // вызов метода ручной сортировки
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
			 // сравниваем совпадает ли столбец с последним выбранным столбцом
			 if( e.Column != sortColumn )
			 {
				 sortColumn = e.Column;	// установка сортировки нового столбца
                 lstvLogSystemFull.Sorting = System.Windows.Forms.SortOrder.Ascending;	//порядок сортировки по умолчанию
			 }
			 else
			 {
				 // меняем порядок сортировки
                 if (lstvLogSystemFull.Sorting == System.Windows.Forms.SortOrder.Ascending)
                     lstvLogSystemFull.Sorting = System.Windows.Forms.SortOrder.Descending;
				 else
                     lstvLogSystemFull.Sorting = System.Windows.Forms.SortOrder.Ascending;
			 }
			 // вызов метода ручной сортировки
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
                // формируем заголовок листинга
                sb.Append( "========================================================================\n" );
                sb.Append( " (События ОКУ и РЗА)" );
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
                // формируем заголовок листинга
                sb.Append( "========================================================================\n" );
                sb.Append( " (Действия пользователей)" );
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
                // формируем заголовок листинга
                sb.Append( "========================================================================\n" );
                sb.Append( " (Сводный системный журнал)" );
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
          saveFileDialog1.Filter = "Ведомости и журналы(*.log)|*.log|Все файлы(*.*)|*.*";
          saveFileDialog1.InitialDirectory = Application.StartupPath;

          if ( DialogResult.OK != saveFileDialog1.ShowDialog( ) )
             return;

          parent.prt.rtbText.SaveFile( saveFileDialog1.FileName, RichTextBoxStreamType.PlainText );

          // вывод текущего журнала в xml файл
          DataSet data = new DataSet( );
          DataTable nt = new DataTable( ( ( TabPage ) lstvCurrent.Parent ).Text );
          // сформируем колонки таблицы
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

			// определить являются ли сравниваемые значения датами
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