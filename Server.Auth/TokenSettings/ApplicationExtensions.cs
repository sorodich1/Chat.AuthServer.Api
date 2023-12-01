using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System.Text.RegularExpressions;

namespace Server.Auth.TokenSettings
{
    /// <summary>
    /// Специальный класс для расширения приложения
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Используйте специальный маршрут для маршрутизации 
        /// </summary>
        public static void UseRouteSlugify(this MvcOptions options)
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParametrs()));
        }
        public class SlugifyParametrs : IOutboundParameterTransformer
        {
            public string? TransformOutbound(object? value)
            {
                var str = value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
                Program.Logger.Info($"маршрут авторизации - [{str}]");
                return str;

                
            }
        }
    }
}
