using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.ViewModel;
using CCCP.Business.Service;

namespace CCCP.Business.Model
{
    public class ChatRoomMessageModel
    {
        #region Constructor

        public ChatRoomMessageModel()
        {
        }
        public ChatRoomMessageModel(ChatRoomMessage entity)
        {
            this.Entity = entity;
        }
        public ChatRoomMessageModel(int ChatRoomMessageId)
        {
            ChatRoomMessage crMessage = new CCCPDbContext().ChatRoomMessage.Find(ChatRoomMessageId);
            this.Entity = crMessage;
        }

        #endregion

        #region Declaration

        public ChatRoomMessage Entity = new ChatRoomMessage();
        public List<ChatRoomAttachment> ChatRoomAttachmentsEntities = new List<ChatRoomAttachment>();

        #endregion 

        #region Public Method
        
        #endregion

        #region Private Method

        #endregion
    }
}
