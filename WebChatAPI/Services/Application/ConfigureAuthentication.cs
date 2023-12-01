using Microsoft.AspNetCore.Builder;

namespace WebChatAPI.Services.Application
{
    public static class ConfigureAuthentication
    {
        public static void Configure(IApplicationBuilder application)
        {
            application.UseRouting();
            application.UseCors("CorsPolicy");
            application.UseAuthentication();
            application.UseAuthorization();
        }
    }
}
