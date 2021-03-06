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
            incidentQualityNetwork.AffectedArea = omsEventModel.Entity.AffectedArea;
            incidentQualityNetwork.AffectedArea_Chi = omsEventModel.Entity.AffectedArea_Chi;
            incidentQualityNetwork.AffectedBuilding = omsEventModel.Entity.AffectedBuilding;
            incidentQualityNetwork.AffectedBuilding_Chi = omsEventModel.Entity.AffectedBuilding_Chi;
            incidentQualityNetwork.AffectedStreet = omsEventModel.Entity.AffectedStreet;
            incidentQualityNetwork.AffectedStreet_Chi = omsEventModel.Entity.AffectedStreet_Chi;
            incidentQualityNetwork.OutageStartTime = omsEventModel.Entity.OutageStartTime;
            incidentQualityNetwork.FullRestoration = omsEventModel.Entity.FullRestoration;
            incidentQualityNetwork.NoOfBuilding = omsEventModel.Entity.NoOfBuilding;
            incidentQualityNetwork.NoOfCustomerAffected = omsEventModel.Entity.NoOfCustomerAffected;
            incidentQualityNetwork.NoOfPlatinumCustomer = omsEventModel.Entity.NoOfPlatinumCustomer;
            incidentQualityNetwork.NoOfDiamondCustomer = omsEventModel.Entity.NoOfDiamondCustomer;
            incidentQualityNetwork.NoOfGoldCustomer = omsEventModel.Entity.NoOfGoldCustomer;
            incidentQualityNetwork.NoOfSilverCustomer = omsEventModel.Entity.NoOfSilverCustomer;
            incidentQualityNetwork.PossibleCause = omsEventModel.Entity.PossibleCause;
            incidentQualityNetwork.PossibleCause_Chi = omsEventModel.Entity.PossibleCause_Chi;
            incidentQualityNetwork.ExpectedRestorationTime = omsEventModel.Entity.ExpectedRestorationTime;
            incidentQualityNetwork.RestorationMethod = omsEventModel.Entity.RestorationMethod;
            incidentQualityNetwork.RestorationMethod_Chi = omsEventModel.Entity.RestorationMethod_Chi;
            incidentQualityNetwork.StatusUpdate = omsEventModel.Entity.StatusUpdate;
            incidentQualityNetwork.RootCause = omsEventModel.Entity.RootCause;
            incidentQualityNetwork.RootCause_Chi = omsEventModel.Entity.RootCause_Chi;
            incidentQualityNetwork.OutageLevel = (omsEventModel.Entity.MVOutage != null && omsEventModel.Entity.MVOutage.Value ? "MV Outage" :
                                                    (omsEventModel.Entity.LVOutage != null && omsEventModel.Entity.LVOutage.Value ? "LV Outage" : null));
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
            LinkedGeneralEnquiryEntities = new List<usp_Incident_GetLinkedGeneralEnquiry_Result>();
            CrisisEntity = new CrisisModel();
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
        public List<usp_Incident_GetLinkedGeneralEnquiry_Result> LinkedGeneralEnquiryEntities { get; set; }
        public CrisisModel CrisisEntity { get; set; }

        public List<string> Options_LossInterconnection = new List<string>();
        public List<string> Options_LossTransmission = new List<string>();
        public List<string> Options_OutageLevel = new List<string>();
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
            this.Options_LossInterconnection = InputOptionsService.GetIncidentQualityNetworkInputOptions(IncidentQualityNetworkInputKey.IncidentQualityNetwork_LossInterconnection);
            this.Options_LossTransmission = InputOptionsService.GetIncidentQualityNetworkInputOptions(IncidentQualityNetworkInputKey.IncidentQualityNetwork_LossTransmission);
            this.Options_OutageLevel = InputOptionsService.GetIncidentQualityNetworkInputOptions(IncidentQualityNetworkInputKey.IncidentQualityNetwork_OutageLevel);
        }

        #endregion
    }
}
