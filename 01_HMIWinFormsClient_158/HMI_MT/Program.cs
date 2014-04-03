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
        //[MTAThread] // !!!!!!!!!!!!!!!!!!!!!!!!!!!! === для OPC это значение обязательно === !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private static void Main()
        {
            // обнаруживаем предыдущий экземпляр приложения
            //foreach(Process p in Process.GetProcessesByName(Application.ProductName))
            //{
            //   if( p.Id != Process.GetCurrentProcess().Id )
            //   {
            //      MessageBox.Show( "Приложение уже запущено" );
            //      Application.Exit();
            //      return;
            //   }
            //}
            // регистрация глобального обработчика ошибок
            // в отладчке глобальный обработчик ошибок не работает !!!

            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new SplashScreenWindow());

            Application.Run(new MainForm());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // подготовка содержательного сообщения
            string msg = String.Format("(Program.cs) В приложении обнаружена ошибка: \n\n {0}", e.GetType());
            // желает ли пользователь завершить приложение
            DialogResult rezult =
                MessageBox.Show(
                    msg + "\n" + Process.GetCurrentProcess().WorkingSet64.ToString() + " (ФП); " +
                    Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (ВП); "
                    + Process.GetCurrentProcess().Threads.Count.ToString() + " - потоков; " +
                    Process.GetCurrentProcess().HandleCount.ToString() + " - дескр.; ", "Ошибка",
                    MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            if (rezult == DialogResult.Abort)

                TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 52,
                                                      DateTime.Now.ToString() +
                                                      " : (52) Program.cs : CurrentDomain_UnhandledException() : Выход из приложения");
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
                // подготовка содержательного сообщения
                string msg = String.Format("(Program.cs) В приложении обнаружена ошибка: \n\n {0} \n\n {1}",
                                           e.Exception.Message, e.Exception.StackTrace);

                // желает ли пользователь завершить приложение
                switch (
                    MessageBox.Show(
                        msg + "\n" + Process.GetCurrentProcess().WorkingSet64.ToString() + " (ФП); " +
                        Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (ВП); "
                        + Process.GetCurrentProcess().Threads.Count.ToString() + " - потоков; " +
                        Process.GetCurrentProcess().HandleCount.ToString() + " - дескр.; ", "Ошибка",
                        MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error).ToString())
                {
                    case "Abort":
                        TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 74,
                                                              DateTime.Now.ToString() +
                                                              " : (74) Program.cs : Application_ThreadException() : Выход из приложения");
                        TraceSourceDiagMes.FlushLog();

                        Application.Exit();
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                // если информационное окно отобразить не удалось, отобразим более простое сообщение
                try
                {
                    MessageBox.Show(
                        "(Program.cs) Приложение будет закрыто по ошибке\n" +
                        Process.GetCurrentProcess().WorkingSet64.ToString() + " (ФП); " +
                        Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (ВП); "
                        + Process.GetCurrentProcess().Threads.Count.ToString() + " - потоков; " +
                        Process.GetCurrentProcess().HandleCount.ToString() + " - дескр.; ", "Критическая ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 93,
                                                          DateTime.Now.ToString() +
                                                          " : (93) Program.cs : Application_ThreadException() : Выход из приложения");
                    TraceSourceDiagMes.FlushLog();

                    Application.Exit();
                }
            }
        }
    }
}