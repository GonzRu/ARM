using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.IO;
using CommonUtils;
using HMI_MT_Settings;

namespace HMI_MT
{
    public partial class frmUserGroupRights : Form
    {
        string[] strGroups; // ������ �������������
        DataTable dtR, dtG, dtU, dtRI, dtG_One;
        bool isAddedUser = false;  // ������� �������� ���������� ������������
        int kolRights = 0;  //���������� ������������ ����
        int currentGroup = 0;   // ������� ������
        bool isAddGroup = false;    // ����� ���������� ������
		 int currentCell = 0;	// ������� ������
		 ArrayList arrMenuAccessDen = new ArrayList();

        // ����� ������ ������������ ��� ����������
        string newUserPassword;
        string newUserGroup;
        int idUserCurrent;
		 MainForm parent;

        public frmUserGroupRights(MainForm mf )
        {
            InitializeComponent();
				parent = mf;
        }

        private void frmUserGroupRights_Load( object sender, EventArgs e )
        {
            Init_Load();    // ������������� ���������� � ������������� � ��������
            ShowRights( 0 );   // ������� ������ ���� ��� ������ ������            

            cbUserGroup.Items.AddRange( strGroups );

            gbEditUserParameters.Enabled = false;
            btnDelUser.Enabled = false;
			btnGroupSaveChange.Enabled = false;

            dgvGroups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRights.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        
        private void Init_Load()
        {
            dgvUsers.Rows.Clear();
            dgvGroups.Rows.Clear();
            //��������� ������ �� �������������
            // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            asqlconnect.Open();

            // ������������ ������ ��� ������ �������� ���������
            SqlCommand cmd = new SqlCommand( "User~Show", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // ������� ���������
            // 1. @id ������������ ��� 0
            SqlParameter pid = new SqlParameter();
            pid.ParameterName = "@id";
            pid.SqlDbType = SqlDbType.Int;
            pid.Value = 0;  // ��� ������������
            pid.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pid );

            // ���������� DataSet
            DataSet aDS = new DataSet( "ptk" );
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            aSDA.Fill( aDS, "User" );

            //asqlconnect.Close();

            // ��������� �������� �������������
            dtU = aDS.Tables["User"];


            //��������� ������ �� �������
            // ������������ ������ ��� ������ �������� ���������
            cmd = new SqlCommand( "UserGroup~Show", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // ������� ���������
            // 1. @id ������ ��� 0
            SqlParameter pidg = new SqlParameter();
            pidg.ParameterName = "@Id";
            pidg.SqlDbType = SqlDbType.Int;
            pidg.Value = 0;  // ��� ������������
            pidg.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pidg );

            // ���������� DataSet
            aSDA.SelectCommand = cmd;

            aSDA.Fill( aDS, "Group" );

            asqlconnect.Close();

            dtG = aDS.Tables["Group"];

            //��������� ������ �� ������
            // ������������ ������ ��� ������ �������� ���������
            cmd = new SqlCommand( "UserRight~Show", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // ������� ��������� - �����������

            // ���������� DataSet
            aSDA.SelectCommand = cmd;

            aSDA.Fill( aDS, "Rights" );

            asqlconnect.Close();

            dtR = aDS.Tables["Rights"];

            //dgvUsers.DataSource = aDS;
            //dgvUsers.DataMember = "User";

            // ��������� datagrid �� ������� � ��������������
            //�������� ������������ �����
            ArrayList Groups = new ArrayList();
            
            for( int curRow = 0; curRow < dtG.Rows.Count; curRow++ )
                Groups.Add( dtG.Rows[curRow]["GroupName"] );

            strGroups = new string[Groups.Count];

            Groups.CopyTo( 0, strGroups, 0, Groups.Count );
            // ��������� ���������� ������ �����
            cbUserGroup.Items.Clear();
            cbUserGroup.Items.AddRange( strGroups );

            for( int curRow = 0; curRow < dtU.Rows.Count; curRow++ )
            {
                int i = dgvUsers.Rows.Add();   // ����� ������
                dgvUsers["clmUsersName", i].Value = dtU.Rows[curRow]["UserName"];
                dgvUsers["clmUserPass", i].Value = ( string ) dtU.Rows[curRow]["UserPass"];
                dgvUsers["clmComment", i].Value = dtU.Rows[curRow]["Comment"];
                int j = ( int ) dtU.Rows[curRow]["UserGroup"];
                // ------------------------------------------
                // ����������� ������ � ���� ��������
                //��������� ������ �� �������
                // ������������ ������ ��� ������ �������� ���������
                cmd = new SqlCommand( "UserGroup~Show", asqlconnect );
                cmd.CommandType = CommandType.StoredProcedure;

                // ������� ���������
                // 1. @id ������ ��� 0
                SqlParameter pidgg = new SqlParameter();
                pidgg.ParameterName = "@Id";
                pidgg.SqlDbType = SqlDbType.Int;
                pidgg.Value = j;  
                pidgg.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pidgg );

                // ���������� DataSet
                aSDA.SelectCommand = cmd;

                aSDA.Fill( aDS, "GroupOne" );

                asqlconnect.Close();

                dtG_One = aDS.Tables["GroupOne"];
                // ------------------------------------------
                dgvUsers["clmUserGroup", i].Value = dtG_One.Rows[0]["GroupName"];    // strGroups[j - 1];
                dgvUsers["clmDateGentration", i].Value = dtU.Rows[curRow]["CreateTime"];
                DataGridViewButtonCell ce = ( DataGridViewButtonCell )dgvUsers["clmChangeUser", i];
                ce.Value = "��������";
                dgvUsers["clmIdUser", i].Value = dtU.Rows[curRow]["Id"];
                aDS.Tables.Clear();
            }

            // ��������� datagrid �� ������� � ��������
            for( int curRow = 0; curRow < dtG.Rows.Count; curRow++ )
            {
                int i = dgvGroups.Rows.Add();   // ����� ������

                dgvGroups["clmIdGroup", i].Value = dtG.Rows[curRow]["ID"];
                dgvGroups["clmGroupName", i].Value = dtG.Rows[curRow]["GroupName"];
                dgvGroups["clmGroupComment", i].Value = ( string ) dtG.Rows[curRow]["Comment"];
                dgvGroups["clmGroupCreateData", i].Value = dtG.Rows[curRow]["CreateTime"];
                dgvGroups["clmGroupCreateData", i].Value = dtG.Rows[curRow]["CreateTime"];
                dgvGroups["clmGroupEditData", i].Value = dtG.Rows[curRow]["EditTime"];
                dgvGroups["clmGroupRight", i].Value = dtG.Rows[curRow]["GroupRight"];
					 dgvGroups["clmGroupMenuRight", i].Value = dtG.Rows[curRow]["HiddenMenu"];
            }

            // ��������� datagrid �� ������� � �������
            dgvRights.Rows.Clear();
            string[] stR = { "False", "True" };
            kolRights = 0;
            for( int curRow = 0; curRow < dtR.Rows.Count; curRow++ )
            {
					if( dtR.Rows[curRow]["RightName"] == System.DBNull.Value )
                    continue;
                int i = dgvRights.Rows.Add();   // ����� ������
                dgvRights["clmIdRightRecord", i].Value = dtR.Rows[curRow]["ID"];
                dgvRights["clmRightsName", i].Value = dtR.Rows[curRow]["RightName"];
					 dgvRights.Columns["clmRightsName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvRights["clmVedenieLoga", i].Value = ( bool ) dtR.Rows[curRow]["RightLog"];
                dgvRights["clmOutInLog", i].Value = ( bool ) dtR.Rows[curRow]["RightShow"];  
                kolRights++;
            }
    }
      /// <summary>
		/// private void ShowRights( int row)
      /// </summary>
      /// <param Name="row"></param> 
		private void ShowRights( int row)
      {
			string currights;
         CheckBox chB; 

         if( isAddGroup )
         {
				flpGroupRights.Controls.Clear();
            // ��������� ����������� �� �����
            for( int i = 0; i < kolRights; i++ )
            {
					chB = new CheckBox();
               chB.Parent = this.flpGroupRights;
               chB.AutoSize = true;
               chB.Checked = false;
			   chB.CheckedChanged += new EventHandler(chB_CheckedChanged);
               chB.Text = ( string ) dtR.Rows[i]["RightName"];
					if( chB.Text == "������" )
						chB.Visible = false;
				}
				dgvGroups["clmGroupMenuRight", row].Value = "";
            return;
         }

         flpGroupRights.Controls.Clear();
         // �������� ������ � ������� �� ������� ������ � dgvGroups
         if (String.IsNullOrEmpty((string)dgvGroups["clmGroupRight", row].Value))//) ).ToString()
         {
            MessageBox.Show("�� �� ����� ��� ������!", "��������������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }

			currights = ( CommonUtils.CommonUtils.ReversStr( ( string ) dgvGroups["clmGroupRight", row].Value ) ).ToString();

         if (currights == null)
            return;

         // ��������� ����������� �� �����
         for( int i = 0; i < kolRights; i++ )
         {
				switch( (char)currights[i])
            {
					case '0':
						chB = new CheckBox();
						chB.CheckedChanged += new EventHandler(chB_CheckedChanged);
						chB.Parent = this.flpGroupRights;
						chB.AutoSize = true;
						chB.Checked = true;	// �������� ���������
						chB.Text = (string) dtR.Rows[i]["RightName"];
						if( chB.Text == "������" )
							chB.Visible = false;
						break;
					case '1':
						chB = new CheckBox();
						chB.CheckedChanged += new EventHandler(chB_CheckedChanged);
						chB.Parent = this.flpGroupRights;
						chB.AutoSize = true;
						chB.Checked = false;	// �������� ���������
						chB.Text = ( string ) dtR.Rows[i]["RightName"];
						if( chB.Text == "������" )
							chB.Visible = false;
                  break;
					default:
						break;
				}
			}
	}

		void chB_CheckedChanged(object sender, EventArgs e)
		{
			btnGroupSaveChange.Enabled = true;
		}

	/// <summary>
	/// private void ShowGroupMenu( int row)
	/// </summary>
	/// <param Name="row"></param>
	private void ShowGroupMenu( int row )
	{
		string curMenuRights;
		// �������� ������ � ������� �� ����������� ���� �� ������� ������ � dgvGroups
		if( dgvGroups["clmGroupMenuRight", row].Value == DBNull.Value )
		{
			MessageBox.Show("������ " + dgvGroups["clmGroupName", row].Value + " �� ����� ���������� � ����" ) ;
			return;
		}

		curMenuRights = ( string ) dgvGroups["clmGroupMenuRight", row].Value;

      //if (String.IsNullOrEmpty(curMenuRights))
      //   return;

		// ������ ��� ���� ����� �������� � TreeView
		char[] delim = { ';'};
		string[] miAD = curMenuRights.Split(delim);
		// ��������� ������
		uint itmp;
		string stmp;

		arrMenuAccessDen.Clear();
		for(int i = 0; i < miAD.Length; i++)
		{
			stmp = miAD[i];
			if( stmp == "" )
				continue;
			char ctmp = stmp[0];
			if( !Char.IsDigit( ctmp ) )
				continue;
			itmp = Convert.ToUInt32( stmp );
			arrMenuAccessDen.Add( itmp );
		}

		// ���������� ������ � ������ ����
		DisplayTreeView( arrMenuAccessDen );
	}
   /// <summary>
   /// 
   /// </summary>
   /// <param Name="sender"></param>
   /// <param Name="e"></param>
	private void dgvUsers_CellContentClick( object sender, DataGridViewCellEventArgs e )
   {
            if( e.ColumnIndex != 4 ) 
                return;

            isAddedUser = false;    // ����� ������������� ������������� ������������

            tbComment.Text = "";
            lbInNewPass.Enabled = lbInOldPass.Enabled = lbNewPassConf.Enabled = false;
            tbInNewPass.Enabled = tbInNewPassConfirm.Enabled = tbInOldPass.Enabled = false;
            gbEditUserParameters.Enabled = true;

            tbUserName.Text = (string)( ( DataGridViewTextBoxCell ) dgvUsers["clmUsersName", e.RowIndex] ).Value;
            tbComment.Text = ( string ) ( ( DataGridViewTextBoxCell ) dgvUsers["clmComment", e.RowIndex] ).Value;
            idUserCurrent = ( int ) ( ( DataGridViewTextBoxCell ) dgvUsers["clmIdUser", e.RowIndex] ).Value;
            newUserPassword = ( string ) ( ( DataGridViewTextBoxCell ) dgvUsers["clmUserPass", e.RowIndex] ).Value;
            // ���������� ������ ������ ����������
            for( int i = 0; i < cbUserGroup.Items.Count; i++ )
            { 
                string sd = cbUserGroup.Items[i].ToString();
                if( sd == ( ( string ) ( dgvUsers["clmUserGroup", e.RowIndex] ).Value ) )
                {
                    cbUserGroup.SelectedItem = cbUserGroup.Items[i];
                    newUserGroup = cbUserGroup.Items[i].ToString();
                    break;
                }
            }
            tbInOldPass.Text = "";
            tbInNewPass.Text = "";
            tbInNewPassConfirm.Text = "";
            newUserGroup = "";

            btnDelUser.Enabled = true;
        }

        private void btnChUserPass_Click( object sender, EventArgs e )
        {
            lbInOldPass.Enabled = true;
            tbInOldPass.Enabled = true;
            tbInOldPass.Focus();
            // ���������� � ������� �� ����
        }

        private void tbInOldPass_KeyDown( object sender, KeyEventArgs e )
        {
            if( e.KeyValue != ( int ) Keys.Enter )
                return;
            // ���������� ������ - ������������ � ���������
            for( int curRow = 0; curRow < dtU.Rows.Count; curRow++ )
            {
                string un = (string)dtU.Rows[curRow]["UserName"];
                if( tbUserName.Text != un )
                    continue;
                string s = (string)dtU.Rows[curRow]["UserPass"];
                if( tbInOldPass.Text !=  s)
                    return;
                else 
                {
                    lbInNewPass.Enabled = lbNewPassConf.Enabled = true;
                    tbInNewPass.Enabled = tbInNewPassConfirm.Enabled = true;
                    tbInNewPass.Focus();
                    newUserPassword = s;
                    btnSaveChange.Enabled = false;
                }
            }
        }

        private void tbInNewPassConfirm_KeyDown( object sender, KeyEventArgs e )
        {
            if( e.KeyValue != ( int ) Keys.Enter )
                return;
            if( tbInNewPass.Text != tbInNewPassConfirm.Text )
            {
                MessageBox.Show("������ �� �������");
                tbInNewPass.Text = "";
                tbInNewPass.Focus();
                tbInNewPassConfirm.Text = "";
                return;
            }
            // ������ �������, ����� ���� �� ���������� ���������
            newUserPassword = tbInNewPassConfirm.Text;
            btnSaveChange.Enabled = true;

            if ( cbUserGroup.Items.Count > 0 && cbUserGroup.SelectedIndex == -1 )
                cbUserGroup.SelectedIndex = 0;
        }
       
        private void cbUserGroup_TextChanged( object sender, EventArgs e )
        {
            newUserGroup = cbUserGroup.Text;
        }
        
       /// <summary>
       /// ���������� ���������� �� ������ ��� ��������� ���������� ������������
       /// </summary>
       /// <param Name="sender"></param>
       /// <param Name="e"></param>
        private void btnSaveChange_Click( object sender, EventArgs e )
        {
            btnAddUser.Enabled = true;

			  if( DialogResult.No == MessageBox.Show( "��������� ���������?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) )
			  {
				  return;
			  }

           #region �������� ��������� ������
           if (String.IsNullOrEmpty(tbInNewPassConfirm.Text))
           {
              MessageBox.Show("�� �� ����� ������������� ������ ������������!", "��������������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              btnSaveChange.Enabled = false;
              return;
           }

           if (String.IsNullOrEmpty(tbUserName.Text))
           {
              MessageBox.Show("�� �� ����� ��� ������������!", "��������������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              btnSaveChange.Enabled = false; 
              return;
           }

           if (String.IsNullOrEmpty(cbUserGroup.Text))
           {
              MessageBox.Show("�� �� ������� ������ ��� ������������!", "��������������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              btnSaveChange.Enabled = false;
              return;
           }
           else
              newUserGroup = cbUserGroup.Text;
           #endregion

           // ������ ������ � ����������
           pnlHelp.Visible = false;

			  // ������ datagrid
            dgvUsers.Rows.Clear();
            if( isAddedUser )    // ���� ����� ����������, �� �������� �������� ��������� � �������
            {
                AddedUser();
                return;
            }
            // ���������� ������ �� ������������
            // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            asqlconnect.Open();

            // ������������ ������ ��� ������ �������� ���������
            SqlCommand cmd = new SqlCommand( "User~Edit", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // ������� ���������
            // 1. @Id
            SqlParameter pid = new SqlParameter();
            pid.ParameterName = "@id";
            pid.SqlDbType = SqlDbType.Int;
            pid.Value = idUserCurrent;
            pid.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pid );
            // 2. @Username
            SqlParameter pun = new SqlParameter();
            pun.ParameterName = "@Username";
            pun.SqlDbType = SqlDbType.Text;
            pun.Value = tbUserName.Text;
            pun.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pun );
            // 3. @UserPass
            SqlParameter pup = new SqlParameter();
            pup.ParameterName = "@UserPass";
            pup.SqlDbType = SqlDbType.Text;
            pup.Value = newUserPassword;
            pup.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pup );
            // 4. @Comment
            SqlParameter puc = new SqlParameter();
            puc.ParameterName = "@Comment";
            puc.SqlDbType = SqlDbType.Text;
            puc.Value = tbComment.Text;
            puc.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( puc );
            // 4. @UserGroup
            // �������� ��� �� ������
            int idg = 0;
            for( int curRow = 0; curRow < dtG.Rows.Count; curRow++ ) 
            {
                string st = ( string ) cbUserGroup.SelectedItem;

                if( st == ( ( string ) dtG.Rows[curRow]["GroupName"] ) )
                {
                    idg = ( int ) dtG.Rows[curRow]["ID"];
                    break;
                }
            }

            SqlParameter pug = new SqlParameter();
            pug.ParameterName = "@UserGroup";
            pug.SqlDbType = SqlDbType.Int;
            pug.Value = idg;
            pug.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pug );

            // ���������� DataSet
            DataSet aDS = new DataSet( "ptk" );
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            aSDA.Fill( aDS, "ReturnInfo" );

            asqlconnect.Close();

            dtRI = aDS.Tables["ReturnInfo"];
            
            gbEditUserParameters.Enabled = false;

            // ���������� datagrid
            Init_Load();
        }

       /// <summary>
       /// �������� ������������
       /// </summary>
       /// <param Name="sender"></param>
       /// <param Name="e"></param>
        private void btnDelUser_Click( object sender, EventArgs e )
        {
			  if( DialogResult.No == MessageBox.Show( "������� ������������?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) )
			  {
				  return;
			  }            
			  // ������ datagrid
            dgvUsers.Rows.Clear();
            // ���������� ������ �� ������������
            // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            asqlconnect.Open();

            // ������������ ������ ��� ������ �������� ���������
            SqlCommand cmd = new SqlCommand( "User~Erase", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // ������� ���������
            // 1. @Id
            SqlParameter pid = new SqlParameter();
            pid.ParameterName = "@id";
            pid.SqlDbType = SqlDbType.Int;
            pid.Value = idUserCurrent;
            pid.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pid );

            // ���������� DataSet
            DataSet aDS = new DataSet( "ptk" );
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            aSDA.Fill( aDS, "ReturnInfo" );

            asqlconnect.Close();

            dtRI = aDS.Tables["ReturnInfo"];

            gbEditUserParameters.Enabled = false;

            // ���������� datagrid
            Init_Load();

            btnAddUser.Enabled = true;
        }

       /// <summary>
       /// �������� ������������
       /// </summary>
       /// <param Name="sender"></param>
       /// <param Name="e"></param>
        private void btnAddUser_Click( object sender, EventArgs e )
        {
           // �������� ������ � ����������
           pnlHelp.Visible = true;

            isAddedUser = true;    // ����� ���������� ������������ 

            gbEditUserParameters.Enabled = true;
            tbUserName.Enabled = true;
            tbUserName.ReadOnly = false;
            tbUserName.Text = "";
            btnDelUser.Enabled = false;
            btnChUserPass.Enabled = false;

            tbComment.Text = "";
            lbInOldPass.Enabled = false;
            tbInOldPass.Enabled = false;
            lbInNewPass.Enabled = lbNewPassConf.Enabled = true;
            tbInNewPass.Enabled = tbInNewPassConfirm.Enabled = true;

            tbInOldPass.Text = "";
            tbInNewPass.Text = "";
            tbInNewPassConfirm.Text = "";
            newUserGroup = "";

            btnAddUser.Enabled = false;
            btnSaveChange.Enabled = false;
        }

        private void AddedUser( ) 
        {
            // ���������� ������ �� ������������
            // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
           SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            asqlconnect.Open();

            // ������������ ������ ��� ������ �������� ���������
            SqlCommand cmd = new SqlCommand( "User~Create", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // ������� ���������
            // 1. @Username
            SqlParameter pun = new SqlParameter();
            pun.ParameterName = "@Username";
            pun.SqlDbType = SqlDbType.Text;
            pun.Value = tbUserName.Text;
            pun.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pun );
            // 2. @UserPass
            SqlParameter pup = new SqlParameter();
            pup.ParameterName = "@UserPass";
            pup.SqlDbType = SqlDbType.Text;
            pup.Value = newUserPassword;
            pup.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pup );
            // 3. @Comment
            SqlParameter puc = new SqlParameter();
            puc.ParameterName = "@Comment";
            puc.SqlDbType = SqlDbType.Text;
            puc.Value = tbComment.Text;
            puc.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( puc );
            // 4. @UserGroup
            // �������� ��� �� ������
            int idg = 0;
            for( int curRow = 0; curRow < dtG.Rows.Count; curRow++ )
            {
                string st = ( string ) cbUserGroup.SelectedItem;

                if( st == ( ( string ) dtG.Rows[curRow]["GroupName"] ) )
                {
                    idg = ( int ) dtG.Rows[curRow]["ID"];
                    break;
                }
            }

            SqlParameter pug = new SqlParameter();
            pug.ParameterName = "@UserGroup";
            pug.SqlDbType = SqlDbType.Int;
            pug.Value = idg;
            pug.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pug );

            // ���������� DataSet
            DataSet aDS = new DataSet( "ptk" );
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            aSDA.Fill( aDS, "ReturnInfo" );

            asqlconnect.Close();

            dtRI = aDS.Tables["ReturnInfo"];

            gbEditUserParameters.Enabled = false;

            // ���������� datagrid
            Init_Load();
        }

        private void dgvGroups_CellClick( object sender, DataGridViewCellEventArgs e )
        {
			  if( e.RowIndex == -1 )
				  return;	// ��������

			  currentCell = e.RowIndex;
            ShowRights(e.RowIndex);
				ShowGroupMenu( e.RowIndex );
            currentGroup = e.RowIndex;
			btnGroupSaveChange.Enabled = true;
        }

		 /// <summary>
		  /// private TreeNode EnumNode( TreeView tv, StringBuilder sbname )
		  /// ������������ ����� - ���� ����������� ������ ���� �� ������ TreeView
		 /// </summary>
		 /// <param Name="tv"></param>
		 /// <param Name="sbname"></param>
		 /// <returns></returns>
		 private void EnumNode( TreeView tv, StringBuilder sbname )
		 {
			 // ���� � ����� ������� ������.
			 foreach( TreeNode tn in tv.Nodes )
			 {
				if( tn.Checked )
			    continue;

				//��������� ������� �������� ������ ����
				MemoryStream mstream = new MemoryStream();
				BinaryWriter msbw = new BinaryWriter( mstream );
				msbw.Write( tn.Text );

				uint rez = CommonUtils.CommonUtils.CalculateCRC32( mstream );
				
				sbname.Append( rez.ToString() + ";" );
			}
			
			// ���� � ��������.
			 foreach( TreeNode tn in tv.Nodes )
				 // ������ ����� � �����.
				 EnumNodeNodes( tn, sbname );
			}

		 private void EnumNodeNodes( TreeNode treenode, StringBuilder sbname )
			{
				// ���� � ����� ������� ������.
				foreach( TreeNode tn in treenode.Nodes )
				{
					if( tn.Checked )
						continue;

					//��������� ������� �������� ������ ����
					MemoryStream mstream = new MemoryStream();
					BinaryWriter msbw = new BinaryWriter( mstream );
					msbw.Write( tn.Text );

					uint rez = CommonUtils.CommonUtils.CalculateCRC32( mstream );
					sbname.Append( rez.ToString() + ";" );
				}

				// ���� � ��������.
				foreach( TreeNode tn in treenode.Nodes )
					// ������ ����� � �����.
					EnumNodeNodes( tn, sbname );
			}

		 /// <summary>
		 /// btnGroupSaveChange_Click( )
		 /// ��������� ��������� � ������
		 /// </summary>
		 /// <param Name="sender"></param>
		 /// <param Name="e"></param>
        private void btnGroupSaveChange_Click( object sender, EventArgs e )
        {
			  if( DialogResult.No == MessageBox.Show( "��������� ���������?", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) )
			  {
				  // ���������� �������
				  ShowRights( currentCell );
				  ShowGroupMenu( currentCell );
				  currentGroup = currentCell;
				  btnGroupSaveChange.Enabled = false;
				  return;
			  }

			  // ���������� ���������� ������
			  int dgvsrc = 0;
			  dgvsrc = dgvGroups.CurrentCell.RowIndex;

			  // ��������� ������ � crc32-��������� ���� ����������� ������� ������������ ������� ����
			  StringBuilder miDeniedAccess = new StringBuilder();
			  EnumNode( treeViewHideMenu, miDeniedAccess );

            // ��������� ������ � �������
            StringBuilder sr = new StringBuilder();
            for( int i = 0; i < flpGroupRights.Controls.Count; i++ )
            {
                if( !( flpGroupRights.Controls[i] is CheckBox ) )
                    continue;
                CheckBox tch = (CheckBox) flpGroupRights.Controls[i];

                if( tch.Checked )
                    sr.Append( "0" );	// �������� ���������
                else
						  sr.Append( "1" );	// �������� ���������
            }
				sr = CommonUtils.CommonUtils.ReversStr(sr.ToString());
            if (sr == null)
               return;

            // ��������� �����
            // ������ datagrid
            // ���������� ������ �� ������
            // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            asqlconnect.Open();
            SqlCommand cmd;
            DataSet aDS;
            SqlDataAdapter aSDA;
            // ���� ������ �����
            if( isAddGroup )
            {
                isAddGroup = false;
                btnGroupcreate.Enabled = true;
                // ������������ ������ ��� ������ �������� ���������
                cmd = new SqlCommand( "UserGroup~Create", asqlconnect );
                cmd.CommandType = CommandType.StoredProcedure;

                // ������� ���������
                // 1. @Name
                SqlParameter punng = new SqlParameter();
                punng.ParameterName = "@Name";
                punng.SqlDbType = SqlDbType.Text;
                //pun.Value = dtG.Rows[currentGroup]["GroupName"];
                DataGridViewTextBoxCell dtc = ( DataGridViewTextBoxCell ) dgvGroups["clmGroupName", currentGroup];
                punng.Value = dtc.Value;
                punng.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( punng );
                if (String.IsNullOrEmpty(dtc.Value as String))
                {
                   MessageBox.Show("�� �� ����� ��� ������!", "��������������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   return;
                }

                // 2. @comment
                SqlParameter pupng = new SqlParameter();
                pupng.ParameterName = "@comment";
                pupng.SqlDbType = SqlDbType.Text;
                DataGridViewTextBoxCell dtcc = (DataGridViewTextBoxCell) dgvGroups [ "clmGroupComment", currentGroup ];
                pupng.Value = dgvGroups["clmGroupComment", currentGroup].Value;
                pupng.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pupng );
                if( String.IsNullOrEmpty( dtcc.Value as String ) )
                {
                   MessageBox.Show( "�� �� ����� ����������� ��� ������!", "��������������", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                   return;
                }
                // 3. @GroupRight
                SqlParameter pucng = new SqlParameter();
                pucng.ParameterName = "@GroupRight";
                pucng.SqlDbType = SqlDbType.Text;
                pucng.Value = sr.ToString();
                pucng.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pucng );
					 // 4. @HiddenMenu - ������ � crc32 ����������� ������� ����
					 SqlParameter pugng = new SqlParameter();
					 pugng.ParameterName = "@HiddenMenu";
					 pugng.SqlDbType = SqlDbType.Text;
					 pugng.Value = miDeniedAccess.ToString();
					 pugng.Direction = ParameterDirection.Input;
					 cmd.Parameters.Add( pugng );

                // ���������� DataSet
                aDS = new DataSet( "ptk" );
                aSDA = new SqlDataAdapter();
                aSDA.SelectCommand = cmd;

                aSDA.Fill( aDS, "ReturnInfo" );

            }
            else
            {
                // ������������ ������ ��� ������ �������� ���������
                cmd = new SqlCommand( "UserGroup~Edit", asqlconnect );
                cmd.CommandType = CommandType.StoredProcedure;

                // ������� ���������
                // 1. @Id
                SqlParameter pid = new SqlParameter();
                pid.ParameterName = "@id";
                pid.SqlDbType = SqlDbType.Int;
                pid.Value = dtG.Rows[currentGroup]["ID"];
                pid.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pid );
                // 2. @Name
                SqlParameter pun = new SqlParameter();
                pun.ParameterName = "@Name";
                pun.SqlDbType = SqlDbType.Text;
                //pun.Value = dtG.Rows[currentGroup]["GroupName"];
                DataGridViewTextBoxCell dtc = ( DataGridViewTextBoxCell ) dgvGroups["clmGroupName", currentGroup];
                pun.Value = dtc.Value;
                //pun.Value = dgvGroups["clmGroupName", currentGroup].Value;

                pun.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pun );
                // 3. @comment
                SqlParameter pup = new SqlParameter();
                pup.ParameterName = "@comment";
                pup.SqlDbType = SqlDbType.Text;
                //pup.Value = dtG.Rows[currentGroup]["Comment"];
                pup.Value = dgvGroups["clmGroupComment", currentGroup].Value;

                pup.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pup );
                // 4. @GroupRight
                SqlParameter puc = new SqlParameter();
                puc.ParameterName = "@GroupRight";
                puc.SqlDbType = SqlDbType.Text;
                puc.Value = sr.ToString();
                puc.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( puc );
					 // 4. @HiddenMenu - ������ � crc32 ����������� ������� ����
					 SqlParameter pug = new SqlParameter();
					 pug.ParameterName = "@HiddenMenu";
					 pug.SqlDbType = SqlDbType.Text;
					 pug.Value = miDeniedAccess.ToString();
					 pug.Direction = ParameterDirection.Input;
					 cmd.Parameters.Add( pug );


                // ���������� DataSet
                aDS = new DataSet( "ptk" );
                aSDA = new SqlDataAdapter();
                aSDA.SelectCommand = cmd;

                aSDA.Fill( aDS, "ReturnInfo" );
            }
           

            // ��������� ���������� �� ���������� � ������
            for( int i = 0; i < dgvRights.Rows.Count; i++ )
            { 
                //��������� ����� � ����������
                cmd = new SqlCommand( "UserRight~Edit", asqlconnect );
                cmd.CommandType = CommandType.StoredProcedure;

                // ������� ���������
                // 1. @ID
                SqlParameter pidr = new SqlParameter();
                pidr.ParameterName = "@ID";
                pidr.SqlDbType = SqlDbType.Int;
                pidr.Value = (int)dgvRights["clmIdRightRecord", i].Value;
                pidr.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pidr );
                // 2. @RightName
                SqlParameter punr = new SqlParameter();
                punr.ParameterName = "@RightName";
                punr.SqlDbType = SqlDbType.Text;
                punr.Value = dgvRights["clmRightsName", i].Value;
                punr.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( punr );
                // 3. @RightLog
                SqlParameter pupr = new SqlParameter();
                pupr.ParameterName = "@RightLog";
                pupr.SqlDbType = SqlDbType.Bit;
                pupr.Value = dgvRights["clmVedenieLoga", i].Value;
                pupr.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pupr );
                // 4. @RightShow
                SqlParameter pucr = new SqlParameter();
                pucr.ParameterName = "@RightShow";
                pucr.SqlDbType = SqlDbType.Bit;
                pucr.Value = dgvRights["clmOutInLog", i].Value;
                pucr.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pucr );

                // ���������� DataSet
                //DataSet aDS = new DataSet( "ptk" );
                 aSDA = new SqlDataAdapter();//SqlDataAdapter
                aSDA.SelectCommand = cmd;
                aSDA.Fill( aDS, "ReturnInfo" );
            }
            
            asqlconnect.Close();
                // ���������� datagrid
                dgvGroups.Rows.Clear();

            Init_Load();
				SetSelectedRow( dgvGroups, dgvsrc );
        }
		 private void SetSelectedRow( DataGridView dgv, int dgvsrc )
		 {
			 dgv.CurrentCell = dgv.Rows[dgvsrc].Cells[0] ;
		 }
        private void btnGroupcreate_Click( object sender, EventArgs e )
        {
            currentGroup = dgvGroups.Rows.Add();
            isAddGroup = true;
            btnGroupcreate.Enabled = false;
			btnGroupSaveChange.Enabled = true;
            SetSelectedRow(dgvGroups, dgvGroups.Rows.Count - 1);
           //dgvGroups.Rows[dgvGroups.Rows.Count - 1].Cells[0].Selected = true;
        }

        private void btnGroupDel_Click( object sender, EventArgs e )
        {
           string grname = (string)dgvGroups.CurrentRow.Cells[0].Value;

            isAddGroup = false;
            btnGroupcreate.Enabled = true;
			btnGroupSaveChange.Enabled = true;

           if (String.IsNullOrEmpty(grname))
            {
               DataGridViewSelectedRowCollection dgvwrs = dgvGroups.SelectedRows;
               foreach (DataGridViewRow dgvwr in dgvwrs)
                  dgvGroups.Rows.Remove(dgvwr);

               return;
            }

			  if( DialogResult.No == MessageBox.Show( "������� ������ =" + grname + "=? ", "��������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) )
			  {
				  return;
			  }
			  // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
           SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            asqlconnect.Open();

            // ������������ ������ ��� ������ �������� ���������
            SqlCommand cmd = new SqlCommand( "UserGroup~Erase", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // ������� ���������
            // 1. @Id
            SqlParameter pid = new SqlParameter();
            pid.ParameterName = "@id";
            pid.SqlDbType = SqlDbType.Int;
            pid.Value = dgvGroups["clmIdGroup", currentGroup].Value;
            if( pid.Value == null )
               return;
            pid.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pid );


            // ���������� DataSet
            DataSet aDS = new DataSet( "ptk" );
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            aSDA.Fill( aDS );//, "ReturnInfo"
            
            DataTable dt = aDS.Tables[0];

            asqlconnect.Close();
            //PrintDataSet( aDS );
            // ��������� ������ 
            DataTable dtt = aDS.Tables[0];
            bool adata = ( bool ) dtt.Rows[0]["Column1"];
            if( !adata )
                MessageBox.Show("� ������ ���� ������������. ������ �� �������!","������");
            // ������ datagrid
            //dgvGroups.Rows.Clear();
            // ���������� datagrid
            Init_Load();
        }

        #region ����������� ������ DataSet
        static void PrintDataSet( DataSet ds )
        {
            // ����� ��������� ���� �� ���� DataTable ������� DataSet
            Console.WriteLine( "������� � DataSet '{0}'. \n ", ds.DataSetName );
            foreach( DataTable dt in ds.Tables )
            {
                Console.WriteLine( "������� '{0}'. \n ", dt.TableName );
                // ����� ���� ��������
                for( int curCol = 0; curCol < dt.Columns.Count; curCol++ )
                    Console.Write( dt.Columns[curCol].ColumnName.Trim() + "\t" );
                Console.WriteLine( "\n-----------------------------------------------" );

                // ����� DataTable
                for( int curRow = 0; curRow < dt.Rows.Count; curRow++ )
                {
                    for( int curCol = 0; curCol < dt.Columns.Count; curCol++ )
                        Console.Write( dt.Rows[curRow][curCol].ToString() + "\t" );
                    Console.WriteLine();
                }
            }
        }
        #endregion

        private void tabUsers_Enter( object sender, EventArgs e )
        {
            Init_Load();
        }

		 private void tbGroupRights_Enter( object sender, EventArgs e )
		 {
			 // ����� ���������� ������ ��� ������� (����������) ������ - ��� ����� ��������� ������
			 // � ���������� ������

			 ShowGroupMenu( 0 );
			 btnGroupSaveChange.Enabled = false;
			 //DisplayTreeView( new ArrayList() );	// ���������� ���� as is - ��� ��������
		 }

		 /// <summary>
		 /// public void DisplayTreeView( ArrayList aLD )
		 /// ���������� ������ ���� � ������ ��������
		 /// </summary>
		 /// <param Name="aLD"> ������ crc32-������� ���� ����������� ������� ����</param>
		 public void DisplayTreeView( ArrayList aLD )
		 {
			 int j = 0;
			 // ������ TreeView ��� �������� ���� � ����������� ���� �������
			 // � ������ ���� ���� Text �������� �������� ����, ������� ����� ������������ � TreeView
			 // ����� MainForm ������ ����� ������ �������� ���� ��� � ����� ���� � ������ ����, 
			 // � ���, ����� �� ����� ���� ����������� � �������� � TreewView
			 treeViewHideMenu.Nodes.Clear();
             for (int i = 0; i < HMI_MT_Settings.HMI_Settings.alMenu.Count; i++)
			 {
				 MenuStrip ms;
				 ContextMenuStrip cms;
                 if (HMI_MT_Settings.HMI_Settings.alMenu[i] is MenuStrip)
				 {
                     ms = (MenuStrip)HMI_MT_Settings.HMI_Settings.alMenu[i];
					 treeViewHideMenu.Nodes.Add( ms.Text );
					 treeViewHideMenu.Nodes[j].Checked = true;
					 PopulateTreeView( treeViewHideMenu.Nodes[j], ms, aLD );
					 j++;
				 }
                 else if (HMI_MT_Settings.HMI_Settings.alMenu[i] is ContextMenuStrip)
				 {
                     cms = (ContextMenuStrip)HMI_MT_Settings.HMI_Settings.alMenu[i];
					 treeViewHideMenu.Nodes.Add( cms.Text );
					 treeViewHideMenu.Nodes[j].Checked = true;
					 PopulateTreeView( treeViewHideMenu.Nodes[j], cms, aLD );
					 j++;
				 }
			 }

			 treeViewHideMenu.AfterCheck -= treeViewHideMenu_AfterCheck;

			 TestTreeView4WrongCheck(treeViewHideMenu);

			 treeViewHideMenu.AfterCheck +=new TreeViewEventHandler(treeViewHideMenu_AfterCheck);

			 treeViewHideMenu.ExpandAll();
		 }
		/// <summary>
		/// �������� �� ������� ����, ��� ���� �������, �����
		/// ������� ���� �� ���� �� ����������� �����
		/// </summary>
		/// <param Name="treeViewHideMenu"></param>
		 private void TestTreeView4WrongCheck(TreeView treeViewHideMenu)
		 {
			 foreach ( TreeNode tn in treeViewHideMenu.Nodes )
					 tn.Checked = TestNode4Check(tn);
		 }

		 private bool TestNode4Check(TreeNode tn)
		 {
			 if (tn.Nodes.Count != 0)
			 {
				 foreach (TreeNode tnn in tn.Nodes)
					 if (TestNode4Check(tnn))
						 return true;
			 }
			 else
				return tn.Checked;

			 return false;
		 }
		 /// <summary>
		 /// public void PopulateTreeView( TreeNode parentNode )
		 /// ��������� TreeView ��� ����
		 /// </summary>
		 /// <param Name="parentNode"></param>
		 ///  <param Name="ms_csm"></param>		 
		 public void PopulateTreeView( TreeNode parentNode, MenuStrip ms_csm, ArrayList aLD )
		 {
			 // ���������� ������� ���� � TreeView
			 // ���������� ������ ���� (��� �������)
			 for( int i = 0 ; i < ms_csm.Items.Count ; i++ )
			 {
				 ToolStripMenuItem mi = ( ToolStripMenuItem ) ms_csm.Items[i];
				 TreeNode aMINode = new TreeNode( mi.Text );
				 //mi.Available = IsMIDenied( mi.Text, aLD );
				 aMINode.Checked = !CommonUtils.CommonUtils.IsMIDenied( mi.Text, aLD );
				 if (aLD.Count == 0)
					aMINode.Checked = mi.Available ? true : false;	// ���� �������� ��������� ��������� �.�.
				 parentNode.Nodes.Add( aMINode );
				 for( int j = 0 ; j < mi.DropDownItems.Count ; j++ )
				 {
					 if( mi.DropDownItems[j] is ToolStripMenuItem )
					 {
						 ToolStripMenuItem middi = ( ToolStripMenuItem ) mi.DropDownItems[j];

						 TreeNode aMIDDINode = new TreeNode( middi.Text );
						 //middi.Available = IsMIDenied( middi.Text, aLD );
						 aMIDDINode.Checked = !CommonUtils.CommonUtils.IsMIDenied( middi.Text, aLD );	// ���� �������� ��������� ��������� �.�.
						 if( aLD.Count == 0 )
							 aMIDDINode.Checked = middi.Available ? true : false;
						 aMINode.Nodes.Add( aMIDDINode );
					 }
				 }
			 }
		 }

		 /// <summary>
		 /// public void PopulateTreeView( TreeNode parentNode )
		 /// ��������� TreeView ��� ���� (������������� ����� - ��� ContextMenuStrip)
		 /// </summary>
		 /// <param Name="parentNode"></param>
		 ///  <param Name="ms_csm"></param>		 
		 public void PopulateTreeView( TreeNode parentNode, ContextMenuStrip ms_csm, ArrayList aLD )
		 {
			 // ���������� ������� ���� � TreeView
			 // ���������� ������ ���� (��� �������)
			 for( int i = 0 ; i < ms_csm.Items.Count ; i++ )
			 {
				 ToolStripMenuItem mi = ms_csm.Items[i] as ToolStripMenuItem ;
				 if( mi == null )
					 continue;

				 TreeNode aMINode = new TreeNode( mi.Text );
				 //mi.Available = IsMIDenied( mi.Text, aLD );
				 aMINode.Checked = !CommonUtils.CommonUtils.IsMIDenied( mi.Text, aLD );
				 if( aLD.Count == 0 )
					 aMINode.Checked = mi.Available ? true : false;	// ���� �������� ��������� ��������� �.�.
				 parentNode.Nodes.Add( aMINode );
				 for( int j = 0 ; j < mi.DropDownItems.Count ; j++ )
				 {
					 if( mi.DropDownItems[j] is ToolStripMenuItem )
					 {
						 ToolStripMenuItem middi = ( ToolStripMenuItem ) mi.DropDownItems[j];

						 TreeNode aMIDDINode = new TreeNode( middi.Text );
						 //middi.Available = IsMIDenied( middi.Text, aLD );
						 aMIDDINode.Checked = !CommonUtils.CommonUtils.IsMIDenied( middi.Text, aLD );
						 if( aLD.Count == 0 )
							 aMIDDINode.Checked = middi.Available ? true : false;	// ���� �������� ��������� ��������� �.�.
						 aMINode.Nodes.Add( aMIDDINode );
					 }
				 }
			 }
		 }
		 private void tbGroupRights_Paint( object sender, PaintEventArgs e )
		 {
			 gbCustomRight.Height = gbRights.Height;

			 gbCustomRight.Width = ( splitContainer2.Panel1.Width - dgvGroups.Width - gbRights.Width ) / 2;
			 gbMenuTreeview.Width = splitContainer2.Panel1.Width - dgvGroups.Width - gbRights.Width - gbCustomRight.Width;
		 }

		 private void treeViewHideMenu_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
		 {
			 if (e.Button == MouseButtons.Right)
			 {
				 TreeNode node = e.Node;
				 contextMenuStripMenuTreeView.Show( treeViewHideMenu, new Point( e.X, e.Y ) );
			 }
		 }
		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param Name="sender"></param>
		 /// <param Name="e"></param>
		 private void treeViewHideMenu_AfterCheck( object sender, TreeViewEventArgs e )
		 {
			 btnGroupSaveChange.Enabled = true;

			 TreeNode node = e.Node;
			 SelectAllSubnodes( node );
		 }

       /// <summary>
       /// ����� ��� ��������� ������� ��� ���� ��������
       /// </summary>
       /// <param Name="treeNode"></param>
		 void SelectAllSubnodes( TreeNode treeNode )
		 {
			 // ������ ��� ������� ������� �� ���� ��������.
			 foreach( TreeNode treeSubNode in treeNode.Nodes )
			 {
				 treeSubNode.Checked = treeNode.Checked;
			 }
		 }
    }
 }