using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using HMI_MT_Settings;

namespace HMI_MT
{
   /// <summary>
   /// класс для работы со строкой подключения к SQL-серверу
   /// (шаблон одиночка - Singleton)
   /// </summary>
   public sealed class SQLConnectionString
   {
      XDocument xdoc;

      private SQLConnectionString() {}

      public static SQLConnectionString Iinstance { get { return InstanceHolder._instance; } }

      protected class InstanceHolder
      {
         static InstanceHolder() { }
         internal static readonly SQLConnectionString _instance = new SQLConnectionString();
      }

      /// <summary>
      ///  формируем строку подключения к базе в 
      ///  зависимости от типа подключения - Windows-идентификация или 
      ///  SQL-идентификация
      /// </summary>
      /// <param Name="xdoc"></param>
      /// <returns></returns>
      public string GetConnectStrFromPrjFile(XDocument xdoc)
      {
         this.xdoc = xdoc;

         string rez = String.Empty;
         switch (xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Attribute("TypeAuthentication").Value)
         {
            case "Windows":
               string Data_Source = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Windows").Element("Data_Source").Attribute("value").Value;
               string integrated_security = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Windows").Element("integrated_security").Attribute("value").Value;
               string uid = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Windows").Element("uid").Attribute("value").Value;
               string pwd = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Windows").Element("pwd").Attribute("value").Value;
               string Initial_Catalog = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Windows").Element("Initial_Catalog").Attribute("value").Value;
               string Anything_else = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Windows").Element("Anything_else").Attribute("value").Value;
               rez = "Data Source=" + Data_Source
                                       + ";Initial Catalog=" + Initial_Catalog
                                       + ";integrated security=" + integrated_security
                                       + ";uid=" + uid
                                       + ";pwd=" + pwd
                                       + "; " + Anything_else;
               // для контроля на консоль строка соединения
			   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 58, "(58) SQLConnectionString.cs : GetConnectStrFromPrjFile() : Windows Connection: " + rez);
               break;
            case "SQL":
               string SQLData_Source = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("SQL").Element("Data_Source").Attribute("value").Value;
               string SQLInitial_Catalog = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("SQL").Element("Initial_Catalog").Attribute("value").Value;
               string Persist_Security_Info = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("SQL").Element("Persist_Security_Info").Attribute("value").Value;
               string User_ID = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("SQL").Element("User_ID").Attribute("value").Value;
               string Password = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("SQL").Element("Password").Attribute("value").Value;
               string SQLAnything_else = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("SQL").Element("Anything_else").Attribute("value").Value;
               rez = "Data Source=" + SQLData_Source
                                       + ";Initial Catalog=" + SQLInitial_Catalog
                                       + ";Persist Security Info=" + Persist_Security_Info
                                       + ";User ID=" + User_ID
                                       + ";Password=" + Password
                                       + "; " + SQLAnything_else;
               // для контроля на консоль строка соединения
			   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 74, "(74) SQLConnectionString.cs : GetConnectStrFromPrjFile() : SQL Connection: " + rez);
               break;
            case "Any":
               string AnythingCStr = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Any").Attribute("value").Value;			   
               rez = AnythingCStr;
               // для контроля на консоль строка соединения
			   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 74, "(74) SQLConnectionString.cs : GetConnectStrFromPrjFile() : Any Connection: " + rez);
               break;
            default:
               string stre = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Attribute("TypeAuthentication").Value;
               MessageBox.Show(stre + " - некорректный тип соединения с БД.\nПроверьте тип подключения в файле Project.cfg.\nTypeAuthentication = \"Windows\" или \"SQL\" или \"Any\"");
               break;
         }
         return rez;
      }

      public string TestAndTryConnectionBD()
      {
         fConnectionString.fConnect fcnt = new fConnectionString.fConnect();

         fConnectionString.frmSplash fs = new fConnectionString.frmSplash();
         //AddOwnedForm(fs);
         fs.TopMost = true;
         fs.Show();
         Application.DoEvents();

         fcnt.GetLocalNetServers();

         fs.Close();

         fcnt.TopMost = true;
         fcnt.ShowDialog();

         if (String.IsNullOrEmpty(fcnt.ConnectionString))
         {
            return string.Empty;
         }

         if (DialogResult.Yes == MessageBox.Show("Запомнить настройки на источник данных?", "Проверка доступности базы данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
         {
            SaveNewConnectionParams(fcnt.TypeConnectToBD, fcnt.ConnectionString);
            xdoc.Save(HMI_Settings.PathToPrjFile);
         }

         return fcnt.ConnectionString;

         //TestAndTryConnectionBD();
      }

      /// <summary>
      /// сохранить настройки на БД в соответсвии со значением строки соедениеня от внешнего источника
      /// </summary>
      /// <param Name="typecnt"></param>
      /// <param Name="strconnect"></param>
      private void SaveNewConnectionParams(string typecnt, string strconnect)
      {
         if (xdoc == null)
            throw new Exception("SQLConnectionString.cs (128) : SaveNewConnectionParams() : ОШИБКА : xdoc = null");

         SortedDictionary<string, string> srtDict = new SortedDictionary<string, string>();

         ParseConnectStr(strconnect, ref srtDict);


         xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Attribute("TypeAuthentication").Value = typecnt;

         XElement xeCntSect = (from t in xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Descendants()
                               where t.Name == typecnt
                               select t).First();

         switch (typecnt)
         {
            case "Windows":
               for (int i = 0; i < srtDict.Count; i++)
               {
                  xeCntSect = (from t in xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Windows").Descendants()
                               where t.Attribute("sqlname").Value.ToLower() == srtDict.ElementAt(i).Key.ToLower()
                               select t).First();

                  xeCntSect.Attribute("value").Value = srtDict.ElementAt(i).Value;
               }
               break;
            case "SQL":
               break;
            default:
               MessageBox.Show("Неизвестный тип соединения : " + typecnt, "Проверка доступности базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
               break;
         }
         // подстроим udl

         for (int i = 0; i < srtDict.Count; i++)
         {
            try
            {
               xeCntSect = (from t in xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Ole_Udl").Descendants()
                            where t.Attribute("sqlname").Value.ToLower() == srtDict.ElementAt(i).Key.ToLower()
                            select t).First();
            }
            catch 
            {
               continue;
            }

            xeCntSect.Attribute("value").Value = srtDict.ElementAt(i).Value;
         }

      }

      /// <summary>
      /// Разобрать внешнюю строку соединения на пары ключ-значение
      /// </summary>
      /// <param Name="strcnt">исходная строка соединения от некоторого источника</param>
      /// <returns>список ключ-значение для формирования секции соединения с SQL-сервером в файле проекта</returns>
      private void ParseConnectStr(string strcnt, ref SortedDictionary<string, string> srtDict)
      {
         string[] strelems = strcnt.Split(new char[] { ';' });
         string[] cntatr;
         for (int i = 0; i < strelems.Count(); i++)
         {
            cntatr = strelems[i].Split(new char[]{'='});
            srtDict.Add(cntatr[0], cntatr[1]);
         }
      }
   }
}
