namespace ExchangeRateResolver.Parsers
{
    public interface IParser
    {
        object GetLastResult();
        object Parse(string command);
    }
}