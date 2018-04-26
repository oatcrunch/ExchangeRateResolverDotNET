using System;

namespace ExchangeRateResolver.Feeder
{
    public abstract class PriceUpdaterBase : IListener
    {
        public delegate void NotifyObserver(object sender, EventArgs e);
        public event NotifyObserver OnReceived;

        public virtual void UpdateReceived(object sender, EventArgs e)
        {
            if (OnReceived != null)
            {
                OnReceived(this, e);
            }
        }

        public abstract void StartReceivingUpdates();
    }
}