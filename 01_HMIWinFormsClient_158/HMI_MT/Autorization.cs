using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

using DebugStatisticLibrary;

using HMI_MT_Settings;

namespace HMI_MT
{
    public enum Target
    {
        None,
        EnterToSystem,
        ChangeUser,
        BlockSystem
    }
    public partial class frmAutorization : Form
    {
        MainForm parent;

        [DllImport( "user32.dll" )]
        public static extern void LockWorkStation();

        public frmAutorization()
        {
            InitializeComponent();
            this.Height = this.Height - this.ClientSize.Height + btnBlockPC.Top + btnBlockPC.Height;
        }
        public frmAutorization(MainForm f, Target target)
        {
            parent = f;

            InitializeComponent();

            switch( target )
            {
                case Target.EnterToSystem:
                    this.Text = "Вход в систему";
                    this.Height = this.Height - this.ClientSize.Height + btnBlockPC.Top + btnBlockPC.Height;
                    break;
                case Target.ChangeUser:
                    this.Text = "Смена пользователя";
                    this.Height = this.Height - this.ClientSize.Height + btnBlockPC.Top + btnBlockPC.Height;
                    break;
                case Target.BlockSystem:
                    this.Text = "Блокировка системы";
                    btnBlockPC.Visible = true;
                    break;
            }
        }

       public void DoEnterWithoutPassword( string userName, string userPassword)
        {
           int id = DoEffortAccess(userName, userPassword);

           if (!InitUser(id))
           {
              DialogResult = DialogResult.No; // неуспешная инициализация существующего пользователя
              tbPassword.Text = "";
              tbPassword.Focus();
           }

           GetRightForUser();

           DialogResult = DialogResult.OK;
        }

       /// <summary>
       /// регистрируем попытку доступа
       /// </summary>
       private int DoEffortAccess(string userName, string userPassword)
       {
			 CommonUtils.CommonUtils.WriteEventToLog(1, "попытка входа:" + "User " + userName + "; Pass = xxx", false);//, false, false);
          // получаем список доступных пользователей
          // получение строк соединения и поставщика данных из файла *.config
          string dp = ConfigurationManager.AppSettings["provider"];
          //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;

          // создание источника поставщика
          DbProviderFactory df = DbProviderFactories.GetFactory(dp);

          // создание объекта соединения
          DbConnection cn = df.CreateConnection();
		  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 92, "(92) Autorization.cs : DoEffortAccess() : Объект соединения : " + cn.GetType().FullName);
          cn.ConnectionString = HMI_Settings.ProviderPtkSql;
          try
          {
             cn.Open();
          }
          catch(Exception ex)
          {
			  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);

			  return -1;
          }

          // создание объекта команды
          DbCommand cmd = df.CreateCommand();
		  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 107, "(107) Autorization.cs : DoEffortAccess() : Объект команды: " + cmd.GetType().FullName);

		  cmd.Connection = cn;
          string st = "Select Id From Users WHERE UserName = '" + userName + "' AND UserPass = '" + userPassword + "' AND DeleteTime IS NULL";

          cmd.CommandText = st;
          DbDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
		  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 114, "(114) Autorization.cs : DoEffortAccess() : Объект чтения данных : " + dr.GetType().FullName);

          if (!dr.HasRows)
          {
             dr.Close();
             cn.Close();
             tbPassword.Text = "";
             tbPassword.Focus();
             return -1;
          }


          dr.Read();
          int id = (int)dr["Id"];

          dr.Close();
          cn.Close();
          return id;
       }
       
       /// <summary>
       /// Считывание информации по правам группы 
       /// </summary>
       private void GetRightForUser() 
       {
           DebugStatistics.WindowStatistics.AddStatistic( "Запрос логин\\пароля." );

          // успешная инициализация существующего пользователя
          // Считываем информацию по правам группы - получение строк соединения и поставщика данных из файла *.config
          //cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
          SqlConnection asqlconnect = new SqlConnection(HMI_Settings.ProviderPtkSql);//cnStr
          asqlconnect.Open();

          // формирование данных для вызова хранимой процедуры
          SqlCommand cmdSql = new SqlCommand("UserRight~Show", asqlconnect);
          cmdSql.CommandType = CommandType.StoredProcedure;

          // входные параметры - отсутствуют

          // заполнение DataSet
          DataSet aDS = new DataSet("ptk");
          SqlDataAdapter aSDA = new SqlDataAdapter();
          aSDA.SelectCommand = cmdSql;

          //aSDA.sq
          aSDA.Fill(aDS);
          asqlconnect.Close();

          // извлекаем данные по пользователю и передаем их в MainForm
          DataTable dt = aDS.Tables[0];
          // заполняем StringDictionary информацией о всех доступных правах
          for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
          {
             string BitNum = ((int)dt.Rows[curRow]["BitNumber"]).ToString();
             if (dt.Rows[curRow]["RightName"] == System.DBNull.Value)
                continue;
             HMI_MT_Settings.HMI_Settings.sdUserRightsFull[BitNum] = (string)dt.Rows[curRow]["RightName"];
          }
          // на основании parent.UserRight извлекаем из всех доступных прав права текущего пользователя
          for (int i = 0; i < HMI_MT_Settings.HMI_Settings.sdUserRightsFull.Count; i++)
          {
             if (HMI_MT_Settings.HMI_Settings.UserRight[i].ToString() == "1")
             {
                 HMI_MT_Settings.HMI_Settings.sdUserRights[i.ToString()] = HMI_MT_Settings.HMI_Settings.sdUserRightsFull[i.ToString()];
             }
          }

          DebugStatistics.WindowStatistics.AddStatistic( "Запрос логин\\пароля завершен." );
       }

       private void button1_Click( object sender, EventArgs e )
       {
           int id = DoEffortAccess( tbUser.Text, tbPassword.Text );

           if ( !InitUser( id ) )
           {
               // событие начала работы без БД
               CommonUtils.CommonUtils.WriteEventToLog( 41, "Не верный ввод логина или пароля", true );
               MessageBox.Show( this, "Не верный ввод логина или пароля", "Авторизация", MessageBoxButtons.OK,
                                MessageBoxIcon.Error );
               //DialogResult = DialogResult.No; // неуспешная инициализация существующего пользователя
               tbUser.Text = "";
               tbPassword.Text = "";
               tbPassword.Focus();
           }
           else
           {
               GetRightForUser();

               DialogResult = DialogResult.OK;
           }
       }
        /// <summary>
        /// private bool InitUser( int id) 
        /// инициализация пользователя
        /// </summary>
        private bool InitUser( int id) 
        {
            // получаем информацию о пользователе
            // получение строк соединения и поставщика данных из файла *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql ); //cnStr
            asqlconnect.Open();

            // формирование данных для вызова хранимой процедуры
            SqlCommand cmd = new SqlCommand( "User~Login", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // входные параметры
            // 1. Id пользователя
            SqlParameter pidUser = new SqlParameter();
            pidUser.ParameterName = "@id";
            pidUser.SqlDbType = SqlDbType.Int;
            pidUser.Value = id;
            pidUser.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pidUser );

            // заполнение DataSet
            DataSet aDS = new DataSet( "ptk" );
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            //aSDA.sq
            aSDA.Fill( aDS );
            asqlconnect.Close();

            // извлекаем данные по пользователю и передаем их в MainForm
            DataTable dt = aDS.Tables[0];

            HMI_MT_Settings.HMI_Settings.UserID = id; // id пользователя
            
            if (id == -1)
               return false;

            HMI_MT_Settings.HMI_Settings.UserName = (string)dt.Rows[0]["UserName"];
            HMI_MT_Settings.HMI_Settings.UserComment = (string)dt.Rows[0]["Comment"];
            HMI_MT_Settings.HMI_Settings.IDRights = (int)dt.Rows[0]["ID"]; // идентификатор группы прав
            HMI_MT_Settings.HMI_Settings.GroupName = (string)dt.Rows[0]["GroupName"];  // название группы прав
            HMI_MT_Settings.HMI_Settings.GroupComment = (string)dt.Rows[0]["Expr1"];   // комментарий группы прав
            HMI_MT_Settings.HMI_Settings.UserRight = (string)dt.Rows[0]["GroupRight"]; // права пользователя
            HMI_MT_Settings.HMI_Settings.UserMenu = (string)dt.Rows[0]["HiddenMenu"]; // права пользователя по доступу к меню
            return true;
        }

        private void button2_Click( object sender, EventArgs e )
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnBlockPC_Click( object sender, EventArgs e )
        {
			  tbUser.Text = "";
			  tbPassword.Text = "";
            LockWorkStation();  //блокировка рабочей станции
            //DialogResult = DialogResult.Yes;
        }

        private void frmAutorization_Load( object sender, EventArgs e )
        {
            tbUser.Focus();
        }

        private void tbUser_Leave( object sender, EventArgs e )
        {
            tbUser.Text = tbUser.Text.ToLower();
            tbPassword.Focus();
        }

        private void tbPassword_KeyDown( object sender, KeyEventArgs e )
        {
            if( e.KeyCode == Keys.Return )
                btnGotoSystem.Focus();
        }

		private void frmAutorization_InputLanguageChanged(object sender, InputLanguageChangedEventArgs e)
		{
			keyboardLayout1.LoadUC();
		}
    }
}