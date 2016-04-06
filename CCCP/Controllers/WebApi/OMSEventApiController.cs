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
    public class OMSEventApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int Create(OMSEventApiModel omsEvent)
        {
            CCCPDbContext db = new CCCPDbContext();
            OMSEventModel omsEventModel = new OMSEventModel(omsEvent);
            omsEventModel.PrepareSave(PrepareSaveMode.Created);

            db.OMSEvent.Add(omsEventModel.Entity);
            db.SaveChanges();

            return omsEventModel.Entity.OMSEventId;
        }
    }
}