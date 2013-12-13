/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс общего кода для
 *	            функционирования форм
 *                                                                             
 *	Файл                     : X:\Projects\01_HMIWinFormsClient\DeviceFormLib\DeviceFormPublicCode.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 17.02.2012
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator;

using InterfaceLibrary;

using LabelTextbox;
using TraceSourceLib;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
namespace DeviceFormLib
{
    public enum UserCntrlRWAccess 
    {
        ReadWrite,
        ReadOnly
    }
    public static class DeviceFormPublicCode
    {
        /// <summary>
        /// Создание элементов 
        /// на FlowLayoutPanel 
        /// (для старых форм - БМРЗ)
        /// </summary>
        /// <param name="arrSign"></param>
        /// <param name="slFLP"></param>
        public static void CreateLinkTag2InterfaceElements(ArrayList arrSign, SortedList slFLP, UserCntrlRWAccess rwAccess, bool isClickable)
        {
			try
			{
                // размещаем динамически на форме
                for (int i = 0; i < arrSign.Count; i++)
                {
                    FormulaEvalNDS ev = (FormulaEvalNDS)arrSign[i];
                    // смотрим категорию вкладки для размещения тега и его тип

                    // контроллируем привязку
                    if (ev.LinkVariableNewDS == null)
                    {
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 86, string.Format("{0} : {1} : {2} : Попытка привязки к несуществующему тегу : устройство = {3}, TagGUID = {4}.", DateTime.Now.ToString(), "MessageHandler.cs", "PacketParse()", ev.LinkVariableNewDS.Device.UniObjectGUID.ToString(), ev.LinkVariableNewDS.TagGUID.ToString()));
                        continue;
                    }

                    switch (ev.ToT)
                    {
                        case TypeOfTag.Combo:
                            ComboBoxVar cBV = new ComboBoxVar(ev.LinkVariableNewDS.SlEnumsParty, 0, ev);
                            cBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                            cBV.AutoSize = true;
                            cBV.TypeView = TypeViewValue.Textbox;//.Combobox;

                            Binding bndcb = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bndcb.Format += new ConvertEventHandler(cBV.bnd_Format);
                            cBV.tbText.DataBindings.Add(bndcb);
                            ev.LinkVariableNewDS.BindindTag = bndcb;
                            cBV.lblCaption.Text = ev.CaptionFE;
                            break;
                        case TypeOfTag.String:
                        case TypeOfTag.Analog:
                            ctlLabelTextbox usTB = new ctlLabelTextbox(ev);
                            usTB.LabelText = "";
                            usTB.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                            usTB.AutoSize = true;

                            Binding bnd = new Binding("Text", ev.LinkVariableNewDS, "ValueAsString", true);
                            bnd.Format += new ConvertEventHandler(usTB.bnd_Format);
                            bnd.Parse += new ConvertEventHandler(usTB.bnd_Parse);
                            usTB.txtLabelText.DataBindings.Add(bnd);
                            try
                            {
                                ev.LinkVariableNewDS.BindindTag = bnd;
                            }
                            catch (Exception ex)
                            {
                                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
                            }
                            usTB.Caption_Text = ev.CaptionFE;
                            usTB.Dim_Text = ev.Dim;
                            usTB.txtLabelText.ReadOnly = (rwAccess == UserCntrlRWAccess.ReadOnly) ? true : false;
                            break;
                        case TypeOfTag.Discret:
                            CheckBoxVar chBV = new CheckBoxVar(ev);
                            chBV.IsClickable = isClickable;
                            chBV.CheckBox_Text = "";
                            chBV.Parent = (FlowLayoutPanel)slFLP[ev.ToP];
                            chBV.AutoSize = true;

                            Binding bndCB = new Binding("Checked", ev.LinkVariableNewDS, "ValueAsString", true);
                            bndCB.Format += new ConvertEventHandler(chBV.bnd_Format);
                            // bnd_parse - смысла нет т.к. поверху checkbox =:> новое значение задается в реакции на click
                            bndCB.Parse += new ConvertEventHandler(chBV.bnd_Parse); 
                            chBV.checkBox1.DataBindings.Add(bndCB);
                            ev.LinkVariableNewDS.BindindTag = bndCB;
                            chBV.CheckBox_Text = ev.CaptionFE;
                            break;
                        case TypeOfTag.NaN:
                            break;
                        default:
                            MessageBox.Show("Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }

        /// <summary>
        /// заполнить вкладку с информацией о состоянии устройства
        /// </summary>
        /// <param name="PanelInfoTextBox"></param>
        /// <param name="rtbInfo"></param>
        public static void FillTAbPageInfo(TextBox PanelInfoTextBox, RichTextBox rtbInfo, uint unidev)
        {
            SqlConnection asqlconnect = new SqlConnection( HMI_MT_Settings.HMI_Settings.ProviderPtkSql);
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

                CommonUtils.CommonUtils.WriteEventToLog(21, "Нет связи с БД (UstavBD): " + errorMes,false );//"выдана команда WCP - запись уставок."

                System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " : frmBMRZ : Нет связи с БД (UstavBD)");
                asqlconnect.Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Нет связи с Сервером" + Environment.NewLine + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                asqlconnect.Close();
                return;
            }
            // формирование данных для вызова хранимой процедуры
            SqlCommand cmd = new SqlCommand("GetBlockInfo", asqlconnect);
            cmd.CommandType = CommandType.StoredProcedure;

            // входные параметры
            // id устройства
            SqlParameter pidBlock = new SqlParameter();
            pidBlock.ParameterName = "@BlockId";
            pidBlock.SqlDbType = SqlDbType.Int;
            pidBlock.Value = unidev;
            pidBlock.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(pidBlock);

            // заполнение DataSet
            DataSet aDS = new DataSet("ptk");
            SqlDataAdapter aSDA = new SqlDataAdapter();
            aSDA.SelectCommand = cmd;

            //aSDA.sq
            aSDA.Fill(aDS, "TbInfo");

            asqlconnect.Close();

            // извлекаем данные
            DataTable dtI = aDS.Tables["TbInfo"];

            // заполняем RichTextBox
            for (int curRow = 0; curRow < dtI.Rows.Count; curRow++)
            {
                DateTime t = (DateTime)dtI.Rows[curRow]["DateTime_Modify"];
                PanelInfoTextBox.Text = PanelInfoTextBox.Text + CommonUtils.CommonUtils.GetTimeInMTRACustomFormat(t);
                byte[] arrInfo = (byte[])dtI.Rows[curRow]["Data"];

                System.Text.UTF8Encoding utf = new UTF8Encoding();
                string str = utf.GetString(arrInfo);

                rtbInfo.AppendText(utf.GetString(arrInfo));
            }
            aSDA.Dispose();
            aDS.Dispose();
        }
    }
}
