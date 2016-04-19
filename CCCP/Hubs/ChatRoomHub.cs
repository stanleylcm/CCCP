using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CCCP.Business.Service;

namespace CCCP.Hubs
{
    public class ChatRoomHub : Hub
    {
        public void Send(string name, string message, string time, int userId, int chatRoomId, int messageId)
        {
            Clients.All.broadcastMessage(name, message, time, chatRoomId, messageId);
        }
    }
}