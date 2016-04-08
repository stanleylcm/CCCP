using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CCCP.Hubs
{
    public class ChatRoomHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string name, string message, string time)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message, time);
        }

        public void SendImage(string name, string imageUrl, string imageResizeUrl, string time)
        {
            Clients.All.broadcastImageMessage(name, imageUrl, imageResizeUrl, time);
        }

        public void SendVideo(string name, string videoUrl, string time)
        {
            Clients.All.broadcastVideoMessage(name, videoUrl, time);
        }
    }
}