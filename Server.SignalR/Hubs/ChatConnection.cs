using System;

namespace Server.SignalR.Hubs
{
    public class ChatConnection
    {
        public DateTime ConnectedAt { get; set; }

        public string ConnectedId { get; set; } = null!;
    }
}
