using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Auth.Data.Entityes;
using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Server.Auth.Data.DbContext
{
    public class ApplicationUserStore: UserStore<ApplicationUsers, ApplicationRoles, ApplicationDbContext, Guid>
    {
        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }

        //public override Task<ApplicationUsers> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        //{
        //    //return Users
        //    //    .Include(x => x.ApplicationUserId)
        //    //    .FirstOrDefaultAsync(u => u.Id.ToString() == userId, cancellationToken: cancellationToken);
        //}
    }
}
