using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.ViewModel;
using CCCP.Common;
using CCCP.Business.Service;

namespace CCCP.Business.Model
{
    public class RoleFunctionModel
    {
        public RoleFunctionModel()
        {
            init();
        }
        public RoleFunctionModel(RoleFunction entity)
        {
            init();
            this.Entity = entity;
        }

        private void init()
        {
            IsDeleted = false;
        }

        public RoleFunction Entity = new RoleFunction();
        public Boolean IsDeleted { get; set; }
    }
}
