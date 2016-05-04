using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.Business.Service;

namespace CCCP.Business.Model
{
    public class OMSEventApiModel
    {
        public OMSEventApiModel()
        {
        }

        public String OMSNo { get; set; }
        public String AffectedArea { get; set; }
        public String AffectedArea_Chi { get; set; }
        public String AffectedBuilding { get; set; }
        public String AffectedBuilding_Chi { get; set; }
        public String AffectedStreet { get; set; }
        public String AffectedStreet_Chi { get; set; }
        public DateTime? OutageStartTime { get; set; }
        public DateTime? FullRestoration { get; set; }
        public short? NoOfBuilding { get; set; }
        public int? NoOfCustomerAffected { get; set; }
        public int? NoOfPlatinumCustomer { get; set; }
        public int? NoOfDiamondCustomer { get; set; }
        public int? NoOfGoldCustomer { get; set; }
        public int? NoOfSilverCustomer { get; set; }
        public String PossibleCause { get; set; }
        public String PossibleCause_Chi { get; set; }
        public DateTime? ExpectedRestorationDateTime { get; set; }
        public String RestorationMethod { get; set; }
        public String RestorationMethod_Chi { get; set; }
        public String StatusUpdateCode { get; set; }
        public String RootCause { get; set; }
        public String RootCause_Chi { get; set; }
        public Boolean? MVOutage { get; set; }
        public Boolean? LVOutage { get; set; }
        public String AffectedPoints { get; set; }
    }
}
