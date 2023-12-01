using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Auth.Helpers;
using Server.Auth.Middlewares;
using Server.Auth.Services.Configure;
using System;

namespace Server.Auth.Services.Application
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
                    x.Context.Response.Headers.Append("Cache-Control", "public, max-age=600");
                }
            });
            app.UseResponseCaching();
            app.UseETagger();
            app.UseIdentityServer();
            app.UseMiddleware(typeof(ErrorHandlerMiddleware));
            app.UseSwagger();
            app.UseSwaggerUI(ConfigureServiceSwagger.ConfSwagger);

            UserIdentity.Instrance.Configure(app.ApplicationServices.GetService<IHttpContextAccessor>()!);
        }
    }
}
