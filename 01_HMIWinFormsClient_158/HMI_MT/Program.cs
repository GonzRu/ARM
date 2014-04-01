using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using HMI_MT_Settings;

namespace HMI_MT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        //[MTAThread] // !!!!!!!!!!!!!!!!!!!!!!!!!!!! === для OPC это значение обязательно === !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        static void Main()
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

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler( Application_ThreadException );
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler( CurrentDomain_UnhandledException );

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            // проверяем существование файла конфигурации  проекта Project.cfg и файла конфигурации устройств проекта
            string PathToPrjFile = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + "Project.cfg";

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Project"))
            {
                MessageBox.Show("Не найдена папка проекта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ( !File.Exists( PathToPrjFile ) )
            {
                MessageBox.Show("Не найден файл Project.cfg", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                HMI_Settings.PathToPrjFile = PathToPrjFile;
                HMI_Settings.XDoc4PathToPrjFile = XDocument.Load( HMI_Settings.PathToPrjFile );
            }
            catch ( Exception )
            {
                throw new Exception( string.Format( "(423) : MainForm.cs : SetNamesCfgPrgFiles() : Несуществующий файл = {0}", PathToPrjFile ) );
            }

            if ( HMI_MT_Settings.HMI_Settings.XDoc4PathToPrjFile.Element( "Project" ).Elements( "TextListener" ).Count() > 0 )
            {
                string namef = HMI_MT_Settings.HMI_Settings.XDoc4PathToPrjFile.Element( "Project" ).Element( "TextListener" ).Attribute( "fileName" ).Value;
                int sizef = int.Parse( HMI_MT_Settings.HMI_Settings.XDoc4PathToPrjFile.Element( "Project" ).Element( "TextListener" ).Attribute( "fsize_kbait" ).Value );

                CommonUtils.LogMonitoring lm;

                if ( File.Exists( namef ) )
                    lm = new CommonUtils.LogMonitoring( string.Format( "{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, namef ), sizef );
            }

            StartTrace();

            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Critical, 37, DateTime.Now.ToString() + " : (37) Program.cs : Main() : Запуск приложения" );
            TraceSourceLib.TraceSourceDiagMes.FlushLog();

            Application.Run( new MainForm() );
        }
        /// <summary>
        ///старт процесса трассировки приложения
        /// </summary>
        private static void StartTrace()
        {
            TraceSourceLib.TraceSourceDiagMes.CreateLog( "HMI_MT_Client_TraceSource" );
            TraceSourceLib.TraceSourceDiagMes.tracesource.TraceData( TraceEventType.Verbose, 1, "StartTest" );
            TraceSourceLib.TraceSourceDiagMes.tracesource.Flush();
        }

        static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
        {
            // подготовка содержательного сообщения
            string msg = String.Format( "(Program.cs) В приложении обнаружена ошибка: \n\n {0}", e.GetType().ToString() );
            // желает ли пользователь завершить приложение
            DialogResult rezult = MessageBox.Show( msg + "\n" + Process.GetCurrentProcess().WorkingSet64.ToString() + " (ФП); " + Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (ВП); "
            + Process.GetCurrentProcess().Threads.Count.ToString() + " - потоков; " + Process.GetCurrentProcess().HandleCount.ToString() + " - дескр.; ", "Ошибка", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error );
            if ( rezult == DialogResult.Abort )

                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Critical, 52, DateTime.Now.ToString() + " : (52) Program.cs : CurrentDomain_UnhandledException() : Выход из приложения" );
            TraceSourceLib.TraceSourceDiagMes.FlushLog();

            Application.Exit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        static void Application_ThreadException( object sender, ThreadExceptionEventArgs e )
        {
            try
            {
                // подготовка содержательного сообщения
                string msg = String.Format( "(Program.cs) В приложении обнаружена ошибка: \n\n {0} \n\n {1}", e.Exception.Message, e.Exception.StackTrace );

                // желает ли пользователь завершить приложение
                switch ( MessageBox.Show( msg + "\n" + Process.GetCurrentProcess().WorkingSet64.ToString() + " (ФП); " + Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (ВП); "
                    + Process.GetCurrentProcess().Threads.Count.ToString() + " - потоков; " + Process.GetCurrentProcess().HandleCount.ToString() + " - дескр.; ", "Ошибка", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error ).ToString() )
                {
                    case "Abort":
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Critical, 74, DateTime.Now.ToString() + " : (74) Program.cs : Application_ThreadException() : Выход из приложения" );
                        TraceSourceLib.TraceSourceDiagMes.FlushLog();

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
                    MessageBox.Show( "(Program.cs) Приложение будет закрыто по ошибке\n" + Process.GetCurrentProcess().WorkingSet64.ToString() + " (ФП); " + Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (ВП); "
                + Process.GetCurrentProcess().Threads.Count.ToString() + " - потоков; " + Process.GetCurrentProcess().HandleCount.ToString() + " - дескр.; ", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
                finally
                {
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Critical, 93, DateTime.Now.ToString() + " : (93) Program.cs : Application_ThreadException() : Выход из приложения" );
                    TraceSourceLib.TraceSourceDiagMes.FlushLog();

                    Application.Exit();
                }
            }
        }
    }
}