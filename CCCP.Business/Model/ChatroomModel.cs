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
    public class ChatRoomModel
    {
        #region Constructor

        public ChatRoomModel()
        {
        }
        public ChatRoomModel(ChatRoom entity)
        {
            this.Entity = entity;
        }
        public ChatRoomModel(int ChatRoomId)
        {
            ChatRoom cr = new CCCPDbContext().ChatRoom.Find(ChatRoomId);
            this.Entity = cr;
        }

        #endregion

        #region Declaration

        public ChatRoom Entity = new ChatRoom();
        public List<ChatRoomMessageModel> ChatRoomMessagesEntites = new List<ChatRoomMessageModel>();

        #endregion 

        #region Public Method
        
        #endregion

        #region Private Method

        #endregion
    }
}
