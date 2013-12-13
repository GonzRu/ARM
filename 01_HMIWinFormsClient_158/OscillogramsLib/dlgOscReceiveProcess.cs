using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.IO.Compression;
using System.Threading;
using System.IO.Pipes;
using System.Diagnostics;
using InterfaceLibrary;

namespace OscillogramsLib
{
   public partial class dlgOscReceiveProcess : Form
   {
      BackgroundWorker bgwosc = new BackgroundWorker();
      //ClientDataForExchange cdfe;
      //bool isreceiveOSCsuccess;
      System.Timers.Timer tmr_receiveOSC;
      System.Timers.Timer tmr_progressOSC;
      IOscillogramma OSC = null;

      public dlgOscReceiveProcess(IOscillogramma osc)
      {
         InitializeComponent();

         tmr_receiveOSC = new System.Timers.Timer();
         tmr_receiveOSC.Elapsed += new System.Timers.ElapsedEventHandler(tmr_receiveOSC_Elapsed);
         tmr_receiveOSC.Interval = 120000;
         tmr_receiveOSC.Stop();

         tmr_progressOSC = new System.Timers.Timer();
         tmr_progressOSC.Elapsed += new System.Timers.ElapsedEventHandler(tmr_progressOSC_Elapsed);
         tmr_progressOSC.Interval = 1000;
         tmr_progressOSC.Start();

         OSC = osc;
         OSC.OnOscReady += new OSCReadyHandler(osc_OnOscReady);

         this.Focus();
      }

      void osc_OnOscReady(IOscillogramma osc)
      {
          Close();
      }

      //private void DoWork() 
      //{
      //   cdfe.IsOSCinProcessing = true;
      //   progressBarOSC.Value = 0;
      //   bgwosc.DoWork += new DoWorkEventHandler(bgwosc_DoWork);
      //   bgwosc.WorkerSupportsCancellation = true;
      //   bgwosc.WorkerReportsProgress = true;
      //   bgwosc.ProgressChanged += new ProgressChangedEventHandler(bgwosc_ProgressChanged);

      //   bgwosc.RunWorkerAsync();
      //}

      private void btnOSCReceiveCancel_Click(object sender, EventArgs e)
      {
         tmr_progressOSC.Stop();
         tmr_receiveOSC.Stop();
         // установить флаг необходимости прервать чтение осциллограммы
         //cdfe.CancelOSCRead = true;
         //cdfe.slOsc4Read.Clear();
         MessageBox.Show("Чтение текущей осциллограммы из БД прервано.", String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
         Close();
      }

      string[] oscstudy = new string[] { "--", "\\", "|", "/", "--","\\","|", "/" };
      ushort cnt_image_progress = 0;
      /// <summary>
      /// вывод прогресса чтения осциллограммы в сек и изобразении
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      void tmr_progressOSC_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {         
			try
			{
                 lblOSCPercent.Text = oscstudy[cnt_image_progress];
                 cnt_image_progress++;
                 if (cnt_image_progress == 8)
                    cnt_image_progress = 0;

                 lblOSCTime.Text = OSC.GetStrTimeReadOSC() + " сек.";
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

      }

      /// <summary>
      /// обработчик таймера при неудачном чтении осциллограммы
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      void tmr_receiveOSC_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {
          tmr_progressOSC.Stop();

         TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 208, DateTime.Now.ToString() + " : HMI_MT.dlgOscReceiveProcess.cs : tmr_receiveOSC_Elapsed :  срабатывание таймера при чтении осциллограммы : ");

         Close();
      }

      private void dlgOscReceiveProcess_FormClosing(object sender, FormClosingEventArgs e)
      {
         try
         {
             tmr_progressOSC.Stop();
             //cdfe.IsOSCinProcessing = false;
             
             this.Dispose();
            this.Close();
         }
         catch (Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 225, DateTime.Now.ToString() + " : HMI_MT.dlgOscReceiveProcess.cs : CloseDLG :  ошибка при закрытии формы : " + ex.Message);
         }
      }
   }
}
