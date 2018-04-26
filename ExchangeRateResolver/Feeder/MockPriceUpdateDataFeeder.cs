using ExchangeRateResolver.Models;
using ExchangeRateResolver.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ExchangeRateResolver.Feeder
{
    public class MockPriceUpdateDataFeeder : IDataFeeder<PriceUpdate>
    {
        private Stack<PriceUpdate> _dataStack;

        public MockPriceUpdateDataFeeder()
        {
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(executableLocation, @"Data\PriceUpdateData.txt");
            _dataStack = new Stack<PriceUpdate>();
            string currLine;

            if (File.Exists(filePath))
                using (var streamReader = new StreamReader(filePath))
                    while ((currLine = streamReader.ReadLine()) != null)
                    {
                        var parser = new PriceUpdaterParser();
                        parser.Parse(currLine);
                        _dataStack.Push(parser.GetLastResult() as PriceUpdate);
                    }
        }

        public PriceUpdate GetData()
        {
            return _dataStack.Pop();
        }

        public bool HasData()
        {
            return _dataStack.Count > 0;
        }

        public bool IsAlive()
        {
            return true;    //for demo purposes only, it is always alive
        }
    }
}
