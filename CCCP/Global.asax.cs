using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.IO;
using CCCP.Common;

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

            // log4net
            string path = Server.MapPath("~/log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(path));
        }

        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo("en-US");

            if (HttpContext.Current.Request.Cookies["CCCP"] != null)
            {
                string langName;
                if (HttpContext.Current.Request.Cookies["CCCP"]["Language"] != null)
                {
                    langName = HttpContext.Current.Request.Cookies["CCCP"]["Language"];

                    System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo(langName);
                }
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
