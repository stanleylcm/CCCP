using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.IO;
using CCCP.Common;

namespace CCCP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // log4net
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~/log4net.config")));
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
            Response.Clear();

            // error handling
            ex.LogError();

            // route to Error page
            Server.ClearError();
            string action = "Error";
            Response.Redirect(String.Format("~/Error/{0}/?errorMessage={1}", action, ex.Message.Replace("\r\n", "")));
        }        
    }
}
