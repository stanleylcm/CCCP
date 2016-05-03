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
    public class IncidentSystemOTSystemModel : IIncidentModel
    {
        #region Constructor

        public IncidentSystemOTSystemModel()
        {
            init();
        }
        public IncidentSystemOTSystemModel(IncidentSystemOTSystem viewModel)
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
            CrisisEntity = new CrisisModel();
        }

        #endregion

        #region Declaration

        public IncidentSystemOTSystem Entity = new IncidentSystemOTSystem();
        public String OriginalLevelOfSeverity { get; set; }
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
        public CrisisModel CrisisEntity { get; set; }

        public List<string> Options_AffectedSystem = new List<string>();
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
        public String CrisisRejectReason
        {
            get
            {
                if (Entity.CrisisId != null && Entity.CrisisId.Value > 0)
                {
                    return CrisisEntity.Entity.RejectReason;
                }

                return "";
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
        public bool IsAbleToEscalate()
        {
            if (Entity.CrisisId != null && Entity.CrisisId.Value > 0)
            {
                if (this.CrisisEntity.Entity.Status != CrisisStatus.Rejected.ToEnumString())
                    return false;
            }
            if (Entity.IncidentStatus == IncidentStatus.Cancelled.ToEnumString()) return false;
            if (Entity.IncidentStatus == IncidentStatus.Closed.ToEnumString()) return false;
            return true;
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

                    Entity.IncidentStatus = IncidentStatus.Pending.ToEnumString();
                    break;
                case PrepareSaveMode.Last_Updated:
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;

                    Entity.IncidentStatus = GetIncidentStatus().ToEnumString();

                    if (this.OriginalLevelOfSeverity == Entity.LevelOfSeverity)
                        Entity.LevelOfSeverity = IncidentService.GetIncidentLevel(Entity) == IncidentLevel.None ? Entity.LevelOfSeverity : (Convert.ToInt32(IncidentService.GetIncidentLevel(Entity))).ToString();
                    break;
                case PrepareSaveMode.Closed:
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    Entity.CloseById = AccessControlService.CurrentUser.Entity.UserId;
                    Entity.CloseDateTime = now;

                    Entity.IncidentStatus = IncidentStatus.Closed.ToEnumString();
                    break;
                case PrepareSaveMode.Cancelled:
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;

                    Entity.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
                    break;
            }
        }

        #endregion

        #region Private Method

        private void initOptions()
        {
            this.Options_AffectedSystem = InputOptionsService.GetIncidentSystemOTSystemInputOptions(IncidentSystemOTSystemInputKey.IncidentSystemOTSystem_AffectedSystem);
        }

        #endregion
    }
}
