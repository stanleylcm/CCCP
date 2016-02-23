﻿using System;
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
            init();
        }
        public IncidentSystemBillingModel(IncidentSystemBilling viewModel)
        {
            init();
            this.Entity = viewModel;
        }

        private void init()
        {
            initOptions();
        }

        #endregion

        #region Declaration

        public IncidentSystemBilling Entity = new IncidentSystemBilling();
        public List<ChecklistModel> Checklists = new List<ChecklistModel>();
        public List<Checklist> ChecklistEntities
        {
            set
            {
                Checklists = new List<ChecklistModel>();
                foreach (Checklist checklistEntity in value) Checklists.Add(new ChecklistModel(checklistEntity));
            }
        }

        public List<string> Options_ProblemArea = new List<string>();
        public List<string> Options_PossibleCause = new List<string>();
        public List<string> Options_BillingErrorSeriousness = new List<string>();
        public List<string> Options_ContactedBy = new List<string>();
        public List<string> Options_StatusUpdate = new List<string>();
        public List<string> Options_RequireMitigatingAction = new List<string>();
        public List<string> Options_MitigatingAction = new List<string>();

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

                    Entity.IncidentNo = IncidentService.GetNewIncidentNo(SequenceType.Incident, DateTime.Now.Year);

                    Entity.LevelOfSeverity = IncidentService.GetIncidentLevel(Entity) == IncidentLevel.None ? Entity.LevelOfSeverity : (Convert.ToInt32(IncidentService.GetIncidentLevel(Entity))).ToString();
                    break;
                case "LAST UPDATED":
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;

                    Entity.IncidentStatus = GetIncidentStatus().ToEnumString();

                    Entity.LevelOfSeverity = IncidentService.GetIncidentLevel(Entity) == IncidentLevel.None ? Entity.LevelOfSeverity : (Convert.ToInt32(IncidentService.GetIncidentLevel(Entity))).ToString();
                    break;
                case "CLOSED":
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    Entity.CloseById = AccessControlService.CurrentUser.Entity.UserId;
                    Entity.CloseDateTime = now;
                    break;
            }
        }

        #endregion

        #region Private Method

        private void initOptions()
        {
            this.Options_ProblemArea = InputOptionsService.GetIncidentSystemBillingInputOptions(IncidentSystemBillingInputKey.IncidentSystemBilling_ProblemArea);
            this.Options_PossibleCause = InputOptionsService.GetIncidentSystemBillingInputOptions(IncidentSystemBillingInputKey.IncidentSystemBilling_PossibleCause);
            this.Options_BillingErrorSeriousness = InputOptionsService.GetIncidentSystemBillingInputOptions(IncidentSystemBillingInputKey.IncidentSystemBilling_BillingErrorSeriousness);
            this.Options_ContactedBy = InputOptionsService.GetIncidentSystemBillingInputOptions(IncidentSystemBillingInputKey.IncidentSystemBilling_ContactedBy);
            this.Options_StatusUpdate = InputOptionsService.GetIncidentSystemBillingInputOptions(IncidentSystemBillingInputKey.IncidentSystemBilling_StatusUpdate);
            this.Options_RequireMitigatingAction = InputOptionsService.GetIncidentSystemBillingInputOptions(IncidentSystemBillingInputKey.IncidentSystemBilling_RequireMitigatingAction);
            this.Options_MitigatingAction = InputOptionsService.GetIncidentSystemBillingInputOptions(IncidentSystemBillingInputKey.IncidentSystemBilling_MitigatingAction);
        }

        #endregion
    }
}
