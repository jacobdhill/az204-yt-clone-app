using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Cache;

public class CacheService
{
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void Set<T>(string key, T value, TimeSpan expiration = default)
    {
        _cache.Set(key, value, expiration);
    }

    public T Get<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFunction, TimeSpan expiration = default)
    {
        var value = Get<T>(key);
        if (value is null)
        {
            value = await valueFunction();
            Set(key, value, expiration);
        }
        return value;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}
