using Calabonga.Microservices.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.SignalR.Infrastructure.Helpers
{
    public static class EmailHelper
    {
        public static IEnumerable<string> GetEmailAddresses(string emails)
        {
            if(string.IsNullOrWhiteSpace(emails))
            {
                return null;
            }
            var split = emails.Split(new[] { ';', '|', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return split.Where(x => x.IsEmail());
        }
    }
}
