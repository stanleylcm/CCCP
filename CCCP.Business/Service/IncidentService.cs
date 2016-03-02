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

        public static int GetIncidentTypeId(IncidentTypeSubType incidentType)
        {
            int result = 0;
            using (CCCPDbContext db = new CCCPDbContext())
            {
                string incidentTypeStr = incidentType.ToEnumString();
                result = db.IncidentType.SingleOrDefault(x => x.IncidentType1 == incidentTypeStr).IncidentTypeId;
            }

            return result;
        }
    }
}
