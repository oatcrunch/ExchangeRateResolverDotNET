using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Data
{
    public class DataContext
    {
        private ConcurrentDictionary<string, object> _rootContext;

        public DataContext()
        {
            _rootContext = new ConcurrentDictionary<string, object>();
        }

        public void AddData(string key, object data)
        {
            _rootContext.TryAdd(key, data);
        }

        public T GetData<T>(string key)
        {
            object readData = null;
            _rootContext.TryGetValue(key, out readData);

            if (readData is T)
            {
                return (T)readData;
            }
            try
            {
                return (T)Convert.ChangeType(readData, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }
    }
}
