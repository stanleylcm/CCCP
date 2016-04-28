using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CCCP.ViewModel;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.Common;
using CCCP.Controllers.WebApi;

namespace CCCP.Controllers
{
    public class ChecklistMaintenanceController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public ChecklistTemplateModel checklistTemplateModel = new ChecklistTemplateModel();

        // GET: ChecklistMaintenance
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<ChecklistTemplate> checklistTemplates = db.ChecklistTemplate.ToList();
            List<ChecklistTemplateModel> checklistTemplateModels = checklistTemplates.ConvertAll(x => new ChecklistTemplateModel(x));
            return View(checklistTemplateModels);
        }

        // GET: ChecklistMaintenance/Create
        public ActionResult Create()
        {
            checklistTemplateModel = new ChecklistTemplateModel();
            if (Session != null)
            {
                Session["ChecklistTemplateModel"] = checklistTemplateModel;
            }
            return View(checklistTemplateModel);
        }

        // POST: ChecklistMaintenance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChecklistTemplateId,IncidentTypeId,CrisisTypeId,DepartmentId,SortingOrder,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] ChecklistTemplate checklistTemplate,
                                [Bind(Include = "ChecklistActionTemplateId,ChecklistTemplateId,Action,IsCompulsory,AlertThreshold,SortingOrder,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IEnumerable<ChecklistActionTemplate> checklistActionTemplates)
        {
            if (Session != null && Session["ChecklistTemplateModel"] != null)
            {
                checklistTemplateModel = Session["ChecklistTemplateModel"] as ChecklistTemplateModel;
            }
            checklistTemplateModel.Entity = checklistTemplate;

            if (ModelState.IsValid)
            {
                Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
                AccessControlService.CurrentUser = sessionHelper.CurrentUser;
                checklistTemplateModel.PrepareSave(PrepareSaveMode.Created);

                db.ChecklistTemplate.Add(checklistTemplateModel.Entity);

                db.SaveChanges();

                if (checklistActionTemplates != null && checklistActionTemplates.Count() > 0)
                {
                    checklistTemplateModel.ChecklistActionTemplateEntites = checklistActionTemplates.ToList().ConvertAll(x => new ChecklistActionTemplateModel(x));
                    short? sortOrder = 1;
                    foreach (ChecklistActionTemplateModel checklistActionTemplate in checklistTemplateModel.ChecklistActionTemplateEntites)
                    {
                        checklistActionTemplate.Entity.ChecklistTemplateId = checklistTemplateModel.Entity.ChecklistTemplateId;
                        checklistActionTemplate.Entity.SortingOrder = sortOrder;
                        checklistActionTemplate.PrepareSave(PrepareSaveMode.Created);

                        db.ChecklistActionTemplate.Add(checklistActionTemplate.Entity);

                        sortOrder++;
                    }

                    db.SaveChanges();
                }

                string desc = "";

                if (checklistTemplateModel.Entity.CrisisTypeId > 0)
                {
                    desc = MasterTableService.GetIncidentTypeName(MasterTableService.GetIncidentTypeSubType(checklistTemplateModel.Entity.CrisisTypeId)) + " (Crisis)";
                }
                else if (checklistTemplateModel.Entity.IncidentTypeId > 0)
                {
                    desc = MasterTableService.GetIncidentTypeName(MasterTableService.GetIncidentTypeSubType(checklistTemplateModel.Entity.IncidentTypeId));
                }

                return RedirectToAction("Index", new { message = "Checklist for " + desc + " had been created successfully!" });
            }

            return View(checklistTemplateModel);
        }

        // GET: ChecklistMaintenance/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ChecklistTemplate checklistTemplate = db.ChecklistTemplate.Find(id);
            if (checklistTemplate == null) return HttpNotFound();
            else
            {
                LoadData(id.Value);
                if (Session != null)
                {
                    Session["checklistTemplateModel"] = checklistTemplateModel;
                }

                return View(checklistTemplateModel);
            }
        }

        // POST: ChecklistMaintenance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChecklistTemplateId,IncidentTypeId,CrisisTypeId,DepartmentId,SortingOrder,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] ChecklistTemplate checklistTemplate,
                                [Bind(Include = "ChecklistActionTemplateId,ChecklistTemplateId,Action,IsCompulsory,AlertThreshold,SortingOrder,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IEnumerable<ChecklistActionTemplate> checklistActionTemplates)
        {
            if (Session != null && Session["checklistTemplateModel"] != null)
            {
                checklistTemplateModel = Session["checklistTemplateModel"] as ChecklistTemplateModel;
            }
            checklistTemplateModel.Entity = checklistTemplate;
                
            if (ModelState.IsValid)
            {
                Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
                AccessControlService.CurrentUser = sessionHelper.CurrentUser;
                checklistTemplateModel.PrepareSave();

                db.ChecklistTemplate.Attach(checklistTemplateModel.Entity);
                db.Entry(checklistTemplateModel.Entity).State = EntityState.Modified;

                /*
                foreach (ChecklistActionTemplateModel checklistActionTemplate in checklistTemplateModel.ChecklistActionTemplateEntites)
                {
                    checklistActionTemplate.Entity.ChecklistTemplateId = checklistTemplateModel.Entity.ChecklistTemplateId;
                    checklistActionTemplate.PrepareSave();

                    if (!checklistActionTemplate.IsDeleted)
                    {
                        // ignore existing...
                        if (checklistActionTemplate.Entity.ChecklistActionTemplateId == 0) db.ChecklistActionTemplate.Add(checklistActionTemplate.Entity);
                    }
                    else
                    {
                        // remove existing...
                        if (checklistActionTemplate.Entity.ChecklistActionTemplateId > 0) db.ChecklistActionTemplate.Remove(checklistActionTemplate.Entity);
                    }
                }

                db.SaveChanges();
                */
                return RedirectToAction("Details", new { message = "Checklist had been updated successfully!" });
            }
            return View(checklistTemplateModel);
        }

        // POST: ChecklistMaintenance/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            ChecklistTemplate checklistTemplate = db.ChecklistTemplate.Find(id);
            if (checklistTemplate == null) return HttpNotFound();

            LoadData(id.Value);

            db.ChecklistTemplate.Remove(checklistTemplateModel.Entity);

            foreach (ChecklistActionTemplateModel checklistActionTemplate in checklistTemplateModel.ChecklistActionTemplateEntites)
            {
                db.ChecklistActionTemplate.Remove(checklistActionTemplate.Entity);
            }

            db.SaveChanges();

            return RedirectToAction("Index", new { message = "The selected checklist(s) had been deleted!" });
        }

        // POST: ChecklistMaintenance/BatchDelete/1,2,3,4...
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BatchDelete(String idList)
        {
            string[] sIdList = idList.Split(new string[] { ";" }, StringSplitOptions.None);

            foreach(string sId in sIdList)
            {
                int id = Convert.ToInt32(sId);
                ChecklistTemplate checklistTemplate = db.ChecklistTemplate.Find(id);
                if (checklistTemplate == null) continue;

                LoadData(id);

                db.ChecklistTemplate.Remove(checklistTemplateModel.Entity);

                foreach (ChecklistActionTemplateModel checklistActionTemplate in checklistTemplateModel.ChecklistActionTemplateEntites)
                {
                    db.ChecklistActionTemplate.Remove(checklistActionTemplate.Entity);
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index", new { message = "The selected checklist(s) had been deleted!" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void LoadData(int id)
        {
            ChecklistTemplate checklistTemplate = db.ChecklistTemplate.Find(id);
            checklistTemplateModel = new ChecklistTemplateModel(checklistTemplate);
            checklistTemplateModel.ChecklistActionTemplateEntites = (from checklistActionTemplate in db.ChecklistActionTemplate
                                                                     where checklistActionTemplate.ChecklistTemplateId.Equals(id)
                                                                     select new ChecklistActionTemplateModel()
                                                                     {
                                                                         Entity = checklistActionTemplate
                                                                     }).ToList<ChecklistActionTemplateModel>();
        }
    }
}
