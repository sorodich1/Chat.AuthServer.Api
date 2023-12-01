using Microsoft.EntityFrameworkCore;

namespace Chat.AuthServer.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }
        DbSet<ApplicationUserProfile> Profiles { get; set; }
        DbSet<MicroservicePermission> Permissions { get; set; }
        DbSet<ApplicationRole> Role { get; set; }

        int SaveChanges();
    }
}