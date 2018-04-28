using ExchangeRateResolver.Core;
using ExchangeRateResolver.Data;
using ExchangeRateResolver.Feeder;
using ExchangeRateResolver.Models;
using ExchangeRateResolver.Models.Events;
using ExchangeRateResolver.Parsers;
using System;
using System.Threading;

namespace ExchangeRateResolver
{
    class Program
    {
        private static DataContext _dataContext;
        private static CommandProcessor _commandProcessor;
        private static ExchangeRateGraphProcessor _exchangeRateGraphProcessor;
        private static IDataFeeder<PriceUpdate> _dataFeeder;
        private static PriceUpdaterBase _priceUpdater;

        static void Main(string[] args)
        {
            Console.WriteLine("========== Welcome to TenX Exchange Rate Resolver ==========");

            /** Initialize root data **/
            _dataContext = new DataContext();

            /** Initialize processors **/
            _commandProcessor = new CommandProcessor(_dataContext);
            _exchangeRateGraphProcessor = new ExchangeRateGraphProcessor(_dataContext);
            _commandProcessor.SetSuccessor(_exchangeRateGraphProcessor);

            /** Prepare data feeder and updater **/
            _dataFeeder = GetFeederFeatureToggle(); // feature toggle to switch between mock or real production data
            _priceUpdater = _priceUpdater = new PriceUpdater(_dataFeeder);

            /** Establish listening service to get initial data **/
            _priceUpdater.OnReceived += PriceUpdater_OnReceived;
            _priceUpdater.StartReceivingUpdates();

            while (true)
            {
                string cmd = Console.ReadLine().Trim(); //user can either enter price updata data command OR enter the exchange rate request command
                if (cmd.ToLower() == "exit") break;

                try
                {
                    /** Process command **/
                    _commandProcessor.Process(cmd);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void PriceUpdater_OnReceived(object sender, EventArgs e)
        {
            var priceUpdateEvent = e as PriceUpdateEvent;
            Console.WriteLine(priceUpdateEvent?.Data?.OriginalCommand);
            _exchangeRateGraphProcessor.Process(priceUpdateEvent.Data);
        }

        private static IDataFeeder<PriceUpdate> GetFeederFeatureToggle()
        {
            bool useMock = false;

            try
            {
                useMock = ConfigManager.GetInstance().GetValue("EnableMock").ToString().ToLower() == "true";
            }
            catch (Exception)
            {

            }

            if (useMock)
            {
                return new MockPriceUpdateDataFeeder();
            }
            else
            {
                return new PriceUpdateDataFeeder("https://somehost/someapi/exchangerates/pull");
            }
        }
    }
}
