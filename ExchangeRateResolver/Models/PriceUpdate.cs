using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Models
{
    public class PriceUpdate : IParsedObject
    {
        public DateTime TimeStamp { get; set; }
        public string Exchange { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public float ForwardFactor { get; set; }
        public float BackwardFactor { get; set; }
        public string OriginalCommand { get; set; }
    }
}
