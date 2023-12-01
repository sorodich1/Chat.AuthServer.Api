using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.SignalR.Hubs
{
    public class ChatManager
    {
        public List<ChatUsers> Users { get; set; }

        public void ConnectUser(string username, string connectionId)
        {
            var userAlreadyExist = GetConnectedUserByName(username);
            if (userAlreadyExist != null)
            {
                userAlreadyExist.AppendConnection(connectionId);
                return;
            }
            var user = new ChatUsers(username);
            user.AppendConnection(connectionId);
            Users.Add(user);
        }

        public bool DisconnectUser(string connectionId)
        {
            var userExist = GetConnectedUserById(connectionId);
            if(userExist == null)
            {
                return false;
            }
            if(!userExist.Connections.Any())
            {
                return false;
            }
            var connectionExists = userExist.Connections.Select(x => x.ConnectedId).First().Equals(connectionId);
            if(!connectionExists)
            {
                return false;
            }
            if(userExist.Connections.Count() == 1)
            {
                Users.Remove(userExist);
                return true;
            }

            userExist.RemoveConnection(connectionId);
            return false;
        }


        private ChatUsers GetConnectedUserById(string connectedId) 
        {
            return Users
                .FirstOrDefault(x => x.Connections.Select(c => c.ConnectedId)
                .Contains(connectedId));
        }

        private ChatUsers GetConnectedUserByName(string userName)
        {
            return Users
                .FirstOrDefault(x => string.Equals(
                    x.UserName,
                    userName,
                    StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
