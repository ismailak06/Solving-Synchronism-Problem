using Integration.Backend;
using Integration.Common;
using IntegrationSingleServer.Common.Redis;
using IntegrationSingleServer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationSingleServer.Service
{
    public class ItemIntegrationDistributedSystemService
    {

        private ItemOperationBackend ItemIntegrationBackend { get; set; } = new();
        private readonly IRedisService _redisService;

        public ItemIntegrationDistributedSystemService(IRedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Result> SaveItem(string itemContent)
        {
            var keyItem = new KeyItem("saveItem", itemContent, Guid.NewGuid().ToString(), TimeSpan.FromSeconds(40));

            var lockAcquired = await _redisService.AcquireLockAsync(keyItem);

            if (lockAcquired)
            {
                try
                {
                    if (ItemIntegrationBackend.HasContent(itemContent))
                    {
                        return new Result(false, $"Duplicate item received with content {itemContent}.");
                    }

                    var item = ItemIntegrationBackend.SaveItem(itemContent);
                    return new Result(true, $"Item with content {itemContent} saved with id {item.Id}");
                }
                finally
                {
                    await _redisService.KeyDeleteAsync(keyItem.Key);
                }
            }
            else
            {
                return new Result(false, $"This {itemContent} is already processing.");
            }
        }
    }
}