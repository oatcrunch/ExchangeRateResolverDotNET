using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Models
{
    public class Weight<T>
    {
        public T Value { get; private set; }

        public Weight(T t)
        {
            Value = t;
        }
    }
}
