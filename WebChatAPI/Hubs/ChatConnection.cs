using System;

namespace WebChatAPI.Hubs
{
    public class ChatConnection
    {
        public DateTime ConnectedAt { get; set; }

        public string ConnectionId { get; set; } = null!;
    }
}
