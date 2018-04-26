using ExchangeRateResolver.Data;
using ExchangeRateResolver.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ExchangeRateResolver.Helpers;

namespace ExchangeRateResolver.Core
{
    public class ExchangeRateGraphManager
    {
        private DataContext _dataContext;
        private Graph<ExchangePair, float> _exchangeRateGraph;

        public ExchangeRateGraphManager(DataContext dataContext)
        {
            _dataContext = dataContext;
            _exchangeRateGraph = new Graph<ExchangePair, float>();
        }

        public void UpdateGraph(PriceUpdate priceUpdate)
        {
            /** Current exchange of source currency to destination currency of current exchange **/
            this.Map(new ExchangePair(priceUpdate.Exchange, priceUpdate.SourceCurrency), new ExchangePair(priceUpdate.Exchange, priceUpdate.DestinationCurrency), priceUpdate.TimeStamp, priceUpdate.ForwardFactor);

            /** Current exchange of destination currency to source currency of current exchange **/
            this.Map(new ExchangePair(priceUpdate.Exchange, priceUpdate.DestinationCurrency), new ExchangePair(priceUpdate.Exchange, priceUpdate.SourceCurrency), priceUpdate.TimeStamp, priceUpdate.BackwardFactor);

            /** Current exchange of source currency  **/
            var tempEdges = new List<Edge<ExchangePair, float>>(_exchangeRateGraph.Edges);
            foreach (var edge in tempEdges)
            {
                if (edge.CurrentVertex.Value.Exchange == priceUpdate.Exchange) continue;

                if (ShouldMap(priceUpdate, edge))
                {
                    this.Map(new ExchangePair(priceUpdate.Exchange, priceUpdate.SourceCurrency), new ExchangePair(edge.CurrentVertex.Value.Exchange, edge.CurrentVertex.Value.Currency), priceUpdate.TimeStamp);
                    this.Map(new ExchangePair(edge.CurrentVertex.Value.Exchange, edge.CurrentVertex.Value.Currency), new ExchangePair(priceUpdate.Exchange, priceUpdate.SourceCurrency), priceUpdate.TimeStamp);
                    this.Map(new ExchangePair(priceUpdate.Exchange, priceUpdate.DestinationCurrency), new ExchangePair(edge.NextVertex.Value.Exchange, edge.NextVertex.Value.Currency), priceUpdate.TimeStamp);
                    this.Map(new ExchangePair(edge.NextVertex.Value.Exchange, edge.NextVertex.Value.Currency), new ExchangePair(priceUpdate.Exchange, priceUpdate.DestinationCurrency), priceUpdate.TimeStamp);
                }
            }
        }

        private bool ShouldMap(PriceUpdate priceUpdate, Edge<ExchangePair, float> edge)
        {
            return (priceUpdate.Exchange != edge.CurrentVertex.Value.Exchange && priceUpdate.SourceCurrency == edge.CurrentVertex.Value.Currency)
                                && (priceUpdate.Exchange != edge.NextVertex.Value.Exchange && priceUpdate.DestinationCurrency == edge.NextVertex.Value.Currency);
        }

        private IEnumerable<EdgeGroup> GetGroupedEdgesByCurrentVertex()
        {
            var groupedEdges = from p in _exchangeRateGraph.Edges
                               group p by p.CurrentVertex.Value.Identifier into groups
                               select new EdgeGroup()
                               {
                                   Key = groups.Key,
                                   Groups = groups,
                                   Total = groups.Count()
                               };

            return groupedEdges;
        }

        public void ComputeBestRates(ExchangeRateRequest exchangeRateRequest)
        {
            var groupedEdges = this.GetGroupedEdgesByCurrentVertex();
            var vertextCount = groupedEdges.Count();
            var rates = new float?[vertextCount, vertextCount];
            var next = new int?[vertextCount, vertextCount];
            var V = groupedEdges.Select(p => p.Key).ToList();

            /** Initialize rates and next arrays **/
            InitializeRatesAndNextArrays(vertextCount, rates, next);

            /** Set weights to the rates **/
            SetWeightsToRates(groupedEdges, rates, next, V);

            /** Implement modified Floyd-Warshall algorithm **/
            FloydWarshallAlgorithm(vertextCount, rates, next);

            /** Obtain best rate **/
            string sourceVertexId;
            int sourceVertexIndex, destVertexIndex;
            float? bestRate;
            GetBestRate(exchangeRateRequest, rates, V, out sourceVertexId, out sourceVertexIndex, out destVertexIndex, out bestRate);

            /** Obtain path **/
            List<string> path = GetOptimalPath(next, V, sourceVertexId, ref sourceVertexIndex, destVertexIndex);

            /** Store into data context **/
            string baseRequestKey = exchangeRateRequest.Key;
            string bestRateKey = baseRequestKey + "=>RATE";
            string pathKey = baseRequestKey + "=>PATH";
            _dataContext.AddData(bestRateKey, bestRate);
            _dataContext.AddData(pathKey, path);

            /** Print results **/
            Console.WriteLine();
            Console.WriteLine("{0} {1} {2} {3} {4} {5}", Constants.BEST_RATES_BEGIN, exchangeRateRequest.SourceExchange, exchangeRateRequest.SourceCurrency, exchangeRateRequest.DestinationExchange, exchangeRateRequest.DestinationCurrency, bestRate);
            foreach (var p in path)
            {
                var tokens = p.Split('-');
                Console.WriteLine("{0} {1}", tokens[0], tokens[1]);
            }
            Console.WriteLine(Constants.BEST_RATES_END);
            Console.WriteLine();
        }

        private List<string> GetOptimalPath(int?[,] next, List<string> V, string sourceVertexId, ref int sourceVertexIndex, int destVertexIndex)
        {
            var path = new List<string>();
            path.Add(sourceVertexId);

            int? nextPathIndex = -1;
            int counter = 0;
            while (nextPathIndex != destVertexIndex)
            {
                if (counter > 1000000)
                {
                    throw new Exception("No solution found");
                }
                nextPathIndex = next[sourceVertexIndex, destVertexIndex];
                var nextPath = V[nextPathIndex.Value];

                path.Add(nextPath);
                sourceVertexIndex = nextPathIndex.Value;
                counter++;
            }

            return path;
        }

        private void GetBestRate(ExchangeRateRequest exchangeRateRequest, float?[,] rates, List<string> V, out string sourceVertexId, out int sourceVertexIndex, out int destVertexIndex, out float? bestRate)
        {
            sourceVertexId = Utils.GetIdentifier(exchangeRateRequest.SourceExchange, exchangeRateRequest.SourceCurrency);
            var destVertexId = Utils.GetIdentifier(exchangeRateRequest.DestinationExchange, exchangeRateRequest.DestinationCurrency);
            sourceVertexIndex = V.IndexOf(sourceVertexId);
            destVertexIndex = V.IndexOf(destVertexId);
            bestRate = rates[sourceVertexIndex, destVertexIndex];
        }

        private void FloydWarshallAlgorithm(int vertextCount, float?[,] rates, int?[,] next)
        {
            for (int k = 0; k < vertextCount; k++)
            {
                for (int i = 0; i < vertextCount; i++)
                {
                    for (int j = 0; j < vertextCount; j++)
                    {
                        var multipliedFactor = rates[i, k] * rates[k, j];
                        if (rates[i, j] < multipliedFactor)
                        {
                            rates[i, j] = multipliedFactor;
                            next[i, j] = next[i, k];
                        }
                    }
                }
            }
        }

        private void SetWeightsToRates(IEnumerable<EdgeGroup> groupedEdges, float?[,] rates, int?[,] next, List<string> V)
        {
            foreach (var group in groupedEdges)
            {
                var edges = group.Groups;
                foreach (var edge in edges)
                {
                    var currVertex = edge.CurrentVertex;
                    var nextVertex = edge.NextVertex;
                    var u = V.IndexOf(currVertex.Value.Identifier);
                    var v = V.IndexOf(nextVertex.Value.Identifier);
                    rates[u, v] = edge.Weight.Value;
                    next[u, v] = v;
                }
            }
        }

        private void InitializeRatesAndNextArrays(int vertextCount, float?[,] rates, int?[,] next)
        {
            for (int i = 0; i < vertextCount; i++)
            {
                for (int j = 0; j < vertextCount; j++)
                {
                    rates[i, j] = 0;
                    next[i, j] = null;
                }
            }
        }

        public void Map(ExchangePair sourceExchangePair, ExchangePair destinationExchangePair, DateTime timeStamp, float weight = 1)
        {
            var edge = this.GetEdgeByCurrentExchangeCurrencyPairs(sourceExchangePair, destinationExchangePair);

            if (edge == null)
            {
                //insert new record
                var sourceVertex = new Vertex<ExchangePair>(sourceExchangePair);
                var destinationVertex = new Vertex<ExchangePair>(destinationExchangePair);
                var newWeight = new Weight<float>(weight);
                var newData = new Edge<ExchangePair, float>(sourceVertex, destinationVertex, newWeight, timeStamp);
                _exchangeRateGraph.Edges.Add(newData);
            }
            else
            {
                //otherwise update existing record
                if (edge.ModifiedTime < timeStamp)
                {
                    edge.UpdateWeight(new Weight<float>(weight), timeStamp);
                }
            }
        }

        public Edge<ExchangePair, float> GetEdgeByCurrentExchangeCurrencyPairs(ExchangePair current, ExchangePair next)
        {
            if (current == null || next == null) return null;

            Edge<ExchangePair, float> result = null;

            result = _exchangeRateGraph.Edges
                .Where(p => p.CurrentVertex.Value.Identifier == current.Identifier
                    && p.NextVertex.Value.Identifier == next.Identifier)
                    .FirstOrDefault();

            return result;
        }
    }
}
