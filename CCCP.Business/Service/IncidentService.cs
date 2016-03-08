using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.ViewModel;

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
            //if (incident.ContactedBy.IsEquals("Media")) return IncidentLevel.Level3;
            if (incident.ContactedBy.IsContains("Media")) return IncidentLevel.Level_3;

            // Level 2
            if (incident.BillingErrorSeriousness.IsEquals("Danger Zone") &&
                //(incident.ContactedBy.IsEquals("Consumer Council") || incident.ContactedBy.IsEquals("Government"))
                (incident.ContactedBy.IsContains("Consumer Council") || incident.ContactedBy.IsContains("Government"))
                ) return IncidentLevel.Level_2;

            // else
            return IncidentLevel.None;
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
            return IncidentLevel.None;
        }

        /// <summary>
        /// Get Incident Level of System Call Centre
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentSystemCallCentre incident)
        {
            int impactPercent = 0;

            if (incident.Impact.IsContains("% workstation failure"))
            {
                string impact = incident.Impact.ToLower();
                string sImpactPercent = impact.Substring(0, impact.IndexOf("% workstation failure"));
                impactPercent = Convert.ToInt32(sImpactPercent);
            }

            // Level 3
            if (incident.PossibleCause.IsContains("Network Failure") ||
                incident.Impact.IsContains("Suspension of Call Centre") ||
                (incident.Impact.IsContains("% workstation failure") && impactPercent > 50)) return IncidentLevel.Level_3;

            // Level 2
            if (incident.Impact.IsContains("absence of e1 line") ||
                (incident.Impact.IsContains("% workstation failure") && impactPercent >= 30 && impactPercent <= 50)
                ) return IncidentLevel.Level_2;

            // else
            return IncidentLevel.None;
        }

        /// <summary>
        /// Get Incident Level of System IT System
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentSystemITSystem incident)
        {
            // no Level logic for IT System
            return IncidentLevel.None;
        }

        /// <summary>
        /// Get Incident Level of System Network Connectivity
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentSystemNetworkConnectivity incident)
        {
            // no Level logic for Network Connectivity
            return IncidentLevel.None;
        }

        /// <summary>
        /// Get Incident Level of System OT System
        /// </summary>
        /// <param name="incident"></param>
        /// <returns></returns>
        public static IncidentLevel GetIncidentLevel(IncidentSystemOTSystem incident)
        {
            // no Level logic for OT System
            return IncidentLevel.None;
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
            return IncidentLevel.None;
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
    }
}
