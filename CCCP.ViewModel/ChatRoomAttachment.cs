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
    
    public partial class ChatRoomAttachment
    {
        public int ChatRoomAttachmentId { get; set; }
        public int ChatRoomMessageId { get; set; }
        public byte[] Attachment { get; set; }
        public string AttachmentType { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDateTime { get; set; }
    }
}
