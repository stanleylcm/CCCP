//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CCCP.ViewModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class IncidentSystemBilling
    {
        public int IncidentSystemBillingId { get; set; }
        public int ChecklistBatchId { get; set; }
        public int ChatRoomId { get; set; }
        public int GeneralEnquiryId { get; set; }
        public Nullable<int> CrisisId { get; set; }
        public int NotificationId { get; set; }
        public Nullable<int> IssueById { get; set; }
        public Nullable<System.DateTime> IssueDateTime { get; set; }
        public Nullable<int> CloseById { get; set; }
        public Nullable<System.DateTime> CloseDateTime { get; set; }
        public string IncidentNo { get; set; }
        public string LevelOfSeverity { get; set; }
        public string IncidentStatus { get; set; }
        public string IncidentBackground { get; set; }
        public Nullable<bool> IsDrillMode { get; set; }
        public string History { get; set; }
        public string ProblemArea { get; set; }
        public string PossibleCause { get; set; }
        public string BillingErrorSeriousness { get; set; }
        public string ExpectedAffectedCustomerBill { get; set; }
        public string ContactedBy { get; set; }
        public string Impact { get; set; }
        public string StatusUpdate { get; set; }
        public string RequireMitigatingAction { get; set; }
        public string MitigatingAction { get; set; }
        public string MitigatingActionOthers { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
}
