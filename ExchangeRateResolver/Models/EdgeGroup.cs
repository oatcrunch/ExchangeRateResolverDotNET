using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Models
{
    public class EdgeGroup
    {
        public string Key { get; set; }
        public IEnumerable<Edge<ExchangePair, float>> Groups { get; set; }
        public int Total { get; set; }
    }
}
