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
    public class ChecklistActionTemplateModel
    {
        #region Constructor

        public ChecklistActionTemplateModel()
        {
            init();
        }
        public ChecklistActionTemplateModel(ChecklistActionTemplate entity)
        {
            init();
            this.Entity = entity;
        }
        public ChecklistActionTemplateModel(int ChecklistActionTemplateId)
        {
            init();
            ChecklistActionTemplate cr = new CCCPDbContext().ChecklistActionTemplate.Find(ChecklistActionTemplateId);
            this.Entity = cr;
        }
        private void init()
        {
        }

        #endregion

        #region Declaration

        public ChecklistActionTemplate Entity = new ChecklistActionTemplate();

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
