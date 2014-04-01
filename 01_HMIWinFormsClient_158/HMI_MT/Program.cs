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
        //[MTAThread] // !!!!!!!!!!!!!!!!!!!!!!!!!!!! === ��� OPC ��� �������� ����������� === !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        static void Main()
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

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler( Application_ThreadException );
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler( CurrentDomain_UnhandledException );

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            // ��������� ������������� ����� ������������  ������� Project.cfg � ����� ������������ ��������� �������
            string PathToPrjFile = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + "Project.cfg";

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Project"))
            {
                MessageBox.Show("�� ������� ����� �������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ( !File.Exists( PathToPrjFile ) )
            {
                MessageBox.Show("�� ������ ���� Project.cfg", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                HMI_Settings.PathToPrjFile = PathToPrjFile;
                HMI_Settings.XDoc4PathToPrjFile = XDocument.Load( HMI_Settings.PathToPrjFile );
            }
            catch ( Exception )
            {
                throw new Exception( string.Format( "(423) : MainForm.cs : SetNamesCfgPrgFiles() : �������������� ���� = {0}", PathToPrjFile ) );
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

            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Critical, 37, DateTime.Now.ToString() + " : (37) Program.cs : Main() : ������ ����������" );
            TraceSourceLib.TraceSourceDiagMes.FlushLog();

            Application.Run( new MainForm() );
        }
        /// <summary>
        ///����� �������� ����������� ����������
        /// </summary>
        private static void StartTrace()
        {
            TraceSourceLib.TraceSourceDiagMes.CreateLog( "HMI_MT_Client_TraceSource" );
            TraceSourceLib.TraceSourceDiagMes.tracesource.TraceData( TraceEventType.Verbose, 1, "StartTest" );
            TraceSourceLib.TraceSourceDiagMes.tracesource.Flush();
        }

        static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
        {
            // ���������� ��������������� ���������
            string msg = String.Format( "(Program.cs) � ���������� ���������� ������: \n\n {0}", e.GetType().ToString() );
            // ������ �� ������������ ��������� ����������
            DialogResult rezult = MessageBox.Show( msg + "\n" + Process.GetCurrentProcess().WorkingSet64.ToString() + " (��); " + Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (��); "
            + Process.GetCurrentProcess().Threads.Count.ToString() + " - �������; " + Process.GetCurrentProcess().HandleCount.ToString() + " - �����.; ", "������", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error );
            if ( rezult == DialogResult.Abort )

                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Critical, 52, DateTime.Now.ToString() + " : (52) Program.cs : CurrentDomain_UnhandledException() : ����� �� ����������" );
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
                // ���������� ��������������� ���������
                string msg = String.Format( "(Program.cs) � ���������� ���������� ������: \n\n {0} \n\n {1}", e.Exception.Message, e.Exception.StackTrace );

                // ������ �� ������������ ��������� ����������
                switch ( MessageBox.Show( msg + "\n" + Process.GetCurrentProcess().WorkingSet64.ToString() + " (��); " + Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (��); "
                    + Process.GetCurrentProcess().Threads.Count.ToString() + " - �������; " + Process.GetCurrentProcess().HandleCount.ToString() + " - �����.; ", "������", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error ).ToString() )
                {
                    case "Abort":
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Critical, 74, DateTime.Now.ToString() + " : (74) Program.cs : Application_ThreadException() : ����� �� ����������" );
                        TraceSourceLib.TraceSourceDiagMes.FlushLog();

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
                    MessageBox.Show( "(Program.cs) ���������� ����� ������� �� ������\n" + Process.GetCurrentProcess().WorkingSet64.ToString() + " (��); " + Process.GetCurrentProcess().VirtualMemorySize64.ToString() + " (��); "
                + Process.GetCurrentProcess().Threads.Count.ToString() + " - �������; " + Process.GetCurrentProcess().HandleCount.ToString() + " - �����.; ", "����������� ������", MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
                finally
                {
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Critical, 93, DateTime.Now.ToString() + " : (93) Program.cs : Application_ThreadException() : ����� �� ����������" );
                    TraceSourceLib.TraceSourceDiagMes.FlushLog();

                    Application.Exit();
                }
            }
        }
    }
}