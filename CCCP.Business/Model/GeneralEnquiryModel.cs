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
    public class GeneralEnquiryModel
    {
        #region Constructor

        public GeneralEnquiryModel()
        {
            init();
        }
        public GeneralEnquiryModel(GeneralEnquiry viewModel)
        {
            init();
            this.Entity = viewModel;
        }

        private void init()
        {
            Chatroom = new ChatRoomModel();
        }

        #endregion

        #region Declaration

        public GeneralEnquiry Entity = new GeneralEnquiry();
        public ChatRoomModel Chatroom { get; set; }

        public EnquiryStatus EnquiryStatus
        {
            get
            {
                return Entity.Status.ToEnum<EnquiryStatus>();
            }
        }
        public String IssueBy
        {
            get
            {
                CCCPDbContext db = new CCCPDbContext();
                User user = db.User.Find(Entity.IssueById);
                if (user == null) return "";

                UserModel userModel =  new UserModel(user);
                return userModel.GetLastUpdatedBy();
            }
        }
        public String EnquiryType
        {
            get
            {
                return MasterTableService.GetIncidentTypeName(MasterTableService.GetIncidentTypeSubType(Entity.GeneralEnquiryTypeId.Value));
            }
        }

        public String LinkedIncidentNo
        {
            get
            {
                CCCPDbContext db = new CCCPDbContext();

                GeneralEnquiryIncidentLink gelink = db.GeneralEnquiryIncidentLink.Where(m => m.GeneralEnquiryId == Entity.GeneralEnquiryId).FirstOrDefault();

                if (gelink == null || gelink.GeneralEnquiryId != Entity.GeneralEnquiryId) return "";

                switch (MasterTableService.GetIncidentTypeSubType(gelink.IncidentTypeId))
                {
                    case IncidentTypeSubType.EnvironmentAirEmission:
                        return db.IncidentEnvironmentAirEmission.Where(m => m.IncidentEnvironmentAirEmissionId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.EnvironmentLeakage:
                        return db.IncidentEnvironmentLeakage.Where(m => m.IncidentEnvironmentLeakageId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.OHS:
                        return db.IncidentOHS.Where(m => m.IncidentOHSId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.QualityCorporateImage:
                        return db.IncidentQualityCorporateImage.Where(m => m.IncidentQualityCorporateImageId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.QualityGeneration:
                        return db.IncidentQualityGeneration.Where(m => m.IncidentQualityGenerationId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.QualityNetwork:
                        return db.IncidentQualityNetwork.Where(m => m.IncidentQualityNetworkId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.SystemBilling:
                        return db.IncidentSystemBilling.Where(m => m.IncidentSystemBillingId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.SystemInvoicing:
                        return db.IncidentSystemInvoicing.Where(m => m.IncidentSystemInvoicingId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.SystemCallCentre:
                        return db.IncidentSystemCallCentre.Where(m => m.IncidentSystemCallCentreId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.SystemITSystem:
                        return db.IncidentSystemITSystem.Where(m => m.IncidentSystemITSystemId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.SystemNetworkConnectivity:
                        return db.IncidentSystemNetworkConnectivity.Where(m => m.IncidentSystemNetworkConnectivityId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                    case IncidentTypeSubType.SystemOTSystem:
                        return db.IncidentSystemOTSystem.Where(m => m.IncidentSystemOTSystemId == gelink.IncidentId).FirstOrDefault().IncidentNo;
                }

                return "";
            }
        }

        public String LinkedIncidentController
        {
            get
            {
                CCCPDbContext db = new CCCPDbContext();

                GeneralEnquiryIncidentLink gelink = db.GeneralEnquiryIncidentLink.Where(m => m.GeneralEnquiryId == Entity.GeneralEnquiryId).FirstOrDefault();

                if (gelink == null || gelink.GeneralEnquiryId != Entity.GeneralEnquiryId) return "";

                return "Incident" + MasterTableService.GetIncidentTypeSubType(gelink.IncidentTypeId).ToEnumString();
            }
        }

        public int LinkedIncidentId
        {
            get
            {
                CCCPDbContext db = new CCCPDbContext();

                GeneralEnquiryIncidentLink gelink = db.GeneralEnquiryIncidentLink.Where(m => m.GeneralEnquiryId == Entity.GeneralEnquiryId).FirstOrDefault();

                if (gelink == null || gelink.GeneralEnquiryId != Entity.GeneralEnquiryId) return 0;

                return gelink.IncidentId;
            }
        }

        #endregion

        #region Public Method

        public bool IsReadyForClose()
        {
            //return IncidentService.IsReadyForClose(this);
            return true;
        }
        public void PrepareSave(PrepareSaveMode saveMode = PrepareSaveMode.Last_Updated)
        {
            DateTime now = DateTime.Now;

            switch (saveMode)
            {
                case PrepareSaveMode.Created:
                    Entity.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.CreatedDateTime = now;
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    Entity.IssueById = AccessControlService.CurrentUser.Entity.UserId;
                    break;
                case PrepareSaveMode.Last_Updated:
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
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

        #endregion
    }
}
