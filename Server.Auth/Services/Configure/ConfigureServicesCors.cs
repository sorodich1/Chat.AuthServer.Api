using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Auth.Helpers;
using System.Linq;

namespace Server.Auth.Services.Configure
{
    public class ConfigureServicesCors
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string[] origins = configuration.GetSection("Cors")?.GetSection("Origins")?.Value?.Split(',');

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    if (origins != null && origins.Length > 0)
                    {
                        if (origins != null && origins.Contains("*"))
                        {
                            builder.AllowAnyHeader();
                            builder.AllowAnyMethod();
                            builder.SetIsOriginAllowed(host => true);
                            builder.AllowCredentials();
                        }
                        else
                        {
                            foreach (var origin in origins)
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
