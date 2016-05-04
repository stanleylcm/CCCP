using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.Business.Model;
using CCCP.ViewModel;

namespace CCCP.Business.Service
{
    public class ChatRoomService
    {   
        public static int SaveChatMessage(int chatRoomId, int userId, String message, DateTime sendDateTime)
        {
            CCCPDbContext db = new CCCPDbContext();
            UserModel user = new UserModel(db.User.Find(userId));
            ChatRoomMessageModel msgModel = new ChatRoomMessageModel();

            msgModel.Entity.ChatRoomId = chatRoomId;
            msgModel.Entity.SenderUserId = userId;
            msgModel.Entity.SenderDisplayName = user.Entity.DisplayName;
            msgModel.Entity.SendDateTime = sendDateTime;
            msgModel.Entity.Message = message;
            msgModel.Entity.CreatedBy = user.GetLastUpdatedBy();
            msgModel.Entity.CreatedDateTime = DateTime.Now;
            msgModel.Entity.LastUpdatedBy = user.GetLastUpdatedBy();
            msgModel.Entity.LastUpdatedDateTime = DateTime.Now;

            db.ChatRoomMessage.Add(msgModel.Entity);
            db.SaveChanges();

            return msgModel.Entity.ChatRoomMessageId;
        }

        public static int SaveChatAttachment(int messageId, int userId, string fileType, byte[] content)
        {
            CCCPDbContext db = new CCCPDbContext();
            UserModel user = new UserModel(db.User.Find(userId));
            ChatRoomAttachment attach = new ChatRoomAttachment();

            attach.ChatRoomAttachmentId = 0;
            attach.ChatRoomMessageId = messageId;
            attach.Attachment = content;
            attach.AttachmentType = fileType;
            attach.CreatedBy = user.GetLastUpdatedBy();
            attach.CreatedDateTime = DateTime.Now;
            attach.LastUpdatedBy = user.GetLastUpdatedBy();
            attach.LastUpdatedDateTime = DateTime.Now;

            db.ChatRoomAttachment.Add(attach);
            db.SaveChanges();

            return attach.ChatRoomAttachmentId;
        }

        public static int SaveSystemMessage(int chatRoomId, int userId, String message, DateTime sendDateTime)
        {
            CCCPDbContext db = new CCCPDbContext();
            UserModel user = new UserModel(db.User.Find(userId));
            ChatRoomMessage crMsg = new ChatRoomMessage();

            crMsg.ChatRoomId = chatRoomId;
            crMsg.SenderUserId = 0; // userId = 0 as system message?
            crMsg.SenderDisplayName = "System Message";
            crMsg.SendDateTime = sendDateTime;
            crMsg.Message = message;
            crMsg.CreatedBy = user.GetLastUpdatedBy();
            crMsg.CreatedDateTime = DateTime.Now;
            crMsg.LastUpdatedBy = user.GetLastUpdatedBy();
            crMsg.LastUpdatedDateTime = DateTime.Now;

            db.ChatRoomMessage.Add(crMsg);
            db.SaveChanges();

            return crMsg.ChatRoomMessageId;
        }
    }
}
