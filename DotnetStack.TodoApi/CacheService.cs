using System.Text.Json;
using StackExchange.Redis;

namespace DotnetStack.TodoApi;

public class CacheService
{
    private readonly IDatabase _database;

    public CacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }
    
    public async Task<T> GetOrAdd<T>(string key, Func<Task<T>> itemFactory, int expirationInSecond)
    {
        // если такой элемент уже есть, возврашаем его
        var existingItem = await _database.StringGetAsync(key);
        if (existingItem.HasValue)
        {
            return JsonSerializer.Deserialize<T>(existingItem);
        }

        // забираем новый элемент
        var newItem = await itemFactory();

        // добавляем элемент в кэш
        await _database.StringSetAsync(key, JsonSerializer.Serialize(newItem), TimeSpan.FromSeconds(expirationInSecond));

        return newItem;
    }

    public async Task Invalidate(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}

