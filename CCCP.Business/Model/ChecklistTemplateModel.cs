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
    public class ChecklistTemplateModel
    {
        #region Constructor

        public ChecklistTemplateModel()
        {
            init();
        }
        public ChecklistTemplateModel(ChecklistTemplate entity)
        {
            init();
            this.Entity = entity;
        }
        public ChecklistTemplateModel(int ChecklistTemplateId)
        {
            init();
            ChecklistTemplate cr = new CCCPDbContext().ChecklistTemplate.Find(ChecklistTemplateId);
            this.Entity = cr;
        }
        private void init()
        {

        }

        #endregion

        #region Declaration

        public ChecklistTemplate Entity = new ChecklistTemplate();
        public List<ChecklistActionTemplateModel> ChecklistActionTemplateEntites = new List<ChecklistActionTemplateModel>();

        #endregion 

        #region Public Method
        public void PrepareSave(PrepareSaveMode saveMode = PrepareSaveMode.Last_Updated)
        {
            DateTime now = DateTime.Now;

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
        #endregion

        #region Private Method

        #endregion
    }
}
