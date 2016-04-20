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
    public class IncidentQualityNetworkModel : IIncidentModel
    {
        #region Constructor

        public IncidentQualityNetworkModel()
        {
            init();
        }
        public IncidentQualityNetworkModel(IncidentQualityNetwork viewModel)
        {
            init();
            this.Entity = viewModel;
            this.OriginalLevelOfSeverity = viewModel.LevelOfSeverity;
        }
        public IncidentQualityNetworkModel(OMSEventModel omsEventModel)
        {
            init();

            IncidentQualityNetwork incidentQualityNetwork = new IncidentQualityNetwork();

            incidentQualityNetwork.OMSEventId = omsEventModel.Entity.OMSEventId;
            incidentQualityNetwork.AffectedArea = omsEventModel.Entity.AffectedArea; // chi? affectedBuilding? affectedStreet?
            incidentQualityNetwork.OutageStartTime = omsEventModel.Entity.OutageStartTime;
            incidentQualityNetwork.FullRestoration = omsEventModel.Entity.FullRestoration;
            incidentQualityNetwork.NoOfBuilding = omsEventModel.Entity.NoOfBuilding;
            incidentQualityNetwork.NoOfCustomerAffected = omsEventModel.Entity.NoOfCustomerAffected;
            incidentQualityNetwork.NoOfPlatinumCustomer = omsEventModel.Entity.NoOfPlatinumCustomer;
            incidentQualityNetwork.NoOfDiamondCustomer = omsEventModel.Entity.NoOfDiamondCustomer;
            incidentQualityNetwork.NoOfGoldCustomer = omsEventModel.Entity.NoOfGoldCustomer;
            incidentQualityNetwork.NoOfSilverCustomer = omsEventModel.Entity.NoOfSilverCustomer;
            incidentQualityNetwork.PossibleCause = omsEventModel.Entity.PossibleCause; // chi?
            incidentQualityNetwork.ExpectedRestorationTime = omsEventModel.Entity.ExpectedRestorationTime;
            incidentQualityNetwork.RestorationMethod = omsEventModel.Entity.RestorationMethod; // chi?
            incidentQualityNetwork.StatusUpdate = omsEventModel.Entity.StatusUpdate;
            incidentQualityNetwork.RootCause = omsEventModel.Entity.RootCause; // Chi?
            incidentQualityNetwork.MVOutage = omsEventModel.Entity.MVOutage;
            incidentQualityNetwork.LVOutage = omsEventModel.Entity.LVOutage;
            incidentQualityNetwork.IsCriticalPoint = omsEventModel.Entity.CriticalPoint;

            this.Entity = incidentQualityNetwork;

            this.OriginalLevelOfSeverity = "";
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

        public IncidentQualityNetwork Entity = new IncidentQualityNetwork();
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

        public List<string> Options_LossInterconnection = new List<string>();
        public List<string> Options_LossTransmission = new List<string>();
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
            this.Options_LossInterconnection = InputOptionsService.GetIncidentQualityNetworkInputOptions(IncidentQualityNetworkInputKey.IncidentQualityNetwork_LossInterconnection);
            this.Options_LossTransmission = InputOptionsService.GetIncidentQualityNetworkInputOptions(IncidentQualityNetworkInputKey.IncidentQualityNetwork_LossTransmission);
        }

        #endregion
    }
}
