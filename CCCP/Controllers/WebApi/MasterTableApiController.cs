using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.ViewModel;
using CCCP.Common;
using System.Data;
using System.Data.Entity;

namespace CCCP.Controllers.WebApi
{
    public class MasterTableApiController : ApiController
    {
        public List<string> GetIncidentTypes()
        {
            return IncidentTypeSubType.EnvironmentAirEmission.ListAll();
        }

        public List<string> GetDepartments()
        {
            return MasterTableService.GetDepartmentStrs();
        }

        public List<string> GetIncidentLevels()
        {
            return IncidentLevelWithCrisis.Crisis.ListAll();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public Boolean LinkIncident(int id, int typeId, int linkId, int linkTypeId)
        {
            CCCPDbContext db = new CCCPDbContext();

            db.usp_Incident_LinkIncident(id, typeId, linkId, linkTypeId);

            return true;
        }
    }
}