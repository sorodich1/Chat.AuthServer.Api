using AutoMapper;
using Calabonga.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace Server.SignalR.Infrastructure.Mapper
{
    /// <summary>
    /// Общий конвертер для коллекций IPagedList
    /// </summary>
    /// <typeparam name="TMapForm"></typeparam>
    /// <typeparam name="TMapTo"></typeparam>
    public class PageListConverter<TMapForm, TMapTo> : ITypeConverter<IPagedList<TMapForm>, IPagedList<TMapTo>>
    {
        /// <summary>
        /// Выполняет преобразование из исходного в целевой тип
        /// </summary>
        /// <param name="source">Исходный объект</param>
        /// <param name="destination">Целевой объект</param>
        /// <param name="context">Контекст разрешения</param>
        /// <returns>Целевой объект</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IPagedList<TMapTo> Convert(IPagedList<TMapForm> source, IPagedList<TMapTo> destination, ResolutionContext context)
        {
            if(source == null)
            {
                return null;
            }
            var vm = source.Items.Select(x => context.Mapper.Map<TMapForm, TMapTo>(x)).ToList();
            var pageList = PagedList.From<TMapTo, TMapForm>(source, con => context.Mapper.Map<IEnumerable<TMapTo>>(con));
            return pageList;
        }
    }
}
