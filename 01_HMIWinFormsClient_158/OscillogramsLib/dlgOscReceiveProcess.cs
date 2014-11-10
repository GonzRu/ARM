using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OscillogramsLib
{
   public partial class dlgOscReceiveProcess : Form
   {
      System.Timers.Timer tmr_progressOSC;
      DateTime _startTime;

      public dlgOscReceiveProcess()
      {
         InitializeComponent();

         tmr_progressOSC = new System.Timers.Timer();
         tmr_progressOSC.Elapsed += new System.Timers.ElapsedEventHandler(tmr_progressOSC_Elapsed);
         tmr_progressOSC.Interval = 1000;
         tmr_progressOSC.Start();

         this.Focus();
         _startTime = DateTime.Now;
      }

      private void btnOSCReceiveCancel_Click(object sender, EventArgs e)
      {
         tmr_progressOSC.Stop();
         MessageBox.Show("Чтение текущей осциллограммы из БД прервано.", String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
         Close();
      }

      string[] oscstudy = new string[] { "--", "\\", "|", "/", "--","\\","|", "/" };
      ushort cnt_image_progress = 0;
      /// <summary>
      /// вывод прогресса чтения осциллограммы в сек и изобразении
      /// </summary>
      void tmr_progressOSC_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {         
			try
			{
                 lblOSCPercent.Text = oscstudy[cnt_image_progress];
                 cnt_image_progress++;
                 if (cnt_image_progress == 8)
                    cnt_image_progress = 0;

                 lblOSCTime.Text = ((int)(DateTime.Now - _startTime).TotalSeconds).ToString() + " сек.";
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

      }

      /// <summary>
      /// обработчик таймера при неудачном чтении осциллограммы
      /// </summary>
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
