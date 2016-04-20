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

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int SendChatRoomMessage(string name, string message, string time, int userId, int chatRoomId, IEnumerable<HttpPostedFileBase> uploadData)
        {
            JsonResult result = new CCCP.Controllers.ChatRoomController().SaveChatRoomMessageAttachment(chatRoomId, userId, name, Convert.ToDateTime(time), message, uploadData);

            dynamic response = Newtonsoft.Json.JsonConvert.DeserializeObject(result.ToString());
            new CCCP.Hubs.ChatRoomHub().Send(name, message, time, userId, chatRoomId, response.id);
            
            return response.id;
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
    }
}