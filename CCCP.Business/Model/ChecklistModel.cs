using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.ViewModel;

namespace CCCP.Business.Model
{
    public class ChecklistModel
    {
        public ChecklistModel()
        {
        }

        public Checklist Entity = new Checklist();
        public List<ChecklistActionModel> Actions = new List<ChecklistActionModel>();
        public bool HasUpdate
        {
            get
            {
                foreach (ChecklistActionModel action in Actions) if (action.HasUpdate) return true;
                return false;
            }
        }
    }
}
