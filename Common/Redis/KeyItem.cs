using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationSingleServer.Common.Redis
{
    public class KeyItem
    {
        public KeyItem(string tag, string key, string value, TimeSpan expiration)
        {
            Key = $"{tag}:{key}";
            Value = value;
            Expiration = expiration;
        }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public TimeSpan Expiration { get; private set; }
    }
}