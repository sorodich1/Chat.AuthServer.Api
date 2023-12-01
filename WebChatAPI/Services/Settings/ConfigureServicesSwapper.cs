using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Builder;
using WebChatAPI.Infrastructure.Auth;

namespace WebChatAPI.Services
{
    public static class ConfigureServicesSwapper
    {
        private const string AppTitle = "Microservice API";
        private static readonly string AppVersion = $"1.0.0.0";
        private const string SwaggerCongig = "/swagger/v1/swagger.json";
        private const string SwaggerUrl = "api/manual";

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration) => services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = AppTitle,
                Version = AppVersion,
                Description = "Модуль API микросервиса. Этот шаблон основан на NET 7.0"
            });

            options.ResolveConflictingActions(x => x.First());

            var url = configuration.GetSection("IdentityServer").GetValue<string>("Url");

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri($"https://localhost:7120/connect/token", UriKind.Absolute),
                        Scopes = new Dictionary<string, string>
                        {
                            { "api1", "Default scope" }
                        }
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            options.OperationFilter<ApplySummariesOperationFilter>();
        });

        public static void SwaggerSetting(SwaggerUIOptions setting)
        {
            setting.SwaggerEndpoint(SwaggerCongig, $"{AppTitle} v.{AppVersion}");
            setting.RoutePrefix = SwaggerUrl;
            setting.HeadContent = $"8 8";
            setting.DocumentTitle = $"{AppTitle}";
            setting.DefaultModelExpandDepth(0);
            setting.DefaultModelRendering(ModelRendering.Model);
            setting.DefaultModelsExpandDepth(0);
            setting.DocExpansion(DocExpansion.None);
            setting.OAuthClientId("microservice1");
            setting.OAuthScopeSeparator(" ");
            setting.OAuthClientSecret("secret");
            setting.DisplayRequestDuration();
            setting.OAuthAppName("Microservice module API");
        }
    }
}
