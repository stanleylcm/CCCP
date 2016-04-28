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
    public class RoleMaintenanceController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();
        public RoleModel roleModel = new RoleModel();

        // GET: RoleMaintenance
        public ActionResult Index(string message, string searchCriteria)
        {
            ViewBag.Message = message;
            ViewBag.SearchCriteria = searchCriteria;
            List<Role> roles = db.Role.ToList();
            List<RoleModel> roleModels = roles.ConvertAll(x => new RoleModel(x));
            return View(roleModels);
        }

        // GET: RoleMaintenance/Create
        public ActionResult Create()
        {
            roleModel = new RoleModel();
            if (Session != null)
            {
                Session["roleModel"] = roleModel;
            }
            return View(roleModel);
        }

        // POST: RoleMaintenance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleId,Code,Description,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] Role role)
        {
            if (Session != null && Session["roleModel"] != null)
            {
                roleModel = Session["roleModel"] as RoleModel;
            }
            roleModel.Entity = role;

            if (ModelState.IsValid)
            {
                Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
                AccessControlService.CurrentUser = sessionHelper.CurrentUser;
                roleModel.PrepareSave(PrepareSaveMode.Created);

                db.Role.Add(roleModel.Entity);

                db.SaveChanges();

                foreach (RoleFunctionModel roleFunc in roleModel.RoleFunctionEntities)
                {
                    roleFunc.Entity.RoleId = roleModel.Entity.RoleId;
                    if (!roleFunc.IsDeleted) db.RoleFunction.Add(roleFunc.Entity);
                }

                foreach (RoleDepartmentModel roleDept in roleModel.RoleDepartmentEntities)
                {
                    roleDept.Entity.RoleId = roleModel.Entity.RoleId;
                    if (!roleDept.IsDeleted) db.RoleDepartment.Add(roleDept.Entity);
                }

                db.SaveChanges();

                return RedirectToAction("Index", new { message = "Role " + role.Description + " had been created successfully!" });
            }

            return View(roleModel);
        }

        // GET: RoleMaintenance/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Role role = db.Role.Find(id);
            if (role == null) return HttpNotFound();
            else
            {
                LoadData(id.Value);
                if (Session != null)
                {
                    Session["roleModel"] = roleModel;
                }

                return View(roleModel);
            }
        }

        // POST: RoleMaintenance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleId,Code,Description,History,CreatedBy,CreatedDateTime,LastUpdatedBy,LastUpdatedDateTime")] Role role)
        {
            if (Session != null && Session["roleModel"] != null)
            {
                roleModel = Session["roleModel"] as RoleModel;
            }
            roleModel.Entity = role;
                
            if (ModelState.IsValid)
            {
                Helpers.SessionHelper sessionHelper = new Helpers.SessionHelper();
                AccessControlService.CurrentUser = sessionHelper.CurrentUser;
                roleModel.PrepareSave();

                db.Role.Attach(roleModel.Entity);
                db.Entry(roleModel.Entity).State = EntityState.Modified;

                foreach (RoleFunctionModel roleFunc in roleModel.RoleFunctionEntities)
                {
                    roleFunc.Entity.RoleId = roleModel.Entity.RoleId;
                    if (!roleFunc.IsDeleted)
                    {
                        // ignore existing...
                        if (roleFunc.Entity.RoleFunctionId == 0) db.RoleFunction.Add(roleFunc.Entity);
                    }
                    else
                    {
                        // remove existing...
                        if (roleFunc.Entity.RoleFunctionId > 0) db.RoleFunction.Remove(roleFunc.Entity);
                    }
                }

                foreach (RoleDepartmentModel roleDept in roleModel.RoleDepartmentEntities)
                {
                    roleDept.Entity.RoleId = roleModel.Entity.RoleId;
                    if (!roleDept.IsDeleted)
                    {
                        // ignore existing...
                        if (roleDept.Entity.RoleDepartmentId == 0) db.RoleDepartment.Add(roleDept.Entity);
                    }
                    else
                    {
                        // remove existing...
                        if (roleDept.Entity.RoleDepartmentId > 0) db.RoleDepartment.Remove(roleDept.Entity);
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Index", new { message = "Role " + role.Description + " had been updated successfully!" });
            }
            return View(roleModel);
        }

        // POST: RoleMaintenance/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            Role role = db.Role.Find(id);
            if (role == null) return HttpNotFound();

            LoadData(id.Value);

            db.Role.Remove(roleModel.Entity);

            foreach (RoleFunctionModel roleFunc in roleModel.RoleFunctionEntities)
            {
                db.RoleFunction.Remove(roleFunc.Entity);
            }

            foreach (RoleDepartmentModel roleDept in roleModel.RoleDepartmentEntities)
            {
                db.RoleDepartment.Remove(roleDept.Entity);
            }

            db.SaveChanges();

            return RedirectToAction("Index", new { message = "The selected role(s) had been deleted!" });
        }

        // POST: RoleMaintenance/BatchDelete/1,2,3,4...
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BatchDelete(String idList)
        {
            string[] sIdList = idList.Split(new string[] { ";" }, StringSplitOptions.None);

            foreach(string sId in sIdList)
            {
                int id = Convert.ToInt32(sId);
                Role role = db.Role.Find(id);
                if (role == null) continue;

                LoadData(id);

                db.Role.Remove(roleModel.Entity);

                foreach (RoleFunctionModel roleFunc in roleModel.RoleFunctionEntities)
                {
                    db.RoleFunction.Remove(roleFunc.Entity);
                }

                foreach (RoleDepartmentModel roleDept in roleModel.RoleDepartmentEntities)
                {
                    db.RoleDepartment.Remove(roleDept.Entity);
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index", new { message = "The selected role(s) had been deleted!" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void LoadData(int roleId)
        {
            roleModel = new RoleModel();
            Role role = db.Role.Find(roleId);
            roleModel.Entity = role;
            roleModel.RoleDepartmentEntities = (from roleDept in db.RoleDepartment
                                                where roleDept.RoleId.Equals(roleId)
                                                select new RoleDepartmentModel()
                                                {
                                                    Entity = roleDept
                                                }).ToList<RoleDepartmentModel>();
            roleModel.RoleFunctionEntities = (from roleFunc in db.RoleFunction
                                              where roleFunc.RoleId.Equals(roleId)
                                              select new RoleFunctionModel()
                                              {
                                                  Entity = roleFunc
                                              }).ToList<RoleFunctionModel>();
        }

        public ActionResult ToggleRoleDepartment(int departmentId)
        {
            if (Session != null && Session["roleModel"] != null)
            {
                roleModel = Session["roleModel"] as RoleModel;
            }

            Boolean jobDone = false;
            foreach (RoleDepartmentModel roleDept in roleModel.RoleDepartmentEntities)
            {
                // roleDeparment found
                if (roleDept.Entity.DepartmentId == departmentId)
                {
                    roleDept.IsDeleted = (!roleDept.IsDeleted);
                    jobDone = true;
                }
            }
            // roleDepartment not found, should be newly checked
            if (!jobDone)
            {
                roleModel.RoleDepartmentEntities.Add(new RoleDepartmentModel(new RoleDepartment() { RoleId = roleModel.Entity.RoleId, DepartmentId = departmentId }));
            }

            if (Session != null)
            {
                Session["roleModel"] = roleModel;
            }

            return Json(new { result = true });
        }

        public ActionResult ToggleRoleFunction(int functionId)
        {
            if (Session != null && Session["roleModel"] != null)
            {
                roleModel = Session["roleModel"] as RoleModel;
            }

            Boolean jobDone = false;
            foreach (RoleFunctionModel roleFunc in roleModel.RoleFunctionEntities)
            {
                // roleDeparment found
                if (roleFunc.Entity.FunctionId == functionId)
                {
                    roleFunc.IsDeleted = (!roleFunc.IsDeleted);
                    jobDone = true;
                }
            }
            // roleDepartment not found, should be newly checked
            if (!jobDone)
            {
                roleModel.RoleFunctionEntities.Add(new RoleFunctionModel(new RoleFunction() { RoleId = roleModel.Entity.RoleId, FunctionId = functionId }));
            }

            if (Session != null)
            {
                Session["roleModel"] = roleModel;
            }

            return Json(new { result = true });
        }

        public ActionResult DuplicateCode(string code)
        {
            Role role = db.Role.Where(m => m.Code == code).FirstOrDefault();
            return Json(new { result = (role.RoleId > 0) });
        }

        public ActionResult DuplicationDescription(string desc)
        {
            Role role = db.Role.Where(m => m.Description == desc).FirstOrDefault();
            return Json(new { result = (role.RoleId > 0) });
        }
    }
}
