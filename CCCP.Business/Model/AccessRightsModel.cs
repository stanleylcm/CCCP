using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;

namespace CCCP.Business.Model
{
    public class AccessRightsModel
    {
        public AccessRightsModel()
        {
        }

        public List<IncidentTypeSubType> SMRoles;
        public List<IncidentTypeSubType> ECRoles;
        public List<IncidentTypeSubType> CreateUpdateRights = new List<IncidentTypeSubType>();
        public List<IncidentTypeAndLevel> ChatRoomRights = new List<IncidentTypeAndLevel>();
        public List<IncidentTypeAndLevel> NotificationRights = new List<IncidentTypeAndLevel>();
        public List<IncidentTypeAndDepartment> ActionChecklistRights = new List<IncidentTypeAndDepartment>();

        public bool HasUpdateIncidentLevelRights(IncidentTypeSubType incidentType)
        {
            return SMRoles.Contains(incidentType);
        }
        public bool HasApproveCrisisRights(IncidentTypeSubType incidentType)
        {
            return ECRoles.Contains(incidentType);
        }
        public bool HasCloseCrisisRights(IncidentTypeSubType incidentType)
        {
            return ECRoles.Contains(incidentType);
        }
        public bool HasCreateUpdateRights(IncidentTypeSubType incidentType)
        {
            return CreateUpdateRights.Contains(incidentType);
        }
        public bool HasChatRoomRights(IncidentTypeSubType incidentType, IncidentLevelWithCrisis incidentLevel)
        {
            IncidentTypeAndLevel toCheck = new IncidentTypeAndLevel() { IncidentType = incidentType, IncidentLevel = incidentLevel };
            return ChatRoomRights.Contains(toCheck);
        }
        public bool HasNotificationRights(IncidentTypeSubType incidentType, IncidentLevelWithCrisis incidentLevel)
        {
            IncidentTypeAndLevel toCheck = new IncidentTypeAndLevel() { IncidentType = incidentType, IncidentLevel = incidentLevel };
            return NotificationRights.Contains(toCheck);
        }
        public bool HasActionChecklistRights(IncidentTypeSubType incidentType, string department)
        {
            IncidentTypeAndDepartment toCheck = new IncidentTypeAndDepartment() { IncidentType=incidentType, Department=department };
            return ActionChecklistRights.Contains(toCheck);
        }        
    }

    public class IncidentTypeAndLevel : ICloneable
    {
        public IncidentTypeSubType IncidentType;
        public IncidentLevelWithCrisis IncidentLevel;

        public object Clone()
        {
            IncidentTypeAndLevel result = new IncidentTypeAndLevel();
            result.IncidentType = this.IncidentType;
            result.IncidentLevel = this.IncidentLevel;

            return result;
        }
    }
    public class IncidentTypeAndDepartment : ICloneable
    {
        public IncidentTypeSubType IncidentType;
        public string Department = "";

        public object Clone()
        {
            IncidentTypeAndDepartment result = new IncidentTypeAndDepartment();
            result.IncidentType = this.IncidentType;
            result.Department = this.Department;

            return result;
        }
    }
}
