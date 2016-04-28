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
    public class CrisisApprovalController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public CrisisModel crisis = new CrisisModel();

        // GET: CrisisApprovals
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<Crisis> crisiss = new CrisisApiController().GetCrisisApprovalList();
            List<CrisisModel> crisisModels = crisiss.ConvertAll(x => new CrisisModel(x));
            return View(crisisModels);
        }

        // GET: CrisisApprovals/Details/5
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

        public ActionResult Approve(int id)
        {
            new CrisisApiController().Approve(id);

            return RedirectToAction("Details", "CrisisManagement", new { id = crisis.Entity.CrisisId, message = "Crisis " + crisis.Entity.CrisisNo + " had been approved!" });
        }

        public ActionResult Reject(int id, string rejectReason)
        {
            new CrisisApiController().Reject(id, rejectReason);

            return RedirectToAction("Details", new { id = crisis.Entity.CrisisId, message = "Crisis " + crisis.Entity.CrisisNo + " had been rejected!" });
        }

        public void Test()
        {
            crisis.Checklists[1].ChecklistActions[1].ToggleActionStatus();
        }
    }
}
