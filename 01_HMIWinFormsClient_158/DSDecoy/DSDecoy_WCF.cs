/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс, реализующий интерфейс IDSDecoy для работы с конфигурацией DataServer
 *		по протоколу WCF
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\DSDecoy\DSDecoy_WCF.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 11.07.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using HMI_MT_Settings;
using System.Windows.Forms;

namespace DSDecoy
{
	public class DSDecoy_WCF : IDSDecoy
	{
		#region События
		#endregion

		#region Свойства
		#endregion

		#region public
		#endregion

		#region private
		/// <summary>
		/// wcf - доступ
		/// </summary>
		IWcfDataServer WCFproxy;
		/// <summary>
		/// фабрика каналов
		/// </summary>
		ChannelFactory<IWcfDataServer> chfact;
		#endregion

		#region конструктор(ы)
		public DSDecoy_WCF()
		{
			try
			{
				// инициализация wcf
                BasicHttpBinding bhttp = new BasicHttpBinding();
                bhttp.MaxReceivedMessageSize = 1500000;
                bhttp.MaxBufferSize = 1500000;
                bhttp.ReaderQuotas.MaxArrayLength = 1500000;
                string ipds = string.Empty;

                //if (HMI_MT_Settings.HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("Dataserver").Attributes("ip").Count() != 0)
                //    ipds = HMI_MT_Settings.HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("Dataserver").Attribute("ip").Value;
                string path = AppDomain.CurrentDomain.BaseDirectory;

                XDocument XDoc4PathToPrjFile = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project.cfg");

                if (XDoc4PathToPrjFile.Element("Project").Element("Dataserver").Attributes("ip").Count() != 0)//HMI_MT_Settings.HMI_Settings.
                    ipds = XDoc4PathToPrjFile.Element("Project").Element("Dataserver").Attribute("ip").Value;//HMI_MT_Settings.HMI_Settings.
                else
                    ipds = "127.0.0.1";         

                chfact = new ChannelFactory<IWcfDataServer>(bhttp, new EndpointAddress(string.Format("http://{0}:8732/WcfDataServer",ipds) )); //localhost

				WCFproxy = chfact.CreateChannel();
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}
        public DSDecoy_WCF(string ipds)
        {
            try
            {
                // инициализация wcf
                BasicHttpBinding bhttp = new BasicHttpBinding();
                bhttp.MaxReceivedMessageSize = 1500000;
                bhttp.MaxBufferSize = 1500000;
                bhttp.ReaderQuotas.MaxArrayLength = 1500000;

                IPAddress ipa;

                if (!IPAddress.TryParse(ipds,out ipa))
                    MessageBox.Show(string.Format("Некорректный ip-адрес - {0}", ipds), @"(103 : ...\00_DataServer\DSDecoy\DSDecoy_WCF.cs : DSDecoy_WCF()", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                chfact = new ChannelFactory<IWcfDataServer>(bhttp, new EndpointAddress(string.Format("http://{0}:8732/WcfDataServer", ipds))); //localhost

                WCFproxy = chfact.CreateChannel();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
		#endregion

		#region public-методы
		/// <summary>
		/// предоставление клиенту значений тегов
		/// по запросу формата:
		/// devguid.gr.reg.bitmask#devguid.gr.reg.bitmask# ...
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <summary>
		/// получить данные с DataServer в виде строки
		/// </summary>
		/// <param name="str">строка запроса формата: devguid.gr.reg.bitmask#devguid.gr.reg.bitmask# ...</param>
		/// <returns></returns>
		public string GetDataServerDataAsString(string str)
		{
			byte[] buffer = WCFproxy.GetDataServerData(str);
			string rez = string.Empty;

			return rez;
		}
		public MemoryStream GetDSValueAsByteBuffer(byte[] arr)
		{			
            try
			{
                MemoryStream ms = WCFproxy.GetDSValueAsByteBuffer(arr);
				return ms;
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
				return null;
			}
		}

		public void RunCMD(byte numksdu, ushort numvtu, int tagguid, byte[] arr)
		{
			WCFproxy.RunCMD(numksdu, numvtu, tagguid, arr);
		}

		/// <summary>
		/// закрытие соединения
		/// </summary>
		public void CloseExchChannel()
		{
			chfact.Abort();//.Close();
		}
		#endregion

		#region private-методы
		#endregion
	}
}
