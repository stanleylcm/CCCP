using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCCP.Common;
using CCCP.Business.Model;
using CCCP.ViewModel;

namespace CCCP.Business.Service
{
    public class AuditTrailService
    {
        public static string GetHistory(string originalHistory, string updateUser, DateTime updateDateTime, string saveMode = "Last updated")
        {
            string result = "";

            string historyStr =  string.Format("{0} by {1} at {2}", saveMode, updateUser, updateDateTime.ToString("yyyy-MM-dd tthh:mm:ss"));
            if (originalHistory.IsNullOrEmpty()) result = historyStr;
            else result = historyStr + "\r\n" + originalHistory;

            return result;
        }
    }
}
