using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Core.Objects;
using CCCP.ViewModel;
using CCCP.Common;
using CCCP.Business;

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
    }
}
