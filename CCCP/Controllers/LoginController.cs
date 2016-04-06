using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using CCCP.ViewModel;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.Common;
using CCCP.Controllers.WebApi;

namespace CCCP.Controllers
{
    public class LoginController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();

        public ActionResult Index()
        {
            if (Request.Form["loginId"] != null)
            {
                string loginId = Request.Form["loginId"];
                string password = Request.Form["password"];

                if (ModelState.IsValid)
                {
                    if (AccessControlService.IsValid(loginId, password))
                    {
                        FormsAuthentication.SetAuthCookie(loginId, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Login data is incorrect!");
                    }
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}