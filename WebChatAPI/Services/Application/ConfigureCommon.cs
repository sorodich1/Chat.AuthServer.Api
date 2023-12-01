using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebChatAPI.Infrastructure;
using WebChatAPI.Middlewares;
using WebChatAPI.Services.Identity;

namespace WebChatAPI.Services.Application
{
    public static class ConfigureCommon
    {
        public static void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            if(environment.IsDevelopment())
            {
                //mapper.AssertConfigurationIsValid();
                application.UseDeveloperExceptionPage();
            }
            else
            {
               // mapper.CompileMappings();
            }

            application.UseDefaultFiles();

            application.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = x =>
                {
                    x.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }
            });

            application.UseResponseCaching();

            application.UseETagger();

            application.UseIdentityServer();

            application.UseMiddleware(typeof(ErrorHandlingMiddleware));

            application.UseSwagger();

            application.UseSwaggerUI(ConfigureServicesSwapper.SwaggerSetting);

            UserIdentity.Instance.Configure(application.ApplicationServices.GetService<IHttpContextAccessor>()!);
        }
    }
}
