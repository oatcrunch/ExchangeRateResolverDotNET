using System;

namespace ExchangeRateResolver.Feeder
{
    public interface IListener
    {
        void UpdateReceived(object sender, EventArgs e);
    }
}