using ExchangeRateResolver.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Parsers
{
    public class ExchangeRateResolverParser : IParser
    {
        private ExchangeRateRequest _data;

        public ExchangeRateResolverParser()
        {

        }

        public object GetLastResult()
        {
            return _data;
        }

        public object Parse(string command)
        {
            var tokens = command?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens?.Length == 5)
            {
                _data = new ExchangeRateRequest();
                _data.OriginalCommand = command;
                _data.SourceExchange = tokens[1].ToUpper();
                _data.SourceCurrency = tokens[2].ToUpper();
                _data.DestinationExchange = tokens[3].ToUpper();
                _data.DestinationCurrency = tokens[4].ToUpper();
            }

            return _data;
        }
    }
}
