using Calabonga.EntityFrameworkCore.Entities.Base;
using EntityFrameworkCore.AutoHistory.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.AuthServer.DataBase
{
    public abstract class DbContextBase : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        private const string? DefaultUserName = "Анонимный";

        public DbContextBase(DbContextOptions options) : base(options) 
        {
            LastSaveChanges = new SaveChangesResult();
            Database.EnsureCreated();
        }
        
        public SaveChangesResult LastSaveChanges { get; }


        //+++++++++++++++++++
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                DbSaveChanges();
                return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch(Exception ex) 
            {
                LastSaveChanges.Exception = ex;
                return Task.FromResult(0);
            }
        }
        //++++++++++++++++++++
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            try
            {
                DbSaveChanges();
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }
            catch (Exception ex)
            {
                LastSaveChanges.Exception = ex;
                return 0;
            }

        }
        //++++++++++++++++++++++++
        public override int SaveChanges()
        {
            try
            {
                DbSaveChanges();
                return base.SaveChanges();
            }
            catch(Exception ex)
            {
                LastSaveChanges.Exception = ex;
                return 0;
            }
        }
        //++++++++++++++++++++++++++
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                DbSaveChanges();
                return base.SaveChangesAsync(cancellationToken);
            }
            catch(Exception ex)
            {
                LastSaveChanges.Exception = ex; 
                return Task.FromResult(0);
            }

        }
        //+++++++++++++++++++++++++++++++++
        private void DbSaveChanges()
        {
            var createdEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);
            foreach(var entry in createdEntries)
            {
                if(!(entry.Entity is Auditable))
                {
                    continue;
                }
                var creationDate = DateTime.Now.ToUniversalTime();
                var userName = entry.Property("CreatedBy").CurrentValue == null ? DefaultUserName : 
                    entry.Property("CreatedBy").CurrentValue;
                var updatedAt = entry.Property("UpdatedAt").CurrentValue;
                var createdAt = entry.Property("CreatedAt").CurrentValue;
                if( createdAt != null )
                {
                    if(DateTime.Parse(createdAt.ToString()).Year > 1970)
                    {
                        entry.Property("CreatedAt").CurrentValue = ((DateTime)createdAt).ToUniversalTime();
                    }
                    else
                    {
                        entry.Property("CreatedAt").CurrentValue = creationDate;
                    }
                }
                else
                {
                    entry.Property("CreatedAt").CurrentValue = creationDate;
                }
                if(updatedAt != null)
                {
                    if (DateTime.Parse(createdAt.ToString()).Year > 1970)
                    {
                        entry.Property("UpdatedAt").CurrentValue = ((DateTime)updatedAt).ToUniversalTime();
                    }
                    else
                    {
                        entry.Property("UpdatedAt").CurrentValue = creationDate;
                    }
                }
                else
                {
                    entry.Property("UpdatedAt").CurrentValue = creationDate;
                }
                entry.Property("CreatedBy").CurrentValue = userName;
                entry.Property("UpdatedBy").CurrentValue = userName;

                LastSaveChanges.AddMesages($"В отслеживании изменений появились новые объекты: {entry.Entity.GetType()}");
            }

            var updatedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
            foreach( var entry in updatedEntries )
            {
                if(entry.Entity is Auditable)
                {
                    var userName = entry.Property("UpdatedBy").CurrentValue == null ? DefaultUserName :
                        entry.Property("UpdatedBy").CurrentValue;
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now.ToUniversalTime();
                    entry.Property("UpdatedBy").CurrentValue = userName;
                }
                LastSaveChanges.AddMesages($"В среде отслеживания изменений были изменены объекты:: {entry.Entity.GetType()}");
            }
        }

        //++++++++
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<Guid>>().HasNoKey();
            builder.Entity<IdentityUserRole<Guid>>().HasNoKey();
            builder.Entity<IdentityUserToken<Guid>>().HasNoKey();
            builder.Entity<ApplicationUser>()
                .HasOne(x => x.ApplicationUserProfile)
                .WithOne(e => e.ApplicationUser)
                .HasForeignKey<ApplicationUserProfile>(y => y.Id);

            //base.OnModelCreating(builder);

            var applyGenericMethod = typeof(ModelBuilder).GetMethods(BindingFlags.Instance | BindingFlags.Public).First(x => x.Name == "ApplyConfiguration");

            foreach(var type in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.ContainsGenericParameters))
            {
                foreach(var item in type.GetInterfaces())
                {
                    if(!item.IsConstructedGenericType || item.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>))
                    {
                        continue;
                    }

                    var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(item.GenericTypeArguments[0]);
                    applyGenericMethod.Invoke(builder, new[] { Activator.CreateInstance(type) });
                    break;
                }
            }
            builder.EnableAutoHistory(2048);
        }
    }
}
