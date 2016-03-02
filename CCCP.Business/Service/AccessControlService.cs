using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.Business.Model;
using CCCP.ViewModel;


namespace CCCP.Business.Service
{
    public class AccessControlService
    {
        public static UserModel CurrentUser = new UserModel();

        public static AccessRightsModel GetAccessRights(int userId)
        {
            AccessRightsModel result = new AccessRightsModel();

            // 

            // Post processing
            // Chat Room Rights = Notification Rights
            result.ChatRoomRights = result.NotificationRights.Clone<IncidentTypeAndLevel>();

            // SM roles has full rights
            foreach (IncidentTypeSubType incidentType in result.SMRoles)
            {
                // Create Update Rights
                if (!result.CreateUpdateRights.Contains(incidentType)) 
                    result.CreateUpdateRights.Add(incidentType);
                
                // Chat Room Rights
                IncidentTypeAndLevel found = result.ChatRoomRights.Find(x => x.IncidentType == incidentType);
                if (found != null) result.ChatRoomRights.AddRange(found.GetDelta(incidentType)); // incident type found
                else result.ChatRoomRights.AddRange(IncidentTypeAndLevel.GetAllLevels(incidentType)); // incident type not found

                // Action Checklist Rights

            }

            return result;
        }
    }
}
