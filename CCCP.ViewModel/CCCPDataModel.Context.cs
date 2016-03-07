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
        public virtual DbSet<IncidentSystemBilling> IncidentSystemBilling { get; set; }
        public virtual DbSet<IncidentSystemCallCentre> IncidentSystemCallCentre { get; set; }
        public virtual DbSet<IncidentSystemInvoicing> IncidentSystemInvoicing { get; set; }
        public virtual DbSet<InputOption> InputOption { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<IncidentType> IncidentType { get; set; }
        public virtual DbSet<Checklist> Checklist { get; set; }
        public virtual DbSet<IncidentEnvironmentAirEmission> IncidentEnvironmentAirEmission { get; set; }
        public virtual DbSet<IncidentEnvironmentLeakage> IncidentEnvironmentLeakage { get; set; }
        public virtual DbSet<IncidentQualityCorporateImage> IncidentQualityCorporateImage { get; set; }
        public virtual DbSet<IncidentQualityGeneration> IncidentQualityGeneration { get; set; }
        public virtual DbSet<IncidentQualityNetwork> IncidentQualityNetwork { get; set; }
        public virtual DbSet<IncidentSystemITSystem> IncidentSystemITSystem { get; set; }
        public virtual DbSet<IncidentSystemNetworkConnectivity> IncidentSystemNetworkConnectivity { get; set; }
        public virtual DbSet<IncidentSystemOTSystem> IncidentSystemOTSystem { get; set; }
        public virtual DbSet<SystemFunction> SystemFunction { get; set; }
    
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
    }
}
