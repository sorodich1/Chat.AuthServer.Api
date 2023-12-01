using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Auth.Data.Entityes;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Reflection;
using Calabonga.UnitOfWork;
using Calabonga.EntityFrameworkCore.Entities.Base;

namespace Server.Auth.Data.DbContext
{
    public class DbContextBase : IdentityDbContext<ApplicationUsers, IdentityRole<Guid>, Guid>
    {
        private const string DefaultName = "Анонимный";

        public DbContextBase(DbContextOptions options) : base(options)
        {
            LastSaveChanges = new SaveChangesResult();
            Database.EnsureCreated();
        }

        public SaveChangesResult LastSaveChanges { get; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                DbSaveChanges();
                return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch (Exception ex)
            {
                LastSaveChanges.Exception = ex;
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
                LastSaveChanges.Exception = ex;
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
                LastSaveChanges.Exception = ex;
                return 0;
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                DbSaveChanges();
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                LastSaveChanges.Exception = ex;
                return Task.FromResult(0);
            }
        }

        private void DbSaveChanges()
        {
            var createdEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);

            foreach (var entry in createdEntries)
            {
                if(!(entry.Entity is Auditable))
                {
                    continue;
                }

                var creationDate = DateTime.Now.ToUniversalTime();
                var userName = entry.Property("CreateBy").CurrentValue == null ? DefaultName :
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

                LastSaveChanges.AddMessage($"В отслеживании изминений появились новые объекты: {entry.Entity.GetType()}");
            }

            var updateEntryes = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
            foreach (var entry in updateEntryes)
            {
                if (entry.Entity is Auditable)
                {
                    var userName = entry.Property("UpdateBy").CurrentValue == null ? DefaultName :
                        entry.Property("UpdateBy").CurrentValue;
                    entry.Property("UpdateAt").CurrentValue = DateTime.Now.ToUniversalTime();
                    entry.Property("UpdateBy").CurrentValue = userName;
                }
                LastSaveChanges.AddMessage($"В отслеживании изминений обновились новые объекты: {entry.Entity.GetType()}");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserLogin<Guid>>().HasNoKey();
            builder.Entity<IdentityUserRole<Guid>>().HasNoKey();
            builder.Entity<IdentityUserToken<Guid>>().HasNoKey();
            builder.Entity<ApplicationUserProfile>()
                .HasOne(x => x.ApplicationUser)
                .WithOne(x => x.ApplicationUserProfile)
                .HasForeignKey<ApplicationUserProfile>(y => y.ApplicationUserProfileId).IsRequired();


            base.OnModelCreating(builder);
            var applyGenericMethod = typeof(ModelBuilder).GetMethods(BindingFlags.Instance | BindingFlags.Public).First(x => x.Name == "ApplyConfiguration");

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && !x.IsAbstract && x.ContainsGenericParameters))
            {
                foreach (var item in type.GetInterfaces())
                {
                    if (!item.IsConstructedGenericType || item.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>))
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
