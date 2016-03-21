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
    public class IncidentOHSModel : IIncidentModel
    {
        #region Constructor

        public IncidentOHSModel()
        {
            init();
        }
        public IncidentOHSModel(IncidentOHS viewModel)
        {
            init();
            this.Entity = viewModel;
        }

        private void init()
        {
            initOptions();
            Checklists = new List<ChecklistModel>();
        }

        #endregion

        #region Declaration

        public IncidentOHS Entity = new IncidentOHS();
        public IncidentStatus IncidentStatus
        {
            get
            {
                return Entity.IncidentStatus.ToEnum<IncidentStatus>();
            }
        }
        public List<ChecklistModel> Checklists { get; set; }
        public List<ChecklistExtend> ChecklistEntities
        {
            set
            {
                Checklists = new List<ChecklistModel>();
                foreach (ChecklistExtend checklistEntity in value) Checklists.Add(new ChecklistModel(checklistEntity));
            }
        }

        public List<string> Options_PossibleCause = new List<string>();
        public List<string> Options_OHSType = new List<string>();
        public List<string> Options_NatureOfInjury = new List<string>();
        public List<string> Options_Treatment = new List<string>();

        public bool AllowEdit
        {
            get
            {
                return (IncidentStatus != Common.IncidentStatus.Cancelled && IncidentStatus != Common.IncidentStatus.Closed );
            }
        }
        public bool AllowCancel
        {
            get
            {
                return (IncidentStatus != Common.IncidentStatus.Cancelled && IncidentStatus != Common.IncidentStatus.Closed);
            }
        }

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
            this.Options_PossibleCause = InputOptionsService.GetIncidentOHSInputOptions(IncidentOHSInputKey.IncidentOHS_PossibleCause);
            this.Options_OHSType = InputOptionsService.GetIncidentOHSInputOptions(IncidentOHSInputKey.IncidentOHS_OHSType);
            this.Options_NatureOfInjury = InputOptionsService.GetIncidentOHSInputOptions(IncidentOHSInputKey.IncidentOHS_NatureOfInjury);
            this.Options_Treatment = InputOptionsService.GetIncidentOHSInputOptions(IncidentOHSInputKey.IncidentOHS_Treatment);
        }

        #endregion
    }
}
