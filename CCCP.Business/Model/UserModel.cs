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
        public bool IsAuthenticated = false;
        
        public string GetLastUpdatedBy()
        {
            return string.Format("{0} ({1})", Entity.LoginName, Entity.DisplayName);
        }
    }
}
