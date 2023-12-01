using Calabonga.Microservices.Core.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Server.SignalR.ConfigureServices.Extensions
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
            if (key == null) throw new ArgumentNullException(nameof(key));
            return _memoryCache.Get<TEntry>(key);
        }

        public TEntry GetOrCreate<TKey, TEntry>(TKey key, Func<ICacheEntry, TEntry> findIfNotFount)
        {
            return _memoryCache.GetOrCreate(key, findIfNotFount);
        }

        public void SetForMinute<TEntry>(object key, TEntry entry)
        {
            SetWithSlidingExpiration(key, entry, _timeout);
        }

        public void SetForThirtyMinute<TEntry>(object key, TEntry entry)
        {
            SetWithSlidingExpiration(key, entry, TimeSpan.FromMinutes(30));
        }

        public void SetWithSlidingExpiration<TEntry>(object key, TEntry entry, TimeSpan slidingExpiration)
        {
            if (entry == null)
            {
                throw new MicroserviceArgumentNullException(nameof(entry));
            }

            if (key == null)
            {
                throw new MicroserviceArgumentNullException(nameof(key));
            }

            if (slidingExpiration.Ticks == 0)
            {
                slidingExpiration = _timeout;
            }

            var options = new MemoryCacheEntryOptions().SetSlidingExpiration(slidingExpiration);

            _memoryCache.Set(key, entry, options);
        }
    }
}
