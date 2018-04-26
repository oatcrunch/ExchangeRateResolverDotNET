namespace ExchangeRateResolver.Feeder
{
    public interface IPublisher
    {
        void NotifyAllListeners(object data);
        void Subscribe(IListener listener);
        void UnSubscribe(IListener listener);
    }
}