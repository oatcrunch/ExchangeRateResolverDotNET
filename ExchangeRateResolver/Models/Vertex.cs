using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Models
{
    public  class Vertex<T>
    {
        public T Value { get; private set; }

        public Vertex(T t)
        {
            Value = t;
        }
    }
}
