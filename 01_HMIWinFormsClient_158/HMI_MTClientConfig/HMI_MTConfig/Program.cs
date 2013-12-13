/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Настройка конфигурации клиента ПТК Защита
 *                                                                             
 *	Файл                     : X:\Projects\99_MTRAhmi\Client\HMI_MTClientConfig\HMI_MTConfig\Program.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 14.03.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Threading;

namespace HMI_MTConfig
{
   static class Program
   {





		#region Свойства
		#endregion

		#region public
	   /// <summary>
	   /// путь к файлу с доп. параметрами настройки 
	   /// DataServer
	   /// </summary>
	   public static string Path2HMI_MTConfig_xml = string.Empty;

	   /// <summary>
	   /// представление xml-файла для HMI_MTConfig.xml
	   /// </summary>
	   public static XDocument xdoc_HMI_MTConfig_xml;

		/// <summary>
		/// путь к файлу \Project\Project.cfg
		/// </summary>
		public static string Path2Project_cfg = string.Empty;

		/// <summary>
		/// представление xml-файла для \Project\Project.cfg
		/// </summary>
		public static XDocument xdoc_Project_cfg;

		/// <summary>
		/// путь к файлу \Project\PrgDevCFG.cdp
		/// </summary>
		public static string Path2PrgDevCFG_cdp = string.Empty;

		/// <summary>
		/// представление xml-файла для \Project\PrgDevCFG.cdp
		/// </summary>
		public static XDocument xdoc_PrgDevCFG_cdp;

		/// <summary>
		/// представление xml-файла для \TCPClient\TCPClientCFG.xml
		/// </summary>
		public static XDocument xdoctcpclient;

		/// <summary>
		/// путь к файлу \TCPClient\TCPClientCFG.xml
		/// </summary>
		public static string Path2TCPClientCFG_xml = string.Empty;
		#endregion

		#region private
		/// <summary>
		/// StringBuilder для использования вместо String
		/// там где это возможно
		/// </summary>
		static StringBuilder sb = new StringBuilder();
		/// <summary>
		/// Названия обязательных xml-секций
		/// </summary>
		private static ArrayList alRequiredXMLSections = new ArrayList();
		/// <summary>
		/// Названия xml-секций которые нужно удалить
		/// </summary>
		private static ArrayList alLegacyXMLSections = new ArrayList();
		#endregion

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);

          // зададим глобальные обработчики ошибок
          Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
         AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

         // проверим где мы находимся

		 try 
		 {


			 TestAppDir();
			 InitName4XMLSections();
			 Tes4CFGContentCorrection();
		 }catch(Exception ex)
		 {
			 MessageBox.Show(ex.Message, "Ошибка");
			 return;
		 }


         Application.Run(new Form1());
      }


	  private static void InitName4XMLSections()
	  {
		  IEnumerable<XElement> xes = xdoc_HMI_MTConfig_xml.Element("Config").Element("LegacyXMLSections").Elements("LegacyXMLSection");

		  foreach (XElement xe in xes)
			  alLegacyXMLSections.Add(xe.Attribute("name").Value);

		  xes = xdoc_HMI_MTConfig_xml.Element("Config").Element("RequiredXMLSections").Elements("RequiredXMLSection");

		  foreach (XElement xe in xes)
			  alRequiredXMLSections.Add(xe.Attribute("name").Value);
	  }

	   /// <summary>
	   /// проверить корректность содержимого 
	   /// конфигурационных файлов
	   /// </summary>
	  private static void Tes4CFGContentCorrection()
	  {
		  Tes4CFGContentCorrection_Project_Cfg();
	  }

	   /// <summary>
	   /// проверить корректность содержимого 
	   /// конфигурационного файла проекта Project.сfg
	   /// </summary>
	  private static void Tes4CFGContentCorrection_Project_Cfg()
	  {
		  #region проверка необходимых секций в файле Project.сfg
		  foreach (string str in alRequiredXMLSections)
		  {
			  IEnumerable<XElement> xexmlsecs = xdoc_Project_cfg.Descendants(str);
			  if (xexmlsecs.Count() == 0)
				  throw new Exception("В файле Project.сfg отсутсвует секция " + str);
			  else if (xexmlsecs.Count() > 1)
				  throw new Exception("Секция " + str + "встречается в файле Project.сfg " + xexmlsecs.Count().ToString() + " раз.");

			  IEnumerable<XAttribute> xats = xexmlsecs.First().Attributes();
			  bool isUICFound = false;

			  foreach (XAttribute xa in xats)
				  if (xa.Name.ToString() == "UIComment")
				  {
					  isUICFound = true;
					  break;
				  }

			  if (!isUICFound)
				  throw new Exception("В секции " + str + " отсутсвует атрибут UIComment.");

			  if ((xexmlsecs.First().FirstAttribute).Name != "UIComment")
				  throw new Exception("В секции " + str + "атрибут UIComment не является первым.");
		  }
		  #region определяем кто мы - клиент или сервер, для предупреждения ошибок при загрузке
		  string strdbg = xdoc_Project_cfg.Element("Project").Element("ARMRole").Attribute("role").Value;
		  if (strdbg != "IsTCPClient")
			  throw new Exception(@"В файле Project.сfg некорректно определена секция для определения роли приложения. Должно быть <ARMRole UIComment="""" role=""IsTCPClient"">");
		  #endregion
		  #endregion

		  #region проверка устаревших секций в файле Project.сfg для последующего удаления
		  foreach (string str in alLegacyXMLSections)
		  {
			  IEnumerable<XElement> xexmlsecs = xdoc_Project_cfg.Descendants(str);
			  if (xexmlsecs.Count() != 0)
				  throw new Exception("В файле Project.сfg присутствует устаревшая секция " + str);
		  }
		  #endregion
	  }

      static private void TestAppDir()
      {
		  sb.Clear();
		  sb.Append(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "HMI_MTConfig.xml");
		  if (File.Exists(sb.ToString()))
		  {
			  Path2HMI_MTConfig_xml = sb.ToString();
			  xdoc_HMI_MTConfig_xml = new XDocument();
			  xdoc_HMI_MTConfig_xml = XDocument.Load(sb.ToString());
		  }
		  else
			  throw new Exception(@"Текущая папка не является корректной папкой запуска конфигурируемого приложения.
									В папке отсутствует файл HMI_MTConfig.xml с доп параметрами приложения HMI_MTConfig.exe");

		  // загружаем конф файл TCPClient клиента
          if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "TCPClient"))
          {
              Path2TCPClientCFG_xml = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "TCPClient" + Path.DirectorySeparatorChar + "TCPClientCFG.xml";
              xdoctcpclient = XDocument.Load(Path2TCPClientCFG_xml);
          }
		  else
			  throw new Exception(@"Текущая папка не является корректной папкой запуска конфигурируемого приложения.
									В папке отсутсвует папка TCPClient и файл TCPClient\TCPClient.cdp.
									Поместите конфигуратор в папку запуска клиента ПТК Эгида");

         /*
           * далее смотрим есть ли в текущей папке папка Project, 
           * если есть, то открываем ее и смотрим есть ли в ней файл
           * Project.cfg. В противном случае выводим сообщение и закрываемся
           */
         sb.Clear();
         sb.Append(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project");
         if (Directory.Exists(sb.ToString()))
         {
            sb.Clear();
            sb.Append(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project"
               + Path.DirectorySeparatorChar + "Project.cfg");

            if (File.Exists(sb.ToString()))
            {
               Path2Project_cfg = sb.ToString();
               xdoc_Project_cfg = new XDocument();
               xdoc_Project_cfg = XDocument.Load(sb.ToString());

               sb.Clear();
               sb.Append(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project"
                   + Path.DirectorySeparatorChar + "PrgDevCFG.cdp");
               if (File.Exists(sb.ToString()))
               {
                  Path2PrgDevCFG_cdp = sb.ToString();
                  xdoc_PrgDevCFG_cdp = new XDocument();
                  xdoc_PrgDevCFG_cdp = XDocument.Load(sb.ToString());

                  return;
               }
			   else
				   throw new Exception(@"Текущая папка не является корректной папкой запуска конфигурируемого приложения.
									В папке отсутсвует файл Project\PrgDevCFG.cdp.
									Поместите конфигуратор в папку запуска клиента ПТК Эгида");
			}
			else
				throw new Exception(@"Текущая папка не является корректной папкой запуска конфигурируемого приложения.
									В папке отсутсвует файл Project\Project.cfg.
									Поместите конфигуратор в папку запуска клиента ПТК Эгида");
         }


		  else
			 throw new Exception(@"Текущая папка не является корректной папкой запуска конфигурируемого приложения.
									В папке отсутсвует папка Project.
									Поместите конфигуратор в папку запуска клиента ПТК Эгида");
      }

      static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
      {
          // подготовка содержательного сообщения
          string msg = String.Format("(Program.cs) В приложении обнаружена ошибка: \n\n {0}", e.GetType().ToString());
          // желает ли пользователь завершить приложение
          DialogResult rezult = MessageBox.Show(msg, "Ошибка", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
          if (rezult == DialogResult.Abort)
              Application.Exit();
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
      {
          try
          {
              // подготовка содержательного сообщения
              string msg = String.Format("(Program.cs) В приложении обнаружена ошибка: \n\n {0} \n\n {1}", e.Exception.Message, e.Exception.StackTrace);

              // желает ли пользователь завершить приложение
              switch (MessageBox.Show(msg , "Ошибка", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error).ToString())
              {
                  case "Abort":
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
                  MessageBox.Show("(Program.cs) Приложение будет закрыто по ошибке", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
              finally
              {
                  Application.Exit();
              }
          }
      }

   }
}
