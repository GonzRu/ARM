using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using HMI_MT_Settings;

namespace HMI_MT
{
    public partial class frmUserRights : Form
    {
        public frmUserRights( )
        {
            InitializeComponent();
        }

        private void frmUserRights_Load( object sender, EventArgs e )
        {
            splitContainer_UserRights.Width = Parent.Width;
            pnlUser.Width = splitContainer_UserRights.Width / 3;
            pnlGroups.Width = splitContainer_UserRights.Width / 3;
            pnlRights.Width = splitContainer_UserRights.Width / 3;

            //извлекаем данные по пользователям
            // получение строк соединения и поставщика данных из файла *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql );
            asqlconnect.Open();

            // формирование данных для вызова хранимой процедуры
            SqlCommand cmd = new SqlCommand( "User~Show", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // входные параметры
            // 1. @id пользователя или 0
            SqlParameter pid = new SqlParameter();
            pid.ParameterName = "@id";
            pid.SqlDbType = SqlDbType.Int;
            pid.Value = 0;  // все пользователи
            pid.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pid );

            // заполнение DataSet
            DataSet aDS = new DataSet( "ptk" );
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            aSDA.Fill( aDS, "User" );

            //asqlconnect.Close();

            // извлекаем перечень пользователей
            DataTable dtU = aDS.Tables["User"];

            LinkSetLV(lstvUser, null, true );    // очищаем ListView для обновления  

            StringBuilder ts = new StringBuilder();
            for( int curRow = 0; curRow < dtU.Rows.Count; curRow++ )
            {
                ListViewItem li = new ListViewItem();
                li.SubItems.Clear();
                int t = ( int ) dtU.Rows[curRow]["Id"];
                li.SubItems.Add( t.ToString() );

                ts.Length = 0;
                ts.Append( dtU.Rows[curRow]["UserName"] );
                li.SubItems.Add( ts.ToString() );

                LinkSetLV( lstvUser, li, false );
            }

            //извлекаем данные по группам
            // формирование данных для вызова хранимой процедуры
            cmd = new SqlCommand( "UserGroup~Show", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // входные параметры
            // 1. @id группы или 0
            SqlParameter pidg = new SqlParameter();
            pidg.ParameterName = "@Id";
            pidg.SqlDbType = SqlDbType.Int;
            pidg.Value = 0;  // все пользователи
            pidg.Direction = ParameterDirection.Input;
            cmd.Parameters.Add( pidg );

            // заполнение DataSet
            aSDA.SelectCommand = cmd;

            aSDA.Fill( aDS, "Group" );

            asqlconnect.Close();

            DataTable dtG = aDS.Tables["Group"];

            LinkSetLV( lstvGroups, null, true );    // очищаем ListView для обновления  

            //StringBuilder 
            ts = new StringBuilder();
            for( int curRow = 0; curRow < dtG.Rows.Count; curRow++ )
            {
                ListViewItem li = new ListViewItem();
                li.SubItems.Clear();
                int t = ( int ) dtG.Rows[curRow]["ID"];
                li.SubItems.Add( t.ToString() );

                ts.Length = 0;
                ts.Append( dtG.Rows[curRow]["GroupName"] );
                li.SubItems.Add( ts.ToString() );

                LinkSetLV( lstvGroups, li, false );
            }

            //извлекаем данные по правам
            // формирование данных для вызова хранимой процедуры
            cmd = new SqlCommand( "UserRight~Show", asqlconnect );
            cmd.CommandType = CommandType.StoredProcedure;

            // входные параметры - отсутствуют

            // заполнение DataSet
            aSDA.SelectCommand = cmd;

            aSDA.Fill( aDS, "Rights" );

            asqlconnect.Close();

            DataTable dtR = aDS.Tables["Rights"];

            LinkSetLV( lstvRignts, null, true );    // очищаем ListView для обновления  

            //StringBuilder 
            ts = new StringBuilder();
            for( int curRow = 0; curRow < dtR.Rows.Count; curRow++ )
            {
                ListViewItem li = new ListViewItem();
                li.SubItems.Clear();
                int tbn = (int) dtR.Rows[curRow]["BitNumber"]; 
                li.SubItems.Add( tbn.ToString() );

                ts.Length = 0;
                ts.Append( dtR.Rows[curRow]["RightName"] );
                li.SubItems.Add( ts.ToString() );

                ts.Length = 0;
                bool brl = (bool) dtR.Rows[curRow]["RightLog"];
                if (brl)
                    ts.Append( "да" );
                else
                    ts.Append( "нет" );
                li.SubItems.Add( ts.ToString() );

                ts.Length = 0;
                bool brs = ( bool ) dtR.Rows[curRow]["RightShow"];
                if( brs )
                    ts.Append( "да" );
                else
                    ts.Append( "нет" );

                li.SubItems.Add( ts.ToString() );

                LinkSetLV( lstvRignts, li, false );
            }

            //lstvUser.Click += new EventHandler(lstvUser_Click);
        }
        /*==========================================================================*
          *   private void void LinkSetText(object Value)
          *      для потокобезопасного вызова процедуры
          *==========================================================================*/
        delegate void SetLVCallback( ListView LV, ListViewItem li, bool actDellstV );

        // actDellstV - действия с ListView : false - не трогать; true - очистить;
        public void LinkSetLV( ListView LV, object Value, bool actDellstV )
        {
            if( !( Value is ListViewItem ) && !actDellstV )
                return;   // сгенерировать ошибку через исключение

            ListViewItem li = null;
            if( !actDellstV )
                li = ( ListViewItem ) Value;

            if( LV.InvokeRequired )
            {
                if( !actDellstV )
                    SetLV( LV, li, actDellstV );
                else
                    SetLV( LV, null, actDellstV );
            }
            else
            {
                if( !actDellstV )
                     LV.Items.Add( li );
                else
                     LV.Items.Clear();
            }
        }

        /*==========================================================================*
        * private void SetText(ListViewItem li)
        * //для потокобезопасного вызова процедуры
        *==========================================================================*/
        private void SetLV( ListView LV, ListViewItem li, bool actDellstV )
        {
            if( LV.InvokeRequired )
            {
                SetLVCallback d = new SetLVCallback( SetLV );
                this.Invoke( d, new object[] {LV, li, actDellstV } );
            }
            else
            {
                if( !actDellstV )
                    LV.Items.Add( li );
                else
                    LV.Items.Clear();
            }
        }

                
       /*private void lstvUser_Click( object sender, EventArgs e )
        {
           
        }*/

        private void lstvUser_SelectedIndexChanged( object sender, EventArgs e )
        {
for( int i = 0; i < lstvUser.Items.Count; i++ ) //foreach( ListViewItem li in lstvPanMes )
               if( lstvUser.Items[i].Selected )
               {
                   //lstvUser.Items[i].Checked = true;
                   lstvUser.Items[i].BackColor = Color.Blue;
               }
               else
               {
                   //lstvUser.Items[i].Checked = false;
                   lstvUser.Items[i].BackColor = SystemColors.Control;
               }
        }
                
    }
}