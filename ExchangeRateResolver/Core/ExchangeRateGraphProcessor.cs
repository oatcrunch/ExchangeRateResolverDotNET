using ExchangeRateResolver.Data;
using ExchangeRateResolver.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Core
{
    public class ExchangeRateGraphProcessor : Processor
    {
        private ExchangeRateGraphManager _exchangeRateGraphManager;

        public ExchangeRateGraphProcessor(DataContext dataContext)
        {
            _exchangeRateGraphManager = new ExchangeRateGraphManager(dataContext);
        }

        public override void Process(object work)
        {
            if (work is PriceUpdate)
            {
                _exchangeRateGraphManager.UpdateGraph(work as PriceUpdate);
            }
            else if (work is ExchangeRateRequest)
            {
                _exchangeRateGraphManager.ComputeBestRates(work as ExchangeRateRequest);
            }
        }
    }
}
