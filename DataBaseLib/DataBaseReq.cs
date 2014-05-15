/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: представлены различные классы для облегчения работы ADO.Net
 *                                                                             
 *	Файл                     : X:\Projects\33_Virica\Server_new_Interface\crza\CRZADevices\CRZADevices\CRZADeviceEv.cs                                         
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 07.02.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/


using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace DataBaseLib
{
    /// <summary>
    /// делегат для вызова события 
    /// по измененению состояния с БД
    /// </summary>
    /// <param name="state"></param>
    public delegate void BDConnection(bool state);

    /// <summary>
    /// класс представляющий 
    /// параметры команды
    /// </summary>
   public class DataBaseParameter
   {
      public string ParameterName;
      public SqlDbType SqlDbType;
      public object Value;
      public ParameterDirection Direction;

      public DataBaseParameter(string parameterName, ParameterDirection direction, SqlDbType sqlDbType, object value)
      {
         ParameterName = parameterName;
         SqlDbType = sqlDbType;
         Direction = direction;
         Value = value;
      }
   }

   public class DataBaseReq
   {
      List<DataBaseParameter> ListOfCMDParams = new List<DataBaseParameter>();
      SqlDataAdapter aSDA;
      DataSet aDS;
      SqlConnection asqlconnect;
      SqlCommand cmd;

      public DataBaseReq( string strSqlConnection, string cmdname) {
         try
         {
            aSDA = new SqlDataAdapter();
            aDS = new DataSet();

            SetSQLConnection(strSqlConnection/*HMI_Settings.cstr*/);

            CreateStoredProcedure(cmdname);

            ListOfCMDParams = new List<DataBaseParameter>();
         }
         catch (Exception ex)
         {
            throw ex;
         }
      }

      /// <summary>
      /// установить SQLConnection
      /// </summary>
      /// <param name="cstr"></param>
      private void SetSQLConnection(string cstr)
      {
         string errorMes = String.Empty;

         try
         {
            asqlconnect = new SqlConnection( cstr );
            asqlconnect.Open();
         } catch( SqlException ex )
         {            
            errorMes = GetIntegralSQLErrorString(ex);

            CloseConnection();

            throw new Exception(errorMes, ex);
         } catch( Exception ex )
         {
            errorMes = "Нет связи с Сервером" + Environment.NewLine + ex.Message;
            MessageBox.Show( errorMes, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information );

            CloseConnection();

            throw new Exception(errorMes, ex);
         }
      }

      private void CreateStoredProcedure(string cmdname)
      {
         cmd = new SqlCommand(cmdname, asqlconnect);
         cmd.CommandType = CommandType.StoredProcedure;
      }
      /// <summary>
      /// добавить параметр с списку
      /// </summary>
      /// <param name="newparams"></param>
      public void AddCMDParams(DataBaseParameter newparams)
      {
            try
			{
                ListOfCMDParams.Add(newparams);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }
      /// <summary>
      /// выполнить хранимую процедуру
      /// без возврата таблицы
      /// </summary>
      private void DoStoredProcedure() {
         string errorMes = String.Empty;
            try
			{
                SqlParameter[] sqp = new SqlParameter[ListOfCMDParams.Count];

                for (int i = 0; i < ListOfCMDParams.Count; i++)
                {
                    SqlParameter sqpar = new SqlParameter();
                    sqpar.ParameterName = (ListOfCMDParams[i] as DataBaseParameter).ParameterName;
                    sqpar.SqlDbType = (ListOfCMDParams[i] as DataBaseParameter).SqlDbType;
                    sqpar.Value = (ListOfCMDParams[i] as DataBaseParameter).Value;
                    sqpar.Direction = (ListOfCMDParams[i] as DataBaseParameter).Direction;

                    cmd.Parameters.Add(sqpar);
                }

                SqlDataAdapter aSDA = new SqlDataAdapter();
                aSDA.SelectCommand = cmd;

                try
                {
                    aSDA.Fill(aDS);
                }
                catch (SqlException ex)
                {
                    errorMes = GetIntegralSQLErrorString(ex);

                    CloseConnection();

                    throw new Exception(errorMes, ex);
                }

                CloseConnection();
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
                throw;
			}
      }

      /// <summary>
      /// выполнить хранимую процедуру
      /// и получить таблицу в качестве результата
      /// </summary>
      /// <returns></returns>
      public DataTable GetTableAsResultCMD()
      {
          string errorMes = String.Empty; 
          
          try
            {
                SqlParameter[] sqp = new SqlParameter[ListOfCMDParams.Count];

                for (int i = 0; i < ListOfCMDParams.Count; i++)
                {
                    SqlParameter sqpar = new SqlParameter();
                    sqpar.ParameterName = (ListOfCMDParams[i] as DataBaseParameter).ParameterName;
                    sqpar.SqlDbType = (ListOfCMDParams[i] as DataBaseParameter).SqlDbType;
                    sqpar.Value = (ListOfCMDParams[i] as DataBaseParameter).Value;
                    sqpar.Direction = (ListOfCMDParams[i] as DataBaseParameter).Direction;

                    cmd.Parameters.Add(sqpar);
                }

                SqlDataAdapter aSDA = new SqlDataAdapter();
                aSDA.SelectCommand = cmd;

                try
                {
                    aSDA.Fill(aDS);
                }
                catch (SqlException ex)
                {
                    errorMes = GetIntegralSQLErrorString(ex);

                    CloseConnection();

                    throw new Exception(errorMes, ex);
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
          CloseConnection();
          return aDS.Tables[0];
      }

      public void CloseConnection() {
         if (asqlconnect != null)
            asqlconnect.Close();
         if (aSDA != null)
            aSDA.Dispose();
         if (aDS != null)
            aDS.Dispose();
      }

      private string GetIntegralSQLErrorString(SqlException ex)
      {
         string errorMes = String.Empty;

         // интеграция всех возвращаемых ошибок
         foreach (SqlError connectError in ex.Errors)
            errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ")" + Environment.NewLine;

         System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " :(58) DataBaseReq : Ошибка работы с БД : " + errorMes);
         return errorMes;
      }

      /// <summary>
      /// получение блока данных конкретной диаграммы или осциллограммы, блока аварий или уставок из базы данных
      /// </summary>
      public static byte[] GetBlockData(string strconnect, int idrec)
      {
          // получение строк соединения и поставщика данных из файла *.config
          //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
          SqlConnection asqlconnect = new SqlConnection(strconnect);
          try
          {
              asqlconnect.Open();
          }
          catch (SqlException ex)
          {
              string errorMes = "";
              // интеграция всех возвращаемых ошибок
              foreach (SqlError connectError in ex.Errors)
                  errorMes += connectError.Message + " (ощибка: " + connectError.Number.ToString() + ")" + Environment.NewLine;
              //parent.WriteEventToLog(21, "Нет связи с БД (DiagBD): " + errorMes, this.Name, false, true, false); // событие нет связи с БД
              System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " : frmBMRZ : Нет связи с БД (DiagBD)");
              asqlconnect.Close();
              return null;
          }
          catch (Exception ex)
          {
              MessageBox.Show("Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
              asqlconnect.Close();
              return null;
          }
          // формирование данных для вызова хранимой процедуры
          SqlCommand cmd = new SqlCommand("Get_DataLog", asqlconnect);
          cmd.CommandType = CommandType.StoredProcedure;

          // входные параметры
          // 1. ид записи журнала
          SqlParameter pid = new SqlParameter();
          pid.ParameterName = "@Id";
          pid.SqlDbType = SqlDbType.Int;
          pid.Value = idrec;
          pid.Direction = ParameterDirection.Input;
          cmd.Parameters.Add(pid);

          // заполнение DataSet
          DataSet aDS = new DataSet("ptk");
          SqlDataAdapter aSDA = new SqlDataAdapter();
          aSDA.SelectCommand = cmd;

          //aSDA.sq
          try
          {
              aSDA.Fill(aDS, "TbBlockData");//TbAlarm
          }
          catch (SqlException sex)
          {
              MessageBox.Show("Данные из БД недоступны.\nОшибка:" + sex.Message + "\nПовторите запрос.", "Ошибка доступа к базе данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
              asqlconnect.Close();
              aSDA.Dispose();
              aDS.Dispose();
              return null;
          }

          asqlconnect.Close();

          // извлекаем данные 
          DataTable dtBData = aDS.Tables["TbBlockData"];
          // по ide найти запись в dto, извлечь блок с осциллограммой (диаграммой), записать в файл, запустить fastview
          // перечисляем записи в dto

          byte[] arr0 = (byte[])dtBData.Rows[0]["Data"];

          aSDA.Dispose();
          aDS.Dispose();
          return arr0;
      }
   }
}
