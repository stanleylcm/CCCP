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
    public class IncidentSystemBillingModel
    {
        #region Constructor

        public IncidentSystemBillingModel()
        { 
        }
        public IncidentSystemBillingModel(IncidentSystemBilling viewModel)
        {
            this.Entity = viewModel;
        }

        private void init()
        {
            this.IncidentStatus = Entity.IncidentStatus.ToEnum<IncidentStatus>();
        }

        #endregion

        #region Declaration

        public IncidentSystemBilling Entity = new IncidentSystemBilling();
        public IncidentStatus IncidentStatus = IncidentStatus.Pending;
        public List<ChecklistModel> Checklists = new List<ChecklistModel>();
        public List<Checklist> ChecklistEntities
        {
            set
            {
                Checklists = new List<ChecklistModel>();
                foreach (Checklist checklistEntity in value) Checklists.Add(new ChecklistModel(checklistEntity));
            }
        }

        #endregion

        #region Public Method

        public bool IsReadyForClose()
        {
            // check checklist's compulsory actions had been completed or not
            foreach (ChecklistModel checklist in Checklists) if (!checklist.IsAllCompulsoryCompleted()) return false;

            return true;
        }
        public IncidentStatus GetIncidentStatus()
        {
            // original status
            IncidentStatus result = Entity.IncidentStatus.ToEnum<IncidentStatus>();

            // Ready For Close
            if (IsReadyForClose()) result = IncidentStatus.Ready_For_Close;

            return result;
        }
        public void PrepareSave()
        {
            DateTime updateDateTime = DateTime.Now;

            // incident details
            Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            Entity.LastUpdatedDateTime = updateDateTime;

            // check lists
            foreach (ChecklistModel checklist in Checklists) if (checklist.HasUpdate()) checklist.PrepareSave(updateDateTime);

            // chat room
        }

        #endregion
    }
}
