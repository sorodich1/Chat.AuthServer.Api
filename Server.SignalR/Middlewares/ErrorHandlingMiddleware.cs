using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Calabonga.UnitOfWork;
using Newtonsoft.Json;

namespace Server.SignalR.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {
                var result = JsonConvert.SerializeObject(ExceptionHelper.GetMessages(exception), Formatting.Indented);
                if (result?.Length > 4000)
                {
                    return context.Response.WriteAsync("Сообщение об ошибке слишком длинное. Используйте DEBUG в методе HandleExceptionAsync для обработки всего текста исключения.");
                }
                return context.Response.WriteAsync(result);
            }
            catch
            {
                return context.Response.WriteAsync($"{exception.Message} Для получения дополнительной информации используйте DEBUG в методе HandleExceptionAsync для обработки всего текста исключения.");
            }
        }
    }
}
