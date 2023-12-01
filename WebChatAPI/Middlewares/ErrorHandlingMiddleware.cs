using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace WebChatAPI.Middlewares
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
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            try
            {
                var result = JsonConvert.SerializeObject(ExceptionHelper.GetMessages(ex), Formatting.Indented);
                if(result.Length > 4000)
                {
                    return context.Response.WriteAsync("Сообщение об ошибке слишком длинное. Используйте DEBUG в методе HandleExceptionAsync для обработки всего текста исключения.");
                }
                return context.Response.WriteAsync(result);
            }
            catch
            {
                return context.Response.WriteAsync($"{ex.Message} Для получения дополнительной информации используйте DEBUG в методе HandleExceptionAsync для обработки всего текста исключения.");
            }
        }
    }
}
