using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace DummyAPI.Extensions;

public static class DistributedCacheExtensions
{
    private static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = null,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
    {
        return cache.SetAsync(key, value, new DistributedCacheEntryOptions());
    }
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _serializerOptions));
        return cache.SetAsync(key, bytes, options);
    }
    public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T? value)
    {
        var val = cache.Get(key);
        value = default;

        if (val == null) return false;

        value = JsonSerializer.Deserialize<T>(val, _serializerOptions);

        return true;
    }
}