using ExchangeRateResolver.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Parsers
{
    public class PriceUpdaterParser : IParser
    {
        private PriceUpdate _data;

        public PriceUpdaterParser()
        {
           
        }

        public object GetLastResult()
        {
            return _data;
        }

        public object Parse(string command)
        {
            var tokens = command?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens?.Length == 6)
            {
                _data = new PriceUpdate();
                _data.OriginalCommand = command;

                DateTime timeStamp;
                if (DateTime.TryParse(tokens[0], out timeStamp))
                {
                    _data.TimeStamp = timeStamp;
                }

                _data.Exchange = tokens[1].ToUpper();
                _data.SourceCurrency = tokens[2].ToUpper();
                _data.DestinationCurrency = tokens[3].ToUpper();

                float forwardFactor;
                if (float.TryParse(tokens[4], out forwardFactor))
                {
                    _data.ForwardFactor = forwardFactor;
                }

                float backwardFactor;
                if (float.TryParse(tokens[5], out backwardFactor))
                {
                    _data.BackwardFactor = backwardFactor;
                }
            }

            return _data;
        }
    }
}
