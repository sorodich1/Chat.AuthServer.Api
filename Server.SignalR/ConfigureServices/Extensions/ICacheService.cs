using Microsoft.Extensions.Caching.Memory;
using System;

namespace Server.SignalR.ConfigureServices.Extensions
{
    public interface ICacheService
    {
        TEntry Get<TEntry>(object key);

        void SetForMinute<TEntry>(object key, TEntry entry);

        void SetForThirtyMinute<TEntry>(object key, TEntry entry);

        void SetWithSlidingExpiration<TEntry>(object key, TEntry entry, TimeSpan slidingExpiration);

        TEntry GetOrCreate<TKey, TEntry>(TKey key, Func<ICacheEntry, TEntry> findIfNotFount);
    }
}
