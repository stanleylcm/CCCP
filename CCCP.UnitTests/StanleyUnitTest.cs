using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Core.Objects;
using CCCP.ViewModel;
using CCCP.Common;
using CCCP.Business;
using CCCP.Business.Model;
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

            int b = 999;
            string bb = b.ToString("00000");

            string c = "abcdefg";
            string c1 = c.Left(2);
            string c2 = c.Right(3);

            AccessRightsModel test = new AccessRightsModel();
            test.NotificationRights.Add(new IncidentTypeAndLevel() { IncidentType = IncidentTypeSubType.SystemBilling, IncidentLevel = IncidentLevelWithCrisis.Level_1 });
            test.NotificationRights.Add(new IncidentTypeAndLevel() { IncidentType = IncidentTypeSubType.SystemBilling, IncidentLevel = IncidentLevelWithCrisis.Crisis });
            test.ChatRoomRights = test.NotificationRights.Clone<IncidentTypeAndLevel>();
            test.NotificationRights[1].IncidentLevel = IncidentLevelWithCrisis.Level_2;

            IncidentTypeAndLevel il = new IncidentTypeAndLevel() { IncidentType = IncidentTypeSubType.OHS, IncidentLevel = IncidentLevelWithCrisis.Level_2 };
            List<IncidentTypeAndLevel> lid1 = il.GetDelta(IncidentTypeSubType.OHS);
            List<IncidentTypeAndLevel> lid2 = il.GetDelta(IncidentTypeSubType.EnvironmentLeakage);

            int id = IncidentService.GetIncidentTypeId(IncidentTypeSubType.SystemBilling);
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

        [TestMethod]
        public void TestErrorHandle()
        {
            IncidentSystemBillingController controller = new IncidentSystemBillingController();
            controller.Test();

        }

        [TestMethod]
        public void TestGetNewIncidentNo()
        {
            string newIncidentNo = IncidentService.GetNewIncidentNo(SequenceType.Incident, 2016);
        }

        [TestMethod]
        public void TestGetInputOptions()
        {
            List<string> result = InputOptionsService.GetIncidentSystemBillingInputOptions(IncidentSystemBillingInputKey.IncidentSystemBilling_StatusUpdate);
        }
    }
}
