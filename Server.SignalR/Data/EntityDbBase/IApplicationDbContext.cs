using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Server.SignalR.Data.EntityDbBase
{
    public interface IApplicationDbContext
    {
        DatabaseFacade DatabaseFacade { get; }
        ChangeTracker ChangeTracker { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}
