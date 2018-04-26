using ExchangeRateResolver.Models;
using ExchangeRateResolver.Models.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ExchangeRateResolver.Feeder
{
    public class PriceUpdatePublisher : IDataPublisher
    {
        public delegate void NotifyObserver(object sender, EventArgs e);
        public event NotifyObserver NotifyObserverEvent;
        private IDataFeeder<PriceUpdate> _dataFeeder;
        private int _minTrafficLatency = 2000;
        private int _maxTrafficLatency = 3000;

        public PriceUpdatePublisher(IDataFeeder<PriceUpdate> dataFeeder)
        {
            this._dataFeeder = dataFeeder;
            try
            {
                this._minTrafficLatency = int.Parse(ConfigManager.GetInstance().GetValue("MinTrafficLatency").ToString());
            }
            catch (Exception)
            {

            }
            try
            {
                this._maxTrafficLatency = int.Parse(ConfigManager.GetInstance().GetValue("MaxTrafficLatency").ToString());
            }
            catch (Exception)
            {

            }
        }

        public void Subscribe(IListener listener)
        {
            NotifyObserverEvent += listener.UpdateReceived;
        }

        public void UnSubscribe(IListener listener)
        {
            NotifyObserverEvent -= listener.UpdateReceived;
        }

        public void NotifyAllListeners(object data)
        {
            if (NotifyObserverEvent != null)
            {
                NotifyObserverEvent(this, new PriceUpdateEvent(data as PriceUpdate));
            }
        }

        public void StartPublish()
        {
            var t = new Thread(new ThreadStart(() =>
            {
                while (_dataFeeder.IsAlive())
                {
                    if (_dataFeeder.HasData())
                    {
                        var data = _dataFeeder.GetData();
                        var random = new Random();
                        var delay = random.Next(this._minTrafficLatency, this._maxTrafficLatency);
                        Thread.Sleep(delay);    //simulate network
                        this.NotifyAllListeners(data);
                    }
                }
            }));
            t.IsBackground = true;
            t.Start();
        }
    }
}
