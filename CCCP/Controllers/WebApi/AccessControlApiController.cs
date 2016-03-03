using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using CCCP.Business.Model;
using CCCP.Business.Service;
using CCCP.ViewModel;
using CCCP.Common;
using System.Data;
using System.Data.Entity;

namespace CCCP.Controllers.WebApi
{
    public class AccessControlApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public UserModel Authenticate(string loginId, string password)
        {
            UserModel result = new UserModel();
            result.IsAuthenticated = false;
            int userId = checkLogin(loginId, password); // try login and return userID if success
            if (userId > 0)
            {
                // Get access rights
                result.AccessRights = AccessControlService.GetAccessRights(userId);

                result.IsAuthenticated = true;
                result.LoginDateTime = DateTime.Now;
            }

            return result;
        }

        private int checkLogin(string loginId, string password)
        {
            // temp
            return 1;
        }
    }
}