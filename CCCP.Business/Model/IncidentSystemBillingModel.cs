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
        public void PrepareSave(string saveMode = "Last updated")
        {
            DateTime now = DateTime.Now;
            Entity.History = AuditTrailService.GetHistory(Entity.History, AccessControlService.CurrentUser.GetLastUpdatedBy(), now, saveMode);

            switch (saveMode.ToUpper())
            {
                case "CREATED":
                    Entity.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.CreatedDateTime = now;
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    Entity.IssueById = AccessControlService.CurrentUser.Entity.UserId;
                    Entity.IssueDateTime = now;

                    CheckLevel();
                    break;
                case "LAST UPDATED":
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    break;
                case "CLOSED":
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    Entity.CloseById = AccessControlService.CurrentUser.Entity.UserId;
                    Entity.CloseDateTime = now;
                    break;
            }
        }

        public void CheckLevel()
        {
            IncidentLevel level = (IncidentLevel)Convert.ToInt32(Entity.LevelOfSeverity);
            if (level < IncidentLevel.Level3 && Entity.ContactedBy.IsContains(IncidentSystemBillingContactedBy.Media.ToEnumString()))
            {
                Entity.LevelOfSeverity = Convert.ToInt32(IncidentLevel.Level3).ToString();
            }
            else if (level < IncidentLevel.Level2 && Entity.BillingErrorSeriousness.IsEquals(IncidentSystemBillingBillingErrorSeriousness.Danger_Zone.ToEnumString()) && 
                (
                    Entity.ContactedBy.IsContains(IncidentSystemBillingContactedBy.Consumer_Council.ToEnumString()) ||
                    Entity.ContactedBy.IsContains(IncidentSystemBillingContactedBy.Government.ToEnumString())
                ))
            {
                Entity.LevelOfSeverity = Convert.ToInt32(IncidentLevel.Level2).ToString();
            }
        }

        #endregion
    }
}
