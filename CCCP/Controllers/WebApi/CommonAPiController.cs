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
using Newtonsoft.Json;

namespace CCCP.Controllers.WebApi
{
    public class CommonApiController : ApiController
    {
        private CCCPDbContext db = new CCCPDbContext();

        // compress image

        // compress video

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public Boolean SendChatRoomMessage(string name, string message, string time, int userId, int chatRoomId, IEnumerable<HttpPostedFileBase> uploadData)
        {
            JsonResult result = new CCCP.Controllers.ChatRoomController().SaveChatRoomMessageAttachment(chatRoomId, userId, name, Convert.ToDateTime(time), message, uploadData);

            dynamic response = Newtonsoft.Json.JsonConvert.DeserializeObject(result.ToString());
            new CCCP.Hubs.ChatRoomHub().Send(name, message, time, userId, chatRoomId, response.id);
            
            return true;
        }
    }
}