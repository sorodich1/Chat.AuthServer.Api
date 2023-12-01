using System;

namespace Server.SignalR.Infrastructure.Factories
{
    public interface IViewModelFactory<TEntity, out TCreateViewModel, out TUpdateViewModel>
        where TCreateViewModel : IViewModel, new()
        where TUpdateViewModel : IViewModel, new()
        where TEntity : class, IHaveId
    {
        TCreateViewModel GenerateForCreate();

        TUpdateViewModel GenerateForUpdate(Guid id);
    }

    public interface IHaveId
    {
        Guid Id { get; set; }
    }
}
