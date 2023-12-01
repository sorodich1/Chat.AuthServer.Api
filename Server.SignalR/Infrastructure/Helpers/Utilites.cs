using Calabonga.Microservices.Core.Exceptions;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.SignalR.Infrastructure.Helpers
{
    public static class Utilites
    {
        /// <summary>
        /// Вычисление Хэша
        /// </summary>
        /// <param name="filePathh"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static byte[] ComputeHash(string filePathh)
        {
            var runCount = 1;
            while(runCount < 4)
            {
                try
                {
                    if(!File.Exists(filePathh))
                    {
                        throw new FileNotFoundException();
                    }
                }
                catch(IOException ex)
                {
                    if(runCount == 3 || ex.HResult != -2147024864)
                    {
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(Math.Pow(2, runCount)));
                        runCount++;
                    }
                }
            }
            return new byte[20];
        }

        /// <summary>
        /// Возвращение содержимое файла
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<string> GetFileContent(string filePath)
        {
            try
            {
                if(!File.Exists(filePath))
                {
                    throw new FileNotFoundException();
                }
                return await File.ReadAllTextAsync(filePath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Удаление файла из каталога
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            try
            {
                if(!File.Exists(filePath))
                {
                    throw new FileNotFoundException();
                }
                File.Delete(filePath);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Сохранение в файл
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task SetFileContent(string filePath, string content)
        {
            try
            {
                var folder = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                if (File.Exists(filePath))
                {
                    throw new MicroserviceArgumentNullException();
                }

                using(var fs = File.Create(filePath))
                {
                    var info = new UTF8Encoding(true).GetBytes(content);
                    await fs.WriteAsync(info, 0, info.Length);
                }
            }
            catch
            {

            }
        }

        public static string GetETag(string key, byte[] content)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var combinedBytes = Combine(keyBytes, content);
            return GenerateETag(combinedBytes);
        }

        /// <summary>
        /// Возвращаем путь к папке
        /// </summary>
        /// <returns></returns>
        public static string GetWorkingFolder()
        {
            var location = Assembly.GetEntryAssembly().Location;
            return Path.GetDirectoryName(location);
        }

        private static string GenerateETag(byte[] data)
        {
            using(var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                var hex = BitConverter.ToString(hash);
                return hex.Replace("_", "");
            }
        }

        private static byte[] Combine(byte[] a, byte[] b)
        {
            var c = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, c, 0, a.Length);
            Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
            return c;
        }
    }
}
