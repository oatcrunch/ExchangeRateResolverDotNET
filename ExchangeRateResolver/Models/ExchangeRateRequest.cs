using ExchangeRateResolver.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Models
{
    public class ExchangeRateRequest : IParsedObject
    {
        public string SourceExchange { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationExchange { get; set; }
        public string DestinationCurrency { get; set; }
        public string OriginalCommand { get; set; }
        public string Key
        {
            get
            {
                var srcExchCurrKey = Utils.GetIdentifier(SourceExchange, SourceCurrency);
                var destExchCurrKey = Utils.GetIdentifier(DestinationExchange, DestinationCurrency);
                return Utils.GetExchangeRequestKey(srcExchCurrKey, destExchCurrKey);
            }
        }
    }
}
