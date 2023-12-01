using Microsoft.Extensions.Logging;
using System;

namespace Server.Auth.Helpers
{
    internal static class LoggerExtensions
    {
        internal static void MicroserviceUserRegistration(this ILogger source, string userName, Exception? exception = null)
        {
            switch (exception)
            {
                case null:
                    UserRegistrationExecute(source, userName, exception);
                    break;
                default:
                    UserRegistrationFailedExecute(source, userName, exception);
                    break;
            }
        }

        private static readonly Action<ILogger, string, Exception?> UserRegistrationExecute =
            LoggerMessage.Define<string>(LogLevel.Information, EventNumbers.UserRegistrationId,
                "Пользователь {userName} успешно зарегистрирован");

        private static readonly Action<ILogger, string, Exception?> UserRegistrationFailedExecute =
            LoggerMessage.Define<string>(LogLevel.Information, EventNumbers.UserRegistrationId,
        "Пользователь {userName} успешно зарегистрирован");
    }
    static class EventNumbers
    {
        internal static readonly EventId UserRegistrationId = new EventId(9001, nameof(UserRegistrationId));
        internal static readonly EventId PostItemId = new EventId(9002, nameof(PostItemId));
    }
}
