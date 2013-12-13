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
                    this.Text = "���� � �������";
                    this.Height = this.Height - this.ClientSize.Height + btnBlockPC.Top + btnBlockPC.Height;
                    break;
                case Target.ChangeUser:
                    this.Text = "����� ������������";
                    this.Height = this.Height - this.ClientSize.Height + btnBlockPC.Top + btnBlockPC.Height;
                    break;
                case Target.BlockSystem:
                    this.Text = "���������� �������";
                    btnBlockPC.Visible = true;
                    break;
            }
        }

       public void DoEnterWithoutPassword( string userName, string userPassword)
        {
           int id = DoEffortAccess(userName, userPassword);

           if (!InitUser(id))
           {
              DialogResult = DialogResult.No; // ���������� ������������� ������������� ������������
              tbPassword.Text = "";
              tbPassword.Focus();
           }

           GetRightForUser();

           DialogResult = DialogResult.OK;
        }

       /// <summary>
       /// ������������ ������� �������
       /// </summary>
       private int DoEffortAccess(string userName, string userPassword)
       {
			 CommonUtils.CommonUtils.WriteEventToLog(1, "������� �����:" + "User " + userName + "; Pass = xxx", false);//, false, false);
          // �������� ������ ��������� �������������
          // ��������� ����� ���������� � ���������� ������ �� ����� *.config
          string dp = ConfigurationManager.AppSettings["provider"];
          //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;

          // �������� ��������� ����������
          DbProviderFactory df = DbProviderFactories.GetFactory(dp);

          // �������� ������� ����������
          DbConnection cn = df.CreateConnection();
		  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 92, "(92) Autorization.cs : DoEffortAccess() : ������ ���������� : " + cn.GetType().FullName);
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

          // �������� ������� �������
          DbCommand cmd = df.CreateCommand();
		  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 107, "(107) Autorization.cs : DoEffortAccess() : ������ �������: " + cmd.GetType().FullName);

		  cmd.Connection = cn;
          string st = "Select Id From Users WHERE UserName = '" + userName + "' AND UserPass = '" + userPassword + "' AND DeleteTime IS NULL";

          cmd.CommandText = st;
          DbDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
		  TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Information, 114, "(114) Autorization.cs : DoEffortAccess() : ������ ������ ������ : " + dr.GetType().FullName);

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
       /// ���������� ���������� �� ������ ������ 
       /// </summary>
       private void GetRightForUser() 
       {
           DebugStatistics.WindowStatistics.AddStatistic( "������ �����\\������." );

          // �������� ������������� ������������� ������������
          // ��������� ���������� �� ������ ������ - ��������� ����� ���������� � ���������� ������ �� ����� *.config
          //cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
          SqlConnection asqlconnect = new SqlConnection(HMI_Settings.ProviderPtkSql);//cnStr
          asqlconnect.Open();

          // ������������ ������ ��� ������ �������� ���������
          SqlCommand cmdSql = new SqlCommand("UserRight~Show", asqlconnect);
          cmdSql.CommandType = CommandType.StoredProcedure;

          // ������� ��������� - �����������

          // ���������� DataSet
          DataSet aDS = new DataSet("ptk");
          SqlDataAdapter aSDA = new SqlDataAdapter();
          aSDA.SelectCommand = cmdSql;

          //aSDA.sq
          aSDA.Fill(aDS);
          asqlconnect.Close();

          // ��������� ������ �� ������������ � �������� �� � MainForm
          DataTable dt = aDS.Tables[0];
          // ��������� StringDictionary ����������� � ���� ��������� ������
          for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
          {
             string BitNum = ((int)dt.Rows[curRow]["BitNumber"]).ToString();
             if (dt.Rows[curRow]["RightName"] == System.DBNull.Value)
                continue;
             HMI_MT_Settings.HMI_Settings.sdUserRightsFull[BitNum] = (string)dt.Rows[curRow]["RightName"];
          }
          // �� ��������� parent.UserRight ��������� �� ���� ��������� ���� ����� �������� ������������
          for (int i = 0; i < HMI_MT_Settings.HMI_Settings.sdUserRightsFull.Count; i++)
          {
             if (HMI_MT_Settings.HMI_Settings.UserRight[i].ToString() == "1")
             {
                 HMI_MT_Settings.HMI_Settings.sdUserRights[i.ToString()] = HMI_MT_Settings.HMI_Settings.sdUserRightsFull[i.ToString()];
             }
          }

          DebugStatistics.WindowStatistics.AddStatistic( "������ �����\\������ ��������." );
       }

       private void button1_Click( object sender, EventArgs e )
       {
           int id = DoEffortAccess( tbUser.Text, tbPassword.Text );

           if ( !InitUser( id ) )
           {
               // ������� ������ ������ ��� ��
               CommonUtils.CommonUtils.WriteEventToLog( 41, "�� ������ ���� ������ ��� ������", true );
               MessageBox.Show( this, "�� ������ ���� ������ ��� ������", "�����������", MessageBoxButtons.OK,
                                MessageBoxIcon.Error );
               //DialogResult = DialogResult.No; // ���������� ������������� ������������� ������������
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
        /// ������������� ������������
        /// </summary>
        private bool InitUser( int id) 
        {
            // �������� ���������� � ������������
            // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql ); //cnStr
            asqlconnect.Open();

            // ������������ ������ ��� ������ �������� ���������
            SqlCommand cmd = new SqlCommand( "User~Login", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // ������� ���������
            // 1. Id ������������
            SqlParameter pidUser = new SqlParameter();
            pidUser.ParameterName = "@id";
            pidUser.SqlDbType = SqlDbType.Int;
            pidUser.Value = id;
            pidUser.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pidUser );

            // ���������� DataSet
            DataSet aDS = new DataSet( "ptk" );
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            //aSDA.sq
            aSDA.Fill( aDS );
            asqlconnect.Close();

            // ��������� ������ �� ������������ � �������� �� � MainForm
            DataTable dt = aDS.Tables[0];

            HMI_MT_Settings.HMI_Settings.UserID = id; // id ������������
            
            if (id == -1)
               return false;

            HMI_MT_Settings.HMI_Settings.UserName = (string)dt.Rows[0]["UserName"];
            HMI_MT_Settings.HMI_Settings.UserComment = (string)dt.Rows[0]["Comment"];
            HMI_MT_Settings.HMI_Settings.IDRights = (int)dt.Rows[0]["ID"]; // ������������� ������ ����
            HMI_MT_Settings.HMI_Settings.GroupName = (string)dt.Rows[0]["GroupName"];  // �������� ������ ����
            HMI_MT_Settings.HMI_Settings.GroupComment = (string)dt.Rows[0]["Expr1"];   // ����������� ������ ����
            HMI_MT_Settings.HMI_Settings.UserRight = (string)dt.Rows[0]["GroupRight"]; // ����� ������������
            HMI_MT_Settings.HMI_Settings.UserMenu = (string)dt.Rows[0]["HiddenMenu"]; // ����� ������������ �� ������� � ����
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
            LockWorkStation();  //���������� ������� �������
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