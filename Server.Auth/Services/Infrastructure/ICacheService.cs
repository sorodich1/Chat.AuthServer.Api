using Microsoft.Extensions.Caching.Memory;
using System;

namespace Server.Auth.Services.Infrastructure
{
    /// <summary>
    /// Интерфейс службы кэширования
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Получить запись из кеша
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntry Get<TEntry>(object  key);

        /// <summary>
        /// Устанавливает кеш записей на одну минуту скользящего срока действия
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheEntry"></param>
        void SetForMinute<TEntry>(object key, TEntry cacheEntry);

        /// <summary>
        /// Устанавливает кеш записей на 30 минут скользящего срока действия
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheEntry"></param>
        void SetForThirtyMinutes<TEntry>(object key, TEntry cacheEntry);

        /// <summary>
        /// Устанавливает кеш записей для пользовательского скользящего интервала истечения срока действия
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheEntry"></param>
        /// <param name="slidingExpiration"></param>
        void SetWithSlidingExpiration<TEntry>(object key, TEntry cacheEntry, TimeSpan slidingExpiration);

        /// <summary>
        /// Возвращает уже существующую запись или сначала помещает ее в кеш, а потом возвращает запись
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="findIfNotFoundFunc"></param>
        /// <returns></returns>
        TEntry GetOrCreate<TKey, TEntry>(TKey key, Func<ICacheEntry, TEntry> findIfNotFoundFunc);
    }
}
