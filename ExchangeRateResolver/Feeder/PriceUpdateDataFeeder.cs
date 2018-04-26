using ExchangeRateResolver.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Feeder
{
    /** This is the class that should be used for production **/
    public class PriceUpdateDataFeeder : IDataFeeder<PriceUpdate>
    {
        public PriceUpdateDataFeeder(string url)
        {

        }

        public PriceUpdate GetData()
        {
            return null;
        }

        public bool HasData()
        {
            return false;
        }

        public bool IsAlive()
        {
            return false;
        }
    }
}
