using Microsoft.AspNetCore.Builder;
using Server.SignalR.Hubs;

namespace Server.SignalR.ConfigureServices.Application
{
    public static class ConfigureEndpoint
    {
        public static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CommunicationHub>("/chat");
            });
        }
    }
}
