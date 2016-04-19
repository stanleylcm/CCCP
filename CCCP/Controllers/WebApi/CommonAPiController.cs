using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CCCP.ViewModel;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.Common;

namespace CCCP.Controllers.WebApi
{
    public class CommonApiController : ApiController
    {
        private CCCPDbContext db = new CCCPDbContext();

        // compress image

        // compress video

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public Boolean SendChatRoomMessage(string name, string message, string time, int userId, int chatRoomId, object[] files)
        {
            //new CCCP.Hubs.ChatRoomHub().Send(name, message, time, userId, chatRoomId, files);
            return true;
        }
    }
}