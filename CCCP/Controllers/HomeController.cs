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
    }
}