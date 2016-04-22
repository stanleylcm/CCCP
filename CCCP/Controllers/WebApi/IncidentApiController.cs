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
    public class IncidentApiController : ApiController
    {
        private CCCPDbContext db = new CCCPDbContext();

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public Boolean LinkIncident(int id, int typeId, int linkId, int linkTypeId)
        {
            CCCPDbContext db = new CCCPDbContext();

            db.usp_Incident_LinkIncident(id, typeId, linkId, linkTypeId);

            return true;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public Boolean LinkGeneralEnquiry(int id, int typeId, int generalEnquiryId)
        {
            CCCPDbContext db = new CCCPDbContext();

            GeneralEnquiryIncidentLink gelink = new GeneralEnquiryIncidentLink();
            gelink.GeneralEnquiryId = generalEnquiryId;
            gelink.IncidentId = id;
            gelink.IncidentTypeId = typeId;

            db.GeneralEnquiryIncidentLink.Add(gelink);
            db.SaveChanges();

            return true;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<ViewModel.IncidentType> GetIncidentTypeList()
        {
            CCCPDbContext db = new CCCPDbContext();
            return db.IncidentType.Where(m => m.IncidentType1.IndexOf("___") < 0).ToList();
        }
    }
}