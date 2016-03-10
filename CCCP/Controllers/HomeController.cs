using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCCP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AlertSuccess(string message)
        {
            ViewBag.Message = message;
            return PartialView("_AlertSuccess");
        }

        [HttpPost]
        public ActionResult SetLanguage(string name, string currentUrl)
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo(name);

            //HttpContext.Current.Session["culture"] = name;

            if (Response.Cookies["CCCP"] == null)
            {
                HttpCookie myCookie = new HttpCookie("CCCP");
                myCookie["Language"] = name;
                myCookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(myCookie);
            } else
            {
                Response.Cookies["CCCP"]["Language"] = name;
            }

            return Redirect(currentUrl);
        }

        [HttpPost]
        public ActionResult ToggleSidebarCollapse()
        {
            int sidebarCollapse = 0;

            if (Response.Cookies["CCCP"] == null)
            {
                HttpCookie myCookie = new HttpCookie("CCCP");
                myCookie["SidebarCollapse"] = ((sidebarCollapse + 1) % 2).ToString();
                myCookie.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(myCookie);
            }
            else
            {
                sidebarCollapse = Convert.ToInt32(Request.Cookies["CCCP"]["SidebarCollapse"]);
                Response.Cookies["CCCP"]["SidebarCollapse"] = ((sidebarCollapse + 1) % 2).ToString();
            }

            Session["SidebarCollapse"] = Response.Cookies["CCCP"]["SidebarCollapse"];

            return Json (new { result = (sidebarCollapse == 1) });
        }
    }
}