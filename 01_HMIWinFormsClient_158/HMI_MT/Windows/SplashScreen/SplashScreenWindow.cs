using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Configuration;
using HMI_MT_Settings;
using MessagePanel;
using PTKStateLib;
using TraceSourceLib;

namespace HMI_MT.Windows.SplashScreen
{
    public partial class SplashScreenWindow : Form
    {
        public SplashScreenWindow()
        {
            InitializeComponent();
        }

        #region Override metods
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            LoadConfiguration();

            Close();
        }
        #endregion Override metods

        #region Private Metods
        private void ShowProgressMessage(string message)
        {
            label1.Text = message;
            Application.DoEvents();
            //System.Threading.Thread.Sleep(400);
        }

        private void LoadConfiguration()
        {
            ShowProgressMessage("Проверка конфигурации...");
            CheckConfiguration();

            ShowProgressMessage("Иницилизация логгера...");
            InitTraceRouteLib();

            ShowProgressMessage("Загрузка настроек...");
            GetAppSettings();

            ShowProgressMessage("Загрузка конфигурации...");
            InitConfiguration();

            ShowProgressMessage("Иницилизация настроек нормального режима...");
            NormalModeLibrary.ComponentFactory.Factory.LoadXml();

            ShowProgressMessage("Загрузка привязки состояний...");
            PTKState.Iinstance.InitPTKStateInfo();

            ShowProgressMessage("Проверка подключения к БД...");
            InitDB();

            ShowProgressMessage("Подсоединение к серверу данных...");
            HMI_Settings.MessageProvider = new MessageProvider(HMI_Settings.IPADDRES_SERVER);
        }

        /// <summary>
        /// Сформировать имена конфигурац файлов проекта
        /// </summary>
        private void CheckConfiguration()
        {
            try
            {
                #region Project Dir

                string pathToProjectDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Project");
                if (!Directory.Exists(pathToProjectDir))
                {
                    MessageBox.Show("Не найдена папка с конфигурацией.", "Ошибка", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    throw new Exception();
                }

                #endregion

                #region Project.cfg

                // проверяем существование файла конфигурации  проекта Project.cfg и файла конфигурации устройств проекта
                HMI_Settings.PathToPrjFile = Path.Combine(pathToProjectDir, "Project.cfg");

                if (!File.Exists(HMI_Settings.PathToPrjFile))
                {
                    MessageBox.Show("Не найден файл Project.cfg", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new Exception();
                }

                #endregion

                #region Configuration.cfg

                // проверяем существование файла конфигурации устройств проекта Configuration.cfg 
                HMI_Settings.PathToConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Project", "Configuration.cfg");

                if (!File.Exists(HMI_Settings.PathToConfigurationFile))
                {
                    MessageBox.Show("Не найден файл Configuration.cfg", "Ошибка", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    throw new Exception();
                }

                #endregion

                #region PanelState.xml

                // проверяем существование файла с адаптерами для описания состояния ПТК
                HMI_Settings.PathPanelState_xml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Project", "PanelState.xml");
                if (!File.Exists(HMI_Settings.PathPanelState_xml))
                {
                    MessageBox.Show("Не найден файл PanleState.cfg", "Ошибка", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    throw new Exception();
                }

                #endregion

                #region Load Project.cfg & configuration.cfg & PanelState.xml XML

                try
                {
                    HMI_Settings.XDoc4PathToPrjFile = XDocument.Load(HMI_Settings.PathToPrjFile);
                    HMI_Settings.XDoc4PathToConfigurationFile = XDocument.Load(HMI_Settings.PathToConfigurationFile);
                    HMI_Settings.XDoc4PathPanelState_xml = XDocument.Load(HMI_Settings.PathPanelState_xml);
                }
                catch
                {
                    throw new Exception();
                }

                #endregion

                // смотрим существование папки для осциллограмм и диаграмм
                CreateFolderForOscAndDiagram();
            }
            catch (Exception)
            {
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Иницилизация логгера
        /// </summary>
        private void InitTraceRouteLib()
        {
            if (HMI_Settings.XDoc4PathToPrjFile.Element("Project").Elements("TextListener").Count() > 0)
            {
                string namef = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("TextListener").Attribute("fileName").Value;
                int sizef = int.Parse(HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("TextListener").Attribute("fsize_kbait").Value);

                CommonUtils.LogMonitoring lm;

                if (File.Exists(namef))
                    lm = new CommonUtils.LogMonitoring(string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, namef), sizef);
            }

            TraceSourceDiagMes.CreateLog("HMI_MT_Client_TraceSource");
            TraceSourceDiagMes.tracesource.TraceData(TraceEventType.Verbose, 1, "StartTest");
            TraceSourceDiagMes.tracesource.Flush();

            TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 37, DateTime.Now.ToString() + " : (37) Program.cs : Main() : Запуск приложения");
            TraceSourceDiagMes.FlushLog();
        }

        /// <summary>
        /// инициализировать статический класс с настройками для приложения
        /// </summary>
        private void GetAppSettings()
        {
            try
            {
                // ip-адрес сервера для перезапуска
                var xe_tcp = (from t in HMI_Settings.XDoc4PathToConfigurationFile.Element("Project").Element("Configuration").Element("Object").Elements("DSAccessInfo")
                              where t.Attribute("enable").Value.ToLower() == "true"
                              select t).Single();

                HMI_Settings.IPADDRES_SERVER = xe_tcp.Element("CustomiseDriverInfo").Element("IPAddress").Attribute("value").Value;

                HMI_Settings.isRegPass = Convert.ToBoolean(HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IsReqPassword").Value);
                HMI_Settings.isNeedLoginAndPassword = bool.Parse(HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("IsNeedLoginAndPassword").Attribute("value").Value);
                HMI_Settings.pathLogEvent_pnl4 = Properties.Settings.Default.PathToLogFile;
                HMI_Settings.sizeLog_pnl4 = Properties.Settings.Default.LogFileMaxSize;
                HMI_Settings.whatToDoLog_pnl4 = Properties.Settings.Default.WhatToDoLogFileMaxSize;
                HMI_Settings.IsShowToolTip = Properties.Settings.Default.IsToolTipShow;
                HMI_Settings.IsToolTipRefDesign = Properties.Settings.Default.IsToolTipRefDesign;
                HMI_Settings.IsShowTabForms = Properties.Settings.Default.IsShowTabForms;
                HMI_Settings.Precision = Properties.Settings.Default.Precision;
                HMI_Settings.LogOnlyDisk = Properties.Settings.Default.LogOnlyDisk;
                HMI_Settings.IPPointForSerializeMesPan = Properties.Settings.Default.IPPointForSerializeMesPan;
                HMI_Settings.PortPointForSerializeMesPan = Properties.Settings.Default.PortPointForSerializeMesPan;
                HMI_Settings.ViewBtn4MainWindow = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("ViewBtn4MainWindow").Value;
                HMI_Settings.DiagnosticSchema = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("DiagnosticSchema").Value;
                HMI_Settings.MainMnenoSchema = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("MainMnenoSchema").Value;
                HMI_Settings.alMenu = new ArrayList();

                // Debug Mode
                var DebugModeXElement = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("DebugMode");
                if (DebugModeXElement != null)
                    HMI_Settings.IsDebugMode = Boolean.Parse(DebugModeXElement.Attribute("enable").Value);

                // AURA
                InitAura(HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("Aura"));

                var res = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("SchemaTransform").Attribute("x").Value;
                HMI_Settings.SchemaSize.X = float.Parse(res.Replace('.', ','));
                res = HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("SchemaTransform").Attribute("y").Value;
                HMI_Settings.SchemaSize.Y = float.Parse(res.Replace('.', ','));
            }
            catch (Exception ex)
            {
                TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// инициализировать конфигурацию
        /// </summary>
        private void InitConfiguration()
        {
            try
            {
                // инициализируем компонент конфигурации
                ConfigurationFactory cf = new ConfigurationFactory();
                HMI_Settings.CONFIGURATION = cf.CreateConfiguration("OnlyMOACfg", HMI_Settings.PathToConfigurationFile);


                if (HMI_Settings.CONFIGURATION == null)
                    throw new Exception(
                        @"(502) : X:\Projects\40_Tumen_GPP09\Client\HMI_MT\MainForm.cs : InitConfiguration() : некорректная конфигурация");

                /*
                 * для нового варианта ПТК NewDS<->OldHMIClient
                 */
                // формируем список доступных типов блоков архивной информации                
                IEnumerable<XElement> xetypebloks = HMI_Settings.XDoc4PathToConfigurationFile.Element("Project").Element("TypeBlockData").Elements("Type");

                foreach (XElement xetypeblok in xetypebloks)
                    HMI_Settings.CONFIGURATION.SetTypeBlockArchivData(xetypeblok.Attribute("name").Value,
                                                                      xetypeblok.Attribute("value").Value);
            }
            catch (Exception ex)
            {
                TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                throw ex;
            }
        }

        /// <summary>
        /// создать отдельную папку для хранения осциллограмм и диаграмм
        /// </summary>
        private void CreateFolderForOscAndDiagram()
        {
            try
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Oscillogramms");
            }
            catch (Exception ex)
            {
                TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        private void InitAura(XElement auraXElement)
        {
            // AURA
            if (auraXElement != null)
            {
                try
                {
                    XAttribute auraEnableAttribute = auraXElement.Attribute("enable");
                    if (auraEnableAttribute != null)
                        if (Boolean.Parse(auraEnableAttribute.Value))
                            HMI_Settings.AuraUrl = auraXElement.Attribute("url").Value;
                        else HMI_Settings.AuraUrl = null;
                    else
                        HMI_Settings.AuraUrl = auraXElement.Attribute("url").Value;
                }
                catch (Exception)
                {
                    Console.WriteLine("Ошибка в секции настройки АУРы в файле Project.cfg");

                    HMI_Settings.AuraUrl = null;
                }
            }
            else
                HMI_Settings.AuraUrl = null;
        }

        private void InitDB()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SQLConnectionString scs = SQLConnectionString.Iinstance;

            /* 
            * формируем строку подключения к базе в 
            * зависимости от типа подключения - Windows-идентификация или 
            * SQL-идентификация
            */
            HMI_Settings.ProviderPtkSql = scs.GetConnectStrFromPrjFile(HMI_Settings.XDoc4PathToPrjFile);

            ConnectionStringsSection csSection = config.ConnectionStrings;

            csSection.ConnectionStrings["SqlProviderPTK"].ConnectionString = HMI_Settings.ProviderPtkSql;

            // записываем изменения настроек на базу данных
            config.Save(ConfigurationSaveMode.Modified);

            CheckBlockNameTable();
        }

        private void CheckBlockNameTable()
        {
            string sqlCommandString = 
                @"SELECT *
                FROM BlockName
                WHERE Id=1000";

            try
            {
                SqlConnection sqlConnection = new SqlConnection(HMI_Settings.ProviderPtkSql);
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(sqlCommandString, sqlConnection);

                using (SqlDataReader dr = sqlCommand.ExecuteReader())
                {
                    if (!dr.HasRows)
                        MessageBox.Show(
                            "В таблице BlockName базы данных отсутствует запись с Id = 1000. Это приведет к тому, что пользовательские события не будут отображаться в АРМ'е.",
                            "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
            }

        }
        #endregion Private Metods
    }
}
