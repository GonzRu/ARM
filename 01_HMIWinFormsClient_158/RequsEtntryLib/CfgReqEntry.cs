/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс диспетчеризации запросов (на уровне конфигурации ПТК).
 *	            Его задача - принимать запросы клиента на подписку\отписку тегов и направлять их
 *	            компоненту запросов целевого DataServer.
 *                                                                             
 *	Файл                     : X:\Projects\39_ViricaClient4NewDataServer\99_MTRAhmi\Client\RequsEtntryLib\CfgReqEntry.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 15.11.2011 
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
using HMI_MT_Settings;

namespace RequsEtntryLib
{
    public class CfgReqEntry : ICfgReqEntry
    {
        #region События
		#endregion

		#region Свойства
		#endregion

		#region public
		#endregion

		#region private
		#endregion

		#region конструктор(ы)
		#endregion

        #region public-методы реализации интерфейса ICfgReqEntry
        /// <summary>
        /// подписаться на обновление тегов
        /// </summary>
        /// <param name="?"></param>
        public void SubscribeTags(List<ITag> lstTags)
        {
            /*
             * просмотреть строки запросов на теги
             * рассортировать их в списки по DataServer'ам
             * и передать их на обработку соответствующим DataServer'ам
             */

             SortedList<uint,List<ITag>> slTagByDSs = new SortedList<uint,List<ITag>>();
             ITag tagtest = null;
			try
			{
                 foreach( ITag itag in lstTags )
                 { 
                    tagtest = itag;

                     if (!slTagByDSs.Keys.Contains(itag.Device.UniDS_GUID))                 
                     try
			        {

                        slTagByDSs.Add(itag.Device.UniDS_GUID, new List<ITag>());
                    }
			        catch(Exception ex)
			        {
				        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			        }

                     slTagByDSs[itag.Device.UniDS_GUID].Add(itag);
                 }

                 foreach( KeyValuePair<uint,List<ITag>> kvp in slTagByDSs )
                     HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServer(kvp.Key).ReqEntry.SubscribeTags(kvp.Value);  
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
             * просмотреть строку запроса на тег
             * и передать ее на обработку соответствующим DataServer'у
             */

            try
            {
                    HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServer(tag.Device.UniDS_GUID).ReqEntry.SubscribeTag(tag);
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
            /*
             * просмотреть строки запросов на теги
             * рассортировать их в списки по DataServer'ам
             * и передать их на обработку соответствующим DataServer'ам
             */
            SortedList<uint, List<ITag>> slTagByDSs = new SortedList<uint, List<ITag>>();
            try
            {
                foreach (ITag itag in lstTags)
                {
                    if (!slTagByDSs.Keys.Contains(itag.Device.UniDS_GUID))
                        slTagByDSs.Add(itag.Device.UniDS_GUID, new List<ITag>());

                    slTagByDSs[itag.Device.UniDS_GUID].Add(itag);
                }

                foreach (KeyValuePair<uint, List<ITag>> kvp in slTagByDSs)
                    HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServer(kvp.Key).ReqEntry.UnSubscribeTags(kvp.Value);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// отписаться от обновления тега
        /// </summary>
        /// <param name="?"></param>
        public void UnSubscribeTag(ITag tag)
        { 
            /*
             * просмотреть строку запроса на тег
             * и передать его на обработку соответствующим DataServer'ам
             */

            try
            {
                   HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServer(tag.Device.UniDS_GUID).ReqEntry.UnSubscribeTag(tag);
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
        }
        #endregion

        #region public-методы
        #endregion

        #region private-методы
        #endregion
    }
}
