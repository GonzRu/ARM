/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс для контроля связи с БД в приложении
 *                                                                             
 *	Файл                     : X:\Projects\01_HMIWinFormsClient\DataBaseLib\DBConnectionControl.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 14.02.2012 
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
using System.ComponentModel;

namespace DataBaseLib
{
    public class DBConnectionControl
    {
        /// <summary>
        /// событие для оповещения изменения 
        /// связи с БД
        /// </summary>
        public event BDConnection OnBDConnection;
        /// <summary>
        /// таймер проверки соединения с БД
        /// </summary>
        System.Timers.Timer tmrBDConnection;
        /// <summary>
        /// выполнение тестового запроса 
        /// на соед с бд в отд потоке
        /// </summary>
        BackgroundWorker bgw = new BackgroundWorker();
        string strSqlConnection = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public DBConnectionControl(string strSqlConnection)
        {
            this.strSqlConnection = strSqlConnection;

            tmrBDConnection = new System.Timers.Timer();
            tmrBDConnection.Interval = 120000;
            tmrBDConnection.Elapsed += new System.Timers.ElapsedEventHandler(tmrBDConnection_Elapsed);
            tmrBDConnection.Stop();

            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
        }

        /// <summary>
        /// запуск процесса контроля соединения с БД
        /// </summary>
        public void StartControlConnection2BD()
        {
            tmrBDConnection.Start();
        }

        /// <summary>
        /// останов процесса контроля соединения с БД
        /// </summary>
        public void StopControlConnection2BD()
        {
            tmrBDConnection.Stop();
        }

        /// <summary>
        /// установить интервал контроля (ms)
        /// </summary>
        /// <param name="interval"></param>
        public void SetInterval( double interval)
        {
            try
            {
                tmrBDConnection.Interval = interval;
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// запуск тестового запроса к БД
        /// в отд потоке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            tmrBDConnection.Stop();
            bool isConnection = true;
            SqlConnection asqlconnect = null;

            try
            {
                asqlconnect = new SqlConnection(strSqlConnection);
                asqlconnect.Open();
                string sv = asqlconnect.ServerVersion;
                ConnectionState cs = asqlconnect.State;

                DataBaseReq dbs = new DataBaseReq(strSqlConnection, "UserAction~Show");

                // запоминаем в StringDictionary
                DataTable dt = new DataTable();
                    dt = dbs.GetTableAsResultCMD();

                dbs.CloseConnection();
            }
            catch
            {
                isConnection = false;
            }

            ChangeStateConnectionWithBD(isConnection);

            if (asqlconnect != null)
                asqlconnect.Close();

            tmrBDConnection.Start();
        }
        

        void tmrBDConnection_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!bgw.IsBusy)
                bgw.RunWorkerAsync();
        }

        /// <summary>
        /// изменение состояние связи с БД
        /// </summary>
        /// <param name="state"></param>
        /// <param name="errorMes"></param>
        private void ChangeStateConnectionWithBD(bool state)
        {
            try
            {
                if (OnBDConnection != null)
                    OnBDConnection(state);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
    }
}
