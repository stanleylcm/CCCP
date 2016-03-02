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
        public ChecklistModel(ChecklistExtend entity)
        {
            this.Entity = entity;
        }

        #endregion

        #region Declaration

        public ChecklistExtend Entity = new ChecklistExtend();
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

        #endregion

        #region Private Method
        #endregion
    }
}
