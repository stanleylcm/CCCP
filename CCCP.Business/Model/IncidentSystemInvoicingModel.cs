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
    public class IncidentSystemInvoicingModel : IIncidentModel
    {
        #region Constructor

        public IncidentSystemInvoicingModel()
        {
            init();
        }
        public IncidentSystemInvoicingModel(IncidentSystemInvoicing viewModel)
        {
            init();
            this.Entity = viewModel;
        }

        private void init()
        {
            initOptions();
            Checklists = new List<ChecklistModel>();
            LinkedIncidentEntities = new List<usp_Incident_GetLinkedIncident_Result>();
        }

        #endregion

        #region Declaration

        public IncidentSystemInvoicing Entity = new IncidentSystemInvoicing();
        public List<ChecklistModel> Checklists { get; set; }
        public List<ChecklistExtend> ChecklistEntities
        {
            set
            {
                Checklists = new List<ChecklistModel>();
                foreach (ChecklistExtend checklistEntity in value) Checklists.Add(new ChecklistModel(checklistEntity));
            }
        }
        public List<usp_Incident_GetLinkedIncident_Result> LinkedIncidentEntities { get; set; }

        public List<string> Options_ProblemArea = new List<string>();
        public List<string> Options_PossibleCause = new List<string>();
        public List<string> Options_ContactedBy = new List<string>();
        public List<string> Options_Impact = new List<string>();
        public List<string> Options_StatusUpdate = new List<string>();
        public List<string> Options_RequireMitigatingAction = new List<string>();
        public List<string> Options_MitigatingAction = new List<string>();

        #endregion

        #region Public Method

        public bool IsReadyForClose()
        {
            return IncidentService.IsReadyForClose(this);
        }
        public IncidentStatus GetIncidentStatus()
        {
            IncidentStatus originalStatus = Entity.IncidentStatus.ToEnum<IncidentStatus>();
            return IncidentService.GetIncidentStatus(this, originalStatus);
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
                    Entity.IssueById = AccessControlService.CurrentUser.Entity.UserId;
                    Entity.IssueDateTime = now;

                    Entity.IncidentNo = IncidentService.GetNewIncidentNo(SequenceType.Incident, DateTime.Now.Year);

                    Entity.LevelOfSeverity = IncidentService.GetIncidentLevel(Entity) == IncidentLevel.None ? Entity.LevelOfSeverity : (Convert.ToInt32(IncidentService.GetIncidentLevel(Entity))).ToString();
                    break;
                case PrepareSaveMode.Last_Updated:
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;

                    Entity.IncidentStatus = GetIncidentStatus().ToEnumString();

                    Entity.LevelOfSeverity = IncidentService.GetIncidentLevel(Entity) == IncidentLevel.None ? Entity.LevelOfSeverity : (Convert.ToInt32(IncidentService.GetIncidentLevel(Entity))).ToString();
                    break;
                case PrepareSaveMode.Closed:
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
            this.Options_ProblemArea = InputOptionsService.GetIncidentSystemInvoicingInputOptions(IncidentSystemInvoicingInputKey.IncidentSystemInvoicing_ProblemArea);
            this.Options_PossibleCause = InputOptionsService.GetIncidentSystemInvoicingInputOptions(IncidentSystemInvoicingInputKey.IncidentSystemInvoicing_PossibleCause);
            this.Options_ContactedBy = InputOptionsService.GetIncidentSystemInvoicingInputOptions(IncidentSystemInvoicingInputKey.IncidentSystemInvoicing_ContactedBy);
            this.Options_Impact = InputOptionsService.GetIncidentSystemInvoicingInputOptions(IncidentSystemInvoicingInputKey.IncidentSystemInvoicing_Impact);
            this.Options_StatusUpdate = InputOptionsService.GetIncidentSystemInvoicingInputOptions(IncidentSystemInvoicingInputKey.IncidentSystemInvoicing_StatusUpdate);
            this.Options_RequireMitigatingAction = InputOptionsService.GetIncidentSystemInvoicingInputOptions(IncidentSystemInvoicingInputKey.IncidentSystemInvoicing_RequireMitigatingAction);
            this.Options_MitigatingAction = InputOptionsService.GetIncidentSystemInvoicingInputOptions(IncidentSystemInvoicingInputKey.IncidentSystemInvoicing_MitigatingAction);
        }

        #endregion
    }
}
