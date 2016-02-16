using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CCCP.Hubs
{
    public class IncidentEnquiryHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}