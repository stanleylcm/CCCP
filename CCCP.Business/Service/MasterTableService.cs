using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.ViewModel;

namespace CCCP.Business.Service
{
    public class MasterTableService
    {
        #region Incident Type

        public static int GetIncidentTypeId(IncidentTypeSubType incidentType)
        {
            int result = 0;
            using (CCCPDbContext db = new CCCPDbContext())
            {
                string incidentTypeStr = incidentType.ToEnumString();
                result = db.IncidentType.SingleOrDefault(x => x.IncidentType1 == incidentTypeStr).IncidentTypeId;
            }

            return result;
        }

        public static string GetIncidentTypeName(IncidentTypeSubType incidentType)
        {
            string ret = "";
            using (CCCPDbContext db = new CCCPDbContext())
            {
                string incidentTypeStr = incidentType.ToEnumString();
                CCCP.ViewModel.IncidentType result = db.IncidentType.SingleOrDefault(x => x.IncidentType1 == incidentTypeStr);
                ret = result.Type;
                if (result.SubType != null)
                {
                    ret = string.Format("{0} - {1}", ret, result.SubType);
                }
            }

            return ret;
        }

        public static List<ViewModel.IncidentType> GetIncidentTypes()
        {
            List<ViewModel.IncidentType> result = new List<ViewModel.IncidentType>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.IncidentType.Where(x => x.IncidentType1.IndexOf("___") < 0).ToList<ViewModel.IncidentType>();
            }

            return result;
        }

        public static IncidentTypeSubType GetIncidentTypeSubType(int incidentTypeId)
        {
            List<ViewModel.IncidentType> dbIncidentTypes = GetIncidentTypes();
            return GetIncidentTypeSubType(incidentTypeId, dbIncidentTypes);
        }
        public static IncidentTypeSubType GetIncidentTypeSubType(int incidentTypeId, List<ViewModel.IncidentType> dbIncidentTypes)
        {
            string incidentTypeStr = dbIncidentTypes.Find(x => x.IncidentTypeId == incidentTypeId).IncidentType1;
            return incidentTypeStr.ToEnum<IncidentTypeSubType>();
        }

        #endregion

        #region Department

        public static List<string> GetDepartmentStrs()
        {
            List<string> result = new List<string>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.Department.Select(x => x.Department1).ToList();
            }

            return result;
        }

        public static List<Department> GetDepartments()
        {
            List<Department> result = new List<Department>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.Department.ToList<Department>();
            }

            return result;
        }

        public static string GetDepartmentStr(int departmentId)
        {
            List<Department> dbDepartments = GetDepartments();
            return GetDepartmentStr(departmentId, dbDepartments);
        }
        public static string GetDepartmentStr(int departmentId, List<Department> dbDepartments)
        {
            return dbDepartments.Find(x => x.DepartmentId == departmentId).Department1;
        }

        #endregion

        public static List<string> GetCriticalPoints()
        {
            List<string> result = new List<string>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.CriticalPoint.Select(x => x.AffectedPoint).ToList();
            }

            return result;
        }
    }
}
