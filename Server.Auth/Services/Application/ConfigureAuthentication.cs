using Microsoft.AspNetCore.Builder;

namespace Server.Auth.Services.Application
{
    public static class ConfigureAuthentication
    {
        public static void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
