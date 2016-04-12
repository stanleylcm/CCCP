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
using CCCP.Helpers;

namespace CCCP.Controllers
{
    public class LoginController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();

        public ActionResult Index()
        {
            ViewBag.ErrorMsg = "";

            if (Request.Form["loginId"] != null)
            {
                string loginId = Request.Form["loginId"];
                string password = Request.Form["password"];

                if (ModelState.IsValid)
                {
                    UserModel user = new AccessControlApiController().Authenticate(loginId, password.Encode());
                    if (user != null && user.Entity.UserId > 0)
                    {
                        FormsAuthentication.SetAuthCookie(loginId, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Fail to Login!";
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