using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CCCP.Common;

namespace CCCP.ViewModel
{
    public partial class CCCPDbContext
    {
        private const string createdBy = "CreatedBy";
        private const string createdDateTime = "CreatedDateTime";
        private const string lastUpdatedBy = "LastUpdatedBy";
        private const string lastUpdatedDateTime = "LastUpdatedDateTime";

        public int SaveChanges(string userName)
        {
            List<AuditLog> AuditLogs = new List<AuditLog>();

            // Get all Added/Deleted/Modified entities (not Unmodified or Detached)
            foreach (var ent in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
            {
                // Audit Log handling
                List<AuditLog> EntityLogs = GetAuditRecordsForChange(ent, userName);
                foreach (AuditLog x in EntityLogs) this.AuditLog.Add(x);

                // Created By, Updated By handling
                DateTime now = DateTime.Now;
                if (ent.State == EntityState.Added) updateCreatedBy(ent, userName, now);                
                if (ent.State == EntityState.Modified) updateLastUpdated(ent, userName, now);
            }

            return base.SaveChanges();
        }

        private void updateLastUpdated(DbEntityEntry dbEntry, string userName, DateTime recDateTime)
        {
            PropertyInfo[] properties = dbEntry.Entity.GetType().GetProperties();

            // updated by
            if (properties.Contains(lastUpdatedBy)) dbEntry.Entity.GetType().GetProperty(lastUpdatedBy).SetValue(dbEntry.Entity, userName);

            // updated datetime
            if (properties.Contains(lastUpdatedDateTime)) dbEntry.Entity.GetType().GetProperty(lastUpdatedDateTime).SetValue(dbEntry.Entity, recDateTime);
        }
        private void updateCreatedBy(DbEntityEntry dbEntry, string userName, DateTime recDateTime)
        {
            PropertyInfo[] properties = dbEntry.Entity.GetType().GetProperties();

            // created by
            if (properties.Contains(createdBy)) dbEntry.Entity.GetType().GetProperty(createdBy).SetValue(dbEntry.Entity, userName);

            // created datetime
            if (properties.Contains(createdDateTime)) dbEntry.Entity.GetType().GetProperty(createdDateTime).SetValue(dbEntry.Entity, recDateTime);
        }

        private List<AuditLog> GetAuditRecordsForChange(DbEntityEntry dbEntry, string userName)
        {
            List<AuditLog> result = new List<AuditLog>();
            DateTime changeTime = DateTime.Now;
            string tableName = dbEntry.Entity.GetType().Name;

            // Get primary key value (If you have more than one key column, this will need to be adjusted)
            //var keyNames = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).ToList();
            //string keyName = keyNames[0].Name;
            string keyName = dbEntry.Entity.GetType().GetProperties()[0].Name;

            switch (dbEntry.State)
            {
                case EntityState.Added:
                    {
                        foreach (string propertyName in dbEntry.CurrentValues.PropertyNames)
                        {
                            result.Add(new AuditLog()
                            {
                                UserName = userName,
                                EventDateTime = changeTime,
                                EventType = AuditLogEventType.Add.ToEnumString(), // Add
                                TableName = tableName,
                                RecordId = dbEntry.CurrentValues.GetValue<int>(keyName),
                                ColumnName = propertyName,
                                NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                            });
                        }

                        break;
                    }
                case EntityState.Deleted:
                    {
                        result.Add(new AuditLog()
                        {
                            UserName = userName,
                            EventDateTime = changeTime,
                            EventType = AuditLogEventType.Delete.ToEnumString(), // Delete
                            TableName = tableName,
                            RecordId = dbEntry.OriginalValues.GetValue<int>(keyName),
                            ColumnName = "*ALL",
                            NewValue = dbEntry.OriginalValues.ToObject().ToString()
                        });

                        break;
                    }
                case EntityState.Modified:
                    {
                        foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                        {
                            // For updates, we only want to capture the columns that actually changed
                            if (!object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                            {
                                result.Add(new AuditLog()
                                {
                                    UserName = userName,
                                    EventDateTime = changeTime,
                                    EventType = AuditLogEventType.Modifiy.ToEnumString(),    // Modify
                                    TableName = tableName,
                                    RecordId = dbEntry.OriginalValues.GetValue<int>(keyName),
                                    ColumnName = propertyName,
                                    OriginalValue = dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                    NewValue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                                });
                            }
                        }

                        break;
                    }
            }

            // Otherwise, don't do anything, we don't care about Unchanged or Detached entities
            return result;
        }
    }
}

