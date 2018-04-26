using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace ExchangeRateResolver
{
    public class ConfigManager
    {
        private static NameValueCollection _appSettings;
        private static ConfigManager _instance;
        public static ConfigManager GetInstance()
        {
            if (_instance == null) _instance = new ConfigManager();

            return _instance;
        }

        public object GetValue(string key)
        {
            return _appSettings[key];
        }

        private ConfigManager()
        {
            _appSettings = ConfigurationManager.AppSettings;
        }
    }
}
