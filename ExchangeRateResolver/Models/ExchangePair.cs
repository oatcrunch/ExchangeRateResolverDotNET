using System;
using System.Collections.Generic;
using System.Text;
using ExchangeRateResolver.Helpers;

namespace ExchangeRateResolver.Models
{
    public class ExchangePair
    {
        public string Exchange { get; private set; }
        public string Currency { get; private set; }
        public string Identifier
        {
            get
            {
                return Utils.GetIdentifier(Exchange, Currency); //Exchange + "-" + Currency;
            }
        }

        public ExchangePair(string exchange, string currency)
        {
            Exchange = exchange;
            Currency = currency;
        }
    }
}
