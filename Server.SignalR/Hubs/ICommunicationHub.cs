using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.SignalR.Hubs
{
    public interface ICommunicationHub
    {
        Task SendMessageAsync(string userName, string message);

        Task UpdateUserAsync(IEnumerable<string> users);
    }
}
