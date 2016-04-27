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
    public class RoleDepartmentModel
    {
        public RoleDepartmentModel()
        {
            init();
        }
        public RoleDepartmentModel(RoleDepartment entity)
        {
            init();
            this.Entity = entity;
        }

        private void init()
        {
            IsDeleted = false;
        }

        public RoleDepartment Entity = new RoleDepartment();
        public Boolean IsDeleted { get; set; }
    }
}
