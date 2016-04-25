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
            this.OriginalLevelOfSeverity = viewModel.LevelOfSeverity;
        }

        private void init()
        {
            initOptions();
            Checklists = new List<ChecklistModel>();
            Chatroom = new ChatRoomModel();
            LinkedIncidentEntities = new List<usp_Incident_GetLinkedIncident_Result>();
            NotificationEntities = new List<Notification>();
            LinkedGeneralEnquiryEntities = new List<usp_Incident_GetLinkedGeneralEnquiry_Result>();
        }

        #endregion

        #region Declaration

        public IncidentOHS Entity = new IncidentOHS();
        public String OriginalLevelOfSeverity { get; set; }
        public IncidentStatus IncidentStatus
        {
            get
            {
                return Entity.IncidentStatus.ToEnum<IncidentStatus>();
            }
        }
        public ChatRoomModel Chatroom { get; set; }
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
        public List<Notification> NotificationEntities { get; set; }
        public List<usp_Incident_GetLinkedGeneralEnquiry_Result> LinkedGeneralEnquiryEntities { get; set; }

        public List<string> Options_PossibleCause = new List<string>();
        public List<string> Options_OHSType = new List<string>();
        public List<string> Options_NatureOfInjury = new List<string>();
        public List<string> Options_Treatment = new List<string>();
        public String IssueBy
        {
            get
            {
                CCCPDbContext db = new CCCPDbContext();
                User user = db.User.Find(Entity.IssueById);
                if (user == null) return "";

                UserModel userModel = new UserModel(user);
                return userModel.GetLastUpdatedBy();
            }
        }
        public String CloseBy
        {
            get
            {
                CCCPDbContext db = new CCCPDbContext();
                User user = db.User.Find(Entity.CloseById);
                if (user == null) return "";

                UserModel userModel = new UserModel(user);
                return userModel.GetLastUpdatedBy();
            }
        }
        public String IsDrillMode
        {
            get
            {
                return Entity.IsDrillMode != null && Entity.IsDrillMode.Value ? "(Drill)" : "";
            }
        }

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

        public bool IsLevelChanged()
        {
            if (OriginalLevelOfSeverity == null) return false;
            if (OriginalLevelOfSeverity == "") return false;
            return !OriginalLevelOfSeverity.IsEquals(Entity.LevelOfSeverity);
        }

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
                    if (Entity.IssueById == null || Entity.IssueById == 0)
                    {
                        Entity.IssueById = AccessControlService.CurrentUser.Entity.UserId;
                        Entity.IssueDateTime = now;
                    }

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
