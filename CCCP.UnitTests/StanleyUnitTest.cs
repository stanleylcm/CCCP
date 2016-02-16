﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Core.Objects;
using CCCP.ViewModel;
using CCCP.Common;
using CCCP.Business;
using CCCP.Controllers;
using CCCP.Business.Service;

namespace CCCP.UnitTest
{
    [TestClass]
    public class StanleyUnitTest
    {
        [TestMethod]
        public void TestCallSP()
        {
            CCCPDbContext db = new CCCPDbContext();
            ObjectResult<usp_IncidentSystemBilling_Test_Result> result = db.usp_IncidentSystemBilling_Test();
            foreach (usp_IncidentSystemBilling_Test_Result record in result)
            {
                // iterate record
            }
        }

        [TestMethod]
        public void TestEnumExtensionMethod()
        {
            // enum to caption
            IncidentSubType subtype = IncidentSubType.Corporate_Image;
            Console.WriteLine(subtype.ToEnumString());

            // string to enum
            string s = "Corporate Image";
            bool result = false;
            IncidentSubType subtype1 = s.ToEnum<IncidentSubType>(out result);
        }

        [TestMethod]
        public void TestMisc()
        {
            string a = DateTime.Now.ToString("yyyy-MM-dd tthh:mm:ss");
        }

        [TestMethod]
        public void TestIncidentSystemBilling()
        {
            AccessControlService.CurrentUser.Entity.LoginName = "stanleylam";
            AccessControlService.CurrentUser.Entity.DisplayName = "Stanley Lam";
            
            IncidentSystemBillingController controller = new IncidentSystemBillingController();
            controller.Edit(1);
            controller.Test();
            controller.Edit(controller.incident.Entity);
        }

        [TestMethod]
        public void TestAuditLog()
        {
            string userName = AccessControlService.CurrentUser.GetLastUpdatedBy();
            userName = "test";
            CCCPDbContext db = new CCCPDbContext();
            ChecklistBatch x = db.ChecklistBatch.Find(1);
            x.CreatedBy += "1";
            db.SaveChanges(userName);
        }
    }
}
