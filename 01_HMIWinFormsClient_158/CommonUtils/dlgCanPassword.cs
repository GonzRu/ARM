using System;
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
using HMI_MT_Settings;

namespace CommonUtils
{
	public partial class dlgCanPassword : Form
	{
		private string userName;
		private int userID;

		public dlgCanPassword( string UserName, int UserID)
		{
			InitializeComponent();

			userName = UserName;
			userID = UserID;
		}

		private void btnCheckRight_Click( object sender, EventArgs e )
		{
			// проверяем пользователя
			// получение строк соединения и поставщика данных из файла *.config
            string dp = "System.Data.SqlClient";//ConfigurationManager.AppSettings["provider"];
			//string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;

			// создание источника поставщика
			DbProviderFactory df = DbProviderFactories.GetFactory( dp );

			// создание объекта соединения
			DbConnection cn = df.CreateConnection();
			Console.WriteLine( "Объект соединения: {0}", cn.GetType().FullName );
            cn.ConnectionString = HMI_Settings.ProviderPtkSql ;  //cnStr;
			try
			{
				cn.Open();
			} catch
			{
				Console.WriteLine( "Нет связи с БД" );
				return;
			}

			// создание объекта команды
			DbCommand cmd = df.CreateCommand();
			Console.WriteLine( "Объект команды: {0}", cmd.GetType().FullName );
			cmd.Connection = cn;
			string st = "Select Id From Users WHERE UserName = '" + tbNameCurrentUser.Text + "' AND UserPass = '" + tbPasswordCurrentUser.Text + "' AND DeleteTime IS NULL";

			cmd.CommandText = st;
			DbDataReader dr = cmd.ExecuteReader( CommandBehavior.CloseConnection );
			Console.WriteLine( "Объект чтения данных: {0}", dr.GetType().FullName );
			if( !dr.HasRows )
			{
				dr.Close();
				cn.Close();
				tbPasswordCurrentUser.Text = "";
				tbPasswordCurrentUser.Focus();
				return;
			}


			dr.Read();
			int id = ( int ) dr["Id"];

			dr.Close();
			cn.Close();

			if( userID == id )
				DialogResult = DialogResult.OK; // успешная проверка существующего пользователя
			else
				DialogResult = DialogResult.Abort; // неуспешная проверка существующего пользователя




			this.Close();
		}

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }
	}
}