using Microsoft.EntityFrameworkCore;

namespace Server.Auth.Data.Entityes
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUsers> Users { get; set; }
        DbSet<Log> Logs { get; set; }
        DbSet<ApplicationUserProfile> Profiles { get; set; }
        DbSet<MicroservicePermission> Permissions { get; set; }
        DbSet<ApplicationRoles> Role { get; set; }
        int SaveChanges();
    }
}
