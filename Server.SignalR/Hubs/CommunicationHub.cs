using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.SignalR.Hubs
{
    //[Authorize]
    public class CommunicationHub : Hub<ICommunicationHub>
    {
        private readonly ChatManager _chatManager;
        private const string _defaultGroupName = "General";

        public CommunicationHub(ChatManager chatManager)
        {
            _chatManager = chatManager;
        }

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User?.Identity?.Name ?? "Anonymous";
            var connectedId = Context.ConnectionId;
            _chatManager.ConnectUser(userName, connectedId);
            await Groups.AddToGroupAsync(userName, _defaultGroupName);
            await UpdateUsersAsync();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var isUserRemoved = _chatManager.DisconnectUser(Context.ConnectionId);
            if (!isUserRemoved)
            {
                await base.OnDisconnectedAsync(exception);
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _defaultGroupName);
            await UpdateUsersAsync();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task UpdateUsersAsync()
        {
            var users = _chatManager.Users.Select(x => x.UserName).ToList();
            await Clients.All.UpdateUserAsync(users);
        }

        public async Task SendMessageAsync(string username, string message)
        {
            await Clients.All.SendMessageAsync(username, message);
        }
    }
}
