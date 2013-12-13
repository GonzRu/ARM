using System;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

using HMI_MT_Settings;

namespace HelperControlsLibrary
{
    public class PanelBottomEventBlock : SelectControl
    {
        int nfc;
        int nDev;
        ListView lstvEventLog;
        delegate void SetLvuCallback( ListViewItem li, bool actDellstV );
        
        public PanelBottomEventBlock()
        {
            btnUpdate.Text = "Чтение из БД";
            btnUpdate.Click += this.BtnUpdateClick;
        }
        public void InitPanel( int uniDev, ListView lstv )
        {
            this.nfc = uniDev / 256;
            this.nDev = uniDev % 256;
            lstvEventLog = lstv;
        }

        public void LinkSetLvu( object value, bool actDellstV )
        {
            if ( !( value is ListViewItem ) && !actDellstV )
                return;   // сгенерировать ошибку через исключение

            ListViewItem li = null;
            if ( !actDellstV )
                li = (ListViewItem)value;

            if ( lstvEventLog.InvokeRequired )
            {
                this.SetLvu( !actDellstV ? li : null, actDellstV );
            }
            else
            {
                if ( !actDellstV )
                    lstvEventLog.Items.Add( li );
                else
                    lstvEventLog.Items.Clear();
            }
        }
        private SqlCommand CreateCommand( SqlConnection connection )
        {
            // формирование данных для вызова хранимой процедуры
            var cmd = new SqlCommand( "ShowEventLog", connection ) { CommandType = CommandType.StoredProcedure };

            // входные параметры
            // 1. начальное время
            var dtMim = new SqlParameter
                {
                    ParameterName = "@Dt_start",
                    SqlDbType = SqlDbType.DateTime,
                    Value = StartDateCollapsed,
                    Direction = ParameterDirection.Input
                };
            cmd.Parameters.Add( dtMim );

            // 2. конечное время
            var dtMax = new SqlParameter
                {
                    ParameterName = "@Dt_end",
                    SqlDbType = SqlDbType.DateTime,
                    Value = EndTimeCollapsed,
                    Direction = ParameterDirection.Input
                };
            cmd.Parameters.Add( dtMax );

            // 3. id устройства
            var pidBlock = new SqlParameter
                {
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.Int,
                    Value = this.nfc * 256 + this.nDev,
                    Direction = ParameterDirection.Input
                };
            cmd.Parameters.Add( pidBlock );

            // 4. @Name
            var pName = new SqlParameter
                {
                    ParameterName = "@Name",
                    SqlDbType = SqlDbType.VarChar,
                    Value = String.Empty,
                    Direction = ParameterDirection.Input
                };
            // пустая строка
            cmd.Parameters.Add( pName );

            return cmd;
        }
        private void SetLvu( ListViewItem li, bool actDellstV )
        {
            if ( lstvEventLog.InvokeRequired )
            {
                SetLvuCallback d = this.SetLvu;
                Invoke( d, new object[] { li, actDellstV } );
            }
            else
            {
                if ( !actDellstV )
                    lstvEventLog.Items.Add( li );
                else
                    lstvEventLog.Items.Clear();
            }
        }
        private void BtnUpdateClick( object sender, EventArgs e )
        {
            // получение строк соединения и поставщика данных из файла *.config
            using ( var asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql ) )
            {
                try
                {
                    asqlconnect.Open();

                    DataTable dtEventLog;
                    using ( var aDs = new DataSet( "ptk" ) )
                        using ( var aSda = new SqlDataAdapter() )
                        {
                            aSda.SelectCommand = CreateCommand( asqlconnect );
                            aSda.Fill( aDs, "TbEventLog" );
                            dtEventLog = aDs.Tables["TbEventLog"];
                        }
                    
                    // очищаем ListView для обновления
                    this.LinkSetLvu( null, true );
                    var ts = new StringBuilder();
                    for ( int curRow = 0; curRow < dtEventLog.Rows.Count; curRow++ )
                    {
                        var li = new ListViewItem();
                        li.SubItems.Clear();

                        // время фиксации события ФК
                        ts.Length = 0;
                        var t = (DateTime)dtEventLog.Rows[curRow]["LocalTime"];
                        li.Tag = t;	// для сортировки
                        li.SubItems.Add( t.ToShortDateString() + " : " + t.ToLongTimeString() + "." + t.Millisecond );

                        ts.Length = 0;
                        ts.Append( dtEventLog.Rows[curRow]["EventText"] );
                        li.SubItems.Add( ts.ToString() );

                        this.LinkSetLvu( li, false );
                    }
                    // раскращиваем ListView в зебру
                    CommonUtils.CommonUtils.DrawAsZebra( lstvEventLog );
                }
                catch ( SqlException ex )
                {
                    System.Diagnostics.Trace.TraceInformation( ex.Message );
                }
                catch ( Exception ex )
                {
                    MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );
                }
            }
        }
    }
}
