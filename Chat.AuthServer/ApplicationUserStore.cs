using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.AuthServer
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>
    {
        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {

        }

        public override Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
        {
            return Users
                .Include(x => x.ApplicationUserProfile)
                .ThenInclude(x => x.Permission)
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId, cancellationToken: cancellationToken);
        }
    }
}
