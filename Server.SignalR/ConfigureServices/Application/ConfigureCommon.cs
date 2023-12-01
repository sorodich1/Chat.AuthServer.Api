using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.SignalR.ConfigureServices.Collection;
using Server.SignalR.Infrastructure.Auth;
using Server.SignalR.Middlewares;

namespace Server.SignalR.ConfigureServices.Application
{
    public static class ConfigureCommon
    {
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = x =>
                {
                    x.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }
            });
            app.UseResponseCaching();
            app.UseETagger();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseSwagger();
            app.UseSwaggerUI(ConfigureServicesSwagger.ConfSwagger);

            IdentityHelper.Instance.Configure(app.ApplicationServices.GetService<IHttpContextAccessor>()!);
        }
    }
}
