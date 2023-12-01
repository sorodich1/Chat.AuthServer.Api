using Microsoft.AspNetCore.SignalR;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebChatAPI.Hubs
{
    public interface ICommunicationHub
    {
        Task SendMessageAsync(string userName, string message);

        Task UpdateUsersAsync(IEnumerable<string> users);
    }
}
