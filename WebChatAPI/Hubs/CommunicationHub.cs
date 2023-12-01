using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebChatAPI.Infrastructure.Auth;

namespace WebChatAPI.Hubs
{
    [Authorize]
    public class CommunicationHub : Hub<ICommunicationHub>
    {
        private readonly ChatManager _chatManager;
        private readonly IAccountService _accountService;
        private const string groupGeneral = "General";

        public CommunicationHub(ChatManager chatManager, IAccountService accountService)
        {
            _chatManager = chatManager;
            _accountService = accountService;
    }

        public override async Task OnConnectedAsync()
        {
            var connectedId = Context.ConnectionId;
            var principal = await  _accountService.GetUserClaimsAsync(Program.ttt);

            var userName = principal?.Identity?.Name ?? "Anonimus";

           //var userName = Program.Users;

            _chatManager.ConnectUser(userName, connectedId);
            await Groups.AddToGroupAsync(userName, connectedId);
            await UpdateUserAsync();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var isUserRemoved = _chatManager.DisconnectUser(Context.ConnectionId);
            if(!isUserRemoved)
            {
                await base.OnDisconnectedAsync(exception);
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupGeneral);
            await UpdateUserAsync();
            await OnDisconnectedAsync(exception);
        }

        public async Task UpdateUserAsync()
        {
            var users = _chatManager.Users.Select(x => x.UserName).ToList();
            await Clients.All.UpdateUsersAsync(users);
        }

        public async Task SendMessageAsync(string userName, string message)
        {
            await Clients.Caller.SendMessageAsync(userName, message);
        }
    }
}
