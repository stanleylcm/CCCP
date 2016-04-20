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
    public class IncidentSystemNetworkConnectivityModel : IIncidentModel
    {
        #region Constructor

        public IncidentSystemNetworkConnectivityModel()
        {
            init();
        }
        public IncidentSystemNetworkConnectivityModel(IncidentSystemNetworkConnectivity viewModel)
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
        }

        #endregion

        #region Declaration

        public IncidentSystemNetworkConnectivity Entity = new IncidentSystemNetworkConnectivity();
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

        public List<string> Options_AffectedArea = new List<string>();
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
            return IncidentService.IsReadyForClose(this) && (Entity.FullRestoration != null);
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
            this.Options_AffectedArea = InputOptionsService.GetIncidentSystemNetworkConnectivityInputOptions(IncidentSystemNetworkConnectivityInputKey.IncidentSystemNetworkConnectivity_AffectedArea);
        }

        #endregion
    }
}
