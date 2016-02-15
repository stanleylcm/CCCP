using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.ViewModel;

namespace CCCP.Business.Model
{
    public class ChecklistActionModel
    {
        public ChecklistActionModel()
        { 
        }

        public ChecklistAction Entity = new ChecklistAction();
        public CheckListActionStatus ActionStatus
        {
            get
            {
                return Entity.ActionStatus.ToEnum<CheckListActionStatus>();
            }
            set
            {
                Entity.ActionStatus = value.ToEnumString();
            }
        }
        public bool IsCompulsory
        {
            get
            {
                return Entity.IsCompulsory.Value;
            }
        }
        public bool HasUpdate = false;

        public void ToggleActionStatus()
        {
            switch (ActionStatus)
            {
                case CheckListActionStatus.Pending: { ActionStatus = CheckListActionStatus.In_Progress; return; }
                case CheckListActionStatus.In_Progress: { ActionStatus = CheckListActionStatus.Completed; return; }
                case CheckListActionStatus.Completed: { ActionStatus = CheckListActionStatus.Pending; return; }
            }

            HasUpdate = true;
        }
    }
}
