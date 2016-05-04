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
using System.Web.Script.Serialization;
using CCCP.Hubs;
using Microsoft.AspNet.SignalR;
using System.Collections.Specialized;
using System.Web.Security;

namespace CCCP.Controllers.WebApi
{
    public class CommonApiController : ApiController
    {
        private CCCPDbContext db = new CCCPDbContext();

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int SendChatRoomMessage(string name, string message, string time, int userId, int chatRoomId, IEnumerable<HttpPostedFileBase> uploadData)
        {
            JsonResult result = new CCCP.Controllers.ChatRoomController().SaveChatRoomMessageAttachment(chatRoomId, userId, name, Convert.ToDateTime(time), message, uploadData);
            var serializer = new JavaScriptSerializer();

            dynamic response = serializer.Deserialize(serializer.Serialize(result.Data), typeof(object));

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatRoomHub>();
            context.Clients.All.broadcastMessage(name, message, response["sendDateTime"], chatRoomId, response["id"]);

            return response["id"];
        }

        [System.Web.Http.HttpPost]
        public int SendChatRoomAttachment()
        {
            List<HttpPostedFileBase> uploadList = new List<HttpPostedFileBase>();
            HttpPostedFile file = HttpContext.Current.Request.Files["file"];
            HttpPostedFileBase fileBase = new HttpPostedFileWrapper(file);
            uploadList.Add(fileBase);
            
            string tempValue = "";
            NameValueCollection parameters = HttpContext.Current.Request.Params;
            string[] param1 = parameters.GetValues("chatRoomId");
            for (int len = 0; len < param1.Length; len++)
            {
                tempValue += param1[len].ToString();
            }
            int chatRoomId = Convert.ToInt32(tempValue);

            string[] param2 = parameters.GetValues("userId");
            tempValue = "";
            for (int len = 0; len < param2.Length; len++)
            {
                tempValue += param2[len].ToString();
            }
            int userId = Convert.ToInt32(tempValue);

            string name = new CCCPDbContext().User.Find(userId).DisplayName;
            string time = (DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss");
            string message = "";

            return SendChatRoomMessage(name, message, time, userId, chatRoomId, uploadList);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<ChatRoomMessageModel> GetLatestMessages(int chatRoomId, int lastMessageId)
        {
            CCCPDbContext db = new CCCPDbContext();

            List<ChatRoomMessageModel> messageList = (from crMessage in db.ChatRoomMessage
                                                      where crMessage.ChatRoomId.Equals(chatRoomId) && crMessage.ChatRoomMessageId > lastMessageId
                                                      orderby crMessage.SendDateTime
                                                      select new ChatRoomMessageModel()
                                                      {
                                                          Entity = crMessage
                                                      }).ToList<ChatRoomMessageModel>();

            foreach (ChatRoomMessageModel crMessage in messageList)
            {
                crMessage.ChatRoomAttachmentsEntities = (from crAttachment in db.ChatRoomAttachment
                                                         where crAttachment.ChatRoomMessageId.Equals(crMessage.Entity.ChatRoomMessageId)
                                                         select crAttachment).ToList<ChatRoomAttachment>();
            }

            return messageList;
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int SendChatRoomSystemMessage(string message, string time, int userId, int chatRoomId)
        {
            int msgId = ChatRoomService.SaveSystemMessage(chatRoomId, userId, message, Convert.ToDateTime(time));

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatRoomHub>();
            context.Clients.All.broadcastMessage("System Message", message, time, chatRoomId, msgId);

            return msgId;
        }
    }
}