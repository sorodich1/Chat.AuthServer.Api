using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.EntityFrameworkCore.Entities.Base;
using System.Reflection;

namespace Server.SignalR.Data.EntityDbBase
{
    public abstract class DbContextBase<TContext> : DbContext where TContext : DbContext
    {
        private const string DefaultUserName = "Anonimus";

        protected DbContextBase(DbContextOptions<TContext> options) : base(options) 
        {
            LastChangesResult = new SaveChangesResult();
        }

        SaveChangesResult LastChangesResult { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
             try
            {
                DbSaveChanges();
                return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch(Exception ex) 
            {
                LastChangesResult.Exception = ex;
                return Task.FromResult(0);
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            try
            {
                DbSaveChanges();
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }
            catch (Exception ex)
            {
                LastChangesResult.Exception = ex;
                return 0;
            }
        }

        public override int SaveChanges()
        {
            try
            {
                DbSaveChanges();
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                LastChangesResult.Exception = ex;
                return 0;
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                DbSaveChanges();
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                LastChangesResult.Exception = ex;
                return Task.FromResult(0);
            }
        }

        private void DbSaveChanges()
        {
            var createdEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

            foreach (var entry in createdEntries)
            {
                if (!(entry.Entity is IAuditable))
                {
                    continue;
                }

                var creationDate = DateTime.Now.ToUniversalTime();
                var userName = entry.Property("CreateBy").CurrentValue == null ? DefaultUserName :
                    entry.Property("CreateBy").CurrentValue;
                var updatedAt = entry.Property("UpdateAt").CurrentValue;
                var createdAt = entry.Property("CreateAt").CurrentValue;
                if (createdAt != null)
                {
                    if (DateTime.Parse(createdAt.ToString()).Year > 1970)
                    {
                        entry.Property("CreateAt").CurrentValue = ((DateTime)createdAt).ToUniversalTime();
                    }
                    else
                    {
                        entry.Property("CreateAt").CurrentValue = creationDate;
                    }
                }
                else
                {
                    entry.Property("CreateAt").CurrentValue = creationDate;
                }
                if (updatedAt != null)
                {
                    if (DateTime.Parse(createdAt.ToString()).Year > 1970)
                    {
                        entry.Property("UpdateAt").CurrentValue = ((DateTime)updatedAt).ToUniversalTime();
                    }
                    else
                    {
                        entry.Property("UpdateAt").CurrentValue = creationDate;
                    }
                }
                else
                {
                    entry.Property("UpdateAt").CurrentValue = creationDate;
                }
                entry.Property("CreateBy").CurrentValue = userName;
                entry.Property("UpdateBy").CurrentValue = userName;

                LastChangesResult.AddMessage($"В отслеживании изминений появились новые объекты: {entry.Entity.GetType()}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var applyGenericMethod = typeof(ModelBuilder).GetMethods(BindingFlags.Instance | BindingFlags.Public).First(x => x.Name == "ApplyConfiguration");
            foreach(var type in Assembly.GetExecutingAssembly().GetTypes()
                .Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters))
            {
                foreach(var item in type.GetInterfaces())
                {
                    if(!item.IsConstructedGenericType || item.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>))
                    {
                        continue;
                    }

                    var applyConcretedMethod = applyGenericMethod.MakeGenericMethod(item.GetGenericArguments());
                    applyConcretedMethod.Invoke(modelBuilder, new object[] {Activator.CreateInstance(type)});
                }
            }
            modelBuilder.EnableAutoHistory(2048);
        }
    }
}
