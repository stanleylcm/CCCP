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
    public class CrisisModel
    {
        #region Constructor

        public CrisisModel()
        {
            init();
        }
        public CrisisModel(Crisis viewModel)
        {
            init();
            this.Entity = viewModel;
        }

        private void init()
        {
            Chatroom = new ChatRoomModel();
            Checklists = new List<ChecklistModel>();
            ChecklistEntities = new List<ChecklistExtend>();
        }

        #endregion

        #region Declaration

        public Crisis Entity = new Crisis();
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

        public IncidentTypeSubType IncidentType
        {
            get
            {
                CCCPDbContext db = new CCCPDbContext();
                if (db.IncidentEnvironmentAirEmission.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.EnvironmentAirEmission;
                if (db.IncidentEnvironmentLeakage.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.EnvironmentLeakage;
                if (db.IncidentOHS.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.OHS;
                if (db.IncidentQualityCorporateImage.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.QualityCorporateImage;
                if (db.IncidentQualityGeneration.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.QualityGeneration;
                if (db.IncidentQualityNetwork.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.QualityNetwork;
                if (db.IncidentSystemBilling.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.SystemBilling;
                if (db.IncidentSystemCallCentre.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.SystemCallCentre;
                if (db.IncidentSystemInvoicing.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.SystemInvoicing;
                if (db.IncidentSystemITSystem.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.SystemITSystem;
                if (db.IncidentSystemNetworkConnectivity.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.SystemNetworkConnectivity;
                if (db.IncidentSystemOTSystem.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return IncidentTypeSubType.SystemOTSystem;

                return IncidentTypeSubType.EnvironmentAirEmission;
            }
        }
        public int IncidentId
        {
            get
            {
                CCCPDbContext db = new CCCPDbContext();
                if (db.IncidentEnvironmentAirEmission.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentEnvironmentAirEmission.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentEnvironmentAirEmissionId;
                if (db.IncidentEnvironmentLeakage.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentEnvironmentLeakage.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentEnvironmentLeakageId;
                if (db.IncidentOHS.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentOHS.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentOHSId;
                if (db.IncidentQualityCorporateImage.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentQualityCorporateImage.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentQualityCorporateImageId;
                if (db.IncidentQualityGeneration.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentQualityGeneration.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentQualityGenerationId;
                if (db.IncidentQualityNetwork.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentQualityNetwork.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentQualityNetworkId;
                if (db.IncidentSystemBilling.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentSystemBilling.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentSystemBillingId;
                if (db.IncidentSystemCallCentre.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentSystemCallCentre.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentSystemCallCentreId;
                if (db.IncidentSystemInvoicing.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentSystemInvoicing.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentSystemInvoicingId;
                if (db.IncidentSystemITSystem.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentSystemITSystem.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentSystemITSystemId;
                if (db.IncidentSystemNetworkConnectivity.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentSystemNetworkConnectivity.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentSystemNetworkConnectivityId;
                if (db.IncidentSystemOTSystem.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault() != null)
                    return db.IncidentSystemOTSystem.Where(m => m.CrisisId != null && m.CrisisId.Value == Entity.CrisisId).FirstOrDefault().IncidentSystemOTSystemId;

                return 0;
            }
        }

        #endregion

        #region Public Method
        public bool IsReadyForClose()
        {
            foreach (ChecklistModel checklist in this.Checklists) if (!checklist.IsAllCompulsoryCompleted()) return false;
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
                    if (Entity.IssueById == null || Entity.IssueById == 0)
                    {
                        Entity.IssueById = AccessControlService.CurrentUser.Entity.UserId;
                        Entity.IssueDateTime = now;
                    }

                    Entity.CrisisNo = IncidentService.GetNewIncidentNo(SequenceType.Crisis, DateTime.Now.Year);
                    Entity.Status = CrisisStatus.Pending_For_Approval.ToEnumString();
                    break;
                case PrepareSaveMode.Last_Updated:
                    if (IsReadyForClose())
                        Entity.Status = CrisisStatus.Ready_For_Close.ToEnumString();
                    else
                        Entity.Status = CrisisStatus.In_Progress.ToEnumString();
                    break;
                case PrepareSaveMode.Approved:
                    Entity.Status = CrisisStatus.Pending.ToEnumString();
                    break;
                case PrepareSaveMode.Rejected:
                    Entity.Status = CrisisStatus.Rejected.ToEnumString();
                    break;
                case PrepareSaveMode.Closed:
                    Entity.CloseById = AccessControlService.CurrentUser.Entity.UserId;
                    Entity.CloseDateTime = now;
                    Entity.Status = CrisisStatus.Closed.ToEnumString();
                    break;
            }

            Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            Entity.LastUpdatedDateTime = now;
        }

        #endregion

        #region Private Method

        #endregion
    }
}
