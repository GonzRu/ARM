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
               //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 58, "(58) SQLConnectionString.cs : GetConnectStrFromPrjFile() : Windows Connection: " + rez);
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
               //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 74, "(74) SQLConnectionString.cs : GetConnectStrFromPrjFile() : SQL Connection: " + rez);
               break;
            case "Any":
               string AnythingCStr = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Element("Any").Attribute("value").Value;			   
               rez = AnythingCStr;
               // для контроля на консоль строка соединения
               //TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 74, "(74) SQLConnectionString.cs : GetConnectStrFromPrjFile() : Any Connection: " + rez);
               break;
            default:
               string stre = xdoc.Element("Project").Element("ConnectionString").Element("TypeConnection").Attribute("TypeAuthentication").Value;
               MessageBox.Show(stre + " - некорректный тип соединения с БД.\nПроверьте тип подключения в файле Project.cfg.\nTypeAuthentication = \"Windows\" или \"SQL\" или \"Any\"");
               break;
         }
         return rez;
      }
   }
}
