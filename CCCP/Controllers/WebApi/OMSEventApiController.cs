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
        #region WebApi For OMS Interface
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public int SubmitOMSEvent(OMSEventApiModel omsEvent)
        {
            CCCPDbContext db = new CCCPDbContext();
            OMSEventModel omsEventModel = new OMSEventModel();
            List<OMSEvent> omsLists = db.OMSEvent.Where(x => x.OMSNo == omsEvent.OMSNo).ToList();
            if (omsLists.Count() == 0)
            {
                // create
                omsEventModel = new OMSEventModel(omsEvent);
                omsEventModel.PrepareSave(PrepareSaveMode.Created);

                db.OMSEvent.Add(omsEventModel.Entity);
            }
            else
            {
                // Edit
                OMSEvent oms = omsLists.FirstOrDefault();
                oms.OMSStatus = OMSStatus.In_Progress.ToEnumString();
                omsEventModel = new OMSEventModel(oms, omsEvent);
                omsEventModel.PrepareSave();

                db.OMSEvent.Attach(omsEventModel.Entity);

                db.Entry(omsEventModel.Entity).State = EntityState.Modified;
            }

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                // output validation errors
                Console.WriteLine(sb.ToString());
            }

            return omsEventModel.Entity.OMSEventId;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public Boolean CancelOMSEvent(String OMSNo)
        {
            CCCPDbContext db = new CCCPDbContext();
            OMSEvent oms = db.OMSEvent.Where(x => x.OMSNo == OMSNo).FirstOrDefault();
            oms.OMSStatus = OMSStatus.Cancelled.ToEnumString();
            OMSEventModel omsEventModel = new OMSEventModel(oms);
            omsEventModel.PrepareSave();

            db.OMSEvent.Attach(omsEventModel.Entity);

            db.Entry(omsEventModel.Entity).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                // output validation errors
                Console.WriteLine(sb.ToString());
            }

            return omsEventModel.Entity.OMSEventId > 0;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public Boolean CloseOMSEvent(OMSEventApiModel omsEvent)
        {
            CCCPDbContext db = new CCCPDbContext();
            OMSEvent oms = db.OMSEvent.Where(x => x.OMSNo == omsEvent.OMSNo).FirstOrDefault();
            oms.OMSStatus = OMSStatus.Closed.ToEnumString();
            OMSEventModel omsEventModel = new OMSEventModel(oms, omsEvent);
            omsEventModel.PrepareSave();

            db.OMSEvent.Attach(omsEventModel.Entity);

            db.Entry(omsEventModel.Entity).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                // output validation errors
                Console.WriteLine(sb.ToString());
            }

            return omsEventModel.Entity.OMSEventId > 0;
        }
#endregion
        
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public OMSEventModel GetOMSEvent(int id)
        {
            OMSEventModel result = new OMSEventModel();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                // load incident details
                result.Entity = db.OMSEvent.Find(id);
            }

            // result
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public OMSEventModel GetNewOMSEvent()
        {
            OMSEventModel result = new OMSEventModel();
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public OMSEventModel GetOMSEvent()
        {
            OMSEventModel result = new OMSEventModel();
            return result;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public List<OMSEvent> GetOMSEventList()
        {
            List<OMSEvent> result = new List<OMSEvent>();
            using (CCCPDbContext db = new CCCPDbContext())
            {
                result = db.OMSEvent.ToList();
            }
            return result;
        }
    }
}