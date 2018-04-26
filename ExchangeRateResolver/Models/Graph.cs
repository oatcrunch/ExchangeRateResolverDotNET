using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Models
{
    public class Graph<T,K>
    {
        public IList<Edge<T, K>> Edges { get; private set; }

        public Graph()
        {
            Edges = new List<Edge<T, K>>();
        }
    }
}
