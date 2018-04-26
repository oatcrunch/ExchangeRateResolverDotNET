using ExchangeRateResolver.Data;
using ExchangeRateResolver.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Core
{
    public class CommandProcessor : Processor
    {
        private DataContext _dataContext;
        private ParserBuilder _parserBuilder;

        public CommandProcessor(DataContext dataContext)
        {
            _dataContext = dataContext;
            _parserBuilder = new ParserBuilder();
        }

        public override void Process(object work)
        {
            string command = work?.ToString();
            string key = "";

            if (command?.Contains(Constants.EXCHANGE_RATE_REQUEST) == true)
            {
                key = Constants.EXCHANGE_RATE_RESOLVER_PARSER;
            }

            var tokenizedQuery = command?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var firstToken = tokenizedQuery?.Length > 0 ? tokenizedQuery[0] : null;

            DateTime transDate;
            if (DateTime.TryParse(firstToken, out transDate))
            {
                key = Constants.PRICE_UPDATER_PARSER;
            }

            var parser = _parserBuilder.GetParser(key);
            parser?.Parse(command);

            this.successor.Process(parser?.GetLastResult());
        }
    }
}
