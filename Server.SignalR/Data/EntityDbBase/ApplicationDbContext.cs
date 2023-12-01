using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Server.SignalR.Data.EntityDbBase
{
    public class ApplicationDbContext : DbContextBase<ApplicationDbContext>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) 
        { 

        }
        public DatabaseFacade DatabaseFacade => throw new System.NotImplementedException();
    }
}
