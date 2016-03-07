using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CCCP.ViewModel
{
    class Metadata
    {
        
    }

    #region Incident System Billing Metadata
    [MetadataType(typeof(IncidentSystemBillingHelper))]
    public partial class IncidentSystemBilling { }

    public class IncidentSystemBillingHelper
    {
        [Display(Name = "ID")]
        public int IncidentSystemBillingId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis ID")]
        public Nullable<int> CrisisId { get; set; }
        [Display(Name = "Notification ID")]
        public int NotificationId { get; set; }
        [Display(Name = "Issue By")]
        public Nullable<int> IssueById { get; set; }
        [Display(Name = "Issue Date Time")]
        public Nullable<System.DateTime> IssueDateTime { get; set; }
        [Display(Name = "Close By")]
        public Nullable<int> CloseById { get; set; }
        [Display(Name = "Close Date Time")]
        public Nullable<System.DateTime> CloseDateTime { get; set; }
        [Display(Name = "Incident No")]
        public string IncidentNo { get; set; }
        [Display(Name = "Level Of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        public string IncidentBackground { get; set; }
        [Display(Name = "Is Drill Mode?")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }
        [Display(Name = "Problem Area")]
        public string ProblemArea { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [Display(Name = "Billing Error Seriousness")]
        public string BillingErrorSeriousness { get; set; }
        [Display(Name = "Expected Affected Customer Bill")]
        public string ExpectedAffectedCustomerBill { get; set; }
        [Display(Name = "Contacted By")]
        public string ContactedBy { get; set; }
        [Display(Name = "Impact")]
        public string Impact { get; set; }
        [Display(Name = "Status Update")]
        public string StatusUpdate { get; set; }
        [Display(Name = "Require Mitigating Action")]
        public string RequireMitigatingAction { get; set; }
        [Display(Name = "Mitigating Action")]
        public string MitigatingAction { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created Date Time")]
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        [Display(Name = "Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [Display(Name = "Last Updated Date Time")]
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
    #endregion

    #region Incident System Invoicing Metadata
    [MetadataType(typeof(IncidentSystemInvoicingHelper))]
    public partial class IncidentSystemInvoicing { }

    public partial class IncidentSystemInvoicingHelper
    {
        [Display(Name = "ID")]
        public int IncidentSystemInvoicingId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis ID")]
        public Nullable<int> CrisisId { get; set; }
        [Display(Name = "Notification ID")]
        public int NotificationId { get; set; }
        [Display(Name = "Issue By")]
        public Nullable<int> IssueById { get; set; }
        [Display(Name = "Issue Date Time")]
        public Nullable<System.DateTime> IssueDateTime { get; set; }
        [Display(Name = "Close By")]
        public Nullable<int> CloseById { get; set; }
        [Display(Name = "Close Date Time")]
        public Nullable<System.DateTime> CloseDateTime { get; set; }
        [Display(Name = "Incident No")]
        public string IncidentNo { get; set; }
        [Display(Name = "Level Of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        public string IncidentBackground { get; set; }
        [Display(Name = "Is Drill Mode?")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }
        [Display(Name = "Problem Area")]
        public string ProblemArea { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [Display(Name = "Expected Affected No Of Bill")]
        public Nullable<int> ExpectedAffectedNoOfBill { get; set; }
        [Display(Name = "Expected Affected Billing Day")]
        public Nullable<int> ExpectedAffectedBillingDay { get; set; }
        [Display(Name = "Contacted By")]
        public string ContactedBy { get; set; }
        [Display(Name = "Impact")]
        public string Impact { get; set; }
        [Display(Name = "Status Update")]
        public string StatusUpdate { get; set; }
        [Display(Name = "Require Mitigating Action")]
        public string RequireMitigatingAction { get; set; }
        [Display(Name = "Mitigating Action")]
        public string MitigatingAction { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created Date Time")]
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        [Display(Name = "Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [Display(Name = "Last Updated Date Time")]
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
    #endregion

    #region Incident System Call Centre Metadata
    [MetadataType(typeof(IncidentSystemCallCentreHelper))]
    public partial class IncidentSystemCallCentre { }

    public partial class IncidentSystemCallCentreHelper
    {
        [Display(Name = "ID")]
        public int IncidentSystemCallCentreId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis ID")]
        public Nullable<int> CrisisId { get; set; }
        [Display(Name = "Notification ID")]
        public int NotificationId { get; set; }
        [Display(Name = "Issue By")]
        public Nullable<int> IssueById { get; set; }
        [Display(Name = "Issue Date Time")]
        public Nullable<System.DateTime> IssueDateTime { get; set; }
        [Display(Name = "Close By")]
        public Nullable<int> CloseById { get; set; }
        [Display(Name = "Close Date Time")]
        public Nullable<System.DateTime> CloseDateTime { get; set; }
        [Display(Name = "Incident No")]
        public string IncidentNo { get; set; }
        [Display(Name = "Level Of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        public string IncidentBackground { get; set; }
        [Display(Name = "Is Drill Mode?")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }
        [Display(Name = "Impact")]
        public string Impact { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [Display(Name = "Expected Restoration Time")]
        public Nullable<short> ExpectedRestorationTime { get; set; }
        [Display(Name = "Require Mitigating Action")]
        public string RequireMitigatingAction { get; set; }
        [Display(Name = "Mitigating Action")]
        public string MitigatingAction { get; set; }
        [Display(Name = "Status Update")]
        public string StatusUpdate { get; set; }
        [Display(Name = "Full Restoration")]
        public Nullable<System.DateTime> FullRestoration { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created Date Time")]
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        [Display(Name = "Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [Display(Name = "Last Updated Date Time")]
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
    #endregion
}
