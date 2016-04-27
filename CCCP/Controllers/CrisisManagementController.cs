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
    public class CrisisManagementController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public CrisisModel crisis = new CrisisModel();

        // GET: CrisisManagements
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<Crisis> crisiss = new CrisisApiController().GetCrisisList();
            List<CrisisModel> crisisModels = crisiss.ConvertAll(x => new CrisisModel(x));
            return View(crisisModels);
        }

        // GET: CrisisManagements/Details/5
        public ActionResult Details(int? id, string message)
        {
            ViewBag.Message = message;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crisis crisisInstant = db.Crisis.Find(id);
            if (crisisInstant == null)
            {
                return HttpNotFound();
            }
            LoadData(id.Value);
            if (Session != null)
            {
                Session["crisis"] = crisis;
            }

            return View(crisis);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void LoadData(int crisisId)
        {
            this.crisis = new CrisisApiController().GetCrisis(crisisId);
        }

        public ActionResult Close(int id)
        {
            new CrisisApiController().Close(id);

            return RedirectToAction("Details", new { id = crisis.Entity.CrisisId, message = "Crisis " + crisis.Entity.CrisisNo + " had been closed!" });
        }

        public ActionResult ToggleActionStatus(int checklist, int checklistAction)
        {
            Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
            AccessControlService.CurrentUser = sessionHelper.CurrentUser;
            if (Session != null && Session["crisis"] != null)
            {
                crisis = Session["crisis"] as CrisisModel;
            }

            crisis.Checklists[checklist].ChecklistActions[checklistAction].ToggleActionStatus();

            if (Session != null)
            {
                Session["crisis"] = crisis;
            }

            return Json(new { result = true });
        }

        public void Test()
        {
            crisis.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
