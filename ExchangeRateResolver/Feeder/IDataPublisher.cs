using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Feeder
{
    public interface IDataPublisher : IPublisher
    {
        void StartPublish();
    }
}
