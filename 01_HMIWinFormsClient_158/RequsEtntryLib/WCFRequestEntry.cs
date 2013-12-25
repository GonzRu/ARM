﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSRouter;
using InterfaceLibrary;
using System.Collections;
using BlockDataComposer;
using ProviderCustomerExchangeLib;

namespace RequsEtntryLib
{
    public class WCFRequestEntry : IRequestEntry
    {
        #region События
        /// <summary>
        /// событие изменения 
        /// контекста тегов для подписки
        /// </summary>
        public event ChangeRequestTags OnChangeRequestTags;

        /// <summary>
        /// событие изменения значения тега
        /// </summary>
        public event Action<UInt16, UInt32, UInt32, byte [], DateTime, VarQualityNewDs> OnTagValueChanged; 
		#endregion

		#region private
		/// <summary>
		/// таблица в кот хранятся ид акт тегов 
		/// и счетчики их использования в элементах
		/// интерфейса
		/// </summary>
		Hashtable htReqList = new Hashtable();

		/// <summary>
		/// ссылка на callback,
		/// wcf провайдера
		/// </summary>
        ClientServerOnWCF _wcfProvider;
		#endregion

		#region конструктор(ы)
        public WCFRequestEntry(ClientServerOnWCF wcfProvider)
		{
            _wcfProvider = wcfProvider;

            _wcfProvider.Callback.OnNewTagValues += NewTagValueHandler;
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
			uint count;
		    bool isChangedTagsList = false;

			try
			{
				foreach (ITag tag in lstTags)
				{
					string st = GetStIdByTag(tag);

                    if (htReqList.ContainsKey(st))
                    {
                        count = Convert.ToUInt32(htReqList[st]);// (uint);
                        htReqList[st] = count + 1;
                    }
                    else
                    {
                        htReqList.Add(st, (uint) 1);
                        isChangedTagsList = true;
                    }
				}

                if (isChangedTagsList)
                {
                    PrepareAndSubscribeTags(htReqList);

                    // извечаем об изменении контекста подписки
                    if (OnChangeRequestTags != null)
                        OnChangeRequestTags(lstTags.Count, htReqList);
                }
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
                    string st = GetStIdByTag(tag);

                    if (htReqList.ContainsKey(st))
                    {
                        count = Convert.ToUInt32(htReqList[st]);
                        htReqList[st] = count + 1;
                    }
                    else
                    {
                        htReqList.Add(st, (uint) 1);
                        PrepareAndSubscribeTags(htReqList);

                        // извечаем об изменении контекста подписки
                        if (OnChangeRequestTags != null)
                            OnChangeRequestTags(1, htReqList);
                    }
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
            var tagListToUnsubscribe = new List<string>();
	
			try
			{
				foreach (ITag tag in lstTags)
				{
					string st = GetStIdByTag(tag);

					if (htReqList.ContainsKey(st))
					{
						count = (uint)htReqList[st];
                        count--;
						htReqList[st] = count;
                        if (count == 0)
                        {
                            htReqList.Remove(st);
                            tagListToUnsubscribe.Add(st);
                        }
					}
				}
                
                if (tagListToUnsubscribe.Count != 0)
                {
                    HMI_MT_Settings.HMI_Settings.WCFproxy.UnscribeRTUTags(tagListToUnsubscribe.ToArray());

                    // извечаем об изменении контекста подписки
                    if (OnChangeRequestTags != null)
                        OnChangeRequestTags(-lstTags.Count, htReqList);
                }

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

                        if (htReqList.ContainsKey(st))
                        {
                            count = (uint)htReqList[st];
                            count--;
                            htReqList[st] = count;
                            if (count == 0)
                            {
                                htReqList.Remove(st);

                                HMI_MT_Settings.HMI_Settings.WCFproxy.UnscribeRTUTags(new string[1] {st});

                                // извечаем об изменении контекста подписки
                                if (OnChangeRequestTags != null)
                                    OnChangeRequestTags(-1, htReqList);
                            }
                        }
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
            //BCD.FormAndSaveSpecificBankReqPacket(htReqList);
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

        /// <summary>
        /// Обработчик события в Callback при появлении нового значения
        /// </summary>
        private void NewTagValueHandler(Dictionary<string, DSTagValue> tv)
        {
            /* 
            Console.WriteLine("Порция данных");
            foreach (KeyValuePair<string, DSRouter.DSTagValue> kvp in tv)
                if (kvp.Value.VarValueAsObject == null)
                    Console.WriteLine(string.Format("{0} : {1}", kvp.Key, "null"));
                else
                    Console.WriteLine(string.Format("{0} : {1}", kvp.Key, kvp.Value.VarValueAsObject.ToString()));
             */

            foreach (var tag in tv)
            {
                string key = tag.Key.ToString();
                var split = key.Split('.');

                try
                {
                    UInt16 dsGuid = UInt16.Parse(split[0]);
                    UInt32 devGuid = UInt32.Parse(split[1]);
                    UInt32 tagGuid = UInt32.Parse(split[2]);

                    // Перевод значения тега в byte []                    
                    if (tag.Value.VarValueAsObject == null)
                        continue;

                    string strTagValue = tag.Value.VarValueAsObject.ToString();

                    byte[] byteArrTagValue;
                    if (strTagValue == "True" || strTagValue == "False")
                        byteArrTagValue = BitConverter.GetBytes(Boolean.Parse(strTagValue));
                    else
                        byteArrTagValue = BitConverter.GetBytes(Single.Parse(strTagValue));

                    if (OnTagValueChanged != null)
                        OnTagValueChanged(dsGuid, devGuid, tagGuid, byteArrTagValue, DateTime.Now, VarQualityNewDs.vqGood);
                }
                catch
                {
                    Console.WriteLine("NewTagValueHandler: Ошибка при разборе нового значения тега: " + key);
                }
            }
        }

        /// <summary>
        /// Создает массив строковых представлений тегов и вызывает метод wcf
        /// </summary>
        private void PrepareAndSubscribeTags(Hashtable tagsHashTable)
        {
            var tagsList = new List<string>();

            foreach (var tag in tagsHashTable.Keys)
                tagsList.Add(tag.ToString());

            HMI_MT_Settings.HMI_Settings.WCFproxy.SubscribeRTUTags(tagsList.ToArray());
        }
        #endregion
	}
}
