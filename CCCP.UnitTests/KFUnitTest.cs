using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCCP.ViewModel;
using System.Data.Entity.Core.Objects;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CCCP.UnitTest
{
    [TestClass]
    public class KFUnitTest
    {
        [TestMethod]
        public void TestCreateIncidentWebApi()
        {
            CCCPDbContext db = new CCCPDbContext();
            IncidentSystemBilling incidentSystemBilling = db.IncidentSystemBilling.Find(1);

            incidentSystemBilling.IncidentSystemBillingId = 0;
            incidentSystemBilling.ChecklistBatchId = 0;
            incidentSystemBilling.ChatRoomId = 0;
            incidentSystemBilling.GeneralEnquiryId = 0;
            incidentSystemBilling.NotificationId = 0;

            var client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:62029/");

            // HTTP POST
            var response = client.PostAsJsonAsync("/api/incidentSystemBillingApi/CreateIncident", incidentSystemBilling).Result;

            if (response.IsSuccessStatusCode)
            {
            }
        }
    }
}
