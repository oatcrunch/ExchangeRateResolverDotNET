using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Feeder
{
    public interface IDataFeeder<T>
    {
        T GetData();
        bool HasData();
        bool IsAlive();
    }
}
