using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace TraceSourceLib
{
	public class TraceSourceDiagMes
   {
      #region формирование лога - используется TraceSource
      public static TraceSource tracesource;
      //static FileLogTraceListener fileLogTraceListener; // слушатель в файл
		/// <summary>
		/// temp-строка
		/// </summary>
	  static StringBuilder strmsg = new StringBuilder();
      
      public static void CreateLog(string name)
      {
         // создаем Trace
         tracesource = new TraceSource(name);
         foreach( TraceListener tl in tracesource.Listeners)
         {
            StringDictionary sd  = tl.Attributes;
         }
         
         #region создаем слушатель в файл
         //fileLogTraceListener = new FileLogTraceListener("HMI_MT_Client_FileLogTraceListener");
         //fileLogTraceListener.TraceOutputOptions = TraceOptions.DateTime;
         //fileLogTraceListener.Append = false;
         //fileLogTraceListener.BaseFileName = "HMI_MT_Client";
         //fileLogTraceListener.LogFileCreationSchedule = LogFileCreationScheduleOption.Daily;
         //fileLogTraceListener.Location = LogFileLocation.Custom;
         //fileLogTraceListener.CustomLocation = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Logs";
         //fileLogTraceListener.MaxFileSize = 524288;   //512 Кб
         //fileLogTraceListener.DiskSpaceExhaustedBehavior = DiskSpaceExhaustedOption.ThrowException;
         //fileLogTraceListener.ReserveDiskSpace = 1073741824; // 1 Гб оставшегося места на диске гарантировано

         //// добавляем слушателей в TraceSource
         //tracesource.Listeners.Add(fileLogTraceListener);
         //tracesource.Switch.Level = SourceLevels.All;

         //ConsoleTraceListener ctl = new ConsoleTraceListener();
         //tracesource.Listeners.Add(ctl);

         //TextWriterTraceListener twtl = new TextWriterTraceListener();
         //twtl.
         #endregion
      }

      /// <summary>
      /// закрытие лога трассировки
      /// </summary>
      public static void CloseLog()
      {
         tracesource.Flush();
         tracesource.Close();
      }

      /// <summary>
      /// вывод диагностического сообщения
      /// </summary>
      /// <param name="tet"></param>
      /// <param name="idevent"></param>
      /// <param name="msg"></param>
	  public static void WriteDiagnosticMSG(TraceEventType tet, int idevent, string msg)
      {
		  if (tracesource == null)
			  throw new Exception("TraceSourceLib.cs (68) : Трассировка приложения не настроена : tracesource = null");

         tracesource.TraceEvent(tet, idevent, msg);
      }

	  /// <summary>
	  /// диагностическое сообщение по исключению
	  /// с выводом инф. по контексту : 
	  /// TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
	  /// </summary>
	  /// <param name="tet"></param>
	  /// <param name="idevent"></param>
	  /// <param name="msg"></param>
	  public static void WriteDiagnosticMSG(Exception ex)
	  {
		  try
		  {
			  if (tracesource == null)
				  throw new Exception("TraceSourceLib.cs (68) : Трассировка приложения не настроена : tracesource = null");

			  strmsg.Clear();
			  strmsg.Append(DateTime.Now.ToString() + " : " + ex.Source + " : " + ex.TargetSite + "\n" + ex.ToString() + "\n" + ex.StackTrace);

			  tracesource.TraceEvent(TraceEventType.Error, 0, strmsg.ToString());
			  tracesource.Flush();
		  }
		  catch (Exception exx)
		  {
			  Console.WriteLine(exx.Message);
		  }
	  }

      /// <summary>
      /// вывод дампа для контроля (позже определить этот вывод только для отладчного режима)
      /// </summary>
      /// <param name="dumpTitle"></param>
      /// <param name="source"></param>
      public static void WriteDump(TraceEventType tet, int idevent, string dumpTitle, object source)
      {
		 strmsg.Clear();

		 if (tracesource == null)
			 throw new Exception("TraceSourceLib.cs (68) : Трассировка приложения не настроена : tracesource = null");

         tracesource.TraceEvent(tet, idevent, dumpTitle);

         if ((source as string) != null)
            //WriteDiagnosticMSG(source as string);      // формируем строку  
            tracesource.TraceEvent(tet, idevent, source as string);
         else if ((source as MemoryStream) != null && (source as MemoryStream).Length > 0) 
         {
            // содержимое пакета
               tracesource.TraceEvent(TraceEventType.Error, idevent, "Содержимое пакета :");
               tracesource.TraceData(TraceEventType.Error, idevent, BitConverter.ToString((source as MemoryStream).ToArray(), 0, (source as MemoryStream).ToArray().Length));
         }
         else if ((source as MemoryStream) != null && (source as MemoryStream).Length == 0)
             tracesource.TraceEvent(TraceEventType.Error, idevent, "Содержимое пакета : пустой пакет.");
          else
			 WriteDiagnosticMSG(tet, idevent, DateTime.Now.ToString() + " (80) : HMI_MT.ServerDataForExchange.cs : WriteDump : ошибка типа для формирования дампа : ");
      }

      public static void FlushLog()
      {
         tracesource.Flush();
      }
      #endregion
   }
}
