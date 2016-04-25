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
    
    public partial class IncidentQualityNetwork
    {
        public int IncidentQualityNetworkId { get; set; }
        public int OMSEventId { get; set; }
        public int ChecklistBatchId { get; set; }
        public int ChatRoomId { get; set; }
        public Nullable<int> GeneralEnquiryId { get; set; }
        public Nullable<int> CrisisId { get; set; }
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
        public string AffectedArea { get; set; }
        public Nullable<System.DateTime> OutageStartTime { get; set; }
        public Nullable<System.DateTime> FullRestoration { get; set; }
        public Nullable<int> NoOfBuilding { get; set; }
        public Nullable<int> NoOfCustomerAffected { get; set; }
        public Nullable<int> NoOfPlatinumCustomer { get; set; }
        public Nullable<int> NoOfDiamondCustomer { get; set; }
        public Nullable<int> NoOfGoldCustomer { get; set; }
        public Nullable<int> NoOfSilverCustomer { get; set; }
        public string PossibleCause { get; set; }
        public Nullable<short> ExpectedRestorationTime { get; set; }
        public string RestorationMethod { get; set; }
        public string StatusUpdate { get; set; }
        public string RootCause { get; set; }
        public Nullable<int> LossGeneration { get; set; }
        public string LossInterconnection { get; set; }
        public string LossTransmission { get; set; }
        public Nullable<bool> MVOutage { get; set; }
        public Nullable<bool> IsDoubleFault { get; set; }
        public Nullable<bool> LVOutage { get; set; }
        public Nullable<bool> IsPQEventAffectLargeCustomer { get; set; }
        public Nullable<bool> IsCriticalPoint { get; set; }
        public string AffectedPoints { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
}
