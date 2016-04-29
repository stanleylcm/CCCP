using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CCCP.ViewModel;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.Common;

namespace CCCP.Controllers.WebApi
{
    public class IncidentApiController : ApiController
    {
        private CCCPDbContext db = new CCCPDbContext();

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public Boolean LinkIncident(int id, int typeId, int linkId, int linkTypeId)
        {
            CCCPDbContext db = new CCCPDbContext();

            db.usp_Incident_LinkIncident(id, typeId, linkId, linkTypeId);

            return true;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public Boolean LinkGeneralEnquiry(int id, int typeId, int generalEnquiryId)
        {
            CCCPDbContext db = new CCCPDbContext();

            GeneralEnquiryIncidentLink gelink = new GeneralEnquiryIncidentLink();
            gelink.GeneralEnquiryId = generalEnquiryId;
            gelink.IncidentId = id;
            gelink.IncidentTypeId = typeId;

            db.GeneralEnquiryIncidentLink.Add(gelink);
            db.SaveChanges();

            return true;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<ViewModel.IncidentType> GetIncidentTypeList()
        {
            CCCPDbContext db = new CCCPDbContext();
            return db.IncidentType.Where(m => m.IncidentType1.IndexOf("___") < 0).ToList();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int EscalateToCrisis(int id, int typeId)
        {
            CCCPDbContext db = new CCCPDbContext();

            string incidentNo = "";

            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;

            CrisisModel crisis = new CrisisModel();

            crisis.Entity.CrisisId = 0;
            crisis.Entity.ChatRoomId = 0;
            crisis.Entity.ChecklistBatchId = 0;
            crisis.PrepareSave(PrepareSaveMode.Created);

            db.Crisis.Add(crisis.Entity);
            db.SaveChanges();
            
            switch(MasterTableService.GetIncidentTypeSubType(typeId))
            {
                case IncidentTypeSubType.EnvironmentAirEmission:
                    IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(id);
                    incidentEnvironmentAirEmission.CrisisId = crisis.Entity.CrisisId;
                    incidentEnvironmentAirEmission.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentEnvironmentAirEmission.IncidentNo;
                    break;
                case IncidentTypeSubType.EnvironmentLeakage:
                    IncidentEnvironmentLeakage incidentEnvironmentLeakage = db.IncidentEnvironmentLeakage.Find(id);
                    incidentEnvironmentLeakage.CrisisId = crisis.Entity.CrisisId;
                    incidentEnvironmentLeakage.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentEnvironmentLeakage.IncidentNo;
                    break;
                case IncidentTypeSubType.OHS:
                    IncidentOHS incidentOHS = db.IncidentOHS.Find(id);
                    incidentOHS.CrisisId = crisis.Entity.CrisisId;
                    incidentOHS.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentOHS.IncidentNo;
                    break;
                case IncidentTypeSubType.QualityCorporateImage:
                    IncidentQualityCorporateImage incidentQualityCorporateImage = db.IncidentQualityCorporateImage.Find(id);
                    incidentQualityCorporateImage.CrisisId = crisis.Entity.CrisisId;
                    incidentQualityCorporateImage.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentQualityCorporateImage.IncidentNo;
                    break;
                case IncidentTypeSubType.QualityGeneration:
                    IncidentQualityGeneration incidentQualityGeneration = db.IncidentQualityGeneration.Find(id);
                    incidentQualityGeneration.CrisisId = crisis.Entity.CrisisId;
                    incidentQualityGeneration.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentQualityGeneration.IncidentNo;
                    break;
                case IncidentTypeSubType.QualityNetwork:
                    IncidentQualityNetwork incidentQualityNetwork = db.IncidentQualityNetwork.Find(id);
                    incidentQualityNetwork.CrisisId = crisis.Entity.CrisisId;
                    incidentQualityNetwork.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentQualityNetwork.IncidentNo;
                    break;
                case IncidentTypeSubType.SystemBilling:
                    IncidentSystemBilling incidentSystemBilling = db.IncidentSystemBilling.Find(id);
                    incidentSystemBilling.CrisisId = crisis.Entity.CrisisId;
                    incidentSystemBilling.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentSystemBilling.IncidentNo;
                    break;
                case IncidentTypeSubType.SystemCallCentre:
                    IncidentSystemCallCentre incidentSystemCallCentre = db.IncidentSystemCallCentre.Find(id);
                    incidentSystemCallCentre.CrisisId = crisis.Entity.CrisisId;
                    incidentSystemCallCentre.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentSystemCallCentre.IncidentNo;
                    break;
                case IncidentTypeSubType.SystemInvoicing:
                    IncidentSystemInvoicing incidentSystemInvoicing = db.IncidentSystemInvoicing.Find(id);
                    incidentSystemInvoicing.CrisisId = crisis.Entity.CrisisId;
                    incidentSystemInvoicing.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentSystemInvoicing.IncidentNo;
                    break;
                case IncidentTypeSubType.SystemITSystem:
                    IncidentSystemITSystem incidentSystemITSystem = db.IncidentSystemITSystem.Find(id);
                    incidentSystemITSystem.CrisisId = crisis.Entity.CrisisId;
                    incidentSystemITSystem.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentSystemITSystem.IncidentNo;
                    break;
                case IncidentTypeSubType.SystemNetworkConnectivity:
                    IncidentSystemNetworkConnectivity incidentSystemNetworkConnectivity = db.IncidentSystemNetworkConnectivity.Find(id);
                    incidentSystemNetworkConnectivity.CrisisId = crisis.Entity.CrisisId;
                    incidentSystemNetworkConnectivity.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentSystemNetworkConnectivity.IncidentNo;
                    break;
                case IncidentTypeSubType.SystemOTSystem:
                    IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(id);
                    incidentSystemOTSystem.CrisisId = crisis.Entity.CrisisId;
                    incidentSystemOTSystem.IncidentStatus = IncidentStatus.Pending_For_Crisis_Approval.ToEnumString();
                    incidentNo = incidentSystemOTSystem.IncidentNo;
                    break;
            }

            db.SaveChanges();

            NotificationService.SendEscalateCrisisNotification(crisis.Entity.CrisisId, incidentNo, MasterTableService.GetIncidentTypeSubType(typeId));

            return id;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int SetIncidentStatus(int id, int typeId, string incidentStatus)
        {
            CCCPDbContext db = new CCCPDbContext();

            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;

            switch (MasterTableService.GetIncidentTypeSubType(typeId))
            {
                case IncidentTypeSubType.EnvironmentAirEmission:
                    IncidentEnvironmentAirEmission incidentEnvironmentAirEmission = db.IncidentEnvironmentAirEmission.Find(id);
                    incidentEnvironmentAirEmission.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.EnvironmentLeakage:
                    IncidentEnvironmentLeakage incidentEnvironmentLeakage = db.IncidentEnvironmentLeakage.Find(id);
                    incidentEnvironmentLeakage.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.OHS:
                    IncidentOHS incidentOHS = db.IncidentOHS.Find(id);
                    incidentOHS.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.QualityCorporateImage:
                    IncidentQualityCorporateImage incidentQualityCorporateImage = db.IncidentQualityCorporateImage.Find(id);
                    incidentQualityCorporateImage.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.QualityGeneration:
                    IncidentQualityGeneration incidentQualityGeneration = db.IncidentQualityGeneration.Find(id);
                    incidentQualityGeneration.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.QualityNetwork:
                    IncidentQualityNetwork incidentQualityNetwork = db.IncidentQualityNetwork.Find(id);
                    incidentQualityNetwork.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.SystemBilling:
                    IncidentSystemBilling incidentSystemBilling = db.IncidentSystemBilling.Find(id);
                    incidentSystemBilling.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.SystemCallCentre:
                    IncidentSystemCallCentre incidentSystemCallCentre = db.IncidentSystemCallCentre.Find(id);
                    incidentSystemCallCentre.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.SystemInvoicing:
                    IncidentSystemInvoicing incidentSystemInvoicing = db.IncidentSystemInvoicing.Find(id);
                    incidentSystemInvoicing.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.SystemITSystem:
                    IncidentSystemITSystem incidentSystemITSystem = db.IncidentSystemITSystem.Find(id);
                    incidentSystemITSystem.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.SystemNetworkConnectivity:
                    IncidentSystemNetworkConnectivity incidentSystemNetworkConnectivity = db.IncidentSystemNetworkConnectivity.Find(id);
                    incidentSystemNetworkConnectivity.IncidentStatus = incidentStatus;
                    break;
                case IncidentTypeSubType.SystemOTSystem:
                    IncidentSystemOTSystem incidentSystemOTSystem = db.IncidentSystemOTSystem.Find(id);
                    incidentSystemOTSystem.IncidentStatus = incidentStatus;
                    break;
            }

            db.SaveChanges();

            return id;
        }
    }
}