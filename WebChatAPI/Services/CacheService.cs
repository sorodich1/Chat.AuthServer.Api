using Microsoft.Extensions.Caching.Memory;
using System;
using WebChatAPI.Infrastructure.Auth;

namespace WebChatAPI.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(60);

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public TEntry Get<TEntry>(object key)
        {
            if(key == null) throw new ArgumentNullException(nameof(key));
            return _memoryCache.Get<TEntry>(key);
        }

        public TEntry GetOrCreate<TKey, TEntry>(TKey key, Func<ICacheEntry, TEntry> findNotFoundFunc)
        {
            return _memoryCache.GetOrCreate(key, findNotFoundFunc);
        }

        public void SetForMinute<TEntry>(object key, TEntry cacheEntry)
        {
            SetWithSlidingExpiration(key, cacheEntry, _timeout);
        }

        public void SetForThirtyMinutes<TEntry>(object key, TEntry cacheEntry)
        {
            SetWithSlidingExpiration(key, cacheEntry, TimeSpan.FromMinutes(30));
        }

        public void SetWithSlidingExpiration<TEntry>(object key, TEntry cacheEntry, TimeSpan slidingExpiration)
        {
            if(cacheEntry == null)
            {
                throw new Exception(nameof(cacheEntry));
            }
            if(key == null)
            {
                throw new Exception(nameof(key));
            }
            if(slidingExpiration.Ticks == 0)
            {
                slidingExpiration = _timeout;
            }
            var options = new MemoryCacheEntryOptions().SetSlidingExpiration(slidingExpiration);
            _memoryCache.Set(key, cacheEntry, options);
        }
    }
}
