using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Models.Events
{
    public class PriceUpdateEvent : EventArgs
    {
        public PriceUpdate Data { get; set; }

        public PriceUpdateEvent(PriceUpdate data)
        {
            this.Data = data;
        }
    }
}
