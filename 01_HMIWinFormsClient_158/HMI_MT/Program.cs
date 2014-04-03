using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using HMI_MT.Windows.SplashScreen;
using TraceSourceLib;

namespace HMI_MT
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //[MTAThread] // !!!!!!!!!!!!!!!!!!!!!!!!!!!! === ��� OPC ��� �������� ����������� === !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private static void Main()
        {
            // ������������ ���������� ��������� ����������
            //foreach(Process p in Process.GetProcessesByName(Application.ProductName))
            //{
            //   if( p.Id != Process.GetCurrentProcess().Id )
            //   {
            //      MessageBox.Show( "���������� ��� ��������" );
            //      Application.Exit();
            //      return;
            //   }
            //}
            // ����������� ����������� ����������� ������
            // � �������� ���������� ���������� ������ �� �������� !!!

            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new SplashScreenWindow());

            Application.Run(new MainForm());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // ���������� ��������������� ���������
            string msg = String.Format("(Program.cs) � ���������� ���������� ������: \n\n {0}", e.GetType());
            // ������ �� ������������ ��������� ����������
            DialogResult rezult =
                MessageBox.Show(
                    msg + "\n" + Process.GetCurrentProcess().WorkingSet64.ToString() + " (��); " +
                    Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (��); "
                    + Process.GetCurrentProcess().Threads.Count.ToString() + " - �������; " +
                    Process.GetCurrentProcess().HandleCount.ToString() + " - �����.; ", "������",
                    MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            if (rezult == DialogResult.Abort)

                TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 52,
                                                      DateTime.Now.ToString() +
                                                      " : (52) Program.cs : CurrentDomain_UnhandledException() : ����� �� ����������");
            TraceSourceDiagMes.FlushLog();

            Application.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                // ���������� ��������������� ���������
                string msg = String.Format("(Program.cs) � ���������� ���������� ������: \n\n {0} \n\n {1}",
                                           e.Exception.Message, e.Exception.StackTrace);

                // ������ �� ������������ ��������� ����������
                switch (
                    MessageBox.Show(
                        msg + "\n" + Process.GetCurrentProcess().WorkingSet64.ToString() + " (��); " +
                        Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (��); "
                        + Process.GetCurrentProcess().Threads.Count.ToString() + " - �������; " +
                        Process.GetCurrentProcess().HandleCount.ToString() + " - �����.; ", "������",
                        MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error).ToString())
                {
                    case "Abort":
                        TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 74,
                                                              DateTime.Now.ToString() +
                                                              " : (74) Program.cs : Application_ThreadException() : ����� �� ����������");
                        TraceSourceDiagMes.FlushLog();

                        Application.Exit();
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                // ���� �������������� ���� ���������� �� �������, ��������� ����� ������� ���������
                try
                {
                    MessageBox.Show(
                        "(Program.cs) ���������� ����� ������� �� ������\n" +
                        Process.GetCurrentProcess().WorkingSet64.ToString() + " (��); " +
                        Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (��); "
                        + Process.GetCurrentProcess().Threads.Count.ToString() + " - �������; " +
                        Process.GetCurrentProcess().HandleCount.ToString() + " - �����.; ", "����������� ������",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 93,
                                                          DateTime.Now.ToString() +
                                                          " : (93) Program.cs : Application_ThreadException() : ����� �� ����������");
                    TraceSourceDiagMes.FlushLog();

                    Application.Exit();
                }
            }
        }
    }
}