using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.ViewModel;
using CCCP.Business.Model;

namespace CCCP.Business.Service
{
    public class IncidentService
    {
        /// <summary>
        /// Get Incident Level of System Billing
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentSystemBilling incident)
        {
            // Level 3
            if (incident.ContactedBy.IsContains(IncidentSystemBillingContactedBy.Media.ToEnumString())) return IncidentLevel.Level_3;

            // Level 2
            if (incident.BillingErrorSeriousness.IsEquals(IncidentSystemBillingBillingErrorSeriousness.Danger_Zone.ToEnumString()) &&
                (incident.ContactedBy.IsContains(IncidentSystemBillingContactedBy.Consumer_Council.ToEnumString()) || 
                    incident.ContactedBy.IsContains(IncidentSystemBillingContactedBy.Government.ToEnumString()))
                ) return IncidentLevel.Level_2;

            // else default level 1
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of System Invoicing
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentSystemInvoicing incident)
        {
            // Level 3
            if (incident.ExpectedAffectedNoOfBill > 50 && incident.ExpectedAffectedBillingDay > 5) return IncidentLevel.Level_3;

            // Level 2
            if (incident.ExpectedAffectedNoOfBill > 50 &&
                (incident.ExpectedAffectedBillingDay >= 4 && incident.ExpectedAffectedBillingDay <= 5)
                ) return IncidentLevel.Level_2;

            // else
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of System Call Centre
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentSystemCallCentre incident)
        {
            // Level 3
            if (incident.PossibleCause.IsContains(IncidentSystemCallCentrePossibleCause.Network_Failure.ToEnumString()) ||
                incident.Impact.IsContains(IncidentSystemCallCentreImpact.Suspension_of_Call_Centre.ToEnumString()) ||
                (incident.Impact.IsContains(IncidentSystemCallCentreImpact._5_workstation_failure.ToEnumString()) && incident.ImpactWorkstationFailure.Value > 50)) return IncidentLevel.Level_3;

            // Level 2
            if (incident.Impact.IsContains(IncidentSystemCallCentreImpact.Absence_of_E1_Line.ToEnumString()) ||
                (incident.Impact.IsContains(IncidentSystemCallCentreImpact._5_workstation_failure.ToEnumString()) && incident.ImpactWorkstationFailure.Value >= 30 && incident.ImpactWorkstationFailure.Value <= 50)
                ) return IncidentLevel.Level_2;

            // else
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of System IT System
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentSystemITSystem incident)
        {
            // no Level logic for IT System
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of System Network Connectivity
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentSystemNetworkConnectivity incident)
        {
            // no Level logic for Network Connectivity
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of System OT System
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentSystemOTSystem incident)
        {
            // no Level logic for OT System
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of Environment Air Emission
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentEnvironmentAirEmission incident)
        {
            // Level 2
            if (incident.ComplaintOfBlackSmoke != null && incident.ComplaintOfBlackSmoke.Value == true)
                return IncidentLevel.Level_2;

            // else
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of Environment Leakage
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentEnvironmentLeakage incident)
        {   
            // Level 3
            if (incident.AffectedArea != null && incident.AffectedArea == IncidentEnvironmentLeakageAffectedArea.Outside_CEM_premise.ToEnumString())
                return IncidentLevel.Level_3;

            // Level 2
            if (incident.AffectedArea != null && incident.AffectedArea == IncidentEnvironmentLeakageAffectedArea.Within_CEM_premise.ToEnumString())
                return IncidentLevel.Level_2;

            // else
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of Qualtiy Corporate Image
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentQualityCorporateImage incident)
        {
            // Level 3
            if (incident.PossibleCause.IsContains("Prosecution proceedings result for non-compliance") ||
                incident.PossibleCause.IsContains("Corruption and wrong info provision lead to non-compliance") ||
                incident.PossibleCause.IsContains("Large scale labor strike") ||
                incident.PossibleCause.IsContains("Major commnets/complaints from local ass/gov/int'l bodies") ||
                incident.Impact.IsContains("Aroused gov and int'l interest/enquiry")
                )
                return IncidentLevel.Level_3;

            // Level 2
            if (incident.PossibleCause.IsContains("Legal non-compliance") ||
                incident.PossibleCause.IsContains("Corruption and wrong info provision lead to non-compliance") ||
                incident.PossibleCause.IsContains("Rumors of sensitive issues") ||
                incident.PossibleCause.IsContains("Comments/complaints from local ass/gov") ||
                incident.Impact.IsContains("Aroused worker ass complaint") ||
                incident.Impact.IsContains("Small-scale media interest") ||
                incident.Impact.IsContains("Aroused public concern & media enquiry")
                )
                return IncidentLevel.Level_2;

            // else
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of Quality Generation
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentQualityGeneration incident)
        {
            // no Level logic for Quality Generation
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of OHS
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentOHS incident)
        {
            // Level 3
            if (
                (incident.OHSType.IsEquals("Work Accidents") && incident.NoOfDeath > 0) ||
                (incident.OHSType.IsEquals("Epidemic Outbreak") && incident.NoOfInfectiousDisease != null && incident.NoOfInfectiousDisease > 50)
                )
                return IncidentLevel.Level_3;

            // Level 2
            if (
                (incident.OHSType.IsEquals("Work Accidents") && incident.Treatment.IsContains("In-Patient")) ||
                (incident.OHSType.IsEquals("Epidemic Outbreak") && incident.NoOfInfectiousDisease != null && incident.NoOfInfectiousDisease > 1)
                )
                return IncidentLevel.Level_2;

            // else
            return IncidentLevel.Level_1;
        }

        /// <summary>
        /// Get Incident Level of Quality - Network
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentQualityNetwork incident)
        {
            // for later use... LV Outage? MV Outage?
            int outageMinute = 0;
            if (incident.IssueDateTime != null && incident.OutageStartTime != null)
            {
                outageMinute = Convert.ToInt32((incident.IssueDateTime.Value - incident.OutageStartTime.Value).TotalMinutes);
            }

            // Level 3
            if (
                incident.LossGeneration > 200 ||
                (incident.LossInterconnection != null && incident.LossInterconnection.IsEquals("220kV SS")) ||
                (incident.LossTransmission != null && incident.LossTransmission.IsEquals("Loss one primary substation"))
                )
                return IncidentLevel.Level_3;

            // Level 2
            if (
                (incident.LossGeneration >= 80 && incident.LossGeneration <= 200) ||
                (incident.LossInterconnection != null && incident.LossInterconnection.IsEquals(">1220kV Cable")) ||
                (incident.LossInterconnection != null && incident.LossInterconnection.IsEquals(">1220kV TR")) ||
                (incident.LossInterconnection != null && incident.LossInterconnection.IsEquals(">1220kV Busbar")) ||
                (incident.LossTransmission != null && incident.LossTransmission.IsEquals("Transmission network violate N-1 condition due to network fault")) ||
                (incident.LossTransmission != null && incident.LossTransmission.IsEquals("Cable overload")) ||
                (incident.LossTransmission != null && incident.LossTransmission.IsEquals("TR overload")) ||
                (incident.LossTransmission != null && incident.LossTransmission.IsEquals("66/11kV loss two TR")) ||
                (incident.MVOutage != null && incident.MVOutage.Value && outageMinute > 240 && outageMinute <= 360) ||
                (incident.IsDoubleFault != null && incident.IsDoubleFault.Value == true) ||
                // More than 1 condition (at least 2...) of the below had been met
                // No. of Diamond customer > 0
                // No. of Platinum customer > 0
                // No. of Customer Affected >= 100
                // LV Outage >= 240min.
                // Critical PT is TRUE
                (incident.NoOfDiamondCustomer > 0 && incident.NoOfPlatinumCustomer > 0) ||
                (incident.NoOfDiamondCustomer > 0 && incident.NoOfCustomerAffected > 0) ||
                (incident.NoOfDiamondCustomer > 0 && incident.LVOutage != null && incident.LVOutage.Value && outageMinute >= 240) ||
                (incident.NoOfDiamondCustomer > 0 && incident.IsCriticalPoint != null && incident.IsCriticalPoint.Value == true) ||
                (incident.NoOfPlatinumCustomer > 0 && incident.NoOfCustomerAffected > 0) ||
                (incident.NoOfPlatinumCustomer > 0 && incident.LVOutage != null && incident.LVOutage.Value && outageMinute >= 240) ||
                (incident.NoOfPlatinumCustomer > 0 && incident.IsCriticalPoint != null && incident.IsCriticalPoint.Value == true) ||
                (incident.NoOfCustomerAffected > 0 && incident.LVOutage != null && incident.LVOutage.Value && outageMinute >= 240) ||
                (incident.NoOfCustomerAffected > 0 && incident.IsCriticalPoint != null && incident.IsCriticalPoint.Value == true) ||
                (incident.LVOutage != null && incident.LVOutage.Value && outageMinute >= 240 && incident.IsCriticalPoint != null && incident.IsCriticalPoint.Value == true)
                )
                return IncidentLevel.Level_2;

            // Level 1

            // else
            return IncidentLevel.Level_1;
        }

        public static string GetNewIncidentNo(SequenceType type, int year)
        {
            string Result = "";
            string sequenceType = type.ToEnumString();

            // Get new sequence no from SP
            using (CCCPDbContext db = new CCCPDbContext())
            {
                int seqNo = db.usp_GetNextSequenceNo(sequenceType, (short)year).FirstOrDefault().Value;
                Result = string.Format("{0}{1}", year.ToString().Right(2), seqNo.ToString("00000"));
            }

            return Result;
        }

        public static IncidentStatus GetIncidentStatus(IIncidentModel model, IncidentStatus originalStatus)
        {
            // original status
            IncidentStatus result = originalStatus;

            // Crisis status would be updated by Crisis module

            // In progress
            if (isInProgress(model)) result = IncidentStatus.In_Progress;

            // Ready For Close
            if (IsReadyForClose(model)) result = IncidentStatus.Ready_For_Close;

            return result;
        }
        public static bool IsReadyForClose(IIncidentModel model)
        {
            // check checklist's compulsory actions had been completed or not
            foreach (ChecklistModel checklist in model.Checklists) if (!checklist.IsAllCompulsoryCompleted()) return false;
            return true;
        }
        private static bool isInProgress(IIncidentModel model)
        {
            foreach (ChecklistModel checklist in model.Checklists) if (checklist.IsInProgress()) return true;
            return false;
        }
    }
}
