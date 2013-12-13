using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using HMI_MT_Settings;
using CRZADevices;
using System.Globalization;

namespace HelperControlsLibrary
{
    public partial class InformationControl : UserControl
    {
        internal uint UniDevId;

        public InformationControl()
        {
            InitializeComponent();
        }
        private void InformationControlLoad( object sender, EventArgs e ) { InfoDataBase( UniDevId, PanelInfoTextBox, rtbInfo ); }

        private static void InfoDataBase( uint uniDev, Control tBoxText, TextBoxBase rtBox )
        {
            if ( tBoxText == null || rtBox == null )
                return;
            if ( rtBox.Lines.Count() != 0 )
                return;

            using ( var asqlconnect = new SqlConnection( HMI_Settings.ProviderPtkSql ) )
            {
                try
                {
                    asqlconnect.Open();

                    // формирование данных для вызова хранимой процедуры
                    var cmd = new SqlCommand( "GetBlockInfo", asqlconnect ) { CommandType = CommandType.StoredProcedure };

                    // входные параметры
                    // id устройства
                    var pidBlock = new SqlParameter
                    {
                        ParameterName = "@BlockId",
                        SqlDbType = SqlDbType.Int,
                        Value = uniDev,
                        Direction = ParameterDirection.Input
                    };
                    cmd.Parameters.Add( pidBlock );

                    // заполнение DataSet
                    using ( var aDs = new DataSet( "ptk" ) )
                    using ( var aSda = new SqlDataAdapter() )
                    {
                        aSda.SelectCommand = cmd;

                        //aSDA.sq
                        aSda.Fill( aDs, "TbInfo" );

                        // извлекаем данные
                        var dtI = aDs.Tables["TbInfo"];

                        // заполняем RichTextBox
                        for ( var curRow = 0; curRow < dtI.Rows.Count; curRow++ )
                        {
                            tBoxText.Text = tBoxText.Text + CommonCRZADeviceFunction.GetTimeInMTRACustomFormat(
                                (DateTime)dtI.Rows[curRow]["DateTime_Modify"] );
                            var utf = new UTF8Encoding();
                            rtBox.AppendText( utf.GetString( (byte[])dtI.Rows[curRow]["Data"] ) );
                        }
                    }
                }
                catch ( SqlException ex )
                {
                    var errorMes = ex.Errors.Cast<SqlError>().Aggregate( string.Empty,
                                                                         ( current, connectError ) =>
                                                                         current +
                                                                         ( connectError.Message + " (ощибка: " +
                                                                           connectError.Number.ToString(
                                                                               CultureInfo.InvariantCulture ) + ")" +
                                                                           Environment.NewLine ) );

                    // интеграция всех возвращаемых ошибок
                    CommonUtils.CommonUtils.WriteEventToLog( 21, "Нет связи с БД (UstavBD): " + errorMes, false );
                    System.Diagnostics.Trace.TraceInformation( "\n" +
                                                               DateTime.Now.ToString( CultureInfo.InvariantCulture ) +
                                                               " : frmBMRZ : Нет связи с БД (UstavBD)" );
                }
                catch ( Exception ex )
                {
                    MessageBox.Show( "Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка",
                                     MessageBoxButtons.OK, MessageBoxIcon.Information );
                }
            }
        }
    }
}
