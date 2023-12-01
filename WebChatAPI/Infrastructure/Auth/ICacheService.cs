using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebChatAPI.Infrastructure.Auth
{
    public interface ICacheService
    {
        /// <summary>
        /// Получение записи из кеша
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntry Get<TEntry>(object key);
        /// <summary>
        /// Устанавливает кеш записей на одну минуту
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheEntry"></param>
        void SetForMinute<TEntry>(object key, TEntry cacheEntry);
        /// <summary>
        /// Устанавливает кеш записей на 30 минут
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="key"></param>
        /// <param name="cacheEntry"></param>
        void SetForThirtyMinutes<TEntry>(object key, TEntry cacheEntry);
        /// <summary>
        /// Устанавливает кеш записей для пользовательского интервала по истечения срока действия
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
        /// <param name="findNotFoundFunc"></param>
        /// <returns></returns>
        TEntry GetOrCreate<TKey, TEntry>(TKey key, Func<ICacheEntry, TEntry> findNotFoundFunc);
    }
}
