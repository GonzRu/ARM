using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProviderCustomerExchangeLib.WCF
{
    public interface IWcfProvider
    {
        void SubscribeRTUTags(string[] tagsArray);

        void SubscribeRTUTag(string tag);

        void UnscribeRTUTags(string[] tagsArray);

        void UnscribeRTUTag(string tag);
    }
}
