using IntegrationSingleServer.Common.Redis;
using IntegrationSingleServer.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationSingleServer.Service
{
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisService()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            _db = _redis.GetDatabase();
        }

        public async Task<bool> AcquireLockAsync(KeyItem keyItem)
        {
            return await _db.StringSetAsync(keyItem.Key, keyItem.Value, keyItem.Expiration, when: When.NotExists);
        }

        public async Task<bool> KeyDeleteAsync(string key)
        {
            return await _db.KeyDeleteAsync(key);
        }

        public async Task<string> StringGetAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task<bool> StringSetAsync(string key, string value)
        {
            return await _db.StringSetAsync(key, value);
        }
    }
}