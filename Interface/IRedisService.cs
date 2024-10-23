using IntegrationSingleServer.Common.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationSingleServer.Interface
{
    public interface IRedisService
    {
        Task<string> StringGetAsync(string key);
        Task<bool> StringSetAsync(string key, string value);
        Task<bool> KeyDeleteAsync(string key);
        Task<bool> AcquireLockAsync(KeyItem keyItem);
    }
}