using Microsoft.Extensions.Caching.Memory;
using System;

namespace Server.Auth.Services.Infrastructure
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _defaultSlidingExpiration = TimeSpan.FromSeconds(60);

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public TEntry Get<TEntry>(object key)
        {
            if(key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.Get<TEntry>(key);
        }

        public TEntry GetOrCreate<TKey, TEntry>(TKey key, Func<ICacheEntry, TEntry> findIfNotFoundFunc)
        {
            return _cache.GetOrCreate(key, findIfNotFoundFunc);
        }

        public void SetForMinute<TEntry>(object key, TEntry cacheEntry)
        {
            SetWithSlidingExpiration(key, cacheEntry, _defaultSlidingExpiration);
        }

        public void SetForThirtyMinutes<TEntry>(object key, TEntry cacheEntry)
        {
            SetWithSlidingExpiration(key, cacheEntry, _defaultSlidingExpiration);
        }

        public void SetWithSlidingExpiration<TEntry>(object key, TEntry cacheEntry, TimeSpan slidingExpiration)
        {
            if(cacheEntry == null)
            {
                throw new ArgumentNullException(nameof(cacheEntry));
            }
            if(key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if(slidingExpiration.Ticks == 0)
            {
                slidingExpiration = _defaultSlidingExpiration;
            }
            var options = new MemoryCacheEntryOptions().SetSlidingExpiration(slidingExpiration);
            _cache.Set(key, cacheEntry, options);
        }
    }
}
