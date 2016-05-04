using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCCP.ViewModel;
using CCCP.Business;
using CCCP.Business.Model;
using CCCP.Common;
using System.Data.Entity.Core.Objects;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace CCCP.UnitTest
{
    [TestClass]
    public class KFUnitTest
    {
        [TestMethod]
        public void TestCreateIncidentWebApi()
        {
            CCCPDbContext db = new CCCPDbContext();
            IncidentOHS incident = db.IncidentOHS.Find(5);

            incident.IncidentOHSId = 0;
            incident.ChecklistBatchId = 0;
            incident.ChatRoomId = 0;
            incident.GeneralEnquiryId = 0;
            //incidentSystemBilling.NotificationId = 0;

            var client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:8088/");

            // HTTP POST
            var response = client.PostAsJsonAsync("/api/incidentOHSApi/CreateIncident", incident).Result;

            if (response.IsSuccessStatusCode)
            {
            }
        }

        [TestMethod]
        public void TestEditIncidentWebApi()
        {
            CCCPDbContext db = new CCCPDbContext();
            IncidentSystemBillingModel incident = new IncidentSystemBillingModel();
            
            // load incident details
            incident.Entity = db.IncidentSystemBilling.Find(1);

            // load checklists
            int checklistBatchID = incident.Entity.ChecklistBatchId;
            incident.ChecklistEntities = (from checklist in db.Checklist
                                        join department in db.Department on checklist.DepartmentId equals department.DepartmentId
                                        where checklist.ChecklistBatchId.Equals(checklistBatchID)
                                        orderby checklist.SortingOrder
                                        select new ChecklistExtend()
                                        {
                                            ChecklistId = checklist.ChecklistId,
                                            ChecklistBatchId = checklist.ChecklistBatchId,
                                            DepartmentId = checklist.DepartmentId,
                                            SortingOrder = checklist.SortingOrder,
                                            History = checklist.History,
                                            CreatedBy = checklist.CreatedBy,
                                            CreatedDateTime = checklist.CreatedDateTime,
                                            LastUpdatedBy = checklist.LastUpdatedBy,
                                            LastUpdatedDateTime = checklist.LastUpdatedDateTime,
                                            Department = department.Department1
                                        }).ToList<ChecklistExtend>();

            // load checklist actions
            foreach (ChecklistModel checklist in incident.Checklists)
            {
                List<ChecklistAction> actionEntities = (from ca in db.ChecklistAction
                                                        where ca.ChecklistId.Equals(checklist.Entity.ChecklistId)
                                                        orderby ca.SortingOrder
                                                        select ca).ToList<ChecklistAction>();
                checklist.ChecklistActionEntities = actionEntities;
                
            }

            incident.Checklists[1].ChecklistActions[1].ToggleActionStatus();

            var client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost/");

            // HTTP POST
            var response = client.PostAsJsonAsync("/api/incidentSystemBillingApi/EditIncident", incident).Result;

            if (response.IsSuccessStatusCode)
            {
            }
        }

        [TestMethod]
        public void TestCreateOMSEventWebApi()
        {
            CCCPDbContext db = new CCCPDbContext();

            var client = new HttpClient();

            OMSEventApiModel test = new OMSEventApiModel();

            test.OMSNo = "3";
            test.AffectedArea = "test affected area";
            test.AffectedArea_Chi = "測試影響範圍";
            test.AffectedBuilding = "test affected building";
            test.AffectedBuilding_Chi = "測試影響樓宇";
            test.AffectedStreet = "test affected street";
            test.AffectedStreet_Chi = "測試影響街道";
            test.OutageStartTime = DateTime.Now;
            test.NoOfBuilding = 2;
            test.NoOfPlatinumCustomer = 1;
            test.NoOfDiamondCustomer = 1;
            test.NoOfGoldCustomer = 1;
            test.NoOfSilverCustomer = 1;
            test.ExpectedRestorationDateTime = DateTime.Now.AddHours(10);
            test.StatusUpdateCode = "1";
            test.MVOutage = true;
            test.LVOutage = true;
            test.AffectedPoints = "11,22,33,44,55,66,77";


            client.BaseAddress = new Uri("http://localhost/");

            // HTTP POST
            var response = client.PostAsJsonAsync("/api/OMSEventApi/SubmitOMSEvent", test).Result;

            if (response.IsSuccessStatusCode)
            {
            }
        }

        [TestMethod]
        public void TestEditOMSEventWebApi()
        {
            CCCPDbContext db = new CCCPDbContext();

            var client = new HttpClient();

            OMSEventApiModel test = new OMSEventApiModel();

            test.OMSNo = "3";
            test.AffectedArea = "test affected area";
            test.AffectedArea_Chi = "要刪了123";
            test.AffectedBuilding = "test affected building";
            test.AffectedBuilding_Chi = "要刪了";
            test.AffectedStreet = "test affected street";
            test.AffectedStreet_Chi = "要刪了333";
            test.OutageStartTime = DateTime.Now;
            test.NoOfBuilding = 2;
            test.NoOfPlatinumCustomer = 1;
            test.NoOfDiamondCustomer = 1;
            test.NoOfGoldCustomer = 1;
            test.NoOfSilverCustomer = 1;
            test.ExpectedRestorationDateTime = DateTime.Now.AddHours(10);
            test.StatusUpdateCode = "1";
            test.MVOutage = true;
            test.LVOutage = true;
            test.AffectedPoints = "11,22,33,44,55,66,77";


            client.BaseAddress = new Uri("http://localhost/");

            // HTTP POST
            var response = client.PostAsJsonAsync("/api/OMSEventApi/SubmitOMSEvent", test).Result;

            if (response.IsSuccessStatusCode)
            {
            }
        }

        [TestMethod]
        public void TestCancelOMSEventWebApi()
        {
            /*
            CCCPDbContext db = new CCCPDbContext();

            var client = new HttpClient();

            string OMSNo = "2";

            client.BaseAddress = new Uri("http://localhost/");

            // HTTP POST
            var response = client.PostAsJsonAsync("/api/OMSEventApi/Cancel", OMSNo).Result;

            if (response.IsSuccessStatusCode)
            {
            }
            */

            // http://localhost/api/OMSEventApi/Cancel?OMSNo=2
        }



        [TestMethod]
        public void TestCloseOMSEventWebApi()
        {
            CCCPDbContext db = new CCCPDbContext();

            var client = new HttpClient();

            OMSEventApiModel test = new OMSEventApiModel();

            test.OMSNo = "1";
            test.AffectedArea = "test affected area";
            test.AffectedArea_Chi = "要刪了123";
            test.AffectedBuilding = "test affected building";
            test.AffectedBuilding_Chi = "要刪了";
            test.AffectedStreet = "test affected street";
            test.AffectedStreet_Chi = "要刪了333";
            test.OutageStartTime = DateTime.Now;
            test.NoOfBuilding = 2;
            test.NoOfPlatinumCustomer = 1;
            test.NoOfDiamondCustomer = 1;
            test.NoOfGoldCustomer = 1;
            test.NoOfSilverCustomer = 1;
            test.ExpectedRestorationDateTime = DateTime.Now.AddHours(10);
            test.StatusUpdateCode = "1";
            test.MVOutage = true;
            test.LVOutage = true;
            test.AffectedPoints = "11,22,33,44,55,66,77";

            client.BaseAddress = new Uri("http://localhost/");

            // HTTP POST
            var response = client.PostAsJsonAsync("/api/OMSEventApi/Close", test).Result;

            if (response.IsSuccessStatusCode)
            {
            }
        }
    }
}
