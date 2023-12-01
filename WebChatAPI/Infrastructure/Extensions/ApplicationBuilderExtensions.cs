using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace WebChatAPI.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseRouteSlugify(this MvcOptions options) => options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifiParameterTransformer()));
    }
    public class SlugifiParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value) => value == null
            ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}
