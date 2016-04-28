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
    public class RoleModel
    {
        public RoleModel()
        {
            init();
        }
        public RoleModel(Role entity)
        {
            init();
            this.Entity = entity;
        }

        private void init()
        {
            RoleFunctionEntities = new List<RoleFunctionModel>();
            RoleDepartmentEntities = new List<RoleDepartmentModel>();

            FunctionEntities = new CCCPDbContext().Function.ToList();
            DepartmentEntities = new CCCPDbContext().Department.ToList();
        }

        public Role Entity = new Role();
        public List<RoleFunctionModel> RoleFunctionEntities { get; set; }
        public List<RoleDepartmentModel> RoleDepartmentEntities { get; set; }

        public List<Function> FunctionEntities { get; set; }
        public List<Department> DepartmentEntities { get; set; }

        public Boolean IsFunctionSelected(int functionId)
        {
            Boolean isSelected = false;

            foreach (RoleFunctionModel roleFunc in this.RoleFunctionEntities)
            {
                if (roleFunc.Entity.FunctionId == functionId)
                {
                    isSelected = true;
                    break;
                }
            }

            return isSelected;
        }

        public Boolean IsDepartmentSelected(int departmentId)
        {
            Boolean isSelected = false;

            foreach (RoleDepartmentModel roleModel in this.RoleDepartmentEntities)
            {
                if (roleModel.Entity.DepartmentId == departmentId)
                {
                    isSelected = true;
                    break;
                }
            }

            return isSelected;
        }

        public void PrepareSave(PrepareSaveMode saveMode = PrepareSaveMode.Last_Updated)
        {
            DateTime now = DateTime.Now;
            Entity.History = AuditTrailService.GetHistory(Entity.History, AccessControlService.CurrentUser.GetLastUpdatedBy(), now, saveMode.ToEnumString());

            switch (saveMode)
            {
                case PrepareSaveMode.Created:
                    Entity.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.CreatedDateTime = now;
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    break;
                case PrepareSaveMode.Last_Updated:
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    break;
            }
        }
    }
}
