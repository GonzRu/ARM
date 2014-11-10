using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MessagePanel.MessagePanelService;

namespace HMI_MT
{
    /// <summary>
    /// Окно, выполняющее квитирование в фоне и отображающее прогресс операции с возможностью отмены
    /// </summary>
    public partial class KvitWindow : Form
    {
        #region Private Metods
        /// <summary>
        /// Фоновый процесс, выполняющий квитирование
        /// </summary>
        private BackgroundWorker _backgroundWorker = new BackgroundWorker();

        /// <summary>
        /// Параметр для фонового процесса
        /// </summary>
        private object _param;
        #endregion

        #region Constructors

        public KvitWindow(List<TableEventLogAlarm> messages, string comment)
        {
            InitializeComponent();

            progressBar1.Maximum = messages.Count;

            InitBackgroundWorker();

            _param = new Tuple<List<TableEventLogAlarm>, string>(messages, comment);
        }

        public KvitWindow(uint deviceGuid, string comment)
        {
            InitializeComponent();

            InitBackgroundWorker();

            _param = new Tuple<uint, string>(deviceGuid, comment);
        }

        public KvitWindow(string comment)
        {
            InitializeComponent();

            InitBackgroundWorker();

            _param = comment;
        }
        #endregion Constructors

        #region Handlers
        /// <summary>
        /// Обработчик нажатия кнопки старта квитирования
        /// </summary>
        private void YesButton_Click(object sender, EventArgs e)
        {
            mainLabel.Text = "Выполняется квитирование...";
            tableLayoutPanel1.RowStyles[4].Height = 0;
            tableLayoutPanel1.RowStyles[2].Height = 40;

            _backgroundWorker.RunWorkerAsync(_param);
        }

        /// <summary>
        /// Обработчик нажатия кнопки отмены квитирования
        /// </summary>
        private void NoButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки отмены фоновой задачи
        /// </summary>
        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            if (_backgroundWorker.WorkerSupportsCancellation)
                _backgroundWorker.CancelAsync();
        }

        private void OkButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Обработчик события изменения прогресса фоновой задачи
        /// </summary>
        private void BackgroundWorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            progressBar1.Value = progressChangedEventArgs.ProgressPercentage;
        }

        /// <summary>
        /// Обработчик события завершения фоновой задачи
        /// </summary>
        private void BackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {


            tableLayoutPanel1.RowStyles[2].Height = 0;
            tableLayoutPanel1.RowStyles[3].Height = 40;

            if (runWorkerCompletedEventArgs.Result is bool)
            {
                var result = (bool)runWorkerCompletedEventArgs.Result;

                if (result)
                    mainLabel.Text = "Квитирование успешно выполнено";
                else
                    mainLabel.Text = "Не удалось квитировать сообщения";
            }
            else
            {
                Tuple<int, int> result = (Tuple<int, int>) runWorkerCompletedEventArgs.Result;

                if (result.Item1 != progressBar1.Maximum)
                    mainLabel.Text = String.Format(
                        "Квитировано {0} сообщений\nНе удалось квитировать {2}\nНе квитировано из-за отмены {1}",
                        result.Item1.ToString(), (progressBar1.Maximum - (int) result.Item1).ToString(),
                        result.Item2.ToString());
                else
                    mainLabel.Text = String.Format("Квитировано {0} сообщений\nНеудалось квитировать {1}", result.Item1,
                                                   result.Item2);
            }
        }
        #endregion Handlers

        #region Private Metods
        private void InitBackgroundWorker()
        {
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.WorkerReportsProgress = true;

            _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorkerOnProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;

            if (doWorkEventArgs.Argument is Tuple<uint, string>)
            {
                var param = doWorkEventArgs.Argument as Tuple<uint, string>;
                var deviceGuid = param.Item1;
                var comment = param.Item2;

                doWorkEventArgs.Result = HMI_MT_Settings.HMI_Settings.MessageProvider.KvotDeviceMessages(deviceGuid, comment);
            }
            else if (doWorkEventArgs.Argument is Tuple<List<TableEventLogAlarm>, string>)
            {
                var param = doWorkEventArgs.Argument as Tuple<List<TableEventLogAlarm>, string>;
                var messages = param.Item1;
                var comment = param.Item2;

                int i = 1;
                int failureCount = 0;
                foreach (var message in messages)
                {
                    if (!HMI_MT_Settings.HMI_Settings.MessageProvider.KvotMessage(message, comment))
                        failureCount++;

                    backgroundWorker.ReportProgress(i++);

                    if ((backgroundWorker.CancellationPending))
                        break;
                }

                doWorkEventArgs.Result = new Tuple<int, int>(i - 1 - failureCount, failureCount);
            }
            else if (doWorkEventArgs.Argument is String)
            {
                var param = doWorkEventArgs.Argument as string;

                doWorkEventArgs.Result = HMI_MT_Settings.HMI_Settings.MessageProvider.KvotAllMessages(param);
                backgroundWorker.ReportProgress(100);
            }
        }
        #endregion Private Metods
    }
}
