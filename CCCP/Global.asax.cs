using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CCCP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo("en-US");

            if (Request.Cookies["CCCP"] != null)
            {
                string langName;
                if (Request.Cookies["CCCP"]["Language"] != null)
                {
                    langName = Request.Cookies["CCCP"]["Language"];
                    System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo(langName);
                }
                else
                {
                    Request.Cookies["CCCP"]["Language"] = "en-US";
                }

                if (Request.Cookies["CCCP"]["SidebarCollapse"] == null)
                {
                    Request.Cookies["CCCP"]["SidebarCollapse"] = "0";
                }
            }
            else
            {
                HttpCookie myCookie = new HttpCookie("CCCP");
                myCookie["Language"] = "en-US";
                myCookie["SidebarCollapse"] = "0";
                myCookie.Expires = DateTime.Now.AddYears(1);
                Request.Cookies.Add(myCookie);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
        }
    }
}
