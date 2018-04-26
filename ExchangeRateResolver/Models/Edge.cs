using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Models
{
    public class Edge<T, K>
    {
        public Vertex<T> CurrentVertex { get; private set; }
        public Vertex<T> NextVertex { get; private set; }
        public Weight<K> Weight { get; private set; }
        public DateTime ModifiedTime { get; private set; }

        public Edge(Vertex<T> currentVertex, Vertex<T> nextVertex, Weight<K> weight, DateTime timeStamp)
        {
            CurrentVertex = currentVertex;
            NextVertex = nextVertex;
            Weight = weight;
            ModifiedTime = timeStamp;
        }

        public void UpdateCurrentVertex(Vertex<T> newCurrentVertex, DateTime timeStamp)
        {
            CurrentVertex = newCurrentVertex;
            ModifiedTime = timeStamp;
        }

        public void UpdateNextVertex(Vertex<T> newNextVertex, DateTime timeStamp)
        {
            NextVertex = newNextVertex;
            ModifiedTime = timeStamp;
        }

        public void UpdateWeight(Weight<K> newWeight, DateTime timeStamp)
        {
            Weight = newWeight;
            ModifiedTime = timeStamp;
        }
    }
}
