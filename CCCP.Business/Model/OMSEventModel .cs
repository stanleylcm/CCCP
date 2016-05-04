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
    public class OMSEventModel
    {
        #region Constructor

        public OMSEventModel()
        {
            init();
        }
        public OMSEventModel(OMSEvent viewModel)
        {
            init();
            this.Entity = viewModel;
        }
        public OMSEventModel(OMSEventApiModel viewModel)
        {
            OMSEvent oms = new OMSEvent();

            oms.OMSNo = viewModel.OMSNo;
            oms.AffectedArea = viewModel.AffectedArea;
            oms.AffectedArea_Chi = viewModel.AffectedArea_Chi;
            oms.AffectedBuilding = viewModel.AffectedBuilding;
            oms.AffectedBuilding_Chi = viewModel.AffectedBuilding_Chi;
            oms.AffectedStreet = viewModel.AffectedStreet;
            oms.AffectedStreet_Chi = viewModel.AffectedStreet_Chi;
            oms.OutageStartTime = viewModel.OutageStartTime;
            oms.NoOfBuilding = viewModel.NoOfBuilding;
            oms.NoOfCustomerAffected = viewModel.NoOfCustomerAffected;
            oms.NoOfPlatinumCustomer = viewModel.NoOfPlatinumCustomer;
            oms.NoOfDiamondCustomer = viewModel.NoOfDiamondCustomer;
            oms.NoOfGoldCustomer = viewModel.NoOfGoldCustomer;
            oms.NoOfSilverCustomer = viewModel.NoOfSilverCustomer;
            oms.PossibleCause = viewModel.PossibleCause;
            oms.PossibleCause_Chi = viewModel.PossibleCause_Chi;
            oms.ExpectedRestorationDateTime = viewModel.ExpectedRestorationDateTime;
            oms.RestorationMethod = viewModel.RestorationMethod;
            oms.RestorationMethod_Chi = viewModel.RestorationMethod_Chi;
            oms.StatusUpdateCode = viewModel.StatusUpdateCode;
            oms.RootCause = viewModel.RootCause;
            oms.RootCause_Chi = viewModel.RootCause_Chi;
            oms.MVOutage = viewModel.MVOutage;
            oms.LVOutage = viewModel.LVOutage;
            oms.AffectedPoints = viewModel.AffectedPoints;

            this.Entity = oms;
        }
        public OMSEventModel(OMSEvent oms, OMSEventApiModel viewModel)
        {
            oms.OMSNo = viewModel.OMSNo;
            oms.AffectedArea = viewModel.AffectedArea;
            oms.AffectedArea_Chi = viewModel.AffectedArea_Chi;
            oms.AffectedBuilding = viewModel.AffectedBuilding;
            oms.AffectedBuilding_Chi = viewModel.AffectedBuilding_Chi;
            oms.AffectedStreet = viewModel.AffectedStreet;
            oms.AffectedStreet_Chi = viewModel.AffectedStreet_Chi;
            oms.OutageStartTime = viewModel.OutageStartTime;
            oms.NoOfBuilding = viewModel.NoOfBuilding;
            oms.NoOfCustomerAffected = viewModel.NoOfCustomerAffected;
            oms.NoOfPlatinumCustomer = viewModel.NoOfPlatinumCustomer;
            oms.NoOfDiamondCustomer = viewModel.NoOfDiamondCustomer;
            oms.NoOfGoldCustomer = viewModel.NoOfGoldCustomer;
            oms.NoOfSilverCustomer = viewModel.NoOfSilverCustomer;
            oms.PossibleCause = viewModel.PossibleCause;
            oms.PossibleCause_Chi = viewModel.PossibleCause_Chi;
            oms.ExpectedRestorationDateTime = viewModel.ExpectedRestorationDateTime;
            oms.RestorationMethod = viewModel.RestorationMethod;
            oms.RestorationMethod_Chi = viewModel.RestorationMethod_Chi;
            oms.StatusUpdateCode = viewModel.StatusUpdateCode;
            oms.RootCause = viewModel.RootCause;
            oms.RootCause_Chi = viewModel.RootCause_Chi;
            oms.MVOutage = viewModel.MVOutage;
            oms.LVOutage = viewModel.LVOutage;
            oms.AffectedPoints = viewModel.AffectedPoints;

            this.Entity = oms;
        }

        private void init()
        {
            initOptions();
        }

        #endregion

        #region Declaration

        public OMSEvent Entity = new OMSEvent();

        #endregion

        #region Public Method

        public IncidentQualityNetwork GetCreatedIncident()
        {
            if (this.Entity.OMSEventId == 0) return null;
            return new CCCPDbContext().IncidentQualityNetwork.Where(x => x.OMSEventId == this.Entity.OMSEventId).FirstOrDefault();
        }

        public IncidentLevel GetCalculatedLevel()
        {
            IncidentQualityNetworkModel incidentModel = new IncidentQualityNetworkModel(this);
            return IncidentService.GetIncidentLevel(incidentModel.Entity);
        }

        public bool IsCriticalPoint()
        {
            List<string> criticalPoints = MasterTableService.GetCriticalPoints();
            string affectedPointsStr = Entity.AffectedPoints == null ? "" : Entity.AffectedPoints;
            List<string> affectedPoints = affectedPointsStr.Split(',').ToList();

            return criticalPoints.Contains(affectedPoints);
        }

        public void PrepareSave(PrepareSaveMode saveMode = PrepareSaveMode.Last_Updated)
        {
            DateTime now = DateTime.Now;

            switch (saveMode)
            {
                case PrepareSaveMode.Created:
                    Entity.OMSStatus = OMSStatus.Pending.ToEnumString();
                    Entity.CreatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.CreatedDateTime = now;
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    break;
                case PrepareSaveMode.Last_Updated:
                    //Entity.OMSStatus = OMSStatus.In_Progress.ToEnumString();
                    Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
                    Entity.LastUpdatedDateTime = now;
                    break;
            }

            if (Entity.OutageStartTime != null && Entity.ExpectedRestorationDateTime != null)
            {
                Entity.ExpectedRestorationTime = Convert.ToInt16((Entity.ExpectedRestorationDateTime.Value - Entity.OutageStartTime.Value).TotalHours);
            }

            // later need to add logic to retrieve status from master table and append to StatusUpdate

            // critical points
            Entity.CriticalPoint = IsCriticalPoint();
        }

        public void MarkReviewed()
        {
            CCCPDbContext db = new CCCPDbContext();
            Entity.Reviewed = true;

            Entity.LastUpdatedBy = AccessControlService.CurrentUser.GetLastUpdatedBy();
            Entity.LastUpdatedDateTime = DateTime.Now;

            db.OMSEvent.Attach(Entity);
            db.Entry(Entity).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        #endregion

        #region Private Method

        private void initOptions()
        {

        }

        #endregion
    }
}
