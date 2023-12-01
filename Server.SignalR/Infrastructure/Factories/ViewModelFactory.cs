using System;

namespace Server.SignalR.Infrastructure.Factories
{
    public abstract class ViewModelFactory<TEntity, TCreateViewModel, TUpdateViewModel> :
        IViewModelFactory<TEntity, TCreateViewModel, TUpdateViewModel>
        where TCreateViewModel : IViewModel, new()
        where TUpdateViewModel : IViewModel, new()
        where TEntity : class, IHaveId
    {
        public abstract TCreateViewModel GenerateForCreate();

        public abstract TUpdateViewModel GenerateForUpdate(Guid id);
    }
}
