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
    
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> GeneralEnquiryId { get; set; }
        public Nullable<int> IncidentTypeId { get; set; }
        public Nullable<int> IncidentId { get; set; }
        public Nullable<int> CrisisId { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
}
