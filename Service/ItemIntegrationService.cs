using Integration.Common;
using Integration.Backend;
using System.Collections.Concurrent;

namespace Integration.Service;

public sealed class ItemIntegrationService
{
    //This is a dependency that is normally fulfilled externally.
    private ItemOperationBackend ItemIntegrationBackend { get; set; } = new();
    private static readonly ConcurrentDictionary<string, object> _lockItem = new ConcurrentDictionary<string, object>();

    public Result SaveItem(string itemContent)
    {
        var lockContent = _lockItem.GetOrAdd(itemContent, string.Empty);

        //Solution Single Server Scenario: This solution locking sended progressing new data and didn't lock an other new data progress and has been fix same timing problem with if progressing data sending an other progress
        lock (lockContent)
        {
            try
            {
                // Check the backend to see if the content is already saved.
                if (ItemIntegrationBackend.HasContent(itemContent))
                {
                    return new Result(false, $"Duplicate item received with content {itemContent}.");
                }

                var item = ItemIntegrationBackend.SaveItem(itemContent);
                return new Result(true, $"Item with content {itemContent} saved with id {item.Id}");
            }
            finally
            {
                _lockItem.TryRemove(itemContent, out _);
            }
        }
    }

    public List<Item> GetAllItems()
    {
        return ItemIntegrationBackend.GetAllItems();
    }
}