using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
    public class HomeController : Controller
    {
        private CCCPDbContext db = new CCCPDbContext();

        public ActionResult Index()
        {
            #region Incident Progress
            ObjectResult<usp_Dashboard_GetIncidentProgress_Result> incidentProgress = db.usp_Dashboard_GetIncidentProgress();
            
            foreach(usp_Dashboard_GetIncidentProgress_Result record in incidentProgress)
            {
                ViewBag.TotalIncidentCount = record.TotalIncidentCount;

                ViewBag.IncidentProgress = (record.TotalIncidentCount - record.OutstandingIncidentCount).ToString() + "/" + record.TotalIncidentCount.ToString();
                if (record.TotalIncidentCount == 0)
                {
                    ViewBag.IncidentProgressPercentage = 0;
                }
                else
                {
                    ViewBag.IncidentProgressPercentage = (((record.TotalIncidentCount - record.OutstandingIncidentCount) * 1.0) / record.TotalIncidentCount) * 100;
                }
            }
            #endregion

            #region General Enquiry Progress
            ObjectResult<usp_Dashboard_GetGeneralEnquiryProgress_Result> generalEnquiryProgress = db.usp_Dashboard_GetGeneralEnquiryProgress();

            foreach (usp_Dashboard_GetGeneralEnquiryProgress_Result record in generalEnquiryProgress)
            {
                ViewBag.TotalGeneralEnquiryCount = record.TotalGeneralEnquiryCount;

                ViewBag.GeneralEnquiryProgress = (record.TotalGeneralEnquiryCount - record.OutstandingGeneralEnquiryCount).ToString() + "/" + record.TotalGeneralEnquiryCount.ToString();
                if (record.TotalGeneralEnquiryCount == 0)
                {
                    ViewBag.GeneralEnquiryProgressPercentage = 0;
                }
                else
                {
                    ViewBag.GeneralEnquiryProgressPercentage = (((record.TotalGeneralEnquiryCount - record.OutstandingGeneralEnquiryCount) * 1.0) / record.TotalGeneralEnquiryCount) * 100;
                }
            }
            #endregion

            #region Outstanding Incident List

            #endregion

            #region Incident Summary
            ObjectResult<usp_Dashboard_GetIncidentSummary1_Result> incidentSummary = db.usp_Dashboard_GetIncidentSummary1();

            Dictionary<string, int> incidentSummaryDict = new Dictionary<string, int>();

            foreach (usp_Dashboard_GetIncidentSummary1_Result record in incidentSummary)
            {
                incidentSummaryDict.Add(record.NAME, record.Cnt.Value);
            }

            ViewBag.IncidentSummary = incidentSummaryDict;
            #endregion

            #region Crisis Approval List

            #endregion

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