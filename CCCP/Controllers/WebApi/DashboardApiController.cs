﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CCCP.ViewModel;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.Common;

namespace CCCP.Controllers.WebApi
{
    public class DashboardApiController : ApiController
    {
        private CCCPDbContext db = new CCCPDbContext();

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<usp_Dashboard_GetOpenCloseIncident_Result> GetIncidentList()
        {            
            return db.usp_Dashboard_GetOpenCloseIncident().ToList<usp_Dashboard_GetOpenCloseIncident_Result>();
        }
    }
}