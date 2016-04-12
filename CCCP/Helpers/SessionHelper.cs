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

namespace CCCP.Helpers
{
    public class SessionHelper
    {
        public UserModel CurrentUser
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentUser"] != null)
                        return HttpContext.Current.Session["CurrentUser"] as UserModel;
                    else//--- test--data----
                        return new UserModel();
                }
                else
                    return new UserModel();
            }
            set
            {
                HttpContext.Current.Session["CurrentUser"] = value;
            }
        }
    }
}