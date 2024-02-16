using System.Buffers;
using System.Text.Json;
using Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching;

internal sealed class CacheService : ICacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        byte[]? bytes = await _cache.GetAsync(key, cancellationToken);

        return bytes is null ? default :  Deserialize<T>(bytes);
    }

    //public async Task<T> GetOrCreateAsync<T>(
    //    string key,
    //    Func<CancellationToken, Task<T>> factory,
    //    TimeSpan? expiration = null,
    //    CancellationToken cancellationToken = default)
    //{
    //    T result = await _memoryCache.GetOrCreateAsync(
    //        key,
    //        entry =>
    //        {
    //            entry.SetAbsoluteExpiration(expiration ?? DefaultExpiration);

    //            return factory(cancellationToken);
    //        });

    //    return result;
    //}

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        byte[] bytes = Serialize(value);

        return _cache.SetAsync(key, bytes, CacheOptions.Create(expiration), cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default) =>
        _cache.RemoveAsync(key, cancellationToken);

    private static T? Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes);
    }

    private static byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize(writer, value);
        return buffer.WrittenSpan.ToArray();
    }
}
