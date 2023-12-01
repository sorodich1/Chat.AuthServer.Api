using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using WebChatAPI.Services.Identity;

namespace WebChatAPI.Services.Settings
{
    public static class ConfigureServicesCors
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection("Cors")?.GetSection("Origins")?.Value?.Split(',');
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    if(origins != null && origins.Length > 0)
                    {
                        if(origins.Contains("*"))
                        {
                            builder.AllowAnyHeader();
                            builder.AllowAnyMethod();
                            builder.SetIsOriginAllowed(hoest => true);
                            builder.AllowCredentials();
                        }
                        else
                        {
                            foreach(var origin in origins)
                            {
                                builder.WithOrigins(origin);
                            }
                        }
                    }
                });
            });

            services.UseMicroserviceAuthorizationPolicy();
        }
    }
}
