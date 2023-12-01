using Microsoft.EntityFrameworkCore;
using Server.Auth.Data.Entityes;

namespace Server.Auth.Data.DbContext
{
    public class ApplicationDbContext : DbContextBase, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<ApplicationRoles> Role { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<ApplicationUserProfile> Profiles { get; set; }
        public DbSet<MicroservicePermission> Permissions { get; set; }
    }
}
