using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.ViewModel;

namespace CCCP.Business.Model
{
    public class UserModel
    {
        public UserModel()
        { 
        }
        public UserModel(User entity)
        {
            this.Entity = entity;
        }

        public User Entity = new User();
        public bool IsAuthenticated;
        public DateTime LoginDateTime = DateTime.MinValue;
        public AccessRightsModel AccessRights = new AccessRightsModel();
        
        public string GetLastUpdatedBy()
        {
            if (Entity.UserId <= 0)
            {
                // from web api, no login info, use 1 as System acc?
                Entity = new CCCPDbContext().User.Find(1);
            }

            return string.Format("{0} ({1})", Entity.LoginName, Entity.DisplayName);
        }
    }
}
