using Microsoft.AspNetCore.Builder;
using WebChatAPI.Hubs;

namespace WebChatAPI.Services.Application
{
    public static class ConfigureEndpoints
    {
        public static void Configure(IApplicationBuilder application) => application.UseEndpoints(end =>
        {
            end.MapDefaultControllerRoute();
            end.MapControllers();
            end.MapHub<CommunicationHub>("/chat");
        });
    }
}
