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
    
    public partial class GeneralEnquiry
    {
        public int GeneralEnquiryId { get; set; }
        public int ChatRoomId { get; set; }
        public Nullable<int> GeneralEnquiryTypeId { get; set; }
        public string Background { get; set; }
        public string Status { get; set; }
        public Nullable<int> IssueById { get; set; }
        public Nullable<System.DateTime> IssueDateTime { get; set; }
        public Nullable<int> CloseById { get; set; }
        public Nullable<System.DateTime> CloseDateTime { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
}
