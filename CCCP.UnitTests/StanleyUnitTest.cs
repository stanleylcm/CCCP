using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCCP.Models;
using System.Data.Entity.Core.Objects;

namespace CCCP.UnitTests
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
    }
}
