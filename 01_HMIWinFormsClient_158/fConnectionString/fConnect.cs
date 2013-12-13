using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Threading;

namespace fConnectionString
{
    public partial class fConnect : Form
    {
       #region Открытые свойства
       /// <summary>
       /// нужно сохранять логин и пароль для SQL-connection
       /// </summary>
       public bool IsNeedStoragePassword
       {
          get { return checkBox1.Checked; }
       }
       /// <summary>
       /// тип доступа к БД - Windows, SQL
       /// </summary>
       public string TypeConnectToBD 
       {
          get
          {
             if (radioButton1.Checked)
                return "Windows";
             if (radioButton2.Checked)
                return "SQL";
             return String.Empty;
          }
       }
       #endregion

        public fConnect()
        {
            InitializeComponent();
        }

        // Переменные
        private string Security = "Integrated Security=true";
        public string ConnectionString = string.Empty;

        private void Form1_Load( object sender, EventArgs e )
        {
            radioButton1.Checked = true;
            comboBox1.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
        }

        private void button3_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            GetLocalNetServers();
        }

       /// <summary>
       /// получить список доступных лок и сет серверов
       /// </summary>
        public void GetLocalNetServers() 
        {
           comboBox2.Enabled = false;
           comboBox2.Items.Clear();

           button1.Text = "Поиск...";
           button1.Enabled = false;

           #region Получение локального сервера
           RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
           String[] instances = (String[])rk.GetValue("InstalledInstances");
           if (instances.Length > 0)
           {
              foreach (String element in instances)
              {
                 if (element == "MSSQLSERVER")
                    comboBox2.Items.Add(System.Environment.MachineName);
                 else
                    comboBox2.Items.Add(element);
              }
           }
           #endregion

           #region Получение сетевого сервера
           DataTable servers = SqlDataSourceEnumerator.Instance.GetDataSources();
           foreach (DataRow row in servers.Rows)
              comboBox2.Items.Add(row.ItemArray[0]);
           #endregion

           if (comboBox2.Items.Count != 0)
           {
              comboBox2.Text = comboBox2.Items[0].ToString();

              try
              {
                 GetDataBase();

                 pictureBox3.Image = global::fConnectionString.Properties.Resources.imac1;
              }
              catch
              {
                 MessageBox.Show("Выбранный сервер не доступен.\nВыберите другой сервер.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
              }
           }

           button1.Text = "Получить список";
           comboBox2.Enabled = true;
           button1.Enabled = true;            
        }

        #region Получение списка баз данных
        private void GetDataBase()
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "";

            SqlConnection sqlConn = new SqlConnection( "Server=" + comboBox2.Text + ";Integrated Security=SSPI" );

            try
            {
                sqlConn.Open();

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = sqlConn;
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "sp_helpdb";

                SqlDataAdapter da = new SqlDataAdapter( sqlCmd );
                DataSet ds = new DataSet();
                da.Fill( ds );

                foreach ( DataRow row in ds.Tables[0].Rows )
                {
                    comboBox1.Items.Add( row["name"].ToString() );
                }
                sqlConn.Close();

                pictureBox1.Image = global::fConnectionString.Properties.Resources.DB1;

                if ( comboBox2.Text == "" )
                {
                    comboBox1.Enabled = false;
                    button2.Enabled = false;
                    button4.Enabled = false;
                }
                else
                {
                    comboBox1.Enabled = true;
                    button2.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show( "Выбранный сервер не доступен.\nВыберите другой сервер.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information );
                string test = comboBox2.Text;
            }
        }
        #endregion

        private void radioButton1_CheckedChanged( object sender, EventArgs e )
        {
            if ( radioButton1.Checked == true )
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                checkBox1.Enabled = false;
                label2.Enabled = false;
                label3.Enabled = false;
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void radioButton2_CheckedChanged( object sender, EventArgs e )
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            checkBox1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;
            checkBox1.Checked = true;
        }

        private void comboBox2_TextChanged( object sender, EventArgs e )
        {
            try
            {
                comboBox2.Text = comboBox2.Items[comboBox2.SelectedIndex].ToString();

                GetDataBase();
            }
            catch
            {
                GetDataBase();
            }
        }

        private void button4_Click( object sender, EventArgs e )
        {
            if ( radioButton2.Checked == true )
            {
                if ( checkBox1.Checked == true )
                {
                    Security = @"Persist Security Info=True;User ID=" + textBox1.Text + ";Password=" + textBox2.Text + "";
                }
                else
                {
                    Security = @"Persist Security Info=True;User ID=" + textBox1.Text + "";
                }
            }
            else if ( radioButton1.Checked == true )
            {
                Security = "Integrated Security=true";
            }

            SqlConnection sqlConn = new SqlConnection( @"Server=" + comboBox2.Text + ";Initial Catalog=" + comboBox1.Text + ";" + Security );

            try
            {
                sqlConn.Open();
                sqlConn.Close();

                ConnectionString = @"Data Source=" + comboBox2.Text + ";Initial Catalog=" + comboBox1.Text + ";" + Security;
                Close();
            }
            catch
            {
                MessageBox.Show( "Нет соединения с Базой Данных.\n\nSQL Server не доступен.\nВозможно не правильно указали Имя пользователя или пароль.", "Подключение к БД", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }

            
        }

        private void button2_Click( object sender, EventArgs e )
        {
            if ( comboBox1.Text == "" )
            {
                MessageBox.Show( "Вы не выбрали Базу Данных.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
            else
            {
                if ( radioButton2.Checked == true )
                {
                    if ( checkBox1.Checked == true )
                    {
                        Security = @"Persist Security Info=True;User ID=" + textBox1.Text + ";Password=" + textBox2.Text + "";
                    }
                    else
                    {
                        Security = @"Persist Security Info=True;User ID=" + textBox1.Text + "";
                    }
                }
                else if ( radioButton1.Checked == true )
                {
                    Security = "Integrated Security=true";
                }

                SqlConnection sqlConn = new SqlConnection( @"Server=" + comboBox2.Text + ";Initial Catalog=" + comboBox1.Text + ";" + Security );

                try
                {
                    sqlConn.Open();
                    pictureBox2.Image = global::fConnectionString.Properties.Resources.user_blue1;
                    MessageBox.Show( "Проверка подключения выполнена.", "Подключение к БД", MessageBoxButtons.OK, MessageBoxIcon.Information );
                    sqlConn.Close();
                    
                }
                catch
                {
                    MessageBox.Show( "Проверка подключения не выполнена.\n\nSQL Server не доступен.\nВозможно не правильно указали Имя пользователя или пароль.", "Подключение к БД", MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
            }
        }

        private void comboBox1_TextChanged( object sender, EventArgs e )
        {
            if ( comboBox1.Text == "" )
            {
                button4.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
            }
        }

        private void comboBox2_SelectionChangeCommitted( object sender, EventArgs e )
        {
            comboBox1.Focus();
        }

        private void comboBox2_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Enter )
            {
                GetDataBase();
            }
        }

        private void fConnect_Activated(object sender, EventArgs e)
        {
        }

        private void fConnect_Shown(object sender, EventArgs e)
        {
           //isActivated = true;
           //frmSplash fs = new frmSplash();
           ////Thread.Sleep(2000);
           //AddOwnedForm(fs);
           //fs.TopMost = true;
           //fs.Show();

           //GetLocalNetServers();

           //fs.Close();
        }
    }

}
