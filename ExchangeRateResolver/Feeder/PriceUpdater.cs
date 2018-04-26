using ExchangeRateResolver.Models;
using ExchangeRateResolver.Models.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ExchangeRateResolver.Feeder
{
    public class PriceUpdater : PriceUpdaterBase
    {
        private IDataPublisher _dataPublisher;

        public PriceUpdater(IDataFeeder<PriceUpdate> dataFeeder)
        {
            this._dataPublisher = new PriceUpdatePublisher(dataFeeder);
            this._dataPublisher.Subscribe(this);
        }

        public override void UpdateReceived(object sender, EventArgs e)
        {
            base.UpdateReceived(sender, e);
        }

        public override void StartReceivingUpdates()
        {
            this._dataPublisher.StartPublish();
        }
    }
}
