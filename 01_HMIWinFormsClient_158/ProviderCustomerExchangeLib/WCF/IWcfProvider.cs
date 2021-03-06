﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProviderCustomerExchangeLib.WCF
{
    public interface IWcfProvider
    {
        /// <summary>
        /// События переподключения канала связи с роутером
        /// </summary>
        event Action OnProxyRecreated;

        /// <summary>
        /// Запрос значений тегов
        /// </summary>
        void GetTagsValue(string[] tagsArrayToRequest);

        /// <summary>
        /// Запрос значений тегов, чьи значения изменились
        /// </summary>
        void GetTagsValuesUpdated();

        /// <summary>
        /// Получить ссылку на осциллограмму
        /// </summary>
        string GetOscillogramAsUrlById(UInt16 dsGuid, Int32 oscGuid);

        /// <summary>
        /// Получить содержимое архива с осциллограммами и его имя
        /// </summary>
        Tuple<byte [], string> GetOscillogramAsByteArray(UInt16 dsGuid, Int32 oscGuid);
    }
}
