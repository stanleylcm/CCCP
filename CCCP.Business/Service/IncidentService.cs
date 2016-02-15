﻿using System;
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
            if (incident.ContactedBy.IsEquals("Media")) return IncidentLevel.Level3;

            // Level 2
            if (incident.BillingErrorSeriousness.IsEquals("Danger Zone") &&
                (incident.ContactedBy.IsEquals("Consumer Council") || incident.ContactedBy.IsEquals("Government"))
                ) return IncidentLevel.Level2;

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
            if (incident.ExpectedAffectedNoOfBill > 50 && incident.ExpectedAffectedBillingDay > 5) return IncidentLevel.Level3;

            // Level 2
            if (incident.ExpectedAffectedNoOfBill > 50 &&
                (incident.ExpectedAffectedBillingDay >= 4 && incident.ExpectedAffectedBillingDay <= 5)
                ) return IncidentLevel.Level3;

            // else
            return IncidentLevel.None;
        }

    }
}