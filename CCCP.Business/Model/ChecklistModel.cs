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
    public class ChecklistModel
    {
        #region Constructor

        public ChecklistModel()
        {
        }
        public ChecklistModel(Checklist entity)
        {
            this.Entity = entity;
        }

        #endregion

        #region Declaration

        public Checklist Entity = new Checklist();
        public List<ChecklistActionModel> ChecklistActions = new List<ChecklistActionModel>();
        public List<ChecklistAction> ChecklistActionEntities
        {
            set
            {
                ChecklistActions = new List<ChecklistActionModel>();
                foreach (ChecklistAction actionEntity in value) ChecklistActions.Add(new ChecklistActionModel(actionEntity));
            }
        }

        #endregion 

        #region Public Method

        public bool HasUpdate()
        {
            foreach (ChecklistActionModel action in ChecklistActions) if (action.HasUpdate) return true;
            return false;
        }
        public bool IsAllCompulsoryCompleted()
        {
            foreach (ChecklistActionModel checklistAction in ChecklistActions)
            {
                if (checklistAction.IsCompulsory && checklistAction.ActionStatus != CheckListActionStatus.Completed) return false;
            }
            return true;
        }
        public void PrepareSave(DateTime updateDateTime)
        {
            // check list actions
            foreach (ChecklistActionModel action in ChecklistActions) if (action.HasUpdate) action.PrepareSave(updateDateTime);

            // last updated
            Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            Entity.LastUpdatedDateTime = updateDateTime;
        }

        #endregion

        #region Private Method
        #endregion
    }
}
