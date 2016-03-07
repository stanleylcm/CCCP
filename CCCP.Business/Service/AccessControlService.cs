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

            // retreve access rights from DB
            result = getAccessRightsFromDB(userId);

            // Post processing
            // Chat Room Rights = Notification Rights
            result.ChatRoomRights = result.NotificationRights.Clone<IncidentTypeAndLevel>();

            // UpdateIncidentLevelRights = SMRole
            result.UpdateIncidentLevelRights = result.SMRoles.ConvertAll(x => x);

            // ApproveCrisisRights = ECRole
            result.ApproveCrisisRights = result.ECRoles.ConvertAll(x => x);

            // CloseCrisisRights = ECRole
            result.CloseCrisisRights = result.ECRoles.ConvertAll(x => x);

            // SM role has full rights
            processFullRights(ref result, result.SMRoles);

            // EC role has full rights
            processFullRights(ref result, result.ECRoles);

            return result;
        }

        private static void processFullRights(ref AccessRightsModel model, List<IncidentTypeSubType> incidentTypes)
        {
            foreach (IncidentTypeSubType incidentType in incidentTypes)
            {
                // Create Update Rights
                if (!model.CreateUpdateRights.Contains(incidentType))
                    model.CreateUpdateRights.Add(incidentType);

                // Chat Room Rights
                IncidentTypeAndLevel found = model.ChatRoomRights.Find(x => x.IncidentType == incidentType);
                if (found != null) model.ChatRoomRights.AddRange(found.GetDelta(incidentType)); // incident type found
                else model.ChatRoomRights.AddRange(IncidentTypeAndLevel.GetAllLevels(incidentType)); // incident type not found

                // Action Checklist Rights
                IncidentTypeAndDepartment found1 = model.ActionChecklistRights.Find(x => x.IncidentType == incidentType);
                if (found1 != null) model.ActionChecklistRights.AddRange(found1.GetDelta(incidentType)); // incident type found
                else model.ActionChecklistRights.AddRange(IncidentTypeAndDepartment.GetAllDepartments(incidentType)); // incident type not found 
            }
        }

        private static AccessRightsModel getAccessRightsFromDB(int userId)
        {
            AccessRightsModel result = new AccessRightsModel();

            // handle rights
            List<SystemFunctionExtend> functions = new List<SystemFunctionExtend>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                functions = db.usp_GetUserFunctions(userId).ToList<SystemFunctionExtend>();
            }
            addFunctionToAccessRightsModel(ref result, functions);

            // handle roles


            return result;
        }

        private static void addFunctionToAccessRightsModel(ref AccessRightsModel model, List<SystemFunctionExtend> functions)
        {
            List<Department> departments = MasterTableService.GetDepartments();
            List<ViewModel.IncidentType> incidentTypes = MasterTableService.GetIncidentTypes();

            foreach (SystemFunctionExtend function in functions)
            {
                bool toEnumResult = false;
                SystemFunctionCode systemFunction = function.Code.ToEnum<SystemFunctionCode>(out toEnumResult);
                if (toEnumResult)
                {
                    IncidentTypeSubType incidentType = MasterTableService.GetIncidentTypeSubType(function.IncidentTypeId, incidentTypes);

                    switch (systemFunction)
                    {
                        case SystemFunctionCode.SM_Role:
                            {
                                model.SMRoles.Add(incidentType);
                                break;
                            }
                        case SystemFunctionCode.EC_Role:
                            {
                                model.ECRoles.Add(incidentType);
                                break;
                            }
                        case SystemFunctionCode.CreateUpdate:
                            {
                                model.CreateUpdateRights.Add(incidentType);
                                break;
                            }
                        case SystemFunctionCode.Notification:
                            {
                                IncidentLevelWithCrisis incidentLevel = function.IncidentLevel.ToEnum<IncidentLevelWithCrisis>();
                                IncidentTypeAndLevel value = new IncidentTypeAndLevel() { IncidentType = incidentType, IncidentLevel = incidentLevel };
                                model.NotificationRights.Add(value);
                                break;
                            }
                        case SystemFunctionCode.ActionChecklist:
                            {
                                string department = MasterTableService.GetDepartmentStr(function.DepartmentId, departments);
                                IncidentTypeAndDepartment value = new IncidentTypeAndDepartment() { IncidentType = incidentType, Department = department };
                                model.ActionChecklistRights.Add(value);
                                break;
                            }
                    }
                }

            }
        }
    }
}
