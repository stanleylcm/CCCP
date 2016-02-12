using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCCP.Common;
using CCCP.Common;
using CCCP.Models;

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
            if (incident.ContactedBy.IsEquals("Media")) return IncidentLevel.Level3;

            // Level 2
            if (incident.BillingErrorSeriousness.IsEquals("Danger Zone") &&
                (incident.ContactedBy.IsEquals("Consumer Council") || incident.ContactedBy.IsEquals("Government"))
                ) return IncidentLevel.Level2;

            // else
            return IncidentLevel.None;
        }         
    }
}
