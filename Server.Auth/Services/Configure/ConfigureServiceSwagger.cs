using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Auth.Services.Configure
{
    public static class ConfigureServiceSwagger
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Микросервис архитектуры компании Hiron",
                    Version = "1.0.0.0",
                    Description = "API модуля микросервиса с IdentityServer4. Этот шаблон основан на .NET 7.0"
                });
                options.ResolveConflictingActions(x => x.First());

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("https://localhost:10000/connect/token", UriKind.Absolute),
                            Scopes = new Dictionary<string, string>
                            {
                                {"api1", "Default Scope" }
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
            });
        }

        public static void ConfSwagger(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Микросервис архитектуры компании Hiron v.1.0.0.0");
            options.RoutePrefix = "api/manual";
            options.DocumentTitle = "Микросервис архитектуры компании Hiron v.1.0.0.0";
            options.DefaultModelExpandDepth(0);
            options.DefaultModelRendering(ModelRendering.Model);
            options.DefaultModelsExpandDepth(0);
            options.DocExpansion(DocExpansion.None);
            options.OAuthClientId("microservice1");
            options.OAuthScopeSeparator(" ");
            options.OAuthClientSecret("secret");
            options.DisplayRequestDuration();
            options.OAuthAppName("Microservice module API with IdentityServer4");
        }
    }
}
