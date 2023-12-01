using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Server.Auth.Middlewares
{
    /// <summary>
    /// Промежуточное ПО ETag
    /// </summary>
    public class ETagMiddleware
    {
        private readonly RequestDelegate _next;

        public ETagMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var response = context.Response;
            var originalStream = response.Body;

            using(var ms = new MemoryStream())
            {
                response.Body = ms;

                await _next(context);

                Program.Logger.Info($"Статус подключения {response.StatusCode}");

                if(IsEtagSupported(response))
                {
                    var checkSum = CalculateChecksum(ms);

                    Program.Logger.Info($"Url подключения -  {checkSum}");

                    response.Headers[HeaderNames.ETag] = checkSum;
                    if(context.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etag) && checkSum == etag)
                    {
                        response.StatusCode = StatusCodes.Status304NotModified; 
                        return;
                    }
                }
                ms.Position = 0;
                await ms.CopyToAsync(originalStream);
            }
        }

        private static string CalculateChecksum(MemoryStream ms)
        {
            string checksum = "";

            using(var algo = SHA1.Create())
            {
                ms.Position = 0;
                byte[] butes = algo.ComputeHash(ms);
                checksum = $"\"{WebEncoders.Base64UrlEncode(butes)}\"";
            }
            return checksum;
        }

        private bool IsEtagSupported(HttpResponse response)
        {
            if(response.StatusCode != StatusCodes.Status200OK)
            {
                Program.Logger.Error($"Ошибка подключения {response.StatusCode}");
                return false;
            }

            if(response.Body.Length > 100 * 1024)
            {
                return false;
            }

            if(response.Headers.ContainsKey(HeaderNames.ETag))
            {
                return false;
            }
            return true;
        }
    }
}
