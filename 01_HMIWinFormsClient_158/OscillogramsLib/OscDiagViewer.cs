using HMI_MT_Settings;
using Ionic.Zip;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OscillogramsLib
{
   public class OscDiagViewer
   {
      #region private
      string namecmdproc;
      SqlConnection asqlconnect;
      DataSet aDS;
      SqlDataAdapter aSDA;
      #endregion

      #region Свойства
      /// <summary>
      /// id устройства
      /// </summary>
      public int IdDev
      {
          get { return idDev; }
          set { idDev = value; }
      }
      int idDev;

      /// <summary>
      /// тип записи
      /// </summary>
      public int TypeRec//TypeBlockData
      {
          set { typeRec = value; }
      }
      int typeRec;//TypeBlockData 
      /// <summary>
      /// начальная дата
      /// </summary>
      public DateTime DTStartData
      {
         set { dTStartData = value; }
      }
      DateTime dTStartData;
      /// <summary>
      /// начальное время
      /// </summary>
      public DateTime DTStartTime
      {
         set { dTStartTime = value; }
      }
      DateTime dTStartTime;
      /// <summary>
      /// конечная дата
      /// </summary>
      public DateTime DTEndData
      {
         set { dTEndData = value; }
      }
      DateTime dTEndData;
      /// <summary>
      /// конечное время
      /// </summary>
      public DateTime DTEndTime
      {
         set { dTEndTime = value; }
      }
      DateTime dTEndTime;

      #endregion

      #region конструктор, уничтожение объекта
      public OscDiagViewer()
      {
         // получение строк соединения и поставщика данных из файла *.config
         asqlconnect = new SqlConnection(HMI_MT_Settings.HMI_Settings.ProviderPtkSql);
         try
         {
            asqlconnect.Open();
         }
         catch
         {
            System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + "исключение в : frmLogs : OscBD() ");
            return;
         }
      }

      /// <summary>
      /// флаг для определения того, 
      /// вызывался ли метод Dispose()
      /// </summary>
      private bool isdisposed = false;

      public void Dispose()
      {
         // вызываем метод с кодом очистки
         DeleteThis(true);

         // подавляем финализацию
         GC.SuppressFinalize(this);
      }

      /// <summary>
      /// Код очистки
      /// </summary>
      /// <param Name="whois">true - означает, что очистку инициировало приложение, а не сборщик мусора</param>
      private void DeleteThis(bool whois)
      {
         // проверка на то, выполнялась ли очистка
         if (!isdisposed)
         {
            // если whois == true, освобождаем все управляемые ресурсы
            if (whois)
            {
               // здесь осущ очистку всех управляемых ресурсов
               // ...
               aSDA.Dispose();
               aDS.Dispose();
            }
            // здесь осущ очистку всех НЕуправляемых ресурсов
            // ...
         }
         isdisposed = true;
      }
      ~OscDiagViewer()
      {
         // вызываем метод очистки, 
         // false - означает, что очистка была инициирована сборщиком мусора
         DeleteThis(false);
      }
      #endregion

      #region Считывание списка осциллограмм

      public DataTable Do_SQLProc()
      {
         namecmdproc = "ShowDataLog2";

         // формирование данных для вызова хранимой процедуры
         SqlCommand cmd = new SqlCommand(namecmdproc, asqlconnect);
         cmd.CommandType = CommandType.StoredProcedure;
         cmd.CommandTimeout = 3000;
         GetCMDParams(ref cmd);

         // заполнение DataSet
         try 
         {
            aDS = new DataSet("ptk");
            aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            aSDA.Fill(aDS);
         }catch(Exception ex)
         {
            System.Windows.Forms.MessageBox.Show(ex.Message);
         }
         
         asqlconnect.Close();

         return aDS.Tables[0];
      }

      void GetCMDParams(ref SqlCommand cmd)
      {
         // входные параметры
         // 1. ip FC
         SqlParameter pipFC = new SqlParameter();
         pipFC.ParameterName = "@IP";
         pipFC.SqlDbType = SqlDbType.BigInt;
         pipFC.Value = 0;
         pipFC.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(pipFC);

         // 2. id устройства
         SqlParameter pidBlock = new SqlParameter();
         pidBlock.ParameterName = "@id";
         pidBlock.SqlDbType = SqlDbType.Int;
         pidBlock.Value = idDev;
         pidBlock.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(pidBlock);

         // 3. начальное время
         SqlParameter dtMim = new SqlParameter();
         dtMim.ParameterName = "@dt_start";
         dtMim.SqlDbType = SqlDbType.DateTime;
         TimeSpan tss = new TimeSpan(0, dTStartData.Hour - dTStartTime.Hour, dTStartData.Minute - dTStartTime.Minute, dTStartData.Second - dTStartTime.Second);
         DateTime tim = dTStartData - tss;
         dtMim.Value = tim;
         dtMim.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(dtMim);          

         // 2. конечное время
         SqlParameter dtMax = new SqlParameter();
         dtMax.ParameterName = "@dt_end";
         dtMax.SqlDbType = SqlDbType.DateTime;
         tss = new TimeSpan(0, dTEndData.Hour - dTEndTime.Hour, dTEndData.Minute - dTEndTime.Minute, dTEndData.Second - dTEndTime.Second);
         tim = dTEndData - tss;
         dtMax.Value = tim;
         dtMax.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(dtMax);          

         // 5. тип записи
         SqlParameter ptypeRec = new SqlParameter();
         ptypeRec.ParameterName = "@type";
         ptypeRec.SqlDbType = SqlDbType.Int;

         ptypeRec.Value = typeRec;
         ptypeRec.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(ptypeRec);          

         // 6. ид записи журнала
         SqlParameter pid = new SqlParameter();
         pid.ParameterName = "@id_record";
         pid.SqlDbType = SqlDbType.Int;
         pid.Value = 0;
         pid.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(pid);  
      }

      #endregion

      /// <summary>
      /// сформировать имя файла и вызвать OscView
      /// </summary>
      public void ShowOSCDg(UInt16 dsGuid, int oscGuid)
      {
          using (dlgOscReceiveProcess dlgORP = new dlgOscReceiveProcess())
          {
              var cancallationTokenSource = new System.Threading.CancellationTokenSource();

              var task = new Task<bool>(() => StartDownloadOscillogram(dsGuid, oscGuid, cancallationTokenSource.Token));
              task.ContinueWith((t) =>
              {
                  dlgORP.Close();

                  if (t.Result)
                      // Конечно тот ещё говнокод я тут намутил, но что поделать
                      MessageBox.Show(Application.OpenForms[0], "Не удалось скачать осциллограмму.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
              });

              task.Start();

              dlgORP.ShowDialog();

              if (dlgORP.DialogResult == DialogResult.Cancel)
              {
                  cancallationTokenSource.Cancel();
              }
          }
      }

      /// <summary>
      /// Делает запрос на осциллограмму, скачивает и запускает её
      /// </summary>
      /// <returns>Возвращает false, если всё прошло хорошо. True - если не удалось показать осциллограмму</returns>
      private bool StartDownloadOscillogram(UInt16 dsGuid, int oscGuid, CancellationToken cancellationToken)
      {
          #region Получение содержимого ахива с осциллограммами от роутера
          var oscDataAndName = HMI_Settings.CONFIGURATION.GetDataServer(dsGuid).ReqEntry.GetOscillogramAsByteArray(dsGuid, oscGuid);

          if (oscDataAndName == null)
              return true;
          #endregion

          if (cancellationToken.IsCancellationRequested)
              return false;

          #region Сохранение архива с осциллограммами
          var pathToTempFile = Path.GetTempFileName();

          var stream = File.Create(pathToTempFile);
          stream.Write(oscDataAndName.Item1, 0, oscDataAndName.Item1.Length);

          stream.Close();
          #endregion

          if (cancellationToken.IsCancellationRequested)
              return false;

          #region Распаковка архива
          var pathToOscDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Oscillogramms");
          if (!Directory.Exists(pathToOscDir))
              Directory.CreateDirectory(pathToOscDir);

          string pathToExtractZipOscDir = null;
          using (ZipFile zipFile = new ZipFile(pathToTempFile, System.Text.Encoding.GetEncoding("cp866")))
          {
              pathToExtractZipOscDir = Path.Combine(pathToOscDir, Path.GetFileNameWithoutExtension(oscDataAndName.Item2));

              if (Directory.Exists(pathToExtractZipOscDir))
                  Directory.Delete(pathToExtractZipOscDir, true);

              Directory.CreateDirectory(pathToExtractZipOscDir);

              zipFile.ExtractAll(pathToExtractZipOscDir);
          }
          #endregion

          if (cancellationToken.IsCancellationRequested)
              return false;

          #region Запуск OscView
          try
          {
              string pathToOscView = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OscView", "OscView.exe");

              Process oscViewProcess = new Process();
              oscViewProcess.StartInfo.FileName = pathToOscView;

              oscViewProcess.StartInfo.Arguments = String.Format("\"{0}\"", Directory.GetFiles(pathToExtractZipOscDir)[0]);

              oscViewProcess.Start();
          }
          catch (Exception ex)
          {
              TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 0, "Исключение при запуске OscView : " + ex.Message);
              return true;
          }
          #endregion

          return false;
      }
   }
}
