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
    public class IncidentSystemInvoicingController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentSystemInvoicingModel incident = new IncidentSystemInvoicingModel();

        // GET: IncidentSystemInvoicings
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<IncidentSystemInvoicing> incidents = new IncidentSystemInvoicingApiController().GetIncidentList();
            List<IncidentSystemInvoicingModel> incidentModels = incidents.ConvertAll(x => new IncidentSystemInvoicingModel(x));
            return View(incidentModels);
        }

        // GET: IncidentSystemInvoicings/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemInvoicing incidentSystemInvoicing = db.IncidentSystemInvoicing.Find(id);
            if (incidentSystemInvoicing == null)
            {
                return HttpNotFound();
            }
            LoadData(id.Value);
            if (Session != null)
            {
                Session["incident"] = incident;
            }

            return View(incident);
        }

        // GET: IncidentSystemInvoicings/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        // POST: IncidentSystemInvoicings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentSystemInvoicingId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,ProblemArea,PossibleCause,ExpectedAffectedNoOfBill,ExpectedAffectedBillingDay,ContactedBy,Impact,StatusUpdate,RequireMitigatingAction,MitigatingAction,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemInvoicing incidentSystemInvoicing)
        {
            if (ModelState.IsValid)
            {
                new IncidentSystemInvoicingApiController().CreateIncident(incidentSystemInvoicing);
                return RedirectToAction("Index", new { message = "Incident " + incidentSystemInvoicing.IncidentNo + " had been created successfully!" });
            }

            return View(incidentSystemInvoicing);
        }

        // GET: IncidentSystemInvoicings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentSystemInvoicing incidentSystemInvoicing = db.IncidentSystemInvoicing.Find(id);
            if (incidentSystemInvoicing == null) return HttpNotFound();
            else
            {
                LoadData(id.Value);
                if (Session != null)
                {
                    Session["incident"] = incident;
                }

                return View(incident);
            }
        }

        // POST: IncidentSystemInvoicings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentSystemInvoicingId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisIdIssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,ProblemArea,PossibleCause,ExpectedAffectedNoOfBill,ExpectedAffectedBillingDay,ContactedBy,Impact,StatusUpdate,RequireMitigatingAction,MitigatingAction,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemInvoicing incidentSystemInvoicing)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemInvoicingModel;
                incident.Entity = incidentSystemInvoicing;
            }
                
            if (ModelState.IsValid)
            {
                new IncidentSystemInvoicingApiController().EditIncident(incident);

                return RedirectToAction("Details", new { id = incident.Entity.IncidentSystemInvoicingId, message = "Incident had been updated successfully!" });
            }
            return View(incident);
        }

        // GET: IncidentSystemInvoicings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemInvoicing incidentSystemInvoicing = db.IncidentSystemInvoicing.Find(id);
            if (incidentSystemInvoicing == null)
            {
                return HttpNotFound();
            }
            LoadData(id.Value);
            if (Session != null)
            {
                Session["incident"] = incident;
            }
            return View(incident);
        }

        // POST: IncidentSystemInvoicings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IncidentSystemInvoicing incidentSystemInvoicing = db.IncidentSystemInvoicing.Find(id);
            db.IncidentSystemInvoicing.Remove(incidentSystemInvoicing);
            if (Session != null)
            {
                incident = Session["incident"] as IncidentSystemInvoicingModel;
                foreach (ChecklistModel cl in incident.Checklists)
                {
                    foreach (ChecklistActionModel clAction in cl.ChecklistActions)
                    {
                        db.ChecklistAction.Remove(clAction.Entity);
                    }
                    db.Checklist.Remove(cl.Entity);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Cancel(int id)
        {
            IncidentSystemInvoicing incidentSystemInvoicing = db.IncidentSystemInvoicing.Find(id);
            incidentSystemInvoicing.IncidentStatus = IncidentStatus.Cancelled.ToEnumString();
            db.SaveChanges();

            return RedirectToAction("Index", new { id = incident.Entity.IncidentSystemInvoicingId, message = "The Incident had been cancelled successfully" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void LoadData(int incidentId)
        {
            this.incident = new IncidentSystemInvoicingApiController().GetIncident(incidentId);
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemInvoicingModel;
            }

            incident.Checklists[checklist].ChecklistActions[checklistAction].ToggleActionStatus();

            if (Session != null)
            {
                Session["incident"] = incident;
            }

            return Json(new { result = true });
        }

        public ActionResult CloseIncident()
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemInvoicingModel;
            }
            incident.Entity.IncidentStatus = IncidentStatus.Closed.ToEnumString();

            return Edit(incident.Entity);
        }

        public void Test()
        {
            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
