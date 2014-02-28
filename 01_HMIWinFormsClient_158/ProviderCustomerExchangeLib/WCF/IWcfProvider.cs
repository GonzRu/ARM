using System;
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
    }
}
