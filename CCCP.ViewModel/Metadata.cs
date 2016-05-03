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
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
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
        [DataType(DataType.MultilineText)]
        public string MitigatingActionOthers { get; set; }
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
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }
        [Display(Name = "Problem Area")]
        public string ProblemArea { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [Display(Name = "Expected Affected No of Bill")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> ExpectedAffectedNoOfBill { get; set; }
        [Display(Name = "Expected Affected Billing Day")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
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
        [DataType(DataType.MultilineText)]
        public string MitigatingActionOthers { get; set; }
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
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }
        [Display(Name = "Impact")]
        public string Impact { get; set; }
        [DataType(DataType.MultilineText)]
        public string ImpactOthers { get; set; }
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
        [DataType(DataType.MultilineText)]
        public string StatusUpdateOthers { get; set; }
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

    #region Incident System IT System Metadata
    [MetadataType(typeof(IncidentSystemITSystemHelper))]
    public partial class IncidentSystemITSystem { }

    public partial class IncidentSystemITSystemHelper
    {
        [Display(Name = "ID")]
        public int IncidentSystemITSystemId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [Display(Name = "Damage")]
        public string Damage { get; set; }
        [Display(Name = "Affected System")]
        public string AffectedSystem { get; set; }
        [Display(Name = "Affected Area")]
        public string AffectedArea { get; set; }
        [Display(Name = "Expected Restoration Time")]
        public Nullable<System.DateTime> ExpectedRestorationTime { get; set; }
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

    #region Incident System Network Connectivity Metadata
    [MetadataType(typeof(IncidentSystemNetworkConnectivityHelper))]
    public partial class IncidentSystemNetworkConnectivity { }

    public partial class IncidentSystemNetworkConnectivityHelper
    {
        [Display(Name = "ID")]
        public int IncidentSystemNetworkConnectivityId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [Display(Name = "Damage")]
        public string Damage { get; set; }
        [Display(Name = "Affected Area")]
        public string AffectedArea { get; set; }
        [Display(Name = "Expected Restoration Time")]
        public Nullable<System.DateTime> ExpectedRestorationTime { get; set; }
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

    #region Incident System OT System Metadata
    [MetadataType(typeof(IncidentSystemOTSystemHelper))]
    public partial class IncidentSystemOTSystem { }

    public partial class IncidentSystemOTSystemHelper
    {
        [Display(Name = "ID")]
        public int IncidentSystemOTSystemId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [Display(Name = "Damage")]
        public string Damage { get; set; }
        [Display(Name = "Affected System")]
        public string AffectedSystem { get; set; }
        [Display(Name = "Affected Area")]
        public string AffectedArea { get; set; }
        [Display(Name = "No of Affected Substation")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfAffectedSubstation { get; set; }
        [Display(Name = "Expected Restoration Time")]
        public Nullable<System.DateTime> ExpectedRestorationTime { get; set; }
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

    #region Incident Environment Air Emission Metadata
    [MetadataType(typeof(IncidentEnvironmentAirEmissionHelper))]
    public partial class IncidentEnvironmentAirEmission { }

    public partial class IncidentEnvironmentAirEmissionHelper
    {
        [Display(Name = "ID")]
        public int IncidentEnvironmentAirEmissionId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [Display(Name = "Source of Information")]
        public string SourceOfInformation { get; set; }
        [Display(Name = "Abatement System Unavailable")]
        public Nullable<bool> AbatementSystemUnavailable { get; set; }
        [Display(Name = "Complaint of Black Smoke")]
        public Nullable<bool> ComplaintOfBlackSmoke { get; set; }

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

    #region Incident Environment Leakage Metadata
    [MetadataType(typeof(IncidentEnvironmentLeakageHelper))]
    public partial class IncidentEnvironmentLeakage { }

    public partial class IncidentEnvironmentLeakageHelper
    {
        [Display(Name = "ID")]
        public int IncidentEnvironmentLeakageId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [Display(Name = "Damage")]
        public string Damage { get; set; }
        [Display(Name = "Source of Information")]
        public string SourceOfInformation { get; set; }
        [Display(Name = "Type of Leakage")]
        public Nullable<bool> TypeOfLeakage { get; set; }
        [DataType(DataType.MultilineText)]
        public string TypeOfLeakageOthers { get; set; }
        [Display(Name = "Affected Area")]
        public Nullable<bool> AffectedArea { get; set; }

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

    #region Incident OHS Metadata
    [MetadataType(typeof(IncidentOHSHelper))]
    public partial class IncidentOHS { }

    public partial class IncidentOHSHelper
    {
        [Display(Name = "ID")]
        public int IncidentOHSId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [DataType(DataType.MultilineText)]
        public string PossibleCauseOthers { get; set; }
        [Display(Name = "OHS Type")]
        public string OHSType { get; set; }
        [Display(Name = "Nature of Injury")]
        public string NatureOfInjury { get; set; }
        [DataType(DataType.MultilineText)]
        public string NatureOfInjuryOthers { get; set; }
        [Display(Name = "No of Injury")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfInjury { get; set; }
        [Display(Name = "No of Infectious Disease")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfInfectiousDisease { get; set; }
        [Display(Name = "No of Death")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfDeath { get; set; }
        [Display(Name = "No of Infected")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfInfected { get; set; }
        [Display(Name = "Treatment")]
        public string Treatment { get; set; }
        [DataType(DataType.MultilineText)]
        public string TreatmentOthers { get; set; }

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

    #region Incident Quality Corporate Image Metadata
    [MetadataType(typeof(IncidentQualityCorporateImageHelper))]
    public partial class IncidentQualityCorporateImage { }

    public partial class IncidentQualityCorporateImageHelper
    {
        [Display(Name = "ID")]
        public int IncidentQualityCorporateImageId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }

        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [DataType(DataType.MultilineText)]
        public string PossibleCauseOthers { get; set; }
        [Display(Name = "Impact")]
        public string Impact { get; set; }
        [DataType(DataType.MultilineText)]
        public string ImpactOthers { get; set; }
        [Display(Name = "Remedy Action")]
        public string RemedyAction { get; set; }
        [Display(Name = "Status Update")]
        public string StatusUpdate { get; set; }

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

    #region Incident Quality Generation Metadata
    [MetadataType(typeof(IncidentQualityGenerationHelper))]
    public partial class IncidentQualityGeneration { }

    public partial class IncidentQualityGenerationHelper
    {
        [Display(Name = "ID")]
        public int IncidentQualityGenerationId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }

        [Display(Name = "Name of Power Generator")]
        public string NameOfPowerGenerator { get; set; }
        [Display(Name = "Preliminary Cause of Outage")]
        public string PreliminaryCauseOfOutage { get; set; }
        [Display(Name = "Expected Restoration Time (hour)")]
        [Range((double)0, double.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<decimal> ExpectedRestorationTime { get; set; }
        [Display(Name = "Full Restoration")]
        public Nullable<System.DateTime> FullRestoration { get; set; }
        [Display(Name = "CEM Network Being Affected")]
        public Nullable<bool> IsCEMNetworkBeingAffected { get; set; }
        [Display(Name = "Loss of Power (MW)")]
        [Range((double)0, double.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<decimal> LossOfPower { get; set; }

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

    #region Incident Quality Network Metadata
    [MetadataType(typeof(IncidentQualityNetworkHelper))]
    public partial class IncidentQualityNetwork { }

    public partial class IncidentQualityNetworkHelper
    {
        [Display(Name = "ID")]
        public int IncidentQualityNetworkId { get; set; }
        [Display(Name = "Checklist Batch ID")]
        public int ChecklistBatchId { get; set; }
        [Display(Name = "ChatRoom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "General Enquiry ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Crisis No")]
        public Nullable<int> CrisisId { get; set; }
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
        [Display(Name = "Level of Severity")]
        public string LevelOfSeverity { get; set; }
        [Display(Name = "Incident Status")]
        public string IncidentStatus { get; set; }
        [Display(Name = "Incident Background")]
        [DataType(DataType.MultilineText)]
        public string IncidentBackground { get; set; }
        [Display(Name = "Drill Mode")]
        public Nullable<bool> IsDrillMode { get; set; }
        [Display(Name = "History")]
        public string History { get; set; }

        [Display(Name = "Affected Area")]
        [DataType(DataType.MultilineText)]
        public string AffectedArea { get; set; }
        [Display(Name = "Affected Area (Chi)")]
        [DataType(DataType.MultilineText)]
        public string AffectedArea_Chi { get; set; }
        [Display(Name = "Affected Building")]
        [DataType(DataType.MultilineText)]
        public string AffectedBuilding { get; set; }
        [Display(Name = "Affected Building (Chi)")]
        [DataType(DataType.MultilineText)]
        public string AffectedBuilding_Chi { get; set; }
        [Display(Name = "Affected Street")]
        [DataType(DataType.MultilineText)]
        public string AffectedStreet { get; set; }
        [Display(Name = "Affected Street (Chi)")]
        [DataType(DataType.MultilineText)]
        public string AffectedStreet_Chi { get; set; }
        [Display(Name = "Outage Start Time")]
        public Nullable<System.DateTime> OutageStartTime { get; set; }
        [Display(Name = "Full Restoration")]
        public Nullable<System.DateTime> FullRestoration { get; set; }
        [Display(Name = "No of Building")]
        public Nullable<int> NoOfBuilding { get; set; }
        [Display(Name = "No of Customer Affected")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfCustomerAffected { get; set; }
        [Display(Name = "No of Platinum Customer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfPlatinumCustomer { get; set; }
        [Display(Name = "No of Diamond Customer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfDiamondCustomer { get; set; }
        [Display(Name = "No of Gold Customer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfGoldCustomer { get; set; }
        [Display(Name = "No of Silver Customer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfSilverCustomer { get; set; }
        [Display(Name = "Possible Cause")]
        [DataType(DataType.MultilineText)]
        public string PossibleCause { get; set; }
        [Display(Name = "Possible Cause (Chi)")]
        [DataType(DataType.MultilineText)]
        public string PossibleCause_Chi { get; set; }
        [Display(Name = "Expected Restoration Time (hour)")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<short> ExpectedRestorationTime { get; set; }
        [Display(Name = "Restoration Method")]
        [DataType(DataType.MultilineText)]
        public string RestorationMethod { get; set; }
        [Display(Name = "Restoration Method (Chi)")]
        [DataType(DataType.MultilineText)]
        public string RestorationMethod_Chi { get; set; }
        [Display(Name = "Status Update")]
        public string StatusUpdate { get; set; }
        [Display(Name = "Root Cause")]
        [DataType(DataType.MultilineText)]
        public string RootCause { get; set; }
        [Display(Name = "Root Cause (Chi)")]
        [DataType(DataType.MultilineText)]
        public string RootCause_Chi { get; set; }
        [Display(Name = "Loss Generation")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> LossGeneration { get; set; }
        [Display(Name = "Loss Interconnection")]
        public string LossInterconnection { get; set; }
        [Display(Name = "Loss Transmission")]
        public string LossTransmission { get; set; }
        [Display(Name = "Outage Level")]
        public string OutageLevel { get; set; }
        [Display(Name = "Double Fault")]
        public Nullable<bool> IsDoubleFault { get; set; }
        [Display(Name = "PQ Event Affect Large Customer")]
        public Nullable<bool> IsPQEventAffectLargeCustomer { get; set; }
        [Display(Name = "Critical Point")]
        public Nullable<bool> IsCriticalPoint { get; set; }

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

    #region General Enquiry Metadata
    [MetadataType(typeof(GeneralEnquiryHelper))]
    public partial class GeneralEnquiry { }

    public partial class GeneralEnquiryHelper
    {
        [Display(Name = "ID")]
        public int GeneralEnquiryId { get; set; }
        [Display(Name = "Chatroom ID")]
        public int ChatRoomId { get; set; }
        [Display(Name = "Enquiry Type")]
        public Nullable<int> GeneralEnquiryTypeId { get; set; }
        [Display(Name = "Enquiry Background")]
        [DataType(DataType.MultilineText)]
        public string Background { get; set; }
        [Display(Name = "Enquiry Status")]
        public string Status { get; set; }
        [Display(Name = "Issue By")]
        public Nullable<int> IssueById { get; set; }
        [Display(Name = "Issue Date Time")]
        public Nullable<System.DateTime> IssueDateTime { get; set; }
        [Display(Name = "Close By")]
        public Nullable<int> CloseById { get; set; }
        [Display(Name = "Close Date Time")]
        public Nullable<System.DateTime> CloseDateTime { get; set; }
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

    #region Notification Metadata
    [MetadataType(typeof(NotificationHelper))]
    public partial class Notification { }

    public partial class NotificationHelper
    {
        [Display(Name = "Created Date Time")]
        public DateTime? CreatedDateTime { get; set; }
        [Display(Name = "Notification Message")]
        public String Message { get; set; }
    }
    #endregion

    #region OMS Event Metadata
    [MetadataType(typeof(OMSEventHelper))]
    public partial class OMSEvent { }

    public partial class OMSEventHelper
    {
        [Display(Name = "ID")]
        public int OMSEventId { get; set; }
        [Display(Name = "OMS No")]
        public string OMSNo { get; set; }
        [Display(Name = "Affected Area")]
        public string AffectedArea { get; set; }
        [Display(Name = "Affected Area")]
        public string AffectedArea_Chi { get; set; }
        [Display(Name = "Affected Building")]
        public string AffectedBuilding { get; set; }
        [Display(Name = "Affected Building")]
        public string AffectedBuilding_Chi { get; set; }
        [Display(Name = "Affected Street")]
        public string AffectedStreet { get; set; }
        [Display(Name = "Affected Street")]
        public string AffectedStreet_Chi { get; set; }
        [Display(Name = "Outage Start Time")]
        public Nullable<System.DateTime> OutageStartTime { get; set; }
        [Display(Name = "Full Restoration")]
        public Nullable<System.DateTime> FullRestoration { get; set; }
        [Display(Name = "No of Building")]
        public Nullable<short> NoOfBuilding { get; set; }
        [Display(Name = "No of Customer Affected")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfCustomerAffected { get; set; }
        [Display(Name = "No of Platinum Customer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfPlatinumCustomer { get; set; }
        [Display(Name = "No of Diamond Customer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfDiamondCustomer { get; set; }
        [Display(Name = "No of Gold Customer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfGoldCustomer { get; set; }
        [Display(Name = "No of Silver Customer")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<int> NoOfSilverCustomer { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause { get; set; }
        [Display(Name = "Possible Cause")]
        public string PossibleCause_Chi { get; set; }
        [Display(Name = "Expected Restoration DateTime")]
        public Nullable<System.DateTime> ExpectedRestorationDateTime { get; set; }
        [Display(Name = "Expected Restoration Time (hour)")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter positive number")]
        public Nullable<short> ExpectedRestorationTime { get; set; }
        [Display(Name = "Restoration Method")]
        public string RestorationMethod { get; set; }
        [Display(Name = "Restoration Method")]
        public string RestorationMethod_Chi { get; set; }
        [Display(Name = "Status Update Code")]
        public string StatusUpdateCode { get; set; }
        [Display(Name = "Status Update")]
        public string StatusUpdate { get; set; }
        [Display(Name = "Root Cause")]
        public string RootCause { get; set; }
        [Display(Name = "Root Cause")]
        public string RootCause_Chi { get; set; }
        [Display(Name = "MV Outage")]
        public Nullable<bool> MVOutage { get; set; }
        [Display(Name = "LV Outage")]
        public Nullable<bool> LVOutage { get; set; }
        [Display(Name = "Affected Points")]
        public string AffectedPoints { get; set; }
        [Display(Name = "Critcal PT")]
        public Nullable<bool> CriticalPoint { get; set; }
        [Display(Name = "Reviewed")]
        public Nullable<bool> Reviewed { get; set; }
        [Display(Name = "OMS Status")]
        public string OMSStatus { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created DateTime")]
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        [Display(Name = "Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [Display(Name = "Last Update DateTime")]
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
    #endregion

    #region Department Metadata
    [MetadataType(typeof(DepartmentHelper))]
    public partial class Department { }

    public partial class DepartmentHelper
    {
        [Display(Name = "ID")]
        public int DepartmentId { get; set; }
        [Display(Name = "Department")]
        public string Department1 { get; set; }
    }
    #endregion

    #region ChecklistTemplate Metadata
    [MetadataType(typeof(ChecklistTemplateHelper))]
    public partial class ChecklistTemplate { }

    public partial class ChecklistTemplateHelper
    {
        [Display(Name = "ID")]
        public int ChecklistTemplateId { get; set; }
        [Display(Name = "Incident Type ID")]
        public int IncidentTypeId { get; set; }
        [Display(Name = "Crisis Type ID")]
        public int CrisisTypeId { get; set; }
        [Display(Name = "Department ID")]
        public int DepartmentId { get; set; }
        [Display(Name = "Sorting Order")]
        public Nullable<short> SortingOrder { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created DateTime")]
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        [Display(Name = "Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [Display(Name = "Last Updated DateTime")]
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
    #endregion

    #region ChecklistActionTemplate Metadata
    [MetadataType(typeof(ChecklistActionTemplateHelper))]
    public partial class ChecklistActionTemplate { }

    public partial class ChecklistActionTemplateHelper
    {
        [Display(Name = "ID")]
        public int ChecklistActionTemplateId { get; set; }
        [Display(Name = "Checklist Template ID")]
        public int ChecklistTemplateId { get; set; }
        [Display(Name = "Action")]
        public string Action { get; set; }
        [Display(Name = "Is Compulsory?")]
        public Nullable<bool> IsCompulsory { get; set; }
        [Display(Name = "ID")]
        public Nullable<short> AlertThreshold { get; set; }
        [Display(Name = "Sorting Order")]
        public Nullable<short> SortingOrder { get; set; }
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created DateTime")]
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        [Display(Name = "Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [Display(Name = "Last Updated DateTime")]
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
    #endregion
}
