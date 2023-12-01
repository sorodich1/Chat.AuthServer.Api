using Microsoft.AspNetCore.Builder;

namespace Server.Auth.Middlewares
{
    /// <summary>
    /// Расширение ETagger
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Использовать пользовательское промежуточное ПО
        /// </summary>
        /// <param name="app"></param>
        public static void UseETagger(this IApplicationBuilder app)
        {
            app.UseMiddleware<ETagMiddleware>();
        }
    }
}
