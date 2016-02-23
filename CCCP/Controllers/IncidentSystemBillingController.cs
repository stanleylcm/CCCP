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

namespace CCCP.Controllers
{
    public class IncidentSystemBillingController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public IncidentSystemBillingModel incident = new IncidentSystemBillingModel();

        // GET: IncidentSystemBillings
        public ActionResult Index()
        {
            return View(db.IncidentSystemBilling.ToList());
        }

        // GET: IncidentSystemBillings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemBilling incidentSystemBilling = db.IncidentSystemBilling.Find(id);
            if (incidentSystemBilling == null)
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

        // GET: IncidentSystemBillings/Create
        public ActionResult Create()
        {
            return View(incident);
        }

        // POST: IncidentSystemBillings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncidentSystemBillingId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisId,NotificationId,IssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,ProblemArea,PossibleCause,BillingErrorSeriousness,ExpectedAffectedCustomerBill,ContactedBy,Impact,StatusUpdate,RequireMitigatingAction,MitigatingAction,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemBilling incidentSystemBilling)
        {
            if (ModelState.IsValid)
            {
                incidentSystemBilling.IncidentStatus = Common.IncidentStatus.Pending.ToString();
                incident.Entity = incidentSystemBilling;
                incident.PrepareSave("Created");
                incidentSystemBilling = incident.Entity;

                db.IncidentSystemBilling.Add(incidentSystemBilling);
                db.SaveChanges();
                db.usp_Incident_PostCreate(incidentSystemBilling.IncidentSystemBillingId, 6, incident.Entity.CreatedBy, Common.CheckListActionStatus.Pending.ToString());
                return RedirectToAction("Index");
            }

            return View(incidentSystemBilling);
        }

        // GET: IncidentSystemBillings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            IncidentSystemBilling incidentSystemBilling = db.IncidentSystemBilling.Find(id);
            if (incidentSystemBilling == null) return HttpNotFound();
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

        // POST: IncidentSystemBillings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncidentSystemBillingId,ChecklistBatchId,ChatRoomId,GeneralEnquiryId,CrisisId,NotificationId,IssueById,IssueDateTime,CloseById,CloseDateTime,IncidentNo,LevelOfSeverity,IncidentStatus,IncidentBackground,IsDrillMode,History,ProblemArea,PossibleCause,BillingErrorSeriousness,ExpectedAffectedCustomerBill,ContactedBy,Impact,StatusUpdate,RequireMitigatingAction,MitigatingAction,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] IncidentSystemBilling incidentSystemBilling)
        {
                if (Session != null && Session["incident"] != null)
                {
                    incident = Session["incident"] as IncidentSystemBillingModel;
                    incident.Entity = incidentSystemBilling;
                }
                
            if (ModelState.IsValid)
            {
                // prepare history etc. before save
                if (incident.Entity.IncidentStatus == IncidentStatus.Closed.ToEnumString())
                {
                    incident.PrepareSave("Closed");
                }
                else
                {
                    incident.PrepareSave();
                }

                if (Session != null && Session["incident"] != null)
                {
                    db.IncidentSystemBilling.Attach(incident.Entity);
                    foreach (ChecklistModel cl in incident.Checklists)
                    {
                        foreach (ChecklistActionModel clAction in cl.ChecklistActions)
                        {
                            db.ChecklistAction.Attach(clAction.Entity);
                            db.Entry(clAction.Entity).State = EntityState.Modified;
                        }
                    }
                }
                db.Entry(incident.Entity).State = EntityState.Modified;
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

                return RedirectToAction("Details", new { id = incident.Entity.IncidentSystemBillingId });
            }
            return View(incident);
        }

        // GET: IncidentSystemBillings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncidentSystemBilling incidentSystemBilling = db.IncidentSystemBilling.Find(id);
            if (incidentSystemBilling == null)
            {
                return HttpNotFound();
            }
            LoadData(id.Value);
            return View(incident);
        }

        // POST: IncidentSystemBillings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IncidentSystemBilling incidentSystemBilling = db.IncidentSystemBilling.Find(id);
            db.IncidentSystemBilling.Remove(incidentSystemBilling);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            incident = new IncidentSystemBillingModel();

            // load incident details
            incident.Entity = db.IncidentSystemBilling.Find(incidentId);

            // load checklists
            int checklistBatchID = incident.Entity.ChecklistBatchId;
            incident.ChecklistEntities = (from c in db.Checklist                                          
                                          where c.ChecklistBatchId.Equals(checklistBatchID)
                                          orderby c.SortingOrder
                                          select c).ToList<Checklist>();

            // load checklist actions
            foreach (ChecklistModel checklist in incident.Checklists)
            {
                List<ChecklistAction> actionEntities = (from ca in db.ChecklistAction
                                                        where ca.ChecklistId.Equals(checklist.Entity.ChecklistId)
                                                        orderby ca.SortingOrder
                                                        select ca).ToList<ChecklistAction>();
                checklist.ChecklistActionEntities = actionEntities;
            }

            // load chat room

        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            if (Session != null && Session["incident"] != null)
            {
                incident = Session["incident"] as IncidentSystemBillingModel;
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
                incident = Session["incident"] as IncidentSystemBillingModel;
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
