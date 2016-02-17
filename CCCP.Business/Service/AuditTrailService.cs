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
        public static string GetHistory(string originalHistory, string updateUser, DateTime updateDateTime)
        {
            string result = "";

            string historyStr =  string.Format("Last updated by {0} at {1}", updateUser, updateDateTime.ToString("yyyy-MM-dd tthh:mm:ss"));
            if (originalHistory.IsNullOrEmpty()) result = historyStr;
            else result = historyStr + "\r\n" + originalHistory;

            return result;
        }
    }
}
