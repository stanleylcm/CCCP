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
        public void Send(string name, string message, string time, int userId, int chatRoomId)
        {
            int messageId = ChatRoomService.SaveChatMessage(chatRoomId, userId, message, Convert.ToDateTime(time));
            Clients.All.broadcastMessage(name, message, time, chatRoomId, messageId);
        }

        public void SendImage(string name, string imageUrl, string imageResizeUrl, string time, int userId, int chatRoomId)
        {
            int messageId = 0;
            Clients.All.broadcastImageMessage(name, imageUrl, imageResizeUrl, time, chatRoomId, messageId);
        }

        public void SendVideo(string name, string videoUrl, string time, int userId, int chatRoomId)
        {
            int messageId = 0;
            Clients.All.broadcastVideoMessage(name, videoUrl, time, chatRoomId, messageId);
        }
    }
}