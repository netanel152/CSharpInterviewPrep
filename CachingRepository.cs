using CSharpInterviewPrep;
using Microsoft.Extensions.Caching.Memory;

public class CachingRepository : IRepository
{
    private IRepository _decorated;
    private IMemoryCache _cache;
    private MemoryCacheEntryOptions _cacheOptions;

    public CachingRepository(IRepository decorated, IMemoryCache cache)
    {
        _decorated = decorated;
        _cache = cache;
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
    }

    public async Task<string> GetById(int id)
    {
        string cacheKey = $"data_{id}";
        string? cachedValue = _cache.Get<string>(cacheKey);
        if (cachedValue != null)
        {
            return cachedValue;
        }
        string data = await _decorated.GetById(id);
        _cache.Set(cacheKey, data, _cacheOptions);
        return data;
    }
}