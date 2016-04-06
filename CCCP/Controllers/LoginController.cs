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
            return View();
        }

        public ActionResult Login()
        {
            string loginId = Request.Form["loginId"];
            string password = Request.Form["password"];

            CCCPDbContext db = new CCCPDbContext();

            var userObject = (from data in db.User
                              where data.LoginName == loginId
                              && data.Password == password
                              select data);

            if (userObject.Count() > 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
            //FormsAuthentication.RedirectFromLoginPage(loginId, false);
        }
    }
}