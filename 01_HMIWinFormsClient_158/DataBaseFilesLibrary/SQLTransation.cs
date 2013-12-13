using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataBaseFilesLibrary
{
    class SqlTransaction
    {
        public enum ActionSqlTransaction
        {
            List,
            Add,
            Delete,
            Recovery,
            Read
        }
        public event EventHandler SqlTransactionEvent;

        public void ExecuteOperation( SqlCommand command, DataSet dataSet, ActionSqlTransaction action )
        {
            using ( var sqlConnection = new SqlConnection( HMI_MT_Settings.HMI_Settings.ProviderPtkSql ) )
            {
                sqlConnection.Open();
                command.Connection = sqlConnection;
                ExecuteTableCommand( command, dataSet, action );
            }
        }
        public void ExecuteOperation( IEnumerable<SqlCommand> commands, DataSet dataSet, ActionSqlTransaction action )
        {
            using ( var sqlConnection = new SqlConnection( HMI_MT_Settings.HMI_Settings.ProviderPtkSql ) )
            {
                sqlConnection.Open();
                foreach ( var command in commands )
                {
                    command.Connection = sqlConnection;
                    ExecuteTableCommand( command, dataSet, action );
                }
            }
        }
        public void ExecuteOperation( SqlCommand command, ActionSqlTransaction action )
        {
            using ( var sqlConnection = new SqlConnection( HMI_MT_Settings.HMI_Settings.ProviderPtkSql ) )
            {
                sqlConnection.Open();
                command.Connection = sqlConnection;
                ExecuteParameterCommand( command, action );
            }
        }
        public void ExecuteOperation( IEnumerable<SqlCommand> commands, ActionSqlTransaction action )
        {
            using ( var sqlConnection = new SqlConnection( HMI_MT_Settings.HMI_Settings.ProviderPtkSql ) )
            {
                sqlConnection.Open();
                foreach ( var command in commands )
                {
                    command.Connection = sqlConnection;
                    ExecuteParameterCommand( command, action );
                }
            }
        }
        private void ExecuteTableCommand( SqlCommand command, DataSet dataSet, ActionSqlTransaction action )
        {
            using ( command )
            {
                using ( var da = new SqlDataAdapter( command ) )
                {
                    try
                    {
                        var res = da.Fill( dataSet );
                        if ( this.SqlTransactionEvent != null )
                            this.SqlTransactionEvent( this, new SqlTransactionEvenArgs( res, action ) );
                    }
                    catch ( Exception ex )
                    {
                        throw new SqlTransationException( ex.Message ); /* мое исключение */
                    }
                }
            }
        }
        private void ExecuteParameterCommand( SqlCommand command, ActionSqlTransaction action )
        {
            using ( command )
            {
                try
                {
                    var res = command.ExecuteNonQuery();

                    var outParam =
                        command.Parameters.Cast<SqlParameter>().FirstOrDefault(
                            parameter => parameter.Direction == ParameterDirection.Output );

                    if ( this.SqlTransactionEvent != null )
                        this.SqlTransactionEvent( this,
                                                  outParam != null
                                                      ? new SqlTransactionEvenArgs( (int)outParam.Value, action )
                                                      : new SqlTransactionEvenArgs( res, action ) );
                }
                catch ( Exception ex )
                {
                    throw new SqlTransationException( ex.Message ); /* мое исключение */
                }
            }
        }

        public static SqlCommand GetDataList( int blockId, bool showDelete )
        {
            // подготавливаем процедуру
            var cmd = new SqlCommand( "UnitFile_List" ) { CommandType = CommandType.StoredProcedure };

            // формируем параметры процедуры
            var sqlParameter = new SqlParameter
                {
                    ParameterName = "@BlockId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = blockId
                };
            cmd.Parameters.Add( sqlParameter );

            sqlParameter = new SqlParameter
                {
                    ParameterName = "@ShowDelete",
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    Value = ( showDelete ) ? 1 : 0
                };
            cmd.Parameters.Add( sqlParameter );

            return cmd;
        }
        public static SqlCommand AddFile( int blockId, int userId, string comment, string fileName, byte[] record )
        {
            // подготавливаем процедуру
            var cmd = new SqlCommand( "UnitFile_Add" ) { CommandType = CommandType.StoredProcedure };

            // формируем параметры процедуры
            var sqlParameter = new SqlParameter
                {
                    ParameterName = "@BlockId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = blockId
                };
            cmd.Parameters.Add( sqlParameter );

            sqlParameter = new SqlParameter
                {
                    ParameterName = "@UserId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = userId
                };
            cmd.Parameters.Add( sqlParameter );

            sqlParameter = new SqlParameter
                {
                    ParameterName = "@Comment",
                    SqlDbType = SqlDbType.VarChar,
                    Size = comment.Length,
                    Direction = ParameterDirection.Input,
                    Value = comment
                };
            cmd.Parameters.Add( sqlParameter );

            sqlParameter = new SqlParameter
                {
                    ParameterName = "@fn",
                    SqlDbType = SqlDbType.VarChar,
                    Size = fileName.Length,
                    Direction = ParameterDirection.Input,
                    Value = fileName
                };
            cmd.Parameters.Add( sqlParameter );

            sqlParameter = new SqlParameter
                {
                    ParameterName = "@Record",
                    SqlDbType = SqlDbType.Image,
                    Direction = ParameterDirection.Input,
                    Size = record.Length,
                    Value = record
                };
            cmd.Parameters.Add( sqlParameter );

            // формируем выходные параметры процедуры
            sqlParameter = new SqlParameter
                {
                    ParameterName = "@out",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
            cmd.Parameters.Add( sqlParameter );

            return cmd;
        }
        public static SqlCommand DeleteFile( int recordId, int userId, string comment )
        {
            // подготавливаем процедуру
            var cmd = new SqlCommand( "UnitFile_Delete" ) { CommandType = CommandType.StoredProcedure };

            // формируем параметры процедуры
            var sqlParameter = new SqlParameter
                {
                    ParameterName = "@RecordId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = recordId
                };
            cmd.Parameters.Add( sqlParameter );

            sqlParameter = new SqlParameter
                {
                    ParameterName = "@UserId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = userId
                };
            cmd.Parameters.Add( sqlParameter );

            sqlParameter = new SqlParameter
                {
                    ParameterName = "@Comment",
                    SqlDbType = SqlDbType.VarChar,
                    Size = comment.Length,
                    Direction = ParameterDirection.Input,
                    Value = comment
                };
            cmd.Parameters.Add( sqlParameter );

            // формируем выходные параметры процедуры
            sqlParameter = new SqlParameter
                {
                    ParameterName = "@out",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
            cmd.Parameters.Add( sqlParameter );

            return cmd;
        }
        public static SqlCommand RecoveryFile( int recordId )
        {
            // подготавливаем процедуру
            var cmd = new SqlCommand( "UnitFile_Recovery" ) { CommandType = CommandType.StoredProcedure };

            // формируем параметры процедуры
            var sqlParameter = new SqlParameter
                {
                    ParameterName = "@RecordId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = recordId
                };
            cmd.Parameters.Add( sqlParameter );

            // формируем выходные параметры процедуры
            sqlParameter = new SqlParameter
                {
                    ParameterName = "@out",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
            cmd.Parameters.Add( sqlParameter );

            return cmd;
        }
        public static SqlCommand ReadFile( int recordId )
        {
            // подготавливаем процедуру
            var cmd = new SqlCommand( "UnitFile_Get" ) { CommandType = CommandType.StoredProcedure };

            // формируем параметры процедуры
            var sqlParameter = new SqlParameter
                {
                    ParameterName = "@RecordId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    Value = recordId
                };
            cmd.Parameters.Add( sqlParameter );

            return cmd;
        }
    }

    class SqlTransactionEvenArgs : EventArgs
    {
        public SqlTransactionEvenArgs( int result, SqlTransaction.ActionSqlTransaction action )
        {
            Result = result;
            Action = action;
        }
        public int Result { get; private set; }
        public SqlTransaction.ActionSqlTransaction Action { get; private set; }
    }
}
