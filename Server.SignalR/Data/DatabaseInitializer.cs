using Microsoft.Extensions.DependencyInjection;
using Server.SignalR.Data.EntityDbBase;
using System;

namespace Server.SignalR.Data
{
    public static class DatabaseInitializer
    {
        public static async void Seed(IServiceProvider service)
        {
            using var scope = service.CreateScope();
            await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            if(context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
