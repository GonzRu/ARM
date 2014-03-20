using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using InterfaceLibrary;

namespace OscillogramsLib
{
    //public delegate void OSCReadyHandler();

   public class OscDiagViewer
   {
      #region private
      string namecmdproc;
      SqlConnection asqlconnect;
      DataSet aDS;
      SqlDataAdapter aSDA;
      /// <summary>
      /// массив идентиф осциллограмм, 
      /// выделенных для объединения
      /// </summary>
       ArrayList ArrIDE;
       //bool isReqOSCRead;    // флаг необходимости чтения осциллограммы

       UInt32 idReqOsc = 0;        // идентификатор записи с осциллограммой в БД
       UInt32 DsNumber = 0;
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
      /// id источника
      /// </summary>
      public int IdFC
      {
          get { return idFC; }
          set { idFC = value; }
      }
      int idFC;
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

      ///// <summary>
      ///// число выделенных осц или диагр
      ///// </summary>
      //public short CntSelectOSC 
      //{
      //   set { cntSelectOSC = value; }
      //}
      //short cntSelectOSC = 1;
      #endregion
      #region Осциллограммы
      /// <summary>
      /// список осциллограмм для чтения 
      /// (требуется для случая объединения осциллограмм, 
      /// когда их читается несколько)
      /// </summary>
      public SortedList<int, string> slOsc4Read = new SortedList<int, string>();
      public bool CancelOSCRead
      {
          get { return cancelOSCRead; }
          set
          {
              cancelOSCRead = value;
              if (value)
                  stopwatch.Stop();
          }
      }
      bool cancelOSCRead;

      /// <summary>
      /// флаг показывающий состояние получения осциллограммы
      /// </summary>
      public bool IsOSCinProcessing;
      // массив содержимого осциллограммы
      public byte[] arrOsc;
      // событие готовности осциллограммы
      //public event OSCReadyHandler OSCReadyEvent;
      /// <summary>
      /// класс для замера времени
      /// </summary>
      Stopwatch stopwatch;
      #endregion

      #region конструктор, уничтожение объекта
      public OscDiagViewer()
      {
         ArrIDE = new ArrayList();

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

         stopwatch = new Stopwatch();
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
         pipFC.Value = idFC;
         pipFC.Direction = ParameterDirection.Input;
         cmd.Parameters.Add(pipFC);

         // 2. id устройства
         SqlParameter pidBlock = new SqlParameter();
         pidBlock.ParameterName = "@id";
         pidBlock.SqlDbType = SqlDbType.Int;
         pidBlock.Value = idDev + idFC * 256;
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

      /// <summary>
      /// сформировать имя файла и вызвать OscView
      /// </summary>
      /// <param Name="?"></param>
      /// <param Name="?"></param>
      public void ShowOSCDg(UInt32 ds, DataTable dt, int ide)
      {
         byte[] arrO = null;
         string ifa = String.Empty;
         short cntSelectOSC = 1;
         if (dt == null)
            return;
         
         DsNumber = ds;

         // по ide найти запись в dt, извлечь блок с осциллограммой (диаграммой), записать в файл, запустить fastview
         // перечисляем записи в dt
         int curRow;

         for (curRow = 0; curRow < dt.Rows.Count; curRow++)
         {
            if (ide == ((int)dt.Rows[curRow]["ID"]))
            {
               arrO = GetArrBlockData(dt, curRow);
               ifa = GetFName(dt.Rows[curRow]);
               break;
            }
            else
               continue;
         }

         if (String.IsNullOrEmpty(ifa))
            return;

         AddOSC2List(ide, ifa);
         StartProceccReadOSC(cntSelectOSC);
      }

      /// <summary>
      /// установить требование для чтения осциллограммы с сервера
      /// </summary>
      /// <param Name="idrec"></param>
      void SetReqForOSCRead(UInt32 idrec)
      {
          stopwatch.Reset();
          stopwatch.Start();
          //isReqOSCRead = true;
          idReqOsc = idrec;
          cancelOSCRead = false;

          // запрос к серверу на чтение осциллограммы
          IOscillogramma osc = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetOscData(DsNumber, idrec);

          osc.OnOscReady += new OSCReadyHandler(osc_OnOscReady);

          // запуск диалогового окна с изображением процесса получения осциллограммы
          using (dlgOscReceiveProcess dlgORP = new dlgOscReceiveProcess(osc))  //this
          {
              dlgORP.ShowDialog();
          }
      }

      void osc_OnOscReady( IOscillogramma osc)
      {
          ClientDataForExchange_OSCReadyEvent(osc.ContentBlockOsc);
      }

      MemoryStream ms_arrosc4copy;

      public void SetArrOSC(byte[] arrosc)
      {
          arrOsc = new byte[arrosc.Length];
          Buffer.BlockCopy(arrosc, 0, arrOsc, 0, arrosc.Length);

          // чтобы развязать связку диалогового окна и этого класса создадим таймер, 
          // по срабатыванию которого вызовется событие окончания чтения осциллограммы
          tmrOscevent = new System.Timers.Timer();
          tmrOscevent.Elapsed += new System.Timers.ElapsedEventHandler(tmrOscevent_Elapsed);
          tmrOscevent.Interval = 500;
          ms_arrosc4copy = new MemoryStream(arrosc);
          tmrOscevent.Start();
          //ClientDataForExchange_OSCReadyEvent(arrosc);
      }

      System.Timers.Timer tmrOscevent;

      void tmrOscevent_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
      {
          tmrOscevent.Stop();

          //OnOSCReadyEvent();

          ClientDataForExchange_OSCReadyEvent(ms_arrosc4copy.ToArray());
      }

      //public void OnOSCReadyEvent()
      //{
      //    if (OSCReadyEvent != null)
      //        OSCReadyEvent();
      //}

      /// <summary>
      /// 
      /// </summary>
      /// <param Name="ide">идентификатор записи с блоком данных в БД</param>
      /// <param Name="ifa">имя файла для записи осц.</param>
      private void AddOSC2List(int ide, string ifa)
      {
          if (!slOsc4Read.ContainsKey(ide))
              slOsc4Read.Add(ide, ifa);
      }

      short cntSelectOSC;
      /// <summary>
      /// старт процесса чтения осциллограммы
      /// </summary>
      public void StartProceccReadOSC(short cntselectOSC)
      {
          /*   работаем только с первой записью, 
               если список пуст, то это говорит о том, что все осциллограммы считаны
           */
          cntSelectOSC = cntselectOSC;
          SetReqForOSCRead(Convert.ToUInt32(slOsc4Read.ElementAt(0).Key));
      }

      private byte[] GetArrBlockData(DataTable dt, int curRow)
      {
         //if (HMI_Settings.IsLocalSystem)
         //   return (byte[])dt.Rows[curRow]["Data"];
         //else
            return null;
      }


      private string GetFName(DataRow dr)
      {
         string ifa = (string)dr["BlockName"] + "_#_" + Convert.ToString(dr["BlockID"]) + "_#Tblock_" + Convert.ToString(dr["TimeBlock"]) + "_#TFC_" + Convert.ToString(dr["TimeFC"]);

         // удаляем пробелы
         string[] sp = ifa.Split(new char[]{' '});
         StringBuilder sb = new StringBuilder();
         for (int i = 0; i < sp.Length; i++)
         {
            sb.Append(sp[i]);
         }
         string sbb = sb.ToString();
         sb.Length = 0;
         for (int i = 0; i < sbb.Length; i++)
         {
            if (sbb[i] == '.' || sbb[i] == '/' || sbb[i] == '\\' || sbb[i] == '/' || sbb[i] == ':')
               sb.Append('_');
            else
               sb.Append(sbb[i]);
         }

         ifa = sb.ToString();
         AddExtended(dr, ref ifa);

         return ifa;
      }

      /// <summary>
      /// добавить расширение файла в 
      /// соответствии с типом блока 
      /// и типом данных (осциллограмма или диаграмма)
      /// </summary>
      /// <param Name="dt"></param>
      /// <param Name="?"></param>
      private void AddExtended(DataRow dr, ref string ifa)
      {
         int it = (int)dr["TypeID"];
         switch(it)
         {
            case 4:  // Осциллограмма БМРЗ
               ifa += ".zosc";
               break;
            case 5:  // Диаграмма БМРЗ
               ifa += ".dgm";
               break;
            case 8:  // Осциллограмма Сириус
               ifa += ".trd";
               break;
            case 10:  // Осциллограмма Экра
               ifa += ".dfr";
               break;
            default:
               System.Windows.Forms.MessageBox.Show("Осциллограмма/диаграмма типа " + it.ToString() + " не поддерживается.", "Просмотр осциллограмм/диаграмм", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
               break;
         }
      }
      ArrayList asb = new ArrayList();

      void ClientDataForExchange_OSCReadyEvent(byte[] arrosc)
      {
          StringBuilder sb = new StringBuilder();

            try
			{
                /*
                * осциллограмма может состоять из нескольких кусков,
                * поэтому полученный пакет имеет формат исходя 
                * из которого можно эти куски вычленить:
                * 1 байт - число кусков;
                * 2 байта - длина очередного куска
                * n байт - содержимое очередного куска
                */
                BinaryReader br = new BinaryReader(new MemoryStream(arrosc));

                // выясним сколько фрагментов в осциллограмме
                byte cntfragments = 1;//br.ReadByte();

                asb.Add(slOsc4Read.ElementAt(0).Value);  // добавили название файла с осциллограммой

                // выяснили раздельно имя файла и его расширение
                string filename = AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + Path.GetFileNameWithoutExtension(slOsc4Read.ElementAt(0).Value);
                string fileextens = Path.GetExtension(slOsc4Read.ElementAt(0).Value);

                CreateOscFile(filename, fileextens, br);

                // удаляем верхнюю запись в списке считываемых осциллограмм
                slOsc4Read.RemoveAt(0);

                // если есть другие фрагменты осциллограммы то формируем файлы из них
                for (int i = 1; i < cntfragments; i++)
                    CreateOscFile(filename + "_" + i.ToString() /*номер фрагмента*/, fileextens, br);

                if (slOsc4Read.Count != 0)
                {
                    StartProceccReadOSC(cntSelectOSC); // читаем дальше осциллограммы
                    return;
                }

                // все осциллограммы прочитаны - запускаем fastview OscView.exe
                stopwatch.Stop();    // остановили подсчет времени

                // запускаем OscView.exe
                string oscviewexePath = AppDomain.CurrentDomain.BaseDirectory + "\\OscView\\OscView.exe";
                if (!File.Exists(oscviewexePath))
                {
                    MessageBox.Show("Отсутствует файл запуска OscView. Путь : " + oscviewexePath, "(725) ClientDataForExchange.cs : ClientDataForExchange_OSCReadyEvent", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Process prc = new Process();

                switch (cntSelectOSC)
                {
                    case 0:
                        return;
                    case 1:
                        prc.StartInfo.FileName = oscviewexePath;
                        prc.StartInfo.Arguments = "\"" + AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + asb[0].ToString() + "\"";
                        break;
                    default: // 2 и более осциллограммы
                        sb = new StringBuilder();
                        foreach (string s in asb)
                        {
                            sb.Append("\"" + s.ToString() + "\"");
                            sb.Append(" ");
                        }
                        prc.StartInfo.FileName = oscviewexePath;
                        prc.StartInfo.Arguments = "-o " + sb.ToString();
                        break;
                }
                prc.Start();
                asb.Clear();
                slOsc4Read.Clear();
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }

      private void CreateOscFile(string p, string e, BinaryReader br)
      {
          FileStream f;
          int lenfragment = 0;
			try
			{
                try
                {
                    //lenfragment = br.ReadInt32();
                    lenfragment = (int)br.BaseStream.Length;
                    f = File.Create(p + e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    f = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + "последняя_осциллограмма.osc");//ifa
                }

                f.Write(br.ReadBytes((int)lenfragment), 0, (int)lenfragment);
                f.Close();
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }

      /// <summary>
      /// возвращает строку с временем в сек
      /// прошедшим с начала формирования осциллограмм(ы)
      /// </summary>
      /// <returns></returns>
      public string GetStrTimeReadOSC()
      {
          return Convert.ToString(stopwatch.ElapsedTicks / Stopwatch.Frequency);
      }

      private void RunOscView(string ifa,byte[] arrO)
      {
         #region запись в файл, запуск OscView
         FileStream f = null;
         try
         {
            f = File.Create(AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + ifa);
         }
         catch (Exception ex)
         {
            System.Windows.Forms.MessageBox.Show(ex.ToString() + "\nФайл : " + AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + ifa, "Просмотр осциллограмм/диаграмм", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            return;
         }

         f.Write(arrO, 0, arrO.Length);
         f.Close();
         // запускаем fastview

         Process prc = new Process();
         prc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\OscView\\OscView.exe  ";
         prc.StartInfo.Arguments = "\"" + AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + ifa + "\"";
         prc.Start();
         #endregion
      }

   #region Объединение осциллограмм/диаграмм
      public void ClearArrayIDE()
      {
         ArrIDE.Clear(); 
      }
	   /// <summary>
      /// добавление идентификатора в массив идентификаторов
      /// осциллограмм для объединения
      /// </summary>
      public void AddIde2ArrayIde(int ide) 
      {
         ArrIDE.Add(ide);
      }

      public void ShowUnionOSCDg(DataTable drc)
      {
         ArrayList asb = new ArrayList();    // для хранения имен файлов
         string ifa;

         // перечисляем записи в таблице dbO, смотрим отмеченные, формируем файлы, вызываем fastview
         short cntSelectOSC = 0;
         foreach (DataRow dr in drc.Rows)
         {
            if (!ArrIDE.Contains(dr["Id"]))
               continue;

            // формируем имя файла - сохраняем имя в массиве
            ifa = AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms\\" + GetFName(dr);

            asb.Add(ifa);

            //HMI_Settings.ClientDFE.AddOSC2List((int)dr["Id"], ifa); 
               cntSelectOSC++;
         }

         //HMI_Settings.ClientDFE.StartProceccReadOSC(cntSelectOSC); // старт процесса чтения осциллограмм
      }

      private void RunOscDiagViewUnion(ArrayList listf)
      {
         // запускаем OscView
         Process prc = new Process();
         StringBuilder sb = new StringBuilder();

         foreach (string s in listf)
         {
            // каждыый путь д.б. заключен в свои отдельные кавычки
            sb.Append("\"" +s + "\"");
            sb.Append(" ");
         }
         prc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\OscView\\OscView.exe  ";
         
         prc.StartInfo.Arguments = "-o " + sb.ToString();

         prc.Start();
      }
	#endregion
   }
}
