using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.SignalR.Hubs
{
    public class ChatUsers
    {
        private readonly List<ChatConnection> _connection;

        public ChatUsers(string userName)
        {
            UserName = userName ?? throw new Exception(nameof(userName));
            _connection = new List<ChatConnection>();
        }

        public string UserName { get; set; }
        public IEnumerable<ChatConnection> Connections => _connection;

        public DateTime? ConnectedAd
        {
            get
            {
                if(Connections.Any())
                {
                    return Connections
                        .OrderByDescending(x => x.ConnectedAt)
                        .Select(x => x.ConnectedAt)
                        .First();
                }
                return null;
            }
        }

        public void AppendConnection(string connectionId)
        {
            if(connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }
            var connection = new ChatConnection
            {
                ConnectedAt = DateTime.UtcNow,
                ConnectedId = connectionId
            };

            _connection.Add(connection);
        }

        public void RemoveConnection(string connectionId)
        {
            if(connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }
            var connection = _connection.SingleOrDefault(x => x.ConnectedId.Equals(connectionId));
            if(connection == null)
            {
                return;
            }
            _connection.Remove(connection);
        }
    }
}
