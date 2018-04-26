using ExchangeRateResolver.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Parsers
{
    public class ParserBuilder
    {
        public IParser GetParser(string type)
        {
            switch (type)
            {
                case Constants.EXCHANGE_RATE_RESOLVER_PARSER:
                    return new ExchangeRateResolverParser();
                case Constants.PRICE_UPDATER_PARSER:
                    return new PriceUpdaterParser();
                default:
                    return null;
            }
        }
    }
}
