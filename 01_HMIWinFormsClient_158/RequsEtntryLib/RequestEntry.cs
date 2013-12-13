/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Компонент запросов - создание структур данных для обеспечения работы 
 *				запросного механизма к DataServer
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\ClientWPF\RequestEntry.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 27.10.2011 
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
using InterfaceLibrary;
using BlockDataComposer;

namespace RequsEtntryLib
{
	public class RequestEntry : IRequestEntry
	{
		#region События
        /// <summary>
        /// событие изменения 
        /// контекста тегов для подписки
        /// </summary>
        public event ChangeRequestTags OnChangeRequestTags; 
		#endregion

		#region Свойства
		#endregion

		#region public
		#endregion

		#region private
		/// <summary>
		/// таблица в кот хранятся ид акт тегов 
		/// и счетчики их использования в элементах
		/// интерфейса
		/// </summary>
		Hashtable htReqList = new Hashtable();
		/// <summary>
		/// ссылка на экземпляр формирователя пакетов,
		/// связанный с данным компонентом запросов 
		/// </summary>
		IBlockDataComposer BCD;
		/// <summary>
		/// timer для обновления данных с DataServer
		/// </summary>
		System.Timers.Timer tmpRENewDSData = new System.Timers.Timer();
		#endregion

		#region конструктор(ы)
		public RequestEntry(IBlockDataComposer bcd)
		{
			BCD = bcd;

			tmpRENewDSData.Elapsed += new System.Timers.ElapsedEventHandler(tmpRENewDSData_Elapsed);
			tmpRENewDSData.Interval = 1000;
			tmpRENewDSData.Stop();
		}

		void tmpRENewDSData_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			try
			{
				tmpRENewDSData.Stop();

				if (htReqList.Keys.Count != 0)
					//BCD.FormAndSaveReqPacket(htReqList);
                    BCD.FormAndSaveSpecificBankReqPacket(htReqList);
				tmpRENewDSData.Start();
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

		}
		#endregion

		#region public-методы реализации интерфейсa IRequestEntry
		/// <summary>
		/// подписаться на обновление тегов
		/// </summary>
		/// <param name="?"></param>
        public void SubscribeTags(List<ITag> lstTags)
		{
			/*
			 * элементы списка в формате:
			 * ds.objectGuid.TagGuid
			 * Из этих элементов формир хеш-список в кот
			 * 2-й элемент - счетчик использования - когда он равен 0,
			 * то элемент списка подлежит удалению, о чем сообщается 
			 * серверу вызовом функции UnSubscribeTags()
			 */
			uint count = 0;

			try
			{
				tmpRENewDSData.Stop();

				foreach (ITag tag in lstTags)
				{
					string st = GetStIdByTag(tag);

                    //if (tag.Device.UniObjectGUID != 516 && tag.Device.UniObjectGUID != 769 && tag.Device.UniObjectGUID != 1281 )
                    //    continue;

					if (htReqList.ContainsKey(st))
					{
						count = Convert.ToUInt32(htReqList[st]);// (uint);
						htReqList[st] = count + 1;
					}
					else
						htReqList.Add(st, (uint)1);
				}
                //BCD.FormAndSaveReqPacket(htReqList);
                BCD.FormAndSaveSpecificBankReqPacket(htReqList);

                // извечаем об изменении контекста подписки
                if (OnChangeRequestTags != null)
                    OnChangeRequestTags(lstTags.Count, htReqList);

				tmpRENewDSData.Start();
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}
        /// <summary>
        /// подписаться на обновление тега
        /// </summary>
        /// <param name="?"></param>
        public void SubscribeTag(ITag tag)
        {
            /*
             * элементы :
             * ds.objectGuid.TagGuid
             * Из этого элементов формир хеш-список в кот
             * 2-й элемент - счетчик использования - когда он равен 0,
             * то элемент подлежит удалению, о чем сообщается 
             * серверу вызовом функции UnSubscribeTag()
             */
            uint count = 0;

            try
            {
                tmpRENewDSData.Stop();

                    string st = GetStIdByTag(tag);

                    //if (tag.Device.UniObjectGUID == 516 || tag.Device.UniObjectGUID == 769 || tag.Device.UniObjectGUID == 1281)
                    //{
                        if (htReqList.ContainsKey(st))
                        {
                            count = Convert.ToUInt32(htReqList[st]);
                            htReqList[st] = count + 1;
                        }
                        else
                            htReqList.Add(st, (uint)1);

                        BCD.FormAndSaveSpecificBankReqPacket(htReqList);

                        // извечаем об изменении контекста подписки
                        if (OnChangeRequestTags != null)
                            OnChangeRequestTags(1, htReqList);
                    //}

                tmpRENewDSData.Start();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

		/// <summary>
		/// отписаться от обновления тегов
		/// </summary>
		/// <param name="?"></param>
        public void UnSubscribeTags(List<ITag> lstTags)
		{
			uint count = 0;
	
			try
			{
				foreach (ITag tag in lstTags)
				{
					string st = GetStIdByTag(tag);

                    //if (tag.Device.UniObjectGUID != 516 && tag.Device.UniObjectGUID != 769 && tag.Device.UniObjectGUID != 1281)
                    //    continue;

					if (htReqList.ContainsKey(st))
					{
						count = (uint)htReqList[st];
                        count--;
						htReqList[st] = count;
						if (count == 0)
							htReqList.Remove(st);
					}
				}

                // извечаем об изменении контекста подписки
                if (OnChangeRequestTags != null)
                    OnChangeRequestTags(-lstTags.Count, htReqList);

			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}
        /// <summary>
        /// отписаться от обновления тега
        /// </summary>
        /// <param name="?"></param>
        public void UnSubscribeTag(ITag tag)
        {
            uint count = 0;

            try
            {
                    string st = GetStIdByTag(tag);

                    //if (tag.Device.UniObjectGUID == 516 || tag.Device.UniObjectGUID == 769 || tag.Device.UniObjectGUID == 1281)
                    //{
                        if (htReqList.ContainsKey(st))
                        {
                            count = (uint)htReqList[st];
                            count--;
                            htReqList[st] = count;
                            if (count == 0)
                                htReqList.Remove(st);
                        }

                        // извечаем об изменении контекста подписки
                        if (OnChangeRequestTags != null)
                            OnChangeRequestTags(-1, htReqList);
                    //}
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
		/// <summary>
		/// обновить инфо запросив данные по активным тегам
		/// </summary>
		public void UpdateHMIInfo()
		{
            //BCD.FormAndSaveReqPacket(htReqList);
            BCD.FormAndSaveSpecificBankReqPacket(htReqList);
		}
        /// <summary>
        /// получить текущий список
        /// тегов запрашиваемых с сервера
        /// </summary>
        /// <returns></returns>
        public Hashtable GetTagsReqList()
        {
            return htReqList;
        }
		#endregion

		#region public-методы
		#endregion

		#region private-методы
		/// <summary>
		/// сформировать идентификационную строку по опис тега
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		private string GetStIdByTag(ITag tag)
		{
			string rez = string.Empty;

			if (tag != null)
				rez = string.Format("{0}.{1}.{2}", tag.Device.UniDS_GUID.ToString(), tag.Device.UniObjectGUID.ToString(), tag.TagGUID.ToString());

			return rez;
		}
		#endregion
	}
}
