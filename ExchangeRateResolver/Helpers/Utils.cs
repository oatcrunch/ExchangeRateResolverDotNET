using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Helpers
{
    public class Utils
    {
        public static string GetIdentifier(string exchange, string currency)
        {
            return exchange.ToUpper() + "-" + currency.ToUpper();
        }

        public static string GetExchangeRequestKey(string srcExchCurrKey, string destExchCurrKey)
        {
            return srcExchCurrKey.ToUpper() + ":" + destExchCurrKey.ToUpper();
        }
    }
}
