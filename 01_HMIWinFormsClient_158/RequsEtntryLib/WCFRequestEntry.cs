using System;
using System.Collections.Generic;
using System.Timers;
using InterfaceLibrary;
using System.Collections;
using ProviderCustomerExchangeLib.WCF;

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
		#endregion

		#region private
		/// <summary>
		/// таблица в кот хранятся ид акт тегов 
		/// и счетчики их использования в элементах
		/// интерфейса
		/// </summary>
		private Hashtable htReqList = new Hashtable();

		/// <summary>
		/// ссылка на callback,
		/// wcf провайдера
		/// </summary>
        private IWcfProvider _wcfProvider;

        private Timer getTagsValuePeriodicTimer = new Timer();
		#endregion

		#region конструктор(ы)
        public WCFRequestEntry(IWcfProvider wcfProvider)
		{
            _wcfProvider = wcfProvider;
            _wcfProvider.OnProxyRecreated += WcfProviderOnOnProxyRecreated;
            WcfProviderOnOnProxyRecreated();

            getTagsValuePeriodicTimer.Elapsed += GetTagsValuePeriodicTimerOnElapsed;
            getTagsValuePeriodicTimer.Interval = 1000;
            getTagsValuePeriodicTimer.Stop();
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

            getTagsValuePeriodicTimer.Stop();

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
                    SubscribeToTags();

                    // извечаем об изменении контекста подписки
                    if (OnChangeRequestTags != null)
                        OnChangeRequestTags(lstTags.Count, htReqList);
                }
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
            finally
			{
                getTagsValuePeriodicTimer.Start();
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

            getTagsValuePeriodicTimer.Stop();

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
                        SubscribeToTags();

                        // извечаем об изменении контекста подписки
                        if (OnChangeRequestTags != null)
                            OnChangeRequestTags(1, htReqList);
                    }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
            finally
            {
                getTagsValuePeriodicTimer.Start();
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

            getTagsValuePeriodicTimer.Stop();
	
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
                    SubscribeToTags();

                    // извечаем об изменении контекста подписки
                    if (OnChangeRequestTags != null)
                        OnChangeRequestTags(-lstTags.Count, htReqList);
                }

			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
            finally
			{
                getTagsValuePeriodicTimer.Start();
			}
		}
        /// <summary>
        /// отписаться от обновления тега
        /// </summary>
        /// <param name="?"></param>
        public void UnSubscribeTag(ITag tag)
        {
            uint count = 0;

            getTagsValuePeriodicTimer.Stop();

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

                                SubscribeToTags();

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
            finally
            {
                getTagsValuePeriodicTimer.Start();
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
        /// Convert HastTable to string array
        /// </summary>
        private string[] HashTableToArray(Hashtable tagsHashTable)
        {
            var tagsList = new List<string>();

            foreach (var tag in tagsHashTable.Keys)
                tagsList.Add(tag.ToString());

            return tagsList.ToArray();
        }

        /// <summary>
        /// Periodic request tags value
        /// </summary>
        private void GetTagsValuePeriodicTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                getTagsValuePeriodicTimer.Stop();

                if (htReqList.Keys.Count != 0)
                    _wcfProvider.GetTagsValuesUpdated();

                getTagsValuePeriodicTimer.Start();
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// В случае разрыва соединения - заново подписывается на нужные теги
        /// </summary>
        private void WcfProviderOnOnProxyRecreated()
        {
            SubscribeToTags();
        }

        private void SubscribeToTags()
        {
            _wcfProvider.GetTagsValue(HashTableToArray(htReqList));
        }
        #endregion
	}
}
