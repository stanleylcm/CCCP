﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class CCCPDbContext : DbContext
    {
        public CCCPDbContext()
            : base("name=CCCPDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<ChecklistAction> ChecklistAction { get; set; }
        public virtual DbSet<ChecklistBatch> ChecklistBatch { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<InputOption> InputOption { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<IncidentType> IncidentType { get; set; }
        public virtual DbSet<Checklist> Checklist { get; set; }
        public virtual DbSet<SystemFunction> SystemFunction { get; set; }
        public virtual DbSet<IncidentEnvironmentAirEmission> IncidentEnvironmentAirEmission { get; set; }
        public virtual DbSet<IncidentSystemBilling> IncidentSystemBilling { get; set; }
        public virtual DbSet<IncidentSystemITSystem> IncidentSystemITSystem { get; set; }
        public virtual DbSet<IncidentSystemNetworkConnectivity> IncidentSystemNetworkConnectivity { get; set; }
        public virtual DbSet<IncidentSystemOTSystem> IncidentSystemOTSystem { get; set; }
        public virtual DbSet<OMSEvent> OMSEvent { get; set; }
        public virtual DbSet<IncidentEnvironmentLeakage> IncidentEnvironmentLeakage { get; set; }
        public virtual DbSet<IncidentOHS> IncidentOHS { get; set; }
        public virtual DbSet<IncidentQualityCorporateImage> IncidentQualityCorporateImage { get; set; }
        public virtual DbSet<IncidentSystemCallCentre> IncidentSystemCallCentre { get; set; }
        public virtual DbSet<ChatRoom> ChatRoom { get; set; }
        public virtual DbSet<ChatRoomMessage> ChatRoomMessage { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<IncidentSystemInvoicing> IncidentSystemInvoicing { get; set; }
        public virtual DbSet<IncidentQualityNetwork> IncidentQualityNetwork { get; set; }
        public virtual DbSet<ChatRoomAttachment> ChatRoomAttachment { get; set; }
        public virtual DbSet<GeneralEnquiry> GeneralEnquiry { get; set; }
        public virtual DbSet<GeneralEnquiryIncidentLink> GeneralEnquiryIncidentLink { get; set; }
        public virtual DbSet<IncidentQualityGeneration> IncidentQualityGeneration { get; set; }
    
        public virtual ObjectResult<usp_IncidentSystemBilling_Test_Result> usp_IncidentSystemBilling_Test()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_IncidentSystemBilling_Test_Result>("usp_IncidentSystemBilling_Test");
        }
    
        public virtual int usp_Incident_PostCreate(Nullable<int> incidentId, Nullable<int> incidentTypeId, string createdBy, string defaultActionStatus)
        {
            var incidentIdParameter = incidentId.HasValue ?
                new ObjectParameter("IncidentId", incidentId) :
                new ObjectParameter("IncidentId", typeof(int));
    
            var incidentTypeIdParameter = incidentTypeId.HasValue ?
                new ObjectParameter("IncidentTypeId", incidentTypeId) :
                new ObjectParameter("IncidentTypeId", typeof(int));
    
            var createdByParameter = createdBy != null ?
                new ObjectParameter("CreatedBy", createdBy) :
                new ObjectParameter("CreatedBy", typeof(string));
    
            var defaultActionStatusParameter = defaultActionStatus != null ?
                new ObjectParameter("DefaultActionStatus", defaultActionStatus) :
                new ObjectParameter("DefaultActionStatus", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_Incident_PostCreate", incidentIdParameter, incidentTypeIdParameter, createdByParameter, defaultActionStatusParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> usp_GetNextSequenceNo(string sequenceType, Nullable<short> year)
        {
            var sequenceTypeParameter = sequenceType != null ?
                new ObjectParameter("SequenceType", sequenceType) :
                new ObjectParameter("SequenceType", typeof(string));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("Year", year) :
                new ObjectParameter("Year", typeof(short));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("usp_GetNextSequenceNo", sequenceTypeParameter, yearParameter);
        }
    
        public virtual ObjectResult<SystemFunctionExtend> usp_GetUserFunctions(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SystemFunctionExtend>("usp_GetUserFunctions", userIdParameter);
        }
    
        public virtual ObjectResult<usp_Dashboard_GetIncidentProgress_Result> usp_Dashboard_GetIncidentProgress()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Dashboard_GetIncidentProgress_Result>("usp_Dashboard_GetIncidentProgress");
        }
    
        public virtual ObjectResult<usp_Dashboard_GetGeneralEnquiryProgress_Result> usp_Dashboard_GetGeneralEnquiryProgress()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Dashboard_GetGeneralEnquiryProgress_Result>("usp_Dashboard_GetGeneralEnquiryProgress");
        }
    
        public virtual ObjectResult<usp_Dashboard_GetIncidentSummary_Result> usp_Dashboard_GetIncidentSummary()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Dashboard_GetIncidentSummary_Result>("usp_Dashboard_GetIncidentSummary");
        }
    
        public virtual ObjectResult<usp_Dashboard_GetIncidentSummary1_Result> usp_Dashboard_GetIncidentSummary1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Dashboard_GetIncidentSummary1_Result>("usp_Dashboard_GetIncidentSummary1");
        }
    
        public virtual ObjectResult<usp_Dashboard_GetOpenCloseIncident_Result> usp_Dashboard_GetOpenCloseIncident()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Dashboard_GetOpenCloseIncident_Result>("usp_Dashboard_GetOpenCloseIncident");
        }
    
        public virtual ObjectResult<usp_Dashboard_GetOutstandingCrisis_Result> usp_Dashboard_GetOutstandingCrisis()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Dashboard_GetOutstandingCrisis_Result>("usp_Dashboard_GetOutstandingCrisis");
        }
    
        public virtual ObjectResult<usp_Dashboard_GetOutstandingGeneralEnquiry_Result> usp_Dashboard_GetOutstandingGeneralEnquiry()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Dashboard_GetOutstandingGeneralEnquiry_Result>("usp_Dashboard_GetOutstandingGeneralEnquiry");
        }
    
        public virtual ObjectResult<usp_Dashboard_GetOutstandingIncident_Result> usp_Dashboard_GetOutstandingIncident()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Dashboard_GetOutstandingIncident_Result>("usp_Dashboard_GetOutstandingIncident");
        }
    
        public virtual ObjectResult<usp_Dashboard_GetOutstandingIncident1_Result> usp_Dashboard_GetOutstandingIncident1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Dashboard_GetOutstandingIncident1_Result>("usp_Dashboard_GetOutstandingIncident1");
        }
    
        public virtual ObjectResult<usp_Incident_GetIncidentForLink_Result> usp_Incident_GetIncidentForLink(string incidentId, string incidentTypeId)
        {
            var incidentIdParameter = incidentId != null ?
                new ObjectParameter("IncidentId", incidentId) :
                new ObjectParameter("IncidentId", typeof(string));
    
            var incidentTypeIdParameter = incidentTypeId != null ?
                new ObjectParameter("IncidentTypeId", incidentTypeId) :
                new ObjectParameter("IncidentTypeId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Incident_GetIncidentForLink_Result>("usp_Incident_GetIncidentForLink", incidentIdParameter, incidentTypeIdParameter);
        }
    
        public virtual int usp_Incident_LinkIncident(Nullable<int> incidentId, Nullable<int> incidentTypeId, Nullable<int> linkIncidentId, Nullable<int> linkIncidentTypeId)
        {
            var incidentIdParameter = incidentId.HasValue ?
                new ObjectParameter("IncidentId", incidentId) :
                new ObjectParameter("IncidentId", typeof(int));
    
            var incidentTypeIdParameter = incidentTypeId.HasValue ?
                new ObjectParameter("IncidentTypeId", incidentTypeId) :
                new ObjectParameter("IncidentTypeId", typeof(int));
    
            var linkIncidentIdParameter = linkIncidentId.HasValue ?
                new ObjectParameter("LinkIncidentId", linkIncidentId) :
                new ObjectParameter("LinkIncidentId", typeof(int));
    
            var linkIncidentTypeIdParameter = linkIncidentTypeId.HasValue ?
                new ObjectParameter("LinkIncidentTypeId", linkIncidentTypeId) :
                new ObjectParameter("LinkIncidentTypeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_Incident_LinkIncident", incidentIdParameter, incidentTypeIdParameter, linkIncidentIdParameter, linkIncidentTypeIdParameter);
        }
    
        public virtual ObjectResult<usp_Incident_GetLinkedIncident_Result> usp_Incident_GetLinkedIncident(Nullable<int> incidentId, Nullable<int> incidentTypeId)
        {
            var incidentIdParameter = incidentId.HasValue ?
                new ObjectParameter("IncidentId", incidentId) :
                new ObjectParameter("IncidentId", typeof(int));
    
            var incidentTypeIdParameter = incidentTypeId.HasValue ?
                new ObjectParameter("IncidentTypeId", incidentTypeId) :
                new ObjectParameter("IncidentTypeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Incident_GetLinkedIncident_Result>("usp_Incident_GetLinkedIncident", incidentIdParameter, incidentTypeIdParameter);
        }
    
        public virtual int usp_GeneralEnquiry_PostCreate(Nullable<int> generalEnquiryId, string createdBy)
        {
            var generalEnquiryIdParameter = generalEnquiryId.HasValue ?
                new ObjectParameter("GeneralEnquiryId", generalEnquiryId) :
                new ObjectParameter("GeneralEnquiryId", typeof(int));
    
            var createdByParameter = createdBy != null ?
                new ObjectParameter("CreatedBy", createdBy) :
                new ObjectParameter("CreatedBy", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_GeneralEnquiry_PostCreate", generalEnquiryIdParameter, createdByParameter);
        }
    
        public virtual ObjectResult<usp_Dashboard_GetOutstandingGeneralEnquiry1_Result> usp_Dashboard_GetOutstandingGeneralEnquiry1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Dashboard_GetOutstandingGeneralEnquiry1_Result>("usp_Dashboard_GetOutstandingGeneralEnquiry1");
        }
    
        public virtual ObjectResult<usp_Incident_GetLinkedGeneralEnquiry_Result> usp_Incident_GetLinkedGeneralEnquiry(Nullable<int> incidentId, Nullable<int> incidentTypeId)
        {
            var incidentIdParameter = incidentId.HasValue ?
                new ObjectParameter("IncidentId", incidentId) :
                new ObjectParameter("IncidentId", typeof(int));
    
            var incidentTypeIdParameter = incidentTypeId.HasValue ?
                new ObjectParameter("IncidentTypeId", incidentTypeId) :
                new ObjectParameter("IncidentTypeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Incident_GetLinkedGeneralEnquiry_Result>("usp_Incident_GetLinkedGeneralEnquiry", incidentIdParameter, incidentTypeIdParameter);
        }
    }
}
