using Chat.AuthServer.DataBase;
using Chat.AuthServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chat.AuthServer
{
    public class ApplicationDbContext : DbContextBase, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        #region
        public DbSet<ApplicationUserProfile> Profiles { get; set; }
        public DbSet<MicroservicePermission> Permissions { get; set; }
        public DbSet<ApplicationRole> Role { get; set; }
        #endregion

    }
}
